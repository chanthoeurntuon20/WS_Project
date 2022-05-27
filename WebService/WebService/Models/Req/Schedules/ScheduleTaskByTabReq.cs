using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.Models.Req.Schedules
{
    public class ScheduleTaskByTabReq
    {
        public string Token { get; set; }
        public string MsgId { get; set; }
        public string JsonFormat { get; set; }
    }
}