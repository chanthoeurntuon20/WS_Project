using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using WebService.App_Start;

namespace WebService
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                        | SecurityProtocolType.Tls11
                        | SecurityProtocolType.Tls12
                        | SecurityProtocolType.Ssl3;

            PreSendRequestHeaders += new EventHandler(Application_PreSendRequestHeaders);

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            //RouteTable.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //    );

            GlobalConfiguration.Configuration.Formatters.JsonFormatter.MediaTypeMappings.Add(
            new QueryStringMapping("type", "json", new MediaTypeHeaderValue("application/json")));

            GlobalConfiguration.Configuration.Formatters.XmlFormatter.MediaTypeMappings.Add(
            new QueryStringMapping("type", "xml", new MediaTypeHeaderValue("application/xml")));

            #region imgPath  
            Class1 c = new Class1();
            try
            {
                DataTable dt = c.ReturnDT("select * from tblSetting where SettingID=5");
                string SettingValue = dt.Rows[0]["SettingValue"].ToString();
                string[] sVal = SettingValue.Split('|');
                sVal[2] = c.Decrypt(sVal[2], c.SeekKeyGet());
                c.ImgPathSet(sVal);
                c.T24_AddLog("Global", "GetSetting", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + ": " + SettingValue, "Application_Start");
            }
            catch (Exception ex)
            {
                c.T24_AddLog("Global", "GetSetting", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + ": " + ex.Message.ToString(), "Application_Start");
            }

            #endregion

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
       | SecurityProtocolType.Tls11
       | SecurityProtocolType.Tls12
       | SecurityProtocolType.Ssl3;

            //string[] allowedOrigin = new string[] { "*" };
            //var origin = HttpContext.Current.Request.Headers["Origin"];
            //if (origin != null && allowedOrigin.Contains(origin))
            //{
            //    HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", origin);
            //    HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "GET,POST");
            //}

            //HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");
            //HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "GET,POST");

            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");
            if (HttpContext.Current.Request.HttpMethod == "OPTIONS")
            {
                //HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE");
                //HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "*");
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "GET,POST");
                HttpContext.Current.Response.AddHeader("Access-Control-Max-Age", "1728000");
                HttpContext.Current.Response.End();
            }

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }

        void Application_PreSendRequestHeaders(object sender, EventArgs e)
        {
           // HttpContext.Current.Response.Headers.Remove("Access-Control-Allow-Origin");
        }


    }
}