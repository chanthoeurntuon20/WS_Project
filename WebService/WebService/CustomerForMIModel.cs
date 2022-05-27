using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService
{
    public class CustomerForMIModel
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
        public List<CustForMIList> DataList { get; set; }
    }
    public class CustForMIList
    {
        public string customerid { get; set; }
        public string enname { get; set; }
        public string maritalcode { get; set; }        
        public string identifyID { get; set; }
        public string telephone { get; set; }
        public string gender { get; set; }
        public string identifytype { get; set; }
        public string dateofbirth { get; set; }
        public string province { get; set; }
        public string district { get; set; }
        public string commune { get; set; }
        public string village { get; set; }
        public string spousenameEN { get; set; }
        public string spouseGender { get; set; }
        public string spouseDateBirth { get; set; }
        public string Nationality { get; set; }
    }

}