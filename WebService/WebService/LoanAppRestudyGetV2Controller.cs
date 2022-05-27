using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace WebService
{
    [BasicAuthentication]
    public class LoanAppRestudyGetV2Controller : ApiController
    {
        public IEnumerable<LoanAppRestudyGetV2RS> Post([FromUri]string api_name, string api_key, string username, [FromBody]string json) {
            #region incoming
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "", ExSMS = "";
            List<LoanAppRestudyGetV2RS> RSData = new List<LoanAppRestudyGetV2RS>();
            LoanAppRestudyGetV2RS ListHeader = new LoanAppRestudyGetV2RS();
            List<LoanAppRestudyGetV2RSData> data = new List<LoanAppRestudyGetV2RSData>();
            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string ServerDateForFileName = ServerDate.Replace("-", "_").Replace(" ", "_").Replace(":", "_");
            string ControllerName = "LoanAppRestudyGetV2";
            string FileNameForLog = username + "_" + api_name + "_" + ServerDateForFileName;
            #endregion incoming
            try
            {
                c.T24_AddLog(FileNameForLog, "RQ", json, ControllerName);

                #region check json
                if (json == null || json == "")
                {
                    ERR = "Error";
                    SMS = "Invalid JSON";
                }
                #endregion check json
                #region json
                string user = "", pwd = "", device_id = "", app_vName = "";
                LoanAppRestudyGetV2RQ jObj = null;
                if (ERR != "Error")
                {
                    try
                    {
                        jObj = JsonConvert.DeserializeObject<LoanAppRestudyGetV2RQ>(json.Replace("\r\n    ", ""));
                        user = jObj.user;
                        pwd = jObj.pwd;
                        device_id = jObj.device_id;
                        app_vName = jObj.app_vName;
                    }
                    catch
                    {
                        ERR = "Error";
                        SMS = "Invalid JSON";
                    }
                }
                #endregion json
                #region get userid
                string UserID = "";
                if (ERR != "Error")
                {
                    DataTable dt = c.ReturnDT("exec T24_check_user @user='" + user + "',@pwd='" + c.Encrypt(pwd, c.SeekKeyGet()) + "'");
                    ERR = dt.Rows[0]["ERR"].ToString();
                    SMS = dt.Rows[0]["SMS"].ToString();
                    UserID = SMS;
                }
                #endregion get userid
                #region GetData
                if (ERR != "Error")
                {
                    try {
                        string sql = "exec T24_LoanAppRestudyGetV2_IDList @COUserID='"+ UserID + "'";
                        DataTable dt = c.ReturnDT(sql);
                        for (int i = 0; i < dt.Rows.Count; i++) {
                            LoanAppRestudyGetV2RSData dd = new LoanAppRestudyGetV2RSData();
                            dd.LoanAppID = dt.Rows[i]["LoanAppID"].ToString();
                            data.Add(dd);
                        }
                    } catch (Exception ex)
                    {
                        ERR = "Error";
                        SMS = "Unable to get loan list";
                        ExSMS = ex.Message.ToString();
                    }
                    

                }
                #endregion
            }
            catch (Exception ex) {
                ERR = "Error";
                SMS = "Something was wrong";// at line:" + c.GetLineNumber(ex) + " | Ex:" + ex.Message.ToString();
                ExSMS = ex.Message.ToString();
            }
            #region return
            ListHeader.ERR = ERR;
            ListHeader.SMS = SMS;
            ListHeader.LoanAppIDList = data;
            RSData.Add(ListHeader);
            try
            {
                var jsonRS = new JavaScriptSerializer().Serialize(RSData);
                c.T24_AddLog(FileNameForLog, "RS", jsonRS.ToString(), ControllerName);
            }
            catch { }
            return RSData;
            #endregion return
        }        
    }
    public class LoanAppRestudyGetV2RQ
    {
        public string user { get; set; }
        public string pwd { get; set; }
        public string device_id { get; set; }
        public string app_vName { get; set; }
    }
    public class LoanAppRestudyGetV2RS
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
        public List<LoanAppRestudyGetV2RSData> LoanAppIDList;
    }
    public class LoanAppRestudyGetV2RSData
    {
        public string LoanAppID { get; set; }
    }

}