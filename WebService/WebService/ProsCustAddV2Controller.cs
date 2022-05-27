using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;

namespace WebService
{
    [BasicAuthentication]
    public class ProsCustAddV2Controller : ApiController
    {
        //[EnableCors(origins: "*", headers: "*", methods: "*")]
        // GET api/<controller>
        //public IEnumerable<ProsCustAddRS> Post([FromUri]string api_name, string api_key, [FromBody]string json)
        public string Post([FromUri]string p, [FromUri]string msgid, [FromBody]string json)
        {
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "", ExSMS = "", ERRCode = "";
            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string ControllerName = "ProsCustAddV2";
            string FileNameForLog = msgid + "_" + ServerDate.Replace("-", "_").Replace(" ", "_").Replace(":", "_").Replace(".", "_");
            List<ProsCustAddRS> RSData = new List<ProsCustAddRS>();
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
                #region check json
                if (ERR != "Error")
                {
                    string[] str = c.CheckObjED(json, "2");
                    ERR = str[0];
                    SMS = str[1];
                    ExSMS = str[2];
                    json = str[3];
                }
                #endregion check json
                #region read json
                ProsCustAddV2RQ jObj = null;
                if (ERR != "Error")
                {
                    try
                    {
                        jObj = JsonConvert.DeserializeObject<ProsCustAddV2RQ>(json);
                        //user = jObj.user;
                        //user = c.Encrypt(user, c.SeekKeyGet());
                        //pwd = jObj.pwd;
                        //pwd = c.Encrypt(pwd, c.SeekKeyGet());
                        //device_id = jObj.device_id;
                        //app_vName = jObj.app_vName;
                        //mac_address = jObj.mac_address;
                    }
                    catch(Exception ex)
                    {
                        ERR = "Error";
                        SMS = "Invalid JSON";
                        ExSMS = ex.Message.ToString();
                    }
                }
                #endregion json                
                #region data
                if (ERR != "Error")
                {
                    ProsCustAddRS ListHeader = new ProsCustAddRS();
                    ListHeader.ERR = ERR;
                    ListHeader.SMS = SMS;

                    List<ProsCustAddSMSRS> DataList = new List<ProsCustAddSMSRS>();
                    int i = 0;
                    foreach (var data in jObj.ProsCustDataList)
                    {
                        string CustClientID = "", CustServerID="";
                        string DataERR= "Succeed";
                        string DataSMS="";
                        i++;
                        try {
                            #region para
                            CustClientID = data.CustClientID;
                            CustServerID = data.CustServerID;
                            string ProsCode = data.ProsCode;
                            string CreateDateClient = data.CreateDateClient;
                            string VBID = data.VBID;
                            string ReferByID = data.ReferByID;
                            string ReferName = data.ReferName;
                            string NameKhLast = data.NameKhLast;
                            string NameKhFirst = data.NameKhFirst;
                            string NameEnLast = data.NameKhFirst;
                            string NameEnFirst = data.NameKhFirst;
                            string TitleID = data.TitleID;
                            string GenderID = data.GenderID;
                            string DateOfBirth = data.DateOfBirth;
                            string IDCardType = data.IDCardType;
                            string IDCardNumber = data.IDCardNumber;
                            string IDCardIssueDate = data.IDCardIssueDate;
                            string IDCardExpiryDate = data.IDCardExpiryDate;
                            string MaritalStatusID = data.MaritalStatusID;
                            string EducationLevelID = data.EducationLevelID;
                            string Phone = data.Phone;
                            string PlaceOfBirth = data.PlaceOfBirth;
                            string VillageIDPer = data.VillageIDPer;
                            string VillageIDCur = data.VillageIDCur;
                            string ShortAddress = data.ShortAddress;
                            string FamilyMenber = data.FamilyMenber;
                            string FamilyMenberActive = data.FamilyMenberActive;
                            string PoorLevelID = data.PoorLevelID;
                            string LatLon = data.LatLon;
                            #endregion para
                            #region validation
                            #region CustClientID
                            if (DataERR != "Error") {
                                int val;
                                if (!int.TryParse(CustClientID, out val))
                                {
                                    DataERR = "Error";
                                    DataSMS = "Invalid CustClientID";
                                }
                            }
                            #endregion CustClientID
                            #region CustServerID 
                            if (DataERR != "Error")
                            {
                                if (CustServerID != "")
                                {
                                    int val;
                                    if (!int.TryParse(CustServerID, out val))
                                    {
                                        DataERR = "Error";
                                        DataSMS = "Invalid CustServerID ";
                                    }
                                }
                            }
                            #endregion CustServerID 
                            #region CreateDateClient
                            if (DataERR != "Error")
                            {
                                DateTime val;
                                if (!DateTime.TryParse(CreateDateClient, out val))
                                {
                                    DataERR = "Error";
                                    DataSMS = "Invalid CreateDateClient";
                                }
                            }
                            #endregion CreateDateClient                            
                            #region ReferByID
                            if (DataERR != "Error")
                            {
                                try {
                                    if (ReferByID.Length == 0)
                                    {
                                        DataERR = "Error";
                                        DataSMS = "Invalid ReferByID";
                                    }
                                    else {
                                        if (!(ReferByID == "AMK" || ReferByID == "SAP" || ReferByID == "VBP")) {
                                            DataERR = "Error";
                                            DataSMS = "Invalid ReferByID";
                                        }
                                    }
                                } catch (Exception ex) {
                                    DataERR = "Error";
                                    DataSMS = "Invalid ReferByID";
                                }
                            }
                            #endregion ReferByID
                            #region ReferName
                            if (DataERR != "Error")
                            {
                                if (ReferByID.Length > 0) {
                                    if (ReferName.Length == 0) {
                                        DataERR = "Error";
                                        DataSMS = "Invalid ReferName";
                                    }
                                }
                            }
                            #endregion ReferName
                            #region NameKhLast 
                            if (DataERR != "Error")
                            {
                                if (NameKhLast.Length ==0)
                                {
                                    DataERR = "Error";
                                    DataSMS = "Invalid NameKhLast";
                                }
                            }
                            #endregion NameKhLast 
                            #region NameKhFirst 
                            if (DataERR != "Error")
                            {
                                if (NameKhFirst.Length == 0)
                                {
                                    DataERR = "Error";
                                    DataSMS = "Invalid NameKhFirst";
                                }
                            }
                            #endregion NameKhFirst
                            #region NameEnLast  
                            if (DataERR != "Error")
                            {
                                if (NameEnLast.Length == 0)
                                {
                                    DataERR = "Error";
                                    DataSMS = "Invalid NameEnLast";
                                }
                            }
                            #endregion NameEnLast
                            #region NameEnFirst  
                            if (DataERR != "Error")
                            {
                                if (NameEnFirst.Length == 0)
                                {
                                    DataERR = "Error";
                                    DataSMS = "Invalid NameEnFirst";
                                }
                            }
                            #endregion NameEnFirst
                            #region GenderID
                            if (DataERR != "Error")
                            {
                                if (!(GenderID == "FEMALE" || GenderID == "MALE"))
                                {
                                    DataERR = "Error";
                                    DataSMS = "Invalid GenderID";
                                }
                            }
                            #endregion GenderID
                            #region DateOfBirth
                            if (DataERR != "") {
                                if (DateOfBirth.Length == 0)
                                {
                                    DataERR = "Error";
                                    DataSMS = "Invalid DateOfBirth";
                                }
                            }
                            if (DataERR != "Error")
                            {
                                DateTime val;
                                if (!DateTime.TryParse(DateOfBirth, out val))
                                {
                                    DataERR = "Error";
                                    DataSMS = "Invalid DateOfBirth";
                                }
                            }
                            int Age = 0;
                            if (DataERR != "Error")
                            {
                                try {
                                    Age = (int)Math.Floor((DateTime.Now - Convert.ToDateTime(DateOfBirth)).TotalDays / 365.25D);
                                } catch {
                                    DataERR = "Error";
                                    DataSMS = "Invalid DateOfBirth";
                                }
                            }
                            if (DataERR != "Error")
                            {
                                if (Convert.ToInt16(Age) < 18) {
                                    DataERR = "Error";
                                    DataSMS = "Invalid DateOfBirth";
                                }
                                if (Convert.ToInt16(Age) > 70)
                                {
                                    DataERR = "Error";
                                    DataSMS = "Invalid DateOfBirth";
                                }
                            }
                            #endregion DateOfBirth
                            #region IDCardType
                            if (DataERR != "Error")
                            {
                                try
                                {
                                    if (IDCardType.Length == 0)
                                    {
                                        DataERR = "Error";
                                        DataSMS = "Invalid IDCardType";
                                    }
                                    else
                                    {
                                        if (!(IDCardType == "1" || IDCardType == "2" || IDCardType == "3" || IDCardType == "4" || IDCardType == "5" || IDCardType == "6" || IDCardType == "7" || IDCardType == "8" || IDCardType == "9" || IDCardType == "10"))
                                        {
                                            DataERR = "Error";
                                            DataSMS = "Invalid IDCardType";
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    DataERR = "Error";
                                    DataSMS = "Invalid IDCardType";
                                }
                            }
                            #endregion IDCardType
                            #region IDCardNumber
                            if (DataERR != "Error")
                            {
                                if (IDCardType != "10")
                                {
                                    int IDCardNumMin = 0, IDCardNumMax = 0;
                                    if (IDCardType == "1")
                                    {
                                        IDCardNumMin = 9; IDCardNumMax = 9;
                                    }
                                    else if (IDCardType == "2")
                                    {
                                        IDCardNumMin = 1; IDCardNumMax = 20;
                                    }
                                    else if (IDCardType == "3")
                                    {
                                        IDCardNumMin = 5; IDCardNumMax = 8;
                                    }
                                    else if (IDCardType == "4")
                                    {
                                        IDCardNumMin = 5; IDCardNumMax = 8;
                                    }
                                    else if (IDCardType == "5")
                                    {
                                        IDCardNumMin = 1; IDCardNumMax = 20;
                                    }
                                    else if (IDCardType == "6")
                                    {
                                        IDCardNumMin = 2; IDCardNumMax = 20;
                                    }
                                    else if (IDCardType == "7")
                                    {
                                        IDCardNumMin = 1; IDCardNumMax = 15;
                                    }
                                    else if (IDCardType == "8")
                                    {
                                        IDCardNumMin = 2; IDCardNumMax = 12;
                                    }
                                    else if (IDCardType == "9")
                                    {
                                        IDCardNumMin = 1; IDCardNumMax = 15;
                                    }

                                    if (!(IDCardNumber.Length >= IDCardNumMin || IDCardNumber.Length <= IDCardNumMax))
                                    {
                                        DataERR = "Error";
                                        DataSMS = "Invalid IDCardNumber";
                                    }
                                }
                                
                            }
                            #endregion IDCardNumber
                            #region IDCardIssueDate
                            if (DataERR != "Error")
                            {
                                try
                                {
                                    if ((IDCardType == "1" || IDCardType == "2" || IDCardType == "3" || IDCardType == "4" || IDCardType == "5" || IDCardType == "6" || IDCardType == "9"))
                                    {
                                        if (IDCardIssueDate.Length == 0)
                                        {
                                            DataERR = "Error";
                                            DataSMS = "Invalid IDCardIssueDate";
                                        }
                                        else {
                                            DateTime val;
                                            if (!DateTime.TryParse(IDCardIssueDate, out val))
                                            {
                                                DataERR = "Error";
                                                DataSMS = "Invalid IDCardIssueDate";
                                            }
                                            else {
                                                if (Convert.ToDateTime(IDCardIssueDate) > Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"))) {
                                                    DataERR = "Error";
                                                    DataSMS = "Invalid IDCardIssueDate";
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    DataERR = "Error";
                                    DataSMS = "Invalid IDCardIssueDate";
                                }
                            }
                            #endregion IDCardIssueDate
                            #region IDCardExpiryDate
                            if (DataERR != "Error")
                            {
                                try
                                {
                                    if ((IDCardType == "1" || IDCardType == "3" || IDCardType == "4" || IDCardType == "5" ))
                                    {
                                        if (IDCardIssueDate.Length == 0)
                                        {
                                            DataERR = "Error";
                                            DataSMS = "Invalid IDCardExpiryDate";
                                        }
                                        else
                                        {
                                            DateTime val;
                                            if (!DateTime.TryParse(IDCardExpiryDate, out val))
                                            {
                                                DataERR = "Error";
                                                DataSMS = "Invalid IDCardExpiryDate";
                                            }
                                            else
                                            {
                                                if (Convert.ToDateTime(IDCardExpiryDate) <= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")))
                                                {
                                                    DataERR = "Error";
                                                    DataSMS = "Invalid IDCardExpiryDate";
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    DataERR = "Error";
                                    DataSMS = "Invalid IDCardExpiryDate";
                                }
                            }
                            #endregion IDCardExpiryDate
                            #region MaritalStatusID
                            if (DataERR != "Error")
                            {
                                try
                                {
                                    if (MaritalStatusID.Length == 0)
                                    {
                                        DataERR = "Error";
                                        DataSMS = "Invalid MaritalStatusID";
                                    }
                                    else
                                    {
                                        if (!(MaritalStatusID == "DEFACTO" || MaritalStatusID == "DIVORCED" || MaritalStatusID == "MARRIED" || MaritalStatusID == "SEPARATED" || MaritalStatusID == "SINGLE" || MaritalStatusID == "WIDOW" || MaritalStatusID == "WIDOWER"))
                                        {
                                            DataERR = "Error";
                                            DataSMS = "Invalid MaritalStatusID";
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    DataERR = "Error";
                                    DataSMS = "Invalid MaritalStatusID";
                                }
                            }
                            #endregion MaritalStatusID
                            #region EducationLevelID
                            if (DataERR != "Error")
                            {
                                try
                                {
                                    if (EducationLevelID.Length == 0)
                                    {
                                        DataERR = "Error";
                                        DataSMS = "Invalid EducationLevelID";
                                    }
                                    else
                                    {
                                        if (!(EducationLevelID == "1" || EducationLevelID == "2" || EducationLevelID == "3" || EducationLevelID == "4" || EducationLevelID == "5" || EducationLevelID == "6"))
                                        {
                                            DataERR = "Error";
                                            DataSMS = "Invalid EducationLevelID";
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    DataERR = "Error";
                                    DataSMS = "Invalid EducationLevelID";
                                }
                            }
                            #endregion EducationLevelID
                            #region Phone
                            if (DataERR != "Error")
                            {
                                int val;
                                if (!int.TryParse(Phone, out val))
                                {
                                    DataERR = "Error";
                                    DataSMS = "Invalid Phone";
                                }
                            }
                            if (DataERR != "Error")
                            {
                                if (!(Phone.Length==9 || Phone.Length == 10))
                                {
                                    DataERR = "Error";
                                    DataSMS = "Invalid Phone";
                                }
                            }
                            #endregion Phone
                            #region PlaceOfBirth
                            if (DataERR != "Error")
                            {
                                if (PlaceOfBirth.Length==0)
                                {
                                    DataERR = "Error";
                                    DataSMS = "Invalid PlaceOfBirth";
                                }
                            }
                            #endregion PlaceOfBirth
                            #region VillageIDPer
                            if (DataERR != "Error")
                            {
                                if (VillageIDPer.Length == 0)
                                {
                                    DataERR = "Error";
                                    DataSMS = "Invalid VillageIDPer";
                                }
                            }
                            #endregion VillageIDPer
                            #region VillageIDCur
                            if (DataERR != "Error")
                            {
                                if (VillageIDCur.Length == 0)
                                {
                                    DataERR = "Error";
                                    DataSMS = "Invalid VillageIDCur";
                                }
                            }
                            #endregion VillageIDCur
                            #region ShortAddress
                            if (DataERR != "Error")
                            {
                                if (ShortAddress.Length == 0)
                                {
                                    DataERR = "Error";
                                    DataSMS = "Invalid ShortAddress";
                                }
                            }
                            #endregion ShortAddress
                            #region FamilyMenber
                            if (DataERR != "Error")
                            {
                                try
                                {
                                    if (FamilyMenber.Length == 0)
                                    {
                                        DataERR = "Error";
                                        DataSMS = "Invalid FamilyMenber";
                                    }
                                    else
                                    {
                                        int val;
                                        if (!int.TryParse(FamilyMenber, out val))
                                        {
                                            DataERR = "Error";
                                            DataSMS = "Invalid FamilyMenber ";
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    DataERR = "Error";
                                    DataSMS = "Invalid FamilyMenber";
                                }
                            }
                            #endregion FamilyMenber
                            #region FamilyMenberActive
                            if (DataERR != "Error")
                            {
                                try
                                {
                                    if (FamilyMenberActive.Length == 0)
                                    {
                                        DataERR = "Error";
                                        DataSMS = "Invalid FamilyMenberActive";
                                    }
                                    else
                                    {
                                        int val;
                                        if (!int.TryParse(FamilyMenberActive, out val))
                                        {
                                            DataERR = "Error";
                                            DataSMS = "Invalid FamilyMenberActive ";
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    DataERR = "Error";
                                    DataSMS = "Invalid FamilyMenberActive";
                                }
                            }
                            #endregion FamilyMenberActive
                            #region PoorLevelID
                            if (DataERR != "Error")
                            {
                                try
                                {
                                    if (PoorLevelID.Length == 0)
                                    {
                                        DataERR = "Error";
                                        DataSMS = "Invalid PoorLevelID";
                                    }
                                    else
                                    {
                                        if (!(PoorLevelID == "LEVEL1" || PoorLevelID == "LEVEL2" || PoorLevelID == "NONPOOR" || PoorLevelID == "NOTAVAIL"))
                                        {
                                            DataERR = "Error";
                                            DataSMS = "Invalid PoorLevelID";
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    DataERR = "Error";
                                    DataSMS = "Invalid PoorLevelID";
                                }
                            }
                            #endregion PoorLevelID


                            #endregion validation
                            #region To DB
                            if (DataERR != "Error")
                            {
                                SqlConnection Con1 = new SqlConnection(c.ConStr());
                                Con1.Open();
                                SqlCommand Com1 = new SqlCommand();
                                Com1.Connection = Con1;
                                try
                                {
                                    #region To DB
                                    string sql = "exec V2_ProsCustAddEdit @CustServerID=@CustServerID,@CustClientID=@CustClientID,@CreateByUserID=@CreateByUserID"
                                    + ",@CreateDateClient=@CreateDateClient,@ProspectCode=@ProspectCode"
                                    + ",@VBID=@VBID,@ReferByID=@ReferByID,@ReferName=@ReferName"
                                    + ",@NameKhLast=@NameKhLast,@NameKhFirst=@NameKhFirst,@NameEnLast=@NameEnLast,@NameEnFirst=@NameEnFirst,@TitleID=@TitleID,@GenderID=@GenderID"
                                    + ",@DateOfBirth=@DateOfBirth,@IDCardType=@IDCardType,@IDCardNumber=@IDCardNumber"
                                    + ",@IDCardIssueDate=@IDCardIssueDate,@IDCardExpiryDate=@IDCardExpiryDate,@MaritalStatusID=@MaritalStatusID"
                                    + ",@EducationLevelID=@EducationLevelID,@Phone=@Phone,@PlaceOfBirth=@PlaceOfBirth"
                                    + ",@VillageIDPer=@VillageIDPer,@VillageIDCur=@VillageIDCur,@ShortAddress=@ShortAddress"
                                    + ",@FamilyMenber=@FamilyMenber,@FamilyMenberActive=@FamilyMenberActive,@PoorLevelID=@PoorLevelID,@LatLon=@LatLon";
                                    Com1.CommandText = sql;
                                    Com1.Parameters.Clear();
                                    Com1.Parameters.AddWithValue("@CustServerID", CustServerID);
                                    Com1.Parameters.AddWithValue("@CustClientID", CustClientID);
                                    Com1.Parameters.AddWithValue("@CreateByUserID", UserID);
                                    Com1.Parameters.AddWithValue("@CreateDateClient", CreateDateClient);
                                    Com1.Parameters.AddWithValue("@ProspectCode", ProsCode);
                                    Com1.Parameters.AddWithValue("@VBID", VBID);
                                    Com1.Parameters.AddWithValue("@ReferByID", ReferByID);
                                    Com1.Parameters.AddWithValue("@ReferName", ReferName);
                                    Com1.Parameters.AddWithValue("@NameKhLast", NameKhLast);
                                    Com1.Parameters.AddWithValue("@NameKhFirst", NameKhFirst);
                                    Com1.Parameters.AddWithValue("@NameEnLast", NameEnLast);
                                    Com1.Parameters.AddWithValue("@NameEnFirst", NameEnFirst);
                                    Com1.Parameters.AddWithValue("@TitleID", TitleID);
                                    Com1.Parameters.AddWithValue("@GenderID", GenderID);
                                    Com1.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
                                    Com1.Parameters.AddWithValue("@IDCardType", IDCardType);
                                    Com1.Parameters.AddWithValue("@IDCardNumber", IDCardNumber);
                                    Com1.Parameters.AddWithValue("@IDCardIssueDate", IDCardIssueDate);
                                    Com1.Parameters.AddWithValue("@IDCardExpiryDate", IDCardExpiryDate);
                                    Com1.Parameters.AddWithValue("@MaritalStatusID", MaritalStatusID);
                                    Com1.Parameters.AddWithValue("@EducationLevelID", EducationLevelID);
                                    Com1.Parameters.AddWithValue("@Phone", Phone);
                                    Com1.Parameters.AddWithValue("@PlaceOfBirth", PlaceOfBirth);
                                    Com1.Parameters.AddWithValue("@VillageIDPer", VillageIDPer);
                                    Com1.Parameters.AddWithValue("@VillageIDCur", VillageIDCur);
                                    Com1.Parameters.AddWithValue("@ShortAddress", ShortAddress);
                                    Com1.Parameters.AddWithValue("@FamilyMenber", FamilyMenber);
                                    Com1.Parameters.AddWithValue("@FamilyMenberActive", FamilyMenberActive);
                                    Com1.Parameters.AddWithValue("@PoorLevelID", PoorLevelID);
                                    Com1.Parameters.AddWithValue("@LatLon", LatLon);
                                    DataTable dt1 = new DataTable();
                                    dt1.Load(Com1.ExecuteReader());
                                    #endregion To DB
                                    #region params
                                    CustServerID = dt1.Rows[0]["ERR"].ToString();
                                    DataSMS = dt1.Rows[0]["SMS"].ToString();
                                    if (CustServerID != "0")
                                    {
                                        DataERR = "Succeed";
                                        DataSMS = "";
                                    }
                                    else {
                                        DataERR = "Error";
                                    }
                                    #endregion params
                                }
                                catch (Exception ex)
                                {
                                    DataERR = "Error";
                                    DataSMS = "Cannot add record to DB";
                                    ExSMS = ex.Message.ToString();
                                }
                                Con1.Close();
                            }   
                            #endregion To DB
                        }
                        catch (Exception ex) {
                            DataERR = "Error";
                            DataSMS ="Cannot read index "+i.ToString();
                            ExSMS = ex.Message.ToString();
                        }
                        ProsCustAddSMSRS dPro = new ProsCustAddSMSRS();
                        dPro.ERR = DataERR;
                        dPro.SMS = DataSMS;
                        dPro.CustClientID = CustClientID;
                        dPro.CustServerID = CustServerID;
                        DataList.Add(dPro);
                    }                    

                    ListHeader.ProsCustAddSMSRS = DataList;
                    RSData.Add(ListHeader);
                }
                #endregion data
            }
            catch (Exception ex)
            {
                ERR = "Error";
                SMS = "Something was wrong";
                ExSMS = ex.Message.ToString();
            }
            #region if Error
            if (ERR == "Error")
            {
                ProsCustAddRS ListHeader = new ProsCustAddRS();
                ListHeader.ERR = ERR;
                ListHeader.SMS = SMS;
                ListHeader.ERRCode = ERRCode;
                //ListHeader.DataList = null;
                RSData.Add(ListHeader);
            }
            #endregion if Error
            string RSDataStr = "";
            try
            {
                var jsonRS = new JavaScriptSerializer().Serialize(RSData);
                RSDataStr = c.Encrypt(jsonRS, c.SeekKeyGet());
                c.T24_AddLog(FileNameForLog, "RS", jsonRS.ToString(), ControllerName);
            }
            catch { }
            return RSDataStr;
        }

    }

    public class ProsCustAddV2RQ {
        public string user { get; set; } = "";
        public string pwd { get; set; } = "";
        public string device_id { get; set; } = "";
        public string app_vName { get; set; } = "";
        public string mac_address { get; set; } = "";
        public List<ProsCustDataList> ProsCustDataList;
    }
    public class ProsCustDataList {
        public string CustClientID { get; set; }
        public string CustServerID { get; set; }
        public string ProsCode { get; set; }
        public string CreateDateClient { get; set; }
        public string VBID { get; set; }
        public string ReferByID { get; set; }
        public string ReferName { get; set; }
        public string NameKhLast { get; set; }
        public string NameKhFirst { get; set; }
        public string NameEnLast { get; set; }
        public string NameEnFirst { get; set; }
        public string TitleID { get; set; }
        public string GenderID { get; set; }
        public string DateOfBirth { get; set; }
        public string IDCardType { get; set; }
        public string IDCardNumber { get; set; }
        public string IDCardIssueDate { get; set; }
        public string IDCardExpiryDate { get; set; }
        public string MaritalStatusID { get; set; }
        public string EducationLevelID { get; set; }
        public string Phone { get; set; }
        public string PlaceOfBirth { get; set; }
        public string VillageIDPer { get; set; }
        public string VillageIDCur { get; set; }
        public string ShortAddress { get; set; }
        public string FamilyMenber { get; set; }
        public string FamilyMenberActive { get; set; }
        public string PoorLevelID { get; set; }
        public string LatLon { get; set; }
    }
    
    public class ProsCustAddRS
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
        public string ERRCode { get; set; }
        public List<ProsCustAddSMSRS> ProsCustAddSMSRS;
    }
    public class ProsCustAddSMSRS {
        public string ERR { get; set; }
        public string SMS { get; set; }
        public string CustClientID { get; set; }
        public string CustServerID { get; set; }
    }

}