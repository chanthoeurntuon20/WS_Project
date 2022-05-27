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
    public class DefinedFieldGetV2Controller : ApiController
    {
        //[EnableCors(origins: "*", headers: "*", methods: "*")]
        // GET api/<controller>
        //public IEnumerable<DefinedFieldModel> Post([FromUri]string api_name, string api_key, [FromBody]string json)
        public IEnumerable<DefinedFieldModel> Post([FromUri]string p, [FromUri]string msgid, [FromBody]string json)//json={"criteriaValue":""}
        {
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "", ExSMS = "", ERRCode = "";
            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string ControllerName = "DefinedFieldGetV2";
            string FileNameForLog = msgid + "_" + ServerDate.Replace("-", "_").Replace(" ", "_").Replace(":", "_").Replace(".", "_");
            List<DefinedFieldModel> RSData = new List<DefinedFieldModel>();
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
                DefinedFieldGetV2RQModel jObj = null;
                string criteriaValue = "";
                if (ERR != "Error")
                {
                    try
                    {
                        jObj = JsonConvert.DeserializeObject<DefinedFieldGetV2RQModel>(json);
                        criteriaValue = jObj.criteriaValue;
                    }
                    catch (Exception ex)
                    {
                        ERR = "Error";
                        ExSMS = ex.Message.ToString();
                        //get sms
                        string[] str = c.GetSMSByMsgID("12");
                        ERR = str[0];
                        if (ERR == "Error")
                        {
                            SMS = str[1];
                            ExSMS = ExSMS + "|" + str[2];
                        }
                        else
                        {
                            SMS = str[3];
                        }
                        ERR = "Error";
                    }
                }
                #endregion

                #region data
                if (ERR != "Error")
                {
                    DefinedFieldModel ListHeader = new DefinedFieldModel();
                    ListHeader.ERR = ERR;
                    ListHeader.SMS = SMS;
                    ListHeader.ERRCode = ERRCode;
                    List<DefinedFieldList> DataList = new List<DefinedFieldList>();

                    DataTable dt = c.ReturnDT("exec T24_GetLookUpByDevice @UserID='0',@criteriaValue='"+ criteriaValue + "'");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        #region params
                        DefinedFieldList data = new DefinedFieldList();
                        data.LOOKUPID = dt.Rows[i]["LOOKUPID"].ToString();
                        data.criteriaValue = dt.Rows[i]["criteriaValue"].ToString();
                        data.DESCRIPTION = dt.Rows[i]["DESCRIPTION"].ToString();
                        data.OrderBy = dt.Rows[i]["OrderBy"].ToString();
                        data.Parent = dt.Rows[i]["ParentID"].ToString();
                        data.ParentCriteriaValue = dt.Rows[i]["ParentCriteriaValue"].ToString();
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
                ExSMS = ex.Message.ToString();
            }
            
            try {
                #region if Error
                if (ERR == "Error")
                {
                    DefinedFieldModel ListHeader = new DefinedFieldModel();
                    ListHeader.ERR = ERR;
                    ListHeader.SMS = SMS;
                    ListHeader.ERRCode = ERRCode;
                    ListHeader.DataList = null;
                    RSData.Add(ListHeader);
                }
                #endregion if Error
                //Add log
                var jsonRS = new JavaScriptSerializer().Serialize(RSData);
                c.T24_AddLog(FileNameForLog, "RS", jsonRS.ToString(), "DefinedFieldGetV2");
            } catch { }

            return RSData;
        }

    }

    public class DefinedFieldGetV2RQModel
    {
        public string criteriaValue { get; set; }
    }
    public class DefinedFieldRS {
        public string user { get; set; }
        public string pwd { get; set; }
        public string device_id { get; set; }
        public string app_vName { get; set; }
        public string mac_address { get; set; }
        public string criteriaValue { get; set; }
    }

}