using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebService.Models.Req.LoanApp;
using WebService.Repositories;
using WebService.Models.Res.LoanApp;
using Newtonsoft.Json;
using System.Data;

namespace WebService.ApiControllers.Loans
{
    [BasicAuthentication]
    public class LoanResudyController : ApiController
    {
        [Route("api/v1/loanresudy-convert-status")]
        [HttpPost()]
        public List<LoanRestudyRes> ConvertStatusLoanResudy([FromUri] string p, [FromUri] string msgid, [FromBody] string json)
        {
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "", ExSMS = "", ERRCode = "";
            LoanRestudyRepo loanRestudy = new LoanRestudyRepo();
            LoanRestudyRes loanRes = new LoanRestudyRes();
            var loanList = new List<CheckStatusLoanRestudyRes>();
            var loanResList= new List<LoanRestudyRes>();
            var jObj =new List<CheckStatusLoanRestudy>();
            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string ControllerName = "LoanResudy";
            string FileNameForLog = msgid +"_" + ServerDate.Replace("-", "_").Replace(" ", "_").Replace(":", "_").Replace(".", "_");
            c.T24_AddLog(FileNameForLog, "1.RQ:", json, ControllerName);
            try
            {
                #region msgid
                if (ERR != "Error")
                {
                    string[] str = c.CheckMsgID(msgid);
                    ERR = str[0];
                    SMS = str[1];
                }
                #endregion
                #region p -> SessionID
                string UserID = "";
                if (ERR != "Error")
                {
                    string[] rs = c.SessionIDCheck(ServerDate, p);
                    ERR = rs[0];
                    SMS = rs[1];
                    ExSMS = rs[2];
                    UserID = rs[3];
                    ERRCode = rs[4];
                }
                if (ERR == "Error")
                {
                    var loanE = new CheckStatusLoanRestudyRes
                    {
                        Status = "0",
                        Message = $"Invalid Session: {p}"
                    };
                    loanList.Add(loanE);

                    loanRes.ERR = "Error";
                    loanRes.DataList = loanList;
                    loanResList.Add(loanRes);

                    c.T24_AddLog(FileNameForLog, $"3.ERR:Update Sync Status Is Invalid Session : {p}", json, ControllerName);
                    return loanResList;
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
                if (ERR != "Error")
                {
                    #endregion check json
                    jObj = JsonConvert.DeserializeObject<List<CheckStatusLoanRestudy>>(json.Replace("\r\n    ", ""));
                   
                    if (jObj.Count() > 0)
                    {
                        string resStr = "";
                        foreach (var item in jObj)
                        {
                            var res = loanRestudy.UpdateSyncStatusRestudyByLoanAppId(item);
                            var loans = new CheckStatusLoanRestudyRes
                            {
                                Status = res,
                                Message = res == "1" ? $"Update Sync Status By LoanAppId:{item.LoanAppId} Is Successfully." : $"Update Sync Status LoanAppId:{item.LoanAppId} Is Failure."
                            };
                            loanList.Add(loans);
                            resStr += "#" + loans.Message;
                        }

                        loanRes.ERR = "Success";
                        loanRes.DataList = loanList;
                        loanResList.Add(loanRes);

                        c.T24_AddLog(FileNameForLog, "2.RS", $"{resStr}.", ControllerName);
                        return loanResList;
                    }
                }
                var loan = new CheckStatusLoanRestudyRes
                {
                    Status = "0",
                    Message = $"Invalid Json Request: {json}"
                };
                loanList.Add(loan);

                loanRes.ERR = "Error";
                loanRes.DataList = loanList;
                loanResList.Add(loanRes);

                c.T24_AddLog(FileNameForLog, "3.ERR:Update sync status is invalid", json, ControllerName);
                return loanResList;

            }
            catch(Exception ex)
            {
                var loan = new CheckStatusLoanRestudyRes
                {
                    Status = "0",
                    Message = $"Invalid Json Request: {ex.Message}"
                };
                loanList.Add(loan);

                loanRes.ERR = "Error";
                loanRes.DataList = loanList;
                loanResList.Add(loanRes);

                c.T24_AddLog(FileNameForLog, "3.ERR:Update sync status is invalid",json, ControllerName);
                return loanResList;
            }
            
        }

    }
}