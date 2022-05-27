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
    [BasicAuthentication]
    public class AccountCreationToCBSV2Controller : ApiController
    {
        //public IEnumerable<AccountCreationToCBSModel> Post([FromUri]string api_name, string api_key, string json)
        public string Post([FromUri]string p, [FromUri]string msgid, [FromBody]string json)
        {////json=[{"user":"none","pwd":"none","device_id":"none","app_vName":"none","CreCompany":"none","CustomerID":"none","CATEGORY":"none","ACCOUNTTITLE1":"none","SHORTTITLE":"none","LoanCurrency":"none"}]
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "", ExSMS = "", ERRCode = "";
            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string ControllerName = "AccountCreationToCBS";
            string FileNameForLog = msgid + "_" + ServerDate.Replace("-", "_").Replace(" ", "_").Replace(":", "_").Replace(".", "_");
            List<AccountCreationToCBSModel> RSData = new List<AccountCreationToCBSModel>();
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
                string CreCompany = "", CustomerID = "", CATEGORY = "", ACCOUNTTITLE1 = "", SHORTTITLE = "", LoanCurrency = "";
                if (ERR != "Error")
                {
                    string[] CheckJson = c.CheckJsonForCreateAccount(json);
                    ERR = CheckJson[0];
                    if (ERR == "Error")
                    {
                        SMS = CheckJson[1];
                    }
                    else
                    {
                        CreCompany = CheckJson[4];
                        CustomerID = CheckJson[5];
                        CATEGORY = CheckJson[6];
                        ACCOUNTTITLE1 = CheckJson[7];
                        SHORTTITLE = CheckJson[8];
                        LoanCurrency = CheckJson[9];
                    }
                }
                #endregion 
                #region data
                if (ERR != "Error")
                {
                    AccountCreationToCBSModel ListHeader = new AccountCreationToCBSModel();
                    ListHeader.ERR = ERR;
                    ListHeader.SMS = SMS;
                    ListHeader.ERRCode = ERRCode;
                    List<AccountCreationToCBSList> DataList = new List<AccountCreationToCBSList>();

                    #region get T24 Url
                    string sql = "exec T24_GetT24_Url @UserID='" + UserID + "',@UrlID=21";
                    DataTable dt2 = new DataTable();
                    dt2 = c.ReturnDT(sql);
                    string CreUrl = dt2.Rows[0]["CreUrl"].ToString();
                    //string CreCompany = CreCompany;// dt2.Rows[0]["CreCompany"].ToString();
                    string CreUserName = dt2.Rows[0]["CreUserName"].ToString();
                    string CrePassword = dt2.Rows[0]["CrePassword"].ToString();
                    #endregion get T24 Url
                    #region xml
                    #region old
                    //string xmlStr = "<?xml version=\"1.0\"?><soapenv:Envelope xmlns:acc=\"http://temenos.com/ACCOUNTAMKSAOPENWS\" "
                    //+ "xmlns:amk=\"http://temenos.com/AMKTABACOPEN\" xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\"><soapenv:Header/>"
                    //+ "<soapenv:Body><amk:ACCOUNTCREATION><WebRequestCommon><company>" + CreCompany
                    //+ "</company><password>" + CrePassword + "</password><userName>" + CreUserName + "</userName></WebRequestCommon>"
                    //+ "<OfsFunction> </OfsFunction><ACCOUNTAMKSAOPENWSType id=\"\">"
                    //+ "<acc:CUSTOMER>" + CustomerID + "</acc:CUSTOMER><acc:CATEGORY>" + CATEGORY + "</acc:CATEGORY>"
                    ////+ "<acc:ACCOUNTTITLE1>" + CustLastName + ", " + CustFirstName + "</acc:ACCOUNTTITLE1><acc:SHORTTITLE>" + CustLastName + ", " + CustFirstName
                    //+ "<acc:ACCOUNTTITLE1>" + ACCOUNTTITLE1 + "</acc:ACCOUNTTITLE1><acc:SHORTTITLE>" + SHORTTITLE
                    //+ "</acc:SHORTTITLE><acc:MNEMONIC></acc:MNEMONIC><acc:CURRENCY>" + LoanCurrency
                    //+ "</acc:CURRENCY><acc:ACCOUNTOFFICER>1</acc:ACCOUNTOFFICER><acc:SMSAlerts>NO</acc:SMSAlerts>"
                    //+ "<acc:EmailAlerts>NO</acc:EmailAlerts><acc:ConditionofOperation>1</acc:ConditionofOperation>"
                    //+ "</ACCOUNTAMKSAOPENWSType></amk:ACCOUNTCREATION></soapenv:Body></soapenv:Envelope>";
                    #endregion old
                    #region new
                    string xmlStr = "<S:Envelope xmlns:S=\"http://schemas.xmlsoap.org/soap/envelope/\">"
                    + "<S:Body>"
                    + "<ns5:WSACCTCRTTBLTINP xmlns:ns2=\"http://temenos.com/ACCOUNT\" xmlns:ns3=\"http://temenos.com/ACCOUNTAMKSAOPENWS\" xmlns:ns4=\"http://temenos.com/ACCOUNTTUSACRTWSTBLT\" xmlns:ns5=\"http://temenos.com/AMKACCT\">"
                    + "<WebRequestCommon>"
                    + "<company>" + CreCompany + "</company>"
                    + "<password>" + CrePassword + "</password>"
                    + "<userName>" + CreUserName + "</userName>"
                    + "</WebRequestCommon><OfsFunction/>"
                    + "<ACCOUNTTUSACRTWSTBLTType>"
                    + "<ns4:CUSTOMER>" + CustomerID + "</ns4:CUSTOMER>"
                    + "<ns4:CATEGORY>" + CATEGORY + "</ns4:CATEGORY>"
                    + "<ns4:ACCOUNTTITLE1>" + ACCOUNTTITLE1 + "</ns4:ACCOUNTTITLE1>"
                    + "<ns4:SHORTTITLE>" + SHORTTITLE + "</ns4:SHORTTITLE>"
                    + "<ns4:CURRENCY>" + LoanCurrency + "</ns4:CURRENCY>"
                    + "<ns4:ACCOUNTOFFICER>1</ns4:ACCOUNTOFFICER>"
                    + "<ns4:ConditionofOperation>1</ns4:ConditionofOperation>"
                    + "</ACCOUNTTUSACRTWSTBLTType>"
                    + "</ns5:WSACCTCRTTBLTINP>"
                    + "</S:Body>"
                    + "</S:Envelope>";
                    #endregion new
                    #endregion xml
                    //add log
                    FileNameForLog = CustomerID + "_" + FileNameForLog;
                    //c.T24_AddLog(fileHeader, "AccountAdd", xmlStr, "AccountAdd");
                    c.T24_AddLog(FileNameForLog, "2.XmlRQ", xmlStr, ControllerName);

                    var client = new RestClient(CreUrl);
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("cache-control", "no-cache");
                    request.AddHeader("content-type", "text/xml");
                    request.AddParameter("text/xml", xmlStr, ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    string xmlContent = response.Content.ToString();
                    //add log
                    //c.T24_AddLog(fileHeader, "AccountAdd", xmlContent, "AccountAdd");
                    c.T24_AddLog(FileNameForLog, "3.XmlRS", xmlContent, ControllerName);

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xmlContent);
                    string successIndicator = doc.GetElementsByTagName("successIndicator").Item(0).InnerText;
                    if (successIndicator == "Success")
                    {
                        string AccountID = doc.GetElementsByTagName("transactionId").Item(0).InnerText;
                        AccountCreationToCBSList data = new AccountCreationToCBSList();
                        data.AccountID = AccountID;
                        DataList.Add(data);

                        ListHeader.DataList = DataList;

                        RSData.Add(ListHeader);
                    }
                    else
                    {
                        //add account to T24 error
                        ERR = "Error";
                        SMS = "Error while create account to T24";
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
                AccountCreationToCBSModel CustHeader = new AccountCreationToCBSModel();
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

    public class AccountCreationToCBSModel
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
        public string ERRCode { get; set; }
        public List<AccountCreationToCBSList> DataList { get; set; }
    }
    public class AccountCreationToCBSList
    {
        public string AccountID { get; set; }
        public string ProductCode { get; set; }
        public string CATEGDESC { get; set; }
        public string AVAILBAL { get; set; }
    }

}