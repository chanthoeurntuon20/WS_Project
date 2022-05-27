using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService
{
    public class LoanAppModel
    {
        public string user { get; set; }
        public string pwd { get; set; }
        public string device_id { get; set; }
        public string app_vName { get; set; }
        public List<LoanApp> LoanApp;
        

    }
    public class LoanApp
    {
        #region LoanApp
        public string IDOnDevice { get; set; }
        //public string LoanAppID { get; set; }
        //public string LoanAppStatusID { get; set; }
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
        public string MonthlyFee { get; set; }
        public string Compulsory { get; set; }
        //public string CompulsoryTerm { get; set; }
        public string Currency { get; set; }
        public string UpFrontFee { get; set; }
        public string UpFrontAmt { get; set; }
        //public string isCBCCheck { get; set; }
        public string CompulsoryOptionID { get; set; }
        public string FundSource { get; set; }
        public string IsNewCollateral { get; set; }
        #endregion LoanApp
        public List<PurposeDetail> PurposeDetail;
        public List<Person> Person;        
        public List<Opinion> Opinion;
        public List<CashFlow> CashFlow;
    }
    public class PurposeDetail
    {
        public string LoanAppPurposeDetail { get; set; }
        public string Quantity { get; set; }
        public string UnitPrice { get; set; }
    }
    public class Person
    {
        #region Person
        public string LoanAppPersonID { get; set; }
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
        public string DeviceDate { get; set; }
        public string AltName2 { get; set; }
        #endregion Person
        public List<Creditor> Creditor;
        public List<ClientAsset> ClientAsset;
        public List<ClientBusiness> ClientBusiness;
        public List<ClientCollateral> ClientCollateral;
        public List<GuarantorBusiness> GuarantorBusiness;
        public List<GuarantorAsset> GuarantorAsset;
        public List<PersonImg> PersonImg;
    }
    public class Creditor
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
        public string LoanAppCreditorID { get; set; }
    }
    public class ClientAsset
    {
        public string Description { get; set; }
        public string Quantity { get; set; }
        public string UnitPrice { get; set; }
    }
    public class ClientBusiness
    {
        public string Description { get; set; }
        public string Quantity { get; set; }
        public string UnitPrice { get; set; }
    }
    public class ClientCollateral
    {
        public string LoanAppClientCollateralID { get; set; }
        public string ColleteralTypeID { get; set; }
        public string ColleteralDocTypeID { get; set; }
        public string ColleteralDocNumber { get; set; }
        public string Description { get; set; }
        public string Quantity { get; set; }
        public string UnitPrice { get; set; }

        public List<ClientCollateralImg> ClientCollateralImg;
    }
    public class ClientCollateralImg
    {
        public string ImgName { get; set; }
        public string Ext { get; set; }
    }

    public class GuarantorBusiness
    {
        public string Description { get; set; }
        public string NetProfitPerYear { get; set; }
    }
    public class GuarantorAsset
    {
        public string Description { get; set; }
        public string Quantity { get; set; }
        public string UnitPrice { get; set; }
    }
    public class PersonImg
    {
        public string ImgName { get; set; }
        public string Ext { get; set; }
        public string OneCardTwoDoc { get; set; }
    }

    public class Opinion
    {
        public string Description { get; set; }
        //public string CreateBy { get; set; }
        public string DeviceDate { get; set; }
    }
    public class CashFlow
    {
        public string StudyMonthAmount { get; set; }
        public string StudyStartMonth { get; set; }
        public string FamilyExpensePerMonth { get; set; }
        public string OtherExpensePerMonth { get; set; }
        public List<MSI> MSI;
    }
    public class MSI
    {
        public string IncomeTypeID { get; set; }
        public string MainSourceIncomeID { get; set; }
        public string Remark { get; set; }
        public string Quantity { get; set; }
        public string ExAge { get; set; }
        public string BusAge { get; set; }
        public string isMSI { get; set; }
        public List<MSIRegular> MSIRegular;
        public List<MSIIrregular> MSIIrregular;
    }
    public class MSIRegular
    {
        public string Description { get; set; }
        public string Amount { get; set; }
        public string UnitID { get; set; }
        public string Cost { get; set; }
        public string OneIncomeTwoExpense { get; set; }
        public string Currency { get; set; }
        public string Month { get; set; }
    }
    public class MSIIrregular
    {
        public string Description { get; set; }
        public string Amount { get; set; }
        public string UnitID { get; set; }
        public string Cost { get; set; }
        public string OneIncomeTwoExpense { get; set; }
        public string Currency { get; set; }
        public string Month { get; set; }
    }

}