using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.Models.Res.CustomerAmls
{
    public class WatchlistAmlRes
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public WatchlistAml WatchlistAml { get; set; }
    }
    public class WatchlistAml
    {
        public string CID { get; set; }
        public string BlockStatus { get; set; }
        public string WatchListScreeningStatus { get; set; }
        public string WatchListCaseUrl { get; set; }
        public string RiskProfiling { get; set; }
        public string AMLApprovalStatus { get; set; }
        public string WatchListExposition { get; set; }
        public string ProductAndService { get; set; }
    }
}