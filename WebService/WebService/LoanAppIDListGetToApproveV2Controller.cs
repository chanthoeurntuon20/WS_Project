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
    public class LoanAppIDListGetToApproveV2Controller : ApiController
    {
        public IEnumerable<LoanAppIDListGetToApproveV2RS> Post([FromUri]string api_name, string api_key, string username, [FromBody]string json) {
            #region incoming
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "", ExSMS = "";
            List<LoanAppIDListGetToApproveV2RS> RSData = new List<LoanAppIDListGetToApproveV2RS>();
            LoanAppIDListGetToApproveV2RS ListHeader = new LoanAppIDListGetToApproveV2RS();
            List<LoanAppIDListGetToApproveV2RSLoan> LoanList = new List<LoanAppIDListGetToApproveV2RSLoan>();
            List<LoanAppIDListGetToApproveV2RSCO> COList=new List<LoanAppIDListGetToApproveV2RSCO>();
            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string ServerDateForFileName = ServerDate.Replace("-", "_").Replace(" ", "_").Replace(":", "_");
            string ControllerName = "LoanAppIDListGetToApproveV2";
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
                LoanAppIDListGetToApproveV2RQ jObj = null;
                if (ERR != "Error")
                {
                    try
                    {
                        jObj = JsonConvert.DeserializeObject<LoanAppIDListGetToApproveV2RQ>(json.Replace("\r\n    ", ""));
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
                #region loan list
                if (ERR != "Error")
                {
                    try {
                        string sql = "exec T24_LoanAppGetByAM_Header @AMUserID='" + UserID + "',@UserLoginID='0'";
                        DataTable dt = c.ReturnDT(sql);
                        for (int i = 0; i < dt.Rows.Count; i++) {
                            LoanAppIDListGetToApproveV2RSLoan dd = new LoanAppIDListGetToApproveV2RSLoan();
                            dd.LoanAppID = dt.Rows[i]["LoanAppID"].ToString();
                            dd.COName = dt.Rows[i]["COName"].ToString();
                            dd.VBName = dt.Rows[i]["VBName"].ToString();
                            dd.RequestDate = dt.Rows[i]["RequestDate"].ToString();
                            dd.Customer = dt.Rows[i]["Customer"].ToString();
                            dd.GroupNo = dt.Rows[i]["GroupNo"].ToString();
                            dd.Product = dt.Rows[i]["Product"].ToString();
                            dd.LoanSize = dt.Rows[i]["LoanSize"].ToString();
                            dd.VBID = dt.Rows[i]["VBID"].ToString();
                            LoanList.Add(dd);
                        }
                    } catch (Exception ex)
                    {
                        ERR = "Error";
                        SMS = "Unable to get loan list";
                        ExSMS = ex.Message.ToString();
                    }
                }
                #endregion
                #region co list
                if (ERR != "Error")
                {
                    try
                    {
                        string sql = "exec T24_GetCOUnderAMByAM @AMUserID='" + UserID + "',@UserLoginID='0'";
                        DataTable dt = c.ReturnDT(sql);
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            LoanAppIDListGetToApproveV2RSCO dd = new LoanAppIDListGetToApproveV2RSCO();
                            dd.COUserID = dt.Rows[i]["COUserID"].ToString();
                            dd.UserName = dt.Rows[i]["UserName"].ToString();
                            dd.COName = dt.Rows[i]["COName"].ToString();
                            dd.AMUserID = dt.Rows[i]["AMUserID"].ToString();
                            COList.Add(dd);
                        }
                    }
                    catch (Exception ex)
                    {
                        ERR = "Error";
                        SMS = "Unable to get loan list";
                        ExSMS = ex.Message.ToString();
                    }
                }
                #endregion
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
            ListHeader.LoanAppIDList = LoanList;
            ListHeader.COUnderAMList = COList;
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
    public class LoanAppIDListGetToApproveV2RQ
    {
        public string user { get; set; }
        public string pwd { get; set; }
        public string device_id { get; set; }
        public string app_vName { get; set; }
    }
    public class LoanAppIDListGetToApproveV2RS
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
        public List<LoanAppIDListGetToApproveV2RSLoan> LoanAppIDList;
        public List<LoanAppIDListGetToApproveV2RSCO> COUnderAMList;
    }
    public class LoanAppIDListGetToApproveV2RSLoan
    {
        public string LoanAppID { get; set; }
        public string COName { get; set; }
        public string VBName { get; set; }
        public string RequestDate { get; set; }
        public string Customer { get; set; }
        public string GroupNo { get; set; }
        public string Product { get; set; }
        public string LoanSize { get; set; }
        public string VBID { get; set; }
    }
    public class LoanAppIDListGetToApproveV2RSCO
    {
        public string COUserID { get; set; }
        public string UserName { get; set; }
        public string COName { get; set; }
        public string AMUserID { get; set; }
    }

}