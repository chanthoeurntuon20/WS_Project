using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;

namespace WebService
{
    [BasicAuthentication]
    public class HolidayActionByWebController : ApiController
    {
        //[EnableCors(origins: "*", headers: "*", methods: "*")]
        // GET api/<controller>
        //public IEnumerable<ProspectGetV2RSModel> Post([FromUri]string api_name, string api_key,string username, [FromBody]string json)
        public string Post([FromUri] string api_name, string api_key, string username, [FromBody] string json)
        {//json={"Action":"1","HolidayID":"0","HolidayDate":"yyyy-MM-dd","Description":"abc","ActionByUserID":"1"}->ED | Action=1(Add),Action=2(Edit),Action=3(Delete),
            string msgid = username;
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "", ExSMS = "", ERRCode = "";
            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string ControllerName = "HolidayActionByWeb";
            string FileNameForLog = msgid + "_" + ServerDate.Replace("-", "_").Replace(" ", "_").Replace(":", "_").Replace(".", "_");
            List<HolidayActionByTabRSModel> RSData = new List<HolidayActionByTabRSModel>();
            HolidayActionByTabRSModel ListHeader = new HolidayActionByTabRSModel();
            List<HolidayActionByTabRSList> DataList = new List<HolidayActionByTabRSList>();
            try
            {
                #region msgid
                if (ERR != "Error")
                {
                    //string[] str = c.CheckMsgID(msgid);
                    //ERR = str[0];
                    //SMS = str[1];
                }
                #endregion
                #region add log
                if (ERR != "Error")
                {
                    c.T24_AddLog(FileNameForLog, "1.RQ", "api_name:" + api_name + " | api_key:" + api_key + " | json:" + json, ControllerName);
                }
                #endregion
                #region p -> SessionID
                string UserID = "";
                if (ERR != "Error")
                {
                    ////p = System.Web.HttpUtility.UrlDecode(p);
                    //string[] rs = c.SessionIDCheck(ServerDate, p);
                    //ERR = rs[0];
                    //SMS = rs[1];
                    //ExSMS = rs[2];
                    //UserID = rs[3];
                    //ERRCode = rs[4];
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
                HolidayActionByTabRQ jObj = null;
                if (ERR != "Error")
                {
                    try
                    {
                        jObj = JsonConvert.DeserializeObject<HolidayActionByTabRQ>(json);                        
                    }
                    catch
                    {
                        ERR = "Error";
                        SMS = "Invalid JSON";
                    }
                }
                #endregion
                #region data
                if (ERR != "Error")
                {
                    SqlConnection Con1 = new SqlConnection(c.ConStr());
                    Con1.Open();
                    SqlCommand Com1 = new SqlCommand();
                    Com1.Connection = Con1;

                    try {
                        string sql = "exec sp_HolidayActionFromWeb @Action=@Action,@HolidayID=@HolidayID,@HolidayDate=@HolidayDate,@Description=@Description,@UserID=@UserID";
                        Com1.CommandText = sql;
                        Com1.Parameters.Clear();
                        Com1.Parameters.AddWithValue("@Action", jObj.Action);
                        Com1.Parameters.AddWithValue("@HolidayID", jObj.HolidayID);
                        Com1.Parameters.AddWithValue("@HolidayDate", jObj.HolidayDate);
                        Com1.Parameters.AddWithValue("@Description", jObj.Description);
                        Com1.Parameters.AddWithValue("@UserID", jObj.ActionByUserID);
                        DataTable dt1 = new DataTable();
                        dt1.Load(Com1.ExecuteReader());
                        ERR = dt1.Rows[0]["ERR"].ToString();
                        SMS = dt1.Rows[0]["SMS"].ToString();
                        string HolidayIDRS = dt1.Rows[0]["HolidayIDRS"].ToString();

                        HolidayActionByTabRSList dlist = new HolidayActionByTabRSList();
                        dlist.HolidayID = HolidayIDRS;
                        DataList.Add(dlist);

                    } catch (Exception ex) {
                        ERR = "Error";
                        SMS = "Something was wrong";
                        ExSMS = ex.Message.ToString();
                    }

                    Con1.Close();
                }
                #endregion data
            }
            catch (Exception ex)
            {
                ERR = "Error";
                SMS = "Something was wrong";
                ExSMS = ex.Message.ToString();
            }
            
            string RSDataStr = "";
            try
            {
                ListHeader.ERR = ERR;
                ListHeader.SMS = SMS;
                ListHeader.ERRCode = ERRCode;
                ListHeader.DataList = DataList;
                RSData.Add(ListHeader);

                var jsonRS = new JavaScriptSerializer().Serialize(RSData);
                RSDataStr = c.Encrypt(jsonRS, c.SeekKeyGet());
                c.T24_AddLog(FileNameForLog, "RS", jsonRS.ToString(), ControllerName);
            }
            catch { }
            return RSDataStr;
        }

    }
    public class HolidayActionByTabRQ
    {
        public string Action { get; set; }
        public string HolidayID { get; set; }
        public string HolidayDate { get; set; }
        public string Description { get; set; }
        public string ActionByUserID { get; set; }
    }
    public class HolidayActionByTabRSModel
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
        public string ERRCode { get; set; }
        public List<HolidayActionByTabRSList> DataList { get; set; }
    }
    public class HolidayActionByTabRSList
    {
        public string HolidayID { get; set; }
    }

}