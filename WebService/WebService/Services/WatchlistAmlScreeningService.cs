using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using WebService.Models.Res.CustomerAmls;

namespace WebService.Services
{
    public class WatchlistAmlScreeningService
    {
        public static WatchlistAml GetWatchlistAML(string CID)
        {
            try
            {
                Class1 os = new Class1();
                string fileHeader = "";
                string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                fileHeader = "WatchlistAMLScreening_InstID_" +CID+ "_" + ServerDate;
                DateTime dt_msgIDDate = DateTime.Now;
                string msgIDDate = dt_msgIDDate.ToString("yyyyMMddhhmmss");
                var msgID = $"BA{msgIDDate}";
                var watchlist = new WatchlistAml();
                var gett24Url = os.ReturnDT("exec GetT24Url '1'");
                var urlT24 = gett24Url.Select()[0];
                var xmlStr = @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:amk=""http://temenos.com/AMKINDCUST"" xmlns:cus=""http://temenos.com/CUSTOMERTUAMLEXEC"">
	                                <soapenv:Header/>
	                                <soapenv:Body>
		                                <amk:WSCUSTAMLEXEC>
			                                <WebRequestCommon>
				                                <company>" + urlT24[1].ToString() + @"</company>
				                                <password>" + urlT24[3].ToString() + @"</password>
				                                <userName>" + urlT24[2].ToString() + @"</userName>
			                                </WebRequestCommon>
			                                <OfsFunction>
				                                <messageId>"+ msgID + @"</messageId>
			                                </OfsFunction>
			                                <CUSTOMERTUAMLEXECType id="""+CID+@""">
				                                <cus:Blocked>Y</cus:Blocked>
                                                <cus:AMLServiceType>WATCH.VAL</cus:AMLServiceType>
                                                <cus:AMLServiceExec>EXEC</cus:AMLServiceExec>
			                                </CUSTOMERTUAMLEXECType>
		                                </amk:WSCUSTAMLEXEC>
	                                </soapenv:Body>
                                </soapenv:Envelope>";
                os.T24_AddLog(fileHeader, "WatchlistCustomer_RQ", xmlStr, "Watchlist");

                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(SoapConfigurer.GetXMLResult(xmlStr, urlT24[0].ToString()));

                os.T24_AddLog(fileHeader, "WatchlistCustomer_RS: " + 200, xmldoc.InnerXml, "Watchlist");

                XmlNodeList usernodes = xmldoc.GetElementsByTagName("CUSTOMERType");
                if (usernodes.Count <= 0)
                {
                    os.T24_AddLog(fileHeader, "WatchlistCustomer_RS: " + 500, xmldoc.InnerXml, "Watchlist");
                }
                  
                var datas = XMLNoteListToDataTable.ConvertXmlNodeListToDataTable(usernodes);
                var data = datas.Select()[0];

                watchlist.CID = CID;
                watchlist.BlockStatus = data[118].ToString();
                watchlist.WatchListScreeningStatus = data[265].ToString();
                watchlist.WatchListCaseUrl = data[266].ToString();
                watchlist.RiskProfiling = data[267].ToString();
                watchlist.AMLApprovalStatus = data[268].ToString();
                watchlist.WatchListExposition = data[269].ToString();
                watchlist.ProductAndService = data[270].ToString();

                return watchlist;
                
            }
            catch
            {
                throw;
            }
        }

    }
}