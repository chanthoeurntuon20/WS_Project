using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using System.Xml;

namespace WebService
{
    public class LoanAppRejectToCBSController : ApiController
    {
        //public IEnumerable<LoanAppRejectToCBSModel> Get(string api_name, string api_key, string json)
        public string Post([FromUri]string p, [FromUri]string msgid, [FromBody]string json)
        {////json=[{"user":"none","pwd":"none","device_id":"none","app_vName":"none","criteriaValue":"CBSKey","criteriaValue2":"OfficeID"}]
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "", ExSMS = "", ERRCode = "";
            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string ControllerName = "LoanAppRejectToCBS";
            string FileNameForLog = msgid + "_" + ServerDate.Replace("-", "_").Replace(" ", "_").Replace(":", "_").Replace(".", "_");
            List<LoanAppRejectToCBSModel> RSData = new List<LoanAppRejectToCBSModel>();
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
                string UserID = "";

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
                    }
                }
                else {
                    string strs = p.Substring(3, p.Length - 3);
                    ERR = "";
                    SMS = "";
                    ExSMS = "";
                    UserID = c.Decrypt(strs, c.SeekKeyGet());                    
                    ERRCode = "";
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
                string criteriaValue = "", criteriaValue2 = "";
                if (ERR != "Error")
                {
                    string[] CheckJson = c.CheckJson(json);
                    ERR = CheckJson[0];
                    if (ERR == "Error")
                    {
                        SMS = CheckJson[1];
                    }
                    else
                    {
                        //UserID = CheckJson[1];
                        criteriaValue = CheckJson[2];
                        criteriaValue2 = CheckJson[4];
                    }
                }
                #endregion
                #region data
                string Reference = "";
                if (ERR != "Error")
                {
                    LoanAppRejectToCBSModel ListHeader = new LoanAppRejectToCBSModel();
                    ListHeader.ERR = ERR;
                    ListHeader.SMS = SMS;
                    ListHeader.ERRCode = ERRCode;

                    List<LoanAppRejectToCBSModelList> DataList = new List<LoanAppRejectToCBSModelList>();

                    #region get T24 Url
                    DataTable dtT24Url = new DataTable();
                    dtT24Url = c.ReturnDT("exec T24_GetT24_Url @UserID=0,@UrlID=8");
                    string CreUrl = dtT24Url.Rows[0]["CreUrl"].ToString();
                    string CreCompany = criteriaValue2;// dtT24Url.Rows[0]["CreCompany"].ToString();
                    string CreUserName = dtT24Url.Rows[0]["CreUserName"].ToString();
                    string CrePassword = dtT24Url.Rows[0]["CrePassword"].ToString();
                    #endregion get T24 Url
                    #region xml
                    string xmlStr = "<?xml version=\"1.0\"?><soapenv:Envelope xmlns:amk1=\"http://temenos.com/AMKDLAPPLICATIONBOLNREJECT\" "
                    + "xmlns:amk=\"http://temenos.com/AMKREJLOAN\" xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\"><soapenv:Header/>"
                    + "<soapenv:Body><amk:REJECTLOAN><WebRequestCommon>"
                    + "<company>" + CreCompany + "</company><password>" + CrePassword + "</password><userName>" + CreUserName + "</userName>"
                    + "</WebRequestCommon><OfsFunction> </OfsFunction>"
                    + "<AMKDLAPPLICATIONBOLNREJECTType id=\"" + criteriaValue + "\"> </AMKDLAPPLICATIONBOLNREJECTType></amk:REJECTLOAN></soapenv:Body></soapenv:Envelope>";
                    #endregion xml
                    #region add log
                    FileNameForLog = UserID + "_" + criteriaValue + "_" + FileNameForLog;
                    c.T24_AddLog(FileNameForLog, "2.XmlRQ", xmlStr, ControllerName);
                    #endregion add log
                    #region call
                    var client = new RestClient(CreUrl);
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("cache-control", "no-cache");
                    request.AddHeader("content-type", "text/xml");
                    request.AddParameter("text/xml", xmlStr, ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    string xmlContent = response.Content.ToString();
                    #endregion call
                    #region add log
                    c.T24_AddLog(FileNameForLog, "3.XmlRS", xmlContent, ControllerName);
                    #endregion add log
                    #region read xml                    
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xmlContent);
                    string successIndicator = doc.GetElementsByTagName("successIndicator").Item(0).InnerText;
                    if (successIndicator == "Success")
                    {
                        Reference = doc.GetElementsByTagName("transactionId").Item(0).InnerText;
                        LoanAppRejectToCBSModelList data = new LoanAppRejectToCBSModelList();
                        data.Reference = Reference;
                        DataList.Add(data);
                        ListHeader.DataList = DataList;
                        RSData.Add(ListHeader);
                    }
                    else {
                        ERR = "Error";
                        SMS = "Error: Reject to CBS | " + xmlContent;
                    }
                    #endregion read xml

                }
                #endregion data
            }
            catch (Exception ex)
            {
                ERR = "Error";
                SMS = "Error: Reject to CBS | Can't read response";
                ExSMS = ex.Message.ToString();
            }
            #region if Error
            if (ERR == "Error")
            {
                LoanAppRejectToCBSModel CustHeader = new LoanAppRejectToCBSModel();
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
                c.T24_AddLog(FileNameForLog, "4.RS", jsonRS.ToString(), ControllerName);
            }
            catch { }
            return RSDataStr;
        }

    }
    public class LoanAppRejectToCBSModel
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
        public string ERRCode { get; set; }
        public List<LoanAppRejectToCBSModelList> DataList { get; set; }
    }
    public class LoanAppRejectToCBSModelList
    {
        public string Reference { get; set; }
    }

}