using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.Models.Req.Schedules
{
    public class ScheduleTaskByWebReq
    {
        public string ApiKey { get; set; }
        public string ApiName { get; set; }
        public string Username { get; set; }
        public string JsonFormat { get; set; }
    }
}