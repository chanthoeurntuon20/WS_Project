using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.Models.Req.Schedules
{
    public class ReportCOByWebRQ
    {
        public string UserOwnerID { get; set; }
        public string PMuserID { get; set; }
        public string BMUserID { get; set; }
        public string AMUserID { get; set; }
        public string OwnerType { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}