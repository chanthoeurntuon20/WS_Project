using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
 
namespace WebService
{
    public class LoanAppApproveToCBCModel
    {
        public string user { get; set; }
        public string pwd { get; set; }
        public string device_id { get; set; }
        public string app_vName { get; set; }
        public List<LoanAppApproveToCBS> LoanAppApproveToCBS;
    }
    public class LoanAppApproveToCBS
    {
        public string LoanAppID { get; set; }
        public string DeskCheckID { get; set; }
        public string PreCheckID { get; set; }
        public string AMDebtFound { get; set; }
        public string AMOpinion { get; set; }
        public string AMApproveAmt { get; set; }
        public string AMApproveTerm { get; set; }
        public string AMApproveRate { get; set; }
        public string GroupNumber { get; set; }
        public string DisbursementDate { get; set; }
        public string FirstRepaymentDate { get; set; }
        public string CBSKey { get; set; }
        public string AccountID { get; set; }
    }
}