using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebService.Models.Req.LoanApp
{
    public class CheckLoanRestudyReq
    {
        [Required]
        public string User { get; set; }
        [Required]
        public string Pwd { get; set; }
        [Required]
        public string Device_id { get; set; }
        [Required]
        public string App_vName { get; set; }

        public List<CheckStatusLoanRestudy> LoanUploadList { get; set; }
    }
}