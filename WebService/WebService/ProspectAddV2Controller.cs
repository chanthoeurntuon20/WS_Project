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

    public class ProspectAddV2Controller : ApiController
    {
        //[EnableCors(origins: "*", headers: "*", methods: "*")]
        // GET api/<controller>
        //public IEnumerable<ProspectAddRS> Post([FromUri]string api_name, string api_key, string username, [FromBody]string json)
        public string Post([FromUri]string p, [FromUri]string msgid, [FromBody]string json)
        {
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "", ExSMS = "", ERRCode = "";
            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string ControllerName = "ProspectAddV2";
            string FileNameForLog = msgid + "_" + ServerDate.Replace("-", "_").Replace(" ", "_").Replace(":", "_").Replace(".", "_");
            List<ProspectAddRS> RSData = new List<ProspectAddRS>();
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
                string pSwitch = p.Substring(0, 3);
                string UserID = "", api_name="";

                if (pSwitch != "sw_")
                {
                    if (ERR != "Error")
                    {
                        //p = System.Web.HttpUtility.UrlDecode(p);
                        string[] rs = c.SessionIDCheck(ServerDate, p);
                        ERR = rs[0];
                        SMS = rs[1];
                        ExSMS = rs[2];
                        UserID = rs[3];
                        ERRCode = rs[4];
                        api_name = rs[5];
                    }
                }
                else {

                    string strs = p.Substring(3, p.Length - 3).Replace(" ", "+");
                    ERR = "";
                    SMS = "";
                    ExSMS = "";
                    UserID = c.Decrypt(strs, c.SeekKeyGet());
                    ExSMS = "";
                    ERRCode = "";
                    api_name = "";
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
                ProspectAddV2RQ jObj = null;
                if (ERR != "Error")
                {
                    try
                    {
                        jObj = JsonConvert.DeserializeObject<ProspectAddV2RQ>(json);
                        if (jObj.ProspectDataList.Count == 0) {
                            ERR = "Error";
                            SMS = "No Data";
                        }
                    }
                    catch (Exception ex)
                    {
                        ERR = "Error";
                        SMS = "Invalid JSON";
                        ExSMS = ex.ToString();
                    }
                }
                #endregion                
                #region data
                if (ERR != "Error")
                {
                    ProspectAddRS ListHeader = new ProspectAddRS();
                    ListHeader.ERR = ERR;
                    ListHeader.SMS = SMS;

                    List<ProspectAddSMSRS> DataList = new List<ProspectAddSMSRS>();
                    int i = 0;
                    foreach (var data in jObj.ProspectDataList)
                    {
                        string ProspectClientID = "", Code = "";
                        string DataERR = "Succeed";
                        string DataSMS = "";
                        i++;
                        try
                        {
                            #region para
                            ProspectClientID = data.ProspectClientID;
                            string CreateDateClient = data.CreateDateClient;
                            Code = data.Code;
                            string RegisterDate = data.RegisterDate;
                            string ReferByID = data.ReferByID;
                            string ReferName = data.ReferName;
                            string NameKh = data.NameKh;
                            string NameEn = data.NameEn;
                            string GenderID = data.GenderID;
                            string Age = data.Age;
                            string Phone = data.Phone;
                            string VillageID = data.VillageID;
                            string BizStatusID = data.BizStatusID;
                            string CollateralStatusID = data.CollateralStatusID;
                            string LoanEligibleID = data.LoanEligibleID;
                            string PromotionDate1 = data.PromotionDate1;
                            string PromotionDate2 = data.PromotionDate2;
                            string PromotionDate3 = data.PromotionDate3;
                            string CustComment = data.CustComment;
                            string ExpectOnBoardDate = data.ExpectOnBoardDate;
                            string ProspectStatusID = data.ProspectStatusID;
                            string IsOldCust = data.IsOldCust;
                            string OccupationType = data.OccupationType;
                            string COCommends = data.COCommends;
                            #endregion para
                            #region validation
                            #region ProspectClientID
                            if (DataERR != "Error")
                            {
                                int val;
                                if (!int.TryParse(ProspectClientID, out val))
                                {
                                    DataERR = "Error";
                                    DataSMS = "Invalid ProspectClientID";
                                }
                            }
                            #endregion ProspectClientID
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
                            #region RegisterDate
                            if (DataERR != "Error")
                            {
                                DateTime val;
                                if (!DateTime.TryParse(RegisterDate, out val))
                                {
                                    DataERR = "Error";
                                    DataSMS = "Invalid RegisterDate";
                                }
                            }
                            #endregion RegisterDate
                            #region ReferByID
                            if (DataERR != "Error")
                            {
                                try
                                {
                                    if (ReferByID.Length == 0)
                                    {
                                        DataERR = "Error";
                                        DataSMS = "Invalid ReferByID";
                                    }
                                    //else
                                    //{
                                    //    if (!(ReferByID == "0" || ReferByID == "AMK" || ReferByID == "SAP" || ReferByID == "VBP" || ReferByID == "Pipay"))
                                    //    {
                                    //        DataERR = "Error";
                                    //        DataSMS = "Invalid ReferByID";
                                    //    }
                                    //}
                                }
                                catch (Exception ex)
                                {
                                    DataERR = "Error";
                                    DataSMS = "Invalid ReferByID";
                                }
                            }
                            #endregion ReferByID
                            #region ReferName
                            if (DataERR != "Error")
                            {
                                if (ReferByID.Length > 0)
                                {
                                    if (ReferByID != "0")
                                    {
                                        if (ReferName.Length == 0)
                                        {
                                            DataERR = "Error";
                                            DataSMS = "Invalid ReferName";
                                        }
                                    }

                                }
                            }
                            #endregion ReferName
                            #region NameKh
                            if (DataERR != "Error")
                            {
                                if (NameKh.Length == 0)
                                {
                                    DataERR = "Error";
                                    DataSMS = "Invalid NameKh";
                                }
                            }
                            #endregion NameKh
                            #region NameEn
                            if (DataERR != "Error")
                            {
                                if (NameEn.Length == 0)
                                {
                                    DataERR = "Error";
                                    DataSMS = "Invalid NameEn";
                                }
                            }
                            #endregion NameEn
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
                            #region Age
                            if (DataERR != "Error")
                            {
                                int val;
                                if (!int.TryParse(Age, out val))
                                {
                                    DataERR = "Error";
                                    DataSMS = "Invalid Age";
                                }
                            }
                            if (DataERR != "Error")
                            {
                                if (Convert.ToInt16(Age) < 18)
                                {
                                    DataERR = "Error";
                                    DataSMS = "Invalid Age";
                                }
                                if (Convert.ToInt16(Age) > 70)
                                {
                                    DataERR = "Error";
                                    DataSMS = "Invalid Age";
                                }
                            }
                            #endregion Age
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
                                if (!(Phone.Length == 9 || Phone.Length == 10))
                                {
                                    DataERR = "Error";
                                    DataSMS = "Invalid Phone";
                                }
                            }
                            #endregion Phone
                            #region VillageID
                            if (DataERR != "Error")
                            {
                                if (VillageID.Length == 0)
                                {
                                    DataERR = "Error";
                                    DataSMS = "Invalid VillageID";
                                }
                            }
                            #endregion VillageID
                            #region BizStatusID
                            if (DataERR != "Error")
                            {
                                try
                                {
                                    if (BizStatusID.Length == 0)
                                    {
                                        DataERR = "Error";
                                        DataSMS = "Invalid BizStatusID";
                                    }
                                    else
                                    {
                                        if (!(Convert.ToInt16(BizStatusID) == 0 || Convert.ToInt16(BizStatusID) == 1 || Convert.ToInt16(BizStatusID) == 2))
                                        {
                                            DataERR = "Error";
                                            DataSMS = "Invalid BizStatusID";
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    DataERR = "Error";
                                    DataSMS = "Invalid BizStatusID";
                                }
                            }
                            #endregion BizStatusID
                            #region CollateralStatusID
                            if (DataERR != "Error")
                            {
                                try
                                {
                                    if (CollateralStatusID.Length == 0)
                                    {
                                        DataERR = "Error";
                                        DataSMS = "Invalid CollateralStatusID";
                                    }
                                    else
                                    {
                                        if (!(Convert.ToInt16(CollateralStatusID) == 0 || Convert.ToInt16(CollateralStatusID) == 1 || Convert.ToInt16(CollateralStatusID) == 2))
                                        {
                                            DataERR = "Error";
                                            DataSMS = "Invalid CollateralStatusID";
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    DataERR = "Error";
                                    DataSMS = "Invalid CollateralStatusID";
                                }
                            }
                            #endregion CollateralStatusID
                            #region LoanEligibleID
                            if (DataERR != "Error")
                            {
                                try
                                {
                                    if (LoanEligibleID.Length == 0)
                                    {
                                        DataERR = "Error";
                                        DataSMS = "Invalid LoanEligibleID";
                                    }
                                    else
                                    {
                                        if (!(Convert.ToInt16(LoanEligibleID) == 0 || Convert.ToInt16(LoanEligibleID) == 1 || Convert.ToInt16(LoanEligibleID) == 2))
                                        {
                                            DataERR = "Error";
                                            DataSMS = "Invalid LoanEligibleID";
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    DataERR = "Error";
                                    DataSMS = "Invalid LoanEligibleID";
                                }
                            }
                            #endregion LoanEligibleID
                            #region PromotionDate1
                            if (DataERR != "Error")
                            {
                                if (PromotionDate1.Length > 0)
                                {
                                    DateTime val;
                                    if (!DateTime.TryParse(PromotionDate1, out val))
                                    {
                                        DataERR = "Error";
                                        DataSMS = "Invalid PromotionDate1";
                                    }
                                    else
                                    {
                                        try
                                        {
                                            //if (Convert.ToDateTime(PromotionDate1) < Convert.ToDateTime(RegisterDate))
                                            //{
                                            //    DataERR = "Error";
                                            //    DataSMS = "Invalid PromotionDate1";
                                            //}
                                        }
                                        catch (Exception ex)
                                        {
                                            DataERR = "Error";
                                            DataSMS = "Invalid PromotionDate1";
                                        }
                                    }
                                }
                            }
                            #endregion PromotionDate1
                            #region PromotionDate2
                            if (DataERR != "Error")
                            {
                                if (PromotionDate2.Length > 0)
                                {
                                    DateTime val;
                                    if (!DateTime.TryParse(PromotionDate2, out val))
                                    {
                                        DataERR = "Error";
                                        DataSMS = "Invalid PromotionDate2";
                                    }
                                    else
                                    {
                                        try
                                        {
                                            if (Convert.ToDateTime(PromotionDate2) < Convert.ToDateTime(PromotionDate1))
                                            {
                                                DataERR = "Error";
                                                DataSMS = "Invalid PromotionDate2";
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            DataERR = "Error";
                                            DataSMS = "Invalid PromotionDate2";
                                        }
                                    }
                                }
                            }
                            #endregion PromotionDate2
                            #region PromotionDate3
                            if (DataERR != "Error")
                            {
                                if (PromotionDate3.Length > 0)
                                {
                                    DateTime val;
                                    if (!DateTime.TryParse(PromotionDate3, out val))
                                    {
                                        DataERR = "Error";
                                        DataSMS = "Invalid PromotionDate3";
                                    }
                                    else
                                    {
                                        try
                                        {
                                            if (Convert.ToDateTime(PromotionDate3) < Convert.ToDateTime(PromotionDate2))
                                            {
                                                DataERR = "Error";
                                                DataSMS = "Invalid PromotionDate3";
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            DataERR = "Error";
                                            DataSMS = "Invalid PromotionDate3";
                                        }
                                    }
                                }
                            }
                            #endregion PromotionDate3
                            #region ExpectOnBoardDate
                            if (DataERR != "Error")
                            {
                                if (ExpectOnBoardDate.Length > 0)
                                {
                                    DateTime val;
                                    if (!DateTime.TryParse(ExpectOnBoardDate, out val))
                                    {
                                        DataERR = "Error";
                                        DataSMS = "Invalid ExpectOnBoardDate";
                                    }
                                    else
                                    {
                                        try
                                        {
                                            //if (Convert.ToDateTime(ExpectOnBoardDate) < Convert.ToDateTime(DateTime.Now))
                                            //{
                                            //    DataERR = "Error";
                                            //    DataSMS = "Invalid ExpectOnBoardDate";
                                            //}
                                        }
                                        catch (Exception ex)
                                        {
                                            DataERR = "Error";
                                            DataSMS = "Invalid ExpectOnBoardDate";
                                        }
                                    }
                                }
                            }
                            #endregion ExpectOnBoardDate
                            #region ProspectStatusID
                            if (DataERR != "Error")
                            {
                                try
                                {
                                    if (ProspectStatusID.Length == 0)
                                    {
                                        DataERR = "Error";
                                        DataSMS = "Invalid ProspectStatusID";
                                    }
                                    else
                                    {
                                        if (!(Convert.ToInt16(ProspectStatusID) == 0 || Convert.ToInt16(ProspectStatusID) == 1 || Convert.ToInt16(ProspectStatusID) == 2 || Convert.ToInt16(ProspectStatusID) == 3))
                                        {
                                            DataERR = "Error";
                                            DataSMS = "Invalid ProspectStatusID";
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    DataERR = "Error";
                                    DataSMS = "Invalid ProspectStatusID";
                                }
                            }
                            #endregion ProspectStatusID
                            #region OccupationType
                          
                            if (DataERR != "Error")
                            {
                                try
                                {
                                    if(api_name == "Tablet")
                                    {
                                        if (OccupationType.Length == 0)
                                        {
                                            DataERR = "Error";
                                            DataSMS = "Invalid OccupationType is required";
                                        }else if(COCommends.Length == 0)
                                        {
                                            DataERR = "Error";
                                            DataSMS = "Invalid COCommends is required";
                                        }
                                    }
                                   
                                }
                                catch (Exception ex)
                                {
                                    DataERR = "Error";
                                    DataSMS = "Invalid ProspectStatusID";
                                }
                            }
                            #endregion ProspectStatusID
                            #endregion validation
                            #region To DB
                            if (DataERR != "Error")
                            {
                                try
                                {

                                    #region My New Code
                                    using (SqlConnection conn = new SqlConnection(c.ConStr())) {
                                        using (SqlDataAdapter da = new SqlDataAdapter()) {
                                            da.SelectCommand = new SqlCommand("V2_ProspectAddEdit", conn);
                                            da.SelectCommand.CommandType = CommandType.StoredProcedure;
                                            da.SelectCommand.Parameters.AddWithValue("@ProspectClientID", ProspectClientID);
                                            da.SelectCommand.Parameters.AddWithValue("@CreateByUserID", UserID);
                                            da.SelectCommand.Parameters.AddWithValue("@CreateDateClient", CreateDateClient);
                                            da.SelectCommand.Parameters.AddWithValue("@Code", Code);
                                            da.SelectCommand.Parameters.AddWithValue("@RegisterDate", RegisterDate);
                                            da.SelectCommand.Parameters.AddWithValue("@ReferByID", ReferByID);
                                            da.SelectCommand.Parameters.AddWithValue("@ReferName", ReferName);
                                            da.SelectCommand.Parameters.AddWithValue("@NameKh", NameKh);
                                            da.SelectCommand.Parameters.AddWithValue("@NameEn", NameEn);
                                            da.SelectCommand.Parameters.AddWithValue("@GenderID", GenderID);
                                            da.SelectCommand.Parameters.AddWithValue("@Age", Age);
                                            da.SelectCommand.Parameters.AddWithValue("@Phone", Phone);
                                            da.SelectCommand.Parameters.AddWithValue("@VillageID", VillageID);
                                            da.SelectCommand.Parameters.AddWithValue("@BizStatusID", BizStatusID);
                                            da.SelectCommand.Parameters.AddWithValue("@CollateralStatusID", CollateralStatusID);
                                            da.SelectCommand.Parameters.AddWithValue("@LoanEligibleID", LoanEligibleID);
                                            da.SelectCommand.Parameters.AddWithValue("@PromotionDate1", PromotionDate1);
                                            da.SelectCommand.Parameters.AddWithValue("@PromotionDate2", PromotionDate2);
                                            da.SelectCommand.Parameters.AddWithValue("@PromotionDate3", PromotionDate3);
                                            da.SelectCommand.Parameters.AddWithValue("@CustComment", CustComment);
                                            da.SelectCommand.Parameters.AddWithValue("@ExpectOnBoardDate", ExpectOnBoardDate);
                                            da.SelectCommand.Parameters.AddWithValue("@ProspectStatusID", ProspectStatusID);
                                            da.SelectCommand.Parameters.AddWithValue("@api_name", api_name);
                                            da.SelectCommand.Parameters.AddWithValue("@IsOldCust", IsOldCust);
                                            da.SelectCommand.Parameters.AddWithValue("@OccupationType", OccupationType);
                                            da.SelectCommand.Parameters.AddWithValue("@COCommends", COCommends);
                                            DataTable dt = new DataTable();
                                            da.Fill(dt);

                                            #region params
                                            DataERR = dt.Rows[0]["ERR"].ToString();
                                            DataSMS = dt.Rows[0]["SMS"].ToString();
                                            if (DataERR == "Succeed")
                                            {
                                                Code = DataSMS;
                                                DataSMS = "";
                                            }
                                            #endregion params

                                        }
                                    }

                                    #endregion
                                }
                                catch (Exception ex)
                                {
                                    DataERR = "Error";
                                    DataSMS = "Cannot add record to DB";
                                    ExSMS = ex.ToString();
                                }
                                //Con1.Close();
                            }
                            #endregion To DB
                        }
                        catch (Exception ex)
                        {
                            DataERR = "Error";
                            DataSMS = "Cannot read index " + i.ToString();
                            ExSMS = ex.ToString();
                        }
                        ProspectAddSMSRS dPro = new ProspectAddSMSRS();
                        dPro.ERR = DataERR;
                        dPro.SMS = DataSMS;
                        dPro.ProspectClientID = ProspectClientID;
                        dPro.Code = Code;
                        DataList.Add(dPro);
                    }



                    ListHeader.ProspectAddSMSRS = DataList;
                    RSData.Add(ListHeader);
                }
                #endregion data
            }
            catch (Exception ex)
            {
                ERR = "Error";
                SMS = "Something was wrong";
                ExSMS = ex.ToString();
            }
            #region if Error
            if (ERR == "Error")
            {
                ProspectAddRS ListHeader = new ProspectAddRS();
                ListHeader.ERR = ERR;
                ListHeader.SMS = SMS;
                ListHeader.ERRCode = ERRCode;
                //ListHeader.DataList = null;
                RSData.Add(ListHeader);
            }
            #endregion if Error

            try
            {                
                if (ExSMS != "") {
                    c.T24_AddLog(FileNameForLog, "RS_Error", ExSMS, ControllerName);
                }
            }
            catch { }

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

    public class ProspectAddV2RQ
    {
        public string user { get; set; } = "";
        public string pwd { get; set; } = "";
        public string device_id { get; set; } = "";
        public string app_vName { get; set; } = "";
        public string mac_address { get; set; } = "";
        public List<ProspectDataList> ProspectDataList;
    }
    public class ProspectDataList
    {
        public string ProspectClientID { get; set; }
        public string CreateDateClient { get; set; }
        public string Code { get; set; }
        public string RegisterDate { get; set; }
        public string ReferByID { get; set; }
        public string ReferName { get; set; }
        public string NameKh { get; set; }
        public string NameEn { get; set; }
        public string GenderID { get; set; }
        public string Age { get; set; }
        public string Phone { get; set; }
        public string VillageID { get; set; }
        public string BizStatusID { get; set; }
        public string CollateralStatusID { get; set; }
        public string LoanEligibleID { get; set; }
        public string PromotionDate1 { get; set; }
        public string PromotionDate2 { get; set; }
        public string PromotionDate3 { get; set; }
        public string CustComment { get; set; }
        public string ExpectOnBoardDate { get; set; }
        public string ProspectStatusID { get; set; }
        public string IsOldCust { get; set; }
        public string OccupationType { get; set; } = ""; 
        public string COCommends { get; set; } = "";
    }

    public class ProspectAddRS
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
        public string ERRCode { get; set; }
        public List<ProspectAddSMSRS> ProspectAddSMSRS;
    }
    public class ProspectAddSMSRS
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
        public string ProspectClientID { get; set; }
        public string Code { get; set; }
    }

}