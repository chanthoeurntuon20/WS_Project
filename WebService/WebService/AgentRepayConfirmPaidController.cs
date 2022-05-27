using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace WebService
{
    [BasicAuthentication]
    public class AgentRepayConfirmPaidController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<AgentRepayConfirmPaidRS> Post([FromUri]string api_name, string api_key,string username, [FromBody]string json)
        {
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "", SMSErrorEx="";
            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string ControllerName = "AgentRepayConfirmPaid";
            string FileNameForLog = username + "_" + api_name + "_" + ServerDate.Replace("-", "_").Replace(" ", "_").Replace(":", "_");
            List<AgentRepayConfirmPaidRS> RSData = new List<AgentRepayConfirmPaidRS>();
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
                    SMSErrorEx = ex.Message.ToString();
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
                //AgentRepayConfirmPaidRQ jObj = null;
                //string CompulsoryAcc = "", CompulsoryAmt="", Currency="", AA="";
                #region json
                if (ERR != "Error")
                {
                    try
                    {
                        //jObj = JsonConvert.DeserializeObject<AgentRepayConfirmPaidRQ>(json);
                        //CompulsoryAcc = jObj.CompulsoryAcc;
                        //CompulsoryAmt = jObj.CompulsoryAmt;
                        //Currency = jObj.Currency;
                        //AA = jObj.AA;
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
                        List<AgentRepayConfirmPaidRQ> jObj = JsonConvert.DeserializeObject<List<AgentRepayConfirmPaidRQ>>(json);                        
                        foreach (var r in jObj) {
                            string AA=r.AA;
                            string ComAmt = r.CompulsoryAmt;
                            string Currency = r.Currency;
                            string CompulsoryAccountID = r.CompulsoryAcc;
                            string LoanAcc = r.LoanAcc;
                            string PayInAcc = r.PayInAcc;
                            string LoanAmt = r.LoanAmt;
                            string EnquiryAccType = r.EnquiryAccType;
                            string CollDate = r.CollDate;

                            SqlConnection Con1 = new SqlConnection(c.ConStr());
                            Con1.Open();
                            SqlCommand Com1 = new SqlCommand();
                            Com1.Connection = Con1;
                            Com1.Parameters.Clear();
                            string sql = "AgentAppSP_ConfirmPaidRepay";
                            Com1.CommandText = sql;
                            Com1.CommandType = CommandType.StoredProcedure;
                            Com1.Parameters.AddWithValue("@FileNameForLog", FileNameForLog);
                            Com1.Parameters.AddWithValue("@Username", username);
                            Com1.Parameters.AddWithValue("@AA", AA);
                            Com1.Parameters.AddWithValue("@ComAmt", ComAmt);
                            Com1.Parameters.AddWithValue("@Currency", Currency);
                            Com1.Parameters.AddWithValue("@CompulsoryAccountID", CompulsoryAccountID);
                            Com1.Parameters.AddWithValue("@LoanAcc", LoanAcc);
                            Com1.Parameters.AddWithValue("@PayInAcc", PayInAcc);
                            Com1.Parameters.AddWithValue("@LoanAmt", LoanAmt);
                            Com1.Parameters.AddWithValue("@CollDate", CollDate);
                            Com1.Parameters.AddWithValue("@EnquiryAccType", EnquiryAccType);
                            Com1.ExecuteNonQuery();
                            Con1.Close();
                        }

                    }
                    catch (Exception ex)
                    {
                        ERR = "Error";
                        SMS = "Something was wrong while connect to DB";
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
                AgentRepayConfirmPaidRS ListHeader = new AgentRepayConfirmPaidRS();
                ListHeader.ERR = ERR;
                ListHeader.SMS = SMS;
                RSData.Add(ListHeader);

                var jsonRS = new JavaScriptSerializer().Serialize(RSData);
                c.T24_AddLog(FileNameForLog, "RS", jsonRS.ToString(), ControllerName);
            }
            catch { }
            return RSData;
        }

    }

    public class AgentRepayConfirmPaidRQ
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
    }
    public class AgentRepayConfirmPaidRS
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
    }
}