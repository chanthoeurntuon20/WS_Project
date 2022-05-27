using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.Models.Req.Geolocations
{
    public class GeolocationReq
    {
        public string Geolocation { get; set; }
        public string EndDate { get; set; }
        public string StartDate { get; set; }
    }
}