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
    public class RepayGetFromCBSController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<RepayGetFromCBSResModel> Get([FromUri]string api_name, string api_key)
        {
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "";
            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            ServerDate = ServerDate.Replace("-", "_").Replace(" ", "_").Replace("HH:mm:ss.fff", "");
            try {                
                //Add log
                c.T24_AddLog("RepayGetFromCBS_" + ServerDate.Replace("-", "_").Replace(" ", "_").Replace("HH:mm:ss.fff", ""), "RepayGetFromCBS", "api_name: " + api_name, "RepayGetFromCBS");

                //file
                string StrDate = DateTime.Now.ToString("yyyyMMdd");
                string FName = "REPAY_" + StrDate + ".csv";//REPAY_20170407.csv
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
                //downsavepath= downsavepath+"_"+ServerDate;
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
                            //remoteFileName = remoteFileName + "_" + ServerDate.Replace(":", "").Replace(" ", "").Replace(".", "_")+".csv";
                            //if (FName == remoteFileName)//live
                            if (remoteFileName.StartsWith("REPAY_"))//Test
                            {
                                #region is file existed?
                                int isAllow = 1;
                                if (File.Exists(downsavepath + remoteFileName))
                                {
                                    try
                                    {
                                        DataTable dtIsFileExist = new DataTable();
                                        dtIsFileExist = c.ReturnDT("select SettingValue from tblSetting where SettingID=2");
                                        isAllow = Convert.ToInt16(dtIsFileExist.Rows[0]["SettingValue"]);
                                        if (isAllow > 0)
                                        {
                                            c.ReturnDT("update tblSetting set SettingValue=SettingValue-1 where SettingID=2");
                                        }
                                    }
                                    catch { }
                                }
                                #endregion is file existed?

                                if (isAllow > 0) {
                                    //write file
                                    using (Stream fileStream = File.Create(downsavepath + remoteFileName))
                                    {
                                        sftp.DownloadFile(remoteDirectory + remoteFileName, fileStream);                                                                                                                      
                                        sftp.DeleteFile(remoteDirectory + remoteFileName);
                                    }
                                    try {
                                        string copyto = downsavepath + remoteFileName + "_" + ServerDate.Replace(":", "").Replace(" ", "").Replace(".", "_") + ".csv";
                                        File.Copy(downsavepath + remoteFileName, copyto);
                                    } catch { }
                                    #region to db
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
                                        Com1.Parameters.AddWithValue("@FType", "REPAY");
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

                                    //Desc11 = "Desc: Repay Header: " + SMS;
                                    //backgroundWorker11.ReportProgress(5);

                                    #region exec details
                                    if (Err != "0")
                                    {
                                        string rowLog = "";
                                        try
                                        {
                                            #region read CVS
                                            string[,] values = c.LoadCsv(downsavepath + remoteFileName);
                                            int num_rows = values.GetUpperBound(0) + 1;
                                            int num_cols = values.GetUpperBound(1) + 1;
                                            #endregion read CVS
                                            for (int r = 0; r < num_rows; r++)
                                            {
                                                try
                                                {
                                                    rowLog = r.ToString();
                                                    #region Params
                                                    string LoanRef = values[r, 0];
                                                    string COID = values[r, 1];
                                                    string CollDate = values[r, 2];
                                                    string UnderVBFlag = values[r, 3];
                                                    string VBRef = values[r, 4];
                                                    string GroupID = values[r, 5];
                                                    string CustomerID = values[r, 6];
                                                    string CustomerName = values[r, 7];
                                                    string LoanCcy = values[r, 8];
                                                    string LoanType = values[r, 9];
                                                    string PriDue = values[r, 10];
                                                    string IntDue = values[r, 11];
                                                    string PenDue = values[r, 12];
                                                    string TotDueAmt = values[r, 13];
                                                    string RoundTotAmt = values[r, 14];
                                                    string MaturityDate = values[r, 15];
                                                    string ClosureAmt = values[r, 16];
                                                    string RoundClosAmt = values[r, 17];
                                                    string Currency = values[r, 18];
                                                    string PayAccount = values[r, 19];
                                                    string PayAcBal = values[r, 20];
                                                    string LoanBrnCode = values[r, 22];
                                                    string MonthlyFee = values[r, 23];
                                                    #endregion Params
                                                    #region exec sql
                                                    sql = "exec T24_AddRepay @RepayHeadID=@RepayHeadID,@LoanRef=@LoanRef,@COID=@COID,@CollDate=@CollDate,@UnderVBFlag=@UnderVBFlag,@VBRef=@VBRef,@GroupID=@GroupID,@CustomerID=@CustomerID,@CustomerName=@CustomerName,@LoanCcy=@LoanCcy,@LoanType=@LoanType,@PriDue=@PriDue,@IntDue=@IntDue,@PenDue=@PenDue,@TotDueAmt=@TotDueAmt,@RoundTotAmt=@RoundTotAmt,@MaturityDate=@MaturityDate,@ClosureAmt=@ClosureAmt,@RoundClosAmt=@RoundClosAmt,@Currency=@Currency,@PayAccount=@PayAccount,@PayAcBal=@PayAcBal,@LoanBrnCode=@LoanBrnCode,@MonthlyFee=@MonthlyFee";
                                                    Com1.Parameters.Clear();
                                                    Com1.CommandText = sql;
                                                    Com1.Parameters.AddWithValue("@RepayHeadID", Err);
                                                    Com1.Parameters.AddWithValue("@LoanRef", LoanRef);
                                                    Com1.Parameters.AddWithValue("@COID", COID);
                                                    Com1.Parameters.AddWithValue("@CollDate", CollDate);
                                                    Com1.Parameters.AddWithValue("@UnderVBFlag", UnderVBFlag);
                                                    Com1.Parameters.AddWithValue("@VBRef", VBRef);
                                                    Com1.Parameters.AddWithValue("@GroupID", GroupID);
                                                    Com1.Parameters.AddWithValue("@CustomerID", CustomerID);
                                                    Com1.Parameters.AddWithValue("@CustomerName", CustomerName);
                                                    Com1.Parameters.AddWithValue("@LoanCcy", LoanCcy);
                                                    Com1.Parameters.AddWithValue("@LoanType", LoanType);
                                                    Com1.Parameters.AddWithValue("@PriDue", PriDue);
                                                    Com1.Parameters.AddWithValue("@IntDue", IntDue);
                                                    Com1.Parameters.AddWithValue("@PenDue", PenDue);
                                                    Com1.Parameters.AddWithValue("@TotDueAmt", TotDueAmt);
                                                    Com1.Parameters.AddWithValue("@RoundTotAmt", RoundTotAmt);
                                                    Com1.Parameters.AddWithValue("@MaturityDate", MaturityDate);
                                                    Com1.Parameters.AddWithValue("@ClosureAmt", ClosureAmt);
                                                    Com1.Parameters.AddWithValue("@RoundClosAmt", RoundClosAmt);
                                                    Com1.Parameters.AddWithValue("@Currency", Currency);
                                                    Com1.Parameters.AddWithValue("@PayAccount", PayAccount);
                                                    Com1.Parameters.AddWithValue("@PayAcBal", PayAcBal);
                                                    Com1.Parameters.AddWithValue("@LoanBrnCode", LoanBrnCode);
                                                    Com1.Parameters.AddWithValue("@MonthlyFee", MonthlyFee);
                                                    Com1.ExecuteNonQuery();
                                                    #endregion sql
                                                }
                                                catch (Exception ex)
                                                {
                                                    //add ErrorReadFile
                                                    c.T24_AddLog("ErrorReadFile_" + remoteFileName + "_" + ServerDate.Replace("-", "_").Replace(" ", "_").Replace("HH:mm:ss.fff", ""), "RepayGetFromCBS", "at line: " + c.GetLineNumber(ex) + " | CSV at line: " + r.ToString(), "RepayGetFromCBS");
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            Err = "0";
                                            SMS = "Error exec details: " + rowLog + " | " + ex.Message.ToString();
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
                                    #endregion to db
                                }
                                
                            }
                        }
                    }
                }
                #endregion sFTP
            }
            catch {
                ERR = "Error";
                SMS = "Something was wrong";
            }

            if (ERR == "Error") {
                //Add log
                c.T24_AddLog("RepayGetFromCBS_Error_" + ServerDate.Replace("-", "_").Replace(" ", "_").Replace("HH:mm:ss.fff", ""), "RepayGetFromCBS", "SMS: " + SMS, "RepayGetFromCBS");
            }

            List<RepayGetFromCBSResModel> RSData = new List<RepayGetFromCBSResModel>();
            RepayGetFromCBSResModel ResSMS = new RepayGetFromCBSResModel();
            ResSMS.ERR = ERR;
            ResSMS.SMS = SMS;
            RSData.Add(ResSMS);
            return RSData;
        }
        public void BulkInsertToDB(string conStr,SftpClient sftp, string path) {
            var sqlBulk = new SqlBulkCopy(conStr);
            sqlBulk.DestinationTableName = "T24_Repay";
            var count = 0;
            var c = 0;
            var lines = sftp.ReadAllLines(path);
            int PerBulk = 10000;
            int Counter = 0;
            if (lines.Count() == 0) return;
            int numRow = lines.Count();
            var columns = lines[0].Split(',');
            var table = new DataTable();
            // Add column to datatable or header
            for (int f = 0; f < columns.Count(); f++)
            {
                table.Columns.Add(f.ToString());
            }
            // loop to add row
            for (int i = 0; i < numRow; i++)
            {
                Counter++;
                var col = lines[i].Split(',');
                var str = @"";

                /// get 39 columns for row 
                for (int a = 0; a < 22; a++)
                {
                    if (a == 22)
                    {
                        str += col[a];
                    }
                    else
                    {
                        str += col[a] + ",";
                    }

                }
                /// Condition to insert Data to database Per Bulk
                if (Counter == PerBulk || i == numRow - 1)
                {
                    count++;
                    c++;
                    //add row to datatable
                    table.Rows.Add(str.Split(','));
                    /// Insert data to database per bulk
                    sqlBulk.WriteToServer(table);
                    /// Clear rows of datatable for insert next bulk
                    table.Rows.Clear();
                    Counter = 0;

                }
                else if (Counter < PerBulk)
                {
                    c++;
                    //add row to datatable
                    table.Rows.Add(str.Split(','));
                }

            }


        }
        
    }
}