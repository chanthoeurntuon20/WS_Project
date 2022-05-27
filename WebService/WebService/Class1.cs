using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Renci.SshNet;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Serialization;
using WebService.Models.Req.CustomerAmls;
using WebService.Models.Req.Persons;
using WebService.Services;

namespace WebService
{
    public class Class1
    {
        ValidationCheck vc = new ValidationCheck();
        #region ed
        public string SeekKeyGet() {
            string rs = "CxTh2u3gedgY+0JVS3VuucSYrU1kf+FFz8dloyYy2R8=";
            rs= Decrypt(rs, "AMKTablet*&*2014");
            return rs;
        }
        //--------------------------------------------
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
        public byte[] Encrypt(byte[] plainBytes, RijndaelManaged rijndaelManaged)
        {
            return rijndaelManaged.CreateEncryptor()
                .TransformFinalBlock(plainBytes, 0, plainBytes.Length);
        }

        public byte[] Decrypt(byte[] encryptedData, RijndaelManaged rijndaelManaged)
        {
            return rijndaelManaged.CreateDecryptor()
                .TransformFinalBlock(encryptedData, 0, encryptedData.Length);
        }
        public String Encrypt(String plainText, String key)
        {
            var plainBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(Encrypt(plainBytes, GetRijndaelManaged(key)));
        }
        public String Decrypt(String encryptedText, String key)
        {
            var encryptedBytes = Convert.FromBase64String(encryptedText);
            return Encoding.UTF8.GetString(Decrypt(encryptedBytes, GetRijndaelManaged(key)));
        }

        //---------------------------------------------
        #endregion
        public string WebPathGet()
        {
            return ConfigurationManager.AppSettings["WebPath"];
        }

        public static string[] TabletLoanImage;//0=path,1=user,2=pwd
        public void ImgPathSet(string[] str)
        {
            TabletLoanImage = str;
        }
        public string[] ImgPathTabletLoanImageGet()
        {
            return TabletLoanImage;
        }
        //public string ImgPathGet()
        //{
        //    //return ConfigurationManager.AppSettings["ImgPath"];
        //}
        public string ConStr() {
            return ConfigurationManager.AppSettings["ConStr"];
        }
        public String ConStr2()
        {
            string HostConStr = ConfigurationManager.AppSettings["ConStrSecond"];
            return HostConStr;
        }
        public string ConStrInsightConn()
        {
            return ConfigurationManager.AppSettings["ConStrInsightConn"];
        }
        public DataTable ReturnDT(string sql) {
            DataTable dt = new DataTable();
            try {
                SqlConnection cn = new SqlConnection(ConStr());
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter da = new SqlDataAdapter(sql, cn);
                da.SelectCommand.CommandTimeout = Convert.ToInt16(ConfigurationManager.AppSettings["SqlTimeout"]);//SqlTimeout
                da.Fill(dt);
                cn.Close();
                cn.Dispose();
                SqlConnection.ClearAllPools();
            } catch(Exception ex) {
                string xx = ex.Message.ToString();
            }
            return dt;
        }
        public DataTable ReturnDT2(string sql)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection cn = new SqlConnection(ConStr2());
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter da = new SqlDataAdapter(sql, cn);
                da.SelectCommand.CommandTimeout = Convert.ToInt16(ConfigurationManager.AppSettings["SqlTimeout"]);//SqlTimeout
                da.Fill(dt);
                cn.Close();
                cn.Dispose();
                SqlConnection.ClearAllPools();
            }
            catch (Exception ex)
            {
                string xx = ex.Message.ToString();
            }
            return dt;
        }
        public string ConStrInsight()
        {
            return ConfigurationManager.AppSettings["ConStrInsight"];
        }
        public DataTable ReturnDTInsight(string sql)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection cn = new SqlConnection(ConStrInsight());
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter da = new SqlDataAdapter(sql, cn);
                //da.SelectCommand.CommandTimeout = 3000;
                da.Fill(dt);
                cn.Close();
                cn.Dispose();
                SqlConnection.ClearAllPools();
            }
            catch (Exception ex)
            {
                string xx = ex.Message.ToString();
            }
            return dt;
        }

        public string GetTabletWSUrl()
        {
            string Str = ConfigurationManager.AppSettings["TabletWSUrl"];
            return Str;
        }
        public string GetTabletWSAPIName()
        {
            string Str = ConfigurationManager.AppSettings["TabletWSAPIName"];
            return Str;
        }
        public string GetTabletWSAPIKey()
        {
            string Str = ConfigurationManager.AppSettings["TabletWSAPIKey"];
            return Str;
        }
        public string GetTabletWSUser()
        {
            string Str = ConfigurationManager.AppSettings["TabletWSUser"];
            return Str;
        }
        public string GetTabletWSPwd()
        {
            string Str = ConfigurationManager.AppSettings["TabletWSPwd"];
            return Str;
        }
        public int GetTabletWSRestTimeout()
        {
            int Str = Convert.ToInt32(ConfigurationManager.AppSettings["RestTimeout"]);
            return Str;
        }
        #region Calcutor
        public string GetCalculatorWSUrl()
        {
            string Str = ConfigurationManager.AppSettings["CalculatorWSUrl"];
            return Str;
        }
        public string GetCalculatorWSAPIName()
        {
            string Str = ConfigurationManager.AppSettings["CalculatorWSAPIName"];
            return Str;
        }
        public string GetCalculatorWSAPIKey()
        {
            string Str = ConfigurationManager.AppSettings["CalculatorWSAPIKey"];
            return Str;
        }
        public string GetCalculatorWSUser()
        {
            string Str = ConfigurationManager.AppSettings["CalculatorWSUser"];
            return Str;
        }
        public string GetCalculatorWSPwd()
        {
            string Str = ConfigurationManager.AppSettings["CalculatorWSPwd"];
            return Str;
        }
        #endregion  Calculator

        public string[] CheckDateFormat(string s,string lb) {
            string[] rs =new string[2];
            string rs0 = "Error",rs1="";
            try {
                //string s = "2010-02-29";
                DateTime result;
                if (DateTime.TryParseExact(
                     s,
                     "yyyy-MM-dd",
                     CultureInfo.InvariantCulture,
                     DateTimeStyles.AssumeUniversal,
                     out result))

                {
                    rs0 = "Succeed";
                    rs1 = "";
                }
                else {
                    rs0 = "Error";
                    rs1 = "Invalid Date of " + lb;
                }
            } catch { }
            rs[0] = rs0;
            rs[1] = rs1;
            return rs;
        }
        public string[] CheckDateTimeFormat(string s, string lb)
        {
            string[] rs = new string[2];
            string rs0 = "Error", rs1 = "";
            try
            {
                //string s = "2010-02-29";
                DateTime result;
                if (DateTime.TryParseExact(
                     s,
                     "yyyy-MM-dd HH:mm:ss.fff",
                     CultureInfo.InvariantCulture,
                     DateTimeStyles.AssumeUniversal,
                     out result))

                {
                    rs0 = "Succeed";
                    rs1 = "";
                }
                else
                {
                    rs0 = "Error";
                    rs1 = "Invalid Date of " + lb;
                }
            }
            catch { }
            rs[0] = rs0;
            rs[1] = rs1;
            return rs;
        }
        public string[] CheckIsMoneyFormat(string s, string lb)
        {
            string[] rs = new string[2];
            string rs0 = "Error", rs1 = "";
            try
            {
                var regex = @"^[0-9]+\.[0-9]{2}$|[0-9]+\.[0-9]{2}[^0-9]";//100000000.20
                var isMoney = Regex.IsMatch(s, regex);

                if (isMoney)
                {
                    rs0 = "Succeed";
                    rs1 = "";
                }
                else {
                    rs0 = "Error";
                    rs1 = "Invalid Money of " + lb;
                }                
            }
            catch { }
            rs[0] = rs0;
            rs[1] = rs1;
            return rs;
        }
        public string[] CheckIsIntFormat(string s, string lb)
        {
            string[] rs = new string[2];
            string rs0 = "Error", rs1 = "";
            try
            {
                int value;
                if (int.TryParse(s, out value)) {
                    rs0 = "Succeed";
                    rs1 = "";
                }
                else
                {
                    rs0 = "Error";
                    rs1 = "Invalid Number of " + lb;
                }
            }
            catch { }
            rs[0] = rs0;
            rs[1] = rs1;
            return rs;
        }

        public string formatDateHourDate(string st)
        {

            string rv = "";

            try
            {
                string TM = st.Substring(0, 8);
                string Y = st.Substring(15, 4);
                string M = st.Substring(12, 2);
                string D = st.Substring(9, 2);
                rv = Y + "-" + M + "-" + D + " " + TM;
            }
            catch (Exception) { }
            return rv;
        }
        public string[] CheckProductValidation(string PRODUCTID,string CURRENCY,string Amount,string Term,string Rate,string MonthlyFee,string UpFrontFee)
        {
            string[] rs = new string[2];
            string rs0 = "Error", rs1 = "";
            try
            {
                SqlConnection Con1 = new SqlConnection(ConStr());
                Con1.Open();
                SqlCommand Com1 = new SqlCommand();
                Com1.Connection = Con1;
                Com1.Parameters.Clear();
                Com1.CommandText = "exec T24_ProductValidation @PRODUCTID=@PRODUCTID,@CURRENCY=@CURRENCY,@Amount=@Amount,@Term=@Term,@Rate=@Rate,@MonthlyFee=@MonthlyFee,@UpFrontFee=@UpFrontFee";
                Com1.Parameters.AddWithValue("@PRODUCTID", PRODUCTID);
                Com1.Parameters.AddWithValue("@CURRENCY", CURRENCY);
                Com1.Parameters.AddWithValue("@Amount", Amount);
                Com1.Parameters.AddWithValue("@Term", Term);
                Com1.Parameters.AddWithValue("@Rate", Rate);
                Com1.Parameters.AddWithValue("@MonthlyFee", MonthlyFee);
                Com1.Parameters.AddWithValue("@UpFrontFee", UpFrontFee);
                DataTable dt1 = new DataTable();
                dt1.Load(Com1.ExecuteReader());
                if (dt1.Rows.Count == 0) {
                    rs0 = "Error";
                    rs1 = "Invalid Product Valivation";
                }else
                {
                    rs0 = "Succeed";
                    rs1 = "";
                }
                Con1.Close();
            }
            catch {
                rs0 = "Error";
                rs1 = "Invalid Product Valivation";
            }
            rs[0] = rs0;
            rs[1] = rs1;
            return rs;
        }
        public string[] CheckImage(string ImgName,string Ext, string lb)
        {
            string[] rs = new string[2];
            string rs0 = "Error", rs1 = "";
            try
            {
                if (ImgName.Length == 0)
                {
                    rs0 = "Error";
                    rs1 = "Invalid Image/Ext of " + lb;
                }
                else{
                    if (Ext == ".jpg" || Ext == ".png")
                    {
                        rs0 = "Succeed";
                        rs1 = "";
                    }
                    else
                    {
                        rs0 = "Error";
                        rs1 = "Invalid Image of " + lb;
                        
                    }
                }
                
            }
            catch { }
            rs[0] = rs0;
            rs[1] = rs1;
            return rs;
        }
        public string[] GetSMSByMsgID(string MsgID)
        {
            string ERR = "Succeed", SMS = "", ExSMS = "", Msg="", MsgCode="";
            string[] rs = new string[5];
            try
            {
                DataTable dt = new DataTable();
                SqlConnection cn = new SqlConnection(ConStr());
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter da = new SqlDataAdapter("select Msg,MsgCode from tblMsgError where MsgID='" + MsgID + "'", cn);
                da.SelectCommand.CommandTimeout = Convert.ToInt16(ConfigurationManager.AppSettings["SqlTimeout"]);//SqlTimeout
                da.Fill(dt);
                cn.Close();

                Msg = dt.Rows[0]["Msg"].ToString();
                MsgCode = dt.Rows[0]["MsgCode"].ToString();
            }
            catch (Exception ex)
            {
                ERR = "Error";
                SMS = "Cannot get message from DB";
                ExSMS = ex.Message.ToString();
            }
            rs[0] = ERR;
            rs[1] = SMS;
            rs[2] = ExSMS;
            rs[3] = Msg;
            rs[4] = MsgCode;
            return rs;
        }

        public string[] CheckObjED(string json,string MsgID)
        {
            string ERR = "Succeed", SMS = "", ExSMS = "",jsonStr="";
            string[] rs = new string[4];
            #region validate
            if (json == null)
            {
                ERR = "Error";
            }
            if (ERR != "Error")
            {
                if (json.Length==0)
                {
                    ERR = "Error";
                }
            }
            if (ERR != "Error")
            {
                try
                {
                    jsonStr = Decrypt(json, SeekKeyGet());
                }
                catch (Exception ex)
                {
                    ERR = "Error";
                    ExSMS = ex.Message.ToString();
                }
            }
            #endregion
            #region get sms
            if (ERR == "Error")
            {
                string[] str = GetSMSByMsgID(MsgID);
                ERR = str[0];
                if (ERR == "Error")
                {
                    SMS = str[1];
                }
                else
                {
                    SMS = str[3];
                }
                ERR = "Error";
                ExSMS = str[2];
            }
            #endregion
            rs[0] = ERR;
            rs[1] = SMS;
            rs[2] = ExSMS;
            rs[3] = jsonStr;
            return rs;
        }
        public string[] CheckMsgID(string msgid)
        {
            string ERR = "Succeed", SMS = "", ExSMS = "";
            string[] rs = new string[3];
            #region validate
            if (msgid == null)
            {
                ERR = "Error";
            }
            if (ERR != "Error")
            {
                if ((msgid.Length < 3) || (msgid.Length > 15))
                {
                    ERR = "Error";
                }
            }
            if (ERR != "Error")
            {
                if (vc.hasSpecialChar(msgid))
                {
                    ERR = "Error";
                }
            }
            #endregion
            #region get sms
            if (ERR == "Error") {
                string[] str=GetSMSByMsgID("1");
                ERR = str[0];
                if (ERR == "Error")
                {
                    SMS = str[1];
                }
                else {
                    SMS = str[3];
                }
                ERR = "Error";
                ExSMS = str[2];
            }
            #endregion
            rs[0] = ERR;
            rs[1] = SMS;
            rs[2] = ExSMS;
            return rs;
        }
        public string[] CheckUrlParam(string p)
        {
            string ERR = "Succeed", SMS = "",ExSMS="";
            string api_name = "", api_key = "";
            string[] rs = new string[5];
            try
            {
                if ((p == null) || (p == ""))
                {
                    ERR = "Error";
                    SMS = "Invalid Paramter";
                }
                if (ERR != "Error")
                {
                    try
                    {
                        //api_name || api_key
                        string pStr =Decrypt(p, SeekKeyGet());
                        string[] pSplit = pStr.Split(new string[] { "||" }, StringSplitOptions.None);
                        api_name = pSplit[0];
                        api_key = Encrypt(pSplit[1], SeekKeyGet());
                    }
                    catch(Exception ex)
                    {
                        ERR = "Error";
                        SMS = "Invalid Paramter";
                        ExSMS = ex.Message.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                ERR = "Error";
                SMS = "Something was wrong while checking Api";
                ExSMS = ex.Message.ToString();
            }
            rs[0] = ERR;
            rs[1] = SMS;
            rs[2] = api_name;
            rs[3] = api_key;
            rs[4] = ExSMS;
            return rs;
        }
        public string[] CheckApi(string api_name, string api_key) {
            string ERR = "Succeed", SMS = "";
            string[] rs = new string[2];
            try {
                if (api_name == null || api_name == "" || api_key == null || api_key == "")
                {
                    ERR = "Error";
                    SMS = "Invalid API";
                }
                if (ERR != "Error") {
                    try {
                        Decrypt(api_key, SeekKeyGet());
                    } catch {
                        ERR = "Error";
                        SMS = "Invalid API";
                    }
                }
                if (ERR != "Error")
                {
                    if (vc.hasSpecialChar(api_name)) {
                        ERR = "Error";
                        SMS = "Invalid API";
                    }
                }

                if (ERR!= "Error") {
                    DataTable dt = ReturnDT("exec T24_check_api @api_name='"+ api_name + "',@api_key='"+ api_key + "'");
                    ERR = dt.Rows[0]["ERR"].ToString();
                    SMS = dt.Rows[0]["SMS"].ToString();
                }
            } catch {
                ERR = "Error";
                SMS = "Something was wrong while checking Api";
            }
            rs[0] = ERR;
            rs[1] = SMS;
            return rs;
        }
        public string[] CheckJson(string json)
        {
            string ERR = "Succeed", SMS = "", criteriaValue = "0",GroupID="", criteriaValue2="", ExSMS = "";
            string[] rs = new string[5];
            try
            {
                if (json == null || json == "")
                {
                    ERR = "Error";
                    SMS = "Invalid JSON";
                }

                //string user = "", pwd = "";
                #region check json to get user/pwd
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
                                //if (key == "user")
                                //{
                                //    user = item.Value.ToString();
                                //}
                                //if (key == "pwd")
                                //{
                                //    pwd = item.Value.ToString();
                                //}
                                if (key == "criteriaValue")
                                {
                                    criteriaValue = item.Value.ToString();
                                }
                                if (key == "criteriaValue2")
                                {
                                    criteriaValue2 = item.Value.ToString();
                                }
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        ERR = "Error";
                        SMS = "Invalid JSON";
                        ExSMS = ex.Message.ToString();
                    }
                }
                #endregion check json to get user/pwd
                #region get userid
                //if (ERR != "Error")
                //{
                //    if (pwd != "none") {
                //        pwd = Encrypt(pwd, SeekKeyGet());
                //    }
                //    DataTable dt = ReturnDT("exec T24_check_user @user='" + user + "',@pwd='" + pwd + "'");
                //    ERR = dt.Rows[0]["ERR"].ToString();
                //    SMS = dt.Rows[0]["SMS"].ToString();
                //    GroupID = dt.Rows[0]["GroupID"].ToString();
                //}
                #endregion get userid
            }
            catch
            {
                ERR = "Error";
                SMS = "Something was wrong";
            }
            rs[0] = ERR;
            rs[1] = SMS;
            rs[2] = criteriaValue;
            rs[3] = GroupID;
            rs[4] = criteriaValue2;
            return rs;
        }
        public string[] CheckJsonForCreateAccount(string json)
        {
            string ERR = "Succeed", SMS = "", ExSMS="", criteriaValue = "0", GroupID = "", CreCompany="", CustomerID = ""
                , CATEGORY="", ACCOUNTTITLE1="", SHORTTITLE="", LoanCurrency="";
            string[] rs = new string[10];
            try
            {
                if (json == null || json == "")
                {
                    ERR = "Error";
                    SMS = "Invalid JSON";
                }

                //string user = "", pwd = "";
                #region check json to get user/pwd
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
                                //if (key == "user")
                                //{
                                //    user = item.Value.ToString();
                                //}
                                //if (key == "pwd")
                                //{
                                //    pwd = item.Value.ToString();
                                //}
                                if (key == "CreCompany")
                                {
                                    CreCompany = item.Value.ToString();
                                }
                                if (key == "CustomerID")
                                {
                                    CustomerID = item.Value.ToString();
                                }
                                if (key == "CATEGORY")
                                {
                                    CATEGORY = item.Value.ToString();
                                }
                                if (key == "ACCOUNTTITLE1")
                                {
                                    ACCOUNTTITLE1 = item.Value.ToString();
                                }
                                if (key == "SHORTTITLE")
                                {
                                    SHORTTITLE = item.Value.ToString();
                                }
                                if (key == "LoanCurrency")
                                {
                                    LoanCurrency = item.Value.ToString();
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
                #endregion check json to get user/pwd
                #region get userid
                //if (ERR != "Error")
                //{
                //    if (pwd != "none")
                //    {
                //        pwd = Encrypt(pwd, SeekKeyGet());
                //    }
                //    DataTable dt = ReturnDT("exec T24_check_user @user='" + user + "',@pwd='" + pwd + "'");
                //    ERR = dt.Rows[0]["ERR"].ToString();
                //    SMS = dt.Rows[0]["SMS"].ToString();
                //    GroupID = dt.Rows[0]["GroupID"].ToString();
                //}
                #endregion get userid
            }
            catch
            {
                ERR = "Error";
                SMS = "Something was wrong";
            }
            rs[0] = ERR;
            rs[1] = SMS;
            rs[2] = criteriaValue;
            rs[3] = GroupID;
            rs[4] = CreCompany;
            rs[5] = CustomerID;
            rs[6] = CATEGORY;
            rs[7] = ACCOUNTTITLE1;
            rs[8] = SHORTTITLE;
            rs[9] = LoanCurrency;

            return rs;
        }

        public string[] SessionIDCheck(string ServerDate,string SessionID)
        {
            string ERR = "Succeed", SMS = "", ExSMS = "", UserID = "", ERRCode = "", api_name = "";
            string[] rs = new string[6];
            #region ed
            try {
                string x = Decrypt(SessionID, SeekKeyGet());
                string[] xStr = x.Split('|');
                api_name = xStr[3];
            } catch (Exception ex)
            {
                ERR = "Error";
                ExSMS = ex.Message.ToString();
                #region get sms
                if (ERR == "Error")
                {
                    string[] str = GetSMSByMsgID("9");
                    ERR = str[0];
                    if (ERR == "Error")
                    {
                        SMS = str[1];
                    }
                    else
                    {
                        SMS = str[3];
                    }
                    ERR = "Error";
                    ExSMS = str[2];
                    ERRCode = str[4];
                }
                #endregion
            }
            #endregion
            #region go to db
            if (ERR != "Error") {
                SqlConnection cn = new SqlConnection(ConStr());
                cn.Open();
                try
                {
                    string sql = "exec sp_SessionCheck @ServerDate=@ServerDate,@SessionID=@SessionID";
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = sql;
                    cmd.Connection = cn;
                    cmd.Parameters.AddWithValue("@ServerDate", ServerDate);
                    cmd.Parameters.AddWithValue("@SessionID", SessionID);
                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());

                    ERR = dt.Rows[0]["ERR"].ToString();
                    SMS = dt.Rows[0]["SMS"].ToString();
                    UserID = dt.Rows[0]["UserID"].ToString();
                    ERRCode = dt.Rows[0]["ERRCode"].ToString();
                }
                catch (Exception ex)
                {
                    ERR = "Error";
                    SMS = "Cannot get data from DB";
                    ExSMS = ex.Message.ToString();
                }
                cn.Close();
            }            
            #endregion
            rs[0] = ERR;
            rs[1] = SMS;
            rs[2] = ExSMS;
            rs[3] = UserID;
            rs[4] = ERRCode;
            rs[5] = api_name;
            return rs;
        }

        //public string[] CheckJsonLoanApp(string json)
        //{
        //    string ERR = "Succeed", SMS = "";
        //    string[] rs = new string[2];
        //    try
        //    {
        //        if (json == null || json == "")
        //        {
        //            ERR = "Error";
        //            SMS = "Invalid JSON";
        //        }

        //        string user = "", pwd = "";
        //        #region check json to get user/pwd
        //        //if (ERR != "Error")
        //        //{
        //        //    try
        //        //    {
        //        //        var objects = JArray.Parse(json);
        //        //        foreach (JObject root in objects)
        //        //        {
        //        //            foreach (var item in root)
        //        //            {
        //        //                string key = item.Key;
        //        //                if (key == "user")
        //        //                {
        //        //                    user = item.Value.ToString();
        //        //                }
        //        //                if (key == "pwd")
        //        //                {
        //        //                    pwd = item.Value.ToString();
        //        //                }
        //        //                //nested LoanApp
        //        //                if (key == "LoanApp")
        //        //                {


        //        //                    //var LoanItem = item.Value;
        //        //                    //var j1 = JArray.Parse(LoanItem.ToString());
        //        //                    //foreach (JObject root_j1 in j1) {
        //        //                    //    foreach (var li in root_j1) {
        //        //                    //        string li_key = li.Key;
        //        //                    //        //read LoanApp

        //        //                    //        //nested PurpsoeDetail
        //        //                    //        if (li_key == "PurpsoeDetail") {
        //        //                    //            var li2Item = li.Value;

        //        //                    //        }
        //        //                    //        //nested Person
        //        //                    //        if (li_key == "Person")
        //        //                    //        {
        //        //                    //            var li2Item = li.Value;
        //        //                    //            //read Person

        //        //                    //            //nested Person

        //        //                    //        }
        //        //                    //        //nested Creditor
        //        //                    //        if (li_key == "Creditor")
        //        //                    //        {
        //        //                    //            var li2Item = li.Value;

        //        //                    //        }
        //        //                    //        //nested Opinion
        //        //                    //        if (li_key == "Opinion")
        //        //                    //        {
        //        //                    //            var li2Item = li.Value;

        //        //                    //        }
        //        //                    //        //nested CashFlow
        //        //                    //        if (li_key == "CashFlow")
        //        //                    //        {
        //        //                    //            var li2Item = li.Value;

        //        //                    //        }


        //        //                    //    }
        //        //                    //}
        //        //                }

        //        //            }
        //        //        }
        //        //    }
        //        //    catch(Exception ex)
        //        //    {
        //        //        ERR = "Error";
        //        //        SMS = "Invalid JSON";
        //        //    }
        //        //}
        //        #endregion check json to get user/pwd

        //        LoanAppModel jObj=null;
        //        #region json
        //        if (ERR != "Error") {
        //            try {
        //                jObj = JsonConvert.DeserializeObject<LoanAppModel>(json);
        //                user=jObj.user;
        //                pwd = jObj.pwd;
        //            } catch {
        //                ERR = "Error";
        //                SMS = "Invalid JSON";
        //            }
        //        }
        //        #endregion json
        //        #region get userid
        //        if (ERR != "Error") {
        //            DataTable dt = ReturnDT("exec T24_check_user @user='" + user + "',@pwd='" + Encrypt(pwd, SeekKeyGet()) + "'");
        //            ERR = dt.Rows[0]["ERR"].ToString();
        //            SMS = dt.Rows[0]["SMS"].ToString();
        //        }
        //        #endregion get userid
        //        #region LoanApp
        //        if (ERR != "Error")
        //        {
        //            LoanAppResModel ListHeader = new LoanAppResModel();
        //            List<LoanAppResSMS> SMSList = new List<LoanAppResSMS>();
        //            #region checking
        //            try
        //            {                        
        //                foreach (var loan in jObj.LoanApp)
        //                {
        //                    #region LoanApp                            
        //                    string IDOnDevice = loan.IDOnDevice;
        //                    string LoanAppID = loan.LoanAppID;
        //                    string LoanAppStatusID = loan.LoanAppStatusID;

        //                    string DeviceDate = loan.DeviceDate;
        //                    rs = CheckDateTimeFormat(DeviceDate, "DeviceDate");
        //                    ERR = rs[0];
        //                    SMS = rs[1];
        //                    if (ERR == "Error") {
        //                        LoanAppResSMS data = new LoanAppResSMS();
        //                        data.SMS = SMS;
        //                        SMSList.Add(data);
        //                    }

        //                    string ProductID = loan.ProductID;
        //                    string LoanRequestAmount = loan.LoanRequestAmount;
        //                    rs = CheckIsMoneyFormat(LoanRequestAmount, "LoanRequestAmount");
        //                    ERR = rs[0];
        //                    SMS = rs[1];
        //                    if (ERR == "Error")
        //                    {
        //                        LoanAppResSMS data = new LoanAppResSMS();
        //                        data.SMS = SMS;
        //                        SMSList.Add(data);
        //                    }

        //                    string LoanPurposeID1 = loan.LoanPurposeID1;
        //                    string LoanPurposeID2 = loan.LoanPurposeID2;
        //                    string LoanPurposeID3 = loan.LoanPurposeID3;
        //                    string OwnCapital = loan.OwnCapital;
        //                    rs = CheckIsMoneyFormat(OwnCapital, "OwnCapital");
        //                    ERR = rs[0];
        //                    SMS = rs[1];
        //                    if (ERR == "Error")
        //                    {
        //                        LoanAppResSMS data = new LoanAppResSMS();
        //                        data.SMS = SMS;
        //                        SMSList.Add(data);
        //                    }

        //                    string DisbursementDate = loan.DisbursementDate;
        //                    rs = CheckDateFormat(DisbursementDate, "DisbursementDate");
        //                    ERR = rs[0];
        //                    SMS = rs[1];
        //                    if (ERR == "Error")
        //                    {
        //                        LoanAppResSMS data = new LoanAppResSMS();
        //                        data.SMS = SMS;
        //                        SMSList.Add(data);
        //                    }

        //                    string FirstWithdrawal = loan.FirstWithdrawal;
        //                    rs = CheckIsMoneyFormat(FirstWithdrawal, "FirstWithdrawal");
        //                    ERR = rs[0];
        //                    SMS = rs[1];
        //                    if (ERR == "Error")
        //                    {
        //                        LoanAppResSMS data = new LoanAppResSMS();
        //                        data.SMS = SMS;
        //                        SMSList.Add(data);
        //                    }

        //                    string LoanTerm = loan.LoanTerm;
        //                    string FirstRepaymentDate = loan.FirstRepaymentDate;
        //                    rs = CheckDateFormat(FirstRepaymentDate, "FirstRepaymentDate");
        //                    ERR = rs[0];
        //                    SMS = rs[1];
        //                    if (ERR == "Error")
        //                    {
        //                        LoanAppResSMS data = new LoanAppResSMS();
        //                        data.SMS = SMS;
        //                        SMSList.Add(data);
        //                    }

        //                    string LoanInterestRate = loan.LoanInterestRate;
        //                    string CustomerRequestRate = loan.CustomerRequestRate;
        //                    string CompititorRate = loan.CompititorRate;
        //                    string CustomerConditionID = loan.CustomerConditionID;
        //                    string COProposedAmount = loan.COProposedAmount;
        //                    rs = CheckIsMoneyFormat(COProposedAmount, "COProposedAmount");
        //                    ERR = rs[0];
        //                    SMS = rs[1];
        //                    if (ERR == "Error")
        //                    {
        //                        LoanAppResSMS data = new LoanAppResSMS();
        //                        data.SMS = SMS;
        //                        SMSList.Add(data);
        //                    }
        //                    string COProposedTerm = loan.COProposedTerm;
        //                    string COProposeRate = loan.COProposeRate;
        //                    string FrontBackOfficeID = loan.FrontBackOfficeID;
        //                    string GroupNumber = loan.GroupNumber;
        //                    string LoanCycleID = loan.LoanCycleID;
        //                    string RepaymentHistoryID = loan.RepaymentHistoryID;
        //                    string LoanReferralID = loan.LoanReferralID;
        //                    string DebtIinfoID = loan.DebtIinfoID;
        //                    string MonthlyFee = loan.MonthlyFee;
        //                    string Compulsory = loan.Compulsory;
        //                    string CompulsoryTerm = loan.CompulsoryTerm;
        //                    string Currency = loan.Currency;
        //                    string UpFrontFee = loan.UpFrontFee;
        //                    string UpFrontAmt = loan.UpFrontAmt;
        //                    #region validate product
        //                    rs = CheckProductValidation(ProductID, Currency, COProposedAmount, LoanTerm, LoanInterestRate, MonthlyFee, UpFrontFee);
        //                    ERR = rs[0];
        //                    SMS = rs[1];
        //                    if (ERR == "Error")
        //                    {
        //                        LoanAppResSMS data = new LoanAppResSMS();
        //                        data.SMS = SMS;
        //                        SMSList.Add(data);
        //                    }
        //                    #endregion validate product
        //                    //

        //                    #endregion LoanApp
        //                    #region PurposeDetail
        //                    if (loan.PurposeDetail.Count == 0) {
        //                        ERR = "Error";
        //                        SMS = "PurpsoeDetail is required";
        //                        LoanAppResSMS data = new LoanAppResSMS();
        //                        data.SMS = SMS;
        //                        SMSList.Add(data);
        //                    }
        //                    foreach (var lp in loan.PurposeDetail)
        //                    {
        //                        string LoanAppPurpsoeDetail = lp.LoanAppPurposeDetail;
        //                        string Quantity = lp.Quantity;
        //                        rs = CheckIsIntFormat(Quantity, "PurpsoeDetail-Quantity");
        //                        ERR = rs[0];
        //                        SMS = rs[1];
        //                        if (ERR == "Error")
        //                        {
        //                            LoanAppResSMS data = new LoanAppResSMS();
        //                            data.SMS = SMS;
        //                            SMSList.Add(data);
        //                        }

        //                        string UnitPrice = lp.UnitPrice;
        //                        rs = CheckIsMoneyFormat(UnitPrice, "PurpsoeDetail-UnitPrice");
        //                        ERR = rs[0];
        //                        SMS = rs[1];
        //                        if (ERR == "Error")
        //                        {
        //                            LoanAppResSMS data = new LoanAppResSMS();
        //                            data.SMS = SMS;
        //                            SMSList.Add(data);
        //                        }
        //                        //

        //                    }
        //                    #endregion PurpsoeDetail
        //                    #region Person
        //                    if (loan.Person.Count == 0)
        //                    {
        //                        ERR = "Error";
        //                        SMS = "Person is required";
        //                        LoanAppResSMS data = new LoanAppResSMS();
        //                        data.SMS = SMS;
        //                        SMSList.Add(data);
        //                    }
        //                    foreach (var lp in loan.Person)
        //                    {
        //                        #region Person
        //                        string LoanAppPersonID = lp.LoanAppPersonID;
        //                        string LoanAppPersonTypeID = lp.LoanAppPersonTypeID;
        //                        string PersonID = lp.PersonID;
        //                        string CustomerID = lp.CustomerID;
        //                        string Number = lp.Number;
        //                        string VillageBankID = lp.VillageBankID;
        //                        string AltName = lp.AltName;
        //                        string TitleID = lp.TitleID;
        //                        string LastName = lp.LastName;
        //                        string FirstName = lp.FirstName;
        //                        string GenderID = lp.GenderID;
        //                        string DateOfBirth = lp.DateOfBirth;
        //                        rs = CheckDateFormat(DateOfBirth, "Person-DateOfBirth");
        //                        ERR = rs[0];
        //                        SMS = rs[1];
        //                        if (ERR == "Error")
        //                        {
        //                            LoanAppResSMS data = new LoanAppResSMS();
        //                            data.SMS = SMS;
        //                            SMSList.Add(data);
        //                        }

        //                        string IDCardTypeID = lp.IDCardTypeID;
        //                        string IDCardNumber = lp.IDCardNumber;
        //                        string IDCardExpireDate = lp.IDCardExpireDate;
        //                        rs = CheckDateFormat(IDCardExpireDate, "Person-IDCardExpireDate");
        //                        ERR = rs[0];
        //                        SMS = rs[1];
        //                        if (ERR == "Error")
        //                        {
        //                            LoanAppResSMS data = new LoanAppResSMS();
        //                            data.SMS = SMS;
        //                            SMSList.Add(data);
        //                        }

        //                        string IDCardIssuedDate = lp.IDCardIssuedDate;
        //                        rs = CheckDateFormat(IDCardIssuedDate, "Person-IDCardIssuedDate");
        //                        ERR = rs[0];
        //                        SMS = rs[1];
        //                        if (ERR == "Error")
        //                        {
        //                            LoanAppResSMS data = new LoanAppResSMS();
        //                            data.SMS = SMS;
        //                            SMSList.Add(data);
        //                        }

        //                        string MaritalStatusID = lp.MaritalStatusID;
        //                        string EducationID = lp.EducationID;
        //                        string CityOfBirthID = lp.CityOfBirthID;
        //                        string Telephone3 = lp.Telephone3;
        //                        string VillageIDPermanent = lp.VillageIDPermanent;
        //                        string LocationCodeIDPermanent = lp.LocationCodeIDPermanent;
        //                        string VillageIDCurrent = lp.VillageIDCurrent;
        //                        string LocationCodeIDCurrent = lp.LocationCodeIDCurrent;
        //                        string SortAddress = lp.SortAddress;
        //                        string FamilyMember = lp.FamilyMember;
        //                        string FamilyMemberActive = lp.FamilyMemberActive;
        //                        string PoorID = lp.PoorID;
        //                        string DeviceDate_Person = lp.DeviceDate;
        //                        rs = CheckDateFormat(DeviceDate_Person, "Person-DeviceDate");
        //                        ERR = rs[0];
        //                        SMS = rs[1];
        //                        if (ERR == "Error")
        //                        {
        //                            LoanAppResSMS data = new LoanAppResSMS();
        //                            data.SMS = SMS;
        //                            SMSList.Add(data);
        //                        }
        //                        //

        //                        #endregion Person

        //                        #region Creditor 
        //                        foreach (var lp2 in lp.Creditor)
        //                        {
        //                            string CreditorID = lp2.CreditorID;
        //                            string ApprovedAmount = lp2.ApprovedAmount;
        //                            rs = CheckIsMoneyFormat(ApprovedAmount, "Creditor-ApprovedAmount");
        //                            ERR = rs[0];
        //                            SMS = rs[1];
        //                            if (ERR == "Error")
        //                            {
        //                                LoanAppResSMS data = new LoanAppResSMS();
        //                                data.SMS = SMS;
        //                                SMSList.Add(data);
        //                            }

        //                            string OutstandingBalance = lp2.OutstandingBalance;
        //                            rs = CheckIsMoneyFormat(OutstandingBalance, "Creditor-OutstandingBalance");
        //                            ERR = rs[0];
        //                            SMS = rs[1];
        //                            if (ERR == "Error")
        //                            {
        //                                LoanAppResSMS data = new LoanAppResSMS();
        //                                data.SMS = SMS;
        //                                SMSList.Add(data);
        //                            }

        //                            string InterestRate = lp2.InterestRate;
        //                            string RepaymentTypeID = lp2.RepaymentTypeID;
        //                            string RepaymentTermID = lp2.RepaymentTermID;
        //                            string LoanStartDate = lp2.LoanStartDate;
        //                            rs = CheckDateFormat(LoanStartDate, "Creditor-LoanStartDate");
        //                            ERR = rs[0];
        //                            SMS = rs[1];
        //                            if (ERR == "Error")
        //                            {
        //                                LoanAppResSMS data = new LoanAppResSMS();
        //                                data.SMS = SMS;
        //                                SMSList.Add(data);
        //                            }

        //                            string LoanEndDate = lp2.LoanEndDate;
        //                            rs = CheckDateFormat(LoanEndDate, "Creditor-LoanEndDate");
        //                            ERR = rs[0];
        //                            SMS = rs[1];
        //                            if (ERR == "Error")
        //                            {
        //                                LoanAppResSMS data = new LoanAppResSMS();
        //                                data.SMS = SMS;
        //                                SMSList.Add(data);
        //                            }

        //                            string RemainingInstallment = lp2.RemainingInstallment;
        //                            string LoanAppCreditorID = lp2.LoanAppCreditorID;
        //                            //

        //                        }
        //                        #endregion Creditor 
        //                        #region ClientAsset
        //                        foreach (var lp2 in lp.ClientAsset)
        //                        {
        //                            string Description = lp2.Description;
        //                            string Quantity = lp2.Quantity;
        //                            string UnitPrice = lp2.UnitPrice;
        //                        }
        //                        #endregion ClientAsset
        //                        #region ClientBusiness
        //                        foreach (var lp2 in lp.ClientBusiness)
        //                        {
        //                            string Description = lp2.Description;
        //                            string Quantity = lp2.Quantity;
        //                            string UnitPrice = lp2.UnitPrice;
        //                        }
        //                        #endregion ClientBusiness
        //                        #region ClientCollateral
        //                        foreach (var lp2 in lp.ClientCollateral)
        //                        {
        //                            string ColleteralTypeID = lp2.ColleteralTypeID;
        //                            string ColleteralDocTypeID = lp2.ColleteralDocTypeID;
        //                            string ColleteralDocNumber = lp2.ColleteralDocNumber;
        //                            string Description = lp2.Description;
        //                            string Quantity = lp2.Quantity;
        //                            string UnitPrice = lp2.UnitPrice;

        //                            #region ClientCollateralImg
        //                            foreach (var lp3 in lp2.ClientCollateralImg)
        //                            {
        //                                string ImgName = lp3.ImgName;
        //                                string Ext = lp3.Ext;
        //                            }
        //                            #endregion ClientCollateralImg
        //                        }
        //                        #endregion ClientCollateral
        //                        #region GuarantorBusiness
        //                        foreach (var lp2 in lp.GuarantorBusiness)
        //                        {
        //                            string Description = lp2.Description;
        //                            string NetProfitPerYear = lp2.NetProfitPerYear;
        //                        }
        //                        #endregion
        //                        #region GuarantorAsset
        //                        foreach (var lp2 in lp.GuarantorAsset)
        //                        {
        //                            string Description = lp2.Description;
        //                            string Quantity = lp2.Quantity;
        //                            string UnitPrice = lp2.UnitPrice;
        //                        }
        //                        #endregion
        //                        #region PersonImg
        //                        foreach (var lp2 in lp.PersonImg)
        //                        {
        //                            string ImgName = lp2.ImgName;
        //                            string Ext = lp2.Ext;
        //                            string OneCardTwoDoc = lp2.OneCardTwoDoc;
        //                        }
        //                        #endregion

        //                    }
        //                    #endregion Person
        //                    #region Opinion
        //                    foreach (var lp in loan.Opinion)
        //                    {
        //                        string Description = lp.Description;
        //                        string CreateBy = lp.CreateBy;
        //                        string DeviceDate_Opinion = lp.DeviceDate;
        //                    }
        //                    #endregion Opinion
        //                    #region CashFlow
        //                    foreach (var lp in loan.CashFlow)
        //                    {
        //                        #region CashFlow
        //                        string StudyMonthAmount = lp.StudyMonthAmount;
        //                        string StudyStartMonth = lp.StudyStartMonth;
        //                        string FamilyExpensePerMonth = lp.FamilyExpensePerMonth;
        //                        string OtherExpensePerMonth = lp.OtherExpensePerMonth;
        //                        #endregion CashFlow
        //                        #region MSI
        //                        foreach (var lp2 in lp.MSI)
        //                        {
        //                            #region MSI
        //                            string IncomeTypeID = lp2.IncomeTypeID;
        //                            string MainSourceIncomeID = lp2.MainSourceIncomeID;
        //                            string Remark = lp2.Remark;
        //                            string Quantity = lp2.Quantity;
        //                            string ExAge = lp2.ExAge;
        //                            string BusAge = lp2.BusAge;
        //                            string isMSI = lp2.isMSI;
        //                            #endregion MSI
        //                            #region MSIRegular
        //                            foreach (var lp3 in lp2.MSIRegular)
        //                            {
        //                                string Description = lp3.Description;
        //                                string Amount = lp3.Amount;
        //                                string UnitID = lp3.UnitID;
        //                                string Cost = lp3.Cost;
        //                                string OneIncomeTwoExpense = lp3.OneIncomeTwoExpense;
        //                                string CurrencyID = lp3.Currency;
        //                                string Month = lp3.Month;
        //                            }
        //                            #endregion MSIRegular
        //                            #region MSIIrregular
        //                            foreach (var lp3 in lp2.MSIIrregular)
        //                            {
        //                                string Description = lp3.Description;
        //                                string Amount = lp3.Amount;
        //                                string UnitID = lp3.UnitID;
        //                                string Cost = lp3.Cost;
        //                                string OneIncomeTwoExpense = lp3.OneIncomeTwoExpense;
        //                                string CurrencyID = lp3.Currency;
        //                                string Month = lp3.Month;
        //                            }
        //                            #endregion MSIIrregular

        //                        }
        //                        #endregion MSI

        //                    }
        //                    #endregion CashFlow

        //                }
        //            }
        //            catch
        //            {
        //                ERR = "Error";
        //                SMS = "Something was wrong in LoanApp JSON";
        //            }
        //            #endregion checking
        //            ListHeader.ERR = ERR;
        //            ListHeader.SMS = SMS;
        //            ListHeader.SMSList = SMSList;

        //        }
        //        #endregion LoanApp
        //    }
        //    catch
        //    {
        //        ERR = "Error";
        //        SMS = "Something was wrong";
        //    }
        //    rs[0] = ERR;
        //    rs[1] = SMS;
        //    return rs;
        //}

        public void T24_AddLog(string filename, string func, string sms,string folder)
        {
            try
            {
                DateTime dt = DateTime.Now;
                string todayDate = dt.ToString("yyyyMMdd");

                string path = "\\"+ folder;
                try {
                    string dir = AppDomain.CurrentDomain.BaseDirectory + "Log" + path + "\\";
                    if (Directory.Exists(dir)){}
                    else
                    {
                        System.IO.Directory.CreateDirectory(dir);
                    }
                } catch { }
                
                try
                {
                    string dir = AppDomain.CurrentDomain.BaseDirectory + "Log" + path + "\\"+ todayDate+"\\";
                    if (Directory.Exists(dir)) { }
                    else
                    {
                        System.IO.Directory.CreateDirectory(dir);
                    }
                }
                catch { }


                filename = filename.Replace(" ", "_").Replace("-", "_").Replace(":", "_").Replace(".", "_");
                string filePath = AppDomain.CurrentDomain.BaseDirectory + "Log"+ path + "\\" + todayDate + "\\" + filename + ".log";
                if (!File.Exists(filePath))
                {
                    FileStream fs = File.Create(filePath);
                    fs.Close();
                }
                StreamWriter sw = File.AppendText(filePath);
                sw.WriteLine("");
                if (!string.IsNullOrEmpty(sms))
                {
                    sw.WriteLine(dt.ToString("HH:mm:ss") + " - " + func + " : " + sms);
                }
                else
                {
                    sw.WriteLine(dt.ToString("HH:mm:ss") + " - " + func + " : " + sms);
                }

                sw.Flush();
                sw.Close();
            }
            catch (Exception ex) {
                string xx = ex.Message.ToString();
            }
        }
        
        public int GetLineNumber(Exception ex)
        {
            var lineNumber = 0;
            const string lineSearch = ":line ";
            var index = ex.StackTrace.LastIndexOf(lineSearch);
            if (index != -1)
            {
                var lineNumberText = ex.StackTrace.Substring(index + lineSearch.Length);
                if (int.TryParse(lineNumberText, out lineNumber))
                {
                }
            }
            return lineNumber;
        }

        public string[] AddEditCustomer_bk(string fileHeader, string LoanAppPersonID,string LoanAppPersonTypeID)
        {
            string[] rs = new string[2];
            string ERR = "Error",SMS="",rsContent="";
            try
            {
                fileHeader = fileHeader.Replace(" ", "_").Replace("-", "_").Replace(":", "_").Replace(".", "_");
                string sql = "exec T24_LoanAppListForOpenToCBS2 @LoanAppPersonID='" + LoanAppPersonID + "'";
                DataTable dt = ReturnDT(sql);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    #region params
                    string CreUrl = dt.Rows[i]["CreUrl"].ToString();
                    string CreCompany = dt.Rows[i]["CreCompany"].ToString();
                    string CreUserName = dt.Rows[i]["CreUserName"].ToString();
                    string CrePassword = dt.Rows[i]["CrePassword"].ToString();

                    //string LoanAppPersonID = dt.Rows[i]["LoanAppPersonID"].ToString();
                    string LoanAppID = dt.Rows[i]["LoanAppID"].ToString();
                    string CustomerID = dt.Rows[i]["CustomerID"].ToString();
                    string SHORTNAME = dt.Rows[i]["SHORTNAME"].ToString();
                    string NAME1 = dt.Rows[i]["NAME1"].ToString();
                    string STREET = dt.Rows[i]["STREET"].ToString();
                    string RELATIONCODE = dt.Rows[i]["RELATIONCODE"].ToString();
                    string RELCUSTOMER = dt.Rows[i]["RELCUSTOMER"].ToString();
                    string SECTOR = dt.Rows[i]["SECTOR"].ToString();
                    string ACCOUNTOFFICER = dt.Rows[i]["ACCOUNTOFFICER"].ToString();
                    string INDUSTRY = dt.Rows[i]["INDUSTRY"].ToString();
                    string TARGET = dt.Rows[i]["TARGET"].ToString();
                    string NATIONALITY = dt.Rows[i]["NATIONALITY"].ToString();
                    string CUSTOMERSTATUS = dt.Rows[i]["CUSTOMERSTATUS"].ToString();
                    string RESIDENCE = dt.Rows[i]["RESIDENCE"].ToString();
                    string BIRTHINCORPDATE = dt.Rows[i]["BIRTHINCORPDATE"].ToString();
                    string COMPANYBOOK = dt.Rows[i]["COMPANYBOOK"].ToString();
                    string TITLE = dt.Rows[i]["TITLE"].ToString();
                    string GENDER = dt.Rows[i]["GENDER"].ToString();
                    string MARITALSTATUS = dt.Rows[i]["MARITALSTATUS"].ToString();
                    string OCCUPATION = dt.Rows[i]["OCCUPATION"].ToString();
                    string FURTHERDETAILS = dt.Rows[i]["FURTHERDETAILS"].ToString();
                    string PostalCode = dt.Rows[i]["PostalCode"].ToString();
                    if (PostalCode == "")
                    {
                        PostalCode = "";
                    }
                    else
                    {
                        PostalCode = "<cus:PostalCode>" + PostalCode + "</cus:PostalCode>";
                    }
                    string EmployersAddress = dt.Rows[i]["EmployersAddress"].ToString();
                    string HomePhoneNo = dt.Rows[i]["HomePhoneNo"].ToString();
                    string WorkPhoneNo = dt.Rows[i]["WorkPhoneNo"].ToString();
                    string MobilePhoneNo = dt.Rows[i]["MobilePhoneNo"].ToString();
                    string ResidenceYN = dt.Rows[i]["ResidenceYN"].ToString();
                    string EmailAddress = dt.Rows[i]["EmailAddress"].ToString();
                    string EmployerName = dt.Rows[i]["EmployerName"].ToString();
                    string TaxpayerRegistrationNo = dt.Rows[i]["TaxpayerRegistrationNo"].ToString();
                    string BlockedYN = dt.Rows[i]["BlockedYN"].ToString();
                    string AMKStaff = dt.Rows[i]["AMKStaff"].ToString();
                    string NoFamilyMember = dt.Rows[i]["NoFamilyMember"].ToString();
                    string EmployeeCode = dt.Rows[i]["EmployeeCode"].ToString();
                    string DateofEmployment = dt.Rows[i]["DateofEmployment"].ToString();
                    string FaxNo = dt.Rows[i]["FaxNo"].ToString();
                    string Profession = dt.Rows[i]["Profession"].ToString();
                    string EmploymentStatus = dt.Rows[i]["EmploymentStatus"].ToString();
                    string MainSourceofIncome = dt.Rows[i]["MainSourceofIncome"].ToString();
                    string EmployersCode = dt.Rows[i]["EmployersCode"].ToString();
                    string SpouseName = dt.Rows[i]["SpouseName"].ToString();
                    string SpouseOccupation = dt.Rows[i]["SpouseOccupation"].ToString();
                    string ConsenttoDisclosure = dt.Rows[i]["ConsenttoDisclosure"].ToString();
                    string DateofSignature = dt.Rows[i]["DateofSignature"].ToString();
                    string PlaceofStudy = dt.Rows[i]["PlaceofStudy"].ToString();
                    string DurationofCourse = dt.Rows[i]["DurationofCourse"].ToString();
                    string FieldofStudy = dt.Rows[i]["FieldofStudy"].ToString();
                    string NominationForm = dt.Rows[i]["NominationForm"].ToString();
                    string NominationCustomer = dt.Rows[i]["NominationCustomer"].ToString();
                    string NominationBeneficiary = dt.Rows[i]["NominationBeneficiary"].ToString();
                    string NominationAddress1 = dt.Rows[i]["NominationAddress1"].ToString();
                    string NominationAddress2 = dt.Rows[i]["NominationAddress2"].ToString();
                    string NominationAddress3 = dt.Rows[i]["NominationAddress3"].ToString();
                    string NominationAddress4 = dt.Rows[i]["NominationAddress4"].ToString();
                    string INSPostalCode = dt.Rows[i]["INSPostalCode"].ToString();
                    if (INSPostalCode == "")
                    {
                        INSPostalCode = "";
                    }
                    else {
                        INSPostalCode = "<cus:INSPostalCode>" + INSPostalCode + "</cus:INSPostalCode>";
                    }
                    string INSPhoneNo = dt.Rows[i]["INSPhoneNo"].ToString();
                    string RelationshiptoCustomer = dt.Rows[i]["RelationshiptoCustomer"].ToString();
                    string AmountorofLegacy = dt.Rows[i]["AmountorofLegacy"].ToString();
                    string NameofBusiness = dt.Rows[i]["NameofBusiness"].ToString();
                    string NatureofBusiness = dt.Rows[i]["NatureofBusiness"].ToString();
                    string BusinessType = dt.Rows[i]["BusinessType"].ToString();
                    string BusinessPlan = dt.Rows[i]["BusinessPlan"].ToString();
                    string RoleinBusiness = dt.Rows[i]["RoleinBusiness"].ToString();
                    string BusinessAddress = dt.Rows[i]["BusinessAddress"].ToString();
                    string BusinessAddress2 = dt.Rows[i]["BusinessAddress2"].ToString();
                    string BusinessAddress3 = dt.Rows[i]["BusinessAddress3"].ToString();
                    string BusinessAddress4 = dt.Rows[i]["BusinessAddress4"].ToString();
                    string TaxClearanceHeldYN = dt.Rows[i]["TaxClearanceHeldYN"].ToString();
                    string TaxClearanceExpiryDate = dt.Rows[i]["TaxClearanceExpiryDate"].ToString();
                    string Student = dt.Rows[i]["Student"].ToString();
                    string AMKStaffType = dt.Rows[i]["AMKStaffType"].ToString();
                    string SpouseCustomerNumber = dt.Rows[i]["SpouseCustomerNumber"].ToString();
                    string NoofEmployees = dt.Rows[i]["NoofEmployees"].ToString();
                    string ProvinceCity = dt.Rows[i]["ProvinceCity"].ToString();
                    string District = dt.Rows[i]["District"].ToString();
                    string Commune = dt.Rows[i]["Commune"].ToString();
                    string Village = dt.Rows[i]["Village"].ToString();
                    string SpouseNameinKhmer = dt.Rows[i]["SpouseNameinKhmer"].ToString();
                    string SurnameKhmer = dt.Rows[i]["SurnameKhmer"].ToString();
                    string FirstNameKhmer = dt.Rows[i]["FirstNameKhmer"].ToString();
                    string SpouseDateofBirth = dt.Rows[i]["SpouseDateofBirth"].ToString();
                    string SpouseIDType = dt.Rows[i]["SpouseIDType"].ToString();
                    string SpouseIDNumber = dt.Rows[i]["SpouseIDNumber"].ToString();
                    string SpouseIDIssueDate = dt.Rows[i]["SpouseIDIssueDate"].ToString();
                    string SpouseIDExpiryDate = dt.Rows[i]["SpouseIDExpiryDate"].ToString();
                    string NoActiveMember = dt.Rows[i]["NoActiveMember"].ToString();
                    string ProvertyStatus = dt.Rows[i]["ProvertyStatus"].ToString();
                    string VillageBank = dt.Rows[i]["VillageBank"].ToString();
                    if (VillageBank == "0")
                    {
                        VillageBank = "";
                    }
                    else {
                        VillageBank = "<cus:VillageBank>" + VillageBank + "</cus:VillageBank>";
                    }
                    string VillageBankPresident = dt.Rows[i]["VillageBankPresident"].ToString();
                    string MobileNumberType = dt.Rows[i]["MobileNumberType"].ToString();
                    string PlaceofBirth = dt.Rows[i]["PlaceofBirth"].ToString();
                    string BlacklistStatus = dt.Rows[i]["BlacklistStatus"].ToString();
                    string RiskStatus = dt.Rows[i]["RiskStatus"].ToString();
                    string BlackListCheckDate = dt.Rows[i]["BlackListCheckDate"].ToString();
                    string BlacklistCurrentStatus = dt.Rows[i]["BlacklistCurrentStatus"].ToString();
                    string HighRiskCateogries = dt.Rows[i]["HighRiskCateogries"].ToString();
                    string FATCAStatus = dt.Rows[i]["FATCAStatus"].ToString();
                    string FATCACountryofBirth = dt.Rows[i]["FATCACountryofBirth"].ToString();
                    string FATCANationality = dt.Rows[i]["FATCANationality"].ToString();
                    string FATCATaxIdentNumber = dt.Rows[i]["FATCATaxIdentNumber"].ToString();
                    string FATCAAddress = dt.Rows[i]["FATCAAddress"].ToString();
                    string FATCAPostalCode = dt.Rows[i]["FATCAPostalCode"].ToString();
                    string USTaxIdentificationNumber = dt.Rows[i]["USTaxIdentificationNumber"].ToString();
                    string FATCACountryCode = dt.Rows[i]["FATCACountryCode"].ToString();
                    string RecalcitrantStatus = dt.Rows[i]["RecalcitrantStatus"].ToString();
                    string OccupationType = dt.Rows[i]["OccupationType"].ToString();
                    string OccupationDetails = dt.Rows[i]["OccupationDetails"].ToString();
                    string IDType = dt.Rows[i]["IDType"].ToString();
                    string IDNumber = dt.Rows[i]["IDNumber"].ToString();
                    string IssueDate = dt.Rows[i]["IssueDate"].ToString();
                    string ExpiryDate = dt.Rows[i]["ExpiryDate"].ToString();
                    string Position = dt.Rows[i]["Position"].ToString();
                    string Department = dt.Rows[i]["Department"].ToString();
                    #endregion
                    #region Issue Date | ExpiryDateTag | SpouseIDIssueDate
                    string IssueDateTag = "";
                    if (IssueDate != "1900-01-01")
                    {
                        IssueDateTag = "<cus:IssueDate>" + IssueDate + "</cus:IssueDate>";
                    }
                    string ExpiryDateTag = "";
                    if (ExpiryDate != "1900-01-01")
                    {
                        ExpiryDateTag = "<cus:ExpiryDate>" + ExpiryDate + "</cus:ExpiryDate>";
                    }
                    string SpouseIDIssueDateTag = "";
                    if (SpouseIDIssueDate != "1900-01-01")
                    {
                        SpouseIDIssueDateTag = "<cus:SpouseIDIssueDate>" + SpouseIDIssueDate + "</cus:SpouseIDIssueDate>";
                    }
                    string SpouseIDExpiryDateTag = "";
                    if (SpouseIDExpiryDate != "1900-01-01")
                    {
                        SpouseIDExpiryDateTag = "<cus:SpouseIDExpiryDate>" + SpouseIDExpiryDate + "</cus:SpouseIDExpiryDate>";
                    }
                    #endregion Issue Date
                    #region CustomerID - Add/Edit
                    try
                    {
                        string isError = "", ErrXml="", Remark="";
                        string xmlStr = "";
                        int OneSingleTwoWithSpouce = 1;
                        DateTime dt_msgIDDate = DateTime.Now;
                        string msgIDDate = dt_msgIDDate.ToString("yyyy-MM-dd");
                        #region xml

                        if (MARITALSTATUS == "MARRIED")
                        {
                            if (LoanAppPersonTypeID == "32" || LoanAppPersonTypeID == "34")
                            {
                                //xml single
                                OneSingleTwoWithSpouce = 1;
                            }
                            else
                            {
                                //xml with spouse
                                OneSingleTwoWithSpouce = 2;
                            }
                        }
                        else {
                            OneSingleTwoWithSpouce = 1;
                        }
                        
                        if (OneSingleTwoWithSpouce == 1)
                        {
                            #region xml single
                            xmlStr = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:amk=\"http://temenos.com/AMKINDCUST\" xmlns:cus=\"http://temenos.com/CUSTOMERAMKINDIVWS\"><soapenv:Header/>"
                            + "<soapenv:Body><amk:INDIVCUSTOMERCREATION><WebRequestCommon><company>" + CreCompany + "</company><password>" + CrePassword + "</password>"
                            + "<userName>" + CreUserName + "</userName></WebRequestCommon>"
                            + "<OfsFunction>"
                            + "<messageId>" + "C-" + LoanAppPersonID + "-" + SHORTNAME +"-"+ msgIDDate + "</messageId>"
                            + "</OfsFunction>"
                            + "<CUSTOMERAMKINDIVWSType id=\"" + CustomerID + "\">"
                            + "<cus:gSHORTNAME g=\"1\"><cus:SHORTNAME>" + SHORTNAME + "</cus:SHORTNAME></cus:gSHORTNAME><cus:gNAME1 g=\"1\"><cus:NAME1>" + NAME1
                            + "</cus:NAME1></cus:gNAME1><cus:gSTREET g=\"1\"><cus:STREET>" + STREET + "</cus:STREET></cus:gSTREET><cus:gRELATIONCODE g=\"1\">"
                            + "<cus:mRELATIONCODE m=\"1\"><cus:RELATIONCODE>" + RELATIONCODE + "</cus:RELATIONCODE><cus:RELCUSTOMER>" + RELCUSTOMER
                            + "</cus:RELCUSTOMER></cus:mRELATIONCODE></cus:gRELATIONCODE><cus:SECTOR>" + SECTOR + "</cus:SECTOR><cus:ACCOUNTOFFICER>" + ACCOUNTOFFICER
                            + "</cus:ACCOUNTOFFICER><cus:INDUSTRY>" + INDUSTRY + "</cus:INDUSTRY><cus:TARGET>" + TARGET + "</cus:TARGET><cus:NATIONALITY>" + NATIONALITY
                            + "</cus:NATIONALITY><cus:CUSTOMERSTATUS>" + CUSTOMERSTATUS + "</cus:CUSTOMERSTATUS><cus:RESIDENCE>" + RESIDENCE
                            + "</cus:RESIDENCE><cus:BIRTHINCORPDATE>" + BIRTHINCORPDATE + "</cus:BIRTHINCORPDATE><cus:TITLE>" + TITLE + "</cus:TITLE><cus:GENDER>" + GENDER + "</cus:GENDER><cus:MARITALSTATUS>" + MARITALSTATUS
                            + "</cus:MARITALSTATUS><cus:OCCUPATION>" + OCCUPATION + "</cus:OCCUPATION><cus:gFURTHERDETAILS g=\"1\">"
                            + "<cus:FURTHERDETAILS>" + FURTHERDETAILS + "</cus:FURTHERDETAILS></cus:gFURTHERDETAILS>" + PostalCode + "<cus:gEMPLOY.BUS.ADDR g=\"1\"><cus:EmployersAddress>" + EmployersAddress
                            + "</cus:EmployersAddress></cus:gEMPLOY.BUS.ADDR><cus:HomePhoneNo>" + HomePhoneNo + "</cus:HomePhoneNo><cus:WorkPhoneNo>" + WorkPhoneNo
                            + "</cus:WorkPhoneNo><cus:MobilePhoneNo>" + MobilePhoneNo + "</cus:MobilePhoneNo><cus:ResidenceYN>" + ResidenceYN
                            + "</cus:ResidenceYN><cus:EmailAddress>" + EmailAddress + "</cus:EmailAddress><cus:EmployerName>" + EmployerName
                            + "</cus:EmployerName><cus:TaxpayerRegistrationNo>" + TaxpayerRegistrationNo + "</cus:TaxpayerRegistrationNo><cus:BlockedYN>" + BlockedYN
                            + "</cus:BlockedYN><cus:AMKStaff>" + AMKStaff + "</cus:AMKStaff><cus:NoFamilyMember>" + NoFamilyMember
                            + "</cus:NoFamilyMember><cus:EmployeeCode>" + EmployeeCode + "</cus:EmployeeCode>"
                            //+"<cus:DateofEmployment>" + DateofEmployment + "</cus:DateofEmployment>"
                            //+"<cus:FaxNo>" + FaxNo + "</cus:FaxNo>"
                            +"<cus:Profession>" + Profession + "</cus:Profession>"
                            //+"<cus:EmploymentStatus>" + EmploymentStatus + "</cus:EmploymentStatus>"
                            +"<cus:MainSourceofIncome>" + MainSourceofIncome + "</cus:MainSourceofIncome>"
                            //+"<cus:EmployersCode>" + EmployersCode + "</cus:EmployersCode>"
                            //+"<cus:SpouseName></cus:SpouseName>"
                            //+ "<cus:SpouseOccupation></cus:SpouseOccupation>"
                            +"<cus:ConsenttoDisclosure>" + ConsenttoDisclosure + "</cus:ConsenttoDisclosure>"
                            +"<cus:DateofSignature>" + DateofSignature + "</cus:DateofSignature>"

                            //+ "<cus:gPLACE.STUDY g=\"1\"><cus:mPLACE.STUDY m=\"1\">"
                            //+ "<cus:PlaceofStudy>" + PlaceofStudy + "</cus:PlaceofStudy>"
                            //+ "<cus:DurationofCourse>" + DurationofCourse + "</cus:DurationofCourse>"
                            //+ "<cus:FieldofStudy>" + FieldofStudy + "</cus:FieldofStudy>"
                            //+"</cus:mPLACE.STUDY></cus:gPLACE.STUDY>"

                            //+ "<cus:NominationForm>" + NominationForm + "</cus:NominationForm>"

                            //+"<cus:gINS.MEM.NO g=\"1\"><cus:mINS.MEM.NO m=\"1\">"
                            //+ "<cus:NominationCustomer>" + NominationCustomer + "</cus:NominationCustomer>"
                            //+"<cus:NominationBeneficiary>" + NominationBeneficiary + "</cus:NominationBeneficiary>"
                            //+"<cus:NominationAddress1>" + NominationAddress1 + "</cus:NominationAddress1>"
                            //+"<cus:NominationAddress2>" + NominationAddress2 + "</cus:NominationAddress2>"
                            //+"<cus:NominationAddress3>" + NominationAddress3 + "</cus:NominationAddress3>"
                            //+"<cus:NominationAddress4>" + NominationAddress4 + "</cus:NominationAddress4>"
                            //+INSPostalCode
                            //+"<cus:INSPhoneNo>" + INSPhoneNo + "</cus:INSPhoneNo>"
                            //+"<cus:RelationshiptoCustomer>" + RelationshiptoCustomer + "</cus:RelationshiptoCustomer>"
                            //+ "<cus:AmountorofLegacy>" + AmountorofLegacy + "</cus:AmountorofLegacy>"
                            //+"</cus:mINS.MEM.NO></cus:gINS.MEM.NO>"

                            //+"<cus:gBUS.NAME g=\"1\"><cus:mBUS.NAME m=\"1\">"
                            //+"<cus:NameofBusiness>" + NameofBusiness + "</cus:NameofBusiness>"
                            //+"<cus:NatureofBusiness>" + NatureofBusiness + "</cus:NatureofBusiness>"
                            //+"<cus:BusinessType>" + BusinessType + "</cus:BusinessType>"
                            //+"<cus:BusinessPlan>" + BusinessPlan + "</cus:BusinessPlan>"
                            //+"<cus:RoleinBusiness>" + RoleinBusiness + "</cus:RoleinBusiness>"
                            //+"<cus:BusinessAddress>" + BusinessAddress + "</cus:BusinessAddress>"
                            //+"<cus:BusinessAddress2>" + BusinessAddress2 + "</cus:BusinessAddress2>"
                            //+"<cus:BusinessAddress3>" + BusinessAddress3 + "</cus:BusinessAddress3>"
                            //+"<cus:BusinessAddress4>" + BusinessAddress4 + "</cus:BusinessAddress4>"
                            //+"<cus:TaxClearanceHeldYN>" + TaxClearanceHeldYN + "</cus:TaxClearanceHeldYN>"
                            //+"<cus:TaxClearanceExpiryDate>" + TaxClearanceExpiryDate + "</cus:TaxClearanceExpiryDate>"
                            //+"</cus:mBUS.NAME></cus:gBUS.NAME>"

                            //+"<cus:Student>" + Student + "</cus:Student>"
                            //+"<cus:AMKStaffType>" + AMKStaffType + "</cus:AMKStaffType>"
                            //+"<cus:SpouseCustomerNumber></cus:SpouseCustomerNumber>"
                            //+"<cus:NoofEmployees>" + NoofEmployees + "</cus:NoofEmployees>"
                            + "<cus:ProvinceCity>" + ProvinceCity + "</cus:ProvinceCity><cus:District>" + District
                            + "</cus:District><cus:Commune>" + Commune + "</cus:Commune><cus:Village>" + Village + "</cus:Village><cus:SurnameKhmer>" + SurnameKhmer
                            + "</cus:SurnameKhmer><cus:FirstNameKhmer>" + FirstNameKhmer + "</cus:FirstNameKhmer><cus:NoActiveMember>" + NoActiveMember
                            + "</cus:NoActiveMember><cus:ProvertyStatus>" + ProvertyStatus + "</cus:ProvertyStatus>"
                            + VillageBank
                            +"<cus:VillageBankPresident>" + VillageBankPresident + "</cus:VillageBankPresident><cus:MobileNumberType>" + MobileNumberType
                            + "</cus:MobileNumberType><cus:PlaceofBirth>" + PlaceofBirth + "</cus:PlaceofBirth>"

                            //+"<cus:BlacklistStatus>" + BlacklistStatus + "</cus:BlacklistStatus>"
                            //+"<cus:RiskStatus>" + RiskStatus + "</cus:RiskStatus>"
                            //+"<cus:BlackListCheckDate>" + BlackListCheckDate + "</cus:BlackListCheckDate>"
                            //+"<cus:BlacklistCurrentStatus>" + BlacklistCurrentStatus + "</cus:BlacklistCurrentStatus>"
                            //+ "<cus:HighRiskCateogries>" + HighRiskCateogries + "</cus:HighRiskCateogries>"
                            + "<cus:FATCAStatus>" + FATCAStatus + "</cus:FATCAStatus>"
                            + "<cus:FATCACountryofBirth>" + FATCACountryofBirth + "</cus:FATCACountryofBirth>"

                            + "<cus:gAMK.F.I.NAT g=\"1\"><cus:mAMK.F.I.NAT m=\"1\">"
                            + "<cus:FATCANationality>" + FATCANationality + "</cus:FATCANationality>"
                            + "<cus:FATCATaxIdentNumber>" + FATCATaxIdentNumber + "</cus:FATCATaxIdentNumber>"
                            + "</cus:mAMK.F.I.NAT></cus:gAMK.F.I.NAT>"

                            //+"<cus:gAMK.F.I.ADDRESS g=\"1\">"
                            //+"<cus:FATCAAddress>" + FATCAAddress + "</cus:FATCAAddress>"
                            //+"</cus:gAMK.F.I.ADDRESS>"

                            //+"<cus:FATCAPostalCode>" + FATCAPostalCode + "</cus:FATCAPostalCode>"
                            //+"<cus:USTaxIdentificationNumber>" + USTaxIdentificationNumber + "</cus:USTaxIdentificationNumber>"
                            //+ "<cus:FATCACountryCode>" + FATCACountryCode + "</cus:FATCACountryCode>"
                            //+"<cus:RecalcitrantStatus>" + RecalcitrantStatus + "</cus:RecalcitrantStatus>"
                            + "<cus:OccupationType>" + OccupationType + "</cus:OccupationType>"
                            + "<cus:OccupationDetails>" + OccupationDetails + "</cus:OccupationDetails>"

                            + "<cus:gAMK.ID.TYPE g=\"1\"><cus:mAMK.ID.TYPE m=\"1\">"
                            + "<cus:IDType>" + IDType + "</cus:IDType>"
                            + "<cus:IDNumber>" + IDNumber + "</cus:IDNumber>"
                            + IssueDateTag + ExpiryDateTag
                            + "</cus:mAMK.ID.TYPE></cus:gAMK.ID.TYPE>"

                            //+"<cus:Position>" + Position + "</cus:Position>"
                            //+"<cus:Department>" + Department + "</cus:Department>"
                            //+"<cus:gAMK.KYC.IMAGE g=\"1\"><cus:KYCImage></cus:KYCImage></cus:gAMK.KYC.IMAGE>"

                            + "</CUSTOMERAMKINDIVWSType></amk:INDIVCUSTOMERCREATION></soapenv:Body></soapenv:Envelope>";
                            #endregion xml single
                        }
                        else {
                            #region xml with spouse
                            xmlStr = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" "
                            + "xmlns:amk=\"http://temenos.com/AMKINDCUST\" xmlns:cus=\"http://temenos.com/CUSTOMERAMKINDIVWS\"><soapenv:Header/>"
                            + "<soapenv:Body><amk:INDIVCUSTOMERCREATION><WebRequestCommon><company>" + CreCompany + "</company><password>" + CrePassword + "</password>"
                            + "<userName>" + CreUserName + "</userName></WebRequestCommon>"
                            + "<OfsFunction>"
                            + "<messageId>" + "C-" + LoanAppPersonID + "-" + SHORTNAME + "-" + msgIDDate + "</messageId>"
                            + "</OfsFunction>"
                            + "<CUSTOMERAMKINDIVWSType id=\"" + CustomerID + "\">"
                            +"<cus:gSHORTNAME g=\"1\"><cus:SHORTNAME>" + SHORTNAME + "</cus:SHORTNAME></cus:gSHORTNAME>"
                            +"<cus:gNAME1 g=\"1\"><cus:NAME1>" + NAME1 + "</cus:NAME1></cus:gNAME1>"
                            +"<cus:gSTREET g=\"1\"><cus:STREET>" + STREET + "</cus:STREET></cus:gSTREET>"
                            +"<cus:gRELATIONCODE g=\"1\">"
                            + "<cus:mRELATIONCODE m=\"1\"><cus:RELATIONCODE>" + RELATIONCODE + "</cus:RELATIONCODE>"
                            +"<cus:RELCUSTOMER>" + RELCUSTOMER + "</cus:RELCUSTOMER>"
                            +"</cus:mRELATIONCODE></cus:gRELATIONCODE>"
                            +"<cus:SECTOR>" + SECTOR + "</cus:SECTOR>"
                            +"<cus:ACCOUNTOFFICER>" + ACCOUNTOFFICER + "</cus:ACCOUNTOFFICER>"
                            +"<cus:INDUSTRY>" + INDUSTRY + "</cus:INDUSTRY>"
                            +"<cus:TARGET>" + TARGET + "</cus:TARGET>"
                            +"<cus:NATIONALITY>" + NATIONALITY + "</cus:NATIONALITY>"
                            +"<cus:CUSTOMERSTATUS>" + CUSTOMERSTATUS + "</cus:CUSTOMERSTATUS>"
                            +"<cus:RESIDENCE>" + RESIDENCE + "</cus:RESIDENCE>"
                            +"<cus:BIRTHINCORPDATE>" + BIRTHINCORPDATE + "</cus:BIRTHINCORPDATE>"
                            +"<cus:TITLE>" + TITLE + "</cus:TITLE><cus:GENDER>" + GENDER + "</cus:GENDER>"
                            +"<cus:MARITALSTATUS>" + MARITALSTATUS + "</cus:MARITALSTATUS>"
                            //+"<cus:OCCUPATION>" + OCCUPATION + "</cus:OCCUPATION>"
                            //+"<cus:gFURTHERDETAILS g=\"1\">"
                            //+"<cus:FURTHERDETAILS>" + FURTHERDETAILS + "</cus:FURTHERDETAILS>"
                            //+"</cus:gFURTHERDETAILS>" 
                            + PostalCode 
                            //+ "<cus:gEMPLOY.BUS.ADDR g=\"1\">"
                            //+ "<cus:EmployersAddress>" + EmployersAddress + "</cus:EmployersAddress>"
                            //+"</cus:gEMPLOY.BUS.ADDR>"
                            //+"<cus:HomePhoneNo>" + HomePhoneNo + "</cus:HomePhoneNo>"
                            //+"<cus:WorkPhoneNo>" + WorkPhoneNo + "</cus:WorkPhoneNo>"
                            +"<cus:MobilePhoneNo>" + MobilePhoneNo + "</cus:MobilePhoneNo>"
                            +"<cus:ResidenceYN>" + ResidenceYN + "</cus:ResidenceYN>"
                            //+"<cus:EmailAddress>" + EmailAddress + "</cus:EmailAddress>"
                            //+"<cus:EmployerName>" + EmployerName + "</cus:EmployerName>"
                            //+"<cus:TaxpayerRegistrationNo>" + TaxpayerRegistrationNo + "</cus:TaxpayerRegistrationNo>"
                            +"<cus:BlockedYN>" + BlockedYN + "</cus:BlockedYN>"
                            +"<cus:AMKStaff>" + AMKStaff + "</cus:AMKStaff>"
                            +"<cus:NoFamilyMember>" + NoFamilyMember + "</cus:NoFamilyMember>"
                            //+"<cus:EmployeeCode>" + EmployeeCode + "</cus:EmployeeCode>"
                            //+"<cus:DateofEmployment>" + DateofEmployment + "</cus:DateofEmployment>"
                            //+"<cus:FaxNo>" + FaxNo + "</cus:FaxNo>"
                            +"<cus:Profession>" + Profession + "</cus:Profession>"
                            //+"<cus:EmploymentStatus>" + EmploymentStatus + "</cus:EmploymentStatus>"
                            +"<cus:MainSourceofIncome>" + MainSourceofIncome + "</cus:MainSourceofIncome>"
                            //+"<cus:EmployersCode>" + EmployersCode + "</cus:EmployersCode>"
                            +"<cus:SpouseName>" + SpouseName + "</cus:SpouseName>"
                            //+"<cus:SpouseOccupation>" + SpouseOccupation + "</cus:SpouseOccupation>"
                            +"<cus:ConsenttoDisclosure>" + ConsenttoDisclosure + "</cus:ConsenttoDisclosure>"
                            + "<cus:DateofSignature>" + DateofSignature + "</cus:DateofSignature>"

                            //+"<cus:gPLACE.STUDY g=\"1\"><cus:mPLACE.STUDY m=\"1\">"
                            //+"<cus:PlaceofStudy>" + PlaceofStudy + "</cus:PlaceofStudy>"
                            //+"<cus:DurationofCourse>" + DurationofCourse + "</cus:DurationofCourse>"
                            //+"<cus:FieldofStudy>" + FieldofStudy + "</cus:FieldofStudy>"
                            //+"</cus:mPLACE.STUDY></cus:gPLACE.STUDY>"

                            //+"<cus:NominationForm>" + NominationForm + "</cus:NominationForm>"

                            //+"<cus:gINS.MEM.NO g=\"1\"><cus:mINS.MEM.NO m=\"1\">"
                            //+"<cus:NominationCustomer>" + NominationCustomer + "</cus:NominationCustomer>"
                            //+"<cus:NominationBeneficiary>" + NominationBeneficiary + "</cus:NominationBeneficiary>"
                            //+"<cus:NominationAddress1>" + NominationAddress1 + "</cus:NominationAddress1>"
                            //+"<cus:NominationAddress2>" + NominationAddress2 + "</cus:NominationAddress2>"
                            //+"<cus:NominationAddress3>" + NominationAddress3 + "</cus:NominationAddress3>"
                            //+"<cus:NominationAddress4>" + NominationAddress4 + "</cus:NominationAddress4>"
                            //+ INSPostalCode 
                            //+ "<cus:INSPhoneNo>" + INSPhoneNo + "</cus:INSPhoneNo>"
                            //+"<cus:RelationshiptoCustomer>" + RelationshiptoCustomer + "</cus:RelationshiptoCustomer>"
                            //+"<cus:AmountorofLegacy>" + AmountorofLegacy + "</cus:AmountorofLegacy>"
                            //+"</cus:mINS.MEM.NO></cus:gINS.MEM.NO>"

                            //+"<cus:gBUS.NAME g=\"1\"><cus:mBUS.NAME m=\"1\">"
                            //+ "<cus:NameofBusiness>" + NameofBusiness + "</cus:NameofBusiness>"
                            //+"<cus:NatureofBusiness>" + NatureofBusiness + "</cus:NatureofBusiness>"
                            //+"<cus:BusinessType>" + BusinessType + "</cus:BusinessType>"
                            //+"<cus:BusinessPlan>" + BusinessPlan + "</cus:BusinessPlan>"
                            //+"<cus:RoleinBusiness>" + RoleinBusiness + "</cus:RoleinBusiness>"
                            //+"<cus:BusinessAddress>" + BusinessAddress + "</cus:BusinessAddress>"
                            //+"<cus:BusinessAddress2>" + BusinessAddress2 + "</cus:BusinessAddress2>"
                            //+"<cus:BusinessAddress3>" + BusinessAddress3 + "</cus:BusinessAddress3>"
                            //+"<cus:BusinessAddress4>" + BusinessAddress4 + "</cus:BusinessAddress4>"
                            //+"<cus:TaxClearanceHeldYN>" + TaxClearanceHeldYN + "</cus:TaxClearanceHeldYN>"
                            //+"<cus:TaxClearanceExpiryDate>" + TaxClearanceExpiryDate + "</cus:TaxClearanceExpiryDate>"
                            //+"</cus:mBUS.NAME></cus:gBUS.NAME>"

                            //+"<cus:Student>" + Student + "</cus:Student>"
                            //+"<cus:AMKStaffType>" + AMKStaffType + "</cus:AMKStaffType>"
                            + "<cus:SpouseCustomerNumber>" + SpouseCustomerNumber + "</cus:SpouseCustomerNumber>"
                            //+"<cus:NoofEmployees>" + NoofEmployees + "</cus:NoofEmployees>"
                            + "<cus:ProvinceCity>" + ProvinceCity + "</cus:ProvinceCity>"
                            + "<cus:District>" + District + "</cus:District>"
                            + "<cus:Commune>" + Commune + "</cus:Commune>"
                            + "<cus:Village>" + Village + "</cus:Village>"
                            + "<cus:SpouseNameinKhmer>" + SpouseNameinKhmer + "</cus:SpouseNameinKhmer>"
                            + "<cus:SurnameKhmer>" + SurnameKhmer + "</cus:SurnameKhmer>"
                            + "<cus:FirstNameKhmer>" + FirstNameKhmer + "</cus:FirstNameKhmer>"
                            + "<cus:SpouseDateofBirth>" + SpouseDateofBirth + "</cus:SpouseDateofBirth>"

                            + "<cus:gAMK.SP.ID.TYPE g=\"1\"><cus:mAMK.SP.ID.TYPE m=\"1\">"
                            + "<cus:SpouseIDType>" + SpouseIDType + "</cus:SpouseIDType>"
                            + "<cus:SpouseIDNumber>" + SpouseIDNumber + "</cus:SpouseIDNumber>"
                            + SpouseIDIssueDateTag
                            + SpouseIDExpiryDateTag
                            + "</cus:mAMK.SP.ID.TYPE></cus:gAMK.SP.ID.TYPE>"

                            + "<cus:NoActiveMember>" + NoActiveMember + "</cus:NoActiveMember>"
                            + "<cus:ProvertyStatus>" + ProvertyStatus + "</cus:ProvertyStatus>"
                            + VillageBank
                            + "<cus:VillageBankPresident>" + VillageBankPresident + "</cus:VillageBankPresident>"
                            + "<cus:MobileNumberType>" + MobileNumberType + "</cus:MobileNumberType>"
                            + "<cus:PlaceofBirth>" + PlaceofBirth + "</cus:PlaceofBirth>"
                            //+"<cus:BlacklistStatus>" + BlacklistStatus + "</cus:BlacklistStatus>"
                            //+"<cus:RiskStatus>" + RiskStatus + "</cus:RiskStatus>"
                            //+"<cus:BlackListCheckDate>" + BlackListCheckDate + "</cus:BlackListCheckDate>"
                            //+"<cus:BlacklistCurrentStatus>" + BlacklistCurrentStatus + "</cus:BlacklistCurrentStatus>"
                            //+ "<cus:HighRiskCateogries>" + HighRiskCateogries + "</cus:HighRiskCateogries>"
                            + "<cus:FATCAStatus>" + FATCAStatus + "</cus:FATCAStatus>"
                            + "<cus:FATCACountryofBirth>" + FATCACountryofBirth + "</cus:FATCACountryofBirth>"

                            + "<cus:gAMK.F.I.NAT g=\"1\"><cus:mAMK.F.I.NAT m=\"1\">"
                            + "<cus:FATCANationality>" + FATCANationality + "</cus:FATCANationality>"
                            + "<cus:FATCATaxIdentNumber>" + FATCATaxIdentNumber + "</cus:FATCATaxIdentNumber>"
                            + "</cus:mAMK.F.I.NAT></cus:gAMK.F.I.NAT>"

                            //+"<cus:gAMK.F.I.ADDRESS g=\"1\">"
                            //+"<cus:FATCAAddress>" + FATCAAddress + "</cus:FATCAAddress>"
                            //+"</cus:gAMK.F.I.ADDRESS>"

                            //+ FATCAPostalCode 
                            //+ "<cus:USTaxIdentificationNumber>" + USTaxIdentificationNumber + "</cus:USTaxIdentificationNumber>"
                            //+"<cus:FATCACountryCode>" + FATCACountryCode + "</cus:FATCACountryCode>"
                            //+"<cus:RecalcitrantStatus>" + RecalcitrantStatus + "</cus:RecalcitrantStatus>"

                            + "<cus:OccupationType>" + OccupationType + "</cus:OccupationType>"
                            + "<cus:OccupationDetails>" + OccupationDetails + "</cus:OccupationDetails>"

                            + "<cus:gAMK.ID.TYPE g=\"1\"><cus:mAMK.ID.TYPE m=\"1\">"
                            + "<cus:IDType>" + IDType + "</cus:IDType>"
                            + "<cus:IDNumber>" + IDNumber + "</cus:IDNumber>"
                            + IssueDateTag + ExpiryDateTag
                            + "</cus:mAMK.ID.TYPE></cus:gAMK.ID.TYPE>"

                            //+"<cus:Position>" + Position + "</cus:Position>"
                            //+"<cus:Department>" + Department + "</cus:Department>"
                            //+"<cus:gAMK.KYC.IMAGE g=\"1\"><cus:KYCImage></cus:KYCImage></cus:gAMK.KYC.IMAGE>"

                            + "</CUSTOMERAMKINDIVWSType>"
                            + "</amk:INDIVCUSTOMERCREATION></soapenv:Body></soapenv:Envelope>";
                            #endregion xml with spouse
                        }

                        #endregion xml
                        T24_AddLog(fileHeader, "AddEditCustomer_RQ", xmlStr, "LoanAdd");
                        #region hit to T24 create Customer
                        
                        var client = new RestClient(CreUrl);
                        client.Timeout = GetTabletWSRestTimeout();
                        var request = new RestRequest(Method.POST);
                        request.AddHeader("cache-control", "no-cache");
                        request.AddHeader("content-type", "text/xml");
                        request.AddParameter("text/xml", xmlStr, ParameterType.RequestBody);
                        IRestResponse response = null;
                        string xmlContent = "", resCode="";
                        try {
                            System.Threading.Thread.Sleep(10);
                            response = client.Execute(request);
                            //res code
                            resCode=response.StatusCode.ToString();
                            xmlContent = response.Content.ToString();
                        } catch(Exception ex) {
                            SMS = ex.Message.ToString();
                        }
                        
                        T24_AddLog(fileHeader, "AddEditCustomer_RS: "+ resCode, xmlContent, "LoanAdd");
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(xmlContent);
                        string successIndicator = doc.GetElementsByTagName("successIndicator").Item(0).InnerText;
                        string transactionId = doc.GetElementsByTagName("transactionId").Item(0).InnerText;
                        //update Switch tblLoanAppPerson2
                        if (successIndicator == "Success")
                        {                            
                            ReturnDT("update tblLoanAppPerson2 set PersonID='" + transactionId + "',CustomerID='" + transactionId 
                            + "',Number='" + transactionId + "' where LoanAppPersonID='" + LoanAppPersonID + "'");
                            ERR = transactionId;
                            isError = "0";
                            Remark = ERR;
                        }
                        else
                        {
                            string messages = doc.GetElementsByTagName("messages").Item(0).InnerText;
                            if (messages == "LIVE RECORD NOT CHANGED" 
                                || messages == "COMPANY.BOOK:1:1=NOCHANGE FIELD" || messages== "EB-LIVE.RECORD.NOT.CHANGED")
                            {
                                ERR = CustomerID;
                                isError = "0";
                                Remark = ERR;
                            }
                            else
                            {
                                //add error log
                                isError = "1";
                                ErrXml = xmlContent;
                                #region Error
                                if (xmlContent.Contains("<Status>"))
                                {
                                    int pFrom = xmlContent.IndexOf("<Status>");// + "<Status>".Length;
                                    int pTo = xmlContent.IndexOf("</Status>") + "</Status>".Length;
                                    string MSGxmlContent = xmlContent.Substring(pFrom, pTo - pFrom).Trim();
                                    LoanAppApproveToCBSError.Status obj = GenerateXmlObject<LoanAppApproveToCBSError.Status>(MSGxmlContent);
                                    string strMsg = "";
                                    for (int iMsg = 0; iMsg < obj.Messages.Count; iMsg++)
                                    {
                                        if (iMsg == 0)
                                        {
                                            strMsg = obj.Messages[iMsg];
                                        }
                                        else
                                        {
                                            strMsg = strMsg + " | " + obj.Messages[iMsg];
                                        }
                                    }
                                    SMS = strMsg;
                                }
                                #endregion Error
                            }
                        }

                        #region log
                        T24_LoanAppAddLogAdd(LoanAppID, LoanAppPersonID, fileHeader, isError, ErrXml, Remark, "0");                        
                        #endregion log

                        #endregion hit to T24 create Customer

                    }
                    catch (Exception ex)
                    {
                        //add error log
                        string line = GetLineNumber(ex).ToString();
                        SMS = ex.Message.ToString() + line;
                    }
                    #endregion CustomerID - Add/Edit
                }
            }
            catch (Exception ex)
            {
                ERR = "Error";
                SMS = ex.Message.ToString();
            }
            rs[0] = ERR;
            rs[1] = SMS;
            return rs;
        }
        public string[] AddLoan_bk(string fileHeader,string LoanAppID, string CUSTOMERID,string api_name)
        {
            string[] rs = new string[2];
            string isError = "", ErrXml = "";
            string ERR = "Error", SMS = "", xmlContent = "", CBSKey = "", CBSAcc = "", Remark = "Start", LoanAppStatusID = "12";
            try
            {
                fileHeader = fileHeader.Replace(" ", "_").Replace("-", "_").Replace(":", "_").Replace(".", "_");
                string sql = "exec T24_LoanAppListForOpenToCBS3 @LoanAppID='" + LoanAppID + "'";
                if (api_name == "AM") {
                    sql = "exec T24_LoanAppListForOpenToCBS3ForChangeDISBDate @LoanAppID='" + LoanAppID + "'";
                }
                DataTable dt = ReturnDT(sql);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    #region params
                    string CreUrl = dt.Rows[i]["CreUrl"].ToString();
                    string CreCompany = dt.Rows[i]["CreCompany"].ToString();
                    string CreUserName = dt.Rows[i]["CreUserName"].ToString();
                    string CrePassword = dt.Rows[i]["CrePassword"].ToString();

                    string APPDATE = dt.Rows[i]["APPDATE"].ToString();
                    string PRODUCTID = dt.Rows[i]["PRODUCTID"].ToString();
                    string CURRENCY = dt.Rows[i]["CURRENCY"].ToString();
                    string LOANTERM = dt.Rows[i]["LOANTERM"].ToString();
                    string AMOUNT = dt.Rows[i]["AMOUNT"].ToString();
                    string INTRATE = dt.Rows[i]["INTRATE"].ToString();
                    string REPAYSTDATE = dt.Rows[i]["REPAYSTDATE"].ToString();
                    string VILLAGEBANK = dt.Rows[i]["VILLAGEBANK"].ToString();
                    string LOANCYCLE = dt.Rows[i]["LOANCYCLE"].ToString();
                    string GROUPNO = dt.Rows[i]["GROUPNO"].ToString();
                    string OPERATION = dt.Rows[i]["OPERATION"].ToString();
                    string LOANREFERBY = dt.Rows[i]["LOANREFERBY"].ToString();
                    string CREDITOFFICER = dt.Rows[i]["CREDITOFFICER"].ToString();
                    string MAINBUSINESS = dt.Rows[i]["MAINBUSINESS"].ToString();
                    string SEMIBALLONFREQ = dt.Rows[i]["SEMIBALLONFREQ"].ToString();
                    //string LOANPURPOSE = dt.Rows[i]["LOANPURPOSE"].ToString();
                    string lp1 = dt.Rows[i]["lp1"].ToString();
                    string lp2 = dt.Rows[i]["lp2"].ToString();
                    string lp3 = dt.Rows[i]["lp3"].ToString();

                    string CBCREQUIRED = dt.Rows[i]["CBCREQUIRED"].ToString();
                    string GURANTORCODE = dt.Rows[i]["GURANTORCODE"].ToString();
                    //string COLLATERALCODE = dt.Rows[i]["COLLATERALCODE"].ToString();
                    //string COLLATERALCCY = dt.Rows[i]["COLLATERALCCY"].ToString();
                    //string COLLATERALTYPE = dt.Rows[i]["COLLATERALTYPE"].ToString();
                    //string PROPERTYTYPE = dt.Rows[i]["PROPERTYTYPE"].ToString();
                    //string COLLDOCTYPE = dt.Rows[i]["COLLDOCTYPE"].ToString();
                    //string COLLATERALQNTY = dt.Rows[i]["COLLATERALQNTY"].ToString();
                    //string COLLATERALUNIT = dt.Rows[i]["COLLATERALUNIT"].ToString();
                    //string COLLATERALPRICE = dt.Rows[i]["COLLATERALPRICE"].ToString();
                    //string COLLATERALVALUE = dt.Rows[i]["COLLATERALVALUE"].ToString();
                    //string TOTALCOLLATERAL = dt.Rows[i]["TOTALCOLLATERAL"].ToString();
                    //string COLLATERALLVR = dt.Rows[i]["COLLATERALLVR"].ToString();
                    string INCEXPCCY = dt.Rows[i]["INCEXPCCY"].ToString();
                    string TOTALINCOME = dt.Rows[i]["TOTALINCOME"].ToString();
                    string TOTALEXPENSE = dt.Rows[i]["TOTALEXPENSE"].ToString();
                    string AVGINCOME = dt.Rows[i]["AVGINCOME"].ToString();
                    string RESCHEDLOAN = dt.Rows[i]["RESCHEDLOAN"].ToString();

                    string UPFRONTFEE = dt.Rows[i]["UPFRONTFEE"].ToString();
                    string INCOMEFREQ = dt.Rows[i]["INCOMEFREQ"].ToString();
                    string BSNSEXPERIENCE = dt.Rows[i]["BSNSEXPERIENCE"].ToString();
                    string BSNSAGE = dt.Rows[i]["BSNSAGE"].ToString();
                    string MONTHLYTXNFEE = dt.Rows[i]["MONTHLYTXNFEE"].ToString();
                    string NOBSNSACT = dt.Rows[i]["NOBSNSACT"].ToString();
                    string COBRWCUSTOMER= dt.Rows[i]["COBRWCUSTOMER"].ToString();
                    #endregion params
                    #region xml
                    #region collateral
                    string COLLATERALCODE = "", COLLATERALCCY = "", COLLATERALTYPE = "", COLLDOCTYPE = "", COLLATERALQNTY = "", COLLATERALUNIT = "", COLLATERALPRICE = "";
                    string collStr = "";
                    try
                    {
                        DataTable dtColl = new DataTable();
                        dtColl = ReturnDT("exec T24_LoanAppListForOpenToCBS3_Collateral @LoanAppID='" + LoanAppID + "'");
                        for (int j = 0; j < dtColl.Rows.Count; j++)
                        {
                            COLLATERALCODE = dtColl.Rows[j]["COLLATERALCODE"].ToString();
                            COLLATERALCCY = dtColl.Rows[j]["COLLATERALCCY"].ToString();
                            COLLATERALTYPE = dtColl.Rows[j]["COLLATERALTYPE"].ToString();
                            COLLDOCTYPE = dtColl.Rows[j]["COLLDOCTYPE"].ToString();
                            COLLATERALQNTY = dtColl.Rows[j]["COLLATERALQNTY"].ToString();
                            COLLATERALUNIT = dtColl.Rows[j]["COLLATERALUNIT"].ToString();
                            COLLATERALPRICE = dtColl.Rows[j]["COLLATERALPRICE"].ToString();
                            collStr = collStr
                            + "<amk1:mCOLLATERALCODE m=\"1\"><!--Optional:-->"
                            + "<amk1:COLLATERALCODE>" + COLLATERALCODE + "</amk1:COLLATERALCODE><!--Optional:-->"
                            //+ "<amk1:COLLATERALCCY>" + COLLATERALCCY + "</amk1:COLLATERALCCY><!--Optional:-->"
                            + "<amk1:sgCOLLATERALTYPE sg=\"1\"><!--Zero or more repetitions:-->"
                            + "<amk1:COLLATERALTYPE s=\"1\"><!--Optional:-->"
                            + "<amk1:COLLATERALTYPE>" + COLLATERALTYPE + "</amk1:COLLATERALTYPE><!--Optional:-->"
                            + "<amk1:COLLDOCTYPE>" + COLLDOCTYPE + "</amk1:COLLDOCTYPE><!--Optional:-->"
                            + "<amk1:COLLATERALQNTY>" + COLLATERALQNTY + "</amk1:COLLATERALQNTY><!--Optional:-->"
                            + "<amk1:COLLATERALUNIT>" + COLLATERALUNIT + "</amk1:COLLATERALUNIT><!--Optional:-->"
                            + "<amk1:COLLATERALPRICE>" + COLLATERALPRICE + "</amk1:COLLATERALPRICE>"
                            + "</amk1:COLLATERALTYPE></amk1:sgCOLLATERALTYPE>"
                            + "</amk1:mCOLLATERALCODE>";
                        }
                    }
                    catch { }
                    #endregion collateral
                    #region lp
                    string LOANPURPOSE = "";
                    if (lp1.Trim() != "")
                    {
                        LOANPURPOSE = "<amk1:LOANPURPOSE>" + lp1 + "</amk1:LOANPURPOSE>";
                    }
                    if (lp2.Trim() != "")
                    {
                        LOANPURPOSE = LOANPURPOSE + "<amk1:LOANPURPOSE>" + lp2 + "</amk1:LOANPURPOSE>";
                    }
                    if (lp3.Trim() != "")
                    {
                        LOANPURPOSE = LOANPURPOSE + "<amk1:LOANPURPOSE>" + lp3 + "</amk1:LOANPURPOSE>";
                    }
                    #endregion lp
                    #region SemiBallon
                    string PRREPAYFREQStr = "";
                    if (SEMIBALLONFREQ != "") {
                        if (SEMIBALLONFREQ != "0")
                        {
                            PRREPAYFREQStr = "<amk1:PRREPAYFREQ>" + SEMIBALLONFREQ + "</amk1:PRREPAYFREQ>";
                        }
                    }
                    #endregion
                    #region xml new
                    string xmlStr = "<?xml version=\"1.0\"?><soapenv:Envelope xmlns:amk1=\"http://temenos.com/AMKDLAPPLICATIONBOINP\" "
                    + "xmlns:amk=\"http://temenos.com/AMKCREATELOAN\" xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\"><soapenv:Header/>"
                    + "<soapenv:Body><amk:CREATELOAN><!--Optional:--><WebRequestCommon><!--Optional:-->"
                    + "<company>" + CreCompany + "</company><password>" + CrePassword + "</password><userName>" + CreUserName + "</userName></WebRequestCommon><!--Optional:-->"
                    +"<OfsFunction>"
                    + "<messageId>" + "L-" + fileHeader + "</messageId>"
                    + "</OfsFunction>"
                    + "<!--Optional:--><AMKDLAPPLICATIONBOINPType id=\"\"><!--Optional:-->"
                    + "<amk1:APPDATE>" + APPDATE + "</amk1:APPDATE><!--Optional:-->"
                    + "<amk1:CUSTOMERID>" + CUSTOMERID + "</amk1:CUSTOMERID><!--Optional:-->"
                    + "<amk1:PRODUCTID>" + PRODUCTID + "</amk1:PRODUCTID><!--Optional:-->"
                    + "<amk1:CURRENCY>" + CURRENCY + "</amk1:CURRENCY><!--Optional:-->"
                    + "<amk1:LOANTERM>" + LOANTERM + "</amk1:LOANTERM><!--Optional:-->"
                    + "<amk1:AMOUNT>" + AMOUNT + "</amk1:AMOUNT>"
                    + "<amk1:INTRATE>" + INTRATE + "</amk1:INTRATE><!--Optional:-->"
                    + "<amk1:REPAYSTDATE>" + REPAYSTDATE + "</amk1:REPAYSTDATE><!--Optional:-->"
                    + "<amk1:LOANCYCLE>" + LOANCYCLE + "</amk1:LOANCYCLE><!--Optional:-->"
                    + "<amk1:GROUPNO>" + GROUPNO + "</amk1:GROUPNO><!--Optional:-->"
                    + "<amk1:OPERATION>" + OPERATION + "</amk1:OPERATION><!--Optional:-->"
                    + "<amk1:LOANREFERBY>" + LOANREFERBY + "</amk1:LOANREFERBY><!--Optional:-->"
                    + "<amk1:CREDITOFFICER>" + CREDITOFFICER + "</amk1:CREDITOFFICER><!--Optional:-->"
                    + "<amk1:gMAINBUSINESS g=\"1\"><amk1:MAINBUSINESS>" + MAINBUSINESS + "</amk1:MAINBUSINESS></amk1:gMAINBUSINESS>"
                    + "<amk1:gLOANPURPOSE g=\"1\">" + LOANPURPOSE + "</amk1:gLOANPURPOSE>"
                    + "<amk1:CBCREQUIRED>" + CBCREQUIRED + "</amk1:CBCREQUIRED><!--Optional:-->"
                    + "<amk1:gGURANTORCODE g=\"1\"><!--Zero or more repetitions:--><amk1:GURANTORCODE>" + GURANTORCODE + "</amk1:GURANTORCODE></amk1:gGURANTORCODE><!--Optional:-->"
                    + "<amk1:gCOBRWCUSTOMER g=\"1\"><!--Zero or more repetitions:--><amk1:COBRWCUSTOMER>" + COBRWCUSTOMER + "</amk1:COBRWCUSTOMER></amk1:gCOBRWCUSTOMER><!--Optional:-->"

                    + "<amk1:gCOLLATERALCODE g=\"1\"><!--Zero or more repetitions:-->"

                    + collStr

                    + "</amk1:gCOLLATERALCODE><!--Optional:-->"

                    + "<amk1:INCEXPCCY>" + INCEXPCCY + "</amk1:INCEXPCCY><!--Optional:-->"
                    + "<amk1:TOTALINCOME>" + TOTALINCOME + "</amk1:TOTALINCOME><!--Optional:-->"
                    + "<amk1:TOTALEXPENSE>" + TOTALEXPENSE + "</amk1:TOTALEXPENSE><!--Optional:-->"
                    + "<amk1:AVGINCOME>" + AVGINCOME + "</amk1:AVGINCOME>"
                    + "<amk1:RESCHEDLOAN>" + RESCHEDLOAN + "</amk1:RESCHEDLOAN>"
                    + PRREPAYFREQStr
                    + "<amk1:UPFRONTFEE>" + UPFRONTFEE + "</amk1:UPFRONTFEE><!--Optional:-->"
                    + "<amk1:INCOMEFREQ>" + INCOMEFREQ + "</amk1:INCOMEFREQ><!--Optional:-->"
                    + "<amk1:BSNSEXPERIENCE>" + BSNSEXPERIENCE + "</amk1:BSNSEXPERIENCE><!--Optional:-->"
                    + "<amk1:BSNSAGE>" + BSNSAGE + "</amk1:BSNSAGE><!--Optional:-->"
                    + "<amk1:MONTHLYTXNFEE>" + MONTHLYTXNFEE + "</amk1:MONTHLYTXNFEE>"
                    + "<amk1:NOBSNSACT>" + NOBSNSACT + "</amk1:NOBSNSACT>"
                    + "</AMKDLAPPLICATIONBOINPType></amk:CREATELOAN></soapenv:Body></soapenv:Envelope>";
                    #endregion xml new
                    #endregion xml
                    T24_AddLog(fileHeader, "AddLoan_RQ", xmlStr, "LoanAdd");
                    #region call to T24
                    var client = new RestClient(CreUrl);
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("cache-control", "no-cache");
                    request.AddHeader("content-type", "text/xml");
                    request.AddParameter("text/xml", xmlStr, ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    string resCode = response.StatusCode.ToString();
                    xmlContent = response.Content.ToString();                    
                    T24_AddLog(fileHeader, "AddLoan_RS: "+ resCode, xmlContent, "LoanAdd");
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xmlContent);
                    string successIndicator = doc.GetElementsByTagName("successIndicator").Item(0).InnerText;
                    string transactionId = doc.GetElementsByTagName("transactionId").Item(0).InnerText;
                    if (successIndicator == "Success")
                    {
                        CBSKey = transactionId;
                        CBSAcc = transactionId;
                        Remark = CBSKey;
                        LoanAppStatusID = "7";
                        ERR = "Success";
                        isError = "0";
                    }
                    else
                    {
                        //add error log
                        //Remark = xmlContent;
                        LoanAppStatusID = "12";
                        ERR = "Error";
                        isError= "1";
                        ErrXml = xmlContent;
                        #region Error
                        if (xmlContent.Contains("<Status>"))
                        {
                            int pFrom = xmlContent.IndexOf("<Status>");// + "<Status>".Length;
                            int pTo = xmlContent.IndexOf("</Status>") + "</Status>".Length;
                            string MSGxmlContent = xmlContent.Substring(pFrom, pTo - pFrom).Trim();
                            LoanAppApproveToCBSError.Status obj = GenerateXmlObject<LoanAppApproveToCBSError.Status>(MSGxmlContent);
                            string strMsg = "";
                            for (int iMsg = 0; iMsg < obj.Messages.Count; iMsg++)
                            {
                                if (iMsg == 0)
                                {
                                    strMsg = obj.Messages[iMsg];
                                }
                                else
                                {
                                    strMsg = strMsg + " | " + obj.Messages[iMsg];
                                }
                            }
                            SMS = strMsg;
                            Remark = SMS;
                        }
                        #endregion Error
                    }
                    #endregion call to T24

                    #region log
                    T24_LoanAppAddLogAdd(LoanAppID,"0", fileHeader, isError, ErrXml, Remark, "0");
                    #endregion log

                }

            }
            catch (Exception ex)
            {
                //add error log
                Remark = ex.Message.ToString();
                LoanAppStatusID = "12";
                ERR = "Error";
            }
            #region add log
            UpdateLoanAppStatus(LoanAppID, CBSKey, CBSAcc, "Error AddLoan: "+Remark, LoanAppStatusID);
            #endregion add log
            rs[0] = ERR;
            rs[1] = SMS;
            return rs;
        }

        #region New Function to create customer and loan

        public string[] AddEditCustomer(string fileHeader, string LoanAppPersonID, string LoanAppPersonTypeID)
        {
            string[] rs = new string[2];
            string ERR = "Error", SMS = "", rsContent = "", xmlContent = "";
            try
            {
                fileHeader = fileHeader.Replace(" ", "_").Replace("-", "_").Replace(":", "_").Replace(".", "_");
                string sql = "exec T24_LoanAppListForOpenToCBS2 @LoanAppPersonID='" + LoanAppPersonID + "'";
                DataTable dt = ReturnDT(sql);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    #region params
                    string CreUrl = dt.Rows[i]["CreUrl"].ToString();
                    string CreCompany = dt.Rows[i]["CreCompany"].ToString();
                    string CreUserName = dt.Rows[i]["CreUserName"].ToString();
                    string CrePassword = dt.Rows[i]["CrePassword"].ToString();

                    //string LoanAppPersonID = dt.Rows[i]["LoanAppPersonID"].ToString();
                    string LoanAppID = dt.Rows[i]["LoanAppID"].ToString();
                    string CustomerID = dt.Rows[i]["CustomerID"].ToString();
                    string SHORTNAME = dt.Rows[i]["SHORTNAME"].ToString();
                    string NAME1 = dt.Rows[i]["NAME1"].ToString();
                    string STREET = dt.Rows[i]["STREET"].ToString();
                    string RELATIONCODE = dt.Rows[i]["RELATIONCODE"].ToString();
                    string RELCUSTOMER = dt.Rows[i]["RELCUSTOMER"].ToString();
                    string SECTOR = dt.Rows[i]["SECTOR"].ToString();
                    string ACCOUNTOFFICER = dt.Rows[i]["ACCOUNTOFFICER"].ToString();
                    string INDUSTRY = dt.Rows[i]["INDUSTRY"].ToString();
                    string TARGET = dt.Rows[i]["TARGET"].ToString();
                    string NATIONALITY = dt.Rows[i]["NATIONALITY"].ToString();
                    string CUSTOMERSTATUS = dt.Rows[i]["CUSTOMERSTATUS"].ToString();
                    string RESIDENCE = dt.Rows[i]["RESIDENCE"].ToString();
                    string BIRTHINCORPDATE = dt.Rows[i]["BIRTHINCORPDATE"].ToString();
                    string COMPANYBOOK = dt.Rows[i]["COMPANYBOOK"].ToString();
                    string TITLE = dt.Rows[i]["TITLE"].ToString();
                    string GENDER = dt.Rows[i]["GENDER"].ToString();
                    string MARITALSTATUS = dt.Rows[i]["MARITALSTATUS"].ToString();
                    string OCCUPATION = dt.Rows[i]["OCCUPATION"].ToString();
                    string FURTHERDETAILS = dt.Rows[i]["FURTHERDETAILS"].ToString();
                    string PostalCode = dt.Rows[i]["PostalCode"].ToString();
                    if (PostalCode == "")
                    {
                        PostalCode = "";
                    }
                    else
                    {
                        PostalCode = "<cus:PostalCode>" + PostalCode + "</cus:PostalCode>";
                    }
                    string EmployersAddress = dt.Rows[i]["EmployersAddress"].ToString();
                    string HomePhoneNo = dt.Rows[i]["HomePhoneNo"].ToString();
                    string WorkPhoneNo = dt.Rows[i]["WorkPhoneNo"].ToString();
                    string MobilePhoneNo = dt.Rows[i]["MobilePhoneNo"].ToString();
                    string ResidenceYN = dt.Rows[i]["ResidenceYN"].ToString();
                    string EmailAddress = dt.Rows[i]["EmailAddress"].ToString();
                    string EmployerName = dt.Rows[i]["EmployerName"].ToString();
                    string TaxpayerRegistrationNo = dt.Rows[i]["TaxpayerRegistrationNo"].ToString();
                    string BlockedYN = dt.Rows[i]["BlockedYN"].ToString();
                    string AMKStaff = dt.Rows[i]["AMKStaff"].ToString();
                    string NoFamilyMember = dt.Rows[i]["NoFamilyMember"].ToString();
                    string EmployeeCode = dt.Rows[i]["EmployeeCode"].ToString();
                    string DateofEmployment = dt.Rows[i]["DateofEmployment"].ToString();
                    string FaxNo = dt.Rows[i]["FaxNo"].ToString();
                    string Profession = dt.Rows[i]["Profession"].ToString();
                    string EmploymentStatus = dt.Rows[i]["EmploymentStatus"].ToString();
                    string MainSourceofIncome = dt.Rows[i]["MainSourceofIncome"].ToString();
                    string EmployersCode = dt.Rows[i]["EmployersCode"].ToString();
                    string SpouseName = dt.Rows[i]["SpouseName"].ToString();
                    string SpouseOccupation = dt.Rows[i]["SpouseOccupation"].ToString();
                    string ConsenttoDisclosure = dt.Rows[i]["ConsenttoDisclosure"].ToString();
                    string DateofSignature = dt.Rows[i]["DateofSignature"].ToString();
                    string PlaceofStudy = dt.Rows[i]["PlaceofStudy"].ToString();
                    string DurationofCourse = dt.Rows[i]["DurationofCourse"].ToString();
                    string FieldofStudy = dt.Rows[i]["FieldofStudy"].ToString();
                    string NominationForm = dt.Rows[i]["NominationForm"].ToString();
                    string NominationCustomer = dt.Rows[i]["NominationCustomer"].ToString();
                    string NominationBeneficiary = dt.Rows[i]["NominationBeneficiary"].ToString();
                    string NominationAddress1 = dt.Rows[i]["NominationAddress1"].ToString();
                    string NominationAddress2 = dt.Rows[i]["NominationAddress2"].ToString();
                    string NominationAddress3 = dt.Rows[i]["NominationAddress3"].ToString();
                    string NominationAddress4 = dt.Rows[i]["NominationAddress4"].ToString();
                    string INSPostalCode = dt.Rows[i]["INSPostalCode"].ToString();
                    if (INSPostalCode == "")
                    {
                        INSPostalCode = "";
                    }
                    else
                    {
                        INSPostalCode = "<cus:INSPostalCode>" + INSPostalCode + "</cus:INSPostalCode>";
                    }
                    string INSPhoneNo = dt.Rows[i]["INSPhoneNo"].ToString();
                    string RelationshiptoCustomer = dt.Rows[i]["RelationshiptoCustomer"].ToString();
                    string AmountorofLegacy = dt.Rows[i]["AmountorofLegacy"].ToString();
                    string NameofBusiness = dt.Rows[i]["NameofBusiness"].ToString();
                    string NatureofBusiness = dt.Rows[i]["NatureofBusiness"].ToString();
                    string BusinessType = dt.Rows[i]["BusinessType"].ToString();
                    string BusinessPlan = dt.Rows[i]["BusinessPlan"].ToString();
                    string RoleinBusiness = dt.Rows[i]["RoleinBusiness"].ToString();
                    string BusinessAddress = dt.Rows[i]["BusinessAddress"].ToString();
                    string BusinessAddress2 = dt.Rows[i]["BusinessAddress2"].ToString();
                    string BusinessAddress3 = dt.Rows[i]["BusinessAddress3"].ToString();
                    string BusinessAddress4 = dt.Rows[i]["BusinessAddress4"].ToString();
                    string TaxClearanceHeldYN = dt.Rows[i]["TaxClearanceHeldYN"].ToString();
                    string TaxClearanceExpiryDate = dt.Rows[i]["TaxClearanceExpiryDate"].ToString();
                    string Student = dt.Rows[i]["Student"].ToString();
                    string AMKStaffType = dt.Rows[i]["AMKStaffType"].ToString();
                    string SpouseCustomerNumber = dt.Rows[i]["SpouseCustomerNumber"].ToString();
                    string NoofEmployees = dt.Rows[i]["NoofEmployees"].ToString();
                    string ProvinceCity = dt.Rows[i]["ProvinceCity"].ToString();
                    string District = dt.Rows[i]["District"].ToString();
                    string Commune = dt.Rows[i]["Commune"].ToString();
                    string Village = dt.Rows[i]["Village"].ToString();
                    string SpouseNameinKhmer = dt.Rows[i]["SpouseNameinKhmer"].ToString();
                    string SurnameKhmer = dt.Rows[i]["SurnameKhmer"].ToString();
                    string FirstNameKhmer = dt.Rows[i]["FirstNameKhmer"].ToString();
                    string SpouseDateofBirth = dt.Rows[i]["SpouseDateofBirth"].ToString();
                    string SpouseIDType = dt.Rows[i]["SpouseIDType"].ToString();
                    string SpouseIDNumber = dt.Rows[i]["SpouseIDNumber"].ToString();
                    string SpouseIDIssueDate = dt.Rows[i]["SpouseIDIssueDate"].ToString();
                    string SpouseIDExpiryDate = dt.Rows[i]["SpouseIDExpiryDate"].ToString();
                    string NoActiveMember = dt.Rows[i]["NoActiveMember"].ToString();
                    string ProvertyStatus = dt.Rows[i]["ProvertyStatus"].ToString();
                    string VillageBank = dt.Rows[i]["VillageBank"].ToString();
                    if (VillageBank == "0")
                    {
                        VillageBank = "";
                    }
                    else
                    {
                        VillageBank = "<cus:VillageBank>" + VillageBank + "</cus:VillageBank>";
                    }
                    string VillageBankPresident = dt.Rows[i]["VillageBankPresident"].ToString();
                    string MobileNumberType = dt.Rows[i]["MobileNumberType"].ToString();
                    string PlaceofBirth = dt.Rows[i]["PlaceofBirth"].ToString();
                    string BlacklistStatus = dt.Rows[i]["BlacklistStatus"].ToString();
                    string RiskStatus = dt.Rows[i]["RiskStatus"].ToString();
                    string BlackListCheckDate = dt.Rows[i]["BlackListCheckDate"].ToString();
                    string BlacklistCurrentStatus = dt.Rows[i]["BlacklistCurrentStatus"].ToString();
                    string HighRiskCateogries = dt.Rows[i]["HighRiskCateogries"].ToString();
                    string FATCAStatus = dt.Rows[i]["FATCAStatus"].ToString();
                    string FATCACountryofBirth = dt.Rows[i]["FATCACountryofBirth"].ToString();
                    string FATCANationality = dt.Rows[i]["FATCANationality"].ToString();
                    string FATCATaxIdentNumber = dt.Rows[i]["FATCATaxIdentNumber"].ToString();
                    string FATCAAddress = dt.Rows[i]["FATCAAddress"].ToString();
                    string FATCAPostalCode = dt.Rows[i]["FATCAPostalCode"].ToString();
                    string USTaxIdentificationNumber = dt.Rows[i]["USTaxIdentificationNumber"].ToString();
                    string FATCACountryCode = dt.Rows[i]["FATCACountryCode"].ToString();
                    string RecalcitrantStatus = dt.Rows[i]["RecalcitrantStatus"].ToString();
                    string OccupationType = dt.Rows[i]["OccupationType"].ToString();
                    string OccupationDetails = dt.Rows[i]["OccupationDetails"].ToString();
                    string IDType = dt.Rows[i]["IDType"].ToString();
                    string IDNumber = dt.Rows[i]["IDNumber"].ToString();
                    string IssueDate = dt.Rows[i]["IssueDate"].ToString();
                    string ExpiryDate = dt.Rows[i]["ExpiryDate"].ToString();
                    string Position = dt.Rows[i]["Position"].ToString();
                    string Department = dt.Rows[i]["Department"].ToString();
                    #endregion
                    #region Issue Date | ExpiryDateTag | SpouseIDIssueDate
                    string IssueDateTag = "";
                    if (IssueDate != "1900-01-01")
                    {
                        IssueDateTag = "<cus:IssueDate>" + IssueDate + "</cus:IssueDate>";
                    }
                    string ExpiryDateTag = "";
                    if (ExpiryDate != "1900-01-01")
                    {
                        ExpiryDateTag = "<cus:ExpiryDate>" + ExpiryDate + "</cus:ExpiryDate>";
                    }
                    string SpouseIDIssueDateTag = "";
                    if (SpouseIDIssueDate != "1900-01-01")
                    {
                        SpouseIDIssueDateTag = "<cus:SpouseIDIssueDate>" + SpouseIDIssueDate + "</cus:SpouseIDIssueDate>";
                    }
                    string SpouseIDExpiryDateTag = "";
                    if (SpouseIDExpiryDate != "1900-01-01")
                    {
                        SpouseIDExpiryDateTag = "<cus:SpouseIDExpiryDate>" + SpouseIDExpiryDate + "</cus:SpouseIDExpiryDate>";
                    }
                    #endregion Issue Date
                    #region CustomerID - Add/Edit
                    try
                    {
                        string isError = "", ErrXml = "", Remark = "";
                        string xmlStr = "";
                        int OneSingleTwoWithSpouce = 1;
                        DateTime dt_msgIDDate = DateTime.Now;
                        string msgIDDate = dt_msgIDDate.ToString("yyyy-MM-dd");
                        #region xml
                        if (MARITALSTATUS == "SINGLE")
                        {
                            //xml single
                            OneSingleTwoWithSpouce = 1;
                        }
                        else
                        {
                            if (LoanAppPersonTypeID == "32" || LoanAppPersonTypeID == "34")
                            {
                                //xml single
                                OneSingleTwoWithSpouce = 1;
                            }
                            else
                            {
                                //xml with spouse
                                OneSingleTwoWithSpouce = 2;
                            }
                        }
                        if (OneSingleTwoWithSpouce == 1)
                        {
                            #region xml single
                            xmlStr = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:amk=\"http://temenos.com/AMKINDCUST\" xmlns:cus=\"http://temenos.com/CUSTOMERAMKINDIVWS\"><soapenv:Header/>"
                            + "<soapenv:Body><amk:INDIVCUSTOMERCREATION><WebRequestCommon><company>" + CreCompany + "</company><password>" + CrePassword + "</password>"
                            + "<userName>" + CreUserName + "</userName></WebRequestCommon>"
                            + "<OfsFunction>"
                            + "<messageId>" + "C-" + LoanAppPersonID + "-" + SHORTNAME + "-" + msgIDDate + "</messageId>"
                            + "</OfsFunction>"
                            + "<CUSTOMERAMKINDIVWSType id=\"" + CustomerID + "\">"
                            + "<cus:gSHORTNAME g=\"1\"><cus:SHORTNAME>" + SHORTNAME + "</cus:SHORTNAME></cus:gSHORTNAME><cus:gNAME1 g=\"1\"><cus:NAME1>" + NAME1
                            + "</cus:NAME1></cus:gNAME1><cus:gSTREET g=\"1\"><cus:STREET>" + STREET + "</cus:STREET></cus:gSTREET><cus:gRELATIONCODE g=\"1\">"
                            + "<cus:mRELATIONCODE m=\"1\"><cus:RELATIONCODE>" + RELATIONCODE + "</cus:RELATIONCODE><cus:RELCUSTOMER>" + RELCUSTOMER
                            + "</cus:RELCUSTOMER></cus:mRELATIONCODE></cus:gRELATIONCODE><cus:SECTOR>" + SECTOR + "</cus:SECTOR><cus:ACCOUNTOFFICER>" + ACCOUNTOFFICER
                            + "</cus:ACCOUNTOFFICER><cus:INDUSTRY>" + INDUSTRY + "</cus:INDUSTRY><cus:TARGET>" + TARGET + "</cus:TARGET><cus:NATIONALITY>" + NATIONALITY
                            + "</cus:NATIONALITY><cus:CUSTOMERSTATUS>" + CUSTOMERSTATUS + "</cus:CUSTOMERSTATUS><cus:RESIDENCE>" + RESIDENCE
                            + "</cus:RESIDENCE><cus:BIRTHINCORPDATE>" + BIRTHINCORPDATE + "</cus:BIRTHINCORPDATE><cus:TITLE>" + TITLE + "</cus:TITLE><cus:GENDER>" + GENDER + "</cus:GENDER><cus:MARITALSTATUS>" + MARITALSTATUS
                            + "</cus:MARITALSTATUS><cus:OCCUPATION>" + OCCUPATION + "</cus:OCCUPATION><cus:gFURTHERDETAILS g=\"1\">"
                            + "<cus:FURTHERDETAILS>" + FURTHERDETAILS + "</cus:FURTHERDETAILS></cus:gFURTHERDETAILS>" + PostalCode + "<cus:gEMPLOY.BUS.ADDR g=\"1\"><cus:EmployersAddress>" + EmployersAddress
                            + "</cus:EmployersAddress></cus:gEMPLOY.BUS.ADDR><cus:HomePhoneNo>" + HomePhoneNo + "</cus:HomePhoneNo><cus:WorkPhoneNo>" + WorkPhoneNo
                            + "</cus:WorkPhoneNo><cus:MobilePhoneNo>" + MobilePhoneNo + "</cus:MobilePhoneNo><cus:ResidenceYN>" + ResidenceYN
                            + "</cus:ResidenceYN><cus:EmailAddress>" + EmailAddress + "</cus:EmailAddress><cus:EmployerName>" + EmployerName
                            + "</cus:EmployerName><cus:TaxpayerRegistrationNo>" + TaxpayerRegistrationNo + "</cus:TaxpayerRegistrationNo><cus:BlockedYN>" + BlockedYN
                            + "</cus:BlockedYN><cus:AMKStaff>" + AMKStaff + "</cus:AMKStaff><cus:NoFamilyMember>" + NoFamilyMember
                            + "</cus:NoFamilyMember><cus:EmployeeCode>" + EmployeeCode + "</cus:EmployeeCode>"
                            //+"<cus:DateofEmployment>" + DateofEmployment + "</cus:DateofEmployment>"
                            //+"<cus:FaxNo>" + FaxNo + "</cus:FaxNo>"
                            + "<cus:Profession>" + Profession + "</cus:Profession>"
                            //+"<cus:EmploymentStatus>" + EmploymentStatus + "</cus:EmploymentStatus>"
                            + "<cus:MainSourceofIncome>" + MainSourceofIncome + "</cus:MainSourceofIncome>"
                            //+"<cus:EmployersCode>" + EmployersCode + "</cus:EmployersCode>"
                            //+"<cus:SpouseName></cus:SpouseName>"
                            //+ "<cus:SpouseOccupation></cus:SpouseOccupation>"
                            + "<cus:ConsenttoDisclosure>" + ConsenttoDisclosure + "</cus:ConsenttoDisclosure>"
                            + "<cus:DateofSignature>" + DateofSignature + "</cus:DateofSignature>"

                            //+ "<cus:gPLACE.STUDY g=\"1\"><cus:mPLACE.STUDY m=\"1\">"
                            //+ "<cus:PlaceofStudy>" + PlaceofStudy + "</cus:PlaceofStudy>"
                            //+ "<cus:DurationofCourse>" + DurationofCourse + "</cus:DurationofCourse>"
                            //+ "<cus:FieldofStudy>" + FieldofStudy + "</cus:FieldofStudy>"
                            //+"</cus:mPLACE.STUDY></cus:gPLACE.STUDY>"

                            //+ "<cus:NominationForm>" + NominationForm + "</cus:NominationForm>"

                            //+"<cus:gINS.MEM.NO g=\"1\"><cus:mINS.MEM.NO m=\"1\">"
                            //+ "<cus:NominationCustomer>" + NominationCustomer + "</cus:NominationCustomer>"
                            //+"<cus:NominationBeneficiary>" + NominationBeneficiary + "</cus:NominationBeneficiary>"
                            //+"<cus:NominationAddress1>" + NominationAddress1 + "</cus:NominationAddress1>"
                            //+"<cus:NominationAddress2>" + NominationAddress2 + "</cus:NominationAddress2>"
                            //+"<cus:NominationAddress3>" + NominationAddress3 + "</cus:NominationAddress3>"
                            //+"<cus:NominationAddress4>" + NominationAddress4 + "</cus:NominationAddress4>"
                            //+INSPostalCode
                            //+"<cus:INSPhoneNo>" + INSPhoneNo + "</cus:INSPhoneNo>"
                            //+"<cus:RelationshiptoCustomer>" + RelationshiptoCustomer + "</cus:RelationshiptoCustomer>"
                            //+ "<cus:AmountorofLegacy>" + AmountorofLegacy + "</cus:AmountorofLegacy>"
                            //+"</cus:mINS.MEM.NO></cus:gINS.MEM.NO>"

                            //+"<cus:gBUS.NAME g=\"1\"><cus:mBUS.NAME m=\"1\">"
                            //+"<cus:NameofBusiness>" + NameofBusiness + "</cus:NameofBusiness>"
                            //+"<cus:NatureofBusiness>" + NatureofBusiness + "</cus:NatureofBusiness>"
                            //+"<cus:BusinessType>" + BusinessType + "</cus:BusinessType>"
                            //+"<cus:BusinessPlan>" + BusinessPlan + "</cus:BusinessPlan>"
                            //+"<cus:RoleinBusiness>" + RoleinBusiness + "</cus:RoleinBusiness>"
                            //+"<cus:BusinessAddress>" + BusinessAddress + "</cus:BusinessAddress>"
                            //+"<cus:BusinessAddress2>" + BusinessAddress2 + "</cus:BusinessAddress2>"
                            //+"<cus:BusinessAddress3>" + BusinessAddress3 + "</cus:BusinessAddress3>"
                            //+"<cus:BusinessAddress4>" + BusinessAddress4 + "</cus:BusinessAddress4>"
                            //+"<cus:TaxClearanceHeldYN>" + TaxClearanceHeldYN + "</cus:TaxClearanceHeldYN>"
                            //+"<cus:TaxClearanceExpiryDate>" + TaxClearanceExpiryDate + "</cus:TaxClearanceExpiryDate>"
                            //+"</cus:mBUS.NAME></cus:gBUS.NAME>"

                            //+"<cus:Student>" + Student + "</cus:Student>"
                            //+"<cus:AMKStaffType>" + AMKStaffType + "</cus:AMKStaffType>"
                            //+"<cus:SpouseCustomerNumber></cus:SpouseCustomerNumber>"
                            //+"<cus:NoofEmployees>" + NoofEmployees + "</cus:NoofEmployees>"
                            + "<cus:ProvinceCity>" + ProvinceCity + "</cus:ProvinceCity><cus:District>" + District
                            + "</cus:District><cus:Commune>" + Commune + "</cus:Commune><cus:Village>" + Village + "</cus:Village><cus:SurnameKhmer>" + SurnameKhmer
                            + "</cus:SurnameKhmer><cus:FirstNameKhmer>" + FirstNameKhmer + "</cus:FirstNameKhmer><cus:NoActiveMember>" + NoActiveMember
                            + "</cus:NoActiveMember><cus:ProvertyStatus>" + ProvertyStatus + "</cus:ProvertyStatus>"
                            + VillageBank
                            + "<cus:VillageBankPresident>" + VillageBankPresident + "</cus:VillageBankPresident><cus:MobileNumberType>" + MobileNumberType
                            + "</cus:MobileNumberType><cus:PlaceofBirth>" + PlaceofBirth + "</cus:PlaceofBirth>"

                            //+"<cus:BlacklistStatus>" + BlacklistStatus + "</cus:BlacklistStatus>"
                            //+"<cus:RiskStatus>" + RiskStatus + "</cus:RiskStatus>"
                            //+"<cus:BlackListCheckDate>" + BlackListCheckDate + "</cus:BlackListCheckDate>"
                            //+"<cus:BlacklistCurrentStatus>" + BlacklistCurrentStatus + "</cus:BlacklistCurrentStatus>"
                            //+ "<cus:HighRiskCateogries>" + HighRiskCateogries + "</cus:HighRiskCateogries>"
                            + "<cus:FATCAStatus>" + FATCAStatus + "</cus:FATCAStatus>"
                            + "<cus:FATCACountryofBirth>" + FATCACountryofBirth + "</cus:FATCACountryofBirth>"

                            + "<cus:gAMK.F.I.NAT g=\"1\"><cus:mAMK.F.I.NAT m=\"1\">"
                            + "<cus:FATCANationality>" + FATCANationality + "</cus:FATCANationality>"
                            + "<cus:FATCATaxIdentNumber>" + FATCATaxIdentNumber + "</cus:FATCATaxIdentNumber>"
                            + "</cus:mAMK.F.I.NAT></cus:gAMK.F.I.NAT>"

                            //+"<cus:gAMK.F.I.ADDRESS g=\"1\">"
                            //+"<cus:FATCAAddress>" + FATCAAddress + "</cus:FATCAAddress>"
                            //+"</cus:gAMK.F.I.ADDRESS>"

                            //+"<cus:FATCAPostalCode>" + FATCAPostalCode + "</cus:FATCAPostalCode>"
                            //+"<cus:USTaxIdentificationNumber>" + USTaxIdentificationNumber + "</cus:USTaxIdentificationNumber>"
                            //+ "<cus:FATCACountryCode>" + FATCACountryCode + "</cus:FATCACountryCode>"
                            //+"<cus:RecalcitrantStatus>" + RecalcitrantStatus + "</cus:RecalcitrantStatus>"
                            + "<cus:OccupationType>" + OccupationType + "</cus:OccupationType>"
                            + "<cus:OccupationDetails>" + OccupationDetails + "</cus:OccupationDetails>"

                            + "<cus:gAMK.ID.TYPE g=\"1\"><cus:mAMK.ID.TYPE m=\"1\">"
                            + "<cus:IDType>" + IDType + "</cus:IDType>"
                            + "<cus:IDNumber>" + IDNumber + "</cus:IDNumber>"
                            + IssueDateTag + ExpiryDateTag
                            + "</cus:mAMK.ID.TYPE></cus:gAMK.ID.TYPE>"

                            //+"<cus:Position>" + Position + "</cus:Position>"
                            //+"<cus:Department>" + Department + "</cus:Department>"
                            //+"<cus:gAMK.KYC.IMAGE g=\"1\"><cus:KYCImage></cus:KYCImage></cus:gAMK.KYC.IMAGE>"

                            + "</CUSTOMERAMKINDIVWSType></amk:INDIVCUSTOMERCREATION></soapenv:Body></soapenv:Envelope>";
                            #endregion xml single
                        }
                        else
                        {
                            #region xml with spouse
                            xmlStr = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" "
                            + "xmlns:amk=\"http://temenos.com/AMKINDCUST\" xmlns:cus=\"http://temenos.com/CUSTOMERAMKINDIVWS\"><soapenv:Header/>"
                            + "<soapenv:Body><amk:INDIVCUSTOMERCREATION><WebRequestCommon><company>" + CreCompany + "</company><password>" + CrePassword + "</password>"
                            + "<userName>" + CreUserName + "</userName></WebRequestCommon>"
                            + "<OfsFunction>"
                            + "<messageId>" + "C-" + LoanAppPersonID + "-" + SHORTNAME + "-" + msgIDDate + "</messageId>"
                            + "</OfsFunction>"
                            + "<CUSTOMERAMKINDIVWSType id=\"" + CustomerID + "\">"
                            + "<cus:gSHORTNAME g=\"1\"><cus:SHORTNAME>" + SHORTNAME + "</cus:SHORTNAME></cus:gSHORTNAME>"
                            + "<cus:gNAME1 g=\"1\"><cus:NAME1>" + NAME1 + "</cus:NAME1></cus:gNAME1>"
                            + "<cus:gSTREET g=\"1\"><cus:STREET>" + STREET + "</cus:STREET></cus:gSTREET>"
                            + "<cus:gRELATIONCODE g=\"1\">"
                            + "<cus:mRELATIONCODE m=\"1\"><cus:RELATIONCODE>" + RELATIONCODE + "</cus:RELATIONCODE>"
                            + "<cus:RELCUSTOMER>" + RELCUSTOMER + "</cus:RELCUSTOMER>"
                            + "</cus:mRELATIONCODE></cus:gRELATIONCODE>"
                            + "<cus:SECTOR>" + SECTOR + "</cus:SECTOR>"
                            + "<cus:ACCOUNTOFFICER>" + ACCOUNTOFFICER + "</cus:ACCOUNTOFFICER>"
                            + "<cus:INDUSTRY>" + INDUSTRY + "</cus:INDUSTRY>"
                            + "<cus:TARGET>" + TARGET + "</cus:TARGET>"
                            + "<cus:NATIONALITY>" + NATIONALITY + "</cus:NATIONALITY>"
                            + "<cus:CUSTOMERSTATUS>" + CUSTOMERSTATUS + "</cus:CUSTOMERSTATUS>"
                            + "<cus:RESIDENCE>" + RESIDENCE + "</cus:RESIDENCE>"
                            + "<cus:BIRTHINCORPDATE>" + BIRTHINCORPDATE + "</cus:BIRTHINCORPDATE>"
                            + "<cus:TITLE>" + TITLE + "</cus:TITLE><cus:GENDER>" + GENDER + "</cus:GENDER>"
                            + "<cus:MARITALSTATUS>" + MARITALSTATUS + "</cus:MARITALSTATUS>"
                            //+"<cus:OCCUPATION>" + OCCUPATION + "</cus:OCCUPATION>"
                            //+"<cus:gFURTHERDETAILS g=\"1\">"
                            //+"<cus:FURTHERDETAILS>" + FURTHERDETAILS + "</cus:FURTHERDETAILS>"
                            //+"</cus:gFURTHERDETAILS>" 
                            + PostalCode
                            //+ "<cus:gEMPLOY.BUS.ADDR g=\"1\">"
                            //+ "<cus:EmployersAddress>" + EmployersAddress + "</cus:EmployersAddress>"
                            //+"</cus:gEMPLOY.BUS.ADDR>"
                            //+"<cus:HomePhoneNo>" + HomePhoneNo + "</cus:HomePhoneNo>"
                            //+"<cus:WorkPhoneNo>" + WorkPhoneNo + "</cus:WorkPhoneNo>"
                            + "<cus:MobilePhoneNo>" + MobilePhoneNo + "</cus:MobilePhoneNo>"
                            + "<cus:ResidenceYN>" + ResidenceYN + "</cus:ResidenceYN>"
                            //+"<cus:EmailAddress>" + EmailAddress + "</cus:EmailAddress>"
                            //+"<cus:EmployerName>" + EmployerName + "</cus:EmployerName>"
                            //+"<cus:TaxpayerRegistrationNo>" + TaxpayerRegistrationNo + "</cus:TaxpayerRegistrationNo>"
                            + "<cus:BlockedYN>" + BlockedYN + "</cus:BlockedYN>"
                            + "<cus:AMKStaff>" + AMKStaff + "</cus:AMKStaff>"
                            + "<cus:NoFamilyMember>" + NoFamilyMember + "</cus:NoFamilyMember>"
                            //+"<cus:EmployeeCode>" + EmployeeCode + "</cus:EmployeeCode>"
                            //+"<cus:DateofEmployment>" + DateofEmployment + "</cus:DateofEmployment>"
                            //+"<cus:FaxNo>" + FaxNo + "</cus:FaxNo>"
                            + "<cus:Profession>" + Profession + "</cus:Profession>"
                            //+"<cus:EmploymentStatus>" + EmploymentStatus + "</cus:EmploymentStatus>"
                            + "<cus:MainSourceofIncome>" + MainSourceofIncome + "</cus:MainSourceofIncome>"
                            //+"<cus:EmployersCode>" + EmployersCode + "</cus:EmployersCode>"
                            + "<cus:SpouseName>" + SpouseName + "</cus:SpouseName>"
                            //+"<cus:SpouseOccupation>" + SpouseOccupation + "</cus:SpouseOccupation>"
                            + "<cus:ConsenttoDisclosure>" + ConsenttoDisclosure + "</cus:ConsenttoDisclosure>"
                            + "<cus:DateofSignature>" + DateofSignature + "</cus:DateofSignature>"

                            //+"<cus:gPLACE.STUDY g=\"1\"><cus:mPLACE.STUDY m=\"1\">"
                            //+"<cus:PlaceofStudy>" + PlaceofStudy + "</cus:PlaceofStudy>"
                            //+"<cus:DurationofCourse>" + DurationofCourse + "</cus:DurationofCourse>"
                            //+"<cus:FieldofStudy>" + FieldofStudy + "</cus:FieldofStudy>"
                            //+"</cus:mPLACE.STUDY></cus:gPLACE.STUDY>"

                            //+"<cus:NominationForm>" + NominationForm + "</cus:NominationForm>"

                            //+"<cus:gINS.MEM.NO g=\"1\"><cus:mINS.MEM.NO m=\"1\">"
                            //+"<cus:NominationCustomer>" + NominationCustomer + "</cus:NominationCustomer>"
                            //+"<cus:NominationBeneficiary>" + NominationBeneficiary + "</cus:NominationBeneficiary>"
                            //+"<cus:NominationAddress1>" + NominationAddress1 + "</cus:NominationAddress1>"
                            //+"<cus:NominationAddress2>" + NominationAddress2 + "</cus:NominationAddress2>"
                            //+"<cus:NominationAddress3>" + NominationAddress3 + "</cus:NominationAddress3>"
                            //+"<cus:NominationAddress4>" + NominationAddress4 + "</cus:NominationAddress4>"
                            //+ INSPostalCode 
                            //+ "<cus:INSPhoneNo>" + INSPhoneNo + "</cus:INSPhoneNo>"
                            //+"<cus:RelationshiptoCustomer>" + RelationshiptoCustomer + "</cus:RelationshiptoCustomer>"
                            //+"<cus:AmountorofLegacy>" + AmountorofLegacy + "</cus:AmountorofLegacy>"
                            //+"</cus:mINS.MEM.NO></cus:gINS.MEM.NO>"

                            //+"<cus:gBUS.NAME g=\"1\"><cus:mBUS.NAME m=\"1\">"
                            //+ "<cus:NameofBusiness>" + NameofBusiness + "</cus:NameofBusiness>"
                            //+"<cus:NatureofBusiness>" + NatureofBusiness + "</cus:NatureofBusiness>"
                            //+"<cus:BusinessType>" + BusinessType + "</cus:BusinessType>"
                            //+"<cus:BusinessPlan>" + BusinessPlan + "</cus:BusinessPlan>"
                            //+"<cus:RoleinBusiness>" + RoleinBusiness + "</cus:RoleinBusiness>"
                            //+"<cus:BusinessAddress>" + BusinessAddress + "</cus:BusinessAddress>"
                            //+"<cus:BusinessAddress2>" + BusinessAddress2 + "</cus:BusinessAddress2>"
                            //+"<cus:BusinessAddress3>" + BusinessAddress3 + "</cus:BusinessAddress3>"
                            //+"<cus:BusinessAddress4>" + BusinessAddress4 + "</cus:BusinessAddress4>"
                            //+"<cus:TaxClearanceHeldYN>" + TaxClearanceHeldYN + "</cus:TaxClearanceHeldYN>"
                            //+"<cus:TaxClearanceExpiryDate>" + TaxClearanceExpiryDate + "</cus:TaxClearanceExpiryDate>"
                            //+"</cus:mBUS.NAME></cus:gBUS.NAME>"

                            //+"<cus:Student>" + Student + "</cus:Student>"
                            //+"<cus:AMKStaffType>" + AMKStaffType + "</cus:AMKStaffType>"
                            + "<cus:SpouseCustomerNumber>" + SpouseCustomerNumber + "</cus:SpouseCustomerNumber>"
                            //+"<cus:NoofEmployees>" + NoofEmployees + "</cus:NoofEmployees>"
                            + "<cus:ProvinceCity>" + ProvinceCity + "</cus:ProvinceCity>"
                            + "<cus:District>" + District + "</cus:District>"
                            + "<cus:Commune>" + Commune + "</cus:Commune>"
                            + "<cus:Village>" + Village + "</cus:Village>"
                            + "<cus:SpouseNameinKhmer>" + SpouseNameinKhmer + "</cus:SpouseNameinKhmer>"
                            + "<cus:SurnameKhmer>" + SurnameKhmer + "</cus:SurnameKhmer>"
                            + "<cus:FirstNameKhmer>" + FirstNameKhmer + "</cus:FirstNameKhmer>"
                            + "<cus:SpouseDateofBirth>" + SpouseDateofBirth + "</cus:SpouseDateofBirth>"

                            + "<cus:gAMK.SP.ID.TYPE g=\"1\"><cus:mAMK.SP.ID.TYPE m=\"1\">"
                            + "<cus:SpouseIDType>" + SpouseIDType + "</cus:SpouseIDType>"
                            + "<cus:SpouseIDNumber>" + SpouseIDNumber + "</cus:SpouseIDNumber>"
                            + SpouseIDIssueDateTag
                            + SpouseIDExpiryDateTag
                            + "</cus:mAMK.SP.ID.TYPE></cus:gAMK.SP.ID.TYPE>"

                            + "<cus:NoActiveMember>" + NoActiveMember + "</cus:NoActiveMember>"
                            + "<cus:ProvertyStatus>" + ProvertyStatus + "</cus:ProvertyStatus>"
                            + VillageBank
                            + "<cus:VillageBankPresident>" + VillageBankPresident + "</cus:VillageBankPresident>"
                            + "<cus:MobileNumberType>" + MobileNumberType + "</cus:MobileNumberType>"
                            + "<cus:PlaceofBirth>" + PlaceofBirth + "</cus:PlaceofBirth>"
                            //+"<cus:BlacklistStatus>" + BlacklistStatus + "</cus:BlacklistStatus>"
                            //+"<cus:RiskStatus>" + RiskStatus + "</cus:RiskStatus>"
                            //+"<cus:BlackListCheckDate>" + BlackListCheckDate + "</cus:BlackListCheckDate>"
                            //+"<cus:BlacklistCurrentStatus>" + BlacklistCurrentStatus + "</cus:BlacklistCurrentStatus>"
                            //+ "<cus:HighRiskCateogries>" + HighRiskCateogries + "</cus:HighRiskCateogries>"
                            + "<cus:FATCAStatus>" + FATCAStatus + "</cus:FATCAStatus>"
                            + "<cus:FATCACountryofBirth>" + FATCACountryofBirth + "</cus:FATCACountryofBirth>"

                            + "<cus:gAMK.F.I.NAT g=\"1\"><cus:mAMK.F.I.NAT m=\"1\">"
                            + "<cus:FATCANationality>" + FATCANationality + "</cus:FATCANationality>"
                            + "<cus:FATCATaxIdentNumber>" + FATCATaxIdentNumber + "</cus:FATCATaxIdentNumber>"
                            + "</cus:mAMK.F.I.NAT></cus:gAMK.F.I.NAT>"

                            //+"<cus:gAMK.F.I.ADDRESS g=\"1\">"
                            //+"<cus:FATCAAddress>" + FATCAAddress + "</cus:FATCAAddress>"
                            //+"</cus:gAMK.F.I.ADDRESS>"

                            //+ FATCAPostalCode 
                            //+ "<cus:USTaxIdentificationNumber>" + USTaxIdentificationNumber + "</cus:USTaxIdentificationNumber>"
                            //+"<cus:FATCACountryCode>" + FATCACountryCode + "</cus:FATCACountryCode>"
                            //+"<cus:RecalcitrantStatus>" + RecalcitrantStatus + "</cus:RecalcitrantStatus>"

                            + "<cus:OccupationType>" + OccupationType + "</cus:OccupationType>"
                            + "<cus:OccupationDetails>" + OccupationDetails + "</cus:OccupationDetails>"

                            + "<cus:gAMK.ID.TYPE g=\"1\"><cus:mAMK.ID.TYPE m=\"1\">"
                            + "<cus:IDType>" + IDType + "</cus:IDType>"
                            + "<cus:IDNumber>" + IDNumber + "</cus:IDNumber>"
                            + IssueDateTag + ExpiryDateTag
                            + "</cus:mAMK.ID.TYPE></cus:gAMK.ID.TYPE>"

                            //+"<cus:Position>" + Position + "</cus:Position>"
                            //+"<cus:Department>" + Department + "</cus:Department>"
                            //+"<cus:gAMK.KYC.IMAGE g=\"1\"><cus:KYCImage></cus:KYCImage></cus:gAMK.KYC.IMAGE>"

                            + "</CUSTOMERAMKINDIVWSType>"
                            + "</amk:INDIVCUSTOMERCREATION></soapenv:Body></soapenv:Envelope>";
                            #endregion xml with spouse
                        }

                        #endregion xml



                        #region Call To T24

                        T24_AddLog(fileHeader, "AddEditCustomer_RQ", xmlStr, "LoanAdd");
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(CreUrl);
                        request.ContentType = "text/xml"; // or application/soap+xml for SOAP 1.2
                        request.Method = "POST";
                        request.KeepAlive = false;

                        byte[] byteArray = Encoding.UTF8.GetBytes(xmlStr);
                        request.ContentLength = byteArray.Length;

                        Stream requestStream = request.GetRequestStream();
                        requestStream.Write(byteArray, 0, byteArray.Length);
                        requestStream.Close();

                        HttpWebResponse response = null;
                        try
                        {
                            response = (HttpWebResponse)request.GetResponse();
                        }
                        catch (WebException ex)
                        {
                            response = (HttpWebResponse)ex.Response;
                        }

                        Stream receiveStream = response.GetResponseStream();
                        StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(readStream.ReadToEnd());
                        string successIndicator = doc.GetElementsByTagName("successIndicator").Item(0).InnerText;
                        string transactionId = doc.GetElementsByTagName("transactionId").Item(0).InnerText;
                        T24_AddLog(fileHeader, "AddEditCustomer_RS: " + (int)response.StatusCode, doc.InnerXml, "LoanAdd");

                        #endregion

                        #region Condiction

                        if (successIndicator == "Success")
                        {
                            ReturnDT("update tblLoanAppPerson2 set PersonID='" + transactionId + "',CustomerID='" + transactionId
                            + "',Number='" + transactionId + "' where LoanAppPersonID='" + LoanAppPersonID + "'");
                            ERR = transactionId;
                            isError = "0";
                            Remark = ERR;
                        }
                        else {

                            string messages = doc.GetElementsByTagName("messages").Item(0).InnerText;
                            if (messages == "LIVE RECORD NOT CHANGED"
                                || messages == "COMPANY.BOOK:1:1=NOCHANGE FIELD" || messages == "EB-LIVE.RECORD.NOT.CHANGED")
                            {
                                ERR = CustomerID;
                                isError = "0";
                                Remark = ERR + " " + messages;
                            }
                            else
                            {
                                xmlContent = doc.InnerXml;
                                //add error log
                                isError = "1";
                                #region Error
                                if (xmlContent.Contains("<Status>"))
                                {
                                    int pFrom = xmlContent.IndexOf("<Status>");// + "<Status>".Length;
                                    int pTo = xmlContent.IndexOf("</Status>") + "</Status>".Length;
                                    string MSGxmlContent = xmlContent.Substring(pFrom, pTo - pFrom).Trim();
                                    LoanAppApproveToCBSError.Status obj = GenerateXmlObject<LoanAppApproveToCBSError.Status>(MSGxmlContent);
                                    string strMsg = "";
                                    for (int iMsg = 0; iMsg < obj.Messages.Count; iMsg++)
                                    {
                                        if (iMsg == 0)
                                        {
                                            strMsg = obj.Messages[iMsg];
                                        }
                                        else
                                        {
                                            strMsg = strMsg + " | " + obj.Messages[iMsg];
                                        }
                                    }
                                    SMS = strMsg;
                                }
                                #endregion Error
                            }

                        }

                        #endregion

                        #region log
                        T24_LoanAppAddLogAdd(LoanAppID, LoanAppPersonID, fileHeader, isError, doc.InnerXml, Remark, "0");
                        #endregion log

                        #region Close Stream

                        readStream.Close();
                        requestStream.Close();
                        receiveStream.Close();
                        response.Close();

                        #endregion

                    }
                    catch (Exception ex)
                    {
                        //add error log
                        string line = GetLineNumber(ex).ToString();
                        SMS = ex.Message.ToString() + line;
                    }
                    #endregion CustomerID - Add/Edit
                }
            }
            catch (Exception ex)
            {
                ERR = "Error";
                SMS = ex.Message.ToString();
            }
            rs[0] = ERR;
            rs[1] = SMS;
            return rs;
        }

        public string[] AddLoan(string fileHeader, string LoanAppID, string CUSTOMERID, string api_name)
        {
            string[] rs = new string[2];
            string isError = "", ErrXml = "";
            string ERR = "Error", SMS = "", xmlContent = "", CBSKey = "", CBSAcc = "", Remark = "Start", LoanAppStatusID = "12";
            try
            {
                fileHeader = fileHeader.Replace(" ", "_").Replace("-", "_").Replace(":", "_").Replace(".", "_");
                string sql = "exec T24_LoanAppListForOpenToCBS3 @LoanAppID='" + LoanAppID + "'";
                if (api_name == "AM")
                {
                    sql = "exec T24_LoanAppListForOpenToCBS3ForChangeDISBDate @LoanAppID='" + LoanAppID + "'";
                }
                DataTable dt = ReturnDT(sql);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    #region params
                    string CreUrl = dt.Rows[i]["CreUrl"].ToString();
                    string CreCompany = dt.Rows[i]["CreCompany"].ToString();
                    string CreUserName = dt.Rows[i]["CreUserName"].ToString();
                    string CrePassword = dt.Rows[i]["CrePassword"].ToString();

                    string APPDATE = dt.Rows[i]["APPDATE"].ToString();
                    string PRODUCTID = dt.Rows[i]["PRODUCTID"].ToString();
                    string CURRENCY = dt.Rows[i]["CURRENCY"].ToString();
                    string LOANTERM = dt.Rows[i]["LOANTERM"].ToString();
                    string AMOUNT = dt.Rows[i]["AMOUNT"].ToString();
                    string INTRATE = dt.Rows[i]["INTRATE"].ToString();
                    string REPAYSTDATE = dt.Rows[i]["REPAYSTDATE"].ToString();
                    string VILLAGEBANK = dt.Rows[i]["VILLAGEBANK"].ToString();
                    string LOANCYCLE = dt.Rows[i]["LOANCYCLE"].ToString();
                    string GROUPNO = dt.Rows[i]["GROUPNO"].ToString();
                    string OPERATION = dt.Rows[i]["OPERATION"].ToString();
                    string LOANREFERBY = dt.Rows[i]["LOANREFERBY"].ToString();
                    string CREDITOFFICER = dt.Rows[i]["CREDITOFFICER"].ToString();
                    string MAINBUSINESS = dt.Rows[i]["MAINBUSINESS"].ToString();
                    string SEMIBALLONFREQ = dt.Rows[i]["SEMIBALLONFREQ"].ToString();
                    //string LOANPURPOSE = dt.Rows[i]["LOANPURPOSE"].ToString();
                    string lp1 = dt.Rows[i]["lp1"].ToString();
                    string lp2 = dt.Rows[i]["lp2"].ToString();
                    string lp3 = dt.Rows[i]["lp3"].ToString();

                    string CBCREQUIRED = dt.Rows[i]["CBCREQUIRED"].ToString();
                    string GURANTORCODE = dt.Rows[i]["GURANTORCODE"].ToString();
                    //string COLLATERALCODE = dt.Rows[i]["COLLATERALCODE"].ToString();
                    //string COLLATERALCCY = dt.Rows[i]["COLLATERALCCY"].ToString();
                    //string COLLATERALTYPE = dt.Rows[i]["COLLATERALTYPE"].ToString();
                    //string PROPERTYTYPE = dt.Rows[i]["PROPERTYTYPE"].ToString();
                    //string COLLDOCTYPE = dt.Rows[i]["COLLDOCTYPE"].ToString();
                    //string COLLATERALQNTY = dt.Rows[i]["COLLATERALQNTY"].ToString();
                    //string COLLATERALUNIT = dt.Rows[i]["COLLATERALUNIT"].ToString();
                    //string COLLATERALPRICE = dt.Rows[i]["COLLATERALPRICE"].ToString();
                    //string COLLATERALVALUE = dt.Rows[i]["COLLATERALVALUE"].ToString();
                    //string TOTALCOLLATERAL = dt.Rows[i]["TOTALCOLLATERAL"].ToString();
                    //string COLLATERALLVR = dt.Rows[i]["COLLATERALLVR"].ToString();
                    string INCEXPCCY = dt.Rows[i]["INCEXPCCY"].ToString();
                    string TOTALINCOME = dt.Rows[i]["TOTALINCOME"].ToString();
                    string TOTALEXPENSE = dt.Rows[i]["TOTALEXPENSE"].ToString();
                    string AVGINCOME = dt.Rows[i]["AVGINCOME"].ToString();
                    string RESCHEDLOAN = dt.Rows[i]["RESCHEDLOAN"].ToString();

                    string UPFRONTFEE = dt.Rows[i]["UPFRONTFEE"].ToString();
                    string INCOMEFREQ = dt.Rows[i]["INCOMEFREQ"].ToString();
                    string BSNSEXPERIENCE = dt.Rows[i]["BSNSEXPERIENCE"].ToString();
                    string BSNSAGE = dt.Rows[i]["BSNSAGE"].ToString();
                    string MONTHLYTXNFEE = dt.Rows[i]["MONTHLYTXNFEE"].ToString();
                    string NOBSNSACT = dt.Rows[i]["NOBSNSACT"].ToString();
                    string COBRWCUSTOMER = dt.Rows[i]["COBRWCUSTOMER"].ToString();
                    #endregion params
                    #region xml
                    #region collateral
                    string COLLATERALCODE = "", COLLATERALCCY = "", COLLATERALTYPE = "", COLLDOCTYPE = "", COLLATERALQNTY = "", COLLATERALUNIT = "", COLLATERALPRICE = "";
                    string collStr = "";
                    try
                    {
                        DataTable dtColl = new DataTable();
                        dtColl = ReturnDT("exec T24_LoanAppListForOpenToCBS3_Collateral @LoanAppID='" + LoanAppID + "'");
                        for (int j = 0; j < dtColl.Rows.Count; j++)
                        {
                            COLLATERALCODE = dtColl.Rows[j]["COLLATERALCODE"].ToString();
                            COLLATERALCCY = dtColl.Rows[j]["COLLATERALCCY"].ToString();
                            COLLATERALTYPE = dtColl.Rows[j]["COLLATERALTYPE"].ToString();
                            COLLDOCTYPE = dtColl.Rows[j]["COLLDOCTYPE"].ToString();
                            COLLATERALQNTY = dtColl.Rows[j]["COLLATERALQNTY"].ToString();
                            COLLATERALUNIT = dtColl.Rows[j]["COLLATERALUNIT"].ToString();
                            COLLATERALPRICE = dtColl.Rows[j]["COLLATERALPRICE"].ToString();
                            collStr = collStr
                            + "<amk1:mCOLLATERALCODE m=\"1\"><!--Optional:-->"
                            + "<amk1:COLLATERALCODE>" + COLLATERALCODE + "</amk1:COLLATERALCODE><!--Optional:-->"
                            //+ "<amk1:COLLATERALCCY>" + COLLATERALCCY + "</amk1:COLLATERALCCY><!--Optional:-->"
                            + "<amk1:sgCOLLATERALTYPE sg=\"1\"><!--Zero or more repetitions:-->"
                            + "<amk1:COLLATERALTYPE s=\"1\"><!--Optional:-->"
                            + "<amk1:COLLATERALTYPE>" + COLLATERALTYPE + "</amk1:COLLATERALTYPE><!--Optional:-->"
                            + "<amk1:COLLDOCTYPE>" + COLLDOCTYPE + "</amk1:COLLDOCTYPE><!--Optional:-->"
                            + "<amk1:COLLATERALQNTY>" + COLLATERALQNTY + "</amk1:COLLATERALQNTY><!--Optional:-->"
                            + "<amk1:COLLATERALUNIT>" + COLLATERALUNIT + "</amk1:COLLATERALUNIT><!--Optional:-->"
                            + "<amk1:COLLATERALPRICE>" + COLLATERALPRICE + "</amk1:COLLATERALPRICE>"
                            + "</amk1:COLLATERALTYPE></amk1:sgCOLLATERALTYPE>"
                            + "</amk1:mCOLLATERALCODE>";
                        }
                    }
                    catch { }
                    #endregion collateral
                    #region lp
                    string LOANPURPOSE = "";
                    if (lp1.Trim() != "")
                    {
                        LOANPURPOSE = "<amk1:LOANPURPOSE>" + lp1 + "</amk1:LOANPURPOSE>";
                    }
                    if (lp2.Trim() != "")
                    {
                        LOANPURPOSE = LOANPURPOSE + "<amk1:LOANPURPOSE>" + lp2 + "</amk1:LOANPURPOSE>";
                    }
                    if (lp3.Trim() != "")
                    {
                        LOANPURPOSE = LOANPURPOSE + "<amk1:LOANPURPOSE>" + lp3 + "</amk1:LOANPURPOSE>";
                    }
                    #endregion lp
                    #region SemiBallon
                    string PRREPAYFREQStr = "";
                    if (SEMIBALLONFREQ != "")
                    {
                        if (SEMIBALLONFREQ != "0")
                        {
                            PRREPAYFREQStr = "<amk1:PRREPAYFREQ>" + SEMIBALLONFREQ + "</amk1:PRREPAYFREQ>";
                        }
                    }
                    #endregion
                    #region xml new
                    string xmlStr = "<?xml version=\"1.0\"?><soapenv:Envelope xmlns:amk1=\"http://temenos.com/AMKDLAPPLICATIONBOINP\" "
                    + "xmlns:amk=\"http://temenos.com/AMKCREATELOAN\" xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\"><soapenv:Header/>"
                    + "<soapenv:Body><amk:CREATELOAN><!--Optional:--><WebRequestCommon><!--Optional:-->"
                    + "<company>" + CreCompany + "</company><password>" + CrePassword + "</password><userName>" + CreUserName + "</userName></WebRequestCommon><!--Optional:-->"
                    + "<OfsFunction>"
                    + "<messageId>" + "L-" + fileHeader + "</messageId>"
                    + "</OfsFunction>"
                    + "<!--Optional:--><AMKDLAPPLICATIONBOINPType id=\"\"><!--Optional:-->"
                    + "<amk1:APPDATE>" + APPDATE + "</amk1:APPDATE><!--Optional:-->"
                    + "<amk1:CUSTOMERID>" + CUSTOMERID + "</amk1:CUSTOMERID><!--Optional:-->"
                    + "<amk1:PRODUCTID>" + PRODUCTID + "</amk1:PRODUCTID><!--Optional:-->"
                    + "<amk1:CURRENCY>" + CURRENCY + "</amk1:CURRENCY><!--Optional:-->"
                    + "<amk1:LOANTERM>" + LOANTERM + "</amk1:LOANTERM><!--Optional:-->"
                    + "<amk1:AMOUNT>" + AMOUNT + "</amk1:AMOUNT>"
                    + "<amk1:INTRATE>" + INTRATE + "</amk1:INTRATE><!--Optional:-->"
                    + "<amk1:REPAYSTDATE>" + REPAYSTDATE + "</amk1:REPAYSTDATE><!--Optional:-->"
                    + "<amk1:LOANCYCLE>" + LOANCYCLE + "</amk1:LOANCYCLE><!--Optional:-->"
                    + "<amk1:GROUPNO>" + GROUPNO + "</amk1:GROUPNO><!--Optional:-->"
                    + "<amk1:OPERATION>" + OPERATION + "</amk1:OPERATION><!--Optional:-->"
                    + "<amk1:LOANREFERBY>" + LOANREFERBY + "</amk1:LOANREFERBY><!--Optional:-->"
                    + "<amk1:CREDITOFFICER>" + CREDITOFFICER + "</amk1:CREDITOFFICER><!--Optional:-->"
                    + "<amk1:gMAINBUSINESS g=\"1\"><amk1:MAINBUSINESS>" + MAINBUSINESS + "</amk1:MAINBUSINESS></amk1:gMAINBUSINESS>"
                    + "<amk1:gLOANPURPOSE g=\"1\">" + LOANPURPOSE + "</amk1:gLOANPURPOSE>"
                    + "<amk1:CBCREQUIRED>" + CBCREQUIRED + "</amk1:CBCREQUIRED><!--Optional:-->"
                    + "<amk1:gGURANTORCODE g=\"1\"><!--Zero or more repetitions:--><amk1:GURANTORCODE>" + GURANTORCODE + "</amk1:GURANTORCODE></amk1:gGURANTORCODE><!--Optional:-->"
                    + "<amk1:gCOBRWCUSTOMER g=\"1\"><!--Zero or more repetitions:--><amk1:COBRWCUSTOMER>" + COBRWCUSTOMER + "</amk1:COBRWCUSTOMER></amk1:gCOBRWCUSTOMER><!--Optional:-->"

                    + "<amk1:gCOLLATERALCODE g=\"1\"><!--Zero or more repetitions:-->"

                    + collStr

                    + "</amk1:gCOLLATERALCODE><!--Optional:-->"

                    + "<amk1:INCEXPCCY>" + INCEXPCCY + "</amk1:INCEXPCCY><!--Optional:-->"
                    + "<amk1:TOTALINCOME>" + TOTALINCOME + "</amk1:TOTALINCOME><!--Optional:-->"
                    + "<amk1:TOTALEXPENSE>" + TOTALEXPENSE + "</amk1:TOTALEXPENSE><!--Optional:-->"
                    + "<amk1:AVGINCOME>" + AVGINCOME + "</amk1:AVGINCOME>"
                    + "<amk1:RESCHEDLOAN>" + RESCHEDLOAN + "</amk1:RESCHEDLOAN>"
                    + PRREPAYFREQStr
                    + "<amk1:UPFRONTFEE>" + UPFRONTFEE + "</amk1:UPFRONTFEE><!--Optional:-->"
                    + "<amk1:INCOMEFREQ>" + INCOMEFREQ + "</amk1:INCOMEFREQ><!--Optional:-->"
                    + "<amk1:BSNSEXPERIENCE>" + BSNSEXPERIENCE + "</amk1:BSNSEXPERIENCE><!--Optional:-->"
                    + "<amk1:BSNSAGE>" + BSNSAGE + "</amk1:BSNSAGE><!--Optional:-->"
                    + "<amk1:MONTHLYTXNFEE>" + MONTHLYTXNFEE + "</amk1:MONTHLYTXNFEE>"
                    + "<amk1:NOBSNSACT>" + NOBSNSACT + "</amk1:NOBSNSACT>"
                    + "</AMKDLAPPLICATIONBOINPType></amk:CREATELOAN></soapenv:Body></soapenv:Envelope>";
                    #endregion xml new
                    #endregion xml

                    #region Call To T24

                    T24_AddLog(fileHeader, "AddLoan_RQ", xmlStr, "LoanAdd");
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(CreUrl);
                    request.ContentType = "text/xml"; // or application/soap+xml for SOAP 1.2
                    request.Method = "POST";
                    request.KeepAlive = false;

                    byte[] byteArray = Encoding.UTF8.GetBytes(xmlStr);
                    request.ContentLength = byteArray.Length;

                    Stream requestStream = request.GetRequestStream();
                    requestStream.Write(byteArray, 0, byteArray.Length);
                    requestStream.Close();

                    HttpWebResponse response = null;
                    try
                    {
                        response = (HttpWebResponse)request.GetResponse();
                    }
                    catch (WebException ex)
                    {
                        response = (HttpWebResponse)ex.Response;
                    }

                    Stream receiveStream = response.GetResponseStream();
                    StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(readStream.ReadToEnd());
                    T24_AddLog(fileHeader, "AddLoan_RS: " + (int)response.StatusCode, doc.InnerXml, "LoanAdd");
                    #endregion


                    #region call to T24
                    string successIndicator = doc.GetElementsByTagName("successIndicator").Item(0).InnerText;
                    string transactionId = doc.GetElementsByTagName("transactionId").Item(0).InnerText;
                    if (successIndicator == "Success")
                    {
                        CBSKey = transactionId;
                        CBSAcc = transactionId;
                        Remark = CBSKey;
                        LoanAppStatusID = "7";
                        ERR = "Success";
                        isError = "0";
                    }
                    else
                    {
                        //add error log
                        //Remark = xmlContent;
                        LoanAppStatusID = "12";
                        ERR = "Error";
                        isError = "1";

                        #region Error
                        xmlContent = doc.InnerXml;
                        if (xmlContent.Contains("<Status>"))
                        {
                            int pFrom = xmlContent.IndexOf("<Status>");// + "<Status>".Length;
                            int pTo = xmlContent.IndexOf("</Status>") + "</Status>".Length;
                            string MSGxmlContent = xmlContent.Substring(pFrom, pTo - pFrom).Trim();
                            LoanAppApproveToCBSError.Status obj = GenerateXmlObject<LoanAppApproveToCBSError.Status>(MSGxmlContent);
                            string strMsg = "";
                            for (int iMsg = 0; iMsg < obj.Messages.Count; iMsg++)
                            {
                                if (iMsg == 0)
                                {
                                    strMsg = obj.Messages[iMsg];
                                }
                                else
                                {
                                    strMsg = strMsg + " | " + obj.Messages[iMsg];
                                }
                            }
                            SMS = strMsg;
                            Remark = SMS;
                        }
                        #endregion Error


                    }
                    #endregion call to T24

                    #region log
                    T24_LoanAppAddLogAdd(LoanAppID, "0", fileHeader, isError, doc.InnerXml, Remark, "0");
                    #endregion log

                }

            }
            catch (Exception ex)
            {
                //add error log
                Remark = ex.Message.ToString();
                LoanAppStatusID = "12";
                ERR = "Error";
            }
            #region add log
            UpdateLoanAppStatus(LoanAppID, CBSKey, CBSAcc, "Error AddLoan: " + Remark, LoanAppStatusID);
            #endregion add log
            rs[0] = ERR;
            rs[1] = SMS;
            return rs;
        }

        #endregion

        public void UpdateLoanAppStatus(string LoanAppID, string CBSKey, string CBSAcc, string Remark, string LoanAppStatusID)
        {
            try
            {
                SqlConnection Con1 = new SqlConnection(ConStr());
                Con1.Open();
                SqlCommand Com1 = new SqlCommand();
                Com1.Connection = Con1;
                Com1.Parameters.Clear();
                Com1.CommandText = "exec T24_LoanAppListForOpenToCBSReturn @LoanAppID=@LoanAppID,@CBSKey=@CBSKey,@CBSAcc=@CBSAcc,@Remark=@Remark,@LoanAppStatusID=@LoanAppStatusID";
                Com1.Parameters.AddWithValue("@LoanAppID", LoanAppID);
                Com1.Parameters.AddWithValue("@CBSKey", CBSKey);
                Com1.Parameters.AddWithValue("@CBSAcc", CBSAcc);
                Com1.Parameters.AddWithValue("@Remark", Remark);
                Com1.Parameters.AddWithValue("@LoanAppStatusID", LoanAppStatusID);
                Com1.ExecuteNonQuery();
                Con1.Close();
                Con1.Dispose();
                SqlConnection.ClearAllPools();
            }
            catch (Exception ex)
            {                
                string x = ex.Message.ToString();
            }
        }
        public void T24_LoanAppAddLogAdd(string LoanAppID, string LoanAppPersonID,string fileHeader,string isError,string ErrXml,string CBSKey,string CreateBy) {
            try
            {
                SqlConnection Con1 = new SqlConnection(ConStr());
                Con1.Open();
                SqlCommand Com1 = new SqlCommand();
                Com1.Connection = Con1;
                Com1.Parameters.Clear();
                string sql = "T24_LoanAppAddLogAdd";
                Com1.CommandText = sql;
                Com1.CommandType = CommandType.StoredProcedure;
                Com1.Parameters.AddWithValue("@LoanAppID", LoanAppID);
                Com1.Parameters.AddWithValue("@LoanAppPersonID", LoanAppPersonID);
                Com1.Parameters.AddWithValue("@FName", fileHeader);
                Com1.Parameters.AddWithValue("@isError", isError);
                Com1.Parameters.AddWithValue("@ErrXml", ErrXml);
                Com1.Parameters.AddWithValue("@Remark", CBSKey);
                Com1.Parameters.AddWithValue("@CreateBy", CreateBy);
                Com1.ExecuteNonQuery();
                Con1.Close();
            }
            catch (Exception ex)
            {
                T24_AddLog(fileHeader, "T24_LoanAppAddLogAdd", ex.Message.ToString(), "LoanAdd");
            }
        }

        public string[,] LoadCsv(string filename)
        {
            // Get the file's text.
            string whole_file = "";// File.ReadAllText(filename);
            try
            {
                whole_file = File.ReadAllText(filename);
            }
            catch (Exception ex) { }

            // Split into lines.
            whole_file = whole_file.Replace("\r", "");
            whole_file = whole_file.Replace('\n', '\r');
            string[] lines = whole_file.Split(new char[] { '\r' },
                StringSplitOptions.RemoveEmptyEntries);

            // See how many rows and columns there are.
            int num_rows = lines.Length;
            int num_cols = lines[0].Split(',').Length;

            // Allocate the data array.
            string[,] values = new string[num_rows, num_cols];

            // Load the array.
            for (int r = 0; r < num_rows; r++)
            {
                string[] line_r = lines[r].Split(',');
                for (int c = 0; c < num_cols; c++)
                {
                    values[r, c] = line_r[c];
                }
            }

            // Return the values.
            return values;
        }

        //T24
        public string[] WriteCSVFile(DataTable dt, string fName, string T24Path, string T24PathUsername, string T24PathPwd, string T24Server)
        {
            string[] rs = new string[2];
            string err = "1", sms = "Succeed";
            try
            {
                ////Build the CSV file data as a Comma separated string.
                string csv = string.Empty;
                #region header
                //int r = 0;
                //int c = 0;
                ////Add the Header row for CSV file.
                //foreach (DataColumn column in dt.Columns)
                //{
                //    c++;
                //    if (c >= dt.Columns.Count) break;
                //    csv += column.ColumnName + ',';
                //}
                ////Add new line.
                //csv += "\r\n";
                #endregion header

                try
                {
                    //Adding the Rows
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            string txt = dt.Rows[i][j].ToString();
                            csv += txt.Replace(",", ";") + ',';
                        }
                        //Console.WriteLine(csv.ToString());
                        //Add new line.
                        csv += "\r\n";
                    }
                }
                catch (Exception ex)
                {
                    err = "0";
                    sms = "Error: read data: " + ex.Message.ToString();
                }

                #region Exporting to CSV.
                string filePath = AppDomain.CurrentDomain.BaseDirectory + "Uploads\\" + fName;
                if (err != "0")
                {
                    try
                    {
                        File.WriteAllText(filePath, csv);
                    }
                    catch (Exception ex)
                    {
                        err = "0";
                        sms = "Error: write data: " + ex.Message.ToString();
                    }
                }
                #endregion Exporting to CSV.

                //Copy to T24 Path
                if (err != "0")
                {
                    try
                    {
                        //File.Copy(filePath, T24Path, true);
                        string host = T24Server;
                        string username = T24PathUsername;
                        string password = T24PathPwd;
                        string downsavepath = filePath;
                        string remoteDirectory = T24Path;
                        using (var sftp = new SftpClient(host, username, password))
                        {
                            sftp.Connect();
                            var fileStream = new FileStream(downsavepath, FileMode.Open);
                            sftp.UploadFile(fileStream, remoteDirectory + fName, null);
                            sftp.Disconnect();
                            sftp.Dispose();
                        }
                    }
                    catch (Exception ex)
                    {
                        err = "0";
                        sms = "Error: copy data to T24: " + ex.Message.ToString();
                    }
                }

            }
            catch (Exception ex)
            {
                err = "0";
                sms = "Error: " + ex.Message.ToString();
            }
            rs[0] = err;
            rs[1] = sms;
            return rs;
        }
        public string ConvertDateFromYYYYMMDD(string str)
        {
            string rs = "";
            try
            {
                string y = str.Substring(0, 4);
                string m = str.Substring(4, 2);
                string d = str.Substring(6, 2);
                rs = y + "-" + m + "-" + d;
            }
            catch { }
            return rs;
        }

        public T GenerateXmlObject<T>(String xml)
        {
            T returnedXmlClass = default(T);

            try
            {
                using (TextReader reader = new StringReader(xml))
                {
                    try
                    {
                        returnedXmlClass = (T)new XmlSerializer(typeof(T)).Deserialize(reader);
                    }
                    catch (InvalidOperationException ex)
                    {
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return returnedXmlClass;
        }

        //V2
        public string[] AddLoanFromDevice(string IDOnDevice,string LoanAppID, string LoanAppStatusID,string DeviceDate,string ServerDate, string CreateBy
            ,string ProductID,string LoanRequestAmount,string OwnCapital,string DisbursementDate,string FirstWithdrawal
            ,string LoanTerm,string FirstRepaymentDate,string LoanInterestRate,string COProposedAmount
            ,string COProposedTerm,string COProposeRate,string GroupNumber,string LoanCycleID,string RepaymentHistoryID
            ,string DebtIinfoID,string Compulsory,string CompulsoryTerm, string Currency,string UpFrontFee,string UpFrontAmt
            ,string CompulsoryOptionID,string FundSource,string IsNewCollateral, string AgriBuddy,string semiBallonFreqID
            ,string PaymentMethodID,string LoanTypeID,string GracePeriodID,string MITypeID
            , string MonthlyFee,string CompititorRate,string CustomerConditionID,string LoanReferralID, string FrontBackOfficeID,string CollateralDebt) {

            string[] rs = new string[4];
            string rs0 = "Succeed"
                , rs1 = ""//SMS
                , rs2 = ""//KeyID
                , rs3 = "";//ExSMS
            try {
                SqlConnection Con1 = new SqlConnection(ConStr());
                Con1.Open();
                SqlCommand Com1 = new SqlCommand();
                Com1.Connection = Con1;
                Com1.Parameters.Clear();
                string sql = "sp_LoanApp1Add2_V2";
                Com1.CommandText = sql;
                Com1.CommandType = CommandType.StoredProcedure;
                Com1.Parameters.AddWithValue("@IDOnDevice", IDOnDevice);
                Com1.Parameters.AddWithValue("@LoanAppID", LoanAppID);
                Com1.Parameters.AddWithValue("@LoanAppStatusID", LoanAppStatusID);
                Com1.Parameters.AddWithValue("@DeviceDate", DeviceDate);
                Com1.Parameters.AddWithValue("@ServerDate", ServerDate);
                Com1.Parameters.AddWithValue("@CreateBy", CreateBy);
                Com1.Parameters.AddWithValue("@ProductID", ProductID);
                Com1.Parameters.AddWithValue("@LoanRequestAmount", LoanRequestAmount);
                //Com1.Parameters.AddWithValue("@LoanPurposeID1", null);
                //Com1.Parameters.AddWithValue("@LoadPurposeID2", null);
                //Com1.Parameters.AddWithValue("@LoadPurposeID3", null);
                Com1.Parameters.AddWithValue("@OwnCapital", OwnCapital);
                Com1.Parameters.AddWithValue("@DisbursementDate", DisbursementDate);
                Com1.Parameters.AddWithValue("@FirstWithdrawal", FirstWithdrawal);
                Com1.Parameters.AddWithValue("@LoanTerm", LoanTerm);
                Com1.Parameters.AddWithValue("@FirstRepaymentDate", FirstRepaymentDate);
                Com1.Parameters.AddWithValue("@LoanInterestRate", LoanInterestRate);
                Com1.Parameters.AddWithValue("@CustomerRequestRate", LoanInterestRate);//=LoanInterestRate
                Com1.Parameters.AddWithValue("@CompititorRate", CompititorRate);//not founnd
                Com1.Parameters.AddWithValue("@CustomerConditionID", CustomerConditionID);//not founnd
                Com1.Parameters.AddWithValue("@COProposedAmount", COProposedAmount);
                Com1.Parameters.AddWithValue("@COProposedTerm", COProposedTerm);
                Com1.Parameters.AddWithValue("@COProposeRate", COProposeRate);
                Com1.Parameters.AddWithValue("@FrontBackOfficeID", FrontBackOfficeID);//not founnd //
                Com1.Parameters.AddWithValue("@GroupNumber", GroupNumber);
                Com1.Parameters.AddWithValue("@LoanCycleID", LoanCycleID);
                Com1.Parameters.AddWithValue("@RepaymentHistoryID", RepaymentHistoryID);
                Com1.Parameters.AddWithValue("@LoanReferralID", LoanReferralID);//not founnd //
                Com1.Parameters.AddWithValue("@DebtIinfoID", DebtIinfoID);//if has creditor | 1 or 2
                Com1.Parameters.AddWithValue("@MonthlyFee", MonthlyFee);//not found //
                Com1.Parameters.AddWithValue("@Compulsory", Compulsory);//calculate
                Com1.Parameters.AddWithValue("@CompulsoryTerm", CompulsoryTerm);//
                Com1.Parameters.AddWithValue("@Currency", Currency);
                Com1.Parameters.AddWithValue("@UpFrontFee", UpFrontFee);
                Com1.Parameters.AddWithValue("@UpFrontAmt", UpFrontAmt);//calculate
                Com1.Parameters.AddWithValue("@CompulsoryOptionID", CompulsoryOptionID);
                Com1.Parameters.AddWithValue("@isCBCCheck", "0");
                Com1.Parameters.AddWithValue("@FundSource", FundSource);
                Com1.Parameters.AddWithValue("@IsNewCollateral", IsNewCollateral);
                Com1.Parameters.AddWithValue("@AgriBuddy", AgriBuddy);
                Com1.Parameters.AddWithValue("@semiBallonFreqID", semiBallonFreqID);
                Com1.Parameters.AddWithValue("@PaymentMethodID", PaymentMethodID);
                Com1.Parameters.AddWithValue("@LoanTypeID", LoanTypeID);
                Com1.Parameters.AddWithValue("@GracePeriodID", GracePeriodID);
                Com1.Parameters.AddWithValue("@MITypeID", MITypeID);
                Com1.Parameters.AddWithValue("@CollateralDebt", CollateralDebt);
                DataTable dt = new DataTable();
                dt.Load(Com1.ExecuteReader());
                rs0 = dt.Rows[0]["rs0"].ToString();
                rs1 = dt.Rows[0]["rs1"].ToString();
                rs2 = dt.Rows[0]["rs2"].ToString();
                Con1.Close();
            } catch (Exception ex) {
                rs0 = "Error";
                rs1 = "";//SMS
                rs2 = "";//KeyID
                rs3 = ex.Message.ToString();//ExSMS
            }
            rs[0] = rs0;
            rs[1] = rs1;
            rs[2] = rs2;
            rs[3] = rs3;
            return rs;
        }
        public string[] AddCustFromDevice(string LoanAppID, string LoanAppPersonTypeID,string CustomerID
            ,string VillageBankID,string NameKhLast,string TitleID,string LastName,string FirstName,string GenderID
            ,string DateOfBirth,string IDCardTypeID,string IDCardNumber,string IDCardExpireDate,string MaritalStatusID
            ,string EducationID,string CityOfBirthID,string Telephone3,string VillageIDPermanent,string VillageIDCurrent
            ,string SortAddress,string FamilyMember,string FamilyMemberActive,string PoorID,string LoanAppPersonIDOnDevice
            ,string DeviceDate,string IDCardIssuedDate,string NameKhFirst,string ProspectCode,string ReferByID
            ,string ReferName,string LatLon,string CustServerID,string IsNeedSaving,string Nationality)
        {

            string[] rs = new string[4];
            string rs0 = "Succeed"
                , rs1 = ""//SMS
                , rs2 = ""//KeyID
                , rs3 = "";//ExSMS
            try
            {
                SqlConnection Con1 = new SqlConnection(ConStr());
                Con1.Open();
                SqlCommand Com1 = new SqlCommand();
                Com1.Connection = Con1;
                Com1.Parameters.Clear();
                string sql = "sp_LoanAppPerson2Add2_V2";
                Com1.CommandText = sql;
                Com1.CommandType = CommandType.StoredProcedure;
                Com1.Parameters.AddWithValue("@LoanAppID", LoanAppID);
                Com1.Parameters.AddWithValue("@LoanAppPersonTypeID", LoanAppPersonTypeID);
                Com1.Parameters.AddWithValue("@CustomerID", CustomerID);
                Com1.Parameters.AddWithValue("@VillageBankID", VillageBankID);
                Com1.Parameters.AddWithValue("@AltName", NameKhLast);
                Com1.Parameters.AddWithValue("@TitleID", TitleID);
                Com1.Parameters.AddWithValue("@LastName", LastName);
                Com1.Parameters.AddWithValue("@FirstName", FirstName);
                Com1.Parameters.AddWithValue("@GenderID", GenderID);
                Com1.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
                //Com1.Parameters.AddWithValue("@IDCardTypeID", IDCardTypeID);
                //Com1.Parameters.AddWithValue("@IDCardNumber", IDCardNumber);
                //Com1.Parameters.AddWithValue("@IDCardExpireDate", IDCardExpireDate);
                Com1.Parameters.AddWithValue("@MaritalStatusID", MaritalStatusID);
                Com1.Parameters.AddWithValue("@EducationID", EducationID);
                Com1.Parameters.AddWithValue("@CityOfBirthID", CityOfBirthID);
                Com1.Parameters.AddWithValue("@Telephone3", Telephone3);
                Com1.Parameters.AddWithValue("@VillageIDPermanent", VillageIDPermanent);
                Com1.Parameters.AddWithValue("@VillageIDCurrent", VillageIDCurrent);
                Com1.Parameters.AddWithValue("@SortAddress", SortAddress);
                Com1.Parameters.AddWithValue("@FamilyMember", FamilyMember);
                Com1.Parameters.AddWithValue("@FamilyMemberActive", FamilyMemberActive);
                Com1.Parameters.AddWithValue("@PoorID", PoorID);
                Com1.Parameters.AddWithValue("@LoanAppPersonIDOnDevice", LoanAppPersonIDOnDevice);
                Com1.Parameters.AddWithValue("@DeviceDate", DeviceDate);
                //Com1.Parameters.AddWithValue("@IDCardIssuedDate", IDCardIssuedDate);
                Com1.Parameters.AddWithValue("@AltName2", NameKhFirst);
                Com1.Parameters.AddWithValue("@ProspectCode", ProspectCode);
                Com1.Parameters.AddWithValue("@ReferByID", ReferByID);
                Com1.Parameters.AddWithValue("@ReferName", ReferName);
                Com1.Parameters.AddWithValue("@LatLon", LatLon);
                Com1.Parameters.AddWithValue("@CustServerID", CustServerID);
                Com1.Parameters.AddWithValue("@IsNeedSaving", IsNeedSaving);
                Com1.Parameters.AddWithValue("@Nationality", Nationality);
                DataTable dt = new DataTable();
                dt.Load(Com1.ExecuteReader());
                rs2 = dt.Rows[0][0].ToString();
                Con1.Close();
            }
            catch (Exception ex)
            {
                rs0 = "Error";
                rs1 = "";//SMS
                rs2 = "";//KeyID
                rs3 = ex.Message.ToString();//ExSMS
            }
            rs[0] = rs0;
            rs[1] = rs1;
            rs[2] = rs2;
            rs[3] = rs3;
            return rs;
        }
        public int AddCustOccupation(string LoanAppPersonId,string OccupationId)
        {
            try
            {
                SqlConnection Con1 = new SqlConnection(ConStr());
                Con1.Open();
                SqlCommand Com1 = new SqlCommand();
                Com1.Connection = Con1;
                Com1.Parameters.Clear();
                string sql = "sp_LoanAppPersonOccupation";
                Com1.CommandText = sql;
                Com1.CommandType = CommandType.StoredProcedure;
                Com1.Parameters.AddWithValue("@LoanAppPersonId", LoanAppPersonId);
                Com1.Parameters.AddWithValue("@OccupationId", OccupationId);
                DataTable dt = new DataTable();
                dt.Load(Com1.ExecuteReader());
                var rs_id = dt.Rows[0][0];
                Con1.Close();
                return int.Parse(rs_id.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Customer occupation cannot add: {ex.Message.ToString()}");
                return 0;
            }
       
         
        }
        public int AddCustOccupationDetail(int PersonOccupationId,int OccupationDetailId)
        {

            try
            {
                SqlConnection Con1 = new SqlConnection(ConStr());
                Con1.Open();
                SqlCommand Com1 = new SqlCommand();
                Com1.Connection = Con1;
                Com1.Parameters.Clear();
                string sql = "sp_LoanAppPersonOccupationDetail";
                Com1.CommandText = sql;
                Com1.CommandType = CommandType.StoredProcedure;
                Com1.Parameters.AddWithValue("@PersonOccupationId", PersonOccupationId);
                Com1.Parameters.AddWithValue("@OccupationDetailId", OccupationDetailId);
                DataTable dt = new DataTable();
                dt.Load(Com1.ExecuteReader());
                var rs = dt.Rows[0][0];
                Con1.Close();
                return int.Parse(rs.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message.ToString()}");
                return 0;
            }
           
        }
        public string[] AddCustImgFromDevice(string LoanAppPersonID,string OneCardTwoDoc,string Ext,string ImgPath,string LoanAppID,string LoanAppPersonImageIDOnDevice)
        {
            string[] rs = new string[4];
            string rs0 = "Succeed"
                , rs1 = ""//SMS
                , rs2 = ""//KeyID
                , rs3 = "";//ExSMS
            try
            {
                SqlConnection Con1 = new SqlConnection(ConStr());
                Con1.Open();
                SqlCommand Com1 = new SqlCommand();
                Com1.Connection = Con1;
                Com1.Parameters.Clear();
                string sql = "sp_LoanAppPerson21ImageImageAdd2_V2";
                Com1.CommandText = sql;
                Com1.CommandType = CommandType.StoredProcedure;
                Com1.Parameters.AddWithValue("@LoanAppPersonID", LoanAppPersonID);
                Com1.Parameters.AddWithValue("@OneCardTwoDoc", OneCardTwoDoc);
                Com1.Parameters.AddWithValue("@Ext", Ext);
                Com1.Parameters.AddWithValue("@ImgPath", ImgPath);
                Com1.Parameters.AddWithValue("@LoanAppID", LoanAppID);
                Com1.Parameters.AddWithValue("@LoanAppPersonImageIDOnDevice", LoanAppPersonImageIDOnDevice);
                DataTable dt = new DataTable();
                dt.Load(Com1.ExecuteReader());
                rs2 = dt.Rows[0][0].ToString();
                Con1.Close();
            }
            catch (Exception ex)
            {
                rs0 = "Error";
                rs1 = "";//SMS
                rs2 = "";//KeyID
                rs3 = ex.Message.ToString();//ExSMS
            }
            rs[0] = rs0;
            rs[1] = rs1;
            rs[2] = rs2;
            rs[3] = rs3;
            return rs;
        }
        public string[] AddCustAssetFromDevice(string LoanAppPersonID, string Description, string Quantity, string UnitPrice, string LoanAppClientAssetIDOnDevice, string LoanAppID,string asset_lookupID,string assetOtherDescription,string unit)
        {

            string[] rs = new string[4];
            string rs0 = "Succeed"
                , rs1 = ""//SMS
                , rs2 = ""//KeyID
                , rs3 = "";//ExSMS
            try
            {
                SqlConnection Con1 = new SqlConnection(ConStr());
                Con1.Open();
                SqlCommand Com1 = new SqlCommand();
                Com1.Connection = Con1;
                Com1.Parameters.Clear();
                string sql = "sp_LoanApp13ClientAssetAdd2_V2";
                Com1.CommandText = sql;
                Com1.CommandType = CommandType.StoredProcedure;
                Com1.Parameters.AddWithValue("@LoanAppPersonID", LoanAppPersonID);
                Com1.Parameters.AddWithValue("@Description", Description);
                Com1.Parameters.AddWithValue("@Quantity", Quantity);
                Com1.Parameters.AddWithValue("@UnitPrice", UnitPrice);
                Com1.Parameters.AddWithValue("@LoanAppClientAssetIDOnDevice", LoanAppClientAssetIDOnDevice);
                Com1.Parameters.AddWithValue("@LoanAppID", LoanAppID);
                Com1.Parameters.AddWithValue("@asset_lookupID", asset_lookupID);
                Com1.Parameters.AddWithValue("@assetOtherDescription", assetOtherDescription);
                Com1.Parameters.AddWithValue("@Unit", unit);

                DataTable dt = new DataTable();
                dt.Load(Com1.ExecuteReader());
                rs2 = dt.Rows[0][0].ToString();
                Con1.Close();
            }
            catch (Exception ex)
            {
                rs0 = "Error";
                rs1 = "";//SMS
                rs2 = "";//KeyID
                rs3 = ex.Message.ToString();//ExSMS
            }
            rs[0] = rs0;
            rs[1] = rs1;
            rs[2] = rs2;
            rs[3] = rs3;
            return rs;
        }
        public string[] AddCustAsseImgFromDevice(string AssetImageClientID, string AssetServerID, string CreateDateClient, string Ext, string ImgPath)
        {

            string[] rs = new string[4];
            string rs0 = "Succeed"
                , rs1 = ""//SMS
                , rs2 = ""//KeyID
                , rs3 = "";//ExSMS
            try
            {
                SqlConnection Con1 = new SqlConnection(ConStr());
                Con1.Open();
                SqlCommand Com1 = new SqlCommand();
                Com1.Connection = Con1;
                Com1.Parameters.Clear();
                string sql = "sp_LoanApp13ClientAssetImgAdd2_V2";
                Com1.CommandText = sql;
                Com1.CommandType = CommandType.StoredProcedure;
                Com1.Parameters.AddWithValue("@AssetImageClientID", AssetImageClientID);
                Com1.Parameters.AddWithValue("@AssetServerID", AssetServerID);
                Com1.Parameters.AddWithValue("@CreateDateClient", CreateDateClient);
                Com1.Parameters.AddWithValue("@Ext", Ext);
                Com1.Parameters.AddWithValue("@ImgPath", ImgPath);
                DataTable dt = new DataTable();
                dt.Load(Com1.ExecuteReader());
                rs2 = dt.Rows[0][0].ToString();
                Con1.Close();
            }
            catch (Exception ex)
            {
                rs0 = "Error";
                rs1 = "";//SMS
                rs2 = "";//KeyID
                rs3 = ex.Message.ToString();//ExSMS
            }
            rs[0] = rs0;
            rs[1] = rs1;
            rs[2] = rs2;
            rs[3] = rs3;
            return rs;
        }
        public string[] AddCustCreditorFromDevice(string CreditorClientID, string CreditorID, string Currency, string ApprovedAmount, string OutstandingBalance
            ,string InterestRate,string RepaymentTypeID,string RepaymentTermID,string LoanStartDate,string LoanEndDate,string IsReFinance,string ReFinanceAmount
            ,string ServerLoanAppPersonID,string ServerLoanAppID,string CreditorName)
        {

            string[] rs = new string[4];
            string rs0 = "Succeed"
                , rs1 = ""//SMS
                , rs2 = ""//KeyID
                , rs3 = "";//ExSMS
            try
            {
                SqlConnection Con1 = new SqlConnection(ConStr());
                Con1.Open();
                SqlCommand Com1 = new SqlCommand();
                Com1.Connection = Con1;
                Com1.Parameters.Clear();
                string sql = "sp_LoanApp12CreditorAdd2_V2";
                Com1.CommandText = sql;
                Com1.CommandType = CommandType.StoredProcedure;
                Com1.Parameters.AddWithValue("@CreditorClientID", CreditorClientID);
                Com1.Parameters.AddWithValue("@CreditorID", CreditorID);
                Com1.Parameters.AddWithValue("@Currency", Currency);
                Com1.Parameters.AddWithValue("@ApprovedAmount", ApprovedAmount);
                Com1.Parameters.AddWithValue("@OutstandingBalance", OutstandingBalance);
                Com1.Parameters.AddWithValue("@InterestRate", InterestRate);
                Com1.Parameters.AddWithValue("@RepaymentTypeID", RepaymentTypeID);
                Com1.Parameters.AddWithValue("@RepaymentTermID", RepaymentTermID);
                Com1.Parameters.AddWithValue("@LoanStartDate", LoanStartDate);
                Com1.Parameters.AddWithValue("@LoanEndDate", LoanEndDate);
                Com1.Parameters.AddWithValue("@IsReFinance", IsReFinance);
                Com1.Parameters.AddWithValue("@ReFinanceAmount", ReFinanceAmount);
                Com1.Parameters.AddWithValue("@ServerLoanAppPersonID", ServerLoanAppPersonID);
                Com1.Parameters.AddWithValue("@ServerLoanAppID", ServerLoanAppID);
                Com1.Parameters.AddWithValue("@CreditorName", CreditorName);
                DataTable dt = new DataTable();
                dt.Load(Com1.ExecuteReader());
                rs2 = dt.Rows[0][0].ToString();
                Con1.Close();
            }
            catch (Exception ex)
            {
                rs0 = "Error";
                rs1 = "";//SMS
                rs2 = "";//KeyID
                rs3 = ex.Message.ToString();//ExSMS
            }
            rs[0] = rs0;
            rs[1] = rs1;
            rs[2] = rs2;
            rs[3] = rs3;
            return rs;
        }
        public string[] AddCustRealEstateFromDevice(string CollateralClientID, string LoanClientID, string LoanAppID, string CustClientID, string CustServerID
            , string CollateralDocGroupTypeID, string CollateralDocHardTypeID, string CollateralDocSoftTypeID, string CollateralOwnerTypeID, string CollateralLocationVillageID
            , string CollateralRoadAccessibilityID, string PropertyTypeID,string LandTypeID,string LandSize,string LandMarketPrice,string LandForcedSalePrice
            , string BuildingTypeID, string BuildingSize,string BuildingMarketPrice,string BuildingForcedSalesPrice)
        {

            string[] rs = new string[4];
            string rs0 = "Succeed"
                , rs1 = ""//SMS
                , rs2 = ""//KeyID
                , rs3 = "";//ExSMS
            try
            {
                SqlConnection Con1 = new SqlConnection(ConStr());
                Con1.Open();
                SqlCommand Com1 = new SqlCommand();
                Com1.Connection = Con1;
                Com1.Parameters.Clear();
                string sql = "sp_LoanApp15ClientCollateralRealEstate_V2";
                Com1.CommandText = sql;
                Com1.CommandType = CommandType.StoredProcedure;
                Com1.Parameters.AddWithValue("@CollateralClientID", CollateralClientID);
                Com1.Parameters.AddWithValue("@LoanClientID", LoanClientID);
                Com1.Parameters.AddWithValue("@LoanAppID", LoanAppID);
                Com1.Parameters.AddWithValue("@CustClientID", CustClientID);
                Com1.Parameters.AddWithValue("@CustServerID", CustServerID);
                Com1.Parameters.AddWithValue("@CollateralDocGroupTypeID", CollateralDocGroupTypeID);
                Com1.Parameters.AddWithValue("@CollateralDocHardTypeID", CollateralDocHardTypeID);
                Com1.Parameters.AddWithValue("@CollateralDocSoftTypeID", CollateralDocSoftTypeID);
                Com1.Parameters.AddWithValue("@CollateralOwnerTypeID", CollateralOwnerTypeID);
                Com1.Parameters.AddWithValue("@CollateralLocationVillageID", CollateralLocationVillageID);
                Com1.Parameters.AddWithValue("@CollateralRoadAccessibilityID", CollateralRoadAccessibilityID);
                Com1.Parameters.AddWithValue("@PropertyTypeID", PropertyTypeID);
                Com1.Parameters.AddWithValue("@LandTypeID", LandTypeID);
                Com1.Parameters.AddWithValue("@LandSize", LandSize);
                Com1.Parameters.AddWithValue("@LandMarketPrice", LandMarketPrice);
                Com1.Parameters.AddWithValue("@LandForcedSalePrice", LandForcedSalePrice);
                Com1.Parameters.AddWithValue("@BuildingTypeID", BuildingTypeID);
                Com1.Parameters.AddWithValue("@BuildingSize", BuildingSize);
                Com1.Parameters.AddWithValue("@BuildingMarketPrice", BuildingMarketPrice);
                Com1.Parameters.AddWithValue("@BuildingForcedSalesPrice", BuildingForcedSalesPrice);
                DataTable dt = new DataTable();
                dt.Load(Com1.ExecuteReader());
                rs2 = dt.Rows[0][0].ToString();
                Con1.Close();
            }
            catch (Exception ex)
            {
                rs0 = "Error";
                rs1 = "";//SMS
                rs2 = "";//KeyID
                rs3 = ex.Message.ToString();//ExSMS
            }
            rs[0] = rs0;
            rs[1] = rs1;
            rs[2] = rs2;
            rs[3] = rs3;
            return rs;
        }
        public string[] AddCustRealEstateImgFromDevice(string ImageClientID, string CollateralServerID, string CollateralClientID, string CreateDateClient, string Ext, string ImgPath)
        {

            string[] rs = new string[4];
            string rs0 = "Succeed"
                , rs1 = ""//SMS
                , rs2 = ""//KeyID
                , rs3 = "";//ExSMS
            try
            {
                SqlConnection Con1 = new SqlConnection(ConStr());
                Con1.Open();
                SqlCommand Com1 = new SqlCommand();
                Com1.Connection = Con1;
                Com1.Parameters.Clear();
                string sql = "sp_LoanApp15ClientCollateralRealEstateImg_V2";
                Com1.CommandText = sql;
                Com1.CommandType = CommandType.StoredProcedure;
                Com1.Parameters.AddWithValue("@ImageClientID", ImageClientID);
                Com1.Parameters.AddWithValue("@CollateralServerID", CollateralServerID);
                Com1.Parameters.AddWithValue("@CollateralClientID", CollateralClientID);
                Com1.Parameters.AddWithValue("@CreateDateClient", CreateDateClient);
                Com1.Parameters.AddWithValue("@Ext", Ext);
                Com1.Parameters.AddWithValue("@ImgPath", ImgPath);
                DataTable dt = new DataTable();
                dt.Load(Com1.ExecuteReader());
                rs2 = dt.Rows[0][0].ToString();
                Con1.Close();
            }
            catch (Exception ex)
            {
                rs0 = "Error";
                rs1 = "";//SMS
                rs2 = "";//KeyID
                rs3 = ex.Message.ToString();//ExSMS
            }
            rs[0] = rs0;
            rs[1] = rs1;
            rs[2] = rs2;
            rs[3] = rs3;
            return rs;
        }
        public string[] AddCustDepositFromDevice(string CollateralClientID, string LoanAppID, string LoanClientID
            , string CustServerID, string CustClientID, string FixedDepositAccountNo, string StartDate
            , string MaturityDate, string Amount, string AccountOwnerName, string Currency, string RelationshipID
            , string DOB, string GenderID, string NIDNo, string IssueDate, string IssuedBy, string SortAddress, string VillageID)
        {

            string[] rs = new string[4];
            string rs0 = "Succeed"
                , rs1 = ""//SMS
                , rs2 = ""//KeyID
                , rs3 = "";//ExSMS
            try
            {
                SqlConnection Con1 = new SqlConnection(ConStr());
                Con1.Open();
                SqlCommand Com1 = new SqlCommand();
                Com1.Connection = Con1;
                Com1.Parameters.Clear();
                string sql = "sp_LoanApp15ClientCollateralDeposit_V2";
                Com1.CommandText = sql;
                Com1.CommandType = CommandType.StoredProcedure;
                Com1.Parameters.AddWithValue("@CollateralClientID", CollateralClientID);
                Com1.Parameters.AddWithValue("@LoanAppID", LoanAppID);
                Com1.Parameters.AddWithValue("@LoanClientID", LoanClientID);
                Com1.Parameters.AddWithValue("@CustServerID", CustServerID);
                Com1.Parameters.AddWithValue("@CustClientID", CustClientID);
                Com1.Parameters.AddWithValue("@FixedDepositAccountNo", FixedDepositAccountNo);
                Com1.Parameters.AddWithValue("@StartDate", StartDate);
                Com1.Parameters.AddWithValue("@MaturityDate", MaturityDate);
                Com1.Parameters.AddWithValue("@Amount", Amount);
                Com1.Parameters.AddWithValue("@AccountOwnerName", AccountOwnerName);
                Com1.Parameters.AddWithValue("@Currency", Currency);
                Com1.Parameters.AddWithValue("@RelationshipID", RelationshipID);
                Com1.Parameters.AddWithValue("@DOB", DOB);
                Com1.Parameters.AddWithValue("@GenderID", GenderID);
                Com1.Parameters.AddWithValue("@NIDNo", NIDNo);
                Com1.Parameters.AddWithValue("@IssueDate", IssueDate);
                Com1.Parameters.AddWithValue("@IssuedBy", IssuedBy);
                Com1.Parameters.AddWithValue("@SortAddress", SortAddress);
                Com1.Parameters.AddWithValue("@VillageID", VillageID);
                DataTable dt = new DataTable();
                dt.Load(Com1.ExecuteReader());
                rs2 = dt.Rows[0][0].ToString();
                Con1.Close();
            }
            catch (Exception ex)
            {
                rs0 = "Error";
                rs1 = "";//SMS
                rs2 = "";//KeyID
                rs3 = ex.Message.ToString();//ExSMS
            }
            rs[0] = rs0;
            rs[1] = rs1;
            rs[2] = rs2;
            rs[3] = rs3;
            return rs;
        }
        public string[] AddLoanApp11PurpsoeFromDevice(string LoanPurposeClientID, string LoanAppID, string LoanPurposeID)
        {
            string[] rs = new string[4];
            string rs0 = "Succeed"
                , rs1 = ""//SMS
                , rs2 = ""//KeyID
                , rs3 = "";//ExSMS
            try
            {
                SqlConnection Con1 = new SqlConnection(ConStr());
                Con1.Open();
                SqlCommand Com1 = new SqlCommand();
                Com1.Connection = Con1;
                Com1.Parameters.Clear();
                string sql = "sp_LoanApp11Purpsoe_V2";
                Com1.CommandText = sql;
                Com1.CommandType = CommandType.StoredProcedure;
                Com1.Parameters.AddWithValue("@LoanPurposeClientID", LoanPurposeClientID);
                Com1.Parameters.AddWithValue("@LoanAppID", LoanAppID);
                Com1.Parameters.AddWithValue("@LoanPurposeID", LoanPurposeID);
                DataTable dt = new DataTable();
                dt.Load(Com1.ExecuteReader());
                rs2 = dt.Rows[0][0].ToString();
                Con1.Close();
            }
            catch (Exception ex)
            {
                rs0 = "Error";
                rs1 = "";//SMS
                rs2 = "";//KeyID
                rs3 = ex.Message.ToString();//ExSMS
            }
            rs[0] = rs0;
            rs[1] = rs1;
            rs[2] = rs2;
            rs[3] = rs3;
            return rs;
        }
        public string[] AddLoanApp11PurpsoeDetailFromDevice(string LoanPurposeClientDetailID, string LoanAppID
            , string LoanAppPurpsoeDetail,string Quantity,string UnitPrice,string LoanPurposeServerID)
        {
            string[] rs = new string[4];
            string rs0 = "Succeed"
                , rs1 = ""//SMS
                , rs2 = ""//KeyID
                , rs3 = "";//ExSMS
            try
            {
                SqlConnection Con1 = new SqlConnection(ConStr());
                Con1.Open();
                SqlCommand Com1 = new SqlCommand();
                Com1.Connection = Con1;
                Com1.Parameters.Clear();
                string sql = "sp_LoanApp11PurpsoeDetail_V2";
                Com1.CommandText = sql;
                Com1.CommandType = CommandType.StoredProcedure;
                Com1.Parameters.AddWithValue("@LoanPurposeClientDetailID", LoanPurposeClientDetailID);
                Com1.Parameters.AddWithValue("@LoanAppID", LoanAppID);
                Com1.Parameters.AddWithValue("@LoanAppPurpsoeDetail", LoanAppPurpsoeDetail);
                Com1.Parameters.AddWithValue("@Quantity", Quantity);
                Com1.Parameters.AddWithValue("@UnitPrice", UnitPrice);
                Com1.Parameters.AddWithValue("@LoanPurposeServerID", LoanPurposeServerID);
                DataTable dt = new DataTable();
                dt.Load(Com1.ExecuteReader());
                rs2 = dt.Rows[0][0].ToString();
                Con1.Close();
            }
            catch (Exception ex)
            {
                rs0 = "Error";
                rs1 = "";//SMS
                rs2 = "";//KeyID
                rs3 = ex.Message.ToString();//ExSMS
            }
            rs[0] = rs0;
            rs[1] = rs1;
            rs[2] = rs2;
            rs[3] = rs3;
            return rs;
        }
        public string[] AddLoanApp51CashFlowFromDevice(string LoanAppID,string CashFlowClientID
            , string StudyMonthAmount, string StudyStartMonth, string FamilyExpensePerMonth, string OtherExpensePerMonth)
        {
            string[] rs = new string[4];
            string rs0 = "Succeed"
                , rs1 = ""//SMS
                , rs2 = ""//KeyID
                , rs3 = "";//ExSMS
            try
            {
                SqlConnection Con1 = new SqlConnection(ConStr());
                Con1.Open();
                SqlCommand Com1 = new SqlCommand();
                Com1.Connection = Con1;
                Com1.Parameters.Clear();
                string sql = "sp_LoanApp51CashFlowAdd2_V2";
                Com1.CommandText = sql;
                Com1.CommandType = CommandType.StoredProcedure;
                Com1.Parameters.AddWithValue("@LoanAppID", LoanAppID);
                Com1.Parameters.AddWithValue("@LoanAppCashFlowIDOnDevice", CashFlowClientID);
                Com1.Parameters.AddWithValue("@StudyMonthAmount", StudyMonthAmount);
                Com1.Parameters.AddWithValue("@StudyStartMonth", StudyStartMonth);
                Com1.Parameters.AddWithValue("@FamilyExpensePerMonth", FamilyExpensePerMonth);
                Com1.Parameters.AddWithValue("@OtherExpensePerMonth", OtherExpensePerMonth);
                DataTable dt = new DataTable();
                dt.Load(Com1.ExecuteReader());
                rs2 = dt.Rows[0][0].ToString();
                Con1.Close();
            }
            catch (Exception ex)
            {
                rs0 = "Error";
                rs1 = "";//SMS
                rs2 = "";//KeyID
                rs3 = ex.Message.ToString();//ExSMS
            }
            rs[0] = rs0;
            rs[1] = rs1;
            rs[2] = rs2;
            rs[3] = rs3;
            return rs;
        }
        public string[] AddLoanAppCashFlowMSIFromDevice(string ServerLoanAppCashFlowID, string IncomeTypeID
            , string MainSourceIncomeID, string Remark, string Quantity, string ExAge,string BusAge,string isMSI
            ,string OccupationTypeID, string IncomeOwnerID,string WorkingPlaceID,string PhoneNumber,string Name)
        {
            string[] rs = new string[4];
            string rs0 = "Succeed"
                , rs1 = ""//SMS
                , rs2 = ""//KeyID
                , rs3 = "";//ExSMS
            try
            {
                SqlConnection Con1 = new SqlConnection(ConStr());
                Con1.Open();
                SqlCommand Com1 = new SqlCommand();
                Com1.Connection = Con1;
                Com1.Parameters.Clear();
                string sql = "sp_LoanAppCashFlowMSIAdd_V2";
                Com1.CommandText = sql;
                Com1.CommandType = CommandType.StoredProcedure;
                Com1.Parameters.AddWithValue("@ServerLoanAppCashFlowID", ServerLoanAppCashFlowID);
                Com1.Parameters.AddWithValue("@IncomeTypeID", IncomeTypeID);
                Com1.Parameters.AddWithValue("@MainSourceIncomeID", MainSourceIncomeID);
                Com1.Parameters.AddWithValue("@Remark", Remark);
                Com1.Parameters.AddWithValue("@Quantity", Quantity);
                Com1.Parameters.AddWithValue("@ExAge", ExAge);
                Com1.Parameters.AddWithValue("@BusAge", BusAge);
                Com1.Parameters.AddWithValue("@isMSI", isMSI);
                Com1.Parameters.AddWithValue("@OccupationTypeID", OccupationTypeID);
                Com1.Parameters.AddWithValue("@IncomeOwnerID", IncomeOwnerID);
                Com1.Parameters.AddWithValue("@WorkingPlaceID", WorkingPlaceID);
                Com1.Parameters.AddWithValue("@PhoneNumber", PhoneNumber);
                Com1.Parameters.AddWithValue("@Name", Name);
                DataTable dt = new DataTable();
                dt.Load(Com1.ExecuteReader());
                rs2 = dt.Rows[0][0].ToString();
                Con1.Close();
            }
            catch (Exception ex)
            {
                rs0 = "Error";
                rs1 = "";//SMS
                rs2 = "";//KeyID
                rs3 = ex.Message.ToString();//ExSMS
            }
            rs[0] = rs0;
            rs[1] = rs1;
            rs[2] = rs2;
            rs[3] = rs3;
            return rs;
        }
        public string[] AddLoanAppCashFlowMSIInExFromDevice(string LoanAppCashFlowMSIInExClientID, string LoanAppCashFlowMSIID
            , string InExCodeID, string Description, string Month, string Amount, string UnitID, string Cost, string OneIncomeTwoExpense)
        {
            string[] rs = new string[4];
            string rs0 = "Succeed"
                , rs1 = ""//SMS
                , rs2 = ""//KeyID
                , rs3 = "";//ExSMS
            try
            {
                SqlConnection Con1 = new SqlConnection(ConStr());
                Con1.Open();
                SqlCommand Com1 = new SqlCommand();
                Com1.Connection = Con1;
                Com1.Parameters.Clear();
                string sql = "sp_LoanAppCashFlowMSIInExAdd_V2";
                Com1.CommandText = sql;
                Com1.CommandType = CommandType.StoredProcedure;
                Com1.Parameters.AddWithValue("@LoanAppCashFlowMSIInExClientID", LoanAppCashFlowMSIInExClientID);
                Com1.Parameters.AddWithValue("@LoanAppCashFlowMSIID", LoanAppCashFlowMSIID);
                Com1.Parameters.AddWithValue("@InExCodeID", InExCodeID);
                Com1.Parameters.AddWithValue("@Description", Description);
                Com1.Parameters.AddWithValue("@Month", Month);
                Com1.Parameters.AddWithValue("@Amount", Amount);
                Com1.Parameters.AddWithValue("@UnitID", UnitID);
                Com1.Parameters.AddWithValue("@Cost", Cost);
                Com1.Parameters.AddWithValue("@OneIncomeTwoExpense", OneIncomeTwoExpense);
                DataTable dt = new DataTable();
                dt.Load(Com1.ExecuteReader());
                rs2 = dt.Rows[0][0].ToString();
                Con1.Close();
            }
            catch (Exception ex)
            {
                rs0 = "Error";
                rs1 = "";//SMS
                rs2 = "";//KeyID
                rs3 = ex.Message.ToString();//ExSMS
            }
            rs[0] = rs0;
            rs[1] = rs1;
            rs[2] = rs2;
            rs[3] = rs3;
            return rs;
        }

        #region New Function to create customer and loan
        public string[] AddEditCustomerV2(string fileHeader, string LoanAppPersonID, string LoanAppPersonTypeID)
        {
            string[] rs = new string[2];
            string ERR = "Error", SMS = "", rsContent = "", xmlContent="";
            try
            {
                fileHeader = fileHeader.Replace(" ", "_").Replace("-", "_").Replace(":", "_").Replace(".", "_");
                string sql = "exec T24_LoanAppListForOpenToCBS2_P2_Cust @LoanAppPersonID='" + LoanAppPersonID + "'";
                DataTable dt = ReturnDT(sql);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    #region params
                    string CreUrl = dt.Rows[i]["CreUrl"].ToString();
                    string CreCompany = dt.Rows[i]["CreCompany"].ToString();
                    string CreUserName = dt.Rows[i]["CreUserName"].ToString();
                    string CrePassword = dt.Rows[i]["CrePassword"].ToString();

                    //string LoanAppPersonID = dt.Rows[i]["LoanAppPersonID"].ToString();
                    string LoanAppID = dt.Rows[i]["LoanAppID"].ToString();
                    string CustomerID = dt.Rows[i]["CustomerID"].ToString();
                    string SHORTNAME = dt.Rows[i]["SHORTNAME"].ToString();
                    string NAME1 = dt.Rows[i]["NAME1"].ToString();
                    string STREET = dt.Rows[i]["STREET"].ToString();
                    string RELATIONCODE = dt.Rows[i]["RELATIONCODE"].ToString();
                    string RELCUSTOMER = dt.Rows[i]["RELCUSTOMER"].ToString();
                    string SECTOR = dt.Rows[i]["SECTOR"].ToString();
                    string ACCOUNTOFFICER = dt.Rows[i]["ACCOUNTOFFICER"].ToString();
                    string INDUSTRY = dt.Rows[i]["INDUSTRY"].ToString();
                    string TARGET = dt.Rows[i]["TARGET"].ToString();
                    string NATIONALITY = dt.Rows[i]["NATIONALITY"].ToString();
                    string CUSTOMERSTATUS = dt.Rows[i]["CUSTOMERSTATUS"].ToString();
                    string RESIDENCE = dt.Rows[i]["RESIDENCE"].ToString();
                    string BIRTHINCORPDATE = dt.Rows[i]["BIRTHINCORPDATE"].ToString();
                    string COMPANYBOOK = dt.Rows[i]["COMPANYBOOK"].ToString();
                    string TITLE = dt.Rows[i]["TITLE"].ToString();
                    string GENDER = dt.Rows[i]["GENDER"].ToString();
                    string MARITALSTATUS = dt.Rows[i]["MARITALSTATUS"].ToString();
                    string OCCUPATION = dt.Rows[i]["OCCUPATION"].ToString();
                    string FURTHERDETAILS = dt.Rows[i]["FURTHERDETAILS"].ToString();
                    string PostalCode = dt.Rows[i]["PostalCode"].ToString();
                    if (PostalCode == "")
                    {
                        PostalCode = "";
                    }
                    else
                    {
                        PostalCode = "<cus:PostalCode>" + PostalCode + "</cus:PostalCode>";
                    }
                    string EmployersAddress = dt.Rows[i]["EmployersAddress"].ToString();
                    string HomePhoneNo = dt.Rows[i]["HomePhoneNo"].ToString();
                    string WorkPhoneNo = dt.Rows[i]["WorkPhoneNo"].ToString();
                    string MobilePhoneNo = dt.Rows[i]["MobilePhoneNo"].ToString();
                    string ResidenceYN = dt.Rows[i]["ResidenceYN"].ToString();
                    string EmailAddress = dt.Rows[i]["EmailAddress"].ToString();
                    string EmployerName = dt.Rows[i]["EmployerName"].ToString();
                    string TaxpayerRegistrationNo = dt.Rows[i]["TaxpayerRegistrationNo"].ToString();
                    string BlockedYN = dt.Rows[i]["BlockedYN"].ToString();
                    string AMKStaff = dt.Rows[i]["AMKStaff"].ToString();
                    string NoFamilyMember = dt.Rows[i]["NoFamilyMember"].ToString();
                    string EmployeeCode = dt.Rows[i]["EmployeeCode"].ToString();
                    string DateofEmployment = dt.Rows[i]["DateofEmployment"].ToString();
                    string FaxNo = dt.Rows[i]["FaxNo"].ToString();
                    string Profession = dt.Rows[i]["Profession"].ToString();
                    string EmploymentStatus = dt.Rows[i]["EmploymentStatus"].ToString();
                    string MainSourceofIncome = dt.Rows[i]["MainSourceofIncome"].ToString();
                    string EmployersCode = dt.Rows[i]["EmployersCode"].ToString();
                    string SpouseName = dt.Rows[i]["SpouseName"].ToString();
                    string SpouseOccupation = dt.Rows[i]["SpouseOccupation"].ToString();
                    string ConsenttoDisclosure = dt.Rows[i]["ConsenttoDisclosure"].ToString();
                    string DateofSignature = dt.Rows[i]["DateofSignature"].ToString();
                    string PlaceofStudy = dt.Rows[i]["PlaceofStudy"].ToString();
                    string DurationofCourse = dt.Rows[i]["DurationofCourse"].ToString();
                    string FieldofStudy = dt.Rows[i]["FieldofStudy"].ToString();
                    string NominationForm = dt.Rows[i]["NominationForm"].ToString();
                    string NominationCustomer = dt.Rows[i]["NominationCustomer"].ToString();
                    string NominationBeneficiary = dt.Rows[i]["NominationBeneficiary"].ToString();
                    string NominationAddress1 = dt.Rows[i]["NominationAddress1"].ToString();
                    string NominationAddress2 = dt.Rows[i]["NominationAddress2"].ToString();
                    string NominationAddress3 = dt.Rows[i]["NominationAddress3"].ToString();
                    string NominationAddress4 = dt.Rows[i]["NominationAddress4"].ToString();
                    string INSPostalCode = dt.Rows[i]["INSPostalCode"].ToString();
                    if (INSPostalCode == "")
                    {
                        INSPostalCode = "";
                    }
                    else
                    {
                        INSPostalCode = "<cus:INSPostalCode>" + INSPostalCode + "</cus:INSPostalCode>";
                    }
                    string INSPhoneNo = dt.Rows[i]["INSPhoneNo"].ToString();
                    string RelationshiptoCustomer = dt.Rows[i]["RelationshiptoCustomer"].ToString();
                    string AmountorofLegacy = dt.Rows[i]["AmountorofLegacy"].ToString();
                    string NameofBusiness = dt.Rows[i]["NameofBusiness"].ToString();
                    string NatureofBusiness = dt.Rows[i]["NatureofBusiness"].ToString();
                    string BusinessType = dt.Rows[i]["BusinessType"].ToString();
                    string BusinessPlan = dt.Rows[i]["BusinessPlan"].ToString();
                    string RoleinBusiness = dt.Rows[i]["RoleinBusiness"].ToString();
                    string BusinessAddress = dt.Rows[i]["BusinessAddress"].ToString();
                    string BusinessAddress2 = dt.Rows[i]["BusinessAddress2"].ToString();
                    string BusinessAddress3 = dt.Rows[i]["BusinessAddress3"].ToString();
                    string BusinessAddress4 = dt.Rows[i]["BusinessAddress4"].ToString();
                    string TaxClearanceHeldYN = dt.Rows[i]["TaxClearanceHeldYN"].ToString();
                    string TaxClearanceExpiryDate = dt.Rows[i]["TaxClearanceExpiryDate"].ToString();
                    string Student = dt.Rows[i]["Student"].ToString();
                    string AMKStaffType = dt.Rows[i]["AMKStaffType"].ToString();
                    string SpouseCustomerNumber = dt.Rows[i]["SpouseCustomerNumber"].ToString();
                    string NoofEmployees = dt.Rows[i]["NoofEmployees"].ToString();
                    string ProvinceCity = dt.Rows[i]["ProvinceCity"].ToString();
                    string District = dt.Rows[i]["District"].ToString();
                    string Commune = dt.Rows[i]["Commune"].ToString();
                    string Village = dt.Rows[i]["Village"].ToString();
                    string SpouseNameinKhmer = dt.Rows[i]["SpouseNameinKhmer"].ToString();
                    string SurnameKhmer = dt.Rows[i]["SurnameKhmer"].ToString();
                    string FirstNameKhmer = dt.Rows[i]["FirstNameKhmer"].ToString();
                    string SpouseDateofBirth = dt.Rows[i]["SpouseDateofBirth"].ToString();
                    string SpouseIDType = dt.Rows[i]["SpouseIDType"].ToString();
                    string SpouseIDNumber = dt.Rows[i]["SpouseIDNumber"].ToString();
                    string SpouseIDIssueDate = dt.Rows[i]["SpouseIDIssueDate"].ToString();
                    string SpouseIDExpiryDate = dt.Rows[i]["SpouseIDExpiryDate"].ToString();
                    string NoActiveMember = dt.Rows[i]["NoActiveMember"].ToString();
                    string ProvertyStatus = dt.Rows[i]["ProvertyStatus"].ToString();
                    string VillageBank = dt.Rows[i]["VillageBank"].ToString();
                    if (VillageBank == "0")
                    {
                        VillageBank = "";
                    }
                    else
                    {
                        VillageBank = "<cus:VillageBank>" + VillageBank + "</cus:VillageBank>";
                    }
                    string VillageBankPresident = dt.Rows[i]["VillageBankPresident"].ToString();
                    string MobileNumberType = dt.Rows[i]["MobileNumberType"].ToString();
                    string PlaceofBirth = dt.Rows[i]["PlaceofBirth"].ToString();
                    string BlacklistStatus = dt.Rows[i]["BlacklistStatus"].ToString();
                    string RiskStatus = dt.Rows[i]["RiskStatus"].ToString();
                    string BlackListCheckDate = dt.Rows[i]["BlackListCheckDate"].ToString();
                    string BlacklistCurrentStatus = dt.Rows[i]["BlacklistCurrentStatus"].ToString();
                    string HighRiskCateogries = dt.Rows[i]["HighRiskCateogries"].ToString();
                    string FATCAStatus = dt.Rows[i]["FATCAStatus"].ToString();
                    string FATCACountryofBirth = dt.Rows[i]["FATCACountryofBirth"].ToString();
                    string FATCANationality = dt.Rows[i]["FATCANationality"].ToString();
                    string FATCATaxIdentNumber = dt.Rows[i]["FATCATaxIdentNumber"].ToString();
                    string FATCAAddress = dt.Rows[i]["FATCAAddress"].ToString();
                    string FATCAPostalCode = dt.Rows[i]["FATCAPostalCode"].ToString();
                    string USTaxIdentificationNumber = dt.Rows[i]["USTaxIdentificationNumber"].ToString();
                    string FATCACountryCode = dt.Rows[i]["FATCACountryCode"].ToString();
                    string RecalcitrantStatus = dt.Rows[i]["RecalcitrantStatus"].ToString();
                    string OccupationType = dt.Rows[i]["OccupationType"].ToString();
                    string OccupationDetails = dt.Rows[i]["OccupationDetails"].ToString();
                    string IDType = dt.Rows[i]["IDType"].ToString();
                    string IDNumber = dt.Rows[i]["IDNumber"].ToString();
                    string IssueDate = dt.Rows[i]["IssueDate"].ToString();
                    string ExpiryDate = dt.Rows[i]["ExpiryDate"].ToString();
                    string Position = dt.Rows[i]["Position"].ToString();
                    string Department = dt.Rows[i]["Department"].ToString();
                    #endregion
                    #region Issue Date | ExpiryDateTag | SpouseIDIssueDate
                    string IssueDateTag = "";
                    if (IssueDate != "1900-01-01")
                    {
                        IssueDateTag = "<cus:IssueDate>" + IssueDate + "</cus:IssueDate>";
                    }
                    string ExpiryDateTag = "";
                    if (ExpiryDate != "1900-01-01")
                    {
                        ExpiryDateTag = "<cus:ExpiryDate>" + ExpiryDate + "</cus:ExpiryDate>";
                    }
                    string SpouseIDIssueDateTag = "";
                    if (SpouseIDIssueDate != "1900-01-01")
                    {
                        SpouseIDIssueDateTag = "<cus:SpouseIDIssueDate>" + SpouseIDIssueDate + "</cus:SpouseIDIssueDate>";
                    }
                    string SpouseIDExpiryDateTag = "";
                    if (SpouseIDExpiryDate != "1900-01-01")
                    {
                        SpouseIDExpiryDateTag = "<cus:SpouseIDExpiryDate>" + SpouseIDExpiryDate + "</cus:SpouseIDExpiryDate>";
                    }
                    #endregion Issue Date
                    #region CustomerID - Add/Edit
                    try
                    {
                        string isError = "", ErrXml = "", Remark = "";
                        string xmlStr = "";
                        int OneSingleTwoWithSpouce = 1;
                        DateTime dt_msgIDDate = DateTime.Now;
                        string msgIDDate = dt_msgIDDate.ToString("yyyy-MM-dd");
                        #region xml
                        if (MARITALSTATUS == "MARRIED")
                        {
                            if (LoanAppPersonTypeID == "32" || LoanAppPersonTypeID == "34")
                            {
                                //xml single
                                OneSingleTwoWithSpouce = 1;
                            }
                            else
                            {
                                //xml with spouse
                                OneSingleTwoWithSpouce = 2;
                            }
                        }
                        else
                        {
                            //xml single
                            OneSingleTwoWithSpouce = 1;
                        }
                        if (OneSingleTwoWithSpouce == 1)
                        {
                            #region xml single
                            xmlStr = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:amk=\"http://temenos.com/AMKINDCUST\" xmlns:cus=\"http://temenos.com/CUSTOMERAMKINDIVWS\"><soapenv:Header/>"
                            + "<soapenv:Body><amk:INDIVCUSTOMERCREATION><WebRequestCommon><company>" + CreCompany + "</company><password>" + CrePassword + "</password>"
                            + "<userName>" + CreUserName + "</userName></WebRequestCommon>"
                            + "<OfsFunction>"
                            + "<messageId>" + "C-" + LoanAppPersonID + "-" + SHORTNAME + "-" + msgIDDate + "</messageId>"
                            + "</OfsFunction>"
                            + "<CUSTOMERAMKINDIVWSType id=\"" + CustomerID + "\">"
                            + "<cus:gSHORTNAME g=\"1\"><cus:SHORTNAME>" + SHORTNAME + "</cus:SHORTNAME></cus:gSHORTNAME><cus:gNAME1 g=\"1\"><cus:NAME1>" + NAME1
                            + "</cus:NAME1></cus:gNAME1><cus:gSTREET g=\"1\"><cus:STREET>" + STREET + "</cus:STREET></cus:gSTREET><cus:gRELATIONCODE g=\"1\">"
                            + "<cus:mRELATIONCODE m=\"1\"><cus:RELATIONCODE>" + RELATIONCODE + "</cus:RELATIONCODE><cus:RELCUSTOMER>" + RELCUSTOMER
                            + "</cus:RELCUSTOMER></cus:mRELATIONCODE></cus:gRELATIONCODE><cus:SECTOR>" + SECTOR + "</cus:SECTOR><cus:ACCOUNTOFFICER>" + ACCOUNTOFFICER
                            + "</cus:ACCOUNTOFFICER><cus:INDUSTRY>" + INDUSTRY + "</cus:INDUSTRY><cus:TARGET>" + TARGET + "</cus:TARGET><cus:NATIONALITY>" + NATIONALITY
                            + "</cus:NATIONALITY><cus:CUSTOMERSTATUS>" + CUSTOMERSTATUS + "</cus:CUSTOMERSTATUS><cus:RESIDENCE>" + RESIDENCE
                            + "</cus:RESIDENCE><cus:BIRTHINCORPDATE>" + BIRTHINCORPDATE + "</cus:BIRTHINCORPDATE><cus:TITLE>" + TITLE + "</cus:TITLE><cus:GENDER>" + GENDER + "</cus:GENDER><cus:MARITALSTATUS>" + MARITALSTATUS
                            + "</cus:MARITALSTATUS><cus:OCCUPATION>" + OCCUPATION + "</cus:OCCUPATION><cus:gFURTHERDETAILS g=\"1\">"
                            + "<cus:FURTHERDETAILS>" + FURTHERDETAILS + "</cus:FURTHERDETAILS></cus:gFURTHERDETAILS>" + PostalCode + "<cus:gEMPLOY.BUS.ADDR g=\"1\"><cus:EmployersAddress>" + EmployersAddress
                            + "</cus:EmployersAddress></cus:gEMPLOY.BUS.ADDR><cus:HomePhoneNo>" + HomePhoneNo + "</cus:HomePhoneNo><cus:WorkPhoneNo>" + WorkPhoneNo
                            + "</cus:WorkPhoneNo><cus:MobilePhoneNo>" + MobilePhoneNo + "</cus:MobilePhoneNo><cus:ResidenceYN>" + ResidenceYN
                            + "</cus:ResidenceYN><cus:EmailAddress>" + EmailAddress + "</cus:EmailAddress><cus:EmployerName>" + EmployerName
                            + "</cus:EmployerName><cus:TaxpayerRegistrationNo>" + TaxpayerRegistrationNo + "</cus:TaxpayerRegistrationNo><cus:BlockedYN>" + BlockedYN
                            + "</cus:BlockedYN><cus:AMKStaff>" + AMKStaff + "</cus:AMKStaff><cus:NoFamilyMember>" + NoFamilyMember
                            + "</cus:NoFamilyMember><cus:EmployeeCode>" + EmployeeCode + "</cus:EmployeeCode>"
                            //+"<cus:DateofEmployment>" + DateofEmployment + "</cus:DateofEmployment>"
                            //+"<cus:FaxNo>" + FaxNo + "</cus:FaxNo>"
                            + "<cus:Profession>" + Profession + "</cus:Profession>"
                            //+"<cus:EmploymentStatus>" + EmploymentStatus + "</cus:EmploymentStatus>"
                            + "<cus:MainSourceofIncome>" + MainSourceofIncome + "</cus:MainSourceofIncome>"
                            //+"<cus:EmployersCode>" + EmployersCode + "</cus:EmployersCode>"
                            //+"<cus:SpouseName></cus:SpouseName>"
                            //+ "<cus:SpouseOccupation></cus:SpouseOccupation>"
                            + "<cus:ConsenttoDisclosure>" + ConsenttoDisclosure + "</cus:ConsenttoDisclosure>"
                            + "<cus:DateofSignature>" + DateofSignature + "</cus:DateofSignature>"

                            //+ "<cus:gPLACE.STUDY g=\"1\"><cus:mPLACE.STUDY m=\"1\">"
                            //+ "<cus:PlaceofStudy>" + PlaceofStudy + "</cus:PlaceofStudy>"
                            //+ "<cus:DurationofCourse>" + DurationofCourse + "</cus:DurationofCourse>"
                            //+ "<cus:FieldofStudy>" + FieldofStudy + "</cus:FieldofStudy>"
                            //+"</cus:mPLACE.STUDY></cus:gPLACE.STUDY>"

                            //+ "<cus:NominationForm>" + NominationForm + "</cus:NominationForm>"

                            //+"<cus:gINS.MEM.NO g=\"1\"><cus:mINS.MEM.NO m=\"1\">"
                            //+ "<cus:NominationCustomer>" + NominationCustomer + "</cus:NominationCustomer>"
                            //+"<cus:NominationBeneficiary>" + NominationBeneficiary + "</cus:NominationBeneficiary>"
                            //+"<cus:NominationAddress1>" + NominationAddress1 + "</cus:NominationAddress1>"
                            //+"<cus:NominationAddress2>" + NominationAddress2 + "</cus:NominationAddress2>"
                            //+"<cus:NominationAddress3>" + NominationAddress3 + "</cus:NominationAddress3>"
                            //+"<cus:NominationAddress4>" + NominationAddress4 + "</cus:NominationAddress4>"
                            //+INSPostalCode
                            //+"<cus:INSPhoneNo>" + INSPhoneNo + "</cus:INSPhoneNo>"
                            //+"<cus:RelationshiptoCustomer>" + RelationshiptoCustomer + "</cus:RelationshiptoCustomer>"
                            //+ "<cus:AmountorofLegacy>" + AmountorofLegacy + "</cus:AmountorofLegacy>"
                            //+"</cus:mINS.MEM.NO></cus:gINS.MEM.NO>"

                            //+"<cus:gBUS.NAME g=\"1\"><cus:mBUS.NAME m=\"1\">"
                            //+"<cus:NameofBusiness>" + NameofBusiness + "</cus:NameofBusiness>"
                            //+"<cus:NatureofBusiness>" + NatureofBusiness + "</cus:NatureofBusiness>"
                            //+"<cus:BusinessType>" + BusinessType + "</cus:BusinessType>"
                            //+"<cus:BusinessPlan>" + BusinessPlan + "</cus:BusinessPlan>"
                            //+"<cus:RoleinBusiness>" + RoleinBusiness + "</cus:RoleinBusiness>"
                            //+"<cus:BusinessAddress>" + BusinessAddress + "</cus:BusinessAddress>"
                            //+"<cus:BusinessAddress2>" + BusinessAddress2 + "</cus:BusinessAddress2>"
                            //+"<cus:BusinessAddress3>" + BusinessAddress3 + "</cus:BusinessAddress3>"
                            //+"<cus:BusinessAddress4>" + BusinessAddress4 + "</cus:BusinessAddress4>"
                            //+"<cus:TaxClearanceHeldYN>" + TaxClearanceHeldYN + "</cus:TaxClearanceHeldYN>"
                            //+"<cus:TaxClearanceExpiryDate>" + TaxClearanceExpiryDate + "</cus:TaxClearanceExpiryDate>"
                            //+"</cus:mBUS.NAME></cus:gBUS.NAME>"

                            //+"<cus:Student>" + Student + "</cus:Student>"
                            //+"<cus:AMKStaffType>" + AMKStaffType + "</cus:AMKStaffType>"
                            //+"<cus:SpouseCustomerNumber></cus:SpouseCustomerNumber>"
                            //+"<cus:NoofEmployees>" + NoofEmployees + "</cus:NoofEmployees>"
                            + "<cus:ProvinceCity>" + ProvinceCity + "</cus:ProvinceCity><cus:District>" + District
                            + "</cus:District><cus:Commune>" + Commune + "</cus:Commune><cus:Village>" + Village + "</cus:Village><cus:SurnameKhmer>" + SurnameKhmer
                            + "</cus:SurnameKhmer><cus:FirstNameKhmer>" + FirstNameKhmer + "</cus:FirstNameKhmer><cus:NoActiveMember>" + NoActiveMember
                            + "</cus:NoActiveMember><cus:ProvertyStatus>" + ProvertyStatus + "</cus:ProvertyStatus>"
                            + VillageBank
                            + "<cus:VillageBankPresident>" + VillageBankPresident + "</cus:VillageBankPresident><cus:MobileNumberType>" + MobileNumberType
                            + "</cus:MobileNumberType><cus:PlaceofBirth>" + PlaceofBirth + "</cus:PlaceofBirth>"

                            //+"<cus:BlacklistStatus>" + BlacklistStatus + "</cus:BlacklistStatus>"
                            //+"<cus:RiskStatus>" + RiskStatus + "</cus:RiskStatus>"
                            //+"<cus:BlackListCheckDate>" + BlackListCheckDate + "</cus:BlackListCheckDate>"
                            //+"<cus:BlacklistCurrentStatus>" + BlacklistCurrentStatus + "</cus:BlacklistCurrentStatus>"
                            //+ "<cus:HighRiskCateogries>" + HighRiskCateogries + "</cus:HighRiskCateogries>"
                            + "<cus:FATCAStatus>" + FATCAStatus + "</cus:FATCAStatus>"
                            + "<cus:FATCACountryofBirth>" + FATCACountryofBirth + "</cus:FATCACountryofBirth>"

                            + "<cus:gAMK.F.I.NAT g=\"1\"><cus:mAMK.F.I.NAT m=\"1\">"
                            + "<cus:FATCANationality>" + FATCANationality + "</cus:FATCANationality>"
                            + "<cus:FATCATaxIdentNumber>" + FATCATaxIdentNumber + "</cus:FATCATaxIdentNumber>"
                            + "</cus:mAMK.F.I.NAT></cus:gAMK.F.I.NAT>"

                            //+"<cus:gAMK.F.I.ADDRESS g=\"1\">"
                            //+"<cus:FATCAAddress>" + FATCAAddress + "</cus:FATCAAddress>"
                            //+"</cus:gAMK.F.I.ADDRESS>"

                            //+"<cus:FATCAPostalCode>" + FATCAPostalCode + "</cus:FATCAPostalCode>"
                            //+"<cus:USTaxIdentificationNumber>" + USTaxIdentificationNumber + "</cus:USTaxIdentificationNumber>"
                            //+ "<cus:FATCACountryCode>" + FATCACountryCode + "</cus:FATCACountryCode>"
                            //+"<cus:RecalcitrantStatus>" + RecalcitrantStatus + "</cus:RecalcitrantStatus>"
                            + "<cus:OccupationType>" + OccupationType + "</cus:OccupationType>"
                            + "<cus:OccupationDetails>" + OccupationDetails + "</cus:OccupationDetails>"

                            + "<cus:gAMK.ID.TYPE g=\"1\"><cus:mAMK.ID.TYPE m=\"1\">"
                            + "<cus:IDType>" + IDType + "</cus:IDType>"
                            + "<cus:IDNumber>" + IDNumber + "</cus:IDNumber>"
                            + IssueDateTag + ExpiryDateTag
                            + "</cus:mAMK.ID.TYPE></cus:gAMK.ID.TYPE>"

                            //+"<cus:Position>" + Position + "</cus:Position>"
                            //+"<cus:Department>" + Department + "</cus:Department>"
                            //+"<cus:gAMK.KYC.IMAGE g=\"1\"><cus:KYCImage></cus:KYCImage></cus:gAMK.KYC.IMAGE>"

                            + "</CUSTOMERAMKINDIVWSType></amk:INDIVCUSTOMERCREATION></soapenv:Body></soapenv:Envelope>";
                            #endregion xml single
                        }
                        else
                        {
                            #region xml with spouse
                            xmlStr = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" "
                            + "xmlns:amk=\"http://temenos.com/AMKINDCUST\" xmlns:cus=\"http://temenos.com/CUSTOMERAMKINDIVWS\"><soapenv:Header/>"
                            + "<soapenv:Body><amk:INDIVCUSTOMERCREATION><WebRequestCommon><company>" + CreCompany + "</company><password>" + CrePassword + "</password>"
                            + "<userName>" + CreUserName + "</userName></WebRequestCommon>"
                            + "<OfsFunction>"
                            + "<messageId>" + "C-" + LoanAppPersonID + "-" + SHORTNAME + "-" + msgIDDate + "</messageId>"
                            + "</OfsFunction>"
                            + "<CUSTOMERAMKINDIVWSType id=\"" + CustomerID + "\">"
                            + "<cus:gSHORTNAME g=\"1\"><cus:SHORTNAME>" + SHORTNAME + "</cus:SHORTNAME></cus:gSHORTNAME>"
                            + "<cus:gNAME1 g=\"1\"><cus:NAME1>" + NAME1 + "</cus:NAME1></cus:gNAME1>"
                            + "<cus:gSTREET g=\"1\"><cus:STREET>" + STREET + "</cus:STREET></cus:gSTREET>"
                            + "<cus:gRELATIONCODE g=\"1\">"
                            + "<cus:mRELATIONCODE m=\"1\"><cus:RELATIONCODE>" + RELATIONCODE + "</cus:RELATIONCODE>"
                            + "<cus:RELCUSTOMER>" + RELCUSTOMER + "</cus:RELCUSTOMER>"
                            + "</cus:mRELATIONCODE></cus:gRELATIONCODE>"
                            + "<cus:SECTOR>" + SECTOR + "</cus:SECTOR>"
                            + "<cus:ACCOUNTOFFICER>" + ACCOUNTOFFICER + "</cus:ACCOUNTOFFICER>"
                            + "<cus:INDUSTRY>" + INDUSTRY + "</cus:INDUSTRY>"
                            + "<cus:TARGET>" + TARGET + "</cus:TARGET>"
                            + "<cus:NATIONALITY>" + NATIONALITY + "</cus:NATIONALITY>"
                            + "<cus:CUSTOMERSTATUS>" + CUSTOMERSTATUS + "</cus:CUSTOMERSTATUS>"
                            + "<cus:RESIDENCE>" + RESIDENCE + "</cus:RESIDENCE>"
                            + "<cus:BIRTHINCORPDATE>" + BIRTHINCORPDATE + "</cus:BIRTHINCORPDATE>"
                            + "<cus:TITLE>" + TITLE + "</cus:TITLE><cus:GENDER>" + GENDER + "</cus:GENDER>"
                            + "<cus:MARITALSTATUS>" + MARITALSTATUS + "</cus:MARITALSTATUS>"
                            //+"<cus:OCCUPATION>" + OCCUPATION + "</cus:OCCUPATION>"
                            //+"<cus:gFURTHERDETAILS g=\"1\">"
                            //+"<cus:FURTHERDETAILS>" + FURTHERDETAILS + "</cus:FURTHERDETAILS>"
                            //+"</cus:gFURTHERDETAILS>" 
                            + PostalCode
                            //+ "<cus:gEMPLOY.BUS.ADDR g=\"1\">"
                            //+ "<cus:EmployersAddress>" + EmployersAddress + "</cus:EmployersAddress>"
                            //+"</cus:gEMPLOY.BUS.ADDR>"
                            //+"<cus:HomePhoneNo>" + HomePhoneNo + "</cus:HomePhoneNo>"
                            //+"<cus:WorkPhoneNo>" + WorkPhoneNo + "</cus:WorkPhoneNo>"
                            + "<cus:MobilePhoneNo>" + MobilePhoneNo + "</cus:MobilePhoneNo>"
                            + "<cus:ResidenceYN>" + ResidenceYN + "</cus:ResidenceYN>"
                            //+"<cus:EmailAddress>" + EmailAddress + "</cus:EmailAddress>"
                            //+"<cus:EmployerName>" + EmployerName + "</cus:EmployerName>"
                            //+"<cus:TaxpayerRegistrationNo>" + TaxpayerRegistrationNo + "</cus:TaxpayerRegistrationNo>"
                            + "<cus:BlockedYN>" + BlockedYN + "</cus:BlockedYN>"
                            + "<cus:AMKStaff>" + AMKStaff + "</cus:AMKStaff>"
                            + "<cus:NoFamilyMember>" + NoFamilyMember + "</cus:NoFamilyMember>"
                            //+"<cus:EmployeeCode>" + EmployeeCode + "</cus:EmployeeCode>"
                            //+"<cus:DateofEmployment>" + DateofEmployment + "</cus:DateofEmployment>"
                            //+"<cus:FaxNo>" + FaxNo + "</cus:FaxNo>"
                            + "<cus:Profession>" + Profession + "</cus:Profession>"
                            //+"<cus:EmploymentStatus>" + EmploymentStatus + "</cus:EmploymentStatus>"
                            + "<cus:MainSourceofIncome>" + MainSourceofIncome + "</cus:MainSourceofIncome>"
                            //+"<cus:EmployersCode>" + EmployersCode + "</cus:EmployersCode>"
                            + "<cus:SpouseName>" + SpouseName + "</cus:SpouseName>"
                            //+"<cus:SpouseOccupation>" + SpouseOccupation + "</cus:SpouseOccupation>"
                            + "<cus:ConsenttoDisclosure>" + ConsenttoDisclosure + "</cus:ConsenttoDisclosure>"
                            + "<cus:DateofSignature>" + DateofSignature + "</cus:DateofSignature>"

                            //+"<cus:gPLACE.STUDY g=\"1\"><cus:mPLACE.STUDY m=\"1\">"
                            //+"<cus:PlaceofStudy>" + PlaceofStudy + "</cus:PlaceofStudy>"
                            //+"<cus:DurationofCourse>" + DurationofCourse + "</cus:DurationofCourse>"
                            //+"<cus:FieldofStudy>" + FieldofStudy + "</cus:FieldofStudy>"
                            //+"</cus:mPLACE.STUDY></cus:gPLACE.STUDY>"

                            //+"<cus:NominationForm>" + NominationForm + "</cus:NominationForm>"

                            //+"<cus:gINS.MEM.NO g=\"1\"><cus:mINS.MEM.NO m=\"1\">"
                            //+"<cus:NominationCustomer>" + NominationCustomer + "</cus:NominationCustomer>"
                            //+"<cus:NominationBeneficiary>" + NominationBeneficiary + "</cus:NominationBeneficiary>"
                            //+"<cus:NominationAddress1>" + NominationAddress1 + "</cus:NominationAddress1>"
                            //+"<cus:NominationAddress2>" + NominationAddress2 + "</cus:NominationAddress2>"
                            //+"<cus:NominationAddress3>" + NominationAddress3 + "</cus:NominationAddress3>"
                            //+"<cus:NominationAddress4>" + NominationAddress4 + "</cus:NominationAddress4>"
                            //+ INSPostalCode 
                            //+ "<cus:INSPhoneNo>" + INSPhoneNo + "</cus:INSPhoneNo>"
                            //+"<cus:RelationshiptoCustomer>" + RelationshiptoCustomer + "</cus:RelationshiptoCustomer>"
                            //+"<cus:AmountorofLegacy>" + AmountorofLegacy + "</cus:AmountorofLegacy>"
                            //+"</cus:mINS.MEM.NO></cus:gINS.MEM.NO>"

                            //+"<cus:gBUS.NAME g=\"1\"><cus:mBUS.NAME m=\"1\">"
                            //+ "<cus:NameofBusiness>" + NameofBusiness + "</cus:NameofBusiness>"
                            //+"<cus:NatureofBusiness>" + NatureofBusiness + "</cus:NatureofBusiness>"
                            //+"<cus:BusinessType>" + BusinessType + "</cus:BusinessType>"
                            //+"<cus:BusinessPlan>" + BusinessPlan + "</cus:BusinessPlan>"
                            //+"<cus:RoleinBusiness>" + RoleinBusiness + "</cus:RoleinBusiness>"
                            //+"<cus:BusinessAddress>" + BusinessAddress + "</cus:BusinessAddress>"
                            //+"<cus:BusinessAddress2>" + BusinessAddress2 + "</cus:BusinessAddress2>"
                            //+"<cus:BusinessAddress3>" + BusinessAddress3 + "</cus:BusinessAddress3>"
                            //+"<cus:BusinessAddress4>" + BusinessAddress4 + "</cus:BusinessAddress4>"
                            //+"<cus:TaxClearanceHeldYN>" + TaxClearanceHeldYN + "</cus:TaxClearanceHeldYN>"
                            //+"<cus:TaxClearanceExpiryDate>" + TaxClearanceExpiryDate + "</cus:TaxClearanceExpiryDate>"
                            //+"</cus:mBUS.NAME></cus:gBUS.NAME>"

                            //+"<cus:Student>" + Student + "</cus:Student>"
                            //+"<cus:AMKStaffType>" + AMKStaffType + "</cus:AMKStaffType>"
                            + "<cus:SpouseCustomerNumber>" + SpouseCustomerNumber + "</cus:SpouseCustomerNumber>"
                            //+"<cus:NoofEmployees>" + NoofEmployees + "</cus:NoofEmployees>"
                            + "<cus:ProvinceCity>" + ProvinceCity + "</cus:ProvinceCity>"
                            + "<cus:District>" + District + "</cus:District>"
                            + "<cus:Commune>" + Commune + "</cus:Commune>"
                            + "<cus:Village>" + Village + "</cus:Village>"
                            + "<cus:SpouseNameinKhmer>" + SpouseNameinKhmer + "</cus:SpouseNameinKhmer>"
                            + "<cus:SurnameKhmer>" + SurnameKhmer + "</cus:SurnameKhmer>"
                            + "<cus:FirstNameKhmer>" + FirstNameKhmer + "</cus:FirstNameKhmer>"
                            + "<cus:SpouseDateofBirth>" + SpouseDateofBirth + "</cus:SpouseDateofBirth>"

                            + "<cus:gAMK.SP.ID.TYPE g=\"1\"><cus:mAMK.SP.ID.TYPE m=\"1\">"
                            + "<cus:SpouseIDType>" + SpouseIDType + "</cus:SpouseIDType>"
                            + "<cus:SpouseIDNumber>" + SpouseIDNumber + "</cus:SpouseIDNumber>"
                            + SpouseIDIssueDateTag
                            + SpouseIDExpiryDateTag
                            + "</cus:mAMK.SP.ID.TYPE></cus:gAMK.SP.ID.TYPE>"

                            + "<cus:NoActiveMember>" + NoActiveMember + "</cus:NoActiveMember>"
                            + "<cus:ProvertyStatus>" + ProvertyStatus + "</cus:ProvertyStatus>"
                            + VillageBank
                            + "<cus:VillageBankPresident>" + VillageBankPresident + "</cus:VillageBankPresident>"
                            + "<cus:MobileNumberType>" + MobileNumberType + "</cus:MobileNumberType>"
                            + "<cus:PlaceofBirth>" + PlaceofBirth + "</cus:PlaceofBirth>"
                            //+"<cus:BlacklistStatus>" + BlacklistStatus + "</cus:BlacklistStatus>"
                            //+"<cus:RiskStatus>" + RiskStatus + "</cus:RiskStatus>"
                            //+"<cus:BlackListCheckDate>" + BlackListCheckDate + "</cus:BlackListCheckDate>"
                            //+"<cus:BlacklistCurrentStatus>" + BlacklistCurrentStatus + "</cus:BlacklistCurrentStatus>"
                            //+ "<cus:HighRiskCateogries>" + HighRiskCateogries + "</cus:HighRiskCateogries>"
                            + "<cus:FATCAStatus>" + FATCAStatus + "</cus:FATCAStatus>"
                            + "<cus:FATCACountryofBirth>" + FATCACountryofBirth + "</cus:FATCACountryofBirth>"

                            + "<cus:gAMK.F.I.NAT g=\"1\"><cus:mAMK.F.I.NAT m=\"1\">"
                            + "<cus:FATCANationality>" + FATCANationality + "</cus:FATCANationality>"
                            + "<cus:FATCATaxIdentNumber>" + FATCATaxIdentNumber + "</cus:FATCATaxIdentNumber>"
                            + "</cus:mAMK.F.I.NAT></cus:gAMK.F.I.NAT>"

                            //+"<cus:gAMK.F.I.ADDRESS g=\"1\">"
                            //+"<cus:FATCAAddress>" + FATCAAddress + "</cus:FATCAAddress>"
                            //+"</cus:gAMK.F.I.ADDRESS>"

                            //+ FATCAPostalCode 
                            //+ "<cus:USTaxIdentificationNumber>" + USTaxIdentificationNumber + "</cus:USTaxIdentificationNumber>"
                            //+"<cus:FATCACountryCode>" + FATCACountryCode + "</cus:FATCACountryCode>"
                            //+"<cus:RecalcitrantStatus>" + RecalcitrantStatus + "</cus:RecalcitrantStatus>"

                            + "<cus:OccupationType>" + OccupationType + "</cus:OccupationType>"
                            + "<cus:OccupationDetails>" + OccupationDetails + "</cus:OccupationDetails>"

                            + "<cus:gAMK.ID.TYPE g=\"1\"><cus:mAMK.ID.TYPE m=\"1\">"
                            + "<cus:IDType>" + IDType + "</cus:IDType>"
                            + "<cus:IDNumber>" + IDNumber + "</cus:IDNumber>"
                            + IssueDateTag + ExpiryDateTag
                            + "</cus:mAMK.ID.TYPE></cus:gAMK.ID.TYPE>"

                            //+"<cus:Position>" + Position + "</cus:Position>"
                            //+"<cus:Department>" + Department + "</cus:Department>"
                            //+"<cus:gAMK.KYC.IMAGE g=\"1\"><cus:KYCImage></cus:KYCImage></cus:gAMK.KYC.IMAGE>"

                            + "</CUSTOMERAMKINDIVWSType>"
                            + "</amk:INDIVCUSTOMERCREATION></soapenv:Body></soapenv:Envelope>";
                            #endregion xml with spouse
                        }

                        #endregion xml
                        #region Call To T24
                        T24_AddLog(fileHeader, "AddEditCustomer_RQ", xmlStr, "LoanAdd");
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(CreUrl);
                        request.ContentType = "text/xml"; // or application/soap+xml for SOAP 1.2
                        request.Method = "POST";
                        request.KeepAlive = false;

                        byte[] byteArray = Encoding.UTF8.GetBytes(xmlStr);
                        request.ContentLength = byteArray.Length;

                        Stream requestStream = request.GetRequestStream();
                        requestStream.Write(byteArray, 0, byteArray.Length);
                        requestStream.Close();

                        HttpWebResponse response = null;
                        try
                        {
                            response = (HttpWebResponse)request.GetResponse();
                        }
                        catch (WebException ex)
                        {
                            response = (HttpWebResponse)ex.Response;
                        }

                        Stream receiveStream = response.GetResponseStream();
                        StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(readStream.ReadToEnd());
                        string successIndicator = doc.GetElementsByTagName("successIndicator").Item(0).InnerText;
                        string transactionId = doc.GetElementsByTagName("transactionId").Item(0).InnerText;
                        T24_AddLog(fileHeader, "AddEditCustomer_RS: " + (int)response.StatusCode, doc.InnerXml, "LoanAdd");

                        #endregion
                        #region Condiction
                        if (successIndicator == "Success")
                        {
                            ReturnDT("update tblLoanAppPerson2 set PersonID='" + transactionId + "',CustomerID='" + transactionId
                            + "',Number='" + transactionId + "' where LoanAppPersonID='" + LoanAppPersonID + "'");
                            ERR = transactionId;
                            isError = "0";
                            Remark = ERR;
                        }
                        else
                        {

                            string messages = doc.GetElementsByTagName("messages").Item(0).InnerText;
                            if (messages == "LIVE RECORD NOT CHANGED"
                                || messages == "COMPANY.BOOK:1:1=NOCHANGE FIELD" || messages == "EB-LIVE.RECORD.NOT.CHANGED")
                            {
                                ERR = CustomerID;
                                isError = "0";
                                Remark = ERR;
                            }
                            else { 

                                #region Error
                                xmlContent = doc.InnerXml;
                                if (xmlContent.Contains("<Status>"))
                                {
                                    int pFrom = xmlContent.IndexOf("<Status>");// + "<Status>".Length;
                                    int pTo = xmlContent.IndexOf("</Status>") + "</Status>".Length;
                                    string MSGxmlContent = xmlContent.Substring(pFrom, pTo - pFrom).Trim();
                                    LoanAppApproveToCBSError.Status obj = GenerateXmlObject<LoanAppApproveToCBSError.Status>(MSGxmlContent);
                                    string strMsg = "";
                                    for (int iMsg = 0; iMsg < obj.Messages.Count; iMsg++)
                                    {
                                        if (iMsg == 0)
                                        {
                                            strMsg = obj.Messages[iMsg];
                                        }
                                        else
                                        {
                                            strMsg = strMsg + " | " + obj.Messages[iMsg];
                                        }
                                    }
                                    SMS = strMsg;
                                    Remark = SMS;
                                }
                                #endregion Error

                                ERR = "Error";
                            }
                        }

                        #endregion

                        #region log
                        T24_LoanAppAddLogAdd(LoanAppID, LoanAppPersonID, fileHeader, isError, doc.InnerXml, Remark, "0");
                        #endregion log
                        #region Close Stream
                        readStream.Close();
                        requestStream.Close();
                        receiveStream.Close();
                        response.Close();
                        #endregion

                    }
                    catch (Exception ex)
                    {
                        //add error log
                        string line = GetLineNumber(ex).ToString();
                        SMS = ex.Message.ToString() + line;
                    }
                    #endregion CustomerID - Add/Edit
                }
            }
            catch (Exception ex)
            {
                ERR = "Error";
                SMS = ex.Message.ToString();
            }
            rs[0] = ERR;
            rs[1] = SMS;
            return rs;
        }
        //kyc version
        public string[] AddEditCustomerV3(string fileHeader, string LoanAppPersonID, string LoanAppPersonTypeID)
        {
            string[] rs = new string[2];
            string ERR = "Error", SMS = "", rsContent = "", xmlContent = "";
            try
            {
                fileHeader = fileHeader.Replace(" ", "_").Replace("-", "_").Replace(":", "_").Replace(".", "_");
                string sql = "exec T24_LoanAppListForOpenToCBS2_P2_Cust @LoanAppPersonID='" + LoanAppPersonID + "'";
                DataTable dt = ReturnDT(sql);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    #region params
                    string CreUrl = dt.Rows[i]["CreUrl"].ToString();
                    string CreCompany = dt.Rows[i]["CreCompany"].ToString();
                    string CreUserName = dt.Rows[i]["CreUserName"].ToString();
                    string CrePassword = dt.Rows[i]["CrePassword"].ToString();

                    //string LoanAppPersonID = dt.Rows[i]["LoanAppPersonID"].ToString();
                    string LoanAppID = dt.Rows[i]["LoanAppID"].ToString();
                    string CustomerID = dt.Rows[i]["CustomerID"].ToString();
                    string SHORTNAME = dt.Rows[i]["SHORTNAME"].ToString();
                    string NAME1 = dt.Rows[i]["NAME1"].ToString();
                    string STREET = dt.Rows[i]["STREET"].ToString();
                    string RELATIONCODE = dt.Rows[i]["RELATIONCODE"].ToString();
                    string RELCUSTOMER = dt.Rows[i]["RELCUSTOMER"].ToString();
                    string SECTOR = dt.Rows[i]["SECTOR"].ToString();
                    string ACCOUNTOFFICER = dt.Rows[i]["ACCOUNTOFFICER"].ToString();
                    string INDUSTRY = dt.Rows[i]["INDUSTRY"].ToString();
                    string TARGET = dt.Rows[i]["TARGET"].ToString();
                    string NATIONALITY = dt.Rows[i]["NATIONALITY"].ToString();
                    string CUSTOMERSTATUS = dt.Rows[i]["CUSTOMERSTATUS"].ToString();
                    string RESIDENCE = dt.Rows[i]["RESIDENCE"].ToString();
                    string BIRTHINCORPDATE = dt.Rows[i]["BIRTHINCORPDATE"].ToString();
                    string COMPANYBOOK = dt.Rows[i]["COMPANYBOOK"].ToString();
                    string TITLE = dt.Rows[i]["TITLE"].ToString();
                    string GENDER = dt.Rows[i]["GENDER"].ToString();
                    string MARITALSTATUS = dt.Rows[i]["MARITALSTATUS"].ToString();
                    string OCCUPATION = dt.Rows[i]["OCCUPATION"].ToString();
                    string FURTHERDETAILS = dt.Rows[i]["FURTHERDETAILS"].ToString();
                    string PostalCode = dt.Rows[i]["PostalCode"].ToString();
                    if (PostalCode == "")
                    {
                        PostalCode = "";
                    }
                    else
                    {
                        PostalCode = "<cus:PostalCode>" + PostalCode + "</cus:PostalCode>";
                    }
                    string EmployersAddress = dt.Rows[i]["EmployersAddress"].ToString();
                    string HomePhoneNo = dt.Rows[i]["HomePhoneNo"].ToString();
                    string WorkPhoneNo = dt.Rows[i]["WorkPhoneNo"].ToString();
                    string MobilePhoneNo = dt.Rows[i]["MobilePhoneNo"].ToString();
                    string ResidenceYN = dt.Rows[i]["ResidenceYN"].ToString();
                    string EmailAddress = dt.Rows[i]["EmailAddress"].ToString();
                    string EmployerName = dt.Rows[i]["EmployerName"].ToString();
                    string TaxpayerRegistrationNo = dt.Rows[i]["TaxpayerRegistrationNo"].ToString();
                    string BlockedYN = dt.Rows[i]["BlockedYN"].ToString();
                    string AMKStaff = dt.Rows[i]["AMKStaff"].ToString();
                    string NoFamilyMember = dt.Rows[i]["NoFamilyMember"].ToString();
                    string EmployeeCode = dt.Rows[i]["EmployeeCode"].ToString();
                    string DateofEmployment = dt.Rows[i]["DateofEmployment"].ToString();
                    string FaxNo = dt.Rows[i]["FaxNo"].ToString();
                    string Profession = dt.Rows[i]["Profession"].ToString();
                    string EmploymentStatus = dt.Rows[i]["EmploymentStatus"].ToString();
                    string MainSourceofIncome = dt.Rows[i]["MainSourceofIncome"].ToString();
                    string EmployersCode = dt.Rows[i]["EmployersCode"].ToString();
                    string SpouseName = dt.Rows[i]["SpouseName"].ToString();
                    string SpouseOccupation = dt.Rows[i]["SpouseOccupation"].ToString();
                    string ConsenttoDisclosure = dt.Rows[i]["ConsenttoDisclosure"].ToString();
                    string DateofSignature = dt.Rows[i]["DateofSignature"].ToString();
                    string PlaceofStudy = dt.Rows[i]["PlaceofStudy"].ToString();
                    string DurationofCourse = dt.Rows[i]["DurationofCourse"].ToString();
                    string FieldofStudy = dt.Rows[i]["FieldofStudy"].ToString();
                    string NominationForm = dt.Rows[i]["NominationForm"].ToString();
                    string NominationCustomer = dt.Rows[i]["NominationCustomer"].ToString();
                    string NominationBeneficiary = dt.Rows[i]["NominationBeneficiary"].ToString();
                    string NominationAddress1 = dt.Rows[i]["NominationAddress1"].ToString();
                    string NominationAddress2 = dt.Rows[i]["NominationAddress2"].ToString();
                    string NominationAddress3 = dt.Rows[i]["NominationAddress3"].ToString();
                    string NominationAddress4 = dt.Rows[i]["NominationAddress4"].ToString();
                    string INSPostalCode = dt.Rows[i]["INSPostalCode"].ToString();
                    if (INSPostalCode == "")
                    {
                        INSPostalCode = "";
                    }
                    else
                    {
                        INSPostalCode = "<cus:INSPostalCode>" + INSPostalCode + "</cus:INSPostalCode>";
                    }
                    string INSPhoneNo = dt.Rows[i]["INSPhoneNo"].ToString();
                    string RelationshiptoCustomer = dt.Rows[i]["RelationshiptoCustomer"].ToString();
                    string AmountorofLegacy = dt.Rows[i]["AmountorofLegacy"].ToString();
                    string NameofBusiness = dt.Rows[i]["NameofBusiness"].ToString();
                    string NatureofBusiness = dt.Rows[i]["NatureofBusiness"].ToString();
                    string BusinessType = dt.Rows[i]["BusinessType"].ToString();
                    string BusinessPlan = dt.Rows[i]["BusinessPlan"].ToString();
                    string RoleinBusiness = dt.Rows[i]["RoleinBusiness"].ToString();
                    string BusinessAddress = dt.Rows[i]["BusinessAddress"].ToString();
                    string BusinessAddress2 = dt.Rows[i]["BusinessAddress2"].ToString();
                    string BusinessAddress3 = dt.Rows[i]["BusinessAddress3"].ToString();
                    string BusinessAddress4 = dt.Rows[i]["BusinessAddress4"].ToString();
                    string TaxClearanceHeldYN = dt.Rows[i]["TaxClearanceHeldYN"].ToString();
                    string TaxClearanceExpiryDate = dt.Rows[i]["TaxClearanceExpiryDate"].ToString();
                    string Student = dt.Rows[i]["Student"].ToString();
                    string AMKStaffType = dt.Rows[i]["AMKStaffType"].ToString();
                    string SpouseCustomerNumber = dt.Rows[i]["SpouseCustomerNumber"].ToString();
                    string NoofEmployees = dt.Rows[i]["NoofEmployees"].ToString();
                    string ProvinceCity = dt.Rows[i]["ProvinceCity"].ToString();
                    string District = dt.Rows[i]["District"].ToString();
                    string Commune = dt.Rows[i]["Commune"].ToString();
                    string Village = dt.Rows[i]["Village"].ToString();
                    string SpouseNameinKhmer = dt.Rows[i]["SpouseNameinKhmer"].ToString();
                    string SurnameKhmer = dt.Rows[i]["SurnameKhmer"].ToString();
                    string FirstNameKhmer = dt.Rows[i]["FirstNameKhmer"].ToString();
                    string SpouseDateofBirth = dt.Rows[i]["SpouseDateofBirth"].ToString();
                    string SpouseIDType = dt.Rows[i]["SpouseIDType"].ToString();
                    string SpouseIDNumber = dt.Rows[i]["SpouseIDNumber"].ToString();
                    string SpouseIDIssueDate = dt.Rows[i]["SpouseIDIssueDate"].ToString();
                    string SpouseIDExpiryDate = dt.Rows[i]["SpouseIDExpiryDate"].ToString();
                    string NoActiveMember = dt.Rows[i]["NoActiveMember"].ToString();
                    string ProvertyStatus = dt.Rows[i]["ProvertyStatus"].ToString();
                    string VillageBank = dt.Rows[i]["VillageBank"].ToString();
                    if (VillageBank == "0")
                    {
                        VillageBank = "";
                    }
                    else
                    {
                        VillageBank = "<cus:VillageBank>" + VillageBank + "</cus:VillageBank>";
                    }
                    string VillageBankPresident = dt.Rows[i]["VillageBankPresident"].ToString();
                    string MobileNumberType = dt.Rows[i]["MobileNumberType"].ToString();
                    string PlaceofBirth = dt.Rows[i]["PlaceofBirth"].ToString();
                    string BlacklistStatus = dt.Rows[i]["BlacklistStatus"].ToString();
                    string RiskStatus = dt.Rows[i]["RiskStatus"].ToString();
                    string BlackListCheckDate = dt.Rows[i]["BlackListCheckDate"].ToString();
                    string BlacklistCurrentStatus = dt.Rows[i]["BlacklistCurrentStatus"].ToString();
                    string HighRiskCateogries = dt.Rows[i]["HighRiskCateogries"].ToString();
                    string FATCAStatus = dt.Rows[i]["FATCAStatus"].ToString();
                    string FATCACountryofBirth = dt.Rows[i]["FATCACountryofBirth"].ToString();
                    string FATCANationality = dt.Rows[i]["FATCANationality"].ToString();
                    string FATCATaxIdentNumber = dt.Rows[i]["FATCATaxIdentNumber"].ToString();
                    string FATCAAddress = dt.Rows[i]["FATCAAddress"].ToString();
                    string FATCAPostalCode = dt.Rows[i]["FATCAPostalCode"].ToString();
                    string USTaxIdentificationNumber = dt.Rows[i]["USTaxIdentificationNumber"].ToString();
                    string FATCACountryCode = dt.Rows[i]["FATCACountryCode"].ToString();
                    string RecalcitrantStatus = dt.Rows[i]["RecalcitrantStatus"].ToString();
                    string OccupationType = dt.Rows[i]["OccupationType"].ToString();
                    string OccupationDetails = dt.Rows[i]["OccupationDetails"].ToString();
                    string IDType = dt.Rows[i]["IDType"].ToString();
                    string IDNumber = dt.Rows[i]["IDNumber"].ToString();
                    string IssueDate = dt.Rows[i]["IssueDate"].ToString();
                    string ExpiryDate = dt.Rows[i]["ExpiryDate"].ToString();
                    string Position = dt.Rows[i]["Position"].ToString();
                    string Department = dt.Rows[i]["Department"].ToString();
                    #endregion
                    
                    #region CustomerID - Add/Edit
                    try
                    {
                        string isError = "", ErrXml = "", Remark = "";
                        string xmlStr = "";
                        int OneSingleTwoWithSpouce = 1;
                        DateTime dt_msgIDDate = DateTime.Now;
                        string msgIDDate = dt_msgIDDate.ToString("yyyy-MM-dd hhmmss");
                        #region xml
                        #region MARRIED or Single
                        if (MARITALSTATUS == "MARRIED")
                        {
                            if (LoanAppPersonTypeID == "32" || LoanAppPersonTypeID == "34")
                            {
                                //xml single
                                OneSingleTwoWithSpouce = 1;
                            }
                            else
                            {
                                //xml with spouse
                                OneSingleTwoWithSpouce = 2;
                            }
                        }
                        else
                        {
                            //xml single
                            OneSingleTwoWithSpouce = 1;
                        }
                        #endregion
                        #region IDCardList
                        string IDCardList = "", KYCServerIDList = "";
                        if (1 == 1) {
                            DataTable dtIDCard = ReturnDT("exec sp_LoanAppPersonKYCGetForPostToT24 @LoanAppPersonID='" + LoanAppPersonID + "'");
                            for (int i2 = 0; i2 < dtIDCard.Rows.Count; i2++)
                            {
                                KYCServerIDList = (KYCServerIDList == "") ? dtIDCard.Rows[i2]["KYCServerID"].ToString() : KYCServerIDList + ","+dtIDCard.Rows[i2]["KYCServerID"].ToString();

                                #region Issue Date | ExpiryDateTag | SpouseIDIssueDate
                                string IssueDateTag = "";
                                if (dtIDCard.Rows[i2]["IDIssueDate"].ToString() == "1900-01-01") { }
                                else if (dtIDCard.Rows[i2]["IDIssueDate"].ToString() == "") { }
                                else
                                {
                                    IssueDateTag = "<cus:IssueDate>" + dtIDCard.Rows[i2]["IDIssueDate"].ToString() + "</cus:IssueDate>";
                                }
                                string ExpiryDateTag = "";
                                if (dtIDCard.Rows[i2]["IDExpireDate"].ToString() == "1900-01-01"){}
                                else if (dtIDCard.Rows[i2]["IDExpireDate"].ToString() == "") { }
                                else
                                {
                                    ExpiryDateTag = "<cus:ExpiryDate>" + dtIDCard.Rows[i2]["IDExpireDate"].ToString() + "</cus:ExpiryDate>";

                                }
                                //string SpouseIDIssueDateTag = "";
                                //if (SpouseIDIssueDate != "1900-01-01")
                                //{
                                //    SpouseIDIssueDateTag = "<cus:SpouseIDIssueDate>" + SpouseIDIssueDate + "</cus:SpouseIDIssueDate>";
                                //}
                                //string SpouseIDExpiryDateTag = "";
                                //if (SpouseIDExpiryDate != "1900-01-01")
                                //{
                                //    SpouseIDExpiryDateTag = "<cus:SpouseIDExpiryDate>" + SpouseIDExpiryDate + "</cus:SpouseIDExpiryDate>";
                                //}
                                #endregion Issue Date

                                string idList = "<cus:mAMK.ID.TYPE m=\"" + dtIDCard.Rows[i2]["IDCardType"].ToString() + "\">"
                                    + "<cus:IDType>" + dtIDCard.Rows[i2]["IDCardType"].ToString() + "</cus:IDType>"
                                    + "<cus:IDNumber>" + dtIDCard.Rows[i2]["IDNumber"].ToString() + "</cus:IDNumber>"
                                    + IssueDateTag + ExpiryDateTag
                                    + "</cus:mAMK.ID.TYPE>";

                                IDCardList = IDCardList + idList;

                            }
                        }
                        #endregion
                        #region IDCardListSpouse
                        //if (OneSingleTwoWithSpouce != 1) {
                        //    #region IDCardList
                        //    string IDCardListSpouse = "";
                        //    if (1 == 1)
                        //    {
                        //        DataTable dtIDCard = ReturnDT("exec sp_LoanAppPersonKYCGetForPostToT24 @LoanAppPersonID='" + LoanAppPersonIDSpouse + "'");
                        //        for (int i2 = 0; i2 < dtIDCard.Rows.Count; i2++)
                        //        {
                        //            #region Issue Date | ExpiryDateTag | SpouseIDIssueDate
                        //            //string IssueDateTag = "";
                        //            //if (dtIDCard.Rows[i2]["IDIssueDate"].ToString() != "1900-01-01")
                        //            //{
                        //            //    IssueDateTag = "<cus:IssueDate>" + dtIDCard.Rows[i2]["IDIssueDate"].ToString() + "</cus:IssueDate>";
                        //            //}
                        //            //string ExpiryDateTag = "";
                        //            //if (dtIDCard.Rows[i2]["IDExpireDate"].ToString() != "1900-01-01")
                        //            //{
                        //            //    ExpiryDateTag = "<cus:ExpiryDate>" + dtIDCard.Rows[i2]["IDExpireDate"].ToString() + "</cus:ExpiryDate>";
                        //            //}
                        //            string SpouseIDIssueDateTag = "";
                        //            if (dtIDCard.Rows[i2]["IDIssueDate"].ToString() != "1900-01-01")
                        //            {
                        //                SpouseIDIssueDateTag = "<cus:SpouseIDIssueDate>" + dtIDCard.Rows[i2]["IDIssueDate"].ToString() + "</cus:SpouseIDIssueDate>";
                        //            }
                        //            string SpouseIDExpiryDateTag = "";
                        //            if (dtIDCard.Rows[i2]["IDExpireDate"].ToString() != "1900-01-01")
                        //            {
                        //                SpouseIDExpiryDateTag = "<cus:SpouseIDExpiryDate>" + dtIDCard.Rows[i2]["IDExpireDate"].ToString() + "</cus:SpouseIDExpiryDate>";
                        //            }
                        //            #endregion Issue Date

                        //            string idList = "<cus:mAMK.SP.ID.TYPE m=\"1\">"
                        //            + "<cus:SpouseIDType>" + SpouseIDType + "</cus:SpouseIDType>"
                        //            + "<cus:SpouseIDNumber>" + SpouseIDNumber + "</cus:SpouseIDNumber>"
                        //            + SpouseIDIssueDateTag
                        //            + SpouseIDExpiryDateTag
                        //            + "</cus:mAMK.SP.ID.TYPE>";

                        //            IDCardListSpouse = IDCardListSpouse + idList;

                        //        }
                        //    }
                        //    #endregion
                        //}
                        #endregion
                        #region KYC Image
                        string KYCImageList = "";
                        if (1 == 1) {
                            DataTable dtIDCardImg = ReturnDT("exec sp_LoanAppPersonKYCImageGetForPostToT24 @KYCServerID='" + KYCServerIDList + "'");
                            for (int i2 = 0; i2 < dtIDCardImg.Rows.Count; i2++)
                            {
                                string KYCImgName = dtIDCardImg.Rows[i2]["KYCImgName"].ToString();
                                KYCImageList = KYCImageList+ "<cus:KYCImage>"+ KYCImgName + "</cus:KYCImage>";
                            }
                        }
                        #endregion
                        if (OneSingleTwoWithSpouce == 1)
                        {
                            #region xml single
                            xmlStr = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:amk=\"http://temenos.com/AMKINDCUST\" xmlns:cus=\"http://temenos.com/CUSTOMERAMKINDIVWS\"><soapenv:Header/>"
                            + "<soapenv:Body><amk:INDIVCUSTOMERCREATION><WebRequestCommon><company>" + CreCompany + "</company><password>" + CrePassword + "</password>"
                            + "<userName>" + CreUserName + "</userName></WebRequestCommon>"
                            + "<OfsFunction>"
                            + "<messageId>" + "C-" + LoanAppPersonID + "-" + SHORTNAME + "-" + msgIDDate + "</messageId>"
                            + "</OfsFunction>"
                            + "<CUSTOMERAMKINDIVWSType id=\"" + CustomerID + "\">"
                            + "<cus:gSHORTNAME g=\"1\"><cus:SHORTNAME>" + SHORTNAME + "</cus:SHORTNAME></cus:gSHORTNAME><cus:gNAME1 g=\"1\"><cus:NAME1>" + NAME1
                            + "</cus:NAME1></cus:gNAME1><cus:gSTREET g=\"1\"><cus:STREET>" + STREET + "</cus:STREET></cus:gSTREET><cus:gRELATIONCODE g=\"1\">"
                            + "<cus:mRELATIONCODE m=\"1\"><cus:RELATIONCODE>" + RELATIONCODE + "</cus:RELATIONCODE><cus:RELCUSTOMER>" + RELCUSTOMER
                            + "</cus:RELCUSTOMER></cus:mRELATIONCODE></cus:gRELATIONCODE><cus:SECTOR>" + SECTOR + "</cus:SECTOR><cus:ACCOUNTOFFICER>" + ACCOUNTOFFICER
                            + "</cus:ACCOUNTOFFICER><cus:INDUSTRY>" + INDUSTRY + "</cus:INDUSTRY><cus:TARGET>" + TARGET + "</cus:TARGET><cus:NATIONALITY>" + NATIONALITY
                            + "</cus:NATIONALITY><cus:CUSTOMERSTATUS>" + CUSTOMERSTATUS + "</cus:CUSTOMERSTATUS><cus:RESIDENCE>" + RESIDENCE
                            + "</cus:RESIDENCE><cus:BIRTHINCORPDATE>" + BIRTHINCORPDATE + "</cus:BIRTHINCORPDATE><cus:TITLE>" + TITLE + "</cus:TITLE><cus:GENDER>" + GENDER + "</cus:GENDER><cus:MARITALSTATUS>" + MARITALSTATUS
                            + "</cus:MARITALSTATUS><cus:OCCUPATION>" + OCCUPATION + "</cus:OCCUPATION><cus:gFURTHERDETAILS g=\"1\">"
                            + "<cus:FURTHERDETAILS>" + FURTHERDETAILS + "</cus:FURTHERDETAILS></cus:gFURTHERDETAILS>" + PostalCode + "<cus:gEMPLOY.BUS.ADDR g=\"1\"><cus:EmployersAddress>" + EmployersAddress
                            + "</cus:EmployersAddress></cus:gEMPLOY.BUS.ADDR><cus:HomePhoneNo>" + HomePhoneNo + "</cus:HomePhoneNo><cus:WorkPhoneNo>" + WorkPhoneNo
                            + "</cus:WorkPhoneNo><cus:MobilePhoneNo>" + MobilePhoneNo + "</cus:MobilePhoneNo><cus:ResidenceYN>" + ResidenceYN
                            + "</cus:ResidenceYN><cus:EmailAddress>" + EmailAddress + "</cus:EmailAddress><cus:EmployerName>" + EmployerName
                            + "</cus:EmployerName><cus:TaxpayerRegistrationNo>" + TaxpayerRegistrationNo + "</cus:TaxpayerRegistrationNo><cus:BlockedYN>" + BlockedYN
                            + "</cus:BlockedYN><cus:AMKStaff>" + AMKStaff + "</cus:AMKStaff><cus:NoFamilyMember>" + NoFamilyMember
                            + "</cus:NoFamilyMember><cus:EmployeeCode>" + EmployeeCode + "</cus:EmployeeCode>"
                            //+"<cus:DateofEmployment>" + DateofEmployment + "</cus:DateofEmployment>"
                            //+"<cus:FaxNo>" + FaxNo + "</cus:FaxNo>"
                            + "<cus:Profession>" + Profession + "</cus:Profession>"
                            //+"<cus:EmploymentStatus>" + EmploymentStatus + "</cus:EmploymentStatus>"
                            + "<cus:MainSourceofIncome>" + MainSourceofIncome + "</cus:MainSourceofIncome>"
                            //+"<cus:EmployersCode>" + EmployersCode + "</cus:EmployersCode>"
                            //+"<cus:SpouseName></cus:SpouseName>"
                            //+ "<cus:SpouseOccupation></cus:SpouseOccupation>"
                            + "<cus:ConsenttoDisclosure>" + ConsenttoDisclosure + "</cus:ConsenttoDisclosure>"
                            + "<cus:DateofSignature>" + DateofSignature + "</cus:DateofSignature>"

                            //+ "<cus:gPLACE.STUDY g=\"1\"><cus:mPLACE.STUDY m=\"1\">"
                            //+ "<cus:PlaceofStudy>" + PlaceofStudy + "</cus:PlaceofStudy>"
                            //+ "<cus:DurationofCourse>" + DurationofCourse + "</cus:DurationofCourse>"
                            //+ "<cus:FieldofStudy>" + FieldofStudy + "</cus:FieldofStudy>"
                            //+"</cus:mPLACE.STUDY></cus:gPLACE.STUDY>"

                            //+ "<cus:NominationForm>" + NominationForm + "</cus:NominationForm>"

                            //+"<cus:gINS.MEM.NO g=\"1\"><cus:mINS.MEM.NO m=\"1\">"
                            //+ "<cus:NominationCustomer>" + NominationCustomer + "</cus:NominationCustomer>"
                            //+"<cus:NominationBeneficiary>" + NominationBeneficiary + "</cus:NominationBeneficiary>"
                            //+"<cus:NominationAddress1>" + NominationAddress1 + "</cus:NominationAddress1>"
                            //+"<cus:NominationAddress2>" + NominationAddress2 + "</cus:NominationAddress2>"
                            //+"<cus:NominationAddress3>" + NominationAddress3 + "</cus:NominationAddress3>"
                            //+"<cus:NominationAddress4>" + NominationAddress4 + "</cus:NominationAddress4>"
                            //+INSPostalCode
                            //+"<cus:INSPhoneNo>" + INSPhoneNo + "</cus:INSPhoneNo>"
                            //+"<cus:RelationshiptoCustomer>" + RelationshiptoCustomer + "</cus:RelationshiptoCustomer>"
                            //+ "<cus:AmountorofLegacy>" + AmountorofLegacy + "</cus:AmountorofLegacy>"
                            //+"</cus:mINS.MEM.NO></cus:gINS.MEM.NO>"

                            //+"<cus:gBUS.NAME g=\"1\"><cus:mBUS.NAME m=\"1\">"
                            //+"<cus:NameofBusiness>" + NameofBusiness + "</cus:NameofBusiness>"
                            //+"<cus:NatureofBusiness>" + NatureofBusiness + "</cus:NatureofBusiness>"
                            //+"<cus:BusinessType>" + BusinessType + "</cus:BusinessType>"
                            //+"<cus:BusinessPlan>" + BusinessPlan + "</cus:BusinessPlan>"
                            //+"<cus:RoleinBusiness>" + RoleinBusiness + "</cus:RoleinBusiness>"
                            //+"<cus:BusinessAddress>" + BusinessAddress + "</cus:BusinessAddress>"
                            //+"<cus:BusinessAddress2>" + BusinessAddress2 + "</cus:BusinessAddress2>"
                            //+"<cus:BusinessAddress3>" + BusinessAddress3 + "</cus:BusinessAddress3>"
                            //+"<cus:BusinessAddress4>" + BusinessAddress4 + "</cus:BusinessAddress4>"
                            //+"<cus:TaxClearanceHeldYN>" + TaxClearanceHeldYN + "</cus:TaxClearanceHeldYN>"
                            //+"<cus:TaxClearanceExpiryDate>" + TaxClearanceExpiryDate + "</cus:TaxClearanceExpiryDate>"
                            //+"</cus:mBUS.NAME></cus:gBUS.NAME>"

                            //+"<cus:Student>" + Student + "</cus:Student>"
                            //+"<cus:AMKStaffType>" + AMKStaffType + "</cus:AMKStaffType>"
                            //+"<cus:SpouseCustomerNumber></cus:SpouseCustomerNumber>"
                            //+"<cus:NoofEmployees>" + NoofEmployees + "</cus:NoofEmployees>"
                            + "<cus:ProvinceCity>" + ProvinceCity + "</cus:ProvinceCity><cus:District>" + District
                            + "</cus:District><cus:Commune>" + Commune + "</cus:Commune><cus:Village>" + Village + "</cus:Village><cus:SurnameKhmer>" + SurnameKhmer
                            + "</cus:SurnameKhmer><cus:FirstNameKhmer>" + FirstNameKhmer + "</cus:FirstNameKhmer><cus:NoActiveMember>" + NoActiveMember
                            + "</cus:NoActiveMember><cus:ProvertyStatus>" + ProvertyStatus + "</cus:ProvertyStatus>"
                            + VillageBank
                            + "<cus:VillageBankPresident>" + VillageBankPresident + "</cus:VillageBankPresident><cus:MobileNumberType>" + MobileNumberType
                            + "</cus:MobileNumberType><cus:PlaceofBirth>" + PlaceofBirth + "</cus:PlaceofBirth>"

                            //+"<cus:BlacklistStatus>" + BlacklistStatus + "</cus:BlacklistStatus>"
                            //+"<cus:RiskStatus>" + RiskStatus + "</cus:RiskStatus>"
                            //+"<cus:BlackListCheckDate>" + BlackListCheckDate + "</cus:BlackListCheckDate>"
                            //+"<cus:BlacklistCurrentStatus>" + BlacklistCurrentStatus + "</cus:BlacklistCurrentStatus>"
                            //+ "<cus:HighRiskCateogries>" + HighRiskCateogries + "</cus:HighRiskCateogries>"
                            + "<cus:FATCAStatus>" + FATCAStatus + "</cus:FATCAStatus>"
                            + "<cus:FATCACountryofBirth>" + FATCACountryofBirth + "</cus:FATCACountryofBirth>"

                            + "<cus:gAMK.F.I.NAT g=\"1\"><cus:mAMK.F.I.NAT m=\"1\">"
                            + "<cus:FATCANationality>" + FATCANationality + "</cus:FATCANationality>"
                            + "<cus:FATCATaxIdentNumber>" + FATCATaxIdentNumber + "</cus:FATCATaxIdentNumber>"
                            + "</cus:mAMK.F.I.NAT></cus:gAMK.F.I.NAT>"

                            //+"<cus:gAMK.F.I.ADDRESS g=\"1\">"
                            //+"<cus:FATCAAddress>" + FATCAAddress + "</cus:FATCAAddress>"
                            //+"</cus:gAMK.F.I.ADDRESS>"

                            //+"<cus:FATCAPostalCode>" + FATCAPostalCode + "</cus:FATCAPostalCode>"
                            //+"<cus:USTaxIdentificationNumber>" + USTaxIdentificationNumber + "</cus:USTaxIdentificationNumber>"
                            //+ "<cus:FATCACountryCode>" + FATCACountryCode + "</cus:FATCACountryCode>"
                            //+"<cus:RecalcitrantStatus>" + RecalcitrantStatus + "</cus:RecalcitrantStatus>"
                            + "<cus:OccupationType>" + OccupationType + "</cus:OccupationType>"
                            + "<cus:OccupationDetails>" + OccupationDetails + "</cus:OccupationDetails>"

                            + "<cus:gAMK.ID.TYPE g=\"1\">"
                            + IDCardList
                            + "</cus:gAMK.ID.TYPE>"

                            //+ "<cus:gAMK.KYC.IMAGE>"
                            //+ KYCImageList
                            //+ "</cus:gAMK.KYC.IMAGE>"

                            //+"<cus:Position>" + Position + "</cus:Position>"
                            //+"<cus:Department>" + Department + "</cus:Department>"
                            //+"<cus:gAMK.KYC.IMAGE g=\"1\"><cus:KYCImage></cus:KYCImage></cus:gAMK.KYC.IMAGE>"

                            + "</CUSTOMERAMKINDIVWSType></amk:INDIVCUSTOMERCREATION></soapenv:Body></soapenv:Envelope>";
                            #endregion xml single
                        }
                        else
                        {
                            #region xml with spouse
                            xmlStr = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" "
                            + "xmlns:amk=\"http://temenos.com/AMKINDCUST\" xmlns:cus=\"http://temenos.com/CUSTOMERAMKINDIVWS\"><soapenv:Header/>"
                            + "<soapenv:Body><amk:INDIVCUSTOMERCREATION><WebRequestCommon><company>" + CreCompany + "</company><password>" + CrePassword + "</password>"
                            + "<userName>" + CreUserName + "</userName></WebRequestCommon>"
                            + "<OfsFunction>"
                            + "<messageId>" + "C-" + LoanAppPersonID + "-" + SHORTNAME + "-" + msgIDDate + "</messageId>"
                            + "</OfsFunction>"
                            + "<CUSTOMERAMKINDIVWSType id=\"" + CustomerID + "\">"
                            + "<cus:gSHORTNAME g=\"1\"><cus:SHORTNAME>" + SHORTNAME + "</cus:SHORTNAME></cus:gSHORTNAME>"
                            + "<cus:gNAME1 g=\"1\"><cus:NAME1>" + NAME1 + "</cus:NAME1></cus:gNAME1>"
                            + "<cus:gSTREET g=\"1\"><cus:STREET>" + STREET + "</cus:STREET></cus:gSTREET>"
                            + "<cus:gRELATIONCODE g=\"1\">"
                            + "<cus:mRELATIONCODE m=\"1\"><cus:RELATIONCODE>" + RELATIONCODE + "</cus:RELATIONCODE>"
                            + "<cus:RELCUSTOMER>" + RELCUSTOMER + "</cus:RELCUSTOMER>"
                            + "</cus:mRELATIONCODE></cus:gRELATIONCODE>"
                            + "<cus:SECTOR>" + SECTOR + "</cus:SECTOR>"
                            + "<cus:ACCOUNTOFFICER>" + ACCOUNTOFFICER + "</cus:ACCOUNTOFFICER>"
                            + "<cus:INDUSTRY>" + INDUSTRY + "</cus:INDUSTRY>"
                            + "<cus:TARGET>" + TARGET + "</cus:TARGET>"
                            + "<cus:NATIONALITY>" + NATIONALITY + "</cus:NATIONALITY>"
                            + "<cus:CUSTOMERSTATUS>" + CUSTOMERSTATUS + "</cus:CUSTOMERSTATUS>"
                            + "<cus:RESIDENCE>" + RESIDENCE + "</cus:RESIDENCE>"
                            + "<cus:BIRTHINCORPDATE>" + BIRTHINCORPDATE + "</cus:BIRTHINCORPDATE>"
                            + "<cus:TITLE>" + TITLE + "</cus:TITLE><cus:GENDER>" + GENDER + "</cus:GENDER>"
                            + "<cus:MARITALSTATUS>" + MARITALSTATUS + "</cus:MARITALSTATUS>"
                            //+"<cus:OCCUPATION>" + OCCUPATION + "</cus:OCCUPATION>"
                            //+"<cus:gFURTHERDETAILS g=\"1\">"
                            //+"<cus:FURTHERDETAILS>" + FURTHERDETAILS + "</cus:FURTHERDETAILS>"
                            //+"</cus:gFURTHERDETAILS>" 
                            + PostalCode
                            //+ "<cus:gEMPLOY.BUS.ADDR g=\"1\">"
                            //+ "<cus:EmployersAddress>" + EmployersAddress + "</cus:EmployersAddress>"
                            //+"</cus:gEMPLOY.BUS.ADDR>"
                            //+"<cus:HomePhoneNo>" + HomePhoneNo + "</cus:HomePhoneNo>"
                            //+"<cus:WorkPhoneNo>" + WorkPhoneNo + "</cus:WorkPhoneNo>"
                            + "<cus:MobilePhoneNo>" + MobilePhoneNo + "</cus:MobilePhoneNo>"
                            + "<cus:ResidenceYN>" + ResidenceYN + "</cus:ResidenceYN>"
                            //+"<cus:EmailAddress>" + EmailAddress + "</cus:EmailAddress>"
                            //+"<cus:EmployerName>" + EmployerName + "</cus:EmployerName>"
                            //+"<cus:TaxpayerRegistrationNo>" + TaxpayerRegistrationNo + "</cus:TaxpayerRegistrationNo>"
                            + "<cus:BlockedYN>" + BlockedYN + "</cus:BlockedYN>"
                            + "<cus:AMKStaff>" + AMKStaff + "</cus:AMKStaff>"
                            + "<cus:NoFamilyMember>" + NoFamilyMember + "</cus:NoFamilyMember>"
                            //+"<cus:EmployeeCode>" + EmployeeCode + "</cus:EmployeeCode>"
                            //+"<cus:DateofEmployment>" + DateofEmployment + "</cus:DateofEmployment>"
                            //+"<cus:FaxNo>" + FaxNo + "</cus:FaxNo>"
                            + "<cus:Profession>" + Profession + "</cus:Profession>"
                            //+"<cus:EmploymentStatus>" + EmploymentStatus + "</cus:EmploymentStatus>"
                            + "<cus:MainSourceofIncome>" + MainSourceofIncome + "</cus:MainSourceofIncome>"
                            //+"<cus:EmployersCode>" + EmployersCode + "</cus:EmployersCode>"
                            + "<cus:SpouseName>" + SpouseName + "</cus:SpouseName>"
                            //+"<cus:SpouseOccupation>" + SpouseOccupation + "</cus:SpouseOccupation>"
                            + "<cus:ConsenttoDisclosure>" + ConsenttoDisclosure + "</cus:ConsenttoDisclosure>"
                            + "<cus:DateofSignature>" + DateofSignature + "</cus:DateofSignature>"

                            //+"<cus:gPLACE.STUDY g=\"1\"><cus:mPLACE.STUDY m=\"1\">"
                            //+"<cus:PlaceofStudy>" + PlaceofStudy + "</cus:PlaceofStudy>"
                            //+"<cus:DurationofCourse>" + DurationofCourse + "</cus:DurationofCourse>"
                            //+"<cus:FieldofStudy>" + FieldofStudy + "</cus:FieldofStudy>"
                            //+"</cus:mPLACE.STUDY></cus:gPLACE.STUDY>"

                            //+"<cus:NominationForm>" + NominationForm + "</cus:NominationForm>"

                            //+"<cus:gINS.MEM.NO g=\"1\"><cus:mINS.MEM.NO m=\"1\">"
                            //+"<cus:NominationCustomer>" + NominationCustomer + "</cus:NominationCustomer>"
                            //+"<cus:NominationBeneficiary>" + NominationBeneficiary + "</cus:NominationBeneficiary>"
                            //+"<cus:NominationAddress1>" + NominationAddress1 + "</cus:NominationAddress1>"
                            //+"<cus:NominationAddress2>" + NominationAddress2 + "</cus:NominationAddress2>"
                            //+"<cus:NominationAddress3>" + NominationAddress3 + "</cus:NominationAddress3>"
                            //+"<cus:NominationAddress4>" + NominationAddress4 + "</cus:NominationAddress4>"
                            //+ INSPostalCode 
                            //+ "<cus:INSPhoneNo>" + INSPhoneNo + "</cus:INSPhoneNo>"
                            //+"<cus:RelationshiptoCustomer>" + RelationshiptoCustomer + "</cus:RelationshiptoCustomer>"
                            //+"<cus:AmountorofLegacy>" + AmountorofLegacy + "</cus:AmountorofLegacy>"
                            //+"</cus:mINS.MEM.NO></cus:gINS.MEM.NO>"

                            //+"<cus:gBUS.NAME g=\"1\"><cus:mBUS.NAME m=\"1\">"
                            //+ "<cus:NameofBusiness>" + NameofBusiness + "</cus:NameofBusiness>"
                            //+"<cus:NatureofBusiness>" + NatureofBusiness + "</cus:NatureofBusiness>"
                            //+"<cus:BusinessType>" + BusinessType + "</cus:BusinessType>"
                            //+"<cus:BusinessPlan>" + BusinessPlan + "</cus:BusinessPlan>"
                            //+"<cus:RoleinBusiness>" + RoleinBusiness + "</cus:RoleinBusiness>"
                            //+"<cus:BusinessAddress>" + BusinessAddress + "</cus:BusinessAddress>"
                            //+"<cus:BusinessAddress2>" + BusinessAddress2 + "</cus:BusinessAddress2>"
                            //+"<cus:BusinessAddress3>" + BusinessAddress3 + "</cus:BusinessAddress3>"
                            //+"<cus:BusinessAddress4>" + BusinessAddress4 + "</cus:BusinessAddress4>"
                            //+"<cus:TaxClearanceHeldYN>" + TaxClearanceHeldYN + "</cus:TaxClearanceHeldYN>"
                            //+"<cus:TaxClearanceExpiryDate>" + TaxClearanceExpiryDate + "</cus:TaxClearanceExpiryDate>"
                            //+"</cus:mBUS.NAME></cus:gBUS.NAME>"

                            //+"<cus:Student>" + Student + "</cus:Student>"
                            //+"<cus:AMKStaffType>" + AMKStaffType + "</cus:AMKStaffType>"
                            + "<cus:SpouseCustomerNumber>" + SpouseCustomerNumber + "</cus:SpouseCustomerNumber>"
                            //+"<cus:NoofEmployees>" + NoofEmployees + "</cus:NoofEmployees>"
                            + "<cus:ProvinceCity>" + ProvinceCity + "</cus:ProvinceCity>"
                            + "<cus:District>" + District + "</cus:District>"
                            + "<cus:Commune>" + Commune + "</cus:Commune>"
                            + "<cus:Village>" + Village + "</cus:Village>"
                            + "<cus:SpouseNameinKhmer>" + SpouseNameinKhmer + "</cus:SpouseNameinKhmer>"
                            + "<cus:SurnameKhmer>" + SurnameKhmer + "</cus:SurnameKhmer>"
                            + "<cus:FirstNameKhmer>" + FirstNameKhmer + "</cus:FirstNameKhmer>"
                            + "<cus:SpouseDateofBirth>" + SpouseDateofBirth + "</cus:SpouseDateofBirth>"

                            //+ "<cus:gAMK.SP.ID.TYPE g=\"1\">"
                            //+ IDCardListSpouse
                            //+"</cus:gAMK.SP.ID.TYPE>"

                            + "<cus:NoActiveMember>" + NoActiveMember + "</cus:NoActiveMember>"
                            + "<cus:ProvertyStatus>" + ProvertyStatus + "</cus:ProvertyStatus>"
                            + VillageBank
                            + "<cus:VillageBankPresident>" + VillageBankPresident + "</cus:VillageBankPresident>"
                            + "<cus:MobileNumberType>" + MobileNumberType + "</cus:MobileNumberType>"
                            + "<cus:PlaceofBirth>" + PlaceofBirth + "</cus:PlaceofBirth>"
                            //+"<cus:BlacklistStatus>" + BlacklistStatus + "</cus:BlacklistStatus>"
                            //+"<cus:RiskStatus>" + RiskStatus + "</cus:RiskStatus>"
                            //+"<cus:BlackListCheckDate>" + BlackListCheckDate + "</cus:BlackListCheckDate>"
                            //+"<cus:BlacklistCurrentStatus>" + BlacklistCurrentStatus + "</cus:BlacklistCurrentStatus>"
                            //+ "<cus:HighRiskCateogries>" + HighRiskCateogries + "</cus:HighRiskCateogries>"
                            + "<cus:FATCAStatus>" + FATCAStatus + "</cus:FATCAStatus>"
                            + "<cus:FATCACountryofBirth>" + FATCACountryofBirth + "</cus:FATCACountryofBirth>"

                            + "<cus:gAMK.F.I.NAT g=\"1\"><cus:mAMK.F.I.NAT m=\"1\">"
                            + "<cus:FATCANationality>" + FATCANationality + "</cus:FATCANationality>"
                            + "<cus:FATCATaxIdentNumber>" + FATCATaxIdentNumber + "</cus:FATCATaxIdentNumber>"
                            + "</cus:mAMK.F.I.NAT></cus:gAMK.F.I.NAT>"

                            //+"<cus:gAMK.F.I.ADDRESS g=\"1\">"
                            //+"<cus:FATCAAddress>" + FATCAAddress + "</cus:FATCAAddress>"
                            //+"</cus:gAMK.F.I.ADDRESS>"

                            //+ FATCAPostalCode 
                            //+ "<cus:USTaxIdentificationNumber>" + USTaxIdentificationNumber + "</cus:USTaxIdentificationNumber>"
                            //+"<cus:FATCACountryCode>" + FATCACountryCode + "</cus:FATCACountryCode>"
                            //+"<cus:RecalcitrantStatus>" + RecalcitrantStatus + "</cus:RecalcitrantStatus>"

                            + "<cus:OccupationType>" + OccupationType + "</cus:OccupationType>"
                            + "<cus:OccupationDetails>" + OccupationDetails + "</cus:OccupationDetails>"

                            + "<cus:gAMK.ID.TYPE g=\"1\">"
                            + IDCardList
                            + "</cus:gAMK.ID.TYPE>"

                            //+ "<cus:gAMK.KYC.IMAGE>"
                            //+ KYCImageList
                            //+ "</cus:gAMK.KYC.IMAGE>"

                            //+"<cus:Position>" + Position + "</cus:Position>"
                            //+"<cus:Department>" + Department + "</cus:Department>"
                            //+"<cus:gAMK.KYC.IMAGE g=\"1\"><cus:KYCImage></cus:KYCImage></cus:gAMK.KYC.IMAGE>"

                            + "</CUSTOMERAMKINDIVWSType>"
                            + "</amk:INDIVCUSTOMERCREATION></soapenv:Body></soapenv:Envelope>";
                            #endregion xml with spouse
                        }

                        #endregion xml
                        #region Call To T24
                        T24_AddLog(fileHeader, "AddEditCustomer_RQ", xmlStr, "LoanAdd");
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(CreUrl);
                        request.ContentType = "text/xml"; // or application/soap+xml for SOAP 1.2
                        request.Method = "POST";
                        request.KeepAlive = false;

                        byte[] byteArray = Encoding.UTF8.GetBytes(xmlStr);
                        request.ContentLength = byteArray.Length;

                        Stream requestStream = request.GetRequestStream();
                        requestStream.Write(byteArray, 0, byteArray.Length);
                        requestStream.Close();

                        HttpWebResponse response = null;
                        try
                        {
                            response = (HttpWebResponse)request.GetResponse();
                        }
                        catch (WebException ex)
                        {
                            response = (HttpWebResponse)ex.Response;
                        }

                        Stream receiveStream = response.GetResponseStream();
                        StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(readStream.ReadToEnd());
                        string successIndicator = doc.GetElementsByTagName("successIndicator").Item(0).InnerText;
                        string transactionId = doc.GetElementsByTagName("transactionId").Item(0).InnerText;
                        T24_AddLog(fileHeader, "AddEditCustomer_RS: " + (int)response.StatusCode, doc.InnerXml, "LoanAdd");

                        #endregion
                        #region Condiction - Success-> update and add log upload kyc/image to T24
                        if (successIndicator == "Success")
                        {
                            string[] rsUpdateCID = T24_LoanAppListForOpenToCBS2_UpdateCID(LoanAppPersonID, transactionId, LoanAppID, KYCServerIDList);
                            ERR = transactionId;
                            isError = (rsUpdateCID[0] == "Succeed") ? "0" : "1";
                            Remark = (rsUpdateCID[0] == "Succeed") ? ERR : rsUpdateCID[3];

                            var customerType = doc.GetElementsByTagName("CUSTOMERType");
                            var datas = XMLNoteListToDataTable.ConvertXmlNodeListToDataTable(customerType);
                            var data = datas.Select()[0];

                            var customerAML = new CustomerAmlReq();
                            customerAML.LoanAppID = LoanAppID;
                            customerAML.LoanAppPersonID = LoanAppPersonID;
                            customerAML.CreateBy = "";
                            
                            customerAML.BlockStatus = data[118].ToString();
                            customerAML.WatchListScreeningStatus = data[264].ToString();
                            customerAML.WatchListCaseUrl = data[265].ToString();
                            customerAML.RiskProfiling = data[266].ToString();
                            customerAML.AMLApprovalStatus = data[267].ToString();
                            customerAML.WatchListExposition = data[268].ToString(); 
                            customerAML.ProductAndService = data[269].ToString();

                            AddLoanAppPersonAMLService.AddLoanAMLCreation(customerAML);
                        }
                        else
                        {

                            string messages = doc.GetElementsByTagName("messages").Item(0).InnerText;
                            if (messages == "LIVE RECORD NOT CHANGED"
                                || messages == "COMPANY.BOOK:1:1=NOCHANGE FIELD" || messages == "EB-LIVE.RECORD.NOT.CHANGED")
                            {
                                ERR = CustomerID;
                                isError = "0";
                                Remark = ERR;
                            }
                            else
                            {

                                #region Error
                                xmlContent = doc.InnerXml;
                                if (xmlContent.Contains("<Status>"))
                                {
                                    int pFrom = xmlContent.IndexOf("<Status>");// + "<Status>".Length;
                                    int pTo = xmlContent.IndexOf("</Status>") + "</Status>".Length;
                                    string MSGxmlContent = xmlContent.Substring(pFrom, pTo - pFrom).Trim();
                                    LoanAppApproveToCBSError.Status obj = GenerateXmlObject<LoanAppApproveToCBSError.Status>(MSGxmlContent);
                                    string strMsg = "";
                                    for (int iMsg = 0; iMsg < obj.Messages.Count; iMsg++)
                                    {
                                        if (iMsg == 0)
                                        {
                                            strMsg = obj.Messages[iMsg];
                                        }
                                        else
                                        {
                                            strMsg = strMsg + " | " + obj.Messages[iMsg];
                                        }
                                    }
                                    SMS = strMsg;
                                    Remark = SMS;
                                }
                                #endregion Error

                                ERR = "Error";
                            }
                        }

                        #endregion

                        #region log
                        T24_LoanAppAddLogAdd(LoanAppID, LoanAppPersonID, fileHeader, isError, doc.InnerXml, Remark, "0");
                        #endregion log
                        #region Close Stream
                        readStream.Close();
                        requestStream.Close();
                        receiveStream.Close();
                        response.Close();
                        #endregion

                    }
                    catch (Exception ex)
                    {
                        //add error log
                        string line = GetLineNumber(ex).ToString();
                        SMS = ex.Message.ToString() + line;
                    }
                    #endregion CustomerID - Add/Edit
                }
            }
            catch (Exception ex)
            {
                ERR = "Error";
                SMS = ex.Message.ToString();
            }
            rs[0] = ERR;
            rs[1] = SMS;
            return rs;
        }

        public string[] AddLoanV2(string fileHeader, string LoanAppID, string CUSTOMERID, string api_name)
        {
            string[] rs = new string[2];
            string isError = "", ErrXml = "";
            string ERR = "Error", SMS = "", xmlContent = "", CBSKey = "", CBSAcc = "", Remark = "Start", LoanAppStatusID = "12";
            try
            {
                fileHeader = fileHeader.Replace(" ", "_").Replace("-", "_").Replace(":", "_").Replace(".", "_");
                string sql = "exec T24_LoanAppListForOpenToCBS3_P2_Loan @LoanAppID='" + LoanAppID + "',@LoanAppStatusID=3";
                if (api_name == "AM")
                {
                    //sql = "exec T24_LoanAppListForOpenToCBS3ForChangeDISBDate @LoanAppID='" + LoanAppID + "'";//need to change
                    sql = "exec T24_LoanAppListForOpenToCBS3_P2_Loan @LoanAppID='" + LoanAppID + "',@LoanAppStatusID=17";
                }
                DataTable dt = ReturnDT(sql);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    #region params
                    string CreUrl = dt.Rows[i]["CreUrl"].ToString();
                    string CreCompany = dt.Rows[i]["CreCompany"].ToString();
                    string CreUserName = dt.Rows[i]["CreUserName"].ToString();
                    string CrePassword = dt.Rows[i]["CrePassword"].ToString();

                    string APPDATE = dt.Rows[i]["APPDATE"].ToString();
                    string PRODUCTID = dt.Rows[i]["PRODUCTID"].ToString();
                    string CURRENCY = dt.Rows[i]["CURRENCY"].ToString();
                    string LOANTERM = dt.Rows[i]["LOANTERM"].ToString();
                    string AMOUNT = dt.Rows[i]["AMOUNT"].ToString();
                    string INTRATE = dt.Rows[i]["INTRATE"].ToString();
                    string REPAYSTDATE = dt.Rows[i]["REPAYSTDATE"].ToString();
                    string VILLAGEBANK = dt.Rows[i]["VILLAGEBANK"].ToString();
                    string LOANCYCLE = dt.Rows[i]["LOANCYCLE"].ToString();
                    string GROUPNO = dt.Rows[i]["GROUPNO"].ToString();
                    string OPERATION = dt.Rows[i]["OPERATION"].ToString();
                    string LOANREFERBY = dt.Rows[i]["LOANREFERBY"].ToString();
                    string CREDITOFFICER = dt.Rows[i]["CREDITOFFICER"].ToString();
                    string MAINBUSINESS = dt.Rows[i]["MAINBUSINESS"].ToString();
                    string SEMIBALLONFREQ = dt.Rows[i]["SEMIBALLONFREQ"].ToString();
                    //string LOANPURPOSE = dt.Rows[i]["LOANPURPOSE"].ToString();
                    string lp1 = dt.Rows[i]["lp1"].ToString();
                    string lp2 = dt.Rows[i]["lp2"].ToString();
                    string lp3 = dt.Rows[i]["lp3"].ToString();

                    string CBCREQUIRED = dt.Rows[i]["CBCREQUIRED"].ToString();
                    string GURANTORCODE = dt.Rows[i]["GURANTORCODE"].ToString();
                    //string COLLATERALCODE = dt.Rows[i]["COLLATERALCODE"].ToString();
                    //string COLLATERALCCY = dt.Rows[i]["COLLATERALCCY"].ToString();
                    //string COLLATERALTYPE = dt.Rows[i]["COLLATERALTYPE"].ToString();
                    //string PROPERTYTYPE = dt.Rows[i]["PROPERTYTYPE"].ToString();
                    //string COLLDOCTYPE = dt.Rows[i]["COLLDOCTYPE"].ToString();
                    //string COLLATERALQNTY = dt.Rows[i]["COLLATERALQNTY"].ToString();
                    //string COLLATERALUNIT = dt.Rows[i]["COLLATERALUNIT"].ToString();
                    //string COLLATERALPRICE = dt.Rows[i]["COLLATERALPRICE"].ToString();
                    //string COLLATERALVALUE = dt.Rows[i]["COLLATERALVALUE"].ToString();
                    //string TOTALCOLLATERAL = dt.Rows[i]["TOTALCOLLATERAL"].ToString();
                    //string COLLATERALLVR = dt.Rows[i]["COLLATERALLVR"].ToString();
                    string INCEXPCCY = dt.Rows[i]["INCEXPCCY"].ToString();
                    string TOTALINCOME = dt.Rows[i]["TOTALINCOME"].ToString();
                    string TOTALEXPENSE = dt.Rows[i]["TOTALEXPENSE"].ToString();
                    string AVGINCOME = dt.Rows[i]["AVGINCOME"].ToString();
                    string RESCHEDLOAN = dt.Rows[i]["RESCHEDLOAN"].ToString();
                    string GRACEPERIOD = dt.Rows[i]["GRACEPERIOD"].ToString();

                    if (GRACEPERIOD != "0")
                    {
                        GRACEPERIOD = "<amk1:GRACEPERIOD>" + GRACEPERIOD + "M</amk1:GRACEPERIOD>";
                    }
                    else {
                        GRACEPERIOD = "";
                    }

                    string UPFRONTFEE = dt.Rows[i]["UPFRONTFEE"].ToString();
                    string INCOMEFREQ = dt.Rows[i]["INCOMEFREQ"].ToString();
                    string BSNSEXPERIENCE = dt.Rows[i]["BSNSEXPERIENCE"].ToString();
                    string BSNSAGE = dt.Rows[i]["BSNSAGE"].ToString();
                    string MONTHLYTXNFEE = dt.Rows[i]["MONTHLYTXNFEE"].ToString();
                    string NOBSNSACT = dt.Rows[i]["NOBSNSACT"].ToString();
                    string COBRWCUSTOMER = dt.Rows[i]["COBRWCUSTOMER"].ToString();
                    string REFINANCELOAN = dt.Rows[i]["REFINANCELOAN"].ToString();
                    string SOURCEOFFUND = dt.Rows[i]["SOURCEOFFUND"].ToString().ToUpper();
                    #endregion params
                    #region xml
                    #region collateral
                    string COLLATERALCODE = "", COLLATERALCCY = "", COLLATERALTYPE = "", COLLDOCTYPE = "", COLLATERALQNTY = "", COLLATERALUNIT = "", COLLATERALPRICE = "";
                    string collStr = "";
                    try
                    {
                        DataTable dtColl = new DataTable();
                        dtColl = ReturnDT("exec T24_LoanAppListForOpenToCBS3_Collateral @LoanAppID='" + LoanAppID + "'");
                        DataView view = new DataView(dtColl);
                        DataTable distinct_dtColl = view.ToTable(true, "COLLATERALCODE");
                        for (int z = 0; z < distinct_dtColl.Rows.Count; z++)
                        {
                            COLLATERALCODE = distinct_dtColl.Rows[z]["COLLATERALCODE"].ToString();
                            DataRow[] result = dtColl.Select("COLLATERALCODE = '" + COLLATERALCODE + "'");
                            #region head
                            collStr = collStr + "<amk1:mCOLLATERALCODE m=\"1\"><!--Optional:-->"
                            + "<amk1:COLLATERALCODE>" + COLLATERALCODE + "</amk1:COLLATERALCODE><!--Optional:-->"
                            //+ "<amk1:COLLATERALCCY>" + COLLATERALCCY + "</amk1:COLLATERALCCY><!--Optional:-->"
                            + "<amk1:sgCOLLATERALTYPE sg=\"1\"><!--Zero or more repetitions:-->";
                            #endregion
                            string collTypeList = "";
                            foreach (DataRow row in result)
                            {
                                //Console.WriteLine("{0}, {1}", row[0], row[1]);
                                COLLATERALCCY = row["COLLATERALCCY"].ToString();
                                COLLATERALTYPE = row["COLLATERALTYPE"].ToString();
                                COLLDOCTYPE = row["COLLDOCTYPE"].ToString();
                                COLLATERALQNTY = row["COLLATERALQNTY"].ToString();
                                COLLATERALUNIT = row["COLLATERALUNIT"].ToString();
                                COLLATERALPRICE = row["COLLATERALPRICE"].ToString();

                                collTypeList = collTypeList + "<amk1:COLLATERALTYPE s=\"1\"><!--Optional:-->"
                                + "<amk1:COLLATERALTYPE>" + COLLATERALTYPE + "</amk1:COLLATERALTYPE><!--Optional:-->"
                                + "<amk1:COLLDOCTYPE>" + COLLDOCTYPE + "</amk1:COLLDOCTYPE><!--Optional:-->"
                                + "<amk1:COLLATERALQNTY>" + COLLATERALQNTY + "</amk1:COLLATERALQNTY><!--Optional:-->"
                                + "<amk1:COLLATERALUNIT>" + COLLATERALUNIT + "</amk1:COLLATERALUNIT><!--Optional:-->"
                                + "<amk1:COLLATERALPRICE>" + COLLATERALPRICE + "</amk1:COLLATERALPRICE>"
                                + "</amk1:COLLATERALTYPE>";
                            }
                            #region foot
                            collStr = collStr + collTypeList;
                            collStr = collStr + "</amk1:sgCOLLATERALTYPE>"
                            + "</amk1:mCOLLATERALCODE>";
                            #endregion
                        }
                        #region old code
                        //for (int j = 0; j < dtColl.Rows.Count; j++)
                        //{
                        //    COLLATERALCODE = dtColl.Rows[j]["COLLATERALCODE"].ToString();
                        //    COLLATERALCCY = dtColl.Rows[j]["COLLATERALCCY"].ToString();
                        //    COLLATERALTYPE = dtColl.Rows[j]["COLLATERALTYPE"].ToString();
                        //    COLLDOCTYPE = dtColl.Rows[j]["COLLDOCTYPE"].ToString();
                        //    COLLATERALQNTY = dtColl.Rows[j]["COLLATERALQNTY"].ToString();
                        //    COLLATERALUNIT = dtColl.Rows[j]["COLLATERALUNIT"].ToString();
                        //    COLLATERALPRICE = dtColl.Rows[j]["COLLATERALPRICE"].ToString();
                        //    collStr = collStr
                        //    + "<amk1:mCOLLATERALCODE m=\"1\"><!--Optional:-->"
                        //    + "<amk1:COLLATERALCODE>" + COLLATERALCODE + "</amk1:COLLATERALCODE><!--Optional:-->"
                        //    //+ "<amk1:COLLATERALCCY>" + COLLATERALCCY + "</amk1:COLLATERALCCY><!--Optional:-->"
                        //    + "<amk1:sgCOLLATERALTYPE sg=\"1\"><!--Zero or more repetitions:-->"

                        //    + "<amk1:COLLATERALTYPE s=\"1\"><!--Optional:-->"
                        //    + "<amk1:COLLATERALTYPE>" + COLLATERALTYPE + "</amk1:COLLATERALTYPE><!--Optional:-->"
                        //    + "<amk1:COLLDOCTYPE>" + COLLDOCTYPE + "</amk1:COLLDOCTYPE><!--Optional:-->"
                        //    + "<amk1:COLLATERALQNTY>" + COLLATERALQNTY + "</amk1:COLLATERALQNTY><!--Optional:-->"
                        //    + "<amk1:COLLATERALUNIT>" + COLLATERALUNIT + "</amk1:COLLATERALUNIT><!--Optional:-->"
                        //    + "<amk1:COLLATERALPRICE>" + COLLATERALPRICE + "</amk1:COLLATERALPRICE>"
                        //    + "</amk1:COLLATERALTYPE>"

                        //    +"</amk1:sgCOLLATERALTYPE>"
                        //    + "</amk1:mCOLLATERALCODE>";
                        //}
                        #endregion
                    }
                    catch { }
                    #endregion collateral
                    #region lp
                    string LOANPURPOSE = "";
                    if (lp1.Trim() != "")
                    {
                        LOANPURPOSE = "<amk1:LOANPURPOSE>" + lp1 + "</amk1:LOANPURPOSE>";
                    }
                    if (lp2.Trim() != "")
                    {
                        LOANPURPOSE = LOANPURPOSE + "<amk1:LOANPURPOSE>" + lp2 + "</amk1:LOANPURPOSE>";
                    }
                    if (lp3.Trim() != "")
                    {
                        LOANPURPOSE = LOANPURPOSE + "<amk1:LOANPURPOSE>" + lp3 + "</amk1:LOANPURPOSE>";
                    }
                    #endregion lp
                    #region SemiBallon
                    string PRREPAYFREQStr = "";
                    if (SEMIBALLONFREQ != "")
                    {
                        if (SEMIBALLONFREQ != "0")
                        {
                            PRREPAYFREQStr = "<amk1:PRREPAYFREQ>" + SEMIBALLONFREQ + "</amk1:PRREPAYFREQ>";
                        }
                    }
                    #endregion
                   
                    #region xml new
                    string xmlStr = "<?xml version=\"1.0\"?><soapenv:Envelope xmlns:amk1=\"http://temenos.com/AMKDLAPPLICATIONBOINP\" "
                    + "xmlns:amk=\"http://temenos.com/AMKCREATELOAN\" xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\"><soapenv:Header/>"
                    + "<soapenv:Body><amk:CREATELOAN><!--Optional:--><WebRequestCommon><!--Optional:-->"
                    + "<company>" + CreCompany + "</company><password>" + CrePassword + "</password><userName>" + CreUserName + "</userName></WebRequestCommon><!--Optional:-->"
                    + "<OfsFunction>"
                    + "<messageId>" + "L-" + fileHeader + "</messageId>"
                    + "</OfsFunction>"
                    + "<!--Optional:--><AMKDLAPPLICATIONBOINPType id=\"\"><!--Optional:-->"
                    + "<amk1:APPDATE>" + APPDATE + "</amk1:APPDATE><!--Optional:-->"
                    + "<amk1:CUSTOMERID>" + CUSTOMERID + "</amk1:CUSTOMERID><!--Optional:-->"
                    + "<amk1:PRODUCTID>" + PRODUCTID + "</amk1:PRODUCTID><!--Optional:-->"
                    + "<amk1:CURRENCY>" + CURRENCY + "</amk1:CURRENCY><!--Optional:-->"
                    + "<amk1:LOANTERM>" + LOANTERM + "</amk1:LOANTERM><!--Optional:-->"
                    + "<amk1:AMOUNT>" + AMOUNT + "</amk1:AMOUNT>"
                    + "<amk1:INTRATE>" + INTRATE + "</amk1:INTRATE><!--Optional:-->"
                    + "<amk1:REPAYSTDATE>" + REPAYSTDATE + "</amk1:REPAYSTDATE><!--Optional:-->"
                    + "<amk1:LOANCYCLE>" + LOANCYCLE + "</amk1:LOANCYCLE><!--Optional:-->"
                    + "<amk1:GROUPNO>" + GROUPNO + "</amk1:GROUPNO><!--Optional:-->"
                    + "<amk1:OPERATION>" + OPERATION + "</amk1:OPERATION><!--Optional:-->"
                    + "<amk1:LOANREFERBY>" + LOANREFERBY + "</amk1:LOANREFERBY><!--Optional:-->"
                    + "<amk1:CREDITOFFICER>" + CREDITOFFICER + "</amk1:CREDITOFFICER><!--Optional:-->"
                    + "<amk1:gMAINBUSINESS g=\"1\"><amk1:MAINBUSINESS>" + MAINBUSINESS + "</amk1:MAINBUSINESS></amk1:gMAINBUSINESS>"
                    + "<amk1:gLOANPURPOSE g=\"1\">" + LOANPURPOSE + "</amk1:gLOANPURPOSE>"
                    + "<amk1:CBCREQUIRED>" + CBCREQUIRED + "</amk1:CBCREQUIRED><!--Optional:-->"
                    + "<amk1:gGURANTORCODE g=\"1\"><!--Zero or more repetitions:--><amk1:GURANTORCODE>" + GURANTORCODE + "</amk1:GURANTORCODE></amk1:gGURANTORCODE><!--Optional:-->"
                    + "<amk1:gCOBRWCUSTOMER g=\"1\"><!--Zero or more repetitions:--><amk1:COBRWCUSTOMER>" + COBRWCUSTOMER + "</amk1:COBRWCUSTOMER></amk1:gCOBRWCUSTOMER><!--Optional:-->"

                    + "<amk1:gCOLLATERALCODE g=\"1\"><!--Zero or more repetitions:-->"

                    + collStr

                    + "</amk1:gCOLLATERALCODE><!--Optional:-->"

                    + "<amk1:INCEXPCCY>" + INCEXPCCY + "</amk1:INCEXPCCY><!--Optional:-->"
                    + "<amk1:TOTALINCOME>" + TOTALINCOME + "</amk1:TOTALINCOME><!--Optional:-->"
                    + "<amk1:TOTALEXPENSE>" + TOTALEXPENSE + "</amk1:TOTALEXPENSE><!--Optional:-->"
                    + "<amk1:AVGINCOME>" + AVGINCOME + "</amk1:AVGINCOME>"
                    + "<amk1:RESCHEDLOAN>" + RESCHEDLOAN + "</amk1:RESCHEDLOAN>"
                    + GRACEPERIOD
                    + PRREPAYFREQStr
                    + "<amk1:UPFRONTFEE>" + UPFRONTFEE + "</amk1:UPFRONTFEE><!--Optional:-->"
                    + "<amk1:INCOMEFREQ>" + INCOMEFREQ + "</amk1:INCOMEFREQ><!--Optional:-->"
                    + "<amk1:BSNSEXPERIENCE>" + BSNSEXPERIENCE + "</amk1:BSNSEXPERIENCE><!--Optional:-->"
                    + "<amk1:BSNSAGE>" + BSNSAGE + "</amk1:BSNSAGE><!--Optional:-->"
                    + "<amk1:MONTHLYTXNFEE>" + MONTHLYTXNFEE + "</amk1:MONTHLYTXNFEE>"
                    + "<amk1:NOBSNSACT>" + NOBSNSACT + "</amk1:NOBSNSACT>"

                    + "<amk1:SOURCEOFFUND>" + SOURCEOFFUND + "</amk1:SOURCEOFFUND>"
                    + "<amk1:AMKREFINANCELOAN>" + REFINANCELOAN + "</amk1:AMKREFINANCELOAN>"
                    + "</AMKDLAPPLICATIONBOINPType></amk:CREATELOAN></soapenv:Body></soapenv:Envelope>";
                    #endregion xml new
                    #endregion xml
                    #region Call To T24
                    T24_AddLog(fileHeader, "AddLoan_RQ", xmlStr, "LoanAdd");
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(CreUrl);
                    request.ContentType = "text/xml"; // or application/soap+xml for SOAP 1.2
                    request.Method = "POST";
                    request.KeepAlive = false;

                    byte[] byteArray = Encoding.UTF8.GetBytes(xmlStr);
                    request.ContentLength = byteArray.Length;

                    Stream requestStream = request.GetRequestStream();
                    requestStream.Write(byteArray, 0, byteArray.Length);
                    requestStream.Close();

                    HttpWebResponse response = null;
                    try
                    {
                        response = (HttpWebResponse)request.GetResponse();
                    }
                    catch (WebException ex)
                    {
                        response = (HttpWebResponse)ex.Response;
                    }

                    Stream receiveStream = response.GetResponseStream();
                    StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(readStream.ReadToEnd());
                    T24_AddLog(fileHeader, "AddLoan_RS: " + (int)response.StatusCode, doc.InnerXml, "LoanAdd");
                    #endregion
                    #region read rs
                    string successIndicator = doc.GetElementsByTagName("successIndicator").Item(0).InnerText;
                    string transactionId = doc.GetElementsByTagName("transactionId").Item(0).InnerText;
                    if (successIndicator == "Success")
                    {
                        CBSKey = transactionId;
                        CBSAcc = transactionId;
                        Remark = CBSKey;
                        LoanAppStatusID = "7";
                        ERR = "Success";
                        isError = "0";
                    }
                    else
                    {
                        //add error log
                        //Remark = xmlContent;
                        LoanAppStatusID = "12";
                        ERR = "Error";
                        isError = "1";

                        #region Error
                        xmlContent = doc.InnerXml;
                        if (xmlContent.Contains("<Status>"))
                        {
                            int pFrom = xmlContent.IndexOf("<Status>");// + "<Status>".Length;
                            int pTo = xmlContent.IndexOf("</Status>") + "</Status>".Length;
                            string MSGxmlContent = xmlContent.Substring(pFrom, pTo - pFrom).Trim();
                            LoanAppApproveToCBSError.Status obj = GenerateXmlObject<LoanAppApproveToCBSError.Status>(MSGxmlContent);
                            string strMsg = "";
                            for (int iMsg = 0; iMsg < obj.Messages.Count; iMsg++)
                            {
                                if (iMsg == 0)
                                {
                                    strMsg = obj.Messages[iMsg];
                                }
                                else
                                {
                                    strMsg = strMsg + " | " + obj.Messages[iMsg];
                                }
                            }
                            SMS = strMsg;
                            Remark = SMS;
                        }
                        #endregion Error

                    }
                    #endregion call to T24
                    #region log
                    T24_LoanAppAddLogAdd(LoanAppID, "0", fileHeader, isError, doc.InnerXml, Remark, "0");
                    #endregion log
                }

            }
            catch (Exception ex)
            {
                //add error log
                Remark = ex.Message.ToString();
                LoanAppStatusID = "12";
                ERR = "Error";
            }
            #region add log
            UpdateLoanAppStatus(LoanAppID, CBSKey, CBSAcc, "Error AddLoan: " + Remark, LoanAppStatusID);
            #endregion add log
            rs[0] = ERR;
            rs[1] = SMS;
            return rs;
        }

        #endregion

        public bool GetApprovedCondiction(string loanAppID)
        {

            bool rv = false;

            DataTable oDT = new DataTable();
            oDT = ReturnDT2("exec loanAppApproveWithCondiction '" + loanAppID + "'");
            int GroupMemberAMT = 0, condiction = 0;

            try
            {
                GroupMemberAMT = Convert.ToInt32(oDT.Rows[0]["GroupMemberAMT"]);
                condiction = Convert.ToInt32(oDT.Rows[0]["approvedCondiction"]);
            }
            catch { }

            //if (GroupMemberAMT < 6 && condiction == 1)
            if (GroupMemberAMT <= 6)
            {
                rv = true;
            }
            else
            {
                rv = false;
            }

            return rv;

        }

        public string[] AddCustKYCFromDevice(string LoanAppPersonID, string LoanAppID, PersonKYC kYC)
        {

            string[] rs = new string[4];
            string rs0 = "Succeed"
                , rs1 = ""//SMS
                , rs2 = ""//KeyID
                , rs3 = "";//ExSMS
            try
            {
                SqlConnection Con1 = new SqlConnection(ConStr());
                Con1.Open();
                SqlCommand Com1 = new SqlCommand();
                Com1.Connection = Con1;
                Com1.Parameters.Clear();
                string sql = "sp_LoanAppPersonKYCAdd";
                Com1.CommandText = sql;
                Com1.CommandType = CommandType.StoredProcedure;
                Com1.Parameters.AddWithValue("@LoanAppPersonID", LoanAppPersonID);
                Com1.Parameters.AddWithValue("@LoanAppID", LoanAppID);
                Com1.Parameters.AddWithValue("@KYCClientID", kYC.KYCClientID);
                Com1.Parameters.AddWithValue("@IDCardType", kYC.IDCardType);
                Com1.Parameters.AddWithValue("@IDNumber", kYC.IDNumber);
                Com1.Parameters.AddWithValue("@IDIssueDate", kYC.IDIssueDate);
                Com1.Parameters.AddWithValue("@IDExpireDate", kYC.IDExpireDate);
                Com1.Parameters.AddWithValue("@isNew", kYC.IsNew);
                Com1.Parameters.AddWithValue("@IsCurrentKYC", kYC.IsCurrentKYC);
                DataTable dt = new DataTable();
                dt.Load(Com1.ExecuteReader());
                rs2 = dt.Rows[0][0].ToString();
                Con1.Close();
            }
            catch (Exception ex)
            {
                rs0 = "Error";
                rs1 = "";//SMS
                rs2 = "";//KeyID
                rs3 = ex.Message.ToString();//ExSMS
            }
            rs[0] = rs0;
            rs[1] = rs1;
            rs[2] = rs2;
            rs[3] = rs3;
            return rs;
        }
        public string[] AddCustKYCImgFromDevice(string KYCServerID,string LoanAppPersonID, string LoanAppID, string KYCImageClientID, string Ext, string ImgPath,string Remark)
        {
            string[] rs = new string[4];
            string rs0 = "Succeed"
                , rs1 = ""//SMS
                , rs2 = ""//KeyID
                , rs3 = "";//ExSMS
            try
            {
                SqlConnection Con1 = new SqlConnection(ConStr());
                Con1.Open();
                SqlCommand Com1 = new SqlCommand();
                Com1.Connection = Con1;
                Com1.Parameters.Clear();
                string sql = "sp_LoanAppPersonKYCImgAdd";
                Com1.CommandText = sql;
                Com1.CommandType = CommandType.StoredProcedure;
                Com1.Parameters.AddWithValue("@KYCServerID", KYCServerID);
                Com1.Parameters.AddWithValue("@LoanAppPersonID", LoanAppPersonID);
                Com1.Parameters.AddWithValue("@LoanAppID", LoanAppID);
                Com1.Parameters.AddWithValue("@KYCImageClientID", KYCImageClientID);
                Com1.Parameters.AddWithValue("@Ext", Ext);
                Com1.Parameters.AddWithValue("@ImgPath", ImgPath);
                Com1.Parameters.AddWithValue("@Remark", (Remark==null) ? "" : Remark);
                DataTable dt = new DataTable();
                dt.Load(Com1.ExecuteReader());
                rs2 = dt.Rows[0][0].ToString();
                Con1.Close();
            }
            catch (Exception ex)
            {
                rs0 = "Error";
                rs1 = "";//SMS
                rs2 = "";//KeyID
                rs3 = ex.Message.ToString();//ExSMS
            }
            rs[0] = rs0;
            rs[1] = rs1;
            rs[2] = rs2;
            rs[3] = rs3;
            return rs;
        }
        public string[] T24_LoanAppListForOpenToCBS2_UpdateCID(string LoanAppPersonID,string CustomerID, string LoanAppID, string KYCServerIDList)
        {
            string[] rs = new string[4];
            string rs0 = "Succeed"
                , rs1 = ""//SMS
                , rs2 = ""//KeyID
                , rs3 = "";//ExSMS
            try
            {
                SqlConnection Con1 = new SqlConnection(ConStr());
                Con1.Open();
                SqlCommand Com1 = new SqlCommand();
                Com1.Connection = Con1;
                Com1.Parameters.Clear();
                string sql = "T24_LoanAppListForOpenToCBS2_UpdateCID";
                Com1.CommandText = sql;
                Com1.CommandType = CommandType.StoredProcedure;
                Com1.Parameters.AddWithValue("@LoanAppPersonID", LoanAppPersonID);
                Com1.Parameters.AddWithValue("@CustomerID", CustomerID);
                Com1.Parameters.AddWithValue("@LoanAppID", LoanAppID);
                Com1.Parameters.AddWithValue("@KYCServerIDList", KYCServerIDList);
                //DataTable dt = new DataTable();
                //dt.Load(Com1.ExecuteReader());
                //rs2 = dt.Rows[0][0].ToString();
                Com1.ExecuteNonQuery();
                Con1.Close();
            }
            catch (Exception ex)
            {
                rs0 = "Error";
                rs1 = "Error during update customer CID: T24_LoanAppListForOpenToCBS2_UpdateCID";//SMS
                rs2 = "";//KeyID
                rs3 = ex.Message.ToString();//ExSMS
            }
            rs[0] = rs0;
            rs[1] = rs1;
            rs[2] = rs2;
            rs[3] = rs3;
            return rs;
        }



        #region New Function
        String FakeSeed = "AMKTablet*&*2014";

        public String GetHostConStr()
        {
            string HostConStr = ConfigurationManager.AppSettings["ConStr"];
            return HostConStr;
        }
        public String GetFakeSeed()
        {
            return FakeSeed;
        }

        public void Switch_AddLog(string filename, string func, string sms)
        {
            string filePath = "";
            DateTime dt = DateTime.Now;
            try
            {
                string todayDate = dt.ToString("yyyyMMdd");
                try
                {
                    string dir = AppDomain.CurrentDomain.BaseDirectory + "Log\\Log\\" + todayDate;
                    if (Directory.Exists(dir))
                    {

                    }
                    else
                    {
                        System.IO.Directory.CreateDirectory(dir);
                    }
                }
                catch { }


                filename = filename.Replace(" ", "_").Replace("-", "_").Replace(":", "_").Replace(".", "_");
                filePath = AppDomain.CurrentDomain.BaseDirectory + "Log\\Log\\" + todayDate + "\\" + filename + ".log";
                if (!File.Exists(filePath))
                {
                    FileStream fs = File.Create(filePath);
                    fs.Close();
                }
                StreamWriter sw = File.AppendText(filePath);
                if (!string.IsNullOrEmpty(sms))
                {
                    sw.WriteLine(dt.ToString("HH:mm:ss.fff") + " - " + func + " : " + sms);
                }
                else
                {
                    sw.WriteLine(dt.ToString("HH:mm:ss.fff") + " - " + func + " : " + sms);
                }

                sw.Flush();
                sw.Close();
            }
            catch (Exception ex)
            {
                try
                {
                    if (!File.Exists("Error_" + filePath))
                    {
                        FileStream fs = File.Create("Error_" + filePath);
                        fs.Close();
                    }
                    StreamWriter sw = File.AppendText("Error_" + filePath);
                    if (!string.IsNullOrEmpty(sms))
                    {
                        sw.WriteLine(dt.ToString("HH:mm:ss.fff") + " - " + func + " : " + sms);
                    }
                    else
                    {
                        sw.WriteLine(dt.ToString("HH:mm:ss.fff") + " - " + func + " : " + sms);
                    }

                    sw.Flush();
                    sw.Close();
                }
                catch { }
            }
        }

        public string[] T24_DeviceLogAdd(string LogTypeID, string UserName, string device_id, string app_vName, string mac_address)
        {
            string[] rs = new string[2];
            int err = 1;
            string sms = "ok";
            try
            {
                string sql = "exec T24_DeviceLogAdd @LogTypeID=@LogTypeID,@UserName=@UserName,@device_id=@device_id,@app_vName=@app_vName,@mac_address=@mac_address";
                SqlConnection Con1 = new SqlConnection(GetHostConStr());
                Con1.Open();
                SqlCommand Com1 = new SqlCommand();
                Com1.Connection = Con1;
                try
                {
                    Com1.Parameters.Clear();
                    Com1.CommandText = sql;
                    Com1.Parameters.AddWithValue("@LogTypeID", LogTypeID);
                    Com1.Parameters.AddWithValue("@UserName", UserName);
                    Com1.Parameters.AddWithValue("@device_id", device_id);
                    Com1.Parameters.AddWithValue("@app_vName", app_vName);
                    Com1.Parameters.AddWithValue("@mac_address", mac_address);
                    Com1.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    err = 0;
                    sms = ex.Message.ToString();
                }
                Con1.Close();
            }
            catch (Exception ex)
            {
                err = 0;
                sms = ex.Message.ToString();
            }
            rs[0] = err.ToString();
            rs[1] = sms;
            return rs;
        }

        #endregion


        #region Check Version

        public string[] CheckAppVersion(string app_verion) {

            string[] rs = new string[2];
            SqlConnection con = new SqlConnection(ConStr());
            SqlDataAdapter da = new SqlDataAdapter("check_app_version", con);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;

            try {
                string app_version_db = "";
                DataTable dt = new DataTable();
                da.Fill(dt);
                app_version_db = "" + dt.Rows[0]["app_version"];
                if (app_version_db.ToUpper() == app_verion.ToUpper())
                {
                    rs[0] = "Succeed";
                    rs[1] = "Succeed";
                }
                else {
                    rs[0] = "Error";
                    rs[1] = "The version is not matched, pls update the lastest version !";
                }

            }
            catch (Exception ex) {
                rs[0] = "Error";
                rs[1] = "Catch, The version is not matched, pls update the lastest version !";
            }

            return rs;

        }

        #endregion
    }
}