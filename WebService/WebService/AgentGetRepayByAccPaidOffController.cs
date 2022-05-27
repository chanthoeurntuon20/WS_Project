using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using WebService.func;

namespace WebService
{
    [BasicAuthentication]
    public class AgentGetRepayByAccPaidOffController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<AgentGetRepayByAccPaidOffRS> Post([FromUri]string api_name, string api_key,string username, [FromBody]string json)
        {
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "",SMSErrorEx="";
            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string ControllerName = "AgentGetRepayByAccPaidOff";
            string FileNameForLog = username + "_" + api_name + "_" + ServerDate.Replace("-", "_").Replace(" ", "_").Replace(":", "_");
            List<AgentGetRepayByAccPaidOffRS> RSData = new List<AgentGetRepayByAccPaidOffRS>();
            List<AgentGetRepayByAccPaidOffData> DataList = new List<AgentGetRepayByAccPaidOffData>();
            try
            {
                //Add log
                c.T24_AddLog(FileNameForLog, "RQ", json, ControllerName);

                #region api
                try
                {
                    string[] CheckApi = c.CheckApi(api_name, api_key);
                    ERR = CheckApi[0];
                    SMS = CheckApi[1];
                }
                catch (Exception ex)
                {
                    ERR = "Error";
                    SMS = "Invalid Api";
                }
                #endregion api
                #region check json
                if (ERR != "Error")
                {
                    if (json == null || json == "")
                    {
                        ERR = "Error";
                        SMS = "Invalid JSON";
                    }
                }
                #endregion check json
                AgentGetRepayByAccPaidOffRQ jObj = null;
                string Acc = "";
                #region json
                if (ERR != "Error")
                {
                    try
                    {
                        jObj = JsonConvert.DeserializeObject<AgentGetRepayByAccPaidOffRQ>(json);
                        Acc = jObj.Acc;
                    }
                    catch
                    {
                        ERR = "Error";
                        SMS = "Invalid JSON";
                    }
                }
                #endregion json

                #region data
                if (ERR != "Error")
                {
                    try {
                        DataTable dt = c.ReturnDT("exec T24_GetRepayForPaidOff @VBID=null,@Acc='" + Acc + "'");
                        if (dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                string PaidOFFAmt = "";
                                #region get var for paid off
                                string paidOffDate = dt.Rows[i]["paidOffDate"].ToString();
                                string maturityDate = dt.Rows[i]["maturityDate"].ToString();
                                string disbursementDate = dt.Rows[i]["disbursementDate"].ToString();
                                string firstRepaymentDate = dt.Rows[i]["firstRepaymentDate"].ToString();
                                string filingDate = dt.Rows[i]["filingDate"].ToString();
                                string prodCode = dt.Rows[i]["prodCode"].ToString();
                                double approvedAmount = Convert.ToDouble(dt.Rows[i]["approvedAmount"].ToString());
                                double amountToClose = Convert.ToDouble(dt.Rows[i]["amountToClose"].ToString());
                                bool isMigration = (dt.Rows[i]["isMigration"].ToString()=="0")?false:true;
                                int loanTerm = Convert.ToInt16(dt.Rows[i]["loanTerm"].ToString());
                                double outstandingBalance = Convert.ToDouble(dt.Rows[i]["outstandingBalance"].ToString());
                                double originalPriDueAmount = Convert.ToDouble(dt.Rows[i]["originalPriDueAmount"].ToString());
                                double ClosureAmt = Convert.ToDouble(dt.Rows[i]["ClosureAmt"].ToString());
                                double RoundClosAmt = Convert.ToDouble(dt.Rows[i]["RoundClosAmt"].ToString());
                                #endregion
                                //hit to api calculate paid off
                                #region Calculation
                                firstRepaymentDate = ((Convert.ToDateTime(firstRepaymentDate)).AddDays((loanTerm * 30) / 2)).ToString("yyyy-MM-dd");
                                try
                                {
                                    PaidOffEngine pOff = new PaidOffEngine();
                                    double paidOff = pOff.PaidOffCalculator(prodCode, amountToClose, paidOffDate, maturityDate
                                    , disbursementDate, firstRepaymentDate, filingDate, approvedAmount, isMigration
                                    , loanTerm, outstandingBalance, originalPriDueAmount);
                                    PaidOFFAmt = paidOff.ToString();
                                }
                                catch (Exception ex)
                                {
                                    ERR = "Error";
                                    SMS = "ErrorEx: cannot calculate data";
                                    SMSErrorEx = ex.Message.ToString();
                                }
                                #endregion

                                if (ERR != "Error") {
                                    AgentGetRepayByAccPaidOffData data = new AgentGetRepayByAccPaidOffData();
                                    data.LoanAcc = dt.Rows[i]["LoanAcc"].ToString();
                                    data.PayInAcc = dt.Rows[i]["PayInAcc"].ToString();
                                    data.CompulsoryAcc = dt.Rows[i]["CompulsoryAcc"].ToString();
                                    data.LoanAmt = dt.Rows[i]["LoanAmt"].ToString();
                                    data.CompulsoryAmt = dt.Rows[i]["CompulsoryAmt"].ToString();
                                    data.Currency = dt.Rows[i]["Currency"].ToString();
                                    data.AA = dt.Rows[i]["AA"].ToString();
                                    data.EnquiryAccType = dt.Rows[i]["EnquiryAccType"].ToString();
                                    data.CollDate = dt.Rows[i]["CollDate"].ToString();
                                    data.CustName = dt.Rows[i]["CustName"].ToString();
                                    data.CustNameKh = dt.Rows[i]["CustNameKh"].ToString();
                                    data.TotalRoundUp = dt.Rows[i]["TotalRoundUp"].ToString();
                                    data.FOBO = dt.Rows[i]["FOBO"].ToString();
                                    data.IntDue = dt.Rows[i]["IntDue"].ToString();
                                    data.PenDue = dt.Rows[i]["PenDue"].ToString();
                                    data.OutstandingBalance = outstandingBalance.ToString();
                                    data.PaidOFFAmt = PaidOFFAmt;
                                    data.ClosureAmt = dt.Rows[i]["ClosureAmt"].ToString();
                                    data.RoundClosAmt = dt.Rows[i]["RoundClosAmt"].ToString();
                                    DataList.Add(data);
                                }
                                
                            }
                        }
                        else {
                            ERR = "Error";
                            SMS = "No Data";
                        }
                    } catch (Exception ex)
                    {
                        ERR = "Error";
                        SMS = "No Data";
                        SMSErrorEx = ex.Message.ToString();
                    }
                }
                #endregion data
            }
            catch (Exception ex)
            {
                ERR = "Error";
                SMS = "Something was wrong";
                SMSErrorEx = ex.Message.ToString();
            }
            #region if Error
            if (SMSErrorEx != "")
            {
                c.T24_AddLog(FileNameForLog, "Ex", SMSErrorEx, ControllerName);
            }
            #endregion

            try
            {
                AgentGetRepayByAccPaidOffRS ListHeader = new AgentGetRepayByAccPaidOffRS();
                ListHeader.ERR = ERR;
                ListHeader.SMS = SMS;
                ListHeader.DataList = DataList;
                RSData.Add(ListHeader);

                var jsonRS = new JavaScriptSerializer().Serialize(RSData);
                c.T24_AddLog(FileNameForLog, "RS", jsonRS.ToString(), ControllerName);
            }
            catch { }
            return RSData;
        }

    }

    public class AgentGetRepayByAccPaidOffRQ
    {
        public string Acc { get; set; }
    }
    public class AgentGetRepayByAccPaidOffRS
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
        public List<AgentGetRepayByAccPaidOffData> DataList { get; set; }
    }
    public class AgentGetRepayByAccPaidOffData
    {
        public string LoanAcc { get; set; }
        public string PayInAcc { get; set; }
        public string CompulsoryAcc { get; set; }
        public string LoanAmt { get; set; }
        public string CompulsoryAmt { get; set; }
        public string Currency { get; set; }
        public string AA { get; set; }
        public string EnquiryAccType { get; set; }//1=LoanAcc | 2=PayInAcc
        public string CollDate { get; set; }
        public string CustName { get; set; }
        public string CustNameKh { get; set; }
        public string TotalRoundUp { get; set; }
        public string FOBO { get; set; }
        public string PaidOFFAmt { get; set; }
        public string IntDue { get; set; }
        public string PenDue { get; set; }
        public string OutstandingBalance { get; set; }
        public string ClosureAmt { get; set; }
        public string RoundClosAmt { get; set; }
    }
}