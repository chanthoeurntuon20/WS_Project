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
    public class AgentGetRepayByAccController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<AgentGetRepayByAccRS> Post([FromUri]string api_name, string api_key,string username, [FromBody]string json)
        {
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "",SMSErrorEx="";
            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string ControllerName = "AgentGetRepayByAcc";
            string FileNameForLog = username + "_" + api_name + "_" + ServerDate.Replace("-", "_").Replace(" ", "_").Replace(":", "_");
            List<AgentGetRepayByAccRS> RSData = new List<AgentGetRepayByAccRS>();
            List<AgentGetRepayByAccData> DataList = new List<AgentGetRepayByAccData>();
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
                AgentGetRepayByAccRQ jObj = null;
                string Acc = "";
                #region json
                if (ERR != "Error")
                {
                    try
                    {
                        jObj = JsonConvert.DeserializeObject<AgentGetRepayByAccRQ>(json);
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
                        DataTable dt = c.ReturnDT("exec T24_GetRepayByVB @VBID=null,@Acc='"+ Acc + "'");
                        if (dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                AgentGetRepayByAccData data = new AgentGetRepayByAccData();
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
                                data.SettlementBalance = dt.Rows[i]["SettlementBalance"].ToString();
                                DataList.Add(data);
                            }
                        }
                        else {
                            ERR = "Error";
                            SMS = "No Data";
                        }
                    } catch { }
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
                AgentGetRepayByAccRS ListHeader = new AgentGetRepayByAccRS();
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

    public class AgentGetRepayByAccRQ
    {
        public string Acc { get; set; }
    }
    public class AgentGetRepayByAccRS
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
        public List<AgentGetRepayByAccData> DataList { get; set; }
    }
    public class AgentGetRepayByAccData
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
        public string SettlementBalance { get; set; }
    }
}