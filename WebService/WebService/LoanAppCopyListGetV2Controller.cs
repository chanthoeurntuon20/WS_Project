using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace WebService
{
    [BasicAuthentication]
    public class LoanAppCopyListGetV2Controller : ApiController
    {
        // POST api/<controller>
        public IEnumerable<LoanAppCopyListGetV2RS> Post([FromUri]string api_name, string api_key, [FromBody]string json)
        {
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "";
            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            List<LoanAppCopyListGetV2RS> RSData = new List<LoanAppCopyListGetV2RS>();
            string ControllerName = "LoanAppCopyListGetV2";
            string FileNameForLog = api_name + "_" + ServerDate.Replace("-", "_").Replace(" ", "_").Replace(":", "_");
            try
            {
                c.T24_AddLog(FileNameForLog, "RQ", json, ControllerName);
                #region api
                string[] CheckApi = c.CheckApi(api_name, api_key);
                ERR = CheckApi[0];
                SMS = CheckApi[1];
                #endregion api

                LoanAppCopyListGetV2RQ jObj = null;
                string user = "", pwd = "", device_id = "", app_vName = "", mac_address = "", criteria="", criteriaValue = "", UserID = "";
                #region json
                if (ERR != "Error")
                {
                    try
                    {
                        jObj = JsonConvert.DeserializeObject<LoanAppCopyListGetV2RQ>(json);
                        user = jObj.user;
                        user = c.Encrypt(user, c.SeekKeyGet());
                        pwd = jObj.pwd;
                        pwd = c.Encrypt(pwd, c.SeekKeyGet());
                        device_id = jObj.device_id;
                        app_vName = jObj.app_vName;
                        mac_address = jObj.mac_address;
                        criteria = jObj.criteria;
                        criteriaValue = jObj.criteriaValue;
                    }
                    catch (Exception ex)
                    {
                        ERR = "Error";
                        SMS = "Invalid JSON";
                    }
                }
                #endregion json
                #region Get User Info.
                if (ERR != "Error")
                {
                    try
                    {
                        SqlConnection Con1 = new SqlConnection(c.ConStr());
                        Con1.Open();
                        SqlCommand Com1 = new SqlCommand();
                        Com1.Connection = Con1;
                        string sql = "exec T24_GetUserInfo_V2 @user=@user,@pwd=@pwd";
                        Com1.CommandText = sql;
                        Com1.Parameters.Clear();
                        Com1.Parameters.AddWithValue("@user", user);
                        Com1.Parameters.AddWithValue("@pwd", pwd);
                        DataTable dt1 = new DataTable();
                        dt1.Load(Com1.ExecuteReader());
                        UserID = dt1.Rows[0]["UserID"].ToString();
                        Con1.Close();
                    }
                    catch (Exception ex)
                    {
                        ERR = "Error";
                        SMS = "invalid user";
                    }
                }
                #endregion Get User Info.

                #region data
                if (ERR != "Error")
                {
                    LoanAppCopyListGetV2RS ListHeader = new LoanAppCopyListGetV2RS();
                    List<LoanAppCopyListGetV2RSList> DataList = new List<LoanAppCopyListGetV2RSList>();
                    DataTable dt = c.ReturnDT("exec sp_LoanAppCopyListGetByCID @CID='" + criteriaValue + "',@UserID='"+ UserID + "'");
                    if (dt.Rows.Count == 0)
                    {
                        ERR = "Error";
                        SMS = "No Data";
                    }
                    else {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            #region params
                            LoanAppCopyListGetV2RSList data = new LoanAppCopyListGetV2RSList();
                            data.LoanAppID = dt.Rows[i]["LoanAppID"].ToString();
                            data.CID = dt.Rows[i]["CID"].ToString();
                            data.CustName = dt.Rows[i]["CustName"].ToString();
                            data.IDCardNumber = dt.Rows[i]["IDCardNumber"].ToString();
                            data.AA = dt.Rows[i]["AA"].ToString();
                            data.ProductType = dt.Rows[i]["ProductType"].ToString();
                            data.Currency = dt.Rows[i]["Currency"].ToString();
                            data.ApproveAmt = dt.Rows[i]["ApproveAmt"].ToString();
                            DataList.Add(data);
                            #endregion params
                        }                       
                    }
                    ListHeader.ERR = ERR;
                    ListHeader.SMS = SMS;
                    ListHeader.DataList = DataList;
                    RSData.Add(ListHeader);
                }
                #endregion data

            }
            catch { }
            try
            {
                var jsonRS = new JavaScriptSerializer().Serialize(RSData);
                c.T24_AddLog(FileNameForLog, "RS", jsonRS.ToString(), ControllerName);
            }
            catch { }
            return RSData;
        }

    }
    public class LoanAppCopyListGetV2RQ
    {
        public string user { get; set; }
        public string pwd { get; set; }
        public string device_id { get; set; }
        public string app_vName { get; set; }
        public string mac_address { get; set; }
        public string criteria { get; set; }//CID
        public string criteriaValue { get; set; }//'2743959'
    }
    public class LoanAppCopyListGetV2RS
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
        public List<LoanAppCopyListGetV2RSList> DataList { get; set; }
    }
    public class LoanAppCopyListGetV2RSList
    {
        public string LoanAppID { get; set; }
        public string CID { get; set; }
        public string CustName { get; set; }
        public string IDCardNumber { get; set; }
        public string AA { get; set; }
        public string ProductType { get; set; }
        public string Currency { get; set; }
        public string ApproveAmt { get; set; }
    }
}