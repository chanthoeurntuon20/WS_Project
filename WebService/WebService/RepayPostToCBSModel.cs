using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService
{
    public class RepayPostToCBSModel
    {
        public string user { get; set; }
        public string pwd { get; set; }
        public string device_id { get; set; }
        public string app_vName { get; set; }
        public List<RepayParam> RepayParam;
    }
    public class RepayParam
    {
        public string TraDate { get; set; }
        public string COUserID { get; set; }
        public string DataType { get; set; }
        public string UserID { get; set; }

    }
}