using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebService.Helpers;
using WebService.Models.LoanApp;

namespace WebService.Repositories
{
    public class LoanAppRepository
    {
        public void AddLoanAppPurchaseProperty(LoanAppPurchaseProperty req)
        {
            try
            {
                SqlConnection Con1 = new SqlConnection(AppConfig.ConStr());
                Con1.Open();
                SqlCommand Com1 = new SqlCommand();
                Com1.Connection = Con1;
                Com1.Parameters.Clear();
                string sql = "sp_LoanAppPurchaseProperty";
                Com1.CommandText = sql;
                Com1.CommandType = CommandType.StoredProcedure;
                Com1.Parameters.AddWithValue("@LoanAppID", req.LoanAppID);
                Com1.Parameters.AddWithValue("@MainBorrowerTypeID", req.MainBorrowerTypeID);
                Com1.Parameters.AddWithValue("@PchPropertyTypeID", req.PchPropertyTypeID);
                Com1.Parameters.AddWithValue("@TitlePchTypeID", req.TitlePchTypeID);
                Com1.Parameters.AddWithValue("@DateConstruction ", req.DateConstruction);
                Com1.Parameters.AddWithValue("@BuiltArea", req.BuiltArea);
                Com1.Parameters.AddWithValue("@BuildingWith", req.BuildingWith);
                Com1.Parameters.AddWithValue("@LandArea", req.LandArea);
                Com1.Parameters.AddWithValue("@LandWith", req.LandWith);
                Com1.Parameters.AddWithValue("@PricePchProperty", req.PricePchProperty);
                Com1.Parameters.AddWithValue("@PricePchLand", req.PricePchLand);
                Com1.Parameters.AddWithValue("@FloorNumber", req.FloorNumber);
                Com1.Parameters.AddWithValue("@NumberBedRoom", req.NumberBedRoom);
                Com1.Parameters.AddWithValue("@ProvinceID", req.ProvinceID);
                Com1.Parameters.AddWithValue("@DistrictID", req.DistrictID);
                Com1.Parameters.AddWithValue("@CommuneID", req.CommuneID);
                Com1.Parameters.AddWithValue("@VillageID", req.VillageID);
                Com1.Parameters.AddWithValue("@StreetNo", req.StreetNo);
                DataTable dt = new DataTable();
                dt.Load(Com1.ExecuteReader());

                Con1.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Internal Server Error: @{ex.Message}", ex.Message);
            }
        }
    }
}