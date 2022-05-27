using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService
{
    public class DisbPostFromDeviceModel
    {
        public string user { get; set; }
        public string pwd { get; set; }
        public string device_id { get; set; }
        public string app_vName { get; set; }
        public List<DisbList> DisbList;
    }
    public class DisbList
    {
        public string DisburseID { get; set; }
        public string FeeAmount { get; set; }
        public string ApprovedAmount { get; set; }
        public string Ref { get; set; }
        public string RefFee { get; set; }
        public string DeviceDate { get; set; }
    }
}