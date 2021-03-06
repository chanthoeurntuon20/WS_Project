using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace WebService
{
    [BasicAuthentication]
    public class pmsNewAccountApointmentNewMeetController : ApiController
    {


        public string Post([FromUri]string p, [FromUri]string msgid, [FromBody]string json)
        {
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "", ExSMS = "", ERRCode = "";
            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string ControllerName = "pmsNewAccountApointmentNewMeet";
            string FileNameForLog = msgid + "_" + ServerDate.Replace("-", "_").Replace(" ", "_").Replace(":", "_").Replace(".", "_");
            List<pmsNewAccountApointmentNewMeetRSModel> RSData = new List<pmsNewAccountApointmentNewMeetRSModel>();
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
                c.T24_AddLog(FileNameForLog, "2.RQ", json, ControllerName);
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
                pmsNewAccountApointmentNewMeetRQModel jObj = null;
                string
                    pms_apo_id = "", CID = "", date_meet_new = "", comment_meet_new = "", create_by = "";

                if (ERR != "Error")
                {
                    try
                    {
                        jObj = JsonConvert.DeserializeObject<pmsNewAccountApointmentNewMeetRQModel>(json);
                        pms_apo_id = jObj.pms_apo_id;
                        CID = jObj.CID;
                        try
                        {

                            string yyyy = jObj.date_meet_new.Substring(0, 4);
                            string mm = jObj.date_meet_new.Substring(5, 2);
                            string dd = jObj.date_meet_new.Substring(8, 2);
                            string hh = jObj.date_meet_new.Substring(11, 2);
                            string ms = jObj.date_meet_new.Substring(14, 2);
                            string am = jObj.date_meet_new.Substring(17, 2);

                            date_meet_new = Convert.ToDateTime(yyyy + "-" + mm + "-" + dd + " " + hh + ":" + ms + " " + am).ToString("yyyy-MM-dd hh:mm tt");

                        }
                        catch (Exception ex)
                        {

                            ERR = "Error";
                            ExSMS = ex.Message.ToString();
                            //get sms
                            string[] str = c.GetSMSByMsgID("2");
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

                        comment_meet_new = jObj.comment_meet_new;
                        create_by = jObj.create_by;
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
                    try
                    {
                        string sql = "exec fact_pms_apointment_new_account_new_meet @pms_apo_id,@CID,@date_meet_new,@comment_meet_new,@create_by";
                        Com1.CommandText = sql;
                        Com1.Parameters.Clear();
                        Com1.Parameters.AddWithValue("@pms_apo_id", pms_apo_id);
                        Com1.Parameters.AddWithValue("@CID", CID);
                        Com1.Parameters.AddWithValue("@date_meet_new", date_meet_new);
                        Com1.Parameters.AddWithValue("@comment_meet_new", comment_meet_new);
                        Com1.Parameters.AddWithValue("@create_by", create_by);

                        DataTable dt = new DataTable();
                        dt.Load(Com1.ExecuteReader());

                        if (dt.Rows[0][0].ToString() != "0")
                        {
                            ERR = "Error";
                            SMS = dt.Rows[0][1].ToString();
                        }
                    }
                    catch (Exception ex)
                    {
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

            #region Response

            pmsNewAccountApointmentNewMeetRSModel Header = new pmsNewAccountApointmentNewMeetRSModel();
            Header.ERR = ERR;
            Header.SMS = SMS;
            Header.ERRCode = ERRCode;
            RSData.Add(Header);

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
}

#region model

public class pmsNewAccountApointmentNewMeetRQModel
{
    public string pms_apo_id { get; set; }
    public string CID { get; set; }
    public string date_meet_new { get; set; }
    public string comment_meet_new { get; set; }
    public string create_by { get; set; }
}

public class pmsNewAccountApointmentNewMeetRSModel
{
    public string ERR { get; set; }
    public string SMS { get; set; }
    public string ERRCode { get; set; }
}

#endregion