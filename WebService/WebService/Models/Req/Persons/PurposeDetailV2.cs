using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.Models.Req.Persons
{
    public class PurposeDetailV2
    {
        public string LoanPurposeDetailClientID { get; set; }
        public string LoanPurposeDetailServerID { get; set; }
        public string LoanPurposeClientID { get; set; }
        public string LoanPurposeServerID { get; set; }
        public string LoanAppPurpsoeDetail { get; set; }
        public string Quantity { get; set; }
        public string UnitPrice { get; set; }
    }
}