using Newtonsoft.Json;
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
    public class CBCCheckGetFromCBSBKController : ApiController
    {
        //public IEnumerable<CBCCheckGetFromCBSModel> Get(string api_name, string api_key, string json)////json=[{"user":"none","pwd":"none","device_id":"none","app_vName":"none","criteriaValue":"AMKDL1823915870"}]
        public string Post([FromUri]string p, [FromUri]string msgid, [FromBody]string json)
        {
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "", ExSMS = "", ERRCode = "";
            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string ControllerName = "CBCCheckGetFromCBS";
            string FileNameForLog = msgid + "_" + ServerDate.Replace("-", "_").Replace(" ", "_").Replace(":", "_").Replace(".", "_");
            List<CBCCheckGetFromCBSModel> RSData = new List<CBCCheckGetFromCBSModel>();
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
                string criteriaValue="", criteriaValue2, InstID="1";
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
                        if (criteriaValue2 != "") {
                            InstID = criteriaValue2;
                        }
                    }
                }
                #endregion json
                #region data
                if (ERR != "Error")
                {
                    CBCCheckGetFromCBSModel ListHeader = new CBCCheckGetFromCBSModel();
                    ListHeader.ERR = ERR;
                    ListHeader.SMS = SMS;
                    ListHeader.ERRCode = ERRCode;
                    List<CBCCheckGetFromCBSList> DataList = new List<CBCCheckGetFromCBSList>();

                    #region get T24 Url
                    string sql = "exec T24_GetT24_Url @UserID='" + UserID + "',@UrlID=23,@InstID='"+ InstID + "'";
                    DataTable dt = new DataTable();
                    dt = c.ReturnDT(sql);
                    string CreUrl = dt.Rows[0]["CreUrl"].ToString();
                    string CreCompany = dt.Rows[0]["CreCompany"].ToString();
                    string CreUserName = dt.Rows[0]["CreUserName"].ToString();
                    string CrePassword = dt.Rows[0]["CrePassword"].ToString();
                    #endregion get T24 Url
                    #region xml
                    string xmlStr = "<?xml version=\"1.0\"?><soapenv:Envelope xmlns:amk=\"http://temenos.com/AMKCBCDC\" xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\"><soapenv:Header/><soapenv:Body><amk:DEBITORCREDITORINFO><WebRequestCommon>"
                    + "<company>" + CreCompany + "</company>"
                    + "<password>" + CrePassword + "</password>"
                    + "<userName>" + CreUserName + "</userName>"
                    + "</WebRequestCommon><AMKECBCACCTINFOType><enquiryInputCollection>"
                    + "<columnName>CBC.ENQ.REQ</columnName>"
                    + "<criteriaValue>" + criteriaValue + "</criteriaValue>"
                    + "<operand>EQ</operand></enquiryInputCollection></AMKECBCACCTINFOType></amk:DEBITORCREDITORINFO></soapenv:Body></soapenv:Envelope>";
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
                        XmlNode node0 = doc.GetElementsByTagName("ns2:gAMKECBCACCTINFODetailType").Item(0);
                        if (node0 == null)
                        {
                            ERR = "Error";
                            SMS = "No Record";
                        }
                        else {
                            int inode0 = node0.ChildNodes.Count;
                            for (int n = 0; n < inode0; n++)
                            {
                                XmlNode node1 = doc.GetElementsByTagName("ns2:mAMKECBCACCTINFODetailType").Item(n);
                                int inode1 = node1.ChildNodes.Count;
                                for (int n1 = 0; n1 < inode1; n1++)
                                {
                                    XmlNode node2 = doc.GetElementsByTagName("ns2:RESPDETAIL").Item(n1);
                                    int inode2 = node2.ChildNodes.Count;
                                    foreach (XmlNode item in node2.ChildNodes)
                                    {
                                        string itemVal = item.InnerText;

                                        string CDataOjbStr = itemVal.TrimEnd(']').TrimStart('[');
                                        CBCCheckGetFromCBSCDATAModel CDataOjb = c.GenerateXmlObject<CBCCheckGetFromCBSCDATAModel>(CDataOjbStr);

                                        CBCCheckGetFromCBSList data = new CBCCheckGetFromCBSList();
                                        data.CData = itemVal;
                                        DataList.Add(data);
                                        ListHeader.DataList = DataList;
                                        RSData.Add(ListHeader);

                                    }
                                }
                            }
                        }                        
                        #endregion make return
                    }
                    else
                    {
                        //get account to T24
                        string T24_messages = doc.GetElementsByTagName("messages").Item(1).InnerText;

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
                CBCCheckGetFromCBSModel CustHeader = new CBCCheckGetFromCBSModel();
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
    public class CBCCheckGetFromCBSModel
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
        public string ERRCode { get; set; }
        public List<CBCCheckGetFromCBSList> DataList { get; set; }
    }
    public class CBCCheckGetFromCBSList
    {
        public string CData { get; set; }
    }


}