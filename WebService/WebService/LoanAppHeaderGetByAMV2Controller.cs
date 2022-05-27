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
    public class LoanAppHeaderGetByAMV2Controller : ApiController
    {
        public IEnumerable<LoanAppHeaderGetByAMV2RS> Post([FromUri]string api_name, string api_key, string username, [FromBody]string json) {
            #region incoming
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "", ExSMS = "";
            List<LoanAppHeaderGetByAMV2RS> RSData = new List<LoanAppHeaderGetByAMV2RS>();
            LoanAppHeaderGetByAMV2RS ListHeader = new LoanAppHeaderGetByAMV2RS();
            List<LoanAppHeaderGetByAMV2RSData> data = new List<LoanAppHeaderGetByAMV2RSData>();
            List<LoanAppHeaderGetByAMV2RSDataCO> dataCO = new List<LoanAppHeaderGetByAMV2RSDataCO>();
            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string ServerDateForFileName = ServerDate.Replace("-", "_").Replace(" ", "_").Replace(":", "_");
            string ControllerName = "LoanAppHeaderGetByAMV2";
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
                string KeyID="", user = "", pwd = "", device_id = "", app_vName = "", criteriaValue="";
                LoanAppHeaderGetByAMV2RQ jObj = null;
                if (ERR != "Error")
                {
                    try
                    {
                        jObj = JsonConvert.DeserializeObject<LoanAppHeaderGetByAMV2RQ>(json.Replace("\r\n    ", ""));
                        user = jObj.user;
                        pwd = jObj.pwd;
                        device_id = jObj.device_id;
                        app_vName = jObj.app_vName;
                        criteriaValue = jObj.criteriaValue;
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
                    if (criteriaValue == "")
                    {
                        KeyID = UserID;

                    }
                    else
                    {
                        KeyID = criteriaValue;
                    }
                }
                #endregion get userid
                #region GetData Loan
                if (ERR != "Error")
                {
                    try {
                        string sql = "exec T24_LoanAppGetByAM_Header @AMUserID='" + KeyID + "',@UserLoginID='" + UserID + "'";
                        DataTable dt = c.ReturnDT(sql);
                        if (dt.Rows.Count == 0)
                        {
                            ERR = "Error";
                            SMS = "No Data - Loan";
                        }
                        else {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                LoanAppHeaderGetByAMV2RSData dd = new LoanAppHeaderGetByAMV2RSData();
                                dd.LoanAppID = dt.Rows[i]["LoanAppID"].ToString();
                                dd.COName = dt.Rows[i]["COName"].ToString();
                                dd.VBName = dt.Rows[i]["VBName"].ToString();
                                dd.RequestDate = Convert.ToDateTime(dt.Rows[i]["RequestDate"]).ToString("yyyy-MM-dd");
                                dd.Customer = dt.Rows[i]["Customer"].ToString();
                                dd.GroupNo = dt.Rows[i]["GroupNo"].ToString();
                                dd.Product = dt.Rows[i]["Product"].ToString();
                                dd.LoanSize = dt.Rows[i]["LoanSize"].ToString();
                                dd.VBID = dt.Rows[i]["VBID"].ToString();
                                data.Add(dd);
                            }
                        }
                        
                    } catch (Exception ex)
                    {
                        ERR = "Error";
                        SMS = "Unable to get loan list";
                        ExSMS = ex.Message.ToString();
                    }

                }
                #endregion
                #region GetData CO
                if (ERR != "Error")
                {
                    try
                    {
                        string sql = "exec T24_GetCOUnderAMByAM @AMUserID='" + KeyID + "'";
                        DataTable dt = c.ReturnDT(sql);
                        if (dt.Rows.Count == 0)
                        {
                            ERR = "Error";
                            SMS = "No Data - CO";
                        }
                        else
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                LoanAppHeaderGetByAMV2RSDataCO dd = new LoanAppHeaderGetByAMV2RSDataCO();
                                dd.COUserID = dt.Rows[i]["COUserID"].ToString();
                                dd.UserName = dt.Rows[i]["UserName"].ToString();
                                dd.COName = dt.Rows[i]["COName"].ToString();
                                dd.AMUserID = dt.Rows[i]["AMUserID"].ToString();
                                dataCO.Add(dd);
                            }
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
            ListHeader.COList = dataCO;
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
    public class LoanAppHeaderGetByAMV2RQ
    {
        public string user { get; set; }
        public string pwd { get; set; }
        public string device_id { get; set; }
        public string app_vName { get; set; }
        public string criteriaValue { get; set; }
    }
    public class LoanAppHeaderGetByAMV2RS
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
        public List<LoanAppHeaderGetByAMV2RSData> LoanAppIDList;
        public List<LoanAppHeaderGetByAMV2RSDataCO> COList;
    }
    public class LoanAppHeaderGetByAMV2RSData
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
    public class LoanAppHeaderGetByAMV2RSDataCO
    {
        public string COUserID { get; set; }
        public string UserName { get; set; }
        public string COName { get; set; }
        public string AMUserID { get; set; }
    }

}