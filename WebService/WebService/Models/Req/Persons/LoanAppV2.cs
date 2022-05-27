using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebService.Models.LoanApp;
using WebService.Models.Req.CashFlows;
using WebService.Models.Req.Geolocations;

namespace WebService.Models.Req.Persons
{
    public class LoanAppV2
    {
        //old
        #region LoanApp
        public string LoanClientID { get; set; }//old - IDOnDevice
        public string LoanAppID { get; set; }//old
        public string LoanAppStatusID { get; set; }//old
        public string DeviceDate { get; set; }//old
        public string ProductID { get; set; }//ok
        public string LoanRequestAmount { get; set; }//old
        public string OwnCapital { get; set; }//old
        public string DisbursementDate { get; set; }//old
        public string FirstWithdrawal { get; set; }//old
        public string LoanTerm { get; set; }//old
        public string FirstRepaymentDate { get; set; }//old
        public string LoanInterestRate { get; set; }//old
        public string CustomerRequestRate { get; set; }//old - need to check
        public string CompititorRate { get; set; }//old - need to check
        public string CustomerConditionID { get; set; }//old - need to check
        public string COProposedAmount { get; set; }//old
        public string COProposedTerm { get; set; }//old
        public string COProposeRate { get; set; }//old
        public string FrontBackOfficeID { get; set; }//old - need to check
        public string GroupNumber { get; set; }//old
        public string LoanCycleID { get; set; }//old
        public string RepaymentHistoryID { get; set; }//old
        public string LoanReferralID { get; set; }//old - need to check
        public string DebtIinfoID { get; set; }//old
        public string MonthlyFee { get; set; }//old - need to check
        public string Compulsory { get; set; }//old
        public string CompulsoryTerm { get; set; }//old - need to check
        public string Currency { get; set; }//old
        public string UpFrontFee { get; set; }//old
        public string UpFrontAmt { get; set; }//old - need to check
        public string CompulsoryOptionID { get; set; }//old
        public string FundSource { get; set; }//old
        public string IsNewCollateral { get; set; }//old
        public string AgriBuddy { get; set; }//old
        public string SemiBallonFreqID { get; set; }//old

        public string LoanTypeID { get; set; }//new
        public string PaymentMethodID { get; set; }//new
        public string GracePeriodID { get; set; }//new
        public string MITypeID { get; set; }//new
        public string DeskCheckID { get; set; } = "0";
        public string PreCheckID { get; set; } = "0";
        public string CBSKey { get; set; } = "";

        public string AMApproveAmt { get; set; }
        public string CollateralDebt { get; set; }

        public string CBCRef { get; set; }
        public GeolocationLoanCreation Geolocation { get; set; }
        public List<LoanAppPurchaseProperty> LoanAppPurchaseProperties { get; set; }

        #endregion LoanApp
        public List<PersonV2> Person;//old
        public List<PurposeV2> Purpose;//new
        public List<CashFlowV2> CashFlow;//old
    }
}