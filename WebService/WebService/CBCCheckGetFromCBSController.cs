using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;
using System.Xml;

namespace WebService
{
    [BasicAuthentication]
    public class CBCCheckGetFromCBSController : ApiController
    {
        public IEnumerable<CBCCheckGetFromCBSModel> Get(string api_name, string api_key, string json)////json=[{"user":"none","pwd":"none","device_id":"none","app_vName":"none","criteriaValue":"AMKDL1823915870"}]
        {
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "", criteriaValue = "";
            DateTime dt_LogDateTime = DateTime.Now;
            string LogDateTime = dt_LogDateTime.ToString("yyyy-MM-dd HH.mm.ss");

            List<CBCCheckGetFromCBSModel> RSData = new List<CBCCheckGetFromCBSModel>();
            try
            {
                #region api
                string[] CheckApi = c.CheckApi(api_name, api_key);
                ERR = CheckApi[0];
                SMS = CheckApi[1];
                #endregion api

                #region json
                string UserID = "", criteriaValue2, InstID = "1";
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
                        criteriaValue2 = CheckJson[4];
                        if (criteriaValue2 != "")
                        {
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

                    List<CBCCheckGetFromCBSList> DataList = new List<CBCCheckGetFromCBSList>();

                    #region get T24 Url
                    string sql = "exec T24_GetT24_Url @UserID='" + UserID + "',@UrlID=23,@InstID='" + InstID + "'";
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
                    string fileHeader = UserID + "_" + criteriaValue + "_" + LogDateTime;
                    c.T24_AddLog(fileHeader, "CBCCheckGet", xmlStr, "CBCCheck");
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
                    c.T24_AddLog(fileHeader, "CBCCheckGetResult", xmlContent, "CBCCheck");
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
                        else
                        {
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
            }
            #region if Error
            if (ERR == "Error")
            {
                CBCCheckGetFromCBSModel CustHeader = new CBCCheckGetFromCBSModel();
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