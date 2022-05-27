using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.Models.Req.Schedules
{
    public class SchduleTaskGetByWebRQ
    {
        public string UserOwnerID { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}