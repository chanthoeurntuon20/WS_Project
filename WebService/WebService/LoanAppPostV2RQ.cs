using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebService.Models.LoanApp;
using WebService.Models.Req.Persons;

namespace WebService
{
    public class LoanAppPostV2RQ
    {
        public string user { get; set; } = "";
        public string pwd { get; set; } = "";
        public string device_id { get; set; } = "";
        public string app_vName { get; set; } = "";
        public List<LoanAppV2> LoanApp { get; set; }
    }
   
}