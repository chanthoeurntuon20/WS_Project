using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace WebService
{
    [BasicAuthentication]
    public class ProsRefComGetV2Controller : ApiController
    {
        // POST api/<controller>
        public IEnumerable<ProsRefComGetV2RS> Post([FromUri]string api_name, string api_key,string username, [FromBody]string json)
        {
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "";
            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            List<ProsRefComGetV2RS> RSData = new List<ProsRefComGetV2RS>();
            string ControllerName = "ProsRefComGetV2";
            string FileNameForLog = username + "_" + api_name + "_" + ServerDate.Replace("-", "_").Replace(" ", "_").Replace(":", "_");
            try
            {
                c.T24_AddLog(FileNameForLog, "RQ", json, ControllerName);
                #region api
                string[] CheckApi = c.CheckApi(api_name, api_key);
                ERR = CheckApi[0];
                SMS = CheckApi[1];
                #endregion api

                ProsRefComGetV2 jObj = null;
                string user = "", pwd = "", device_id = "", app_vName = "", mac_address = "", criteriaValue = "", UserID = "";
                #region json
                if (ERR != "Error")
                {
                    try
                    {
                        jObj = JsonConvert.DeserializeObject<ProsRefComGetV2>(json);
                        user = jObj.user;
                        user = c.Encrypt(user, c.SeekKeyGet());
                        pwd = jObj.pwd;
                        pwd = c.Encrypt(pwd, c.SeekKeyGet());
                        device_id = jObj.device_id;
                        app_vName = jObj.app_vName;
                        mac_address = jObj.mac_address;
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
                    DataTable dt = c.ReturnDT("exec V2_ProsRefComGet @UserID='" + UserID + "',@ProspectCode='"+criteriaValue+"'");
                    string LoanAppID = "0";
                    try {
                        LoanAppID = dt.Rows[0]["LoanAppID"].ToString();
                    } catch { }
                    string Commission = "0", NameKh="", NameEn="", Phone="", LoanAmount="", DisbDate="";
                    if (LoanAppID == "0")
                    {
                        Commission = "0";
                        SMS = "not found data";
                    }
                    else {
                        try {
                            string Currency = dt.Rows[0]["Currency"].ToString();
                            string DisbAmt = dt.Rows[0]["DisbAmt"].ToString();
                            string ExRate = dt.Rows[0]["ExRate"].ToString();
                            NameKh = dt.Rows[0]["NameKh"].ToString();
                            NameEn = dt.Rows[0]["NameEn"].ToString();
                            Phone = dt.Rows[0]["Phone"].ToString();
                            LoanAmount = dt.Rows[0]["LoanAmount"].ToString();
                            DisbDate = dt.Rows[0]["DisbDate"].ToString();

                            string jsonWS = "{\"currency\":\"" + Currency + "\",\"exchange\":\"" + ExRate + "\",\"disbursement\":\"" + DisbAmt + "\"}";
                            string url = c.GetCalculatorWSUrl() + "/api/RefferalCommission?api_name=" + c.GetCalculatorWSAPIName() + "&api_key=" + c.GetCalculatorWSAPIKey() + "&type=json";
                            var client = new RestClient(url);
                            var Authenticator = new HttpBasicAuthenticator(c.GetCalculatorWSUser(), c.GetCalculatorWSPwd());
                            client.Authenticator = Authenticator;
                            var request = new RestRequest(Method.POST);
                            request.AddParameter("application/json; charset=utf-8", JsonConvert.SerializeObject(jsonWS), ParameterType.RequestBody);
                            IRestResponse response = client.Execute(request);
                            string jres = response.Content.ToString().TrimEnd(']').TrimStart('[');
                            JObject jsonObj = JObject.Parse(jres);
                            ERR = jsonObj.GetValue("ERR").ToObject<string>();
                            SMS = jsonObj.GetValue("SMS").ToObject<string>();
                            Commission = jsonObj.GetValue("Commission").ToObject<string>();

                        }
                        catch { }
                    }

                    ProsRefComGetV2RS ListHeader = new ProsRefComGetV2RS();
                    ListHeader.ERR = ERR;
                    ListHeader.SMS = SMS;
                    ListHeader.Commssion = Commission;
                    ListHeader.NameKh = NameKh;
                    ListHeader.NameEn = NameEn;
                    ListHeader.Phone = Phone;
                    ListHeader.LoanAmount = LoanAmount;
                    ListHeader.DisbDate = DisbDate;
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
    public class ProsRefComGetV2
    {
        public string user { get; set; }
        public string pwd { get; set; }
        public string device_id { get; set; }
        public string app_vName { get; set; }
        public string mac_address { get; set; }
        public string criteriaValue { get; set; }
    }
    public class ProsRefComGetV2RS
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
        public string Commssion { get; set; }
        public string NameKh { get; set; }
        public string NameEn { get; set; }
        public string Phone { get; set; }
        public string LoanAmount { get; set; }
        public string DisbDate { get; set; }
    }
}