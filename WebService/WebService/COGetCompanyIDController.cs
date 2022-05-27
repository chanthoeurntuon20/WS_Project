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
    public class COGetCompanyIDController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<COGetCompanyIDModel> Get(string api_name, string api_key, string json)////json=[{"user":"none","pwd":"none","device_id":"none","app_vName":"none","criteriaValue":"KH0010101"}]
        {
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "", OfficeID = "";
            List<COGetCompanyIDModel> RSData = new List<COGetCompanyIDModel>();
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
                        OfficeID = CheckJson[2];
                    }
                }
                #endregion json

                #region data
                if (ERR != "Error")
                {
                    COGetCompanyIDModel ListHeader = new COGetCompanyIDModel();
                    ListHeader.ERR = ERR;
                    ListHeader.SMS = SMS;

                    List<COGetCompanyIDList> DataList = new List<COGetCompanyIDList>();

                    DataTable dt = c.ReturnDT("exec T24_GetCOByCompany @UserID='" + UserID + "',@CompanyID='" + OfficeID + "'");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        #region params
                        COGetCompanyIDList data = new COGetCompanyIDList();
                        data.UserID = dt.Rows[i]["UserID"].ToString();
                        data.UserName = dt.Rows[i]["UserName"].ToString();
                        DataList.Add(data);
                        #endregion params
                    }

                    ListHeader.DataList = DataList;

                    RSData.Add(ListHeader);
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
                COGetCompanyIDModel CustHeader = new COGetCompanyIDModel();
                CustHeader.ERR = ERR;
                CustHeader.SMS = SMS;
                CustHeader.DataList = null;
                RSData.Add(CustHeader);
            }
            #endregion if Error

            return RSData;
        }

    }
    public class COGetCompanyIDModel
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
        public List<COGetCompanyIDList> DataList { get; set; }
    }
    public class COGetCompanyIDList
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
    }

}