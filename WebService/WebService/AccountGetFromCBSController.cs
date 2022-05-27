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
    public class AccountGetFromCBSController : ApiController
    {
        public IEnumerable<AccountCreationToCBSModel> Get(string api_name, string api_key, string json)
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

                #region jsonForAccount
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
                #endregion jsonForAccount

                #region data
                if (ERR != "Error")
                {
                    AccountCreationToCBSModel ListHeader = new AccountCreationToCBSModel();
                    ListHeader.ERR = ERR;
                    ListHeader.SMS = SMS;
                    List<AccountCreationToCBSList> DataList = new List<AccountCreationToCBSList>();

                    #region get T24 Url
                    string sql = "exec T24_GetT24_Url @UserID='" + UserID + "',@UrlID=4";
                    DataTable dt2 = new DataTable();
                    dt2 = c.ReturnDT(sql);
                    string CreUrl = dt2.Rows[0]["CreUrl"].ToString();
                    //string CreCompany = CreCompany;// dt2.Rows[0]["CreCompany"].ToString();
                    string CreUserName = dt2.Rows[0]["CreUserName"].ToString();
                    string CrePassword = dt2.Rows[0]["CrePassword"].ToString();
                    #endregion get T24 Url
                    #region xml
                    string xmlStr = "<?xml version=\"1.0\"?><soapenv:Envelope xmlns:amk=\"http://temenos.com/AMKTABACLIST\" xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\"><soapenv:Header/>"
                    + "<soapenv:Body><amk:TABLETCUSTOMERACCOUNTLIST><WebRequestCommon><company>" + CreCompany + "</company><password>" + CrePassword
                    + "</password><userName>" + CreUserName + "</userName></WebRequestCommon><AMKETABCUSTACCTLISTType><enquiryInputCollection>"
                    + "<columnName>CUSTOMER</columnName><criteriaValue>" + CustomerID + "</criteriaValue><operand>EQ</operand></enquiryInputCollection>"
                    + "<enquiryInputCollection><columnName>CURRENCY</columnName><criteriaValue>" + LoanCurrency + "</criteriaValue><operand>EQ</operand></enquiryInputCollection>"
                    + "</AMKETABCUSTACCTLISTType></amk:TABLETCUSTOMERACCOUNTLIST></soapenv:Body></soapenv:Envelope>";
                    #endregion xml
                    //add log
                    DateTime dt_LogDateTime = DateTime.Now;
                    string LogDateTime = dt_LogDateTime.ToString("yyyy-MM-dd HH.mm.ss");
                    string fileHeader = CustomerID + "_" + LogDateTime;
                    c.T24_AddLog(fileHeader, "AccountGet", xmlStr, "AccountGet");


                   


                    var client = new RestClient(CreUrl);
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("cache-control", "no-cache");
                    request.AddHeader("content-type", "text/xml");
                    request.AddParameter("text/xml", xmlStr, ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    string xmlContent = response.Content.ToString();
                    //add log
                    c.T24_AddLog(fileHeader, "AccountGet", xmlContent, "AccountGet");

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xmlContent);
                    string successIndicator = doc.GetElementsByTagName("successIndicator").Item(0).InnerText;
                    if (successIndicator == "Success")
                    {
                        #region make account list for return
                        XmlNode node0 = doc.GetElementsByTagName("ns2:gAMKETABCUSTACCTLISTDetailType").Item(0);
                        int inode0 = node0.ChildNodes.Count;
                        for (int n = 0; n < inode0; n++)
                        {
                            XmlNode node1 = doc.GetElementsByTagName("ns2:mAMKETABCUSTACCTLISTDetailType").Item(n);
                            try
                            {
                                string ACCTID = "", ProductCode = "", CATEGDESC = "", AVAILBAL = "";
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
                                    if (item.LocalName == "AVAILBAL")
                                    {
                                        AVAILBAL = itemVal;
                                    }
                                }
                                if (ProductCode == CATEGORY)
                                {
                                    AccountCreationToCBSList data = new AccountCreationToCBSList();
                                    data.AccountID = ACCTID;
                                    data.ProductCode = ProductCode;
                                    data.CATEGDESC = CATEGDESC;
                                    data.AVAILBAL = AVAILBAL;
                                    DataList.Add(data);
                                }
                            }
                            catch
                            {
                                ERR = "Error";
                                SMS = "Error while read account from T24";
                            }
                        }
                        #endregion make account list for return


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
                try
                {
                    ERR = "Error";
                    SMS = "Something was wrong while connect to T24 | " + ex.ToString().Substring(0, 50);
                }
                catch
                {
                    ERR = "Error";
                    SMS = "Something was wrong.";
                }
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

}