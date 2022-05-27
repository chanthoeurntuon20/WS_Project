using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Configuration;

namespace WebService
{
    public class DisbPostToCBSErrorController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<DisbPostToCBSErrorResModel> Get([FromUri]string api_name, string api_key)
        {
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "";
            int TabletDisbRepayTimeToRead = 10;
            try {
                string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                //Add log
                c.T24_AddLog("DisbPostToCBSError_" + ServerDate.Replace("-", "_").Replace(" ", "_").Replace("HH:mm:ss.fff", ""), "DisbPostToCBSError", "api_name: " + api_name, "DisbPostToCBSError");
                //file
                string StrDate = DateTime.Now.ToString("yyyyMMdd");
                string FName = "DISB_" + StrDate + ".csv";//ERROR_DISB_POST_0_2018.08.24.18.20.19.111__2018.08.24.csv

                #region GetTime
                try
                {
                    DataTable dtTime = new DataTable();
                    dtTime = c.ReturnDT2("select SettingValue from tblSetting where SettingName = 'TabletDisbRepayTimeToRead'");

                    TabletDisbRepayTimeToRead = Convert.ToInt32(dtTime.Rows[0][0]);
                }
                catch (Exception) { }
                #endregion

                #region - get T24 path
                string T24PathStr = "", T24PathUsername = "", T24PathPwd = "", T24Server = "";
                DataTable dtT24Path = new DataTable();
                dtT24Path = c.ReturnDT("select * from T24_Path where PathID=2");
                T24PathStr = dtT24Path.Rows[0]["PathStr"].ToString();
                T24PathUsername = dtT24Path.Rows[0]["Username"].ToString();
                T24PathPwd = dtT24Path.Rows[0]["Pwd"].ToString();
                T24Server = dtT24Path.Rows[0]["Server"].ToString();
                #endregion - T24 path                
                #region sFTP
                string host = T24Server;
                string username = T24PathUsername;
                string password = T24PathPwd;
                //#region create path
                //try {
                //    string path = "\\" + StrDate;
                //    string dir = AppDomain.CurrentDomain.BaseDirectory + "Download" + path + "\\";
                //    if (Directory.Exists(dir)) { }
                //    else
                //    {
                //        System.IO.Directory.CreateDirectory(dir);
                //    }
                //} catch { }
                //#region create path
                //string downsavepath = HttpContext.Current.Server.MapPath("~/Download/"+ StrDate + "/");
                string downsavepath = HttpContext.Current.Server.MapPath("~/Download/");
                string remoteDirectory = T24PathStr;// "/Oradata/export/home/t24mst/bnk/bnk.interface/tabout/";
                using (var sftp = new SftpClient(host, username, password)) {
                    sftp.Connect();
                    var files = sftp.ListDirectory(remoteDirectory);
                    foreach (var file in files) {
                        if (!file.Name.StartsWith(".")) {
                            string remoteFileName = file.Name;
                            string ext = Path.GetExtension(remoteFileName);//new
                            if (ext.ToLower() == ".csv") {//new
                                if (remoteFileName.StartsWith("ERROR_DISB_"))
                                {
                                    int isOKToRead = 1;
                                    try
                                    {
                                        DateTime f1 = file.LastWriteTime;
                                        DateTime f2 = DateTime.Now;
                                        int f3 = (f2 - f1).Seconds;
                                        
                                        if (f3 > TabletDisbRepayTimeToRead)
                                        {
                                            isOKToRead = 1;
                                        }
                                        else
                                        {
                                            isOKToRead = 0;
                                        }
                                    }
                                    catch { }

                                    if (isOKToRead == 1) { 

                                        #region is file existed?
                                    if (!File.Exists(downsavepath + remoteFileName))
                                    {
                                        string OriPostFileName = remoteFileName.Replace("ERROR_", "");
                                        #region write file
                                        using (Stream fileStream = File.Create(downsavepath + remoteFileName))
                                        {
                                            sftp.DownloadFile(remoteDirectory + remoteFileName, fileStream);
                                            #region file info
                                            try
                                            {
                                                FileInfo fi = new FileInfo(remoteDirectory + remoteFileName);
                                                DateTime FDate = fi.LastWriteTime;
                                                DateTime NDate = DateTime.Now;

                                            }
                                            catch { }
                                            #endregion file info

                                            //delete file from T24 ftp path
                                            sftp.DeleteFile(remoteDirectory + remoteFileName);
                                        }
                                        #endregion write file
                                        #region read CVS
                                        long FileLength = new System.IO.FileInfo(downsavepath + remoteFileName).Length;
                                        if (FileLength > 0)
                                        {
                                            string[,] values = c.LoadCsv(downsavepath + remoteFileName);
                                            int num_rows = values.GetUpperBound(0) + 1;
                                            int num_cols = values.GetUpperBound(1) + 1;
                                            for (int r = 0; r < num_rows; r++)
                                            {
                                                #region Params
                                                string LoanRef = values[r, 0];
                                                string T24Error = values[r, 23];
                                                #region update record
                                                try
                                                {
                                                    SqlConnection Con1 = new SqlConnection(c.ConStr());
                                                    Con1.Open();
                                                    SqlCommand Com1 = new SqlCommand();
                                                    Com1.Connection = Con1;
                                                    Com1.Parameters.Clear();
                                                    //Com1.CommandText = "update T24_DisbPost set CBSStatusID=2,CBSRemark=@CBSRemark where DisbPostID=(select max(p.DisbPostID) from T24_DisbPost p left join T24_Disb d on d.DisbID=p.DisbID where p.fName='" + OriPostFileName + "' and d.LoanRef='" + LoanRef + "')";
                                                    Com1.CommandText = "exec T24_DisbPostErrorLogFromCBS @CBSRemark=@CBSRemark,@OriPostFileName=@OriPostFileName,@LoanRef=@LoanRef";
                                                    Com1.Parameters.AddWithValue("@CBSRemark", T24Error);
                                                    Com1.Parameters.AddWithValue("@OriPostFileName", OriPostFileName);
                                                    Com1.Parameters.AddWithValue("@LoanRef", LoanRef);
                                                    Com1.ExecuteNonQuery();
                                                    Con1.Close();
                                                    Con1.Dispose();
                                                    SqlConnection.ClearAllPools();
                                                }
                                                catch { }
                                                #endregion update record
                                                #endregion Params
                                            }
                                        }

                                        #endregion read CVS                                    
                                        #region update T24_DisbPostCom
                                        try
                                        {
                                            c.ReturnDT("exec T24_DisbPostLogCompulsoryForPostUpdateToPost @fName='" + OriPostFileName + "'");
                                        }
                                        catch { }
                                        #endregion update T24_DisbPostCom
                                    }
                                        #endregion is file existed?

                                    }

                                }
                            }                            
                        }
                    }
                }
                #endregion sFTP
            }
            catch (Exception ex)
            {
                ERR = "Error";
                SMS = "Something was wrong";
            }
            List<DisbPostToCBSErrorResModel> RSData = new List<DisbPostToCBSErrorResModel>();
            DisbPostToCBSErrorResModel ResSMS = new DisbPostToCBSErrorResModel();
            ResSMS.ERR = ERR;
            ResSMS.SMS = SMS;
            RSData.Add(ResSMS);
            return RSData;
        }
        
    }

    public class DisbPostToCBSErrorResModel
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
    }
}