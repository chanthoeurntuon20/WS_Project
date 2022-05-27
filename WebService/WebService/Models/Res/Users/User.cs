using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.Models.Res.Users
{
    public class User
    {
		public string Status { get; set; }
		public string Message { get; set; }
		public string UserID{ get; set; }
		public string GroupID{ get; set; }
		public string OfficeID { get; set; }
		public string OfficeHierachyID { get; set; }
		
	}
}