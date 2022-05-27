using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using WebService.Models.LoanApp;
using WebService.Models.Req.Assets;
using WebService.Models.Req.Persons;
using WebService.Models.Req.CashFlows;
using WebService.Models.Req.Occupations;

namespace WebService
{
    [BasicAuthentication]
    public class LoanAppRestudyLoanAppIDGetV2Controller : ApiController
    {
        public IEnumerable<LoanAppRestudyLoanAppIDGetV2RS> Post([FromUri]string api_name, string api_key, string username, [FromBody]string json)
        {
            #region incoming
            string accountIDList = "", AgriBuddy="";
            string CreCompany = "", CustomerID = "", Currency = "";
            string CustLastName = "", CustFirstName = "",groupID="";

            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "", ExSMS = "";
            List<LoanAppRestudyLoanAppIDGetV2RS> RSData = new List<LoanAppRestudyLoanAppIDGetV2RS>();
            LoanAppRestudyLoanAppIDGetV2RS ListHeader = new LoanAppRestudyLoanAppIDGetV2RS();
            List<LoanAppV2> LoanApp = new List<LoanAppV2>();
            
            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string ServerDateForFileName = ServerDate.Replace("-", "_").Replace(" ", "_").Replace(":", "_");
            string ControllerName = "LoanAppRestudyLoanAppIDGetV2";
            string FileNameForLog = username + "_" + api_name + "_" + ServerDateForFileName;
            #endregion incoming
            try
            {
                c.T24_AddLog(FileNameForLog, "RQ", json, ControllerName);

                #region check json
                if (json == null || json == "")
                {
                    ERR = "Error";
                    SMS = "Invalid JSON";
                }
                #endregion check json
                #region json
                string user = "", pwd = "", device_id = "", app_vName = "", Criteria = "", CriteriaValue = "";
                LoanAppRestudyLoanAppIDGetV2RQ jObj = null;
                if (ERR != "Error")
                {
                    try
                    {
                        jObj = JsonConvert.DeserializeObject<LoanAppRestudyLoanAppIDGetV2RQ>(json.Replace("\r\n    ", ""));
                        user = jObj.user;
                        pwd = jObj.pwd;
                        device_id = jObj.device_id;
                        app_vName = jObj.app_vName;
                        Criteria = jObj.Criteria;
                        CriteriaValue = jObj.CriteriaValue;
                        if (Criteria != "LoanAppID")
                        {
                            ERR = "Error";
                            SMS = "Invalid Criteria";
                        }
                        else
                        {
                            if (CriteriaValue == "")
                            {
                                ERR = "Error";
                                SMS = "Invalid CriteriaValue";
                            }
                        }
                    }
                    catch
                    {
                        ERR = "Error";
                        SMS = "Invalid JSON";
                    }
                }
                #endregion json
                #region get userid
                string UserID = "";
                if (ERR != "Error")
                {
                    DataTable dt = c.ReturnDT("exec T24_check_user @user='" + user + "',@pwd='" + c.Encrypt(pwd, c.SeekKeyGet()) + "'");
                    ERR = dt.Rows[0]["ERR"].ToString();
                    SMS = dt.Rows[0]["SMS"].ToString();
                    groupID = dt.Rows[0]["GroupID"].ToString();
                    UserID = SMS;
                    SMS = "";
                }
                #endregion get userid
                #region GetData
                if (ERR != "Error")
                {
                    #region LoanApp
                    try
                    {
                        string sql = "exec T24_LoanAppRestudyGetV2_1LoanApp @LoanAppID='" + CriteriaValue + "',@UserID='" + UserID + "'";
                        DataTable dt = c.ReturnDT2(sql);
                        if (dt.Rows.Count == 0) {
                            ERR = "Error";
                            SMS = "No Data";
                        }
                        else { 
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                #region LoanApp
                                LoanAppV2 dd = new LoanAppV2();
                                dd.LoanClientID = dt.Rows[i]["LoanClientID"].ToString();
                                dd.LoanAppID = dt.Rows[i]["LoanAppID"].ToString();
                                dd.LoanAppStatusID = dt.Rows[i]["LoanAppStatusID"].ToString();
                                dd.DeviceDate = dt.Rows[i]["DeviceDate"].ToString();
                                dd.ProductID = dt.Rows[i]["ProductID"].ToString();
                                dd.LoanRequestAmount = dt.Rows[i]["LoanRequestAmount"].ToString();
                                dd.OwnCapital = dt.Rows[i]["OwnCapital"].ToString();
                                dd.DisbursementDate = dt.Rows[i]["DisbursementDate"].ToString();
                                dd.FirstWithdrawal = dt.Rows[i]["FirstWithdrawal"].ToString();
                                dd.LoanTerm = dt.Rows[i]["LoanTerm"].ToString();
                                dd.FirstRepaymentDate = dt.Rows[i]["FirstRepaymentDate"].ToString();
                                dd.LoanInterestRate = dt.Rows[i]["LoanInterestRate"].ToString();
                                dd.CustomerRequestRate = dt.Rows[i]["CustomerRequestRate"].ToString();
                                dd.CompititorRate = dt.Rows[i]["CompititorRate"].ToString();
                                dd.CustomerConditionID = dt.Rows[i]["CustomerConditionID"].ToString();
                                dd.COProposedAmount = dt.Rows[i]["COProposedAmount"].ToString();
                                dd.COProposedTerm = dt.Rows[i]["COProposedTerm"].ToString();
                                dd.COProposeRate = dt.Rows[i]["COProposeRate"].ToString();
                                dd.FrontBackOfficeID = dt.Rows[i]["FrontBackOfficeID"].ToString();
                                dd.GroupNumber = dt.Rows[i]["GroupNumber"].ToString();
                                dd.LoanCycleID = dt.Rows[i]["LoanCycleID"].ToString();
                                dd.RepaymentHistoryID = dt.Rows[i]["RepaymentHistoryID"].ToString();
                                dd.LoanReferralID = dt.Rows[i]["LoanReferralID"].ToString();
                                dd.DebtIinfoID = dt.Rows[i]["DebtIinfoID"].ToString();
                                dd.MonthlyFee = dt.Rows[i]["MonthlyFee"].ToString();
                                dd.Compulsory = dt.Rows[i]["Compulsory"].ToString();
                                dd.CompulsoryTerm = dt.Rows[i]["CompulsoryTerm"].ToString();
                                dd.Currency = dt.Rows[i]["Currency"].ToString();
                                dd.UpFrontFee = dt.Rows[i]["UpFrontFee"].ToString();
                                dd.UpFrontAmt = dt.Rows[i]["UpFrontAmt"].ToString();
                                dd.CompulsoryOptionID = dt.Rows[i]["CompulsoryOptionID"].ToString();
                                dd.FundSource = dt.Rows[i]["FundSource"].ToString();
                                dd.IsNewCollateral = dt.Rows[i]["IsNewCollateral"].ToString();
                                dd.AgriBuddy = dt.Rows[i]["AgriBuddy"].ToString();
                                dd.SemiBallonFreqID = dt.Rows[i]["SemiBallonFreqID"].ToString();
                                dd.LoanTypeID = dt.Rows[i]["LoanTypeID"].ToString();
                                dd.PaymentMethodID = dt.Rows[i]["PaymentMethodID"].ToString();
                                dd.GracePeriodID = dt.Rows[i]["GracePeriodID"].ToString();
                                dd.MITypeID = dt.Rows[i]["MITypeID"].ToString();
                                dd.DeskCheckID = dt.Rows[i]["DeskCheckID"].ToString();
                                dd.PreCheckID = dt.Rows[i]["PreCheckID"].ToString();
                                dd.CBSKey = dt.Rows[i]["CBSKey"].ToString();
                                dd.AMApproveAmt = dt.Rows[i]["AMApproveAmt"].ToString();
                                dd.CollateralDebt = dt.Rows[i]["CollateralDebt"].ToString();
                                dd.CBCRef = dt.Rows[i]["CBCRef"].ToString();

                                AgriBuddy = dt.Rows[i]["AgriBuddy"].ToString();
                                CreCompany = dt.Rows[i]["OfficeID"].ToString();
                                CustomerID = dt.Rows[i]["CustomerID"].ToString();
                                Currency = dd.Currency;
                                CustFirstName = dt.Rows[i]["FirstName"].ToString();
                                CustLastName = dt.Rows[i]["LastName"].ToString();


                                #endregion
                                #region  LoanApp Purchase Property
                                List<LoanAppPurchaseProperty> loanAppPurchasePropertyList = new List<LoanAppPurchaseProperty>();
                                var loanAppPurchaseProperty = new LoanAppPurchaseProperty();
                                try
                                {
                                    sql = " exec sp_GetLoanAppPurchaseProperty @LoanAppID = '" + CriteriaValue + "'";
                                    DataTable dt2 = c.ReturnDT2(sql);
                                    if (dt2.Rows.Count > 0)
                                    {
                                        for (int i2 = 0; i2 < dt2.Rows.Count; i2++)
                                        {

                                            #region Paramater
                                            loanAppPurchaseProperty.Id = int.Parse(dt2.Rows[i2]["ID"].ToString());
                                            loanAppPurchaseProperty.LoanAppID = int.Parse(dt2.Rows[i2]["LoanAppID"].ToString());
                                            loanAppPurchaseProperty.MainBorrowerTypeID = int.Parse(dt2.Rows[i2]["MainBorrowerStatusID"].ToString());
                                            loanAppPurchaseProperty.PchPropertyTypeID = int.Parse(dt2.Rows[i2]["PurchasedPropertyTypeID"].ToString());
                                            loanAppPurchaseProperty.TitlePchTypeID = int.Parse(dt2.Rows[i2]["TitleOfPurchasedPropertyID"].ToString());
                                            loanAppPurchaseProperty.DateConstruction = DateTime.Parse(dt2.Rows[i2]["DateConstruction"].ToString());
                                            loanAppPurchaseProperty.BuiltArea = decimal.Parse(dt2.Rows[i2]["BuiltArea"].ToString());
                                            loanAppPurchaseProperty.BuildingWith = decimal.Parse(dt2.Rows[i2]["BuildingWith"].ToString());
                                            loanAppPurchaseProperty.LandArea = decimal.Parse(dt2.Rows[i2]["LandArea"].ToString());
                                            loanAppPurchaseProperty.LandWith = decimal.Parse(dt2.Rows[i2]["LandWith"].ToString());
                                            loanAppPurchaseProperty.PricePchProperty = dt2.Rows[i2]["PricePchProperty"].ToString();
                                            loanAppPurchaseProperty.PricePchLand = dt2.Rows[i2]["PricePchLand"].ToString();
                                            loanAppPurchaseProperty.FloorNumber = int.Parse(dt2.Rows[i2]["FloorNumber"].ToString());
                                            loanAppPurchaseProperty.NumberBedRoom = int.Parse(dt2.Rows[i2]["NumberBedRoom"].ToString());
                                            loanAppPurchaseProperty.ProvinceID = dt2.Rows[i2]["ProvinceID"].ToString();
                                            loanAppPurchaseProperty.DistrictID = dt2.Rows[i2]["DistrictID"].ToString();
                                            loanAppPurchaseProperty.CommuneID = dt2.Rows[i2]["CommuneID"].ToString();
                                            loanAppPurchaseProperty.VillageID = dt2.Rows[i2]["VillageID"].ToString();
                                            loanAppPurchaseProperty.StreetNo = dt2.Rows[i2]["StreetNo"].ToString();
                                            loanAppPurchasePropertyList.Add(loanAppPurchaseProperty);
                                            #endregion
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    ERR = "Error";
                                    SMS = "Unable to get loan app purchase property";
                                    ExSMS = ex.Message.ToString();
                                }
                                #endregion
                                #region Person
                                List<PersonV2> Person = new List<PersonV2>();
                                try
                                {
                                    sql = "exec T24_LoanAppRestudyGetV2_2Person @LoanAppID='" + CriteriaValue + "'";
                                    DataTable dt2 = c.ReturnDT2(sql);
                                    for (int i2 = 0; i2 < dt2.Rows.Count; i2++)
                                    {
                                        if (ERR != "Error")
                                        {
                                            #region Person
                                            #region para
                                            string CustClientID = "";// dt2.Rows[i2]["IDOnDevice"].ToString();//old - LoanAppPersonID
                                            string T24CustID = dt2.Rows[i2]["CustomerID"].ToString();//old
                                            string CustServerID = dt2.Rows[i2]["LoanAppPersonID"].ToString();//old
                                            string ProspectCode = dt2.Rows[i2]["ProspectCode"].ToString();//old
                                            string CreateDateClient = "";// dt2.Rows[i2][""].ToString();//old - DeviceDate
                                            string LoanClientID = "";// dt2.Rows[i2][""].ToString();//old
                                            string LoanAppID = dt2.Rows[i2]["LoanAppID"].ToString();//old
                                            string LoanAppPersonTypeID = dt2.Rows[i2]["LoanAppPersonTypeID"].ToString();//old
                                            string VBID = dt2.Rows[i2]["VillageBankID"].ToString();//old
                                            string ReferByID = dt2.Rows[i2]["ReferByID"].ToString();//old
                                            string ReferName = dt2.Rows[i2]["ReferName"].ToString();//old
                                            string NameKhLast = dt2.Rows[i2]["AltName"].ToString();//old - AltName
                                            string NameKhFirst = dt2.Rows[i2]["AltName2"].ToString();//old - AltName2
                                            string TitleID = dt2.Rows[i2]["TitleID"].ToString();//old
                                            string NameEnLast = dt2.Rows[i2]["LastName"].ToString();//old
                                            string NameEnFirst = dt2.Rows[i2]["FirstName"].ToString();//old
                                            string GenderID = dt2.Rows[i2]["GenderID"].ToString();//old
                                            string DateOfBirth = dt2.Rows[i2]["DateOfBirth"].ToString();//old
                                            string IDCardTypeID = dt2.Rows[i2]["IDCardTypeID"].ToString();//old
                                            string IDCardNumber = dt2.Rows[i2]["IDCardNumber"].ToString();//old
                                            string IDCardIssueDate = dt2.Rows[i2]["IDCardIssuedDate"].ToString();//old
                                            if (IDCardIssueDate == "1900-01-01")
                                            {
                                                IDCardIssueDate = "";
                                            }

                                            string IDCardExpiryDate = dt2.Rows[i2]["IDCardExpireDate"].ToString();//old
                                            if (IDCardExpiryDate == "1900-01-01")
                                            {
                                                IDCardExpiryDate = "";
                                            }

                                            string MaritalStatusID = dt2.Rows[i2]["MaritalStatusID"].ToString();//old
                                            string EducationLevelID = dt2.Rows[i2]["EducationID"].ToString();//old
                                            string Phone = dt2.Rows[i2]["Telephone3"].ToString();//old
                                            string PlaceOfBirth = dt2.Rows[i2]["CityOfBirthID"].ToString();//old - CityOfBirthID
                                            string VillageIDPer = dt2.Rows[i2]["VillageIDPermanent"].ToString();//old - VillageIDPermanent
                                            string VillageIDCur = dt2.Rows[i2]["VillageIDCurrent"].ToString();//old - VillageIDCurrent
                                            string ShortAddress = dt2.Rows[i2]["SortAddress"].ToString();//old
                                            string FamilyMenber = dt2.Rows[i2]["FamilyMember"].ToString();//old
                                            string FamilyMenberActive = dt2.Rows[i2]["FamilyMemberActive"].ToString();//old
                                            string PoorLevelID = dt2.Rows[i2]["PoorID"].ToString();//old - PoorID
                                            string LatLon = dt2.Rows[i2]["LatLon"].ToString();//new
                                            string IsNeedSaving = dt2.Rows[i2]["IsNeedSaving"].ToString();//kyc
                                            string Nationality =  dt2.Rows[i2]["Nationality"].ToString();
                                            #endregion
                                            #region add
                                            PersonV2 person = new PersonV2();
                                            person.CustClientID = CustClientID;
                                            person.T24CustID = T24CustID;
                                            person.CustServerID = CustServerID;
                                            person.ProspectCode = ProspectCode;
                                            //person.CreateDateClient
                                            person.LoanClientID = LoanClientID;
                                            person.LoanAppID = LoanAppID;
                                            person.LoanAppPersonTypeID = LoanAppPersonTypeID;
                                            person.VBID = VBID;
                                            person.ReferByID = ReferByID;
                                            person.ReferName = ReferName;
                                            person.NameKhLast = NameKhLast;
                                            person.NameKhFirst = NameKhFirst;
                                            person.TitleID = TitleID;
                                            person.NameEnLast = NameEnLast;
                                            person.NameEnFirst = NameEnFirst;
                                            person.GenderID = GenderID;
                                            person.DateOfBirth = DateOfBirth;
                                            person.IDCardTypeID = IDCardTypeID;
                                            person.IDCardNumber = IDCardNumber;
                                            person.IDCardIssueDate = IDCardIssueDate;
                                            person.IDCardExpiryDate = IDCardExpiryDate;
                                            person.MaritalStatusID = MaritalStatusID;
                                            person.EducationLevelID = EducationLevelID;
                                            person.Phone = Phone;
                                            person.PlaceOfBirth = PlaceOfBirth;
                                            person.VillageIDPer = VillageIDPer;
                                            person.VillageIDCur = VillageIDCur;
                                            person.ShortAddress = ShortAddress;
                                            person.FamilyMember = FamilyMenber;
                                            person.FamilyMemberActive = FamilyMenberActive;
                                            person.PoorLevelID = PoorLevelID;
                                            person.LatLon = LatLon;
                                            person.IsNeedSaving = IsNeedSaving;
                                            person.Nationality = Nationality;
                                            #endregion
                                            #endregion
                                            #region PersonImg
                                            List<PersonImgV2> PersonImg = new List<PersonImgV2>();
                                            if (ERR != "Error")
                                            {
                                                try
                                                {
                                                    sql = "exec T24_LoanAppRestudyGetV2_2Person_1Img @LoanAppPersonID='" + CustServerID + "'";
                                                    DataTable dt21 = c.ReturnDT2(sql);
                                                    for (int i21 = 0; i21 < dt21.Rows.Count; i21++)
                                                    {
                                                        PersonImgV2 personImg = new PersonImgV2();
                                                        personImg.CustImageServerID = dt21.Rows[i21]["LoanAppPersonImageID"].ToString();
                                                        personImg.OneCardTwoDoc = dt21.Rows[i21]["OneCardTwoDoc"].ToString();
                                                        personImg.Ext = dt21.Rows[i21]["Ext"].ToString();
                                                        personImg.ImgPath = dt21.Rows[i21]["ImgPath"].ToString();
                                                        PersonImg.Add(personImg);
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    ERR = "Error";
                                                    SMS = "Unable to get PersonImg";
                                                    ExSMS = ex.Message.ToString();
                                                }
                                            }
                                            #endregion
                                            #region Asset
                                            List<AssetV2> Asset = new List<AssetV2>();
                                            if (ERR != "Error")
                                            {
                                                try
                                                {
                                                    if (LoanAppPersonTypeID == "31") 
                                                    {
                                                        sql = "exec T24_LoanAppRestudyGetV2_2Person_2Asset @LoanAppPersonID='" + CustServerID + "'";
                                                        DataTable dt21 = c.ReturnDT2(sql);
                                                        for (int i21 = 0; i21 < dt21.Rows.Count; i21++)
                                                        {
                                                            string AssetServerID = dt21.Rows[i21]["LoanAppClientAssetID"].ToString();
                                                            AssetV2 asset = new AssetV2();
                                                            asset.AssetServerID = AssetServerID;
                                                            asset.Description = dt21.Rows[i21]["Description"].ToString();
                                                            asset.Quantity = dt21.Rows[i21]["Quantity"].ToString();
                                                            asset.UnitPrice = dt21.Rows[i21]["UnitPrice"].ToString();
                                                            asset.AssetLookUpID = dt21.Rows[i21]["asset_lookupID"].ToString();
                                                            asset.AssetOtherDescription = dt21.Rows[i21]["assetOtherDescription"].ToString();
                                                            asset.LoanAppID = CriteriaValue;
                                                            asset.CustServerID = CustServerID;
                                                            asset.Unit = dt21.Rows[i21]["Unit"].ToString();
                                                            #region AssetImg
                                                            List<AssetImgV2> AssetImg = new List<AssetImgV2>();
                                                            if (ERR != "Error")
                                                            {
                                                                try
                                                                {
                                                                    if (ERR != "Error")
                                                                    {
                                                                        sql = "exec T24_LoanAppRestudyGetV2_2Person_21AssetImg @AssetServerID='" + AssetServerID + "'";
                                                                        DataTable dt221 = c.ReturnDT2(sql);
                                                                        string url = c.WebPathGet();
                                                                        for (int i221 = 0; i221 < dt221.Rows.Count; i221++)
                                                                        {
                                                                            AssetImgV2 assetImg = new AssetImgV2();
                                                                            assetImg.AssetServerID = AssetServerID;
                                                                            assetImg.AssetImageServerID = dt221.Rows[i221]["AssetImageServerID"].ToString();
                                                                            assetImg.Ext = dt221.Rows[i221]["Ext"].ToString();
                                                                            assetImg.ImgPath = dt221.Rows[i221]["ImgPath"].ToString();
                                                                            AssetImg.Add(assetImg);
                                                                        }
                                                                    }
                                                                }
                                                                catch (Exception ex)
                                                                {
                                                                    ERR = "Error";
                                                                    SMS = "Unable to get AssetImg";
                                                                    ExSMS = ex.Message.ToString();
                                                                }
                                                            }
                                                            #endregion
                                                            asset.AssetImg = AssetImg;
                                                            Asset.Add(asset);
                                                        }
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    ERR = "Error";
                                                    SMS = "Unable to get Asset";
                                                    ExSMS = ex.Message.ToString();
                                                }
                                            }
                                            #endregion
                                            #region Creditor
                                            List<CreditorV2> Creditor = new List<CreditorV2>();
                                            if (ERR != "Error")
                                            {

                                                if (LoanAppPersonTypeID == "31")
                                                {
                                                    sql = "exec T24_LoanAppRestudyGetV2_2Person_3Creditor @LoanAppPersonID='" + CustServerID + "'";
                                                    DataTable dt23 = c.ReturnDT2(sql);
                                                    for (int i23 = 0; i23 < dt23.Rows.Count; i23++)
                                                    {
                                                        CreditorV2 creditor = new CreditorV2();
                                                        creditor.CustServerID = CustServerID;
                                                        creditor.LoanAppID = CriteriaValue;
                                                        creditor.CreditorServerID = dt23.Rows[i23]["LoanAppCreditorID"].ToString();
                                                        creditor.CreditorID = dt23.Rows[i23]["CreditorID"].ToString();
                                                        creditor.Currency = dt23.Rows[i23]["Currency"].ToString();
                                                        creditor.ApprovedAmount = dt23.Rows[i23]["ApprovedAmount"].ToString();
                                                        creditor.OutstandingBalance = dt23.Rows[i23]["OutstandingBalance"].ToString();
                                                        creditor.InterestRate = dt23.Rows[i23]["InterestRate"].ToString();
                                                        creditor.RepaymentTypeID = dt23.Rows[i23]["RepaymentTypeID"].ToString();
                                                        creditor.RepaymentTermID = dt23.Rows[i23]["RepaymentTermID"].ToString();
                                                        creditor.LoanStartDate = Convert.ToDateTime(dt23.Rows[i23]["LoanStartDate"]).ToString("yyyy-MM-dd");
                                                        creditor.LoanEndDate = Convert.ToDateTime(dt23.Rows[i23]["LoanEndDate"]).ToString("yyyy-MM-dd");
                                                        creditor.IsReFinance = dt23.Rows[i23]["IsReFinance"].ToString();
                                                        creditor.ReFinanceAmount = dt23.Rows[i23]["ReFinanceAmount"].ToString();
                                                        creditor.RemainingInstallment = dt23.Rows[i23]["RemainingInstallment"].ToString();
                                                        creditor.CreditorName = dt23.Rows[i23]["CreditorName"].ToString();
                                                        Creditor.Add(creditor);
                                                    }
                                                }
                                            }
                                            #endregion
                                            #region RealEstate
                                            List<PersonRealEstateV2> RealEstate = new List<PersonRealEstateV2>();
                                            if (ERR != "Error")
                                            {

                                                if (LoanAppPersonTypeID == "31")
                                                {
                                                    sql = "exec T24_LoanAppRestudyGetV2_2Person_4RealEstate @LoanAppPersonID='" + CustServerID + "'";
                                                    DataTable dt24 = c.ReturnDT2(sql);
                                                    for (int i24 = 0; i24 < dt24.Rows.Count; i24++)
                                                    {
                                                        #region para
                                                        PersonRealEstateV2 realEstate = new PersonRealEstateV2();
                                                        string CollateralServerID = dt24.Rows[i24]["CollateralServerID"].ToString();
                                                        realEstate.CustServerID = CustServerID;
                                                        realEstate.LoanAppID = CriteriaValue;
                                                        realEstate.CollateralServerID = CollateralServerID;
                                                        realEstate.CollateralDocGroupTypeID = dt24.Rows[i24]["CollateralDocGroupTypeID"].ToString();
                                                        realEstate.CollateralDocHardTypeID = dt24.Rows[i24]["CollateralDocHardTypeID"].ToString();
                                                        realEstate.CollateralDocSoftTypeID = dt24.Rows[i24]["CollateralDocSoftTypeID"].ToString();
                                                        realEstate.CollateralOwnerTypeID = dt24.Rows[i24]["CollateralOwnerTypeID"].ToString();
                                                        realEstate.CollateralLocationVillageID = dt24.Rows[i24]["CollateralLocationVillageID"].ToString();
                                                        realEstate.CollateralRoadAccessibilityID = dt24.Rows[i24]["CollateralRoadAccessibilityID"].ToString();
                                                        realEstate.PropertyTypeID = dt24.Rows[i24]["PropertyTypeID"].ToString();
                                                        realEstate.LandTypeID = dt24.Rows[i24]["LandTypeID"].ToString();
                                                        realEstate.LandSize = dt24.Rows[i24]["LandSize"].ToString();
                                                        realEstate.LandMarketPrice = dt24.Rows[i24]["LandMarketPrice"].ToString();
                                                        realEstate.LandForcedSalePrice = dt24.Rows[i24]["LandForcedSalePrice"].ToString();
                                                        realEstate.BuildingTypeID = dt24.Rows[i24]["BuildingTypeID"].ToString();
                                                        realEstate.BuildingSize = dt24.Rows[i24]["BuildingSize"].ToString();
                                                        realEstate.BuildingMarketPrice = dt24.Rows[i24]["BuildingMarketPrice"].ToString();
                                                        realEstate.BuildingForcedSalesPrice = dt24.Rows[i24]["BuildingForcedSalesPrice"].ToString();
                                                        #endregion
                                                        #region img
                                                        List<PersonRealEstateImgV2> RealEstateImg = new List<PersonRealEstateImgV2>();
                                                        sql = "exec T24_LoanAppRestudyGetV2_2Person_41RealEstateImg @CollateralServerID='" + CollateralServerID + "'";
                                                        DataTable dt241 = c.ReturnDT2(sql);
                                                        for (int i241 = 0; i241 < dt241.Rows.Count; i241++)
                                                        {
                                                            PersonRealEstateImgV2 personRealEstateImgV2 = new PersonRealEstateImgV2();
                                                            personRealEstateImgV2.CollateralServerID = CollateralServerID;
                                                            personRealEstateImgV2.ImageServerID = dt241.Rows[i241]["ImageServerID"].ToString();
                                                            personRealEstateImgV2.Ext = dt241.Rows[i241]["Ext"].ToString();
                                                            personRealEstateImgV2.ImgPath = dt241.Rows[i241]["ImgPath"].ToString();
                                                            RealEstateImg.Add(personRealEstateImgV2);
                                                        }
                                                        #endregion
                                                        realEstate.RealEstateImg = RealEstateImg;
                                                        RealEstate.Add(realEstate);
                                                    }
                                                }
                                            }
                                            #endregion
                                            #region PersonDeposit
                                            List<PersonDepositV2> PersonDeposit = new List<PersonDepositV2>();
                                            if (ERR != "Error")
                                            {
                                                if (LoanAppPersonTypeID == "31")
                                                {
                                                    sql = "exec T24_LoanAppRestudyGetV2_2Person_5Deposit @LoanAppPersonID='" + CustServerID + "'";
                                                    DataTable dt25 = c.ReturnDT2(sql);
                                                    for (int i25 = 0; i25 < dt25.Rows.Count; i25++)
                                                    {
                                                        PersonDepositV2 personDeposit = new PersonDepositV2();
                                                        personDeposit.CustServerID = CustServerID;
                                                        personDeposit.LoanAppID = CriteriaValue;
                                                        personDeposit.CollateralServerID = dt25.Rows[i25]["CollateralServerID"].ToString();
                                                        personDeposit.FixedDepositAccountNo = dt25.Rows[i25]["FixedDepositAccountNo"].ToString();

                                                        string personDepositStartDate = Convert.ToDateTime(dt25.Rows[i25]["StartDate"]).ToString("yyyy-MM-dd");
                                                        if (personDepositStartDate == "1900-01-01")
                                                        {
                                                            personDepositStartDate = "";
                                                        }
                                                        personDeposit.StartDate = personDepositStartDate;

                                                        string personDepositMaturityDate = Convert.ToDateTime(dt25.Rows[i25]["MaturityDate"]).ToString("yyyy-MM-dd");
                                                        if (personDepositMaturityDate == "1900-01-01")
                                                        {
                                                            personDepositMaturityDate = "";
                                                        }
                                                        personDeposit.MaturityDate = personDepositMaturityDate;

                                                        personDeposit.Amount = dt25.Rows[i25]["Amount"].ToString();
                                                        personDeposit.AccountOwnerName = dt25.Rows[i25]["AccountOwnerName"].ToString();
                                                        personDeposit.Currency = dt25.Rows[i25]["Currency"].ToString();
                                                        personDeposit.RelationshipID = dt25.Rows[i25]["RelationshipID"].ToString();

                                                        string personDepositDOB = Convert.ToDateTime(dt25.Rows[i25]["DOB"]).ToString("yyyy-MM-dd");
                                                        if (personDepositDOB == "1900-01-01")
                                                        {
                                                            personDepositDOB = "";
                                                        }
                                                        personDeposit.DOB = personDepositDOB;

                                                        personDeposit.GenderID = dt25.Rows[i25]["GenderID"].ToString();
                                                        personDeposit.NIDNo = dt25.Rows[i25]["NIDNo"].ToString();

                                                        string personDepositIssueDate = Convert.ToDateTime(dt25.Rows[i25]["IssueDate"]).ToString("yyyy-MM-dd");
                                                        if (personDepositIssueDate == "1900-01-01")
                                                        {
                                                            personDepositIssueDate = "";
                                                        }
                                                        personDeposit.IssueDate = personDepositIssueDate;

                                                        personDeposit.IssuedBy = dt25.Rows[i25]["IssuedBy"].ToString();

                                                        personDeposit.SortAddress = dt25.Rows[i25]["SortAddress"].ToString();
                                                        personDeposit.VillageID = dt25.Rows[i25]["VillageID"].ToString();
                                                        PersonDeposit.Add(personDeposit);
                                                    }
                                                }
                                            }
                                            #endregion
                                            #region PersonKYC
                                            List<PersonKYC> PersonKYC = new List<PersonKYC>();
                                            if (ERR != "Error")
                                            {
                                                sql = "exec sp_LoanAppPersonKYCGet @LoanAppPersonID='" + CustServerID + "'";
                                                DataTable dt24 = c.ReturnDT2(sql);
                                                for (int i24 = 0; i24 < dt24.Rows.Count; i24++)
                                                {
                                                    #region para
                                                    PersonKYC personKYC = new PersonKYC();
                                                    string KYCServerID = dt24.Rows[i24]["KYCServerID"].ToString();
                                                    personKYC.KYCServerID = KYCServerID;
                                                    personKYC.KYCClientID = dt24.Rows[i24]["KYCClientID"].ToString();
                                                    personKYC.IDCardType = dt24.Rows[i24]["IDCardType"].ToString();
                                                    personKYC.IDNumber = dt24.Rows[i24]["IDNumber"].ToString();
                                                    personKYC.IDIssueDate = dt24.Rows[i24]["IDIssueDate"].ToString();
                                                    personKYC.IDExpireDate = dt24.Rows[i24]["IDExpireDate"].ToString();
                                                    personKYC.IsCurrentKYC = dt24.Rows[i24]["IsCurrentKYC"].ToString();
                                                    #endregion
                                                    #region img
                                                    List<PersonKYCImage> PersonKYCImage = new List<PersonKYCImage>();
                                                    sql = "exec sp_LoanAppPersonKYCImgGet @KYCServerID='" + KYCServerID + "'";
                                                    DataTable dt241 = c.ReturnDT2(sql);
                                                    for (int i241 = 0; i241 < dt241.Rows.Count; i241++)
                                                    {
                                                        PersonKYCImage personKYCImage = new PersonKYCImage();
                                                        personKYCImage.KYCImageServerID = dt241.Rows[i241]["ImageServerID"].ToString();
                                                        personKYCImage.KYCImageClientID = "0";
                                                        personKYCImage.Ext = dt241.Rows[i241]["Ext"].ToString();
                                                        personKYCImage.ImgPath = dt241.Rows[i241]["ImgPath"].ToString();
                                                        PersonKYCImage.Add(personKYCImage);
                                                    }
                                                    #endregion
                                                    personKYC.PersonKYCImage = PersonKYCImage;
                                                    PersonKYC.Add(personKYC);
                                                }
                                            }
                                            #endregion
                                            #region PersonOccupation
                                            var PersonOccList = new List<PersonOccupation>();
                                            if (ERR != "Error")
                                            {
                                                sql = "exec sp_LoanAppPersonGetOccupation @LoanAppPersonID='" + CustServerID + "'";
                                                DataTable dt24 = c.ReturnDT2(sql);
                                                for (int i24 = 0; i24 < dt24.Rows.Count; i24++)
                                                {
                                                    #region para
                                                    PersonOccupation PersonOcc = new PersonOccupation();
                                                    PersonOcc.Id = int.Parse(dt24.Rows[i24]["ID"].ToString());
                                                    PersonOcc.OccupationId = dt24.Rows[i24]["OccupationID"].ToString();
                                                    PersonOcc.LoanAppPersonId = dt24.Rows[i24]["LoanAppPersonID"].ToString();
                                                    PersonOcc.OccupationName = dt24.Rows[i24]["OccupationName"].ToString();
                                                    #endregion
                                                    #region PersonOccupationDetail
                                                   var personOccDetailList = new List<PersonOccupationDetail>();
                                                    sql = "exec sp_LoanAppPersonGetOccupationDetail @PersonOccupationId='" + PersonOcc.Id + "'";
                                                    DataTable dt241 = c.ReturnDT2(sql);
                                                    for (int i241 = 0; i241 < dt241.Rows.Count; i241++)
                                                    {
                                                        PersonOccupationDetail personOccDetail = new PersonOccupationDetail();
                                                        personOccDetail.Id = int.Parse(dt241.Rows[i241]["ID"].ToString());
                                                        personOccDetail.OccupationDetailId = int.Parse(dt241.Rows[i241]["OccupationDetailID"].ToString());
                                                        personOccDetail.PersonOccDetailName = dt241.Rows[i241]["OccupationDetail"].ToString();
                                                        personOccDetailList.Add(personOccDetail);
                                                    }
                                                    #endregion
                                                    PersonOcc.PersonOccupationDetials = personOccDetailList;
                                                    PersonOccList.Add(PersonOcc);
                                                }
                                            }
                                            #endregion
                                            person.PersonImg = PersonImg;
                                            person.Asset = Asset;
                                            person.Creditor = Creditor;
                                            person.RealEstate = RealEstate;
                                            person.PersonDeposit = PersonDeposit;
                                            person.PersonKYC = PersonKYC;
                                            person.LoanOccupation = PersonOccList;
                                            Person.Add(person);
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    ERR = "Error";
                                    SMS = "Unable to get Person";
                                    ExSMS = ex.Message.ToString();
                                }
                                #endregion
                                #region Purpose
                                List<PurposeV2> Purpose = new List<PurposeV2>();
                                if (ERR != "Error")
                                {
                                    try
                                    {
                                        sql = "exec T24_LoanAppRestudyGetV2_3Purpose @LoanAppID='" + CriteriaValue + "'";
                                        DataTable dt3 = c.ReturnDT2(sql);
                                        for (int i3 = 0; i3 < dt3.Rows.Count; i3++)
                                        {
                                            if (ERR != "Error")
                                            {
                                                #region Purpose
                                                PurposeV2 purpose = new PurposeV2();
                                                string LoanPurposeServerID = dt3.Rows[i3]["LoanPurposeServerID"].ToString();
                                                //purpose.LoanPurposeClientID = dt3.Rows[i3]["LoanPurposeClientID"].ToString();
                                                purpose.LoanPurposeServerID = LoanPurposeServerID;
                                                //purpose.LoanClientID = dt3.Rows[i3]["LoanPurposeClientID"].ToString();
                                                //purpose.LoanAppID = dt3.Rows[i3]["LoanAppID"].ToString();
                                                purpose.LoanPurposeID = dt3.Rows[i3]["LoanPurposeID"].ToString();
                                                purpose.LoanAppID = CriteriaValue;
                                                #endregion
                                                #region PurposeDetail
                                                List<PurposeDetailV2> PurposeDetail = new List<PurposeDetailV2>();
                                                sql = "exec T24_LoanAppRestudyGetV2_31PurposeDetail @LoanPurposeServerID='" + LoanPurposeServerID + "'";
                                                DataTable dt31 = c.ReturnDT2(sql);
                                                for (int i31 = 0; i31 < dt31.Rows.Count; i31++)
                                                {
                                                    PurposeDetailV2 purposeDetail = new PurposeDetailV2();
                                                    purposeDetail.LoanPurposeServerID = LoanPurposeServerID;
                                                    purposeDetail.LoanPurposeDetailServerID = dt31.Rows[i31]["LoanAppPurpsoeDetailID"].ToString();
                                                    purposeDetail.LoanAppPurpsoeDetail = dt31.Rows[i31]["LoanAppPurpsoeDetail"].ToString();
                                                    purposeDetail.Quantity = dt31.Rows[i31]["Quantity"].ToString();
                                                    purposeDetail.UnitPrice = dt31.Rows[i31]["UnitPrice"].ToString();
                                                    PurposeDetail.Add(purposeDetail);
                                                }
                                                #endregion
                                                purpose.PurposeDetail = PurposeDetail;
                                                Purpose.Add(purpose);
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        ERR = "Error";
                                        SMS = "Unable to get Purpose";
                                        ExSMS = ex.Message.ToString();
                                    }
                                }
                                #endregion
                                #region CashFlow
                                List<CashFlowV2> CashFlow = new List<CashFlowV2>();
                                if (ERR != "Error")
                                {
                                    try
                                    {
                                        sql = "exec T24_LoanAppRestudyGetV2_4CashFlow @LoanAppID='" + CriteriaValue + "'";
                                        DataTable dt4 = c.ReturnDT2(sql);
                                        for (int i4 = 0; i4 < dt4.Rows.Count; i4++)
                                        {
                                            if (ERR != "Error")
                                            {
                                                #region CashFlow
                                                CashFlowV2 cashFlow = new CashFlowV2();
                                                string CashFlowServerID = dt4.Rows[i4]["LoanAppCashFlowID"].ToString();
                                                cashFlow.LoanAppID = CriteriaValue;
                                                cashFlow.CashFlowServerID = CashFlowServerID;
                                                cashFlow.StudyMonthAmount = dt4.Rows[i4]["StudyMonthAmount"].ToString();
                                                cashFlow.StudyStartMonth = dt4.Rows[i4]["StudyStartMonth"].ToString();
                                                cashFlow.FamilyExpensePerMonth = dt4.Rows[i4]["FamilyExpensePerMonth"].ToString();
                                                cashFlow.OtherExpensePerMonth = dt4.Rows[i4]["OtherExpensePerMonth"].ToString();
                                                #endregion
                                                #region MSI
                                                List<CashFlowMSIV2> CashFlowMSI = new List<CashFlowMSIV2>();
                                                sql = "exec T24_LoanAppRestudyGetV2_41CashFlowMSI @CashFlowServerID='" + CashFlowServerID + "'";
                                                DataTable dt41 = c.ReturnDT2(sql);
                                                for (int i41 = 0; i41 < dt41.Rows.Count; i41++)
                                                {
                                                    CashFlowMSIV2 cashFlowMSI = new CashFlowMSIV2();
                                                    string LoanAppCashFlowMSIID = dt41.Rows[i41]["LoanAppCashFlowMSIID"].ToString();
                                                    cashFlowMSI.CashFlowMSIServerID = LoanAppCashFlowMSIID;
                                                    cashFlowMSI.IncomeTypeID = dt41.Rows[i41]["IncomeTypeID"].ToString();
                                                    cashFlowMSI.OccupationTypeID = dt41.Rows[i41]["OccupationTypeID"].ToString();
                                                    cashFlowMSI.MainSourceIncomeID = dt41.Rows[i41]["MainSourceIncomeID"].ToString();
                                                    cashFlowMSI.ExAge = dt41.Rows[i41]["ExAge"].ToString();
                                                    cashFlowMSI.BusAge = dt41.Rows[i41]["BusAge"].ToString();
                                                    cashFlowMSI.isMSI = dt41.Rows[i41]["isMSI"].ToString();
                                                    cashFlowMSI.CashFlowServerID = CashFlowServerID;
                                                    cashFlowMSI.IncomeOwnerID = dt41.Rows[i41]["IncomeOwnerID"].ToString();
                                                    cashFlowMSI.WorkingPlaceID = dt41.Rows[i41]["WorkingPlaceID"].ToString();
                                                    cashFlowMSI.PhoneNumber = dt41.Rows[i41]["PhoneNumber"].ToString();
                                                    cashFlowMSI.Name = dt41.Rows[i41]["Name"].ToString();
                                                    #region InEx
                                                    List<CashFlowMSIInExV2> CashFlowMSIInEx = new List<CashFlowMSIInExV2>();
                                                    sql = "exec T24_LoanAppRestudyGetV2_42CashFlowInEx @LoanAppCashFlowMSIID='" + LoanAppCashFlowMSIID + "'";
                                                    DataTable dt42 = c.ReturnDT2(sql);
                                                    for (int i42 = 0; i42 < dt42.Rows.Count; i42++)
                                                    {
                                                        CashFlowMSIInExV2 cashFlowMSIInEx = new CashFlowMSIInExV2();
                                                        cashFlowMSIInEx.CashFlowMSIServerID = LoanAppCashFlowMSIID;
                                                        cashFlowMSIInEx.LoanAppCashFlowMSIInExServerID = dt42.Rows[i42]["LoanAppCashFlowMSIInExID"].ToString();
                                                        cashFlowMSIInEx.InExCodeID = dt42.Rows[i42]["InExCodeID"].ToString();
                                                        cashFlowMSIInEx.Description = dt42.Rows[i42]["Description"].ToString();
                                                        cashFlowMSIInEx.Month = dt42.Rows[i42]["Month"].ToString();
                                                        cashFlowMSIInEx.Amount = dt42.Rows[i42]["Amount"].ToString();
                                                        cashFlowMSIInEx.UnitID = dt42.Rows[i42]["UnitID"].ToString();
                                                        cashFlowMSIInEx.Cost = dt42.Rows[i42]["Cost"].ToString();
                                                        cashFlowMSIInEx.OneIncomeTwoExpense = dt42.Rows[i42]["OneIncomeTwoExpense"].ToString();
                                                        CashFlowMSIInEx.Add(cashFlowMSIInEx);
                                                    }
                                                    #endregion
                                                    cashFlowMSI.CashFlowMSIInEx = CashFlowMSIInEx;
                                                    CashFlowMSI.Add(cashFlowMSI);
                                                }
                                                #endregion
                                                cashFlow.CashFlowMSI = CashFlowMSI;
                                                CashFlow.Add(cashFlow);
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        ERR = "Error";
                                        SMS = "Unable to get CashFlow";
                                        ExSMS = ex.Message.ToString();
                                    }
                                }
                                #endregion

                                dd.Person = Person;
                                dd.Purpose = Purpose;
                                dd.CashFlow = CashFlow;
                                dd.LoanAppPurchaseProperties = loanAppPurchasePropertyList;
                                LoanApp.Add(dd);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ERR = "Error";
                        SMS = "Unable to get LoanApp";
                        ExSMS = ex.Message.ToString();
                    }
                    #endregion
                }
                #endregion

                if (ERR != "Error")
                {
                    if (groupID == "4" || groupID == "10")
                    {
                        #region AccountID
                        if (ERR != "Error")
                        {
                            try
                            {

                                if (AgriBuddy == "" || AgriBuddy == "1" || AgriBuddy == "&nbsp;" || AgriBuddy == "0")
                                {
                                    #region get AccountID
                                    try
                                    {

                                        string jsonWS = "[{\"user\":\"none\",\"pwd\":\"none\",\"device_id\":\"none\",\"app_vName\":\"none\""
                                        + ",\"CreCompany\":\"" + CreCompany + "\""
                                        + ",\"CustomerID\":\"" + CustomerID + "\""
                                        //+ ",\"CATEGORY\":\"6001\""
                                        + ",\"CATEGORY\":\"6012\""
                                        + ",\"ACCOUNTTITLE1\":\"None\""
                                        + ",\"SHORTTITLE\":\"None\""
                                        + ",\"LoanCurrency\":\"" + Currency + "\"}]";
                                        string url = c.GetTabletWSUrl() + "/api/AccountGetFromCBS?api_name=" + c.GetTabletWSAPIName() + "&api_key=" + c.GetTabletWSAPIKey() + "&type=json&json=" + jsonWS;
                                        var Authenticator = new HttpBasicAuthenticator(c.GetTabletWSUser(), c.GetTabletWSPwd());
                                        var client = new RestClient(url);
                                        client.Authenticator = Authenticator;
                                        var request = new RestRequest(Method.GET);
                                        IRestResponse response = client.Execute(request);
                                        string jres = response.Content.ToString().TrimEnd(']').TrimStart('[');
                                        AccountCreationToCBSModel jObjGETAccount = JsonConvert.DeserializeObject<AccountCreationToCBSModel>(jres);
                                        ERR = jObjGETAccount.ERR;
                                        if (ERR == "Error")
                                        {
                                            //err = 0;
                                            //sms = jObj.SMS;
                                        }
                                        else
                                        {
                                            foreach (var ai in jObjGETAccount.DataList)
                                            {
                                                accountIDList = accountIDList + ai.AccountID + ",";
                                            }
                                        }
                                    }
                                    catch
                                    {

                                    }
                                    #endregion get AccountID

                                    #region add account
                                    if (accountIDList.Length <= 2)
                                    {
                                        try
                                        {
                                            string jsonWS = "[{\"user\":\"none\",\"pwd\":\"none\",\"device_id\":\"none\",\"app_vName\":\"none\""
                                            + ",\"CreCompany\":\"" + CreCompany + "\""
                                            + ",\"CustomerID\":\"" + CustomerID + "\""
                                            //+ ",\"CATEGORY\":\"6001\""
                                            + ",\"CATEGORY\":\"6012\""
                                            + ",\"ACCOUNTTITLE1\":\"" + CustLastName + " " + CustFirstName + "\""
                                            + ",\"SHORTTITLE\":\"" + CustLastName + " " + CustFirstName + "\""
                                            + ",\"LoanCurrency\":\"" + Currency + "\"}]";
                                            string url = c.GetTabletWSUrl() + "/api/AccountCreationToCBS?api_name=" + c.GetTabletWSAPIName() + "&api_key=" + c.GetTabletWSAPIKey() + "&type=json&json=" + jsonWS;
                                            var Authenticator = new HttpBasicAuthenticator(c.GetTabletWSUser(), c.GetTabletWSPwd());
                                            var client = new RestClient(url);
                                            client.Authenticator = Authenticator;
                                            var request = new RestRequest(Method.POST);
                                            IRestResponse response = client.Execute(request);
                                            string jres = response.Content.ToString().TrimEnd(']').TrimStart('[');
                                            AccountCreationToCBSModel jObjAddAccount = JsonConvert.DeserializeObject<AccountCreationToCBSModel>(jres);
                                            ERR = jObjAddAccount.ERR;
                                            if (ERR == "Error")
                                            {
                                                SMS = jObjAddAccount.SMS;
                                            }
                                        }
                                        catch
                                        {
                                            ERR = "Error";
                                            SMS = "Error while create account";
                                        }
                                    }
                                    #endregion add account

                                    #region get AccountID
                                    if (accountIDList.Length <= 2)
                                    {
                                        try
                                        {

                                            string jsonWS = "[{\"user\":\"none\",\"pwd\":\"none\",\"device_id\":\"none\",\"app_vName\":\"none\""
                                            + ",\"CreCompany\":\"" + CreCompany + "\""
                                            + ",\"CustomerID\":\"" + CustomerID + "\""
                                            //+ ",\"CATEGORY\":\"6001\""
                                            + ",\"CATEGORY\":\"6012\""
                                            + ",\"ACCOUNTTITLE1\":\"None\""
                                            + ",\"SHORTTITLE\":\"None\""
                                            + ",\"LoanCurrency\":\"" + Currency + "\"}]";
                                            string url = c.GetTabletWSUrl() + "/api/AccountGetFromCBS?api_name=" + c.GetTabletWSAPIName() + "&api_key=" + c.GetTabletWSAPIKey() + "&type=json&json=" + jsonWS;
                                            var Authenticator = new HttpBasicAuthenticator(c.GetTabletWSUser(), c.GetTabletWSPwd());
                                            var client = new RestClient(url);
                                            client.Authenticator = Authenticator;
                                            var request = new RestRequest(Method.GET);
                                            IRestResponse response = client.Execute(request);
                                            string jres = response.Content.ToString().TrimEnd(']').TrimStart('[');
                                            AccountCreationToCBSModel jObjGETAccount = JsonConvert.DeserializeObject<AccountCreationToCBSModel>(jres);
                                            ERR = jObjGETAccount.ERR;
                                            if (ERR == "Error")
                                            {
                                                //err = 0;
                                                //sms = jObj.SMS;
                                            }
                                            else
                                            {
                                                foreach (var ai in jObjGETAccount.DataList)
                                                {
                                                    accountIDList = accountIDList + ai.AccountID + ",";
                                                }
                                            }
                                        }
                                        catch
                                        {
                                            ERR = "Error";
                                            SMS = "Error while get account";
                                        }
                                    }

                                    #endregion get AccountID

                                }
                                else
                                {
                                    string sql = "select o.AgriBuddySettlementAcc as AccountID from T24_Office o where o.isAgriBuddy=1 and o.OfficeID in(select u.OfficeID from tblUser u where u.UserID in (select l.CreateBy from tblLoanApp1 l where l.LoanAppID='" + CriteriaValue + "'))";
                                    DataTable dtAgri = c.ReturnDT2(sql);
                                    if (dtAgri.Rows.Count == 0)
                                    {

                                    }
                                    else
                                    {
                                        for (int i = 0; i <= dtAgri.Rows.Count - 1; i++)
                                        {
                                            accountIDList = accountIDList + dtAgri.Rows[i]["AccountID"] + ",";
                                        }
                                    }
                                }
                            }
                            catch { }
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                ERR = "Error";
                SMS = "Something was wrong";// at line:" + c.GetLineNumber(ex) + " | Ex:" + ex.Message.ToString();
                ExSMS = ex.Message.ToString();
            }
            #region return
            ListHeader.ERR = ERR;
            ListHeader.SMS = SMS;
            ListHeader.LoanApp = LoanApp;
            ListHeader.AccountID = accountIDList;
            RSData.Add(ListHeader);
            try
            {
                var jsonRS = new JavaScriptSerializer().Serialize(RSData);
                c.T24_AddLog(FileNameForLog, "RS", jsonRS.ToString(), ControllerName);
            }
            catch { }
            return RSData;
            #endregion return
        }
    }
    public class LoanAppRestudyLoanAppIDGetV2RQ
    {
        public string user { get; set; }
        public string pwd { get; set; }
        public string device_id { get; set; }
        public string app_vName { get; set; }
        public string Criteria { get; set; }
        public string CriteriaValue { get; set; }
    }
    public class LoanAppRestudyLoanAppIDGetV2RS
    {
        public string ERR { get; set; }
        public string SMS { get; set; }

        public List<LoanAppV2> LoanApp;
        public string AccountID { get; set; }
    }
    

}