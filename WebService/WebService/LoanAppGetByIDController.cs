using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml;

namespace WebService
{
    [BasicAuthentication]
    public class LoanAppGetByIDController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<LoanAppGetByIDModel> Get(string api_name, string api_key, string json)//json=[{"user":"01804","pwd":"040882","device_id":"352405061542333","app_vName":"1.6","criteriaValue":""}]
        {
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "";
            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            List<LoanAppGetByIDModel> RSData = new List<LoanAppGetByIDModel>();
            LoanAppGetByIDModel ListHeader = new LoanAppGetByIDModel();
            List<ErrorListGetByID> SMSList = new List<ErrorListGetByID>();

            List<LoanAppGetByID> LoanAppGetByID = new List<LoanAppGetByID>();
            List<PurposeDetailGetByID> PurposeDetailGetByID = new List<PurposeDetailGetByID>();
            List<OpinionGetByID> OpinionGetByID = new List<OpinionGetByID>();

            List<PersonGetByID> PersonGetByID = new List<PersonGetByID>();
            List<AccountListGetByID> AccountListGetByID = new List<AccountListGetByID>();
            List<CreditorGetByID> CreditorGetByID = new List<CreditorGetByID>();
            List<ClientAssetGetByID> ClientAssetGetByID = new List<ClientAssetGetByID>();
            List<ClientBusinessGetByID> ClientBusinessGetByID = new List<ClientBusinessGetByID>();
            List<ClientCollateralGetByID> ClientCollateralGetByID = new List<ClientCollateralGetByID>();
            List<GuarantorBusinessGetByID> GuarantorBusinessGetByID = new List<GuarantorBusinessGetByID>();
            List<GuarantorAssetGetByID> GuarantorAssetGetByID = new List<GuarantorAssetGetByID>();
            List<PersonImgGetByID> PersonImgGetByID = new List<PersonImgGetByID>();
            List<CBCReportGetByID> CBCReportGetByID = new List<CBCReportGetByID>();

            List<CashFlowGetByID> CashFlowGetByID = new List<CashFlowGetByID>();
            List<MSIGetByID> MSIGetByID = new List<MSIGetByID>();
            List<MSIRegularGetByID> MSIRegularGetByID = new List<MSIRegularGetByID>(); 
            List<MSIIrregularGetByID> MSIIrregularGetByID = new List<MSIIrregularGetByID>();

            try {                
                //Add log
                c.T24_AddLog("LoanAppGetByID_" + ServerDate.Replace("-", "_").Replace(" ", "_").Replace(":", "_"), "LoanAppGetByID", json, "LoanAppGetByID");

                #region api
                string[] CheckApi = c.CheckApi(api_name, api_key);
                ERR = CheckApi[0];
                SMS = CheckApi[1];
                if (ERR == "Error") {
                    ErrorListGetByID data = new ErrorListGetByID();
                    data.SMS = SMS;
                    SMSList.Add(data);
                }
                #endregion api

                #region json
                string UserID = "", criteriaValue = "", GroupID="";
                if (ERR != "Error")
                {
                    string[] CheckJson = c.CheckJson(json);
                    ERR = CheckJson[0];
                    if (ERR == "Error")
                    {
                        SMS = CheckJson[1];
                        ErrorListGetByID data = new ErrorListGetByID();
                        data.SMS = SMS;
                        SMSList.Add(data);
                    }
                    else
                    {
                        UserID = CheckJson[1];
                        criteriaValue = CheckJson[2];
                        GroupID = CheckJson[3];
                    }
                }
                #endregion json

                #region tblLoanApp: j0
                if (ERR != "Error") { 
                    try {
                        string sql = "exec sp_LoanAppRestudyGetByCO2_j0 @LoanAppID='" + criteriaValue + "'";
                        DataTable dtj0 = new DataTable();
                        dtj0 = c.ReturnDT(sql);
                        #region no record
                        if (dtj0.Rows.Count == 0)
                        {
                            ERR = "Error";
                            SMS = "Error: not found";
                            ErrorListGetByID data = new ErrorListGetByID();
                            data.SMS = SMS;
                            SMSList.Add(data);
                        }
                        #endregion no record
                        else
                        {
                            for (int i0=0; i0 < dtj0.Rows.Count; i0++) {
                                #region j1: tblLoanApp11PurpsoeDetail
                                try
                                {
                                    sql = "exec sp_LoanAppRestudyGetByCO2_j1 @LoanAppID='" + criteriaValue + "'";
                                    DataTable dtj1 = new DataTable();
                                    dtj1 = c.ReturnDT(sql);
                                    for (int i = 0; i < dtj1.Rows.Count; i++)
                                    {
                                        PurposeDetailGetByID j1 = new PurposeDetailGetByID();
                                        j1.LoanAppPurposeDetail = dtj1.Rows[i]["LoanAppPurpsoeDetail"].ToString();
                                        j1.Quantity = dtj1.Rows[i]["Quantity"].ToString();
                                        j1.UnitPrice = dtj1.Rows[i]["UnitPrice"].ToString();
                                        PurposeDetailGetByID.Add(j1);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    ERR = "Error";
                                    SMS = "Error: during get loan purpose detail";
                                    ErrorListGetByID data = new ErrorListGetByID();
                                    data.SMS = SMS;
                                    SMSList.Add(data);
                                }
                                #endregion
                                #region j6: tblLoanApp20Opinion
                                try
                                {
                                    sql = "select * from tblLoanApp20Opinion where LoanAppID='" + criteriaValue + "'";
                                    DataTable dtj6 = new DataTable();
                                    dtj6 = c.ReturnDT(sql);
                                    for (int i6 = 0; i6 < dtj6.Rows.Count; i6++) {
                                        OpinionGetByID j6 = new OpinionGetByID();
                                        j6.Description = dtj6.Rows[i6]["Opinion"].ToString();
                                        j6.CreateBy = dtj6.Rows[i6]["CreateBy"].ToString();
                                        OpinionGetByID.Add(j6);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    ERR = "Error";
                                    SMS = "Error: during get opinion";
                                }
                                #endregion
                                #region j11: tblLoanAppPerson2
                                try
                                {
                                    sql = "exec sp_LoanAppRestudyGetByCO2_j11_T24 @LoanAppID='" + criteriaValue + "'";
                                    DataTable dtj11 = new DataTable();
                                    dtj11 = c.ReturnDT(sql);
                                    //Cust Info.
                                    for (int i11 = 0; i11 < dtj11.Rows.Count; i11++)
                                    {
                                        #region param
                                        string LoanAppPersonID = dtj11.Rows[i11]["LoanAppPersonID"].ToString();
                                        string LoanAppPersonTypeID = dtj11.Rows[i11]["LoanAppPersonTypeID"].ToString();
                                        string PersonID = dtj11.Rows[i11]["PersonID"].ToString();
                                        string CustomerID = dtj11.Rows[i11]["CustomerID"].ToString();
                                        string Number = dtj11.Rows[i11]["Number"].ToString();
                                        string VillageBankID = dtj11.Rows[i11]["VillageBankID"].ToString();
                                        string AltName = dtj11.Rows[i11]["AltName"].ToString();
                                        string TitleID = dtj11.Rows[i11]["TitleID"].ToString();
                                        string LastName = dtj11.Rows[i11]["LastName"].ToString();
                                        string FirstName = dtj11.Rows[i11]["FirstName"].ToString();
                                        string GenderID = dtj11.Rows[i11]["GenderID"].ToString();
                                        string DateOfBirth = dtj11.Rows[i11]["DateOfBirth"].ToString();
                                        string IDCardTypeID = dtj11.Rows[i11]["IDCardTypeID"].ToString();
                                        string IDCardNumber = dtj11.Rows[i11]["IDCardNumber"].ToString();
                                        string IDCardExpireDate = dtj11.Rows[i11]["IDCardExpireDate"].ToString();
                                        string IDCardIssuedDate = dtj11.Rows[i11]["IDCardIssuedDate"].ToString();
                                        string MaritalStatusID = dtj11.Rows[i11]["MaritalStatusID"].ToString();
                                        string EducationID = dtj11.Rows[i11]["EducationID"].ToString();
                                        string CityOfBirthID = dtj11.Rows[i11]["CityOfBirthID"].ToString();
                                        string Telephone3 = dtj11.Rows[i11]["Telephone3"].ToString();
                                        string VillageIDPermanent = dtj11.Rows[i11]["VillageIDPermanent"].ToString();
                                        string LocationCodeIDPermanent = dtj11.Rows[i11]["LocationCodeIDPermanent"].ToString();
                                        string VillageIDCurrent = dtj11.Rows[i11]["VillageIDCurrent"].ToString();
                                        string LocationCodeIDCurrent = dtj11.Rows[i11]["LocationCodeIDCurrent"].ToString();
                                        string SortAddress = dtj11.Rows[i11]["SortAddress"].ToString();
                                        string FamilyMember = dtj11.Rows[i11]["FamilyMember"].ToString();
                                        string FamilyMemberActive = dtj11.Rows[i11]["FamilyMemberActive"].ToString();
                                        string PoorID = dtj11.Rows[i11]["PoorID"].ToString();
                                        #endregion param

                                        #region j15: Account List
                                        //if (LoanAppPersonTypeID == "31")
                                        //{                                            
                                        //    if (GroupID == "4")
                                        //    {
                                        //        try
                                        //        {
                                        //            #region data list of acc
                                        //            DataTable dtAcc = new DataTable();
                                        //            dtAcc.Columns.Add(new DataColumn("LoanAppID", typeof(string)));
                                        //            dtAcc.Columns.Add(new DataColumn("CustomerID", typeof(string)));
                                        //            dtAcc.Columns.Add(new DataColumn("AccountID", typeof(string)));
                                        //            dtAcc.Columns.Add(new DataColumn("CATEGORY", typeof(string)));
                                        //            dtAcc.Columns.Add(new DataColumn("CATEGDESC", typeof(string)));
                                        //            dtAcc.Columns.Add(new DataColumn("Balance", typeof(string)));
                                        //            dtAcc.Columns.Add(new DataColumn("Restrictions", typeof(string)));
                                        //            dtAcc.Columns.Add(new DataColumn("CURRENCY", typeof(string)));
                                        //            #endregion data list of acc
                                        //            #region get T24 Url
                                        //            sql = "exec T24_GetT24_Url @UserID='" + criteriaValue + "',@UrlID=4";
                                        //            DataTable dt = new DataTable();
                                        //            dt = c.ReturnDT(sql);
                                        //            string CreUrl = dt.Rows[0]["CreUrl"].ToString();
                                        //            string CreCompany = dt.Rows[0]["CreCompany"].ToString();
                                        //            string CreUserName = dt.Rows[0]["CreUserName"].ToString();
                                        //            string CrePassword = dt.Rows[0]["CrePassword"].ToString();
                                        //            #endregion get T24 Url
                                        //            string fileHeader = UserID + "_" + criteriaValue + "_" + ServerDate;
                                        //            int ZeroGetOneAddAcc = 0;
                                        //            #region call to T24 get Account
                                        //            if (ZeroGetOneAddAcc == 0)
                                        //            {
                                        //                #region xml
                                        //                string xmlStr = "<?xml version=\"1.0\"?><soapenv:Envelope xmlns:amk=\"http://temenos.com/AMKTABACLIST\" xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\"><soapenv:Header/>"
                                        //                + "<soapenv:Body><amk:TABLETCUSTOMERACCOUNTLIST><WebRequestCommon><company>" + CreCompany + "</company><password>" + CrePassword
                                        //                + "</password><userName>" + CreUserName + "</userName></WebRequestCommon><AMKETABCUSTACCTLISTType><enquiryInputCollection>"
                                        //                + "<columnName>CUSTOMER</columnName><criteriaValue>" + CustomerID + "</criteriaValue><operand>EQ</operand></enquiryInputCollection>"
                                        //                + "<enquiryInputCollection><columnName>CURRENCY</columnName><criteriaValue>" + dtj0.Rows[i0]["Currency"].ToString() + "</criteriaValue><operand>EQ</operand></enquiryInputCollection>"
                                        //                + "</AMKETABCUSTACCTLISTType></amk:TABLETCUSTOMERACCOUNTLIST></soapenv:Body></soapenv:Envelope>";
                                        //                #endregion xml
                                        //                //add log
                                        //                c.T24_AddLog(fileHeader, "AccountGet", xmlStr);
                                        //                #region call to T24
                                        //                var client = new RestClient(CreUrl);
                                        //                var request = new RestRequest(Method.POST);
                                        //                request.AddHeader("cache-control", "no-cache");
                                        //                request.AddHeader("content-type", "text/xml");
                                        //                request.AddParameter("text/xml", xmlStr, ParameterType.RequestBody);
                                        //                IRestResponse response = client.Execute(request);
                                        //                string xmlContent = response.Content.ToString();
                                        //                #endregion call to T24
                                        //                //add log
                                        //                c.T24_AddLog(fileHeader, "AccountGetResult", xmlContent);

                                        //                XmlDocument doc = new XmlDocument();
                                        //                doc.LoadXml(xmlContent);
                                        //                string successIndicator = doc.GetElementsByTagName("successIndicator").Item(0).InnerText;
                                        //                if (successIndicator == "Success")
                                        //                {
                                        //                    #region xml succeed
                                        //                    //make account list for return
                                        //                    XmlNode node0 = doc.GetElementsByTagName("ns2:gAMKETABCUSTACCTLISTDetailType").Item(0);
                                        //                    int inode0 = node0.ChildNodes.Count;
                                        //                    for (int n = 0; n < inode0; n++)
                                        //                    {
                                        //                        XmlNode node1 = doc.GetElementsByTagName("ns2:mAMKETABCUSTACCTLISTDetailType").Item(n);
                                        //                        try
                                        //                        {
                                        //                            string ACCTID = "", ProductCode = "", CATEGDESC = "";
                                        //                            foreach (XmlNode item in node1.ChildNodes)
                                        //                            {
                                        //                                string itemVal = item.InnerText;
                                        //                                if (item.LocalName == "ACCTID")
                                        //                                {
                                        //                                    ACCTID = itemVal;
                                        //                                }
                                        //                                if (item.LocalName == "ProductCode")
                                        //                                {
                                        //                                    ProductCode = itemVal;
                                        //                                }
                                        //                                if (item.LocalName == "CATEGDESC")
                                        //                                {
                                        //                                    CATEGDESC = itemVal;
                                        //                                }
                                        //                            }
                                        //                            DataRow dr = null;
                                        //                            dr = dtAcc.NewRow();
                                        //                            dr["LoanAppID"] = criteriaValue;
                                        //                            dr["CustomerID"] = CustomerID;
                                        //                            dr["AccountID"] = ACCTID;
                                        //                            dr["CATEGORY"] = ProductCode;
                                        //                            dr["CATEGDESC"] = CATEGDESC;
                                        //                            dr["Balance"] = "";
                                        //                            dr["Restrictions"] = "";
                                        //                            dr["CURRENCY"] = dtj0.Rows[i0]["Currency"].ToString();
                                        //                            dtAcc.Rows.Add(dr);
                                        //                        }
                                        //                        catch
                                        //                        {
                                        //                            ERR = "Error";
                                        //                            SMS = "Error j15: get account from T24: " + fileHeader;
                                        //                        }
                                        //                    }
                                        //                    #endregion xml succeed
                                        //                }
                                        //                else
                                        //                {
                                        //                    #region xml error
                                        //                    //get account to T24
                                        //                    string T24_messages = doc.GetElementsByTagName("messages").Item(1).InnerText;
                                        //                    if (T24_messages == "No records were found that matched the selection criteria")
                                        //                    {
                                        //                        ZeroGetOneAddAcc = 1;
                                        //                    }
                                        //                    else
                                        //                    {
                                        //                        //get account from T24 error
                                        //                        ERR = "Error";
                                        //                        SMS = "Error j15: get account from T24: " + fileHeader;
                                        //                    }
                                        //                    #endregion xml error
                                        //                }
                                        //            }
                                        //            #endregion call to T24 get Account
                                        //            #region call to T24 add acc
                                        //            if (ZeroGetOneAddAcc == 1)
                                        //            {
                                        //                #region get T24 Url
                                        //                sql = "exec T24_GetT24_Url @UserID='" + UserID + "',@UrlID=21";
                                        //                DataTable dt2 = new DataTable();
                                        //                dt2 = c.ReturnDT(sql);
                                        //                CreUrl = dt2.Rows[0]["CreUrl"].ToString();
                                        //                CreCompany = dt2.Rows[0]["CreCompany"].ToString();
                                        //                CreUserName = dt2.Rows[0]["CreUserName"].ToString();
                                        //                CrePassword = dt2.Rows[0]["CrePassword"].ToString();
                                        //                #endregion get T24 Url
                                        //                #region xml
                                        //                string xmlStr = "<?xml version=\"1.0\"?><soapenv:Envelope xmlns:acc=\"http://temenos.com/ACCOUNTAMKSAOPENWS\" "
                                        //                + "xmlns:amk=\"http://temenos.com/AMKTABACOPEN\" xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\"><soapenv:Header/>"
                                        //                + "<soapenv:Body><amk:ACCOUNTCREATION><!--Optional:--><WebRequestCommon><!--Optional:--><company>" + CreCompany
                                        //                + "</company><password>" + CrePassword + "</password><userName>" + CreUserName + "</userName></WebRequestCommon><!--Optional:-->"
                                        //                + "<OfsFunction> </OfsFunction><!--Optional:--><ACCOUNTAMKSAOPENWSType id=\"\"><!--Optional:-->"
                                        //                + "<acc:CUSTOMER>" + CustomerID + "</acc:CUSTOMER><!--Optional:--><acc:CATEGORY>6001</acc:CATEGORY><!--Optional:-->"
                                        //                + "<acc:ACCOUNTTITLE1>" + LastName + ", " + FirstName + "</acc:ACCOUNTTITLE1><!--Optional:--><acc:SHORTTITLE>" + LastName + ", " + FirstName
                                        //                + "</acc:SHORTTITLE><!--Optional:--><acc:MNEMONIC></acc:MNEMONIC><!--Optional:--><acc:CURRENCY>" + dtj0.Rows[i0]["Currency"].ToString()
                                        //                + "</acc:CURRENCY><!--Optional:--><acc:ACCOUNTOFFICER>1</acc:ACCOUNTOFFICER><!--Optional:--><acc:SMSAlerts>NO</acc:SMSAlerts>"
                                        //                + "<!--Optional:--><acc:EmailAlerts>NO</acc:EmailAlerts><!--Optional:--><acc:ConditionofOperation>1</acc:ConditionofOperation>"
                                        //                + "</ACCOUNTAMKSAOPENWSType></amk:ACCOUNTCREATION></soapenv:Body></soapenv:Envelope>";
                                        //                #endregion xml
                                        //                //add log
                                        //                c.T24_AddLog(fileHeader, "AccountAdd", xmlStr);
                                        //                #region call to T24
                                        //                var client = new RestClient(CreUrl);
                                        //                var request = new RestRequest(Method.POST);
                                        //                request.AddHeader("cache-control", "no-cache");
                                        //                request.AddHeader("content-type", "text/xml");
                                        //                request.AddParameter("text/xml", xmlStr, ParameterType.RequestBody);
                                        //                IRestResponse response = client.Execute(request);
                                        //                string xmlContent = response.Content.ToString();
                                        //                #endregion call to T24
                                        //                //add log
                                        //                c.T24_AddLog(fileHeader, "AccountAddResult", xmlContent);

                                        //                XmlDocument doc = new XmlDocument();
                                        //                doc.LoadXml(xmlContent);
                                        //                string successIndicator = doc.GetElementsByTagName("successIndicator").Item(0).InnerText;
                                        //                if (successIndicator == "Success")
                                        //                {
                                        //                    #region xml succeed
                                        //                    //make account list for return
                                        //                    XmlNode node0 = doc.GetElementsByTagName("ns2:gAMKETABCUSTACCTLISTDetailType").Item(0);
                                        //                    int inode0 = node0.ChildNodes.Count;
                                        //                    for (int n = 0; n < inode0; n++)
                                        //                    {
                                        //                        XmlNode node1 = doc.GetElementsByTagName("ns2:mAMKETABCUSTACCTLISTDetailType").Item(n);
                                        //                        try
                                        //                        {
                                        //                            string ACCTID = "", ProductCode = "", CATEGDESC = "";
                                        //                            foreach (XmlNode item in node1.ChildNodes)
                                        //                            {
                                        //                                string itemVal = item.InnerText;
                                        //                                if (item.LocalName == "ACCTID")
                                        //                                {
                                        //                                    ACCTID = itemVal;
                                        //                                }
                                        //                                if (item.LocalName == "ProductCode")
                                        //                                {
                                        //                                    ProductCode = itemVal;
                                        //                                }
                                        //                                if (item.LocalName == "CATEGDESC")
                                        //                                {
                                        //                                    CATEGDESC = itemVal;
                                        //                                }
                                        //                            }
                                        //                            DataRow dr = null;
                                        //                            dr = dtAcc.NewRow();
                                        //                            dr["LoanAppID"] = criteriaValue;
                                        //                            dr["CustomerID"] = CustomerID;
                                        //                            dr["AccountID"] = ACCTID;
                                        //                            dr["CATEGORY"] = ProductCode;
                                        //                            dr["CATEGDESC"] = CATEGDESC;
                                        //                            dr["Balance"] = "";
                                        //                            dr["Restrictions"] = "";
                                        //                            dr["CURRENCY"] = dtj0.Rows[i0]["Currency"].ToString();
                                        //                            dtAcc.Rows.Add(dr);
                                        //                        }
                                        //                        catch
                                        //                        {
                                        //                            ERR = "Error";
                                        //                            SMS = "Error j15: get account from T24: " + fileHeader;
                                        //                        }
                                        //                    }
                                        //                    #endregion xml succeed
                                        //                }
                                        //                else
                                        //                {
                                        //                    #region xml Error
                                        //                    //add account to T24 error
                                        //                    ERR = "Error";
                                        //                    SMS = "Error j15: add account to T24: " + fileHeader;
                                        //                    #endregion xml Error
                                        //                }
                                        //            }
                                        //            #endregion call to T24 add acc
                                        //            #region j15 in json
                                        //            if (ERR != "Error")
                                        //            {
                                        //                for (int i15 = 0; i15 < dtAcc.Rows.Count; i15++) {
                                        //                    AccountListGetByID j15 = new AccountListGetByID();
                                        //                    j15.CustomerID = dtAcc.Rows[i15]["CustomerID"].ToString();
                                        //                    j15.AccountID = dtAcc.Rows[i15]["AccountID"].ToString();
                                        //                    j15.CATEGORY = dtAcc.Rows[i15]["CATEGORY"].ToString();
                                        //                    j15.CATEGDESC = dtAcc.Rows[i15]["CATEGDESC"].ToString();
                                        //                    j15.Balance = dtAcc.Rows[i15]["Balance"].ToString();
                                        //                    j15.Restrictions = dtAcc.Rows[i15]["Restrictions"].ToString();
                                        //                    j15.CURRENCY = dtAcc.Rows[i15]["CURRENCY"].ToString();
                                        //                    AccountListGetByID.Add(j15);
                                        //                }
                                        //            }
                                        //            #endregion j15 in json
                                        //        }
                                        //        catch (Exception ex)
                                        //        {
                                        //            ERR = "Error";
                                        //            SMS = "Error j15: " + ex.Message.ToString();
                                        //        }
                                        //    }                                            
                                        //}
                                        #endregion j15: Account List
                                        #region j2: tblLoanApp12Creditor
                                        try
                                        {
                                            sql = "exec sp_LoanAppRestudyGetByCO2_j2_T24 @LoanAppPersonID='" + LoanAppPersonID + "'";
                                            DataTable dtj2 = new DataTable();
                                            dtj2 = c.ReturnDT(sql);
                                            for (int i2 = 0; i2 < dtj2.Rows.Count; i2++) {
                                                CreditorGetByID j2 = new CreditorGetByID();
                                                j2.CreditorID = dtj2.Rows[i2]["CreditorID"].ToString();
                                                j2.ApprovedAmount = dtj2.Rows[i2]["ApprovedAmount"].ToString();
                                                j2.OutstandingBalance = dtj2.Rows[i2]["OutstandingBalance"].ToString();
                                                j2.InterestRate = dtj2.Rows[i2]["InterestRate"].ToString();
                                                j2.RepaymentTypeID = dtj2.Rows[i2]["RepaymentTypeID"].ToString();
                                                j2.RepaymentTermID = dtj2.Rows[i2]["RepaymentTermID"].ToString();
                                                j2.LoanStartDate = dtj2.Rows[i2]["LoanStartDate"].ToString();
                                                j2.LoanEndDate = dtj2.Rows[i2]["LoanEndDate"].ToString();
                                                j2.RemainingInstallment = dtj2.Rows[i2]["RemainingInstallment"].ToString();
                                                CreditorGetByID.Add(j2);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            ERR = "Error";
                                            SMS = "Error j2: during get creditor | " + ex.Message.ToString();
                                        }
                                        #endregion
                                        #region j3: tblLoanApp13ClientAsset
                                        try
                                        {
                                            sql = "exec sp_LoanAppRestudyGetByCO2_j3_T24 @LoanAppPersonID='" + LoanAppPersonID + "'";
                                            DataTable dtj3 = new DataTable();
                                            dtj3 = c.ReturnDT(sql);
                                            for (int i3 = 0; i3 < dtj3.Rows.Count; i3++) {
                                                ClientAssetGetByID j3 = new ClientAssetGetByID();
                                                j3.Description = dtj3.Rows[i3]["Description"].ToString();
                                                j3.Quantity = dtj3.Rows[i3]["Quantity"].ToString();
                                                j3.UnitPrice = dtj3.Rows[i3]["UnitPrice"].ToString();
                                                ClientAssetGetByID.Add(j3);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            ERR = "Error";
                                            SMS = "Error j3: during get client asset" + ex.Message.ToString();
                                        }
                                        #endregion
                                        #region j4: tblLoanApp14ClientBusiness
                                        try
                                        {
                                            sql = "exec sp_LoanAppRestudyGetByCO2_j4_T24 @LoanAppPersonID='" + LoanAppPersonID + "'";
                                            DataTable dtj4 = new DataTable();
                                            dtj4 = c.ReturnDT(sql);
                                            for (int i4 = 0; i4 < dtj4.Rows.Count; i4++) {
                                                ClientBusinessGetByID j4 = new ClientBusinessGetByID();
                                                j4.Description = dtj4.Rows[i4]["Description"].ToString();
                                                j4.Quantity = dtj4.Rows[i4]["Quantity"].ToString();
                                                j4.UnitPrice = dtj4.Rows[i4]["UnitPrice"].ToString();
                                                ClientBusinessGetByID.Add(j4);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            ERR = "Error";
                                            SMS = "Error j4: during get client business " + ex.Message.ToString();
                                        }
                                        #endregion
                                        #region j12: tblLoanApp15ClientCollateral
                                        try
                                        {
                                            sql = "exec sp_LoanAppRestudyGetByCO2_j12_T24 @LoanAppPersonID='" + LoanAppPersonID + "'";
                                            DataTable dtj12 = new DataTable();
                                            dtj12 = c.ReturnDT(sql);
                                            for (int i12 = 0; i12 < dtj12.Rows.Count; i12++) {
                                                ClientCollateralGetByID j12 = new ClientCollateralGetByID();
                                                string LoanAppClientCollateralID = dtj12.Rows[i12]["LoanAppClientCollateralID"].ToString();
                                                j12.ColleteralTypeID = dtj12.Rows[i12]["ColleteralTypeID"].ToString();
                                                j12.ColleteralDocTypeID = dtj12.Rows[i12]["ColleteralDocTypeID"].ToString();
                                                j12.ColleteralDocNumber = dtj12.Rows[i12]["ColleteralDocNumber"].ToString();
                                                j12.Description = dtj12.Rows[i12]["Description"].ToString();
                                                j12.Quantity = dtj12.Rows[i12]["Quantity"].ToString();
                                                j12.UnitPrice = dtj12.Rows[i12]["UnitPrice"].ToString();
                                                //img
                                                List<ClientCollateralImgGetByID> ClientCollateralImgGetByID = new List<ClientCollateralImgGetByID>();
                                                sql = "select Ext,ImgPath from tblLoanApp16ClientCollateralImage where LoanAppClientCollateralID='"+ LoanAppClientCollateralID + "'";
                                                DataTable dtj12_1 = new DataTable();
                                                dtj12_1 = c.ReturnDT(sql);
                                                for (int i12_1 = 0; i12_1 < dtj12_1.Rows.Count; i12_1++) {
                                                    ClientCollateralImgGetByID j12_1 = new ClientCollateralImgGetByID();
                                                    j12_1.Ext = dtj12_1.Rows[i12_1]["Ext"].ToString();
                                                    j12_1.ImgName = dtj12_1.Rows[i12_1]["ImgPath"].ToString();
                                                    ClientCollateralImgGetByID.Add(j12_1);
                                                }
                                                j12.ClientCollateralImg = ClientCollateralImgGetByID;
                                                ClientCollateralGetByID.Add(j12);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            ERR = "Error";
                                            SMS = "Error j12: during get client collateral " + ex.Message.ToString();
                                        }
                                        #endregion
                                        #region j13: tblLoanApp17GuarantorBusiness
                                        try
                                        {
                                            sql = "exec sp_LoanAppRestudyGetByCO2_j13_T24 @LoanAppPersonID='" + LoanAppPersonID + "'";
                                            DataTable dtj13 = new DataTable();
                                            dtj13 = c.ReturnDT(sql);
                                            for (int i13 = 0; i13 < dtj13.Rows.Count; i13++) {
                                                GuarantorBusinessGetByID j13 = new GuarantorBusinessGetByID();
                                                j13.Description = dtj13.Rows[i13]["Description"].ToString();
                                                j13.NetProfitPerYear = dtj13.Rows[i13]["NetProfitPerYear"].ToString();
                                                GuarantorBusinessGetByID.Add(j13);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            ERR = "Error";
                                            SMS = "Error j13: during get guarantor business " + ex.Message.ToString();
                                        }
                                        #endregion
                                        #region j5: tblLoanApp19GuarantorAsset
                                        try
                                        {
                                            sql = "exec sp_LoanAppRestudyGetByCO2_j5_T24 @LoanAppPersonID='" + LoanAppPersonID + "'";
                                            DataTable dtj5 = new DataTable();
                                            dtj5 = c.ReturnDT(sql);
                                            for (int i5 = 0; i5 < dtj5.Rows.Count; i5++)
                                            {
                                                GuarantorAssetGetByID j5 = new GuarantorAssetGetByID();
                                                j5.Description = dtj5.Rows[i5]["Description"].ToString();
                                                j5.Quantity = dtj5.Rows[i5]["Quantity"].ToString();
                                                j5.UnitPrice = dtj5.Rows[i5]["UnitPrice"].ToString();
                                                GuarantorAssetGetByID.Add(j5);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            ERR = "Error";
                                            SMS = "Error j5: during get GuarantorAsset " + ex.Message.ToString();
                                        }
                                        #endregion
                                        #region PersonImg
                                        sql = "select Ext,ImgPath from tblLoanAppPerson21Image where LoanAppPersonID='" + LoanAppPersonID + "'";
                                        DataTable dtj11_1 = new DataTable();
                                        dtj11_1 = c.ReturnDT(sql);
                                        for (int i11_1 = 0; i11_1 < dtj11_1.Rows.Count; i11_1++)
                                        {
                                            PersonImgGetByID j11_1 = new PersonImgGetByID();
                                            j11_1.Ext = dtj11_1.Rows[i11_1]["Ext"].ToString();
                                            j11_1.ImgName = dtj11_1.Rows[i11_1]["ImgPath"].ToString();
                                            PersonImgGetByID.Add(j11_1);
                                        }
                                        #endregion PersonImg
                                        #region j14: CBCReport
                                        try
                                        {
                                            //sql = "EXEC utnetamk.dbo.sp_AmkTablet_CBC_OutstandingBal @AccountNumber='" + CBSAcc + "',@CustomerID=" + CustomerID + "";
                                            //DataTable dt = new DataTable();
                                            //dt = c.ReturnDTBranch(sql, c.GetCentreDBConStr());
                                            //List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                                            //Dictionary<string, object> row = null;
                                            //foreach (DataRow dr in dt.Rows)
                                            //{
                                            //    row = new Dictionary<string, object>();
                                            //    foreach (DataColumn col in dt.Columns)
                                            //    {
                                            //        row.Add(col.ColumnName.Trim(), dr[col].ToString());
                                            //    }
                                            //    rows.Add(row);
                                            //}
                                            //j14 = serializer.Serialize(rows);
                                        }
                                        catch (Exception ex)
                                        {
                                            ERR = "Error";
                                            SMS = "Error j14: during get CBCReport " + ex.Message.ToString();
                                        }
                                        #endregion

                                        PersonGetByID data = new PersonGetByID();
                                        data.AccountList = AccountListGetByID;
                                        data.Creditor = CreditorGetByID;
                                        data.ClientAsset = ClientAssetGetByID;
                                        data.ClientBusiness = ClientBusinessGetByID;
                                        data.ClientCollateral = ClientCollateralGetByID;
                                        data.GuarantorBusiness = GuarantorBusinessGetByID;
                                        data.GuarantorAsset = GuarantorAssetGetByID;
                                        data.PersonImg = PersonImgGetByID;
                                        data.CBCReport = CBCReportGetByID;
                                        #region person
                                        data.LoanAppPersonTypeID = LoanAppPersonTypeID;
                                        data.PersonID = PersonID;
                                        data.CustomerID = CustomerID;
                                        data.Number = Number;
                                        data.VillageBankID = VillageBankID;
                                        data.AltName = AltName;
                                        data.TitleID = TitleID;
                                        data.LastName = LastName;
                                        data.FirstName = FirstName;
                                        data.GenderID = GenderID;
                                        data.DateOfBirth = DateOfBirth;
                                        data.IDCardTypeID = IDCardTypeID;
                                        data.IDCardNumber = IDCardNumber;
                                        data.IDCardExpireDate = IDCardExpireDate;
                                        data.IDCardIssuedDate = IDCardIssuedDate;
                                        data.MaritalStatusID = MaritalStatusID;
                                        data.EducationID = EducationID;
                                        data.CityOfBirthID = CityOfBirthID;
                                        data.Telephone3 = Telephone3;
                                        data.VillageIDPermanent = VillageIDPermanent;
                                        data.LocationCodeIDPermanent = LocationCodeIDPermanent;
                                        data.VillageIDCurrent = VillageIDCurrent;
                                        data.LocationCodeIDCurrent = LocationCodeIDCurrent;
                                        data.SortAddress = SortAddress;
                                        data.FamilyMember = FamilyMember;
                                        data.FamilyMemberActive = FamilyMemberActive;
                                        data.PoorID = PoorID;
                                        #endregion person
                                        PersonGetByID.Add(data);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    ERR = "Error";
                                    SMS = "Error: during get person";
                                }
                                #endregion
                                #region j7: tblLoanAppCashFlow1
                                try
                                {
                                    sql = "exec sp_LoanAppRestudyGetByCO2_j7 @LoanAppID='" + criteriaValue + "'";
                                    DataTable dtj7 = new DataTable();
                                    dtj7 = c.ReturnDT(sql);
                                    for (int i7 = 0; i7 < dtj7.Rows.Count; i7++) {
                                        #region param
                                        string LoanAppCashFlowID = dtj7.Rows[i7]["LoanAppCashFlowID"].ToString();
                                        string StudyMonthAmount = dtj7.Rows[i7]["StudyMonthAmount"].ToString();
                                        string StudyStartMonth = dtj7.Rows[i7]["StudyStartMonth"].ToString();
                                        string FamilyExpensePerMonth = dtj7.Rows[i7]["FamilyExpensePerMonth"].ToString();
                                        string OtherExpensePerMonth = dtj7.Rows[i7]["OtherExpensePerMonth"].ToString();
                                        #endregion param
                                        #region j8: tblLoanAppCashFlow2MSI
                                        try
                                        {
                                            sql = "exec sp_LoanAppRestudyGetByCO2_j8_T24 @LoanAppCashFlowID='" + LoanAppCashFlowID + "'";
                                            DataTable dtj8 = new DataTable();
                                            dtj8 = c.ReturnDT(sql);
                                            for (int i8 = 0; i8 < dtj8.Rows.Count; i8++) {
                                                MSIGetByID j8 = new MSIGetByID();
                                                string LoanAppCashFlowMSIID = dtj8.Rows[i8]["LoanAppCashFlowMSIID"].ToString();
                                                string IncomeTypeID = dtj8.Rows[i8]["IncomeTypeID"].ToString();
                                                string MainSourceIncomeID = dtj8.Rows[i8]["MainSourceIncomeID"].ToString();
                                                string Remark = dtj8.Rows[i8]["Remark"].ToString();
                                                string Quantity = dtj8.Rows[i8]["Quantity"].ToString();
                                                string ExAge = dtj8.Rows[i8]["ExAge"].ToString();
                                                string BusAge = dtj8.Rows[i8]["BusAge"].ToString();
                                                string isMSI = dtj8.Rows[i8]["isMSI"].ToString();

                                                #region j9: tblLoanAppCashFlow21MSIRegular
                                                try
                                                {
                                                    sql = "exec sp_LoanAppRestudyGetByCO2_j9_T24 @LoanAppCashFlowMSIID='" + LoanAppCashFlowMSIID + "'";
                                                    DataTable dtj9 = new DataTable();
                                                    dtj9 = c.ReturnDT(sql);
                                                    for (int i9 = 0; i9 < dtj9.Rows.Count; i9++) {
                                                        MSIRegularGetByID j9 = new MSIRegularGetByID();
                                                        j9.Description = dtj9.Rows[i9]["Description"].ToString();
                                                        j9.Amount = dtj9.Rows[i9]["Amount"].ToString();
                                                        j9.UnitID = dtj9.Rows[i9]["UnitID"].ToString();
                                                        j9.Cost = dtj9.Rows[i9]["Cost"].ToString();
                                                        j9.OneIncomeTwoExpense = dtj9.Rows[i9]["OneIncomeTwoExpense"].ToString();
                                                        j9.CurrencyID = dtj9.Rows[i9]["CurrencyID"].ToString();
                                                        j9.Month = dtj9.Rows[i9]["Month"].ToString();
                                                        MSIRegularGetByID.Add(j9);
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    ERR = "Error";
                                                    SMS = "Error j9: during get Regular MSI " + ex.Message.ToString();
                                                }
                                                #endregion
                                                #region j10: tblLoanAppCashFlow22MSIIrregular
                                                try
                                                {
                                                    sql = "exec sp_LoanAppRestudyGetByCO2_j10_T24 @LoanAppCashFlowMSIID='" + LoanAppCashFlowMSIID + "'";
                                                    DataTable dtj10 = new DataTable();
                                                    dtj10 = c.ReturnDT(sql);
                                                    for (int i10 = 0; i10 < dtj10.Rows.Count; i10++)
                                                    {
                                                        MSIIrregularGetByID j10 = new MSIIrregularGetByID();
                                                        j10.Description = dtj10.Rows[i10]["Description"].ToString();
                                                        j10.Amount = dtj10.Rows[i10]["Amount"].ToString();
                                                        j10.UnitID = dtj10.Rows[i10]["UnitID"].ToString();
                                                        j10.Cost = dtj10.Rows[i10]["Cost"].ToString();
                                                        j10.OneIncomeTwoExpense = dtj10.Rows[i10]["OneIncomeTwoExpense"].ToString();
                                                        j10.CurrencyID = dtj10.Rows[i10]["CurrencyID"].ToString();
                                                        j10.Month = dtj10.Rows[i10]["Month"].ToString();
                                                        MSIIrregularGetByID.Add(j10);
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    ERR = "Error";
                                                    SMS = "Error j9: during get Regular MSI " + ex.Message.ToString();
                                                }
                                                #endregion
                                                j8.MSIRegular = MSIRegularGetByID;
                                                j8.MSIIrregular = MSIIrregularGetByID;

                                                #region j8
                                                j8.IncomeTypeID = IncomeTypeID;
                                                j8.MainSourceIncomeID = MainSourceIncomeID;
                                                j8.Remark = Remark;
                                                j8.Quantity = Quantity;
                                                j8.ExAge = ExAge;
                                                j8.BusAge = BusAge;
                                                j8.isMSI = isMSI;
                                                #endregion j8
                                                MSIGetByID.Add(j8);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            ERR = "Error";
                                            SMS = "Error j8: during get MSI " + ex.Message.ToString();
                                        }
                                        #endregion

                                        CashFlowGetByID data = new CashFlowGetByID();
                                        data.MSI = MSIGetByID;
                                        #region CashFlow
                                        data.StudyMonthAmount = StudyMonthAmount;
                                        data.StudyStartMonth = StudyStartMonth;
                                        data.FamilyExpensePerMonth = FamilyExpensePerMonth;
                                        data.OtherExpensePerMonth = OtherExpensePerMonth;
                                        #endregion CashFlow
                                        CashFlowGetByID.Add(data);
                                    }

                                }
                                catch (Exception ex)
                                {
                                    ERR = "Error";
                                    SMS = "Error j7: during get CashFlow " + ex.Message.ToString();
                                }
                                #endregion

                                #region tblLoanApp: j0
                                try
                                {
                                    LoanAppGetByID data = new LoanAppGetByID();
                                    #region param
                                    data.IDOnDevice = dtj0.Rows[i0]["IDOnDevice"].ToString();
                                    data.LoanAppID = dtj0.Rows[i0]["LoanAppID"].ToString();
                                    data.LoanAppStatusID = dtj0.Rows[i0]["LoanAppStatusID"].ToString();
                                    data.DeviceDate = dtj0.Rows[i0]["DeviceDate"].ToString();
                                    data.ProductID = dtj0.Rows[i0]["ProductID"].ToString();
                                    data.LoanRequestAmount = dtj0.Rows[i0]["LoanRequestAmount"].ToString();
                                    data.LoanPurposeID1 = dtj0.Rows[i0]["LoanPurposeID1"].ToString();
                                    data.LoanPurposeID2 = dtj0.Rows[i0]["LoadPurposeID2"].ToString();
                                    data.LoanPurposeID3 = dtj0.Rows[i0]["LoadPurposeID3"].ToString();
                                    data.OwnCapital = dtj0.Rows[i0]["OwnCapital"].ToString();
                                    data.DisbursementDate = dtj0.Rows[i0]["DisbursementDate"].ToString();
                                    data.FirstWithdrawal = dtj0.Rows[i0]["FirstWithdrawal"].ToString();
                                    data.LoanTerm = dtj0.Rows[i0]["LoanTerm"].ToString();
                                    data.FirstRepaymentDate = dtj0.Rows[i0]["FirstRepaymentDate"].ToString();
                                    data.LoanInterestRate = dtj0.Rows[i0]["LoanInterestRate"].ToString();
                                    data.CustomerRequestRate = dtj0.Rows[i0]["CustomerRequestRate"].ToString();
                                    data.CompititorRate = dtj0.Rows[i0]["CompititorRate"].ToString();
                                    data.CustomerConditionID = dtj0.Rows[i0]["CustomerConditionID"].ToString();
                                    data.COProposedAmount = dtj0.Rows[i0]["COProposedAmount"].ToString();
                                    data.COProposedTerm = dtj0.Rows[i0]["COProposedTerm"].ToString();
                                    data.COProposeRate = dtj0.Rows[i0]["COProposeRate"].ToString();
                                    data.FrontBackOfficeID = dtj0.Rows[i0]["FrontBackOfficeID"].ToString();
                                    data.GroupNumber = dtj0.Rows[i0]["GroupNumber"].ToString();
                                    data.LoanCycleID = dtj0.Rows[i0]["LoanCycleID"].ToString();
                                    data.RepaymentHistoryID = dtj0.Rows[i0]["RepaymentHistoryID"].ToString();
                                    data.LoanReferralID = dtj0.Rows[i0]["LoanReferralID"].ToString();
                                    data.DebtIinfoID = dtj0.Rows[i0]["DebtIinfoID"].ToString();
                                    data.CBSKey = dtj0.Rows[i0]["CBSKey"].ToString();
                                    data.AMDebtFound = dtj0.Rows[i0]["AMDebtFound"].ToString();
                                    data.AMApproveAmt = dtj0.Rows[i0]["AMApproveAmt"].ToString();
                                    data.AMApproveTerm = dtj0.Rows[i0]["AMApproveTerm"].ToString();
                                    data.AMApproveRate = dtj0.Rows[i0]["AMApproveRate"].ToString();
                                    data.AMOpinion = dtj0.Rows[i0]["AMOpinion"].ToString();
                                    data.MonthlyFee = dtj0.Rows[i0]["MonthlyFee"].ToString();
                                    data.Compulsory = dtj0.Rows[i0]["Compulsory"].ToString();
                                    data.CompulsoryTerm = dtj0.Rows[i0]["CompulsoryTerm"].ToString();
                                    data.CreateBy = dtj0.Rows[i0]["CreateBy"].ToString();
                                    data.DeskCheckID = dtj0.Rows[i0]["DeskCheckID"].ToString();
                                    data.PreCheckID = dtj0.Rows[i0]["PreCheckID"].ToString();
                                    data.Currency = dtj0.Rows[i0]["Currency"].ToString();
                                    data.UpFrontFee = dtj0.Rows[i0]["UpFrontFee"].ToString();
                                    data.UpFrontAmt = dtj0.Rows[i0]["UpFrontAmt"].ToString();
                                    #endregion param
                                    data.PurposeDetail = PurposeDetailGetByID;
                                    data.Opinion = OpinionGetByID;
                                    data.Person = PersonGetByID;
                                    data.CashFlow = CashFlowGetByID;
                                    LoanAppGetByID.Add(data);
                                }
                                catch
                                {
                                    ERR = "Error";
                                    SMS = "Error: during get loan info.";
                                    ErrorListGetByID data = new ErrorListGetByID();
                                    data.SMS = SMS;
                                    SMSList.Add(data);
                                }
                                #endregion tblLoanApp: j0   
                            }
                        }
                        

                    } catch {
                        ERR = "Error";
                        SMS = "Error during get loan info.";
                        ErrorListGetByID data = new ErrorListGetByID();
                        data.SMS = SMS;
                        SMSList.Add(data);
                    }
                }
                #endregion tblLoanApp: j0




            }
            catch {
                ERR = "Error";
                SMS = "Something was wrong";
            }

            ListHeader.ERR = ERR;
            ListHeader.SMS = SMS;
            ListHeader.LoanApp = LoanAppGetByID;
            ListHeader.ErrorListGetByID = SMSList;
            RSData.Add(ListHeader);

            return RSData;
        }
        
    }
}