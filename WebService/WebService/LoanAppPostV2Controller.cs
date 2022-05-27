using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace WebService
{
    [BasicAuthentication]
    public class LoanAppPostV2Controller : ApiController
    {
        // POST api/<controller>
        //public IEnumerable<LoanAppResModel> Post([FromUri]string api_name, string api_key, string username, [FromBody]string json)
        public string Post([FromUri]string p, [FromUri]string msgid, [FromBody]string json)
        {
            #region incoming
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "", ExSMS = "", ERRCode = "", RSIDOnDevice = "", RSLoanAppID = "";
            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string FileNameForLog = msgid + "_" + ServerDate.Replace("-", "_").Replace(" ", "_").Replace(":", "_").Replace(".", "_");
            string ControllerName = "LoanAppPostV2";

            string ServerDateForFileName = ServerDate.Replace("-", "_").Replace(" ", "_").Replace(":", "_").Replace(".", "_");
            List<LoanAppResModel> RSData = new List<LoanAppResModel>();
            LoanAppResModel ListHeader = new LoanAppResModel();
            List<LoanAppResSMS> SMSList = new List<LoanAppResSMS>();
            List<LoanAppResImgList> ImgList = new List<LoanAppResImgList>();
            #endregion incoming
            try
            {
                #region msgid
                if (ERR != "Error")
                {
                    string[] str = c.CheckMsgID(msgid);
                    ERR = str[0];
                    SMS = str[1];
                }
                #endregion
                #region add log
                if (ERR != "Error")
                {
                    c.T24_AddLog(FileNameForLog, "1.RQ", p, ControllerName);
                }
                #endregion
                #region p -> SessionID
                string UserID = "";
                if (ERR != "Error")
                {
                    //p = System.Web.HttpUtility.UrlDecode(p);
                    string[] rs = c.SessionIDCheck(ServerDate, p);
                    ERR = rs[0];
                    SMS = rs[1];
                    ExSMS = rs[2];
                    UserID = rs[3];
                    ERRCode = rs[4];
                }
                #endregion
                #region check json
                if (ERR != "Error")
                {
                    c.T24_AddLog(FileNameForLog, "1.1.RQJson", json, ControllerName);
                    string[] str = c.CheckObjED(json, "2");
                    ERR = str[0];
                    SMS = str[1];
                    ExSMS = str[2];
                    json = str[3];
                    c.T24_AddLog(FileNameForLog, "1.2.RQJsonDE", json, ControllerName);
                }
                #endregion check json
                #region read json
                string user = "", pwd = "", app_vName = "";
                LoanAppPostV2RQ jObj = null;
                if (ERR != "Error")
                {
                    try
                    {
                        jObj = JsonConvert.DeserializeObject<LoanAppPostV2RQ>(json.Replace("\r\n    ", ""));
                        user = jObj.user;
                        pwd = jObj.pwd;
                        app_vName = jObj.app_vName;
                    }
                    catch(Exception ex)
                    {
                        ERR = "Error";
                        SMS = "Invalid JSON";
                        LoanAppResSMS data = new LoanAppResSMS();
                        data.SMS = SMS;
                        SMSList.Add(data);
                    }
                }
                #endregion
                #region check smslist
                if (ERR != "Error")
                {
                    if (SMSList.Count > 0)
                    {
                        ERR = "Error";
                        SMS = "Something was wrong in LoanApp";
                    }
                }
                #endregion check smslist

                #region Validation
                if (ERR != "Error")
                {
                    string ServerLoanAppID = "0";
                    try
                    {
                        string PersonType = "31";
                        foreach (var loan in jObj.LoanApp)
                        {
                            try
                            {
                                #region LoanApp Param
                                string LoanClientID = loan.LoanClientID;
                                string LoanAppID = loan.LoanAppID;
                                string LoanAppStatusID = loan.LoanAppStatusID;
                                string DeviceDate = loan.DeviceDate;
                                string ProductID = loan.ProductID;
                                string LoanRequestAmount = loan.LoanRequestAmount.Replace(",", "");
                                string OwnCapital = loan.OwnCapital;
                                string DisbursementDate = loan.DisbursementDate;
                                string FirstWithdrawal = loan.FirstWithdrawal;
                                string LoanTerm = loan.LoanTerm;
                                string FirstRepaymentDate = loan.FirstRepaymentDate;
                                string LoanInterestRate = loan.LoanInterestRate;
                                string CustomerRequestRate = loan.CustomerRequestRate;
                                string CompititorRate = loan.CompititorRate;
                                string CustomerConditionID = loan.CustomerConditionID;
                                string COProposedAmount = loan.COProposedAmount;
                                string COProposedTerm = loan.COProposedTerm;
                                string COProposeRate = loan.COProposeRate;
                                string FrontBackOfficeID = loan.FrontBackOfficeID;
                                string GroupNumber = loan.GroupNumber;
                                string LoanCycleID = loan.LoanCycleID;
                                string RepaymentHistoryID = loan.RepaymentHistoryID;
                                string LoanReferralID = loan.LoanReferralID;
                                //string DebtIinfoID = loan.DebtIinfoID;
                                string MonthlyFee = loan.MonthlyFee;
                                string Compulsory = loan.Compulsory;
                                string CompulsoryTerm = loan.CompulsoryTerm;
                                string Currency = loan.Currency;
                                string UpFrontFee = loan.UpFrontFee;
                                //string UpFrontAmt=loan.UpFrontAmt;
                                string CompulsoryOptionID = loan.CompulsoryOptionID;
                                string FundSource = loan.FundSource;
                                string IsNewCollateral = loan.IsNewCollateral;
                                string AgriBuddy = loan.AgriBuddy;
                                string semiBallonFreqID = loan.SemiBallonFreqID;

                                string LoanTypeID = loan.LoanTypeID;
                                //string AMApproveAmt = loan.AMApproveAmt;
                                //string AMApproveTerm = loan.AMApproveTerm;
                                //string AMApproveRate = loan.AMApproveRate;
                                string PaymentMethodID = loan.PaymentMethodID;
                                string GracePeriodID = loan.GracePeriodID;
                                string MITypeID = loan.MITypeID;

                                string DebtIinfoID = "1";
                                foreach (var Person in loan.Person)
                                {
                                    PersonType = Person.LoanAppPersonTypeID;
                                    if (Person.LoanAppPersonTypeID == "31")
                                    {
                                        if (Person.Creditor != null)
                                        {
                                            if (Person.Creditor !=null)
                                            {
                                                DebtIinfoID = "2";
                                            }
                                        }
                                    }

                                }

                                string UpFromAmt = "";
                                UpFromAmt = ((Convert.ToDouble(LoanRequestAmount) * Convert.ToDouble(UpFrontFee)) / 100).ToString();

                                #endregion LoanApp Param
                                #region sql
                                try
                                {
                                    #region LoanClientID
                                    if (LoanClientID == null || LoanClientID == "")
                                    {
                                        LoanAppResSMS data = new LoanAppResSMS();
                                        data.SMS = "LoanApp - LoanClientID is required";
                                        SMSList.Add(data);
                                    }
                                    #endregion
                                    #region LoanAppID
                                    if (LoanAppID == null || LoanAppID == "")
                                    {
                                        LoanAppResSMS data = new LoanAppResSMS();
                                        data.SMS = "LoanApp - LoanAppID is required";
                                        SMSList.Add(data);
                                    }
                                    else
                                    {
                                        try
                                        {
                                            int x = Convert.ToInt32(LoanAppID);
                                        }
                                        catch
                                        {
                                            LoanAppResSMS data = new LoanAppResSMS();
                                            data.SMS = "LoanApp - LoanAppID is required in INT";
                                            SMSList.Add(data);
                                        }
                                    }
                                    #endregion
                                    #region LoanAppStatusID
                                    if (LoanAppStatusID == null || LoanAppStatusID == "")
                                    {
                                        LoanAppResSMS data = new LoanAppResSMS();
                                        data.SMS = "LoanApp - LoanAppStatusID is required";
                                        SMSList.Add(data);
                                    }
                                    else
                                    {
                                        try
                                        {
                                            int x = Convert.ToInt32(LoanAppID);
                                        }
                                        catch
                                        {
                                            LoanAppResSMS data = new LoanAppResSMS();
                                            data.SMS = "LoanApp - LoanAppStatusID is required in INT";
                                            SMSList.Add(data);
                                        }
                                    }
                                    #endregion
                                    #region DeviceDate
                                    if (DeviceDate == null || DeviceDate == "")
                                    {
                                        LoanAppResSMS data = new LoanAppResSMS();
                                        data.SMS = "LoanApp - DeviceDate is required";
                                        SMSList.Add(data);
                                    }
                                    else
                                    {
                                        try
                                        {
                                            DateTime x = Convert.ToDateTime(DeviceDate);
                                        }
                                        catch
                                        {
                                            LoanAppResSMS data = new LoanAppResSMS();
                                            data.SMS = "LoanApp - DeviceDate is required in DateTime";
                                            SMSList.Add(data);
                                        }
                                    }
                                    #endregion
                                    #region ProductID
                                    if (ProductID == null || ProductID == "")
                                    {
                                        LoanAppResSMS data = new LoanAppResSMS();
                                        data.SMS = "LoanApp - ProductID is required";
                                        SMSList.Add(data);
                                    }
                                    #endregion
                                    #region LoanRequestAmount
                                    if (LoanRequestAmount == null || LoanRequestAmount == "")
                                    {
                                        LoanAppResSMS data = new LoanAppResSMS();
                                        data.SMS = "LoanApp - LoanRequestAmount is required";
                                        SMSList.Add(data);
                                    }
                                    else
                                    {
                                        try
                                        {
                                            double x = Convert.ToDouble(LoanRequestAmount);
                                        }
                                        catch
                                        {
                                            LoanAppResSMS data = new LoanAppResSMS();
                                            data.SMS = "LoanApp - LoanRequestAmount is required in Money";
                                            SMSList.Add(data);
                                        }
                                    }
                                    #endregion
                                    #region OwnCapital
                                    if (OwnCapital == null || OwnCapital == "")
                                    {
                                        LoanAppResSMS data = new LoanAppResSMS();
                                        data.SMS = "LoanApp - OwnCapital is required";
                                        SMSList.Add(data);
                                    }
                                    else
                                    {
                                        try
                                        {
                                            double x = Convert.ToDouble(OwnCapital);
                                        }
                                        catch
                                        {
                                            LoanAppResSMS data = new LoanAppResSMS();
                                            data.SMS = "LoanApp - OwnCapital is required in Money";
                                            SMSList.Add(data);
                                        }
                                    }
                                    #endregion
                                    #region DisbursementDate
                                    if (DisbursementDate == null || DisbursementDate == "")
                                    {
                                        LoanAppResSMS data = new LoanAppResSMS();
                                        data.SMS = "LoanApp - DisbursementDate is required";
                                        SMSList.Add(data);
                                    }
                                    else
                                    {
                                        try
                                        {
                                            DateTime x = Convert.ToDateTime(DisbursementDate);
                                        }
                                        catch
                                        {
                                            LoanAppResSMS data = new LoanAppResSMS();
                                            data.SMS = "LoanApp - DisbursementDate is required in Date";
                                            SMSList.Add(data);
                                        }
                                    }
                                    #endregion
                                    #region FirstWithdrawal
                                    if (FirstWithdrawal == null || FirstWithdrawal == "")
                                    {
                                        LoanAppResSMS data = new LoanAppResSMS();
                                        data.SMS = "LoanApp - FirstWithdrawal is required";
                                        SMSList.Add(data);
                                    }
                                    else
                                    {
                                        try
                                        {
                                            double x = Convert.ToDouble(FirstWithdrawal);
                                        }
                                        catch
                                        {
                                            LoanAppResSMS data = new LoanAppResSMS();
                                            data.SMS = "LoanApp - FirstWithdrawal is required in Money";
                                            SMSList.Add(data);
                                        }
                                    }
                                    #endregion
                                    #region LoanTerm
                                    if (LoanTerm == null || LoanTerm == "")
                                    {
                                        LoanAppResSMS data = new LoanAppResSMS();
                                        data.SMS = "LoanApp - LoanTerm is required";
                                        SMSList.Add(data);
                                    }
                                    else
                                    {
                                        try
                                        {
                                            int x = Convert.ToInt32(LoanTerm);
                                        }
                                        catch
                                        {
                                            LoanAppResSMS data = new LoanAppResSMS();
                                            data.SMS = "LoanApp - LoanTerm is required in INT";
                                            SMSList.Add(data);
                                        }
                                    }
                                    #endregion
                                    #region  FirstRepaymentDate
                                    if (FirstRepaymentDate == null || FirstRepaymentDate == "")
                                    {
                                        LoanAppResSMS data = new LoanAppResSMS();
                                        data.SMS = "LoanApp - FirstRepaymentDate is required";
                                        SMSList.Add(data);
                                    }
                                    else
                                    {
                                        try
                                        {
                                            DateTime x = Convert.ToDateTime(FirstRepaymentDate);
                                        }
                                        catch
                                        {
                                            LoanAppResSMS data = new LoanAppResSMS();
                                            data.SMS = "LoanApp - FirstRepaymentDate is required in DateTime";
                                            SMSList.Add(data);
                                        }
                                    }
                                    #endregion
                                    #region  LoanInterestRate
                                    if (LoanInterestRate == null || LoanInterestRate == "")
                                    {
                                        LoanAppResSMS data = new LoanAppResSMS();
                                        data.SMS = "LoanApp - LoanInterestRate is required";
                                        SMSList.Add(data);
                                    }
                                    else
                                    {
                                        try
                                        {
                                            double x = Convert.ToDouble(LoanInterestRate);
                                        }
                                        catch
                                        {
                                            LoanAppResSMS data = new LoanAppResSMS();
                                            data.SMS = "LoanApp - LoanInterestRate is required in Money";
                                            SMSList.Add(data);
                                        }
                                    }
                                    #endregion  
                                    #region CustomerRequestRate
                                    if (CustomerRequestRate == null || CustomerRequestRate == "")
                                    {
                                        LoanAppResSMS data = new LoanAppResSMS();
                                        data.SMS = "LoanApp - CustomerRequestRate is required";
                                        SMSList.Add(data);
                                    }
                                    else
                                    {
                                        try
                                        {
                                            double x = Convert.ToDouble(CustomerRequestRate);
                                        }
                                        catch
                                        {
                                            LoanAppResSMS data = new LoanAppResSMS();
                                            data.SMS = "LoanApp - CustomerRequestRate is required in Money";
                                            SMSList.Add(data);
                                        }
                                    }
                                    #endregion 
                                    #region CompititorRate
                                    if (CompititorRate == null || CompititorRate == "")
                                    {
                                        LoanAppResSMS data = new LoanAppResSMS();
                                        data.SMS = "LoanApp - CompititorRate is required";
                                        SMSList.Add(data);
                                    }
                                    else
                                    {
                                        try
                                        {
                                            double x = Convert.ToDouble(CompititorRate);
                                        }
                                        catch
                                        {
                                            LoanAppResSMS data = new LoanAppResSMS();
                                            data.SMS = "LoanApp - CompititorRate is required in Money";
                                            SMSList.Add(data);
                                        }
                                    }
                                    #endregion 
                                    #region CustomerConditionID
                                    if (CustomerConditionID == null || CustomerConditionID == "")
                                    {
                                        LoanAppResSMS data = new LoanAppResSMS();
                                        data.SMS = "LoanApp - CustomerConditionID is required";
                                        SMSList.Add(data);
                                    }
                                    else
                                    {
                                        try
                                        {
                                            int x = Convert.ToInt32(CustomerConditionID);
                                        }
                                        catch
                                        {
                                            LoanAppResSMS data = new LoanAppResSMS();
                                            data.SMS = "LoanApp - CustomerConditionID is required in INT";
                                            SMSList.Add(data);
                                        }
                                    }
                                    #endregion
                                    #region COProposedAmount
                                    if (COProposedAmount == null || COProposedAmount == "")
                                    {
                                        LoanAppResSMS data = new LoanAppResSMS();
                                        data.SMS = "LoanApp - COProposedAmount is required";
                                        SMSList.Add(data);
                                    }
                                    else
                                    {
                                        try
                                        {
                                            double x = Convert.ToDouble(COProposedAmount);
                                        }
                                        catch
                                        {
                                            LoanAppResSMS data = new LoanAppResSMS();
                                            data.SMS = "LoanApp - COProposedAmount is required in Money";
                                            SMSList.Add(data);
                                        }
                                    }
                                    #endregion
                                    #region COProposedTerm
                                    if (COProposedTerm == null || COProposedTerm == "")
                                    {
                                        LoanAppResSMS data = new LoanAppResSMS();
                                        data.SMS = "LoanApp - COProposedTerm is required";
                                        SMSList.Add(data);
                                    }
                                    else
                                    {
                                        try
                                        {
                                            int x = Convert.ToInt32(COProposedTerm);
                                        }
                                        catch
                                        {
                                            LoanAppResSMS data = new LoanAppResSMS();
                                            data.SMS = "LoanApp - COProposedTerm is required in INT";
                                            SMSList.Add(data);
                                        }
                                    }
                                    #endregion
                                    #region COProposeRate
                                    if (COProposeRate == null || COProposeRate == "")
                                    {
                                        LoanAppResSMS data = new LoanAppResSMS();
                                        data.SMS = "LoanApp - COProposeRate is required";
                                        SMSList.Add(data);
                                    }
                                    else
                                    {
                                        try
                                        {
                                            double x = Convert.ToDouble(COProposeRate);
                                        }
                                        catch
                                        {
                                            LoanAppResSMS data = new LoanAppResSMS();
                                            data.SMS = "LoanApp - COProposeRate is required in Money";
                                            SMSList.Add(data);
                                        }
                                    }
                                    #endregion
                                    #region FrontBackOfficeID
                                    if (FrontBackOfficeID == null || FrontBackOfficeID == "")
                                    {
                                        LoanAppResSMS data = new LoanAppResSMS();
                                        data.SMS = "LoanApp - FrontBackOfficeID is required";
                                        SMSList.Add(data);
                                    }
                                    else
                                    {
                                        try
                                        {
                                            int x = Convert.ToInt32(FrontBackOfficeID);
                                        }
                                        catch
                                        {
                                            LoanAppResSMS data = new LoanAppResSMS();
                                            data.SMS = "LoanApp - FrontBackOfficeID is required in INT";
                                            SMSList.Add(data);
                                        }
                                    }
                                    #endregion
                                    #region GroupNumber
                                    //if (GroupNumber == null || GroupNumber =="")
                                    //{
                                    //    LoanAppResSMS data = new LoanAppResSMS();
                                    //    data.SMS = "LoanApp - GroupNumber is required";
                                    //    SMSList.Add(data);
                                    //}
                                    #endregion
                                    #region LoanCycleID
                                    if (LoanCycleID == null || LoanCycleID == "")
                                    {
                                        LoanAppResSMS data = new LoanAppResSMS();
                                        data.SMS = "LoanApp - LoanCycleID is required";
                                        SMSList.Add(data);
                                    }
                                    #endregion
                                    #region RepaymentHistoryID
                                    if (RepaymentHistoryID == null || RepaymentHistoryID == "")
                                    {
                                        LoanAppResSMS data = new LoanAppResSMS();
                                        data.SMS = "LoanApp - RepaymentHistoryID is required";
                                        SMSList.Add(data);
                                    }
                                    else
                                    {
                                        try
                                        {
                                            int x = Convert.ToInt32(RepaymentHistoryID);
                                        }
                                        catch
                                        {
                                            LoanAppResSMS data = new LoanAppResSMS();
                                            data.SMS = "LoanApp - RepaymentHistoryID is required in INT";
                                            SMSList.Add(data);
                                        }
                                    }
                                    #endregion
                                    #region LoanReferralID
                                    //if (LoanReferralID == null || LoanReferralID =="")
                                    //{
                                    //    LoanAppResSMS data = new LoanAppResSMS();
                                    //    data.SMS = "LoanApp - LoanReferralID is required";
                                    //    SMSList.Add(data);
                                    //}
                                    #endregion
                                    #region MonthlyFee
                                    if (MonthlyFee == null || MonthlyFee == "")
                                    {
                                        LoanAppResSMS data = new LoanAppResSMS();
                                        data.SMS = "LoanApp - MonthlyFee is required";
                                        SMSList.Add(data);
                                    }
                                    else
                                    {
                                        try
                                        {
                                            double x = Convert.ToDouble(MonthlyFee);
                                        }
                                        catch
                                        {
                                            LoanAppResSMS data = new LoanAppResSMS();
                                            data.SMS = "LoanApp - MonthlyFee is required in Money";
                                            SMSList.Add(data);
                                        }
                                    }
                                    #endregion
                                    #region Compulsory
                                    if (Compulsory == null || Compulsory == "")
                                    {
                                        LoanAppResSMS data = new LoanAppResSMS();
                                        data.SMS = "LoanApp - Compulsory is required";
                                        SMSList.Add(data);
                                    }
                                    else
                                    {
                                        try
                                        {
                                            double x = Convert.ToDouble(Compulsory);
                                        }
                                        catch
                                        {
                                            LoanAppResSMS data = new LoanAppResSMS();
                                            data.SMS = "LoanApp - Compulsory is required in Money";
                                            SMSList.Add(data);
                                        }
                                    }
                                    #endregion
                                    #region CompulsoryTerm
                                    if (CompulsoryTerm == null || CompulsoryTerm == "")
                                    {
                                        LoanAppResSMS data = new LoanAppResSMS();
                                        data.SMS = "LoanApp - CompulsoryTerm is required";
                                        SMSList.Add(data);
                                    }
                                    else
                                    {
                                        try
                                        {
                                            int x = Convert.ToInt32(CompulsoryTerm);
                                        }
                                        catch
                                        {
                                            LoanAppResSMS data = new LoanAppResSMS();
                                            data.SMS = "LoanApp - CompulsoryTerm is required in INT";
                                            SMSList.Add(data);
                                        }
                                    }
                                    #endregion
                                    #region Currency
                                    if (Currency == null || Currency == "")
                                    {
                                        LoanAppResSMS data = new LoanAppResSMS();
                                        data.SMS = "LoanApp - Currency is required";
                                        SMSList.Add(data);
                                    }
                                    #endregion
                                    #region UpFrontFee
                                    if (UpFrontFee == null || UpFrontFee == "")
                                    {
                                        LoanAppResSMS data = new LoanAppResSMS();
                                        data.SMS = "LoanApp - UpFrontFee is required";
                                        SMSList.Add(data);
                                    }
                                    else
                                    {
                                        try
                                        {
                                            double x = Convert.ToDouble(UpFrontFee);
                                        }
                                        catch
                                        {
                                            LoanAppResSMS data = new LoanAppResSMS();
                                            data.SMS = "LoanApp - UpFrontFee is required in Money";
                                            SMSList.Add(data);
                                        }
                                    }
                                    #endregion
                                    #region CompulsoryOptionID
                                    if (CompulsoryOptionID == null || CompulsoryOptionID == "")
                                    {
                                        LoanAppResSMS data = new LoanAppResSMS();
                                        data.SMS = "LoanApp - CompulsoryOptionID is required";
                                        SMSList.Add(data);
                                    }
                                    else
                                    {
                                        try
                                        {
                                            int x = Convert.ToInt32(CompulsoryOptionID);
                                        }
                                        catch
                                        {
                                            LoanAppResSMS data = new LoanAppResSMS();
                                            data.SMS = "LoanApp - CompulsoryOptionID is required in INT";
                                            SMSList.Add(data);
                                        }
                                    }
                                    #endregion
                                    #region FundSource
                                    if (FundSource == null || FundSource == "")
                                    {
                                        LoanAppResSMS data = new LoanAppResSMS();
                                        data.SMS = "LoanApp - FundSource is required";
                                        SMSList.Add(data);
                                    }
                                    else
                                    {
                                        try
                                        {
                                            int x = Convert.ToInt32(FundSource);
                                        }
                                        catch
                                        {
                                            LoanAppResSMS data = new LoanAppResSMS();
                                            data.SMS = "LoanApp - FundSource is required in INT";
                                            SMSList.Add(data);
                                        }
                                    }
                                    #endregion
                                    #region IsNewCollateral
                                    if (IsNewCollateral == null || IsNewCollateral == "")
                                    {
                                        LoanAppResSMS data = new LoanAppResSMS();
                                        data.SMS = "LoanApp - IsNewCollateral is required";
                                        SMSList.Add(data);
                                    }
                                    else
                                    {
                                        try
                                        {
                                            int x = Convert.ToInt32(IsNewCollateral);
                                        }
                                        catch
                                        {
                                            LoanAppResSMS data = new LoanAppResSMS();
                                            data.SMS = "LoanApp - IsNewCollateral is required in INT";
                                            SMSList.Add(data);
                                        }
                                    }
                                    #endregion
                                    #region AgriBuddy
                                    if (AgriBuddy == null || AgriBuddy == "")
                                    {
                                        LoanAppResSMS data = new LoanAppResSMS();
                                        data.SMS = "LoanApp - AgriBuddy is required";
                                        SMSList.Add(data);
                                    }
                                    #endregion
                                    #region semiBallonFreqID
                                    if (semiBallonFreqID == null || semiBallonFreqID == "")
                                    {
                                        LoanAppResSMS data = new LoanAppResSMS();
                                        data.SMS = "LoanApp - semiBallonFreqID is required";
                                        SMSList.Add(data);
                                    }
                                    #endregion
                                    #region LoanTypeID
                                    if (LoanTypeID == null || LoanTypeID == "")
                                    {
                                        LoanAppResSMS data = new LoanAppResSMS();
                                        data.SMS = "LoanApp - LoanTypeID is required";
                                        SMSList.Add(data);
                                    }
                                    else
                                    {
                                        try
                                        {
                                            int x = Convert.ToInt32(LoanTypeID);
                                        }
                                        catch
                                        {
                                            LoanAppResSMS data = new LoanAppResSMS();
                                            data.SMS = "LoanApp - LoanTypeID is required in INT";
                                            SMSList.Add(data);
                                        }
                                    }
                                    #endregion
                                    #region PaymentMethodID
                                    if (PaymentMethodID == null || PaymentMethodID == "")
                                    {
                                        LoanAppResSMS data = new LoanAppResSMS();
                                        data.SMS = "LoanApp - PaymentMethodID is required";
                                        SMSList.Add(data);
                                    }
                                    #endregion
                                    #region GracePeriodID
                                    if (GracePeriodID == null || GracePeriodID == "")
                                    {
                                        LoanAppResSMS data = new LoanAppResSMS();
                                        data.SMS = "LoanApp - GracePeriodID is required";
                                        SMSList.Add(data);
                                    }
                                    #endregion
                                    #region MITypeID
                                    if (MITypeID == null || MITypeID == "")
                                    {
                                        LoanAppResSMS data = new LoanAppResSMS();
                                        data.SMS = "LoanApp - MITypeID is required";
                                        SMSList.Add(data);
                                    }
                                    #endregion

                                    RSIDOnDevice = LoanClientID;
                                    RSLoanAppID = ServerLoanAppID;
                                }
                                catch (Exception ex)
                                {
                                    ERR = "Error";
                                    SMS = "Cannot add loan to switch DB";
                                    ExSMS = ex.Message.ToString();
                                }
                                #endregion sql
                            }
                            catch (Exception ex)
                            {
                                ERR = "Error";
                                SMS = "Cannot read loan parameter " + ex.Message.ToString();
                                ExSMS = ex.Message.ToString();
                            }

                            //if (PersonType == "31")
                            //{
                                if (ERR != "Error")
                                {
                                    string ServerLoanAppPersonID = "0";
                                    #region Person
                                    if (loan.Person != null)
                                    {
                                        foreach (var Person in loan.Person)
                                        {
                                            if (ERR != "Error")
                                            {
                                                try
                                                {
                                                    #region Person Para
                                                    string LoanAppPersonTypeID = Person.LoanAppPersonTypeID;
                                                    string CustomerID = Person.T24CustID;
                                                    string VillageBankID = Person.VBID;
                                                    string NameKhLast = Person.NameKhLast;
                                                    string TitleID = Person.TitleID;
                                                    string LastName = Person.NameEnLast;
                                                    string FirstName = Person.NameEnFirst;
                                                    string GenderID = Person.GenderID;
                                                    string DateOfBirth = Person.DateOfBirth;
                                                    string IDCardTypeID = Person.IDCardTypeID;
                                                    string IDCardNumber = Person.IDCardNumber;
                                                    string IDCardExpireDate = Person.IDCardExpiryDate;
                                                    string MaritalStatusID = Person.MaritalStatusID;
                                                    string EducationID = Person.EducationLevelID;
                                                    string CityOfBirthID = Person.PlaceOfBirth;
                                                    string Telephone3 = Person.Phone;
                                                    string VillageIDPermanent = Person.VillageIDPer;
                                                    string VillageIDCurrent = Person.VillageIDCur;
                                                    string SortAddress = Person.ShortAddress;
                                                    string FamilyMember = Person.FamilyMember;
                                                    string FamilyMemberActive = Person.FamilyMemberActive;
                                                    string PoorID = Person.PoorLevelID;
                                                    string LoanAppPersonIDOnDevice = Person.CustClientID;
                                                    string CustDeviceDate = Person.CreateDateClient;
                                                    string IDCardIssuedDate = Person.IDCardIssueDate;
                                                    string NameKhFirst = Person.NameKhFirst;
                                                    string ProspectCode = Person.ProspectCode;
                                                    string ReferByID = Person.ReferByID;
                                                    string ReferName = Person.ReferName;
                                                    string LatLon = Person.LatLon;
                                                    string CustServerID = Person.CustServerID;
                                                    #endregion Person Para
                                                    #region sql
                                                    try
                                                    {
                                                        #region LoanAppPersonTypeID
                                                        if (LoanAppPersonTypeID == null || LoanAppPersonTypeID == "")
                                                        {
                                                            LoanAppResSMS data = new LoanAppResSMS();
                                                            data.SMS = "Person - LoanAppPersonTypeID is required";
                                                            SMSList.Add(data);
                                                        }
                                                        else
                                                        {
                                                            try
                                                            {
                                                                int x = Convert.ToInt32(LoanAppPersonTypeID);
                                                            }
                                                            catch
                                                            {
                                                                LoanAppResSMS data = new LoanAppResSMS();
                                                                data.SMS = "Person - LoanAppPersonTypeID is required in INT";
                                                                SMSList.Add(data);
                                                            }
                                                        }
                                                        #endregion
                                                        #region NameKhLast
                                                        if (NameKhLast == null || NameKhLast == "")
                                                        {
                                                            LoanAppResSMS data = new LoanAppResSMS();
                                                            data.SMS = "Person - NameKhLast is required";
                                                            SMSList.Add(data);
                                                        }
                                                        #endregion
                                                        #region TitleID
                                                        if (TitleID == null || TitleID == "")
                                                        {
                                                            LoanAppResSMS data = new LoanAppResSMS();
                                                            data.SMS = "Person - TitleID is required";
                                                            SMSList.Add(data);
                                                        }
                                                        #endregion
                                                        #region LastName
                                                        if (LastName == null || LastName == "")
                                                        {
                                                            LoanAppResSMS data = new LoanAppResSMS();
                                                            data.SMS = "Person - NameEnLast is required";
                                                            SMSList.Add(data);
                                                        }
                                                        #endregion
                                                        #region FirstName
                                                        if (FirstName == null || FirstName == "")
                                                        {
                                                            LoanAppResSMS data = new LoanAppResSMS();
                                                            data.SMS = "Person - NameEnFirst is required";
                                                            SMSList.Add(data);
                                                        }
                                                        #endregion
                                                        #region GenderID
                                                        if (GenderID == null || GenderID == "")
                                                        {
                                                            LoanAppResSMS data = new LoanAppResSMS();
                                                            data.SMS = "Person - GenderID is required";
                                                            SMSList.Add(data);
                                                        }
                                                        #endregion
                                                        #region DateOfBirth
                                                        if (DateOfBirth == null || DateOfBirth == "")
                                                        {
                                                            LoanAppResSMS data = new LoanAppResSMS();
                                                            data.SMS = "Person - DateOfBirth is required";
                                                            SMSList.Add(data);
                                                        }
                                                        else
                                                        {
                                                            try
                                                            {
                                                                DateTime x = Convert.ToDateTime(DateOfBirth);
                                                            }
                                                            catch
                                                            {
                                                                LoanAppResSMS data = new LoanAppResSMS();
                                                                data.SMS = "Person - DateOfBirth is required in Date";
                                                                SMSList.Add(data);
                                                            }
                                                        }
                                                        #endregion
                                                        #region IDCardTypeID
                                                        if (IDCardTypeID == null || IDCardTypeID == "")
                                                        {
                                                            LoanAppResSMS data = new LoanAppResSMS();
                                                            data.SMS = "Person - IDCardTypeID is required";
                                                            SMSList.Add(data);
                                                        }
                                                        else
                                                        {
                                                            try
                                                            {
                                                                int x = Convert.ToInt32(IDCardTypeID);
                                                            }
                                                            catch
                                                            {
                                                                LoanAppResSMS data = new LoanAppResSMS();
                                                                data.SMS = "Person - IDCardTypeID is required in INT";
                                                                SMSList.Add(data);
                                                            }
                                                        }
                                                        #endregion
                                                        #region MaritalStatusID
                                                        if (MaritalStatusID == null || MaritalStatusID == "")
                                                        {
                                                            LoanAppResSMS data = new LoanAppResSMS();
                                                            data.SMS = "Person - MaritalStatusID is required";
                                                            SMSList.Add(data);
                                                        }
                                                        #endregion
                                                        #region EducationID
                                                        if (EducationID == null || EducationID == "")
                                                        {
                                                            LoanAppResSMS data = new LoanAppResSMS();
                                                            data.SMS = "Person - EducationLevelID is required";
                                                            SMSList.Add(data);
                                                        }
                                                        #endregion
                                                        #region CityOfBirthID
                                                        if (CityOfBirthID == null || CityOfBirthID == "")
                                                        {
                                                            LoanAppResSMS data = new LoanAppResSMS();
                                                            data.SMS = "Person - PlaceOfBirth is required";
                                                            SMSList.Add(data);
                                                        }
                                                        #endregion
                                                        #region Telephone3
                                                        if (Telephone3 == null || Telephone3 == "")
                                                        {
                                                            LoanAppResSMS data = new LoanAppResSMS();
                                                            data.SMS = "Person - Phone is required";
                                                            SMSList.Add(data);
                                                        }
                                                        #endregion
                                                        #region VillageIDPermanent
                                                        if (VillageIDPermanent == null || VillageIDPermanent == "")
                                                        {
                                                            LoanAppResSMS data = new LoanAppResSMS();
                                                            data.SMS = "Person - VillageIDPer is required";
                                                            SMSList.Add(data);
                                                        }
                                                        #endregion
                                                        #region VillageIDCurrent
                                                        if (VillageIDCurrent == null || VillageIDCurrent == "")
                                                        {
                                                            LoanAppResSMS data = new LoanAppResSMS();
                                                            data.SMS = "Person - VillageIDCur is required";
                                                            SMSList.Add(data);
                                                        }
                                                        #endregion
                                                        #region FamilyMember
                                                        if (FamilyMember == null || FamilyMember == "")
                                                        {
                                                            LoanAppResSMS data = new LoanAppResSMS();
                                                            data.SMS = "Person - FamilyMember is required";
                                                            SMSList.Add(data);
                                                        }
                                                        #endregion
                                                        #region FamilyMemberActive
                                                        if (FamilyMemberActive == null || FamilyMemberActive == "")
                                                        {
                                                            LoanAppResSMS data = new LoanAppResSMS();
                                                            data.SMS = "Person - FamilyMemberActive is required";
                                                            SMSList.Add(data);
                                                        }
                                                        else
                                                        {
                                                            try
                                                            {
                                                                int x = Convert.ToInt32(FamilyMemberActive);
                                                            }
                                                            catch
                                                            {
                                                                LoanAppResSMS data = new LoanAppResSMS();
                                                                data.SMS = "Person - FamilyMemberActive is required in INT";
                                                                SMSList.Add(data);
                                                            }
                                                        }
                                                        #endregion
                                                        #region PoorID
                                                        if (PoorID == null || PoorID == "")
                                                        {
                                                            LoanAppResSMS data = new LoanAppResSMS();
                                                            data.SMS = "Person - PoorLevelID is required";
                                                            SMSList.Add(data);
                                                        }
                                                        #endregion
                                                        #region LoanAppPersonIDOnDevice
                                                        if (LoanAppPersonIDOnDevice == null || LoanAppPersonIDOnDevice == "")
                                                        {
                                                            LoanAppResSMS data = new LoanAppResSMS();
                                                            data.SMS = "Person - CustClientID is required";
                                                            SMSList.Add(data);
                                                        }
                                                        else
                                                        {
                                                            try
                                                            {
                                                                int x = Convert.ToInt32(LoanAppPersonIDOnDevice);
                                                            }
                                                            catch
                                                            {
                                                                LoanAppResSMS data = new LoanAppResSMS();
                                                                data.SMS = "Person - CustClientID is required in INT";
                                                                SMSList.Add(data);
                                                            }
                                                        }
                                                        #endregion
                                                        #region CustDeviceDate
                                                        if (CustDeviceDate == null || CustDeviceDate == "")
                                                        {
                                                            LoanAppResSMS data = new LoanAppResSMS();
                                                            data.SMS = "Person - CreateDateClient is required";
                                                            SMSList.Add(data);
                                                        }
                                                        else
                                                        {
                                                            try
                                                            {
                                                                DateTime x = Convert.ToDateTime(CustDeviceDate);
                                                            }
                                                            catch
                                                            {
                                                                LoanAppResSMS data = new LoanAppResSMS();
                                                                data.SMS = "Person - CreateDateClient is required in DateTime";
                                                                SMSList.Add(data);
                                                            }
                                                        }
                                                        #endregion
                                                        #region NameKhFirst
                                                        if (NameKhFirst == null || NameKhFirst == "")
                                                        {
                                                            LoanAppResSMS data = new LoanAppResSMS();
                                                            data.SMS = "Person - NameKhFirst is required";
                                                            SMSList.Add(data);
                                                        }
                                                        #endregion
                                                        #region ReferByID
                                                        if (ReferByID == null || ReferByID == "")
                                                        {
                                                            LoanAppResSMS data = new LoanAppResSMS();
                                                            data.SMS = "Person - ReferByID is required";
                                                            SMSList.Add(data);
                                                        }
                                                        else
                                                        {
                                                            if (ReferByID != "0")
                                                            {
                                                                if (ReferName == null || ReferName == "")
                                                                {
                                                                    LoanAppResSMS data = new LoanAppResSMS();
                                                                    data.SMS = "Person - ReferName is required";
                                                                    SMSList.Add(data);
                                                                }
                                                            }
                                                        }
                                                        #endregion
                                                        #region CustServerID
                                                        if (CustServerID == null || CustServerID == "")
                                                        {
                                                            LoanAppResSMS data = new LoanAppResSMS();
                                                            data.SMS = "Person - CustServerID is required";
                                                            SMSList.Add(data);
                                                        }
                                                        else
                                                        {
                                                            try
                                                            {
                                                                int x = Convert.ToInt32(CustServerID);
                                                            }
                                                            catch
                                                            {
                                                                LoanAppResSMS data = new LoanAppResSMS();
                                                                data.SMS = "Person - CustServerID is required in INT";
                                                                SMSList.Add(data);
                                                            }
                                                        }
                                                        #endregion
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        ERR = "Error";
                                                        SMS = "Cannot add customer to switch DB";
                                                        ExSMS = ex.Message.ToString();
                                                    }
                                                    #endregion sql

                                                    try
                                                    {
                                                        #region PersonImg

                                                        if (Person.PersonImg != null)
                                                        {
                                                            //foreach (var PersonImg in Person.PersonImg)
                                                            //{
                                                            //    if (ERR != "Error")
                                                            //    {
                                                            //        #region Para
                                                            //        string CustImageClientID = PersonImg.CustImageClientID;
                                                            //        string perImg_custImageServerID = PersonImg.CustImageServerID;
                                                            //        string perImg_custClientID = PersonImg.CustClientID;
                                                            //        string perImg_createDateClient = PersonImg.CreateDateClient;
                                                            //        string OneCardTwoDoc = PersonImg.OneCardTwoDoc;
                                                            //        string perImg_ext = PersonImg.Ext;
                                                            //        string perImg_imgPath = PersonImg.ImgPath;//File Name
                                                            //        string perImg_remark = PersonImg.Remark;

                                                            //        string perImg_fName = "PersonImg_" + ServerLoanAppID + "_" + ServerLoanAppPersonID + "_" + OneCardTwoDoc + "_" + CustImageClientID + "_" + ServerDateForFileName + "." + perImg_ext.Replace(".", "");
                                                            //        #endregion Para
                                                            //        #region sql
                                                            //        try
                                                            //        {
                                                            //            if (CustImageClientID == null || CustImageClientID == "")
                                                            //            {
                                                            //                LoanAppResSMS data = new LoanAppResSMS();
                                                            //                data.SMS = "PersonImg - CustImageClientID is required";
                                                            //                SMSList.Add(data);
                                                            //            }
                                                            //            //if (perImg_custImageServerID == null || perImg_custImageServerID == "")
                                                            //            //{
                                                            //            //    LoanAppResSMS data = new LoanAppResSMS();
                                                            //            //    data.SMS = "PersonImg - CustImageServerID is required";
                                                            //            //    SMSList.Add(data);
                                                            //            //}
                                                            //            if (perImg_custClientID == null || perImg_custClientID == "")
                                                            //            {
                                                            //                LoanAppResSMS data = new LoanAppResSMS();
                                                            //                data.SMS = "PersonImg - CustClientID is required";
                                                            //                SMSList.Add(data);
                                                            //            }
                                                            //            if (perImg_createDateClient == null || perImg_createDateClient == "")
                                                            //            {
                                                            //                LoanAppResSMS data = new LoanAppResSMS();
                                                            //                data.SMS = "PersonImg - CreateDateClient is required";
                                                            //                SMSList.Add(data);
                                                            //            }
                                                            //            if (OneCardTwoDoc == null || OneCardTwoDoc == "")
                                                            //            {
                                                            //                LoanAppResSMS data = new LoanAppResSMS();
                                                            //                data.SMS = "PersonImg - OneCardTwoDoc is required";
                                                            //                SMSList.Add(data);
                                                            //            }
                                                            //            if (perImg_ext == null || perImg_ext == "")
                                                            //            {
                                                            //                LoanAppResSMS data = new LoanAppResSMS();
                                                            //                data.SMS = "PersonImg - Ext is required";
                                                            //                SMSList.Add(data);
                                                            //            }
                                                            //            if (perImg_imgPath == null || perImg_imgPath == "")
                                                            //            {
                                                            //                LoanAppResSMS data = new LoanAppResSMS();
                                                            //                data.SMS = "PersonImg - ImgPath is required";
                                                            //                SMSList.Add(data);
                                                            //            }
                                                            //        }
                                                            //        catch (Exception ex)
                                                            //        {
                                                            //            ERR = "Error";
                                                            //            SMS = "Cannot add customer image to switch DB";
                                                            //            ExSMS = ex.Message.ToString();
                                                            //        }
                                                            //        #endregion sql
                                                            //    }
                                                            //}
                                                        }

                                                        #endregion PersonImg 
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        ERR = "Error";
                                                        SMS = "Error PersonImg";
                                                        ExSMS = ex.Message.ToString();
                                                    }

                                                    try
                                                    {
                                                        #region Asset
                                                        if (Person.Asset != null)
                                                        {
                                                            foreach (var Asset in Person.Asset)
                                                            {
                                                                if (ERR != "Error")
                                                                {
                                                                    string ServerLoanAppClientAssetID = "0";
                                                                    #region Para
                                                                    string asset_assetClientID = Asset.AssetClientID;
                                                                    string asset_assetServerID = Asset.AssetServerID;
                                                                    string asset_loanPurposeClientID = Asset.LoanPurposeClientID;
                                                                    string asset_loanClientID = Asset.LoanClientID;
                                                                    string asset_loanAppID = Asset.LoanAppID;
                                                                    string asset_description = Asset.Description;
                                                                    string asset_quantity = Asset.Quantity;
                                                                    string asset_unitPrice = Asset.UnitPrice;
                                                                    string asset_custClientID = Asset.CustClientID;
                                                                    string asset_custServerID = Asset.CustServerID;
                                                                    #endregion Para
                                                                    #region sql
                                                                    try
                                                                    {
                                                                        if (asset_description == null || asset_description == "")
                                                                        {
                                                                            LoanAppResSMS data = new LoanAppResSMS();
                                                                            data.SMS = "Asset - Description is required";
                                                                            SMSList.Add(data);
                                                                        }
                                                                        if (asset_quantity == null || asset_quantity == "")
                                                                        {
                                                                            LoanAppResSMS data = new LoanAppResSMS();
                                                                            data.SMS = "Asset - Quantity is required";
                                                                            SMSList.Add(data);
                                                                        }
                                                                        if (asset_unitPrice == null || asset_unitPrice == "")
                                                                        {
                                                                            LoanAppResSMS data = new LoanAppResSMS();
                                                                            data.SMS = "Asset - UnitPrice is required";
                                                                            SMSList.Add(data);
                                                                        }
                                                                        if (asset_custClientID == null || asset_custClientID == "")
                                                                        {
                                                                            LoanAppResSMS data = new LoanAppResSMS();
                                                                            data.SMS = "Asset - CustClientID is required";
                                                                            SMSList.Add(data);
                                                                        }
                                                                    }
                                                                    catch (Exception ex)
                                                                    {
                                                                        ERR = "Error";
                                                                        SMS = "Cannot add asset to switch DB";
                                                                        ExSMS = ex.Message.ToString();
                                                                    }
                                                                    #endregion sql
                                                                    foreach (var AssetImg in Asset.AssetImg)
                                                                    {
                                                                        if (ERR != "Error")
                                                                        {
                                                                            try
                                                                            {
                                                                                #region Para
                                                                                string AssetImageClientID = AssetImg.AssetImageClientID;
                                                                                string AssetImageServerID = AssetImg.AssetImageServerID;
                                                                                string AssetClientID = AssetImg.AssetClientID;
                                                                                string AssetServerID = AssetImg.AssetServerID;
                                                                                string CreateDateClient = AssetImg.CreateDateClient;
                                                                                string AssetImg_Ext = AssetImg.Ext;
                                                                                string AssetImg_ImgPath = AssetImg.ImgPath;//file name
                                                                                string Remark = AssetImg.Remark;

                                                                                string Asset_fName = "AssetImg_" + ServerLoanAppID + "_" + ServerLoanAppClientAssetID + "_" + AssetImageClientID + "_" + ServerDateForFileName + "." + AssetImg_Ext.Replace(".", "");
                                                                                #endregion Para
                                                                                #region sql
                                                                                try
                                                                                {
                                                                                    if (AssetImageClientID == null || AssetImageClientID == "")
                                                                                    {
                                                                                        LoanAppResSMS data = new LoanAppResSMS();
                                                                                        data.SMS = "AssetImg - AssetImageClientID is required";
                                                                                        SMSList.Add(data);
                                                                                    }
                                                                                    if (CreateDateClient == null || CreateDateClient == "")
                                                                                    {
                                                                                        LoanAppResSMS data = new LoanAppResSMS();
                                                                                        data.SMS = "AssetImg - CreateDateClient is required";
                                                                                        SMSList.Add(data);
                                                                                    }
                                                                                    if (AssetImg_Ext == null || AssetImg_Ext == "")
                                                                                    {
                                                                                        LoanAppResSMS data = new LoanAppResSMS();
                                                                                        data.SMS = "AssetImg - Ext is required";
                                                                                        SMSList.Add(data);
                                                                                    }
                                                                                    if (Asset_fName == null || Asset_fName == "")
                                                                                    {
                                                                                        LoanAppResSMS data = new LoanAppResSMS();
                                                                                        data.SMS = "AssetImg - ImgPath is required";
                                                                                        SMSList.Add(data);
                                                                                    }
                                                                                }
                                                                                catch (Exception ex)
                                                                                {
                                                                                    ERR = "Error";
                                                                                    SMS = "Cannot add asset image to switch DB";
                                                                                    ExSMS = ex.Message.ToString();
                                                                                }
                                                                                #endregion sql
                                                                            }
                                                                            catch (Exception ex)
                                                                            {
                                                                                ERR = "Error";
                                                                                SMS = "Cannot add asset image to switch DB";
                                                                                ExSMS = ex.Message.ToString();
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }

                                                        #endregion Person
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        //ERR = "Error";
                                                        //SMS = "Erro Person";
                                                        //ExSMS = ex.Message.ToString();
                                                    }

                                                    try
                                                    {
                                                        #region Creditor
                                                        if (ERR != "Error")
                                                        {
                                                        
                                                            if (Person.Creditor != null)
                                                            {
                                                                foreach (var Creditor in Person.Creditor)
                                                                {
                                                                    try
                                                                    {
                                                                        if (ERR != "Error")
                                                                        {
                                                                            #region Para
                                                                            string CreditorClientID = Creditor.CreditorClientID;
                                                                            string creditor_creditorServerID = Creditor.CreditorServerID;
                                                                            string creditor_loanClientID = Creditor.LoanClientID;
                                                                            string creditor_loanAppID = Creditor.LoanAppID;
                                                                            string CreditorID = Creditor.CreditorID;
                                                                            string creditor_currency = Creditor.Currency;
                                                                            string creditor_approvedAmount = Creditor.ApprovedAmount;
                                                                            string creditor_outstandingBalance = Creditor.OutstandingBalance;
                                                                            string creditor_interestRate = Creditor.InterestRate;
                                                                            string creditor_repaymentTypeID = Creditor.RepaymentTypeID;
                                                                            string creditor_repaymentTermID = Creditor.RepaymentTermID;
                                                                            string creditor_loanStartDate = Creditor.LoanStartDate;
                                                                            string creditor_loanEndDate = Creditor.LoanEndDate;
                                                                            string creditor_isReFinance = Creditor.IsReFinance;
                                                                            string creditor_reFinanceAmount = Creditor.ReFinanceAmount;
                                                                            string creditor_custClientID = Creditor.CustClientID;
                                                                            string creditor_custServerID = Creditor.CustServerID;
                                                                            #endregion Para
                                                                            #region sql
                                                                            try
                                                                            {
                                                                                if (CreditorClientID == null || CreditorClientID == "")
                                                                                {
                                                                                    LoanAppResSMS data = new LoanAppResSMS();
                                                                                    data.SMS = "Creditor - CreditorClientID is required";
                                                                                    SMSList.Add(data);
                                                                                }
                                                                                if (CreditorID == null || CreditorID == "")
                                                                                {
                                                                                    LoanAppResSMS data = new LoanAppResSMS();
                                                                                    data.SMS = "Creditor - CreditorID is required";
                                                                                    SMSList.Add(data);
                                                                                }
                                                                                if (creditor_currency == null || creditor_currency == "")
                                                                                {
                                                                                    LoanAppResSMS data = new LoanAppResSMS();
                                                                                    data.SMS = "Creditor - Currency is required";
                                                                                    SMSList.Add(data);
                                                                                }
                                                                                if (creditor_approvedAmount == null || creditor_approvedAmount == "")
                                                                                {
                                                                                    LoanAppResSMS data = new LoanAppResSMS();
                                                                                    data.SMS = "Creditor - ApprovedAmount is required";
                                                                                    SMSList.Add(data);
                                                                                }
                                                                                if (creditor_outstandingBalance == null || creditor_outstandingBalance == "")
                                                                                {
                                                                                    LoanAppResSMS data = new LoanAppResSMS();
                                                                                    data.SMS = "Creditor - OutstandingBalance is required";
                                                                                    SMSList.Add(data);
                                                                                }
                                                                                if (creditor_interestRate == null || creditor_interestRate == "")
                                                                                {
                                                                                    LoanAppResSMS data = new LoanAppResSMS();
                                                                                    data.SMS = "Creditor - InterestRate is required";
                                                                                    SMSList.Add(data);
                                                                                }
                                                                                if (creditor_repaymentTypeID == null || creditor_repaymentTypeID == "")
                                                                                {
                                                                                    LoanAppResSMS data = new LoanAppResSMS();
                                                                                    data.SMS = "Creditor - RepaymentTypeID is required";
                                                                                    SMSList.Add(data);
                                                                                }
                                                                                if (creditor_repaymentTermID == null || creditor_repaymentTermID == "")
                                                                                {
                                                                                    LoanAppResSMS data = new LoanAppResSMS();
                                                                                    data.SMS = "Creditor - RepaymentTermID is required";
                                                                                    SMSList.Add(data);
                                                                                }
                                                                                if (creditor_loanStartDate == null || creditor_loanStartDate == "")
                                                                                {
                                                                                    LoanAppResSMS data = new LoanAppResSMS();
                                                                                    data.SMS = "Creditor - LoanStartDate is required";
                                                                                    SMSList.Add(data);
                                                                                }
                                                                                if (creditor_loanEndDate == null || creditor_loanEndDate == "")
                                                                                {
                                                                                    LoanAppResSMS data = new LoanAppResSMS();
                                                                                    data.SMS = "Creditor - LoanEndDate is required";
                                                                                    SMSList.Add(data);
                                                                                }
                                                                                if (creditor_isReFinance == null || creditor_isReFinance == "")
                                                                                {
                                                                                    LoanAppResSMS data = new LoanAppResSMS();
                                                                                    data.SMS = "Creditor - isReFinance is required";
                                                                                    SMSList.Add(data);
                                                                                }
                                                                                if (creditor_reFinanceAmount == null || creditor_reFinanceAmount == "")
                                                                                {
                                                                                    LoanAppResSMS data = new LoanAppResSMS();
                                                                                    data.SMS = "Creditor - ReFinanceAmount is required";
                                                                                    SMSList.Add(data);
                                                                                }

                                                                            }
                                                                            catch (Exception ex)
                                                                            {
                                                                                ERR = "Error";
                                                                                SMS = "Cannot add Creditor to switch DB";
                                                                                ExSMS = ex.Message.ToString();
                                                                            }
                                                                            #endregion sql
                                                                        }
                                                                    }
                                                                    catch (Exception ex)
                                                                    {
                                                                        ERR = "Error";
                                                                        SMS = "Cannot read Creditor";
                                                                        ExSMS = ex.Message.ToString();
                                                                    }

                                                                }
                                                            }
                                                        
                                                    }
                                                        #endregion Creditor
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        ERR = "Error";
                                                        SMS = "Cannot read Creditor";
                                                        ExSMS = ex.Message.ToString();
                                                    }

                                                    try
                                                    {
                                                        #region RealEstate
                                                        if (ERR != "Error")
                                                        {
                                                            if (Person.RealEstate != null)
                                                            {
                                                                foreach (var RealEstate in Person.RealEstate)
                                                                {
                                                                    if (ERR != "Error")
                                                                    {
                                                                        try
                                                                        {
                                                                            string CollateralServerID = "0";
                                                                            #region RealEstate
                                                                            string re_CollateralClientID = RealEstate.CollateralClientID;
                                                                            string re_CollateralServerID = RealEstate.CollateralServerID;
                                                                            string re_LoanClientID = RealEstate.LoanClientID;
                                                                            string re_LoanAppID = RealEstate.LoanAppID;
                                                                            string re_CustClientID = RealEstate.CustClientID;
                                                                            string re_CustServerID = RealEstate.CustServerID;
                                                                            string re_CollateralDocGroupTypeID = RealEstate.CollateralDocGroupTypeID;
                                                                            string re_CollateralDocHardTypeID = RealEstate.CollateralDocHardTypeID;
                                                                            string re_CollateralDocSoftTypeID = RealEstate.CollateralDocSoftTypeID;
                                                                            string re_CollateralOwnerTypeID = RealEstate.CollateralOwnerTypeID;
                                                                            string re_CollateralLocationVillageID = RealEstate.CollateralLocationVillageID;
                                                                            string re_CollateralRoadAccessibilityID = RealEstate.CollateralRoadAccessibilityID;
                                                                            string re_PropertyTypeID = RealEstate.PropertyTypeID;
                                                                            string re_LandTypeID = RealEstate.LandTypeID;
                                                                            string re_LandSize = RealEstate.LandSize;
                                                                            string re_LandMarketPrice = RealEstate.LandMarketPrice;
                                                                            string re_LandForcedSalePrice = RealEstate.LandForcedSalePrice;
                                                                            string re_BuildingTypeID = RealEstate.BuildingTypeID;
                                                                            string re_BuildingSize = RealEstate.BuildingSize;
                                                                            string re_BuildingMarketPrice = RealEstate.BuildingMarketPrice;
                                                                            string re_BuildingForcedSalesPrice = RealEstate.BuildingForcedSalesPrice;
                                                                            #endregion RealEstate
                                                                            #region sql
                                                                            try
                                                                            {
                                                                                #region General Validation
                                                                                if (re_PropertyTypeID == null || re_PropertyTypeID == "")
                                                                                {
                                                                                    LoanAppResSMS data = new LoanAppResSMS();
                                                                                    data.SMS = "RealEstate - PropertyTypeID is required";
                                                                                    SMSList.Add(data);
                                                                                }

                                                                                if (re_CollateralClientID == null || re_CollateralClientID == "")
                                                                                {
                                                                                    LoanAppResSMS data = new LoanAppResSMS();
                                                                                    data.SMS = "RealEstate - CollateralClientID is required";
                                                                                    SMSList.Add(data);
                                                                                }
                                                                                if (re_CollateralDocGroupTypeID == null || re_CollateralDocGroupTypeID == "")
                                                                                {
                                                                                    LoanAppResSMS data = new LoanAppResSMS();
                                                                                    data.SMS = "RealEstate - CollateralDocGroupTypeID is required";
                                                                                    SMSList.Add(data);
                                                                                }
                                                                                if (re_CollateralDocHardTypeID == null || re_CollateralDocHardTypeID == "")
                                                                                {
                                                                                    LoanAppResSMS data = new LoanAppResSMS();
                                                                                    data.SMS = "RealEstate - CollateralDocHardTypeID is required";
                                                                                    SMSList.Add(data);
                                                                                }
                                                                                if (re_CollateralDocSoftTypeID == null || re_CollateralDocSoftTypeID == "")
                                                                                {
                                                                                    LoanAppResSMS data = new LoanAppResSMS();
                                                                                    data.SMS = "RealEstate - CollateralDocSoftTypeID is required";
                                                                                    SMSList.Add(data);
                                                                                }
                                                                                if (re_CollateralOwnerTypeID == null || re_CollateralOwnerTypeID == "")
                                                                                {
                                                                                    LoanAppResSMS data = new LoanAppResSMS();
                                                                                    data.SMS = "RealEstate - CollateralOwnerTypeID is required";
                                                                                    SMSList.Add(data);
                                                                                }
                                                                                if (re_CollateralLocationVillageID == null || re_CollateralLocationVillageID == "")
                                                                                {
                                                                                    LoanAppResSMS data = new LoanAppResSMS();
                                                                                    data.SMS = "RealEstate - CollateralLocationVillageID is required";
                                                                                    SMSList.Add(data);
                                                                                }
                                                                                if (re_CollateralRoadAccessibilityID == null || re_CollateralRoadAccessibilityID == "")
                                                                                {
                                                                                    LoanAppResSMS data = new LoanAppResSMS();
                                                                                    data.SMS = "RealEstate - CollateralRoadAccessibilityID is required";
                                                                                    SMSList.Add(data);
                                                                                }
                                                                                #endregion

                                                                                if (re_PropertyTypeID == "1")
                                                                                {
                                                                                    if (re_LandTypeID == null || re_LandTypeID == "")
                                                                                    {
                                                                                        LoanAppResSMS data = new LoanAppResSMS();
                                                                                        data.SMS = "RealEstate - LandTypeID is required";
                                                                                        SMSList.Add(data);
                                                                                    }
                                                                                    if (re_LandSize == null || re_LandSize == "")
                                                                                    {
                                                                                        LoanAppResSMS data = new LoanAppResSMS();
                                                                                        data.SMS = "RealEstate - LandSize is required";
                                                                                        SMSList.Add(data);
                                                                                    }
                                                                                    if (re_LandMarketPrice == null || re_LandMarketPrice == "")
                                                                                    {
                                                                                        LoanAppResSMS data = new LoanAppResSMS();
                                                                                        data.SMS = "RealEstate - LandMarketPrice is required";
                                                                                        SMSList.Add(data);
                                                                                    }
                                                                                    if (re_LandForcedSalePrice == null || re_LandMarketPrice == "")
                                                                                    {
                                                                                        LoanAppResSMS data = new LoanAppResSMS();
                                                                                        data.SMS = "RealEstate - LandForcedSalePrice is required";
                                                                                        SMSList.Add(data);
                                                                                    }
                                                                                }
                                                                                else if (re_PropertyTypeID == "2")
                                                                                {
                                                                                    if (re_BuildingTypeID == null || re_BuildingTypeID == "")
                                                                                    {
                                                                                        LoanAppResSMS data = new LoanAppResSMS();
                                                                                        data.SMS = "RealEstate - BuildingTypeID is required";
                                                                                        SMSList.Add(data);
                                                                                    }
                                                                                    if (re_BuildingSize == null || re_BuildingSize == "")
                                                                                    {
                                                                                        LoanAppResSMS data = new LoanAppResSMS();
                                                                                        data.SMS = "RealEstate - BuildingSize is required";
                                                                                        SMSList.Add(data);
                                                                                    }
                                                                                    if (re_BuildingMarketPrice == null || re_BuildingMarketPrice == "")
                                                                                    {
                                                                                        LoanAppResSMS data = new LoanAppResSMS();
                                                                                        data.SMS = "RealEstate - BuildingMarketPrice is required";
                                                                                        SMSList.Add(data);
                                                                                    }
                                                                                    if (re_BuildingForcedSalesPrice == null || re_BuildingForcedSalesPrice == "")
                                                                                    {
                                                                                        LoanAppResSMS data = new LoanAppResSMS();
                                                                                        data.SMS = "RealEstate - BuildingForcedSalesPrice is required";
                                                                                        SMSList.Add(data);
                                                                                    }
                                                                                }
                                                                                else if (re_PropertyTypeID == "3")
                                                                                {
                                                                                    if (re_LandTypeID == null || re_LandTypeID == "")
                                                                                    {
                                                                                        LoanAppResSMS data = new LoanAppResSMS();
                                                                                        data.SMS = "RealEstate - LandTypeID is required";
                                                                                        SMSList.Add(data);
                                                                                    }
                                                                                    if (re_LandSize == null || re_LandSize == "")
                                                                                    {
                                                                                        LoanAppResSMS data = new LoanAppResSMS();
                                                                                        data.SMS = "RealEstate - LandSize is required";
                                                                                        SMSList.Add(data);
                                                                                    }
                                                                                    if (re_LandMarketPrice == null || re_LandMarketPrice == "")
                                                                                    {
                                                                                        LoanAppResSMS data = new LoanAppResSMS();
                                                                                        data.SMS = "RealEstate - LandMarketPrice is required";
                                                                                        SMSList.Add(data);
                                                                                    }
                                                                                    if (re_LandForcedSalePrice == null || re_LandMarketPrice == "")
                                                                                    {
                                                                                        LoanAppResSMS data = new LoanAppResSMS();
                                                                                        data.SMS = "RealEstate - LandForcedSalePrice is required";
                                                                                        SMSList.Add(data);
                                                                                    }

                                                                                    if (re_BuildingTypeID == null || re_BuildingTypeID == "")
                                                                                    {
                                                                                        LoanAppResSMS data = new LoanAppResSMS();
                                                                                        data.SMS = "RealEstate - BuildingTypeID is required";
                                                                                        SMSList.Add(data);
                                                                                    }
                                                                                    if (re_BuildingSize == null || re_BuildingSize == "")
                                                                                    {
                                                                                        LoanAppResSMS data = new LoanAppResSMS();
                                                                                        data.SMS = "RealEstate - BuildingSize is required";
                                                                                        SMSList.Add(data);
                                                                                    }
                                                                                    if (re_BuildingMarketPrice == null || re_BuildingMarketPrice == "")
                                                                                    {
                                                                                        LoanAppResSMS data = new LoanAppResSMS();
                                                                                        data.SMS = "RealEstate - BuildingMarketPrice is required";
                                                                                        SMSList.Add(data);
                                                                                    }
                                                                                    if (re_BuildingForcedSalesPrice == null || re_BuildingForcedSalesPrice == "")
                                                                                    {
                                                                                        LoanAppResSMS data = new LoanAppResSMS();
                                                                                        data.SMS = "RealEstate - BuildingForcedSalesPrice is required";
                                                                                        SMSList.Add(data);
                                                                                    }
                                                                                }


                                                                            }
                                                                            catch (Exception ex)
                                                                            {
                                                                                ERR = "Error";
                                                                                SMS = "Cannot read RealEstate";
                                                                                ExSMS = ex.Message.ToString();
                                                                            }
                                                                            #endregion sql
                                                                            #region img
                                                                            if (ERR != "Error")
                                                                            {
                                                                                try
                                                                                {
                                                                                    if (RealEstate.RealEstateImg != null)
                                                                                    {
                                                                                        foreach (var PREImgV2 in RealEstate.RealEstateImg)
                                                                                        {
                                                                                            if (ERR != "Error")
                                                                                            {
                                                                                                try
                                                                                                {
                                                                                                    #region Para
                                                                                                    string PREImgV2_imageClientID = PREImgV2.ImageClientID;
                                                                                                    string PREImgV2_imageServerID = PREImgV2.ImageServerID;
                                                                                                    string PREImgV2_collateralClientID = PREImgV2.CollateralClientID;
                                                                                                    string PREImgV2_collateralServerID = PREImgV2.CollateralServerID;
                                                                                                    string PREImgV2_createDateClient = PREImgV2.CreateDateClient;
                                                                                                    string PREImgV2_ext = PREImgV2.Ext;
                                                                                                    string PREImgV2_imgPath = PREImgV2.ImgPath;
                                                                                                    string PREImgV2_remark = PREImgV2.Remark;

                                                                                                    string RealEstate_fName = "RealEstateImg_" + ServerLoanAppID + "_" + CollateralServerID + "_" + PREImgV2_imageClientID + "_" + ServerDateForFileName + "." + PREImgV2_ext.Replace(".", "");
                                                                                                    #endregion Para
                                                                                                    #region sql
                                                                                                    try
                                                                                                    {
                                                                                                        if (PREImgV2_imageClientID == null || PREImgV2_imageClientID == "")
                                                                                                        {
                                                                                                            LoanAppResSMS data = new LoanAppResSMS();
                                                                                                            data.SMS = "RealEstateImg - ImageClientID is required";
                                                                                                            SMSList.Add(data);
                                                                                                        }
                                                                                                        if (PREImgV2_ext == null || PREImgV2_ext == "")
                                                                                                        {
                                                                                                            LoanAppResSMS data = new LoanAppResSMS();
                                                                                                            data.SMS = "RealEstateImg - ext is required";
                                                                                                            SMSList.Add(data);
                                                                                                        }
                                                                                                        if (RealEstate_fName == null || RealEstate_fName == "")
                                                                                                        {
                                                                                                            LoanAppResSMS data = new LoanAppResSMS();
                                                                                                            data.SMS = "RealEstateImg - fName is required";
                                                                                                            SMSList.Add(data);
                                                                                                        }

                                                                                                    }
                                                                                                    catch (Exception ex)
                                                                                                    {
                                                                                                        ERR = "Error";
                                                                                                        SMS = "Cannot read RealEstate image";
                                                                                                        ExSMS = ex.Message.ToString();
                                                                                                    }
                                                                                                    #endregion sql
                                                                                                }
                                                                                                catch (Exception ex)
                                                                                                {
                                                                                                    ERR = "Error";
                                                                                                    SMS = "Cannot read RealEstate image";
                                                                                                    ExSMS = ex.Message.ToString();
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }

                                                                                }
                                                                                catch (Exception ex)
                                                                                {
                                                                                    ERR = "Error";
                                                                                    SMS = "Cannot read RealEstate image";
                                                                                    ExSMS = ex.Message.ToString();
                                                                                }
                                                                            }
                                                                            #endregion img
                                                                        }
                                                                        catch (Exception ex)
                                                                        {
                                                                            ERR = "Error";
                                                                            SMS = "Cannot read RealEstate";
                                                                            ExSMS = ex.Message.ToString();
                                                                        }
                                                                    }
                                                                }
                                                            }

                                                        }
                                                        #endregion RealEstate
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        ERR = "Error";
                                                        SMS = "Cannot read RealEstate";
                                                        ExSMS = ex.Message.ToString();
                                                    }

                                                    try
                                                    {
                                                        #region PersonDeposit
                                                        if (ERR != "Error")
                                                        {
                                                            try
                                                            {
                                                                if (Person.PersonDeposit != null)
                                                                {
                                                                    foreach (var PersonDeposit in Person.PersonDeposit)
                                                                    {
                                                                        if (ERR != "Error")
                                                                        {
                                                                            try
                                                                            {
                                                                                #region Para
                                                                                string dep_CollateralClientID = PersonDeposit.CollateralClientID;
                                                                                string deposit_collateralServerID = PersonDeposit.CollateralServerID;
                                                                                string dep_LoanClientID = PersonDeposit.LoanClientID;
                                                                                string deposit_loanAppID = PersonDeposit.LoanAppID;
                                                                                string dep_CustClientID = PersonDeposit.CustClientID;
                                                                                string deposit_custServerID = PersonDeposit.CustServerID;
                                                                                string FixedDepositAccountNo = PersonDeposit.FixedDepositAccountNo;
                                                                                string dep_StartDate = PersonDeposit.StartDate;
                                                                                string dep_MaturityDate = PersonDeposit.MaturityDate;
                                                                                string dep_Amount = PersonDeposit.Amount;
                                                                                string dep_AccountOwnerName = PersonDeposit.AccountOwnerName;
                                                                                string dep_Currency = PersonDeposit.Currency;
                                                                                string dep_RelationshipID = PersonDeposit.RelationshipID;
                                                                                string dep_DOB = PersonDeposit.DOB;
                                                                                string dep_GenderID = PersonDeposit.GenderID;
                                                                                string dep_NIDNo = PersonDeposit.NIDNo;
                                                                                string dep_IssueDate = PersonDeposit.IssueDate;
                                                                                string dep_IssuedBy = PersonDeposit.IssuedBy;
                                                                                string dep_SortAddress = PersonDeposit.SortAddress;
                                                                                string dep_VillageID = PersonDeposit.VillageID;
                                                                                #endregion Para
                                                                                #region sql
                                                                                try
                                                                                {
                                                                                    if (dep_CollateralClientID == null || dep_CollateralClientID == "")
                                                                                    {
                                                                                        LoanAppResSMS data = new LoanAppResSMS();
                                                                                        data.SMS = "PersonDeposit - CollateralClientID is required";
                                                                                        SMSList.Add(data);
                                                                                    }
                                                                                    if (FixedDepositAccountNo == null || FixedDepositAccountNo == "")
                                                                                    {
                                                                                        LoanAppResSMS data = new LoanAppResSMS();
                                                                                        data.SMS = "PersonDeposit - FixedDepositAccountNo is required";
                                                                                        SMSList.Add(data);
                                                                                    }
                                                                                    if (dep_StartDate == null || dep_StartDate == "")
                                                                                    {
                                                                                        LoanAppResSMS data = new LoanAppResSMS();
                                                                                        data.SMS = "PersonDeposit - StartDate is required";
                                                                                        SMSList.Add(data);
                                                                                    }
                                                                                    if (dep_MaturityDate == null || dep_MaturityDate == "")
                                                                                    {
                                                                                        LoanAppResSMS data = new LoanAppResSMS();
                                                                                        data.SMS = "PersonDeposit - MaturityDate is required";
                                                                                        SMSList.Add(data);
                                                                                    }
                                                                                    if (dep_Amount == null || dep_Amount == "")
                                                                                    {
                                                                                        LoanAppResSMS data = new LoanAppResSMS();
                                                                                        data.SMS = "PersonDeposit - Amount is required";
                                                                                        SMSList.Add(data);
                                                                                    }
                                                                                    if (dep_AccountOwnerName == null || dep_AccountOwnerName == "")
                                                                                    {
                                                                                        LoanAppResSMS data = new LoanAppResSMS();
                                                                                        data.SMS = "PersonDeposit - AccountOwnerName is required";
                                                                                        SMSList.Add(data);
                                                                                    }
                                                                                    if (dep_Currency == null || dep_Currency == "")
                                                                                    {
                                                                                        LoanAppResSMS data = new LoanAppResSMS();
                                                                                        data.SMS = "PersonDeposit - Currency is required";
                                                                                        SMSList.Add(data);
                                                                                    }
                                                                                    if (dep_RelationshipID == null || dep_RelationshipID == "")
                                                                                    {
                                                                                        LoanAppResSMS data = new LoanAppResSMS();
                                                                                        data.SMS = "PersonDeposit - RelationshipID is required";
                                                                                        SMSList.Add(data);
                                                                                    }
                                                                                    if (dep_DOB == null || dep_DOB == "")
                                                                                    {
                                                                                        LoanAppResSMS data = new LoanAppResSMS();
                                                                                        data.SMS = "PersonDeposit - DOB is required";
                                                                                        SMSList.Add(data);
                                                                                    }
                                                                                    if (dep_GenderID == null || dep_GenderID == "")
                                                                                    {
                                                                                        LoanAppResSMS data = new LoanAppResSMS();
                                                                                        data.SMS = "PersonDeposit - GenderID is required";
                                                                                        SMSList.Add(data);
                                                                                    }
                                                                                    if (dep_NIDNo == null || dep_NIDNo == "")
                                                                                    {
                                                                                        LoanAppResSMS data = new LoanAppResSMS();
                                                                                        data.SMS = "PersonDeposit - NIDNo is required";
                                                                                        SMSList.Add(data);
                                                                                    }
                                                                                    if (dep_VillageID == null || dep_VillageID == "")
                                                                                    {
                                                                                        LoanAppResSMS data = new LoanAppResSMS();
                                                                                        data.SMS = "PersonDeposit - VillageID is required";
                                                                                        SMSList.Add(data);
                                                                                    }

                                                                                }
                                                                                catch (Exception ex)
                                                                                {
                                                                                    ERR = "Error";
                                                                                    SMS = "Cannot read Deposit";
                                                                                    ExSMS = ex.Message.ToString();
                                                                                }
                                                                                #endregion sql
                                                                            }
                                                                            catch (Exception ex)
                                                                            {
                                                                                ERR = "Error";
                                                                                SMS = "Cannot read Deposit";
                                                                                ExSMS = ex.Message.ToString();
                                                                            }
                                                                        }
                                                                    }
                                                                }

                                                            }
                                                            catch (Exception ex)
                                                            {
                                                                ERR = "Error";
                                                                SMS = "Cannot read Deposit";
                                                                ExSMS = ex.Message.ToString();
                                                            }
                                                        }
                                                        #endregion PersonDeposit
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        ERR = "Error";
                                                        SMS = "Cannot read Deposit";
                                                        ExSMS = ex.Message.ToString();
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    ERR = "Error";
                                                    SMS = "Cannot read customer parameter 1" + ex.Message.ToString();
                                                    ExSMS = ex.Message.ToString();
                                                }
                                            }
                                        }
                                    }
                                    #endregion Person

                                    #region Purpose
                                    if (ERR != "Error")
                                    {
                                        try
                                        {
                                            if (loan.Purpose != null)
                                            {
                                                foreach (var Purpose in loan.Purpose)
                                                {
                                                    if (ERR != "Error")
                                                    {
                                                        try
                                                        {
                                                            string LoanPurposeServerID = "0";
                                                            #region Para
                                                            string LoanPurposeClientID = Purpose.LoanPurposeClientID;
                                                            string purpose_loanPurposeServerID = Purpose.LoanPurposeServerID;
                                                            string purpose_loanClientID = Purpose.LoanClientID;
                                                            string purpose_loanAppID = Purpose.LoanAppID;
                                                            string pur_LoanPurposeID = Purpose.LoanPurposeID;
                                                            #endregion Para
                                                            #region sql
                                                            try
                                                            {
                                                                if (LoanPurposeClientID == null || LoanPurposeClientID == "")
                                                                {
                                                                    LoanAppResSMS data = new LoanAppResSMS();
                                                                    data.SMS = "Purpose - LoanPurposeClientID is required";
                                                                    SMSList.Add(data);
                                                                }
                                                                if (pur_LoanPurposeID == null || pur_LoanPurposeID == "")
                                                                {
                                                                    LoanAppResSMS data = new LoanAppResSMS();
                                                                    data.SMS = "Purpose - LoanPurposeID is required";
                                                                    SMSList.Add(data);
                                                                }

                                                            }
                                                            catch (Exception ex)
                                                            {
                                                                ERR = "Error";
                                                                SMS = "Cannot add customer image to switch DB";
                                                                ExSMS = ex.Message.ToString();
                                                            }
                                                            #endregion sql

                                                            #region Purpose Detail
                                                            if (ERR != "Error")
                                                            {
                                                                try
                                                                {
                                                                    foreach (var PurposeDetail in Purpose.PurposeDetail)
                                                                    {
                                                                        if (ERR != "Error")
                                                                        {
                                                                            #region Para
                                                                            string LoanPurposeDetailClientID = PurposeDetail.LoanPurposeDetailClientID;
                                                                            string purposeDetail_loanPurposeDetailServerID = PurposeDetail.LoanPurposeDetailServerID;
                                                                            string purposeDetail_loanPurposeClientID = PurposeDetail.LoanPurposeClientID;
                                                                            string purposeDetail_loanPurposeServerID = PurposeDetail.LoanPurposeServerID;
                                                                            string LoanAppPurpsoeDetail = PurposeDetail.LoanAppPurpsoeDetail;
                                                                            string pur_Quantity = PurposeDetail.Quantity;
                                                                            string pur_UnitPrice = PurposeDetail.UnitPrice;
                                                                            #endregion Para
                                                                            #region sql
                                                                            try
                                                                            {
                                                                                if (LoanPurposeDetailClientID == null || LoanPurposeDetailClientID == "")
                                                                                {
                                                                                    LoanAppResSMS data = new LoanAppResSMS();
                                                                                    data.SMS = "Purpose - LoanPurposeDetailClientID is required";
                                                                                    SMSList.Add(data);
                                                                                }
                                                                                if (LoanAppPurpsoeDetail == null || LoanAppPurpsoeDetail == "")
                                                                                {
                                                                                    LoanAppResSMS data = new LoanAppResSMS();
                                                                                    data.SMS = "Purpose - LoanAppPurpsoeDetail is required";
                                                                                    SMSList.Add(data);
                                                                                }
                                                                                if (pur_Quantity == null || pur_Quantity == "")
                                                                                {
                                                                                    LoanAppResSMS data = new LoanAppResSMS();
                                                                                    data.SMS = "Purpose - Quantity is required";
                                                                                    SMSList.Add(data);
                                                                                }
                                                                                if (pur_UnitPrice == null || pur_UnitPrice == "")
                                                                                {
                                                                                    LoanAppResSMS data = new LoanAppResSMS();
                                                                                    data.SMS = "Purpose - UnitPrice is required";
                                                                                    SMSList.Add(data);
                                                                                }

                                                                            }
                                                                            catch (Exception ex)
                                                                            {
                                                                                ERR = "Error";
                                                                                SMS = "Cannot read Purpose Detail";
                                                                                ExSMS = ex.Message.ToString();
                                                                            }
                                                                            #endregion sql
                                                                        }
                                                                    }
                                                                }
                                                                catch (Exception ex)
                                                                {
                                                                    ERR = "Error";
                                                                    SMS = "Cannot read Purpose Detail";
                                                                    ExSMS = ex.Message.ToString();
                                                                }
                                                            }
                                                            #endregion Purpose Detail
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            ERR = "Error";
                                                            SMS = "Cannot read Purpose";
                                                            ExSMS = ex.Message.ToString();
                                                        }
                                                    }
                                                }
                                            }

                                        }
                                        catch (Exception ex)
                                        {
                                            ERR = "Error";
                                            SMS = "Cannot read Purpose";
                                            ExSMS = ex.Message.ToString();
                                        }
                                    }
                                    #endregion Purpose

                                    #region CashFlow
                                    if (ERR != "Error")
                                    {
                                        try
                                        {
                                            if (loan.CashFlow!=null)
                                            {
                                                foreach (var CashFlow in loan.CashFlow)
                                                {
                                                    string ServerLoanAppCashFlowID = "0";
                                                    #region Para
                                                    string CashFlowClientID = CashFlow.CashFlowClientID;
                                                    string cashFlow_cashFlowServerID = CashFlow.CashFlowServerID;
                                                    string cashFlow_loanClientID = CashFlow.LoanClientID;
                                                    string cashFlow_loanAppID = CashFlow.LoanAppID;
                                                    string StudyMonthAmount = CashFlow.StudyMonthAmount;
                                                    string StudyStartMonth = CashFlow.StudyStartMonth;
                                                    string FamilyExpensePerMonth = CashFlow.FamilyExpensePerMonth;
                                                    string OtherExpensePerMonth = CashFlow.OtherExpensePerMonth;
                                                    #endregion Para
                                                    #region sql
                                                    try
                                                    {
                                                        if (CashFlowClientID == null || CashFlowClientID == "")
                                                        {
                                                            LoanAppResSMS data = new LoanAppResSMS();
                                                            data.SMS = "CashFlow - CashFlowClientID is required";
                                                            SMSList.Add(data);
                                                        }
                                                        if (StudyMonthAmount == null || StudyMonthAmount == "")
                                                        {
                                                            LoanAppResSMS data = new LoanAppResSMS();
                                                            data.SMS = "CashFlow - StudyMonthAmount is required";
                                                            SMSList.Add(data);
                                                        }
                                                        if (StudyStartMonth == null || StudyStartMonth == "")
                                                        {
                                                            LoanAppResSMS data = new LoanAppResSMS();
                                                            data.SMS = "CashFlow - StudyStartMonth is required";
                                                            SMSList.Add(data);
                                                        }
                                                        if (FamilyExpensePerMonth == null || FamilyExpensePerMonth == "")
                                                        {
                                                            LoanAppResSMS data = new LoanAppResSMS();
                                                            data.SMS = "CashFlow - FamilyExpensePerMonth is required";
                                                            SMSList.Add(data);
                                                        }
                                                        if (OtherExpensePerMonth == null || OtherExpensePerMonth == "")
                                                        {
                                                            LoanAppResSMS data = new LoanAppResSMS();
                                                            data.SMS = "CashFlow - OtherExpensePerMonth is required";
                                                            SMSList.Add(data);
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        ERR = "Error";
                                                        SMS = "Cannot read cash flow";
                                                        ExSMS = ex.Message.ToString();
                                                    }
                                                    #endregion sql

                                                    #region CashFlowMSI
                                                    if (ERR != "Error")
                                                    {
                                                        try
                                                        {
                                                            if (CashFlow.CashFlowMSI != null)
                                                            {
                                                                foreach (var CashFlowMSI in CashFlow.CashFlowMSI)
                                                                {
                                                                    string ServerLoanAppCashFlowMSIID = "0";
                                                                    if (ERR != "Error")
                                                                    {
                                                                        #region Para
                                                                        string cashFlowMSI_cashFlowMSIClientID = CashFlowMSI.CashFlowMSIClientID;
                                                                        string cashFlowMSI_cashFlowMSIServerID = CashFlowMSI.CashFlowMSIServerID;
                                                                        string cashFlowMSI_cashFlowClientID = CashFlowMSI.CashFlowClientID;
                                                                        string cashFlowMSI_cashFlowServerID = CashFlowMSI.CashFlowServerID;
                                                                        string IncomeTypeID = CashFlowMSI.IncomeTypeID;
                                                                        string MSI_OccupationTypeID = CashFlowMSI.OccupationTypeID;
                                                                        string MainSourceIncomeID = CashFlowMSI.MainSourceIncomeID;
                                                                        string MSI_ExAge = CashFlowMSI.ExAge;
                                                                        string MSI_BusAge = CashFlowMSI.BusAge;
                                                                        #endregion Para
                                                                        #region sql
                                                                        try
                                                                        {
                                                                            if (IncomeTypeID == null || IncomeTypeID == "")
                                                                            {
                                                                                LoanAppResSMS data = new LoanAppResSMS();
                                                                                data.SMS = "IncomeTypeID is required";
                                                                                SMSList.Add(data);
                                                                            }
                                                                            if (MainSourceIncomeID == null || MainSourceIncomeID == "")
                                                                            {
                                                                                LoanAppResSMS data = new LoanAppResSMS();
                                                                                data.SMS = "MainSourceIncomeID is required";
                                                                                SMSList.Add(data);
                                                                            }
                                                                            if (MSI_ExAge == null || MSI_ExAge == "")
                                                                            {
                                                                                LoanAppResSMS data = new LoanAppResSMS();
                                                                                data.SMS = "MSI_ExAge is required";
                                                                                SMSList.Add(data);
                                                                            }
                                                                            if (MSI_BusAge == null || MSI_BusAge == "")
                                                                            {
                                                                                LoanAppResSMS data = new LoanAppResSMS();
                                                                                data.SMS = "MSI_BusAge is required";
                                                                                SMSList.Add(data);
                                                                            }
                                                                            if (MSI_OccupationTypeID == null || MSI_OccupationTypeID == "")
                                                                            {
                                                                                LoanAppResSMS data = new LoanAppResSMS();
                                                                                data.SMS = "MSI_OccupationTypeID is required";
                                                                                SMSList.Add(data);
                                                                            }
                                                                        }
                                                                        catch (Exception ex)
                                                                        {
                                                                            ERR = "Error";
                                                                            SMS = "Cannot read cash flow";
                                                                            ExSMS = ex.Message.ToString();
                                                                        }
                                                                        #endregion sql

                                                                        #region CashFlowMSIInEx
                                                                        if (ERR != "Error")
                                                                        {
                                                                            try
                                                                            {
                                                                                if (CashFlowMSI.CashFlowMSIInEx != null)
                                                                                {
                                                                                    foreach (var CashFlowMSIInEx in CashFlowMSI.CashFlowMSIInEx)
                                                                                    {
                                                                                        if (ERR != "Error")
                                                                                        {
                                                                                            #region Para
                                                                                            string LoanAppCashFlowMSIInExClientID = CashFlowMSIInEx.LoanAppCashFlowMSIInExClientID;
                                                                                            string mSIInEx_loanAppCashFlowMSIInExServerID = CashFlowMSIInEx.LoanAppCashFlowMSIInExServerID;
                                                                                            string mSIInEx_cashFlowMSIClientID = CashFlowMSIInEx.CashFlowMSIClientID;
                                                                                            string mSIInEx_cashFlowMSIServerID = CashFlowMSIInEx.CashFlowMSIServerID;
                                                                                            string InExCodeID = CashFlowMSIInEx.InExCodeID;
                                                                                            string MSIInEx_Description = CashFlowMSIInEx.Description;
                                                                                            string MSIInEx_Month = CashFlowMSIInEx.Month;
                                                                                            string MSIInEx_Mount = CashFlowMSIInEx.Amount;
                                                                                            string MSIInEx_UnitID = CashFlowMSIInEx.UnitID;
                                                                                            string MSIInEx_Cost = CashFlowMSIInEx.Cost;
                                                                                            string OneIncomeTwoExpense = CashFlowMSIInEx.OneIncomeTwoExpense;
                                                                                            #endregion Para
                                                                                            #region sql
                                                                                            try
                                                                                            {
                                                                                                if (LoanAppCashFlowMSIInExClientID == null || LoanAppCashFlowMSIInExClientID == "")
                                                                                                {
                                                                                                    LoanAppResSMS data = new LoanAppResSMS();
                                                                                                    data.SMS = "CashFlowMSIInEx - LoanAppCashFlowMSIInExClientID is required";
                                                                                                    SMSList.Add(data);
                                                                                                }
                                                                                                if (InExCodeID == null || InExCodeID == "")
                                                                                                {
                                                                                                    LoanAppResSMS data = new LoanAppResSMS();
                                                                                                    data.SMS = "CashFlowMSIInEx - InExCodeID is required";
                                                                                                    SMSList.Add(data);
                                                                                                }
                                                                                                if (MSIInEx_Description == null || MSIInEx_Description == "")
                                                                                                {
                                                                                                    LoanAppResSMS data = new LoanAppResSMS();
                                                                                                    data.SMS = "CashFlowMSIInEx - Description is required";
                                                                                                    SMSList.Add(data);
                                                                                                }
                                                                                                if (MSIInEx_Month == null || MSIInEx_Month == "")
                                                                                                {
                                                                                                    LoanAppResSMS data = new LoanAppResSMS();
                                                                                                    data.SMS = "CashFlowMSIInEx - Month is required";
                                                                                                    SMSList.Add(data);
                                                                                                }
                                                                                                if (MSIInEx_Mount == null || MSIInEx_Mount == "")
                                                                                                {
                                                                                                    LoanAppResSMS data = new LoanAppResSMS();
                                                                                                    data.SMS = "CashFlowMSIInEx - Mount is required";
                                                                                                    SMSList.Add(data);
                                                                                                }
                                                                                                if (MSIInEx_UnitID == null || MSIInEx_UnitID == "")
                                                                                                {
                                                                                                    LoanAppResSMS data = new LoanAppResSMS();
                                                                                                    data.SMS = "CashFlowMSIInEx - UnitID is required";
                                                                                                    SMSList.Add(data);
                                                                                                }
                                                                                                if (MSIInEx_Cost == null || MSIInEx_Cost == "")
                                                                                                {
                                                                                                    LoanAppResSMS data = new LoanAppResSMS();
                                                                                                    data.SMS = "CashFlowMSIInEx - Cost is required";
                                                                                                    SMSList.Add(data);
                                                                                                }
                                                                                                if (OneIncomeTwoExpense == null || OneIncomeTwoExpense == "")
                                                                                                {
                                                                                                    LoanAppResSMS data = new LoanAppResSMS();
                                                                                                    data.SMS = "CashFlowMSIInEx - OneIncomeTwoExpense is required";
                                                                                                    SMSList.Add(data);
                                                                                                }

                                                                                            }
                                                                                            catch (Exception ex)
                                                                                            {
                                                                                                ERR = "Error";
                                                                                                SMS = "Cannot read cash flow msi InEx";
                                                                                                ExSMS = ex.Message.ToString();
                                                                                            }
                                                                                            #endregion sql
                                                                                        }
                                                                                    }
                                                                                }

                                                                            }
                                                                            catch (Exception ex)
                                                                            {
                                                                                ERR = "Error";
                                                                                SMS = "Cannot read cash flow msi InEx";
                                                                                ExSMS = ex.Message.ToString();
                                                                            }
                                                                        }
                                                                        #endregion CashFlowMSIInEx
                                                                    }
                                                                }
                                                            }


                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            ERR = "Error";
                                                            SMS = "Cannot read cash flow msi";
                                                            ExSMS = ex.Message.ToString();
                                                        }
                                                    }
                                                    #endregion CashFlowMSI
                                                }
                                            }

                                        }
                                        catch (Exception ex)
                                        {
                                            ERR = "Error";
                                            SMS = "Cannot read cash flow";
                                            ExSMS = ex.Message.ToString();
                                        }
                                    }
                                    #endregion CashFlow

                                }
                            //}
                        }
                    }
                    catch { }
                }

                #endregion
                #region check smslist
                if (ERR != "Error")
                {
                    if (SMSList.Count > 0)
                    {
                        ERR = "Error";
                        SMS = "Validation is fail";
                    }
                }
                #endregion check smslist

                #region old code
                //#region LoanApp
                //if (ERR != "Error")
                //{
                //    string ServerLoanAppID = "0", PersonType = "";
                //    try
                //    {
                //        foreach (var loan in jObj.LoanApp)
                //        {
                //            try
                //            {
                //                #region LoanApp Param
                //                string LoanClientID = loan.LoanClientID;
                //                string LoanAppID = loan.LoanAppID;
                //                string LoanAppStatusID = loan.LoanAppStatusID;
                //                string DeviceDate = loan.DeviceDate;
                //                string ProductID = loan.ProductID;
                //                string LoanRequestAmount = loan.LoanRequestAmount.Replace(",", "");
                //                string OwnCapital = loan.OwnCapital;
                //                string DisbursementDate = loan.DisbursementDate;
                //                string FirstWithdrawal = loan.FirstWithdrawal;
                //                string LoanTerm = loan.LoanTerm;
                //                string FirstRepaymentDate = loan.FirstRepaymentDate;
                //                string LoanInterestRate = loan.LoanInterestRate;
                //                string CustomerRequestRate = loan.CustomerRequestRate;
                //                string CompititorRate = loan.CompititorRate;
                //                string CustomerConditionID = loan.CustomerConditionID;
                //                string COProposedAmount = loan.COProposedAmount;
                //                string COProposedTerm = loan.COProposedTerm;
                //                string COProposeRate = loan.COProposeRate;
                //                string FrontBackOfficeID = loan.FrontBackOfficeID;
                //                string GroupNumber = loan.GroupNumber;
                //                string LoanCycleID = loan.LoanCycleID;
                //                string RepaymentHistoryID = loan.RepaymentHistoryID;
                //                string LoanReferralID = loan.LoanReferralID;
                //                //string DebtIinfoID = loan.DebtIinfoID;
                //                string MonthlyFee = loan.MonthlyFee;
                //                string Compulsory = loan.Compulsory;
                //                string CompulsoryTerm = loan.CompulsoryTerm;
                //                string Currency = loan.Currency;
                //                string UpFrontFee = loan.UpFrontFee;
                //                //string UpFrontAmt=loan.UpFrontAmt;
                //                string CompulsoryOptionID = loan.CompulsoryOptionID;
                //                string FundSource = loan.FundSource;
                //                string IsNewCollateral = loan.IsNewCollateral;
                //                string AgriBuddy = loan.AgriBuddy;
                //                string semiBallonFreqID = loan.SemiBallonFreqID;

                //                string LoanTypeID = loan.LoanTypeID;
                //                //string AMApproveAmt = loan.AMApproveAmt;
                //                //string AMApproveTerm = loan.AMApproveTerm;
                //                //string AMApproveRate = loan.AMApproveRate;
                //                string PaymentMethodID = loan.PaymentMethodID;
                //                string GracePeriodID = loan.GracePeriodID;
                //                string MITypeID = loan.MITypeID;

                //                string DebtIinfoID = "1";
                //                foreach (var Person in loan.Person)
                //                {
                //                    PersonType = Person.LoanAppPersonTypeID;
                //                    if (Person.LoanAppPersonTypeID == "31")
                //                    {
                //                        if (Person.Creditor != null)
                //                        {
                //                            DebtIinfoID = "2";
                //                        }
                //                    }
                //                }
                //                string UpFromAmt = "";
                //                UpFromAmt = ((Convert.ToDouble(LoanRequestAmount) * Convert.ToDouble(UpFrontFee)) / 100).ToString();

                //                #endregion LoanApp Param
                //                #region sql
                //                try
                //                {

                //                    if (CompulsoryTerm == null)
                //                    {
                //                        CompulsoryTerm = "-1";
                //                    }
                //                    if (MonthlyFee == null)
                //                    {
                //                        MonthlyFee = "-1";
                //                    }
                //                    if (CompititorRate == null)
                //                    {
                //                        CompititorRate = "-1";
                //                    }
                //                    if (CustomerConditionID == null)
                //                    {
                //                        CustomerConditionID = "-1";
                //                    }
                //                    if (LoanReferralID == null)
                //                    {
                //                        LoanReferralID = "-1";
                //                    }
                //                    if (FrontBackOfficeID == null)
                //                    {
                //                        FrontBackOfficeID = "-1";
                //                    }

                //                    string CollateralDebt = loan.CollateralDebt;

                //                    string[] rs = c.AddLoanFromDevice(LoanClientID, LoanAppID, LoanAppStatusID, DeviceDate, ServerDate, UserID, ProductID
                //                    , LoanRequestAmount, OwnCapital, DisbursementDate, FirstWithdrawal, LoanTerm, FirstRepaymentDate
                //                    , LoanInterestRate, COProposedAmount, COProposedTerm, COProposeRate, GroupNumber, LoanCycleID
                //                    , RepaymentHistoryID, DebtIinfoID, Compulsory, CompulsoryTerm, Currency, UpFrontFee, UpFromAmt, CompulsoryOptionID
                //                    , FundSource, IsNewCollateral, AgriBuddy, semiBallonFreqID
                //                    , PaymentMethodID, LoanTypeID, GracePeriodID, MITypeID

                //                    , MonthlyFee, CompititorRate, CustomerConditionID, LoanReferralID
                //                    , FrontBackOfficeID, CollateralDebt);

                //                    ERR = rs[0];
                //                    SMS = rs[1];
                //                    ServerLoanAppID = rs[2];
                //                    ExSMS = rs[3];

                //                    RSIDOnDevice = LoanClientID;
                //                    RSLoanAppID = ServerLoanAppID;
                //                }
                //                catch (Exception ex)
                //                {
                //                    ERR = "Error";
                //                    SMS = "Cannot add loan to switch DB";
                //                    ExSMS = ex.Message.ToString();
                //                }
                //                #endregion sql
                //            }
                //            catch (Exception ex)
                //            {
                //                ERR = "Error";
                //                SMS = "Cannot read loan parameter";
                //                ExSMS = ex.Message.ToString();
                //            }

                //            if (ERR != "Error")
                //            {
                //                string ServerLoanAppPersonID = "0", VillageBankIDForGetCOIDOri = "";
                //                #region Person

                //                if (loan.Person != null)
                //                {
                //                    foreach (var Person in loan.Person)
                //                    {
                //                        if (ERR != "Error")
                //                        {
                //                            try
                //                            {
                //                                #region Person Para
                //                                string LoanAppPersonTypeID = Person.LoanAppPersonTypeID;
                //                                string CustomerID = Person.T24CustID;
                //                                string VillageBankID = Person.VBID;
                //                                if (LoanAppPersonTypeID == "31")
                //                                {
                //                                    VillageBankIDForGetCOIDOri = VillageBankID;
                //                                }
                //                                string NameKhLast = Person.NameKhLast;
                //                                string TitleID = Person.TitleID;
                //                                string LastName = Person.NameEnLast;
                //                                string FirstName = Person.NameEnFirst;
                //                                string GenderID = Person.GenderID;
                //                                string DateOfBirth = Person.DateOfBirth;
                //                                string IDCardTypeID = Person.IDCardTypeID;
                //                                string IDCardNumber = Person.IDCardNumber;
                //                                string IDCardExpireDate = Person.IDCardExpiryDate;
                //                                string MaritalStatusID = Person.MaritalStatusID;
                //                                string EducationID = Person.EducationLevelID;
                //                                string CityOfBirthID = Person.PlaceOfBirth;
                //                                string Telephone3 = Person.Phone;
                //                                string VillageIDPermanent = Person.VillageIDPer;
                //                                string VillageIDCurrent = Person.VillageIDCur;
                //                                string SortAddress = Person.ShortAddress;
                //                                string FamilyMember = Person.FamilyMember;
                //                                string FamilyMemberActive = Person.FamilyMemberActive;
                //                                string PoorID = Person.PoorLevelID;
                //                                string LoanAppPersonIDOnDevice = Person.CustClientID;
                //                                string CustDeviceDate = Person.CreateDateClient;
                //                                string IDCardIssuedDate = Person.IDCardIssueDate;
                //                                string NameKhFirst = Person.NameKhFirst;
                //                                string ProspectCode = Person.ProspectCode;
                //                                string ReferByID = Person.ReferByID;
                //                                string ReferName = Person.ReferName;
                //                                string LatLon = Person.LatLon;
                //                                string CustServerID = Person.CustServerID;
                //                                #endregion Person Para
                //                                #region sql
                //                                try
                //                                {
                //                                    string[] rs = c.AddCustFromDevice(ServerLoanAppID, LoanAppPersonTypeID, CustomerID, VillageBankID
                //                                    , NameKhLast, TitleID, LastName, FirstName, GenderID, DateOfBirth, IDCardTypeID, IDCardNumber
                //                                    , IDCardExpireDate, MaritalStatusID, EducationID, CityOfBirthID, Telephone3, VillageIDPermanent
                //                                    , VillageIDCurrent, SortAddress, FamilyMember, FamilyMemberActive, PoorID, LoanAppPersonIDOnDevice
                //                                    , CustDeviceDate, IDCardIssuedDate, NameKhFirst, ProspectCode, ReferByID, ReferName, LatLon, CustServerID);

                //                                    ERR = rs[0];
                //                                    SMS = rs[1];
                //                                    ServerLoanAppPersonID = rs[2];
                //                                    ExSMS = rs[3];

                //                                }
                //                                catch (Exception ex)
                //                                {
                //                                    ERR = "Error";
                //                                    SMS = "Cannot add customer to switch DB";
                //                                    ExSMS = ex.Message.ToString();
                //                                }
                //                                #endregion sql

                //                                #region PersonImg
                //                                if (Person.PersonImg != null)
                //                                {
                //                                    foreach (var PersonImg in Person.PersonImg)
                //                                    {
                //                                        if (ERR != "Error")
                //                                        {
                //                                            #region Para
                //                                            string CustImageClientID = PersonImg.CustImageClientID;
                //                                            string perImg_custImageServerID = PersonImg.CustImageServerID;
                //                                            string perImg_custClientID = PersonImg.CustClientID;
                //                                            string perImg_createDateClient = PersonImg.CreateDateClient;
                //                                            string OneCardTwoDoc = PersonImg.OneCardTwoDoc;
                //                                            string perImg_ext = PersonImg.Ext;
                //                                            string perImg_imgPath = PersonImg.ImgPath;//File Name
                //                                            string perImg_remark = PersonImg.Remark;

                //                                            string perImg_fName = "PersonImg_" + ServerLoanAppID + "_" + ServerLoanAppPersonID + "_" + OneCardTwoDoc + "_" + CustImageClientID + "_" + ServerDateForFileName + "." + perImg_ext.Replace(".", "");
                //                                            #endregion Para
                //                                            #region sql
                //                                            try
                //                                            {
                //                                                string[] rs = c.AddCustImgFromDevice(ServerLoanAppPersonID, OneCardTwoDoc, perImg_ext, c.ImgPathGet()+perImg_fName, ServerLoanAppID, CustImageClientID);

                //                                                ERR = rs[0];
                //                                                SMS = rs[1];
                //                                                string ServerLoanAppPersonImageID = rs[2];
                //                                                ExSMS = rs[3];

                //                                                if (ERR != "Error")
                //                                                {
                //                                                    LoanAppResImgList ilist = new LoanAppResImgList();
                //                                                    ilist.ImgType = "PersonImg";
                //                                                    ilist.ClientID = CustImageClientID;
                //                                                    ilist.ServerID = ServerLoanAppPersonImageID;
                //                                                    ilist.OriImgName = perImg_imgPath;
                //                                                    ilist.ServerImgName = perImg_fName;
                //                                                    ImgList.Add(ilist);
                //                                                }
                //                                            }
                //                                            catch (Exception ex)
                //                                            {
                //                                                ERR = "Error";
                //                                                SMS = "Cannot add customer image to switch DB";
                //                                                ExSMS = ex.Message.ToString();
                //                                            }
                //                                            #endregion sql
                //                                        }
                //                                    }
                //                                }

                //                                #endregion PersonImg

                //                                if (Person.LoanAppPersonTypeID == "31")
                //                                {

                //                                    #region Asset

                //                                    if (Person.Asset != null)
                //                                    {
                //                                        foreach (var Asset in Person.Asset)
                //                                        {
                //                                            if (ERR != "Error")
                //                                            {
                //                                                string ServerLoanAppClientAssetID = "0";
                //                                                #region Para
                //                                                string asset_assetClientID = Asset.AssetClientID;
                //                                                string asset_assetServerID = Asset.AssetServerID;
                //                                                string asset_loanPurposeClientID = Asset.LoanPurposeClientID;
                //                                                string asset_loanClientID = Asset.LoanClientID;
                //                                                string asset_loanAppID = Asset.LoanAppID;
                //                                                string asset_description = Asset.Description;
                //                                                string asset_quantity = Asset.Quantity;
                //                                                string asset_unitPrice = Asset.UnitPrice;
                //                                                string asset_custClientID = Asset.CustClientID;
                //                                                string asset_custServerID = Asset.CustServerID;
                //                                                string asset_lookupID = Asset.AssetLookUpID;
                //                                                string assetOtherDescription = Asset.AssetOtherDescription;
                //                                                #endregion Para
                //                                                #region sql
                //                                                try
                //                                                {
                //                                                    string[] rs = c.AddCustAssetFromDevice(ServerLoanAppPersonID, asset_description, asset_quantity, asset_unitPrice, asset_custClientID, ServerLoanAppID, asset_lookupID, assetOtherDescription);

                //                                                    ERR = rs[0];
                //                                                    SMS = rs[1];
                //                                                    ServerLoanAppClientAssetID = rs[2];
                //                                                    ExSMS = rs[3];
                //                                                }
                //                                                catch (Exception ex)
                //                                                {
                //                                                    ERR = "Error";
                //                                                    SMS = "Cannot add asset to switch DB";
                //                                                    ExSMS = ex.Message.ToString();
                //                                                }
                //                                                #endregion sql
                //                                                if (Asset.AssetImg != null)
                //                                                {
                //                                                    foreach (var AssetImg in Asset.AssetImg)
                //                                                    {
                //                                                        if (ERR != "Error")
                //                                                        {
                //                                                            try
                //                                                            {
                //                                                                #region Para
                //                                                                string AssetImageClientID = AssetImg.AssetImageClientID;
                //                                                                string AssetImageServerID = AssetImg.AssetImageServerID;
                //                                                                string AssetClientID = AssetImg.AssetClientID;
                //                                                                string AssetServerID = AssetImg.AssetServerID;
                //                                                                string CreateDateClient = AssetImg.CreateDateClient;
                //                                                                string AssetImg_Ext = AssetImg.Ext;
                //                                                                string AssetImg_ImgPath = AssetImg.ImgPath;//file name
                //                                                                string Remark = AssetImg.Remark;

                //                                                                string Asset_fName = "AssetImg_" + ServerLoanAppID + "_" + ServerLoanAppClientAssetID + "_" + AssetImageClientID + "_" + ServerDateForFileName + "." + AssetImg_Ext.Replace(".", "");
                //                                                                #endregion Para
                //                                                                #region sql
                //                                                                try
                //                                                                {
                //                                                                    string[] rs = c.AddCustAsseImgFromDevice(AssetImageClientID, ServerLoanAppClientAssetID, CreateDateClient, AssetImg_Ext, c.ImgPathGet() + Asset_fName);

                //                                                                    ERR = rs[0];
                //                                                                    SMS = rs[1];
                //                                                                    AssetServerID = rs[2];
                //                                                                    ExSMS = rs[3];

                //                                                                    if (ERR != "Error")
                //                                                                    {
                //                                                                        LoanAppResImgList ilist = new LoanAppResImgList();
                //                                                                        ilist.ImgType = "AssetImg";
                //                                                                        ilist.ClientID = AssetImageClientID;
                //                                                                        ilist.ServerID = AssetServerID;
                //                                                                        ilist.OriImgName = AssetImg_ImgPath;
                //                                                                        ilist.ServerImgName = Asset_fName;
                //                                                                        ImgList.Add(ilist);
                //                                                                    }
                //                                                                }
                //                                                                catch (Exception ex)
                //                                                                {
                //                                                                    ERR = "Error";
                //                                                                    SMS = "Cannot add asset image to switch DB";
                //                                                                    ExSMS = ex.Message.ToString();
                //                                                                }
                //                                                                #endregion sql
                //                                                            }
                //                                                            catch (Exception ex)
                //                                                            {
                //                                                                ERR = "Error";
                //                                                                SMS = "Cannot add asset image to switch DB";
                //                                                                ExSMS = ex.Message.ToString();
                //                                                            }
                //                                                        }
                //                                                    }
                //                                                }

                //                                            }
                //                                        }
                //                                        //if Error
                //                                        if (ERR == "Error")
                //                                        {
                //                                            LoanAppResSMS data = new LoanAppResSMS();
                //                                            data.SMS = "Asset: " + ExSMS;
                //                                            SMSList.Add(data);
                //                                        }
                //                                    }
                //                                    #endregion Asset

                //                                    #region Creditor
                //                                    if (ERR != "Error")
                //                                    {

                //                                        if (Person.Creditor != null)
                //                                        {
                //                                            foreach (var Creditor in Person.Creditor)
                //                                            {
                //                                                try
                //                                                {
                //                                                    if (ERR != "Error")
                //                                                    {
                //                                                        #region Para
                //                                                        string CreditorClientID = Creditor.CreditorClientID;
                //                                                        string creditor_creditorServerID = Creditor.CreditorServerID;
                //                                                        string creditor_loanClientID = Creditor.LoanClientID;
                //                                                        string creditor_loanAppID = Creditor.LoanAppID;
                //                                                        string CreditorID = Creditor.CreditorID;
                //                                                        string creditor_currency = Creditor.Currency;
                //                                                        string creditor_approvedAmount = Creditor.ApprovedAmount;
                //                                                        string creditor_outstandingBalance = Creditor.OutstandingBalance;
                //                                                        string creditor_interestRate = Creditor.InterestRate;
                //                                                        string creditor_repaymentTypeID = Creditor.RepaymentTypeID;
                //                                                        string creditor_repaymentTermID = Creditor.RepaymentTermID;
                //                                                        string creditor_loanStartDate = Creditor.LoanStartDate;
                //                                                        string creditor_loanEndDate = Creditor.LoanEndDate;
                //                                                        string creditor_isReFinance = Creditor.IsReFinance;
                //                                                        string creditor_reFinanceAmount = Creditor.ReFinanceAmount;
                //                                                        string creditor_custClientID = Creditor.CustClientID;
                //                                                        string creditor_custServerID = Creditor.CustServerID;
                //                                                        #endregion Para
                //                                                        #region sql
                //                                                        try
                //                                                        {
                //                                                            string[] rs = c.AddCustCreditorFromDevice(CreditorClientID, CreditorID, creditor_currency, creditor_approvedAmount
                //                                                                , creditor_outstandingBalance, creditor_interestRate, creditor_repaymentTypeID, creditor_repaymentTermID
                //                                                                , creditor_loanStartDate, creditor_loanEndDate, creditor_isReFinance, creditor_reFinanceAmount, ServerLoanAppPersonID
                //                                                                , ServerLoanAppID);

                //                                                            ERR = rs[0];
                //                                                            SMS = rs[1];
                //                                                            string ServerLoanAppCreditorID = rs[2];
                //                                                            ExSMS = rs[3];

                //                                                        }
                //                                                        catch (Exception ex)
                //                                                        {
                //                                                            ERR = "Error";
                //                                                            SMS = "Cannot add Creditor to switch DB";
                //                                                            ExSMS = ex.Message.ToString();
                //                                                        }
                //                                                        #endregion sql
                //                                                    }
                //                                                }
                //                                                catch (Exception ex)
                //                                                {
                //                                                    ERR = "Error";
                //                                                    SMS = "Cannot read Creditor";
                //                                                    ExSMS = ex.Message.ToString();
                //                                                }

                //                                            }
                //                                        }
                //                                        //if Error
                //                                        if (ERR == "Error")
                //                                        {
                //                                            LoanAppResSMS data = new LoanAppResSMS();
                //                                            data.SMS = "Creditor: " + ExSMS;
                //                                            SMSList.Add(data);
                //                                        }
                //                                    }
                //                                    #endregion Creditor

                //                                    #region RealEstate
                //                                    if (ERR != "Error")
                //                                    {

                //                                        if (Person.RealEstate != null)
                //                                        {
                //                                            foreach (var RealEstate in Person.RealEstate)
                //                                            {
                //                                                if (ERR != "Error")
                //                                                {
                //                                                    try
                //                                                    {
                //                                                        string CollateralServerID = "0";
                //                                                        #region RealEstate
                //                                                        string re_CollateralClientID = RealEstate.CollateralClientID;
                //                                                        string re_CollateralServerID = RealEstate.CollateralServerID;
                //                                                        string re_CoanClientID = RealEstate.LoanClientID;
                //                                                        string re_LoanAppID = RealEstate.LoanAppID;
                //                                                        string re_CustClientID = RealEstate.CustClientID;
                //                                                        string re_CustServerID = RealEstate.CustServerID;
                //                                                        string re_CollateralDocGroupTypeID = RealEstate.CollateralDocGroupTypeID;
                //                                                        string re_CollateralDocHardTypeID = RealEstate.CollateralDocHardTypeID;
                //                                                        string re_CollateralDocSoftTypeID = RealEstate.CollateralDocSoftTypeID;
                //                                                        string re_CollateralOwnerTypeID = RealEstate.CollateralOwnerTypeID;
                //                                                        string re_CollateralLocationVillageID = RealEstate.CollateralLocationVillageID;
                //                                                        string re_CollateralRoadAccessibilityID = RealEstate.CollateralRoadAccessibilityID;
                //                                                        string re_PropertyTypeID = RealEstate.PropertyTypeID;
                //                                                        string re_LandTypeID = RealEstate.LandTypeID;
                //                                                        string re_LandSize = RealEstate.LandSize;
                //                                                        string re_LandMarketPrice = RealEstate.LandMarketPrice;
                //                                                        string re_LandForcedSalePrice = RealEstate.LandForcedSalePrice;
                //                                                        string re_BuildingTypeID = RealEstate.BuildingTypeID;
                //                                                        string re_BuildingSize = RealEstate.BuildingSize;
                //                                                        string re_BuildingMarketPrice = RealEstate.BuildingMarketPrice;
                //                                                        string re_BuildingForcedSalesPrice = RealEstate.BuildingForcedSalesPrice;
                //                                                        #endregion RealEstate
                //                                                        #region sql
                //                                                        try
                //                                                        {
                //                                                            string[] rs = c.AddCustRealEstateFromDevice(re_CollateralClientID, re_CoanClientID, ServerLoanAppID, re_CustClientID
                //                                                            , ServerLoanAppPersonID, re_CollateralDocGroupTypeID, re_CollateralDocHardTypeID, re_CollateralDocSoftTypeID
                //                                                            , re_CollateralOwnerTypeID, re_CollateralLocationVillageID, re_CollateralRoadAccessibilityID, re_PropertyTypeID
                //                                                            , re_LandTypeID, re_LandSize, re_LandMarketPrice, re_LandForcedSalePrice, re_BuildingTypeID, re_BuildingSize
                //                                                            , re_BuildingMarketPrice, re_BuildingForcedSalesPrice);

                //                                                            ERR = rs[0];
                //                                                            SMS = rs[1];
                //                                                            CollateralServerID = rs[2];
                //                                                            ExSMS = rs[3];

                //                                                        }
                //                                                        catch (Exception ex)
                //                                                        {
                //                                                            ERR = "Error";
                //                                                            SMS = "Cannot read RealEstate";
                //                                                            ExSMS = ex.Message.ToString();
                //                                                        }
                //                                                        #endregion sql
                //                                                        #region img
                //                                                        if (ERR != "Error")
                //                                                        {
                //                                                            try
                //                                                            {
                //                                                                if (RealEstate.RealEstateImg != null)
                //                                                                {
                //                                                                    foreach (var PREImgV2 in RealEstate.RealEstateImg)
                //                                                                    {
                //                                                                        if (ERR != "Error")
                //                                                                        {
                //                                                                            try
                //                                                                            {
                //                                                                                #region Para
                //                                                                                string PREImgV2_imageClientID = PREImgV2.ImageClientID;
                //                                                                                string PREImgV2_imageServerID = PREImgV2.ImageServerID;
                //                                                                                string PREImgV2_collateralClientID = PREImgV2.CollateralClientID;
                //                                                                                string PREImgV2_collateralServerID = PREImgV2.CollateralServerID;
                //                                                                                string PREImgV2_createDateClient = PREImgV2.CreateDateClient;
                //                                                                                string PREImgV2_ext = PREImgV2.Ext;
                //                                                                                string PREImgV2_imgPath = PREImgV2.ImgPath;
                //                                                                                string PREImgV2_remark = PREImgV2.Remark;

                //                                                                                string RealEstate_fName = "RealEstateImg_" + ServerLoanAppID + "_" + CollateralServerID + "_" + PREImgV2_imageClientID + "_" + ServerDateForFileName + "." + PREImgV2_ext.Replace(".", "");
                //                                                                                #endregion Para
                //                                                                                #region sql
                //                                                                                try
                //                                                                                {
                //                                                                                    string[] rs = c.AddCustRealEstateImgFromDevice(PREImgV2_imageClientID, CollateralServerID
                //                                                                                    , PREImgV2_collateralClientID, PREImgV2_createDateClient, PREImgV2_ext, c.ImgPathGet() + RealEstate_fName);

                //                                                                                    ERR = rs[0];
                //                                                                                    SMS = rs[1];
                //                                                                                    string RealEstateImageServerID = rs[2];
                //                                                                                    ExSMS = rs[3];

                //                                                                                    if (ERR != "Error")
                //                                                                                    {
                //                                                                                        LoanAppResImgList ilist = new LoanAppResImgList();
                //                                                                                        ilist.ImgType = "RealEstateImg";
                //                                                                                        ilist.ClientID = PREImgV2_imageClientID;
                //                                                                                        ilist.ServerID = RealEstateImageServerID;
                //                                                                                        ilist.OriImgName = PREImgV2_imgPath;
                //                                                                                        ilist.ServerImgName = RealEstate_fName;
                //                                                                                        ImgList.Add(ilist);
                //                                                                                    }
                //                                                                                }
                //                                                                                catch (Exception ex)
                //                                                                                {
                //                                                                                    ERR = "Error";
                //                                                                                    SMS = "Cannot read RealEstate image";
                //                                                                                    ExSMS = ex.Message.ToString();
                //                                                                                }
                //                                                                                #endregion sql
                //                                                                            }
                //                                                                            catch (Exception ex)
                //                                                                            {
                //                                                                                ERR = "Error";
                //                                                                                SMS = "Cannot read RealEstate image";
                //                                                                                ExSMS = ex.Message.ToString();
                //                                                                            }
                //                                                                        }
                //                                                                    }
                //                                                                }
                //                                                            }
                //                                                            catch (Exception ex)
                //                                                            {
                //                                                                ERR = "Error";
                //                                                                SMS = "Cannot read RealEstate image";
                //                                                                ExSMS = ex.Message.ToString();
                //                                                            }
                //                                                        }
                //                                                        #endregion img
                //                                                    }
                //                                                    catch (Exception ex)
                //                                                    {
                //                                                        ERR = "Error";
                //                                                        SMS = "Cannot read RealEstate";
                //                                                        ExSMS = ex.Message.ToString();
                //                                                    }
                //                                                }
                //                                            }
                //                                        }
                //                                        //if Error
                //                                        if (ERR == "Error")
                //                                        {
                //                                            LoanAppResSMS data = new LoanAppResSMS();
                //                                            data.SMS = "RealEstate: " + ExSMS;
                //                                            SMSList.Add(data);
                //                                        }
                //                                    }
                //                                    #endregion RealEstate

                //                                    #region PersonDeposit
                //                                    if (ERR != "Error")
                //                                    {
                //                                        try
                //                                        {

                //                                            if (Person.PersonDeposit != null)
                //                                            {
                //                                                foreach (var PersonDeposit in Person.PersonDeposit)
                //                                                {
                //                                                    if (ERR != "Error")
                //                                                    {
                //                                                        try
                //                                                        {
                //                                                            #region Para
                //                                                            string dep_CollateralClientID = PersonDeposit.CollateralClientID;
                //                                                            string deposit_collateralServerID = PersonDeposit.CollateralServerID;
                //                                                            string dep_LoanClientID = PersonDeposit.LoanClientID;
                //                                                            string deposit_loanAppID = PersonDeposit.LoanAppID;
                //                                                            string dep_CustClientID = PersonDeposit.CustClientID;
                //                                                            string deposit_custServerID = PersonDeposit.CustServerID;
                //                                                            string FixedDepositAccountNo = PersonDeposit.FixedDepositAccountNo;
                //                                                            string dep_StartDate = PersonDeposit.StartDate;
                //                                                            string dep_MaturityDate = PersonDeposit.MaturityDate;
                //                                                            string dep_Amount = PersonDeposit.Amount;
                //                                                            string dep_AccountOwnerName = PersonDeposit.AccountOwnerName;
                //                                                            string dep_Currency = PersonDeposit.Currency;
                //                                                            string dep_RelationshipID = PersonDeposit.RelationshipID;
                //                                                            string dep_DOB = PersonDeposit.DOB;
                //                                                            string dep_GenderID = PersonDeposit.GenderID;
                //                                                            string dep_NIDNo = PersonDeposit.NIDNo;
                //                                                            string dep_IssueDate = PersonDeposit.IssueDate;
                //                                                            string dep_IssuedBy = PersonDeposit.IssuedBy;
                //                                                            string dep_SortAddress = PersonDeposit.SortAddress;
                //                                                            string dep_VillageID = PersonDeposit.VillageID;
                //                                                            #endregion Para
                //                                                            #region sql
                //                                                            try
                //                                                            {
                //                                                                string[] rs = c.AddCustDepositFromDevice(dep_CollateralClientID, ServerLoanAppID
                //                                                                , dep_LoanClientID, ServerLoanAppPersonID, dep_CustClientID, FixedDepositAccountNo
                //                                                                , dep_StartDate, dep_MaturityDate, dep_Amount, dep_AccountOwnerName, dep_Currency
                //                                                                , dep_RelationshipID, dep_DOB, dep_GenderID, dep_NIDNo, dep_IssueDate, dep_IssuedBy
                //                                                                , dep_SortAddress, dep_VillageID);

                //                                                                ERR = rs[0];
                //                                                                SMS = rs[1];
                //                                                                string CollateralServerID = rs[2];
                //                                                                ExSMS = rs[3];

                //                                                            }
                //                                                            catch (Exception ex)
                //                                                            {
                //                                                                ERR = "Error";
                //                                                                SMS = "Cannot read Deposit";
                //                                                                ExSMS = ex.Message.ToString();
                //                                                            }
                //                                                            #endregion sql
                //                                                        }
                //                                                        catch (Exception ex)
                //                                                        {
                //                                                            ERR = "Error";
                //                                                            SMS = "Cannot read Deposit";
                //                                                            ExSMS = ex.Message.ToString();
                //                                                        }
                //                                                    }
                //                                                }
                //                                            }
                //                                        }
                //                                        catch (Exception ex)
                //                                        {
                //                                            ERR = "Error";
                //                                            SMS = "Cannot read Deposit";
                //                                            ExSMS = ex.Message.ToString();
                //                                        }
                //                                        //if Error
                //                                        if (ERR == "Error")
                //                                        {
                //                                            LoanAppResSMS data = new LoanAppResSMS();
                //                                            data.SMS = "PersonDeposit: " + ExSMS;
                //                                            SMSList.Add(data);
                //                                        }
                //                                    }
                //                                    #endregion PersonDeposit
                //                                }
                //                            }
                //                            catch (Exception ex)
                //                            {
                //                                ERR = "Error";
                //                                SMS = "Cannot read customer parameter 2 " + ex.Message.ToString();
                //                                ExSMS = ex.Message.ToString();
                //                            }

                //                            //if Error
                //                            if (ERR == "Error")
                //                            {
                //                                LoanAppResSMS data = new LoanAppResSMS();
                //                                data.SMS = "Person: " + ExSMS;
                //                                SMSList.Add(data);
                //                            }
                //                        }
                //                    }
                //                }

                //                #endregion Person

                //                #region Purpose
                //                if (ERR != "Error")
                //                {
                //                    try
                //                    {

                //                        if (loan.Purpose != null)
                //                        {
                //                            foreach (var Purpose in loan.Purpose)
                //                            {
                //                                if (ERR != "Error")
                //                                {
                //                                    try
                //                                    {
                //                                        string LoanPurposeServerID = "0";
                //                                        #region Para
                //                                        string LoanPurposeClientID = Purpose.LoanPurposeClientID;
                //                                        string purpose_loanPurposeServerID = Purpose.LoanPurposeServerID;
                //                                        string purpose_loanClientID = Purpose.LoanClientID;
                //                                        string purpose_loanAppID = Purpose.LoanAppID;
                //                                        string pur_LoanPurposeID = Purpose.LoanPurposeID;
                //                                        #endregion Para
                //                                        #region sql
                //                                        try
                //                                        {

                //                                            string[] rs = c.AddLoanApp11PurpsoeFromDevice(LoanPurposeClientID, ServerLoanAppID, pur_LoanPurposeID);

                //                                            ERR = rs[0];
                //                                            SMS = rs[1];
                //                                            LoanPurposeServerID = rs[2];
                //                                            ExSMS = rs[3];

                //                                        }
                //                                        catch (Exception ex)
                //                                        {
                //                                            ERR = "Error";
                //                                            SMS = "Cannot add customer image to switch DB";
                //                                            ExSMS = ex.Message.ToString();
                //                                        }
                //                                        #endregion sql

                //                                        #region Purpose Detail
                //                                        if (ERR != "Error")
                //                                        {
                //                                            try
                //                                            {

                //                                                if (Purpose.PurposeDetail != null)
                //                                                {
                //                                                    foreach (var PurposeDetail in Purpose.PurposeDetail)
                //                                                    {
                //                                                        if (ERR != "Error")
                //                                                        {
                //                                                            #region Para
                //                                                            string LoanPurposeDetailClientID = PurposeDetail.LoanPurposeDetailClientID;
                //                                                            string purposeDetail_loanPurposeDetailServerID = PurposeDetail.LoanPurposeDetailServerID;
                //                                                            string purposeDetail_loanPurposeClientID = PurposeDetail.LoanPurposeClientID;
                //                                                            string purposeDetail_loanPurposeServerID = PurposeDetail.LoanPurposeServerID;
                //                                                            string LoanAppPurpsoeDetail = PurposeDetail.LoanAppPurpsoeDetail;
                //                                                            string pur_Quantity = PurposeDetail.Quantity;
                //                                                            string pur_UnitPrice = PurposeDetail.UnitPrice;
                //                                                            #endregion Para
                //                                                            #region sql
                //                                                            try
                //                                                            {
                //                                                                string[] rs = c.AddLoanApp11PurpsoeDetailFromDevice(LoanPurposeDetailClientID, ServerLoanAppID, LoanAppPurpsoeDetail, pur_Quantity, pur_UnitPrice, LoanPurposeServerID);

                //                                                                ERR = rs[0];
                //                                                                SMS = rs[1];
                //                                                                string ServerLoanAppPurpsoeDetailID = rs[2];
                //                                                                ExSMS = rs[3];

                //                                                            }
                //                                                            catch (Exception ex)
                //                                                            {
                //                                                                ERR = "Error";
                //                                                                SMS = "Cannot read Purpose Detail";
                //                                                                ExSMS = ex.Message.ToString();
                //                                                            }
                //                                                            #endregion sql
                //                                                        }
                //                                                    }
                //                                                }
                //                                            }
                //                                            catch (Exception ex)
                //                                            {
                //                                                ERR = "Error";
                //                                                SMS = "Cannot read Purpose Detail";
                //                                                ExSMS = ex.Message.ToString();
                //                                            }
                //                                        }
                //                                        #endregion Purpose Detail
                //                                    }
                //                                    catch (Exception ex)
                //                                    {
                //                                        ERR = "Error";
                //                                        SMS = "Cannot read Purpose";
                //                                        ExSMS = ex.Message.ToString();
                //                                    }
                //                                }
                //                            }
                //                        }
                //                    }
                //                    catch (Exception ex)
                //                    {
                //                        ERR = "Error";
                //                        SMS = "Cannot read Purpose";
                //                        ExSMS = ex.Message.ToString();
                //                    }
                //                    //if Error
                //                    if (ERR == "Error")
                //                    {
                //                        LoanAppResSMS data = new LoanAppResSMS();
                //                        data.SMS = "Purpose: "+ExSMS;
                //                        SMSList.Add(data);
                //                    }
                //                }
                //                #endregion Purpose

                //                #region CashFlow
                //                if (ERR != "Error")
                //                {
                //                    try
                //                    {

                //                        if (loan.CashFlow != null)
                //                        {
                //                            foreach (var CashFlow in loan.CashFlow)
                //                            {
                //                                string ServerLoanAppCashFlowID = "0";
                //                                #region Para
                //                                string CashFlowClientID = CashFlow.CashFlowClientID;
                //                                string cashFlow_cashFlowServerID = CashFlow.CashFlowServerID;
                //                                string cashFlow_loanClientID = CashFlow.LoanClientID;
                //                                string cashFlow_loanAppID = CashFlow.LoanAppID;
                //                                string StudyMonthAmount = CashFlow.StudyMonthAmount;
                //                                string StudyStartMonth = CashFlow.StudyStartMonth;
                //                                string FamilyExpensePerMonth = CashFlow.FamilyExpensePerMonth;
                //                                string OtherExpensePerMonth = CashFlow.OtherExpensePerMonth;
                //                                #endregion Para
                //                                #region sql
                //                                try
                //                                {
                //                                    string[] rs = c.AddLoanApp51CashFlowFromDevice(ServerLoanAppID, CashFlowClientID
                //                                        , StudyMonthAmount, StudyStartMonth, FamilyExpensePerMonth, OtherExpensePerMonth);

                //                                    ERR = rs[0];
                //                                    SMS = rs[1];
                //                                    ServerLoanAppCashFlowID = rs[2];
                //                                    ExSMS = rs[3];

                //                                }
                //                                catch (Exception ex)
                //                                {
                //                                    ERR = "Error";
                //                                    SMS = "Cannot read cash flow";
                //                                    ExSMS = ex.Message.ToString();
                //                                }
                //                                #endregion sql

                //                                #region CashFlowMSI
                //                                if (ERR != "Error")
                //                                {
                //                                    try
                //                                    {

                //                                        if (CashFlow.CashFlowMSI != null)
                //                                        {
                //                                            foreach (var CashFlowMSI in CashFlow.CashFlowMSI)
                //                                            {
                //                                                string ServerLoanAppCashFlowMSIID = "0";
                //                                                if (ERR != "Error")
                //                                                {
                //                                                    #region Para
                //                                                    string cashFlowMSI_cashFlowMSIClientID = CashFlowMSI.CashFlowMSIClientID;
                //                                                    string cashFlowMSI_cashFlowMSIServerID = CashFlowMSI.CashFlowMSIServerID;
                //                                                    string cashFlowMSI_cashFlowClientID = CashFlowMSI.CashFlowClientID;
                //                                                    string cashFlowMSI_cashFlowServerID = CashFlowMSI.CashFlowServerID;
                //                                                    string IncomeTypeID = CashFlowMSI.IncomeTypeID;
                //                                                    string MSI_OccupationTypeID = CashFlowMSI.OccupationTypeID;
                //                                                    string MainSourceIncomeID = CashFlowMSI.MainSourceIncomeID;
                //                                                    string MSI_ExAge = CashFlowMSI.ExAge;
                //                                                    string MSI_BusAge = CashFlowMSI.BusAge;
                //                                                    string isMSI = CashFlowMSI.isMSI;


                //                                                    #endregion Para
                //                                                    #region sql
                //                                                    try
                //                                                    {
                //                                                        string[] rs = c.AddLoanAppCashFlowMSIFromDevice(ServerLoanAppCashFlowID, IncomeTypeID
                //                                                            , MainSourceIncomeID, "", "0", MSI_ExAge, MSI_BusAge, isMSI, MSI_OccupationTypeID);

                //                                                        ERR = rs[0];
                //                                                        SMS = rs[1];
                //                                                        ServerLoanAppCashFlowMSIID = rs[2];
                //                                                        ExSMS = rs[3];

                //                                                    }
                //                                                    catch (Exception ex)
                //                                                    {
                //                                                        ERR = "Error";
                //                                                        SMS = "Cannot read cash flow";
                //                                                        ExSMS = ex.Message.ToString();
                //                                                    }
                //                                                    #endregion sql

                //                                                    #region CashFlowMSIInEx
                //                                                    if (ERR != "Error")
                //                                                    {
                //                                                        try
                //                                                        {

                //                                                            if (CashFlowMSI.CashFlowMSIInEx != null)
                //                                                            {
                //                                                                foreach (var CashFlowMSIInEx in CashFlowMSI.CashFlowMSIInEx)
                //                                                                {
                //                                                                    if (ERR != "Error")
                //                                                                    {
                //                                                                        #region Para
                //                                                                        string LoanAppCashFlowMSIInExClientID = CashFlowMSIInEx.LoanAppCashFlowMSIInExClientID;
                //                                                                        string mSIInEx_loanAppCashFlowMSIInExServerID = CashFlowMSIInEx.LoanAppCashFlowMSIInExServerID;
                //                                                                        string mSIInEx_cashFlowMSIClientID = CashFlowMSIInEx.CashFlowMSIClientID;
                //                                                                        string mSIInEx_cashFlowMSIServerID = CashFlowMSIInEx.CashFlowMSIServerID;
                //                                                                        string InExCodeID = CashFlowMSIInEx.InExCodeID;
                //                                                                        string MSIInEx_Description = CashFlowMSIInEx.Description;
                //                                                                        string MSIInEx_Month = CashFlowMSIInEx.Month;
                //                                                                        string MSIInEx_Mount = CashFlowMSIInEx.Amount;
                //                                                                        string MSIInEx_UnitID = CashFlowMSIInEx.UnitID;
                //                                                                        string MSIInEx_Cost = CashFlowMSIInEx.Cost;
                //                                                                        string OneIncomeTwoExpense = CashFlowMSIInEx.OneIncomeTwoExpense;
                //                                                                        #endregion Para
                //                                                                        #region sql
                //                                                                        try
                //                                                                        {

                //                                                                            string[] rs = c.AddLoanAppCashFlowMSIInExFromDevice(LoanAppCashFlowMSIInExClientID
                //                                                                                , ServerLoanAppCashFlowMSIID, InExCodeID, MSIInEx_Description, MSIInEx_Month
                //                                                                                , MSIInEx_Mount, MSIInEx_UnitID, MSIInEx_Cost, OneIncomeTwoExpense);

                //                                                                            ERR = rs[0];
                //                                                                            SMS = rs[1];
                //                                                                            string ServerLoanAppCashFlowMSIInExID = rs[2];
                //                                                                            ExSMS = rs[3];

                //                                                                        }
                //                                                                        catch (Exception ex)
                //                                                                        {
                //                                                                            ERR = "Error";
                //                                                                            SMS = "Cannot read cash flow msi InEx";
                //                                                                            ExSMS = ex.Message.ToString();
                //                                                                        }
                //                                                                        #endregion sql
                //                                                                    }
                //                                                                }
                //                                                            }

                //                                                        }
                //                                                        catch (Exception ex)
                //                                                        {
                //                                                            ERR = "Error";
                //                                                            SMS = "Cannot read cash flow msi InEx";
                //                                                            ExSMS = ex.Message.ToString();
                //                                                        }
                //                                                    }
                //                                                    #endregion CashFlowMSIInEx
                //                                                }
                //                                            }
                //                                        }
                //                                    }
                //                                    catch (Exception ex)
                //                                    {
                //                                        ERR = "Error";
                //                                        SMS = "Cannot read cash flow msi";
                //                                        ExSMS = ex.Message.ToString();
                //                                    }
                //                                }
                //                                #endregion CashFlowMSI
                //                            }
                //                        }
                //                    }
                //                    catch (Exception ex)
                //                    {
                //                        ERR = "Error";
                //                        SMS = "Cannot read cash flow";
                //                        ExSMS = ex.Message.ToString();
                //                    }
                //                    //if Error
                //                    if (ERR == "Error")
                //                    {
                //                        LoanAppResSMS data = new LoanAppResSMS();
                //                        data.SMS = "CashFlow: " + ExSMS;
                //                        SMSList.Add(data);
                //                    }
                //                }
                //                #endregion CashFlow

                //                #region Update Ori CO
                //                if (ERR != "Error")
                //                {
                //                    try
                //                    {
                //                        string sql = "exec T24_LoanAppUpdateOriCO @VBID='" + VillageBankIDForGetCOIDOri + "',@LoanAppID='" + ServerLoanAppID + "'";
                //                        DataTable dt2 = c.ReturnDT(sql);
                //                        if (dt2.Rows[0][0].ToString() == "0")
                //                        {
                //                            ERR = "Error";
                //                            SMS = dt2.Rows[0][1].ToString();
                //                        }
                //                    }
                //                    catch (Exception ex)
                //                    {
                //                        ERR = "Error";
                //                        SMS = "Cannot Error Update Ori CO";
                //                        ExSMS = ex.Message.ToString();
                //                    }
                //                    //if Error
                //                    if (ERR == "Error")
                //                    {
                //                        LoanAppResSMS data = new LoanAppResSMS();
                //                        data.SMS = "UpdateOriCO: " + ExSMS;
                //                        SMSList.Add(data);
                //                    }
                //                }
                //                #endregion

                //            }
                //        }
                //    }
                //    catch { }
                //}
                //#region old
                ////if (ERR != "Error")
                ////{
                ////    #region add
                ////    if (SMSList.Count > 0)
                ////    {
                ////        ERR = "Error";
                ////        SMS = "Something was wrong in LoanApp";
                ////    }
                ////    else
                ////    {
                ////        string ServerLoanAppID = "0";
                ////        SqlConnection Con1 = new SqlConnection(c.ConStr());
                ////        Con1.Open();
                ////        SqlCommand Com1 = new SqlCommand();
                ////        Com1.Connection = Con1;
                ////        string sql = "";
                ////        try
                ////        {
                ////            foreach (var loan in jObj.LoanApp)
                ////            {
                ////                #region LoanApp 
                ////                #region LoanApp Param                           
                ////                string IDOnDevice = loan.IDOnDevice;
                ////                RSIDOnDevice = IDOnDevice;
                ////                //string LoanAppID = loan.LoanAppID;
                ////                string LoanAppID = "0";
                ////                //RSLoanAppID = LoanAppID;
                ////                //string LoanAppStatusID = loan.LoanAppStatusID;
                ////                string LoanAppStatusID = "3";
                ////                string DeviceDate = loan.DeviceDate;
                ////                string ProductID = loan.ProductID;
                ////                string LoanRequestAmount = loan.LoanRequestAmount;
                ////                string LoanPurposeID1 = loan.LoanPurposeID1;
                ////                string LoanPurposeID2 = loan.LoanPurposeID2;
                ////                string LoanPurposeID3 = loan.LoanPurposeID3;
                ////                string OwnCapital = loan.OwnCapital;
                ////                string DisbursementDate = loan.DisbursementDate;
                ////                string FirstWithdrawal = loan.FirstWithdrawal;
                ////                string LoanTerm = loan.LoanTerm;
                ////                string FirstRepaymentDate = loan.FirstRepaymentDate;
                ////                string LoanInterestRate = loan.LoanInterestRate;
                ////                string CustomerRequestRate = loan.CustomerRequestRate;
                ////                string CompititorRate = loan.CompititorRate;
                ////                string CustomerConditionID = loan.CustomerConditionID;
                ////                string COProposedAmount = loan.COProposedAmount;
                ////                string COProposedTerm = loan.COProposedTerm;
                ////                string COProposeRate = loan.COProposeRate;
                ////                string FrontBackOfficeID = loan.FrontBackOfficeID;
                ////                string GroupNumber = loan.GroupNumber;
                ////                string LoanCycleID = loan.LoanCycleID;
                ////                string RepaymentHistoryID = loan.RepaymentHistoryID;
                ////                string LoanReferralID = loan.LoanReferralID;
                ////                string DebtIinfoID = loan.DebtIinfoID;
                ////                string MonthlyFee = loan.MonthlyFee;
                ////                string Compulsory = loan.Compulsory;
                ////                //string CompulsoryTerm = loan.CompulsoryTerm;
                ////                string CompulsoryTerm = "0";
                ////                string Currency = loan.Currency;
                ////                string UpFrontFee = loan.UpFrontFee;
                ////                string UpFrontAmt = loan.UpFrontAmt;
                ////                //string isCBCCheck = loan.isCBCCheck;
                ////                string CompulsoryOptionID = loan.CompulsoryOptionID;
                ////                string FundSource = loan.FundSource;
                ////                string IsNewCollateral = loan.IsNewCollateral;
                ////                #endregion LoanApp Param                                
                ////                #region sql add loan
                ////                sql = "exec sp_LoanApp1Add2 @IDOnDevice=@IDOnDevice,@LoanAppID=@LoanAppID,@LoanAppStatusID=@LoanAppStatusID,@DeviceDate=@DeviceDate"
                ////                + ",@ServerDate=@ServerDate,@CreateBy=@CreateBy,@ProductID=@ProductID,@LoanRequestAmount=@LoanRequestAmount"
                ////                + ",@LoanPurposeID1=@LoanPurposeID1,@LoadPurposeID2=@LoadPurposeID2,@LoadPurposeID3=@LoadPurposeID3,@OwnCapital=@OwnCapital"
                ////                + ",@DisbursementDate=@DisbursementDate,@FirstWithdrawal=@FirstWithdrawal,@LoanTerm=@LoanTerm,@FirstRepaymentDate=@FirstRepaymentDate"
                ////                + ",@LoanInterestRate=@LoanInterestRate,@CustomerRequestRate=@CustomerRequestRate,@CompititorRate=@CompititorRate"
                ////                + ",@CustomerConditionID=@CustomerConditionID,@COProposedAmount=@COProposedAmount,@COProposedTerm=@COProposedTerm"
                ////                + ",@COProposeRate=@COProposeRate,@FrontBackOfficeID=@FrontBackOfficeID,@GroupNumber=@GroupNumber,@LoanCycleID=@LoanCycleID"
                ////                + ",@RepaymentHistoryID=@RepaymentHistoryID,@LoanReferralID=@LoanReferralID,@DebtIinfoID=@DebtIinfoID,@MonthlyFee=@MonthlyFee"
                ////                + ",@Compulsory=@Compulsory,@CompulsoryTerm=@CompulsoryTerm,@Currency=@Currency,@UpFrontFee=@UpFrontFee"
                ////                + ",@UpFrontAmt=@UpFrontAmt,@CompulsoryOptionID=@CompulsoryOptionID,@isCBCCheck=@isCBCCheck,@FundSource=@FundSource"
                ////                + ",@IsNewCollateral=@IsNewCollateral";
                ////                Com1.CommandText = sql;
                ////                Com1.Parameters.Clear();
                ////                Com1.Parameters.AddWithValue("@IDOnDevice", IDOnDevice);
                ////                Com1.Parameters.AddWithValue("@LoanAppID", LoanAppID);
                ////                Com1.Parameters.AddWithValue("@LoanAppStatusID", LoanAppStatusID);
                ////                Com1.Parameters.AddWithValue("@DeviceDate", DeviceDate);
                ////                Com1.Parameters.AddWithValue("@ServerDate", ServerDate);
                ////                Com1.Parameters.AddWithValue("@CreateBy", UserID);
                ////                Com1.Parameters.AddWithValue("@ProductID", ProductID);
                ////                Com1.Parameters.AddWithValue("@LoanRequestAmount", LoanRequestAmount);
                ////                Com1.Parameters.AddWithValue("@LoanPurposeID1", LoanPurposeID1);
                ////                Com1.Parameters.AddWithValue("@LoadPurposeID2", LoanPurposeID2);
                ////                Com1.Parameters.AddWithValue("@LoadPurposeID3", LoanPurposeID3);
                ////                Com1.Parameters.AddWithValue("@OwnCapital", OwnCapital);
                ////                Com1.Parameters.AddWithValue("@DisbursementDate", DisbursementDate);
                ////                Com1.Parameters.AddWithValue("@FirstWithdrawal", FirstWithdrawal);
                ////                Com1.Parameters.AddWithValue("@LoanTerm", LoanTerm);
                ////                Com1.Parameters.AddWithValue("@FirstRepaymentDate", FirstRepaymentDate);
                ////                Com1.Parameters.AddWithValue("@LoanInterestRate", LoanInterestRate);
                ////                Com1.Parameters.AddWithValue("@CustomerRequestRate", CustomerRequestRate);
                ////                Com1.Parameters.AddWithValue("@CompititorRate", CompititorRate);
                ////                Com1.Parameters.AddWithValue("@CustomerConditionID", CustomerConditionID);
                ////                Com1.Parameters.AddWithValue("@COProposedAmount", COProposedAmount);
                ////                Com1.Parameters.AddWithValue("@COProposedTerm", COProposedTerm);
                ////                Com1.Parameters.AddWithValue("@COProposeRate", COProposeRate);
                ////                Com1.Parameters.AddWithValue("@FrontBackOfficeID", FrontBackOfficeID);
                ////                Com1.Parameters.AddWithValue("@GroupNumber", GroupNumber);
                ////                Com1.Parameters.AddWithValue("@LoanCycleID", LoanCycleID);
                ////                Com1.Parameters.AddWithValue("@RepaymentHistoryID", RepaymentHistoryID);
                ////                Com1.Parameters.AddWithValue("@LoanReferralID", LoanReferralID);
                ////                Com1.Parameters.AddWithValue("@DebtIinfoID", DebtIinfoID);
                ////                Com1.Parameters.AddWithValue("@MonthlyFee", MonthlyFee);
                ////                Com1.Parameters.AddWithValue("@Compulsory", Compulsory);
                ////                Com1.Parameters.AddWithValue("@CompulsoryTerm", CompulsoryTerm);
                ////                Com1.Parameters.AddWithValue("@Currency", Currency);
                ////                Com1.Parameters.AddWithValue("@UpFrontFee", UpFrontFee);
                ////                Com1.Parameters.AddWithValue("@UpFrontAmt", UpFrontAmt);
                ////                Com1.Parameters.AddWithValue("@CompulsoryOptionID", CompulsoryOptionID);
                ////                Com1.Parameters.AddWithValue("@isCBCCheck", "1");
                ////                Com1.Parameters.AddWithValue("@FundSource", FundSource);
                ////                Com1.Parameters.AddWithValue("@IsNewCollateral", IsNewCollateral);
                ////                DataTable dt1 = new DataTable();
                ////                dt1.Load(Com1.ExecuteReader());
                ////                ServerLoanAppID = dt1.Rows[0][0].ToString();
                ////                //ServerLoanAppID = "1";
                ////                RSLoanAppID = ServerLoanAppID;
                ////                #endregion sql
                ////                #region update Inst
                ////                try
                ////                {
                ////                    if (InstID.Length > 1)
                ////                    {
                ////                        InstID = "1";
                ////                    }
                ////                    c.ReturnDT("update tblLoanApp1 set InstID='" + InstID + "' where LoanAppID='" + ServerLoanAppID + "'");
                ////                }
                ////                catch { }
                ////                #endregion update Inst
                ////                #endregion LoanApp
                ////                #region PurpsoeDetail
                ////                foreach (var lp in loan.PurposeDetail)
                ////                {
                ////                    #region param
                ////                    string LoanAppPurpsoeDetail = lp.LoanAppPurposeDetail;
                ////                    string Quantity = lp.Quantity;
                ////                    string UnitPrice = lp.UnitPrice;
                ////                    #endregion param
                ////                    #region sql
                ////                    sql = "exec sp_LoanApp11Add2 @IDOnDevice=@IDOnDevice,@LoanAppID=@LoanAppID,@LoanAppPurpsoeDetail=@LoanAppPurpsoeDetail"
                ////                    + ",@Quantity=@Quantity,@UnitPrice=@UnitPrice";
                ////                    Com1.CommandText = sql;
                ////                    Com1.Parameters.Clear();
                ////                    Com1.Parameters.AddWithValue("@IDOnDevice", IDOnDevice);
                ////                    Com1.Parameters.AddWithValue("@LoanAppID", ServerLoanAppID);
                ////                    Com1.Parameters.AddWithValue("@LoanAppPurpsoeDetail", LoanAppPurpsoeDetail);
                ////                    Com1.Parameters.AddWithValue("@Quantity", Quantity);
                ////                    Com1.Parameters.AddWithValue("@UnitPrice", UnitPrice);
                ////                    Com1.ExecuteNonQuery();
                ////                    #endregion sql
                ////                }
                ////                #endregion PurpsoeDetail
                ////                #region Person
                ////                foreach (var lp in loan.Person)
                ////                {
                ////                    #region Person
                ////                    string ServerLoanAppPersonID = "";
                ////                    #region Person Param
                ////                    string LoanAppPersonID = lp.LoanAppPersonID;
                ////                    string LoanAppPersonTypeID = lp.LoanAppPersonTypeID;
                ////                    string PersonID = lp.PersonID;
                ////                    string CustomerID = lp.CustomerID;
                ////                    string Number = lp.Number;
                ////                    string VillageBankID = lp.VillageBankID;
                ////                    string AltName = lp.AltName;
                ////                    string TitleID = lp.TitleID;
                ////                    string LastName = lp.LastName;
                ////                    string FirstName = lp.FirstName;
                ////                    string GenderID = lp.GenderID;
                ////                    if (GenderID == "MALE")
                ////                    {
                ////                        GenderID = "0";
                ////                    }
                ////                    if (GenderID == "FEMALE")
                ////                    {
                ////                        GenderID = "1";
                ////                    }
                ////                    string DateOfBirth = lp.DateOfBirth;
                ////                    string IDCardTypeID = lp.IDCardTypeID;
                ////                    string IDCardNumber = lp.IDCardNumber;
                ////                    string IDCardExpireDate = lp.IDCardExpireDate;
                ////                    string IDCardIssuedDate = lp.IDCardIssuedDate;
                ////                    string MaritalStatusID = lp.MaritalStatusID;
                ////                    string EducationID = lp.EducationID;
                ////                    string CityOfBirthID = lp.CityOfBirthID;
                ////                    string Telephone3 = lp.Telephone3;
                ////                    string VillageIDPermanent = lp.VillageIDPermanent;
                ////                    string LocationCodeIDPermanent = lp.LocationCodeIDPermanent;
                ////                    string VillageIDCurrent = lp.VillageIDCurrent;
                ////                    string LocationCodeIDCurrent = lp.LocationCodeIDCurrent;
                ////                    string SortAddress = lp.SortAddress;
                ////                    string FamilyMember = lp.FamilyMember;
                ////                    string FamilyMemberActive = lp.FamilyMemberActive;
                ////                    string PoorID = lp.PoorID;
                ////                    string DeviceDate_Person = lp.DeviceDate;
                ////                    string AltName2 = lp.AltName2;
                ////                    #endregion Person Param
                ////                    #region sql
                ////                    sql = "exec T24_LoanAppPersonAddEdit @IDOnDevice=@IDOnDevice,@LoanAppID=@LoanAppID,@LoanAppPersonTypeID=@LoanAppPersonTypeID"
                ////                    + ",@PersonID=@PersonID,@CustomerID=@CustomerID,@Number=@Number,@VillageBankID=@VillageBankID,@AltName=@AltName,@TitleID=@TitleID"
                ////                    + ",@LastName=@LastName,@FirstName=@FirstName,@FullName=@FullName,@GenderID=@GenderID,@DateOfBirth=@DateOfBirth"
                ////                    + ",@IDCardTypeID=@IDCardTypeID,@IDCardNumber=@IDCardNumber,@IDCardExpireDate=@IDCardExpireDate,@MaritalStatusID=@MaritalStatusID"
                ////                    + ",@EducationID=@EducationID,@CityOfBirthID=@CityOfBirthID,@Telephone3=@Telephone3,@BranchID=@BranchID"
                ////                    + ",@VillageIDPermanent=@VillageIDPermanent"
                ////                    + ",@LocationCodeIDPermanent=@LocationCodeIDPermanent"
                ////                    + ",@VillageIDCurrent=@VillageIDCurrent,@LocationCodeIDCurrent=@LocationCodeIDCurrent"
                ////                    + ",@SortAddress=@SortAddress,@FamilyMember=@FamilyMember,@FamilyMemberActive=@FamilyMemberActive,@PoorID=@PoorID"
                ////                    + ",@OfficeNameID=@OfficeNameID,@LoanAppPersonIDOnDevice=@LoanAppPersonIDOnDevice,@DeviceDate=@DeviceDate"
                ////                    + ",@IDCardIssuedDate=@IDCardIssuedDate,@AltName2=@AltName2";
                ////                    Com1.CommandText = sql;
                ////                    Com1.Parameters.Clear();
                ////                    Com1.Parameters.AddWithValue("@IDOnDevice", IDOnDevice);
                ////                    Com1.Parameters.AddWithValue("@LoanAppID", ServerLoanAppID);
                ////                    Com1.Parameters.AddWithValue("@LoanAppPersonTypeID", LoanAppPersonTypeID);
                ////                    Com1.Parameters.AddWithValue("@PersonID", PersonID);
                ////                    Com1.Parameters.AddWithValue("@CustomerID", CustomerID);
                ////                    Com1.Parameters.AddWithValue("@Number", Number);
                ////                    Com1.Parameters.AddWithValue("@VillageBankID", VillageBankID);
                ////                    Com1.Parameters.AddWithValue("@AltName", AltName);
                ////                    Com1.Parameters.AddWithValue("@TitleID", TitleID);
                ////                    Com1.Parameters.AddWithValue("@LastName", LastName);
                ////                    Com1.Parameters.AddWithValue("@FirstName", FirstName);
                ////                    Com1.Parameters.AddWithValue("@FullName", "");
                ////                    Com1.Parameters.AddWithValue("@GenderID", GenderID);
                ////                    Com1.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
                ////                    Com1.Parameters.AddWithValue("@IDCardTypeID", IDCardTypeID);
                ////                    Com1.Parameters.AddWithValue("@IDCardNumber", IDCardNumber);
                ////                    Com1.Parameters.AddWithValue("@IDCardExpireDate", IDCardExpireDate);
                ////                    Com1.Parameters.AddWithValue("@MaritalStatusID", MaritalStatusID);
                ////                    Com1.Parameters.AddWithValue("@EducationID", EducationID);
                ////                    Com1.Parameters.AddWithValue("@CityOfBirthID", CityOfBirthID);
                ////                    Com1.Parameters.AddWithValue("@Telephone3", Telephone3);
                ////                    Com1.Parameters.AddWithValue("@BranchID", "0");
                ////                    Com1.Parameters.AddWithValue("@VillageIDPermanent", VillageIDPermanent);
                ////                    Com1.Parameters.AddWithValue("@LocationCodeIDPermanent", LocationCodeIDPermanent);
                ////                    Com1.Parameters.AddWithValue("@VillageIDCurrent", VillageIDCurrent);
                ////                    Com1.Parameters.AddWithValue("@LocationCodeIDCurrent", LocationCodeIDCurrent);
                ////                    Com1.Parameters.AddWithValue("@SortAddress", SortAddress);
                ////                    Com1.Parameters.AddWithValue("@FamilyMember", FamilyMember);
                ////                    Com1.Parameters.AddWithValue("@FamilyMemberActive", FamilyMemberActive);
                ////                    Com1.Parameters.AddWithValue("@PoorID", PoorID);
                ////                    Com1.Parameters.AddWithValue("@OfficeNameID", "0");
                ////                    Com1.Parameters.AddWithValue("@LoanAppPersonIDOnDevice", LoanAppPersonID);
                ////                    Com1.Parameters.AddWithValue("@DeviceDate", DeviceDate);
                ////                    Com1.Parameters.AddWithValue("@IDCardIssuedDate", IDCardIssuedDate);
                ////                    Com1.Parameters.AddWithValue("@AltName2", AltName2);
                ////                    DataTable dt2 = new DataTable();
                ////                    dt2.Load(Com1.ExecuteReader());
                ////                    ServerLoanAppPersonID = dt2.Rows[0][0].ToString();
                ////                    //ServerLoanAppPersonID = "1";
                ////                    #endregion sql

                ////                    #endregion Person

                ////                    #region Creditor 
                ////                    foreach (var lp2 in lp.Creditor)
                ////                    {
                ////                        #region Parama
                ////                        string CreditorID = lp2.CreditorID;
                ////                        string ApprovedAmount = lp2.ApprovedAmount;
                ////                        string OutstandingBalance = lp2.OutstandingBalance;
                ////                        string InterestRate = lp2.InterestRate;
                ////                        string RepaymentTypeID = lp2.RepaymentTypeID;
                ////                        string RepaymentTermID = lp2.RepaymentTermID;
                ////                        string LoanStartDate = lp2.LoanStartDate;
                ////                        string LoanEndDate = lp2.LoanEndDate;
                ////                        string RemainingInstallment = lp2.RemainingInstallment;
                ////                        string LoanAppCreditorID = lp2.LoanAppCreditorID;
                ////                        #endregion Parama
                ////                        #region sql
                ////                        sql = "exec T24_LoanAppCreditorAddEdit @ServerLoanAppID=@ServerLoanAppID,@ServerLoanAppPersonID=@ServerLoanAppPersonID"
                ////                        + ",@CreditorID=@CreditorID,@ApprovedAmount=@ApprovedAmount,@OutstandingBalance=@OutstandingBalance,@InterestRate=@InterestRate"
                ////                        + ",@RepaymentTypeID=@RepaymentTypeID,@RepaymentTermID=@RepaymentTermID,@LoanStartDate=@LoanStartDate"
                ////                        + ",@LoanEndDate=@LoanEndDate,@RemainingInstallment=@RemainingInstallment";
                ////                        Com1.CommandText = sql;
                ////                        Com1.Parameters.Clear();
                ////                        Com1.Parameters.AddWithValue("@ServerLoanAppID", ServerLoanAppID);
                ////                        Com1.Parameters.AddWithValue("@ServerLoanAppPersonID", ServerLoanAppPersonID);
                ////                        Com1.Parameters.AddWithValue("@CreditorID", CreditorID);
                ////                        Com1.Parameters.AddWithValue("@ApprovedAmount", ApprovedAmount);
                ////                        Com1.Parameters.AddWithValue("@OutstandingBalance", OutstandingBalance);
                ////                        Com1.Parameters.AddWithValue("@InterestRate", InterestRate);
                ////                        Com1.Parameters.AddWithValue("@RepaymentTypeID", RepaymentTypeID);
                ////                        Com1.Parameters.AddWithValue("@RepaymentTermID", RepaymentTermID);
                ////                        Com1.Parameters.AddWithValue("@LoanStartDate", LoanStartDate);
                ////                        Com1.Parameters.AddWithValue("@LoanEndDate", LoanEndDate);
                ////                        Com1.Parameters.AddWithValue("@RemainingInstallment", RemainingInstallment);
                ////                        Com1.ExecuteNonQuery();
                ////                        #endregion sql
                ////                    }
                ////                    #endregion Creditor 
                ////                    #region ClientAsset
                ////                    foreach (var lp2 in lp.ClientAsset)
                ////                    {
                ////                        #region Param
                ////                        string Description = lp2.Description;
                ////                        string Quantity = lp2.Quantity;
                ////                        string UnitPrice = lp2.UnitPrice;
                ////                        #endregion Param
                ////                        #region sql
                ////                        sql = "exec T24_LoanAppClientAssetAddEdit @ServerLoanAppPersonID=@ServerLoanAppPersonID,@ServerLoanAppID=@ServerLoanAppID"
                ////                        + ",@Description=@Description,@Quantity=@Quantity,@UnitPrice=@UnitPrice";
                ////                        Com1.CommandText = sql;
                ////                        Com1.Parameters.Clear();
                ////                        Com1.Parameters.AddWithValue("@ServerLoanAppID", ServerLoanAppID);
                ////                        Com1.Parameters.AddWithValue("@ServerLoanAppPersonID", ServerLoanAppPersonID);
                ////                        Com1.Parameters.AddWithValue("@Description", Description);
                ////                        Com1.Parameters.AddWithValue("@Quantity", Quantity);
                ////                        Com1.Parameters.AddWithValue("@UnitPrice", UnitPrice);
                ////                        Com1.ExecuteNonQuery();
                ////                        #endregion sql
                ////                    }
                ////                    #endregion ClientAsset
                ////                    #region ClientBusiness
                ////                    foreach (var lp2 in lp.ClientBusiness)
                ////                    {
                ////                        #region Param
                ////                        string Description = lp2.Description;
                ////                        string Quantity = lp2.Quantity;
                ////                        string UnitPrice = lp2.UnitPrice;
                ////                        #endregion Param
                ////                        #region sql
                ////                        sql = "exec T24_LoanAppClientBusinessAddEdit @ServerLoanAppPersonID=@ServerLoanAppPersonID,@ServerLoanAppID=@ServerLoanAppID"
                ////                        + ",@Description=@Description,@Quantity=@Quantity,@UnitPrice=@UnitPrice";
                ////                        Com1.CommandText = sql;
                ////                        Com1.Parameters.Clear();
                ////                        Com1.Parameters.AddWithValue("@ServerLoanAppID", ServerLoanAppID);
                ////                        Com1.Parameters.AddWithValue("@ServerLoanAppPersonID", ServerLoanAppPersonID);
                ////                        Com1.Parameters.AddWithValue("@Description", Description);
                ////                        Com1.Parameters.AddWithValue("@Quantity", Quantity);
                ////                        Com1.Parameters.AddWithValue("@UnitPrice", UnitPrice);
                ////                        Com1.ExecuteNonQuery();
                ////                        #endregion sql
                ////                    }
                ////                    #endregion ClientBusiness
                ////                    #region ClientCollateral
                ////                    foreach (var lp2 in lp.ClientCollateral)
                ////                    {
                ////                        #region Param
                ////                        string ServerLoanAppClientCollateralID = "";
                ////                        string LoanAppClientCollateralID = lp2.LoanAppClientCollateralID;
                ////                        string ColleteralTypeID = lp2.ColleteralTypeID;
                ////                        string ColleteralDocTypeID = lp2.ColleteralDocTypeID;
                ////                        string ColleteralDocNumber = lp2.ColleteralDocNumber;
                ////                        string Description = lp2.Description;
                ////                        string Quantity = lp2.Quantity;
                ////                        string UnitPrice = lp2.UnitPrice;
                ////                        #endregion Param
                ////                        #region sql
                ////                        sql = "exec T24_LoanAppClientCollateralAddEdit @ServerLoanAppPersonID=@ServerLoanAppPersonID,@ServerLoanAppID=@ServerLoanAppID"
                ////                        + ",@ColleteralTypeID=@ColleteralTypeID,@ColleteralDocTypeID=@ColleteralDocTypeID"
                ////                        + ",@ColleteralDocNumber=@ColleteralDocNumber,@Description=@Description,@Quantity=@Quantity,@UnitPrice=@UnitPrice";
                ////                        Com1.CommandText = sql;
                ////                        Com1.Parameters.Clear();
                ////                        Com1.Parameters.AddWithValue("@ServerLoanAppPersonID", ServerLoanAppPersonID);
                ////                        Com1.Parameters.AddWithValue("@ServerLoanAppID", ServerLoanAppID);
                ////                        Com1.Parameters.AddWithValue("@ColleteralTypeID", ColleteralTypeID);
                ////                        Com1.Parameters.AddWithValue("@ColleteralDocTypeID", ColleteralDocTypeID);
                ////                        Com1.Parameters.AddWithValue("@ColleteralDocNumber", ColleteralDocNumber);
                ////                        Com1.Parameters.AddWithValue("@Description", Description);
                ////                        Com1.Parameters.AddWithValue("@Quantity", Quantity);
                ////                        Com1.Parameters.AddWithValue("@UnitPrice", UnitPrice);
                ////                        DataTable dt3 = new DataTable();
                ////                        dt3.Load(Com1.ExecuteReader());
                ////                        ServerLoanAppClientCollateralID = dt3.Rows[0][0].ToString();
                ////                        //ServerLoanAppClientCollateralID = "1";
                ////                        #endregion sql

                ////                        #region ClientCollateralImg
                ////                        foreach (var lp3 in lp2.ClientCollateralImg)
                ////                        {
                ////                            string ImgName = lp3.ImgName;
                ////                            string Ext = lp3.Ext;

                ////                            sql = "exec T24_LoanAppClientCollateralImageAdd @ServerLoanAppID=@ServerLoanAppID,@ServerLoanAppPersonID=@ServerLoanAppPersonID"
                ////                            + ",@ServerLoanAppClientCollateralID=@ServerLoanAppClientCollateralID,@Ext=@Ext,@Remark=@Remark";
                ////                            Com1.CommandText = sql;
                ////                            Com1.Parameters.Clear();
                ////                            Com1.Parameters.AddWithValue("@ServerLoanAppID", ServerLoanAppID);
                ////                            Com1.Parameters.AddWithValue("@ServerLoanAppPersonID", ServerLoanAppPersonID);
                ////                            Com1.Parameters.AddWithValue("@ServerLoanAppClientCollateralID", ServerLoanAppClientCollateralID);
                ////                            Com1.Parameters.AddWithValue("@Ext", Ext);
                ////                            Com1.Parameters.AddWithValue("@Remark", ImgName);
                ////                            DataTable dt4 = new DataTable();
                ////                            dt4.Load(Com1.ExecuteReader());
                ////                            string fname = dt4.Rows[0][0].ToString();
                ////                            //string fname = ImgName + "_1";
                ////                            LoanAppResImgList data = new LoanAppResImgList();
                ////                            data.OriImgName = ImgName;
                ////                            data.ServerImgName = fname;
                ////                            ImgList.Add(data);
                ////                        }
                ////                        #endregion ClientCollateralImg
                ////                    }
                ////                    #endregion ClientCollateral
                ////                    #region GuarantorBusiness
                ////                    foreach (var lp2 in lp.GuarantorBusiness)
                ////                    {
                ////                        string Description = lp2.Description;
                ////                        string NetProfitPerYear = lp2.NetProfitPerYear;
                ////                        //
                ////                        sql = "exec T24_LoanAppGuarantorBusinessAddEdit @ServerLoanAppID=@ServerLoanAppID,@ServerLoanAppPersonID=@ServerLoanAppPersonID"
                ////                        + ",@Description=@Description,@NetProfitPerYear=@NetProfitPerYear";
                ////                        Com1.CommandText = sql;
                ////                        Com1.Parameters.Clear();
                ////                        Com1.Parameters.AddWithValue("@ServerLoanAppID", ServerLoanAppID);
                ////                        Com1.Parameters.AddWithValue("@ServerLoanAppPersonID", ServerLoanAppPersonID);
                ////                        Com1.Parameters.AddWithValue("@Description", Description);
                ////                        Com1.Parameters.AddWithValue("@NetProfitPerYear", NetProfitPerYear);
                ////                        Com1.ExecuteNonQuery();
                ////                    }
                ////                    #endregion
                ////                    #region GuarantorAsset
                ////                    foreach (var lp2 in lp.GuarantorAsset)
                ////                    {
                ////                        string Description = lp2.Description;
                ////                        string Quantity = lp2.Quantity;
                ////                        string UnitPrice = lp2.UnitPrice;
                ////                        //
                ////                        sql = "exec T24_LoanAppGuarantorAssetAddEdit @ServerLoanAppID=@ServerLoanAppID,@ServerLoanAppPersonID=@ServerLoanAppPersonID"
                ////                        + ",@Description=@Description,@Quantity=@Quantity,@UnitPrice=@UnitPrice";
                ////                        Com1.CommandText = sql;
                ////                        Com1.Parameters.Clear();
                ////                        Com1.Parameters.AddWithValue("@ServerLoanAppID", ServerLoanAppID);
                ////                        Com1.Parameters.AddWithValue("@ServerLoanAppPersonID", ServerLoanAppPersonID);
                ////                        Com1.Parameters.AddWithValue("@Description", Description);
                ////                        Com1.Parameters.AddWithValue("@Quantity", Quantity);
                ////                        Com1.Parameters.AddWithValue("@UnitPrice", UnitPrice);
                ////                        Com1.ExecuteNonQuery();
                ////                    }
                ////                    #endregion
                ////                    #region PersonImg
                ////                    foreach (var lp2 in lp.PersonImg)
                ////                    {
                ////                        string ImgName = lp2.ImgName;
                ////                        string Ext = lp2.Ext;
                ////                        string OneCardTwoDoc = lp2.OneCardTwoDoc;
                ////                        //
                ////                        sql = "exec T24_LoanAppPersonImageImageAdd @ServerLoanAppID=@ServerLoanAppID,@ServerLoanAppPersonID=@ServerLoanAppPersonID"
                ////                        + ",@Ext=@Ext,@Remark=@Remark,@OneCardTwoDoc=@OneCardTwoDoc";
                ////                        Com1.CommandText = sql;
                ////                        Com1.Parameters.Clear();
                ////                        Com1.Parameters.AddWithValue("@ServerLoanAppID", ServerLoanAppID);
                ////                        Com1.Parameters.AddWithValue("@ServerLoanAppPersonID", ServerLoanAppPersonID);
                ////                        Com1.Parameters.AddWithValue("@Ext", Ext);
                ////                        Com1.Parameters.AddWithValue("@Remark", ImgName);
                ////                        Com1.Parameters.AddWithValue("@OneCardTwoDoc", OneCardTwoDoc);
                ////                        DataTable dt3 = new DataTable();
                ////                        dt3.Load(Com1.ExecuteReader());
                ////                        string fname = dt3.Rows[0][0].ToString();
                ////                        //string fname = ImgName + "_2";
                ////                        LoanAppResImgList data = new LoanAppResImgList();
                ////                        data.OriImgName = ImgName;
                ////                        data.ServerImgName = fname;
                ////                        ImgList.Add(data);
                ////                    }
                ////                    #endregion

                ////                }
                ////                #endregion Person
                ////                #region Opinion
                ////                foreach (var lp in loan.Opinion)
                ////                {
                ////                    string Description = lp.Description;
                ////                    //string CreateBy = lp.CreateBy;
                ////                    string CreateBy = UserID;
                ////                    string DeviceDate_Opinion = lp.DeviceDate;
                ////                    sql = "exec sp_LoanApp20OpinionAdd2 @LoanAppID=@LoanAppID,@Opinion=@Opinion,@DeviceDate=@DeviceDate,@CreateDate=@CreateDate"
                ////                    + ",@CreateBy=@CreateBy,@LoanAppStatusID=@LoanAppStatusID";
                ////                    Com1.CommandText = sql;
                ////                    Com1.Parameters.Clear();
                ////                    Com1.Parameters.AddWithValue("@LoanAppID", ServerLoanAppID);
                ////                    Com1.Parameters.AddWithValue("@Opinion", Description);
                ////                    Com1.Parameters.AddWithValue("@DeviceDate", DeviceDate_Opinion);
                ////                    Com1.Parameters.AddWithValue("@CreateDate", ServerDate);
                ////                    Com1.Parameters.AddWithValue("@CreateBy", CreateBy);
                ////                    Com1.Parameters.AddWithValue("@LoanAppStatusID", LoanAppStatusID);
                ////                    Com1.ExecuteNonQuery();
                ////                }
                ////                #endregion Opinion
                ////                #region CashFlow
                ////                foreach (var lp in loan.CashFlow)
                ////                {
                ////                    #region CashFlow
                ////                    string ServerLoanAppCashFlowID = "";
                ////                    string StudyMonthAmount = lp.StudyMonthAmount;
                ////                    string StudyStartMonth = lp.StudyStartMonth;
                ////                    string FamilyExpensePerMonth = lp.FamilyExpensePerMonth;
                ////                    string OtherExpensePerMonth = lp.OtherExpensePerMonth;
                ////                    sql = "exec T24_LoanAppCashFlowAddEdit @LoanAppID=@LoanAppID,@StudyMonthAmount=@StudyMonthAmount,@StudyStartMonth=@StudyStartMonth"
                ////                    + ",@FamilyExpensePerMonth=@FamilyExpensePerMonth,@OtherExpensePerMonth=@OtherExpensePerMonth";
                ////                    Com1.CommandText = sql;
                ////                    Com1.Parameters.Clear();
                ////                    Com1.Parameters.AddWithValue("@LoanAppID", ServerLoanAppID);
                ////                    Com1.Parameters.AddWithValue("@StudyMonthAmount", StudyMonthAmount);
                ////                    Com1.Parameters.AddWithValue("@StudyStartMonth", StudyStartMonth);
                ////                    Com1.Parameters.AddWithValue("@FamilyExpensePerMonth", FamilyExpensePerMonth);
                ////                    Com1.Parameters.AddWithValue("@OtherExpensePerMonth", OtherExpensePerMonth);
                ////                    DataTable dt2 = new DataTable();
                ////                    dt2.Load(Com1.ExecuteReader());
                ////                    ServerLoanAppCashFlowID = dt2.Rows[0][0].ToString();
                ////                    //ServerLoanAppCashFlowID = "1";
                ////                    #endregion CashFlow
                ////                    #region MSI
                ////                    foreach (var lp2 in lp.MSI)
                ////                    {
                ////                        #region MSI
                ////                        string ServerLoanAppCashFlowMSIID = "";
                ////                        string IncomeTypeID = lp2.IncomeTypeID;
                ////                        string MainSourceIncomeID = lp2.MainSourceIncomeID;
                ////                        string Remark = lp2.Remark;
                ////                        string Quantity = lp2.Quantity;
                ////                        string ExAge = lp2.ExAge;
                ////                        string BusAge = lp2.BusAge;
                ////                        string isMSI = lp2.isMSI;
                ////                        sql = "exec T24_LoanAppCashFlowMSIAddEdit @ServerLoanAppCashFlowID=@ServerLoanAppCashFlowID,@IncomeTypeID=@IncomeTypeID"
                ////                        + ",@MainSourceIncomeID=@MainSourceIncomeID,@Remark=@Remark,@Quantity=@Quantity,@ExAge=@ExAge,@BusAge=@BusAge,@isMSI=@isMSI";
                ////                        Com1.CommandText = sql;
                ////                        Com1.Parameters.Clear();
                ////                        Com1.Parameters.AddWithValue("@ServerLoanAppCashFlowID", ServerLoanAppCashFlowID);
                ////                        Com1.Parameters.AddWithValue("@IncomeTypeID", IncomeTypeID);
                ////                        Com1.Parameters.AddWithValue("@MainSourceIncomeID", MainSourceIncomeID);
                ////                        Com1.Parameters.AddWithValue("@Remark", Remark);
                ////                        Com1.Parameters.AddWithValue("@Quantity", Quantity);
                ////                        Com1.Parameters.AddWithValue("@ExAge", ExAge);
                ////                        Com1.Parameters.AddWithValue("@BusAge", BusAge);
                ////                        Com1.Parameters.AddWithValue("@isMSI", isMSI);
                ////                        DataTable dt3 = new DataTable();
                ////                        dt3.Load(Com1.ExecuteReader());
                ////                        ServerLoanAppCashFlowMSIID = dt3.Rows[0][0].ToString();
                ////                        //ServerLoanAppCashFlowMSIID = "1";
                ////                        #endregion MSI
                ////                        #region MSIRegular
                ////                        foreach (var lp3 in lp2.MSIRegular)
                ////                        {
                ////                            string Description = lp3.Description;
                ////                            string Amount = lp3.Amount;
                ////                            string UnitID = lp3.UnitID;
                ////                            string Cost = lp3.Cost;
                ////                            string OneIncomeTwoExpense = lp3.OneIncomeTwoExpense;
                ////                            string CurrencyID = lp3.Currency;
                ////                            if (CurrencyID == "KHR")
                ////                            {
                ////                                CurrencyID = "1";
                ////                            }
                ////                            else if (CurrencyID == "USD")
                ////                            {
                ////                                CurrencyID = "2";
                ////                            }
                ////                            else
                ////                            {
                ////                                CurrencyID = "3";
                ////                            }
                ////                            string Month = lp3.Month;
                ////                            sql = "exec T24_LoanAppCashFlowMSIRegAddEdit @LoanAppCashFlowMSIID=@LoanAppCashFlowMSIID,@Description=@Description"
                ////                            + ",@Amount=@Amount,@UnitID=@UnitID,@Cost=@Cost,@OneIncomeTwoExpense=@OneIncomeTwoExpense,@CurrencyID=@CurrencyID,@Month=@Month";
                ////                            Com1.CommandText = sql;
                ////                            Com1.Parameters.Clear();
                ////                            Com1.Parameters.AddWithValue("@LoanAppCashFlowMSIID", ServerLoanAppCashFlowMSIID);
                ////                            Com1.Parameters.AddWithValue("@Description", Description);
                ////                            Com1.Parameters.AddWithValue("@Amount", Amount);
                ////                            Com1.Parameters.AddWithValue("@UnitID", UnitID);
                ////                            Com1.Parameters.AddWithValue("@Cost", Cost);
                ////                            Com1.Parameters.AddWithValue("@OneIncomeTwoExpense", OneIncomeTwoExpense);
                ////                            Com1.Parameters.AddWithValue("@CurrencyID", CurrencyID);
                ////                            Com1.Parameters.AddWithValue("@Month", Month);
                ////                            Com1.ExecuteNonQuery();
                ////                        }
                ////                        #endregion MSIRegular
                ////                        #region MSIIrregular
                ////                        foreach (var lp3 in lp2.MSIIrregular)
                ////                        {
                ////                            string Description = lp3.Description;
                ////                            string Amount = lp3.Amount;
                ////                            string UnitID = lp3.UnitID;
                ////                            string Cost = lp3.Cost;
                ////                            string OneIncomeTwoExpense = lp3.OneIncomeTwoExpense;
                ////                            string CurrencyID = lp3.Currency;
                ////                            if (CurrencyID == "KHR")
                ////                            {
                ////                                CurrencyID = "1";
                ////                            }
                ////                            else if (CurrencyID == "USD")
                ////                            {
                ////                                CurrencyID = "2";
                ////                            }
                ////                            else
                ////                            {
                ////                                CurrencyID = "3";
                ////                            }
                ////                            string Month = lp3.Month;
                ////                            sql = "exec T24_LoanAppCashFlowMSIIRegAddEdit @LoanAppCashFlowMSIID=@LoanAppCashFlowMSIID,@Description=@Description"
                ////                            + ",@Amount=@Amount,@UnitID=@UnitID,@Cost=@Cost,@OneIncomeTwoExpense=@OneIncomeTwoExpense,@CurrencyID=@CurrencyID,@Month=@Month";
                ////                            Com1.CommandText = sql;
                ////                            Com1.Parameters.Clear();
                ////                            Com1.Parameters.AddWithValue("@LoanAppCashFlowMSIID", ServerLoanAppCashFlowMSIID);
                ////                            Com1.Parameters.AddWithValue("@Description", Description);
                ////                            Com1.Parameters.AddWithValue("@Amount", Amount);
                ////                            Com1.Parameters.AddWithValue("@UnitID", UnitID);
                ////                            Com1.Parameters.AddWithValue("@Cost", Cost);
                ////                            Com1.Parameters.AddWithValue("@OneIncomeTwoExpense", OneIncomeTwoExpense);
                ////                            Com1.Parameters.AddWithValue("@CurrencyID", CurrencyID);
                ////                            Com1.Parameters.AddWithValue("@Month", Month);
                ////                            Com1.ExecuteNonQuery();
                ////                        }
                ////                        #endregion MSIIrregular

                ////                    }
                ////                    #endregion MSI

                ////                }
                ////                #endregion CashFlow

                ////            }
                ////        }
                ////        catch (Exception ex)
                ////        {
                ////            int line = c.GetLineNumber(ex);
                ////            ERR = "Error";
                ////            SMS = "Something was wrong while saving LoanApp: " + line.ToString() + " | " + ex.Message.ToString();
                ////            LoanAppResSMS data = new LoanAppResSMS();
                ////            data.SMS = SMS;
                ////            SMSList.Add(data);
                ////        }
                ////        #region Commit or RollBack
                ////        try
                ////        {
                ////            if (ERR == "Error")
                ////            {
                ////                //Tran1.Rollback();
                ////            }
                ////            else
                ////            {
                ////                c.ReturnDT("update tblLoanApp1 set UploadERR=1 where LoanAppID='" + ServerLoanAppID + "'");
                ////                //Tran1.Commit();
                ////                LoanAppResSMS data = new LoanAppResSMS();
                ////                data.SMS = "";
                ////                SMSList.Add(data);
                ////            }
                ////            Con1.Close();
                ////        }
                ////        catch { }
                ////        #endregion Commit or RollBack
                ////    }
                ////    #endregion add
                ////}
                //#endregion old
                //#endregion LoanApp
                //#region LogImg
                //if (ERR != "Error")
                //{
                //    if (ImgList.Count == 0)
                //    {
                //        ERR = "Error";
                //        SMS = "No image";
                //    }
                //    else
                //    {
                //        for (int i = 0; i < ImgList.Count; i++)
                //        {
                //            try
                //            {
                //                string _ImgType = ImgList[i].ImgType;
                //                string _ClientID = ImgList[i].ClientID;
                //                string _ServerID = ImgList[i].ServerID;
                //                string _OriImgName = ImgList[i].OriImgName;
                //                string _ServerImgName = ImgList[i].ServerImgName;
                //                string sql = "exec T24_LoanAppImgLogV2 @LoanAppID='" + RSLoanAppID + "',@ImgType='" + _ImgType + "',@ClientID='" + _ClientID + "',@ServerID='" + _ServerID
                //                + "',@OriImgName=@OriImgName,@ServerImgName=@ServerImgName,@ServerDate='" + ServerDate + "'";
                //                SqlConnection Con1 = new SqlConnection(c.ConStr());
                //                Con1.Open();
                //                SqlCommand Com1 = new SqlCommand();
                //                Com1.Connection = Con1;
                //                Com1.Parameters.Clear();
                //                Com1.CommandText = sql;
                //                Com1.Parameters.AddWithValue("@OriImgName", _OriImgName);
                //                Com1.Parameters.AddWithValue("@ServerImgName", _ServerImgName);
                //                Com1.ExecuteNonQuery();
                //                Con1.Close();
                //            }
                //            catch (Exception ex)
                //            {
                //                ERR = "Error";
                //                SMS = "Cannot add image log";
                //                ExSMS = ex.ToString();
                //            }
                //        }
                //    }
                //}
                //#endregion
                #endregion

                #region LoanApp
                if (ERR != "Error")
                {
                    string ServerLoanAppID = "0", PersonType = "";
                    try
                    {
                        foreach (var loan in jObj.LoanApp)
                        {
                            try
                            {
                                #region LoanApp Param
                                string LoanClientID = loan.LoanClientID;
                                string LoanAppID = loan.LoanAppID;
                                string LoanAppStatusID = loan.LoanAppStatusID;
                                string DeviceDate = loan.DeviceDate;
                                string ProductID = loan.ProductID;
                                string LoanRequestAmount = loan.LoanRequestAmount.Replace(",", "");
                                string OwnCapital = loan.OwnCapital;
                                string DisbursementDate = loan.DisbursementDate;
                                string FirstWithdrawal = loan.FirstWithdrawal;
                                string LoanTerm = loan.LoanTerm;
                                string FirstRepaymentDate = loan.FirstRepaymentDate;
                                string LoanInterestRate = loan.LoanInterestRate;
                                string CustomerRequestRate = loan.CustomerRequestRate;
                                string CompititorRate = loan.CompititorRate;
                                string CustomerConditionID = loan.CustomerConditionID;
                                string COProposedAmount = loan.COProposedAmount;
                                string COProposedTerm = loan.COProposedTerm;
                                string COProposeRate = loan.COProposeRate;
                                string FrontBackOfficeID = loan.FrontBackOfficeID;
                                string GroupNumber = loan.GroupNumber;
                                string LoanCycleID = loan.LoanCycleID;
                                string RepaymentHistoryID = loan.RepaymentHistoryID;
                                string LoanReferralID = loan.LoanReferralID;
                                //string DebtIinfoID = loan.DebtIinfoID;
                                string MonthlyFee = loan.MonthlyFee;
                                string Compulsory = loan.Compulsory;
                                string CompulsoryTerm = loan.CompulsoryTerm;
                                string Currency = loan.Currency;
                                string UpFrontFee = loan.UpFrontFee;
                                //string UpFrontAmt=loan.UpFrontAmt;
                                string CompulsoryOptionID = loan.CompulsoryOptionID;
                                string FundSource = loan.FundSource;
                                string IsNewCollateral = loan.IsNewCollateral;
                                string AgriBuddy = loan.AgriBuddy;
                                string semiBallonFreqID = loan.SemiBallonFreqID;

                                string LoanTypeID = loan.LoanTypeID;
                                //string AMApproveAmt = loan.AMApproveAmt;
                                //string AMApproveTerm = loan.AMApproveTerm;
                                //string AMApproveRate = loan.AMApproveRate;
                                string PaymentMethodID = loan.PaymentMethodID;
                                string GracePeriodID = loan.GracePeriodID;
                                string MITypeID = loan.MITypeID;

                                string DebtIinfoID = "1";
                                foreach (var Person in loan.Person)
                                {
                                    PersonType = Person.LoanAppPersonTypeID;
                                    if (Person.LoanAppPersonTypeID == "31")
                                    {
                                        if (Person.Creditor != null)
                                        {
                                            DebtIinfoID = "2";
                                        }
                                    }
                                }
                                string UpFromAmt = "";
                                UpFromAmt = ((Convert.ToDouble(LoanRequestAmount) * Convert.ToDouble(UpFrontFee)) / 100).ToString();

                                #endregion LoanApp Param
                                #region sql
                                try
                                {

                                    if (CompulsoryTerm == null)
                                    {
                                        CompulsoryTerm = "-1";
                                    }
                                    if (MonthlyFee == null)
                                    {
                                        MonthlyFee = "-1";
                                    }
                                    if (CompititorRate == null)
                                    {
                                        CompititorRate = "-1";
                                    }
                                    if (CustomerConditionID == null)
                                    {
                                        CustomerConditionID = "-1";
                                    }
                                    if (LoanReferralID == null)
                                    {
                                        LoanReferralID = "-1";
                                    }
                                    if (FrontBackOfficeID == null)
                                    {
                                        FrontBackOfficeID = "-1";
                                    }

                                    string CollateralDebt = loan.CollateralDebt;

                                    string[] rs = c.AddLoanFromDevice(LoanClientID, LoanAppID, LoanAppStatusID, DeviceDate, ServerDate, UserID, ProductID
                                    , LoanRequestAmount, OwnCapital, DisbursementDate, FirstWithdrawal, LoanTerm, FirstRepaymentDate
                                    , LoanInterestRate, COProposedAmount, COProposedTerm, COProposeRate, GroupNumber, LoanCycleID
                                    , RepaymentHistoryID, DebtIinfoID, Compulsory, CompulsoryTerm, Currency, UpFrontFee, UpFromAmt, CompulsoryOptionID
                                    , FundSource, IsNewCollateral, AgriBuddy, semiBallonFreqID
                                    , PaymentMethodID, LoanTypeID, GracePeriodID, MITypeID

                                    , MonthlyFee, CompititorRate, CustomerConditionID, LoanReferralID
                                    , FrontBackOfficeID, CollateralDebt);

                                    ERR = rs[0];
                                    SMS = rs[1];
                                    ServerLoanAppID = rs[2];
                                    ExSMS = rs[3];

                                    RSIDOnDevice = LoanClientID;
                                    RSLoanAppID = ServerLoanAppID;
                                }
                                catch (Exception ex)
                                {
                                    ERR = "Error";
                                    SMS = "Cannot add loan to switch DB";
                                    ExSMS = ex.Message.ToString();
                                }
                                #endregion sql
                            }
                            catch (Exception ex)
                            {
                                ERR = "Error";
                                SMS = "Cannot read loan parameter";
                                ExSMS = ex.Message.ToString();
                            }

                            if (ERR != "Error")
                            {
                                string ServerLoanAppPersonID = "0", VillageBankIDForGetCOIDOri = "";
                                #region Person

                                if (loan.Person != null)
                                {
                                    foreach (var Person in loan.Person)
                                    {
                                        if (ERR != "Error")
                                        {
                                            try
                                            {
                                                #region Person Para
                                                string LoanAppPersonTypeID = Person.LoanAppPersonTypeID;
                                                string CustomerID = Person.T24CustID;
                                                string VillageBankID = Person.VBID;
                                                if (LoanAppPersonTypeID == "31")
                                                {
                                                    VillageBankIDForGetCOIDOri = VillageBankID;
                                                }
                                                string NameKhLast = Person.NameKhLast;
                                                string TitleID = Person.TitleID;
                                                string LastName = Person.NameEnLast;
                                                string FirstName = Person.NameEnFirst;
                                                string GenderID = Person.GenderID;
                                                string DateOfBirth = Person.DateOfBirth;
                                                string IDCardTypeID = Person.IDCardTypeID;
                                                string IDCardNumber = Person.IDCardNumber;
                                                string IDCardExpireDate = Person.IDCardExpiryDate;
                                                string MaritalStatusID = Person.MaritalStatusID;
                                                string EducationID = Person.EducationLevelID;
                                                string CityOfBirthID = Person.PlaceOfBirth;
                                                string Telephone3 = Person.Phone;
                                                string VillageIDPermanent = Person.VillageIDPer;
                                                string VillageIDCurrent = Person.VillageIDCur;
                                                string SortAddress = Person.ShortAddress;
                                                string FamilyMember = Person.FamilyMember;
                                                string FamilyMemberActive = Person.FamilyMemberActive;
                                                string PoorID = Person.PoorLevelID;
                                                string LoanAppPersonIDOnDevice = Person.CustClientID;
                                                string CustDeviceDate = Person.CreateDateClient;
                                                string IDCardIssuedDate = Person.IDCardIssueDate;
                                                string NameKhFirst = Person.NameKhFirst;
                                                string ProspectCode = Person.ProspectCode;
                                                string ReferByID = Person.ReferByID;
                                                string ReferName = Person.ReferName;
                                                string LatLon = Person.LatLon;
                                                string CustServerID = Person.CustServerID;
                                                string Nationality = Person.Nationality;
                                                #endregion Person Para
                                                #region sql
                                                try
                                                {
                                                    string[] rs = c.AddCustFromDevice(ServerLoanAppID, LoanAppPersonTypeID, CustomerID, VillageBankID
                                                    , NameKhLast, TitleID, LastName, FirstName, GenderID, DateOfBirth, IDCardTypeID, IDCardNumber
                                                    , IDCardExpireDate, MaritalStatusID, EducationID, CityOfBirthID, Telephone3, VillageIDPermanent
                                                    , VillageIDCurrent, SortAddress, FamilyMember, FamilyMemberActive, PoorID, LoanAppPersonIDOnDevice
                                                    , CustDeviceDate, IDCardIssuedDate, NameKhFirst, ProspectCode, ReferByID, ReferName, LatLon, CustServerID,"0", Nationality);

                                                    ERR = rs[0];
                                                    SMS = rs[1];
                                                    ServerLoanAppPersonID = rs[2];
                                                    ExSMS = rs[3];

                                                }
                                                catch (Exception ex)
                                                {
                                                    ERR = "Error";
                                                    SMS = "Cannot add customer to switch DB";
                                                    ExSMS = ex.Message.ToString();
                                                }
                                                #endregion sql

                                                #region PersonImg
                                                if (Person.PersonImg != null)
                                                {
                                                    foreach (var PersonImg in Person.PersonImg)
                                                    {
                                                        if (ERR != "Error")
                                                        {
                                                            #region Para
                                                            string CustImageClientID = PersonImg.CustImageClientID;
                                                            string perImg_custImageServerID = PersonImg.CustImageServerID;
                                                            string perImg_custClientID = PersonImg.CustClientID;
                                                            string perImg_createDateClient = PersonImg.CreateDateClient;
                                                            string OneCardTwoDoc = PersonImg.OneCardTwoDoc;
                                                            string perImg_ext = PersonImg.Ext;
                                                            string perImg_imgPath = PersonImg.ImgPath;//File Name
                                                            string perImg_remark = PersonImg.Remark;

                                                            string perImg_fName = "PersonImg_" + ServerLoanAppID + "_" + ServerLoanAppPersonID + "_" + OneCardTwoDoc + "_" + CustImageClientID + "_" + ServerDateForFileName + "." + perImg_ext.Replace(".", "");
                                                            #endregion Para
                                                            #region sql
                                                            try
                                                            {
                                                                //string[] rs = c.AddCustImgFromDevice(ServerLoanAppPersonID, OneCardTwoDoc, perImg_ext, c.ImgPathGet()+perImg_fName, ServerLoanAppID, CustImageClientID);
                                                                string[] rs = c.AddCustImgFromDevice(ServerLoanAppPersonID, OneCardTwoDoc, perImg_ext, perImg_fName, ServerLoanAppID, CustImageClientID);

                                                                ERR = rs[0];
                                                                SMS = rs[1];
                                                                string ServerLoanAppPersonImageID = rs[2];
                                                                ExSMS = rs[3];

                                                                if (ERR != "Error")
                                                                {
                                                                    LoanAppResImgList ilist = new LoanAppResImgList();
                                                                    ilist.ImgType = "PersonImg";
                                                                    ilist.ClientID = CustImageClientID;
                                                                    ilist.ServerID = ServerLoanAppPersonImageID;
                                                                    ilist.OriImgName = perImg_imgPath;
                                                                    ilist.ServerImgName = perImg_fName;
                                                                    ImgList.Add(ilist);
                                                                }
                                                            }
                                                            catch (Exception ex)
                                                            {
                                                                ERR = "Error";
                                                                SMS = "Cannot add customer image to switch DB";
                                                                ExSMS = ex.Message.ToString();
                                                            }
                                                            #endregion sql
                                                        }
                                                    }
                                                }

                                                #endregion PersonImg

                                                if (Person.LoanAppPersonTypeID == "31")
                                                {

                                                    #region Asset

                                                    if (Person.Asset != null)
                                                    {
                                                        foreach (var Asset in Person.Asset)
                                                        {
                                                            if (ERR != "Error")
                                                            {
                                                                string ServerLoanAppClientAssetID = "0";
                                                                #region Para
                                                                string asset_assetClientID = Asset.AssetClientID;
                                                                string asset_assetServerID = Asset.AssetServerID;
                                                                string asset_loanPurposeClientID = Asset.LoanPurposeClientID;
                                                                string asset_loanClientID = Asset.LoanClientID;
                                                                string asset_loanAppID = Asset.LoanAppID;
                                                                string asset_description = Asset.Description;
                                                                string asset_quantity = Asset.Quantity;
                                                                string asset_unitPrice = Asset.UnitPrice;
                                                                string asset_custClientID = Asset.CustClientID;
                                                                string asset_custServerID = Asset.CustServerID;
                                                                string asset_lookupID = Asset.AssetLookUpID;
                                                                string assetOtherDescription = Asset.AssetOtherDescription;
                                                                string unit = "";
                                                                #endregion Para
                                                                #region sql
                                                                try
                                                                {
                                                                    string[] rs = c.AddCustAssetFromDevice(ServerLoanAppPersonID, asset_description, asset_quantity, asset_unitPrice, asset_custClientID, ServerLoanAppID, asset_lookupID, assetOtherDescription,unit);

                                                                    ERR = rs[0];
                                                                    SMS = rs[1];
                                                                    ServerLoanAppClientAssetID = rs[2];
                                                                    ExSMS = rs[3];
                                                                }
                                                                catch (Exception ex)
                                                                {
                                                                    ERR = "Error";
                                                                    SMS = "Cannot add asset to switch DB";
                                                                    ExSMS = ex.Message.ToString();
                                                                }
                                                                #endregion sql
                                                                if (Asset.AssetImg != null)
                                                                {
                                                                    foreach (var AssetImg in Asset.AssetImg)
                                                                    {
                                                                        if (ERR != "Error")
                                                                        {
                                                                            try
                                                                            {
                                                                                #region Para
                                                                                string AssetImageClientID = AssetImg.AssetImageClientID;
                                                                                string AssetImageServerID = AssetImg.AssetImageServerID;
                                                                                string AssetClientID = AssetImg.AssetClientID;
                                                                                string AssetServerID = AssetImg.AssetServerID;
                                                                                string CreateDateClient = AssetImg.CreateDateClient;
                                                                                string AssetImg_Ext = AssetImg.Ext;
                                                                                string AssetImg_ImgPath = AssetImg.ImgPath;//file name
                                                                                string Remark = AssetImg.Remark;

                                                                                string Asset_fName = "AssetImg_" + ServerLoanAppID + "_" + ServerLoanAppClientAssetID + "_" + AssetImageClientID + "_" + ServerDateForFileName + "." + AssetImg_Ext.Replace(".", "");
                                                                                #endregion Para
                                                                                #region sql
                                                                                try
                                                                                {
                                                                                    //string[] rs = c.AddCustAsseImgFromDevice(AssetImageClientID, ServerLoanAppClientAssetID, CreateDateClient, AssetImg_Ext, c.ImgPathGet() + Asset_fName);
                                                                                    string[] rs = c.AddCustAsseImgFromDevice(AssetImageClientID, ServerLoanAppClientAssetID, CreateDateClient, AssetImg_Ext, Asset_fName);

                                                                                    ERR = rs[0];
                                                                                    SMS = rs[1];
                                                                                    AssetServerID = rs[2];
                                                                                    ExSMS = rs[3];

                                                                                    if (ERR != "Error")
                                                                                    {
                                                                                        LoanAppResImgList ilist = new LoanAppResImgList();
                                                                                        ilist.ImgType = "AssetImg";
                                                                                        ilist.ClientID = AssetImageClientID;
                                                                                        ilist.ServerID = AssetServerID;
                                                                                        ilist.OriImgName = AssetImg_ImgPath;
                                                                                        ilist.ServerImgName = Asset_fName;
                                                                                        ImgList.Add(ilist);
                                                                                    }
                                                                                }
                                                                                catch (Exception ex)
                                                                                {
                                                                                    ERR = "Error";
                                                                                    SMS = "Cannot add asset image to switch DB";
                                                                                    ExSMS = ex.Message.ToString();
                                                                                }
                                                                                #endregion sql
                                                                            }
                                                                            catch (Exception ex)
                                                                            {
                                                                                ERR = "Error";
                                                                                SMS = "Cannot add asset image to switch DB";
                                                                                ExSMS = ex.Message.ToString();
                                                                            }
                                                                        }
                                                                    }
                                                                }

                                                            }
                                                        }
                                                        //if Error
                                                        if (ERR == "Error")
                                                        {
                                                            LoanAppResSMS data = new LoanAppResSMS();
                                                            data.SMS = "Asset: " + ExSMS;
                                                            SMSList.Add(data);
                                                        }
                                                    }
                                                    #endregion Asset

                                                    #region Creditor
                                                    if (ERR != "Error")
                                                    {

                                                        if (Person.Creditor != null)
                                                        {
                                                            foreach (var Creditor in Person.Creditor)
                                                            {
                                                                try
                                                                {
                                                                    if (ERR != "Error")
                                                                    {
                                                                        #region Para
                                                                        string CreditorClientID = Creditor.CreditorClientID;
                                                                        string creditor_creditorServerID = Creditor.CreditorServerID;
                                                                        string creditor_loanClientID = Creditor.LoanClientID;
                                                                        string creditor_loanAppID = Creditor.LoanAppID;
                                                                        string CreditorID = Creditor.CreditorID;
                                                                        string creditor_currency = Creditor.Currency;
                                                                        string creditor_approvedAmount = Creditor.ApprovedAmount;
                                                                        string creditor_outstandingBalance = Creditor.OutstandingBalance;
                                                                        string creditor_interestRate = Creditor.InterestRate;
                                                                        string creditor_repaymentTypeID = Creditor.RepaymentTypeID;
                                                                        string creditor_repaymentTermID = Creditor.RepaymentTermID;
                                                                        string creditor_loanStartDate = Creditor.LoanStartDate;
                                                                        string creditor_loanEndDate = Creditor.LoanEndDate;
                                                                        string creditor_isReFinance = Creditor.IsReFinance;
                                                                        string creditor_reFinanceAmount = Creditor.ReFinanceAmount;
                                                                        string creditor_custClientID = Creditor.CustClientID;
                                                                        string creditor_custServerID = Creditor.CustServerID;
                                                                        string CreditorName = "";
                                                                        #endregion Para
                                                                        #region sql
                                                                        try
                                                                        {
                                                                            string[] rs = c.AddCustCreditorFromDevice(CreditorClientID, CreditorID, creditor_currency, creditor_approvedAmount
                                                                                , creditor_outstandingBalance, creditor_interestRate, creditor_repaymentTypeID, creditor_repaymentTermID
                                                                                , creditor_loanStartDate, creditor_loanEndDate, creditor_isReFinance, creditor_reFinanceAmount, ServerLoanAppPersonID
                                                                                , ServerLoanAppID, CreditorName);

                                                                            ERR = rs[0];
                                                                            SMS = rs[1];
                                                                            string ServerLoanAppCreditorID = rs[2];
                                                                            ExSMS = rs[3];

                                                                        }
                                                                        catch (Exception ex)
                                                                        {
                                                                            ERR = "Error";
                                                                            SMS = "Cannot add Creditor to switch DB";
                                                                            ExSMS = ex.Message.ToString();
                                                                        }
                                                                        #endregion sql
                                                                    }
                                                                }
                                                                catch (Exception ex)
                                                                {
                                                                    ERR = "Error";
                                                                    SMS = "Cannot read Creditor";
                                                                    ExSMS = ex.Message.ToString();
                                                                }

                                                            }
                                                        }
                                                        //if Error
                                                        if (ERR == "Error")
                                                        {
                                                            LoanAppResSMS data = new LoanAppResSMS();
                                                            data.SMS = "Creditor: " + ExSMS;
                                                            SMSList.Add(data);
                                                        }
                                                    }
                                                    #endregion Creditor

                                                    #region RealEstate
                                                    if (ERR != "Error")
                                                    {

                                                        if (Person.RealEstate != null)
                                                        {
                                                            foreach (var RealEstate in Person.RealEstate)
                                                            {
                                                                if (ERR != "Error")
                                                                {
                                                                    try
                                                                    {
                                                                        string CollateralServerID = "0";
                                                                        #region RealEstate
                                                                        string re_CollateralClientID = RealEstate.CollateralClientID;
                                                                        string re_CollateralServerID = RealEstate.CollateralServerID;
                                                                        string re_CoanClientID = RealEstate.LoanClientID;
                                                                        string re_LoanAppID = RealEstate.LoanAppID;
                                                                        string re_CustClientID = RealEstate.CustClientID;
                                                                        string re_CustServerID = RealEstate.CustServerID;
                                                                        string re_CollateralDocGroupTypeID = RealEstate.CollateralDocGroupTypeID;
                                                                        string re_CollateralDocHardTypeID = RealEstate.CollateralDocHardTypeID;
                                                                        string re_CollateralDocSoftTypeID = RealEstate.CollateralDocSoftTypeID;
                                                                        string re_CollateralOwnerTypeID = RealEstate.CollateralOwnerTypeID;
                                                                        string re_CollateralLocationVillageID = RealEstate.CollateralLocationVillageID;
                                                                        string re_CollateralRoadAccessibilityID = RealEstate.CollateralRoadAccessibilityID;
                                                                        string re_PropertyTypeID = RealEstate.PropertyTypeID;
                                                                        string re_LandTypeID = RealEstate.LandTypeID;
                                                                        string re_LandSize = RealEstate.LandSize;
                                                                        string re_LandMarketPrice = RealEstate.LandMarketPrice;
                                                                        string re_LandForcedSalePrice = RealEstate.LandForcedSalePrice;
                                                                        string re_BuildingTypeID = RealEstate.BuildingTypeID;
                                                                        string re_BuildingSize = RealEstate.BuildingSize;
                                                                        string re_BuildingMarketPrice = RealEstate.BuildingMarketPrice;
                                                                        string re_BuildingForcedSalesPrice = RealEstate.BuildingForcedSalesPrice;
                                                                        #endregion RealEstate
                                                                        #region sql
                                                                        try
                                                                        {
                                                                            string[] rs = c.AddCustRealEstateFromDevice(re_CollateralClientID, re_CoanClientID, ServerLoanAppID, re_CustClientID
                                                                            , ServerLoanAppPersonID, re_CollateralDocGroupTypeID, re_CollateralDocHardTypeID, re_CollateralDocSoftTypeID
                                                                            , re_CollateralOwnerTypeID, re_CollateralLocationVillageID, re_CollateralRoadAccessibilityID, re_PropertyTypeID
                                                                            , re_LandTypeID, re_LandSize, re_LandMarketPrice, re_LandForcedSalePrice, re_BuildingTypeID, re_BuildingSize
                                                                            , re_BuildingMarketPrice, re_BuildingForcedSalesPrice);

                                                                            ERR = rs[0];
                                                                            SMS = rs[1];
                                                                            CollateralServerID = rs[2];
                                                                            ExSMS = rs[3];

                                                                        }
                                                                        catch (Exception ex)
                                                                        {
                                                                            ERR = "Error";
                                                                            SMS = "Cannot read RealEstate";
                                                                            ExSMS = ex.Message.ToString();
                                                                        }
                                                                        #endregion sql
                                                                        #region img
                                                                        if (ERR != "Error")
                                                                        {
                                                                            try
                                                                            {
                                                                                if (RealEstate.RealEstateImg != null)
                                                                                {
                                                                                    foreach (var PREImgV2 in RealEstate.RealEstateImg)
                                                                                    {
                                                                                        if (ERR != "Error")
                                                                                        {
                                                                                            try
                                                                                            {
                                                                                                #region Para
                                                                                                string PREImgV2_imageClientID = PREImgV2.ImageClientID;
                                                                                                string PREImgV2_imageServerID = PREImgV2.ImageServerID;
                                                                                                string PREImgV2_collateralClientID = PREImgV2.CollateralClientID;
                                                                                                string PREImgV2_collateralServerID = PREImgV2.CollateralServerID;
                                                                                                string PREImgV2_createDateClient = PREImgV2.CreateDateClient;
                                                                                                string PREImgV2_ext = PREImgV2.Ext;
                                                                                                string PREImgV2_imgPath = PREImgV2.ImgPath;
                                                                                                string PREImgV2_remark = PREImgV2.Remark;

                                                                                                string RealEstate_fName = "RealEstateImg_" + ServerLoanAppID + "_" + CollateralServerID + "_" + PREImgV2_imageClientID + "_" + ServerDateForFileName + "." + PREImgV2_ext.Replace(".", "");
                                                                                                #endregion Para
                                                                                                #region sql
                                                                                                try
                                                                                                {
                                                                                                    string[] rs = c.AddCustRealEstateImgFromDevice(PREImgV2_imageClientID, CollateralServerID
                                                                                                    , PREImgV2_collateralClientID, PREImgV2_createDateClient, PREImgV2_ext, RealEstate_fName);

                                                                                                    ERR = rs[0];
                                                                                                    SMS = rs[1];
                                                                                                    string RealEstateImageServerID = rs[2];
                                                                                                    ExSMS = rs[3];

                                                                                                    if (ERR != "Error")
                                                                                                    {
                                                                                                        LoanAppResImgList ilist = new LoanAppResImgList();
                                                                                                        ilist.ImgType = "RealEstateImg";
                                                                                                        ilist.ClientID = PREImgV2_imageClientID;
                                                                                                        ilist.ServerID = RealEstateImageServerID;
                                                                                                        ilist.OriImgName = PREImgV2_imgPath;
                                                                                                        ilist.ServerImgName = RealEstate_fName;
                                                                                                        ImgList.Add(ilist);
                                                                                                    }
                                                                                                }
                                                                                                catch (Exception ex)
                                                                                                {
                                                                                                    ERR = "Error";
                                                                                                    SMS = "Cannot read RealEstate image";
                                                                                                    ExSMS = ex.Message.ToString();
                                                                                                }
                                                                                                #endregion sql
                                                                                            }
                                                                                            catch (Exception ex)
                                                                                            {
                                                                                                ERR = "Error";
                                                                                                SMS = "Cannot read RealEstate image";
                                                                                                ExSMS = ex.Message.ToString();
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                            catch (Exception ex)
                                                                            {
                                                                                ERR = "Error";
                                                                                SMS = "Cannot read RealEstate image";
                                                                                ExSMS = ex.Message.ToString();
                                                                            }
                                                                        }
                                                                        #endregion img
                                                                    }
                                                                    catch (Exception ex)
                                                                    {
                                                                        ERR = "Error";
                                                                        SMS = "Cannot read RealEstate";
                                                                        ExSMS = ex.Message.ToString();
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        //if Error
                                                        if (ERR == "Error")
                                                        {
                                                            LoanAppResSMS data = new LoanAppResSMS();
                                                            data.SMS = "RealEstate: " + ExSMS;
                                                            SMSList.Add(data);
                                                        }
                                                    }
                                                    #endregion RealEstate

                                                    #region PersonDeposit
                                                    if (ERR != "Error")
                                                    {
                                                        try
                                                        {

                                                            if (Person.PersonDeposit != null)
                                                            {
                                                                foreach (var PersonDeposit in Person.PersonDeposit)
                                                                {
                                                                    if (ERR != "Error")
                                                                    {
                                                                        try
                                                                        {
                                                                            #region Para
                                                                            string dep_CollateralClientID = PersonDeposit.CollateralClientID;
                                                                            string deposit_collateralServerID = PersonDeposit.CollateralServerID;
                                                                            string dep_LoanClientID = PersonDeposit.LoanClientID;
                                                                            string deposit_loanAppID = PersonDeposit.LoanAppID;
                                                                            string dep_CustClientID = PersonDeposit.CustClientID;
                                                                            string deposit_custServerID = PersonDeposit.CustServerID;
                                                                            string FixedDepositAccountNo = PersonDeposit.FixedDepositAccountNo;
                                                                            string dep_StartDate = PersonDeposit.StartDate;
                                                                            string dep_MaturityDate = PersonDeposit.MaturityDate;
                                                                            string dep_Amount = PersonDeposit.Amount;
                                                                            string dep_AccountOwnerName = PersonDeposit.AccountOwnerName;
                                                                            string dep_Currency = PersonDeposit.Currency;
                                                                            string dep_RelationshipID = PersonDeposit.RelationshipID;
                                                                            string dep_DOB = PersonDeposit.DOB;
                                                                            string dep_GenderID = PersonDeposit.GenderID;
                                                                            string dep_NIDNo = PersonDeposit.NIDNo;
                                                                            string dep_IssueDate = PersonDeposit.IssueDate;
                                                                            string dep_IssuedBy = PersonDeposit.IssuedBy;
                                                                            string dep_SortAddress = PersonDeposit.SortAddress;
                                                                            string dep_VillageID = PersonDeposit.VillageID;
                                                                            #endregion Para
                                                                            #region sql
                                                                            try
                                                                            {
                                                                                string[] rs = c.AddCustDepositFromDevice(dep_CollateralClientID, ServerLoanAppID
                                                                                , dep_LoanClientID, ServerLoanAppPersonID, dep_CustClientID, FixedDepositAccountNo
                                                                                , dep_StartDate, dep_MaturityDate, dep_Amount, dep_AccountOwnerName, dep_Currency
                                                                                , dep_RelationshipID, dep_DOB, dep_GenderID, dep_NIDNo, dep_IssueDate, dep_IssuedBy
                                                                                , dep_SortAddress, dep_VillageID);

                                                                                ERR = rs[0];
                                                                                SMS = rs[1];
                                                                                string CollateralServerID = rs[2];
                                                                                ExSMS = rs[3];

                                                                            }
                                                                            catch (Exception ex)
                                                                            {
                                                                                ERR = "Error";
                                                                                SMS = "Cannot read Deposit";
                                                                                ExSMS = ex.Message.ToString();
                                                                            }
                                                                            #endregion sql
                                                                        }
                                                                        catch (Exception ex)
                                                                        {
                                                                            ERR = "Error";
                                                                            SMS = "Cannot read Deposit";
                                                                            ExSMS = ex.Message.ToString();
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            ERR = "Error";
                                                            SMS = "Cannot read Deposit";
                                                            ExSMS = ex.Message.ToString();
                                                        }
                                                        //if Error
                                                        if (ERR == "Error")
                                                        {
                                                            LoanAppResSMS data = new LoanAppResSMS();
                                                            data.SMS = "PersonDeposit: " + ExSMS;
                                                            SMSList.Add(data);
                                                        }
                                                    }
                                                    #endregion PersonDeposit
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                ERR = "Error";
                                                SMS = "Cannot read customer parameter 2 " + ex.Message.ToString();
                                                ExSMS = ex.Message.ToString();
                                            }

                                            //if Error
                                            if (ERR == "Error")
                                            {
                                                LoanAppResSMS data = new LoanAppResSMS();
                                                data.SMS = "Person: " + ExSMS;
                                                SMSList.Add(data);
                                            }
                                        }
                                    }
                                }

                                #endregion Person

                                #region Purpose
                                if (ERR != "Error")
                                {
                                    try
                                    {

                                        if (loan.Purpose != null)
                                        {
                                            foreach (var Purpose in loan.Purpose)
                                            {
                                                if (ERR != "Error")
                                                {
                                                    try
                                                    {
                                                        string LoanPurposeServerID = "0";
                                                        #region Para
                                                        string LoanPurposeClientID = Purpose.LoanPurposeClientID;
                                                        string purpose_loanPurposeServerID = Purpose.LoanPurposeServerID;
                                                        string purpose_loanClientID = Purpose.LoanClientID;
                                                        string purpose_loanAppID = Purpose.LoanAppID;
                                                        string pur_LoanPurposeID = Purpose.LoanPurposeID;
                                                        #endregion Para
                                                        #region sql
                                                        try
                                                        {

                                                            string[] rs = c.AddLoanApp11PurpsoeFromDevice(LoanPurposeClientID, ServerLoanAppID, pur_LoanPurposeID);

                                                            ERR = rs[0];
                                                            SMS = rs[1];
                                                            LoanPurposeServerID = rs[2];
                                                            ExSMS = rs[3];

                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            ERR = "Error";
                                                            SMS = "Cannot add customer image to switch DB";
                                                            ExSMS = ex.Message.ToString();
                                                        }
                                                        #endregion sql

                                                        #region Purpose Detail
                                                        if (ERR != "Error")
                                                        {
                                                            try
                                                            {

                                                                if (Purpose.PurposeDetail != null)
                                                                {
                                                                    foreach (var PurposeDetail in Purpose.PurposeDetail)
                                                                    {
                                                                        if (ERR != "Error")
                                                                        {
                                                                            #region Para
                                                                            string LoanPurposeDetailClientID = PurposeDetail.LoanPurposeDetailClientID;
                                                                            string purposeDetail_loanPurposeDetailServerID = PurposeDetail.LoanPurposeDetailServerID;
                                                                            string purposeDetail_loanPurposeClientID = PurposeDetail.LoanPurposeClientID;
                                                                            string purposeDetail_loanPurposeServerID = PurposeDetail.LoanPurposeServerID;
                                                                            string LoanAppPurpsoeDetail = PurposeDetail.LoanAppPurpsoeDetail;
                                                                            string pur_Quantity = PurposeDetail.Quantity;
                                                                            string pur_UnitPrice = PurposeDetail.UnitPrice;
                                                                            #endregion Para
                                                                            #region sql
                                                                            try
                                                                            {
                                                                                string[] rs = c.AddLoanApp11PurpsoeDetailFromDevice(LoanPurposeDetailClientID, ServerLoanAppID, LoanAppPurpsoeDetail, pur_Quantity, pur_UnitPrice, LoanPurposeServerID);

                                                                                ERR = rs[0];
                                                                                SMS = rs[1];
                                                                                string ServerLoanAppPurpsoeDetailID = rs[2];
                                                                                ExSMS = rs[3];

                                                                            }
                                                                            catch (Exception ex)
                                                                            {
                                                                                ERR = "Error";
                                                                                SMS = "Cannot read Purpose Detail";
                                                                                ExSMS = ex.Message.ToString();
                                                                            }
                                                                            #endregion sql
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            catch (Exception ex)
                                                            {
                                                                ERR = "Error";
                                                                SMS = "Cannot read Purpose Detail";
                                                                ExSMS = ex.Message.ToString();
                                                            }
                                                        }
                                                        #endregion Purpose Detail
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        ERR = "Error";
                                                        SMS = "Cannot read Purpose";
                                                        ExSMS = ex.Message.ToString();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        ERR = "Error";
                                        SMS = "Cannot read Purpose";
                                        ExSMS = ex.Message.ToString();
                                    }
                                    //if Error
                                    if (ERR == "Error")
                                    {
                                        LoanAppResSMS data = new LoanAppResSMS();
                                        data.SMS = "Purpose: " + ExSMS;
                                        SMSList.Add(data);
                                    }
                                }
                                #endregion Purpose

                                #region CashFlow
                                if (ERR != "Error")
                                {
                                    try
                                    {

                                        if (loan.CashFlow != null)
                                        {
                                            foreach (var CashFlow in loan.CashFlow)
                                            {
                                                string ServerLoanAppCashFlowID = "0";
                                                #region Para
                                                string CashFlowClientID = CashFlow.CashFlowClientID;
                                                string cashFlow_cashFlowServerID = CashFlow.CashFlowServerID;
                                                string cashFlow_loanClientID = CashFlow.LoanClientID;
                                                string cashFlow_loanAppID = CashFlow.LoanAppID;
                                                string StudyMonthAmount = CashFlow.StudyMonthAmount;
                                                string StudyStartMonth = CashFlow.StudyStartMonth;
                                                string FamilyExpensePerMonth = CashFlow.FamilyExpensePerMonth;
                                                string OtherExpensePerMonth = CashFlow.OtherExpensePerMonth;
                                                #endregion Para
                                                #region sql
                                                try
                                                {
                                                    string[] rs = c.AddLoanApp51CashFlowFromDevice(ServerLoanAppID, CashFlowClientID
                                                        , StudyMonthAmount, StudyStartMonth, FamilyExpensePerMonth, OtherExpensePerMonth);

                                                    ERR = rs[0];
                                                    SMS = rs[1];
                                                    ServerLoanAppCashFlowID = rs[2];
                                                    ExSMS = rs[3];

                                                }
                                                catch (Exception ex)
                                                {
                                                    ERR = "Error";
                                                    SMS = "Cannot read cash flow";
                                                    ExSMS = ex.Message.ToString();
                                                }
                                                #endregion sql

                                                #region CashFlowMSI
                                                if (ERR != "Error")
                                                {
                                                    try
                                                    {

                                                        if (CashFlow.CashFlowMSI != null)
                                                        {
                                                            foreach (var CashFlowMSI in CashFlow.CashFlowMSI)
                                                            {
                                                                string ServerLoanAppCashFlowMSIID = "0";
                                                                if (ERR != "Error")
                                                                {
                                                                    #region Para
                                                                    string cashFlowMSI_cashFlowMSIClientID = CashFlowMSI.CashFlowMSIClientID;
                                                                    string cashFlowMSI_cashFlowMSIServerID = CashFlowMSI.CashFlowMSIServerID;
                                                                    string cashFlowMSI_cashFlowClientID = CashFlowMSI.CashFlowClientID;
                                                                    string cashFlowMSI_cashFlowServerID = CashFlowMSI.CashFlowServerID;
                                                                    string IncomeTypeID = CashFlowMSI.IncomeTypeID;
                                                                    string MSI_OccupationTypeID = CashFlowMSI.OccupationTypeID;
                                                                    string MainSourceIncomeID = CashFlowMSI.MainSourceIncomeID;
                                                                    string MSI_ExAge = CashFlowMSI.ExAge;
                                                                    string MSI_BusAge = CashFlowMSI.BusAge;
                                                                    string isMSI = CashFlowMSI.isMSI;
                                                                    string IncomeOwnerID = "";
                                                                    string WorkingPlaceID = "";
                                                                    string PhoneNumber = "";
                                                                    string Name = "";

                                                                    #endregion Para
                                                                    #region sql
                                                                    try
                                                                    {
                                                                        string[] rs = c.AddLoanAppCashFlowMSIFromDevice(ServerLoanAppCashFlowID, IncomeTypeID
                                                                            , MainSourceIncomeID, "", "0", MSI_ExAge, MSI_BusAge, isMSI, MSI_OccupationTypeID, IncomeOwnerID, WorkingPlaceID, PhoneNumber,Name);

                                                                        ERR = rs[0];
                                                                        SMS = rs[1];
                                                                        ServerLoanAppCashFlowMSIID = rs[2];
                                                                        ExSMS = rs[3];

                                                                    }
                                                                    catch (Exception ex)
                                                                    {
                                                                        ERR = "Error";
                                                                        SMS = "Cannot read cash flow";
                                                                        ExSMS = ex.Message.ToString();
                                                                    }
                                                                    #endregion sql

                                                                    #region CashFlowMSIInEx
                                                                    if (ERR != "Error")
                                                                    {
                                                                        try
                                                                        {

                                                                            if (CashFlowMSI.CashFlowMSIInEx != null)
                                                                            {
                                                                                foreach (var CashFlowMSIInEx in CashFlowMSI.CashFlowMSIInEx)
                                                                                {
                                                                                    if (ERR != "Error")
                                                                                    {
                                                                                        #region Para
                                                                                        string LoanAppCashFlowMSIInExClientID = CashFlowMSIInEx.LoanAppCashFlowMSIInExClientID;
                                                                                        string mSIInEx_loanAppCashFlowMSIInExServerID = CashFlowMSIInEx.LoanAppCashFlowMSIInExServerID;
                                                                                        string mSIInEx_cashFlowMSIClientID = CashFlowMSIInEx.CashFlowMSIClientID;
                                                                                        string mSIInEx_cashFlowMSIServerID = CashFlowMSIInEx.CashFlowMSIServerID;
                                                                                        string InExCodeID = CashFlowMSIInEx.InExCodeID;
                                                                                        string MSIInEx_Description = CashFlowMSIInEx.Description;
                                                                                        string MSIInEx_Month = CashFlowMSIInEx.Month;
                                                                                        string MSIInEx_Mount = CashFlowMSIInEx.Amount;
                                                                                        string MSIInEx_UnitID = CashFlowMSIInEx.UnitID;
                                                                                        string MSIInEx_Cost = CashFlowMSIInEx.Cost;
                                                                                        string OneIncomeTwoExpense = CashFlowMSIInEx.OneIncomeTwoExpense;
                                                                                        #endregion Para
                                                                                        #region sql
                                                                                        try
                                                                                        {

                                                                                            string[] rs = c.AddLoanAppCashFlowMSIInExFromDevice(LoanAppCashFlowMSIInExClientID
                                                                                                , ServerLoanAppCashFlowMSIID, InExCodeID, MSIInEx_Description, MSIInEx_Month
                                                                                                , MSIInEx_Mount, MSIInEx_UnitID, MSIInEx_Cost, OneIncomeTwoExpense);

                                                                                            ERR = rs[0];
                                                                                            SMS = rs[1];
                                                                                            string ServerLoanAppCashFlowMSIInExID = rs[2];
                                                                                            ExSMS = rs[3];

                                                                                        }
                                                                                        catch (Exception ex)
                                                                                        {
                                                                                            ERR = "Error";
                                                                                            SMS = "Cannot read cash flow msi InEx";
                                                                                            ExSMS = ex.Message.ToString();
                                                                                        }
                                                                                        #endregion sql
                                                                                    }
                                                                                }
                                                                            }

                                                                        }
                                                                        catch (Exception ex)
                                                                        {
                                                                            ERR = "Error";
                                                                            SMS = "Cannot read cash flow msi InEx";
                                                                            ExSMS = ex.Message.ToString();
                                                                        }
                                                                    }
                                                                    #endregion CashFlowMSIInEx
                                                                }
                                                            }
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        ERR = "Error";
                                                        SMS = "Cannot read cash flow msi";
                                                        ExSMS = ex.Message.ToString();
                                                    }
                                                }
                                                #endregion CashFlowMSI
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        ERR = "Error";
                                        SMS = "Cannot read cash flow";
                                        ExSMS = ex.Message.ToString();
                                    }
                                    //if Error
                                    if (ERR == "Error")
                                    {
                                        LoanAppResSMS data = new LoanAppResSMS();
                                        data.SMS = "CashFlow: " + SMS + "-" + ExSMS;
                                        SMSList.Add(data);
                                    }
                                }
                                #endregion CashFlow

                                #region Update Ori CO
                                if (ERR != "Error")
                                {
                                    try
                                    {
                                        string sql = "exec T24_LoanAppUpdateOriCO @VBID='" + VillageBankIDForGetCOIDOri + "',@LoanAppID='" + ServerLoanAppID + "'";
                                        DataTable dt2 = c.ReturnDT(sql);
                                        if (dt2.Rows[0][0].ToString() == "0")
                                        {
                                            ERR = "Error";
                                            SMS = dt2.Rows[0][1].ToString();
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        ERR = "Error";
                                        SMS = "Cannot Error Update Ori CO";
                                        ExSMS = ex.Message.ToString();
                                    }
                                    //if Error
                                    if (ERR == "Error")
                                    {
                                        LoanAppResSMS data = new LoanAppResSMS();
                                        data.SMS = "UpdateOriCO: " + ExSMS;
                                        SMSList.Add(data);
                                    }
                                }
                                #endregion

                            }
                        }
                    }
                    catch { }
                }
                #region old
                //if (ERR != "Error")
                //{
                //    #region add
                //    if (SMSList.Count > 0)
                //    {
                //        ERR = "Error";
                //        SMS = "Something was wrong in LoanApp";
                //    }
                //    else
                //    {
                //        string ServerLoanAppID = "0";
                //        SqlConnection Con1 = new SqlConnection(c.ConStr());
                //        Con1.Open();
                //        SqlCommand Com1 = new SqlCommand();
                //        Com1.Connection = Con1;
                //        string sql = "";
                //        try
                //        {
                //            foreach (var loan in jObj.LoanApp)
                //            {
                //                #region LoanApp 
                //                #region LoanApp Param                           
                //                string IDOnDevice = loan.IDOnDevice;
                //                RSIDOnDevice = IDOnDevice;
                //                //string LoanAppID = loan.LoanAppID;
                //                string LoanAppID = "0";
                //                //RSLoanAppID = LoanAppID;
                //                //string LoanAppStatusID = loan.LoanAppStatusID;
                //                string LoanAppStatusID = "3";
                //                string DeviceDate = loan.DeviceDate;
                //                string ProductID = loan.ProductID;
                //                string LoanRequestAmount = loan.LoanRequestAmount;
                //                string LoanPurposeID1 = loan.LoanPurposeID1;
                //                string LoanPurposeID2 = loan.LoanPurposeID2;
                //                string LoanPurposeID3 = loan.LoanPurposeID3;
                //                string OwnCapital = loan.OwnCapital;
                //                string DisbursementDate = loan.DisbursementDate;
                //                string FirstWithdrawal = loan.FirstWithdrawal;
                //                string LoanTerm = loan.LoanTerm;
                //                string FirstRepaymentDate = loan.FirstRepaymentDate;
                //                string LoanInterestRate = loan.LoanInterestRate;
                //                string CustomerRequestRate = loan.CustomerRequestRate;
                //                string CompititorRate = loan.CompititorRate;
                //                string CustomerConditionID = loan.CustomerConditionID;
                //                string COProposedAmount = loan.COProposedAmount;
                //                string COProposedTerm = loan.COProposedTerm;
                //                string COProposeRate = loan.COProposeRate;
                //                string FrontBackOfficeID = loan.FrontBackOfficeID;
                //                string GroupNumber = loan.GroupNumber;
                //                string LoanCycleID = loan.LoanCycleID;
                //                string RepaymentHistoryID = loan.RepaymentHistoryID;
                //                string LoanReferralID = loan.LoanReferralID;
                //                string DebtIinfoID = loan.DebtIinfoID;
                //                string MonthlyFee = loan.MonthlyFee;
                //                string Compulsory = loan.Compulsory;
                //                //string CompulsoryTerm = loan.CompulsoryTerm;
                //                string CompulsoryTerm = "0";
                //                string Currency = loan.Currency;
                //                string UpFrontFee = loan.UpFrontFee;
                //                string UpFrontAmt = loan.UpFrontAmt;
                //                //string isCBCCheck = loan.isCBCCheck;
                //                string CompulsoryOptionID = loan.CompulsoryOptionID;
                //                string FundSource = loan.FundSource;
                //                string IsNewCollateral = loan.IsNewCollateral;
                //                #endregion LoanApp Param                                
                //                #region sql add loan
                //                sql = "exec sp_LoanApp1Add2 @IDOnDevice=@IDOnDevice,@LoanAppID=@LoanAppID,@LoanAppStatusID=@LoanAppStatusID,@DeviceDate=@DeviceDate"
                //                + ",@ServerDate=@ServerDate,@CreateBy=@CreateBy,@ProductID=@ProductID,@LoanRequestAmount=@LoanRequestAmount"
                //                + ",@LoanPurposeID1=@LoanPurposeID1,@LoadPurposeID2=@LoadPurposeID2,@LoadPurposeID3=@LoadPurposeID3,@OwnCapital=@OwnCapital"
                //                + ",@DisbursementDate=@DisbursementDate,@FirstWithdrawal=@FirstWithdrawal,@LoanTerm=@LoanTerm,@FirstRepaymentDate=@FirstRepaymentDate"
                //                + ",@LoanInterestRate=@LoanInterestRate,@CustomerRequestRate=@CustomerRequestRate,@CompititorRate=@CompititorRate"
                //                + ",@CustomerConditionID=@CustomerConditionID,@COProposedAmount=@COProposedAmount,@COProposedTerm=@COProposedTerm"
                //                + ",@COProposeRate=@COProposeRate,@FrontBackOfficeID=@FrontBackOfficeID,@GroupNumber=@GroupNumber,@LoanCycleID=@LoanCycleID"
                //                + ",@RepaymentHistoryID=@RepaymentHistoryID,@LoanReferralID=@LoanReferralID,@DebtIinfoID=@DebtIinfoID,@MonthlyFee=@MonthlyFee"
                //                + ",@Compulsory=@Compulsory,@CompulsoryTerm=@CompulsoryTerm,@Currency=@Currency,@UpFrontFee=@UpFrontFee"
                //                + ",@UpFrontAmt=@UpFrontAmt,@CompulsoryOptionID=@CompulsoryOptionID,@isCBCCheck=@isCBCCheck,@FundSource=@FundSource"
                //                + ",@IsNewCollateral=@IsNewCollateral";
                //                Com1.CommandText = sql;
                //                Com1.Parameters.Clear();
                //                Com1.Parameters.AddWithValue("@IDOnDevice", IDOnDevice);
                //                Com1.Parameters.AddWithValue("@LoanAppID", LoanAppID);
                //                Com1.Parameters.AddWithValue("@LoanAppStatusID", LoanAppStatusID);
                //                Com1.Parameters.AddWithValue("@DeviceDate", DeviceDate);
                //                Com1.Parameters.AddWithValue("@ServerDate", ServerDate);
                //                Com1.Parameters.AddWithValue("@CreateBy", UserID);
                //                Com1.Parameters.AddWithValue("@ProductID", ProductID);
                //                Com1.Parameters.AddWithValue("@LoanRequestAmount", LoanRequestAmount);
                //                Com1.Parameters.AddWithValue("@LoanPurposeID1", LoanPurposeID1);
                //                Com1.Parameters.AddWithValue("@LoadPurposeID2", LoanPurposeID2);
                //                Com1.Parameters.AddWithValue("@LoadPurposeID3", LoanPurposeID3);
                //                Com1.Parameters.AddWithValue("@OwnCapital", OwnCapital);
                //                Com1.Parameters.AddWithValue("@DisbursementDate", DisbursementDate);
                //                Com1.Parameters.AddWithValue("@FirstWithdrawal", FirstWithdrawal);
                //                Com1.Parameters.AddWithValue("@LoanTerm", LoanTerm);
                //                Com1.Parameters.AddWithValue("@FirstRepaymentDate", FirstRepaymentDate);
                //                Com1.Parameters.AddWithValue("@LoanInterestRate", LoanInterestRate);
                //                Com1.Parameters.AddWithValue("@CustomerRequestRate", CustomerRequestRate);
                //                Com1.Parameters.AddWithValue("@CompititorRate", CompititorRate);
                //                Com1.Parameters.AddWithValue("@CustomerConditionID", CustomerConditionID);
                //                Com1.Parameters.AddWithValue("@COProposedAmount", COProposedAmount);
                //                Com1.Parameters.AddWithValue("@COProposedTerm", COProposedTerm);
                //                Com1.Parameters.AddWithValue("@COProposeRate", COProposeRate);
                //                Com1.Parameters.AddWithValue("@FrontBackOfficeID", FrontBackOfficeID);
                //                Com1.Parameters.AddWithValue("@GroupNumber", GroupNumber);
                //                Com1.Parameters.AddWithValue("@LoanCycleID", LoanCycleID);
                //                Com1.Parameters.AddWithValue("@RepaymentHistoryID", RepaymentHistoryID);
                //                Com1.Parameters.AddWithValue("@LoanReferralID", LoanReferralID);
                //                Com1.Parameters.AddWithValue("@DebtIinfoID", DebtIinfoID);
                //                Com1.Parameters.AddWithValue("@MonthlyFee", MonthlyFee);
                //                Com1.Parameters.AddWithValue("@Compulsory", Compulsory);
                //                Com1.Parameters.AddWithValue("@CompulsoryTerm", CompulsoryTerm);
                //                Com1.Parameters.AddWithValue("@Currency", Currency);
                //                Com1.Parameters.AddWithValue("@UpFrontFee", UpFrontFee);
                //                Com1.Parameters.AddWithValue("@UpFrontAmt", UpFrontAmt);
                //                Com1.Parameters.AddWithValue("@CompulsoryOptionID", CompulsoryOptionID);
                //                Com1.Parameters.AddWithValue("@isCBCCheck", "1");
                //                Com1.Parameters.AddWithValue("@FundSource", FundSource);
                //                Com1.Parameters.AddWithValue("@IsNewCollateral", IsNewCollateral);
                //                DataTable dt1 = new DataTable();
                //                dt1.Load(Com1.ExecuteReader());
                //                ServerLoanAppID = dt1.Rows[0][0].ToString();
                //                //ServerLoanAppID = "1";
                //                RSLoanAppID = ServerLoanAppID;
                //                #endregion sql
                //                #region update Inst
                //                try
                //                {
                //                    if (InstID.Length > 1)
                //                    {
                //                        InstID = "1";
                //                    }
                //                    c.ReturnDT("update tblLoanApp1 set InstID='" + InstID + "' where LoanAppID='" + ServerLoanAppID + "'");
                //                }
                //                catch { }
                //                #endregion update Inst
                //                #endregion LoanApp
                //                #region PurpsoeDetail
                //                foreach (var lp in loan.PurposeDetail)
                //                {
                //                    #region param
                //                    string LoanAppPurpsoeDetail = lp.LoanAppPurposeDetail;
                //                    string Quantity = lp.Quantity;
                //                    string UnitPrice = lp.UnitPrice;
                //                    #endregion param
                //                    #region sql
                //                    sql = "exec sp_LoanApp11Add2 @IDOnDevice=@IDOnDevice,@LoanAppID=@LoanAppID,@LoanAppPurpsoeDetail=@LoanAppPurpsoeDetail"
                //                    + ",@Quantity=@Quantity,@UnitPrice=@UnitPrice";
                //                    Com1.CommandText = sql;
                //                    Com1.Parameters.Clear();
                //                    Com1.Parameters.AddWithValue("@IDOnDevice", IDOnDevice);
                //                    Com1.Parameters.AddWithValue("@LoanAppID", ServerLoanAppID);
                //                    Com1.Parameters.AddWithValue("@LoanAppPurpsoeDetail", LoanAppPurpsoeDetail);
                //                    Com1.Parameters.AddWithValue("@Quantity", Quantity);
                //                    Com1.Parameters.AddWithValue("@UnitPrice", UnitPrice);
                //                    Com1.ExecuteNonQuery();
                //                    #endregion sql
                //                }
                //                #endregion PurpsoeDetail
                //                #region Person
                //                foreach (var lp in loan.Person)
                //                {
                //                    #region Person
                //                    string ServerLoanAppPersonID = "";
                //                    #region Person Param
                //                    string LoanAppPersonID = lp.LoanAppPersonID;
                //                    string LoanAppPersonTypeID = lp.LoanAppPersonTypeID;
                //                    string PersonID = lp.PersonID;
                //                    string CustomerID = lp.CustomerID;
                //                    string Number = lp.Number;
                //                    string VillageBankID = lp.VillageBankID;
                //                    string AltName = lp.AltName;
                //                    string TitleID = lp.TitleID;
                //                    string LastName = lp.LastName;
                //                    string FirstName = lp.FirstName;
                //                    string GenderID = lp.GenderID;
                //                    if (GenderID == "MALE")
                //                    {
                //                        GenderID = "0";
                //                    }
                //                    if (GenderID == "FEMALE")
                //                    {
                //                        GenderID = "1";
                //                    }
                //                    string DateOfBirth = lp.DateOfBirth;
                //                    string IDCardTypeID = lp.IDCardTypeID;
                //                    string IDCardNumber = lp.IDCardNumber;
                //                    string IDCardExpireDate = lp.IDCardExpireDate;
                //                    string IDCardIssuedDate = lp.IDCardIssuedDate;
                //                    string MaritalStatusID = lp.MaritalStatusID;
                //                    string EducationID = lp.EducationID;
                //                    string CityOfBirthID = lp.CityOfBirthID;
                //                    string Telephone3 = lp.Telephone3;
                //                    string VillageIDPermanent = lp.VillageIDPermanent;
                //                    string LocationCodeIDPermanent = lp.LocationCodeIDPermanent;
                //                    string VillageIDCurrent = lp.VillageIDCurrent;
                //                    string LocationCodeIDCurrent = lp.LocationCodeIDCurrent;
                //                    string SortAddress = lp.SortAddress;
                //                    string FamilyMember = lp.FamilyMember;
                //                    string FamilyMemberActive = lp.FamilyMemberActive;
                //                    string PoorID = lp.PoorID;
                //                    string DeviceDate_Person = lp.DeviceDate;
                //                    string AltName2 = lp.AltName2;
                //                    #endregion Person Param
                //                    #region sql
                //                    sql = "exec T24_LoanAppPersonAddEdit @IDOnDevice=@IDOnDevice,@LoanAppID=@LoanAppID,@LoanAppPersonTypeID=@LoanAppPersonTypeID"
                //                    + ",@PersonID=@PersonID,@CustomerID=@CustomerID,@Number=@Number,@VillageBankID=@VillageBankID,@AltName=@AltName,@TitleID=@TitleID"
                //                    + ",@LastName=@LastName,@FirstName=@FirstName,@FullName=@FullName,@GenderID=@GenderID,@DateOfBirth=@DateOfBirth"
                //                    + ",@IDCardTypeID=@IDCardTypeID,@IDCardNumber=@IDCardNumber,@IDCardExpireDate=@IDCardExpireDate,@MaritalStatusID=@MaritalStatusID"
                //                    + ",@EducationID=@EducationID,@CityOfBirthID=@CityOfBirthID,@Telephone3=@Telephone3,@BranchID=@BranchID"
                //                    + ",@VillageIDPermanent=@VillageIDPermanent"
                //                    + ",@LocationCodeIDPermanent=@LocationCodeIDPermanent"
                //                    + ",@VillageIDCurrent=@VillageIDCurrent,@LocationCodeIDCurrent=@LocationCodeIDCurrent"
                //                    + ",@SortAddress=@SortAddress,@FamilyMember=@FamilyMember,@FamilyMemberActive=@FamilyMemberActive,@PoorID=@PoorID"
                //                    + ",@OfficeNameID=@OfficeNameID,@LoanAppPersonIDOnDevice=@LoanAppPersonIDOnDevice,@DeviceDate=@DeviceDate"
                //                    + ",@IDCardIssuedDate=@IDCardIssuedDate,@AltName2=@AltName2";
                //                    Com1.CommandText = sql;
                //                    Com1.Parameters.Clear();
                //                    Com1.Parameters.AddWithValue("@IDOnDevice", IDOnDevice);
                //                    Com1.Parameters.AddWithValue("@LoanAppID", ServerLoanAppID);
                //                    Com1.Parameters.AddWithValue("@LoanAppPersonTypeID", LoanAppPersonTypeID);
                //                    Com1.Parameters.AddWithValue("@PersonID", PersonID);
                //                    Com1.Parameters.AddWithValue("@CustomerID", CustomerID);
                //                    Com1.Parameters.AddWithValue("@Number", Number);
                //                    Com1.Parameters.AddWithValue("@VillageBankID", VillageBankID);
                //                    Com1.Parameters.AddWithValue("@AltName", AltName);
                //                    Com1.Parameters.AddWithValue("@TitleID", TitleID);
                //                    Com1.Parameters.AddWithValue("@LastName", LastName);
                //                    Com1.Parameters.AddWithValue("@FirstName", FirstName);
                //                    Com1.Parameters.AddWithValue("@FullName", "");
                //                    Com1.Parameters.AddWithValue("@GenderID", GenderID);
                //                    Com1.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
                //                    Com1.Parameters.AddWithValue("@IDCardTypeID", IDCardTypeID);
                //                    Com1.Parameters.AddWithValue("@IDCardNumber", IDCardNumber);
                //                    Com1.Parameters.AddWithValue("@IDCardExpireDate", IDCardExpireDate);
                //                    Com1.Parameters.AddWithValue("@MaritalStatusID", MaritalStatusID);
                //                    Com1.Parameters.AddWithValue("@EducationID", EducationID);
                //                    Com1.Parameters.AddWithValue("@CityOfBirthID", CityOfBirthID);
                //                    Com1.Parameters.AddWithValue("@Telephone3", Telephone3);
                //                    Com1.Parameters.AddWithValue("@BranchID", "0");
                //                    Com1.Parameters.AddWithValue("@VillageIDPermanent", VillageIDPermanent);
                //                    Com1.Parameters.AddWithValue("@LocationCodeIDPermanent", LocationCodeIDPermanent);
                //                    Com1.Parameters.AddWithValue("@VillageIDCurrent", VillageIDCurrent);
                //                    Com1.Parameters.AddWithValue("@LocationCodeIDCurrent", LocationCodeIDCurrent);
                //                    Com1.Parameters.AddWithValue("@SortAddress", SortAddress);
                //                    Com1.Parameters.AddWithValue("@FamilyMember", FamilyMember);
                //                    Com1.Parameters.AddWithValue("@FamilyMemberActive", FamilyMemberActive);
                //                    Com1.Parameters.AddWithValue("@PoorID", PoorID);
                //                    Com1.Parameters.AddWithValue("@OfficeNameID", "0");
                //                    Com1.Parameters.AddWithValue("@LoanAppPersonIDOnDevice", LoanAppPersonID);
                //                    Com1.Parameters.AddWithValue("@DeviceDate", DeviceDate);
                //                    Com1.Parameters.AddWithValue("@IDCardIssuedDate", IDCardIssuedDate);
                //                    Com1.Parameters.AddWithValue("@AltName2", AltName2);
                //                    DataTable dt2 = new DataTable();
                //                    dt2.Load(Com1.ExecuteReader());
                //                    ServerLoanAppPersonID = dt2.Rows[0][0].ToString();
                //                    //ServerLoanAppPersonID = "1";
                //                    #endregion sql

                //                    #endregion Person

                //                    #region Creditor 
                //                    foreach (var lp2 in lp.Creditor)
                //                    {
                //                        #region Parama
                //                        string CreditorID = lp2.CreditorID;
                //                        string ApprovedAmount = lp2.ApprovedAmount;
                //                        string OutstandingBalance = lp2.OutstandingBalance;
                //                        string InterestRate = lp2.InterestRate;
                //                        string RepaymentTypeID = lp2.RepaymentTypeID;
                //                        string RepaymentTermID = lp2.RepaymentTermID;
                //                        string LoanStartDate = lp2.LoanStartDate;
                //                        string LoanEndDate = lp2.LoanEndDate;
                //                        string RemainingInstallment = lp2.RemainingInstallment;
                //                        string LoanAppCreditorID = lp2.LoanAppCreditorID;
                //                        #endregion Parama
                //                        #region sql
                //                        sql = "exec T24_LoanAppCreditorAddEdit @ServerLoanAppID=@ServerLoanAppID,@ServerLoanAppPersonID=@ServerLoanAppPersonID"
                //                        + ",@CreditorID=@CreditorID,@ApprovedAmount=@ApprovedAmount,@OutstandingBalance=@OutstandingBalance,@InterestRate=@InterestRate"
                //                        + ",@RepaymentTypeID=@RepaymentTypeID,@RepaymentTermID=@RepaymentTermID,@LoanStartDate=@LoanStartDate"
                //                        + ",@LoanEndDate=@LoanEndDate,@RemainingInstallment=@RemainingInstallment";
                //                        Com1.CommandText = sql;
                //                        Com1.Parameters.Clear();
                //                        Com1.Parameters.AddWithValue("@ServerLoanAppID", ServerLoanAppID);
                //                        Com1.Parameters.AddWithValue("@ServerLoanAppPersonID", ServerLoanAppPersonID);
                //                        Com1.Parameters.AddWithValue("@CreditorID", CreditorID);
                //                        Com1.Parameters.AddWithValue("@ApprovedAmount", ApprovedAmount);
                //                        Com1.Parameters.AddWithValue("@OutstandingBalance", OutstandingBalance);
                //                        Com1.Parameters.AddWithValue("@InterestRate", InterestRate);
                //                        Com1.Parameters.AddWithValue("@RepaymentTypeID", RepaymentTypeID);
                //                        Com1.Parameters.AddWithValue("@RepaymentTermID", RepaymentTermID);
                //                        Com1.Parameters.AddWithValue("@LoanStartDate", LoanStartDate);
                //                        Com1.Parameters.AddWithValue("@LoanEndDate", LoanEndDate);
                //                        Com1.Parameters.AddWithValue("@RemainingInstallment", RemainingInstallment);
                //                        Com1.ExecuteNonQuery();
                //                        #endregion sql
                //                    }
                //                    #endregion Creditor 
                //                    #region ClientAsset
                //                    foreach (var lp2 in lp.ClientAsset)
                //                    {
                //                        #region Param
                //                        string Description = lp2.Description;
                //                        string Quantity = lp2.Quantity;
                //                        string UnitPrice = lp2.UnitPrice;
                //                        #endregion Param
                //                        #region sql
                //                        sql = "exec T24_LoanAppClientAssetAddEdit @ServerLoanAppPersonID=@ServerLoanAppPersonID,@ServerLoanAppID=@ServerLoanAppID"
                //                        + ",@Description=@Description,@Quantity=@Quantity,@UnitPrice=@UnitPrice";
                //                        Com1.CommandText = sql;
                //                        Com1.Parameters.Clear();
                //                        Com1.Parameters.AddWithValue("@ServerLoanAppID", ServerLoanAppID);
                //                        Com1.Parameters.AddWithValue("@ServerLoanAppPersonID", ServerLoanAppPersonID);
                //                        Com1.Parameters.AddWithValue("@Description", Description);
                //                        Com1.Parameters.AddWithValue("@Quantity", Quantity);
                //                        Com1.Parameters.AddWithValue("@UnitPrice", UnitPrice);
                //                        Com1.ExecuteNonQuery();
                //                        #endregion sql
                //                    }
                //                    #endregion ClientAsset
                //                    #region ClientBusiness
                //                    foreach (var lp2 in lp.ClientBusiness)
                //                    {
                //                        #region Param
                //                        string Description = lp2.Description;
                //                        string Quantity = lp2.Quantity;
                //                        string UnitPrice = lp2.UnitPrice;
                //                        #endregion Param
                //                        #region sql
                //                        sql = "exec T24_LoanAppClientBusinessAddEdit @ServerLoanAppPersonID=@ServerLoanAppPersonID,@ServerLoanAppID=@ServerLoanAppID"
                //                        + ",@Description=@Description,@Quantity=@Quantity,@UnitPrice=@UnitPrice";
                //                        Com1.CommandText = sql;
                //                        Com1.Parameters.Clear();
                //                        Com1.Parameters.AddWithValue("@ServerLoanAppID", ServerLoanAppID);
                //                        Com1.Parameters.AddWithValue("@ServerLoanAppPersonID", ServerLoanAppPersonID);
                //                        Com1.Parameters.AddWithValue("@Description", Description);
                //                        Com1.Parameters.AddWithValue("@Quantity", Quantity);
                //                        Com1.Parameters.AddWithValue("@UnitPrice", UnitPrice);
                //                        Com1.ExecuteNonQuery();
                //                        #endregion sql
                //                    }
                //                    #endregion ClientBusiness
                //                    #region ClientCollateral
                //                    foreach (var lp2 in lp.ClientCollateral)
                //                    {
                //                        #region Param
                //                        string ServerLoanAppClientCollateralID = "";
                //                        string LoanAppClientCollateralID = lp2.LoanAppClientCollateralID;
                //                        string ColleteralTypeID = lp2.ColleteralTypeID;
                //                        string ColleteralDocTypeID = lp2.ColleteralDocTypeID;
                //                        string ColleteralDocNumber = lp2.ColleteralDocNumber;
                //                        string Description = lp2.Description;
                //                        string Quantity = lp2.Quantity;
                //                        string UnitPrice = lp2.UnitPrice;
                //                        #endregion Param
                //                        #region sql
                //                        sql = "exec T24_LoanAppClientCollateralAddEdit @ServerLoanAppPersonID=@ServerLoanAppPersonID,@ServerLoanAppID=@ServerLoanAppID"
                //                        + ",@ColleteralTypeID=@ColleteralTypeID,@ColleteralDocTypeID=@ColleteralDocTypeID"
                //                        + ",@ColleteralDocNumber=@ColleteralDocNumber,@Description=@Description,@Quantity=@Quantity,@UnitPrice=@UnitPrice";
                //                        Com1.CommandText = sql;
                //                        Com1.Parameters.Clear();
                //                        Com1.Parameters.AddWithValue("@ServerLoanAppPersonID", ServerLoanAppPersonID);
                //                        Com1.Parameters.AddWithValue("@ServerLoanAppID", ServerLoanAppID);
                //                        Com1.Parameters.AddWithValue("@ColleteralTypeID", ColleteralTypeID);
                //                        Com1.Parameters.AddWithValue("@ColleteralDocTypeID", ColleteralDocTypeID);
                //                        Com1.Parameters.AddWithValue("@ColleteralDocNumber", ColleteralDocNumber);
                //                        Com1.Parameters.AddWithValue("@Description", Description);
                //                        Com1.Parameters.AddWithValue("@Quantity", Quantity);
                //                        Com1.Parameters.AddWithValue("@UnitPrice", UnitPrice);
                //                        DataTable dt3 = new DataTable();
                //                        dt3.Load(Com1.ExecuteReader());
                //                        ServerLoanAppClientCollateralID = dt3.Rows[0][0].ToString();
                //                        //ServerLoanAppClientCollateralID = "1";
                //                        #endregion sql

                //                        #region ClientCollateralImg
                //                        foreach (var lp3 in lp2.ClientCollateralImg)
                //                        {
                //                            string ImgName = lp3.ImgName;
                //                            string Ext = lp3.Ext;

                //                            sql = "exec T24_LoanAppClientCollateralImageAdd @ServerLoanAppID=@ServerLoanAppID,@ServerLoanAppPersonID=@ServerLoanAppPersonID"
                //                            + ",@ServerLoanAppClientCollateralID=@ServerLoanAppClientCollateralID,@Ext=@Ext,@Remark=@Remark";
                //                            Com1.CommandText = sql;
                //                            Com1.Parameters.Clear();
                //                            Com1.Parameters.AddWithValue("@ServerLoanAppID", ServerLoanAppID);
                //                            Com1.Parameters.AddWithValue("@ServerLoanAppPersonID", ServerLoanAppPersonID);
                //                            Com1.Parameters.AddWithValue("@ServerLoanAppClientCollateralID", ServerLoanAppClientCollateralID);
                //                            Com1.Parameters.AddWithValue("@Ext", Ext);
                //                            Com1.Parameters.AddWithValue("@Remark", ImgName);
                //                            DataTable dt4 = new DataTable();
                //                            dt4.Load(Com1.ExecuteReader());
                //                            string fname = dt4.Rows[0][0].ToString();
                //                            //string fname = ImgName + "_1";
                //                            LoanAppResImgList data = new LoanAppResImgList();
                //                            data.OriImgName = ImgName;
                //                            data.ServerImgName = fname;
                //                            ImgList.Add(data);
                //                        }
                //                        #endregion ClientCollateralImg
                //                    }
                //                    #endregion ClientCollateral
                //                    #region GuarantorBusiness
                //                    foreach (var lp2 in lp.GuarantorBusiness)
                //                    {
                //                        string Description = lp2.Description;
                //                        string NetProfitPerYear = lp2.NetProfitPerYear;
                //                        //
                //                        sql = "exec T24_LoanAppGuarantorBusinessAddEdit @ServerLoanAppID=@ServerLoanAppID,@ServerLoanAppPersonID=@ServerLoanAppPersonID"
                //                        + ",@Description=@Description,@NetProfitPerYear=@NetProfitPerYear";
                //                        Com1.CommandText = sql;
                //                        Com1.Parameters.Clear();
                //                        Com1.Parameters.AddWithValue("@ServerLoanAppID", ServerLoanAppID);
                //                        Com1.Parameters.AddWithValue("@ServerLoanAppPersonID", ServerLoanAppPersonID);
                //                        Com1.Parameters.AddWithValue("@Description", Description);
                //                        Com1.Parameters.AddWithValue("@NetProfitPerYear", NetProfitPerYear);
                //                        Com1.ExecuteNonQuery();
                //                    }
                //                    #endregion
                //                    #region GuarantorAsset
                //                    foreach (var lp2 in lp.GuarantorAsset)
                //                    {
                //                        string Description = lp2.Description;
                //                        string Quantity = lp2.Quantity;
                //                        string UnitPrice = lp2.UnitPrice;
                //                        //
                //                        sql = "exec T24_LoanAppGuarantorAssetAddEdit @ServerLoanAppID=@ServerLoanAppID,@ServerLoanAppPersonID=@ServerLoanAppPersonID"
                //                        + ",@Description=@Description,@Quantity=@Quantity,@UnitPrice=@UnitPrice";
                //                        Com1.CommandText = sql;
                //                        Com1.Parameters.Clear();
                //                        Com1.Parameters.AddWithValue("@ServerLoanAppID", ServerLoanAppID);
                //                        Com1.Parameters.AddWithValue("@ServerLoanAppPersonID", ServerLoanAppPersonID);
                //                        Com1.Parameters.AddWithValue("@Description", Description);
                //                        Com1.Parameters.AddWithValue("@Quantity", Quantity);
                //                        Com1.Parameters.AddWithValue("@UnitPrice", UnitPrice);
                //                        Com1.ExecuteNonQuery();
                //                    }
                //                    #endregion
                //                    #region PersonImg
                //                    foreach (var lp2 in lp.PersonImg)
                //                    {
                //                        string ImgName = lp2.ImgName;
                //                        string Ext = lp2.Ext;
                //                        string OneCardTwoDoc = lp2.OneCardTwoDoc;
                //                        //
                //                        sql = "exec T24_LoanAppPersonImageImageAdd @ServerLoanAppID=@ServerLoanAppID,@ServerLoanAppPersonID=@ServerLoanAppPersonID"
                //                        + ",@Ext=@Ext,@Remark=@Remark,@OneCardTwoDoc=@OneCardTwoDoc";
                //                        Com1.CommandText = sql;
                //                        Com1.Parameters.Clear();
                //                        Com1.Parameters.AddWithValue("@ServerLoanAppID", ServerLoanAppID);
                //                        Com1.Parameters.AddWithValue("@ServerLoanAppPersonID", ServerLoanAppPersonID);
                //                        Com1.Parameters.AddWithValue("@Ext", Ext);
                //                        Com1.Parameters.AddWithValue("@Remark", ImgName);
                //                        Com1.Parameters.AddWithValue("@OneCardTwoDoc", OneCardTwoDoc);
                //                        DataTable dt3 = new DataTable();
                //                        dt3.Load(Com1.ExecuteReader());
                //                        string fname = dt3.Rows[0][0].ToString();
                //                        //string fname = ImgName + "_2";
                //                        LoanAppResImgList data = new LoanAppResImgList();
                //                        data.OriImgName = ImgName;
                //                        data.ServerImgName = fname;
                //                        ImgList.Add(data);
                //                    }
                //                    #endregion

                //                }
                //                #endregion Person
                //                #region Opinion
                //                foreach (var lp in loan.Opinion)
                //                {
                //                    string Description = lp.Description;
                //                    //string CreateBy = lp.CreateBy;
                //                    string CreateBy = UserID;
                //                    string DeviceDate_Opinion = lp.DeviceDate;
                //                    sql = "exec sp_LoanApp20OpinionAdd2 @LoanAppID=@LoanAppID,@Opinion=@Opinion,@DeviceDate=@DeviceDate,@CreateDate=@CreateDate"
                //                    + ",@CreateBy=@CreateBy,@LoanAppStatusID=@LoanAppStatusID";
                //                    Com1.CommandText = sql;
                //                    Com1.Parameters.Clear();
                //                    Com1.Parameters.AddWithValue("@LoanAppID", ServerLoanAppID);
                //                    Com1.Parameters.AddWithValue("@Opinion", Description);
                //                    Com1.Parameters.AddWithValue("@DeviceDate", DeviceDate_Opinion);
                //                    Com1.Parameters.AddWithValue("@CreateDate", ServerDate);
                //                    Com1.Parameters.AddWithValue("@CreateBy", CreateBy);
                //                    Com1.Parameters.AddWithValue("@LoanAppStatusID", LoanAppStatusID);
                //                    Com1.ExecuteNonQuery();
                //                }
                //                #endregion Opinion
                //                #region CashFlow
                //                foreach (var lp in loan.CashFlow)
                //                {
                //                    #region CashFlow
                //                    string ServerLoanAppCashFlowID = "";
                //                    string StudyMonthAmount = lp.StudyMonthAmount;
                //                    string StudyStartMonth = lp.StudyStartMonth;
                //                    string FamilyExpensePerMonth = lp.FamilyExpensePerMonth;
                //                    string OtherExpensePerMonth = lp.OtherExpensePerMonth;
                //                    sql = "exec T24_LoanAppCashFlowAddEdit @LoanAppID=@LoanAppID,@StudyMonthAmount=@StudyMonthAmount,@StudyStartMonth=@StudyStartMonth"
                //                    + ",@FamilyExpensePerMonth=@FamilyExpensePerMonth,@OtherExpensePerMonth=@OtherExpensePerMonth";
                //                    Com1.CommandText = sql;
                //                    Com1.Parameters.Clear();
                //                    Com1.Parameters.AddWithValue("@LoanAppID", ServerLoanAppID);
                //                    Com1.Parameters.AddWithValue("@StudyMonthAmount", StudyMonthAmount);
                //                    Com1.Parameters.AddWithValue("@StudyStartMonth", StudyStartMonth);
                //                    Com1.Parameters.AddWithValue("@FamilyExpensePerMonth", FamilyExpensePerMonth);
                //                    Com1.Parameters.AddWithValue("@OtherExpensePerMonth", OtherExpensePerMonth);
                //                    DataTable dt2 = new DataTable();
                //                    dt2.Load(Com1.ExecuteReader());
                //                    ServerLoanAppCashFlowID = dt2.Rows[0][0].ToString();
                //                    //ServerLoanAppCashFlowID = "1";
                //                    #endregion CashFlow
                //                    #region MSI
                //                    foreach (var lp2 in lp.MSI)
                //                    {
                //                        #region MSI
                //                        string ServerLoanAppCashFlowMSIID = "";
                //                        string IncomeTypeID = lp2.IncomeTypeID;
                //                        string MainSourceIncomeID = lp2.MainSourceIncomeID;
                //                        string Remark = lp2.Remark;
                //                        string Quantity = lp2.Quantity;
                //                        string ExAge = lp2.ExAge;
                //                        string BusAge = lp2.BusAge;
                //                        string isMSI = lp2.isMSI;
                //                        sql = "exec T24_LoanAppCashFlowMSIAddEdit @ServerLoanAppCashFlowID=@ServerLoanAppCashFlowID,@IncomeTypeID=@IncomeTypeID"
                //                        + ",@MainSourceIncomeID=@MainSourceIncomeID,@Remark=@Remark,@Quantity=@Quantity,@ExAge=@ExAge,@BusAge=@BusAge,@isMSI=@isMSI";
                //                        Com1.CommandText = sql;
                //                        Com1.Parameters.Clear();
                //                        Com1.Parameters.AddWithValue("@ServerLoanAppCashFlowID", ServerLoanAppCashFlowID);
                //                        Com1.Parameters.AddWithValue("@IncomeTypeID", IncomeTypeID);
                //                        Com1.Parameters.AddWithValue("@MainSourceIncomeID", MainSourceIncomeID);
                //                        Com1.Parameters.AddWithValue("@Remark", Remark);
                //                        Com1.Parameters.AddWithValue("@Quantity", Quantity);
                //                        Com1.Parameters.AddWithValue("@ExAge", ExAge);
                //                        Com1.Parameters.AddWithValue("@BusAge", BusAge);
                //                        Com1.Parameters.AddWithValue("@isMSI", isMSI);
                //                        DataTable dt3 = new DataTable();
                //                        dt3.Load(Com1.ExecuteReader());
                //                        ServerLoanAppCashFlowMSIID = dt3.Rows[0][0].ToString();
                //                        //ServerLoanAppCashFlowMSIID = "1";
                //                        #endregion MSI
                //                        #region MSIRegular
                //                        foreach (var lp3 in lp2.MSIRegular)
                //                        {
                //                            string Description = lp3.Description;
                //                            string Amount = lp3.Amount;
                //                            string UnitID = lp3.UnitID;
                //                            string Cost = lp3.Cost;
                //                            string OneIncomeTwoExpense = lp3.OneIncomeTwoExpense;
                //                            string CurrencyID = lp3.Currency;
                //                            if (CurrencyID == "KHR")
                //                            {
                //                                CurrencyID = "1";
                //                            }
                //                            else if (CurrencyID == "USD")
                //                            {
                //                                CurrencyID = "2";
                //                            }
                //                            else
                //                            {
                //                                CurrencyID = "3";
                //                            }
                //                            string Month = lp3.Month;
                //                            sql = "exec T24_LoanAppCashFlowMSIRegAddEdit @LoanAppCashFlowMSIID=@LoanAppCashFlowMSIID,@Description=@Description"
                //                            + ",@Amount=@Amount,@UnitID=@UnitID,@Cost=@Cost,@OneIncomeTwoExpense=@OneIncomeTwoExpense,@CurrencyID=@CurrencyID,@Month=@Month";
                //                            Com1.CommandText = sql;
                //                            Com1.Parameters.Clear();
                //                            Com1.Parameters.AddWithValue("@LoanAppCashFlowMSIID", ServerLoanAppCashFlowMSIID);
                //                            Com1.Parameters.AddWithValue("@Description", Description);
                //                            Com1.Parameters.AddWithValue("@Amount", Amount);
                //                            Com1.Parameters.AddWithValue("@UnitID", UnitID);
                //                            Com1.Parameters.AddWithValue("@Cost", Cost);
                //                            Com1.Parameters.AddWithValue("@OneIncomeTwoExpense", OneIncomeTwoExpense);
                //                            Com1.Parameters.AddWithValue("@CurrencyID", CurrencyID);
                //                            Com1.Parameters.AddWithValue("@Month", Month);
                //                            Com1.ExecuteNonQuery();
                //                        }
                //                        #endregion MSIRegular
                //                        #region MSIIrregular
                //                        foreach (var lp3 in lp2.MSIIrregular)
                //                        {
                //                            string Description = lp3.Description;
                //                            string Amount = lp3.Amount;
                //                            string UnitID = lp3.UnitID;
                //                            string Cost = lp3.Cost;
                //                            string OneIncomeTwoExpense = lp3.OneIncomeTwoExpense;
                //                            string CurrencyID = lp3.Currency;
                //                            if (CurrencyID == "KHR")
                //                            {
                //                                CurrencyID = "1";
                //                            }
                //                            else if (CurrencyID == "USD")
                //                            {
                //                                CurrencyID = "2";
                //                            }
                //                            else
                //                            {
                //                                CurrencyID = "3";
                //                            }
                //                            string Month = lp3.Month;
                //                            sql = "exec T24_LoanAppCashFlowMSIIRegAddEdit @LoanAppCashFlowMSIID=@LoanAppCashFlowMSIID,@Description=@Description"
                //                            + ",@Amount=@Amount,@UnitID=@UnitID,@Cost=@Cost,@OneIncomeTwoExpense=@OneIncomeTwoExpense,@CurrencyID=@CurrencyID,@Month=@Month";
                //                            Com1.CommandText = sql;
                //                            Com1.Parameters.Clear();
                //                            Com1.Parameters.AddWithValue("@LoanAppCashFlowMSIID", ServerLoanAppCashFlowMSIID);
                //                            Com1.Parameters.AddWithValue("@Description", Description);
                //                            Com1.Parameters.AddWithValue("@Amount", Amount);
                //                            Com1.Parameters.AddWithValue("@UnitID", UnitID);
                //                            Com1.Parameters.AddWithValue("@Cost", Cost);
                //                            Com1.Parameters.AddWithValue("@OneIncomeTwoExpense", OneIncomeTwoExpense);
                //                            Com1.Parameters.AddWithValue("@CurrencyID", CurrencyID);
                //                            Com1.Parameters.AddWithValue("@Month", Month);
                //                            Com1.ExecuteNonQuery();
                //                        }
                //                        #endregion MSIIrregular

                //                    }
                //                    #endregion MSI

                //                }
                //                #endregion CashFlow

                //            }
                //        }
                //        catch (Exception ex)
                //        {
                //            int line = c.GetLineNumber(ex);
                //            ERR = "Error";
                //            SMS = "Something was wrong while saving LoanApp: " + line.ToString() + " | " + ex.Message.ToString();
                //            LoanAppResSMS data = new LoanAppResSMS();
                //            data.SMS = SMS;
                //            SMSList.Add(data);
                //        }
                //        #region Commit or RollBack
                //        try
                //        {
                //            if (ERR == "Error")
                //            {
                //                //Tran1.Rollback();
                //            }
                //            else
                //            {
                //                c.ReturnDT("update tblLoanApp1 set UploadERR=1 where LoanAppID='" + ServerLoanAppID + "'");
                //                //Tran1.Commit();
                //                LoanAppResSMS data = new LoanAppResSMS();
                //                data.SMS = "";
                //                SMSList.Add(data);
                //            }
                //            Con1.Close();
                //        }
                //        catch { }
                //        #endregion Commit or RollBack
                //    }
                //    #endregion add
                //}
                #endregion old
                #endregion LoanApp
                #region LogImg
                if (ERR != "Error")
                {
                    //if (ImgList.Count == 0)
                    //{
                    //    ERR = "Error";
                    //    SMS = "No image";
                    //}
                    if (ImgList.Count > 0)
                    {
                        for (int i = 0; i < ImgList.Count; i++)
                        {
                            try
                            {
                                string _ImgType = ImgList[i].ImgType;
                                string _ClientID = ImgList[i].ClientID;
                                string _ServerID = ImgList[i].ServerID;
                                string _OriImgName = ImgList[i].OriImgName;
                                string _ServerImgName = ImgList[i].ServerImgName;
                                string sql = "exec T24_LoanAppImgLogV2 @LoanAppID='" + RSLoanAppID + "',@ImgType='" + _ImgType + "',@ClientID='" + _ClientID + "',@ServerID='" + _ServerID
                                + "',@OriImgName=@OriImgName,@ServerImgName=@ServerImgName,@ServerDate='" + ServerDate + "'";
                                SqlConnection Con1 = new SqlConnection(c.ConStr());
                                Con1.Open();
                                SqlCommand Com1 = new SqlCommand();
                                Com1.Connection = Con1;
                                Com1.Parameters.Clear();
                                Com1.CommandText = sql;
                                Com1.Parameters.AddWithValue("@OriImgName", _OriImgName);
                                Com1.Parameters.AddWithValue("@ServerImgName", _ServerImgName);
                                Com1.ExecuteNonQuery();
                                Con1.Close();
                            }
                            catch (Exception ex)
                            {
                                ERR = "Error";
                                SMS = "Cannot add image log";
                                ExSMS = ex.ToString();
                            }
                        }
                    }
                }
                #endregion

            }
            catch (Exception ex)
            {
                #region ex
                ERR = "Error";
                SMS = "Something was wrong at line:" + c.GetLineNumber(ex) + " | Ex:" + ex.Message.ToString();
                LoanAppResSMS data = new LoanAppResSMS();
                data.SMS = SMS;
                SMSList.Add(data);
                #endregion ex
            }
            #region return
            ListHeader.ERR = ERR;
            ListHeader.SMS = SMS;
            ListHeader.ERRCode = ERRCode;
            ListHeader.LoanClientID = RSIDOnDevice;
            ListHeader.LoanAppID = RSLoanAppID;
            ListHeader.SMSList = SMSList;
            ListHeader.ImgList = ImgList;
            RSData.Add(ListHeader);
            string RSDataStr = "";
            try
            {
                var jsonRS = new JavaScriptSerializer().Serialize(RSData);
                RSDataStr = c.Encrypt(jsonRS, c.SeekKeyGet());
                c.T24_AddLog(FileNameForLog, "RS", jsonRS.ToString(), ControllerName);
                if (ExSMS != "")
                {
                    c.T24_AddLog(FileNameForLog, "RS", ExSMS, ControllerName + "_Error");
                }
            }
            catch { }
            return RSDataStr;
            #endregion return
        }
        //string FirstNameForLog="";LoanAppResModel
    }
}