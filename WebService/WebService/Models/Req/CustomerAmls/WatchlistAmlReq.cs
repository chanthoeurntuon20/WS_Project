using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.Models.Req.CustomerAmls
{
    public class WatchlistAmlReq
    {
        public string CID { get; set; }
        public string LoanAppID { get; set; }
        public string LoanAppPersonID { get; set; }
    }
}