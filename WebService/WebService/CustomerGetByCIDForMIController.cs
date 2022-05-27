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
    public class CustomerGetByCIDForMIController : ApiController
    {

        public IEnumerable<CustomerForMIModel> Get(string api_name, string api_key, string json)////json=[{"user":"01804","pwd":"040882","device_id":"352405061542333","app_vName":"1.6","criteriaValue":"1000056"}]
        {
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "";
            List<CustomerForMIModel> RSData = new List<CustomerForMIModel>();
            try
            {
                #region api
                string[] CheckApi = c.CheckApi(api_name, api_key);
                ERR = CheckApi[0];
                SMS = CheckApi[1];
                #endregion api

                #region json
                string UserID = "", criteriaValue="";
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
                        criteriaValue= CheckJson[2];
                    }
                }
                #endregion json

                #region data
                if (ERR != "Error")
                {
                    CustomerForMIModel ListHeader = new CustomerForMIModel();
                    ListHeader.ERR = ERR;
                    ListHeader.SMS = SMS;

                    List<CustForMIList> DataList = new List<CustForMIList>();

                    DataTable dt = c.ReturnDT("exec T24_CustomerGetByCIDForMI @UserID='" + UserID + "',@CUSTID='"+ criteriaValue + "'");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        #region params
                        CustForMIList data = new CustForMIList();
                        data.customerid = dt.Rows[i]["customerid"].ToString();
                        data.enname = dt.Rows[i]["enname"].ToString();
                        data.maritalcode = dt.Rows[i]["maritalcode"].ToString();
                        data.identifyID = dt.Rows[i]["identifyID"].ToString();
                        data.telephone = dt.Rows[i]["telephone"].ToString();
                        data.gender = dt.Rows[i]["gender"].ToString();
                        data.identifytype = dt.Rows[i]["identifytype"].ToString();
                        data.dateofbirth = dt.Rows[i]["dateofbirth"].ToString();
                        data.province = dt.Rows[i]["province"].ToString();
                        data.district = dt.Rows[i]["district"].ToString();
                        data.commune = dt.Rows[i]["commune"].ToString();
                        data.village = dt.Rows[i]["village"].ToString();
                        data.spousenameEN = dt.Rows[i]["spousenameEN"].ToString();
                        data.spouseGender = dt.Rows[i]["spouseGender"].ToString();
                        data.spouseDateBirth = dt.Rows[i]["spouseDateBirth"].ToString();
                        data.Nationality = dt.Rows[i]["Nationality"].ToString();
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
                CustomerForMIModel CustHeader = new CustomerForMIModel();
                CustHeader.ERR = ERR;
                CustHeader.SMS = SMS;
                CustHeader.DataList = null;
                RSData.Add(CustHeader);
            }
            #endregion if Error

            return RSData;
        }


    }
}