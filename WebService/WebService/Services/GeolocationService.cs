using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebService.Models.Req.Geolocations;
using WebService.Models.Req.Persons;

namespace WebService.Services
{
    public class GeolocationService
    {
        public static string ConStr()
        {
            return ConfigurationManager.AppSettings["ConStr"];
        }
        public static void AddGeolocationLoanCreation(LoanAppV2 geolocationDT)
        {
           
            try
            {
                SqlConnection Con1 = new SqlConnection(ConStr());
                Con1.Open();
                SqlCommand Com1 = new SqlCommand();
                Com1.Connection = Con1;
                Com1.Parameters.Clear();
                string sql = "AddGeolocationLoanAppCreation";
                Com1.CommandText = sql;
                Com1.CommandType = CommandType.StoredProcedure;
                Com1.Parameters.AddWithValue("@LoanAppID", geolocationDT.LoanAppID.ToString());
                Com1.Parameters.AddWithValue("@LoanAmount", geolocationDT.LoanRequestAmount.ToString());
                Com1.Parameters.AddWithValue("@LoanCurrency", geolocationDT.Currency.ToString());
                Com1.Parameters.AddWithValue("@LoanProduct", geolocationDT.ProductID.ToString());
                Com1.Parameters.AddWithValue("@LoanCreateGeoLocation", geolocationDT.Geolocation.LoanCreateGeoLocation.ToString());
                Com1.Parameters.AddWithValue("@LoanSubmitGeoLocation", geolocationDT.Geolocation.LoanSubmitGeoLocation.ToString());
                Com1.Parameters.AddWithValue("@StartDate", geolocationDT.Geolocation.LoanStartDate.ToString());
                Com1.Parameters.AddWithValue("@EndDate", geolocationDT.Geolocation.LoanEndDate.ToString());
                Com1.Parameters.AddWithValue("@CashFlowStartDate", geolocationDT.Geolocation.CashFlowStartDate.ToString());
                Com1.Parameters.AddWithValue("@CashFlowEndDate", geolocationDT.Geolocation.CashFlowEndDate.ToString());
                Com1.Parameters.AddWithValue("@CashFlowStartGeoLocation", geolocationDT.Geolocation.CashFlowStartGeoLocation.ToString());
                Com1.Parameters.AddWithValue("@CashFLowEndGeoLocation", geolocationDT.Geolocation.CashFLowEndGeoLocation.ToString());
                Com1.Parameters.AddWithValue("@LoanStatus", geolocationDT.LoanAppStatusID.ToString());
                Com1.ExecuteReader();
                Con1.Close();
            }
            catch
            {
            }
        }
    }
}