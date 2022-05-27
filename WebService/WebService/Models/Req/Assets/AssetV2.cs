using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.Models.Req.Assets
{
    public class AssetV2
    {
        public string AssetClientID { get; set; }//old - LoanAppClientAssetID
        public string AssetServerID { get; set; }//old
        public string LoanPurposeClientID { get; set; }//new
        public string LoanClientID { get; set; }//old
        public string LoanAppID { get; set; }//old
        public string Description { get; set; }//old
        public string Quantity { get; set; }//old
        public string UnitPrice { get; set; }//old
        public string CustClientID { get; set; }//new
        public string CustServerID { get; set; }//new
        public string AssetLookUpID { get; set; }//new
        public string AssetOtherDescription { get; set; }//new
        public string Unit { get; set; }//new

        public List<AssetImgV2> AssetImg;
    }
}