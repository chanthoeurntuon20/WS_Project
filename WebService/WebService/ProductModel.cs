using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService
{
    public class ProductModel
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
        public string ERRCode { get; set; }
        public List<ProList> DataList { get; set; }
    }
    public class ProList
    {
        public string AbacusProductCode { get; set; }
        public string PRODUCTID { get; set; }
        public string PRODUCTDESC { get; set; }
        public string CATEGORY { get; set; }
        public string CURRENCY { get; set; }
        public string BOFO { get; set; }
        public string RATETYPE { get; set; }
        public string EFFDATE { get; set; }
        public string MINTERM { get; set; }
        public string MAXTERM { get; set; }
        public string MINAMOUNT { get; set; }
        public string MAXAMOUNT { get; set; }
        public string MINRATE { get; set; }
        public string MAXRATE { get; set; }
        public string DEFRATE { get; set; }
        public string MINAGE { get; set; }
        public string MAXAGE { get; set; }
        public string REPAYTYPE { get; set; }
        public string MINUPFRONTFEE { get; set; }
        public string MAXUPFRONTFEE { get; set; }
        public string MINMNTHTXNFEE { get; set; }
        public string MAXMNTHTXNFEE { get; set; }
    }
}