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
using WebService.Models.Req.Disbusements;
using WebService.Models.Req.Geolocations;
using WebService.Models.Req.Repayments;
using WebService.Services;

namespace WebService
{
    [BasicAuthentication]
    public class T24_RDVUploadFromDeviceController : ApiController
    {
        Class1 c = new Class1();
        //public IEnumerable<T24_RDVUploadFromDeviceModel> Post([FromUri]string api_name, string api_key, [FromBody] string json)
        public string Post([FromUri]string p, [FromUri]string msgid, [FromBody]string json)
        {
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "", ExSMS = "", ERRCode = "";
            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            List<T24_RDVRS> RSData = new List<T24_RDVRS>();
            string ControllerName = "T24_RDVUploadFromDevice";
            string FileNameForLog = msgid + "_" + ServerDate.Replace("-", "_").Replace(" ", "_").Replace(":", "_");
            try
            {
                #region add log
                c.T24_AddLog(FileNameForLog, "1.RQ-p", p, ControllerName);
                c.T24_AddLog(FileNameForLog, "1.RQ-json", json, ControllerName);
                #region msgid
                if (ERR != "Error")
                {
                    string[] str = c.CheckMsgID(msgid);
                    ERR = str[0];
                    SMS = str[1];
                }
                #endregion
                #endregion
                #region p -> SessionID
                string UserID = "";
                if (ERR != "Error")
                {
                    //p = System.Web.HttpUtility.UrlDecode(p);
                    string[] rs = c.SessionIDCheck(ServerDate, p);
                    ERR = rs[0];
                    SMS = rs[1];
                    ExSMS = rs[2];
                    UserID = rs[3];
                    ERRCode = rs[4];
                }
                #endregion

                #region allow upload
                string UserDeviceID = "";
                if (ERR != "Error")
                {
                    try
                    {
                        DataTable dtCheckIsUpload = new DataTable();
                        dtCheckIsUpload = c.ReturnDT("exec T24_AllowUploadRepayDisbCheck @UserID='" + UserID + "'");
                        if (dtCheckIsUpload.Rows[0][0].ToString() == "0")
                        {
                            ERR = "Error"; //Already uploaded
                            SMS = "Uploading is not allow";
                        }
                        else {
                            try {
                                UserDeviceID = dtCheckIsUpload.Rows[0][1].ToString();
                                if (Convert.ToInt32(UserDeviceID) <= 0) {
                                    ERR = "Error";
                                    SMS = "Error UserDeviceID: Something was wrong";
                                }
                            } catch {
                                ERR = "Error";
                                SMS = "Error UserDeviceID: Something was wrong";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ERR = "Error";
                        SMS = "Error CheckIsUpload: " + ex.Message.ToString();
                    }
                }
                #endregion

                #region check json
                if (ERR != "Error")
                {
                    string[] str = c.CheckObjED(json, "2");
                    ERR = str[0];
                    SMS = str[1];
                    ExSMS = str[2];
                    json = str[3];
                }
                #endregion check json
                #region read json
                T24_RDVRQ jObj = null;
                string JsonCol = "", JsonIss="", JsonVBP="";
                if (ERR != "Error")
                {
                    try
                    {
                        jObj = JsonConvert.DeserializeObject<T24_RDVRQ>(json);
                        JsonCol = jObj.JsonCol;
                        JsonCol = c.Decrypt(JsonCol, c.SeekKeyGet());

                        JsonIss = jObj.JsonIss;
                        JsonIss = c.Decrypt(JsonIss, c.SeekKeyGet());

                        JsonVBP = jObj.JsonVBP;
                        JsonVBP = c.Decrypt(JsonVBP, c.SeekKeyGet());
                    }
                    catch (Exception ex)
                    {
                        ERR = "Error";
                        ExSMS = ex.Message.ToString();
                        //get sms
                        string[] str = c.GetSMSByMsgID("3");
                        ERR = str[0];
                        if (ERR == "Error")
                        {
                            SMS = str[1];
                            ExSMS = ExSMS + "|" + str[2];
                        }
                        else
                        {
                            SMS = str[3];
                        }
                        ERR = "Error";
                    }
                }
                #endregion

                #region Add JsonCol,JsonIss and JsonVBP
                string sql = "";
                if (ERR != "Error")
                {
                    SqlConnection Con1 = new SqlConnection(c.GetHostConStr());
                    Con1.Open();
                    SqlTransaction Tran1;
                    Tran1 = Con1.BeginTransaction();
                    SqlCommand cmd1 = new SqlCommand();
                    cmd1.Connection = Con1;
                    cmd1.Transaction = Tran1;
                    try
                    {
                        #region Add JsonCol
                        if (JsonCol != "[{}]")
                        {

                            var repayments = JsonConvert.DeserializeObject<List<Repayment>>(JsonCol);
                            foreach (var repay in repayments)
                            {
                                var geoLocation = new GeolocationLoanRepayment();
                                geoLocation.RepayID = repay.RepayID; //dtj.Rows[i]["RepayID"].ToString();
                                geoLocation.LoanProduct = repay.ProdCode; //dtj.Rows[i]["ProdCode"].ToString();
                                geoLocation.CollAmount = decimal.Parse(repay.TotalDue);
                                geoLocation.LoanCurrency = repay.CurrencyID; //dtj.Rows[i]["CurrencyID"].ToString();
                                geoLocation.CID = repay.CustomerID; //dtj.Rows[i]["CustomerID"].ToString();
                                geoLocation.CustName = repay.ClientName; //dtj.Rows[i]["ClientName"].ToString();
                                geoLocation.Geolocation = repay.GeoLocation.Geolocation; //
                                geoLocation.StartDate = repay.GeoLocation.StartDate; //
                                geoLocation.EndDate = repay.GeoLocation.EndDate; //

                                if (repay.MeetingDate != "")
                                {
                                    repay.MeetingDate = Convert.ToDateTime(repay.MeetingDate).ToString("yyyy-MM-dd");
                                }

                                sql = "exec T24_PostRepayFromDevice @RepayID=@RepayID,@IntDue=@IntDue,@PenDue=@PenDue,@PriDue=@PriDue,@ActualRepayAmt=@ActualRepayAmt,@Ref=@Ref,@DeviceDate=@DeviceDate,@COUserDeviceID=@COUserDeviceID,@CreateBy=@CreateBy,@CreateDate=@CreateDate,@PayOff=@PayOff,@CompulsorySaving=@CompulsorySaving,@PaidOffPenaltyAmt=@PaidOffPenaltyAmt,@MonthlyFee=@MonthlyFee";
                                cmd1.Parameters.Clear();
                                cmd1.CommandText = sql;
                                cmd1.Parameters.AddWithValue("@RepayID", repay.RepayID);
                                cmd1.Parameters.AddWithValue("@IntDue", repay.IntDue);
                                cmd1.Parameters.AddWithValue("@PenDue", repay.Penalty);
                                cmd1.Parameters.AddWithValue("@PriDue", repay.PriDue);
                                cmd1.Parameters.AddWithValue("@ActualRepayAmt", repay.TotalDue);
                                cmd1.Parameters.AddWithValue("@Ref", repay.Ref);
                                cmd1.Parameters.AddWithValue("@DeviceDate", repay.DeviceDate);
                                cmd1.Parameters.AddWithValue("@COUserDeviceID", UserDeviceID);
                                cmd1.Parameters.AddWithValue("@CreateBy", UserID);
                                cmd1.Parameters.AddWithValue("@CreateDate", ServerDate);
                                cmd1.Parameters.AddWithValue("@PayOff", repay.PayOff);
                                cmd1.Parameters.AddWithValue("@CompulsorySaving", repay.CompulsorySaving);
                                cmd1.Parameters.AddWithValue("@PaidOffPenaltyAmt", repay.PaidOffPenaltyAmt);
                                cmd1.Parameters.AddWithValue("@MonthlyFee", repay.MonthlyFee);
                                cmd1.ExecuteNonQuery();

                                if (repay.PayOff == "1")
                                {
                                    sql = "exec RepayPayOffReason_ins '" + repay.RepayID + "',N'" + repay.PayOffReason + "','" + repay.MeetingDate + "','','" + UserDeviceID + "'";
                                    cmd1.Parameters.Clear();
                                    cmd1.CommandText = sql;
                                    cmd1.ExecuteNonQuery();
                                }

                                // Add value geolocation loan repayment
                                sql = "exec AddGeolocationLoanAppRepay @CID=@CID,@CustName=@CustName,@CollAmount=@CollAmount,@LoanCurrency=@LoanCurrency,@LoanProduct=@LoanProduct,@RepayID=@RepayID,@Geolocation=@Geolocation,@StartDate=@StartDate,@EndDate=@EndDate";
                                cmd1.Parameters.Clear();
                                cmd1.CommandText = sql;
                                cmd1.Parameters.AddWithValue("@CID", geoLocation.CID.ToString());
                                cmd1.Parameters.AddWithValue("@CustName", geoLocation.CustName.ToString());
                                cmd1.Parameters.AddWithValue("@CollAmount", geoLocation.CollAmount.ToString());
                                cmd1.Parameters.AddWithValue("@LoanCurrency", geoLocation.LoanCurrency.ToString());
                                cmd1.Parameters.AddWithValue("@LoanProduct", geoLocation.LoanProduct.ToString());
                                cmd1.Parameters.AddWithValue("@RepayID", geoLocation.RepayID.ToString());
                                cmd1.Parameters.AddWithValue("@Geolocation", geoLocation.Geolocation.ToString());
                                cmd1.Parameters.AddWithValue("@StartDate", geoLocation.StartDate.ToString());
                                cmd1.Parameters.AddWithValue("@EndDate", geoLocation.EndDate.ToString());
                                cmd1.ExecuteNonQuery();
                            }
                        }
                        #endregion

                        #region Add JsonIss
                        if (JsonIss != "[{}]")
                        {
                            var disbs = JsonConvert.DeserializeObject<List<Disbusement>>(JsonIss);
                            foreach (var dibs in disbs)
                            {
                                var geoLocation = new GeolocationLoanDisbusement();
                                geoLocation.LoanAA = dibs.AccountNumber;  //dtj.Rows[i]["AccountNumber"].ToString();
                                geoLocation.LoanProduct = dibs.ProdCode;  //dtj.Rows[i]["ProdCode"].ToString();
                                geoLocation.DisAmount =decimal.Parse(dibs.ApproveAmount);  //decimal.Parse(dtj.Rows[i]["ApprovedAmount"].ToString().Replace(",", ""));
                                geoLocation.LoanCurrency = dibs.DisbCCY;  //dtj.Rows[i]["DisbCCY"].ToString();
                                geoLocation.CID = dibs.ClientID;  //dtj.Rows[i]["ClientID"].ToString();
                                geoLocation.CustName = dibs.ClientName;  //dtj.Rows[i]["ClientName"].ToString();
                                geoLocation.Geolocation = dibs.GeoLocation.Geolocation;  //geol.Geolocation;
                                geoLocation.StartDate = dibs.GeoLocation.StartDate;  //geol.StartDate;
                                geoLocation.EndDate = dibs.GeoLocation.EndDate;  // geol.EndDate;

                                sql = "exec T24_PostDisbFromDevice @DisbID=@DisbID,@ActualChgAmt=@ActualChgAmt,@ActualDisbAmt=@ActualDisbAmt,@Ref=@Ref,@RefFee=@RefFee,@DeviceDate=@DeviceDate,@COUserDeviceID=@COUserDeviceID,@CreateBy=@CreateBy,@CreateDate=@CreateDate,@AAID=@AAID";
                                cmd1.Parameters.Clear();
                                cmd1.CommandText = sql;
                                cmd1.Parameters.AddWithValue("@DisbID", dibs.DisburseID);
                                cmd1.Parameters.AddWithValue("@ActualChgAmt", dibs.FeeAmount);
                                cmd1.Parameters.AddWithValue("@ActualDisbAmt", dibs.ApproveAmount);
                                cmd1.Parameters.AddWithValue("@Ref", dibs.Ref);
                                cmd1.Parameters.AddWithValue("@RefFee", dibs.RefFee);
                                cmd1.Parameters.AddWithValue("@DeviceDate", dibs.DeviceDate);
                                cmd1.Parameters.AddWithValue("@COUserDeviceID", UserDeviceID);
                                cmd1.Parameters.AddWithValue("@CreateBy", UserID);
                                cmd1.Parameters.AddWithValue("@CreateDate", ServerDate);
                                cmd1.Parameters.AddWithValue("@AAID", dibs.AccountNumber);
                                cmd1.ExecuteNonQuery();


                                // Add value geolocation loan disbusement
                                sql = "exec AddGeolocationLoanAppDisbuse @CID=@CID,@CustName=@CustName,@DisbAmount=@DisbAmount,@LoanCurrency=@LoanCurrency,@LoanProduct=@LoanProduct,@LoanAA=@LoanAA,@Geolocation=@Geolocation,@StartDate=@StartDate,@EndDate=@EndDate";
                                cmd1.Parameters.Clear();
                                cmd1.CommandText = sql;
                                cmd1.Parameters.AddWithValue("@CID", geoLocation.CID.ToString());
                                cmd1.Parameters.AddWithValue("@CustName", geoLocation.CustName.ToString());
                                cmd1.Parameters.AddWithValue("@DisbAmount", geoLocation.DisAmount.ToString());
                                cmd1.Parameters.AddWithValue("@LoanCurrency", geoLocation.LoanCurrency.ToString());
                                cmd1.Parameters.AddWithValue("@LoanProduct", geoLocation.LoanProduct.ToString());
                                cmd1.Parameters.AddWithValue("@LoanAA", geoLocation.LoanAA.ToString());
                                cmd1.Parameters.AddWithValue("@Geolocation", geoLocation.Geolocation.ToString());
                                cmd1.Parameters.AddWithValue("@StartDate", geoLocation.StartDate.ToString());
                                cmd1.Parameters.AddWithValue("@EndDate", geoLocation.EndDate.ToString());
                                cmd1.ExecuteNonQuery();
                            }
                        }
                        #endregion

                        #region Add JsonVBP
                        if (JsonVBP != "[{}]")
                        {
                            String ContraAccountIDVBP, RefVBP, ValueDateVBP, PercentageVBP, AmountVBP, BeforeTaxVBP, WitholdingTaxVBP, NetVBPIncentiveVBP, CurrencyIDVBP, VillageBankIDVBP, VillageBankNameVBP, DeviceDateVBP, UserDeviceIDVBP;
                            DataTable dtjVBP = JsonConvert.DeserializeObject<DataTable>(JsonVBP);
                            for (int i = 0; i <= dtjVBP.Rows.Count - 1; i++)
                            {
                                ContraAccountIDVBP = dtjVBP.Rows[i]["ContraAccountID"].ToString();
                                RefVBP = dtjVBP.Rows[i]["Ref"].ToString();
                                ValueDateVBP = dtjVBP.Rows[i]["ValueDate"].ToString();
                                PercentageVBP = dtjVBP.Rows[i]["Percentage"].ToString().Replace(",", "");
                                AmountVBP = dtjVBP.Rows[i]["Amount"].ToString().Replace(",", "");
                                BeforeTaxVBP = dtjVBP.Rows[i]["BeforeTax"].ToString().Replace(",", "");
                                WitholdingTaxVBP = dtjVBP.Rows[i]["WitholdingTax"].ToString().Replace(",", "");
                                NetVBPIncentiveVBP = dtjVBP.Rows[i]["NetVBPIncentive"].ToString().Replace(",", "");
                                CurrencyIDVBP = dtjVBP.Rows[i]["CurrencyID"].ToString();
                                if (CurrencyIDVBP == "KHR")
                                {
                                    CurrencyIDVBP = "1";
                                }
                                else if (CurrencyIDVBP == "USD")
                                {
                                    CurrencyIDVBP = "2";
                                }
                                else
                                {
                                    CurrencyIDVBP = "5";
                                }
                                VillageBankIDVBP = dtjVBP.Rows[i]["VillageBankID"].ToString();
                                VillageBankNameVBP = dtjVBP.Rows[i]["VillageBankName"].ToString();
                                DeviceDateVBP = dtjVBP.Rows[i]["DeviceDate"].ToString();
                                //UserDeviceIDVBP = dtjVBP.Rows[i]["UserDeviceID"].ToString();

                                sql = "insert into tblVBP (ContraAccountID, Ref, ValueDate, Percentage, Amount,BeforeTax,WitholdingTax,NetVBPIncentive, CurrencyID, VillageBankID,VillageBankName, DeviceDate, UserDeviceID,ServerDate,PostStatus,PostStatusID,UploadByUserID) values (@ContraAccountID,@Ref,@ValueDate,@Percentage,@Amount,@BeforeTax,@WitholdingTax,@NetVBPIncentive,@CurrencyID,@VillageBankID,@VillageBankName,@DeviceDate,@UserDeviceID,@ServerDate,@PostStatus,@PostStatusID,@UploadByUserID)";
                                cmd1.Parameters.Clear();
                                cmd1.CommandText = sql;
                                cmd1.Parameters.AddWithValue("@ContraAccountID", ContraAccountIDVBP);
                                cmd1.Parameters.AddWithValue("@Ref", RefVBP);
                                cmd1.Parameters.AddWithValue("@ValueDate", ValueDateVBP);
                                cmd1.Parameters.AddWithValue("@Percentage", PercentageVBP);
                                cmd1.Parameters.AddWithValue("@Amount", AmountVBP);
                                cmd1.Parameters.AddWithValue("@BeforeTax", BeforeTaxVBP);
                                cmd1.Parameters.AddWithValue("@WitholdingTax", WitholdingTaxVBP);
                                cmd1.Parameters.AddWithValue("@NetVBPIncentive", NetVBPIncentiveVBP);
                                cmd1.Parameters.AddWithValue("@CurrencyID", CurrencyIDVBP);
                                cmd1.Parameters.AddWithValue("@VillageBankID", VillageBankIDVBP);
                                cmd1.Parameters.AddWithValue("@VillageBankName", VillageBankNameVBP);
                                cmd1.Parameters.AddWithValue("@DeviceDate", DeviceDateVBP);
                                cmd1.Parameters.AddWithValue("@UserDeviceID", UserDeviceID);
                                cmd1.Parameters.AddWithValue("@ServerDate", ServerDate);
                                cmd1.Parameters.AddWithValue("@PostStatus", "New");
                                cmd1.Parameters.AddWithValue("@PostStatusID", 1);
                                cmd1.Parameters.AddWithValue("@UploadByUserID", UserID);
                                cmd1.ExecuteNonQuery();
                            }
                        }
                        #endregion

                        #region update AllowUpload
                        if (ERR != "Error")
                        {
                            //sql = "update tblUser set AllowUpload=0 where UserID='" + UserID + "'";
                            //cmd1.Parameters.Clear();
                            //cmd1.CommandText = sql;
                            //cmd1.ExecuteNonQuery();
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        ERR = "Error";
                        SMS = "Error Collection: " + ex.Message.ToString();
                    }

                    #region finish trn
                    if (ERR != "Error")
                    {
                        ERR = "Succeed";
                        SMS = "Upload is completed";
                        Tran1.Commit();
                        Con1.Close();
                    }
                    else
                    {
                        Tran1.Rollback();
                        Con1.Close();
                    }
                    #endregion
                }
                #endregion

            }
            catch (Exception ex)
            {
                ERR = "Error";
                SMS = "Something was wrong at line:" + c.GetLineNumber(ex) + " | Ex:" + ex.Message.ToString();
            }

            T24_RDVRS ListHeader = new T24_RDVRS();
            ListHeader.ERR = ERR;
            ListHeader.SMS = SMS;
            ListHeader.ERRCode = ERRCode;
            RSData.Add(ListHeader);

            string RSDataStr = "";
            try
            {
                var jsonRS = new JavaScriptSerializer().Serialize(RSData);
                RSDataStr = c.Encrypt(jsonRS, c.SeekKeyGet());
                c.T24_AddLog(FileNameForLog, "RS", jsonRS.ToString(), ControllerName);
            }
            catch { }
            return RSDataStr;
        }
    }
}

public class T24_RDVRQ{
    public string JsonCol { get; set; }
    public string JsonIss { get; set; }
    public string JsonVBP { get; set; }
}
public class T24_RDVRS
{
    public string ERR { get; set; }
    public string SMS { get; set; }
    public string ERRCode { get; set; }
}