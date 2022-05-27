using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.Models.Req.Reports
{
    public class CommissionReportByOfficeIDGetV21Req
    {
        public string User { get; set; }
        public string App_vName { get; set; }
        public string OfficeID { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
}