using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace WebService
{
    [BasicAuthentication]
    public class LoanAppPostV2ConfirmController : ApiController
    {
        public string Post([FromUri]string p, [FromUri]string msgid, [FromBody]string json)
        {
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "", ExSMS = "", ERRCode = "";
            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string ControllerName = "LoanAppPostV2ConfirmV2";
            string FileNameForLog = msgid + "_" + ServerDate.Replace("-", "_").Replace(" ", "_").Replace(":", "_").Replace(".", "_");
            List<LoanAppPostV2ConfirmRS> RSData = new List<LoanAppPostV2ConfirmRS>();
            try
            {
                #region msgid
                if (ERR != "Error")
                {
                    string[] str = c.CheckMsgID(msgid);
                    ERR = str[0];
                    SMS = str[1];
                }
                #endregion
                #region add log
                if (ERR != "Error")
                {
                    c.T24_AddLog(FileNameForLog, "1.RQ-p", p, ControllerName);
                    c.T24_AddLog(FileNameForLog, "1.1.RQ-json", json, ControllerName);
                }
                #endregion
                #region p -> SessionID
                string UserID = "";
                if (ERR != "Error")
                {
                    //p = System.Web.HttpUtility.UrlDecode(p);
                    string[] rs = c.SessionIDCheck(ServerDate, p);
                    ERR = rs[0];
                    SMS = rs[1];
                    ExSMS = rs[2];
                    UserID = rs[3];
                    ERRCode = rs[4];
                }
                #endregion
                #region check json
                if (ERR != "Error")
                {
                    string[] str = c.CheckObjED(json, "2");
                    ERR = str[0];
                    SMS = str[1];
                    ExSMS = str[2];
                    json = str[3];
                }
                #endregion check json
                #region read json
                LoanAppPostV2ConfirmRQ jObj = null;
                string LoanAppID = "";
                if (ERR != "Error")
                {
                    try
                    {
                        jObj = JsonConvert.DeserializeObject<LoanAppPostV2ConfirmRQ>(json);
                        //pms_sp = jObj.pms_sp;
                        LoanAppID = jObj.LoanAppID;
                    }
                    catch (Exception ex)
                    {
                        ERR = "Error";
                        ExSMS = ex.Message.ToString();
                        //get sms
                        string[] str = c.GetSMSByMsgID("10");
                        ERR = str[0];
                        if (ERR == "Error")
                        {
                            SMS = str[1];
                            ExSMS = ExSMS + "|" + str[2];
                        }
                        else
                        {
                            SMS = str[3];
                        }
                        ERR = "Error";
                    }
                }
                #endregion

                #region LoanAppPostV2Confirm
                if (ERR != "Error")
                {
                    SqlConnection Con1 = new SqlConnection(c.ConStr());
                    Con1.Open();
                    SqlCommand Com1 = new SqlCommand();
                    Com1.Connection = Con1;
                    //c.ReturnDT("exec T24_LoanAppActive @LoanAppID='" + ServerLoanAppID + "'");
                    string sql = "exec T24_LoanAppActiveConfirmV2 @LoanAppID=@LoanAppID";
                    Com1.CommandText = sql;
                    Com1.Parameters.Clear();
                    Com1.Parameters.AddWithValue("@LoanAppID", LoanAppID);
                    DataTable dt1 = new DataTable();
                    dt1.Load(Com1.ExecuteReader());
                    ERR = dt1.Rows[0]["ERR"].ToString();
                    SMS = dt1.Rows[0]["SMS"].ToString();
                    Con1.Close();
                }
                #endregion LoanAppPostV2Confirm

            }
            catch (Exception ex)
            {
                ERR = "Error";
                SMS = "Something was wrong at line:" + c.GetLineNumber(ex) + " | Ex:" + ex.Message.ToString();
            }

            LoanAppPostV2ConfirmRS data = new LoanAppPostV2ConfirmRS();
            data.ERR = ERR;
            data.SMS = SMS;
            data.ERRCode = ERRCode;
            RSData.Add(data);

            string RSDataStr = "";
            try
            {
                var jsonRS = new JavaScriptSerializer().Serialize(RSData);
                RSDataStr = c.Encrypt(jsonRS, c.SeekKeyGet());
                c.T24_AddLog(FileNameForLog, "4.RS", jsonRS.ToString(), ControllerName);
            }
            catch { }
            return RSDataStr;
        }

        // POST api/<controller>
        //public IEnumerable<LoanAppPostV2ConfirmRS> Post([FromUri]string api_name, string api_key, string username, [FromBody]string json)
        //{
        //    Class1 c = new Class1();
        //    string ERR = "Succeed", SMS = "";
        //    string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        //    List<LoanAppPostV2ConfirmRS> RSData = new List<LoanAppPostV2ConfirmRS>();
        //    string ControllerName = "LoanAppPostV2ConfirmV2";
        //    string FileNameForLog = username+"_"+api_name + "_" + ServerDate.Replace("-", "_").Replace(" ", "_").Replace(":", "_");
        //    try {
        //        //Add log
        //        c.T24_AddLog(FileNameForLog, "RQ", json, ControllerName);

        //        #region check json
        //        if (json == null || json == "")
        //        {
        //            ERR = "Error";
        //            SMS = "Invalid JSON";
        //        }
        //        #endregion check json
        //        LoanAppPostV2ConfirmRQ jObj = null;
        //        string user="",pwd="",device_id="",app_vName = "", mac_address="",LoanAppID="";
        //        #region json
        //        if (ERR != "Error")
        //        {
        //            try
        //            {
        //                jObj = JsonConvert.DeserializeObject<LoanAppPostV2ConfirmRQ>(json);
        //                user = jObj.user;
        //                user = c.Encrypt(user, c.SeekKeyGet());
        //                pwd = jObj.pwd;
        //                pwd=c.Encrypt(pwd, c.SeekKeyGet());
        //                device_id = jObj.device_id;
        //                app_vName = jObj.app_vName;
        //                mac_address = jObj.mac_address;
        //                LoanAppID = jObj.LoanAppID;
        //            }
        //            catch
        //            {
        //                ERR = "Error";
        //                SMS = "Invalid JSON";
        //            }
        //        }
        //        #endregion json
        //        #region LoanAppPostV2Confirm
        //        if (ERR != "Error") {
        //            SqlConnection Con1 = new SqlConnection(c.ConStr());
        //            Con1.Open();
        //            SqlCommand Com1 = new SqlCommand();
        //            Com1.Connection = Con1;
        //            //c.ReturnDT("exec T24_LoanAppActive @LoanAppID='" + ServerLoanAppID + "'");
        //            string sql = "exec T24_LoanAppActiveConfirmV2 @LoanAppID=@LoanAppID";
        //            Com1.CommandText = sql;
        //            Com1.Parameters.Clear();
        //            Com1.Parameters.AddWithValue("@LoanAppID", LoanAppID);
        //            DataTable dt1 = new DataTable();
        //            dt1.Load(Com1.ExecuteReader());
        //            ERR = dt1.Rows[0]["ERR"].ToString();
        //            SMS = dt1.Rows[0]["SMS"].ToString();
        //            Con1.Close();
        //        }
        //        #endregion LoanAppPostV2Confirm

        //    }
        //    catch (Exception ex) {
        //        ERR = "Error";
        //        SMS = "Something was wrong at line:" + c.GetLineNumber(ex) + " | Ex:" + ex.Message.ToString();                
        //    }

        //    LoanAppPostV2ConfirmRS data = new LoanAppPostV2ConfirmRS();
        //    data.ERR = ERR;
        //    data.SMS = SMS;
        //    RSData.Add(data);

        //    try
        //    {
        //        var jsonRS = new JavaScriptSerializer().Serialize(RSData);
        //        c.T24_AddLog(FileNameForLog, "RS", jsonRS.ToString(), ControllerName);
        //    }
        //    catch { }

        //    return RSData;
        //}

    }
    public class LoanAppPostV2ConfirmRQ
    {
        public string user { get; set; }
        public string pwd { get; set; }
        public string device_id { get; set; }
        public string app_vName { get; set; }
        public string mac_address { get; set; }
        public string LoanAppID { get; set; } //LoanApp Server ID
    }

    public class LoanAppPostV2ConfirmRS
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
        public string ERRCode { get; set; }
    }

}