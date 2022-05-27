using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace WebService.Helpers
{
    public class AppConfig
    {
        public string WebPathGet()
        {
            return ConfigurationManager.AppSettings["WebPath"];
        }

        public static string[] TabletLoanImage;//0=path,1=user,2=pwd
        public static void ImgPathSet(string[] str)
        {
            TabletLoanImage = str;
        }
        public static string[] ImgPathTabletLoanImageGet()
        {
            return TabletLoanImage;
        }
        public static string ConStr()
        {
            return ConfigurationManager.AppSettings["ConStr"];
        }
        public static String ConStr2()
        {
            string HostConStr = ConfigurationManager.AppSettings["ConStrSecond"];
            return HostConStr;
        }
        public static string ConStrInsightConn()
        {
            return ConfigurationManager.AppSettings["ConStrInsightConn"];
        }

        public static string ConStrInsight()
        {
            return ConfigurationManager.AppSettings["ConStrInsight"];
        }
        public static string GetTabletWSUrl()
        {
            string Str = ConfigurationManager.AppSettings["TabletWSUrl"];
            return Str;
        }
        public static string GetTabletWSAPIName()
        {
            string Str = ConfigurationManager.AppSettings["TabletWSAPIName"];
            return Str;
        }
        public static string GetTabletWSAPIKey()
        {
            string Str = ConfigurationManager.AppSettings["TabletWSAPIKey"];
            return Str;
        }
        public string GetTabletWSUser()
        {
            string Str = ConfigurationManager.AppSettings["TabletWSUser"];
            return Str;
        }
        public static string GetTabletWSPwd()
        {
            string Str = ConfigurationManager.AppSettings["TabletWSPwd"];
            return Str;
        }
        public static int GetTabletWSRestTimeout()
        {
            int Str = Convert.ToInt32(ConfigurationManager.AppSettings["RestTimeout"]);
            return Str;
        }
        #region Calcutor
        public static string GetCalculatorWSUrl()
        {
            string Str = ConfigurationManager.AppSettings["CalculatorWSUrl"];
            return Str;
        }
        public static string GetCalculatorWSAPIName()
        {
            string Str = ConfigurationManager.AppSettings["CalculatorWSAPIName"];
            return Str;
        }
        public static string GetCalculatorWSAPIKey()
        {
            string Str = ConfigurationManager.AppSettings["CalculatorWSAPIKey"];
            return Str;
        }
        public string GetCalculatorWSUser()
        {
            string Str = ConfigurationManager.AppSettings["CalculatorWSUser"];
            return Str;
        }
        public static string GetCalculatorWSPwd()
        {
            string Str = ConfigurationManager.AppSettings["CalculatorWSPwd"];
            return Str;
        }
        #endregion
        #region New Function
        static String FakeSeed = "AMKTablet*&*2014";

        public static String GetHostConStr()
        {
            string HostConStr = ConfigurationManager.AppSettings["ConStr"];
            return HostConStr;
        }
        public static String GetFakeSeed()
        {
            return FakeSeed;
        }

        public static void Switch_AddLog(string filename, string func, string sms)
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
        #endregion
    }

}