using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using System.Xml;

namespace WebService
{
    [BasicAuthentication]
    public class LoanAppUploadByAMV2Controller : ApiController
    {
        public IEnumerable<LoanAppUploadByAMV2RS> Post([FromUri]string api_name, string api_key, string username, [FromBody]string json) {
            #region incoming
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "", ExSMS = "";
            List<LoanAppUploadByAMV2RS> RSData = new List<LoanAppUploadByAMV2RS>();
            LoanAppUploadByAMV2RS ListHeader = new LoanAppUploadByAMV2RS();
            List<SMSList> RSSMSList = new List<SMSList>();
            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string ServerDateForFileName = ServerDate.Replace("-", "_").Replace(" ", "_").Replace(":", "_");
            string ControllerName = "LoanAppUploadByAMV2";
            string FileNameForLog = username + "_" + api_name + "_" + ServerDateForFileName;
            #endregion incoming
            try
            {
                string loanStatusOriginal = "";
                c.T24_AddLog(FileNameForLog, "RQ", json, ControllerName);

                #region check json
                if (json == null || json == "")
                {
                    ERR = "Error";
                    SMS = "Invalid JSON";
                    SMSList sms = new SMSList();
                    sms.SMS = SMS;
                    RSSMSList.Add(sms);
                }
                #endregion check json
                #region json
                string KeyID="", user = "", pwd = "", device_id = "", app_vName = "";
                LoanAppUploadByAMV2RQ jObj = null;
                if (ERR != "Error")
                {
                    try
                    {
                        jObj = JsonConvert.DeserializeObject<LoanAppUploadByAMV2RQ>(json.Replace("\r\n    ", ""));
                        user = jObj.user;
                        pwd = jObj.pwd;
                        device_id = jObj.device_id;
                        app_vName = jObj.app_vName;
                    }
                    catch
                    {
                        ERR = "Error";
                        SMS = "Invalid JSON";
                        SMSList sms = new SMSList();
                        sms.SMS = SMS;
                        RSSMSList.Add(sms);
                    }
                }
                #endregion json
                #region get userid
                string UserID = "";
                if (ERR != "Error")
                {
                    DataTable dt = c.ReturnDT("exec T24_check_user @user='" + user + "',@pwd='" + c.Encrypt(pwd, c.SeekKeyGet()) + "'");
                    ERR = dt.Rows[0]["ERR"].ToString();
                    SMS = dt.Rows[0]["SMS"].ToString();
                    UserID = SMS;
                    
                }
                #endregion get userid
                #region check EOD
                if (ERR != "Error")
                {
                    try
                    {
                        DataTable dt = c.ReturnDT2("exec T24_CheckIsEndOfDay @UserID='0'");
                        string isEOD = dt.Rows[0][0].ToString();
                        if (isEOD == "1")
                        {
                            ERR = "Error";
                            SMS = "Cannot Upload while End Of Day";
                            SMSList sms = new SMSList();
                            sms.SMS = SMS;
                            RSSMSList.Add(sms);
                        }
                    }
                    catch { }
                }
                #endregion
                if (ERR != "Error") {
                    try
                    {
                        foreach (LoanAppUploadByAMV2RQList l in jObj.data) {
                            try {
                                #region param
                                string LoanClientID = l.LoanClientID;
                                string LoanAppID = l.LoanAppID;
                                string LoanAppStatusID = l.LoanAppStatusID;
                                string DeskCheckID = l.DeskCheckID;
                                string PreCheckID = l.PreCheckID;
                                string AMDebtFound = l.AMDebtFound;
                                string AMDSR = l.AMDSR;
                                string AMOpinion = l.AMOpinion;
                                string AMApproveAmt = l.AMApproveAmt;
                                string AMApproveTerm = l.AMApproveTerm;
                                string AMApproveRate = l.AMApproveRate;
                                string GroupNumber = l.GroupNumber;
                                string CBSKey = l.CBSKey;
                                string AccountID = l.AccountID;
                                string DisbursementDate = l.DisbursementDate;
                                string FirstRepaymentDate = l.FirstRepaymentDate;;
                                string LoanAppCheckStatusID = l.LoanAppCheckStatusID;


                                string DisAgreeReasonID = l.DisAgreeReasonID;
                                string OtherReason = l.OtherReason;
                                

                                #endregion
                                #region param validation
                                #region DeskCheckID
                                if ((DeskCheckID != "0") && (DeskCheckID != "1"))
                                {
                                    ERR = "Error";
                                    SMS = "DeskCheckID is allowed only 0 or 1";
                                    SMSList sms = new SMSList();
                                    sms.SMS = SMS;
                                    RSSMSList.Add(sms);
                                }
                                #endregion
                                #region PreCheckID
                                if ((PreCheckID != "0") && (PreCheckID != "1"))
                                {
                                    ERR = "Error";
                                    SMS = "PreCheckID is allowed only 0 or 1";
                                    SMSList sms = new SMSList();
                                    sms.SMS = SMS;
                                    RSSMSList.Add(sms);
                                }
                                #endregion
                                #region AMDebtFound
                                try {
                                    double x = Convert.ToDouble(AMDebtFound);
                                } catch {
                                    ERR = "Error";
                                    SMS = "AMDebtFound is allowed only money";
                                    SMSList sms = new SMSList();
                                    sms.SMS = SMS;
                                    RSSMSList.Add(sms);
                                }
                                #endregion
                                #region AMDSR
                                try
                                {
                                    double x = Convert.ToDouble(AMDSR);
                                }
                                catch
                                {
                                    ERR = "Error";
                                    SMS = "AMDSR is allowed only money";
                                    SMSList sms = new SMSList();
                                    sms.SMS = SMS;
                                    RSSMSList.Add(sms);
                                }
                                #endregion
                                #region AMOpinion
                                if (AMOpinion == "") {
                                    ERR = "Error";
                                    SMS = "AMOpinion is required";
                                    SMSList sms = new SMSList();
                                    sms.SMS = SMS;
                                    RSSMSList.Add(sms);
                                }
                                #endregion
                                #region AMApproveAmt
                                try
                                {
                                    double x = Convert.ToDouble(AMApproveAmt);
                                }
                                catch
                                {
                                    ERR = "Error";
                                    SMS = "AMApproveAmt is allowed only money";
                                    SMSList sms = new SMSList();
                                    sms.SMS = SMS;
                                    RSSMSList.Add(sms);
                                }
                                #endregion
                                #region AMApproveTerm
                                try
                                {
                                    double x = Convert.ToDouble(AMApproveTerm);
                                }
                                catch
                                {
                                    ERR = "Error";
                                    SMS = "AMApproveTerm is allowed only money";
                                    SMSList sms = new SMSList();
                                    sms.SMS = SMS;
                                    RSSMSList.Add(sms);
                                }
                                #endregion
                                #region AMApproveRate
                                try
                                {
                                    double x = Convert.ToDouble(AMApproveRate);
                                }
                                catch
                                {
                                    ERR = "Error";
                                    SMS = "AMApproveRate is allowed only money";
                                    SMSList sms = new SMSList();
                                    sms.SMS = SMS;
                                    RSSMSList.Add(sms);
                                }
                                #endregion
                                #region GroupNumber
                                //try
                                //{
                                //    int x = Convert.ToInt16(GroupNumber);
                                //}
                                //catch
                                //{
                                //    ERR = "Error";
                                //    SMS = "GroupNumber is allowed only number";
                                //    SMSList sms = new SMSList();
                                //    sms.SMS = SMS;
                                //    RSSMSList.Add(sms);
                                //}
                                #endregion
                                #region CBSKey
                                if (AMOpinion == "")
                                {
                                    ERR = "Error";
                                    SMS = "CBSKey is required";
                                    SMSList sms = new SMSList();
                                    sms.SMS = SMS;
                                    RSSMSList.Add(sms);
                                }
                                #endregion
                                #region AccountID
                                if (LoanAppStatusID == "8") { 
                                    if (AccountID == "")
                                    {
                                        ERR = "Error";
                                        SMS = "AccountID is required";
                                        SMSList sms = new SMSList();
                                        sms.SMS = SMS;
                                        RSSMSList.Add(sms);
                                    }
                                }
                                #endregion
                                #region DisbursementDate
                                try
                                {
                                    DateTime x = Convert.ToDateTime(DisbursementDate);
                                }
                                catch
                                {
                                    ERR = "Error";
                                    SMS = "DisbursementDate is allowed only date";
                                    SMSList sms = new SMSList();
                                    sms.SMS = SMS;
                                    RSSMSList.Add(sms);
                                }
                                #endregion
                                #region FirstRepaymentDate
                                try
                                {
                                    DateTime x = Convert.ToDateTime(FirstRepaymentDate);
                                }
                                catch
                                {
                                    ERR = "Error";
                                    SMS = "FirstRepaymentDate is allowed only date";
                                    SMSList sms = new SMSList();
                                    sms.SMS = SMS;
                                    RSSMSList.Add(sms);
                                }
                                #endregion

                                #endregion

                                #region validation approve amount
                                if (ERR != "Error") {
                                    try
                                    {
                                        DataTable dtAppAmt = c.ReturnDT2("exec T24_ApprovalAmtValidationCheckByUserID_P2 @UserID='" + KeyID + "',@LoanAppID='" + LoanAppID + "',@Amt='" + AMApproveAmt + "'");
                                        ERR = dtAppAmt.Rows[0]["ERR"].ToString();
                                        SMS = dtAppAmt.Rows[0]["SMS"].ToString();
                                        if (ERR == "Error") {
                                            SMSList sms = new SMSList();
                                            sms.SMS = SMS;
                                            RSSMSList.Add(sms);
                                        }
                                    }
                                    catch
                                    {
                                        ERR = "Error";
                                        SMS = "Error while validating approval amount";
                                        SMSList sms = new SMSList();
                                        sms.SMS = SMS;
                                        RSSMSList.Add(sms);
                                    }
                                }                                
                                #endregion validation CUQM amount
                                
                                #region get company - LoanAppID
                                string OfficeID = "";
                                if (ERR != "Error")
                                {
                                    DataTable dtcom = c.ReturnDT2("select u.OfficeID,l.LoanAppStatusID from tblLoanApp1 l left join tblUser u on u.UserID=l.CreateBy where l.LoanAppID='" + LoanAppID + "'");
                                    OfficeID = dtcom.Rows[0]["OfficeID"].ToString();
                                    loanStatusOriginal = dtcom.Rows[0]["LoanAppStatusID"].ToString();
                                }
                                #endregion get company - LoanAppID

                                #region Desk-Check 
                                if (ERR != "Error")
                                {
                                    if (DeskCheckID == "1")
                                    {
                                        if (LoanAppCheckStatusID == "0")
                                        {
                                            if (LoanAppStatusID == "7")
                                            {
                                                LoanAppCheckStatusID = "2";
                                            }
                                            if (LoanAppStatusID == "6")
                                            {
                                                LoanAppCheckStatusID = "1";
                                            }
                                            if (LoanAppStatusID == "8")
                                            {
                                                LoanAppCheckStatusID = "4";
                                            }
                                            if (LoanAppStatusID == "9")
                                            {
                                                LoanAppCheckStatusID = "3";
                                            }
                                        }
                                        c.ReturnDT("exec T24_DeskCheckPreCheck_P2 @LoanAppID='" + LoanAppID + "',@OneDeskTwoPreCheck=1,@LoanAppCheckStatusID='" + LoanAppCheckStatusID + "',@CreateBy='" + UserID + "'");
                                    }
                                }
                                #endregion
                                #region Pre-Check 
                                if (ERR != "Error")
                                {
                                    if (PreCheckID == "1")
                                    {
                                        if(LoanAppCheckStatusID == "0") {

                                            if (LoanAppStatusID == "7")
                                            {
                                                LoanAppCheckStatusID = "2";
                                            }
                                            if (LoanAppStatusID == "6")
                                            {
                                                LoanAppCheckStatusID = "1";
                                            }
                                            if (LoanAppStatusID == "8")
                                            {
                                                LoanAppCheckStatusID = "4";
                                            }
                                            if (LoanAppStatusID == "9")
                                            {
                                                LoanAppCheckStatusID = "3";
                                            }
                                        }
                                        c.ReturnDT("exec T24_DeskCheckPreCheck_P2 @LoanAppID='" + LoanAppID + "',@OneDeskTwoPreCheck=2,@LoanAppCheckStatusID='"+ LoanAppCheckStatusID + "',@CreateBy='" + UserID + "'");
                                        c.ReturnDT("update tblLoanApp1 set AMDebtFound='" + AMDebtFound + "',AMOpinion='" + AMOpinion + "' where LoanAppID='" + LoanAppID + "'");
                                    }
                                }
                                #endregion

                                if (loanStatusOriginal == "7")
                                {
                                    #region Re-Study
                                    if (ERR != "Error")
                                    {
                                        if (LoanAppStatusID == "6")
                                        {
                                            SqlConnection Con1 = new SqlConnection(c.ConStr());
                                            Con1.Open();
                                            SqlCommand Com1 = new SqlCommand();
                                            Com1.Connection = Con1;
                                            Com1.Parameters.Clear();
                                            string sql = "T24_AMLoanReStudy_P2";
                                            Com1.CommandText = sql;
                                            Com1.CommandType = CommandType.StoredProcedure;
                                            Com1.Parameters.AddWithValue("@LoanAppID", LoanAppID);
                                            Com1.Parameters.AddWithValue("@CreateBy", UserID);
                                            Com1.Parameters.AddWithValue("@AMDebtFound", AMDebtFound);
                                            Com1.Parameters.AddWithValue("@AMDSR", AMDSR);
                                            Com1.Parameters.AddWithValue("@AMOpinion", AMOpinion);
                                            Com1.Parameters.AddWithValue("@GroupNo", GroupNumber);
                                            Com1.Parameters.AddWithValue("@DisburementDate", DisbursementDate);
                                            Com1.Parameters.AddWithValue("@FirstRepaymentDate", FirstRepaymentDate);
                                            Com1.Parameters.AddWithValue("@AMApproveAmt", AMApproveAmt);
                                            Com1.Parameters.AddWithValue("@AMApproveTerm", AMApproveTerm);
                                            Com1.Parameters.AddWithValue("@AMApproveRate", AMApproveRate);
                                            Com1.ExecuteNonQuery();
                                            Con1.Close();
                                        }
                                    }
                                    #endregion
                                    #region Reject to CBS
                                    if (ERR != "Error")
                                    {
                                        if (LoanAppStatusID == "9")
                                        {
                                            #region - get T24 url                
                                            DataTable dtT24Url = new DataTable();
                                            dtT24Url = c.ReturnDT2("exec T24_GetT24_Url @UserID=0,@UrlID=8");
                                            string CreUrl = dtT24Url.Rows[0]["CreUrl"].ToString();
                                            string CreCompany = OfficeID;// dtT24Url.Rows[0]["CreCompany"].ToString();
                                            string CreUserName = dtT24Url.Rows[0]["CreUserName"].ToString();
                                            string CrePassword = dtT24Url.Rows[0]["CrePassword"].ToString();
                                            #endregion - T24 url
                                            #region xml
                                            string xmlStr = "<?xml version=\"1.0\"?><soapenv:Envelope xmlns:amk1=\"http://temenos.com/AMKDLAPPLICATIONBOLNREJECT\" "
                                            + "xmlns:amk=\"http://temenos.com/AMKREJLOAN\" xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\"><soapenv:Header/>"
                                            + "<soapenv:Body><amk:REJECTLOAN><WebRequestCommon>"
                                            + "<company>" + CreCompany + "</company><password>" + CrePassword + "</password><userName>" + CreUserName + "</userName>"
                                            + "</WebRequestCommon><OfsFunction> </OfsFunction>"
                                            + "<AMKDLAPPLICATIONBOLNREJECTType id=\"" + CBSKey + "\"> </AMKDLAPPLICATIONBOLNREJECTType></amk:REJECTLOAN></soapenv:Body></soapenv:Envelope>";
                                            #endregion xml
                                            c.T24_AddLog(FileNameForLog, "RQ-RejectLoan", xmlStr, ControllerName);
                                            int Post1 = 0;//0=Fail, 1=Succeed
                                            string RetID = "", Remark = "", Reference = "";
                                            #region call to T24
                                            try
                                            {
                                                var client = new RestClient(CreUrl);
                                                var request = new RestRequest(Method.POST);
                                                request.AddHeader("cache-control", "no-cache");
                                                request.AddHeader("content-type", "text/xml");
                                                request.AddParameter("text/xml", xmlStr, ParameterType.RequestBody);
                                                IRestResponse response = client.Execute(request);
                                                string xmlContent = response.Content.ToString();
                                                c.T24_AddLog(FileNameForLog, "RS-RejectLoan", xmlContent, ControllerName);
                                                XmlDocument doc = new XmlDocument();
                                                doc.LoadXml(xmlContent);
                                                string successIndicator = doc.GetElementsByTagName("successIndicator").Item(0).InnerText;
                                                if (successIndicator == "Success")
                                                {
                                                    Post1 = 1;
                                                    Reference = doc.GetElementsByTagName("transactionId").Item(0).InnerText;
                                                    //rsStatusID = "1";
                                                    //rsStatusSMS = Reference;
                                                }
                                                else
                                                {
                                                    ERR = "Error";
                                                    SMS = "Error: Reject to CBS | " + Remark;
                                                    SMSList sms = new SMSList();
                                                    sms.SMS = SMS;
                                                    RSSMSList.Add(sms);
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                ERR = "Error";
                                                SMS = "Error: Reject to CBS | Can't read response";
                                                ExSMS = ex.Message.ToString();
                                                SMSList sms = new SMSList();
                                                sms.SMS = SMS;
                                                RSSMSList.Add(sms);
                                            }
                                            #endregion call to T24
                                            #region update loan status
                                            if (ERR != "Error")
                                            {
                                                try
                                                {
                                                    #region update to TabDB
                                                    SqlConnection Con1 = new SqlConnection(c.ConStr());
                                                    Con1.Open();
                                                    SqlCommand Com1 = new SqlCommand();
                                                    Com1.Connection = Con1;
                                                    Com1.Parameters.Clear();
                                                    string sql = "T24_AMLoanReject_P2";
                                                    Com1.CommandText = sql;
                                                    Com1.CommandType = CommandType.StoredProcedure;
                                                    Com1.Parameters.AddWithValue("@LoanAppID", LoanAppID);
                                                    Com1.Parameters.AddWithValue("@CreateBy", UserID);
                                                    Com1.Parameters.AddWithValue("@AMDebtFound", AMDebtFound);
                                                    Com1.Parameters.AddWithValue("@AMDSR", AMDSR);
                                                    Com1.Parameters.AddWithValue("@AMOpinion", AMOpinion);
                                                    Com1.Parameters.AddWithValue("@GroupNo", GroupNumber);
                                                    Com1.Parameters.AddWithValue("@DisburementDate", DisbursementDate);
                                                    Com1.Parameters.AddWithValue("@FirstRepaymentDate", FirstRepaymentDate);
                                                    Com1.Parameters.AddWithValue("@AMApproveAmt", AMApproveAmt);
                                                    Com1.Parameters.AddWithValue("@AMApproveTerm", AMApproveTerm);
                                                    Com1.Parameters.AddWithValue("@AMApproveRate", AMApproveRate);
                                                    Com1.Parameters.AddWithValue("@LoanAppStatusID", LoanAppStatusID);
                                                    Com1.Parameters.AddWithValue("@Remark", Remark);
                                                    //Com1.Parameters.AddWithValue("@CBSKey", RetID);
                                                    //Com1.Parameters.AddWithValue("@Reference", Reference);
                                                    Com1.Parameters.AddWithValue("@ZeroFailOneSucceed", Post1);
                                                    Com1.ExecuteNonQuery();
                                                    Con1.Close();
                                                    #endregion update to TabDB
                                                }
                                                catch (Exception ex)
                                                {
                                                    ERR = "Error";
                                                    SMS = "Error: Update loan status in switch";
                                                    ExSMS = ex.Message.ToString();
                                                    SMSList sms = new SMSList();
                                                    sms.SMS = SMS;
                                                    RSSMSList.Add(sms);
                                                }
                                            }
                                            #endregion update loan status
                                        }
                                    }
                                    #endregion
                                    #region Approve
                                    if (ERR != "Error")
                                    {

                                        if (LoanAppStatusID == "8")
                                        {
                                            #region GetApprovedCondiction Group and VB
                                            if (ERR != "Error")
                                            {
                                                if (c.GetApprovedCondiction(LoanAppID) == false)
                                                {
                                                    ERR = "Error";
                                                    SMS = "Can't approve while member of VB group and matunity condiction";
                                                    SMSList sms = new SMSList();
                                                    sms.SMS = SMS;
                                                    RSSMSList.Add(sms);
                                                }
                                            }
                                            #endregion
                                            if (ERR != "Error") {
                                                string isAllow = "";
                                                DataTable dtt = new DataTable();
                                                dtt = c.ReturnDT("select dbo.getLimitUploadLoan('" + LoanAppID + "','" + UserID + "','" + AMApproveAmt + "')");
                                                isAllow = dtt.Rows[0][0].ToString();

                                                if (isAllow == "allow")
                                                {
                                                    string CustomerID="", LoanCurrency="", ACCOUNTTITLE1="", SHORTTITLE="";

                                                    string RetID = "", Remark = "", Reference = "", ErrXml = "";
                                                    #region get T24 url                
                                                    DataTable dtT24Url = new DataTable();
                                                    dtT24Url = c.ReturnDT2("exec T24_GetT24_Url @UserID=0,@UrlID=7");
                                                    string CreUrl = dtT24Url.Rows[0]["CreUrl"].ToString();
                                                    string CreCompany = OfficeID;// dtT24Url.Rows[0]["CreCompany"].ToString();
                                                    string CreUserName = dtT24Url.Rows[0]["CreUserName"].ToString();
                                                    string CrePassword = dtT24Url.Rows[0]["CrePassword"].ToString();
                                                    #endregion - T24 url
                                                    #region Compulsory Account
                                                    if (ERR != "Error")
                                                    {
                                                        #region Get Customer Info
                                                        DataTable dtCus = c.ReturnDT2("select p.CustomerID,(p.LastName+' '+p.FirstName) as ACCOUNTTITLE1,(p.LastName+' '+p.FirstName) as SHORTTITLE,l.Currency as LoanCurrency,l.CompulsoryOptionID from tblLoanAppPerson2 p left join tblLoanApp1 l on l.LoanAppID=p.LoanAppID where p.LoanAppPersonTypeID=31 and p.LoanAppID='" + LoanAppID + "'");
                                                        CustomerID = dtCus.Rows[0]["CustomerID"].ToString();
                                                        ACCOUNTTITLE1 = dtCus.Rows[0]["ACCOUNTTITLE1"].ToString();
                                                        SHORTTITLE = dtCus.Rows[0]["SHORTTITLE"].ToString();
                                                        LoanCurrency = dtCus.Rows[0]["LoanCurrency"].ToString();
                                                        string compulsoryOpt = dtCus.Rows[0]["CompulsoryOptionID"].ToString();
                                                        #endregion Get Customer Info

                                                        if (compulsoryOpt == "1" || compulsoryOpt == "3")
                                                        {
                                                            int isAccAdd = 0;
                                                            #region Get Compulsory Account
                                                            try
                                                            {
                                                                string url = c.GetTabletWSUrl() + "/api/AccountGetFromCBS?api_name=" + c.GetTabletWSAPIName()
                                                                + "&api_key=" + c.GetTabletWSAPIKey()
                                                                + "&type=json&json=[{\"user\": \"none\", \"pwd\":\"none\",\"device_id\":\"none\",\"app_vName\":\"none\","
                                                                + "\"CreCompany\":\"" + CreCompany + "\",\"CustomerID\":\"" + CustomerID
                                                                + "\",\"CATEGORY\":\"6080\",\"ACCOUNTTITLE1\":\"" + ACCOUNTTITLE1
                                                                + "\",\"SHORTTITLE\":\"" + SHORTTITLE + "\",\"LoanCurrency\":\"" + LoanCurrency + "\"}]";
                                                                c.T24_AddLog(FileNameForLog, "RQ-GetCompulsoryAccount", url, ControllerName);
                                                                var client = new RestClient(url);
                                                                var Authenticator = new HttpBasicAuthenticator(c.GetTabletWSUser(), c.GetTabletWSPwd());
                                                                client.Authenticator = Authenticator;
                                                                var request = new RestRequest(Method.GET);
                                                                IRestResponse response = client.Execute(request);
                                                                string jres = response.Content.ToString().TrimEnd(']').TrimStart('[');
                                                                //c.T24_AddLog(fileHeader, "GetCompulsoryAccount", response.Content.ToString().TrimEnd(']').TrimStart('['));
                                                                c.T24_AddLog(FileNameForLog, "RS-GetCompulsoryAccount", jres, ControllerName);
                                                                AccountCreationToCBSRes.AccountCreationToCBSModel ObjC = null;
                                                                try
                                                                {
                                                                    ObjC = JsonConvert.DeserializeObject<AccountCreationToCBSRes.AccountCreationToCBSModel>(jres);
                                                                    int aiCount = 0;
                                                                    if (ObjC != null)
                                                                    {
                                                                        foreach (var ai in ObjC.DataList)
                                                                        {
                                                                            aiCount++;
                                                                            if (aiCount == 1)
                                                                            {
                                                                                isAccAdd = 1;
                                                                                string CompulsoryAccountID = ai.AccountID;
                                                                                c.ReturnDT("update tblLoanApp1 set CompulsoryAccountID='" + CompulsoryAccountID + "' where LoanAppID='" + LoanAppID + "'");
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                catch (Exception ex)
                                                                {
                                                                    //c.T24_AddLog(fileHeader, "GetCompulsoryAccount: Check jObj: ", ex.Message.ToString() + " at line: " + c.GetLineNumber(ex));
                                                                    c.T24_AddLog(FileNameForLog, "RS-GetCompulsoryAccount", ex.Message.ToString(), ControllerName);
                                                                }

                                                            }
                                                            catch (Exception ex)
                                                            {
                                                                ERR = "Error";
                                                                SMS = "Error: Get Compulsory Account";
                                                                Remark = ex.Message.ToString();
                                                                //c.T24_AddLog(fileHeader, "GetCompulsoryAccount", Remark);
                                                                c.T24_AddLog(FileNameForLog, "RQ-GetCompulsoryAccount", ex.Message.ToString(), ControllerName);
                                                                SMSList sms = new SMSList();
                                                                sms.SMS = SMS;
                                                                RSSMSList.Add(sms);
                                                            }
                                                            #endregion Get Compulsory Account

                                                            #region Create Compulsory Account
                                                            if (isAccAdd == 0)
                                                            {
                                                                try
                                                                {
                                                                    string url = c.GetTabletWSUrl() + "/api/AccountCreationToCBS?api_name=" + c.GetTabletWSAPIName()
                                                                    + "&api_key=" + c.GetTabletWSAPIKey()
                                                                    + "&type=json&json=[{\"user\": \"none\", \"pwd\":\"none\",\"device_id\":\"none\",\"app_vName\":\"none\","
                                                                    + "\"CreCompany\":\"" + CreCompany + "\",\"CustomerID\":\"" + CustomerID
                                                                    + "\",\"CATEGORY\":\"6080\",\"ACCOUNTTITLE1\":\"" + ACCOUNTTITLE1
                                                                    + "\",\"SHORTTITLE\":\"" + SHORTTITLE + "\",\"LoanCurrency\":\"" + LoanCurrency + "\"}]";
                                                                    c.T24_AddLog(FileNameForLog, "RQ-CreateCompulsoryAccount", url, ControllerName);
                                                                    var client = new RestClient(url);
                                                                    var Authenticator = new HttpBasicAuthenticator(c.GetTabletWSUser(), c.GetTabletWSPwd());
                                                                    client.Authenticator = Authenticator;
                                                                    var request = new RestRequest(Method.POST);
                                                                    IRestResponse response = client.Execute(request);
                                                                    string jres = response.Content.ToString().TrimEnd(']').TrimStart('[');
                                                                    c.T24_AddLog(FileNameForLog, "RS-CreateCompulsoryAccount", jres, ControllerName);
                                                                    AccountCreationToCBSRes.AccountCreationToCBSModel ObjC = JsonConvert.DeserializeObject<AccountCreationToCBSRes.AccountCreationToCBSModel>(jres);
                                                                    foreach (var ai in ObjC.DataList)
                                                                    {
                                                                        string CompulsoryAccountID = ai.AccountID;
                                                                        c.ReturnDT("update tblLoanApp1 set CompulsoryAccountID='" + CompulsoryAccountID + "' where LoanAppID='" + LoanAppID + "'");
                                                                    }
                                                                }
                                                                catch (Exception ex)
                                                                {
                                                                    ERR = "Error";
                                                                    SMS = "Error: Create Compulsory Account";
                                                                    Remark = ex.Message.ToString();
                                                                    c.T24_AddLog(FileNameForLog, "RQ-CreateCompulsoryAccount", ex.Message.ToString(), ControllerName);
                                                                    SMSList sms = new SMSList();
                                                                    sms.SMS = SMS;
                                                                    RSSMSList.Add(sms);
                                                                }
                                                            }
                                                            #endregion Create Compulsory Account
                                                        }
                                                    }
                                                    #endregion Compulsory Account
                                                    #region Approve to CBS
                                                    if (ERR != "Error")
                                                    {
                                                        #region xml
                                                        string xmlStr = "<?xml version=\"1.0\"?><soapenv:Envelope xmlns:amk1=\"http://temenos.com/AMKDLAPPLICATIONBOLNAPPROVE\" "
                                                        + "xmlns:amk=\"http://temenos.com/AMKAPPLOAN\" xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\"><soapenv:Header/>"
                                                        + "<soapenv:Body><amk:APPROVELOAN><WebRequestCommon>"
                                                        + "<company>" + CreCompany + "</company><password>" + CrePassword + "</password><userName>" + CreUserName + "</userName>"
                                                        + "</WebRequestCommon><OfsFunction> </OfsFunction>"
                                                        + "<AMKDLAPPLICATIONBOLNAPPROVEType id=\"" + CBSKey + "\">"
                                                        + "<amk1:DRAWDOWNACCT>" + AccountID + "</amk1:DRAWDOWNACCT><amk1:SETTLEMENTACCT>" + AccountID + "</amk1:SETTLEMENTACCT>"
                                                        + "</AMKDLAPPLICATIONBOLNAPPROVEType></amk:APPROVELOAN>"
                                                        + "</soapenv:Body></soapenv:Envelope>";
                                                        #endregion xml
                                                        c.T24_AddLog(FileNameForLog, "RQ-ApproveLoan", xmlStr, ControllerName);
                                                        #region call to T24
                                                        string AAID = "";
                                                        try
                                                        {
                                                            var client = new RestClient(CreUrl);
                                                            var request = new RestRequest(Method.POST);
                                                            request.AddHeader("cache-control", "no-cache");
                                                            request.AddHeader("content-type", "text/xml");
                                                            request.AddParameter("text/xml", xmlStr, ParameterType.RequestBody);
                                                            IRestResponse response = client.Execute(request);
                                                            string xmlContent = response.Content.ToString();
                                                            //c.T24_AddLog(fileHeader, "ApproveLoan", xmlContent);
                                                            c.T24_AddLog(FileNameForLog, "RS-ApproveLoan", xmlContent, ControllerName);
                                                            XmlDocument doc = new XmlDocument();
                                                            doc.LoadXml(xmlContent);
                                                            string successIndicator = doc.GetElementsByTagName("successIndicator").Item(0).InnerText;
                                                            if (successIndicator == "Success")
                                                            {
                                                                #region Success
                                                                Reference = doc.GetElementsByTagName("transactionId").Item(0).InnerText;
                                                                //rsStatusID = "1";
                                                                //rsStatusSMS = Reference;
                                                                //get AAID
                                                                if (xmlContent.Contains("ARRANGEMENT:1:1="))
                                                                {
                                                                    int pFrom = xmlContent.IndexOf("ARRANGEMENT:1:1=") + "ARRANGEMENT:1:1=".Length;
                                                                    int pTo = xmlContent.IndexOf("ARRANGEMENT:1:1=") + "ARRANGEMENT:1:1=".Length + 12;
                                                                    AAID = xmlContent.Substring(pFrom, pTo - pFrom).Trim();
                                                                }
                                                                #endregion Success
                                                            }
                                                            else
                                                            {
                                                                ErrXml = xmlContent;
                                                                #region Error
                                                                ERR = "Error";
                                                                //rsStatusSMS = "Error: Approve to CBS | " + Remark;
                                                                if (xmlContent.Contains("<Status>"))
                                                                {
                                                                    int pFrom = xmlContent.IndexOf("<Status>");// + "<Status>".Length;
                                                                    int pTo = xmlContent.IndexOf("</Status>") + "</Status>".Length;
                                                                    string MSGxmlContent = xmlContent.Substring(pFrom, pTo - pFrom).Trim();
                                                                    LoanAppApproveToCBSError.Status obj = c.GenerateXmlObject<LoanAppApproveToCBSError.Status>(MSGxmlContent);
                                                                    string strMsg = "";
                                                                    for (int iMsg = 0; iMsg < obj.Messages.Count; iMsg++)
                                                                    {
                                                                        if (iMsg == 0)
                                                                        {
                                                                            strMsg = obj.Messages[iMsg];
                                                                        }
                                                                        else
                                                                        {
                                                                            strMsg = strMsg + " | " + obj.Messages[iMsg];
                                                                        }
                                                                    }
                                                                    SMS = strMsg;
                                                                    SMSList sms = new SMSList();
                                                                    sms.SMS = SMS;
                                                                    RSSMSList.Add(sms);
                                                                }
                                                                #endregion Error
                                                            }
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            ERR = "Error";
                                                            SMS = "Error: Approve to CBS | Can't read response";
                                                            ExSMS = ex.Message.ToString();
                                                            SMSList sms = new SMSList();
                                                            sms.SMS = SMS;
                                                            RSSMSList.Add(sms);
                                                        }
                                                        //c.T24_AddLog(FileNameForLog, "RS-ApproveLoan-Finish", ERR+" | "+ SMS+" | "+ ExSMS, ControllerName);
                                                        #endregion call to T24                                    
                                                        #region update loan app
                                                        try
                                                        {
                                                            string ZeroFailOneSucceed = "1";
                                                            if (ERR == "Error")
                                                            {
                                                                ZeroFailOneSucceed = "0";
                                                            }

                                                            try
                                                            {
                                                                AMDebtFound = Convert.ToDouble(AMDebtFound).ToString();
                                                            }
                                                            catch
                                                            {
                                                                AMDebtFound = "0";
                                                            }
                                                            #region add kyc - xml create saving
                                                            //c.T24_AddLog(FileNameForLog, "note ZeroFailOneSucceed", ZeroFailOneSucceed, ControllerName);
                                                            string xmlCreateSaving = "";
                                                            if (ZeroFailOneSucceed == "1") {
                                                                string jsonWS = "[{\"user\":\"none\",\"pwd\":\"none\",\"device_id\":\"none\",\"app_vName\":\"none\""
                                                                        + ",\"CreCompany\":\"" + CreCompany + "\""
                                                                        + ",\"CustomerID\":\"" + CustomerID + "\""
                                                                        + ",\"CATEGORY\":\"6001\""
                                                                        + ",\"ACCOUNTTITLE1\":\"" + ACCOUNTTITLE1 + "\""
                                                                        + ",\"SHORTTITLE\":\"" + SHORTTITLE + "\""
                                                                        + ",\"LoanCurrency\":\"" + LoanCurrency + "\"}]";
                                                                string url = c.GetTabletWSUrl() + "/api/AccountCreationToCBS?api_name=" + c.GetTabletWSAPIName() + "&api_key=" + c.GetTabletWSAPIKey() + "&type=json&json=" + jsonWS;
                                                                xmlCreateSaving = url;
                                                            }
                                                            //c.T24_AddLog(FileNameForLog, "Create Saving XML", xmlCreateSaving, ControllerName);
                                                            #endregion
                                                            SqlConnection Con1 = new SqlConnection(c.ConStr());
                                                            Con1.Open();
                                                            SqlCommand Com1 = new SqlCommand();
                                                            Com1.Connection = Con1;
                                                            Com1.Parameters.Clear();
                                                            //add kyc - saving in this sp
                                                            string sql = "T24_AMApproveLoan_P2";
                                                            Com1.CommandText = sql;
                                                            Com1.CommandType = CommandType.StoredProcedure;
                                                            Com1.Parameters.AddWithValue("@UserID", UserID);
                                                            Com1.Parameters.AddWithValue("@LoanAppID", LoanAppID);
                                                            Com1.Parameters.AddWithValue("@LoanAppStatusID", "10");
                                                            Com1.Parameters.AddWithValue("@CBSKey", CBSKey);
                                                            Com1.Parameters.AddWithValue("@Remark", Remark);
                                                            Com1.Parameters.AddWithValue("@Reference", Reference);
                                                            Com1.Parameters.AddWithValue("@ZeroFailOneSucceed", ZeroFailOneSucceed);
                                                            Com1.Parameters.AddWithValue("@AMDebtFound", AMDebtFound.Replace(",","").ToString());
                                                            Com1.Parameters.AddWithValue("@AMDSR", AMDSR);
                                                            Com1.Parameters.AddWithValue("@AMOpinion", AMOpinion);
                                                            Com1.Parameters.AddWithValue("@AMApproveAmt", AMApproveAmt.Replace(",", "").ToString());
                                                            Com1.Parameters.AddWithValue("@AMApproveTerm", AMApproveTerm.Replace(",", "").ToString());
                                                            Com1.Parameters.AddWithValue("@AMApproveRate", AMApproveRate.Replace(",", "").ToString());
                                                            Com1.Parameters.AddWithValue("@GroupNo", GroupNumber);
                                                            Com1.Parameters.AddWithValue("@DisburementDate", DisbursementDate);
                                                            Com1.Parameters.AddWithValue("@FirstRepaymentDate", FirstRepaymentDate);
                                                            Com1.Parameters.AddWithValue("@AccountID", AccountID);
                                                            Com1.Parameters.AddWithValue("@AAID", AAID);
                                                            Com1.Parameters.AddWithValue("@xmlCreateSaving", xmlCreateSaving);
                                                            Com1.ExecuteNonQuery();
                                                            Con1.Close();
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            ERR = "Error";
                                                            SMS = "Error: Update loan status in switch";
                                                            ExSMS = ex.Message.ToString();
                                                            SMSList sms = new SMSList();
                                                            sms.SMS = SMS;
                                                            RSSMSList.Add(sms);
                                                        }
                                                        #endregion
                                                    }
                                                    #endregion
                                                }
                                                else
                                                {
                                                    ERR = "Error";
                                                    SMS = "Over approve amount";
                                                    SMSList sms = new SMSList();
                                                    sms.SMS = SMS;
                                                    RSSMSList.Add(sms);
                                                }
                                            }
                                            
                                        }
                                    }
                                    #endregion
                                }
                                else {
                                    ERR = "Error";
                                    SMS = "LIVE RECORD NOT CHANGED";
                                    SMSList sms = new SMSList();
                                    sms.SMS = SMS;
                                    RSSMSList.Add(sms);
                                }

                                #region DisAgreeReason
                                if (ERR != "Error")
                                {
                                    if (DisAgreeReasonID != "")
                                    {
                                        try
                                        {
                                            c.ReturnDT("exec v2_tbl_LoanAppRejectReason_ins '" + LoanAppID + "',N'" + DisAgreeReasonID + "',N'" + OtherReason + "','" + UserID + "'");
                                        }
                                        catch (Exception ex)
                                        {
                                            ERR = "Error";
                                            SMS = "DisAgree reason error !";
                                            ExSMS = ex.Message.ToString();
                                            SMSList sms = new SMSList();
                                            sms.SMS = SMS;
                                            RSSMSList.Add(sms);
                                        }
                                    }
                                }
                                #endregion

                            }
                            catch (Exception ex)
                            {
                                ERR = "Error";
                                SMS = "Something was wrong";
                                ExSMS = ex.Message.ToString();
                                SMSList sms = new SMSList();
                                sms.SMS = SMS;
                                RSSMSList.Add(sms);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ERR = "Error";
                        SMS = "Something was wrong";
                        ExSMS = ex.Message.ToString();
                        SMSList sms = new SMSList();
                        sms.SMS = SMS;
                        RSSMSList.Add(sms);
                    }
                }
                
            }
            catch (Exception ex) {
                ERR = "Error";
                SMS = "Something was wrong";// at line:" + c.GetLineNumber(ex) + " | Ex:" + ex.Message.ToString();
                ExSMS = ex.Message.ToString();
                SMSList sms = new SMSList();
                sms.SMS = SMS;
                RSSMSList.Add(sms);
            }
            #region return
            ListHeader.ERR = ERR;
            ListHeader.SMS = RSSMSList;
            RSData.Add(ListHeader);
            try
            {
                if (ERR == "Error")
                {
                    c.T24_AddLog(FileNameForLog, "RS-Ex", ExSMS, ControllerName);
                }

                var jsonRS = new JavaScriptSerializer().Serialize(RSData);
                c.T24_AddLog(FileNameForLog, "RS-Tab", jsonRS.ToString(), ControllerName);
            }
            catch { }
            return RSData;
            #endregion return
        }        
    }
    public class LoanAppUploadByAMV2RQ
    {
        public string user { get; set; }
        public string pwd { get; set; }
        public string device_id { get; set; }
        public string app_vName { get; set; }
        public List<LoanAppUploadByAMV2RQList> data;
    }
    public class LoanAppUploadByAMV2RQList
    {
        public string LoanClientID { get; set; }//old - IDOnDevice
        public string LoanAppID { get; set; }//ok
        public string LoanAppStatusID { get; set; }//ok//7=just-review-only | 6=Re-Study | 8=AM-Approved | 9=AM-Rejected
        public string DeskCheckID { get; set; }//ok 0 | 1
        public string PreCheckID { get; set; }//ok 0 | 1
        public string AMDebtFound { get; set; }//no -> add
        public string AMDSR { get; set; }//No -> add
        public string AMOpinion { get; set; }//No -> add
        public string AMApproveAmt { get; set; }//ok
        public string AMApproveTerm { get; set; }//ok
        public string AMApproveRate { get; set; }//ok
        public string GroupNumber { get; set; }//ok
        public string CBSKey { get; set; }//no -> add
        public string AccountID { get; set; }//no -> add
        public string DisbursementDate { get; set; }//ok
        public string FirstRepaymentDate { get; set; }//ok
        public string LoanAppCheckStatusID { get; set; } = "0";//ok

        public string DisAgreeReasonID { get; set; }
        public string OtherReason { get; set; }
    }
    public class LoanAppUploadByAMV2RS
    {
        public string ERR { get; set; }
        public List<SMSList> SMS;
    }
    public class SMSList {
        public string SMS { get; set; }
    }

}