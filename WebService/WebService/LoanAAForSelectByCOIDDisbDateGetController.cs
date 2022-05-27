using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebService
{
    [BasicAuthentication]
    public class LoanAAForSelectByCOIDDisbDateGetController : ApiController
    {
        public IEnumerable<LoanAAForSelectByCOIDDisbDateGetModel> Get(string api_name, string api_key, string json)////json=[{"user":"none","pwd":"none","device_id":"none","app_vName":"none","criteriaValue":"3485","criteriaValue2":"2018-08-27"}]
        {
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "", criteriaValue = "", criteriaValue2 = "";
            List<LoanAAForSelectByCOIDDisbDateGetModel> RSData = new List<LoanAAForSelectByCOIDDisbDateGetModel>();
            try
            {
                #region api
                string[] CheckApi = c.CheckApi(api_name, api_key);
                ERR = CheckApi[0];
                SMS = CheckApi[1];
                #endregion api

                #region json
                string UserID = "";
                if (ERR != "Error")
                {
                    string[] CheckJson = c.CheckJson(json);
                    ERR = CheckJson[0];
                    if (ERR == "Error")
                    {
                        SMS = CheckJson[1];
                    }
                    else
                    {
                        UserID = CheckJson[1];
                        criteriaValue = CheckJson[2];
                        criteriaValue2 = CheckJson[4];
                    }
                }
                #endregion json

                #region T24_GetLoanAAForSelectByCOIDDisbDate
                if (ERR != "Error")
                {
                    try
                    {
                        DataTable dt = c.ReturnDT("exec T24_GetLoanAAForSelectByCOIDDisbDate @UserID='" + UserID + "',@CreateBy='" + criteriaValue + "',@DisbursementDate='" + criteriaValue2 + "'");
                        if (dt.Rows.Count == 0)
                        {
                            ERR = "Error";
                            SMS = "No Loan";
                        }
                        else
                        {
                            LoanAAForSelectByCOIDDisbDateGetModel ListHeader = new LoanAAForSelectByCOIDDisbDateGetModel();
                            ListHeader.ERR = ERR;
                            ListHeader.SMS = SMS;
                            List<LoanAAForSelectByCOIDDisbDateGetList> DataList = new List<LoanAAForSelectByCOIDDisbDateGetList>();


                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                #region params
                                LoanAAForSelectByCOIDDisbDateGetList data = new LoanAAForSelectByCOIDDisbDateGetList();
                                data.LoanAppID = dt.Rows[i]["LoanAppID"].ToString();
                                data.CustomerName = dt.Rows[i]["CustomerName"].ToString();
                                data.AMApproveAmt = dt.Rows[i]["AMApproveAmt"].ToString();
                                data.AAID = dt.Rows[i]["AAID"].ToString();
                                DataList.Add(data);
                                #endregion params
                            }


                            ListHeader.DataList = DataList;
                            RSData.Add(ListHeader);
                        }
                    }
                    catch
                    {
                        ERR = "Error";
                        SMS = "Error while get loan from switch";
                    }
                }
                #endregion T24_GetLoanAAForSelectByCOIDDisbDate

            }
            catch (Exception ex)
            {
                ERR = "Error";
                SMS = "Something was wrong";
            }
            #region if Error
            if (ERR == "Error")
            {
                LoanAAForSelectByCOIDDisbDateGetModel CustHeader = new LoanAAForSelectByCOIDDisbDateGetModel();
                CustHeader.ERR = ERR;
                CustHeader.SMS = SMS;
                CustHeader.DataList = null;
                RSData.Add(CustHeader);
            }
            #endregion if Error

            return RSData;
        }

    }
    public class LoanAAForSelectByCOIDDisbDateGetModel
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
        public List<LoanAAForSelectByCOIDDisbDateGetList> DataList { get; set; }
    }
    public class LoanAAForSelectByCOIDDisbDateGetList
    {
        public string LoanAppID { get; set; }
        public string CustomerName { get; set; }
        public string AMApproveAmt { get; set; }
        public string AAID { get; set; }
    }
}