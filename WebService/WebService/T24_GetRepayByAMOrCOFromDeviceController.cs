using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace WebService
{
    [BasicAuthentication]
    public class T24_GetRepayByAMOrCOFromDeviceController : ApiController
    {
        Class1 c = new Class1();
        //public IEnumerable<T24_GetRepayByAMOrCOFromDeviceModel> Post([FromUri]string api_name, string api_key, [FromBody] string json)
        public string Post([FromUri]string p, [FromUri]string msgid, [FromBody]string json)
        {
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "", ExSMS = "", ERRCode = "";
            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            List<T24_GetRepayRS> RSData = new List<T24_GetRepayRS>();
            string ControllerName = "T24_GetRepayByAMOrCOFromDevice";
            string FileNameForLog = msgid + "_" + ServerDate.Replace("-", "_").Replace(" ", "_").Replace(":", "_");
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
                    string[] str = c.CheckObjED(json, "2");
                    ERR = str[0];
                    SMS = str[1];
                    ExSMS = str[2];
                    json = str[3];
                }
                #endregion check json
                #region read json
                T24_GetRepayRQ jObj = null;
                string VBIDList = "";
                if (ERR != "Error")
                {
                    try
                    {
                        jObj = JsonConvert.DeserializeObject<T24_GetRepayRQ>(json);
                        VBIDList = jObj.VBIDList;
                    }
                    catch (Exception ex)
                    {
                        ERR = "Error";
                        ExSMS = ex.Message.ToString();
                        //get sms
                        string[] str = c.GetSMSByMsgID("3");
                        ERR = str[0];
                        if (ERR == "Error")
                        {
                            SMS = str[1];
                            ExSMS = ExSMS + "|" + str[2];
                        }
                        else
                        {
                            SMS = str[3];
                        }
                        ERR = "Error";
                    }
                }
                #endregion

                #region data
                if (ERR != "Error")
                {
                    T24_GetRepayRS ListHeader = new T24_GetRepayRS();
                    ListHeader.ERR = ERR;
                    ListHeader.SMS = SMS;
                    ListHeader.ERRCode = ERRCode;

                    List<T24_GetRepayRSDataList> DataList = new List<T24_GetRepayRSDataList>();

                    DataTable dt = c.ReturnDT("exec T24_GetRepayByVB_V2 @VBID='" + VBIDList + "'");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        #region params
                        T24_GetRepayRSDataList data = new T24_GetRepayRSDataList();
                        data.RepayID = dt.Rows[i]["RepayID"].ToString();
                        data.VillageBankID = dt.Rows[i]["VillageBankID"].ToString();
                        data.VBName = dt.Rows[i]["VBName"].ToString();
                        data.CurrencyID = dt.Rows[i]["CurrencyID"].ToString();
                        data.LoanCcy = dt.Rows[i]["LoanCcy"].ToString();
                        data.CustomerID = dt.Rows[i]["CustomerID"].ToString();
                        data.ClientName = dt.Rows[i]["ClientName"].ToString();
                        data.CUAccountID = dt.Rows[i]["CUAccountID"].ToString();
                        data.CUAccountNumber = dt.Rows[i]["CUAccountNumber"].ToString();
                        data.GroupNumber = dt.Rows[i]["GroupNumber"].ToString();
                        data.ProdCode = dt.Rows[i]["ProdCode"].ToString();
                        data.ProdDesc = dt.Rows[i]["ProdDesc"].ToString();
                        data.IntDue = dt.Rows[i]["IntDue"].ToString();
                        data.PenDue = dt.Rows[i]["Penalty"].ToString(); //dt.Rows[i]["PenDue"].ToString();
                        data.Penalty = dt.Rows[i]["Penalty"].ToString();
                        data.CompulsorySaving = dt.Rows[i]["CompulsorySaving"].ToString();
                        data.FeeDue = dt.Rows[i]["FeeDue"].ToString();
                        data.PriDue = dt.Rows[i]["PriDue"].ToString();
                        data.TotalDue = dt.Rows[i]["TotalDue"].ToString();
                        data.TotDueAmt = dt.Rows[i]["TotDueAmt"].ToString();
                        data.ClosureAmt = dt.Rows[i]["ClosureAmt"].ToString();
                        data.AmountToClose = dt.Rows[i]["AmountToClose"].ToString();
                        data.ArrearsDay = dt.Rows[i]["ArrearsDay"].ToString();
                        data.MaturityDate = dt.Rows[i]["MaturityDate"].ToString();
                        data.CollectionDate = dt.Rows[i]["CollectionDate"].ToString();
                        data.ValueDate = dt.Rows[i]["ValueDate"].ToString();
                        data.CompulsorySaving = dt.Rows[i]["CompulsorySaving"].ToString();
                        data.MonthlyFee = dt.Rows[i]["MonthlyFee"].ToString();
                        data.InsuranceAmount = dt.Rows[i]["InsuranceAmount"].ToString();
                        data.CompulsoryAccID = dt.Rows[i]["CompulsoryAccID"].ToString();
                        data.CUAccountPortfolioID = dt.Rows[i]["CUAccountPortfolioID"].ToString();
                        data.COName = dt.Rows[i]["COName"].ToString();
                        data.BranchName = dt.Rows[i]["BranchName"].ToString();
                        data.Term = dt.Rows[i]["Term"].ToString();
                        data.LoanRef = dt.Rows[i]["LoanRef"].ToString();
                        data.CompulsoryOptionID = dt.Rows[i]["CompulsoryOptionID"].ToString();
                        data.DisbDate = dt.Rows[i]["DisbDate"].ToString();
                        data.MITypeID = dt.Rows[i]["MITypeID"].ToString();
                        data.OutstandingBalance = dt.Rows[i]["OutstandingBalance"].ToString();
                        data.first_repayment_date = dt.Rows[i]["first_repayment_date"].ToString();
                        data.loan_term = dt.Rows[i]["loan_term"].ToString();
                        data.approved_amount = dt.Rows[i]["approved_amount"].ToString();
                        data.filing_date = dt.Rows[i]["filing_date"].ToString();
                        data.loanAppPersonType = dt.Rows[i]["loanAppPersonType"].ToString();
                        data.SettlementBalance = dt.Rows[i]["SettlementBalance"].ToString();

                        DataList.Add(data);
                        #endregion params
                    }

                    ListHeader.DataList = DataList;

                    RSData.Add(ListHeader);
                }
                #endregion data
            }
            catch (Exception ex)
            {
                ERR = "Error";
                SMS = "Something was wrong at line:" + c.GetLineNumber(ex) + " | Ex:" + ex.Message.ToString();
            }

            #region if Error
            if (ERR == "Error")
            {
                T24_GetRepayRS ListHeader = new T24_GetRepayRS();
                ListHeader.ERR = ERR;
                ListHeader.SMS = SMS;
                ListHeader.ERRCode = ERRCode;
                ListHeader.DataList = null;
                RSData.Add(ListHeader);
            }
            #endregion
            string RSDataStr = "";
            try
            {
                var jsonRS = new JavaScriptSerializer().Serialize(RSData);
                RSDataStr = c.Encrypt(jsonRS, c.SeekKeyGet());
                c.T24_AddLog(FileNameForLog, "RS", jsonRS.ToString(), ControllerName);
            }
            catch { }
            return RSDataStr;
        }
    }
}

public class T24_GetRepayRQ{
    public string VBIDList { get; set; }
}
public class T24_GetRepayRS
{
    public string ERR { get; set; }
    public string SMS { get; set; }
    public string ERRCode { get; set; }
    public List<T24_GetRepayRSDataList> DataList { get; set; }
}
public class T24_GetRepayRSDataList
{
    public string RepayID { get; set; }
    public string VillageBankID { get; set; }
    public string VBName { get; set; }
    public string CurrencyID { get; set; }
    public string LoanCcy { get; set; }
    public string CustomerID { get; set; }
    public string ClientName { get; set; }
    public string CUAccountID { get; set; }
    public string CUAccountNumber { get; set; }
    public string GroupNumber { get; set; }
    public string ProdCode { get; set; }
    public string ProdDesc { get; set; }
    public string IntDue { get; set; }
    public string PenDue { get; set; }
    public string Penalty { get; set; }
    public string FeeDue { get; set; }
    public string PriDue { get; set; }
    public string TotalDue { get; set; }
    public string TotDueAmt { get; set; }
    public string ClosureAmt { get; set; }
    public string AmountToClose { get; set; }
    public string ArrearsDay { get; set; }
    public string MaturityDate { get; set; }
    public string CollectionDate { get; set; }
    public string ValueDate { get; set; }
    public string CompulsorySaving { get; set; }
    public string MonthlyFee { get; set; }
    public string InsuranceAmount { get; set; }
    public string CompulsoryAccID { get; set; }
    public string CUAccountPortfolioID { get; set; }
    public string COName { get; set; }
    public string BranchName { get; set; }
    public string Term { get; set; }
    public string LoanRef { get; set; }
    public string CompulsoryOptionID { get; set; }
    public string DisbDate { get; set; }
    public string MITypeID { get; set; }
    public string OutstandingBalance { get; set; }
    public string first_repayment_date { get; set; }
    public string loan_term { get; set; }
    public string approved_amount { get; set; }
    public string filing_date { get; set; }
    public string loanAppPersonType { get; set; }
    public string SettlementBalance { get; set; }
}