using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.Models.Req.Persons
{
    public class PurposeV2
    {
        public string LoanPurposeClientID { get; set; }
        public string LoanPurposeServerID { get; set; }
        public string LoanClientID { get; set; }
        public string LoanAppID { get; set; }
        public string LoanPurposeID { get; set; }
        public List<PurposeDetailV2> PurposeDetail;
    }
}