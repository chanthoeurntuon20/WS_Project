using Newtonsoft.Json;
using System.Collections.Generic;

namespace WebService.Models.Req.Occupations
{
    public class PersonOccupation
    {
        [JsonIgnore]
        public int Id { get; set; }
        [JsonIgnore]
        public string LoanAppPersonId { get; set; }
        public string OccupationId { get; set; }
        [JsonIgnore]
        public string OccupationName { get; set; }
        public List<PersonOccupationDetail> PersonOccupationDetials { get; set; }
    }
   
}