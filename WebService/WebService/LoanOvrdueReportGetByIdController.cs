using System;
using System.Web.Http;
using System.Data;

namespace WebService
{
    [BasicAuthentication]
    public class LoanOvrdueReportGetByIdController : ApiController
    {
        // GET api/<controller>
        public JsonResponse<LoanOverdueList> Get(string api_name, string api_key, string json)//json=[{"AccountNo":"none"}]
        {
            var response = new JsonResponse<LoanOverdueList>();
            Common cmn = new Common();
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "";
            var data = new LoanOverdueList();
            try
            {
                #region api
                string[] CheckApi = c.CheckApi(api_name, api_key);
                ERR = CheckApi[0];
                SMS = CheckApi[1];
                #endregion api

                #region json
                string AccountNo = "";
                if (ERR != "Error")
                {
                    string[] CheckJson = cmn.CheckJsonString(json);

                    ERR = CheckJson[0];
                    if (ERR == "Error")
                    {
                        SMS = CheckJson[1];
                    }
                    else
                    {
                        AccountNo = CheckJson[6];
                    }
                }
                #endregion json

                #region data
                if (ERR != "Error")
                {
                    DataTable dt = c.ReturnDT("exec T24_GetLoanOverdue @LoanAA='" + AccountNo + "'");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        #region params
                        data.Id = dt.Rows[i]["Id"].ToString();
                        data.LoanAA = dt.Rows[i]["LoanAA"].ToString();
                        data.LoanAcc = dt.Rows[i]["LoanAcc"].ToString();
                        data.Operation = dt.Rows[i]["Operation"].ToString();
                        data.VBName = dt.Rows[i]["VBName"].ToString();
                        data.CID = dt.Rows[i]["CID"].ToString();
                        data.CustName = dt.Rows[i]["CustName"].ToString();
                        data.PhoneNo = dt.Rows[i]["PhoneNo"].ToString();
                        data.ProductType = dt.Rows[i]["ProductType"].ToString();
                        data.DisbDate = dt.Rows[i]["DisbDate"].ToString();
                        data.Maturity = dt.Rows[i]["Maturity"].ToString();
                        data.Currency = dt.Rows[i]["Currency"].ToString();
                        data.DisbAmount = dt.Rows[i]["DisbAmount"].ToString();
                        data.OutStanding = dt.Rows[i]["OutStanding"].ToString();
                        data.SavingBalance = dt.Rows[i]["SavingBalance"].ToString();
                        data.TotalDue = dt.Rows[i]["TotalDue"].ToString();
                        data.PrinDue = dt.Rows[i]["PrinDue"].ToString();
                        data.IntDue = dt.Rows[i]["IntDue"].ToString();
                        data.MthlyDue = dt.Rows[i]["MthlyDue"].ToString();
                        data.PenaltyDue = dt.Rows[i]["PenaltyDue"].ToString();
                        data.Arrear = dt.Rows[i]["Arrear"].ToString();
                        data.DateAdded = dt.Rows[i]["DateAdded"].ToString();
                        #endregion params
                        response.ERR = ERR;
                        response.SMS = SMS;
                        response.Data = data;
                    }

                }
                #endregion data
            }
            catch (Exception ex)
            {
                response.ERR = "Error";
                response.SMS = "Something was wrong";
            }
            #region if Error
            if (ERR == "Error")
            {
                response.ERR = ERR;
                response.SMS = SMS;
                response.Data = null;
            }
            #endregion if Error

            return response;
        }

    }

}