using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService
{
    public class AddressModel
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
        public string ERRCode { get; set; }
        public List<AddressList> DataList { get; set; }
    }
    public class AddressList
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string ParentID { get; set; }
        public string LevelID { get; set; }
    }

}