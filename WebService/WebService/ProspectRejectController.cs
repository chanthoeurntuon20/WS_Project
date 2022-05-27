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
    public class ProspectRejectController : ApiController
    {
        //[EnableCors(origins: "*", headers: "*", methods: "*")]
        // GET api/<controller>
        //public IEnumerable<ProspectAddRS> Post([FromUri]string api_name, string api_key, string username, [FromBody]string json)
        public string Post([FromUri]string p, [FromUri]string msgid, [FromBody]string json)//ed -> {"ProspectCode":"1234"} -> NJxMGX6Dvt+4/ZVwNNokSaMCmDxweHB473MzesheQCA=
        {
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "", ExSMS = "", ERRCode = "";
            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string ControllerName = "ProspectReject";
            string FileNameForLog = msgid + "_" + ServerDate.Replace("-", "_").Replace(" ", "_").Replace(":", "_").Replace(".", "_");
            List<ProspectRejectRS> RSData = new List<ProspectRejectRS>();
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
                string pSwitch = p.Substring(0, 3);
                string UserID = "", api_name="";

                if (pSwitch != "sw_")
                {
                    if (ERR != "Error")
                    {
                        //p = System.Web.HttpUtility.UrlDecode(p);
                        string[] rs = c.SessionIDCheck(ServerDate, p);
                        ERR = rs[0];
                        SMS = rs[1];
                        ExSMS = rs[2];
                        UserID = rs[3];
                        ERRCode = rs[4];
                        api_name = rs[5];
                    }
                }
                else {

                    string strs = p.Substring(3, p.Length - 3).Replace(" ", "+");
                    ERR = "";
                    SMS = "";
                    ExSMS = "";
                    UserID = c.Decrypt(strs, c.SeekKeyGet());
                    ExSMS = "";
                    ERRCode = "";
                    api_name = "";
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
                ProspectRejectRQ jObj = null;
                if (ERR != "Error")
                {
                    try
                    {
                        jObj = JsonConvert.DeserializeObject<ProspectRejectRQ>(json);
                    }
                    catch (Exception ex)
                    {
                        ERR = "Error";
                        SMS = "Invalid JSON";
                        ExSMS = ex.ToString();
                    }
                }
                #endregion                
                #region data
                if (ERR != "Error")
                {
                    //get var from json obj
                    string prospectCode = jObj.ProspectCode;
                    //call to db to reject
                    string sql = "exec sp_ProspectReject @UserID='"+ UserID + "',@Code='"+ prospectCode + "'";
                    DataTable dt = c.ReturnDT(sql);
                    //result from db
                    ERR = dt.Rows[0]["ERR"].ToString(); //Succeed or Error
                    SMS = dt.Rows[0]["SMS"].ToString(); //Error->message
                }
                #endregion data
            }
            catch (Exception ex)
            {
                ERR = "Error";
                SMS = "Something was wrong";
                ExSMS = ex.ToString();
            }

            ProspectRejectRS ListHeader = new ProspectRejectRS();
            ListHeader.ERR = ERR;
            ListHeader.SMS = SMS;
            ListHeader.ERRCode = ERRCode;
            //ListHeader.DataList = null;
            RSData.Add(ListHeader);

            try
            {                
                if (ExSMS != "") {
                    c.T24_AddLog(FileNameForLog, "RS_Error", ExSMS, ControllerName);
                }
            }
            catch { }

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

    public class ProspectRejectRQ
    {
        public string ProspectCode { get; set; }//require
    }

    public class ProspectRejectRS
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
        public string ERRCode { get; set; }
    }

}