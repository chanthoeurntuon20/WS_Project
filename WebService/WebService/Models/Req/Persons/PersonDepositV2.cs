using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.Models.Req.Persons
{
    public class PersonDepositV2
    {
        public string CollateralClientID { get; set; }
        public string CollateralServerID { get; set; }
        public string LoanClientID { get; set; }
        public string LoanAppID { get; set; }
        public string CustClientID { get; set; }
        public string CustServerID { get; set; }
        public string FixedDepositAccountNo { get; set; }
        public string StartDate { get; set; }
        public string MaturityDate { get; set; }
        public string Amount { get; set; }
        public string AccountOwnerName { get; set; }
        public string Currency { get; set; }
        public string RelationshipID { get; set; }
        public string DOB { get; set; }
        public string GenderID { get; set; }
        public string NIDNo { get; set; }
        public string IssueDate { get; set; }
        public string IssuedBy { get; set; }
        public string SortAddress { get; set; }
        public string VillageID { get; set; }
    }
}