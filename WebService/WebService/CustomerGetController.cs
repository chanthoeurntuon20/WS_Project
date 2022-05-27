using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace WebService
{
    [BasicAuthentication]
    public class CustomerGetController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<CustomerModel> Get(string api_name, string api_key, string json)////json=[{"user":"01804","pwd":"040882","device_id":"352405061542333","app_vName":"1.6"}]
        {
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "";
            List<CustomerModel> RSData = new List<CustomerModel>();
            try {
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
                    if (ERR == "Error") {
                        SMS = CheckJson[1];
                    }
                    else {
                        UserID = CheckJson[1];
                    }
                }
                #endregion json

                #region data
                if (ERR != "Error") {
                    CustomerModel ListHeader = new CustomerModel();
                    ListHeader.ERR = ERR;
                    ListHeader.SMS = SMS;

                    List<CustList> DataList = new List<CustList>();
                    
                    DataTable dt = c.ReturnDT("exec T24_GetCustomerByAMOrCOByDevice @UserID='"+ UserID + "'");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        #region params
                        CustList data = new CustList();
                        data.CUSTID = dt.Rows[i]["CUSTID"].ToString();
                        data.CUSTTYPE = dt.Rows[i]["CUSTTYPE"].ToString();
                        data.SHORTNAME = dt.Rows[i]["SHORTNAME"].ToString();
                        data.NAME1 = dt.Rows[i]["NAME1"].ToString();
                        data.BIRTHINCORPDATE = dt.Rows[i]["BIRTHINCORPDATE"].ToString();
                        data.AMKBIRTHPLACE = dt.Rows[i]["AMKBIRTHPLACE"].ToString();
                        data.GENDER = dt.Rows[i]["GENDER"].ToString();
                        data.MARITALSTATUS = dt.Rows[i]["MARITALSTATUS"].ToString();
                        data.RESIDEYN = dt.Rows[i]["RESIDEYN"].ToString();
                        data.NATIONALITY = dt.Rows[i]["NATIONALITY"].ToString();
                        data.RESIDENCE = dt.Rows[i]["RESIDENCE"].ToString();
                        data.AMKIDTYPE = dt.Rows[i]["AMKIDTYPE"].ToString();
                        data.AMKIDNO = dt.Rows[i]["AMKIDNO"].ToString();
                        data.AMKIDISSDATE = dt.Rows[i]["AMKIDISSDATE"].ToString();
                        data.AMKIDEXDATE = dt.Rows[i]["AMKIDEXDATE"].ToString();
                        data.STREET = dt.Rows[i]["STREET"].ToString();
                        data.AMKPROVINCE = dt.Rows[i]["AMKPROVINCE"].ToString();
                        data.AMKDISTRICT = dt.Rows[i]["AMKDISTRICT"].ToString();
                        data.AMKCOMMUNE = dt.Rows[i]["AMKCOMMUNE"].ToString();
                        data.AMKVILLAGE = dt.Rows[i]["AMKVILLAGE"].ToString();
                        data.POSTALCODE = dt.Rows[i]["POSTALCODE"].ToString();
                        data.TELMOBILE = dt.Rows[i]["TELMOBILE"].ToString();
                        data.EMAILADDRESS = dt.Rows[i]["EMAILADDRESS"].ToString();
                        data.AMKOCCUPTYPE = dt.Rows[i]["AMKOCCUPTYPE"].ToString();
                        data.AMKOCCUPDET = dt.Rows[i]["AMKOCCUPDET"].ToString();
                        data.SPMEMNO = dt.Rows[i]["SPMEMNO"].ToString();
                        data.SPNAME = dt.Rows[i]["SPNAME"].ToString();
                        data.AMKSPDOB = dt.Rows[i]["AMKSPDOB"].ToString();
                        data.AMKSPIDTYPE = dt.Rows[i]["AMKSPIDTYPE"].ToString();
                        data.AMKSPIDNO = dt.Rows[i]["AMKSPIDNO"].ToString();
                        data.AMKSPIDISDT = dt.Rows[i]["AMKSPIDISDT"].ToString();
                        data.AMKSPIDEXDT = dt.Rows[i]["AMKSPIDEXDT"].ToString();
                        data.PROFESSION = dt.Rows[i]["PROFESSION"].ToString();
                        data.AMKPOVERTYST = dt.Rows[i]["AMKPOVERTYST"].ToString();
                        data.NOOFDEPEND = dt.Rows[i]["NOOFDEPEND"].ToString();
                        data.MAININCOME = dt.Rows[i]["MAININCOME"].ToString();
                        data.TITLE = dt.Rows[i]["TITLE"].ToString();
                        data.AMKVILLAGEBK = dt.Rows[i]["AMKVILLAGEBK"].ToString();
                        data.AMKNOACTMEM = dt.Rows[i]["AMKNOACTMEM"].ToString();
                        data.KhmerName = dt.Rows[i]["KhmerName"].ToString();
                        data.KhmerFirstName = dt.Rows[i]["KhmerFirstName"].ToString();
                        data.KhmerLastName = dt.Rows[i]["KhmerLastName"].ToString();
                        data.LocationCode = dt.Rows[i]["LocationCode"].ToString();
                        DataList.Add(data);
                        #endregion params
                    }

                    ListHeader.DataList = DataList;

                    RSData.Add(ListHeader);
                }
                #endregion data
            }
            catch (Exception ex) {
                ERR = "Error";
                SMS = "Something was wrong";
            }
            #region if Error
            if (ERR == "Error")
            {
                CustomerModel CustHeader = new CustomerModel();
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