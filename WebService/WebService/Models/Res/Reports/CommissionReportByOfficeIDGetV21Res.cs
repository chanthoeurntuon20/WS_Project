using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.Models.Res.Reports
{
    public class CommissionReportByOfficeIDGetV21Res
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
        public List<CommissionReportByOfficeIDGetV21List> DataList { get; set; }
    }
}