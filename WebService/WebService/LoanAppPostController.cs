using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace WebService
{
    public class LoanAppPostController : ApiController
    {
        
        // POST api/<controller>
        public IEnumerable<LoanAppResModel> Post([FromUri]string api_name,string api_key,string json)
        {
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "", RSIDOnDevice="", RSLoanAppID="";
            List<LoanAppResModel> RSData = new List<LoanAppResModel>();
            LoanAppResModel ListHeader = new LoanAppResModel();
            List<LoanAppResSMS> SMSList = new List<LoanAppResSMS>();
            List<LoanAppResImgList> ImgList = new List<LoanAppResImgList>();
            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            try
            {
                
                //Add log
                c.T24_AddLog("LoanAppPost_" + api_name + "_" + ServerDate.Replace("-","_").Replace(" ", "_").Replace(":", "_"), "RQ", json, "LoanAppPost");

                #region check json
                if (json == null || json == "")
                {
                    ERR = "Error";
                    SMS = "Invalid JSON";
                    LoanAppResSMS data = new LoanAppResSMS();
                    data.SMS = SMS;
                    SMSList.Add(data);
                }
                #endregion check json
                string user = "", pwd = "",InstID="";
                #region check json to get user/pwd
                //if (ERR != "Error")
                //{
                //    try
                //    {
                //        var objects = JArray.Parse(json);
                //        foreach (JObject root in objects)
                //        {
                //            foreach (var item in root)
                //            {
                //                string key = item.Key;
                //                if (key == "user")
                //                {
                //                    user = item.Value.ToString();
                //                }
                //                if (key == "pwd")
                //                {
                //                    pwd = item.Value.ToString();
                //                }
                //                //nested LoanApp
                //                if (key == "LoanApp")
                //                {


                //                    //var LoanItem = item.Value;
                //                    //var j1 = JArray.Parse(LoanItem.ToString());
                //                    //foreach (JObject root_j1 in j1) {
                //                    //    foreach (var li in root_j1) {
                //                    //        string li_key = li.Key;
                //                    //        //read LoanApp

                //                    //        //nested PurpsoeDetail
                //                    //        if (li_key == "PurpsoeDetail") {
                //                    //            var li2Item = li.Value;

                //                    //        }
                //                    //        //nested Person
                //                    //        if (li_key == "Person")
                //                    //        {
                //                    //            var li2Item = li.Value;
                //                    //            //read Person

                //                    //            //nested Person

                //                    //        }
                //                    //        //nested Creditor
                //                    //        if (li_key == "Creditor")
                //                    //        {
                //                    //            var li2Item = li.Value;

                //                    //        }
                //                    //        //nested Opinion
                //                    //        if (li_key == "Opinion")
                //                    //        {
                //                    //            var li2Item = li.Value;

                //                    //        }
                //                    //        //nested CashFlow
                //                    //        if (li_key == "CashFlow")
                //                    //        {
                //                    //            var li2Item = li.Value;

                //                    //        }


                //                    //    }
                //                    //}
                //                }

                //            }
                //        }
                //    }
                //    catch(Exception ex)
                //    {
                //        ERR = "Error";
                //        SMS = "Invalid JSON";
                //    }
                //}
                #endregion check json to get user/pwd

                LoanAppModel jObj = null;
                #region json
                if (ERR != "Error")
                {
                    try
                    {
                        jObj = JsonConvert.DeserializeObject<LoanAppModel>(json);
                        user = jObj.user;
                        pwd = jObj.pwd;
                        InstID = jObj.app_vName;
                    }
                    catch
                    {
                        ERR = "Error";
                        SMS = "Invalid JSON";
                        LoanAppResSMS data = new LoanAppResSMS();
                        data.SMS = SMS;
                        SMSList.Add(data);
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
                    UserID = SMS;
                    if (ERR == "Error")
                    {
                        LoanAppResSMS data = new LoanAppResSMS();
                        data.SMS = SMS;
                        SMSList.Add(data);
                    }
                }
                #endregion get userid
                #region LoanApp
                if (ERR != "Error")
                {
                    
                    #region checking
                    //try
                    //{
                    //    foreach (var loan in jObj.LoanApp)
                    //    {
                    //        #region LoanApp                            
                    //        string IDOnDevice = loan.IDOnDevice;
                    //        RSIDOnDevice = IDOnDevice;
                    //        string LoanAppID = loan.LoanAppID;
                    //        RSLoanAppID = LoanAppID;
                    //        string LoanAppStatusID = loan.LoanAppStatusID;
                    //        if (LoanAppStatusID == "3")
                    //        {

                    //        }
                    //        else if (LoanAppStatusID == "4")
                    //        {

                    //        }
                    //        else if (LoanAppStatusID == "5")
                    //        {

                    //        }
                    //        else {
                    //            ERR = "Error";
                    //            SMS = "Invalid LoanAppStatusID";
                    //            LoanAppResSMS data = new LoanAppResSMS();
                    //            data.SMS = SMS;
                    //            SMSList.Add(data);
                    //        }

                    //        string DeviceDate = loan.DeviceDate;
                    //        string []rs = c.CheckDateTimeFormat(DeviceDate, "DeviceDate");
                    //        ERR = rs[0];
                    //        SMS = rs[1];
                    //        if (ERR == "Error")
                    //        {
                    //            LoanAppResSMS data = new LoanAppResSMS();
                    //            data.SMS = SMS;
                    //            SMSList.Add(data);
                    //        }

                    //        string ProductID = loan.ProductID;
                    //        string LoanRequestAmount = loan.LoanRequestAmount;
                    //        rs = c.CheckIsMoneyFormat(LoanRequestAmount, "LoanRequestAmount");
                    //        ERR = rs[0];
                    //        SMS = rs[1];
                    //        if (ERR == "Error")
                    //        {
                    //            LoanAppResSMS data = new LoanAppResSMS();
                    //            data.SMS = SMS;
                    //            SMSList.Add(data);
                    //        }

                    //        string LoanPurposeID1 = loan.LoanPurposeID1;
                    //        string LoanPurposeID2 = loan.LoanPurposeID2;
                    //        string LoanPurposeID3 = loan.LoanPurposeID3;
                    //        string OwnCapital = loan.OwnCapital;
                    //        rs = c.CheckIsMoneyFormat(OwnCapital, "OwnCapital");
                    //        ERR = rs[0];
                    //        SMS = rs[1];
                    //        if (ERR == "Error")
                    //        {
                    //            LoanAppResSMS data = new LoanAppResSMS();
                    //            data.SMS = SMS;
                    //            SMSList.Add(data);
                    //        }

                    //        string DisbursementDate = loan.DisbursementDate;
                    //        rs = c.CheckDateFormat(DisbursementDate, "DisbursementDate");
                    //        ERR = rs[0];
                    //        SMS = rs[1];
                    //        if (ERR == "Error")
                    //        {
                    //            LoanAppResSMS data = new LoanAppResSMS();
                    //            data.SMS = SMS;
                    //            SMSList.Add(data);
                    //        }

                    //        string FirstWithdrawal = loan.FirstWithdrawal;
                    //        rs = c.CheckIsMoneyFormat(FirstWithdrawal, "FirstWithdrawal");
                    //        ERR = rs[0];
                    //        SMS = rs[1];
                    //        if (ERR == "Error")
                    //        {
                    //            LoanAppResSMS data = new LoanAppResSMS();
                    //            data.SMS = SMS;
                    //            SMSList.Add(data);
                    //        }

                    //        string LoanTerm = loan.LoanTerm;
                    //        string FirstRepaymentDate = loan.FirstRepaymentDate;
                    //        rs = c.CheckDateFormat(FirstRepaymentDate, "FirstRepaymentDate");
                    //        ERR = rs[0];
                    //        SMS = rs[1];
                    //        if (ERR == "Error")
                    //        {
                    //            LoanAppResSMS data = new LoanAppResSMS();
                    //            data.SMS = SMS;
                    //            SMSList.Add(data);
                    //        }

                    //        string LoanInterestRate = loan.LoanInterestRate;
                    //        string CustomerRequestRate = loan.CustomerRequestRate;
                    //        string CompititorRate = loan.CompititorRate;
                    //        string CustomerConditionID = loan.CustomerConditionID;
                    //        string COProposedAmount = loan.COProposedAmount;
                    //        rs = c.CheckIsMoneyFormat(COProposedAmount, "COProposedAmount");
                    //        ERR = rs[0];
                    //        SMS = rs[1];
                    //        if (ERR == "Error")
                    //        {
                    //            LoanAppResSMS data = new LoanAppResSMS();
                    //            data.SMS = SMS;
                    //            SMSList.Add(data);
                    //        }
                    //        string COProposedTerm = loan.COProposedTerm;
                    //        string COProposeRate = loan.COProposeRate;
                    //        string FrontBackOfficeID = loan.FrontBackOfficeID;
                    //        string GroupNumber = loan.GroupNumber;
                    //        string LoanCycleID = loan.LoanCycleID;
                    //        string RepaymentHistoryID = loan.RepaymentHistoryID;
                    //        string LoanReferralID = loan.LoanReferralID;
                    //        string DebtIinfoID = loan.DebtIinfoID;
                    //        string MonthlyFee = loan.MonthlyFee;
                    //        string Compulsory = loan.Compulsory;
                    //        string CompulsoryTerm = loan.CompulsoryTerm;
                    //        string Currency = loan.Currency;
                    //        string UpFrontFee = loan.UpFrontFee;
                    //        string UpFrontAmt = loan.UpFrontAmt;
                    //        string isCBCCheck = loan.isCBCCheck;
                    //        #region validate product
                    //        rs = c.CheckProductValidation(ProductID, Currency, COProposedAmount, LoanTerm, LoanInterestRate, MonthlyFee, UpFrontFee);
                    //        ERR = rs[0];
                    //        SMS = rs[1];
                    //        if (ERR == "Error")
                    //        {
                    //            LoanAppResSMS data = new LoanAppResSMS();
                    //            data.SMS = SMS;
                    //            SMSList.Add(data);
                    //        }
                    //        #endregion validate product
                    //        //

                    //        #endregion LoanApp
                    //        #region PurpsoeDetail
                    //        if (loan.PurposeDetail.Count == 0)
                    //        {
                    //            ERR = "Error";
                    //            SMS = "PurpsoeDetail is required";
                    //            LoanAppResSMS data = new LoanAppResSMS();
                    //            data.SMS = SMS;
                    //            SMSList.Add(data);
                    //        }
                    //        foreach (var lp in loan.PurposeDetail)
                    //        {
                    //            string LoanAppPurpsoeDetail = lp.LoanAppPurposeDetail;
                    //            string Quantity = lp.Quantity;
                    //            rs = c.CheckIsIntFormat(Quantity, "PurpsoeDetail-Quantity");
                    //            ERR = rs[0];
                    //            SMS = rs[1];
                    //            if (ERR == "Error")
                    //            {
                    //                LoanAppResSMS data = new LoanAppResSMS();
                    //                data.SMS = SMS;
                    //                SMSList.Add(data);
                    //            }

                    //            string UnitPrice = lp.UnitPrice;
                    //            rs = c.CheckIsMoneyFormat(UnitPrice, "PurpsoeDetail-UnitPrice");
                    //            ERR = rs[0];
                    //            SMS = rs[1];
                    //            if (ERR == "Error")
                    //            {
                    //                LoanAppResSMS data = new LoanAppResSMS();
                    //                data.SMS = SMS;
                    //                SMSList.Add(data);
                    //            }
                    //            //

                    //        }
                    //        #endregion PurpsoeDetail
                    //        #region Person
                    //        if (loan.Person.Count == 0)
                    //        {
                    //            ERR = "Error";
                    //            SMS = "Person is required";
                    //            LoanAppResSMS data = new LoanAppResSMS();
                    //            data.SMS = SMS;
                    //            SMSList.Add(data);
                    //        }
                    //        foreach (var lp in loan.Person)
                    //        {
                    //            #region Person
                    //            string LoanAppPersonID = lp.LoanAppPersonID;
                    //            string LoanAppPersonTypeID = lp.LoanAppPersonTypeID;
                    //            string PersonID = lp.PersonID;
                    //            string CustomerID = lp.CustomerID;
                    //            string Number = lp.Number;
                    //            string VillageBankID = lp.VillageBankID;
                    //            string AltName = lp.AltName;
                    //            string TitleID = lp.TitleID;
                    //            string LastName = lp.LastName;
                    //            string FirstName = lp.FirstName;
                    //            FirstNameForLog = FirstName;
                    //            string GenderID = lp.GenderID;
                    //            string DateOfBirth = lp.DateOfBirth;
                    //            rs = c.CheckDateFormat(DateOfBirth, "Person-DateOfBirth");
                    //            ERR = rs[0];
                    //            SMS = rs[1];
                    //            if (ERR == "Error")
                    //            {
                    //                LoanAppResSMS data = new LoanAppResSMS();
                    //                data.SMS = SMS;
                    //                SMSList.Add(data);
                    //            }

                    //            string IDCardTypeID = lp.IDCardTypeID;
                    //            string IDCardNumber = lp.IDCardNumber;
                    //            string IDCardExpireDate = lp.IDCardExpireDate;
                    //            rs = c.CheckDateFormat(IDCardExpireDate, "Person-IDCardExpireDate");
                    //            ERR = rs[0];
                    //            SMS = rs[1];
                    //            if (ERR == "Error")
                    //            {
                    //                LoanAppResSMS data = new LoanAppResSMS();
                    //                data.SMS = SMS;
                    //                SMSList.Add(data);
                    //            }

                    //            string IDCardIssuedDate = lp.IDCardIssuedDate;
                    //            rs = c.CheckDateFormat(IDCardIssuedDate, "Person-IDCardIssuedDate");
                    //            ERR = rs[0];
                    //            SMS = rs[1];
                    //            if (ERR == "Error")
                    //            {
                    //                LoanAppResSMS data = new LoanAppResSMS();
                    //                data.SMS = SMS;
                    //                SMSList.Add(data);
                    //            }

                    //            string MaritalStatusID = lp.MaritalStatusID;
                    //            string EducationID = lp.EducationID;
                    //            string CityOfBirthID = lp.CityOfBirthID;
                    //            string Telephone3 = lp.Telephone3;
                    //            string VillageIDPermanent = lp.VillageIDPermanent;
                    //            string LocationCodeIDPermanent = lp.LocationCodeIDPermanent;
                    //            string VillageIDCurrent = lp.VillageIDCurrent;
                    //            string LocationCodeIDCurrent = lp.LocationCodeIDCurrent;
                    //            string SortAddress = lp.SortAddress;
                    //            string FamilyMember = lp.FamilyMember;
                    //            string FamilyMemberActive = lp.FamilyMemberActive;
                    //            string PoorID = lp.PoorID;
                    //            string DeviceDate_Person = lp.DeviceDate;
                    //            rs = c.CheckDateTimeFormat(DeviceDate_Person, "Person-DeviceDate");
                    //            ERR = rs[0];
                    //            SMS = rs[1];
                    //            if (ERR == "Error")
                    //            {
                    //                LoanAppResSMS data = new LoanAppResSMS();
                    //                data.SMS = SMS;
                    //                SMSList.Add(data);
                    //            }
                    //            //

                    //            #endregion Person

                    //            #region Creditor 
                    //            foreach (var lp2 in lp.Creditor)
                    //            {
                    //                string CreditorID = lp2.CreditorID;
                    //                string ApprovedAmount = lp2.ApprovedAmount;
                    //                rs = c.CheckIsMoneyFormat(ApprovedAmount, "Creditor-ApprovedAmount");
                    //                ERR = rs[0];
                    //                SMS = rs[1];
                    //                if (ERR == "Error")
                    //                {
                    //                    LoanAppResSMS data = new LoanAppResSMS();
                    //                    data.SMS = SMS;
                    //                    SMSList.Add(data);
                    //                }

                    //                string OutstandingBalance = lp2.OutstandingBalance;
                    //                rs = c.CheckIsMoneyFormat(OutstandingBalance, "Creditor-OutstandingBalance");
                    //                ERR = rs[0];
                    //                SMS = rs[1];
                    //                if (ERR == "Error")
                    //                {
                    //                    LoanAppResSMS data = new LoanAppResSMS();
                    //                    data.SMS = SMS;
                    //                    SMSList.Add(data);
                    //                }

                    //                string InterestRate = lp2.InterestRate;
                    //                string RepaymentTypeID = lp2.RepaymentTypeID;
                    //                string RepaymentTermID = lp2.RepaymentTermID;
                    //                string LoanStartDate = lp2.LoanStartDate;
                    //                rs = c.CheckDateFormat(LoanStartDate, "Creditor-LoanStartDate");
                    //                ERR = rs[0];
                    //                SMS = rs[1];
                    //                if (ERR == "Error")
                    //                {
                    //                    LoanAppResSMS data = new LoanAppResSMS();
                    //                    data.SMS = SMS;
                    //                    SMSList.Add(data);
                    //                }

                    //                string LoanEndDate = lp2.LoanEndDate;
                    //                rs = c.CheckDateFormat(LoanEndDate, "Creditor-LoanEndDate");
                    //                ERR = rs[0];
                    //                SMS = rs[1];
                    //                if (ERR == "Error")
                    //                {
                    //                    LoanAppResSMS data = new LoanAppResSMS();
                    //                    data.SMS = SMS;
                    //                    SMSList.Add(data);
                    //                }

                    //                string RemainingInstallment = lp2.RemainingInstallment;
                    //                string LoanAppCreditorID = lp2.LoanAppCreditorID;
                    //                //

                    //            }
                    //            #endregion Creditor 
                    //            #region ClientAsset
                    //            foreach (var lp2 in lp.ClientAsset)
                    //            {
                    //                string Description = lp2.Description;
                    //                string Quantity = lp2.Quantity;
                    //                string UnitPrice = lp2.UnitPrice;
                    //                rs = c.CheckIsMoneyFormat(UnitPrice, "ClientAsset-UnitPrice");
                    //                ERR = rs[0];
                    //                SMS = rs[1];
                    //                if (ERR == "Error")
                    //                {
                    //                    LoanAppResSMS data = new LoanAppResSMS();
                    //                    data.SMS = SMS;
                    //                    SMSList.Add(data);
                    //                }
                    //                //

                    //            }
                    //            #endregion ClientAsset
                    //            #region ClientBusiness
                    //            foreach (var lp2 in lp.ClientBusiness)
                    //            {
                    //                string Description = lp2.Description;
                    //                string Quantity = lp2.Quantity;
                    //                string UnitPrice = lp2.UnitPrice;
                    //                rs = c.CheckIsMoneyFormat(UnitPrice, "ClientBusiness-UnitPrice");
                    //                ERR = rs[0];
                    //                SMS = rs[1];
                    //                if (ERR == "Error")
                    //                {
                    //                    LoanAppResSMS data = new LoanAppResSMS();
                    //                    data.SMS = SMS;
                    //                    SMSList.Add(data);
                    //                }
                    //                //

                    //            }
                    //            #endregion ClientBusiness
                    //            #region ClientCollateral
                    //            foreach (var lp2 in lp.ClientCollateral)
                    //            {
                    //                string ColleteralTypeID = lp2.ColleteralTypeID;
                    //                string ColleteralDocTypeID = lp2.ColleteralDocTypeID;
                    //                string ColleteralDocNumber = lp2.ColleteralDocNumber;
                    //                string Description = lp2.Description;
                    //                string Quantity = lp2.Quantity;
                    //                string UnitPrice = lp2.UnitPrice;
                    //                rs = c.CheckIsMoneyFormat(UnitPrice, "ClientCollateral-UnitPrice");
                    //                ERR = rs[0];
                    //                SMS = rs[1];
                    //                if (ERR == "Error")
                    //                {
                    //                    LoanAppResSMS data = new LoanAppResSMS();
                    //                    data.SMS = SMS;
                    //                    SMSList.Add(data);
                    //                }
                    //                //

                    //                #region ClientCollateralImg
                    //                int iClientCollateralImg = 0;
                    //                foreach (var lp3 in lp2.ClientCollateralImg)
                    //                {
                    //                    iClientCollateralImg ++;
                    //                    string ImgName = lp3.ImgName;
                    //                    string Ext = lp3.Ext;
                    //                    rs = c.CheckImage(ImgName,Ext, "ClientCollateral-Image");
                    //                    ERR = rs[0];
                    //                    SMS = rs[1];
                    //                    if (ERR == "Error")
                    //                    {
                    //                        LoanAppResSMS data = new LoanAppResSMS();
                    //                        data.SMS = SMS;
                    //                        SMSList.Add(data);
                    //                    }
                    //                }
                    //                if (iClientCollateralImg == 0) {
                    //                    ERR = "Error";
                    //                    SMS = "ClientCollateral-Image is required";
                    //                    LoanAppResSMS data = new LoanAppResSMS();
                    //                    data.SMS = SMS;
                    //                    SMSList.Add(data);
                    //                }
                    //                #endregion ClientCollateralImg
                    //            }
                    //            #endregion ClientCollateral
                    //            #region GuarantorBusiness
                    //            foreach (var lp2 in lp.GuarantorBusiness)
                    //            {
                    //                string Description = lp2.Description;
                    //                string NetProfitPerYear = lp2.NetProfitPerYear;
                    //                rs = c.CheckIsMoneyFormat(NetProfitPerYear, "GuarantorBusiness-NetProfitPerYear");
                    //                ERR = rs[0];
                    //                SMS = rs[1];
                    //                if (ERR == "Error")
                    //                {
                    //                    LoanAppResSMS data = new LoanAppResSMS();
                    //                    data.SMS = SMS;
                    //                    SMSList.Add(data);
                    //                }
                    //                //

                    //            }
                    //            #endregion
                    //            #region GuarantorAsset
                    //            foreach (var lp2 in lp.GuarantorAsset)
                    //            {
                    //                string Description = lp2.Description;
                    //                string Quantity = lp2.Quantity;
                    //                string UnitPrice = lp2.UnitPrice;
                    //                rs = c.CheckIsMoneyFormat(UnitPrice, "GuarantorAsset-UnitPrice");
                    //                ERR = rs[0];
                    //                SMS = rs[1];
                    //                if (ERR == "Error")
                    //                {
                    //                    LoanAppResSMS data = new LoanAppResSMS();
                    //                    data.SMS = SMS;
                    //                    SMSList.Add(data);
                    //                }
                    //                //

                    //            }
                    //            #endregion
                    //            #region PersonImg
                    //            foreach (var lp2 in lp.PersonImg)
                    //            {
                    //                string ImgName = lp2.ImgName;
                    //                string Ext = lp2.Ext;
                    //                string OneCardTwoDoc = lp2.OneCardTwoDoc;
                    //                rs = c.CheckImage(ImgName, Ext, "Person-Image");
                    //                ERR = rs[0];
                    //                SMS = rs[1];
                    //                if (ERR == "Error")
                    //                {
                    //                    LoanAppResSMS data = new LoanAppResSMS();
                    //                    data.SMS = SMS;
                    //                    SMSList.Add(data);
                    //                }
                    //            }
                    //            #endregion

                    //        }
                    //        #endregion Person
                    //        #region Opinion
                    //        foreach (var lp in loan.Opinion)
                    //        {
                    //            string Description = lp.Description;
                    //            string CreateBy = lp.CreateBy;
                    //            string DeviceDate_Opinion = lp.DeviceDate;
                    //            rs = c.CheckDateTimeFormat(DeviceDate_Opinion, "Opinion-DeviceDate");
                    //            ERR = rs[0];
                    //            SMS = rs[1];
                    //            if (ERR == "Error")
                    //            {
                    //                LoanAppResSMS data = new LoanAppResSMS();
                    //                data.SMS = SMS;
                    //                SMSList.Add(data);
                    //            }
                    //        }
                    //        #endregion Opinion
                    //        #region CashFlow
                    //        foreach (var lp in loan.CashFlow)
                    //        {
                    //            #region CashFlow
                    //            string StudyMonthAmount = lp.StudyMonthAmount;
                    //            string StudyStartMonth = lp.StudyStartMonth;
                    //            string FamilyExpensePerMonth = lp.FamilyExpensePerMonth;
                    //            rs = c.CheckIsMoneyFormat(FamilyExpensePerMonth, "CashFlow-FamilyExpensePerMonth");
                    //            ERR = rs[0];
                    //            SMS = rs[1];
                    //            if (ERR == "Error")
                    //            {
                    //                LoanAppResSMS data = new LoanAppResSMS();
                    //                data.SMS = SMS;
                    //                SMSList.Add(data);
                    //            }

                    //            string OtherExpensePerMonth = lp.OtherExpensePerMonth;
                    //            rs = c.CheckIsMoneyFormat(OtherExpensePerMonth, "CashFlow-OtherExpensePerMonth");
                    //            ERR = rs[0];
                    //            SMS = rs[1];
                    //            if (ERR == "Error")
                    //            {
                    //                LoanAppResSMS data = new LoanAppResSMS();
                    //                data.SMS = SMS;
                    //                SMSList.Add(data);
                    //            }

                    //            #endregion CashFlow
                    //            #region MSI
                    //            foreach (var lp2 in lp.MSI)
                    //            {
                    //                #region MSI
                    //                string IncomeTypeID = lp2.IncomeTypeID;
                    //                string MainSourceIncomeID = lp2.MainSourceIncomeID;
                    //                string Remark = lp2.Remark;
                    //                string Quantity = lp2.Quantity;
                    //                string ExAge = lp2.ExAge;
                    //                rs = c.CheckIsIntFormat(ExAge, "MSI-ExAge");
                    //                ERR = rs[0];
                    //                SMS = rs[1];
                    //                if (ERR == "Error")
                    //                {
                    //                    LoanAppResSMS data = new LoanAppResSMS();
                    //                    data.SMS = SMS;
                    //                    SMSList.Add(data);
                    //                }

                    //                string BusAge = lp2.BusAge;
                    //                rs = c.CheckIsIntFormat(BusAge, "MSI-BusAge");
                    //                ERR = rs[0];
                    //                SMS = rs[1];
                    //                if (ERR == "Error")
                    //                {
                    //                    LoanAppResSMS data = new LoanAppResSMS();
                    //                    data.SMS = SMS;
                    //                    SMSList.Add(data);
                    //                }

                    //                string isMSI = lp2.isMSI;
                    //                #endregion MSI
                    //                #region MSIRegular
                    //                foreach (var lp3 in lp2.MSIRegular)
                    //                {
                    //                    string Description = lp3.Description;
                    //                    string Amount = lp3.Amount;
                    //                    string UnitID = lp3.UnitID;
                    //                    string Cost = lp3.Cost;
                    //                    string OneIncomeTwoExpense = lp3.OneIncomeTwoExpense;
                    //                    string CurrencyID = lp3.Currency;
                    //                    if (CurrencyID == "KHR")
                    //                    {
                    //                        CurrencyID = "1";
                    //                    }
                    //                    else if (CurrencyID == "USD")
                    //                    {
                    //                        CurrencyID = "2";
                    //                    }
                    //                    else {
                    //                        CurrencyID = "3";
                    //                    }
                    //                    string Month = lp3.Month;
                    //                }
                    //                #endregion MSIRegular
                    //                #region MSIIrregular
                    //                foreach (var lp3 in lp2.MSIIrregular)
                    //                {
                    //                    string Description = lp3.Description;
                    //                    string Amount = lp3.Amount;
                    //                    string UnitID = lp3.UnitID;
                    //                    string Cost = lp3.Cost;
                    //                    string OneIncomeTwoExpense = lp3.OneIncomeTwoExpense;
                    //                    string CurrencyID = lp3.Currency;
                    //                    if (CurrencyID == "KHR")
                    //                    {
                    //                        CurrencyID = "1";
                    //                    }
                    //                    else if (CurrencyID == "USD")
                    //                    {
                    //                        CurrencyID = "2";
                    //                    }
                    //                    else
                    //                    {
                    //                        CurrencyID = "3";
                    //                    }
                    //                    string Month = lp3.Month;
                    //                }
                    //                #endregion MSIIrregular

                    //            }
                    //            #endregion MSI

                    //        }
                    //        #endregion CashFlow

                    //    }
                    //}
                    //catch(Exception ex)
                    //{
                    //    ERR = "Error";
                    //    SMS = "Something was wrong in LoanApp JSON";
                    //    LoanAppResSMS data = new LoanAppResSMS();
                    //    data.SMS = SMS;
                    //    SMSList.Add(data);
                    //}
                    #endregion checking
                    #region add
                    if (SMSList.Count > 0)
                    {
                        ERR = "Error";
                        SMS = "Something was wrong in LoanApp";
                    }
                    else {
                        string ServerLoanAppID = "0";
                        SqlConnection Con1 = new SqlConnection(c.ConStr());
                        Con1.Open();
                        //SqlTransaction Tran1;
                        //Tran1 = Con1.BeginTransaction();
                        SqlCommand Com1 = new SqlCommand();
                        Com1.Connection = Con1;
                        //Com1.Transaction = Tran1;
                        string sql = "";
                        try
                        {
                            foreach (var loan in jObj.LoanApp)
                            {
                                #region LoanApp 
                                #region LoanApp Param                           
                                string IDOnDevice = loan.IDOnDevice;
                                RSIDOnDevice = IDOnDevice;
                                //string LoanAppID = loan.LoanAppID;
                                string LoanAppID = "0";
                                //RSLoanAppID = LoanAppID;
                                //string LoanAppStatusID = loan.LoanAppStatusID;
                                string LoanAppStatusID = "3";
                                string DeviceDate = loan.DeviceDate;
                                string ProductID = loan.ProductID;
                                string LoanRequestAmount = loan.LoanRequestAmount;
                                string LoanPurposeID1 = loan.LoanPurposeID1;
                                string LoanPurposeID2 = loan.LoanPurposeID2;
                                string LoanPurposeID3 = loan.LoanPurposeID3;
                                string OwnCapital = loan.OwnCapital;
                                string DisbursementDate = loan.DisbursementDate;
                                string FirstWithdrawal = loan.FirstWithdrawal;
                                string LoanTerm = loan.LoanTerm;
                                string FirstRepaymentDate = loan.FirstRepaymentDate;
                                string LoanInterestRate = loan.LoanInterestRate;
                                string CustomerRequestRate = loan.CustomerRequestRate;
                                string CompititorRate = loan.CompititorRate;
                                string CustomerConditionID = loan.CustomerConditionID;
                                string COProposedAmount = loan.COProposedAmount;
                                string COProposedTerm = loan.COProposedTerm;
                                string COProposeRate = loan.COProposeRate;
                                string FrontBackOfficeID = loan.FrontBackOfficeID;
                                string GroupNumber = loan.GroupNumber;
                                string LoanCycleID = loan.LoanCycleID;
                                string RepaymentHistoryID = loan.RepaymentHistoryID;
                                string LoanReferralID = loan.LoanReferralID;
                                string DebtIinfoID = loan.DebtIinfoID;
                                string MonthlyFee = loan.MonthlyFee;
                                string Compulsory = loan.Compulsory;
                                //string CompulsoryTerm = loan.CompulsoryTerm;
                                string CompulsoryTerm = "0";
                                string Currency = loan.Currency;
                                string UpFrontFee = loan.UpFrontFee;
                                string UpFrontAmt = loan.UpFrontAmt;
                                //string isCBCCheck = loan.isCBCCheck;
                                string CompulsoryOptionID = loan.CompulsoryOptionID;
                                string FundSource = loan.FundSource;
                                string IsNewCollateral = loan.IsNewCollateral;
                                #endregion LoanApp Param                                
                                #region sql add loan
                                sql = "exec sp_LoanApp1Add2 @IDOnDevice=@IDOnDevice,@LoanAppID=@LoanAppID,@LoanAppStatusID=@LoanAppStatusID,@DeviceDate=@DeviceDate"
                                + ",@ServerDate=@ServerDate,@CreateBy=@CreateBy,@ProductID=@ProductID,@LoanRequestAmount=@LoanRequestAmount"
                                + ",@LoanPurposeID1=@LoanPurposeID1,@LoadPurposeID2=@LoadPurposeID2,@LoadPurposeID3=@LoadPurposeID3,@OwnCapital=@OwnCapital"
                                + ",@DisbursementDate=@DisbursementDate,@FirstWithdrawal=@FirstWithdrawal,@LoanTerm=@LoanTerm,@FirstRepaymentDate=@FirstRepaymentDate"
                                + ",@LoanInterestRate=@LoanInterestRate,@CustomerRequestRate=@CustomerRequestRate,@CompititorRate=@CompititorRate"
                                + ",@CustomerConditionID=@CustomerConditionID,@COProposedAmount=@COProposedAmount,@COProposedTerm=@COProposedTerm"
                                + ",@COProposeRate=@COProposeRate,@FrontBackOfficeID=@FrontBackOfficeID,@GroupNumber=@GroupNumber,@LoanCycleID=@LoanCycleID"
                                + ",@RepaymentHistoryID=@RepaymentHistoryID,@LoanReferralID=@LoanReferralID,@DebtIinfoID=@DebtIinfoID,@MonthlyFee=@MonthlyFee"
                                + ",@Compulsory=@Compulsory,@CompulsoryTerm=@CompulsoryTerm,@Currency=@Currency,@UpFrontFee=@UpFrontFee"
                                + ",@UpFrontAmt=@UpFrontAmt,@CompulsoryOptionID=@CompulsoryOptionID,@isCBCCheck=@isCBCCheck,@FundSource=@FundSource"
                                +",@IsNewCollateral=@IsNewCollateral";
                                Com1.CommandText = sql;
                                Com1.Parameters.Clear();
                                Com1.Parameters.AddWithValue("@IDOnDevice", IDOnDevice);
                                Com1.Parameters.AddWithValue("@LoanAppID", LoanAppID);
                                Com1.Parameters.AddWithValue("@LoanAppStatusID", LoanAppStatusID);
                                Com1.Parameters.AddWithValue("@DeviceDate", DeviceDate);
                                Com1.Parameters.AddWithValue("@ServerDate", ServerDate);
                                Com1.Parameters.AddWithValue("@CreateBy", UserID);
                                Com1.Parameters.AddWithValue("@ProductID", ProductID);
                                Com1.Parameters.AddWithValue("@LoanRequestAmount", LoanRequestAmount);
                                Com1.Parameters.AddWithValue("@LoanPurposeID1", LoanPurposeID1);
                                Com1.Parameters.AddWithValue("@LoadPurposeID2", LoanPurposeID2);
                                Com1.Parameters.AddWithValue("@LoadPurposeID3", LoanPurposeID3);
                                Com1.Parameters.AddWithValue("@OwnCapital", OwnCapital);
                                Com1.Parameters.AddWithValue("@DisbursementDate", DisbursementDate);
                                Com1.Parameters.AddWithValue("@FirstWithdrawal", FirstWithdrawal);
                                Com1.Parameters.AddWithValue("@LoanTerm", LoanTerm);
                                Com1.Parameters.AddWithValue("@FirstRepaymentDate", FirstRepaymentDate);
                                Com1.Parameters.AddWithValue("@LoanInterestRate", LoanInterestRate);
                                Com1.Parameters.AddWithValue("@CustomerRequestRate", CustomerRequestRate);
                                Com1.Parameters.AddWithValue("@CompititorRate", CompititorRate);
                                Com1.Parameters.AddWithValue("@CustomerConditionID", CustomerConditionID);
                                Com1.Parameters.AddWithValue("@COProposedAmount", COProposedAmount);
                                Com1.Parameters.AddWithValue("@COProposedTerm", COProposedTerm);
                                Com1.Parameters.AddWithValue("@COProposeRate", COProposeRate);
                                Com1.Parameters.AddWithValue("@FrontBackOfficeID", FrontBackOfficeID);
                                Com1.Parameters.AddWithValue("@GroupNumber", GroupNumber);
                                Com1.Parameters.AddWithValue("@LoanCycleID", LoanCycleID);
                                Com1.Parameters.AddWithValue("@RepaymentHistoryID", RepaymentHistoryID);
                                Com1.Parameters.AddWithValue("@LoanReferralID", LoanReferralID);
                                Com1.Parameters.AddWithValue("@DebtIinfoID", DebtIinfoID);
                                Com1.Parameters.AddWithValue("@MonthlyFee", MonthlyFee);
                                Com1.Parameters.AddWithValue("@Compulsory", Compulsory);
                                Com1.Parameters.AddWithValue("@CompulsoryTerm", CompulsoryTerm);
                                Com1.Parameters.AddWithValue("@Currency", Currency);
                                Com1.Parameters.AddWithValue("@UpFrontFee", UpFrontFee);
                                Com1.Parameters.AddWithValue("@UpFrontAmt", UpFrontAmt);
                                Com1.Parameters.AddWithValue("@CompulsoryOptionID", CompulsoryOptionID);
                                Com1.Parameters.AddWithValue("@isCBCCheck", "1");
                                Com1.Parameters.AddWithValue("@FundSource", FundSource);
                                Com1.Parameters.AddWithValue("@IsNewCollateral", IsNewCollateral);
                                DataTable dt1 = new DataTable();
                                dt1.Load(Com1.ExecuteReader());
                                ServerLoanAppID = dt1.Rows[0][0].ToString();
                                //ServerLoanAppID = "1";
                                RSLoanAppID = ServerLoanAppID;
                                #endregion sql
                                #region update Inst
                                try
                                {
                                    if (InstID.Length>1) {
                                        InstID = "1";
                                    }
                                    c.ReturnDT("update tblLoanApp1 set InstID='"+ InstID + "' where LoanAppID='"+ ServerLoanAppID + "'");
                                } catch { }
                                #endregion update Inst
                                #endregion LoanApp
                                #region PurpsoeDetail
                                foreach (var lp in loan.PurposeDetail)
                                {
                                    #region param
                                    string LoanAppPurpsoeDetail = lp.LoanAppPurposeDetail;
                                    string Quantity = lp.Quantity;
                                    string UnitPrice = lp.UnitPrice;
                                    #endregion param
                                    #region sql
                                    sql = "exec sp_LoanApp11Add2 @IDOnDevice=@IDOnDevice,@LoanAppID=@LoanAppID,@LoanAppPurpsoeDetail=@LoanAppPurpsoeDetail"
                                    + ",@Quantity=@Quantity,@UnitPrice=@UnitPrice";
                                    Com1.CommandText = sql;
                                    Com1.Parameters.Clear();
                                    Com1.Parameters.AddWithValue("@IDOnDevice", IDOnDevice);
                                    Com1.Parameters.AddWithValue("@LoanAppID", ServerLoanAppID);
                                    Com1.Parameters.AddWithValue("@LoanAppPurpsoeDetail", LoanAppPurpsoeDetail);
                                    Com1.Parameters.AddWithValue("@Quantity", Quantity);
                                    Com1.Parameters.AddWithValue("@UnitPrice", UnitPrice);
                                    Com1.ExecuteNonQuery();
                                    #endregion sql
                                }
                                #endregion PurpsoeDetail
                                #region Person
                                foreach (var lp in loan.Person)
                                {
                                    #region Person
                                    string ServerLoanAppPersonID = "";
                                    #region Person Param
                                    string LoanAppPersonID = lp.LoanAppPersonID;
                                    string LoanAppPersonTypeID = lp.LoanAppPersonTypeID;
                                    string PersonID = lp.PersonID;
                                    string CustomerID = lp.CustomerID;
                                    string Number = lp.Number;
                                    string VillageBankID = lp.VillageBankID;
                                    string AltName = lp.AltName;
                                    string TitleID = lp.TitleID;
                                    string LastName = lp.LastName;
                                    string FirstName = lp.FirstName;
                                    string GenderID = lp.GenderID;
                                    if (GenderID == "MALE") {
                                        GenderID = "0";
                                    }
                                    if (GenderID == "FEMALE")
                                    {
                                        GenderID = "1";
                                    }
                                    string DateOfBirth = lp.DateOfBirth;
                                    string IDCardTypeID = lp.IDCardTypeID;
                                    string IDCardNumber = lp.IDCardNumber;
                                    string IDCardExpireDate = lp.IDCardExpireDate;
                                    string IDCardIssuedDate = lp.IDCardIssuedDate;
                                    string MaritalStatusID = lp.MaritalStatusID;
                                    string EducationID = lp.EducationID;
                                    string CityOfBirthID = lp.CityOfBirthID;
                                    string Telephone3 = lp.Telephone3;
                                    string VillageIDPermanent = lp.VillageIDPermanent;
                                    string LocationCodeIDPermanent = lp.LocationCodeIDPermanent;
                                    string VillageIDCurrent = lp.VillageIDCurrent;
                                    string LocationCodeIDCurrent = lp.LocationCodeIDCurrent;
                                    string SortAddress = lp.SortAddress;
                                    string FamilyMember = lp.FamilyMember;
                                    string FamilyMemberActive = lp.FamilyMemberActive;
                                    string PoorID = lp.PoorID;
                                    string DeviceDate_Person = lp.DeviceDate;
                                    string AltName2 = lp.AltName2;
                                    #endregion Person Param
                                    #region sql
                                    sql = "exec T24_LoanAppPersonAddEdit @IDOnDevice=@IDOnDevice,@LoanAppID=@LoanAppID,@LoanAppPersonTypeID=@LoanAppPersonTypeID"
                                    + ",@PersonID=@PersonID,@CustomerID=@CustomerID,@Number=@Number,@VillageBankID=@VillageBankID,@AltName=@AltName,@TitleID=@TitleID"
                                    + ",@LastName=@LastName,@FirstName=@FirstName,@FullName=@FullName,@GenderID=@GenderID,@DateOfBirth=@DateOfBirth"
                                    + ",@IDCardTypeID=@IDCardTypeID,@IDCardNumber=@IDCardNumber,@IDCardExpireDate=@IDCardExpireDate,@MaritalStatusID=@MaritalStatusID"
                                    + ",@EducationID=@EducationID,@CityOfBirthID=@CityOfBirthID,@Telephone3=@Telephone3,@BranchID=@BranchID"
                                    + ",@VillageIDPermanent=@VillageIDPermanent"
                                    + ",@LocationCodeIDPermanent=@LocationCodeIDPermanent"
                                    + ",@VillageIDCurrent=@VillageIDCurrent,@LocationCodeIDCurrent=@LocationCodeIDCurrent"
                                    + ",@SortAddress=@SortAddress,@FamilyMember=@FamilyMember,@FamilyMemberActive=@FamilyMemberActive,@PoorID=@PoorID"
                                    + ",@OfficeNameID=@OfficeNameID,@LoanAppPersonIDOnDevice=@LoanAppPersonIDOnDevice,@DeviceDate=@DeviceDate"
                                    + ",@IDCardIssuedDate=@IDCardIssuedDate,@AltName2=@AltName2";
                                    Com1.CommandText = sql;
                                    Com1.Parameters.Clear();
                                    Com1.Parameters.AddWithValue("@IDOnDevice", IDOnDevice);
                                    Com1.Parameters.AddWithValue("@LoanAppID", ServerLoanAppID);
                                    Com1.Parameters.AddWithValue("@LoanAppPersonTypeID", LoanAppPersonTypeID);
                                    Com1.Parameters.AddWithValue("@PersonID", PersonID);
                                    Com1.Parameters.AddWithValue("@CustomerID", CustomerID);
                                    Com1.Parameters.AddWithValue("@Number", Number);
                                    Com1.Parameters.AddWithValue("@VillageBankID", VillageBankID);
                                    Com1.Parameters.AddWithValue("@AltName", AltName);
                                    Com1.Parameters.AddWithValue("@TitleID", TitleID);
                                    Com1.Parameters.AddWithValue("@LastName", LastName);
                                    Com1.Parameters.AddWithValue("@FirstName", FirstName);
                                    Com1.Parameters.AddWithValue("@FullName", "");
                                    Com1.Parameters.AddWithValue("@GenderID", GenderID);
                                    Com1.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
                                    Com1.Parameters.AddWithValue("@IDCardTypeID", IDCardTypeID);
                                    Com1.Parameters.AddWithValue("@IDCardNumber", IDCardNumber);
                                    Com1.Parameters.AddWithValue("@IDCardExpireDate", IDCardExpireDate);
                                    Com1.Parameters.AddWithValue("@MaritalStatusID", MaritalStatusID);
                                    Com1.Parameters.AddWithValue("@EducationID", EducationID);
                                    Com1.Parameters.AddWithValue("@CityOfBirthID", CityOfBirthID);
                                    Com1.Parameters.AddWithValue("@Telephone3", Telephone3);
                                    Com1.Parameters.AddWithValue("@BranchID", "0");
                                    Com1.Parameters.AddWithValue("@VillageIDPermanent", VillageIDPermanent);
                                    Com1.Parameters.AddWithValue("@LocationCodeIDPermanent", LocationCodeIDPermanent);
                                    Com1.Parameters.AddWithValue("@VillageIDCurrent", VillageIDCurrent);
                                    Com1.Parameters.AddWithValue("@LocationCodeIDCurrent", LocationCodeIDCurrent);
                                    Com1.Parameters.AddWithValue("@SortAddress", SortAddress);
                                    Com1.Parameters.AddWithValue("@FamilyMember", FamilyMember);
                                    Com1.Parameters.AddWithValue("@FamilyMemberActive", FamilyMemberActive);
                                    Com1.Parameters.AddWithValue("@PoorID", PoorID);
                                    Com1.Parameters.AddWithValue("@OfficeNameID", "0");
                                    Com1.Parameters.AddWithValue("@LoanAppPersonIDOnDevice", LoanAppPersonID);
                                    Com1.Parameters.AddWithValue("@DeviceDate", DeviceDate);
                                    Com1.Parameters.AddWithValue("@IDCardIssuedDate", IDCardIssuedDate);
                                    Com1.Parameters.AddWithValue("@AltName2", AltName2);
                                    DataTable dt2 = new DataTable();
                                    dt2.Load(Com1.ExecuteReader());
                                    ServerLoanAppPersonID = dt2.Rows[0][0].ToString();
                                    //ServerLoanAppPersonID = "1";
                                    #endregion sql

                                    #endregion Person

                                    #region Creditor 
                                    foreach (var lp2 in lp.Creditor)
                                    {
                                        #region Parama
                                        string CreditorID = lp2.CreditorID;
                                        string ApprovedAmount = lp2.ApprovedAmount;
                                        string OutstandingBalance = lp2.OutstandingBalance;
                                        string InterestRate = lp2.InterestRate;
                                        string RepaymentTypeID = lp2.RepaymentTypeID;
                                        string RepaymentTermID = lp2.RepaymentTermID;
                                        string LoanStartDate = lp2.LoanStartDate;
                                        string LoanEndDate = lp2.LoanEndDate;
                                        string RemainingInstallment = lp2.RemainingInstallment;
                                        string LoanAppCreditorID = lp2.LoanAppCreditorID;
                                        #endregion Parama
                                        #region sql
                                        sql = "exec T24_LoanAppCreditorAddEdit @ServerLoanAppID=@ServerLoanAppID,@ServerLoanAppPersonID=@ServerLoanAppPersonID"
                                        + ",@CreditorID=@CreditorID,@ApprovedAmount=@ApprovedAmount,@OutstandingBalance=@OutstandingBalance,@InterestRate=@InterestRate"
                                        + ",@RepaymentTypeID=@RepaymentTypeID,@RepaymentTermID=@RepaymentTermID,@LoanStartDate=@LoanStartDate"
                                        + ",@LoanEndDate=@LoanEndDate,@RemainingInstallment=@RemainingInstallment";
                                        Com1.CommandText = sql;
                                        Com1.Parameters.Clear();
                                        Com1.Parameters.AddWithValue("@ServerLoanAppID", ServerLoanAppID);
                                        Com1.Parameters.AddWithValue("@ServerLoanAppPersonID", ServerLoanAppPersonID);
                                        Com1.Parameters.AddWithValue("@CreditorID", CreditorID);
                                        Com1.Parameters.AddWithValue("@ApprovedAmount", ApprovedAmount);
                                        Com1.Parameters.AddWithValue("@OutstandingBalance", OutstandingBalance);
                                        Com1.Parameters.AddWithValue("@InterestRate", InterestRate);
                                        Com1.Parameters.AddWithValue("@RepaymentTypeID", RepaymentTypeID);
                                        Com1.Parameters.AddWithValue("@RepaymentTermID", RepaymentTermID);
                                        Com1.Parameters.AddWithValue("@LoanStartDate", LoanStartDate);
                                        Com1.Parameters.AddWithValue("@LoanEndDate", LoanEndDate);
                                        Com1.Parameters.AddWithValue("@RemainingInstallment", RemainingInstallment);
                                        Com1.ExecuteNonQuery();
                                        #endregion sql
                                    }
                                    #endregion Creditor 
                                    #region ClientAsset
                                    foreach (var lp2 in lp.ClientAsset)
                                    {
                                        #region Param
                                        string Description = lp2.Description;
                                        string Quantity = lp2.Quantity;
                                        string UnitPrice = lp2.UnitPrice;
                                        #endregion Param
                                        #region sql
                                        sql = "exec T24_LoanAppClientAssetAddEdit @ServerLoanAppPersonID=@ServerLoanAppPersonID,@ServerLoanAppID=@ServerLoanAppID"
                                        + ",@Description=@Description,@Quantity=@Quantity,@UnitPrice=@UnitPrice";
                                        Com1.CommandText = sql;
                                        Com1.Parameters.Clear();
                                        Com1.Parameters.AddWithValue("@ServerLoanAppID", ServerLoanAppID);
                                        Com1.Parameters.AddWithValue("@ServerLoanAppPersonID", ServerLoanAppPersonID);
                                        Com1.Parameters.AddWithValue("@Description", Description);
                                        Com1.Parameters.AddWithValue("@Quantity", Quantity);
                                        Com1.Parameters.AddWithValue("@UnitPrice", UnitPrice);
                                        Com1.ExecuteNonQuery();
                                        #endregion sql
                                    }
                                    #endregion ClientAsset
                                    #region ClientBusiness
                                    foreach (var lp2 in lp.ClientBusiness)
                                    {
                                        #region Param
                                        string Description = lp2.Description;
                                        string Quantity = lp2.Quantity;
                                        string UnitPrice = lp2.UnitPrice;
                                        #endregion Param
                                        #region sql
                                        sql = "exec T24_LoanAppClientBusinessAddEdit @ServerLoanAppPersonID=@ServerLoanAppPersonID,@ServerLoanAppID=@ServerLoanAppID"
                                        + ",@Description=@Description,@Quantity=@Quantity,@UnitPrice=@UnitPrice";
                                        Com1.CommandText = sql;
                                        Com1.Parameters.Clear();
                                        Com1.Parameters.AddWithValue("@ServerLoanAppID", ServerLoanAppID);
                                        Com1.Parameters.AddWithValue("@ServerLoanAppPersonID", ServerLoanAppPersonID);
                                        Com1.Parameters.AddWithValue("@Description", Description);
                                        Com1.Parameters.AddWithValue("@Quantity", Quantity);
                                        Com1.Parameters.AddWithValue("@UnitPrice", UnitPrice);
                                        Com1.ExecuteNonQuery();
                                        #endregion sql
                                    }
                                    #endregion ClientBusiness
                                    #region ClientCollateral
                                    foreach (var lp2 in lp.ClientCollateral)
                                    {
                                        #region Param
                                        string ServerLoanAppClientCollateralID = "";
                                        string LoanAppClientCollateralID = lp2.LoanAppClientCollateralID;
                                        string ColleteralTypeID = lp2.ColleteralTypeID;
                                        string ColleteralDocTypeID = lp2.ColleteralDocTypeID;
                                        string ColleteralDocNumber = lp2.ColleteralDocNumber;
                                        string Description = lp2.Description;
                                        string Quantity = lp2.Quantity;
                                        string UnitPrice = lp2.UnitPrice;
                                        #endregion Param
                                        #region sql
                                        sql = "exec T24_LoanAppClientCollateralAddEdit @ServerLoanAppPersonID=@ServerLoanAppPersonID,@ServerLoanAppID=@ServerLoanAppID"
                                        + ",@ColleteralTypeID=@ColleteralTypeID,@ColleteralDocTypeID=@ColleteralDocTypeID"
                                        + ",@ColleteralDocNumber=@ColleteralDocNumber,@Description=@Description,@Quantity=@Quantity,@UnitPrice=@UnitPrice";
                                        Com1.CommandText = sql;
                                        Com1.Parameters.Clear();
                                        Com1.Parameters.AddWithValue("@ServerLoanAppPersonID", ServerLoanAppPersonID);
                                        Com1.Parameters.AddWithValue("@ServerLoanAppID", ServerLoanAppID);
                                        Com1.Parameters.AddWithValue("@ColleteralTypeID", ColleteralTypeID);
                                        Com1.Parameters.AddWithValue("@ColleteralDocTypeID", ColleteralDocTypeID);
                                        Com1.Parameters.AddWithValue("@ColleteralDocNumber", ColleteralDocNumber);
                                        Com1.Parameters.AddWithValue("@Description", Description);
                                        Com1.Parameters.AddWithValue("@Quantity", Quantity);
                                        Com1.Parameters.AddWithValue("@UnitPrice", UnitPrice);
                                        DataTable dt3 = new DataTable();
                                        dt3.Load(Com1.ExecuteReader());
                                        ServerLoanAppClientCollateralID = dt3.Rows[0][0].ToString();
                                        //ServerLoanAppClientCollateralID = "1";
                                        #endregion sql

                                        #region ClientCollateralImg
                                        foreach (var lp3 in lp2.ClientCollateralImg)
                                        {
                                            string ImgName = lp3.ImgName;
                                            string Ext = lp3.Ext;

                                            sql = "exec T24_LoanAppClientCollateralImageAdd @ServerLoanAppID=@ServerLoanAppID,@ServerLoanAppPersonID=@ServerLoanAppPersonID"
                                            + ",@ServerLoanAppClientCollateralID=@ServerLoanAppClientCollateralID,@Ext=@Ext,@Remark=@Remark";
                                            Com1.CommandText = sql;
                                            Com1.Parameters.Clear();
                                            Com1.Parameters.AddWithValue("@ServerLoanAppID", ServerLoanAppID);
                                            Com1.Parameters.AddWithValue("@ServerLoanAppPersonID", ServerLoanAppPersonID);
                                            Com1.Parameters.AddWithValue("@ServerLoanAppClientCollateralID", ServerLoanAppClientCollateralID);
                                            Com1.Parameters.AddWithValue("@Ext", Ext);
                                            Com1.Parameters.AddWithValue("@Remark", ImgName);
                                            DataTable dt4 = new DataTable();
                                            dt4.Load(Com1.ExecuteReader());
                                            string fname = dt4.Rows[0][0].ToString();
                                            //string fname = ImgName + "_1";
                                            LoanAppResImgList data = new LoanAppResImgList();
                                            data.OriImgName = ImgName;
                                            data.ServerImgName = fname;
                                            ImgList.Add(data);
                                        }
                                        #endregion ClientCollateralImg
                                    }
                                    #endregion ClientCollateral
                                    #region GuarantorBusiness
                                    foreach (var lp2 in lp.GuarantorBusiness)
                                    {
                                        string Description = lp2.Description;
                                        string NetProfitPerYear = lp2.NetProfitPerYear;
                                        //
                                        sql = "exec T24_LoanAppGuarantorBusinessAddEdit @ServerLoanAppID=@ServerLoanAppID,@ServerLoanAppPersonID=@ServerLoanAppPersonID"
                                        + ",@Description=@Description,@NetProfitPerYear=@NetProfitPerYear";
                                        Com1.CommandText = sql;
                                        Com1.Parameters.Clear();
                                        Com1.Parameters.AddWithValue("@ServerLoanAppID", ServerLoanAppID);
                                        Com1.Parameters.AddWithValue("@ServerLoanAppPersonID", ServerLoanAppPersonID);
                                        Com1.Parameters.AddWithValue("@Description", Description);
                                        Com1.Parameters.AddWithValue("@NetProfitPerYear", NetProfitPerYear);
                                        Com1.ExecuteNonQuery();
                                    }
                                    #endregion
                                    #region GuarantorAsset
                                    foreach (var lp2 in lp.GuarantorAsset)
                                    {
                                        string Description = lp2.Description;
                                        string Quantity = lp2.Quantity;
                                        string UnitPrice = lp2.UnitPrice;
                                        //
                                        sql = "exec T24_LoanAppGuarantorAssetAddEdit @ServerLoanAppID=@ServerLoanAppID,@ServerLoanAppPersonID=@ServerLoanAppPersonID"
                                        + ",@Description=@Description,@Quantity=@Quantity,@UnitPrice=@UnitPrice";
                                        Com1.CommandText = sql;
                                        Com1.Parameters.Clear();
                                        Com1.Parameters.AddWithValue("@ServerLoanAppID", ServerLoanAppID);
                                        Com1.Parameters.AddWithValue("@ServerLoanAppPersonID", ServerLoanAppPersonID);
                                        Com1.Parameters.AddWithValue("@Description", Description);
                                        Com1.Parameters.AddWithValue("@Quantity", Quantity);
                                        Com1.Parameters.AddWithValue("@UnitPrice", UnitPrice);
                                        Com1.ExecuteNonQuery();
                                    }
                                    #endregion
                                    #region PersonImg
                                    foreach (var lp2 in lp.PersonImg)
                                    {
                                        string ImgName = lp2.ImgName;
                                        string Ext = lp2.Ext;
                                        string OneCardTwoDoc = lp2.OneCardTwoDoc;
                                        //
                                        sql = "exec T24_LoanAppPersonImageImageAdd @ServerLoanAppID=@ServerLoanAppID,@ServerLoanAppPersonID=@ServerLoanAppPersonID"
                                        + ",@Ext=@Ext,@Remark=@Remark,@OneCardTwoDoc=@OneCardTwoDoc";
                                        Com1.CommandText = sql;
                                        Com1.Parameters.Clear();
                                        Com1.Parameters.AddWithValue("@ServerLoanAppID", ServerLoanAppID);
                                        Com1.Parameters.AddWithValue("@ServerLoanAppPersonID", ServerLoanAppPersonID);
                                        Com1.Parameters.AddWithValue("@Ext", Ext);
                                        Com1.Parameters.AddWithValue("@Remark", ImgName);
                                        Com1.Parameters.AddWithValue("@OneCardTwoDoc", OneCardTwoDoc);
                                        DataTable dt3 = new DataTable();
                                        dt3.Load(Com1.ExecuteReader());
                                        string fname = dt3.Rows[0][0].ToString();
                                        //string fname = ImgName + "_2";
                                        LoanAppResImgList data = new LoanAppResImgList();
                                        data.OriImgName = ImgName;
                                        data.ServerImgName = fname;
                                        ImgList.Add(data);
                                    }
                                    #endregion

                                }
                                #endregion Person
                                #region Opinion
                                foreach (var lp in loan.Opinion)
                                {
                                    string Description = lp.Description;
                                    //string CreateBy = lp.CreateBy;
                                    string CreateBy = UserID;
                                    string DeviceDate_Opinion = lp.DeviceDate;
                                    sql = "exec sp_LoanApp20OpinionAdd2 @LoanAppID=@LoanAppID,@Opinion=@Opinion,@DeviceDate=@DeviceDate,@CreateDate=@CreateDate"
                                    + ",@CreateBy=@CreateBy,@LoanAppStatusID=@LoanAppStatusID";
                                    Com1.CommandText = sql;
                                    Com1.Parameters.Clear();
                                    Com1.Parameters.AddWithValue("@LoanAppID", ServerLoanAppID);
                                    Com1.Parameters.AddWithValue("@Opinion", Description);
                                    Com1.Parameters.AddWithValue("@DeviceDate", DeviceDate_Opinion);
                                    Com1.Parameters.AddWithValue("@CreateDate", ServerDate);
                                    Com1.Parameters.AddWithValue("@CreateBy", CreateBy);
                                    Com1.Parameters.AddWithValue("@LoanAppStatusID", LoanAppStatusID);
                                    Com1.ExecuteNonQuery();
                                }
                                #endregion Opinion
                                #region CashFlow
                                foreach (var lp in loan.CashFlow)
                                {
                                    #region CashFlow
                                    string ServerLoanAppCashFlowID = "";
                                    string StudyMonthAmount = lp.StudyMonthAmount;
                                    string StudyStartMonth = lp.StudyStartMonth;
                                    string FamilyExpensePerMonth = lp.FamilyExpensePerMonth;
                                    string OtherExpensePerMonth = lp.OtherExpensePerMonth;
                                    sql = "exec T24_LoanAppCashFlowAddEdit @LoanAppID=@LoanAppID,@StudyMonthAmount=@StudyMonthAmount,@StudyStartMonth=@StudyStartMonth"
                                    + ",@FamilyExpensePerMonth=@FamilyExpensePerMonth,@OtherExpensePerMonth=@OtherExpensePerMonth";
                                    Com1.CommandText = sql;
                                    Com1.Parameters.Clear();
                                    Com1.Parameters.AddWithValue("@LoanAppID", ServerLoanAppID);
                                    Com1.Parameters.AddWithValue("@StudyMonthAmount", StudyMonthAmount);
                                    Com1.Parameters.AddWithValue("@StudyStartMonth", StudyStartMonth);
                                    Com1.Parameters.AddWithValue("@FamilyExpensePerMonth", FamilyExpensePerMonth);
                                    Com1.Parameters.AddWithValue("@OtherExpensePerMonth", OtherExpensePerMonth);
                                    DataTable dt2 = new DataTable();
                                    dt2.Load(Com1.ExecuteReader());
                                    ServerLoanAppCashFlowID = dt2.Rows[0][0].ToString();
                                    //ServerLoanAppCashFlowID = "1";
                                    #endregion CashFlow
                                    #region MSI
                                    foreach (var lp2 in lp.MSI)
                                    {
                                        #region MSI
                                        string ServerLoanAppCashFlowMSIID = "";
                                        string IncomeTypeID = lp2.IncomeTypeID;
                                        string MainSourceIncomeID = lp2.MainSourceIncomeID;
                                        string Remark = lp2.Remark;
                                        string Quantity = lp2.Quantity;
                                        string ExAge = lp2.ExAge;
                                        string BusAge = lp2.BusAge;
                                        string isMSI = lp2.isMSI;
                                        sql = "exec T24_LoanAppCashFlowMSIAddEdit @ServerLoanAppCashFlowID=@ServerLoanAppCashFlowID,@IncomeTypeID=@IncomeTypeID"
                                        + ",@MainSourceIncomeID=@MainSourceIncomeID,@Remark=@Remark,@Quantity=@Quantity,@ExAge=@ExAge,@BusAge=@BusAge,@isMSI=@isMSI";
                                        Com1.CommandText = sql;
                                        Com1.Parameters.Clear();
                                        Com1.Parameters.AddWithValue("@ServerLoanAppCashFlowID", ServerLoanAppCashFlowID);
                                        Com1.Parameters.AddWithValue("@IncomeTypeID", IncomeTypeID);
                                        Com1.Parameters.AddWithValue("@MainSourceIncomeID", MainSourceIncomeID);
                                        Com1.Parameters.AddWithValue("@Remark", Remark);
                                        Com1.Parameters.AddWithValue("@Quantity", Quantity);
                                        Com1.Parameters.AddWithValue("@ExAge", ExAge);
                                        Com1.Parameters.AddWithValue("@BusAge", BusAge);
                                        Com1.Parameters.AddWithValue("@isMSI", isMSI);
                                        DataTable dt3 = new DataTable();
                                        dt3.Load(Com1.ExecuteReader());
                                        ServerLoanAppCashFlowMSIID = dt3.Rows[0][0].ToString();
                                        //ServerLoanAppCashFlowMSIID = "1";
                                        #endregion MSI
                                        #region MSIRegular
                                        foreach (var lp3 in lp2.MSIRegular)
                                        {
                                            string Description = lp3.Description;
                                            string Amount = lp3.Amount;
                                            string UnitID = lp3.UnitID;
                                            string Cost = lp3.Cost;
                                            string OneIncomeTwoExpense = lp3.OneIncomeTwoExpense;
                                            string CurrencyID = lp3.Currency;
                                            if (CurrencyID == "KHR")
                                            {
                                                CurrencyID = "1";
                                            }
                                            else if (CurrencyID == "USD")
                                            {
                                                CurrencyID = "2";
                                            }
                                            else
                                            {
                                                CurrencyID = "3";
                                            }
                                            string Month = lp3.Month;
                                            sql = "exec T24_LoanAppCashFlowMSIRegAddEdit @LoanAppCashFlowMSIID=@LoanAppCashFlowMSIID,@Description=@Description"
                                            + ",@Amount=@Amount,@UnitID=@UnitID,@Cost=@Cost,@OneIncomeTwoExpense=@OneIncomeTwoExpense,@CurrencyID=@CurrencyID,@Month=@Month";
                                            Com1.CommandText = sql;
                                            Com1.Parameters.Clear();
                                            Com1.Parameters.AddWithValue("@LoanAppCashFlowMSIID", ServerLoanAppCashFlowMSIID);
                                            Com1.Parameters.AddWithValue("@Description", Description);
                                            Com1.Parameters.AddWithValue("@Amount", Amount);
                                            Com1.Parameters.AddWithValue("@UnitID", UnitID);
                                            Com1.Parameters.AddWithValue("@Cost", Cost);
                                            Com1.Parameters.AddWithValue("@OneIncomeTwoExpense", OneIncomeTwoExpense);
                                            Com1.Parameters.AddWithValue("@CurrencyID", CurrencyID);
                                            Com1.Parameters.AddWithValue("@Month", Month);
                                            Com1.ExecuteNonQuery();
                                        }
                                        #endregion MSIRegular
                                        #region MSIIrregular
                                        foreach (var lp3 in lp2.MSIIrregular)
                                        {
                                            string Description = lp3.Description;
                                            string Amount = lp3.Amount;
                                            string UnitID = lp3.UnitID;
                                            string Cost = lp3.Cost;
                                            string OneIncomeTwoExpense = lp3.OneIncomeTwoExpense;
                                            string CurrencyID = lp3.Currency;
                                            if (CurrencyID == "KHR")
                                            {
                                                CurrencyID = "1";
                                            }
                                            else if (CurrencyID == "USD")
                                            {
                                                CurrencyID = "2";
                                            }
                                            else
                                            {
                                                CurrencyID = "3";
                                            }
                                            string Month = lp3.Month;
                                            sql = "exec T24_LoanAppCashFlowMSIIRegAddEdit @LoanAppCashFlowMSIID=@LoanAppCashFlowMSIID,@Description=@Description"
                                            + ",@Amount=@Amount,@UnitID=@UnitID,@Cost=@Cost,@OneIncomeTwoExpense=@OneIncomeTwoExpense,@CurrencyID=@CurrencyID,@Month=@Month";
                                            Com1.CommandText = sql;
                                            Com1.Parameters.Clear();
                                            Com1.Parameters.AddWithValue("@LoanAppCashFlowMSIID", ServerLoanAppCashFlowMSIID);
                                            Com1.Parameters.AddWithValue("@Description", Description);
                                            Com1.Parameters.AddWithValue("@Amount", Amount);
                                            Com1.Parameters.AddWithValue("@UnitID", UnitID);
                                            Com1.Parameters.AddWithValue("@Cost", Cost);
                                            Com1.Parameters.AddWithValue("@OneIncomeTwoExpense", OneIncomeTwoExpense);
                                            Com1.Parameters.AddWithValue("@CurrencyID", CurrencyID);
                                            Com1.Parameters.AddWithValue("@Month", Month);
                                            Com1.ExecuteNonQuery();
                                        }
                                        #endregion MSIIrregular

                                    }
                                    #endregion MSI

                                }
                                #endregion CashFlow

                            }
                        }
                        catch (Exception ex)
                        {
                            int line = c.GetLineNumber(ex);
                            ERR = "Error";
                            SMS = "Something was wrong while saving LoanApp: " + line.ToString()+" | "+ ex.Message.ToString();
                            LoanAppResSMS data = new LoanAppResSMS();
                            data.SMS = SMS;
                            SMSList.Add(data);
                        }
                        #region Commit or RollBack
                        try
                        {
                            if (ERR == "Error")
                            {
                                //Tran1.Rollback();
                            }
                            else
                            {
                                c.ReturnDT("update tblLoanApp1 set UploadERR=1 where LoanAppID='"+ ServerLoanAppID + "'");
                                //Tran1.Commit();
                                LoanAppResSMS data = new LoanAppResSMS();
                                data.SMS = "";
                                SMSList.Add(data);
                            }
                            Con1.Close();
                        }
                        catch { }
                        #endregion Commit or RollBack
                    }
                    #endregion add
                }
                #endregion LoanApp
            }
            catch(Exception ex)
            {
                ERR = "Error";
                SMS = "Something was wrong at line:"+c.GetLineNumber(ex)+" | Ex:"+ex.Message.ToString();
                LoanAppResSMS data = new LoanAppResSMS();
                data.SMS = SMS;
                SMSList.Add(data);
            }

            ListHeader.ERR = ERR;
            ListHeader.SMS = SMS;
            ListHeader.LoanClientID = RSIDOnDevice;
            ListHeader.LoanAppID = RSLoanAppID;
            ListHeader.SMSList = SMSList;
            ListHeader.ImgList = ImgList;
            RSData.Add(ListHeader);
            c.T24_AddLog("LoanAppPost_"+ api_name+"_" + ServerDate.Replace("-", "_").Replace(" ", "_").Replace(":", "_"), "RS", JsonConvert.SerializeObject(RSData), "LoanAppPost");
            return RSData;
        }
        string FirstNameForLog="";
    }
}