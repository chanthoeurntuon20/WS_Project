using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.Models.Res.Reports
{
    public class CommissionReportByOfficeIDGetV21List
    {
        public string ProspectCode { get; set; }
        public string OfficeName { get; set; }
        public string COName { get; set; }
        public string ClientName { get; set; }
        public string VBName { get; set; }
        public string AAID { get; set; }
        public string ProductID { get; set; }
        public string DisbDate { get; set; }
        public string Currency { get; set; }
        public string ActualDisbAmt { get; set; }
        public string Mnemonic { get; set; }
        public string OfficeID { get; set; }
        public string CommissionAmt { get; set; }
    }
}