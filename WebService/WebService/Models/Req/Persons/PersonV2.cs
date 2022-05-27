using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebService.Models.Req.Assets;
using WebService.Models.Req.Occupations;
using WebService.Models.Req.Persons;
namespace WebService.Models.Req.Persons
{
    public class PersonV2
    {
        #region person
        public string CustClientID { get; set; }//old - LoanAppPersonID
        public string T24CustID { get; set; }//old
        public string CustServerID { get; set; }//old
        public string ProspectCode { get; set; }//old
        public string CreateDateClient { get; set; }//old - DeviceDate
        public string LoanClientID { get; set; }//old
        public string LoanAppID { get; set; }//old
        public string LoanAppPersonTypeID { get; set; }//old
        public string VBID { get; set; }//old
        public string ReferByID { get; set; }//old
        public string ReferName { get; set; }//old
        public string NameKhLast { get; set; }//old - AltName
        public string NameKhFirst { get; set; }//old - AltName2
        public string TitleID { get; set; }//old
        public string NameEnLast { get; set; }//old
        public string NameEnFirst { get; set; }//old
        public string GenderID { get; set; }//old
        public string DateOfBirth { get; set; }//old
        public string IDCardTypeID { get; set; } = "";//old
        public string IDCardNumber { get; set; } = "";//old
        public string IDCardIssueDate { get; set; } = "";//old
        public string IDCardExpiryDate { get; set; } = "";//old
        public string MaritalStatusID { get; set; }//old
        public string EducationLevelID { get; set; }//old
        public string Phone { get; set; }//old
        public string PlaceOfBirth { get; set; }//old - CityOfBirthID
        public string VillageIDPer { get; set; }//old - VillageIDPermanent
        public string VillageIDCur { get; set; }//old - VillageIDCurrent
        public string ShortAddress { get; set; }//old
        public string FamilyMember { get; set; }//old
        public string FamilyMemberActive { get; set; }//old
        public string PoorLevelID { get; set; }//old - PoorID
        public string LatLon { get; set; }//new
        public string IsNeedSaving { get; set; } = "0";//KYC -> 0=No | 1=Need
        public string Nationality { get; set; }
        #endregion person
        public List<PersonImgV2> PersonImg { get; set; }//old
        public List<AssetV2> Asset { get; set; }//old
        public List<CreditorV2> Creditor { get; set; }//old
        public List<PersonRealEstateV2> RealEstate { get; set; }//new
        public List<PersonDepositV2> PersonDeposit { get; set; }//new
        public List<PersonKYC> PersonKYC  { get; set; }= null;//KYC - V3
        public List<PersonOccupation> LoanOccupation { get; set; }
     
    }
}