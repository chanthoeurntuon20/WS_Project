using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.Models.Req.Persons
{
    public class PersonKYC
    {
        public string KYCServerID { get; set; }
        public string KYCClientID { get; set; }
        public string IDCardType { get; set; }
        public string IDNumber { get; set; }
        public string IDIssueDate { get; set; }
        public string IDExpireDate { get; set; }
        public string IsNew { get; set; }//0=old | 1=New
        public string IsCurrentKYC { get; set; }
        public List<PersonKYCImage> PersonKYCImage;//KYC Image
    }
}