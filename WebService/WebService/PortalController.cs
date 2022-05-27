using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;

namespace WebService
{
    [BasicAuthentication]
    public class PortalController : ApiController
    {
        public IEnumerable<PortalAddRS> Post([FromUri]string api_name, string api_key, string username, [FromBody]string json)
        {

            #region Variable
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "", SMSEx = "", PreAppIDReturn = "", ProspectServerID = "";
            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            List<PortalAddRS> RSData = new List<PortalAddRS>();
            string ControllerName = "PortalAddV2";
            string FileNameForLog = username + "_" + api_name + "_" + ServerDate.Replace("-", "_").Replace(" ", "_").Replace(":", "_");
            #endregion

            #region CheckAPI
            try
            {
                string[] CheckApi = c.CheckApi(api_name, api_key);
                ERR = CheckApi[0];
                SMS = CheckApi[1];
            }
            catch (Exception ex)
            {
                ERR = "Error";
                SMS = "Invalid Api";
            }
            #endregion

            #region CheckJson
            if (ERR != "Error")
            {
                if (json == null || json == "")
                {
                    ERR = "Error";
                    SMS = "Invalid JSON";
                }
            }
            #endregion

            #region GetValueFromJson
            string user = "", pwd = "", device_id = "", app_vName = "", mac_address = "", UserID = "";
            PortalAddV2RQ jObj = null;


            if (ERR != "Error")
            {
                try
                {
                    jObj = JsonConvert.DeserializeObject<PortalAddV2RQ>(json);
                    user = jObj.user;
                    user = c.Encrypt(user, c.SeekKeyGet());
                    pwd = jObj.pwd;
                    pwd = c.Encrypt(pwd, c.SeekKeyGet());
                    device_id = jObj.device_id;
                    app_vName = jObj.app_vName;
                    mac_address = jObj.mac_address;
                }
                catch (Exception ex)
                {
                    ERR = "Error";
                    SMS = "Invalid JSON";
                    SMSEx = ex.ToString();
                }
            }

            #endregion

            #region CheckUser
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
                    SMS = "invalid user";
                    SMSEx = ex.ToString();
                }
            }
            #endregion

            #region GetValueDetail

            if (ERR != "Error")
            {

                PortalAddRS ListHeader = new PortalAddRS();
                ListHeader.ERR = ERR;
                ListHeader.SMS = SMS;

                foreach (var data in jObj.PortalDataList)
                {
                    string PreAppID = data.PreAppID;
                    string CreateDateClient = data.CreateDateClient;
                    string Code = data.Code;
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

                    string partner_id = data.partner_id;
                    string Category = data.Category;
                    string Currency = data.Currency;
                    string LoanAmount = data.LoanAmount;
                    string CBCRef = data.CBCRef;

                    c.T24_AddLog(PreAppID + "-" + FileNameForLog, ControllerName, "Req" + json.ToString(), ControllerName);

                    #region Validation
                    #region ProspectClientID

                    if (ERR != "Error")
                    {
                        if (PreAppID == null)
                        {
                            ERR = "Error";
                            SMS = "Invalid ProspectClientID";
                        }
                    }
                    #endregion ProspectClientID
                    #region CreateDateClient
                    if (ERR != "Error")
                    {
                        DateTime val;
                        if (!DateTime.TryParse(CreateDateClient, out val))
                        {
                            ERR = "Error";
                            SMS = "Invalid CreateDateClient";
                        }
                    }
                    #endregion CreateDateClient
                    #region RegisterDate
                    if (ERR != "Error")
                    {
                        DateTime val;
                        if (!DateTime.TryParse(RegisterDate, out val))
                        {
                            ERR = "Error";
                            SMS = "Invalid RegisterDate";
                        }
                    }
                    #endregion RegisterDate
                    #region ReferByID
                    if (ERR != "Error")
                    {
                        try
                        {
                            if (ReferByID.Length == 0)
                            {
                                ERR = "Error";
                                SMS = "Invalid ReferByID";
                            }
                            else
                            {
                                if (!(ReferByID == "0" || ReferByID == "AMK" || ReferByID == "SAP" || ReferByID == "VBP" || ReferByID == "Pipay" || ReferByID == "CCD" || ReferByID == "BOOST" || ReferByID == "SALA" || ReferByID == "BNP"))
                                {
                                    ERR = "Error";
                                    SMS = "Invalid ReferByID";
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            ERR = "Error";
                            SMS = "Invalid ReferByID";
                        }
                    }
                    #endregion ReferByID
                    #region ReferName
                    if (ERR != "Error")
                    {
                        if (ReferByID.Length > 0)
                        {
                            if (ReferByID != "0")
                            {
                                if (ReferName.Length == 0)
                                {
                                    ERR = "Error";
                                    SMS = "Invalid ReferName";
                                }
                            }

                        }
                    }
                    #endregion ReferName
                    #region NameKh
                    if (ERR != "Error")
                    {
                        if (NameKh.Length == 0)
                        {
                            ERR = "Error";
                            SMS = "Invalid NameKh";
                        }
                    }
                    #endregion NameKh
                    #region NameEn
                    if (ERR != "Error")
                    {
                        if (NameEn.Length == 0)
                        {
                            ERR = "Error";
                            SMS = "Invalid NameEn";
                        }
                    }
                    #endregion NameEn
                    #region GenderID
                    if (ERR != "Error")
                    {
                        if (!(GenderID == "FEMALE" || GenderID == "MALE"))
                        {
                            ERR = "Error";
                            SMS = "Invalid GenderID";
                        }
                    }
                    #endregion GenderID
                    #region Age
                    if (ERR != "Error")
                    {
                        int val;
                        if (!int.TryParse(Age, out val))
                        {
                            ERR = "Error";
                            SMS = "Invalid Age";
                        }
                    }
                    if (ERR != "Error")
                    {
                        if (Convert.ToInt16(Age) < 18)
                        {
                            ERR = "Error";
                            SMS = "Invalid Age";
                        }
                        if (Convert.ToInt16(Age) > 70)
                        {
                            ERR = "Error";
                            SMS = "Invalid Age";
                        }
                    }
                    #endregion Age
                    #region Phone
                    if (ERR != "Error")
                    {
                        int val;
                        if (!int.TryParse(Phone, out val))
                        {
                            ERR = "Error";
                            SMS = "Invalid Phone";
                        }
                    }
                    if (ERR != "Error")
                    {
                        if (!(Phone.Length == 9 || Phone.Length == 10))
                        {
                            ERR = "Error";
                            SMS = "Invalid Phone";
                        }
                    }
                    #endregion Phone
                    #region VillageID
                    if (ERR != "Error")
                    {
                        if (VillageID.Length == 0)
                        {
                            ERR = "Error";
                            SMS = "Invalid VillageID";
                        }
                    }
                    #endregion VillageID
                    #region BizStatusID
                    if (ERR != "Error")
                    {
                        try
                        {
                            if (BizStatusID.Length == 0)
                            {
                                ERR = "Error";
                                SMS = "Invalid BizStatusID";
                            }
                            else
                            {
                                if (!(Convert.ToInt16(BizStatusID) == 0 || Convert.ToInt16(BizStatusID) == 1 || Convert.ToInt16(BizStatusID) == 2))
                                {
                                    ERR = "Error";
                                    SMS = "Invalid BizStatusID";
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            ERR = "Error";
                            SMS = "Invalid BizStatusID";
                        }
                    }
                    #endregion BizStatusID
                    #region CollateralStatusID
                    if (ERR != "Error")
                    {
                        try
                        {
                            if (CollateralStatusID.Length == 0)
                            {
                                ERR = "Error";
                                SMS = "Invalid CollateralStatusID";
                            }
                            else
                            {
                                if (!(Convert.ToInt16(CollateralStatusID) == 0 || Convert.ToInt16(CollateralStatusID) == 1 || Convert.ToInt16(CollateralStatusID) == 2))
                                {
                                    ERR = "Error";
                                    SMS = "Invalid CollateralStatusID";
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            ERR = "Error";
                            SMS = "Invalid CollateralStatusID";
                        }
                    }
                    #endregion CollateralStatusID
                    #region LoanEligibleID
                    if (ERR != "Error")
                    {
                        try
                        {
                            if (LoanEligibleID.Length == 0)
                            {
                                ERR = "Error";
                                SMS = "Invalid LoanEligibleID";
                            }
                            else
                            {
                                if (!(Convert.ToInt16(LoanEligibleID) == 0 || Convert.ToInt16(LoanEligibleID) == 1 || Convert.ToInt16(LoanEligibleID) == 2))
                                {
                                    ERR = "Error";
                                    SMS = "Invalid LoanEligibleID";
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            ERR = "Error";
                            SMS = "Invalid LoanEligibleID";
                        }
                    }
                    #endregion LoanEligibleID
                    #region PromotionDate1
                    if (ERR != "Error")
                    {
                        if (PromotionDate1.Length > 0)
                        {
                            DateTime val;
                            if (!DateTime.TryParse(PromotionDate1, out val))
                            {
                                ERR = "Error";
                                SMS = "Invalid PromotionDate1";
                            }

                        }
                    }
                    #endregion PromotionDate1
                    #region PromotionDate2
                    if (ERR != "Error")
                    {
                        if (PromotionDate2.Length > 0)
                        {
                            DateTime val;
                            if (!DateTime.TryParse(PromotionDate2, out val))
                            {
                                ERR = "Error";
                                SMS = "Invalid PromotionDate2";
                            }
                            else
                            {
                                try
                                {
                                    if (Convert.ToDateTime(PromotionDate2) < Convert.ToDateTime(PromotionDate1))
                                    {
                                        ERR = "Error";
                                        SMS = "Invalid PromotionDate2";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    ERR = "Error";
                                    SMS = "Invalid PromotionDate2";
                                }
                            }
                        }
                    }
                    #endregion PromotionDate2
                    #region PromotionDate3
                    if (ERR != "Error")
                    {
                        if (PromotionDate3.Length > 0)
                        {
                            DateTime val;
                            if (!DateTime.TryParse(PromotionDate3, out val))
                            {
                                ERR = "Error";
                                SMS = "Invalid PromotionDate3";
                            }
                            else
                            {
                                try
                                {
                                    if (Convert.ToDateTime(PromotionDate3) < Convert.ToDateTime(PromotionDate2))
                                    {
                                        ERR = "Error";
                                        SMS = "Invalid PromotionDate3";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    ERR = "Error";
                                    SMS = "Invalid PromotionDate3";
                                }
                            }
                        }
                    }
                    #endregion PromotionDate3
                    #region ExpectOnBoardDate
                    if (ERR != "Error")
                    {
                        if (ExpectOnBoardDate.Length > 0)
                        {
                            DateTime val;
                            if (!DateTime.TryParse(ExpectOnBoardDate, out val))
                            {
                                ERR = "Error";
                                SMS = "Invalid ExpectOnBoardDate";
                            }
                            else
                            {
                                try
                                {
                                    //if (Convert.ToDateTime(ExpectOnBoardDate) < Convert.ToDateTime(DateTime.Now))
                                    //{
                                    //    ERR = "Error";
                                    //    SMS = "Invalid ExpectOnBoardDate";
                                    //}
                                }
                                catch (Exception ex)
                                {
                                    ERR = "Error";
                                    SMS = "Invalid ExpectOnBoardDate";
                                }
                            }
                        }
                    }
                    #endregion ExpectOnBoardDate
                    #region ProspectStatusID
                    if (ERR != "Error")
                    {
                        try
                        {
                            if (ProspectStatusID.Length == 0)
                            {
                                ERR = "Error";
                                SMS = "Invalid ProspectStatusID";
                            }
                            else
                            {
                                if (!(Convert.ToInt16(ProspectStatusID) == 0 || Convert.ToInt16(ProspectStatusID) == 1 || Convert.ToInt16(ProspectStatusID) == 2 || Convert.ToInt16(ProspectStatusID) == 3))
                                {
                                    ERR = "Error";
                                    SMS = "Invalid ProspectStatusID";
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            ERR = "Error";
                            SMS = "Invalid ProspectStatusID";
                        }
                    }
                    #endregion ProspectStatusID


                    #region PartnerID
                    if (ERR != "Error")
                    {
                        try
                        {
                            if (partner_id.Length == 0)
                            {
                                ERR = "Error";
                                SMS = "Invalid PartnerID";
                            }
                        }
                        catch (Exception ex)
                        {
                            ERR = "Error";
                            SMS = "Invalid PartnerID";
                        }
                    }
                    #endregion PartnerID
                    #region Category
                    if (ERR != "Error")
                    {
                        try
                        {
                            if (Category.Length == 0)
                            {
                                ERR = "Error";
                                SMS = "Invalid Category";
                            }
                        }
                        catch (Exception ex)
                        {
                            ERR = "Error";
                            SMS = "Invalid Category";
                        }
                    }
                    #endregion Category
                    #region Currency
                    if (ERR != "Error")
                    {
                        try
                        {
                            if (Currency.Length == 0)
                            {
                                ERR = "Error";
                                SMS = "Invalid Currency";
                            }
                        }
                        catch (Exception ex)
                        {
                            ERR = "Error";
                            SMS = "Invalid Currency";
                        }
                    }
                    #endregion Currency
                    #region LoanAmount
                    if (ERR != "Error")
                    {
                        try
                        {
                            if (LoanAmount.Length == 0)
                            {
                                ERR = "Error";
                                SMS = "Invalid LoanAmount";
                            }

                            if (Convert.ToDouble(LoanAmount) <= 0)
                            {
                                ERR = "Error";
                                SMS = "Invalid LoanAmount";
                            }
                        }
                        catch (Exception ex)
                        {
                            ERR = "Error";
                            SMS = "Invalid LoanAmount";
                        }
                    }
                    #endregion LoanAmount

                    #endregion

                    #region To DB
                    if (ERR != "Error")
                    {
                        SqlConnection Con1 = new SqlConnection(c.ConStr());
                        Con1.Open();
                        SqlCommand Com1 = new SqlCommand();
                        Com1.Connection = Con1;
                        try
                        {
                            #region To DB                                    
                            string sql = "exec V2_ProspectPortalAdd '" + PreAppID + "','" + UserID
                                + "','" + CreateDateClient + "','" + Code + "','" + RegisterDate + "','" + ReferByID
                                + "',N'" + ReferName + "',N'" + NameKh + "',N'" + NameEn + "','" + GenderID + "','" + Age
                                + "','" + Phone + "','" + VillageID + "','" + BizStatusID + "','" + CollateralStatusID
                                + "','" + LoanEligibleID + "','" + PromotionDate1 + "','" + PromotionDate2 + "','" + PromotionDate3
                                + "','" + CustComment + "','" + ExpectOnBoardDate + "','" + ProspectStatusID + "','" + api_name
                                + "','" + IsOldCust + "','" + partner_id + "','" + Category + "','" + Currency + "','" + LoanAmount + "','" + CBCRef + "'";

                            DataTable dt1 = new DataTable();
                            dt1 = c.ReturnDT(sql);

                            #endregion To DB
                            #region params
                            ERR = dt1.Rows[0]["ERR"].ToString();
                            SMS = dt1.Rows[0]["SMS"].ToString();
                            PreAppIDReturn = dt1.Rows[0]["PreAppID"].ToString();
                            ProspectServerID = dt1.Rows[0]["ProspectServerID"].ToString();
                            if (ERR == "Succeed")
                            {
                                SMS = "";
                            }
                            #endregion params
                        }
                        catch (Exception ex)
                        {
                            ERR = "Error";
                            SMS = "Cannot add record to DB";
                            SMSEx = ex.ToString();
                        }
                        Con1.Close();
                    }
                    #endregion To DB
                    string respons = ERR + "_" + SMS + "_" + PreAppIDReturn + "_" + ProspectServerID;
                    c.T24_AddLog(PreAppID + "-" + FileNameForLog, ControllerName, "Res " + respons + "  " + Code, ControllerName);
                }

            }
            #endregion

            PortalAddRS dPro = new PortalAddRS();
            dPro.ERR = ERR;
            dPro.SMS = SMS;
            dPro.PreAppID = PreAppIDReturn;
            dPro.ProspectServerID = ProspectServerID;
            RSData.Add(dPro);
            return RSData;

        }
    }
}