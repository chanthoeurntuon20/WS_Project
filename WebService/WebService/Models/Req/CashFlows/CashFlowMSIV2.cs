using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.Models.Req.CashFlows
{
    public class CashFlowMSIV2
    {
        public string CashFlowMSIClientID { get; set; }
        public string CashFlowMSIServerID { get; set; }
        public string CashFlowClientID { get; set; }
        public string CashFlowServerID { get; set; }
        public string IncomeTypeID { get; set; }
        public string OccupationTypeID { get; set; }
        public string MainSourceIncomeID { get; set; }
        public string ExAge { get; set; }
        public string BusAge { get; set; }
        public string isMSI { get; set; }
        public string IncomeOwnerID { get; set; }
        public string WorkingPlaceID { get; set; }
        public string PhoneNumber { get; set; }
        public string Name { get; set; }
        public List<CashFlowMSIInExV2> CashFlowMSIInEx;
    }
}