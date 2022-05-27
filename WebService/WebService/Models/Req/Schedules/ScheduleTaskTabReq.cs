using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.Models.Req.Schedules
{
    public class ScheduleTaskTabReq
    {
        public string Action { get; set; }
        public string TaskID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PlanDateStart { get; set; }
        public string PlanTimeStart { get; set; }
        public string PlanDateEnd { get; set; }
        public string PlanTimeEnd { get; set; }
        public string ActualDateStart { get; set; }
        public string ActualTimeStart { get; set; }
        public string ActualDateEnd { get; set; }
        public string ActualTimeEnd { get; set; }
        public string OwnerUserID { get; set; }
        public string OwnerUserText { get; set; }
        public string TaskTypeID { get; set; }
        public string TaskStatusID { get; set; }
        public string Remark { get; set; }
        public string CreateDate { get; set; }
        public string ClientID { get; set; }
    }
}