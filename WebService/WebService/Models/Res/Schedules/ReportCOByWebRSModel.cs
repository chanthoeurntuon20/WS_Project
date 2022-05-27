using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.Models.Res.Schedules
{
    public class ReportCOByWebRSModel
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
        public string ERRCode { get; set; }
        public List<ReportCOByWebRSList> DataList { get; set; }
    }

    public class ReportCOByWebRSList
    {
       
        public string TaskID { get; set; }
        public string COID { get; set; }
        public string COName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string TaskTypeID { get; set; }
        public string TaskType { get; set; }
        public string PlanDateStart { get; set; }
        public string PlanDateEnd { get; set; }
        public string ActualDateStart { get; set; }
        public string ActualDateEnd { get; set; }
        public string TaskStatusID { get; set; }
        public string Remark { get; set; }
        public string TaskStatus { get; set; }
        public string AssignedBy { get; set; }
        public string AssignedByName { get; set; }
    }
}