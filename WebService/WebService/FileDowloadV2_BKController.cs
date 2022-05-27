using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace WebService
{
    [BasicAuthentication]
    public class FileDowloadV2_BKController : ApiController
    {
        //public HttpResponseMessage Get(string api_name, string api_key, string json)
        //public HttpResponseMessage Post([FromUri]string p, [FromUri]string msgid, [FromBody]string json)//json={"criteriaValue":""}
        //{
        //    HttpResponseMessage response = null;
        //    Class1 c = new Class1();
        //    string ERR = "Succeed", SMS = "", ExSMS = "", ERRCode = "";
        //    string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        //    string ControllerName = "FileDowload";
        //    string FileNameForLog = msgid + "_" + ServerDate.Replace("-", "_").Replace(" ", "_").Replace(":", "_").Replace(".", "_");
        //    try
        //    {
        //        #region msgid
        //        if (ERR != "Error")
        //        {
        //            string[] str = c.CheckMsgID(msgid);
        //            ERR = str[0];
        //            SMS = str[1];
        //        }
        //        #endregion
        //        #region add log
        //        if (ERR != "Error")
        //        {
        //            c.T24_AddLog(FileNameForLog, "1.RQ", p, ControllerName);
        //        }
        //        #endregion
        //        #region p -> SessionID
        //        string UserID = "";
        //        if (ERR != "Error")
        //        {
        //            //p = System.Web.HttpUtility.UrlDecode(p);
        //            string[] rs = c.SessionIDCheck(ServerDate, p);
        //            ERR = rs[0];
        //            SMS = rs[1];
        //            ExSMS = rs[2];
        //            UserID = rs[3];
        //            ERRCode = rs[4];
        //        }
        //        #endregion
        //        #region check json
        //        if (ERR != "Error")
        //        {
        //            string[] str = c.CheckObjED(json, "2");
        //            ERR = str[0];
        //            SMS = str[1];
        //            ExSMS = str[2];
        //            json = str[3];
        //        }
        //        #endregion check json
        //        #region read json
        //        DefinedFieldGetV2RQModel jObj = null;
        //        string criteriaValue = "";
        //        if (ERR != "Error")
        //        {
        //            try
        //            {
        //                jObj = JsonConvert.DeserializeObject<DefinedFieldGetV2RQModel>(json);
        //                criteriaValue = jObj.criteriaValue;
        //            }
        //            catch (Exception ex)
        //            {
        //                ERR = "Error";
        //                ExSMS = ex.Message.ToString();
        //                //get sms
        //                string[] str = c.GetSMSByMsgID("13");
        //                ERR = str[0];
        //                if (ERR == "Error")
        //                {
        //                    SMS = str[1];
        //                    ExSMS = ExSMS + "|" + str[2];
        //                }
        //                else
        //                {
        //                    SMS = str[3];
        //                }
        //                ERR = "Error";
        //            }
        //        }
        //        #endregion
        //        #region check img
        //        if (ERR != "Error")
        //        {
        //            //string filePath = c.ImgPathGet() + criteriaValue;
        //            //if (!File.Exists(filePath))
        //            //{
        //            //    ERR = "Error";
        //            //    SMS = "File is not existed";
        //            //}
        //            //else
        //            //{
        //            //    string Ext = Path.GetExtension(filePath);
        //            //    response = new HttpResponseMessage(HttpStatusCode.OK);
        //            //    response.Content = new StreamContent(new FileStream(filePath, FileMode.Open, FileAccess.Read));
        //            //    response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
        //            //    response.Content.Headers.ContentDisposition.FileName = criteriaValue;
        //            //    response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/" + Ext);
        //            //}

        //            Object[] rs = NetworkShare.GetFile(FileNameForLog, ControllerName, response, criteriaValue);
        //            ERR = rs[0].ToString();
        //            SMS = rs[1].ToString();
        //            response = (HttpResponseMessage)rs[2];
        //        }
        //        #endregion check img
        //    }
        //    catch (Exception ex)
        //    {
        //        ERR = "Error";
        //        SMS = "Something was wrong";
        //        ExSMS = ex.Message.ToString();
        //    }
        //    #region return err
        //    if (ERR == "Error")
        //    {
        //        response = new HttpResponseMessage(HttpStatusCode.NoContent);
        //    }
        //    #endregion return err

        //    return response;
        //}

    }
    //public class FileDowloadV2RQ
    //{
    //    public string user { get; set; }
    //    public string pwd { get; set; }
    //    public string device_id { get; set; }
    //    public string app_vName { get; set; }
    //    public string mac_address { get; set; }
    //    public string criteriaValue { get; set; }
    //}

}