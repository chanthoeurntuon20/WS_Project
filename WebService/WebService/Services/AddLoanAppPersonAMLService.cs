using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebService.Models.Req.CustomerAmls;

namespace WebService.Services
{
    public class AddLoanAppPersonAMLService
    {
        public static string ConStr()
        {
            return ConfigurationManager.AppSettings["ConStr"];
        }

        public static void AddLoanAMLCreation(CustomerAmlReq customerAml)
        {

            try
            {
                SqlConnection Con1 = new SqlConnection(ConStr());
                Con1.Open();
                SqlCommand Com1 = new SqlCommand();
                Com1.Connection = Con1;
                Com1.Parameters.Clear();
                string sql = "AddLoanAppPersonAML";
                Com1.CommandText = sql;
                Com1.CommandType = CommandType.StoredProcedure;
                Com1.Parameters.AddWithValue("@LoanAppID", customerAml.LoanAppID.ToString());
                Com1.Parameters.AddWithValue("@LoanAppPersonID", customerAml.LoanAppPersonID.ToString());
                Com1.Parameters.AddWithValue("@BlockStatus", customerAml.BlockStatus.ToString());
                Com1.Parameters.AddWithValue("@WatchListScreeningStatus", customerAml.WatchListScreeningStatus.ToString());
                Com1.Parameters.AddWithValue("@WatchListCaseUrl", customerAml.WatchListCaseUrl.ToString());
                Com1.Parameters.AddWithValue("@RiskProfiling", customerAml.RiskProfiling.ToString());
                Com1.Parameters.AddWithValue("@AMLApprovalStatus", customerAml.AMLApprovalStatus.ToString());
                Com1.Parameters.AddWithValue("@WatchListExposition", customerAml.WatchListExposition.ToString());
                Com1.Parameters.AddWithValue("@ProductAndService", customerAml.ProductAndService.ToString());
                Com1.Parameters.AddWithValue("@CreateBy", customerAml.CreateBy.ToString());
                Com1.ExecuteReader();
                Con1.Close();
            }
            catch
            {
            }
        }
        public static void AddWatchlistAMLCreation(CustomerAmlReq customerAml)
        {

            try
            {
                SqlConnection Con1 = new SqlConnection(ConStr());
                Con1.Open();
                SqlCommand Com1 = new SqlCommand();
                Com1.Connection = Con1;
                Com1.Parameters.Clear();
                string sql = "LoanAppPersonAML_in";
                Com1.CommandText = sql;
                Com1.CommandType = CommandType.StoredProcedure;
                Com1.Parameters.AddWithValue("@LoanAppID", customerAml.LoanAppID.ToString());
                Com1.Parameters.AddWithValue("@LoanAppPersonID", customerAml.LoanAppPersonID.ToString());
                Com1.Parameters.AddWithValue("@BlockStatus", customerAml.BlockStatus.ToString());
                Com1.Parameters.AddWithValue("@WatchListScreeningStatus", customerAml.WatchListScreeningStatus.ToString());
                Com1.Parameters.AddWithValue("@WatchListCaseUrl", customerAml.WatchListCaseUrl.ToString());
                Com1.Parameters.AddWithValue("@RiskProfiling", customerAml.RiskProfiling.ToString());
                Com1.Parameters.AddWithValue("@AMLApprovalStatus", customerAml.AMLApprovalStatus.ToString());
                Com1.Parameters.AddWithValue("@WatchListExposition", customerAml.WatchListExposition.ToString());
                Com1.Parameters.AddWithValue("@ProductAndService", customerAml.ProductAndService.ToString());
                Com1.Parameters.AddWithValue("@CreateBy", customerAml.CreateBy.ToString());
                Com1.ExecuteReader();
                Con1.Close();
            }
            catch
            {
            }
        }
    }
}