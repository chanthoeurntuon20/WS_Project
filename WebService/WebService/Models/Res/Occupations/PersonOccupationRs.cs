using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.Models.Res.Occupations
{
    public class PersonOccupationRs
    {
        public string Id { get; set; }
        public string PersonOccupationName { get; set; }
        public List<PersonOccupationDetailRs> personOccupationDetialRs { get; set; }
    }
}