using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Http;
using Dapper;

namespace WebService
{
    public class LoanOverdueMeetingLogPostController : ApiController
    {
        public JsonResponse<string> Post([FromUri] string api_name, string api_key, string json)
        {
            //json=[{"LoanAA": "none", "OverdueType":"none","MainReason":"none","Reason":"none","SolveBy":"none","CutomerRating":"none","ManagementAction":"none","AccuracyOfUseCredit":"none","StatusOfSolutions":"2017-09-08+07:00","PromisePaymentDate":"2017-09-08+07:00","PromiseAmountCurrency":"USD","PromisedAmount":"none","SourceOfMoneyPaid":"none","CutomerAttitude":"none","SourceOfIncome":"none","GuarantorCollateral":"none","DebtStatus":"none","FamilyStatus":"none","Comments":"none","UserId":"5029"}]
            var response = new JsonResponse<string>();
            Class1 c = new Class1();
            Common cmn = new Common();
            string ERR = "Succeed", SMS = "";
            int id = 0;

            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            try
            {
                #region api
                string[] CheckApi = c.CheckApi(api_name, api_key);
                ERR = CheckApi[0];
                SMS = CheckApi[1];
                #endregion api

                #region json to object    
                string LoanAcc = "", SolveBy = "", PromiseAmountCurrency = "", Comments = "", PromisePaymentDate = "", UserId = "";
                string PromisedAmount = "0";
                string
                    OverdueType = "0", MainReason = "0", Reason = "0", CutomerRating = "0", ManagementAction = "0",
                    AccuracyOfUseCredit = "0", StatusOfSolutions = "0", SourceOfMoneyPaid = "0", CutomerAttitude = "0", SourceOfIncome = "0", GuarantorCollateral = "0", DebtStatus = "0", FamilyStatus = "0";
                LogsOverdueLogsPostingToJsonModel jObj = null;
                if (ERR != "Error")
                {
                    try
                    {
                        string jres = json.TrimEnd(']').TrimStart('[');
                        jObj = JsonConvert.DeserializeObject<LogsOverdueLogsPostingToJsonModel>(jres);
                        LoanAcc = jObj.LoanAcc;
                        OverdueType = jObj.OverdueType;
                        MainReason = jObj.MainReason;
                        Reason = jObj.Reason;
                        SolveBy = jObj.SolveBy;
                        CutomerRating = jObj.CutomerRating;
                        ManagementAction = jObj.ManagementAction;
                        AccuracyOfUseCredit = jObj.AccuracyOfUseCredit;
                        StatusOfSolutions = jObj.StatusOfSolutions;
                        PromisePaymentDate = jObj.PromisePaymentDate;
                        PromiseAmountCurrency = jObj.PromiseAmountCurrency;
                        PromisedAmount = jObj.PromisedAmount;
                        SourceOfMoneyPaid = jObj.SourceOfMoneyPaid;
                        CutomerAttitude = jObj.CutomerAttitude;
                        SourceOfIncome = jObj.SourceOfIncome;
                        GuarantorCollateral = jObj.GuarantorCollateral;
                        DebtStatus = jObj.DebtStatus;
                        FamilyStatus = jObj.FamilyStatus;
                        Comments = jObj.Comments;
                        UserId = jObj.UserId;


                    }
                    catch (Exception ex)
                    {
                        ERR = "Error";
                        SMS = "Invalid JSON";
                    }
                }
                #endregion json to object

                if (ERR != "Error")
                {
                    #region AddLogsMeeting

                    SqlConnection Con1 = new SqlConnection(c.ConStr());
                    Con1.Open();
                    SqlCommand Com1 = new SqlCommand();
                    Com1.Connection = Con1;
                    var result = Con1.Query<string>("sp_LoanOverdueAddLogs", new
                    {
                        LoanAcc = LoanAcc,
                        OverdueType = OverdueType,
                        MainReason = MainReason,
                        Reason = Reason,
                        SolveBy = SolveBy,
                        CutomerRating = CutomerRating,
                        ManagementAction = ManagementAction,
                        AccuracyOfUseCredit = AccuracyOfUseCredit,
                        StatusOfSolutions = StatusOfSolutions,
                        PromisePaymentDate = Extension.ConvertDate(PromisePaymentDate).ToString("hh:mm tt dd-MMM-yyyy"),
                        PromiseAmountCurrency = PromiseAmountCurrency,
                        PromisedAmount = PromisedAmount,
                        SourceOfMoneyPaid = SourceOfMoneyPaid,
                        CutomerAttitude = CutomerAttitude,
                        SourceOfIncome = SourceOfIncome,
                        GuarantorCollateral = GuarantorCollateral,
                        DebtStatus = DebtStatus,
                        FamilyStatus = FamilyStatus,
                        Comments = Comments,
                        UserId = UserId
                    },
                    commandType: CommandType.StoredProcedure);

                    ERR = "Succeed";
                    id = Convert.ToInt32(result.First());
                    Con1.Close();
                    DataTable dtTokenData = c.ReturnDT("Exec T24_getFirebaseToken '" + UserId + "'");
                    DataTable dtkeys = cmn.GetAllKeys();
                    DataTable dtloandata = cmn.GetLoanData(LoanAcc);
                    DataTable dtusername = cmn.GetUserName(UserId);
                    string title = "ព័ត៌មានដោះស្រាយកម្ចីបង់សងយឺត", description = "", type = "NPLReport";
                    description = dtloandata.Rows[0]["LoanAA"].ToString() + " " + dtloandata.Rows[0]["CustName"].ToString() + "  បានចុះទៅដោះស្រាយដោយ  " + dtusername.Rows[0]["user_code"].ToString() + " - " + dtusername.Rows[0]["user_name"].ToString();
                    cmn.SendNotificationToDeviceToken(title, description, type, id, LoanAcc, dtkeys.Rows[0]["SenderID"].ToString(), dtkeys.Rows[0]["SenderKey"].ToString(), dtTokenData.Rows[0]["token_key"].ToString());
                    Con1.Query<string>("sp_AddNotifications", new { UserId = Convert.ToInt32(dtTokenData.Rows[0]["NodeId"]), FromId = UserId, Title = title, Message = description, IsRead = false, Type = type, LoanAcc = LoanAcc }, commandType: CommandType.StoredProcedure);

                    #endregion
                }
            }
            catch (Exception ex)
            {
                ERR = "Error";
            }

            response.ERR = ERR;
            response.SMS = SMS;
            return response;
        }
    }

}