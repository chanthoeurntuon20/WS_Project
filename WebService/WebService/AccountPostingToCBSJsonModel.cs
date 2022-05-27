using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService
{
    public class AccountPostingToCBSJsonModel
    {
        public string user { get; set; }
        public string pwd { get; set; }
        public string device_id { get; set; }
        public string app_vName { get; set; }
        public string CreCompany { get; set; }
        public string TRN_TYPE { get; set; } // Transaction type
        public string DEBITACCTNO { get; set; }
        public string DEBITCURRENCY { get; set; }
        public string DEBITAMOUNT { get; set; }
        public string DEBITVALUEDATE { get; set; }
        public string CREDITACCTNO { get; set; }
        public string CREDITCURRENCY { get; set; }
        public string ORDERINGBANK { get; set; } // PV,RV
        public string PAYMENTDETAILS { get; set; } // Paymentdetail or description of payment
    }
}