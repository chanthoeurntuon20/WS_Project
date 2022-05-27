using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebService
{
    public class LoanAppPostToCBSController : ApiController
    {        
        // POST api/<controller>
        public IEnumerable<LoanAppPostToCBSResModel> Post([FromUri]string api_name, string api_key, string loanappid)
        {
            Class1 c = new Class1();
            string LogSMS = "";
            string ERR = "Succeed", SMS = "", RSLoanAppID = loanappid;
            List<LoanAppPostToCBSResModel> RSData = new List<LoanAppPostToCBSResModel>();
            LoanAppPostToCBSResModel ListHeader = new LoanAppPostToCBSResModel();
            try {
                c.ReturnDT("update tblLoanAppListForOpenToCBSLog set StartDate=getdate() where LoanAppID='" + loanappid + "'");
                string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                //Add log
                //c.T24_AddLog("LoanAppPostToCBS_" + ServerDate.Replace("-", "_").Replace(" ", "_").Replace("HH:mm:ss.fff", ""), "LoanAppPostToCBS", loanappid);
                

                //DataTable dt = new DataTable();
                //string sql = "exec T24_LoanAppListForOpenToCBS";
                //dt = c.ReturnDT(sql);
                string LoanAppID = loanappid;
                string fileHeader = "";
                #region get person list of each loan
                string CUSTOMERID = "";
                string sql = "exec T24_LoanAppListForOpenToCBS1 @LoanAppID='" + LoanAppID + "'";
                DataTable dt1 = c.ReturnDT(sql);
                if (dt1.Rows.Count == 0) {
                    ERR = "Error";
                    SMS = "Something was wrong with LoanAppID: "+ LoanAppID;
                }
                for (int i1 = 0; i1 < dt1.Rows.Count; i1++)
                {
                    if (ERR != "Error") {
                        try
                        {
                            string LoanAppPersonID = dt1.Rows[i1]["LoanAppPersonID"].ToString();
                            string CustomerID = dt1.Rows[i1]["CustomerID"].ToString();
                            string LoanAppPersonTypeID = dt1.Rows[i1]["LoanAppPersonTypeID"].ToString();
                            string MaritalStatusID = dt1.Rows[i1]["MaritalStatusID"].ToString();
                            string InstID = dt1.Rows[i1]["InstID"].ToString();
                            fileHeader = "LoanAppPostToCBS_InstID_" + InstID+ "_" + LoanAppID + "_" + ServerDate;
                            string[] rs = c.AddEditCustomer(fileHeader, LoanAppPersonID, LoanAppPersonTypeID);
                            ERR = rs[0];
                            SMS = rs[1];
                            if (LoanAppPersonTypeID == "31")
                            {
                                CUSTOMERID = ERR;
                            }
                        }
                        catch (Exception ex)
                        {
                            ERR = "Error"; SMS = "Error AddEditCustomer: " + ex.Message.ToString(); LogSMS = ex.Message.ToString();
                        }
                    }                    
                }
                #endregion get person list of each loan
                #region add loan
                if (ERR != "Error")
                {
                    //add loan
                    string[] rs = c.AddLoan(fileHeader, LoanAppID, CUSTOMERID, api_name);
                    ERR = rs[0];
                    SMS = rs[1];
                    c.T24_AddLog(fileHeader, "AddLoan_Finish", "ERR: "+ERR+" | SMS:"+ SMS, "LoanAdd");
                    //if (ERR == "Error")
                    //{
                    //    SMS = "Loan was not created";
                    //}
                }
                else
                {
                    // error add customer
                    c.UpdateLoanAppStatus(LoanAppID, "", "", "Error AddEditCustomer: " + SMS, "12");
                }
                #endregion add loan

            }
            catch(Exception ex) {
                ERR = "Error";
                SMS = "Something was wrong";
            }

            ListHeader.ERR = ERR;
            ListHeader.SMS = SMS;
            RSData.Add(ListHeader);

            return RSData;
        }

    }
}