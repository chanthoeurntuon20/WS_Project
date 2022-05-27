using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace WebService
{
    [BasicAuthentication]
    public class CustomerGetByRangController : ApiController
    {
        // GET api/<controller>
        //public IEnumerable<CustomerModel> Get(string api_name, string api_key, string json)
        public string Post([FromUri]string p, [FromUri]string msgid,[FromBody]string json)//json={"criteriaValue":"1","criteriaValue2":"10"}
        {////json=[{"user":"11774","pwd":"1","device_id":"352405061542333","app_vName":"1.6","criteriaValue":"1","criteriaValue2":"10"}]
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "", ExSMS = "", ERRCode = "";
            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string ControllerName = "CustomerGetByRang";
            string FileNameForLog = msgid + "_" + ServerDate.Replace("-", "_").Replace(" ", "_").Replace(":", "_").Replace(".", "_");
            List<CustomerModel> RSData = new List<CustomerModel>();
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
                string UserID = "";
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
                CustomerGetByRangRQModel jObj = null;
                string criteriaValue = "", criteriaValue2 = "";
                if (ERR != "Error")
                {
                    try
                    {
                        jObj = JsonConvert.DeserializeObject<CustomerGetByRangRQModel>(json);
                        criteriaValue = jObj.criteriaValue;
                        criteriaValue2 = jObj.criteriaValue2;
                        int x = Convert.ToInt32(criteriaValue);
                        int x2 = Convert.ToInt32(criteriaValue2);
                    }
                    catch (Exception ex)
                    {
                        ERR = "Error";
                        ExSMS = ex.Message.ToString();
                        //get sms
                        string[] str = c.GetSMSByMsgID("10");
                        ERR = str[0];
                        if (ERR == "Error")
                        {
                            SMS = str[1];
                            ExSMS = ExSMS + "|" + str[2];
                        }
                        else
                        {
                            SMS = str[3];
                        }
                        ERR = "Error";
                    }
                }
                #endregion
                #region data
                if (ERR != "Error")
                {
                    CustomerModel ListHeader = new CustomerModel();
                    ListHeader.ERR = ERR;
                    ListHeader.SMS = SMS;

                    List<CustList> DataList = new List<CustList>();

                    DataTable dt = c.ReturnDT2("exec T24_GetCustomerByAMOrCOByDeviceByRang @UserID='" + UserID 
                        + "',@FromRow='"+ criteriaValue + "',@ToRow='"+ criteriaValue2 + "'");                    
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
            catch (Exception ex)
            {
                ERR = "Error";
                SMS = "Something was wrong";
            }
            #region if Error
            if (ERR == "Error")
            {
                CustomerModel CustHeader = new CustomerModel();
                CustHeader.ERR = ERR;
                CustHeader.SMS = SMS;
                CustHeader.ERRCode = ERRCode;
                CustHeader.DataList = null;
                RSData.Add(CustHeader);
            }
            #endregion if Error
            string RSDataStr = "";
            try
            {
                var jsonRS = new JavaScriptSerializer().Serialize(RSData);
                RSDataStr = c.Encrypt(jsonRS, c.SeekKeyGet());
                //c.T24_AddLog(FileNameForLog, "RS", jsonRS.ToString(), ControllerName);
            }
            catch { }
            return RSDataStr;
        }


    }

}