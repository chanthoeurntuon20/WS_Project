using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace WebService
{
    public class ApiSecurity
    {
        public static bool VaidateUser(string username, string password)
        {
            Class1 c = new Class1();
            c.T24_AddLog("VaidateUser", "VaidateUser", username+" | " + password, "VaidateUser");
            Boolean rs = false;
            if (username +"|"+ password == ConfigurationManager.AppSettings["Api"]) {
                rs = true;
            }
            return rs;
        }
    }
}