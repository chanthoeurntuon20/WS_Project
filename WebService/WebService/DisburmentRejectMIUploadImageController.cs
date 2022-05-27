using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using WebApiFileUpload.API.Infrastructure;
using WebService;

namespace WebApiFileUpload.API.Controllers
{
    [BasicAuthentication]
    public class DisburmentRejectMIUploadImageController : ApiController
    {
        [MimeMultipart]
        [HttpPost]

        public async Task<IEnumerable<DisburmentRejectMIUploadImageRS>> Post([FromUri]string api_name, string api_key, string json)
        {
            #region Header
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "", SavedFName = "";
            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            List<DisburmentRejectMIUploadImageRS> RSData = new List<DisburmentRejectMIUploadImageRS>();
            string FileNameForLog = "DisburmentRejectMIUploadImage_" + api_name + "_" + ServerDate.Replace("-", "_").Replace(" ", "_").Replace(":", "_");
            string ControllerName = "DisburmentRejectMIUploadImage";
            string ExSMS = "";
            #endregion
            try
            {
                //Add log
                c.T24_AddLog(FileNameForLog, "RQ", json, ControllerName);

                #region check json
                if (json == null || json == "")
                {
                    ERR = "Error";
                    SMS = "Invalid JSON: it is empty";
                }
                #endregion check json

                DisburmentRejectMIUploadImageRQ jObj = null;
                string user = "", pwd = "", device_id = "", app_vName = "", mac_address = "", criteriaValue = "", UserID = "";
                #region json
                if (ERR != "Error")
                {
                    try
                    {
                        jObj = JsonConvert.DeserializeObject<DisburmentRejectMIUploadImageRQ>(json);
                        FileNameForLog = user + "_" + FileNameForLog;
                        user = jObj.user;
                        user = c.Encrypt(user, c.SeekKeyGet());
                        pwd = jObj.pwd;
                        pwd = c.Encrypt(pwd, c.SeekKeyGet());
                        device_id = jObj.device_id;
                        app_vName = jObj.app_vName;
                        mac_address = jObj.mac_address;
                        criteriaValue = jObj.criteriaValue;
                    }
                    catch (Exception ex)
                    {
                        ERR = "Error";
                        SMS = "Invalid JSON: " + ex.Message.ToString();
                        ExSMS = ex.Message.ToString();
                    }
                }
                #endregion json
                #region Get User Info.
                if (ERR != "Error")
                {
                    try
                    {
                        SqlConnection Con1 = new SqlConnection(c.ConStr());
                        Con1.Open();
                        SqlCommand Com1 = new SqlCommand();
                        Com1.Connection = Con1;
                        string sql = "exec T24_GetUserInfo_V2 @user=@user,@pwd=@pwd";
                        Com1.CommandText = sql;
                        Com1.Parameters.Clear();
                        Com1.Parameters.AddWithValue("@user", user);
                        Com1.Parameters.AddWithValue("@pwd", pwd);
                        DataTable dt1 = new DataTable();
                        dt1.Load(Com1.ExecuteReader());
                        UserID = dt1.Rows[0]["UserID"].ToString();
                        Con1.Close();
                    }
                    catch (Exception ex)
                    {
                        ERR = "Error";
                        SMS = "invalid user: " + ex.Message.ToString();
                        ExSMS = ex.Message.ToString();
                    }
                }
                #endregion Get User Info.

                #region save file
                if (ERR != "Error")
                {
                    #region save file
                    var uploadPath = HttpContext.Current.Server.MapPath("~/MIImages");
                    var multipartFormDataStreamProvider = new UploadMultipartFormProvider(uploadPath);

                    // Read the MIME multipart asynchronously 
                    UploadMultipartFormProvider content = await Request.Content.ReadAsMultipartAsync(multipartFormDataStreamProvider);
                    //count content
                    int iContent = content.Contents.Count;
                    if (iContent != 1)
                    {
                        ERR = "Error";
                        SMS = "Only one image is allowed to upload at once";
                    }
                    #endregion
                    if (ERR != "Error")
                    {
                        string SavedFNamePath = multipartFormDataStreamProvider
                            .FileData.Select(multiPartData => multiPartData.LocalFileName).FirstOrDefault();
                        SavedFName = Path.GetFileName(SavedFNamePath);

                        c.ReturnDT("exec V2_mi_images_by_MIImageID '" + criteriaValue + "'");
                    }
                }
                #endregion save file

            }
            catch (Exception ex)
            {
                ERR = "Error";
                SMS = "Something was wrong";
                ExSMS = ex.Message.ToString();
            }

            DisburmentRejectMIUploadImageRS ListHeader = new DisburmentRejectMIUploadImageRS();
            ListHeader.ERR = ERR;
            ListHeader.SMS = SMS;
            ListHeader.FName = SavedFName;
            RSData.Add(ListHeader);

            try
            {
                var jsonRS = new JavaScriptSerializer().Serialize(RSData);
                c.T24_AddLog(FileNameForLog, "RS", jsonRS.ToString(), ControllerName);
                if (ExSMS != "")
                {
                    c.T24_AddLog(FileNameForLog, "RS", ExSMS, ControllerName + "_Error");
                }
            }
            catch { }

            return RSData;
        }
    }
}

public class DisburmentRejectMIUploadImageRQ
{
    public string user { get; set; }
    public string pwd { get; set; }
    public string device_id { get; set; }
    public string app_vName { get; set; }
    public string mac_address { get; set; }
    public string criteriaValue { get; set; }
}
public class DisburmentRejectMIUploadImageRS
{
    public string ERR { get; set; }
    public string SMS { get; set; }
    public string FName { get; set; }
}