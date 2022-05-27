using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.Models.Req.CustomerAmls
{
    public class CustomerAmlReq
    {
        public int ID { get; set; }
        public string LoanAppID { get; set; }
        public string LoanAppPersonID { get; set; }
        public string BlockStatus { get; set; }
        public string WatchListScreeningStatus { get; set; }
        public string WatchListCaseUrl { get; set; }
        public string RiskProfiling { get; set; }
        public string AMLApprovalStatus { get; set; }
        public string WatchListExposition { get; set; }
        public string ProductAndService { get; set; }
        public string CreateBy { get; set; }
    }
}