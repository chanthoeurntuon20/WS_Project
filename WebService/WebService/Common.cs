using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Security.Cryptography;
using System.Configuration;
using RestSharp;
using System.Net;
using System.IO;
using System.Data;

namespace WebService
{

    public enum FilterTypes
    {
        [Display(Name = "Loan Account")]
        LoanAccount = 1,
        [Display(Name = "Customer Name")]
        CustomerName = 2,
        [Display(Name = "VB Name or VB Code")]
        VBCode = 3
    }

    public class Common
    {
        Class1 c = new Class1();
        static string key = "TabletAMK*&*2014";
        public string[] CheckJsonString(string json)
        {
            string ERR = "Succeed", SMS = "", FilterType = "", FilterValue = "0", Page = "", Limit = "", ExSMS = "", AccountNo = "", UserId = "", GroupId = "";
            string[] rs = new string[9];
            try
            {
                if (json == null || json == "")
                {
                    ERR = "Error";
                    SMS = "Invalid JSON";
                }
                #region check json to get jsondata
                if (ERR != "Error")
                {
                    try
                    {
                        var objects = JArray.Parse(json);
                        foreach (JObject root in objects)
                        {
                            foreach (var item in root)
                            {
                                string key = item.Key;
                                if (key == "FilterType")
                                {
                                    FilterType = item.Value.ToString();
                                }
                                if (key == "FilterValue")
                                {
                                    FilterValue = item.Value.ToString();
                                }
                                if (key == "Page")
                                {
                                    Page = item.Value.ToString();
                                }
                                if (key == "Limit")
                                {
                                    Limit = item.Value.ToString();
                                }
                                if (key == "AccountNo")
                                {
                                    AccountNo = item.Value.ToString();
                                }
                                if (key == "GroupId")
                                {
                                    GroupId = item.Value.ToString();
                                }
                                if (key == "UserId")
                                {
                                    UserId = item.Value.ToString();
                                }


                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ERR = "Error";
                        SMS = "Invalid JSON";
                        ExSMS = ex.Message.ToString();
                    }
                }
                #endregion check json to get jsondata
            }
            catch
            {
                ERR = "Error";
                SMS = "Something was wrong";
            }
            rs[0] = ERR;
            rs[1] = SMS;
            rs[2] = FilterType;
            rs[3] = FilterValue;
            rs[4] = Page;
            rs[5] = Limit;
            rs[6] = AccountNo;
            rs[7] = GroupId;
            rs[8] = UserId;
            return rs;
        }


        public String Encrypt(String plainText)
        {
            var plainBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(Encrypt(plainBytes, GetRijndaelManaged(key)));
        }

        private byte[] Encrypt(byte[] plainBytes, RijndaelManaged rijndaelManaged)
        {
            throw new NotImplementedException();
        }

        public RijndaelManaged GetRijndaelManaged(String secretKey)
        {
            var keyBytes = new byte[16];
            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
            Array.Copy(secretKeyBytes, keyBytes, Math.Min(keyBytes.Length, secretKeyBytes.Length));
            return new RijndaelManaged
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7,
                KeySize = 128,
                BlockSize = 128,
                Key = keyBytes,
                IV = keyBytes
            };
        }

        public DateTime Convertdate(string date)
        {
            try
            {
                var dt = DateTime.ParseExact(date, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                return dt;
            }
            catch
            {
                return DateTime.Now;
            }
        }

        public DataTable GetAllKeys()
        {
            return c.ReturnDT("Select * from tbl_firebase_settup");
        }
        public DataTable GetLoanData(string loanAcc)
        {
            return c.ReturnDT("select LoanAA,CustName from tblLoanOverdueReportData where LoanAcc='" + loanAcc + "'");
        }
        public DataTable GetUserName(string userId)
        {
            return c.ReturnDT("select ud.UserName as user_code, o.Name as user_name from tbluser u left join T24_OfficeHierachy o on u.OfficeHierachyID=o.ID left join tblUserDec ud on ud.UserID= u.UserID where u.UserID='" + userId + "'");
        }

        public void SendNotificationToDeviceToken(string title, string body, string type, int id, string accNo, string senderId, string serverKey, string deviceToken)
        {
            var client = new RestClient("https://fcm.googleapis.com/fcm/send");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", $"key={serverKey}");
            request.AddHeader("Sender", $"id={senderId}");
            request.AddHeader("Content-Type", "application/json");
            var firebase = new FirebaseRequest
            {
                registration_ids = new List<string>(),
                data = new Data()
            };
            firebase.data.body = body;
            firebase.data.title = title;
            firebase.data.type = type;

            Values val = new Values();
            val.id = id;
            val.accNo = accNo;
            firebase.data.values = val;

            firebase.registration_ids.Add(deviceToken);

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(firebase);
            request.AddParameter("application/json", json, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            var result = response.Content;
        }
        public void SendNotificationToDeviceToken(string title, string body, string type, int id, string accNo, string senderId, string serverKey, List<string> deviceToken)
        {
            var client = new RestClient("https://fcm.googleapis.com/fcm/send");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", $"key={serverKey}");
            request.AddHeader("Sender", $"id={senderId}");
            request.AddHeader("Content-Type", "application/json");
            var firebase = new FirebaseRequest
            {
                registration_ids = new List<string>(),
                data = new Data()
            };
            firebase.data.body = body;
            firebase.data.title = title;
            firebase.data.type = type;


            Values val = new Values();
            val.id = id;
            val.accNo = accNo;
            firebase.data.values = val;



            firebase.registration_ids.AddRange(deviceToken);
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(firebase);
            request.AddParameter("application/json", json, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            var result = response.Content;
        }



    }
}



public class FirebaseRequest
{
    public List<string> registration_ids { get; set; }
    public Data data { get; set; }
}

public class Data
{
    public string title { get; set; }
    public string body { get; set; }
    public string type { get; set; }
    public Values values { get; set; }
}

public class Values
{
    public int id { get; set; }
    public string accNo { get; set; }
}
