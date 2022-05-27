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
    public class LoanAppStatusGetController : ApiController
    {
        public IEnumerable<LoanAppStatusGetModel> Get(string api_name, string api_key, string json)////json=[{"user":"01804","pwd":"040882","device_id":"352405061542333","app_vName":"1.6","criteriaValue":""}]
        {


            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "";
            List<LoanAppStatusGetModel> RSData = new List<LoanAppStatusGetModel>();
            try
            {
                #region api
                string[] CheckApi = c.CheckApi(api_name, api_key);
                ERR = CheckApi[0];
                SMS = CheckApi[1];
                #endregion api

                #region json
                string UserID = "", criteriaValue = "";
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
                    }
                }
                #endregion json

                #region data
                if (ERR != "Error")
                {
                    LoanAppStatusGetModel ListHeader = new LoanAppStatusGetModel();
                    

                    List<LoanAppStatusGetSMSList> DataList = new List<LoanAppStatusGetSMSList>();

                    DataTable dt = c.ReturnDT("exec T24_LoanAppStatusGet @UserID='" + UserID + "',@LoanAppID='" + criteriaValue + "'");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        #region params
                        ERR = dt.Rows[i]["ERR"].ToString();
                        SMS = dt.Rows[i]["SMS"].ToString();

                        //LoanAppStatusGetSMSList data = new LoanAppStatusGetSMSList();
                        //data.Code = ERR;
                        //data.SMS = SMS;
                        //DataList.Add(data);

                        if (ERR != "Error")
                        {
                            ERR = "Succeed";
                            ListHeader.ERR = ERR;
                            ListHeader.SMS = SMS;
                            //ListHeader.SMSList = DataList;
                            RSData.Add(ListHeader);
                        }
                        #endregion params
                    }

                    
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
                LoanAppStatusGetModel ListHeader = new LoanAppStatusGetModel();
                ListHeader.ERR = ERR;
                ListHeader.SMS = SMS;
                ListHeader.SMSList = null;
                RSData.Add(ListHeader);
            }
            #endregion if Error

            return RSData;
        }

    }

    public class LoanAppStatusGetModel
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
        public List<LoanAppStatusGetSMSList> SMSList { get; set; }
    }
    public class LoanAppStatusGetSMSList
    {
        public string Code { get; set; }
        public string SMS { get; set; }
    }

}