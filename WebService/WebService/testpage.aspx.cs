using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Renci.SshNet;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using WebApiFileUpload.API.Controllers;

namespace WebService
{
    public partial class testpage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try {
                lbAppTitle.Text = ConfigurationManager.AppSettings["AppTitle"];

                //Class1 c = new Class1();
                //DataTable dtIDCard = c.ReturnDT("exec sp_LoanAppPersonKYCGetForPostToT24 @LoanAppPersonID='5195824'");
                //for (int i2 = 0; i2 < dtIDCard.Rows.Count; i2++)
                //{
                //    string xx = dtIDCard.Rows[i2]["IDExpireDate"].ToString();

                //    string ExpiryDateTag = "";
                //    if ((dtIDCard.Rows[i2]["IDExpireDate"].ToString() != "1900-01-01") || (dtIDCard.Rows[i2]["IDExpireDate"].ToString() != ""))
                //    {
                //        ExpiryDateTag = "<cus:ExpiryDate>" + dtIDCard.Rows[i2]["IDExpireDate"].ToString() + "</cus:ExpiryDate>";
                //    }
                //}


                //string Name="";
                //string VB = "";
                //string SDate = "";
                //string EDate = "";
                //string SOBDate = "";
                //string EOBDate = "";

                //string StrWhere = "";
                //if (Name.Length > 0) {
                //    StrWhere = "Name="+ Name;
                //}
                //string sql = "select * from Tablet where "+ StrWhere;

                //ConvertCustomerToProspected();


                //Class1 c = new Class1();
                //string Currency = "USD";
                //string DisbAmt = "1000";
                //string ExRate = "0";

                //string jsonWS = "{\"currency\":\"" + Currency + "\",\"exchange\":\"" + ExRate + "\",\"disbursement\":\"" + DisbAmt + "\"}";
                //string url = c.GetCalculatorWSUrl() + "/api/RefferalCommission?api_name=" + c.GetCalculatorWSAPIName() + "&api_key=" + c.GetCalculatorWSAPIKey() + "&type=json";
                //var client = new RestClient(url);
                //var Authenticator = new HttpBasicAuthenticator(c.GetCalculatorWSUser(), c.GetCalculatorWSPwd());
                //client.Authenticator = Authenticator;
                //var request = new RestRequest(Method.POST);
                //request.AddParameter("application/json; charset=utf-8", JsonConvert.SerializeObject(jsonWS), ParameterType.RequestBody);
                //IRestResponse response = client.Execute(request);
                //string jres = response.Content.ToString().TrimEnd(']').TrimStart('[');
                //JObject jsonObj = JObject.Parse(jres);
                //string err = jsonObj.GetValue("ERR").ToObject<string>();
                //string sms = jsonObj.GetValue("SMS").ToObject<string>();
                //string Commission = jsonObj.GetValue("Commission").ToObject<string>();


                //JsonSchemaGenerator generator = new JsonSchemaGenerator();
                //generator.UndefinedSchemaIdHandling = UndefinedSchemaIdHandling.UseTypeName;
                //JsonSchema schema = generator.Generate(typeof(LoanAppPostV2RQ));
                //string x = "";

                //Class1 c = new Class1();
                //string FileNameForLog="",user = "", pwd = "", device_id = "", app_vName = "", mac_address = "", criteriaValue = "", UserID = "";
                //string json = "{\"user\":\"282959\",\"pwd\":\"7272\",\"device_id\":\"355755087360947\",\"app_vName\":\"3.0.3\"}";
                //FileUploadV2RQ jObj = JsonConvert.DeserializeObject<FileUploadV2RQ>(json);
                //user = jObj.user;
                //FileNameForLog = user + "_" + FileNameForLog;
                //user = c.Encrypt(user, c.SeekKeyGet());
                //pwd = jObj.pwd;
                //pwd = c.Encrypt(pwd, c.SeekKeyGet());
                //device_id = jObj.device_id;
                //app_vName = jObj.app_vName;
                //mac_address = jObj.mac_address;
                //criteriaValue = jObj.criteriaValue;

                //string x = "abc||123|x";
                //string[] r = x.Split(new string[] { "||" }, StringSplitOptions.None);
                //string r0 = r[0];

                //string x = "fL9TBlMTLhrR+qTrY83x88ZLJqljNhBHJeiS2pjuB9kXVtEjJxd3SEf0hnOsgkbI";
                //string xx = System.Web.HttpUtility.UrlEncode(x);
                //string y = xx;



            } catch(Exception ex) {
                string x = ex.Message.ToString();
            }
        }
        public string Convert<T>(T obj)
        {
            List<T> myList = new List<T>();
            myList.Add(obj);
            return new JavaScriptSerializer().Serialize(myList);
        }

        public void abc(string x = "") {

        }
        public void testreadxml() {
            try {
                string xmlContent = "";
                string path = @"E:\x.txt";
                using (var streamReader = File.OpenText(path))
                {
                    var lines = streamReader.ReadToEnd().Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    foreach (var line in lines) {
                        xmlContent = xmlContent + line;
                    }
                }

                xmlContent = xmlContent.Replace("\r\n"," ");
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlContent);
                string successIndicator = doc.GetElementsByTagName("successIndicator").Item(0).InnerText;
                if (successIndicator == "Success")
                {
                    string ACCOUNTType = doc.GetElementsByTagName("ACCOUNTType").Item(0).InnerText; 
                }



            } catch (Exception ex) { }
        }

        public void CallWS() {
            try {
                //Class1 c = new Class1();
                //string jsonWS = "\"{\\\"user\\\":\\\"02732A\\\",\\\"pwd\\\":\\\"0000\\\",\\\"device_id\\\":\\\"359667090987184\\\",\\\"app_vName\\\": \\\"1.6\\\",\\\"mac_address\\\":\\\"123456789\\\",\\\"ProspectDataList\\\":[{\\\"ProspectClientID\\\": \\\"1\\\",\\\"CreateDateClient\\\":\\\"2019-06-25 15:43:11.025\\\",\\\"Code\\\":\\\"\\\",\\\"RegisterDate\\\":\\\"2018-12-25 15:43:11.025\\\",\\\"ReferByID\\\":\\\"AMK\\\",\\\"ReferName\\\":\\\"ReferName\\\",\\\"NameKh\\\":\\\"NameKh\\\",\\\"NameEn\\\":\\\"NameEn\\\",\\\"GenderID\\\":\\\"FEMALE\\\",\\\"Age\\\":\\\"600\\\",\\\"Phone\\\":\\\"012123123\\\",\\\"VillageID\\\":\\\"01020211\\\",\\\"BizStatusID\\\":\\\"1\\\",\\\"CollateralStatusID\\\":\\\"1\\\",\\\"LoanEligibleID\\\":\\\"1\\\",\\\"PromotionDate1\\\":\\\"2019-06-27\\\",\\\"PromotionDate2\\\":\\\"\\\",\\\"PromotionDate3\\\":\\\"\\\",\\\"CustComment\\\":\\\"CustComment\\\",\\\"ExpectOnBoardDate\\\":\\\"\\\",\\\"ProspectStatusID\\\":\\\"2\\\"}]}\"";
                //string url = "https://amkswitchserv2.amkcambodia.com:1001/api/ProspectAddV2?api_name=Tablet&api_key=GoUIaceWQ4Wq2tKFHdsU2w==&type=json";
                //var Authenticator = new HttpBasicAuthenticator(c.GetTabletWSUser(), c.GetTabletWSPwd());
                //var client = new RestClient(url);
                //client.Authenticator = Authenticator;
                //var request = new RestRequest(Method.POST);
                //request.AddParameter("json", jsonWS, ParameterType.RequestBody);
                //IRestResponse response = client.Execute(request);
                //string jres = response.Content.ToString().TrimEnd(']').TrimStart('[');
                //ProspectAddRS jObj = JsonConvert.DeserializeObject<ProspectAddRS>(jres);
                //string ERR = jObj.ERR;
                //string SMS = jObj.SMS;
                //foreach (ProspectAddSMSRS x in jObj.ProspectAddSMSRS) {
                //    string x1 = x.ProspectClientID;
                //    string code = x.Code;
                //}
            } catch (Exception ex) { }
        }

        protected void ConvertCustomerToProspected()
        {
            
            //string URL = "https://amkswitchserv2.amkcambodia.com:1001/api/ProspectAddV2?api_name=Tablet&api_key=GoUIaceWQ4Wq2tKFHdsU2w==&type=json";
            //string DATA = "\"{\\\"user\\\":\\\"02732A\\\",\\\"pwd\\\":\\\"0000\\\",\\\"device_id\\\":\\\"359667090987184\\\",\\\"app_vName\\\": \\\"1.6\\\",\\\"mac_address\\\":\\\"123456789\\\",\\\"ProspectDataList\\\":[{\\\"ProspectClientID\\\": \\\"1\\\",\\\"CreateDateClient\\\":\\\"2019-06-25 15:43:11.025\\\",\\\"Code\\\":\\\"\\\",\\\"RegisterDate\\\":\\\"2018-12-25 15:43:11.025\\\",\\\"ReferByID\\\":\\\"AMK\\\",\\\"ReferName\\\":\\\"ReferName\\\",\\\"NameKh\\\":\\\"NameKh\\\",\\\"NameEn\\\":\\\"NameEn\\\",\\\"GenderID\\\":\\\"FEMALE\\\",\\\"Age\\\":\\\"600\\\",\\\"Phone\\\":\\\"012123123\\\",\\\"VillageID\\\":\\\"01020211\\\",\\\"BizStatusID\\\":\\\"1\\\",\\\"CollateralStatusID\\\":\\\"1\\\",\\\"LoanEligibleID\\\":\\\"1\\\",\\\"PromotionDate1\\\":\\\"2019-06-27\\\",\\\"PromotionDate2\\\":\\\"\\\",\\\"PromotionDate3\\\":\\\"\\\",\\\"CustComment\\\":\\\"CustComment\\\",\\\"ExpectOnBoardDate\\\":\\\"\\\",\\\"ProspectStatusID\\\":\\\"2\\\"}]}\"";
            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            //request.Method = "POST";
            //request.ContentType = "application/json";
            //string encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes("amk:123"));
            //request.Headers.Add("Authorization", "Basic " + encoded);

            //request.ContentLength = DATA.Length;
            //StreamWriter requestWriter = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII);
            //requestWriter.Write(DATA);
            //requestWriter.Close();

            //try
            //{
            //    WebResponse webResponse = request.GetResponse();
            //    Stream webStream = webResponse.GetResponseStream();
            //    StreamReader responseReader = new StreamReader(webStream);
            //    string response = responseReader.ReadToEnd();

            //    string jres = response.TrimEnd(']').TrimStart('[');
            //    ProspectAddRS jObj = JsonConvert.DeserializeObject<ProspectAddRS>(jres);
            //    string ERR = jObj.ERR;
            //    string SMS = jObj.SMS;
            //    foreach (ProspectAddSMSRS x in jObj.ProspectAddSMSRS)
            //    {
            //        string Pros_ERR = x.ERR;
            //        string Pros_SMS = x.SMS;
            //        string ProspectClientID = x.ProspectClientID;
            //        string Code = x.Code;
            //    }
                
            //    responseReader.Close();
            //}
            //catch (Exception e)
            //{
            //    Console.Out.WriteLine("-----------------");
            //    Console.Out.WriteLine(e.Message);
            //}

        }

        protected void btnTest_Click(object sender, EventArgs e)
        {
            //try {
            //    string SavedFNamePath = ConfigurationManager.AppSettings["imgMoveTestPath"].ToString();
            //    string ImgName = ConfigurationManager.AppSettings["imgMoveTestName"].ToString();
            //    string str = NetworkShare.MoveFile("", "", SavedFNamePath, ImgName);
            //    Response.Write(str);
            //}
            //catch (Exception ex) {
            //    Response.Write(ex.Message.ToString());
            //}
        }
    }
}