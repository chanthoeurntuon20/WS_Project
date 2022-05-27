using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.Models.Req.Occupations
{
    public class PersonOccupationDetail
    {
        [JsonIgnore]
        public int Id { get; set; }
        [JsonIgnore]
        public int PersonOccupationId { get; set; }
        [JsonIgnore]
        public string PersonOccDetailName{ get; set; }
        public int OccupationDetailId { get; set; }
    }
}