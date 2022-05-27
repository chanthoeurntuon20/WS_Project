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

namespace WebService
{
    public class VBLinkGetFromCBSController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<VBLinkGetFromCBSRes> Get(string api_name, string api_key)
        {
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "";
            try {
                #region api
                string[] CheckApi = c.CheckApi(api_name, api_key);
                ERR = CheckApi[0];
                SMS = CheckApi[1];
                #endregion api
                if (ERR != "Error") {
                    #region - get T24 path
                    string T24PathStr = "", T24PathUsername = "", T24PathPwd = "", T24Server = "";
                    DataTable dtT24Path = new DataTable();
                    dtT24Path = c.ReturnDT("select * from T24_Path where PathID=2");
                    T24PathStr = dtT24Path.Rows[0]["PathStr"].ToString();
                    T24PathUsername = dtT24Path.Rows[0]["Username"].ToString();
                    T24PathPwd = dtT24Path.Rows[0]["Pwd"].ToString();
                    T24Server = dtT24Path.Rows[0]["Server"].ToString();
                    #endregion - T24 path
                    #region connect to ftp
                    string host = T24Server;
                    string username = T24PathUsername;
                    string password = T24PathPwd;
                    string downsavepath = HttpContext.Current.Server.MapPath("~/Download/");
                    string remoteDirectory = T24PathStr;
                    using (var sftp = new SftpClient(host, username, password)) {
                        sftp.Connect();
                        var files = sftp.ListDirectory(remoteDirectory);
                        foreach (var file in files) {
                            if (!file.Name.StartsWith(".")) {
                                string remoteFileName = file.Name;
                                string CustFile = remoteFileName.Substring(0, 9);
                                if (CustFile == "MCB.GROUP") {
                                    #region is file existed?
                                    if (!File.Exists(downsavepath + remoteFileName))
                                    {
                                        #region write file
                                        using (Stream fileStream = File.Create(downsavepath + remoteFileName))
                                        {
                                            sftp.DownloadFile(remoteDirectory + remoteFileName, fileStream);
                                        }
                                        #region read CVS
                                        string[,] values = c.LoadCsv(downsavepath + remoteFileName);
                                        int num_rows = values.GetUpperBound(0) + 1;
                                        int num_cols = values.GetUpperBound(1) + 1;
                                        #endregion read CVS
                                        for (int r = 0; r < num_rows; r++)
                                        {
                                            #region Params
                                            string ID = values[r, 0];
                                            string GNAME = values[r, 1];
                                            string GTYPE = values[r, 2];
                                            string GSTATUS = values[r, 3];
                                            string GACCTOFFICER = values[r, 4];
                                            string GMEETINGFREQ = values[r, 5];
                                            string GSTREET = values[r, 6];
                                            string GADDRESS = values[r, 7];
                                            string GTOWN = values[r, 8];
                                            string GPOSTCODE = values[r, 9];
                                            string AMKPROVINCE = values[r, 10];
                                            string AMKDISTRICT = values[r, 11];
                                            string AMKCOMMUNE = values[r, 12];
                                            string AMKVILLAGE = values[r, 13];
                                            #endregion Params
                                            #region add to db
                                            SqlConnection Con1 = new SqlConnection(c.ConStr());
                                            Con1.Open();
                                            SqlCommand Com1 = new SqlCommand();
                                            Com1.Connection = Con1;
                                            Com1.Parameters.Clear();
                                            Com1.CommandText = "exec T24_AddUpdateVBLink @VBID=@VBID,@VBName=@VBName,@Status=@Status,@COID=@COID,@MeetingDate=@MeetingDate,@GSTREET=@GSTREET,@GADDRESS=@GADDRESS,@GTOWN=@GTOWN,@GPOSTCODE=@GPOSTCODE,@AMKPROVINCE=@AMKPROVINCE,@AMKDISTRICT=@AMKDISTRICT,@AMKCOMMUNE=@AMKCOMMUNE,@AMKVILLAGE=@AMKVILLAGE";
                                            #region params
                                            Com1.Parameters.AddWithValue("@VBID", ID);
                                            Com1.Parameters.AddWithValue("@VBName", GNAME);
                                            Com1.Parameters.AddWithValue("@Status", GSTATUS);
                                            Com1.Parameters.AddWithValue("@COID", GACCTOFFICER);
                                            Com1.Parameters.AddWithValue("@MeetingDate", GMEETINGFREQ);
                                            Com1.Parameters.AddWithValue("@GSTREET", GSTREET);
                                            Com1.Parameters.AddWithValue("@GADDRESS", GADDRESS);
                                            Com1.Parameters.AddWithValue("@GTOWN", GTOWN);
                                            Com1.Parameters.AddWithValue("@GPOSTCODE", GPOSTCODE);
                                            Com1.Parameters.AddWithValue("@AMKPROVINCE", AMKPROVINCE);
                                            Com1.Parameters.AddWithValue("@AMKDISTRICT", AMKDISTRICT);
                                            Com1.Parameters.AddWithValue("@AMKCOMMUNE", AMKCOMMUNE);
                                            Com1.Parameters.AddWithValue("@AMKVILLAGE", AMKVILLAGE);
                                            #endregion params
                                            Com1.ExecuteNonQuery();
                                            Con1.Close();
                                            #endregion add to db
                                        }
                                        #endregion write file
                                    }
                                    else {
                                        sftp.DeleteFile(remoteDirectory + remoteFileName);
                                    }
                                    #endregion is file existed?
                                }
                            }
                        }

                    }
                    #endregion connect to ftp
                }


            }
            catch(Exception ex) { }
            List<VBLinkGetFromCBSRes> RSData = new List<VBLinkGetFromCBSRes>();
            VBLinkGetFromCBSRes ResSMS = new VBLinkGetFromCBSRes();
            ResSMS.ERR = ERR;
            ResSMS.SMS = SMS;
            RSData.Add(ResSMS);
            return RSData;
        }
        

    }
    public class VBLinkGetFromCBSRes
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
    }
}