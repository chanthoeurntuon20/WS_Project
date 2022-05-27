using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApiFileUpload.API.Infrastructure;
using WebService;

namespace WebApiFileUpload.API.Controllers
{
    
    //[BasicAuthentication]
    public class FileUploadController : ApiController
    {
        [MimeMultipart]
        [HttpPost]
        //public async Task<FileUploadResult> Post(int? par1)
        public async Task<IEnumerable<FileUploadResModel>> Post([FromUri]string api_name, string api_key,string json)//json=[{"user":"01804","pwd":"040882","device_id":"352405061542333","app_vName":"1.6"}]
        {
            //string ERR="", SMS="";

            //try {
            //    #region check json
            //    if (json == null || json == "")
            //    {
            //        ERR = "Error";
            //        SMS = "Invalid JSON";
            //    }
            //    #endregion check json

            //    #region check json to get user/pwd
            //    string user = "", pwd = "";
            //    if (ERR != "Error")
            //    {
            //        try
            //        {
            //            var objects = JArray.Parse(json);
            //            foreach (JObject root in objects)
            //            {
            //                foreach (var item in root)
            //                {
            //                    string key = item.Key;
            //                    if (key == "user")
            //                    {
            //                        user = item.Value.ToString();
            //                    }
            //                    if (key == "pwd")
            //                    {
            //                        pwd = item.Value.ToString();
            //                    }                                
            //                }
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            ERR = "Error";
            //            SMS = "Invalid JSON";
            //        }
            //    }
            //    #endregion check json to get user/pwd

            //    #region save file
            //    if (ERR != "Error") {
            //        var uploadPath = HttpContext.Current.Server.MapPath("~/Uploads");
            //        var multipartFormDataStreamProvider = new UploadMultipartFormProvider(uploadPath);

            //        // Read the MIME multipart asynchronously 
            //        UploadMultipartFormProvider content = await Request.Content.ReadAsMultipartAsync(multipartFormDataStreamProvider);
            //        //count content
            //        int iContent = content.Contents.Count;
            //        if (iContent != 1) {
            //            ERR = "Error";
            //            SMS = "Only one image is allowed to upload at once";
            //        }

            //        if (ERR != "Error") {                    
            //            string SavedFNamePath = multipartFormDataStreamProvider
            //                .FileData.Select(multiPartData => multiPartData.LocalFileName).FirstOrDefault();
            //            string SavedFName = Path.GetFileName(SavedFNamePath);
            //            //'CollImg_'+@ServerLoanAppID+'_'+@ServerLoanAppPersonID+'_'+@ServerLoanAppClientCollateralID+'_'+ @datetime+@Ext
            //            //'PersonImg_'+@ServerLoanAppID+'_'+@ServerLoanAppPersonID+'_'+@OneCardTwoDoc+'_'+ @datetime+@Ext
            //            #region check fname
            //            try
            //            {
            //                string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            //                ServerDate = ServerDate.Replace(" ", ".").Replace("-",".").Replace(":", ".");

            //                string[] fname = SavedFName.Split('_');
            //                string ImgType = fname[0];
            //                Class1 c = new Class1();

            //                #region CollImg
            //                if (ImgType == "CollImg")
            //                {
            //                    string ServerLoanAppID = fname[1];
            //                    string ServerLoanAppPersonID = fname[2];
            //                    string ServerLoanAppClientCollateralID = fname[3];
            //                    string sql = "select LoanAppClientCollateralID from tblLoanApp16ClientCollateralImage where LoanAppClientCollateralID='" + ServerLoanAppClientCollateralID + "' and ImgPath='" + SavedFName + "'";
            //                    DataTable dt = c.ReturnDT(sql);
            //                    if (dt.Rows.Count == 0)
            //                    {
            //                        ERR = "Error";
            //                        SMS = "Invalid Image Name: " + SavedFName;
            //                    }
            //                    else
            //                    {
            //                        try
            //                        {
            //                            //copy img to web path
            //                            string SaveToWebPath = c.ImgPathGet() + ServerDate + "_" + SavedFName;
            //                            File.Copy(SavedFNamePath, SaveToWebPath);
            //                            //update fname
            //                            DataTable dt2=c.ReturnDT("exec T24_tblLoanApp16ClientCollateralImageUpdate @ServerLoanAppID='"+ ServerLoanAppID + "',@ImgPathNew='/imgFromDevice/" + ServerDate + "_" + SavedFName + "',@LoanAppClientCollateralID='" + ServerLoanAppClientCollateralID + "',@ImgPath='" + SavedFName + "'");
            //                            ERR = dt2.Rows[0]["ERR"].ToString();
            //                            SMS = dt2.Rows[0]["SMS"].ToString();
            //                        }
            //                        catch
            //                        {
            //                            ERR = "Error";
            //                            SMS = "Saving Image Error: " + SavedFName;
            //                        }
            //                    }
            //                }
            //                #endregion CollImg

            //                #region PersonImg
            //                else if (ImgType == "PersonImg")
            //                {
            //                    string ServerLoanAppID = fname[1];
            //                    string ServerLoanAppPersonID = fname[2];
            //                    string OneCardTwoDoc = fname[3];
            //                    DataTable dt = c.ReturnDT("select LoanAppPersonID from tblLoanAppPerson21Image where LoanAppPersonID='" + ServerLoanAppPersonID + "' and ImgPath='" + SavedFName + "' and OneCardTwoDoc='" + OneCardTwoDoc + "'");
            //                    if (dt.Rows.Count == 0)
            //                    {
            //                        ERR = "Error";
            //                        SMS = "Invalid Image Name: " + SavedFName;
            //                    }
            //                    else
            //                    {
            //                        try
            //                        {
            //                            //copy img to web path
            //                            string SaveToWebPath = c.ImgPathGet() + ServerDate + "_" + SavedFName;
            //                            File.Copy(SavedFNamePath, SaveToWebPath);
            //                            //update fname
            //                            c.ReturnDT("update tblLoanAppPerson21Image set ImgPath='/imgFromDevice/" + ServerDate + "_" + SavedFName + "' where LoanAppPersonID='" + ServerLoanAppPersonID + "' and ImgPath='" + SavedFName + "' and OneCardTwoDoc='" + OneCardTwoDoc + "'");
            //                            ERR = "Succeed";
            //                            SMS = "Image is saved";
            //                        }
            //                        catch
            //                        {
            //                            ERR = "Error";
            //                            SMS = "Saving Image Error: " + SavedFName;
            //                        }
            //                    }
            //                }
            //                #endregion PersonImg

            //                #region AssetImg
            //                else if (ImgType == "AssetImg")
            //                {
            //                    string ServerLoanAppID = fname[1];
            //                    string ServerLoanAppClientAssetID = fname[2];
            //                    string AssetImageClientID = fname[3];
            //                    DataTable dt = c.ReturnDT("select AssetImageServerID from tblLoanApp13ClientAssetImage where AssetServerID='" + ServerLoanAppClientAssetID + "' and ImgPath='" + SavedFName + "' and AssetClientID='" + AssetImageClientID + "'");
            //                    if (dt.Rows.Count == 0)
            //                    {
            //                        ERR = "Error";
            //                        SMS = "Invalid Image Name: " + SavedFName;
            //                    }
            //                    else
            //                    {
            //                        try
            //                        {
            //                            //copy img to web path
            //                            string SaveToWebPath = c.ImgPathGet() + ServerDate + "_" + SavedFName;
            //                            File.Copy(SavedFNamePath, SaveToWebPath);
            //                            //update fname
            //                            string AssetImageServerID = dt.Rows[0]["AssetImageServerID"].ToString();
            //                            c.ReturnDT("update tblLoanApp13ClientAssetImage set ImgPath='/imgFromDevice/" + ServerDate + "_" + SavedFName + "' where AssetImageServerID='" + AssetImageServerID + "'");
            //                            ERR = "Succeed";
            //                            SMS = "Image is saved";
            //                        }
            //                        catch
            //                        {
            //                            ERR = "Error";
            //                            SMS = "Saving Image Error: " + SavedFName;
            //                        }
            //                    }
            //                }
            //                #endregion AssetImg

            //                #region RealEstateImg
            //                else if (ImgType == "RealEstateImg")
            //                {
            //                    string ServerLoanAppID = fname[1];
            //                    string CollateralServerID = fname[2];
            //                    string PREImgV2_imageClientID = fname[3];
            //                    DataTable dt = c.ReturnDT("select ImageServerID from tblLoanApp15ClientCollateralRealEstateImage where CollateralServerID='" + CollateralServerID + "' and ImgPath='" + SavedFName + "' and ImageClientID='" + PREImgV2_imageClientID + "'");
            //                    if (dt.Rows.Count == 0)
            //                    {
            //                        ERR = "Error";
            //                        SMS = "Invalid Image Name: " + SavedFName;
            //                    }
            //                    else
            //                    {
            //                        try
            //                        {
            //                            //copy img to web path
            //                            string SaveToWebPath = c.ImgPathGet() + ServerDate + "_" + SavedFName;
            //                            File.Copy(SavedFNamePath, SaveToWebPath);
            //                            //update fname
            //                            string ImageServerID = dt.Rows[0]["ImageServerID"].ToString();
            //                            c.ReturnDT("update tblLoanApp15ClientCollateralRealEstateImage set ImgPath='/imgFromDevice/" + ServerDate + "_" + SavedFName + "' where ImageServerID='" + ImageServerID + "'");
            //                            ERR = "Succeed";
            //                            SMS = "Image is saved";
            //                        }
            //                        catch
            //                        {
            //                            ERR = "Error";
            //                            SMS = "Saving Image Error: " + SavedFName;
            //                        }
            //                    }
            //                }
            //                #endregion RealEstateImg

            //                else
            //                {
            //                    ERR = "Error";
            //                    SMS = "Invalid file";
            //                }
            //            }
            //            catch
            //            {
            //                ERR = "Error";
            //                SMS = "Invalid Image Name";
            //            }
            //                #endregion check fname
            //        }
            //    }
            //    #endregion save file

            //}
            //catch { }

            ////rs = "[{\"ERR\":\""+ ERR + "\",\"SMS\":\"" + SMS + "\"}]";
            //FileUploadResModel ListHeader = new FileUploadResModel();
            //ListHeader.ERR = ERR;
            //ListHeader.SMS = SMS;
            //List<FileUploadResModel> RSData = new List<FileUploadResModel>();
            //RSData.Add(ListHeader);


            //return RSData;
            return null;
        }
    }
}


//client call
//private void button1_Click(object sender, EventArgs e)
//{
//    DialogResult dr = this.openFileDialog1.ShowDialog();
//    if (dr == System.Windows.Forms.DialogResult.OK)
//    {
//        try
//        {
//            HttpClient httpClient = new HttpClient();
//            // Read the files 
//            foreach (String file in openFileDialog1.FileNames)
//            {

//                var fileStream = File.Open(file, FileMode.Open);
//                var fileInfo = new FileInfo(file);
//                FileUploadResult uploadResult = null;
//                bool _fileUploaded = false;

//                var content = new MultipartFormDataContent();
//                content.Add(new StreamContent(fileStream), "\"file\"", string.Format("\"{0}\"", fileInfo.Name)
//                );

//                uploadServiceBaseAddress = uploadServiceBaseAddress + "?par1=123";

//                Task taskUpload = httpClient.PostAsync(uploadServiceBaseAddress, content).ContinueWith(task =>
//                {
//                    if (task.Status == TaskStatus.RanToCompletion)
//                    {
//                        var response = task.Result;

//                        if (response.IsSuccessStatusCode)
//                        {
//                            uploadResult = response.Content.ReadAsAsync<FileUploadResult>().Result;
//                            if (uploadResult != null)
//                                _fileUploaded = true;

//                            // Read other header values if you want..
//                            foreach (var header in response.Content.Headers)
//                            {
//                                Debug.WriteLine("{0}: {1}", header.Key, string.Join(",", header.Value));
//                            }
//                        }
//                        else
//                        {
//                            Debug.WriteLine("Status Code: {0} - {1}", response.StatusCode, response.ReasonPhrase);
//                            Debug.WriteLine("Response Body: {0}", response.Content.ReadAsStringAsync().Result);
//                        }
//                    }

//                    fileStream.Dispose();
//                });

//                taskUpload.Wait();
//                if (_fileUploaded)
//                {
//                    AddMessage(uploadResult.FileName + " with length " + uploadResult.FileLength
//                                    + " has been uploaded at " + uploadResult.LocalFilePath);
//                }
//                else
//                {
//                    AddMessage("Unable to connect");
//                }
//            }

//            httpClient.Dispose();
//        }
//        catch (Exception ex)
//        {
//            AddMessage(ex.Message);
//        }
//    }
//}
