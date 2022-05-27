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
    public class CompanyGetController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<CompanyModel> Get(string api_name, string api_key, string json)////json=[{"user":"none","pwd":"none","device_id":"none","app_vName":"none"}]
        {
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "";
            List<CompanyModel> RSData = new List<CompanyModel>();
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
                    }
                }
                #endregion json

                #region data
                if (ERR != "Error")
                {
                    CompanyModel ListHeader = new CompanyModel();
                    ListHeader.ERR = ERR;
                    ListHeader.SMS = SMS;

                    List<CompanyList> DataList = new List<CompanyList>();

                    DataTable dt = c.ReturnDT("exec T24_GetCompany @UserID='" + UserID + "'");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        #region params
                        CompanyList data = new CompanyList();
                        data.OfficeID = dt.Rows[i]["OfficeID"].ToString();
                        data.CompanyCode = dt.Rows[i]["CompanyCode"].ToString();
                        data.OfficeName = dt.Rows[i]["OfficeName"].ToString();
                        data.ShortName = dt.Rows[i]["ShortName"].ToString();
                        data.Mnemonic = dt.Rows[i]["Mnemonic"].ToString();
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
                CompanyModel CustHeader = new CompanyModel();
                CustHeader.ERR = ERR;
                CustHeader.SMS = SMS;
                CustHeader.DataList = null;
                RSData.Add(CustHeader);
            }
            #endregion if Error

            return RSData;
        }


    }

    public class CompanyModel
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
        public List<CompanyList> DataList { get; set; }
    }
    public class CompanyList
    {
        public string OfficeID { get; set; }
        public string CompanyCode { get; set; }
        public string OfficeName { get; set; }
        public string ShortName { get; set; }
        public string Mnemonic { get; set; }
    }


}