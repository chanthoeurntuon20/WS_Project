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
    public class AMUnderCUQMGetController : ApiController
    {
        // GET api/<controller>
        //public IEnumerable<AMUnderCUQMModel> Get(string api_name, string api_key, string json)////json=[{"user":"none","pwd":"none","device_id":"none","app_vName":"none"}]
        public string Post([FromUri]string p, [FromUri]string msgid)
        {
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "", ExSMS = "", ERRCode = "";
            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string ControllerName = "AMUnderCUQMGet";
            string FileNameForLog = msgid + "_" + ServerDate.Replace("-", "_").Replace(" ", "_").Replace(":", "_").Replace(".", "_");
            List<AMUnderCUQMModel> RSData = new List<AMUnderCUQMModel>();
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
                    c.T24_AddLog(FileNameForLog, "1.RQ", p, ControllerName);
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

                #region data
                if (ERR != "Error")
                {
                    
                    List<AMUnderCUQMList> DataList = new List<AMUnderCUQMList>();
                    DataTable dt = c.ReturnDT("exec T24_AMUnderCUQMGetFromDevice @CUQMUserID='" + UserID + "'");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        #region params
                        AMUnderCUQMList data = new AMUnderCUQMList();
                        data.UserTreeID = dt.Rows[i]["UserTreeID"].ToString();
                        data.AMUserID = dt.Rows[i]["AMUserID"].ToString();
                        data.AMUserName = dt.Rows[i]["AMUserName"].ToString();
                        data.AMName = dt.Rows[i]["AMName"].ToString();
                        DataList.Add(data);
                        #endregion params
                    }
                    AMUnderCUQMModel ListHeader = new AMUnderCUQMModel();
                    ListHeader.ERR = ERR;
                    ListHeader.SMS = SMS;
                    ListHeader.ERRCode = ERRCode;
                    ListHeader.DataList = DataList;
                    RSData.Add(ListHeader);
                }
                #endregion data
            }
            catch (Exception ex)
            {
                ERR = "Error";
                SMS = "Something was wrong";
            }
            #region if Error
            if (ERR == "Error")
            {
                AMUnderCUQMModel CustHeader = new AMUnderCUQMModel();
                CustHeader.ERR = ERR;
                CustHeader.SMS = SMS;
                CustHeader.ERRCode = ERRCode;
                CustHeader.DataList = null;
                RSData.Add(CustHeader);
            }
            #endregion if Error
            string RSDataStr = "";
            try
            {
                var jsonRS = new JavaScriptSerializer().Serialize(RSData);
                RSDataStr = c.Encrypt(jsonRS, c.SeekKeyGet());
                c.T24_AddLog(FileNameForLog, "RS", jsonRS.ToString(), ControllerName);
            }
            catch { }
            return RSDataStr;
        }

    }

    public class AMUnderCUQMModel
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
        public string ERRCode { get; set; }
        public List<AMUnderCUQMList> DataList { get; set; }
    }
    public class AMUnderCUQMList
    {
        public string UserTreeID { get; set; }
        public string AMUserID { get; set; }
        public string AMUserName { get; set; }
        public string AMName { get; set; }
    }


}