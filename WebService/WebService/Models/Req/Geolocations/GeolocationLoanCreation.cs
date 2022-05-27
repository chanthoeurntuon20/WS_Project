using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.Models.Req.Geolocations
{
    public class GeolocationLoanCreation
    {
        public string LoanAppID { get; set; }
        public string ParentID { get; set; }
        public string BranchName { get; set; }
        public string AMName { get; set; }
        public string COName { get; set; }
        public string VBName { get; set; }
        public string AMKDL { get; set; }
        public string LoanAA { get; set; }
        public string CID {get; set;}
        public string CustName { get; set;}
        public string LoanProduct { get; set;}
        public string LoanCurrency { get; set;}
        public decimal LoanAmount { get; set;}
        public string DateIn { get; set;}
        public string LoanCreateGeoLocation { get; set;}
        public string LoanSubmitGeoLocation { get; set;}
        public string LoanStartDate { get; set;}
        public string LoanEndDate { get; set; }
        public string CashFlowStartDate { get; set;}
        public string CashFlowEndDate { get; set;}
        public string CashFlowStartGeoLocation { get; set;}
        public string CashFLowEndGeoLocation { get; set;}
        public string LoanStatus { get; set;}

    }
}