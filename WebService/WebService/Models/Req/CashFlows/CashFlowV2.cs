using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.Models.Req.CashFlows
{
    public class CashFlowV2
    {
        public string CashFlowClientID { get; set; }//old
        public string CashFlowServerID { get; set; }//old
        public string LoanClientID { get; set; }
        public string LoanAppID { get; set; }
        public string StudyMonthAmount { get; set; }
        public string StudyStartMonth { get; set; }
        public string FamilyExpensePerMonth { get; set; }
        public string OtherExpensePerMonth { get; set; }
        public List<CashFlowMSIV2> CashFlowMSI;
    }
}