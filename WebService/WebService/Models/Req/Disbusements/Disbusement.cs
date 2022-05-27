using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebService.Models.Req.Geolocations;

namespace WebService.Models.Req.Disbusements
{
    public class Disbusement
    {
        public string AccountNumber { get; set; }
        public string ApproveAmount { get; set; }
        public string ApproveDate { get; set; }
        public string CUAccountID { get; set; }
        public string ClientID { get; set; }
        public string ClientName { get; set; }
        public string ClientNumber { get; set; }
        public string CompulsorySaving { get; set; }
        public string CurrencyID { get; set; }
        public string DGroup { get; set; }
        public string DeviceDate { get; set; }
        public string DisStatus { get; set; }
        public string DisbCCY { get; set; }
        public string DisburseID { get; set; }
        public string EditStatus { get; set; }
        public string FeeAmount { get; set; }
        public string MITypeID { get; set; }
        public string ProdCode { get; set; }
        public string ProdName { get; set; }
        public string Ref { get; set; }
        public string RefFee { get; set; }
        public string RejectStatus { get; set; }
        public string VBName { get; set; }
        public string ValueDat { get; set; }
        public string VillageBankID { get; set; }
        public string isPost { get; set; }
        public string isSelect { get; set; }
        public string loanAppPersonType { get; set; }
        public GeolocationReq GeoLocation { get; set; }
    }
}