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
    public class LoanInfoGetFromCBSController : ApiController
    {
        //public IEnumerable<LoanInfoGetFromCBSModel> Get(string api_name, string api_key, string json)////json=[{"user":"none","pwd":"none","device_id":"none","app_vName":"none","criteriaValue":"AMKDL1823915870"}]
        public string Post([FromUri]string p, [FromUri]string msgid, [FromBody]string json)
        {
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "", ExSMS = "", ERRCode = "";
            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string ControllerName = "LoanInfoGetFromCBS";
            string FileNameForLog = msgid + "_" + ServerDate.Replace("-", "_").Replace(" ", "_").Replace(":", "_").Replace(".", "_");
            List<LoanInfoGetFromCBSModel> RSData = new List<LoanInfoGetFromCBSModel>();
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
                    UserID = c.Decrypt(strs, c.SeekKeyGet());
                    ExSMS = "";
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
                #endregion
                #region read json
                string criteriaValue = "";
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
                        criteriaValue = CheckJson[2];
                    }
                }
                #endregion
                #region data
                if (ERR != "Error")
                {
                    LoanInfoGetFromCBSModel ListHeader = new LoanInfoGetFromCBSModel();
                    ListHeader.ERR = ERR;
                    ListHeader.SMS = SMS;
                    ListHeader.ERRCode = ERRCode;
                    List<LoanInfoGetFromCBSList> DataList = new List<LoanInfoGetFromCBSList>();

                    #region get T24 Url
                    string sql = "exec T24_GetT24_Url @UserID='" + UserID + "',@UrlID=24";
                    DataTable dt = new DataTable();
                    dt = c.ReturnDT(sql);
                    string CreUrl = dt.Rows[0]["CreUrl"].ToString();
                    string CreCompany = dt.Rows[0]["CreCompany"].ToString();
                    string CreUserName = dt.Rows[0]["CreUserName"].ToString();
                    string CrePassword = dt.Rows[0]["CrePassword"].ToString();
                    #endregion get T24 Url
                    #region xml
                    string xmlStr = "<?xml version=\"1.0\"?><soapenv:Envelope xmlns:amk=\"http://temenos.com/AMKLOANINFO\" "
                    +"xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\"><soapenv:Header/><soapenv:Body>"
                    +"<amk:GETLOANINFO><WebRequestCommon><company/>"
                    +"<password>"+ CrePassword + "</password>"
                    +"<userName>"+ CreUserName + "</userName>"
                    +"</WebRequestCommon><AMKEGETLOANINFOType><enquiryInputCollection><columnName>@ID</columnName>"
                    +"<criteriaValue>"+criteriaValue+"</criteriaValue><operand>EQ</operand></enquiryInputCollection>"
                    +"</AMKEGETLOANINFOType></amk:GETLOANINFO></soapenv:Body></soapenv:Envelope>";
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
                        #region make return
                        XmlNode node0 = doc.GetElementsByTagName("ns2:gAMKEGETLOANINFODetailType").Item(0);
                        if (node0 == null)
                        {
                            ERR = "Error";
                            SMS = "No Record";
                        }
                        else
                        {
                            int inode0 = node0.ChildNodes.Count;
                            for (int n = 0; n < inode0; n++)
                            {
                                XmlNode node1 = doc.GetElementsByTagName("ns2:mAMKEGETLOANINFODetailType").Item(n);
                                int inode1 = node1.ChildNodes.Count;
                                #region declare string
                                string CUSTOMERID = "", CUSTOMERNAME = "", PRODUCTID = "", CURRENCY = ""
                                            , AMOUNT = "", MATURITYDATE = "", INTRATE = "", GURANTORCODE = ""
                                            , COBRWCUSTOMER = "", CBCCUSTOMER = "", ENQUIRYREF = "", LOANACCTNO = ""
                                            , CREDITOFFICER = "", CONAME = "", SPID = "", SPNAME = "", VILLAGEBANK = ""
                                            , GROUPNO = "";
                                #endregion declare string
                                for (int n1 = 0; n1 < inode1; n1++)
                                {

                                    foreach (XmlNode item in node1.ChildNodes)
                                    {
                                        #region para
                                        string itemVal = item.InnerText;
                                        if (item.LocalName == "CUSTOMERID")
                                        {
                                            CUSTOMERID = itemVal;
                                        }
                                        if (item.LocalName == "CUSTOMERNAME")
                                        {
                                            CUSTOMERNAME = itemVal;
                                        }
                                        if (item.LocalName == "PRODUCTID")
                                        {
                                            PRODUCTID = itemVal;
                                        }
                                        if (item.LocalName == "CURRENCY")
                                        {
                                            CURRENCY = itemVal;
                                        }
                                        if (item.LocalName == "AMOUNT")
                                        {
                                            AMOUNT = itemVal;
                                        }
                                        if (item.LocalName == "MATURITYDATE")
                                        {
                                            MATURITYDATE = itemVal;
                                        }
                                        if (item.LocalName == "INTRATE")
                                        {
                                            INTRATE = itemVal;
                                        }
                                        if (item.LocalName == "GURANTORCODE")
                                        {
                                            GURANTORCODE = itemVal;
                                        }
                                        if (item.LocalName == "COBRWCUSTOMER")
                                        {
                                            COBRWCUSTOMER = itemVal;
                                        }
                                        if (item.LocalName == "CBCCUSTOMER")
                                        {
                                            CBCCUSTOMER = itemVal;
                                        }
                                        if (item.LocalName == "ENQUIRYREF")
                                        {
                                            ENQUIRYREF = itemVal;
                                        }
                                        if (item.LocalName == "LOANACCTNO")
                                        {
                                            LOANACCTNO = itemVal;
                                        }
                                        if (item.LocalName == "CREDITOFFICER")
                                        {
                                            CREDITOFFICER = itemVal;
                                        }
                                        if (item.LocalName == "CONAME")
                                        {
                                            CONAME = itemVal;
                                        }
                                        if (item.LocalName == "SPID")
                                        {
                                            SPID = itemVal;
                                        }
                                        if (item.LocalName == "SPNAME")
                                        {
                                            SPNAME = itemVal;
                                        }
                                        if (item.LocalName == "VILLAGEBANK")
                                        {
                                            VILLAGEBANK = itemVal;
                                        }
                                        if (item.LocalName == "GROUPNO")
                                        {
                                            GROUPNO = itemVal;
                                        }
                                        #endregion para
                                    }

                                }
                                #region add item
                                LoanInfoGetFromCBSList data = new LoanInfoGetFromCBSList();
                                data.CUSTOMERID = CUSTOMERID;
                                data.CUSTOMERNAME = CUSTOMERNAME;
                                data.PRODUCTID = PRODUCTID;
                                data.CURRENCY = CURRENCY;
                                data.AMOUNT = AMOUNT;
                                data.MATURITYDATE = MATURITYDATE;
                                data.INTRATE = INTRATE;
                                data.GURANTORCODE = GURANTORCODE;
                                data.COBRWCUSTOMER = COBRWCUSTOMER;
                                data.CBCCUSTOMER = CBCCUSTOMER;
                                data.ENQUIRYREF = ENQUIRYREF;
                                data.LOANACCTNO = LOANACCTNO;
                                data.CREDITOFFICER = CREDITOFFICER;
                                data.CONAME = CONAME;
                                data.SPID = SPID;
                                data.SPNAME = SPNAME;
                                data.VILLAGEBANK = VILLAGEBANK;
                                data.GROUPNO = GROUPNO;
                                DataList.Add(data);
                                //ListHeader.DataList = DataList;
                                //RSData.Add(ListHeader);
                                #endregion add item
                            }
                            ListHeader.DataList = DataList;
                            RSData.Add(ListHeader);
                        }
                        #endregion make return
                    }
                    else {
                        ERR = "Error";
                        SMS = successIndicator;
                    }
                    #endregion read xml

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
                LoanInfoGetFromCBSModel CustHeader = new LoanInfoGetFromCBSModel();
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
    public class LoanInfoGetFromCBSModel
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
        public string ERRCode { get; set; }
        public List<LoanInfoGetFromCBSList> DataList { get; set; }
    }
    public class LoanInfoGetFromCBSList
    {
        public string CUSTOMERID { get; set; }
        public string CUSTOMERNAME { get; set; }
        public string PRODUCTID { get; set; }
        public string CURRENCY { get; set; }
        public string AMOUNT { get; set; }
        public string MATURITYDATE { get; set; }
        public string INTRATE { get; set; }
        public string GURANTORCODE { get; set; }
        public string COBRWCUSTOMER { get; set; }
        public string CBCCUSTOMER { get; set; }
        public string ENQUIRYREF { get; set; }
        public string LOANACCTNO { get; set; }
        public string CREDITOFFICER { get; set; }
        public string CONAME { get; set; }
        public string SPID { get; set; }
        public string SPNAME { get; set; }
        public string VILLAGEBANK { get; set; }
        public string GROUPNO { get; set; }
    }

}