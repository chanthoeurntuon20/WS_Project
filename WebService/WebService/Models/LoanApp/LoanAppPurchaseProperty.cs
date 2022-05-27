using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebService.Models.LoanApp
{
    public class LoanAppPurchaseProperty
    {
        public int Id { get; set; }
        [Required]
        public int LoanAppID { get; set; }
        [Required]
        public int MainBorrowerTypeID { get; set; }
        [Required]
        public int PchPropertyTypeID { get; set; }
        [Required]
        public int TitlePchTypeID { get; set; }
        [Required]
        public DateTime DateConstruction { get; set; }
        [Required]
        public decimal BuiltArea { get; set; }
        [Required]
        public decimal BuildingWith { get; set; }
        [Required]
        public decimal LandArea { get; set; }
        [Required]
        public decimal LandWith { get; set; }
        [Required]
        public string PricePchProperty { get; set; }
        [Required]
        public string PricePchLand { get; set; }
        [Required]
        public int FloorNumber { get; set; }
        [Required]
        public int NumberBedRoom { get; set; }
        [Required]
        public string ProvinceID { get; set; }
        [Required]
        public string DistrictID { get; set; }
        [Required]
        public string CommuneID { get; set; }
        [Required]
        public string VillageID { get; set; }
        [Required]
        public string StreetNo { get; set; }
    }
}