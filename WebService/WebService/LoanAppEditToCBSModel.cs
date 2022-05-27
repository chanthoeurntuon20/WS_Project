using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService
{
    public class LoanAppEditToCBSModel
    {
        public string user { get; set; } = "";
        public string pwd { get; set; } = "";
        public string device_id { get; set; } = "";
        public string app_vName { get; set; } = "";
        public List<LoanAppEditToCBS> LoanAppEditToCBS;
    }
    public class LoanAppEditToCBS
    {
        public string LoanAppID { get; set; }
        public string DisbursementDate { get; set; }
        public string AMApproveTerm { get; set; }
        public string AMApproveAmt { get; set; }
        public string AMApproveRate { get; set; }
        public string FirstRepaymentDate { get; set; }
        public string LoanCycle { get; set; }
        public string GroupNumber { get; set; }
        public string MainBusinessID { get; set; }
        public string LoanPurposeID { get; set; }
        public string CBSKey { get; set; }
        public string CBCREQUIRED { get; set; }
    }
}