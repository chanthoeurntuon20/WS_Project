using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService
{
    public class CustomerGetByRangRQModel {
        public string criteriaValue { get; set; }
        public string criteriaValue2 { get; set; }
    }
    public class CustomerGetByRangRQModelV2
    {
        public string UserName { get; set; }
        public string criteriaValue { get; set; }//"1,10"
    }

    public class CustomerModel
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
        public string ERRCode { get; set; }
        public List<CustList> DataList { get; set; }
    }
    public class CustList
    {
        public string CUSTID { get; set; }
        public string CUSTTYPE { get; set; }
        public string SHORTNAME { get; set; }
        public string NAME1 { get; set; }
        public string BIRTHINCORPDATE { get; set; }
        public string AMKBIRTHPLACE { get; set; }
        public string GENDER { get; set; }
        public string MARITALSTATUS { get; set; }
        public string RESIDEYN { get; set; }
        public string NATIONALITY { get; set; }
        public string RESIDENCE { get; set; }
        public string AMKIDTYPE { get; set; }
        public string AMKIDNO { get; set; }
        public string AMKIDISSDATE { get; set; }
        public string AMKIDEXDATE { get; set; }
        public string STREET { get; set; }
        public string AMKPROVINCE { get; set; }
        public string AMKDISTRICT { get; set; }
        public string AMKCOMMUNE { get; set; }
        public string AMKVILLAGE { get; set; }
        public string POSTALCODE { get; set; }
        public string TELMOBILE { get; set; }
        public string EMAILADDRESS { get; set; }
        public string AMKOCCUPTYPE { get; set; }
        public string AMKOCCUPDET { get; set; }
        public string SPMEMNO { get; set; }
        public string SPNAME { get; set; }
        public string AMKSPDOB { get; set; }
        public string AMKSPIDTYPE { get; set; }
        public string AMKSPIDNO { get; set; }
        public string AMKSPIDISDT { get; set; }
        public string AMKSPIDEXDT { get; set; }
        public string PROFESSION { get; set; }
        public string AMKPOVERTYST { get; set; }
        public string NOOFDEPEND { get; set; }
        public string MAININCOME { get; set; }
        public string TITLE { get; set; }
        public string AMKVILLAGEBK { get; set; }
        public string AMKNOACTMEM { get; set; }
        public string KhmerName { get; set; }
        public string KhmerFirstName { get; set; }
        public string KhmerLastName { get; set; }
        public string LocationCode { get; set; }
        public string imgUrl { get; set; }
    }

}