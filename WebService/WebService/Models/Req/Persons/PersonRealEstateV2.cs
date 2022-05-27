using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.Models.Req.Persons
{
    public class PersonRealEstateV2
    {
        #region RealEstate
        public string CollateralClientID { get; set; }
        public string CollateralServerID { get; set; }
        public string LoanClientID { get; set; }
        public string LoanAppID { get; set; }
        public string CustClientID { get; set; }
        public string CustServerID { get; set; }
        public string CollateralDocGroupTypeID { get; set; }
        public string CollateralDocHardTypeID { get; set; }
        public string CollateralDocSoftTypeID { get; set; }
        public string CollateralOwnerTypeID { get; set; }
        public string CollateralLocationVillageID { get; set; }
        public string CollateralRoadAccessibilityID { get; set; }
        public string PropertyTypeID { get; set; }
        public string LandTypeID { get; set; }
        public string LandSize { get; set; }
        public string LandMarketPrice { get; set; }
        public string LandForcedSalePrice { get; set; }
        public string BuildingTypeID { get; set; }
        public string BuildingSize { get; set; }
        public string BuildingMarketPrice { get; set; }
        public string BuildingForcedSalesPrice { get; set; }
        #endregion RealEstate
        public List<PersonRealEstateImgV2> RealEstateImg;
    }
}