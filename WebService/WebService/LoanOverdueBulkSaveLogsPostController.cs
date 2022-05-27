using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Http;
using Dapper;

namespace WebService
{
    public class LoanOverdueBulkSaveLogsPostController : ApiController
    {
        public JsonResponse<string> Post([FromUri] string api_name, string api_key, string json)
        {
            //json=[{"LoanAA": "none", "OverdueType":"none","MainReason":"none","Reason":"none","SolveBy":"none","CutomerRating":"none","ManagementAction":"none","AccuracyOfUseCredit":"none","StatusOfSolutions":"2017-09-08+07:00","PromisePaymentDate":"2017-09-08+07:00","PromiseAmountCurrency":"USD","PromisedAmount":"none","SourceOfMoneyPaid":"none","CutomerAttitude":"none","SourceOfIncome":"none","GuarantorCollateral":"none","DebtStatus":"none","FamilyStatus":"none","Comments":"none"},{"LoanAA": "none", "OverdueType":"none","MainReason":"none","Reason":"none","SolveBy":"none","CutomerRating":"none","ManagementAction":"none","AccuracyOfUseCredit":"none","StatusOfSolutions":"2017-09-08+07:00","PromisePaymentDate":"2017-09-08+07:00","PromiseAmountCurrency":"USD","PromisedAmount":"none","SourceOfMoneyPaid":"none","CutomerAttitude":"none","SourceOfIncome":"none","GuarantorCollateral":"none","DebtStatus":"none","FamilyStatus":"none","Comments":"none","UserId":"5029"}]
            var response = new JsonResponse<string>();
            Class1 c = new Class1();
            Common cmn = new Common();
            string ERR = "Succeed", SMS = "";
            string title = "ព័ត៌មានដោះស្រាយកម្ចីបង់សងយឺត", description = "", type = "NPLReport";
            DataTable dtloandata = new DataTable();
            DataTable dtTokenData = new DataTable();
            DataTable dtusername = new DataTable();
            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            int id = 0;
            string UserID = "", ExSMS = "Succeed", Ext = "";

            try
            {
                #region api
                string[] CheckApi = c.CheckApi(api_name, api_key);
                ERR = CheckApi[0];
                SMS = CheckApi[1];
                #endregion api

                #region AddBulkLogsMeeting  
                if (ERR != "Error")
                {
                    SqlConnection Con1 = new SqlConnection(c.ConStr());
                    try
                    {
                        DataTable dtkeys = cmn.GetAllKeys();
                        Con1.Open();
                        SqlCommand Com1 = new SqlCommand();
                        Com1.Connection = Con1;
                        var jObj = JsonConvert.DeserializeObject<List<LogsOverdueLogsPostingToJsonModel>>(json);

                        foreach (var r in jObj)
                        {

                            string PromisePaymentDateRevise = "";
                            if (r.PromisePaymentDate.Length > 0)
                            {
                                try
                                {
                                    PromisePaymentDateRevise = "" + c.formatDateHourDate(r.PromisePaymentDate);
                                    PromisePaymentDateRevise = Convert.ToDateTime(PromisePaymentDateRevise).ToString("yyyy-MM-dd hh:mm tt");
                                }
                                catch (Exception)
                                {
                                    ERR = "Error";
                                    SMS = "Invalid promiss date !";
                                }
                            }


                            if (ERR != "Error")
                            {

                                UserID = r.UserId;
                                var res = Con1.Query<string>("sp_LoanOverdueAddLogs", new
                                {
                                    LoanAcc = r.LoanAcc,
                                    OverdueType = r.OverdueType,
                                    MainReason = r.MainReason,
                                    Reason = r.Reason,
                                    SolveBy = r.SolveBy,
                                    CutomerRating = r.CutomerRating,
                                    ManagementAction = r.ManagementAction,
                                    AccuracyOfUseCredit = r.AccuracyOfUseCredit,
                                    StatusOfSolutions = r.StatusOfSolutions,
                                    //PromisePaymentDate = Extension.ConvertDate(r.PromisePaymentDate).ToString("hh:mm tt dd-MMM-yyyy"), 
                                    PromisePaymentDate = PromisePaymentDateRevise,
                                    PromiseAmountCurrency = r.PromiseAmountCurrency,
                                    PromisedAmount = r.PromisedAmount,
                                    SourceOfMoneyPaid = r.SourceOfMoneyPaid,
                                    CutomerAttitude = r.CutomerAttitude,
                                    SourceOfIncome = r.SourceOfIncome,
                                    GuarantorCollateral = r.GuarantorCollateral,
                                    DebtStatus = r.DebtStatus,
                                    FamilyStatus = r.FamilyStatus,
                                    Comments = r.Comments,
                                    UserId = r.UserId
                                },
                                commandType: CommandType.StoredProcedure);
                                ERR = "Succeed";
                                id = Convert.ToInt32(res.First());
                                dtTokenData = c.ReturnDT("Exec T24_getFirebaseToken '" + r.UserId + "'");
                                dtusername = cmn.GetUserName(r.UserId);
                                dtloandata = cmn.GetLoanData(r.LoanAcc);
                                description = dtloandata.Rows[0]["LoanAA"].ToString() + " " + dtloandata.Rows[0]["CustName"].ToString() + "  បានចុះទៅដោះស្រាយដោយ  " + dtusername.Rows[0]["user_code"].ToString() + " - " + dtusername.Rows[0]["user_name"].ToString();
                                if (dtTokenData.Rows.Count > 0)
                                {

                                    try
                                    {

                                        Ext = "title=" + title + ",description=" + description + ",type" + type + "id=" + id + ",SenderID=" + dtkeys.Rows[0]["SenderID"].ToString() + ",SenderKey=" + dtkeys.Rows[0]["SenderID"].ToString() + ",token_key=" + dtTokenData.Rows[0]["token_key"].ToString();

                                        cmn.SendNotificationToDeviceToken(
                                            title, description, type, id,
                                            r.LoanAcc, dtkeys.Rows[0]["SenderID"].ToString(),
                                            dtkeys.Rows[0]["SenderKey"].ToString(),
                                            dtTokenData.Rows[0]["token_key"].ToString());
                                    }
                                    catch (Exception ex)
                                    {
                                        ExSMS = "SendNotificationToDeviceToken step " + ex.Message.ToString() + "===" + Ext; ;
                                    }

                                    try
                                    {
                                        Con1.Query<string>("sp_AddNotifications", new
                                        {
                                            UserId = Convert.ToInt32(dtTokenData.Rows[0]["NodeId"]),
                                            FromId = r.UserId,
                                            Title = title,
                                            Message = description,
                                            IsRead = false,
                                            Type = type,
                                            LoanAcc = r.LoanAcc
                                        }, commandType: CommandType.StoredProcedure);

                                    }
                                    catch (Exception ex) { ExSMS = "AddNotifications step " + ex.Message.ToString(); }
                                }

                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        ERR = "Error";
                        SMS = "Invalid JSON";
                        ExSMS = "Invalid JSON step " + ex.Message.ToString();
                    }
                    finally
                    {
                        Con1.Close();
                    }
                }
                #endregion AddBulkLogsMeeting
            }
            catch (Exception ex)
            {
                ERR = "Error";
                ExSMS = "Final step " + ex.Message.ToString();
            }

            string fileHeader = UserID + "_" + ServerDate;
            c.T24_AddLog(fileHeader, "LoanOverdueBulkSaveLogs", json + " Message : " + ExSMS, "LoanOverdueBulkSaveLogs");

            response.ERR = ERR;
            response.SMS = SMS;
            return response;
        }
    }
}