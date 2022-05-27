using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.Models.Req.Persons
{
    public class PersonNationality
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string OrderBy { get; set; }
    }
}