using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml;

namespace WebService
{
    [BasicAuthentication]
    public class LoanScheduleGetFromCBSController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<LoanScheduleGetFromCBSModel> Get(string api_name, string api_key, string json)////json=[{"user":"none","pwd":"none","device_id":"none","app_vName":"none","criteriaValue":"005515","criteriaValue2":"1"}]
        {
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "", criteriaValue = "", criteriaValue2="", INSURANCEFEE = "InsuranceFee"; 
            List<LoanScheduleGetFromCBSModel> RSData = new List<LoanScheduleGetFromCBSModel>();
            try
            {
                #region api
                string[] CheckApi = c.CheckApi(api_name, api_key);
                ERR = CheckApi[0];
                SMS = CheckApi[1];
                #endregion api

                #region json
                string UserID = "";
                if (ERR != "Error")
                {
                    string[] CheckJson = c.CheckJson(json);
                    ERR = CheckJson[0];
                    if (ERR == "Error")
                    {
                        SMS = CheckJson[1];
                    }
                    else
                    {
                        UserID = CheckJson[1];
                        criteriaValue = CheckJson[2];
                        criteriaValue2 = CheckJson[4];
                    }
                }
                #endregion json

                #region get loan by VBID and groupnumber
                if (ERR != "Error") {
                    try {
                        DataTable dt = c.ReturnDT("exec T24_GetLoanAppByVBIDGroupNumber @UserID='" + UserID + "',@VBID='" + criteriaValue + "',@GroupNumber='" + criteriaValue2 + "'");
                        
                        if (dt.Rows.Count == 0)
                        {
                            ERR = "Error";
                            SMS = "No Loan";
                        }
                        else
                        {
                            #region - get T24 url                
                            DataTable dtT24Url = new DataTable();
                            dtT24Url = c.ReturnDT("exec T24_GetT24_Url @UserID=0,@UrlID=22");
                            string CreUrl = dtT24Url.Rows[0]["CreUrl"].ToString();
                            string CreCompany = dtT24Url.Rows[0]["CreCompany"].ToString();
                            string CreUserName = dtT24Url.Rows[0]["CreUserName"].ToString();
                            string CrePassword = dtT24Url.Rows[0]["CrePassword"].ToString();
                            #endregion - T24 url
                            DataTable dtInsurance = c.ReturnDT("exec T24_GetInsuranceFeeFromLookup @INSURANCEFEE='" + INSURANCEFEE + "'");
                            LoanScheduleGetFromCBSModel LoanScheduleGetFromCBS = new LoanScheduleGetFromCBSModel();
                            List<LoanAppListForSchedule> LoanAppList = new List<LoanAppListForSchedule>();                            
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                List<LoanScheduleGetFromCBSList> LoanScheduleGetFromCBSList = new List<LoanScheduleGetFromCBSList>();
                                List<LoanScheduleCompulsoryList> LoanScheduleCompulsoryList = new List<LoanScheduleCompulsoryList>();
                                #region Params
                                string LoanAppID = dt.Rows[i]["LoanAppID"].ToString();
                                string CBSKey = dt.Rows[i]["CBSKey"].ToString();
                                string CompanyName = dt.Rows[i]["CompanyName"].ToString();
                                string CompanyNameKh = dt.Rows[i]["CompanyNameKh"].ToString();
                                string VBName = dt.Rows[i]["VBName"].ToString();
                                string Village = dt.Rows[i]["Village"].ToString();
                                string VillageKh = dt.Rows[i]["VillageKh"].ToString();
                                string Commune = dt.Rows[i]["Commune"].ToString();
                                string CommuneKh = dt.Rows[i]["CommuneKh"].ToString();
                                string District = dt.Rows[i]["District"].ToString();
                                string DistrictKh = dt.Rows[i]["DistrictKh"].ToString();
                                string Province = dt.Rows[i]["Province"].ToString();
                                string ProvinceKh = dt.Rows[i]["ProvinceKh"].ToString();
                                string CustomerNameKh = dt.Rows[i]["CustomerNameKh"].ToString();
                                string GenderID = dt.Rows[i]["GenderID"].ToString();
                                string ProductName = dt.Rows[i]["ProductName"].ToString();
                                string FirstRepaymentDate = dt.Rows[i]["FirstRepaymentDate"].ToString();
                                string LoanPurpose = dt.Rows[i]["LoanPurpose"].ToString();
                                string CompulsoryAccount = dt.Rows[i]["CompulsoryAccount"].ToString();
                                string SettlementAccount = dt.Rows[i]["SettlementAccount"].ToString();
                                string COName = dt.Rows[i]["COName"].ToString();
                                string UpFrontAmt = dt.Rows[i]["UpFrontAmt"].ToString();
                                string UpFrontFee = dt.Rows[i]["UpFrontFee"].ToString();
                                string CompulsoryPercentage = dt.Rows[i]["CompulsoryPercentage"].ToString();
                                string CompulsoryAmount = dt.Rows[i]["CompulsoryAmount"].ToString();
                                string AAID = dt.Rows[i]["AAID"].ToString();
                                string CompulsoryOptionID = dt.Rows[i]["CompulsoryOptionID"].ToString();
                                #endregion Params

                                #region get loan schedule from CBS
                                try
                                {
                                    #region xml
                                    string xmlStr = "<?xml version=\"1.0\"?><soapenv:Envelope xmlns:amk=\"http://temenos.com/AMKLOANSCHED\" "
                                    +"xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\"><soapenv:Header/><soapenv:Body><amk:WSLOANSCHEDULE><WebRequestCommon>"
                                    +"<company>"+ CreCompany + "</company>"
                                    +"<password>"+ CrePassword + "</password>"
                                    +"<userName>"+ CreUserName + "</userName>"
                                    +"</WebRequestCommon>"
                                    +"<AMKENOFLOANSCHEDULEType><enquiryInputCollection><columnName>LOAN.APP.REF</columnName>"
                                    +"<criteriaValue>"+ AAID + "</criteriaValue>"
                                    +"<operand>EQ</operand></enquiryInputCollection></AMKENOFLOANSCHEDULEType></amk:WSLOANSCHEDULE></soapenv:Body></soapenv:Envelope>";
                                    #endregion xml
                                    #region call to T24   
                                    var client = new RestClient(CreUrl);
                                    var request = new RestRequest(Method.POST);
                                    request.AddHeader("cache-control", "no-cache");
                                    request.AddHeader("content-type", "text/xml");
                                    request.AddParameter("text/xml", xmlStr, ParameterType.RequestBody);
                                    IRestResponse response = client.Execute(request);
                                    string xmlContent = response.Content.ToString();
                                    #region read xml
                                    XmlDocument doc = new XmlDocument();
                                    doc.LoadXml(xmlContent);
                                    string successIndicator = doc.GetElementsByTagName("successIndicator").Item(0).InnerText;
                                    if (successIndicator == "Success")
                                    {
                                        try {
                                            XmlNode node0 = doc.GetElementsByTagName("ns2:gAMKENOFLOANSCHEDULEDetailType").Item(0);
                                            int inode0 = node0.ChildNodes.Count;
                                            int monthly = 1;
                                            for (int n = 0; n < inode0; n++)
                                            {
                                                XmlNode node1 = doc.GetElementsByTagName("ns2:mAMKENOFLOANSCHEDULEDetailType").Item(n);

                                                try
                                                {
                                                    string LOANORGID = "", ARRANGEMENTID = "", NOOFSCHEDULE = "", CUSTOMERID = "", CUSTACCOUNT = "", CURRENCY = "", PRODCODE = ""
                                                    , LOANAMOUNT = "", UPFRONTFEE = ""
                                                    , MONTHLYFEE = "", ISSUEDDATE = "", MATURITYDATE = "", INTRATE = "", VBID = "", COID = "", GROUPID = "", SETTLEMENTACCT = ""
                                                    , LOANPURPOSEID = "", DUEDATE = "", DUEAMT = "", PRINAMT = "", INTAMT = "", CHGAMT = "", OUTSAMT = "";
                                                    #region item
                                                    foreach (XmlNode item in node1.ChildNodes)
                                                    {
                                                        try
                                                        {
                                                            string itemVal = item.InnerText;
                                                            if (item.LocalName == "LOANORGID")
                                                            {
                                                                LOANORGID = itemVal;
                                                            }
                                                            if (item.LocalName == "ARRANGEMENTID")
                                                            {
                                                                ARRANGEMENTID = itemVal;
                                                            }
                                                            if (item.LocalName == "NOOFSCHEDULE")
                                                            {
                                                                NOOFSCHEDULE = itemVal;
                                                            }
                                                            if (item.LocalName == "CUSTOMERID")
                                                            {
                                                                CUSTOMERID = itemVal;
                                                            }
                                                            if (item.LocalName == "CUSTACCOUNT")
                                                            {
                                                                CUSTACCOUNT = itemVal;
                                                            }
                                                            if (item.LocalName == "CURRENCY")
                                                            {
                                                                CURRENCY = itemVal;
                                                            }
                                                            if (item.LocalName == "PRODCODE")
                                                            {
                                                                PRODCODE = itemVal;
                                                            }
                                                            if (item.LocalName == "LOANAMOUNT")
                                                            {
                                                                LOANAMOUNT = itemVal;
                                                            }
                                                            if (item.LocalName == "UPFRONTFEE")
                                                            {
                                                                UPFRONTFEE = itemVal;
                                                            }
                                                            if (item.LocalName == "MONTHLYFEE")
                                                            {
                                                                MONTHLYFEE = itemVal;
                                                            }
                                                            if (item.LocalName == "ISSUEDDATE")
                                                            {
                                                                ISSUEDDATE = itemVal;
                                                            }
                                                            if (item.LocalName == "MATURITYDATE")
                                                            {
                                                                MATURITYDATE = itemVal;
                                                            }
                                                            if (item.LocalName == "INTRATE")
                                                            {
                                                                INTRATE = itemVal;
                                                            }
                                                            if (item.LocalName == "VBID")
                                                            {
                                                                VBID = itemVal;
                                                            }
                                                            if (item.LocalName == "COID")
                                                            {
                                                                COID = itemVal;
                                                            }
                                                            if (item.LocalName == "GROUPID")
                                                            {
                                                                GROUPID = itemVal;
                                                            }
                                                            if (item.LocalName == "SETTLEMENTACCT")
                                                            {
                                                                SETTLEMENTACCT = itemVal;
                                                            }
                                                            if (item.LocalName == "LOANPURPOSEID")
                                                            {
                                                                LOANPURPOSEID = itemVal;
                                                            }
                                                            if (item.LocalName == "DUEDATE")
                                                            {
                                                                DUEDATE = itemVal;
                                                            }
                                                            if (item.LocalName == "DUEAMT")
                                                            {
                                                                DUEAMT = itemVal;
                                                            }
                                                            if (item.LocalName == "PRINAMT")
                                                            {
                                                                PRINAMT = itemVal;
                                                            }
                                                            if (item.LocalName == "INTAMT")
                                                            {
                                                                INTAMT = itemVal;
                                                            }
                                                            if (item.LocalName == "CHGAMT")
                                                            {
                                                                CHGAMT = itemVal;
                                                            }
                                                            if (item.LocalName == "OUTSAMT")
                                                            {
                                                                OUTSAMT = itemVal;
                                                            }
                                                            if (item.LocalName == "INSURANCEFEE")
                                                            {
                                                                INSURANCEFEE = itemVal;
                                                            }
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            string xxx = "";
                                                        }
                                                    }
                                                    #endregion item
                                                    #region add list
                                                    LoanScheduleGetFromCBSList LoanScheduleData = new LoanScheduleGetFromCBSList();
                                                    LoanScheduleData.LOANORGID = LOANORGID;
                                                    LoanScheduleData.ARRANGEMENTID = ARRANGEMENTID;
                                                    LoanScheduleData.NOOFSCHEDULE = NOOFSCHEDULE;
                                                    LoanScheduleData.CUSTOMERID = CUSTOMERID;
                                                    LoanScheduleData.CUSTACCOUNT = CUSTACCOUNT;
                                                    LoanScheduleData.CURRENCY = CURRENCY;
                                                    LoanScheduleData.PRODCODE = PRODCODE;
                                                    LoanScheduleData.LOANAMOUNT = LOANAMOUNT;
                                                    LoanScheduleData.UPFRONTFEE = UPFRONTFEE;
                                                    LoanScheduleData.MONTHLYFEE = MONTHLYFEE;
                                                    LoanScheduleData.ISSUEDDATE = ISSUEDDATE;
                                                    LoanScheduleData.MATURITYDATE = MATURITYDATE;
                                                    LoanScheduleData.INTRATE = INTRATE;
                                                    LoanScheduleData.VBID = VBID;
                                                    LoanScheduleData.COID = COID;
                                                    LoanScheduleData.GROUPID = GROUPID;
                                                    LoanScheduleData.SETTLEMENTACCT = SETTLEMENTACCT;
                                                    LoanScheduleData.LOANPURPOSEID = LOANPURPOSEID;
                                                    LoanScheduleData.DUEDATE = DUEDATE;
                                                    LoanScheduleData.DUEAMT = DUEAMT;
                                                    LoanScheduleData.PRINAMT = PRINAMT;
                                                    LoanScheduleData.INTAMT = INTAMT;
                                                    LoanScheduleData.CHGAMT = CHGAMT;
                                                    LoanScheduleData.OUTSAMT = OUTSAMT;
                                                    LoanScheduleData.INSURANCEFEE = "0";
                                                    int res = monthly / 12;
                                                    if (res == 1)
                                                    {
                                                        LoanScheduleData.INSURANCEFEE = dtInsurance.Rows[0]["INSURANCEFEE"].ToString();
                                                        monthly = 0;
                                                    }
                                                    LoanScheduleGetFromCBSList.Add(LoanScheduleData);
                                                    monthly++;
                                                    #endregion add list
                                                }
                                                catch { }
                                            }
                                        } catch { }
                                    }
                                    else
                                    {
                                        //add error log

                                    }
                                    #endregion read xml
                                    #endregion call to T24

                                }
                                catch (Exception ex) {
                                    ERR = "Error";
                                    SMS = "Error while get loan schedule from CBS";
                                }
                                #endregion get loan loan from CBS

                                #region get compulsory list
                                try {
                                    DataTable dtCom = c.ReturnDT("exec T24_GenerateCompulsoryList @UserID='0',@LoanAppID='"+ LoanAppID + "'");
                                    for (int ic = 0; ic < dtCom.Rows.Count; ic++) {
                                        string OrderNo = dtCom.Rows[ic]["OrderNo"].ToString();
                                        string ComAmt = dtCom.Rows[ic]["ComAmt"].ToString();
                                        LoanScheduleCompulsoryList ComData = new LoanScheduleCompulsoryList();
                                        ComData.OrderNo = OrderNo;
                                        ComData.ComAmt = ComAmt;
                                        LoanScheduleCompulsoryList.Add(ComData);
                                    }
                                } catch { }
                                #endregion get compulsory list

                                #region add record
                                LoanAppListForSchedule LoanAppListData = new LoanAppListForSchedule();
                                LoanAppListData.LoanAppID = LoanAppID;
                                LoanAppListData.CBSKey = CBSKey;
                                LoanAppListData.CompanyName = CompanyName;
                                LoanAppListData.CompanyNameKh = CompanyNameKh;
                                LoanAppListData.VBName = VBName;
                                LoanAppListData.Village = Village;
                                LoanAppListData.VillageKh = VillageKh;
                                LoanAppListData.Commune = Commune;
                                LoanAppListData.CommuneKh = CommuneKh;
                                LoanAppListData.District = District;
                                LoanAppListData.DistrictKh = DistrictKh;
                                LoanAppListData.Province = Province;
                                LoanAppListData.ProvinceKh = ProvinceKh;
                                LoanAppListData.CustomerNameKh = CustomerNameKh;
                                LoanAppListData.GenderID = GenderID;
                                LoanAppListData.ProductName = ProductName;
                                LoanAppListData.FirstRepaymentDate = FirstRepaymentDate;
                                LoanAppListData.LoanPurpose = LoanPurpose;
                                LoanAppListData.CompulsoryAccount = CompulsoryAccount;
                                LoanAppListData.SettlementAccount = SettlementAccount;
                                LoanAppListData.COName = COName;
                                LoanAppListData.UpFrontAmt = UpFrontAmt;
                                LoanAppListData.UpFrontFee = UpFrontFee;
                                LoanAppListData.CompulsoryPercentage = CompulsoryPercentage;
                                LoanAppListData.CompulsoryAmount = CompulsoryAmount;
                                LoanAppListData.CompulsoryOptionID = CompulsoryOptionID;
                                LoanAppListData.ScheduleList = LoanScheduleGetFromCBSList;
                                LoanAppListData.CompulsoryList = LoanScheduleCompulsoryList;
                                LoanAppList.Add(LoanAppListData);
                                #endregion add record
                            }
                            if (ERR != "Error") {
                                LoanScheduleGetFromCBS.ERR = ERR;
                                LoanScheduleGetFromCBS.SMS = SMS;
                                LoanScheduleGetFromCBS.DataList = LoanAppList;
                                RSData.Add(LoanScheduleGetFromCBS);


                            }
                                
                        }
                    } catch {
                        ERR = "Error";
                        SMS = "Error while get loan from switch";
                    }
                }
                #endregion get loan by VBID and groupnumber
                    
            }
            catch (Exception ex)
            {
                ERR = "Error";
                SMS = "Something was wrong";
            }
            #region if Error
            if (ERR == "Error")
            {
                LoanScheduleGetFromCBSModel CustHeader = new LoanScheduleGetFromCBSModel();
                CustHeader.ERR = ERR;
                CustHeader.SMS = SMS;
                CustHeader.DataList = null;
                RSData.Add(CustHeader);
            }
            #endregion if Error

            return RSData;
        }

    }
    public class LoanScheduleGetFromCBSModel
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
        public List<LoanAppListForSchedule> DataList { get; set; }
        
    }
    public class LoanAppListForSchedule
    {
        public string LoanAppID { get; set; }
        public string CBSKey { get; set; }
        public string CompanyName { get; set; }
        public string CompanyNameKh { get; set; }
        public string VBName { get; set; }
        public string Village { get; set; }
        public string VillageKh { get; set; }
        public string Commune { get; set; }
        public string CommuneKh { get; set; }
        public string District { get; set; }
        public string DistrictKh { get; set; }
        public string Province { get; set; }
        public string ProvinceKh { get; set; }
        public string CustomerNameKh { get; set; }
        public string GenderID { get; set; }
        public string ProductName { get; set; }
        public string FirstRepaymentDate { get; set; }
        public string LoanPurpose { get; set; }
        public string CompulsoryAccount { get; set; }
        public string SettlementAccount { get; set; }
        public string COName { get; set; }
        public string UpFrontAmt { get; set; }
        public string UpFrontFee { get; set; }
        public string CompulsoryPercentage { get; set; }
        public string CompulsoryAmount { get; set; }
        public string CompulsoryOptionID { get; set; }

        public List<LoanScheduleGetFromCBSList> ScheduleList { get; set; }
        public List<LoanScheduleCompulsoryList> CompulsoryList { get; set; }
    }
    public class LoanScheduleGetFromCBSList
    {
        public string LOANORGID { get; set; }
        public string ARRANGEMENTID { get; set; }
        public string NOOFSCHEDULE { get; set; }
        public string CUSTOMERID { get; set; }
        public string CUSTACCOUNT { get; set; }
        public string CURRENCY { get; set; }
        public string PRODCODE { get; set; }
        public string LOANAMOUNT { get; set; }
        public string UPFRONTFEE { get; set; }
        public string MONTHLYFEE { get; set; }
        public string ISSUEDDATE { get; set; }
        public string MATURITYDATE { get; set; }
        public string INTRATE { get; set; }
        public string VBID { get; set; }
        public string COID { get; set; }
        public string GROUPID { get; set; }
        public string SETTLEMENTACCT { get; set; }
        public string LOANPURPOSEID { get; set; }
        public string DUEDATE { get; set; }
        public string DUEAMT { get; set; }
        public string PRINAMT { get; set; }
        public string INTAMT { get; set; }
        public string CHGAMT { get; set; }
        public string OUTSAMT { get; set; }
        public string INSURANCEFEE { get; set; }
    }
    public class LoanScheduleCompulsoryList {
        public string OrderNo { get; set; }
        public string ComAmt { get; set; }
    }

}