using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.Models.Req.LoanApp
{
    public class LoanAppCustomerReq
    {
        public string LoanAppId { get; set; }
        public string LoanAppPersonId { get; set; }
        public string CustomerId { get; set; }
        public string LoanAppPersonTypeId { get; set; }
        public string MaritalStatusId { get; set; }
        public string InstId { get; set; }
        
    }
}