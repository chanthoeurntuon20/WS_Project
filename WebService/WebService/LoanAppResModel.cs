using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService
{
    public class LoanAppResModel
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
        public string ERRCode { get; set; }
        public string LoanClientID { get; set; }//old - IDOnDevice
        public string LoanAppID { get; set; }
        public List<LoanAppResSMS> SMSList { get; set; }
        public List<LoanAppResImgList> ImgList { get; set; }
    }
    public class LoanAppResSMS
    {
        public string SMS { get; set; }
    }
    public class LoanAppResImgList
    {
        public string ImgType { get; set; }
        public string ClientID { get; set; }
        public string ServerID { get; set; }
        public string OriImgName { get; set; }
        public string ServerImgName { get; set; }
    }

}