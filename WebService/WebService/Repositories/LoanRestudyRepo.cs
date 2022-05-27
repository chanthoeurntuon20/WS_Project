using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebService.Helpers;
using WebService.Models.Req.LoanApp;

namespace WebService.Repositories
{
    public class LoanRestudyRepo
    {
        private readonly AppDbContext c = new AppDbContext();
        public string UpdateSyncStatusRestudyByLoanAppId(CheckStatusLoanRestudy req)
        {
            try
            {
                SqlConnection Con1 = new SqlConnection(AppConfig.ConStr());
                Con1.Open();
                SqlCommand Com1 = new SqlCommand();
                Com1.Connection = Con1;
                Com1.Parameters.Clear();
                string sql = "UpdateSyncStatusLoanRestudy";
                Com1.CommandText = sql;
                Com1.CommandType = CommandType.StoredProcedure;
                Com1.Parameters.AddWithValue("@LoanAppId", req.LoanAppId);
                Com1.Parameters.AddWithValue("@Status", req.Status);
                DataTable dt = new DataTable();
                dt.Load(Com1.ExecuteReader());

                var sms = dt.Rows[0]["SMS"].ToString();

                Con1.Close();
                return sms;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Update sync status is invalid {ex.Message}");
                return "0";
            }

        }
    }
}