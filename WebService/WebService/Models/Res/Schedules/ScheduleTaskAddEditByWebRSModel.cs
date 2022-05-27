using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.Models.Res.Schedules
{
    public class ScheduleTaskAddEditByWebRSModel
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
        public string ERRCode { get; set; }
        public List<ScheduleTaskAddEditByWebRSList> DataList { get; set; }
    }
    public class ScheduleTaskAddEditByWebRSList
    {
        public string TaskID { get; set; }

    }
}