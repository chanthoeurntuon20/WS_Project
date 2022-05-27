using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.Models.Req.Persons
{
    public class CreditorV2
    {
        public string CreditorClientID { get; set; }//old
        public string CreditorServerID { get; set; }//old
        public string LoanClientID { get; set; }//old
        public string LoanAppID { get; set; }//old
        public string CreditorID { get; set; }//old
        public string Currency { get; set; }//new
        public string ApprovedAmount { get; set; }//old
        public string OutstandingBalance { get; set; }//old
        public string InterestRate { get; set; }//old
        public string RepaymentTypeID { get; set; }//old
        public string RepaymentTermID { get; set; }//old
        public string LoanStartDate { get; set; }//old
        public string LoanEndDate { get; set; }//old
        public string IsReFinance { get; set; }//old
        public string ReFinanceAmount { get; set; }//old
        public string RemainingInstallment { get; set; }//old
        public string CustClientID { get; set; }//new
        public string CustServerID { get; set; }//new
        public string CreditorName { get; set; }//new

    }
}