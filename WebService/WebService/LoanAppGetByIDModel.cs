using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService
{
    public class LoanAppGetByIDModel
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
        public List<LoanAppGetByID> LoanApp;
        public List<ErrorListGetByID> ErrorListGetByID;

    }
    public class LoanAppGetByID
    {
        #region LoanApp
        public string IDOnDevice { get; set; }
        public string LoanAppID { get; set; }
        public string LoanAppStatusID { get; set; }
        public string DeviceDate { get; set; }
        public string ProductID { get; set; }
        public string LoanRequestAmount { get; set; }
        public string LoanPurposeID1 { get; set; }
        public string LoanPurposeID2 { get; set; }
        public string LoanPurposeID3 { get; set; }
        public string OwnCapital { get; set; }
        public string DisbursementDate { get; set; }
        public string FirstWithdrawal { get; set; }
        public string LoanTerm { get; set; }
        public string FirstRepaymentDate { get; set; }
        public string LoanInterestRate { get; set; }
        public string CustomerRequestRate { get; set; }
        public string CompititorRate { get; set; }
        public string CustomerConditionID { get; set; }
        public string COProposedAmount { get; set; }
        public string COProposedTerm { get; set; }
        public string COProposeRate { get; set; }
        public string FrontBackOfficeID { get; set; }
        public string GroupNumber { get; set; }
        public string LoanCycleID { get; set; }
        public string RepaymentHistoryID { get; set; }
        public string LoanReferralID { get; set; }
        public string DebtIinfoID { get; set; }

        public string CBSKey { get; set; }
        public string AMDebtFound { get; set; }
        public string AMApproveAmt { get; set; }
        public string AMApproveTerm { get; set; }
        public string AMApproveRate { get; set; }
        public string AMOpinion { get; set; }

        public string MonthlyFee { get; set; }
        public string Compulsory { get; set; }
        public string CompulsoryTerm { get; set; }
        public string CreateBy { get; set; }
        public string DeskCheckID { get; set; }
        public string PreCheckID { get; set; }
        public string Currency { get; set; }
        public string UpFrontFee { get; set; }
        public string UpFrontAmt { get; set; }
        #endregion LoanApp
        public List<PurposeDetailGetByID> PurposeDetail;
        public List<PersonGetByID> Person;
        public List<OpinionGetByID> Opinion;
        public List<CashFlowGetByID> CashFlow;
        
    }
    public class PurposeDetailGetByID
    {
        public string LoanAppPurposeDetail { get; set; }
        public string Quantity { get; set; }
        public string UnitPrice { get; set; }
    }
    public class PersonGetByID
    {
        #region Person
        //public string LoanAppPersonID { get; set; }
        public string LoanAppPersonTypeID { get; set; }
        public string PersonID { get; set; }
        public string CustomerID { get; set; }
        public string Number { get; set; }
        public string VillageBankID { get; set; }
        public string AltName { get; set; }
        public string TitleID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string GenderID { get; set; }
        public string DateOfBirth { get; set; }
        public string IDCardTypeID { get; set; }
        public string IDCardNumber { get; set; }
        public string IDCardExpireDate { get; set; }
        public string IDCardIssuedDate { get; set; }
        public string MaritalStatusID { get; set; }
        public string EducationID { get; set; }
        public string CityOfBirthID { get; set; }
        public string Telephone3 { get; set; }
        public string VillageIDPermanent { get; set; }
        public string LocationCodeIDPermanent { get; set; }
        public string VillageIDCurrent { get; set; }
        public string LocationCodeIDCurrent { get; set; }
        public string SortAddress { get; set; }
        public string FamilyMember { get; set; }
        public string FamilyMemberActive { get; set; }
        public string PoorID { get; set; }
        //public string DeviceDate { get; set; }
        #endregion Person
        public List<AccountListGetByID> AccountList;
        public List<CreditorGetByID> Creditor;
        public List<ClientAssetGetByID> ClientAsset;
        public List<ClientBusinessGetByID> ClientBusiness;
        public List<ClientCollateralGetByID> ClientCollateral;
        public List<GuarantorBusinessGetByID> GuarantorBusiness;
        public List<GuarantorAssetGetByID> GuarantorAsset;
        public List<PersonImgGetByID> PersonImg;
        public List<CBCReportGetByID> CBCReport;
        
    }
    public class CreditorGetByID
    {
        public string CreditorID { get; set; }
        public string ApprovedAmount { get; set; }
        public string OutstandingBalance { get; set; }
        public string InterestRate { get; set; }
        public string RepaymentTypeID { get; set; }
        public string RepaymentTermID { get; set; }
        public string LoanStartDate { get; set; }
        public string LoanEndDate { get; set; }
        public string RemainingInstallment { get; set; }
        //public string LoanAppCreditorID { get; set; }
    }
    public class ClientAssetGetByID
    {
        public string Description { get; set; }
        public string Quantity { get; set; }
        public string UnitPrice { get; set; }
    }
    public class ClientBusinessGetByID
    {
        public string Description { get; set; }
        public string Quantity { get; set; }
        public string UnitPrice { get; set; }
    }
    public class ClientCollateralGetByID
    {
        //public string LoanAppClientCollateralID { get; set; }
        public string ColleteralTypeID { get; set; }
        public string ColleteralDocTypeID { get; set; }
        public string ColleteralDocNumber { get; set; }
        public string Description { get; set; }
        public string Quantity { get; set; }
        public string UnitPrice { get; set; }

        public List<ClientCollateralImgGetByID> ClientCollateralImg;
    }
    public class ClientCollateralImgGetByID
    {
        public string ImgName { get; set; }
        public string Ext { get; set; }
    }

    public class GuarantorBusinessGetByID
    {
        public string Description { get; set; }
        public string NetProfitPerYear { get; set; }
    }
    public class GuarantorAssetGetByID
    {
        public string Description { get; set; }
        public string Quantity { get; set; }
        public string UnitPrice { get; set; }
    }
    public class PersonImgGetByID
    {
        public string ImgName { get; set; }
        public string Ext { get; set; }
        public string OneCardTwoDoc { get; set; }
    }
    public class CBCReportGetByID
    {
        public string Remark { get; set; }
    }
    public class AccountListGetByID
    {
        public string CustomerID { get; set; }
        public string AccountID { get; set; }
        public string CATEGORY { get; set; }
        public string CATEGDESC { get; set; }
        public string Balance { get; set; }
        public string Restrictions { get; set; }
        public string CURRENCY { get; set; }
    }

    public class OpinionGetByID
    {
        public string Description { get; set; }
        public string CreateBy { get; set; }
        //public string DeviceDate { get; set; }
    }
    public class CashFlowGetByID
    {
        public string StudyMonthAmount { get; set; }
        public string StudyStartMonth { get; set; }
        public string FamilyExpensePerMonth { get; set; }
        public string OtherExpensePerMonth { get; set; }
        public List<MSIGetByID> MSI;
    }
    public class MSIGetByID
    {
        public string IncomeTypeID { get; set; }
        public string MainSourceIncomeID { get; set; }
        public string Remark { get; set; }
        public string Quantity { get; set; }
        public string ExAge { get; set; }
        public string BusAge { get; set; }
        public string isMSI { get; set; }
        public List<MSIRegularGetByID> MSIRegular;
        public List<MSIIrregularGetByID> MSIIrregular;
    }
    public class MSIRegularGetByID
    {
        public string Description { get; set; }
        public string Amount { get; set; }
        public string UnitID { get; set; }
        public string Cost { get; set; }
        public string OneIncomeTwoExpense { get; set; }
        public string CurrencyID { get; set; }
        public string Month { get; set; }
    }
    public class MSIIrregularGetByID
    {
        public string Description { get; set; }
        public string Amount { get; set; }
        public string UnitID { get; set; }
        public string Cost { get; set; }
        public string OneIncomeTwoExpense { get; set; }
        public string CurrencyID { get; set; }
        public string Month { get; set; }
    }

    public class ErrorListGetByID
    {
        public string SMS { get; set; }
    }


}