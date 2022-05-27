using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebService.Models.Res.LoanApp;
using WebService.Models.Req.LoanApp;

namespace WebService.ApiControllers.Loans
{
    [BasicAuthentication]
    public class LoanAppController : ApiController
    {
        [Route("api/v1/loanapp/cbs/customer-create")]
        [HttpPost()]
        public LoanAppCustomerRes AddLoanCustomer([FromUri] string api_name, string api_key, [FromBody] LoanAppCustomerReq customerReq)
        {
            Class1 c = new Class1();
            var loanCustomerRes = new LoanAppCustomerRes();

            try
            {
                c.ReturnDT("update tblLoanAppListForOpenToCBSLog set StartDate=getdate() where LoanAppID='" + customerReq.LoanAppId + "'");
                string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

                string fileHeader = "";
                #region get person list of each loan
                try
                {
                    fileHeader = "LoanAppPostToCBSV2_InstID_" + customerReq.InstId + "_LID_" + customerReq.LoanAppId + "_" + ServerDate;
                    //old version -> AddEditCustomerV2 | new verion -> AddEditCustomerV3
                    string[] rs = null;
                    rs = c.AddEditCustomerV2(fileHeader, customerReq.LoanAppPersonId, customerReq.LoanAppPersonTypeId);
                    
                    loanCustomerRes.Status = 201;
                    loanCustomerRes.CustomerId = rs[0];
                    loanCustomerRes.Message = "Loan customer create successfull";

                }
                catch (Exception ex)
                {
                    c.UpdateLoanAppStatus(customerReq.LoanAppId, "", "", $"Error AddEditCustomer: {ex.Message}", "12");
                    loanCustomerRes.Status = 400;
                    loanCustomerRes.CustomerId = "";
                    loanCustomerRes.Message = $"Loan customer create error: {ex.Message}";
                }
                #endregion get person list of each loan
            }
            catch (Exception ex)
            {
                loanCustomerRes.Status = 500;
                loanCustomerRes.CustomerId = "";
                loanCustomerRes.Message = $"Internal Server error: {ex.Message}";
            }

            return loanCustomerRes;
        }
        [Route("api/v1/loanapp/cbs/loan-create")]
        [HttpPost()]
        public LoanAppRes AddLoan([FromUri] string api_name, string api_key,[FromBody] LoanAppReq loanAppReq)
        {
            Class1 c = new Class1();
            var loanRes = new LoanAppRes();
            string fileHeader = "";
            try
            {
                string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                fileHeader = "LoanAppPostToCBSV2_InstID_" + loanAppReq.InstId + "_LID_" + loanAppReq.LoanAppId + "_" + ServerDate;

                #region add loan
                //add loan
                string[] rs = c.AddLoanV2(fileHeader, loanAppReq.LoanAppId, loanAppReq.CustomerId, api_name);
              
                loanRes.Status = 201;
                loanRes.LoanSms = rs[0];
                loanRes.Message = $"Loan create successfull.";
                c.T24_AddLog(fileHeader, "AddLoan_Finish", "ERR: " + loanRes.LoanSms + " | SMS:" + loanRes.Message, "LoanAdd");
              
                #endregion add loan
            }
            catch (Exception ex)
            {
                loanRes.Status = 400;
                loanRes.LoanSms = "";
                loanRes.Message = $"Loan create error: {ex.Message}";
            }
            return loanRes;
        }
    }
}