using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.Models.Res.LoanApp
{
    public class LoanRestudyRes
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
        public List<CheckStatusLoanRestudyRes> DataList { get; set; }
    }
}