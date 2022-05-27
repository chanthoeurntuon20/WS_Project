using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Data;

namespace WebService
{
    [BasicAuthentication]
    public class LoanOverdueReportGetController : ApiController
    {
        // GET api/<controller>
        public JsonResponse<LoanOverdueModel> Get(string api_name, string api_key, string json)//json=[{"Page":"1","Limit":"10","GroupId":"11","UserId":"5031"}]
        {
            var response = new JsonResponse<LoanOverdueModel>();
            Common cmn = new Common();
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "", userId = "", groupId = "";
            string AccountNo = "";
            try
            {
                #region api
                string[] CheckApi = c.CheckApi(api_name, api_key);
                ERR = CheckApi[0];
                SMS = CheckApi[1];
                #endregion api

                #region json
                int page = 0, limit = 0;
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
                        page = Convert.ToInt32(CheckJson[4]);
                        limit = Convert.ToInt32(CheckJson[5]);
                        groupId = CheckJson[7].ToString();
                        userId = CheckJson[8].ToString();
                    }
                }
                #endregion json

                #region data
                if (ERR != "Error")
                {
                    LoanOverdueModel ListHeader = new LoanOverdueModel();
                    ListHeader.ERR = ERR;
                    ListHeader.SMS = SMS;
                    page = page == 0 ? page = 1 : page;
                    limit = limit == 0 ? limit = 1 : limit;
                    List<LoanOverdueList> DataList = new List<LoanOverdueList>();

                    DataTable dt = c.ReturnDT("exec T24_GetLoanOverdue @LoanAA='" + AccountNo + "',@PageNumber=" + page + ",@PageSize=" + limit + ",@GroupId='" + groupId + "',@UserId='" + userId + "'");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        #region params
                        LoanOverdueList data = new LoanOverdueList();
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
                        DataList.Add(data);
                        #endregion params
                    }
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