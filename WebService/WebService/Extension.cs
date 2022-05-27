using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService
{
    public static class Extension
    {
        public static long ToUnixTimestamp(this DateTime target)
        {
            var date = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var unixTimestamp = System.Convert.ToInt64((target - date).TotalSeconds);
            return unixTimestamp;
        }

        public static DateTime ConvertDate(string date)
        {
            try
            {
                var dt = DateTime.ParseExact(date, "hh:mm tt dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                return dt;
            }
            catch (Exception ex)
            {
                return DateTime.Now;
            }
        }
    }
}