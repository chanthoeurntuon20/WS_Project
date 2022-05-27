using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.Models.Res.Schedules
{
    public class ScheduleTaskAddEditByTabRSModel
    {
        public string ERR { get; set; }//Error | succeed
        public string SMS { get; set; }
        public string ERRCode { get; set; }
        public List<ScheduleTaskAddEditByTabRSList> DataList { get; set; }
    }
    public class ScheduleTaskAddEditByTabRSList
    {
        public string TaskID { get; set; }
        public string ClientID { get; set; }
        public string ERR { get; set; }
        public string SMS { get; set; }
    }
}