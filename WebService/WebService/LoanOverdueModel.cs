using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService
{

    public class JsonResponse<T>
    {
        public string ERR { get; set; }
        public string SMS { get; set; } = "";
        public T Data { get; set; }
    }

    public class LoanOverdueModel
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
        public List<LoanOverdueList> DataList { get; set; }
    }

    public class LoanOverdueList
    {
        public string Id { get; set; }
        public string LoanAA { get; set; }
        public string LoanAcc { get; set; }
        public string Operation { get; set; }
        public string VBName { get; set; }
        public string CID { get; set; }
        public string CustName { get; set; }
        public string PhoneNo { get; set; }
        public string ProductType { get; set; }
        public string DisbDate { get; set; }
        public string Maturity { get; set; }
        public string Currency { get; set; }
        public string DisbAmount { get; set; }
        public string OutStanding { get; set; }
        public string SavingBalance { get; set; }
        public string TotalDue { get; set; }
        public string PrinDue { get; set; }
        public string IntDue { get; set; }
        public string MthlyDue { get; set; }
        public string PenaltyDue { get; set; }
        public string Arrear { get; set; }
        public string DateAdded { get; set; }
    }


    public class LoanOverdueLogsModel
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
        public List<LoanOverdueLogsList> DataList { get; set; }
    }
    public class LoanOverdueLogsList
    {
        public string Id { get; set; }
        public string LoanAcc { get; set; }
        public string OverdueType { get; set; }
        public string MainReason { get; set; }
        public string Reason { get; set; }
        public string SolveBy { get; set; }
        public string CutomerRating { get; set; }
        public string ManagementAction { get; set; }
        public string AccuracyOfUseCredit { get; set; }
        public string StatusOfSolutions { get; set; }
        public string PromisePaymentDate { get; set; }
        public string PromiseAmountCurrency { get; set; }
        public string PromisedAmount { get; set; }
        public string SourceOfMoneyPaid { get; set; }
        public string CutomerAttitude { get; set; }
        public string SourceOfIncome { get; set; }
        public string GuarantorCollateral { get; set; }
        public string DebtStatus { get; set; }
        public string FamilyStatus { get; set; }
        public string Comments { get; set; }
        public string PromisePaymentDateTS { get; set; }
    }

    public class LoanOverdueLogsResModel
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
    }

    public class LogsOverdueLogsPostingToJsonModel
    {
        public string LoanAcc { get; set; }
        public string OverdueType { get; set; }
        public string MainReason { get; set; }
        public string Reason { get; set; }
        public string SolveBy { get; set; }
        public string CutomerRating { get; set; }
        public string ManagementAction { get; set; }
        public string AccuracyOfUseCredit { get; set; }
        public string StatusOfSolutions { get; set; }
        public string PromisePaymentDate { get; set; }
        public string PromiseAmountCurrency { get; set; }
        public string PromisedAmount { get; set; }
        public string SourceOfMoneyPaid { get; set; }
        public string CutomerAttitude { get; set; }
        public string SourceOfIncome { get; set; }
        public string GuarantorCollateral { get; set; }
        public string DebtStatus { get; set; }
        public string FamilyStatus { get; set; }
        public string Comments { get; set; }
        public string UserId { get; set; }

    }





    public class NotificationModel
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
        public List<NotificationList> DataList { get; set; }
    }
    public class NotificationList
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public string Type { get; set; }
        public string LoanAcc { get; set; }
        public long Date { get; set; }

    }

    public class NotificationCountModel
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
        public string TotalCount { get; set; }
    }


    public class OverdueType
    {
        public string text { get; set; }
        public int value { get; set; }
    }

    public class MainReason
    {
        public string text { get; set; }
        public int value { get; set; }
        public List<Reason> reason { get; set; }
    }
    public class Reason
    {
        public string text { get; set; }
        public int value { get; set; }
    }
    public class CustomerRating
    {
        public string text { get; set; }
        public int value { get; set; }
    }
    public class ManagementAction
    {
        public string text { get; set; }
        public int value { get; set; }
    }
    public class AccuracyOfUseCredit
    {
        public string text { get; set; }
        public int value { get; set; }

    }
    public class StatusOfSolutions
    {
        public string text { get; set; }
        public int value { get; set; }
    }
    public class PromisedAmountCurrency
    {
        public string text { get; set; }
        public int value { get; set; }
    }
    public class SourceOfMoneyPaid
    {
        public string text { get; set; }
        public int value { get; set; }
    }
    public class CustomerAttitude
    {
        public string text { get; set; }
        public int value { get; set; }
    }
    public class SourceofIncome
    {
        public string text { get; set; }
        public int value { get; set; }
    }
    public class GuarantorCollateral
    {
        public string text { get; set; }
        public int value { get; set; }
    }
    public class DebtStatus
    {
        public string text { get; set; }
        public int value { get; set; }
    }
    public class FamilyStatus
    {
        public string text { get; set; }
        public int value { get; set; }
    }
    public class ListModel
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
        public List<OverdueType> OverdueType { get; set; }
        public List<MainReason> MainReason { get; set; }
        public List<CustomerRating> CustomerRating { get; set; }
        public List<ManagementAction> ManagementAction { get; set; }
        public List<AccuracyOfUseCredit> AccuracyOfUseCredit { get; set; }
        public List<StatusOfSolutions> StatusOfSolutions { get; set; }
        public List<PromisedAmountCurrency> PromisedAmountCurrency { get; set; }
        public List<SourceOfMoneyPaid> SourceOfMoneyPaid { get; set; }
        public List<CustomerAttitude> CustomerAttitude { get; set; }
        public List<SourceofIncome> SourceofIncome { get; set; }
        public List<GuarantorCollateral> GuarantorCollateral { get; set; }
        public List<DebtStatus> DebtStatus { get; set; }
        public List<FamilyStatus> FamilyStatus { get; set; }

    }




}