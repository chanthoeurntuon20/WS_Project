using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.Models.Req.CashFlows
{
    public class CashFlowMSIInExV2
    {
        public string LoanAppCashFlowMSIInExClientID { get; set; }
        public string LoanAppCashFlowMSIInExServerID { get; set; }
        public string CashFlowMSIClientID { get; set; }
        public string CashFlowMSIServerID { get; set; }
        public string InExCodeID { get; set; }
        public string Description { get; set; }
        public string Month { get; set; }
        public string Amount { get; set; }
        public string UnitID { get; set; }
        public string Cost { get; set; }
        public string OneIncomeTwoExpense { get; set; }
    }
}