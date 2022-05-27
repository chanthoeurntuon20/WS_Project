using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml;

namespace WebService
{
    public class LoanAppApproveToCBSController : ApiController
    {

        // POST api/<controller>
        public IEnumerable<LoanAppApproveResModel> Post([FromUri]string api_name, string api_key, string json)
        {
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "";
            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string fileHeader = "";
            try
            {
                #region add log
                try
                {
                    //c.T24_AddLog("ApproveLoan_json_"+ServerDate +"_"+ api_name, "ApproveLoan_json", json);
                    c.T24_AddLog("Incoming_ApproveLoan_" + ServerDate, "Incoming_ApproveLoan_Log", json, "ApproveLoan");
                }
                catch { }
                #endregion add log

                #region api
                string[] CheckApi = c.CheckApi(api_name, api_key);
                ERR = CheckApi[0];
                SMS = CheckApi[1];
                #endregion api

                #region json to object
                string user = "", pwd = "";
                string LoanAppID = "", DeskCheckID = "", PreCheckID = "", AMDebtFound = "", AMOpinion = "", AMApproveAmt = "", AMApproveTerm = ""
                    , AMApproveRate = "", GroupNumber = "", DisbursementDate = "", FirstRepaymentDate = "", CBSKey = "", AccountID = "", AAID = "";
                LoanAppApproveToCBCModel jObj = null;
                if (ERR != "Error")
                {
                    try
                    {
                        jObj = JsonConvert.DeserializeObject<LoanAppApproveToCBCModel>(json);
                        user = jObj.user;
                        pwd = jObj.pwd;
                        #region Loan
                        foreach (var l in jObj.LoanAppApproveToCBS)
                        {
                            LoanAppID = l.LoanAppID;
                            DeskCheckID = l.DeskCheckID;
                            PreCheckID = l.PreCheckID;
                            AMDebtFound = l.AMDebtFound;
                            AMOpinion = l.AMOpinion;
                            AMApproveAmt = l.AMApproveAmt;
                            AMApproveTerm = l.AMApproveTerm;
                            AMApproveRate = l.AMApproveRate;
                            GroupNumber = l.GroupNumber;
                            DisbursementDate = l.DisbursementDate;
                            FirstRepaymentDate = l.FirstRepaymentDate;
                            CBSKey = l.CBSKey;
                            AccountID = l.AccountID;

                        }
                        #endregion Loan
                    }
                    catch(Exception ex)
                    {
                        ERR = "Error";
                        SMS = "Invalid JSON: "+ ex.Message.ToString();
                    }
                }
                #endregion json to object
                #region get userid
                string UserID = "";
                if (ERR != "Error")
                {
                    DataTable dt = c.ReturnDT("exec T24_check_user @user='" + user + "',@pwd='" + c.Encrypt(pwd, c.SeekKeyGet()) + "'");
                    ERR = dt.Rows[0]["ERR"].ToString();
                    SMS = dt.Rows[0]["SMS"].ToString();
                    UserID = SMS;

                    

                    if (ERR == "Error")
                    {
                        SMS = "Invalid User/Pwd";
                    }
                }
                #endregion get userid

                #region Approve to CBS
                if (ERR != "Error") { 
                    string Reference = "", InstID="", ErrXml = "";
                    #region - get InstID
                    DataTable dtInst = c.ReturnDT("select l.InstID,u.OfficeID,l.CBSKey from tblLoanApp1 l left join tblUser u on u.UserID=l.CreateBy where l.LoanAppID='" + LoanAppID + "'");
                    InstID=dtInst.Rows[0]["InstID"].ToString();
                    string CreCompany = dtInst.Rows[0]["OfficeID"].ToString();
                    CBSKey = dtInst.Rows[0]["CBSKey"].ToString();
                    #endregion - get InstID
                    fileHeader = "LoanAppApproveToCBS_InstID_" + InstID+"_" + UserID + "_" + LoanAppID + "_" + ServerDate;
                    #region - get T24 url                
                    DataTable dtT24Url = new DataTable();
                    dtT24Url = c.ReturnDT("exec T24_GetT24_Url @UserID='" + UserID + "',@UrlID=7,@InstID='"+InstID+"'");
                    string CreUrl = dtT24Url.Rows[0]["CreUrl"].ToString();
                    //string CreCompany = dtT24Url.Rows[0]["CreCompany"].ToString();
                    string CreUserName = dtT24Url.Rows[0]["CreUserName"].ToString();
                    string CrePassword = dtT24Url.Rows[0]["CrePassword"].ToString();
                    #endregion - T24 url                                    
                    #region xml
                    //string xmlStr = "<?xml version=\"1.0\"?><soapenv:Envelope xmlns:amk1=\"http://temenos.com/AMKDLAPPLICATIONBOLNAPPROVE\" "
                    //+ "xmlns:amk=\"http://temenos.com/AMKAPPLOAN\" xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\"><soapenv:Header/>"
                    //+ "<soapenv:Body><amk:APPROVELOAN><!--Optional:--><WebRequestCommon><!--Optional:-->"
                    //+ "<company>" + CreCompany + "</company><password>" + CrePassword + "</password><userName>" + CreUserName + "</userName>"
                    //+ "</WebRequestCommon>"
                    //+ "<OfsFunction>"
                    //+ "<messageId>" + "APPLOAN-" + LoanAppID +"-"+ fileHeader+ "</messageId>"
                    //+ "</OfsFunction>"
                    //+ "<AMKDLAPPLICATIONBOLNAPPROVEType id=\"" + CBSKey + "\">"
                    //+ "<amk1:DRAWDOWNACCT>" + AccountID + "</amk1:DRAWDOWNACCT><amk1:SETTLEMENTACCT>" + AccountID + "</amk1:SETTLEMENTACCT>"
                    //+ "</AMKDLAPPLICATIONBOLNAPPROVEType></amk:APPROVELOAN>"
                    //+ "</soapenv:Body></soapenv:Envelope>";
                    string xmlStr = "<?xml version=\"1.0\"?><soapenv:Envelope xmlns:amk1=\"http://temenos.com/AMKDLAPPLICATIONBOLNAPPROVE\" "
                                    + "xmlns:amk=\"http://temenos.com/AMKAPPLOAN\" xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\"><soapenv:Header/>"
                                    + "<soapenv:Body><amk:APPROVELOAN><WebRequestCommon>"
                                    + "<company>" + CreCompany + "</company><password>" + CrePassword + "</password><userName>" + CreUserName + "</userName>"
                                    + "</WebRequestCommon><OfsFunction> </OfsFunction>"
                                    + "<AMKDLAPPLICATIONBOLNAPPROVEType id=\"" + CBSKey + "\">"
                                    + "<amk1:DRAWDOWNACCT>" + AccountID + "</amk1:DRAWDOWNACCT><amk1:SETTLEMENTACCT>" + AccountID + "</amk1:SETTLEMENTACCT>"
                                    + "</AMKDLAPPLICATIONBOLNAPPROVEType></amk:APPROVELOAN>"
                                    + "</soapenv:Body></soapenv:Envelope>";
                    #endregion xml
                    //c.ReturnDT("update tblLoanAppListForOpenToCBSLogApprove set StartDate=getdate() where LoanAppID='" + LoanAppID + "' and CBSKey='" + CBSKey + "' and AccountID='" + AccountID + "'");
                    c.T24_AddLog(fileHeader, "ApproveLoan_RQ", xmlStr, "ApproveLoan");
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
                        c.T24_AddLog(fileHeader, "ApproveLoan_RS", xmlContent, "ApproveLoan");
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
                                    //Reference = doc.GetElementsByTagName("transactionId").Item(0).InnerText;
                                    //ERR = "Succeed";
                                    //SMS = Reference;
                                    ////get AAID
                                    //if (xmlContent.Contains("ARRANGEMENT:1:1="))
                                    //{
                                    //    int pFrom = xmlContent.IndexOf("ARRANGEMENT:1:1=") + "ARRANGEMENT:1:1=".Length;
                                    //    int pTo = xmlContent.IndexOf("ARRANGEMENT:1:1=") + "ARRANGEMENT:1:1=".Length + 12;
                                    //    AAID = xmlContent.Substring(pFrom, pTo - pFrom).Trim();
                                    //}
                                    #region Success
                                    Reference = doc.GetElementsByTagName("transactionId").Item(0).InnerText;
                                    ERR = "Succeed";
                                    SMS = Reference;
                                    //get AAID
                                    if (xmlContent.Contains("ARRANGEMENT:1:1="))
                                    {
                                        int pFrom = xmlContent.IndexOf("ARRANGEMENT:1:1=") + "ARRANGEMENT:1:1=".Length;
                                        int pTo = xmlContent.IndexOf("ARRANGEMENT:1:1=") + "ARRANGEMENT:1:1=".Length + 12;
                                        AAID = xmlContent.Substring(pFrom, pTo - pFrom).Trim();
                                    }
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
                            } catch (Exception ex)
                            {
                                ERR = "Error";
                                SMS = "Error: Approve to CBS | Can't read response";
                            }
                            #endregion read xml
                        }

                    }
                    catch (Exception ex)
                    {
                        ERR = "Error";
                        SMS = "Error: Approve to CBS | Can't read response";
                    }
                    #endregion call to T24   
                    #region update loan
                    try {
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
                        string sql = "T24_LoanApproveUpdateStatus";
                        Com0.CommandText = sql;
                        Com0.CommandType = CommandType.StoredProcedure;
                        Com0.Parameters.AddWithValue("@UserID", UserID);
                        Com0.Parameters.AddWithValue("@LoanAppID", LoanAppID);
                        Com0.Parameters.AddWithValue("@ZeroFailOneSucceed", ZeroFailOneSucceed);
                        Com0.Parameters.AddWithValue("@AAID", AAID);
                        Com0.Parameters.AddWithValue("@Remark", SMS);
                        Com0.ExecuteNonQuery();
                        try { Con0.Close(); } catch { }
                    }
                    catch { }
                    #endregion update loan
                    #region Compulsory Account
                    //if (ERR != "Error")
                    //{
                    //    #region Get Customer Info
                    //    DataTable dtCus = c.ReturnDT("select p.CustomerID,(p.LastName+', '+p.FirstName) as ACCOUNTTITLE1,(p.LastName+', '+p.FirstName) as SHORTTITLE,(select l.Currency from tblLoanApp1 l where LoanAppID=p.LoanAppID) as LoanCurrency from tblLoanAppPerson2 p where p.LoanAppPersonTypeID=31 and p.LoanAppID='" + LoanAppID + "'");
                    //    string CustomerID = dtCus.Rows[0]["CustomerID"].ToString();
                    //    string ACCOUNTTITLE1 = dtCus.Rows[0]["ACCOUNTTITLE1"].ToString();
                    //    string SHORTTITLE = dtCus.Rows[0]["SHORTTITLE"].ToString();
                    //    string LoanCurrency = dtCus.Rows[0]["LoanCurrency"].ToString();
                    //    #endregion Get Customer Info

                    //    int isAccAdd = 0;
                    //    #region Get Compulsory Account
                    //    try
                    //    {
                    //        string url = c.GetTabletWSUrl() + "/api/AccountGetFromCBS?api_name=" + c.GetTabletWSAPIName()
                    //        + "&api_key=" + c.GetTabletWSAPIKey()
                    //        + "&type=json&json=[{\"user\": \"none\", \"pwd\":\"none\",\"device_id\":\"none\",\"app_vName\":\"none\","
                    //        + "\"CreCompany\":\"" + CreCompany + "\",\"CustomerID\":\"" + CustomerID
                    //        + "\",\"CATEGORY\":\"6080\",\"ACCOUNTTITLE1\":\"" + ACCOUNTTITLE1
                    //        + "\",\"SHORTTITLE\":\"" + SHORTTITLE + "\",\"LoanCurrency\":\"" + LoanCurrency + "\"}]";
                    //        c.T24_AddLog(fileHeader, "GetCompulsoryAccount_RQ", url, "ApproveLoan");
                    //        var client = new RestClient(url);
                    //        var Authenticator = new HttpBasicAuthenticator(c.GetTabletWSUser(), c.GetTabletWSPwd());
                    //        client.Authenticator = Authenticator;
                    //        var request = new RestRequest(Method.GET);
                    //        IRestResponse response = client.Execute(request);
                    //        string jres = response.Content.ToString().TrimEnd(']').TrimStart('[');
                    //        //c.T24_AddLog(fileHeader, "GetCompulsoryAccount", response.Content.ToString().TrimEnd(']').TrimStart('['));
                    //        c.T24_AddLog(fileHeader, "GetCompulsoryAccount_RS", response.Content.ToString().TrimEnd(']').TrimStart('['), "ApproveLoan");
                    //        AccountCreationToCBSModel jobAcc = JsonConvert.DeserializeObject<AccountCreationToCBSModel>(jres);
                    //        int aiCount = 0;
                    //        foreach (var ai in jobAcc.DataList)
                    //        {
                    //            aiCount++;
                    //            if (aiCount == 1)
                    //            {
                    //                isAccAdd = 1;
                    //                string CompulsoryAccountID = ai.AccountID;
                    //                c.ReturnDT("update tblLoanApp1 set CompulsoryAccountID='" + CompulsoryAccountID + "' where LoanAppID='" + LoanAppID + "'");
                    //            }
                    //        }
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        ERR = "Error";
                    //        SMS = "Error: Get Compulsory Account";
                    //        c.T24_AddLog(fileHeader, "GetCompulsoryAccount_RS", ex.Message.ToString(), "ApproveLoan");
                    //    }
                    //    #endregion Get Compulsory Account

                    //    #region Create Compulsory Account
                    //    if (isAccAdd == 0)
                    //    {
                    //        try
                    //        {
                    //            string url = c.GetTabletWSUrl() + "/api/AccountCreationToCBS?api_name=" + c.GetTabletWSAPIName()
                    //            + "&api_key=" + c.GetTabletWSAPIKey()
                    //            + "&type=json&json=[{\"user\": \"none\", \"pwd\":\"none\",\"device_id\":\"none\",\"app_vName\":\"none\","
                    //            + "\"CreCompany\":\"" + CreCompany + "\",\"CustomerID\":\"" + CustomerID
                    //            + "\",\"CATEGORY\":\"6080\",\"ACCOUNTTITLE1\":\"" + ACCOUNTTITLE1
                    //            + "\",\"SHORTTITLE\":\"" + SHORTTITLE + "\",\"LoanCurrency\":\"" + LoanCurrency + "\"}]";
                    //            c.T24_AddLog(fileHeader, "CreateCompulsoryAccount_RQ", url, "ApproveLoan");
                    //            var client = new RestClient(url);
                    //            var Authenticator = new HttpBasicAuthenticator(c.GetTabletWSUser(), c.GetTabletWSPwd());
                    //            client.Authenticator = Authenticator;
                    //            var request = new RestRequest(Method.POST);
                    //            IRestResponse response = client.Execute(request);
                    //            string jres = response.Content.ToString().TrimEnd(']').TrimStart('[');
                    //            c.T24_AddLog(fileHeader, "CreateCompulsoryAccount_RS", jres, "ApproveLoan");
                    //            AccountCreationToCBSModel jObAcc = JsonConvert.DeserializeObject<AccountCreationToCBSModel>(jres);
                    //            foreach (var ai in jObAcc.DataList)
                    //            {
                    //                string CompulsoryAccountID = ai.AccountID;
                    //                c.ReturnDT("update tblLoanApp1 set CompulsoryAccountID='" + CompulsoryAccountID + "' where LoanAppID='" + LoanAppID + "'");
                    //            }

                    //        }
                    //        catch (Exception ex)
                    //        {
                    //            ERR = "Error";
                    //            SMS = "Error: Create Compulsory Account";
                    //            c.T24_AddLog(fileHeader, "CreateCompulsoryAccount_RS", ex.Message.ToString(), "ApproveLoan");
                    //        }
                    //    }
                    //    #endregion Create Compulsory Account
                    //}
                    #endregion Compulsory Account
                    #region log
                    try
                    {
                        string isError = "0";
                        if (ERR == "Error")
                        {
                            isError = "1";
                        }
                        SqlConnection Con1 = new SqlConnection(c.ConStr());
                        Con1.Open();
                        SqlCommand Com1 = new SqlCommand();
                        Com1.Connection = Con1;
                        Com1.Parameters.Clear();
                        string sql = "T24_LoanAppApproveLogAdd";
                        Com1.CommandText = sql;
                        Com1.CommandType = CommandType.StoredProcedure;
                        Com1.Parameters.AddWithValue("@LoanAppID", LoanAppID);
                        Com1.Parameters.AddWithValue("@FName", fileHeader);
                        Com1.Parameters.AddWithValue("@isError", isError);
                        Com1.Parameters.AddWithValue("@ErrXml", ErrXml);
                        Com1.Parameters.AddWithValue("@Remark", SMS);
                        Com1.Parameters.AddWithValue("@CreateBy", UserID);
                        Com1.ExecuteNonQuery();
                        Con1.Close();
                    }
                    catch { }
                    #endregion log
                }
                #endregion Approve to CBS


            }
            catch { }

            List<LoanAppApproveResModel> RSData = new List<LoanAppApproveResModel>();
            LoanAppApproveResModel DataList = new LoanAppApproveResModel();
            DataList.ERR = ERR;
            DataList.SMS = SMS;
            RSData.Add(DataList);

            return RSData;
        }

    }
    public class LoanAppApproveResModel
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
    }
}