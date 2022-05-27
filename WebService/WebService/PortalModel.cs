using System.Collections.Generic;

namespace WebService
{
    public class PortalAddV2RQ
    {
        public string user { get; set; }
        public string pwd { get; set; }
        public string device_id { get; set; }
        public string app_vName { get; set; }
        public string mac_address { get; set; }

        public List<PortalDataList> PortalDataList;
    }
    public class PortalDataList
    {
        public string PreAppID { get; set; }
        public string CreateDateClient { get; set; }
        public string Code { get; set; }
        public string RegisterDate { get; set; }
        public string ReferByID { get; set; }
        public string ReferName { get; set; }
        public string NameKh { get; set; }
        public string NameEn { get; set; }
        public string GenderID { get; set; }
        public string Age { get; set; }
        public string Phone { get; set; }
        public string VillageID { get; set; }
        public string BizStatusID { get; set; }
        public string CollateralStatusID { get; set; }
        public string LoanEligibleID { get; set; }
        public string PromotionDate1 { get; set; }
        public string PromotionDate2 { get; set; }
        public string PromotionDate3 { get; set; }
        public string CustComment { get; set; }
        public string ExpectOnBoardDate { get; set; }
        public string ProspectStatusID { get; set; }
        public string IsOldCust { get; set; }

        public string partner_id { get; set; }
        public string Category { get; set; }
        public string Currency { get; set; }
        public string LoanAmount { get; set; }
        public string CBCRef { get; set; }

    }

    public class PortalAddRS
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
        public string PreAppID { get; set; }
        public string ProspectServerID { get; set; }
    }
}