using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.Models.Req.Persons
{
    public class PersonKYCImage
    {
        public string KYCImageServerID { get; set; }
        public string KYCImageClientID { get; set; }
        public string Ext { get; set; }
        public string ImgPath { get; set; }
        public string Remark { get; set; }
    }
}