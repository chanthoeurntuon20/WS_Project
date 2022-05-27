using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebService.Models.Req.LoanApp
{
    public class CheckStatusLoanRestudy
    {  
        [Required]
        public string LoanAppId { get;set; }
        [Required]
        public string Status { get;set; }   
    }
}