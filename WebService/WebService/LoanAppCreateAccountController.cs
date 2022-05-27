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
    public class LoanAppCreateAccountController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<LoanAppCreateAccountResModel> Post([FromUri]string api_name, string api_key, string json)
        {////json=[{"user":"01804","pwd":"040882","device_id":"352405061542333","app_vName":"1.6","criteriaValue":""}]
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "";
            List<LoanAppCreateAccountResModel> RSData = new List<LoanAppCreateAccountResModel>();
            LoanAppCreateAccountResModel DataList = new LoanAppCreateAccountResModel();
            List<LoanAppCreateAccountResAccListModel> AccList = new List<LoanAppCreateAccountResAccListModel>();
            try {
                
                #region api
                string[] CheckApi = c.CheckApi(api_name, api_key);
                ERR = CheckApi[0];
                SMS = CheckApi[1];
                #endregion api
                #region json
                string UserID = "", criteriaValue = "";
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
                        criteriaValue = CheckJson[2];
                    }
                }
                #endregion json
                #region get customer
                string CustomerID = "", CustLastName = "", CustFirstName = "", LoanCurrency = "",COID="", InstID="";
                string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH.mm.ss.fff");
                if (ERR != "Error")
                {
                    try {
                        string sql = "SELECT p.CustomerID,p.LastName,p.FirstName,l.Currency,u.OfficeHierachyID as COID,l.InstID FROM dbo.tblLoanApp1 l left join tblLoanAppPerson2 p on p.LoanAppID=l.LoanAppID and p.LoanAppPersonTypeID=31 left join tblUser u on u.UserID=l.CreateBy WHERE l.LoanAppID='" + criteriaValue + "'";
                        DataTable dt = new DataTable();
                        dt = c.ReturnDT(sql);
                        CustomerID = dt.Rows[0]["CustomerID"].ToString();
                        CustLastName = dt.Rows[0]["LastName"].ToString();
                        CustFirstName = dt.Rows[0]["FirstName"].ToString();
                        LoanCurrency = dt.Rows[0]["Currency"].ToString();
                        COID = dt.Rows[0]["COID"].ToString();
                        InstID = dt.Rows[0]["InstID"].ToString();                        
                    } catch {
                        ERR = "Error";
                        SMS = "Something was wrong during get customer";
                    }
                }
                #endregion get customer
                #region create account
                
                string ACCTID = "", ProductCode = "", CATEGDESC = "";
                string fileHeader = "LoanAppCreateAccount_InstID_" + InstID + "_" + UserID + "_" + criteriaValue + "_" + ServerDate;
                if (ERR != "Error") {
                    #region get T24 Url
                    string sql = "exec T24_GetT24_Url @UserID='" + UserID + "',@UrlID=21,@InstID='"+ InstID + "'";
                    DataTable dt = new DataTable();
                    dt = c.ReturnDT(sql);
                    string CreUrl = dt.Rows[0]["CreUrl"].ToString();
                    string CreCompany = dt.Rows[0]["CreCompany"].ToString();
                    string CreUserName = dt.Rows[0]["CreUserName"].ToString();
                    string CrePassword = dt.Rows[0]["CrePassword"].ToString();
                    #endregion get T24 Url
                    #region xml
                    string xmlStr = "<?xml version=\"1.0\"?><soapenv:Envelope xmlns:acc=\"http://temenos.com/ACCOUNTAMKSAOPENWS\" "
                    + "xmlns:amk=\"http://temenos.com/AMKTABACOPEN\" xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\"><soapenv:Header/>"
                    + "<soapenv:Body><amk:ACCOUNTCREATION><!--Optional:--><WebRequestCommon><!--Optional:--><company>" + CreCompany + "</company>"
                    +"<password>" + CrePassword + "</password>"
                    +"<userName>" + CreUserName + "</userName>"
                    +"</WebRequestCommon>"
                    + "<OfsFunction>"
                    + "<messageId>" + "A-" + CustomerID +"-"+ ServerDate + "</messageId>"
                    + "</OfsFunction>"
                    +"<ACCOUNTAMKSAOPENWSType id=\"\">"
                    + "<acc:CUSTOMER>" + CustomerID + "</acc:CUSTOMER>"
                    +"<acc:CATEGORY>6001</acc:CATEGORY>"
                    + "<acc:ACCOUNTTITLE1>" + CustLastName + ", " + CustFirstName + "</acc:ACCOUNTTITLE1>"
                    +"<acc:SHORTTITLE>" + CustLastName + ", " + CustFirstName + "</acc:SHORTTITLE>"
                    //+"<!--Optional:--><acc:MNEMONIC></acc:MNEMONIC>"
                    +"<acc:CURRENCY>" + LoanCurrency + "</acc:CURRENCY>"
                    +"<acc:ACCOUNTOFFICER>"+ COID + "</acc:ACCOUNTOFFICER>"
                    +"<acc:SMSAlerts>NO</acc:SMSAlerts>"
                    + "<acc:EmailAlerts>NO</acc:EmailAlerts>"
                    +"<acc:ConditionofOperation>1</acc:ConditionofOperation>"
                    + "</ACCOUNTAMKSAOPENWSType></amk:ACCOUNTCREATION></soapenv:Body></soapenv:Envelope>";
                    #endregion xml
                    #region call
                    //fileHeader
                    //string LogDateTime = DateTime.Now.ToString("yyyy-MM-dd HH.mm.ss");
                    //string fileHeader = "LoanAppCreateAccount_InstID_" + InstID+"_" + UserID + "_" + criteriaValue + "_" + ServerDate;
                    //add log
                    c.ReturnDT("update tblLoanAppListForOpenToCBSLogCreateAccount set CustomerID='"+ CustomerID 
                    + "',StartDate=getdate() where LoanAppID='"+criteriaValue+"'");
                    c.T24_AddLog(fileHeader, "AccountAdd_RQ", xmlStr, "AccountAdd");
                    //prepare to call
                    var client = new RestClient(CreUrl);
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("cache-control", "no-cache");
                    request.AddHeader("content-type", "text/xml");
                    request.AddParameter("text/xml", xmlStr, ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    string ResCode = "";
                    string xmlContent = "";
                    try {
                        ResCode = response.StatusCode.ToString();
                        xmlContent = response.Content.ToString();
                    } catch {
                        ERR = "Error";
                        SMS = "Status Code: "+ ResCode;
                    }
                    //add log
                    c.T24_AddLog(fileHeader, "AccountAdd_RS", xmlContent, "AccountAdd");
                    #region read xml
                    if (ERR != "Error") {
                        try {
                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml(xmlContent);
                            string successIndicator = doc.GetElementsByTagName("successIndicator").Item(0).InnerText;
                            string transactionId = doc.GetElementsByTagName("transactionId").Item(0).InnerText;
                            ACCTID = transactionId;
                            if (successIndicator == "Success")
                            {

                            }
                            else
                            {
                                //add account to T24 error
                                ERR = "Error";
                                SMS = "Something was wrong during create account";
                                ACCTID = "0"; ProductCode = ""; CATEGDESC = "";
                            }
                        } catch {
                            ERR = "Error";
                            SMS = "Unable to read XML content";
                        }
                    }                    
                    #endregion read xml
                    #endregion call
                }
                #endregion create account
                #region get Account
                if (ERR != "Error")
                {
                    #region get T24 Url
                    string sql = "exec T24_GetT24_Url @UserID='" + UserID + "',@UrlID=4,@InstID='" + InstID + "'";
                    DataTable dt = new DataTable();
                    dt = c.ReturnDT(sql);
                    string CreUrl = dt.Rows[0]["CreUrl"].ToString();
                    string CreCompany = dt.Rows[0]["CreCompany"].ToString();
                    string CreUserName = dt.Rows[0]["CreUserName"].ToString();
                    string CrePassword = dt.Rows[0]["CrePassword"].ToString();
                    #endregion get T24 Url
                    #region xml
                    string xmlStr = "<?xml version=\"1.0\"?><soapenv:Envelope xmlns:amk=\"http://temenos.com/AMKTABACLIST\" xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\"><soapenv:Header/>"
                    + "<soapenv:Body><amk:TABLETCUSTOMERACCOUNTLIST><WebRequestCommon><company>" + CreCompany + "</company><password>" + CrePassword
                    + "</password><userName>" + CreUserName + "</userName></WebRequestCommon><AMKETABCUSTACCTLISTType><enquiryInputCollection>"
                    + "<columnName>CUSTOMER</columnName><criteriaValue>" + CustomerID + "</criteriaValue><operand>EQ</operand></enquiryInputCollection>"
                    + "<enquiryInputCollection><columnName>CURRENCY</columnName><criteriaValue>" + LoanCurrency + "</criteriaValue><operand>EQ</operand></enquiryInputCollection>"
                    + "</AMKETABCUSTACCTLISTType></amk:TABLETCUSTOMERACCOUNTLIST></soapenv:Body></soapenv:Envelope>";
                    #endregion xml
                    #region call
                    //fileHeader
                    //string LogDateTime = DateTime.Now.ToString("yyyy-MM-dd HH.mm.ss");
                    //string fileHeader = "LoanAppCreateAccount_InstID_" + InstID + "_" + UserID + "_" + criteriaValue + "_" + ServerDate;
                    //add log
                    c.T24_AddLog(fileHeader, "AccountGet_RQ", xmlStr, "AccountAdd");

                    var client = new RestClient(CreUrl);
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("cache-control", "no-cache");
                    request.AddHeader("content-type", "text/xml");
                    request.AddParameter("text/xml", xmlStr, ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    string ResCode = "";
                    string xmlContent = "";
                    try
                    {
                        ResCode = response.StatusCode.ToString();
                        xmlContent = response.Content.ToString();
                    }
                    catch
                    {
                        ERR = "Error";
                        SMS = "Status Code: " + ResCode;
                    }
                    //add log
                    c.T24_AddLog(fileHeader, "AccountGet_RS", xmlContent, "AccountAdd");
                    if (ERR != "Error") {
                        try {
                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml(xmlContent);
                            string successIndicator = doc.GetElementsByTagName("successIndicator").Item(0).InnerText;
                            if (successIndicator == "Success")
                            {
                                //make account list for return
                                XmlNode node0 = doc.GetElementsByTagName("ns2:gAMKETABCUSTACCTLISTDetailType").Item(0);
                                int inode0 = node0.ChildNodes.Count;
                                for (int n = 0; n < inode0; n++)
                                {
                                    XmlNode node1 = doc.GetElementsByTagName("ns2:mAMKETABCUSTACCTLISTDetailType").Item(n);
                                    try
                                    {
                                        #region loop
                                        foreach (XmlNode item in node1.ChildNodes)
                                        {
                                            string itemVal = item.InnerText;
                                            if (item.LocalName == "ACCTID")
                                            {
                                                ACCTID = itemVal;
                                            }
                                            if (item.LocalName == "ProductCode")
                                            {
                                                ProductCode = itemVal;
                                            }
                                            if (item.LocalName == "CATEGDESC")
                                            {
                                                CATEGDESC = itemVal;
                                            }
                                        }
                                        LoanAppCreateAccountResAccListModel data = new LoanAppCreateAccountResAccListModel();
                                        data.LoanAppID = criteriaValue;
                                        data.CustomerID = CustomerID;
                                        data.AccountID = ACCTID;
                                        data.CATEGORY = ProductCode;
                                        data.CATEGDESC = CATEGDESC;
                                        data.Balance = "0";
                                        data.Restrictions = "";
                                        data.CURRENCY = LoanCurrency;
                                        AccList.Add(data);
                                        #endregion loop
                                    }
                                    catch
                                    {
                                        ERR = "Error";
                                        SMS = "Something was wrong during get account";
                                        ACCTID = "0"; ProductCode = ""; CATEGDESC = "";
                                    }
                                }
                            }
                        } catch {
                            ERR = "Error";
                            SMS = "Unable to read XML content";
                        }
                    }                        
                    #endregion call
                }
                #endregion get Account

                //log
                //c.ReturnDT("update tblLoanAppListForOpenToCBSLogCreateAccount set EndDate=getdate(),Remark='" + ERR + "' where CreateDate=(SELECT MAX(CreateDate) FROM tblLoanAppListForOpenToCBSLogCreateAccount) and LoanAppID='" + criteriaValue + "'; update tblLoanApp1 set AccountID='" + ACCTID + "' where LoanAppID='" + criteriaValue + "';");
                c.ReturnDT("update tblLoanAppListForOpenToCBSLogCreateAccount set EndDate=getdate(),Remark='" + ERR + "' where LoanAppID='" + criteriaValue + "'; update tblLoanApp1 set AccountID='" + ACCTID + "' where LoanAppID='" + criteriaValue + "';");
            }
            catch (Exception ex)
            {
                ERR = "Error";
                SMS = "Something was wrong";
            }
            DataList.ERR = ERR;
            DataList.SMS = SMS;
            DataList.DataList = AccList;
            RSData.Add(DataList);

            return RSData;
        }
        
    }
    public class LoanAppCreateAccountResModel
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
        public List<LoanAppCreateAccountResAccListModel> DataList { get; set; }
    }
    public class LoanAppCreateAccountResAccListModel
    {
        public string LoanAppID { get; set; }
        public string CustomerID { get; set; }
        public string AccountID { get; set; }
        public string CATEGORY { get; set; }
        public string CATEGDESC { get; set; }
        public string Balance { get; set; }
        public string Restrictions { get; set; }
        public string CURRENCY { get; set; }
    }


}