using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebService.Helpers;
using WebService.Models.Req.Users;
using WebService.Models.Res.Users;

namespace WebService.Repositories
{
    public class UserLoginRepo
    {
        private readonly AppDbContext c = new AppDbContext();

        public User Login(UserLogin login)
        {
            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string ControllerName = "User";
            string FileNameForLog = "User_" + ServerDate.Replace("-", "_").Replace(" ", "_").Replace(":", "_").Replace(".", "_");
            try
            {
                c.T24_AddLog(FileNameForLog, "1.RQ: Username:", login.Username +"- Pwd:"+ login.Password, ControllerName);
                var user = new User();
                SqlConnection Con1 = new SqlConnection(AppConfig.ConStr());
                Con1.Open();
                SqlCommand Com1 = new SqlCommand();
                Com1.Connection = Con1;
                Com1.Parameters.Clear();
                string sql = "User_Login";
                Com1.CommandText = sql;
                Com1.CommandType = CommandType.StoredProcedure;
                Com1.Parameters.AddWithValue("@username", login.Username);
                Com1.Parameters.AddWithValue("@password",login.Password);
                DataTable dt = new DataTable();
                dt.Load(Com1.ExecuteReader());

                user.Status = dt.Rows[0]["Status"].ToString();
                user.Message = dt.Rows[0]["SMS"].ToString();
                user.UserID = dt.Rows[0]["UserID"].ToString();
                user.GroupID = dt.Rows[0]["GroupID"].ToString();
                user.OfficeHierachyID = dt.Rows[0]["OfficeHierachyID"].ToString();
                user.OfficeID = dt.Rows[0]["OfficeID"].ToString();

                Con1.Close();
                c.T24_AddLog(FileNameForLog, "2.RS","User login is successfully.", ControllerName);
                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login Username and Password Invalid {ex.Message}");
                c.T24_AddLog(FileNameForLog, "3.ERR: Login Username and Password Invalid", login.Username + "-" + login.Password, ControllerName);
                return new User();
            }
        }
    }
}