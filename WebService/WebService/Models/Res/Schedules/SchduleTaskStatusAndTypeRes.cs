using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.Models.Res.Schedules
{
	public class SchduleTaskStatusAndTypeRes
    { 
        public string ERR { get; set; }
        public string SMS { get; set; }
        public string ERRCode { get; set; }
        public List<SchduleTaskStatusAndTypeDetail> DataDetail { get; set; }
    }
}