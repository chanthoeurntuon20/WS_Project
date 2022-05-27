using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebService.Models.Req.Schedules
{
    public class SchduleTaskStatusAndTypeReq
    { 
        [Required(ErrorMessage = "TYPE is required")]
        public string TYPE { get; set; }
    }
}