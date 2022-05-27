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
    public class SQLForTabGetV2Controller : ApiController
    {
        // GET api/<controller>
        public IEnumerable<SQLForTabGetV2RSModel> Post([FromUri]string api_name, string api_key,string username, [FromBody]string json)
        {
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "";
            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            List<SQLForTabGetV2RSModel> RSData = new List<SQLForTabGetV2RSModel>();
            string FileNameForLog = username+"_" + api_name + "_" + ServerDate.Replace("-", "_").Replace(" ", "_").Replace(":", "_");
            string ControllerName = "SQLForTabGetV2";
            string ExSMS = "";
            try
            {
                //Add log
                c.T24_AddLog(FileNameForLog, "RQ", json, ControllerName);

                #region api
                try
                {
                    string[] CheckApi = c.CheckApi(api_name, api_key);
                    ERR = CheckApi[0];
                    SMS = CheckApi[1];
                }
                catch (Exception ex)
                {
                    ERR = "Error";
                    SMS = "Invalid Api";
                    ExSMS = ex.Message.ToString();
                }
                #endregion api
                #region check json
                if (ERR != "Error")
                {
                    if (json == null || json == "")
                    {
                        ERR = "Error";
                        SMS = "Invalid JSON";
                    }
                }
                #endregion check json
                SQLForTabGetV2RQModel jObj = null;
                string user = "", pwd = "", device_id = "", app_vName = "", mac_address = "", UserName="";
                #region json
                if (ERR != "Error")
                {
                    try
                    {
                        jObj = JsonConvert.DeserializeObject<SQLForTabGetV2RQModel>(json);
                        user = jObj.user;
                        UserName = user;
                        user = c.Encrypt(user, c.SeekKeyGet());
                        pwd = jObj.pwd;
                        pwd = c.Encrypt(pwd, c.SeekKeyGet());
                        device_id = jObj.device_id;
                        app_vName = jObj.app_vName;
                        mac_address = jObj.mac_address;
                    }
                    catch(Exception ex)
                    {
                        ERR = "Error";
                        SMS = "Invalid JSON";
                        ExSMS = ex.Message.ToString();
                    }
                }
                #endregion json

                #region data
                if (ERR != "Error")
                {
                    SQLForTabGetV2RSModel ListHeader = new SQLForTabGetV2RSModel();
                    ListHeader.ERR = ERR;
                    ListHeader.SMS = SMS;

                    List<SQLForTabListV2RSModel> DataList = new List<SQLForTabListV2RSModel>();

                    string sql = "select * from tblSqlForTab where UserName='" + UserName + "'";
                    c.T24_AddLog(FileNameForLog, "RQ-GetData", sql, ControllerName);
                    DataTable dt = c.ReturnDT2(sql);
                    string SqlIDList = "";
                    if (dt.Rows.Count == 0)
                    {
                        ERR = "Error";
                        SMS = "No Data";
                    }
                    else {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            #region params
                            if (i == 0)
                            {
                                SqlIDList = dt.Rows[i]["SqlID"].ToString();
                            }
                            else
                            {
                                SqlIDList = SqlIDList + "," + dt.Rows[i]["SqlID"].ToString();
                            }
                            SQLForTabListV2RSModel data = new SQLForTabListV2RSModel();

                            data.SqlID = dt.Rows[i]["SqlID"].ToString();
                            data.Sql = dt.Rows[i]["Sql"].ToString();
                            data.Remark = dt.Rows[i]["Remark"].ToString();
                            DataList.Add(data);
                            #endregion params
                        }
                        if (SqlIDList != "")
                        {
                            sql = "exec sp_SqlForTabSynced @SqlIDList='" + SqlIDList + "'";
                            c.T24_AddLog(FileNameForLog, "RQ-UpdateData", sql, ControllerName);
                            c.ReturnDT(sql);
                        }

                        ListHeader.DataList = DataList;
                        RSData.Add(ListHeader);
                    }
                    
                }
                #endregion data
            }
            catch (Exception ex)
            {
                ERR = "Error";
                SMS = "Something was wrong";
                ExSMS = ex.Message.ToString();
            }
            #region if Error
            if (ERR == "Error")
            {
                SQLForTabGetV2RSModel ListHeader = new SQLForTabGetV2RSModel();
                ListHeader.ERR = ERR;
                ListHeader.SMS = SMS;
                ListHeader.DataList = null;
                RSData.Add(ListHeader);
            }
            #endregion if Error

            try
            {
                var jsonRS = new JavaScriptSerializer().Serialize(RSData);
                c.T24_AddLog(FileNameForLog, "RS", jsonRS.ToString(), ControllerName);
                if (ExSMS != "")
                {
                    c.T24_AddLog(FileNameForLog, "RS", ExSMS, ControllerName + "_Error");
                }
            }
            catch { }

            return RSData;
        }

    }

    public class SQLForTabGetV2RQModel
    {
        public string user { get; set; }
        public string pwd { get; set; }
        public string device_id { get; set; }
        public string app_vName { get; set; }
        public string mac_address { get; set; }
    }
    public class SQLForTabGetV2RSModel
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
        public List<SQLForTabListV2RSModel> DataList { get; set; }
    }
    public class SQLForTabListV2RSModel
    {
        public string SqlID { get; set; }
        public string Sql { get; set; }
        public string Remark { get; set; }
    }
}