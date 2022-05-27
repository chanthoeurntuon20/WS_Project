using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;
using WebApiFileUpload.API.Infrastructure;
using WebService;

namespace WebApiFileUpload.API.Controllers
{

    [BasicAuthentication]
    public class FileUploadV2Controller : ApiController
    {
        [MimeMultipart]
        [HttpPost]
        //public async Task<FileUploadResult> Post(int? par1)
        //public async Task<IEnumerable<FileUploadV2RS>> Post([FromUri]string api_name, string api_key, string json)
        public async Task<IEnumerable<FileUploadV2RS>> Post([FromUri]string p, [FromUri]string msgid)
        {
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "", ExSMS = "", ERRCode = "";
            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            List<FileUploadV2RS> RSData = new List<FileUploadV2RS>();
            string ControllerName = "FileUploadV2";
            string FileNameForLog = msgid + "_" + ServerDate.Replace("-", "_").Replace(" ", "_").Replace(":", "_");
            string SavedFName = "";
            try
            {
                #region msgid
                if (ERR != "Error")
                {
                    string[] str = c.CheckMsgID(msgid);
                    ERR = str[0];
                    SMS = str[1];
                }
                #endregion
                #region add log
                if (ERR != "Error")
                {
                    c.T24_AddLog(FileNameForLog, "1.RQ", p, ControllerName);
                }
                #endregion
                #region p -> SessionID
                string UserID = "";
                if (ERR != "Error")
                {
                    //p = System.Web.HttpUtility.UrlDecode(p);
                    string[] rs = c.SessionIDCheck(ServerDate, p);
                    ERR = rs[0];
                    SMS = rs[1];
                    ExSMS = rs[2];
                    UserID = rs[3];
                    ERRCode = rs[4];
                }
                #endregion

                #region save file - old
                //if (ERR != "Error")
                //{
                //    #region save file
                //    var uploadPath = HttpContext.Current.Server.MapPath("~/Uploads");
                //    var multipartFormDataStreamProvider = new UploadMultipartFormProvider(uploadPath);

                //    // Read the MIME multipart asynchronously 
                //    UploadMultipartFormProvider content = await Request.Content.ReadAsMultipartAsync(multipartFormDataStreamProvider);
                //    //count content
                //    int iContent = content.Contents.Count;
                //    if (iContent != 1)
                //    {
                //        ERR = "Error";
                //        SMS = "Only one image is allowed to upload at once";
                //    }
                //    #endregion
                //    if (ERR != "Error")
                //    {
                //        string SavedFNamePath = multipartFormDataStreamProvider
                //            .FileData.Select(multiPartData => multiPartData.LocalFileName).FirstOrDefault();
                //        SavedFName = Path.GetFileName(SavedFNamePath);
                //        //'CollImg_'+@ServerLoanAppID+'_'+@ServerLoanAppPersonID+'_'+@ServerLoanAppClientCollateralID+'_'+ @datetime+@Ext
                //        //'PersonImg_'+@ServerLoanAppID+'_'+@ServerLoanAppPersonID+'_'+@OneCardTwoDoc+'_'+ @datetime+@Ext
                //        #region check fname
                //        try
                //        {
                //            //FileUploadV2RS ListHeader = new FileUploadV2RS();
                //            //ListHeader.ERR = ERR;
                //            //ListHeader.SMS = SMS;

                //            ServerDate = ServerDate.Replace(" ", ".").Replace("-", ".").Replace(":", ".");

                //            string[] fname = SavedFName.Split('_');
                //            string ImgType = fname[0];

                //            //not use
                //            #region CollImg
                //            if (ImgType == "CollImg")
                //            {
                //                //string ServerLoanAppID = fname[1];
                //                //string ServerLoanAppPersonID = fname[2];
                //                //string ServerLoanAppClientCollateralID = fname[3];
                //                //string sql = "select LoanAppClientCollateralID from tblLoanApp16ClientCollateralImage where LoanAppClientCollateralID='" + ServerLoanAppClientCollateralID + "' and ImgPath='" + SavedFName + "'";
                //                //DataTable dt = c.ReturnDT(sql);
                //                //if (dt.Rows.Count == 0)
                //                //{
                //                //    ERR = "Error";
                //                //    SMS = "Invalid Image Name: " + SavedFName;
                //                //}
                //                //else
                //                //{
                //                //    try
                //                //    {
                //                //        //copy img to web path
                //                //        string SaveToWebPath = c.ImgPathGet() + ServerDate + "_" + SavedFName;
                //                //        File.Copy(SavedFNamePath, SaveToWebPath);
                //                //        //update fname
                //                //        DataTable dt2=c.ReturnDT("exec T24_tblLoanApp16ClientCollateralImageUpdate @ServerLoanAppID='"+ ServerLoanAppID + "',@ImgPathNew='/imgFromDevice/" + ServerDate + "_" + SavedFName + "',@LoanAppClientCollateralID='" + ServerLoanAppClientCollateralID + "',@ImgPath='" + SavedFName + "'");
                //                //        ERR = dt2.Rows[0]["ERR"].ToString();
                //                //        SMS = dt2.Rows[0]["SMS"].ToString();
                //                //    }
                //                //    catch
                //                //    {
                //                //        ERR = "Error";
                //                //        SMS = "Saving Image Error: " + SavedFName;
                //                //    }
                //                //}
                //            }
                //            #endregion CollImg

                //            //ok
                //            #region PersonImg
                //            else if (ImgType == "PersonImg")
                //            {//string perImg_fName = "PersonImg_" + ServerLoanAppID + "_" + ServerLoanAppPersonID + "_" + OneCardTwoDoc + "_" + CustImageClientID+"_"+ ServerDateForFileName + "." + perImg_ext.Replace(".", "");
                //                string ServerLoanAppID = fname[1];
                //                string ServerLoanAppPersonID = fname[2];
                //                string OneCardTwoDoc = fname[3];
                //                //DataTable dt = c.ReturnDT("select LoanAppPersonID from tblLoanAppPerson21Image where LoanAppPersonID='" + ServerLoanAppPersonID + "' and ImgPath='" + SavedFName + "' and OneCardTwoDoc='" + OneCardTwoDoc + "' and UploadDate is null");
                //                DataTable dt = c.ReturnDT("select LoanAppPersonID from tblLoanAppPerson21Image where LoanAppPersonID='" + ServerLoanAppPersonID + "' and ImgPath='" + c.ImgPathGet() + SavedFName + "'");
                //                if (dt.Rows.Count == 0)
                //                {
                //                    ERR = "Error";
                //                    SMS = "Invalid PersonImg Name: Image is not match";
                //                }
                //                else
                //                {
                //                    try
                //                    {
                //                        //copy img to web path
                //                        //string SaveToWebPath = c.ImgPathGet() + ServerDate + "_" + SavedFName;
                //                        string SaveToWebPath = c.ImgPathGet()+ SavedFName;
                //                        File.Copy(SavedFNamePath, SaveToWebPath);
                //                        //File.Delete(SavedFNamePath);
                //                        //update fname
                //                        //c.ReturnDT("update tblLoanAppPerson21Image set UploadDate=getdate(),ImgPath='" + SaveToWebPath + "' where LoanAppPersonID='" + ServerLoanAppPersonID + "' and ImgPath='" + SavedFName + "' and OneCardTwoDoc='" + OneCardTwoDoc + "' and UploadDate is null");
                //                        //c.ReturnDT("update T24_tblLoanAppImgLogV2 set IsUpload=1,UploadedDate=getdate() where LoanAppID='" + ServerLoanAppID + "' and ServerImgName='" + SavedFName + "'");
                //                        //c.ReturnDT("exec T24_LoanAppActive @LoanAppID='" + ServerLoanAppID + "'");

                //                        string sql = "exec T24_LoanAppImgLogV2_Upload @LoanAppID='" + ServerLoanAppID + "',@ServerImgName=@ServerImgName";
                //                        SqlConnection Con1 = new SqlConnection(c.ConStr());
                //                        Con1.Open();
                //                        SqlCommand Com1 = new SqlCommand();
                //                        Com1.Connection = Con1;
                //                        Com1.Parameters.Clear();
                //                        Com1.CommandText = sql;
                //                        Com1.Parameters.AddWithValue("@ServerImgName", SavedFName);
                //                        Com1.ExecuteNonQuery();
                //                        Con1.Close();
                //                        ERR = "Succeed";
                //                        SMS = "Image is saved";
                //                    }
                //                    catch (Exception ex)
                //                    {
                //                        ERR = "Error";
                //                        SMS = "Saving Image Error: " + ex.Message.ToString();
                //                    }
                //                }

                //            }
                //            #endregion PersonImg
                //            //ok
                //            #region AssetImg
                //            else if (ImgType == "AssetImg")
                //            {//string Asset_fName = "AssetImg_" + ServerLoanAppID + "_" + ServerLoanAppClientAssetID + "_" + AssetImageClientID+"_"+ServerDateForFileName + "." + AssetImg_Ext.Replace(".", "");
                //                string ServerLoanAppID = fname[1];
                //                string AssetImageServerID = fname[2];
                //                string AssetImageClientID = fname[3];
                //                //DataTable dt = c.ReturnDT("select AssetImageServerID from tblLoanApp13ClientAssetImage where AssetServerID='" + AssetImageServerID + "' and ImgPath='" + SavedFName + "' and UploadDate is null");
                //                DataTable dt = c.ReturnDT("select AssetImageServerID from tblLoanApp13ClientAssetImage where AssetServerID='" + AssetImageServerID + "' and ImgPath='" + c.ImgPathGet() + SavedFName + "'");
                //                if (dt.Rows.Count == 0)
                //                {
                //                    ERR = "Error";
                //                    SMS = "Invalid AssetImg Name: Image is not match";
                //                }
                //                else
                //                {
                //                    try
                //                    {
                //                        //copy img to web path
                //                        //string SaveToWebPath = c.ImgPathGet() + ServerDate + "_" + SavedFName;
                //                        string SaveToWebPath = c.ImgPathGet() +  SavedFName;
                //                        File.Copy(SavedFNamePath, SaveToWebPath);
                //                        //update fname
                //                        //c.ReturnDT("update tblLoanApp13ClientAssetImage set UploadDate=getdate(),ImgPath='" + SaveToWebPath + "' where AssetServerID='" + AssetImageServerID + "' and ImgPath='" + SavedFName + "' and UploadDate is null");
                //                        //c.ReturnDT("update T24_tblLoanAppImgLogV2 set IsUpload=1,UploadedDate=getdate() where LoanAppID='" + ServerLoanAppID + "' and ServerImgName='" + SavedFName + "'");
                //                        //c.ReturnDT("exec T24_LoanAppActive @LoanAppID='" + ServerLoanAppID + "'");

                //                        string sql = "exec T24_LoanAppImgLogV2_Upload @LoanAppID='" + ServerLoanAppID + "',@ServerImgName=@ServerImgName";
                //                        SqlConnection Con1 = new SqlConnection(c.ConStr());
                //                        Con1.Open();
                //                        SqlCommand Com1 = new SqlCommand();
                //                        Com1.Connection = Con1;
                //                        Com1.Parameters.Clear();
                //                        Com1.CommandText = sql;
                //                        Com1.Parameters.AddWithValue("@ServerImgName", SavedFName);
                //                        Com1.ExecuteNonQuery();
                //                        Con1.Close();
                //                        ERR = "Succeed";
                //                        SMS = "Image is saved";
                //                    }
                //                    catch (Exception ex)
                //                    {
                //                        ERR = "Error";
                //                        SMS = "Saving Image Error: " + ex.Message.ToString();
                //                        ExSMS = ex.Message.ToString();
                //                    }
                //                }

                //            }
                //            #endregion AssetImg
                //            //ok
                //            #region RealEstateImg
                //            else if (ImgType == "RealEstateImg")
                //            {//string RealEstate_fName = "RealEstateImg_" + ServerLoanAppID + "_" + CollateralServerID + "_" + PREImgV2_imageClientID+"_"+ServerDateForFileName + "." + PREImgV2_ext.Replace(".", "");
                //                string ServerLoanAppID = fname[1];
                //                string CollateralServerID = fname[2];
                //                string PREImgV2_imageClientID = fname[3];
                //                //DataTable dt = c.ReturnDT("select CollateralServerID from tblLoanApp15ClientCollateralRealEstateImage where CollateralServerID='" + CollateralServerID + "' and ImgPath='" + SavedFName + "' and UploadDate is null");
                //                DataTable dt = c.ReturnDT("select CollateralServerID from tblLoanApp15ClientCollateralRealEstateImage where CollateralServerID='" + CollateralServerID + "' and ImgPath='" + c.ImgPathGet() + SavedFName + "'");
                //                if (dt.Rows.Count == 0)
                //                {
                //                    ERR = "Error";
                //                    SMS = "Invalid RealEstateImg Name: Image is not match";
                //                }
                //                else
                //                {
                //                    try
                //                    {
                //                        //copy img to web path
                //                        //string SaveToWebPath = c.ImgPathGet() + ServerDate + "_" + SavedFName;
                //                        string SaveToWebPath = c.ImgPathGet() + SavedFName;
                //                        File.Copy(SavedFNamePath, SaveToWebPath);
                //                        //update fname
                //                        //c.ReturnDT("update tblLoanApp15ClientCollateralRealEstateImage set UploadDate=getdate(),ImgPath='" + SaveToWebPath + "' where CollateralServerID='" + CollateralServerID + "' and ImgPath='" + SavedFName + "' and UploadDate is null");
                //                        //c.ReturnDT("update T24_tblLoanAppImgLogV2 set IsUpload=1,UploadedDate=getdate() where LoanAppID='" + ServerLoanAppID + "' and ServerImgName='" + SavedFName + "'");
                //                        //c.ReturnDT("exec T24_LoanAppActive @LoanAppID='" + ServerLoanAppID + "'");

                //                        string sql = "exec T24_LoanAppImgLogV2_Upload @LoanAppID='" + ServerLoanAppID + "',@ServerImgName=@ServerImgName";
                //                        SqlConnection Con1 = new SqlConnection(c.ConStr());
                //                        Con1.Open();
                //                        SqlCommand Com1 = new SqlCommand();
                //                        Com1.Connection = Con1;
                //                        Com1.Parameters.Clear();
                //                        Com1.CommandText = sql;
                //                        Com1.Parameters.AddWithValue("@ServerImgName", SavedFName);
                //                        Com1.ExecuteNonQuery();
                //                        Con1.Close();
                //                        ERR = "Succeed";
                //                        SMS = "Image is saved";
                //                    }
                //                    catch (Exception ex)
                //                    {
                //                        ERR = "Error";
                //                        SMS = "Saving Image Error: " + ex.Message.ToString();
                //                    }
                //                }

                //            }
                //            #endregion RealEstateImg

                //            //not use
                //            #region ProsCustImg
                //            //else if (ImgType == "ProsCustImg")
                //            //{
                //            //    //string CustImageServerID = fname[1];
                //            //    //string CustImageClientID = fname[2];
                //            //    //string CustServerID = fname[3];
                //            //    //string OneCardTwoDoc = fname[4];
                //            //    ////copy img to web path
                //            //    //string SaveToWebPath = c.ImgPathGet() + ServerDate + "_" + SavedFName;
                //            //    //File.Copy(SavedFNamePath, SaveToWebPath);
                //            //    //string Ext = Path.GetExtension(SavedFNamePath);
                //            //    ////update fname
                //            //    //string ImgPath= "/imgFromDevice/" + ServerDate + "_" + SavedFName;
                //            //    //string sql = "exec V2_ProsCustImgAddEdit @UserID='"+ UserID + "',@CustServerID='"+ CustServerID 
                //            //    //+ "',@CustImageServerID='"+ CustImageServerID + "',@CustImageClientID='"+ CustImageClientID 
                //            //    //+ "',@OneCardTwoDoc='"+ OneCardTwoDoc + "',@Ext='"+ Ext + "',@ImgPath='"+ ImgPath + "'";
                //            //    //try {
                //            //    //    DataTable dt = c.ReturnDT(sql);
                //            //    //    if (dt.Rows[0]["err"].ToString() == "0")
                //            //    //    {
                //            //    //        ERR = "Error";
                //            //    //        SMS = dt.Rows[0]["sms"].ToString();
                //            //    //    }
                //            //    //    else
                //            //    //    {
                //            //    //        ERR = "Succeed";
                //            //    //        SMS = "";
                //            //    //        CustImageServerID = dt.Rows[0]["err"].ToString();
                //            //    //    }
                //            //    //} catch(Exception ex) {
                //            //    //    ERR = "Error";
                //            //    //    SMS = "Something was wrong";
                //            //    //}
                //            //    //List<FileUploadV2SMSRS> DataList = new List<FileUploadV2SMSRS>();
                //            //    //FileUploadV2SMSRS dPro = new FileUploadV2SMSRS();
                //            //    //dPro.ERR = ERR;
                //            //    //dPro.SMS = SMS;
                //            //    //dPro.CustImageClientID = CustImageClientID;
                //            //    //dPro.CustImageServerID = CustImageServerID;
                //            //    //DataList.Add(dPro);

                //            //    //ListHeader.FileUploadV2SMSRS = DataList;
                //            //    //RSData.Add(ListHeader);

                //            //}
                //            #endregion ProsCustImg

                //            //ok
                //            #region SendDB
                //            else if (ImgType == "SendDB")
                //            {//string SendDB_fName = "SendDB_" + "xxx.db";
                //                string SaveToWebPath = c.ImgPathGet() + @"db\" + UserID + "_TabletProject.db";
                //                if (File.Exists(SaveToWebPath)) {
                //                    File.Delete(SaveToWebPath);
                //                }
                //                File.Copy(SavedFNamePath, SaveToWebPath);
                //            }
                //            #endregion

                //            else
                //            {
                //                ERR = "Error";
                //                SMS = "Invalid File Type";
                //            }
                //        }
                //        catch (Exception ex)
                //        {
                //            ERR = "Error";
                //            SMS = "Invalid File Name: " + ex.Message.ToString();
                //            ExSMS = ex.Message.ToString();
                //        }
                //        #endregion check fname
                //    }
                //}
                #endregion save file
                #region save file
                if (ERR != "Error")
                {
                    c.T24_AddLog(FileNameForLog, "StartWriteFile", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), ControllerName);
                    #region save file
                    string todayDate = DateTime.Now.ToString("yyyyMMdd");
                    var uploadPath = HttpContext.Current.Server.MapPath("~/Uploads") + "\\" + todayDate;
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }
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
                    c.T24_AddLog(FileNameForLog, "EndWriteFile", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), ControllerName);
                    if (ERR != "Error")
                    {
                        string SavedFNamePath = multipartFormDataStreamProvider
                            .FileData.Select(multiPartData => multiPartData.LocalFileName).FirstOrDefault();
                        SavedFName = Path.GetFileName(SavedFNamePath);
                        //'CollImg_'+@ServerLoanAppID+'_'+@ServerLoanAppPersonID+'_'+@ServerLoanAppClientCollateralID+'_'+ @datetime+@Ext
                        //'PersonImg_'+@ServerLoanAppID+'_'+@ServerLoanAppPersonID+'_'+@OneCardTwoDoc+'_'+ @datetime+@Ext
                        c.T24_AddLog(FileNameForLog, "StartCheckFile", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), ControllerName);
                        #region check fname
                        try
                        {
                            //FileUploadV2RS ListHeader = new FileUploadV2RS();
                            //ListHeader.ERR = ERR;
                            //ListHeader.SMS = SMS;

                            ServerDate = ServerDate.Replace(" ", ".").Replace("-", ".").Replace(":", ".");

                            string[] fname = SavedFName.Split('_');
                            string ImgType = fname[0];

                            #region CollImg
                            if (ImgType == "CollImg")
                            {
                                //string ServerLoanAppID = fname[1];
                                //string ServerLoanAppPersonID = fname[2];
                                //string ServerLoanAppClientCollateralID = fname[3];
                                //string sql = "select LoanAppClientCollateralID from tblLoanApp16ClientCollateralImage where LoanAppClientCollateralID='" + ServerLoanAppClientCollateralID + "' and ImgPath='" + SavedFName + "'";
                                //DataTable dt = c.ReturnDT(sql);
                                //if (dt.Rows.Count == 0)
                                //{
                                //    ERR = "Error";
                                //    SMS = "Invalid Image Name: " + SavedFName;
                                //}
                                //else
                                //{
                                //    try
                                //    {
                                //        //copy img to web path
                                //        string SaveToWebPath = c.ImgPathGet() + ServerDate + "_" + SavedFName;
                                //        File.Copy(SavedFNamePath, SaveToWebPath);
                                //        //update fname
                                //        DataTable dt2=c.ReturnDT("exec T24_tblLoanApp16ClientCollateralImageUpdate @ServerLoanAppID='"+ ServerLoanAppID + "',@ImgPathNew='/imgFromDevice/" + ServerDate + "_" + SavedFName + "',@LoanAppClientCollateralID='" + ServerLoanAppClientCollateralID + "',@ImgPath='" + SavedFName + "'");
                                //        ERR = dt2.Rows[0]["ERR"].ToString();
                                //        SMS = dt2.Rows[0]["SMS"].ToString();
                                //    }
                                //    catch
                                //    {
                                //        ERR = "Error";
                                //        SMS = "Saving Image Error: " + SavedFName;
                                //    }
                                //}
                            }
                            #endregion CollImg

                            //ok  1=tblLoanAppPerson2 -> LoanAppPersonID=ServerLoanAppPersonID
                            #region PersonImg
                            else if (ImgType == "PersonImg")
                            {//string perImg_fName = "PersonImg_" + ServerLoanAppID + "_" + ServerLoanAppPersonID + "_" + OneCardTwoDoc + "_" + CustImageClientID+"_"+ ServerDateForFileName + "." + perImg_ext.Replace(".", "");
                                string ServerLoanAppID = fname[1];
                                string ServerLoanAppPersonID = fname[2];
                                string OneCardTwoDoc = fname[3];
                                //DataTable dt = c.ReturnDT("select LoanAppPersonID from tblLoanAppPerson21Image where LoanAppPersonID='" + ServerLoanAppPersonID + "' and ImgPath='" + SavedFName + "' and OneCardTwoDoc='" + OneCardTwoDoc + "' and UploadDate is null");
                                DataTable dt = c.ReturnDT("select LoanAppPersonID from tblLoanAppPerson21Image where LoanAppPersonID='" + ServerLoanAppPersonID + "' and ImgPath='" + SavedFName + "'");
                                if (1 == 0) //if (dt.Rows.Count == 0)
                                {
                                    ERR = "Error";
                                    SMS = "Invalid PersonImg Name: Image is not match";
                                }
                                else
                                {
                                    try
                                    {
                                        //copy img to web path
                                        //string SaveToWebPath = c.ImgPathGet() + SavedFName;
                                        //string SaveToWebPath = c.ImgPathTabletLoanImageGet()[0] + SavedFName;
                                        //string SaveToWebPathUsername = c.ImgPathTabletLoanImageGet()[1] + SavedFName;
                                        //string SaveToWebPathPwd = c.ImgPathTabletLoanImageGet()[2] + SavedFName;
                                        //File.Move(SavedFNamePath, SaveToWebPath);
                                        //File.Copy(SavedFNamePath, SaveToWebPath);
                                        //File.Delete(SavedFNamePath);
                                        //update fname
                                        //c.ReturnDT("update tblLoanAppPerson21Image set UploadDate=getdate(),ImgPath='" + SaveToWebPath + "' where LoanAppPersonID='" + ServerLoanAppPersonID + "' and ImgPath='" + SavedFName + "' and OneCardTwoDoc='" + OneCardTwoDoc + "' and UploadDate is null");
                                        //c.ReturnDT("update T24_tblLoanAppImgLogV2 set IsUpload=1,UploadedDate=getdate() where LoanAppID='" + ServerLoanAppID + "' and ServerImgName='" + SavedFName + "'");
                                        //c.ReturnDT("exec T24_LoanAppActive @LoanAppID='" + ServerLoanAppID + "'");

                                        c.T24_AddLog(FileNameForLog, "StartCopyFile", "SavedFNamePath " + SavedFNamePath + " - SavedFName " + SavedFName +  "-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), ControllerName);
                                        string SaveToWebPath = NetworkShare.MoveFile(FileNameForLog, ControllerName, SavedFNamePath, SavedFName);
                                        c.T24_AddLog(FileNameForLog, "EndCopyFile", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), ControllerName);
                                        if (SaveToWebPath == "")
                                        {
                                            ERR = "Error";
                                            SMS = "Saving Image Error to share path";
                                        }
                                        else
                                        {
                                            c.T24_AddLog(FileNameForLog, "StartUpdateDB", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), ControllerName);

                                            string tblName = "1";//tblLoanAppPerson2
                                            string tblKey = ServerLoanAppPersonID;
                                            string DocType = OneCardTwoDoc;

                                            string sql = "exec T24_LoanAppImgLogV2_Upload @LoanAppID='" + ServerLoanAppID
                                            + "',@ServerImgName=@ServerImgName,@SavedFName=@SavedFName,@tblName=@tblName,@tblKey=@tblKey,@DocType=@DocType";
                                            SqlConnection Con1 = new SqlConnection(c.ConStr());
                                            Con1.Open();
                                            SqlCommand Com1 = new SqlCommand();
                                            Com1.Connection = Con1;
                                            Com1.Parameters.Clear();
                                            Com1.CommandText = sql;
                                            Com1.Parameters.AddWithValue("@ServerImgName", SaveToWebPath);
                                            Com1.Parameters.AddWithValue("@SavedFName", SavedFName);
                                            Com1.Parameters.AddWithValue("@tblName", tblName);
                                            Com1.Parameters.AddWithValue("@tblKey", tblKey);
                                            Com1.Parameters.AddWithValue("@DocType", DocType);
                                            Com1.ExecuteNonQuery();
                                            Con1.Close();
                                            ERR = "Succeed";
                                            SMS = "Image is saved";

                                            c.T24_AddLog(FileNameForLog, "EndUpdateDB", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), ControllerName);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        ERR = "Error";
                                        SMS = "Saving Image Error: " + ex.Message.ToString();
                                    }
                                }

                            }
                            #endregion PersonImg
                            //ok 2=tblLoanApp13ClientAsset -> LoanAppClientAssetID=AssetImageServerID
                            #region AssetImg
                            else if (ImgType == "AssetImg")
                            {//string Asset_fName = "AssetImg_" + ServerLoanAppID + "_" + ServerLoanAppClientAssetID + "_" + AssetImageClientID+"_"+ServerDateForFileName + "." + AssetImg_Ext.Replace(".", "");
                                string ServerLoanAppID = fname[1];
                                string AssetImageServerID = fname[2];
                                string AssetImageClientID = fname[3];
                                //DataTable dt = c.ReturnDT("select AssetImageServerID from tblLoanApp13ClientAssetImage where AssetServerID='" + AssetImageServerID + "' and ImgPath='" + SavedFName + "' and UploadDate is null");
                                DataTable dt = c.ReturnDT("select AssetImageServerID from tblLoanApp13ClientAssetImage where AssetServerID='" + AssetImageServerID + "' and ImgPath='" + SavedFName + "'");
                                if (dt.Rows.Count == 0)
                                {
                                    ERR = "Error";
                                    SMS = "Invalid AssetImg Name: Image is not match";
                                }
                                else
                                {
                                    try
                                    {
                                        //copy img to web path
                                        //string SaveToWebPath = c.ImgPathGet() + ServerDate + "_" + SavedFName;
                                        //string SaveToWebPath = c.ImgPathGet() + SavedFName;
                                        //File.Copy(SavedFNamePath, SaveToWebPath);
                                        //update fname
                                        //c.ReturnDT("update tblLoanApp13ClientAssetImage set UploadDate=getdate(),ImgPath='" + SaveToWebPath + "' where AssetServerID='" + AssetImageServerID + "' and ImgPath='" + SavedFName + "' and UploadDate is null");
                                        //c.ReturnDT("update T24_tblLoanAppImgLogV2 set IsUpload=1,UploadedDate=getdate() where LoanAppID='" + ServerLoanAppID + "' and ServerImgName='" + SavedFName + "'");
                                        //c.ReturnDT("exec T24_LoanAppActive @LoanAppID='" + ServerLoanAppID + "'");

                                        c.T24_AddLog(FileNameForLog, "StartCopyFile", "SavedFNamePath " + SavedFNamePath + " - SavedFName " + SavedFName + "-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), ControllerName);
                                        string SaveToWebPath = NetworkShare.MoveFile(FileNameForLog, ControllerName, SavedFNamePath, SavedFName);
                                        c.T24_AddLog(FileNameForLog, "EndCopyFile", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), ControllerName);
                                        if (SaveToWebPath == "")
                                        {
                                            ERR = "Error";
                                            SMS = "Saving Image Error to share path";
                                        }
                                        else
                                        {
                                            c.T24_AddLog(FileNameForLog, "StartUpdateDB", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), ControllerName);

                                            string tblName = "2";//tblLoanApp13ClientAsset
                                            string tblKey = AssetImageServerID;
                                            string DocType = "0";

                                            string sql = "exec T24_LoanAppImgLogV2_Upload @LoanAppID='" + ServerLoanAppID
                                            + "',@ServerImgName=@ServerImgName,@SavedFName=@SavedFName,@tblName=@tblName,@tblKey=@tblKey,@DocType=@DocType";
                                            SqlConnection Con1 = new SqlConnection(c.ConStr());
                                            Con1.Open();
                                            SqlCommand Com1 = new SqlCommand();
                                            Com1.Connection = Con1;
                                            Com1.Parameters.Clear();
                                            Com1.CommandText = sql;
                                            Com1.Parameters.AddWithValue("@ServerImgName", SaveToWebPath);
                                            Com1.Parameters.AddWithValue("@SavedFName", SavedFName);
                                            Com1.Parameters.AddWithValue("@tblName", tblName);
                                            Com1.Parameters.AddWithValue("@tblKey", tblKey);
                                            Com1.Parameters.AddWithValue("@DocType", DocType);
                                            Com1.ExecuteNonQuery();
                                            Con1.Close();
                                            ERR = "Succeed";
                                            SMS = "Image is saved";
                                            c.T24_AddLog(FileNameForLog, "EndUpdateDB", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), ControllerName);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        ERR = "Error";
                                        SMS = "Saving Image Error: " + ex.Message.ToString();
                                        ExSMS = ex.Message.ToString();
                                    }
                                }

                            }
                            #endregion AssetImg
                            //ok 3=tblLoanApp15ClientCollateralRealEstate -> CollateralServerID=CollateralServerID
                            #region RealEstateImg
                            else if (ImgType == "RealEstateImg")
                            {//string RealEstate_fName = "RealEstateImg_" + ServerLoanAppID + "_" + CollateralServerID + "_" + PREImgV2_imageClientID+"_"+ServerDateForFileName + "." + PREImgV2_ext.Replace(".", "");
                                string ServerLoanAppID = fname[1];
                                string CollateralServerID = fname[2];
                                string PREImgV2_imageClientID = fname[3];
                                //DataTable dt = c.ReturnDT("select CollateralServerID from tblLoanApp15ClientCollateralRealEstateImage where CollateralServerID='" + CollateralServerID + "' and ImgPath='" + SavedFName + "' and UploadDate is null");
                                DataTable dt = c.ReturnDT("select CollateralServerID from tblLoanApp15ClientCollateralRealEstateImage where CollateralServerID='" + CollateralServerID + "' and ImgPath='" + SavedFName + "'");
                                if (dt.Rows.Count == 0)
                                {
                                    ERR = "Error";
                                    SMS = "Invalid RealEstateImg Name: Image is not match";
                                }
                                else
                                {
                                    try
                                    {
                                        //copy img to web path
                                        //string SaveToWebPath = c.ImgPathGet() + ServerDate + "_" + SavedFName;
                                        //string SaveToWebPath = c.ImgPathGet() + SavedFName;
                                        //File.Copy(SavedFNamePath, SaveToWebPath);
                                        //update fname
                                        //c.ReturnDT("update tblLoanApp15ClientCollateralRealEstateImage set UploadDate=getdate(),ImgPath='" + SaveToWebPath + "' where CollateralServerID='" + CollateralServerID + "' and ImgPath='" + SavedFName + "' and UploadDate is null");
                                        //c.ReturnDT("update T24_tblLoanAppImgLogV2 set IsUpload=1,UploadedDate=getdate() where LoanAppID='" + ServerLoanAppID + "' and ServerImgName='" + SavedFName + "'");
                                        //c.ReturnDT("exec T24_LoanAppActive @LoanAppID='" + ServerLoanAppID + "'");

                                        c.T24_AddLog(FileNameForLog, "StartCopyFile", "SavedFNamePath " + SavedFNamePath + " - SavedFName " + SavedFName + "-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), ControllerName);
                                        string SaveToWebPath = NetworkShare.MoveFile(FileNameForLog, ControllerName, SavedFNamePath, SavedFName);
                                        c.T24_AddLog(FileNameForLog, "EndCopyFile", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), ControllerName);
                                        if (SaveToWebPath == "")
                                        {
                                            ERR = "Error";
                                            SMS = "Saving Image Error to share path";
                                        }
                                        else
                                        {
                                            c.T24_AddLog(FileNameForLog, "StartUpdateDB", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), ControllerName);

                                            string tblName = "3";//tblLoanApp15ClientCollateralRealEstate
                                            string tblKey = CollateralServerID;
                                            string DocType = "0";

                                            string sql = "exec T24_LoanAppImgLogV2_Upload @LoanAppID='" + ServerLoanAppID
                                            + "',@ServerImgName=@ServerImgName,@SavedFName=@SavedFName,@tblName=@tblName,@tblKey=@tblKey,@DocType=@DocType";
                                            SqlConnection Con1 = new SqlConnection(c.ConStr());
                                            Con1.Open();
                                            SqlCommand Com1 = new SqlCommand();
                                            Com1.Connection = Con1;
                                            Com1.Parameters.Clear();
                                            Com1.CommandText = sql;
                                            Com1.Parameters.AddWithValue("@ServerImgName", SaveToWebPath);
                                            Com1.Parameters.AddWithValue("@SavedFName", SavedFName);
                                            Com1.Parameters.AddWithValue("@tblName", tblName);
                                            Com1.Parameters.AddWithValue("@tblKey", tblKey);
                                            Com1.Parameters.AddWithValue("@DocType", DocType);
                                            Com1.ExecuteNonQuery();
                                            Con1.Close();
                                            ERR = "Succeed";
                                            SMS = "Image is saved";
                                            c.T24_AddLog(FileNameForLog, "EndUpdateDB", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), ControllerName);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        ERR = "Error";
                                        SMS = "Saving Image Error: " + ex.Message.ToString();
                                    }
                                }

                            }
                            #endregion RealEstateImg
                            //kyc 4=tblLoanAppPerson2KYC
                            #region KYCImg
                            else if (ImgType == "KYCImg")
                            {//string KYCImg_fName = "KYCImg_" + ServerLoanAppID + "_" + KYCServerID + "_" + KYCImageServerID + "_" + ServerDateForFileName + "." + kyc_Ext.Replace(".", "");
                                string ServerLoanAppID = fname[1];
                                string KYCServerID = fname[2];
                                string KYCImageServerID = fname[3];
                                DataTable dt = c.ReturnDT("select KYCServerID from tblLoanAppPerson2KYCImg where KYCImageServerID='" + KYCImageServerID + "'");// and ImgPath='" + SavedFName + "'
                                if (dt.Rows.Count == 0)
                                {
                                    ERR = "Error";
                                    SMS = "Invalid KYCImg Name: Image is not match";
                                }
                                else
                                {
                                    try
                                    {
                                        //copy img to web path
                                        //string SaveToWebPath = c.ImgPathGet() + ServerDate + "_" + SavedFName;
                                        //string SaveToWebPath = c.ImgPathGet() + SavedFName;
                                        //File.Copy(SavedFNamePath, SaveToWebPath);
                                        //update fname
                                        //c.ReturnDT("update tblLoanApp15ClientCollateralRealEstateImage set UploadDate=getdate(),ImgPath='" + SaveToWebPath + "' where CollateralServerID='" + CollateralServerID + "' and ImgPath='" + SavedFName + "' and UploadDate is null");
                                        //c.ReturnDT("update T24_tblLoanAppImgLogV2 set IsUpload=1,UploadedDate=getdate() where LoanAppID='" + ServerLoanAppID + "' and ServerImgName='" + SavedFName + "'");
                                        //c.ReturnDT("exec T24_LoanAppActive @LoanAppID='" + ServerLoanAppID + "'");

                                        c.T24_AddLog(FileNameForLog, "StartCopyFile", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), ControllerName);
                                        string SaveToWebPath = NetworkShare.MoveFile(FileNameForLog, ControllerName, SavedFNamePath, SavedFName);
                                        c.T24_AddLog(FileNameForLog, "EndCopyFile", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), ControllerName);
                                        if (SaveToWebPath == "")
                                        {
                                            ERR = "Error";
                                            SMS = "Saving Image Error to share path";
                                        }
                                        else
                                        {
                                            c.T24_AddLog(FileNameForLog, "StartUpdateDB", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), ControllerName);

                                            string tblName = "4";//tblLoanAppPerson2KYC
                                            string tblKey = KYCServerID;
                                            string DocType = "0";

                                            string sql = "exec T24_LoanAppImgLogV2_Upload @LoanAppID='" + ServerLoanAppID
                                            + "',@ServerImgName=@ServerImgName,@SavedFName=@SavedFName,@tblName=@tblName,@tblKey=@tblKey,@DocType=@DocType";
                                            SqlConnection Con1 = new SqlConnection(c.ConStr());
                                            Con1.Open();
                                            SqlCommand Com1 = new SqlCommand();
                                            Com1.Connection = Con1;
                                            Com1.Parameters.Clear();
                                            Com1.CommandText = sql;
                                            Com1.Parameters.AddWithValue("@ServerImgName", SaveToWebPath);
                                            Com1.Parameters.AddWithValue("@SavedFName", SavedFName);
                                            Com1.Parameters.AddWithValue("@tblName", tblName);
                                            Com1.Parameters.AddWithValue("@tblKey", tblKey);
                                            Com1.Parameters.AddWithValue("@DocType", DocType);
                                            Com1.ExecuteNonQuery();
                                            Con1.Close();
                                            ERR = "Succeed";
                                            SMS = "Image is saved";
                                            c.T24_AddLog(FileNameForLog, "EndUpdateDB", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), ControllerName);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        ERR = "Error";
                                        SMS = "Saving Image Error: " + ex.Message.ToString();
                                    }
                                }

                            }
                            #endregion RealEstateImg

                            //not use
                            #region ProsCustImg
                            //else if (ImgType == "ProsCustImg")
                            //{
                            //    //string CustImageServerID = fname[1];
                            //    //string CustImageClientID = fname[2];
                            //    //string CustServerID = fname[3];
                            //    //string OneCardTwoDoc = fname[4];
                            //    ////copy img to web path
                            //    //string SaveToWebPath = c.ImgPathGet() + ServerDate + "_" + SavedFName;
                            //    //File.Copy(SavedFNamePath, SaveToWebPath);
                            //    //string Ext = Path.GetExtension(SavedFNamePath);
                            //    ////update fname
                            //    //string ImgPath= "/imgFromDevice/" + ServerDate + "_" + SavedFName;
                            //    //string sql = "exec V2_ProsCustImgAddEdit @UserID='"+ UserID + "',@CustServerID='"+ CustServerID 
                            //    //+ "',@CustImageServerID='"+ CustImageServerID + "',@CustImageClientID='"+ CustImageClientID 
                            //    //+ "',@OneCardTwoDoc='"+ OneCardTwoDoc + "',@Ext='"+ Ext + "',@ImgPath='"+ ImgPath + "'";
                            //    //try {
                            //    //    DataTable dt = c.ReturnDT(sql);
                            //    //    if (dt.Rows[0]["err"].ToString() == "0")
                            //    //    {
                            //    //        ERR = "Error";
                            //    //        SMS = dt.Rows[0]["sms"].ToString();
                            //    //    }
                            //    //    else
                            //    //    {
                            //    //        ERR = "Succeed";
                            //    //        SMS = "";
                            //    //        CustImageServerID = dt.Rows[0]["err"].ToString();
                            //    //    }
                            //    //} catch(Exception ex) {
                            //    //    ERR = "Error";
                            //    //    SMS = "Something was wrong";
                            //    //}
                            //    //List<FileUploadV2SMSRS> DataList = new List<FileUploadV2SMSRS>();
                            //    //FileUploadV2SMSRS dPro = new FileUploadV2SMSRS();
                            //    //dPro.ERR = ERR;
                            //    //dPro.SMS = SMS;
                            //    //dPro.CustImageClientID = CustImageClientID;
                            //    //dPro.CustImageServerID = CustImageServerID;
                            //    //DataList.Add(dPro);

                            //    //ListHeader.FileUploadV2SMSRS = DataList;
                            //    //RSData.Add(ListHeader);

                            //}
                            #endregion ProsCustImg

                            else
                            {
                                ERR = "Error";
                                SMS = "Invalid Image Name";
                            }
                        }
                        catch (Exception ex)
                        {
                            ERR = "Error";
                            SMS = "Invalid Image Name: " + ex.Message.ToString();
                            ExSMS = ex.Message.ToString();
                        }
                        #endregion check fname
                        c.T24_AddLog(FileNameForLog, "EndCheckFile", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), ControllerName);
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

            FileUploadV2RS ListHeader = new FileUploadV2RS();
            ListHeader.ERR = ERR;
            ListHeader.SMS = SMS;
            ListHeader.ERRCode = ERRCode;
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

    public class FileUploadV2RQ
    {
        public string user { get; set; }
        public string pwd { get; set; }
        public string device_id { get; set; }
        public string app_vName { get; set; }
        public string mac_address { get; set; }
        public string criteriaValue { get; set; }
    }
    public class FileUploadV2RS
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
        public string ERRCode { get; set; }
        public string FName { get; set; }
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
