using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebService.Models.Req.Geolocations;

namespace WebService.Models.Req.Repayments
{
    public class Repayment
    {
        public string AmountToClose { get; set; }
        public string ArrearsDay { get; set; }
        public string CUAccountID { get; set; }
        public string CUAccountNumber { get; set; }
        public string ClientName { get; set; }
        public string ClosureAmt { get; set; }
        public string CollectionDate { get; set; }
        public string CompulsorySaving { get; set; }
        public string CurrencyID { get; set; }
        public string CustomerID { get; set; }
        public string DeviceDate { get; set; }
        public string MeetingDate { get; set; }
        public string FeeDue { get; set; }
        public string GroupNumber { get; set; }
        public string IntDue { get; set; }
        public string MaturityDate { get; set; }
        public string MonthlyFee { get; set; }
        public string PaidOffPenaltyAmt { get; set; }
        public string PayOff { get; set; }
        public string PayOffReason { get; set; }
        public string Penalty { get; set; }
        public string PriDue { get; set; }
        public string ProdCode { get; set; }
        public string ProdDesc { get; set; }
        public string Ref { get; set; }
        public string RepayID { get; set; }
        public string Term { get; set; }
        public string TotDueAmt { get; set; }
        public string TotalDue { get; set; }
        public string VBName { get; set; }
        public string ValueDate { get; set; }
        public string VillageBankID { get; set; }
        public string isPost { get; set; }
        public string isSelect { get; set; }
        public GeolocationReq GeoLocation  {get; set;}
}
}