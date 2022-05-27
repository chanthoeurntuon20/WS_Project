using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace WebService
{
    [BasicAuthentication]
    public class pmsDueBalanceController : ApiController
    {
        public string Post([FromUri]string p, [FromUri]string msgid, [FromBody]string json)
        {//json={"search_type":"CID","search_data":"1614328","hit_date_from":"2020-08-21","hit_date_to":"2020-08-21","co_T24":"1234"}
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "", ExSMS = "", ERRCode = "";
            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string ControllerName = "pmsDueBalance";
            string FileNameForLog = msgid + "_" + ServerDate.Replace("-", "_").Replace(" ", "_").Replace(":", "_").Replace(".", "_");
            List<pmsDueBalanceRSModel> RSData = new List<pmsDueBalanceRSModel>();
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
                pmsDueBalanceRQModel jObj = null;
                string search_type = "", search_data="", hit_date_from="", hit_date_to="", co_T24="";
                if (ERR != "Error")
                {
                    try
                    {
                        jObj = JsonConvert.DeserializeObject<pmsDueBalanceRQModel>(json);
                        //pms_sp = jObj.pms_sp;
                        search_type = jObj.search_type;
                        search_data = jObj.search_data;
                        hit_date_from = jObj.hit_date_from;
                        hit_date_to = jObj.hit_date_to;
                        co_T24 = jObj.co_T24;
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
                    SqlConnection Con1 = new SqlConnection(c.ConStr());
                    Con1.Open();
                    SqlCommand Com1 = new SqlCommand();
                    Com1.Connection = Con1;
                    try {
                        string sql = "exec pmsamk_due_balance @search_type=@search_type,@search_data=@search_data,@hit_date_from=@hit_date_from,@hit_date_to=@hit_date_to,@co_T24=@co_T24";
                        Com1.CommandText = sql;
                        Com1.Parameters.Clear();
                        Com1.Parameters.AddWithValue("@search_type", search_type);
                        Com1.Parameters.AddWithValue("@search_data", search_data);
                        Com1.Parameters.AddWithValue("@hit_date_from", hit_date_from);
                        Com1.Parameters.AddWithValue("@hit_date_to", hit_date_to);
                        Com1.Parameters.AddWithValue("@co_T24", co_T24);
                        DataTable dt = new DataTable();
                        dt.Load(Com1.ExecuteReader());

                        pmsDueBalanceRSModel ListHeader = new pmsDueBalanceRSModel();
                        ListHeader.ERR = ERR;
                        ListHeader.SMS = SMS;
                        ListHeader.ERRCode = ERRCode;
                        List<pmsDueBalanceDataRSModel> DataList = new List<pmsDueBalanceDataRSModel>();
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            #region params
                            pmsDueBalanceDataRSModel data = new pmsDueBalanceDataRSModel();
                            data.CID = dt.Rows[i]["CustomerNum"].ToString();
                            data.customer_name_kh = dt.Rows[i]["customerName"].ToString();
                            data.phone_number = dt.Rows[i]["Tel_mobile"].ToString();
                            data.vb_name = dt.Rows[i]["villageBank"].ToString();
                            data.hit_date = dt.Rows[i]["HIT_DATE"].ToString();
                            data.number_of_enquiry = dt.Rows[i]["Due_Account"].ToString();
                            DataList.Add(data);
                            #endregion params
                        }
                        ListHeader.Data = DataList;
                        RSData.Add(ListHeader);
                    } catch(Exception ex) {
                        ERR = "Error";
                        SMS = "Something was wrong";
                        ExSMS = ex.Message.ToString();
                    }
                    Con1.Close();
                }
                #endregion data
            }
            catch (Exception ex)
            {
                ERR = "Error";
                SMS = "Something was wrong";
                ExSMS = ex.Message.ToString();
            }

            #region if Error
            if (ERR == "Error")
            {
                pmsDueBalanceRSModel CustHeader = new pmsDueBalanceRSModel();
                CustHeader.ERR = ERR;
                CustHeader.SMS = SMS;
                CustHeader.ERRCode = ERRCode;
                CustHeader.Data = null;
                RSData.Add(CustHeader);
            }
            #endregion if Error

            string RSDataStr = "";
            try
            {
                var jsonRS = new JavaScriptSerializer().Serialize(RSData);
                RSDataStr = c.Encrypt(jsonRS, c.SeekKeyGet());
                c.T24_AddLog(FileNameForLog, "4.RS", jsonRS.ToString(), ControllerName);
            }
            catch { }
            return RSDataStr;
        }

    }

    #region model    
    public class pmsDueBalanceRQModel
    {
        //public string pms_sp { get; set; }
        public string search_type { get; set; }
        public string search_data { get; set; }
        public string hit_date_from { get; set; }
        public string hit_date_to { get; set; }
        public string co_T24 { get; set; }//OfficeHierachyID
    }

    public class pmsDueBalanceRSModel
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
        public string ERRCode { get; set; }
        public List<pmsDueBalanceDataRSModel> Data { get; set; }
    }
    public class pmsDueBalanceDataRSModel
    {
        public string CID { get; set; }
        public string customer_name_kh { get; set; }
        public string phone_number { get; set; }
        public string vb_name { get; set; }
        public string hit_date { get; set; }
        public string number_of_enquiry { get; set; }
    }
    #endregion

}