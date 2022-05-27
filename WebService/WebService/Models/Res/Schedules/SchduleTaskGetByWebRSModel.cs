using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.Models.Res.Schedules
{
    public class SchduleTaskGetByWebRSModel
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
        public string ERRCode { get; set; }
        public List<SchduleTaskGetByWebRSList> DataList { get; set; }
    }
    public class SchduleTaskGetByWebRSList
    {
        public string TaskID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PlanDateStart { get; set; }
        public string PlanDateEnd { get; set; }
        public string ActualDateStart { get; set; }
        public string ActualDateEnd { get; set; }
        public string OwnerUserID { get; set; }
        public string OwnerUserText { get; set; }
        public string TaskTypeID { get; set; }
        public string TaskStatusID { get; set; }
        public string Remark { get; set; }
        public string CreateDate { get; set; }


    }
}