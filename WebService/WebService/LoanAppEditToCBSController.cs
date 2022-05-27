using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using System.Xml;

namespace WebService
{
    public class LoanAppEditToCBSController : ApiController
    {
        Class1 c = new Class1();
        // POST api/<controller>
        //public IEnumerable<LoanAppEditResModel> Post([FromUri]string api_name, string api_key, string json)
        public string Post([FromUri]string p, [FromUri]string msgid, [FromBody]string json)
        {
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "", ExSMS = "", ERRCode = "";
            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string ControllerName = "LoanAppEditToCBS";
            string FileNameForLog = msgid + "_" + ServerDate.Replace("-", "_").Replace(" ", "_").Replace(":", "_").Replace(".", "_");
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
                #region add log
                if (ERR != "Error")
                {
                    c.T24_AddLog(FileNameForLog, "1.RQ", p, ControllerName);
                }
                #endregion
                #region p -> SessionID
                string pSwitch = p.Substring(0,3);
                string UserID = "";

                if (pSwitch != "sw_")
                {
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
                }
                else {
                    string strs = p.Substring(3, p.Length - 3).Replace(" ","+");
                    ERR = "";
                    SMS = "";
                    UserID = c.Decrypt(strs, c.SeekKeyGet());                    
                    ExSMS = "";
                    ERRCode = "";
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
                //string user = "", pwd = "";
                string LoanAppID = "", AMApproveAmt = "", AMApproveTerm = ""
                    , GroupNumber = "", DisbursementDate = "", FirstRepaymentDate = "", CBSKey = ""
                    , LoanRate = "", LoanCycle = "", MainBusinessID = "", LoanPurposeID = "", CBCREQUIRED = "";
                LoanAppEditToCBSModel jObj = null;
                if (ERR != "Error")
                {
                    try
                    {
                        jObj = JsonConvert.DeserializeObject<LoanAppEditToCBSModel>(json);
                        //user = jObj.user;
                        //pwd = jObj.pwd;
                        #region Loan
                        foreach (var l in jObj.LoanAppEditToCBS)
                        {
                            LoanAppID = l.LoanAppID;
                            DisbursementDate = l.DisbursementDate;//ok
                            AMApproveTerm = l.AMApproveTerm;//ok
                            AMApproveAmt = l.AMApproveAmt;//ok
                            LoanRate = l.AMApproveRate;//ok
                            FirstRepaymentDate = l.FirstRepaymentDate;//ok
                            LoanCycle = l.LoanCycle;//ok
                            GroupNumber = l.GroupNumber;//ok
                            MainBusinessID = l.MainBusinessID;//ok
                            LoanPurposeID = l.LoanPurposeID;//ok
                            CBSKey = l.CBSKey;
                            CBCREQUIRED = l.CBCREQUIRED;
                        }
                        #endregion Loan
                    }
                    catch (Exception ex)
                    {
                        ERR = "Error";
                        SMS = "Invalid JSON: " + ex.Message.ToString();
                    }
                }
                #endregion

                #region Edit to CBS
                if (ERR != "Error")
                {
                    string Reference = "", InstID = "", ErrXml = "";
                    #region - get InstID
                    DataTable dtInst = c.ReturnDT("select l.InstID,u.OfficeID,l.CBSKey from tblLoanApp1 l left join tblUser u on u.UserID=l.CreateBy where l.LoanAppID='" + LoanAppID + "'");
                    InstID = dtInst.Rows[0]["InstID"].ToString();
                    string CreCompany = dtInst.Rows[0]["OfficeID"].ToString();
                    //string CBSKey = dtInst.Rows[0]["CBSKey"].ToString();
                    #endregion - get InstID
                    FileNameForLog = "InstID_" + InstID + "_" + UserID + "_" + LoanAppID + "_" + FileNameForLog;
                    #region - get T24 url                
                    DataTable dtT24Url = new DataTable();
                    dtT24Url = c.ReturnDT("exec T24_GetT24_Url @UserID='" + UserID + "',@UrlID=25,@InstID='" + InstID + "'");
                    string CreUrl = dtT24Url.Rows[0]["CreUrl"].ToString();
                    //string CreCompany = dtT24Url.Rows[0]["CreCompany"].ToString();
                    string CreUserName = dtT24Url.Rows[0]["CreUserName"].ToString();
                    string CrePassword = dtT24Url.Rows[0]["CrePassword"].ToString();
                    #endregion - T24 url                                    
                    #region xml
                    string xmlStr = "<S:Envelope xmlns:S=\"http://schemas.xmlsoap.org/soap/envelope/\"> <S:Body><ns4:WSAMKDLAPPLEDIT "
                    + "xmlns:ns2=\"http://temenos.com/AMKDLAPPLICATIONBOINP\" xmlns:ns3=\"http://temenos.com/AMKDLAPPLICATION\" "
                    + "xmlns:ns4=\"http://temenos.com/AMKTabLnFuncWS\"> <WebRequestCommon> "
                    + "<company>" + CreCompany + "</company> <password>" + CrePassword + "</password> <userName>" + CreUserName + "</userName></WebRequestCommon><OfsFunction/> "
                    + "<AMKDLAPPLICATIONBOINPType id=\"" + CBSKey + "\"> <ns2:APPDATE>" + DisbursementDate
                    + "</ns2:APPDATE> <ns2:LOANTERM>" + AMApproveTerm + "</ns2:LOANTERM> "
                    + "<ns2:AMOUNT>" + AMApproveAmt + "</ns2:AMOUNT>"
                    + "<ns2:INTRATE>" + LoanRate + "</ns2:INTRATE> "
                    + "<ns2:REPAYSTDATE>" + FirstRepaymentDate + "</ns2:REPAYSTDATE> "
                    + "<ns2:LOANCYCLE>" + LoanCycle + "</ns2:LOANCYCLE> "
                    + "<ns2:GROUPNO>" + GroupNumber + "</ns2:GROUPNO> "
                    + "<ns2:gMAINBUSINESS> <ns2:MAINBUSINESS>" + MainBusinessID + "</ns2:MAINBUSINESS></ns2:gMAINBUSINESS> "
                    + "<ns2:gLOANPURPOSE> <ns2:LOANPURPOSE>" + LoanPurposeID + "</ns2:LOANPURPOSE> </ns2:gLOANPURPOSE> "
                    + "<ns2:CBCREQUIRED>" + CBCREQUIRED + "</ns2:CBCREQUIRED> "
                    + "</AMKDLAPPLICATIONBOINPType> </ns4:WSAMKDLAPPLEDIT></S:Body></S:Envelope>";
                    #endregion xml
                    //c.T24_AddLog(fileHeader, "EditLoan_RQ", xmlStr, "EditLoan");
                    c.T24_AddLog(FileNameForLog, "2.XmlRQ", xmlStr, ControllerName);
                    #region call to T24
                    try
                    {
                        var client = new RestClient(CreUrl);
                        var request = new RestRequest(Method.POST);
                        request.AddHeader("cache-control", "no-cache");
                        request.AddHeader("content-type", "text/xml");
                        request.AddParameter("text/xml", xmlStr, ParameterType.RequestBody);
                        IRestResponse response = null;
                        string xmlContent = "", resCode = "";
                        try
                        {
                            System.Threading.Thread.Sleep(10);
                            response = client.Execute(request);
                            //res code
                            resCode = response.StatusCode.ToString();
                            xmlContent = response.Content.ToString();
                        }
                        catch (Exception ex)
                        {
                            ERR = "Error";
                            SMS = ex.Message.ToString();
                        }
                        //c.T24_AddLog(fileHeader, "EditLoan_RS", xmlContent, "EditLoan");
                        c.T24_AddLog(FileNameForLog, "3.XmlRS", xmlContent, ControllerName);
                        if (ERR != "Error")
                        {
                            #region read xml
                            try
                            {
                                XmlDocument doc = new XmlDocument();
                                doc.LoadXml(xmlContent);
                                string successIndicator = doc.GetElementsByTagName("successIndicator").Item(0).InnerText;
                                if (successIndicator == "Success")
                                {
                                    #region Success
                                    Reference = doc.GetElementsByTagName("messageId").Item(0).InnerText;
                                    ERR = "Succeed";
                                    SMS = Reference;
                                    ////get AAID
                                    //if (xmlContent.Contains("ARRANGEMENT:1:1="))
                                    //{
                                    //    int pFrom = xmlContent.IndexOf("ARRANGEMENT:1:1=") + "ARRANGEMENT:1:1=".Length;
                                    //    int pTo = xmlContent.IndexOf("ARRANGEMENT:1:1=") + "ARRANGEMENT:1:1=".Length + 12;
                                    //    AAID = xmlContent.Substring(pFrom, pTo - pFrom).Trim();
                                    //}
                                    #endregion Success
                                }
                                else
                                {
                                    ErrXml = xmlContent;
                                    #region Error
                                    ERR = "Error";
                                    //rsStatusSMS = "Error: Approve to CBS | " + Remark;
                                    if (xmlContent.Contains("<Status>"))
                                    {
                                        int pFrom = xmlContent.IndexOf("<Status>");// + "<Status>".Length;
                                        int pTo = xmlContent.IndexOf("</Status>") + "</Status>".Length;
                                        string MSGxmlContent = xmlContent.Substring(pFrom, pTo - pFrom).Trim();
                                        LoanAppApproveToCBSError.Status obj = c.GenerateXmlObject<LoanAppApproveToCBSError.Status>(MSGxmlContent);
                                        string strMsg = "";
                                        for (int iMsg = 0; iMsg < obj.Messages.Count; iMsg++)
                                        {
                                            if (iMsg == 0)
                                            {
                                                strMsg = obj.Messages[iMsg];
                                            }
                                            else
                                            {
                                                strMsg = strMsg + " | " + obj.Messages[iMsg];
                                            }
                                        }
                                        SMS = strMsg;
                                    }
                                    #endregion Error
                                }
                            }
                            catch (Exception ex)
                            {
                                ERR = "Error";
                                SMS = "Error: Edit to CBS | Can't read response";
                                ExSMS = ex.Message.ToString();
                            }
                            #endregion read xml
                        }

                    }
                    catch (Exception ex)
                    {
                        ERR = "Error";
                        SMS = "Error: Edit to CBS | Can't read response";
                        ExSMS = ex.Message.ToString();
                    }
                    #endregion call to T24   
                    #region update loan
                    try
                    {
                        string ZeroFailOneSucceed = "0";
                        if (ERR == "Succeed")
                        {
                            ZeroFailOneSucceed = "1";
                        }
                        SqlConnection Con0 = new SqlConnection(c.ConStr());
                        try { Con0.Open(); } catch { }
                        SqlCommand Com0 = new SqlCommand();
                        Com0.Connection = Con0;
                        Com0.Parameters.Clear();
                        string sql = "T24_LoanEditToCBSUpdate";
                        Com0.CommandText = sql;
                        Com0.CommandType = CommandType.StoredProcedure;
                        Com0.Parameters.AddWithValue("@UserID", UserID);
                        Com0.Parameters.AddWithValue("@LoanAppID", LoanAppID);
                        Com0.Parameters.AddWithValue("@ZeroFailOneSucceed", ZeroFailOneSucceed);
                        Com0.Parameters.AddWithValue("@DisbDate", DisbursementDate);//ok
                        Com0.Parameters.AddWithValue("@AMApproveTerm", AMApproveTerm.Replace("M", ""));//ok
                        Com0.Parameters.AddWithValue("@AMApproveAmt", AMApproveAmt.Replace(",", ""));//ok
                        Com0.Parameters.AddWithValue("@FirstRepaymentDate", FirstRepaymentDate);//ok                        
                        Com0.Parameters.AddWithValue("@GroupNumber", GroupNumber);//ok
                        Com0.Parameters.AddWithValue("@Remark", SMS);
                        Com0.Parameters.AddWithValue("@Reference", Reference);

                        Com0.Parameters.AddWithValue("@LoanRate", LoanRate);
                        Com0.Parameters.AddWithValue("@LOANCYCLE", LoanCycle);
                        Com0.Parameters.AddWithValue("@MAINBUSINESS", MainBusinessID);
                        Com0.Parameters.AddWithValue("@LOANPURPOSE", LoanPurposeID);
                        Com0.Parameters.AddWithValue("@CBCREQUIRED", CBCREQUIRED);
                        Com0.ExecuteNonQuery();
                        try { Con0.Close(); } catch { }
                    }
                    catch (Exception ex) {
                        ERR = "Error";
                        SMS = "Error: Edit T24_LoanEditToCBSUpdate";
                        ExSMS = ex.Message.ToString();
                    }
                    #endregion update loan

                }
                #endregion Edit to CBS

            }
            catch (Exception ex) { ERR = "Error"; SMS = ex.Message.ToString(); }

            List<LoanAppEditResModel> RSData = new List<LoanAppEditResModel>();
            LoanAppEditResModel DataList = new LoanAppEditResModel();
            DataList.ERR = ERR;
            DataList.SMS = SMS;
            DataList.ERRCode = ERRCode;
            RSData.Add(DataList);

            string RSDataStr = "";
            try
            {
                var jsonRS = new JavaScriptSerializer().Serialize(RSData);
                RSDataStr = c.Encrypt(jsonRS, c.SeekKeyGet());
                c.T24_AddLog(FileNameForLog, "4.RS", jsonRS.ToString(), ControllerName);
            }
            catch { }
            return RSDataStr;
        }

    }
    public class LoanAppEditResModel
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
        public string ERRCode { get; set; }
    }
}