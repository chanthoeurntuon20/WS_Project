namespace AccountCreationToCBSRes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;


    public class AccountCreationToCBSModel
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
        public List<DataList> DataList { get; set; }
    }
    public class DataList
    {
        public string AccountID { get; set; }
        public string ProductCode { get; set; }
        public string CATEGDESC { get; set; }
        public string AVAILBAL { get; set; }
    }
}