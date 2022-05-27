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
    public class LPGetFromCBSController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<LPGetFromCBSResModel> Get(string api_name, string api_key)
        {
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "";
            try
            {
                #region - get T24 url                
                DataTable dtT24Url = new DataTable();
                dtT24Url = c.ReturnDT("exec T24_GetT24_Url @UserID=0,@UrlID=10");
                string CreUrl = dtT24Url.Rows[0]["CreUrl"].ToString();
                string CreCompany = dtT24Url.Rows[0]["CreCompany"].ToString();
                string CreUserName = dtT24Url.Rows[0]["CreUserName"].ToString();
                string CrePassword = dtT24Url.Rows[0]["CrePassword"].ToString();
                #endregion - T24 url
                #region xml
                string xmlStr = "<?xml version=\"1.0\"?><soapenv:Envelope xmlns:amk=\"http://temenos.com/AMKLNPURPOSE\" "
                + "xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\"><soapenv:Header/><soapenv:Body><amk:LOANPURPOSE><WebRequestCommon>"
                + "<company/><password>" + CrePassword + "</password><userName>" + CreUserName + "</userName></WebRequestCommon><AMKELOANPURPOSEType>"
                + "<enquiryInputCollection><columnName/><criteriaValue/><operand/></enquiryInputCollection></AMKELOANPURPOSEType></amk:LOANPURPOSE>"
                + "</soapenv:Body></soapenv:Envelope>";
                #endregion xml
                #region call to T24   
                //Desc18 = "Desc: Call To T24";
                //backgroundWorker18.ReportProgress(20);
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
                    XmlNode node0 = doc.GetElementsByTagName("AMKELOANPURPOSEType").Item(0);
                    int inode0 = node0.ChildNodes.Count;
                    for (int n = 0; n < inode0; n++)
                    {
                        XmlNode node1 = doc.GetElementsByTagName("ns2:gAMKELOANPURPOSEDetailType").Item(n);
                        int inode1 = node1.ChildNodes.Count;
                        for (int n1 = 0; n1 < inode1; n1++)
                        {
                            XmlNode node2 = doc.GetElementsByTagName("ns2:mAMKELOANPURPOSEDetailType").Item(n1);
                            try
                            {
                                string ID = "", DESCRIPTION = "";
                                #region item
                                foreach (XmlNode item in node2.ChildNodes)
                                {
                                    string itemVal = item.InnerText;
                                    if (item.LocalName == "ID")
                                    {
                                        ID = itemVal;
                                    }
                                    if (item.LocalName == "DESCRIPTION")
                                    {
                                        DESCRIPTION = itemVal;
                                    }
                                }
                                #endregion item
                                #region add to db
                                //Desc18 = "Desc: Add to DB: " + n1.ToString() + "/" + inode1.ToString() + " | " + ID + " | " + DESCRIPTION;
                                //backgroundWorker18.ReportProgress(20 + n1);
                                SqlConnection Con1 = new SqlConnection(c.ConStr());
                                Con1.Open();
                                SqlCommand Com1 = new SqlCommand();
                                Com1.Connection = Con1;
                                Com1.Parameters.Clear();
                                Com1.CommandText = "exec T24_AddUpdateLookUp @LOOKUPID=@LOOKUPID,@criteriaValue=@criteriaValue,@DESCRIPTION=@DESCRIPTION";
                                #region params
                                Com1.Parameters.AddWithValue("@LOOKUPID", ID);
                                Com1.Parameters.AddWithValue("@criteriaValue", "LoanPurpose");
                                Com1.Parameters.AddWithValue("@DESCRIPTION", DESCRIPTION);
                                #endregion params
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
            }
            catch
            {
                ERR = "Error";
                SMS = "Something was wrong";
            }
            List<LPGetFromCBSResModel> RSData = new List<LPGetFromCBSResModel>();
            LPGetFromCBSResModel ResSMS = new LPGetFromCBSResModel();
            ResSMS.ERR = ERR;
            ResSMS.SMS = SMS;
            RSData.Add(ResSMS);
            return RSData;
        }
        
    }
    public class LPGetFromCBSResModel
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
    }
}