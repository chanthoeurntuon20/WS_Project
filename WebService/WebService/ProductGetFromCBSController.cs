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
    public class ProductGetFromCBSController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<ProductGetFromCBSResModel> Get(string api_name, string api_key)
        {
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "";
            try {
                #region - get T24 url                
                DataTable dtT24Url = new DataTable();
                dtT24Url = c.ReturnDT("exec T24_GetT24_Url @UserID=0,@UrlID=5");
                string CreUrl = dtT24Url.Rows[0]["CreUrl"].ToString();
                string CreCompany = dtT24Url.Rows[0]["CreCompany"].ToString();
                string CreUserName = dtT24Url.Rows[0]["CreUserName"].ToString();
                string CrePassword = dtT24Url.Rows[0]["CrePassword"].ToString();
                #endregion - T24 url
                #region xml
                string xmlStr = "<?xml version=\"1.0\"?><soapenv:Envelope xmlns:amk=\"http://temenos.com/AMKLOANPROD\" "
                + "xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\"><soapenv:Header/><soapenv:Body><amk:GETLOANPRODUCT>"
                + "<!--Optional:--><WebRequestCommon><!--Optional:-->"
                + "<company>" + CreCompany + "</company><password>" + CrePassword + "</password><userName>" + CreUserName + "</userName>"
                + "</WebRequestCommon><!--Optional:--><AMKEGETLOANPRODUCTType><!--Zero or more repetitions:--><enquiryInputCollection>"
                + "<!--Optional:--><columnName/><!--Optional:--><criteriaValue/><!--Optional:--><operand/></enquiryInputCollection>"
                + "</AMKEGETLOANPRODUCTType></amk:GETLOANPRODUCT></soapenv:Body></soapenv:Envelope>";
                #endregion xml
                #region call to T24   
                //Desc12 = "Desc: Call To T24";
                //backgroundWorker12.ReportProgress(20);
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
                    XmlNode node0 = doc.GetElementsByTagName("ns2:gAMKEGETLOANPRODUCTDetailType").Item(0);
                    int inode0 = node0.ChildNodes.Count;
                    for (int n = 0; n < inode0; n++)
                    {
                        XmlNode node1 = doc.GetElementsByTagName("ns2:mAMKEGETLOANPRODUCTDetailType").Item(n);

                        try
                        {
                            string PRODUCTID = "", PRODUCTDESC = "", CATEGORY = "", CURRENCY = "", BOFO = "", RATETYPE = "", EFFDATE = "", MINTERM = "", MAXTERM = ""
                        , MINAMOUNT = "", MAXAMOUNT = "", MINRATE = "", MAXRATE = "", DEFRATE = "", MINAGE = "", MAXAGE = "", REPAYTYPE = ""
                        , MINUPFRONTFEE = "", MAXUPFRONTFEE = "", MINMNTHTXNFEE = "", MAXMNTHTXNFEE = "";
                            #region item
                            foreach (XmlNode item in node1.ChildNodes)
                            {
                                string itemVal = item.InnerText;
                                if (item.LocalName == "PRODUCTID")
                                {
                                    PRODUCTID = itemVal;
                                }
                                if (item.LocalName == "PRODUCTDESC")
                                {
                                    PRODUCTDESC = itemVal;
                                }
                                if (item.LocalName == "CATEGORY")
                                {
                                    CATEGORY = itemVal;
                                }
                                if (item.LocalName == "CURRENCY")
                                {
                                    CURRENCY = itemVal;
                                }
                                if (item.LocalName == "BOFO")
                                {
                                    BOFO = itemVal;
                                }
                                if (item.LocalName == "RATETYPE")
                                {
                                    RATETYPE = itemVal;
                                }
                                if (item.LocalName == "EFFDATE")
                                {
                                    EFFDATE = itemVal;
                                }
                                if (item.LocalName == "MINTERM")
                                {
                                    MINTERM = itemVal;
                                }
                                if (item.LocalName == "MAXTERM")
                                {
                                    MAXTERM = itemVal;
                                }
                                if (item.LocalName == "MINAMOUNT")
                                {
                                    MINAMOUNT = itemVal;
                                }
                                if (item.LocalName == "MAXAMOUNT")
                                {
                                    MAXAMOUNT = itemVal;
                                }
                                if (item.LocalName == "MINRATE")
                                {
                                    MINRATE = itemVal;
                                }
                                if (item.LocalName == "MAXRATE")
                                {
                                    MAXRATE = itemVal;
                                }
                                if (item.LocalName == "DEFRATE")
                                {
                                    DEFRATE = itemVal;
                                }
                                if (item.LocalName == "MINAGE")
                                {
                                    MINAGE = itemVal;
                                }
                                if (item.LocalName == "MAXAGE")
                                {
                                    MAXAGE = itemVal;
                                }
                                if (item.LocalName == "REPAYTYPE")
                                {
                                    REPAYTYPE = itemVal;
                                }
                                if (item.LocalName == "MINUPFRONTFEE")
                                {
                                    MINUPFRONTFEE = itemVal;
                                }
                                if (item.LocalName == "MAXUPFRONTFEE")
                                {
                                    MAXUPFRONTFEE = itemVal;
                                }
                                if (item.LocalName == "MINMNTHTXNFEE")
                                {
                                    MINMNTHTXNFEE = itemVal;
                                }
                                if (item.LocalName == "MAXMNTHTXNFEE")
                                {
                                    MAXMNTHTXNFEE = itemVal;
                                }
                            }
                            #endregion item
                            #region add to db
                            //Desc12 = "Desc: Add to DB: " + n.ToString() + "/" + inode0.ToString() + " | " + PRODUCTID + " | " + PRODUCTDESC;
                            //backgroundWorker12.ReportProgress(20 + n);
                            SqlConnection Con1 = new SqlConnection(c.ConStr());
                            Con1.Open();
                            SqlCommand Com1 = new SqlCommand();
                            Com1.Connection = Con1;
                            Com1.Parameters.Clear();
                            Com1.CommandText = "exec T24_AddUpdateProduct @PRODUCTID=@PRODUCTID,@PRODUCTDESC=@PRODUCTDESC,@CATEGORY=@CATEGORY,@CURRENCY=@CURRENCY"
                            + ",@BOFO=@BOFO,@RATETYPE=@RATETYPE,@EFFDATE=@EFFDATE,@MINTERM=@MINTERM,@MAXTERM=@MAXTERM,@MINAMOUNT=@MINAMOUNT,@MAXAMOUNT=@MAXAMOUNT"
                            + ",@MINRATE=@MINRATE,@MAXRATE=@MAXRATE,@DEFRATE=@DEFRATE,@MINAGE=@MINAGE,@MAXAGE=@MAXAGE,@REPAYTYPE=@REPAYTYPE,@MINUPFRONTFEE=@MINUPFRONTFEE"
                            + ",@MAXUPFRONTFEE=@MAXUPFRONTFEE,@MINMNTHTXNFEE=@MINMNTHTXNFEE,@MAXMNTHTXNFEE=@MAXMNTHTXNFEE";
                            Com1.Parameters.AddWithValue("@PRODUCTID", PRODUCTID);
                            Com1.Parameters.AddWithValue("@PRODUCTDESC", PRODUCTDESC);
                            Com1.Parameters.AddWithValue("@CATEGORY", CATEGORY);
                            Com1.Parameters.AddWithValue("@CURRENCY", CURRENCY);
                            Com1.Parameters.AddWithValue("@BOFO", BOFO);
                            Com1.Parameters.AddWithValue("@RATETYPE", RATETYPE);
                            Com1.Parameters.AddWithValue("@EFFDATE", EFFDATE);
                            Com1.Parameters.AddWithValue("@MINTERM", MINTERM);
                            Com1.Parameters.AddWithValue("@MAXTERM", MAXTERM);
                            Com1.Parameters.AddWithValue("@MINAMOUNT", MINAMOUNT);
                            Com1.Parameters.AddWithValue("@MAXAMOUNT", MAXAMOUNT);
                            Com1.Parameters.AddWithValue("@MINRATE", MINRATE);
                            Com1.Parameters.AddWithValue("@MAXRATE", MAXRATE);
                            Com1.Parameters.AddWithValue("@DEFRATE", DEFRATE);
                            Com1.Parameters.AddWithValue("@MINAGE", MINAGE);
                            Com1.Parameters.AddWithValue("@MAXAGE", MAXAGE);
                            Com1.Parameters.AddWithValue("@REPAYTYPE", REPAYTYPE);
                            Com1.Parameters.AddWithValue("@MINUPFRONTFEE", MINUPFRONTFEE);
                            Com1.Parameters.AddWithValue("@MAXUPFRONTFEE", MAXUPFRONTFEE);
                            Com1.Parameters.AddWithValue("@MINMNTHTXNFEE", MINMNTHTXNFEE);
                            Com1.Parameters.AddWithValue("@MAXMNTHTXNFEE", MAXMNTHTXNFEE);
                            Com1.ExecuteNonQuery();
                            Con1.Close();
                            #endregion add to db
                        }
                        catch { }
                    }
                }
                else
                {
                    //add error log

                }
                #endregion call to T24
            }
            catch {
                ERR = "Error";
                SMS = "Something was wrong";
            }
            List<ProductGetFromCBSResModel> RSData = new List<ProductGetFromCBSResModel>();
            ProductGetFromCBSResModel ResSMS = new ProductGetFromCBSResModel();
            ResSMS.ERR = ERR;
            ResSMS.SMS = SMS;
            RSData.Add(ResSMS);
            return RSData;
        }
        
    }
    public class ProductGetFromCBSResModel
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
    }

}