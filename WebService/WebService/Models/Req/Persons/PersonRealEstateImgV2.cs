using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.Models.Req.Persons
{
    public class PersonRealEstateImgV2
    {
        public string ImageClientID { get; set; }
        public string ImageServerID { get; set; }
        public string CollateralClientID { get; set; }
        public string CollateralServerID { get; set; }
        public string CreateDateClient { get; set; }
        public string Ext { get; set; }
        public string ImgPath { get; set; }
        public string Remark { get; set; }
    }
}