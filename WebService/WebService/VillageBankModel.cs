using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService
{
    public class VillageBankModel
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
        public string ERRCode { get; set; }
        public List<VBList> DataList { get; set; }
    }
    public class VBList
    {
        public string VBID { get; set; }
        public string VBName { get; set; }
        public string Status { get; set; }
        public string COID { get; set; }
        public string MeetingDate { get; set; }
        public string ExpireDate { get; set; }
        public string COIDRotate { get; set; }
        public string GSTREET { get; set; }
        public string GADDRESS { get; set; }
        public string GTOWN { get; set; }
        public string GPOSTCODE { get; set; }
        public string AMKPROVINCE { get; set; }
        public string AMKDISTRICT { get; set; }
        public string AMKCOMMUNE { get; set; }
        public string AMKVILLAGE { get; set; }
    }
}