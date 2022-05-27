using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.Models.Req.Persons
{
    public class PersonImgV2
    {
        public string CustImageClientID { get; set; }
        public string CustImageServerID { get; set; }
        public string CustClientID { get; set; }
        public string CreateDateClient { get; set; }
        public string OneCardTwoDoc { get; set; }
        public string Ext { get; set; }
        public string ImgPath { get; set; }//Image Name on Client Side
        public string Remark { get; set; }
    }
}