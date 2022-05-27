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
    public class AgentGetAAInfoController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<AgentGetAAInfoRS> Post([FromUri]string api_name, string api_key,string username, [FromBody]string json)
        {
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "", SMSErrorEx="";
            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string ControllerName = "AgentGetAAInfo";
            string FileNameForLog = username + "_" + api_name + "_" + ServerDate.Replace("-", "_").Replace(" ", "_").Replace(":", "_");
            List<AgentGetAAInfoRS> RSData = new List<AgentGetAAInfoRS>();
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
                    SMS = "Something was wrong while checking Api";
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
                AgentGetAAInfoRQ jObj = null;
                string DisbursementDate = "", BusinessDate = "";
                #region json
                if (ERR != "Error")
                {
                    try
                    {
                        jObj = JsonConvert.DeserializeObject<AgentGetAAInfoRQ>(json);
                        DisbursementDate = jObj.DisbursementDate;
                        BusinessDate = jObj.BusinessDate;
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
                        string sql = "SELECT * FROM AMKlocalRef.dbo._tmpAmkLoanInfoForTablet";//"EXEC AMKlocalRef.dbo.sp_AmkLoanInfoForTablet";
                        //if (DisbursementDate == "")
                        //{
                        //    DisbursementDate = null;
                        //}
                        //else {
                        //    sql = sql + " @DisbursementDate='"+ DisbursementDate + "'";
                        //}
                        //if (BusinessDate == "")
                        //{
                        //    BusinessDate = null;
                        //}
                        //else
                        //{
                        //    sql = sql + " ,@BusinessDate='" + BusinessDate + "'";
                        //}

                        SqlConnection Con1 = new SqlConnection(c.ConStrInsightConn());
                        Con1.Open();
                        SqlCommand Com1 = new SqlCommand();
                        Com1.Connection = Con1;
                        Com1.Parameters.Clear();
                        Com1.CommandTimeout = 180000;
                        Com1.CommandText = sql;
                        //Com1.CommandType = CommandType.StoredProcedure;
                        //Com1.Parameters.AddWithValue("@DisbursementDate", DisbursementDate);
                        //Com1.Parameters.AddWithValue("@BusinessDate", BusinessDate);
                        DataTable dt = new DataTable();
                        dt.Load(Com1.ExecuteReader());
                        if (dt.Rows.Count > 0) {
                            DataColumn col = new DataColumn("CreateDate",typeof(DateTime));
                            col.DefaultValue = DateTime.Now;
                            dt.Columns.Add(col);

                            using (SqlConnection bcon = new SqlConnection(c.ConStr())) {
                                using (SqlBulkCopy bCopy = new SqlBulkCopy(bcon)) {
                                    bcon.Open();
                                    bCopy.BulkCopyTimeout= 180000;
                                    bCopy.DestinationTableName = "T24_Insight_AAInfo";
                                    try {
                                        bCopy.WriteToServer(dt);
                                    }
                                    catch (Exception ex)
                                    {
                                        ERR = "Error";
                                        SMS = "Something was wrong while connect to DB";
                                        SMSErrorEx = ex.Message.ToString();
                                    }
                                    bcon.Close();
                                }
                            }
                        }
                        Con1.Close();
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
                AgentGetAAInfoRS ListHeader = new AgentGetAAInfoRS();
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

    public class AgentGetAAInfoRQ
    {
        public string DisbursementDate { get; set; }
        public string BusinessDate { get; set; }
    }
    public class AgentGetAAInfoRS
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
    }
}