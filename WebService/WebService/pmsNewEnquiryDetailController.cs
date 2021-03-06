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
    public class pmsNewEnquiryDetailController : ApiController
    {
        public string Post([FromUri]string p, [FromUri]string msgid, [FromBody]string json)
        {//json={"CID":"123456","hit_date_from":"2020-08-21","hit_date_to":"2020-08-21","co_T24":"1234"}
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "", ExSMS = "", ERRCode = "";
            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string ControllerName = "pmsNewEnquiryDetail";
            string FileNameForLog = msgid + "_" + ServerDate.Replace("-", "_").Replace(" ", "_").Replace(":", "_").Replace(".", "_");

            List<pmsNewEnquiryDetailRSModel> RSData = new List<pmsNewEnquiryDetailRSModel>();
            pmsNewEnquiryDetailRSModel ListHeader = new pmsNewEnquiryDetailRSModel();
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
                pmsNewEnquiryDetailRQModel jObj = null;
                string CID = "", hit_date_from="", hit_date_to="", co_T24="";
                if (ERR != "Error")
                {
                    try
                    {
                        jObj = JsonConvert.DeserializeObject<pmsNewEnquiryDetailRQModel>(json);
                        //pms_sp = jObj.pms_sp;
                        CID = jObj.CID;
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

                    #region Pmsamk_new_enquiry_pms

                    List<pmsNewEnquiryDetailDataRSModel> DataList = new List<pmsNewEnquiryDetailDataRSModel>();

                    try
                    {
                        string sql = "exec Pmsamk_new_enquiry_pms @customerNum=@customerNum,@hit_date_from=@hit_date_from,@hit_date_to=@hit_date_to,@co_T24=@co_T24";
                        Com1.CommandText = sql;
                        Com1.Parameters.Clear();
                        Com1.Parameters.AddWithValue("@customerNum", CID);
                        Com1.Parameters.AddWithValue("@hit_date_from", hit_date_from);
                        Com1.Parameters.AddWithValue("@hit_date_to", hit_date_to);
                        Com1.Parameters.AddWithValue("@co_T24", co_T24);
                        DataTable dt = new DataTable();
                        dt.Load(Com1.ExecuteReader());

                        ListHeader.ERR = ERR;
                        ListHeader.SMS = SMS;
                        ListHeader.ERRCode = ERRCode;

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            #region params
                            pmsNewEnquiryDetailDataRSModel data = new pmsNewEnquiryDetailDataRSModel();
                            data.PRD_TYPE = dt.Rows[i]["PRD_TYPE"].ToString();
                            data.APPLICANT_TYPE = dt.Rows[i]["APPLICANT_TYPE"].ToString();
                            data.MEMBER = dt.Rows[i]["MEMBER"].ToString();
                            data.EnquiryDate = dt.Rows[i]["EnquiryDate"].ToString();
                            data.HitDate = dt.Rows[i]["HitDate"].ToString();
                            DataList.Add(data);
                            #endregion params
                        }
                        
                    }
                    catch (Exception ex) {
                        ERR = "Error";
                        SMS = "Something was wrong";
                        ExSMS = ex.Message.ToString();
                        DataList = null;
                    }

                    ListHeader.Data = DataList;

                    #endregion

                    #region Pmsamk_new_enquiry_pms_amk

                    List<pmsNewEnquiryDetailAMKDataRSModel> DataList_PMS_AMK = new List<pmsNewEnquiryDetailAMKDataRSModel>();

                    if (ERR != "Error")
                    {
                        try
                        {
                            string sql = "exec Pmsamk_new_enquiry_pms_amk @customerNum=@customerNum,@hit_date_from=@hit_date_from,@hit_date_to=@hit_date_to,@co_T24=@co_T24";
                            Com1.CommandText = sql;
                            Com1.Parameters.Clear();
                            Com1.Parameters.AddWithValue("@customerNum", CID);
                            Com1.Parameters.AddWithValue("@hit_date_from", hit_date_from);
                            Com1.Parameters.AddWithValue("@hit_date_to", hit_date_to);
                            Com1.Parameters.AddWithValue("@co_T24", co_T24);
                            DataTable dt = new DataTable();
                            dt.Load(Com1.ExecuteReader());

                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                #region params
                                pmsNewEnquiryDetailAMKDataRSModel data = new pmsNewEnquiryDetailAMKDataRSModel();
                                data.ProductDesc = dt.Rows[i]["ProductDesc"].ToString();
                                data.Balance = dt.Rows[i]["Balance"].ToString();
                                data.OutstandingBalance = dt.Rows[i]["OutstandingBalance"].ToString();
                                data.currency = dt.Rows[i]["currency"].ToString();
                                data.amk_status = dt.Rows[i]["ParAging"].ToString();
                                DataList_PMS_AMK.Add(data);
                                #endregion params
                            }

                        }
                        catch (Exception ex)
                        {
                            ERR = "Error";
                            SMS = "Something was wrong";
                            ExSMS = ex.Message.ToString();
                            DataList_PMS_AMK = null;
                        }
                    }
                    else {
                        DataList_PMS_AMK = null;
                    }

                    ListHeader.DataAMK = DataList_PMS_AMK;

                    #endregion

                    #region Pmsamk_new_enquiry_pms_status

                    List<pmsNewEnquiryStatusRSModel> DataListStatus = new List<pmsNewEnquiryStatusRSModel>();

                    if (ERR != "Error")
                    {
                        try
                        {
                            string sql = "exec fact_pms_apointment_new_enquiry_status @CID";
                            Com1.CommandText = sql;
                            Com1.Parameters.Clear();
                            Com1.Parameters.AddWithValue("@CID", CID);
                            DataTable dt = new DataTable();
                            dt.Load(Com1.ExecuteReader());

                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                #region params
                                pmsNewEnquiryStatusRSModel data = new pmsNewEnquiryStatusRSModel();
                                data.pms_status = dt.Rows[i]["pms_status"].ToString();
                                data.pms_apo_id = dt.Rows[i]["pms_apo_id"].ToString();
                                try
                                {
                                    data.date_meet = Convert.ToDateTime(dt.Rows[i]["date_meet"]).ToString("yyyy-MM-dd hh:mm tt");
                                }
                                catch (Exception) { data.date_meet = ""; }
                                data.comment_new = dt.Rows[i]["comment_new"].ToString();
                                DataListStatus.Add(data);
                                #endregion params
                            }

                        }
                        catch (Exception ex)
                        {
                            ERR = "Error";
                            SMS = "Something was wrong";
                            ExSMS = ex.Message.ToString();
                            DataListStatus = null;
                        }
                    }
                    else {
                        DataListStatus = null;
                    }

                    ListHeader.DataStatus = DataListStatus;

                    #endregion

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


            #region Response

            ListHeader.ERR = ERR;
            ListHeader.SMS = SMS;
            ListHeader.ERRCode = ERRCode;
            RSData.Add(ListHeader);

            #endregion

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
    public class pmsNewEnquiryDetailRQModel
    {
        public string CID { get; set; }
        public string hit_date_from { get; set; }
        public string hit_date_to { get; set; }
        public string co_T24 { get; set; }//OfficeHierachyID
    }
    public class pmsNewEnquiryDetailRSModel
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
        public string ERRCode { get; set; }
        public List<pmsNewEnquiryDetailDataRSModel> Data { get; set; }
        public List<pmsNewEnquiryDetailAMKDataRSModel> DataAMK { get; set; }
        public List<pmsNewEnquiryStatusRSModel> DataStatus { get; set; }
    }
    public class pmsNewEnquiryDetailDataRSModel
    {
        public string PRD_TYPE { get; set; }
        public string APPLICANT_TYPE { get; set; }
        public string MEMBER { get; set; }
        public string EnquiryDate { get; set; }
        public string HitDate { get; set; }
    }
    public class pmsNewEnquiryDetailAMKDataRSModel
    {
        public string ProductDesc { get; set; }
        public string Balance { get; set; }
        public string OutstandingBalance { get; set; }
        public string currency { get; set; }
        public string amk_status { get; set; }//ParAging
    }

    public class pmsNewEnquiryStatusRSModel
    {
        public string pms_status { get; set; }
        public string pms_apo_id { get; set; }
        public string date_meet { get; set; }
        public string comment_new { get; set; }
    }

    #endregion

}