using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.Models.Res.Schedules
{
    public class SchduleTaskStatusAndTypeDetail
    {
        public string ID { get; set; }
        public string DESCRIPTION { get; set; }
        public string TYPE { get; set; }// Type = Status(Task status) ,Type = Type(Task type) 
    }
}