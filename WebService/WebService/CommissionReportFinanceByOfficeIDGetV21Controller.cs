using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using WebService.Models.Res.Reports;

namespace WebService
{
    [BasicAuthentication]
    public class CommissionReportFinanceByOfficeIDGetV21Controller : ApiController
    {
        public IEnumerable<CommissionReportByOfficeIDGetV21RS> Post([FromUri]string api_name, string api_key, string username, [FromBody]string json)
        {
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "";
            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            List<CommissionReportByOfficeIDGetV21RS> RSData = new List<CommissionReportByOfficeIDGetV21RS>();
            string ControllerName = "CommissionReportFinanceByOfficeIDGetV21";
            string FileNameForLog = username + "_" + api_name + "_" + ServerDate.Replace("-", "_").Replace(" ", "_").Replace(":", "_");
            try
            {
                c.T24_AddLog(FileNameForLog, "RQ", json, ControllerName);
                #region api
                string[] CheckApi = c.CheckApi(api_name, api_key);
                ERR = CheckApi[0];
                SMS = CheckApi[1];
                #endregion api

                CommissionReportByOfficeIDGetV21RQ jObj = null;
                string user = "", pwd = "", device_id = "", app_vName = "", mac_address = "", criteriaValue = "", criteriaValue2 = "", criteriaValue3 = "", UserID = "";
                #region json
                if (ERR != "Error")
                {
                    try
                    {
                        jObj = JsonConvert.DeserializeObject<CommissionReportByOfficeIDGetV21RQ>(json);
                        user = jObj.user;
                        user = c.Encrypt(user, c.SeekKeyGet());
                        pwd = jObj.pwd;
                        pwd = c.Encrypt(pwd, c.SeekKeyGet());
                        device_id = jObj.device_id;
                        app_vName = jObj.app_vName;
                        mac_address = jObj.mac_address;
                        criteriaValue = jObj.criteriaValue;
                        criteriaValue2 = jObj.criteriaValue2;
                        criteriaValue3 = jObj.criteriaValue3;
                    }
                    catch (Exception ex)
                    {
                        ERR = "Error";
                        SMS = "Invalid JSON";
                    }
                }
                #endregion json
                #region Get User Info.
                if (ERR != "Error")
                {
                    try
                    {
                        SqlConnection Con1 = new SqlConnection(c.ConStr());
                        Con1.Open();
                        SqlCommand Com1 = new SqlCommand();
                        Com1.Connection = Con1;
                        string sql = "exec T24_GetUserInfo_V2 @user=@user,@pwd=@pwd";
                        Com1.CommandText = sql;
                        Com1.Parameters.Clear();
                        Com1.Parameters.AddWithValue("@user", user);
                        Com1.Parameters.AddWithValue("@pwd", pwd);
                        DataTable dt1 = new DataTable();
                        dt1.Load(Com1.ExecuteReader());
                        UserID = dt1.Rows[0]["UserID"].ToString();
                        Con1.Close();
                    }
                    catch (Exception ex)
                    {
                        ERR = "Error";
                        SMS = "invalid user";
                    }
                }
                #endregion Get User Info.

                #region data
                if (ERR != "Error")
                {
                    List<CommissionReportByOfficeIDGetV21List> RSDataList = new List<CommissionReportByOfficeIDGetV21List>();
                    DataTable dt = c.ReturnDT2("exec T24_GetCommissionFinanceReportByOfficeID_V2 @OfficeID='" + criteriaValue + "',@FDate='" + criteriaValue2 + "',@TDate='" + criteriaValue3 + "'");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string ProspectCode = dt.Rows[i]["ProspectCode"].ToString();
                        string OfficeName = dt.Rows[i]["OfficeName"].ToString();
                        string COName = dt.Rows[i]["COName"].ToString();
                        string ClientName = dt.Rows[i]["ClientName"].ToString();
                        string VBName = dt.Rows[i]["VBName"].ToString();
                        string AAID = dt.Rows[i]["AAID"].ToString();
                        string ProductID = dt.Rows[i]["ProductID"].ToString();
                        string DisbDate = dt.Rows[i]["DisbDate"].ToString();
                        string Currency = dt.Rows[i]["Currency"].ToString();
                        string ActualDisbAmt = dt.Rows[i]["ActualDisbAmt"].ToString();
                        string Mnemonic = dt.Rows[i]["Mnemonic"].ToString();
                        string OfficeID = dt.Rows[i]["OfficeID"].ToString();
                        string CommissionAmt = dt.Rows[i]["CommissionAmt"].ToString();
                        CommissionReportByOfficeIDGetV21List DataList = new CommissionReportByOfficeIDGetV21List();
                        DataList.ProspectCode = ProspectCode;
                        DataList.OfficeName = OfficeName;
                        DataList.COName = COName;
                        DataList.ClientName = ClientName;
                        DataList.VBName = VBName;
                        DataList.AAID = AAID;
                        DataList.ProductID = ProductID;
                        DataList.DisbDate = DisbDate;
                        DataList.Currency = Currency;
                        DataList.ActualDisbAmt = ActualDisbAmt;
                        DataList.Mnemonic = Mnemonic;
                        DataList.OfficeID = OfficeID;
                        DataList.CommissionAmt = CommissionAmt;
                        RSDataList.Add(DataList);
                    }

                    CommissionReportByOfficeIDGetV21RS ListHeader = new CommissionReportByOfficeIDGetV21RS();
                    ListHeader.ERR = ERR;
                    ListHeader.SMS = SMS;
                    ListHeader.DataList = RSDataList;
                    RSData.Add(ListHeader);
                }
                #endregion data

            }
            catch { }
            try
            {
                var jsonRS = new JavaScriptSerializer().Serialize(RSData);
                c.T24_AddLog(FileNameForLog, "RS", jsonRS.ToString(), ControllerName);
            }
            catch { }
            return RSData;
        }
    }
}


public class CommissionReportByOfficeIDGetV21RQ
{
    public string user { get; set; }
    public string pwd { get; set; }
    public string device_id { get; set; }
    public string app_vName { get; set; }
    public string mac_address { get; set; }
    public string criteriaValue { get; set; }
    public string criteriaValue2 { get; set; }
    public string criteriaValue3 { get; set; }
}
public class CommissionReportByOfficeIDGetV21RS
{
    public string ERR { get; set; }
    public string SMS { get; set; }
    public List<CommissionReportByOfficeIDGetV21List> DataList;
}
