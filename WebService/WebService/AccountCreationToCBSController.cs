using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml;

namespace WebService
{
    [BasicAuthentication]
    public class AccountCreationToCBSController : ApiController
    {
        public IEnumerable<AccountCreationToCBSModel> Post([FromUri]string api_name, string api_key, string json)
        {////json=[{"user":"none","pwd":"none","device_id":"none","app_vName":"none","CreCompany":"none","CustomerID":"none","CATEGORY":"none","ACCOUNTTITLE1":"none","SHORTTITLE":"none","LoanCurrency":"none"}]
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "";
            List<AccountCreationToCBSModel> RSData = new List<AccountCreationToCBSModel>();
            try
            {
                #region api
                string[] CheckApi = c.CheckApi(api_name, api_key);
                ERR = CheckApi[0];
                SMS = CheckApi[1];
                #endregion api

                #region json
                string UserID = "";
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
                        UserID = CheckJson[1];
                    }
                }
                #endregion json

                #region jsonForCreateAccount
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
                #endregion jsonForCreateAccount

                #region data
                if (ERR != "Error")
                {
                    AccountCreationToCBSModel ListHeader = new AccountCreationToCBSModel();
                    ListHeader.ERR = ERR;
                    ListHeader.SMS = SMS;
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
                    DateTime dt_LogDateTime = DateTime.Now;
                    string LogDateTime = dt_LogDateTime.ToString("yyyy-MM-dd HH.mm.ss");
                    string fileHeader = CustomerID + "_" + LogDateTime;
                    c.T24_AddLog(fileHeader, "AccountAdd", xmlStr, "AccountAdd");

                    var client = new RestClient(CreUrl);
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("cache-control", "no-cache");
                    request.AddHeader("content-type", "text/xml");
                    request.AddParameter("text/xml", xmlStr, ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    string xmlContent = response.Content.ToString();
                    //add log
                    c.T24_AddLog(fileHeader, "AccountAdd", xmlContent, "AccountAdd");

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
            }
            #region if Error
            if (ERR == "Error")
            {
                AccountCreationToCBSModel CustHeader = new AccountCreationToCBSModel();
                CustHeader.ERR = ERR;
                CustHeader.SMS = SMS;
                CustHeader.DataList = null;
                RSData.Add(CustHeader);
            }
            #endregion if Error

            return RSData;
        }


    }

    //public class AccountCreationToCBSModel
    //{
    //    public string ERR { get; set; }
    //    public string SMS { get; set; }
    //    public List<AccountCreationToCBSList> DataList { get; set; }
    //}
    //public class AccountCreationToCBSList
    //{
    //    public string AccountID { get; set; }
    //    public string ProductCode { get; set; }
    //    public string CATEGDESC { get; set; }
    //    public string AVAILBAL { get; set; }
    //}

}