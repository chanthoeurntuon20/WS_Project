using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.Models.Req.Geolocations
{
    public class GeolocationLoanDisbusement
    {
        public string CID { get; set; }
        public string CustName { get; set; }
        public string LoanProduct { get; set; }
        public string LoanCurrency { get; set; }
        public decimal DisAmount { get; set; }
        public string AMKDL { get; set; }
        public string LoanAA { get; set; }
        public string DateIn { get; set; }
        public string Geolocation { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}