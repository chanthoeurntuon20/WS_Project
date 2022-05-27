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
    public class CustomerGetCountController : ApiController
    {
        // GET api/<controller>
        //public IEnumerable<CustomerGetCountResModel> Get(string api_name, string api_key, string json)////json=[{"user":"01804","pwd":"040882","device_id":"352405061542333","app_vName":"1.6"}]
        public string Post([FromUri]string p, [FromUri]string msgid)
        {
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "", ExSMS = "", ERRCode = "", CustCount="0";
            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string ControllerName = "CustomerGetCount";
            string FileNameForLog = msgid + "_" + ServerDate.Replace("-", "_").Replace(" ", "_").Replace(":", "_").Replace(".", "_");
            List<CustomerGetCountResModel> RSData = new List<CustomerGetCountResModel>();
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
                    DataTable dt = c.ReturnDT("exec T24_GetCustomerByAMOrCOByDeviceCount @UserID='" + UserID + "'");
                    CustCount = dt.Rows[0][0].ToString();
                }
                #endregion data
            }
            catch (Exception ex)
            {
                ERR = "Error";
                SMS = "Something was wrong";
            }


            
            CustomerGetCountResModel DataList = new CustomerGetCountResModel();
            DataList.ERR = ERR;
            DataList.SMS = SMS;
            DataList.ERRCode = ERRCode;
            DataList.CustCount = CustCount;
            RSData.Add(DataList);

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
    public class CustomerGetCountResModel
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
        public string ERRCode { get; set; }
        public string CustCount { get; set; }
    }
}