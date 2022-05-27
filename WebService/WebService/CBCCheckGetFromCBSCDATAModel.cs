using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace WebService
{
    [XmlRoot(ElementName = "RESPONSE")]
    public class CBCCheckGetFromCBSCDATAModel
    {
        [XmlElement("SERVICE")]
        public string SERVICE { get; set; }
        [XmlElement("ACTION")]
        public string ACTION { get; set; }
        [XmlElement("STATUS")]
        public string STATUS { get; set; }
        [XmlElement("HEADER")]
        public List<HEADER> HEADER { get; set; }
        [XmlElement("MESSAGE")]
        public List<MESSAGE> MESSAGE { get; set; }
    }
    public class HEADER
    {
        [XmlElement("MEMBER_ID")]
        public string MEMBER_ID { get; set; }
        [XmlElement("USER_ID")]
        public string USER_ID { get; set; }
        [XmlElement("RUN_NO")]
        public string RUN_NO { get; set; }
        [XmlElement("TOT_ITEMS")]
        public string TOT_ITEMS { get; set; }
        [XmlElement("ERR_ITEMS")]
        public string ERR_ITEMS { get; set; }
    }
    public class MESSAGE
    {
        [XmlElement("ITEM")]
        public List<ITEM> ITEM { get; set; }
    }


    //////////////////////////////////////////////////////////


    public class ITEM
    {
        [XmlElement("ENQUIRY_REFERENCE")]
        public string ENQUIRY_REFERENCE { get; set; }
        [XmlElement("RSP_REPORT")]
        public List<RSP_REPORT> RSP_REPORT { get; set; }
    }
    public class RSP_REPORT
    {
        #region 1
        [XmlElement("ENQUIRY_TYPE")]
        public string ENQUIRY_TYPE { get; set; }
        [XmlElement("REPORT_DATE")]
        public string REPORT_DATE { get; set; }
        [XmlElement("ENQUIRY_NO")]
        public string ENQUIRY_NO { get; set; }
        [XmlElement("PRODUCT_TYPE")]
        public string PRODUCT_TYPE { get; set; }
        [XmlElement("NO_OF_APPLICANTS")]
        public string NO_OF_APPLICANTS { get; set; }
        [XmlElement("ACCOUNT_TYPE")]
        public string ACCOUNT_TYPE { get; set; }
        [XmlElement("ENQUIRY_REFERENCE")]
        public string ENQUIRY_REFERENCE { get; set; }
        [XmlElement("AMOUNT")]
        public string AMOUNT { get; set; }
        [XmlElement("CURRENCY")]
        public string CURRENCY { get; set; }
        #endregion 1
        [XmlElement("CONSUMER")]
        public List<CONSUMER> CONSUMER { get; set; }
        [XmlElement("DISCLAIMER")]
        public List<DISCLAIMER> DISCLAIMER { get; set; }
    }

    //////////////////////////////////////////////////////////
    public class CID
    {
        [XmlElement("CID1")]
        public string CID1 { get; set; }
        [XmlElement("CID2")]
        public string CID2 { get; set; }
        [XmlElement("CID3")]
        public string CID3 { get; set; }
    }
    public class PROVIDED
    {
        [XmlElement("PCNAM")]
        public List<PCNAM> PCNAM { get; set; }
        [XmlElement("PCDOB")]
        public string PCDOB { get; set; }
        [XmlElement("PCPLB")]
        public List<PCPLB> PCPLB { get; set; }
        [XmlElement("PCGND")]
        public string PCGND { get; set; }
        [XmlElement("PCMAR")]
        public string PCMAR { get; set; }
        [XmlElement("PCNAT")]
        public string PCNAT { get; set; }
        [XmlElement("PCEML")]
        public string PCEML { get; set; }
    }
    public class PCNAM
    {
        [XmlElement("PCNMFE")]
        public string PCNMFE { get; set; }
        [XmlElement("PCNM1E")]
        public string PCNM1E { get; set; }
    }
    public class PCPLB
    {
        [XmlElement("PCPLBC")]
        public string PCPLBC { get; set; }
        [XmlElement("PCPLBP")]
        public string PCPLBP { get; set; }
    }
    public class ACID
    {
        [XmlElement("ACID1")]
        public string ACID1 { get; set; }
        [XmlElement("ACID2")]
        //[XmlElement(ElementName = "ACID2")]
        public string ACID2 { get; set; }
        [XmlElement("ACID3")]
        public string ACID3 { get; set; }
    }
    public class ACPLB
    {
        [XmlElement("ACPLBC")]
        public string ACPLBC { get; set; }
        [XmlElement("ACPLBP")]
        public string ACPLBP { get; set; }
    }
    public class AVAILABLE
    {
        [XmlElement("ACID")]
        public List<ACID> ACID { get; set; }
        [XmlElement("ACNAM")]
        public string ACNAM { get; set; }
        [XmlElement("ACDOB")]
        public string ACDOB { get; set; }
        [XmlElement("ACPLB")]
        public List<ACPLB> ACPLB { get; set; }
        [XmlElement("ACGND")]
        public string ACGND { get; set; }
        [XmlElement("ACMAR")]
        public string ACMAR { get; set; }
        [XmlElement("ACNAT")]
        public string ACNAT { get; set; }
        //[XmlElement("ACEML")]
        //public string ACEML { get; set; }
        ////<ACEML>&LT;SCRIPT&GT;ALERT(1);&LT;/SCRIPT&GT;</ACEML>
    }
    public class ADDITIONAL_NAMES
    {
        [XmlElement("ADDITIONAL_NAME")]
        public List<ADDITIONAL_NAME> ADDITIONAL_NAME { get; set; }
    }
    public class ADDITIONAL_NAME
    {
        [XmlElement("ANM_CNMFE")]
        public string ANM_CNMFE { get; set; }
        [XmlElement("ANM_CNM1E")]
        public string ANM_CNM1E { get; set; }
        [XmlElement("ANM_CNM2E")]
        public string ANM_CNM2E { get; set; }
        [XmlElement("ANM_CNM3E")]
        public string ANM_CNM3E { get; set; }
        [XmlElement("ANM_CNM7E")]
        public string ANM_CNM7E { get; set; }
        [XmlElement("ANM_LOAD_DT")]
        public string ANM_LOAD_DT { get; set; }
    }

    public class PREV_ENQUIRY
    {
        [XmlElement("PE_DATE")]
        public string PE_DATE { get; set; }
        [XmlElement("PE_ENQR")]
        public string PE_ENQR { get; set; }
        [XmlElement("PE_TYPE")]
        public string PE_TYPE { get; set; }
        [XmlElement("PE_ACCT")]
        public string PE_ACCT { get; set; }
        [XmlElement("PE_MEMB_REF")]
        public string PE_MEMB_REF { get; set; }
        [XmlElement("PE_PRD")]
        public string PE_PRD { get; set; }
        [XmlElement("PE_AMOUNT")]
        public string PE_AMOUNT { get; set; }
        [XmlElement("PE_CURR")]
        public string PE_CURR { get; set; }
        [XmlElement("PE_APPL")]
        public string PE_APPL { get; set; }
        [XmlElement("PE_NAME")]
        public List<PE_NAME> PE_NAME { get; set; }
    }
    public class PE_NAME
    {
        [XmlElement("PE_NMFE")]
        public string PE_NMFE { get; set; }
        [XmlElement("PE_NM1E")]
        public string PE_NM1E { get; set; }
    }
    public class PREV_ENQUIRIES
    {
        [XmlElement("PREV_ENQUIRY")]
        public List<PREV_ENQUIRY> PREV_ENQUIRY { get; set; }

    }
    public class ADDRESSES
    {
        [XmlElement("ADDRESS")]
        public List<ADDRESS> ADDRESS { get; set; }
    }
    public class ADDRESS
    {
        [XmlElement("CA_LOAD_DT")]
        public string CA_LOAD_DT { get; set; }
        [XmlElement("CA_CADT")]
        public string CA_CADT { get; set; }
        [XmlElement("CA_PROV")]
        public string CA_PROV { get; set; }
        [XmlElement("CA_DIST")]
        public string CA_DIST { get; set; }
        [XmlElement("CA_COMM")]
        public string CA_COMM { get; set; }
        [XmlElement("CA_VILL")]
        public string CA_VILL { get; set; }
        [XmlElement("CA_HOUSE")]
        public string CA_HOUSE { get; set; }
        [XmlElement("CA_STREET")]
        public string CA_STREET { get; set; }
        [XmlElement("CA_CAD9")]
        public string CA_CAD9 { get; set; }
    }
    public class EMPLOYERS
    {
        [XmlElement("EMPLOYER")]
        public List<EMPLOYER> EMPLOYER { get; set; }
    }
    public class EMPLOYER
    {
        [XmlElement("EDLD")]
        public string EDLD { get; set; }
        [XmlElement("ETYP")]
        public string ETYP { get; set; }
        [XmlElement("ESLF")]
        public string ESLF { get; set; }
        [XmlElement("EOCE")]
        public string EOCE { get; set; }
        [XmlElement("ELEN")]
        public string ELEN { get; set; }
        [XmlElement("ETMS")]
        public string ETMS { get; set; }
        [XmlElement("ECURR")]
        public string ECURR { get; set; }
        [XmlElement("ENME")]
        public string ENME { get; set; }
        [XmlElement("EADR")]
        public List<EADR> EADR { get; set; }
    }
    public class EADR
    {
        [XmlElement("EA_CADT")]
        public string EA_CADT { get; set; }
        [XmlElement("EA_PROV")]
        public string EA_PROV { get; set; }
        [XmlElement("EA_DIST")]
        public string EA_DIST { get; set; }
        [XmlElement("EA_COMM")]
        public string EA_COMM { get; set; }
        [XmlElement("EA_VILL")]
        public string EA_VILL { get; set; }
        [XmlElement("EA_HOUSE")]
        public string EA_HOUSE { get; set; }
        [XmlElement("EA_STREET")]
        public string EA_STREET { get; set; }
        [XmlElement("EA_AD9")]
        public string EA_AD9 { get; set; }
    }
    public class ADVISORY_SUMMARY_TEXT
    {
        [XmlElement("ADVISORY_TEXT")]
        public string ADVISORY_TEXT { get; set; }
    }
    public class SUMMARY
    {
        #region 1
        [XmlElement("CNT_PE")]
        public string CNT_PE { get; set; }
        [XmlElement("CNT_MTDE")]
        public string CNT_MTDE { get; set; }
        [XmlElement("CNT_ACC")]
        public string CNT_ACC { get; set; }
        [XmlElement("CNT_ACC_NORM")]
        public string CNT_ACC_NORM { get; set; }
        [XmlElement("CNT_ACC_DELQ")]
        public string CNT_ACC_DELQ { get; set; }
        [XmlElement("CNT_ACC_CLO")]
        public string CNT_ACC_CLO { get; set; }
        [XmlElement("CNT_ACC_REJ")]
        public string CNT_ACC_REJ { get; set; }
        [XmlElement("CNT_ACC_WRO")]
        public string CNT_ACC_WRO { get; set; }
        [XmlElement("CNT_GACC")]
        public string CNT_GACC { get; set; }
        [XmlElement("CNT_GACC_NORM")]
        public string CNT_GACC_NORM { get; set; }
        [XmlElement("CNT_GACC_DELQ")]
        public string CNT_GACC_DELQ { get; set; }
        [XmlElement("CNT_GACC_ACC_CLO")]
        public string CNT_GACC_ACC_CLO { get; set; }
        [XmlElement("CNT_GACC_REJ")]
        public string CNT_GACC_REJ { get; set; }
        [XmlElement("CNT_GACC_WRO")]
        public string CNT_GACC_WRO { get; set; }
        [XmlElement("EISDT")]
        public string EISDT { get; set; }
        #endregion 1
        [XmlElement("TOT_LIMITS")]
        public List<TOT_LIMITS> TOT_LIMITS { get; set; }
        [XmlElement("TOT_GLIMITS")]
        public List<TOT_GLIMITS> TOT_GLIMITS { get; set; }
        [XmlElement("TOT_LIABILITIES")]
        public List<TOT_LIABILITIES> TOT_LIABILITIES { get; set; }
        [XmlElement("TOT_GLIABILITIES")]
        public List<TOT_GLIABILITIES> TOT_GLIABILITIES { get; set; }
        [XmlElement("TOT_WO_AMT")]
        public List<TOT_WO_AMT> TOT_WO_AMT { get; set; }
        [XmlElement("TOT_GWO_AMT")]
        public List<TOT_GWO_AMT> TOT_GWO_AMT { get; set; }
        [XmlElement("TOT_WO_OS_BALANCE")]
        public List<TOT_WO_OS_BALANCE> TOT_WO_OS_BALANCE { get; set; }
        [XmlElement("TOT_GWO_OS_BALANCE")]
        public List<TOT_GWO_OS_BALANCE> TOT_GWO_OS_BALANCE { get; set; }
    }
    public class TOT_LIMITS
    {
        [XmlElement("TOT_LIM_CURR")]
        public string TOT_LIM_CURR { get; set; }
        [XmlElement("TOT_LIM")]
        public string TOT_LIM { get; set; }
    }
    public class TOT_GLIMITS
    {
        [XmlElement("TOT_GLIM_CURR")]
        public string TOT_GLIM_CURR { get; set; }
        [XmlElement("TOT_GLIM")]
        public string TOT_GLIM { get; set; }
    }
    public class TOT_LIABILITIES
    {
        [XmlElement("TOT_LIAB_CURR")]
        public string TOT_LIAB_CURR { get; set; }
        [XmlElement("TOT_LIAB")]
        public string TOT_LIAB { get; set; }
    }
    public class TOT_GLIABILITIES
    {
        [XmlElement("TOT_GLIAB_CURR")]
        public string TOT_GLIAB_CURR { get; set; }
        [XmlElement("TOT_GLIAB")]
        public string TOT_GLIAB { get; set; }
    }
    public class TOT_WO_AMT
    {
        [XmlElement("TOT_WO_CURR")]
        public string TOT_WO_CURR { get; set; }
        [XmlElement("TOT_WO")]
        public string TOT_WO { get; set; }
    }
    public class TOT_GWO_AMT
    {
        [XmlElement("TOT_GWO_CURR")]
        public string TOT_GWO_CURR { get; set; }
        [XmlElement("TOT_GWO")]
        public string TOT_GWO { get; set; }
    }
    public class TOT_WO_OS_BALANCE
    {
        [XmlElement("TOT_WO_OS_CURR")]
        public string TOT_WO_OS_CURR { get; set; }
        [XmlElement("TOT_WO_OS_BAL")]
        public string TOT_WO_OS_BAL { get; set; }
    }
    public class TOT_GWO_OS_BALANCE
    {
        [XmlElement("TOT_GWO_OS_CURR")]
        public string TOT_GWO_OS_CURR { get; set; }
        [XmlElement("TOT_GWO_OS_BAL")]
        public string TOT_GWO_OS_BAL { get; set; }
    }
    public class SCORE
    {
        [XmlElement("SC_ERROR")]
        public string SC_ERROR { get; set; }
    }
    public class CONSUMER
    {
        [XmlElement("CAPL")]
        public string CAPL { get; set; }
        [XmlElement("CID")]
        public List<CID> CID { get; set; }
        [XmlElement("PROVIDED")]
        public List<PROVIDED> PROVIDED { get; set; }
        [XmlElement("AVAILABLE")]
        public List<AVAILABLE> AVAILABLE { get; set; }
        [XmlElement("ADDITIONAL_NAMES")]
        public List<ADDITIONAL_NAMES> ADDITIONAL_NAMES { get; set; }

        ////[XmlElement("PREV_ENQUIRIES")]
        ////public List<PREV_ENQUIRIES> PREV_ENQUIRIES { get; set; }

        [XmlElement("ADDRESSES")]
        public List<ADDRESSES> ADDRESSES { get; set; }
        [XmlElement("EMPLOYERS")]
        public List<EMPLOYERS> EMPLOYERS { get; set; }
        [XmlElement("ADVISORY_SUMMARY_TEXT")]
        public List<ADVISORY_SUMMARY_TEXT> ADVISORY_SUMMARY_TEXT { get; set; }
        [XmlElement("SUMMARY")]
        public List<SUMMARY> SUMMARY { get; set; }
        [XmlElement("SCORE")]
        public List<SCORE> SCORE { get; set; }

    }
    public class DISCLAIMER
    {
        [XmlElement("DI_TEXT")]
        public string DI_TEXT { get; set; }
    }

}