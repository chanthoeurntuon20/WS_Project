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
    [BasicAuthentication]
    public class AddressGetFromCBSController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<AddressGetFromCBSResModel> Post(string api_name, string api_key)
        {
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "";
            try {
                #region Province
                try
                {
                    #region - get T24 url                
                    DataTable dtT24Url = new DataTable();
                    dtT24Url = c.ReturnDT("exec T24_GetT24_Url @UserID=0,@UrlID=14");
                    string CreUrl = dtT24Url.Rows[0]["CreUrl"].ToString();
                    string CreCompany = dtT24Url.Rows[0]["CreCompany"].ToString();
                    string CreUserName = dtT24Url.Rows[0]["CreUserName"].ToString();
                    string CrePassword = dtT24Url.Rows[0]["CrePassword"].ToString();
                    #endregion - T24 url
                    #region xml
                    string xmlStr = "<?xml version=\"1.0\"?><soapenv:Envelope xmlns:amk=\"http://temenos.com/AMKGETPROVINCE\" "
                    + "xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\"><soapenv:Header/><soapenv:Body><amk:GETPORVINCE><WebRequestCommon>"
                    + "<company>" + CreCompany + "</company><password>" + CrePassword + "</password><userName>" + CreUserName + "</userName>"
                    + "</WebRequestCommon><AMKEGETPROVINCEType><enquiryInputCollection><columnName/><criteriaValue/><operand/></enquiryInputCollection>"
                    + "</AMKEGETPROVINCEType></amk:GETPORVINCE></soapenv:Body></soapenv:Envelope>";
                    #endregion xml
                    #region call to T24   
                    //Desc14 = "Desc: Call To T24";
                    //backgroundWorker14.ReportProgress(20);
                    var client = new RestClient(CreUrl);
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("cache-control", "no-cache");
                    request.AddHeader("content-type", "text/xml");
                    request.AddParameter("text/xml", xmlStr, ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    string xmlContent = response.Content.ToString();
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xmlContent);
                    string successIndicator = doc.GetElementsByTagName("successIndicator").Item(0).InnerText;
                    if (successIndicator == "Success")
                    {
                        XmlNode node0 = doc.GetElementsByTagName("AMKEGETPROVINCEType").Item(0);
                        int inode0 = node0.ChildNodes.Count;
                        for (int n = 0; n < inode0; n++)
                        {
                            XmlNode node1 = doc.GetElementsByTagName("ns2:gAMKEGETPROVINCEDetailType").Item(n);
                            int inode1 = node1.ChildNodes.Count;
                            for (int n1 = 0; n1 < inode1; n1++)
                            {
                                XmlNode node2 = doc.GetElementsByTagName("ns2:mAMKEGETPROVINCEDetailType").Item(n1);
                                try
                                {
                                    string LOOKUPID = "", DESCRIPTION = "";
                                    #region item
                                    foreach (XmlNode item in node2.ChildNodes)
                                    {
                                        string itemVal = item.InnerText;
                                        if (item.LocalName == "PROVINCEID")
                                        {
                                            LOOKUPID = itemVal;
                                        }
                                        if (item.LocalName == "DESCRIPTION")
                                        {
                                            DESCRIPTION = itemVal;
                                        }
                                    }
                                    #endregion item
                                    #region add to db
                                    //Desc14 = "Desc: Add to DB: " + n1.ToString() + "/" + inode1.ToString() + " | " + LOOKUPID + " | " + DESCRIPTION;
                                    //backgroundWorker14.ReportProgress(20 + n1);
                                    SqlConnection Con1 = new SqlConnection(c.ConStr());
                                    Con1.Open();
                                    SqlCommand Com1 = new SqlCommand();
                                    Com1.Connection = Con1;
                                    Com1.Parameters.Clear();
                                    Com1.CommandText = "exec T24_AddUpdateAddress @ID=@ID,@Name=@Name,@ParentID=@ParentID,@LevelID=@LevelID";
                                    Com1.Parameters.AddWithValue("@ID", LOOKUPID);
                                    Com1.Parameters.AddWithValue("@Name", DESCRIPTION);
                                    Com1.Parameters.AddWithValue("@ParentID", "0");
                                    Com1.Parameters.AddWithValue("@LevelID", "1");
                                    Com1.ExecuteNonQuery();
                                    Con1.Close();
                                    #endregion add to db
                                }
                                catch { }
                            }
                        }
                    }
                    else
                    {
                        //add error log

                    }
                    #endregion call to T24
                    System.Threading.Thread.Sleep(1000);
                }
                catch { }
                #endregion Province
                #region District
                try
                {
                    #region - get T24 url                
                    DataTable dtT24Url = new DataTable();
                    dtT24Url = c.ReturnDT("exec T24_GetT24_Url @UserID=0,@UrlID=15");
                    string CreUrl = dtT24Url.Rows[0]["CreUrl"].ToString();
                    string CreCompany = dtT24Url.Rows[0]["CreCompany"].ToString();
                    string CreUserName = dtT24Url.Rows[0]["CreUserName"].ToString();
                    string CrePassword = dtT24Url.Rows[0]["CrePassword"].ToString();
                    #endregion - T24 url
                    #region xml
                    string xmlStr = "<?xml version=\"1.0\"?><soapenv:Envelope xmlns:amk=\"http://temenos.com/AMKGETDISTRICT\" "
                    + "xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\"><soapenv:Header/><soapenv:Body><amk:GETDISTRICT><WebRequestCommon>"
                    + "<company>" + CreCompany + "</company><password>" + CrePassword + "</password><userName>" + CreUserName + "</userName></WebRequestCommon>"
                    + "<AMKEGETDISTRICTType><enquiryInputCollection><columnName/><criteriaValue/><operand/></enquiryInputCollection></AMKEGETDISTRICTType>"
                    + "</amk:GETDISTRICT></soapenv:Body></soapenv:Envelope>";
                    #endregion xml
                    #region call to T24   
                    //Desc14 = "Desc: Call To T24";
                    //backgroundWorker14.ReportProgress(20);
                    var client = new RestClient(CreUrl);
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("cache-control", "no-cache");
                    request.AddHeader("content-type", "text/xml");
                    request.AddParameter("text/xml", xmlStr, ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    string xmlContent = response.Content.ToString();
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xmlContent);
                    string successIndicator = doc.GetElementsByTagName("successIndicator").Item(0).InnerText;
                    if (successIndicator == "Success")
                    {
                        XmlNode node0 = doc.GetElementsByTagName("AMKEGETDISTRICTType").Item(0);
                        int inode0 = node0.ChildNodes.Count;
                        for (int n = 0; n < inode0; n++)
                        {
                            XmlNode node1 = doc.GetElementsByTagName("ns2:gAMKEGETDISTRICTDetailType").Item(n);
                            int inode1 = node1.ChildNodes.Count;
                            for (int n1 = 0; n1 < inode1; n1++)
                            {
                                XmlNode node2 = doc.GetElementsByTagName("ns2:mAMKEGETDISTRICTDetailType").Item(n1);
                                try
                                {
                                    string LOOKUPID = "", DESCRIPTION = "", PROVINCEID = "";
                                    #region item
                                    foreach (XmlNode item in node2.ChildNodes)
                                    {
                                        string itemVal = item.InnerText;
                                        if (item.LocalName == "DISTRICTID")
                                        {
                                            LOOKUPID = itemVal;
                                        }
                                        if (item.LocalName == "DESCRIPTION")
                                        {
                                            DESCRIPTION = itemVal;
                                        }
                                        if (item.LocalName == "PROVINCEID")
                                        {
                                            PROVINCEID = itemVal;
                                        }
                                    }
                                    #endregion item
                                    #region add to db
                                    //Desc14 = "Desc: Add to DB: " + n1.ToString() + "/" + inode1.ToString() + " | " + LOOKUPID + " | " + DESCRIPTION;
                                    //backgroundWorker14.ReportProgress(20 + n1);
                                    SqlConnection Con1 = new SqlConnection(c.ConStr());
                                    Con1.Open();
                                    SqlCommand Com1 = new SqlCommand();
                                    Com1.Connection = Con1;
                                    Com1.Parameters.Clear();
                                    Com1.CommandText = "exec T24_AddUpdateAddress @ID=@ID,@Name=@Name,@ParentID=@ParentID,@LevelID=@LevelID";
                                    Com1.Parameters.AddWithValue("@ID", LOOKUPID);
                                    Com1.Parameters.AddWithValue("@Name", DESCRIPTION);
                                    Com1.Parameters.AddWithValue("@ParentID", PROVINCEID);
                                    Com1.Parameters.AddWithValue("@LevelID", "2");
                                    Com1.ExecuteNonQuery();
                                    Con1.Close();
                                    #endregion add to db
                                }
                                catch { }
                            }
                        }
                    }
                    else
                    {
                        //add error log

                    }
                    #endregion call to T24
                    System.Threading.Thread.Sleep(1000);
                }
                catch { }
                #endregion District
                #region Commune
                try
                {
                    #region - get T24 url                
                    DataTable dtT24Url = new DataTable();
                    dtT24Url = c.ReturnDT("exec T24_GetT24_Url @UserID=0,@UrlID=16");
                    string CreUrl = dtT24Url.Rows[0]["CreUrl"].ToString();
                    string CreCompany = dtT24Url.Rows[0]["CreCompany"].ToString();
                    string CreUserName = dtT24Url.Rows[0]["CreUserName"].ToString();
                    string CrePassword = dtT24Url.Rows[0]["CrePassword"].ToString();
                    #endregion - T24 url
                    #region xml
                    string xmlStr = "<?xml version=\"1.0\"?><soapenv:Envelope xmlns:amk=\"http://temenos.com/AMKGETCOMMUNE\" "
                    + "xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\"><soapenv:Header/><soapenv:Body><amk:GETCOMMUNE><WebRequestCommon>"
                    + "<company>" + CreCompany + "</company><password>" + CrePassword + "</password><userName>" + CreUserName + "</userName></WebRequestCommon>"
                    + "<AMKEGETCOMMUNEType><enquiryInputCollection><columnName/><criteriaValue/><operand/></enquiryInputCollection></AMKEGETCOMMUNEType>"
                    + "</amk:GETCOMMUNE></soapenv:Body></soapenv:Envelope>";
                    #endregion xml
                    #region call to T24   
                    //Desc14 = "Desc: Call To T24";
                    //backgroundWorker14.ReportProgress(20);
                    var client = new RestClient(CreUrl);
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("cache-control", "no-cache");
                    request.AddHeader("content-type", "text/xml");
                    request.AddParameter("text/xml", xmlStr, ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    string xmlContent = response.Content.ToString();
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xmlContent);
                    string successIndicator = doc.GetElementsByTagName("successIndicator").Item(0).InnerText;
                    if (successIndicator == "Success")
                    {
                        XmlNode node0 = doc.GetElementsByTagName("AMKEGETCOMMUNEType").Item(0);
                        int inode0 = node0.ChildNodes.Count;
                        for (int n = 0; n < inode0; n++)
                        {
                            XmlNode node1 = doc.GetElementsByTagName("ns2:gAMKEGETCOMMUNEDetailType").Item(n);
                            int inode1 = node1.ChildNodes.Count;
                            for (int n1 = 0; n1 < inode1; n1++)
                            {
                                XmlNode node2 = doc.GetElementsByTagName("ns2:mAMKEGETCOMMUNEDetailType").Item(n1);
                                try
                                {
                                    string LOOKUPID = "", DESCRIPTION = "", ParentID = "";
                                    #region item
                                    foreach (XmlNode item in node2.ChildNodes)
                                    {
                                        string itemVal = item.InnerText;
                                        if (item.LocalName == "COMMUNEID")
                                        {
                                            LOOKUPID = itemVal;
                                        }
                                        if (item.LocalName == "DESCRIPTION")
                                        {
                                            DESCRIPTION = itemVal;
                                        }
                                        if (item.LocalName == "DISTRICTID")
                                        {
                                            ParentID = itemVal;
                                        }
                                    }
                                    #endregion item
                                    #region add to db
                                    //Desc14 = "Desc: Add to DB: " + n1.ToString() + "/" + inode1.ToString() + " | " + LOOKUPID + " | " + DESCRIPTION;
                                    //backgroundWorker14.ReportProgress(20 + n1);
                                    SqlConnection Con1 = new SqlConnection(c.ConStr());
                                    Con1.Open();
                                    SqlCommand Com1 = new SqlCommand();
                                    Com1.Connection = Con1;
                                    Com1.Parameters.Clear();
                                    Com1.CommandText = "exec T24_AddUpdateAddress @ID=@ID,@Name=@Name,@ParentID=@ParentID,@LevelID=@LevelID";
                                    Com1.Parameters.AddWithValue("@ID", LOOKUPID);
                                    Com1.Parameters.AddWithValue("@Name", DESCRIPTION);
                                    Com1.Parameters.AddWithValue("@ParentID", ParentID);
                                    Com1.Parameters.AddWithValue("@LevelID", "3");
                                    Com1.ExecuteNonQuery();
                                    Con1.Close();
                                    #endregion add to db
                                }
                                catch { }
                            }
                        }
                    }
                    else
                    {
                        //add error log

                    }
                    #endregion call to T24
                    System.Threading.Thread.Sleep(1000);
                }
                catch { }
                #endregion Commune
                #region Village
                try
                {
                    #region - get T24 url                
                    DataTable dtT24Url = new DataTable();
                    dtT24Url = c.ReturnDT("exec T24_GetT24_Url @UserID=0,@UrlID=17");
                    string CreUrl = dtT24Url.Rows[0]["CreUrl"].ToString();
                    string CreCompany = dtT24Url.Rows[0]["CreCompany"].ToString();
                    string CreUserName = dtT24Url.Rows[0]["CreUserName"].ToString();
                    string CrePassword = dtT24Url.Rows[0]["CrePassword"].ToString();
                    #endregion - T24 url
                    #region xml
                    string xmlStr = "<?xml version=\"1.0\"?><soapenv:Envelope xmlns:amk=\"http://temenos.com/AMKGETVILLAGE\" "
                    + "xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\"><soapenv:Header/><soapenv:Body><amk:GETVILLAGE><WebRequestCommon>"
                    + "<company>" + CreCompany + "</company><password>" + CrePassword + "</password><userName>" + CreUserName + "</userName></WebRequestCommon>"
                    + "<AMKEGETVILLAGEType><enquiryInputCollection><columnName/><criteriaValue/><operand/></enquiryInputCollection>"
                    + "</AMKEGETVILLAGEType></amk:GETVILLAGE></soapenv:Body></soapenv:Envelope>";
                    #endregion xml
                    #region call to T24   
                    //Desc14 = "Desc: Call To T24";
                    //backgroundWorker14.ReportProgress(20);
                    var client = new RestClient(CreUrl);
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("cache-control", "no-cache");
                    request.AddHeader("content-type", "text/xml");
                    request.AddParameter("text/xml", xmlStr, ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    string xmlContent = response.Content.ToString();
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xmlContent);
                    string successIndicator = doc.GetElementsByTagName("successIndicator").Item(0).InnerText;
                    if (successIndicator == "Success")
                    {
                        XmlNode node0 = doc.GetElementsByTagName("AMKEGETVILLAGEType").Item(0);
                        int inode0 = node0.ChildNodes.Count;
                        for (int n = 0; n < inode0; n++)
                        {
                            XmlNode node1 = doc.GetElementsByTagName("ns2:gAMKEGETVILLAGEDetailType").Item(n);
                            int inode1 = node1.ChildNodes.Count;
                            for (int n1 = 0; n1 < inode1; n1++)
                            {
                                XmlNode node2 = doc.GetElementsByTagName("ns2:mAMKEGETVILLAGEDetailType").Item(n1);
                                try
                                {
                                    string LOOKUPID = "", DESCRIPTION = "", ParentID = "";
                                    #region item
                                    foreach (XmlNode item in node2.ChildNodes)
                                    {
                                        string itemVal = item.InnerText;
                                        if (item.LocalName == "VILLAGEID")
                                        {
                                            LOOKUPID = itemVal;
                                        }
                                        if (item.LocalName == "DESCRIPTION")
                                        {
                                            DESCRIPTION = itemVal;
                                        }
                                        if (item.LocalName == "COMMUNEID")
                                        {
                                            ParentID = itemVal;
                                        }
                                    }
                                    #endregion item
                                    #region add to db
                                    //Desc14 = "Desc: Add to DB: " + n1.ToString() + "/" + inode1.ToString() + " | " + LOOKUPID + " | " + DESCRIPTION;
                                    //backgroundWorker14.ReportProgress(20 + n1);
                                    SqlConnection Con1 = new SqlConnection(c.ConStr());
                                    Con1.Open();
                                    SqlCommand Com1 = new SqlCommand();
                                    Com1.Connection = Con1;
                                    Com1.Parameters.Clear();
                                    Com1.CommandText = "exec T24_AddUpdateAddress @ID=@ID,@Name=@Name,@ParentID=@ParentID,@LevelID=@LevelID";
                                    Com1.Parameters.AddWithValue("@ID", LOOKUPID);
                                    Com1.Parameters.AddWithValue("@Name", DESCRIPTION);
                                    Com1.Parameters.AddWithValue("@ParentID", ParentID);
                                    Com1.Parameters.AddWithValue("@LevelID", "4");
                                    Com1.ExecuteNonQuery();
                                    Con1.Close();
                                    #endregion add to db
                                }
                                catch { }
                            }
                        }
                    }
                    else
                    {
                        //add error log

                    }
                    #endregion call to T24
                    System.Threading.Thread.Sleep(1000);
                }
                catch { }
                #endregion Village
            }
            catch {
                ERR = "Error";
                SMS = "Something was wrong";
            }
            List<AddressGetFromCBSResModel> RSData = new List<AddressGetFromCBSResModel>();
            AddressGetFromCBSResModel ResSMS = new AddressGetFromCBSResModel();
            ResSMS.ERR = ERR;
            ResSMS.SMS = SMS;
            RSData.Add(ResSMS);
            return RSData;
        }
        
    }
    public class AddressGetFromCBSResModel
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
    }
}