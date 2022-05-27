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
    public class VillageBankGetV2Controller : ApiController
    {
        // GET api/<controller>
        //public IEnumerable<VillageBankModel> POST([FromUri]string api_name, string api_key, [FromBody]string json)
        public string Post([FromUri]string p, [FromUri]string msgid)
        {
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "", ExSMS = "", ERRCode = "";
            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string ControllerName = "VillageBankGetV2";
            string FileNameForLog = msgid + "_" + ServerDate.Replace("-", "_").Replace(" ", "_").Replace(":", "_").Replace(".", "_");
            List<VillageBankModel> RSData = new List<VillageBankModel>();
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
                #region check json
                //if (ERR != "Error")
                //{
                //    string[] str = c.CheckObjED(json, "2");
                //    ERR = str[0];
                //    SMS = str[1];
                //    ExSMS = str[2];
                //    json = str[3];
                //}
                #endregion

                #region data
                if (ERR != "Error")
                {
                    VillageBankModel ListHeader = new VillageBankModel();
                    ListHeader.ERR = ERR;
                    ListHeader.SMS = SMS;
                    ListHeader.ERRCode = ERRCode;
                    List<VBList> DataList = new List<VBList>();

                    DataTable dt = c.ReturnDT("exec T24_GetVBLinkByAMOrCOByDevice @UserID='" + UserID + "'");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        #region params
                        VBList data = new VBList();
                        data.VBID = dt.Rows[i]["VBID"].ToString();
                        data.VBName = dt.Rows[i]["VBName"].ToString();
                        data.Status = dt.Rows[i]["Status"].ToString();
                        data.COID = dt.Rows[i]["COID"].ToString();
                        data.MeetingDate = dt.Rows[i]["MeetingDate"].ToString();
                        data.ExpireDate = dt.Rows[i]["ExpireDate"].ToString();
                        data.COIDRotate = dt.Rows[i]["COIDRotate"].ToString();
                        data.GSTREET = dt.Rows[i]["GSTREET"].ToString();
                        data.GADDRESS = dt.Rows[i]["GADDRESS"].ToString();
                        data.GTOWN = dt.Rows[i]["GTOWN"].ToString();
                        data.GPOSTCODE = dt.Rows[i]["GPOSTCODE"].ToString();
                        data.AMKPROVINCE = dt.Rows[i]["AMKPROVINCE"].ToString();
                        data.AMKDISTRICT = dt.Rows[i]["AMKDISTRICT"].ToString();
                        data.AMKCOMMUNE = dt.Rows[i]["AMKCOMMUNE"].ToString();
                        data.AMKVILLAGE = dt.Rows[i]["AMKVILLAGE"].ToString();
                        DataList.Add(data);
                        #endregion params
                    }

                    ListHeader.DataList = DataList;
                    RSData.Add(ListHeader);
                }
                #endregion data
            }
            catch (Exception ex)
            {
                ERR = "Error";
                SMS = "Something was wrong";
                ERRCode = ex.Message.ToString();
            }
            #region if Error
            if (ERR == "Error")
            {
                VillageBankModel ListHeader = new VillageBankModel();
                ListHeader.ERR = ERR;
                ListHeader.SMS = SMS;
                ListHeader.ERRCode = ERRCode;
                ListHeader.DataList = null;
                RSData.Add(ListHeader);
            }
            #endregion if Error

            string RSDataStr = "";
            try
            {
                var jsonRS = new JavaScriptSerializer().Serialize(RSData);
                RSDataStr = c.Encrypt(jsonRS, c.SeekKeyGet());
                c.T24_AddLog(FileNameForLog, "4.RS", jsonRS.ToString(), ControllerName);
            }
            catch { }
            return RSDataStr;
        }



    }
    public class VillageBankGetV2RQ
    {
        public string user { get; set; }
        public string pwd { get; set; }
        public string device_id { get; set; }
        public string app_vName { get; set; }
        public string mac_address { get; set; }
    }

}