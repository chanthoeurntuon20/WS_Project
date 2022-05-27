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
    public class DisbGetFromCBSController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<DisbGetFromCBSResModel> Get([FromUri]string api_name, string api_key)
        {
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "";
            try
            {
                string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                ServerDate = ServerDate.Replace("-", "_").Replace(" ", "_").Replace("HH:mm:ss.fff", "");
                //Add log
                c.T24_AddLog("DisbGetFromCBS_" + ServerDate.Replace("-", "_").Replace(" ", "_").Replace("HH:mm:ss.fff", ""), "DisbGetFromCBS", "api_name: " + api_name, "DisbGetFromCBS");

                //file
                string StrDate = DateTime.Now.ToString("yyyyMMdd");
                string FName = "DISB_" + StrDate + ".csv";//DISB_20170407.csv

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
                //string downsavepath = Application.StartupPath + "\\T24CSV\\Download\\";
                string downsavepath = HttpContext.Current.Server.MapPath("~/Download/");
                //downsavepath = downsavepath + "_" + ServerDate;
                string remoteDirectory = T24PathStr;// "/Oradata/export/home/t24mst/bnk/bnk.interface/tabout/";
                using (var sftp = new SftpClient(host, username, password))
                {
                    sftp.Connect();
                    var files = sftp.ListDirectory(remoteDirectory);
                    foreach (var file in files)
                    {
                        if (!file.Name.StartsWith("."))
                        {
                            string remoteFileName = file.Name;
                            //remoteFileName = remoteFileName + "_" + ServerDate.Replace(":", "").Replace(" ", "").Replace(".", "_") + ".csv";
                            if (remoteFileName.StartsWith("DISB_"))
                            {
                                #region is file existed?
                                int DISBAllow = 1;
                                if (File.Exists(downsavepath + remoteFileName))
                                {
                                    try
                                    {
                                        DataTable dtIsFileExist = new DataTable();
                                        dtIsFileExist = c.ReturnDT("select SettingValue from tblSetting where SettingID=1");
                                        DISBAllow = Convert.ToInt16(dtIsFileExist.Rows[0]["SettingValue"]);
                                        if (DISBAllow > 0)
                                        {
                                            c.ReturnDT("update tblSetting set SettingValue=SettingValue-1 where SettingID=1");
                                        }
                                    }
                                    catch { }
                                }
                                #endregion is file existed?
                                if (DISBAllow > 0)
                                {
                                    //write file
                                    using (Stream fileStream = File.Create(downsavepath + remoteFileName))
                                    {
                                        sftp.DownloadFile(remoteDirectory + remoteFileName, fileStream);
                                        sftp.DeleteFile(remoteDirectory + remoteFileName);
                                    }
                                    try
                                    {
                                        string copyto = downsavepath + remoteFileName + "_" + ServerDate.Replace(":", "").Replace(" ", "").Replace(".", "_") + ".csv";
                                        File.Copy(downsavepath + remoteFileName, copyto);
                                    }
                                    catch { }
                                    //to db
                                    string Err = ""; SMS = "";
                                    #region create sql trn
                                    SqlConnection Con1 = new SqlConnection(c.ConStr());
                                    Con1.Open();
                                    //SqlTransaction Tran1;
                                    SqlDataReader reader = null;
                                    //Tran1 = Con1.BeginTransaction();
                                    SqlCommand Com1 = new SqlCommand();
                                    Com1.Connection = Con1;
                                    //Com1.Transaction = Tran1;
                                    string sql = "";
                                    #endregion create sql trn
                                    #region check/Create File Header
                                    try
                                    {
                                        sql = "exec T24_AddFileHeader @FName=@FName,@FType=@FType";
                                        Com1.Parameters.Clear();
                                        Com1.CommandText = sql;
                                        Com1.Parameters.AddWithValue("@FName", remoteFileName);
                                        Com1.Parameters.AddWithValue("@FType", "DISB");
                                        DataTable dtr = new DataTable();
                                        reader = Com1.ExecuteReader();
                                        dtr.Load(reader);
                                        Err = dtr.Rows[0]["Err"].ToString();
                                        SMS = dtr.Rows[0]["SMS"].ToString();
                                    }
                                    catch (Exception ex)
                                    {
                                        Err = "0";
                                        SMS = "Error check/Create File Header: " + ex.Message.ToString();
                                    }
                                    #endregion check/Create File Header

                                    #region exec details
                                    if (Err != "0")
                                    {
                                        try
                                        {
                                            #region read CVS
                                            string[,] values = c.LoadCsv(downsavepath + remoteFileName);
                                            int num_rows = values.GetUpperBound(0) + 1;
                                            int num_cols = values.GetUpperBound(1) + 1;
                                            //add log
                                            try {
                                                c.T24_AddLog("ErrorReadFile_" + remoteFileName + "_" + ServerDate.Replace("-", "_").Replace(" ", "_").Replace("HH:mm:ss.fff", ""), "DisbGetFromCBS", num_rows.ToString(), "DisbGetFromCBS"); 
                                            } catch { }
                                            #endregion read CVS
                                            for (int r = 0; r < num_rows; r++)
                                            {
                                                try
                                                {
                                                    #region Params
                                                    string LoanRef = values[r, 0];
                                                    string COID = values[r, 1];
                                                    string VBID = values[r, 2];
                                                    string CustomerID = values[r, 3];
                                                    string CustomerName = values[r, 4];
                                                    string LoanRefID = values[r, 5];
                                                    string ProdID = values[r, 6];
                                                    string DisbAccount = values[r, 7];
                                                    string DisbCCY = values[r, 8];
                                                    string DisbAmt = values[r, 9];
                                                    string InitPayDate = values[r, 10];
                                                    string MatDate = values[r, 11];
                                                    string CHGAmt = values[r, 12];
                                                    string BranchCode = values[r, 13];
                                                    string VBRefer = values[r, 14];
                                                    string GroupID = values[r, 18];
                                                    string DisbDate = values[r, 19];
                                                    #endregion Params
                                                    #region exec sql
                                                    sql = "exec T24_AddDisb @RepayHeadID=@RepayHeadID,@LoanRef=@LoanRef,@COID=@COID,@VBID=@VBID,@CustomerID=@CustomerID,@CustomerName=@CustomerName,@LoanRefID=@LoanRefID,@ProdID=@ProdID,@DisbAccount=@DisbAccount,@DisbCCY=@DisbCCY,@DisbAmt=@DisbAmt,@InitPayDate=@InitPayDate,@MatDate=@MatDate,@CHGAmt=@CHGAmt,@BranchCode=@BranchCode,@VBRefer=@VBRefer,@GroupID=@GroupID,@DisbDate=@DisbDate";
                                                    Com1.Parameters.Clear();
                                                    Com1.CommandText = sql;
                                                    Com1.Parameters.AddWithValue("@RepayHeadID", Err);
                                                    Com1.Parameters.AddWithValue("@LoanRef", LoanRef);
                                                    Com1.Parameters.AddWithValue("@COID", COID);
                                                    Com1.Parameters.AddWithValue("@VBID", VBID);
                                                    Com1.Parameters.AddWithValue("@CustomerID", CustomerID);
                                                    Com1.Parameters.AddWithValue("@CustomerName", CustomerName);
                                                    Com1.Parameters.AddWithValue("@LoanRefID", LoanRefID);
                                                    Com1.Parameters.AddWithValue("@ProdID", ProdID);
                                                    Com1.Parameters.AddWithValue("@DisbAccount", DisbAccount);
                                                    Com1.Parameters.AddWithValue("@DisbCCY", DisbCCY);
                                                    Com1.Parameters.AddWithValue("@DisbAmt", DisbAmt);
                                                    Com1.Parameters.AddWithValue("@InitPayDate", InitPayDate);
                                                    Com1.Parameters.AddWithValue("@MatDate", MatDate);
                                                    Com1.Parameters.AddWithValue("@CHGAmt", CHGAmt);
                                                    Com1.Parameters.AddWithValue("@BranchCode", BranchCode);
                                                    Com1.Parameters.AddWithValue("@VBRefer", VBRefer);
                                                    Com1.Parameters.AddWithValue("@GroupID", GroupID);
                                                    Com1.Parameters.AddWithValue("@DisbDate", DisbDate);
                                                    Com1.ExecuteNonQuery();
                                                    //DataTable dt1 = new DataTable();
                                                    //reader = Com1.ExecuteReader();
                                                    //dt1.Load(reader);
                                                    #endregion sql     
                                                   
                                                }
                                                catch (Exception ex)
                                                {
                                                    //add ErrorReadFile
                                                    c.T24_AddLog("ErrorReadFile_" + remoteFileName + "_" + ServerDate.Replace("-", "_").Replace(" ", "_").Replace("HH:mm:ss.fff", ""), "DisbGetFromCBS", "at line: " + c.GetLineNumber(ex) + " | CSV at line: " + r.ToString(), "DisbGetFromCBS");
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            Err = "0";
                                            SMS = "Error exec details: " + ex.Message.ToString();
                                        }
                                    }
                                    #endregion exec details
                                    #region Commit or Rollback
                                    if (Err != "0")
                                    {
                                        //Tran1.Commit();
                                        Con1.Close();
                                    }
                                    else
                                    {
                                        //Tran1.Rollback();
                                        Con1.Close();
                                        //write error log
                                        ERR = "Error";
                                    }
                                    try { reader.Close(); } catch { }
                                    #endregion Commit or Rollback
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
            List<DisbGetFromCBSResModel> RSData = new List<DisbGetFromCBSResModel>();
            DisbGetFromCBSResModel ResSMS = new DisbGetFromCBSResModel();
            ResSMS.ERR = ERR;
            ResSMS.SMS = SMS;
            RSData.Add(ResSMS);
            return RSData;
        }

    }

    public class DisbGetFromCBSResModel
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
    }

}