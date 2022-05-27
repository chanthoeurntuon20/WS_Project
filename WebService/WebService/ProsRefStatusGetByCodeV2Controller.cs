using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
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
    public class ProsRefStatusGetByCodeV2Controller : ApiController
    {
        // POST api/<controller>
        public IEnumerable<ProsRefStatusGetByCodeV2RS> Post([FromUri]string api_name, string api_key,string username, [FromBody]string json)
        {
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "";
            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            List<ProsRefStatusGetByCodeV2RS> RSData = new List<ProsRefStatusGetByCodeV2RS>();
            string ControllerName = "ProsRefStatusGetByCodeV2";
            string FileNameForLog = username + "_" + api_name + "_" + ServerDate.Replace("-", "_").Replace(" ", "_").Replace(":", "_");
            try
            {
                c.T24_AddLog(FileNameForLog, "RQ", json, ControllerName);
                #region api
                string[] CheckApi = c.CheckApi(api_name, api_key);
                ERR = CheckApi[0];
                SMS = CheckApi[1];
                #endregion api

                ProsRefStatusGetByCodeV2RQ jObj = null;
                string user = "", pwd = "", device_id = "", app_vName = "", mac_address = "", criteriaValue = "", UserID = "";
                #region json
                if (ERR != "Error")
                {
                    try
                    {
                        jObj = JsonConvert.DeserializeObject<ProsRefStatusGetByCodeV2RQ>(json);
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
                    DataTable dt = c.ReturnDT("exec V2_ProspectStatusGetForRefferalByCode @Code='"+criteriaValue+"'");
                    string StatusID="",StatusDate = "", IsMapAA="";
                    try {
                        StatusID = dt.Rows[0]["StatusID"].ToString();
                        StatusDate = dt.Rows[0]["StatusDate"].ToString();
                        IsMapAA = dt.Rows[0]["IsMapAA"].ToString();
                    } catch { }

                    ProsRefStatusGetByCodeV2RS ListHeader = new ProsRefStatusGetByCodeV2RS();
                    ListHeader.ERR = ERR;
                    ListHeader.SMS = SMS;
                    ListHeader.StatusID = StatusID;
                    ListHeader.StatusDate = StatusDate;
                    ListHeader.IsMapAA = IsMapAA;
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
    public class ProsRefStatusGetByCodeV2RQ
    {
        public string user { get; set; }
        public string pwd { get; set; }
        public string device_id { get; set; }
        public string app_vName { get; set; }
        public string mac_address { get; set; }
        public string criteriaValue { get; set; }
    }
    public class ProsRefStatusGetByCodeV2RS
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
        public string StatusID { get; set; }
        public string StatusDate { get; set; }
        public string IsMapAA { get; set; }
    }
}