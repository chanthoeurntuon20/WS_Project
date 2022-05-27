using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using WebService.Helpers;

namespace WebService.Extensions
{
    public static class CheckValidation
    {
        public static bool hasSpecialChar(string input)
        {
            string specialChar = @"\|!#$%&/()=?»«@£§€{}.-;'<>_,";
            foreach (var item in specialChar)
            {
                if (input.Contains(item)) return true;
            }

            return false;
        }
        public static string[] CheckDateFormat(string s, string lb)
        {
            string[] rs = new string[2];
            string rs0 = "Error", rs1 = "";
            try
            {
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
        public static string[] CheckDateTimeFormat(string s, string lb)
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
        public static string[] CheckIsMoneyFormat(string s, string lb)
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
                else
                {
                    rs0 = "Error";
                    rs1 = "Invalid Money of " + lb;
                }
            }
            catch { }
            rs[0] = rs0;
            rs[1] = rs1;
            return rs;
        }
        public static string[] CheckIsIntFormat(string s, string lb)
        {
            string[] rs = new string[2];
            string rs0 = "Error", rs1 = "";
            try
            {
                int value;
                if (int.TryParse(s, out value))
                {
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
       
        public static string[] CheckImage(string ImgName, string Ext, string lb)
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
                else
                {
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
        
        public static string[] CheckUrlParam(string p)
        {
            string ERR = "Succeed", SMS = "", ExSMS = "";
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
                        string pStr = Cryptography.Decrypt(p, Cryptography.SeekKeyGet());
                        string[] pSplit = pStr.Split(new string[] { "||" }, StringSplitOptions.None);
                        api_name = pSplit[0];
                        api_key = Cryptography.Encrypt(pSplit[1], Cryptography.SeekKeyGet());
                    }
                    catch (Exception ex)
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
        
        public static string[] CheckJson(string json)
        {
            string ERR = "Succeed", SMS = "", criteriaValue = "0", GroupID = "", criteriaValue2 = "", ExSMS = "";
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
        public static string[] CheckJsonForCreateAccount(string json)
        {
            string ERR = "Succeed", SMS = "", ExSMS = "", criteriaValue = "0", GroupID = "", CreCompany = "", CustomerID = ""
                , CATEGORY = "", ACCOUNTTITLE1 = "", SHORTTITLE = "", LoanCurrency = "";
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
        public static string ConvertDateFromYYYYMMDD(string str)
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
    }
}