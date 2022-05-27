using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Data;

namespace WebService
{
    public class ResolvingInformationCompleteOfflineController : ApiController
    {
        public JsonResponse<LoanOverdueLogsModel> Get(string api_name, string api_key, string json)//json=[{"UserId":"5031"}]
        {
            var response = new JsonResponse<LoanOverdueLogsModel>();
            Class1 c = new Class1();
            Common cmn = new Common();
            string ERR = "Succeed", SMS = "";
            string userId = "";
            int page = 0, limit = 0;

            try
            {
                #region api
                string[] CheckApi = c.CheckApi(api_name, api_key);
                ERR = CheckApi[0];
                SMS = CheckApi[1];
                #endregion api

                #region json
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
                        userId = CheckJson[8].ToString();
                    }
                }
                #endregion json

                #region data
                if (ERR != "Error")
                {
                    LoanOverdueLogsModel ListHeader = new LoanOverdueLogsModel();
                    ListHeader.ERR = ERR;
                    ListHeader.SMS = SMS;
                    page = page == 0 ? page = 1 : page;
                    limit = limit == 0 ? limit = 1 : limit;
                    List<LoanOverdueLogsList> DataList = new List<LoanOverdueLogsList>();

                    DataTable dt = c.ReturnDT("exec T24_GetLoanOverdueLogs '',0,0,'" + userId + "'");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        #region params
                        LoanOverdueLogsList data = new LoanOverdueLogsList();
                        data.Id = dt.Rows[i]["Id"].ToString();
                        data.LoanAcc = dt.Rows[i]["LoanAcc"].ToString();
                        data.OverdueType = dt.Rows[i]["OverdueType"].ToString();
                        data.MainReason = dt.Rows[i]["MainReason"].ToString();
                        data.Reason = dt.Rows[i]["Reason"].ToString();
                        data.SolveBy = dt.Rows[i]["SolvedbyCode"].ToString();
                        data.CutomerRating = dt.Rows[i]["CutomerRating"].ToString();
                        data.ManagementAction = dt.Rows[i]["ManagementAction"].ToString();
                        data.AccuracyOfUseCredit = dt.Rows[i]["AccuracyOfUseCredit"].ToString();
                        data.StatusOfSolutions = dt.Rows[i]["StatusOfSolutions"].ToString();

                        if (dt.Rows[i]["PromisePaymentDate"].ToString() != "")
                        {
                            data.PromisePaymentDate = Convert.ToDateTime(dt.Rows[i]["PromisePaymentDate"]).ToString("hh:mm tt dd-MM-yyyy");
                        }
                        else
                        {
                            data.PromisePaymentDate = "";
                        }

                        data.PromiseAmountCurrency = dt.Rows[i]["PromiseAmountCurrency"].ToString();
                        data.PromisedAmount = dt.Rows[i]["PromisedAmount"].ToString();
                        data.SourceOfMoneyPaid = dt.Rows[i]["SourceOfMoneyPaid"].ToString();
                        data.CutomerAttitude = dt.Rows[i]["CutomerAttitude"].ToString();
                        data.SourceOfIncome = dt.Rows[i]["SourceOfIncome"].ToString();
                        data.GuarantorCollateral = dt.Rows[i]["GuarantorCollateral"].ToString();
                        data.DebtStatus = dt.Rows[i]["DebtStatus"].ToString();
                        data.FamilyStatus = dt.Rows[i]["FamilyStatus"].ToString();
                        data.Comments = dt.Rows[i]["Comments"].ToString();

                        if (dt.Rows[i]["DateAdded"].ToString() != "")
                        {
                            data.PromisePaymentDateTS = Convert.ToDateTime(dt.Rows[i]["DateAdded"]).ToString("hh:mm tt dd-MM-yyyy");
                        }
                        else
                        {
                            data.PromisePaymentDateTS = "";
                        }

                        DataList.Add(data);
                        #endregion params
                    }

                    ListHeader.DataList = DataList;

                    ListHeader.DataList = DataList;
                    response.ERR = ERR;
                    response.SMS = SMS;
                    response.Data = ListHeader;
                }
                #endregion data
            }
            catch (Exception ex)
            {
                ERR = "Error";
                SMS = "Something was wrong";
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