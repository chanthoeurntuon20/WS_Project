using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml;

namespace WebService
{
    public class AccountPostingToCBSController : ApiController
    {
        public IEnumerable<AccountPostingToCBSJsonRes> Post([FromUri]string api_name, string api_key, string json)
        {//json=[{"user": "none", "pwd":"none","device_id":"none","app_vName":"none","CreCompany":"KH0010101","DEBITACCTNO":"1325","DEBITCURRENCY":"USD","DEBITAMOUNT":"100","DEBITVALUEDATE":"2017-09-08+07:00","CREDITACCTNO":"1457","CREDITCURRENCY":"USD","PAYMENTDETAILS":"note"}]
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "";
            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string fileHeader = "";
            try
            {
                #region api
                string[] CheckApi = c.CheckApi(api_name, api_key);
                ERR = CheckApi[0];
                SMS = CheckApi[1];
                #endregion api

                #region json to object    
                string user = "", pwd = "", CreCompany = "", TRN_TYPE = "", DEBITACCTNO = "", DEBITCURRENCY = "", DEBITAMOUNT = "", DEBITVALUEDATE = "", CREDITACCTNO = "", CREDITCURRENCY = "", ORDERINGBANK = "", PAYMENTDETAILS = "";
                AccountPostingToCBSJsonModel jObj = null;
                if (ERR != "Error")
                {
                    try
                    {
                        string jres = json.TrimEnd(']').TrimStart('[');
                        jObj = JsonConvert.DeserializeObject<AccountPostingToCBSJsonModel>(jres);
                        user = jObj.user;
                        pwd = jObj.pwd;
                        CreCompany = jObj.CreCompany;
                        TRN_TYPE = jObj.TRN_TYPE;
                        DEBITACCTNO = jObj.DEBITACCTNO;
                        DEBITCURRENCY = jObj.DEBITCURRENCY;
                        DEBITAMOUNT = jObj.DEBITAMOUNT;
                        DEBITVALUEDATE = jObj.DEBITVALUEDATE;
                        CREDITACCTNO = jObj.CREDITACCTNO;
                        CREDITCURRENCY = jObj.CREDITCURRENCY;
                        ORDERINGBANK = jObj.ORDERINGBANK;
                        PAYMENTDETAILS = jObj.PAYMENTDETAILS;

                        fileHeader = user+ "_AccountPosting_DR"+ DEBITACCTNO +"_CR"+ CREDITACCTNO +"_"+ ServerDate;
                    }
                    catch
                    {
                        ERR = "Error";
                        SMS = "Invalid JSON";
                    }
                }
                #endregion json to object
                if (ERR != "Error") { 
                    #region - get T24 url                             
                    DataTable dtT24Url = new DataTable();
                    dtT24Url = c.ReturnDT("exec T24_GetT24_Url @UserID=0,@UrlID=3");
                    string CreUrl = dtT24Url.Rows[0]["CreUrl"].ToString();
                    //string CreCompany = dtT24Url.Rows[0]["CreCompany"].ToString();
                    string CreUserName = dtT24Url.Rows[0]["CreUserName"].ToString();
                    string CrePassword = dtT24Url.Rows[0]["CrePassword"].ToString();
                    #endregion - T24 url
                    #region xml

                    string xmlStr = "<?xml version=\"1.0\"?><soapenv:Envelope xmlns:fun=\"http://temenos.com/FUNDSTRANSFERAMKVBPAL\" xmlns:amk=\"http://temenos.com/WSPOSTVBPA\" "
                    + "xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\"><soapenv:Header/><soapenv:Body><amk:WSPOSTVBPA><WebRequestCommon>"
                    + "<company>" + CreCompany + "</company>"
                    + "<password>" + CrePassword + "</password>"
                    + "<userName>" + CreUserName + "</userName>"
                    + "</WebRequestCommon><OfsFunction> </OfsFunction><FUNDSTRANSFERAMKVBPALType id=\"\">"

                    + "<fun:FT1>" + TRN_TYPE + "</fun:FT1>" // Transaction type

                    + "<fun:DEBITACCTNO>" + DEBITACCTNO + "</fun:DEBITACCTNO>"
                    + "<fun:DEBITCURRENCY>" + DEBITCURRENCY + "</fun:DEBITCURRENCY>"
                    + "<fun:DEBITAMOUNT>" + DEBITAMOUNT + "</fun:DEBITAMOUNT>"
                    + "<fun:DEBITVALUEDATE>" + DEBITVALUEDATE + "</fun:DEBITVALUEDATE>"
                    + "<fun:CREDITACCTNO>" + CREDITACCTNO + "</fun:CREDITACCTNO>"
                    + "<fun:CREDITCURRENCY>" + CREDITCURRENCY + "</fun:CREDITCURRENCY>"
                    + "<fun:CREDITVALUEDATE>" + DEBITVALUEDATE + "</fun:CREDITVALUEDATE>"

                    + "<fun:gORDERINGBANK>"
                    + "<fun:ORDERINGBANK>" + ORDERINGBANK + "</fun:ORDERINGBANK>" // PV,RV
                    + "</fun:gORDERINGBANK>"


                    + "<fun:gPAYMENTDETAILS g=\"1\">"
                    + "<fun:PAYMENTDETAILS>" + PAYMENTDETAILS + "</fun:PAYMENTDETAILS>" // Paymentdetail or description of payment
                    + "</fun:gPAYMENTDETAILS>"

                    + "</FUNDSTRANSFERAMKVBPALType></amk:WSPOSTVBPA></soapenv:Body></soapenv:Envelope>";

                    #endregion xml
                    #region call to T24   
                    c.T24_AddLog(fileHeader, "AccountPosting", xmlStr, "AccountPosting");
                    var client = new RestClient(CreUrl);
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("cache-control", "no-cache");
                    request.AddHeader("content-type", "text/xml");
                    request.AddParameter("text/xml", xmlStr, ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    try {
                        ERR = "Fail";
                        string xmlContent = response.Content.ToString();
                        c.T24_AddLog(fileHeader, "AccountPosting", xmlContent, "AccountPosting");
                        SMS = xmlContent;
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(xmlContent);                        
                        string successIndicator = doc.GetElementsByTagName("successIndicator").Item(0).InnerText;
                        if (successIndicator == "Success")
                        {
                            ERR = "Succeed";
                        }
                    } catch {
                    }
                        
                    #endregion call to T24
                }
            }
            catch { }

            List<AccountPostingToCBSJsonRes> RSData = new List<AccountPostingToCBSJsonRes>();
            AccountPostingToCBSJsonRes ResSMS = new AccountPostingToCBSJsonRes();
            ResSMS.ERR = ERR;
            ResSMS.SMS = SMS;
            RSData.Add(ResSMS);
            return RSData;
        }
    }

}