using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
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
    public class HolidayGetByWebController : ApiController
    {
        //[EnableCors(origins: "*", headers: "*", methods: "*")]
        // GET api/<controller>
        //public IEnumerable<ProspectGetV2RSModel> Post([FromUri]string api_name, string api_key,string username, [FromBody]string json)
        //public string Post([FromUri]string p, [FromUri]string msgid, [FromBody]string json)//{"FYear":"2025","TYear":"2025"}->ED
        public string Post([FromUri]string api_name, string api_key, string username, [FromBody]string json)//{"FYear":"2020","TYear":"2021"}->ED
        {
            string msgid = username;
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "", ExSMS = "", ERRCode = "";
            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string ControllerName = "HolidayGetByWeb";
            string FileNameForLog = msgid + "_" + ServerDate.Replace("-", "_").Replace(" ", "_").Replace(":", "_").Replace(".", "_");
            List<HolidayGetByWebRSModel> RSData = new List<HolidayGetByWebRSModel>();
            HolidayGetByWebRSModel ListHeader = new HolidayGetByWebRSModel();
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
                    c.T24_AddLog(FileNameForLog, "1.RQ", "api_name:"+ api_name+ " | api_key:"+ api_key+ " | json:"+ json, ControllerName);
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
                HolidayGetByWebRQ jObj = null;
                string FYear="",TYear="";
                if (ERR != "Error")
                {
                    try
                    {
                        jObj = JsonConvert.DeserializeObject<HolidayGetByWebRQ>(json);
                        FYear = jObj.FYear;
                        TYear = jObj.TYear;
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
                   

                    List<HolidayGetByWebRSList> DataList = new List<HolidayGetByWebRSList>();
                    string sql = "exec sp_HolidayGetFromWeb @FYear='" + FYear+ "',@TYear='" + TYear + "'";
                    DataTable dt = c.ReturnDT(sql);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        #region params
                        HolidayGetByWebRSList data = new HolidayGetByWebRSList();
                        data.OrderNo = dt.Rows[i]["OrderNo"].ToString();
                        data.HolidayID = dt.Rows[i]["HolidayID"].ToString(); 
                        data.HolidayDate = dt.Rows[i]["HolidayDate"].ToString();
                        data.Description = dt.Rows[i]["Description"].ToString();
                        DataList.Add(data);
                        #endregion params
                    }
                    ListHeader.DataList = DataList;
                    
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
                RSData.Add(ListHeader);

                var jsonRS = new JavaScriptSerializer().Serialize(RSData);
                RSDataStr = c.Encrypt(jsonRS, c.SeekKeyGet());
                c.T24_AddLog(FileNameForLog, "RS", jsonRS.ToString(), ControllerName);
            }
            catch { }
            return RSDataStr;
        }

    }
    public class HolidayGetByWebRQ
    {
        public string FYear { get; set; }
        public string TYear { get; set; }
    }
    public class HolidayGetByWebRSModel
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
        public string ERRCode { get; set; }
        public List<HolidayGetByWebRSList> DataList { get; set; }
    }
    public class HolidayGetByWebRSList
    {
        public string OrderNo { get; set; }
        public string HolidayID { get; set; }
        public string HolidayDate { get; set; }        
        public string Description { get; set; }
    }

}