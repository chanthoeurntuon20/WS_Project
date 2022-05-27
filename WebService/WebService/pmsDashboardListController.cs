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
    public class pmsDashboardListController : ApiController
    {

        public string Post([FromUri]string p, [FromUri]string msgid, [FromBody]string json)
        {
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "", ExSMS = "", ERRCode = "";
            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string ControllerName = "pmsDashboardList";
            string FileNameForLog = msgid + "_" + ServerDate.Replace("-", "_").Replace(" ", "_").Replace(":", "_").Replace(".", "_");
            List<pmsDashboardListDataRSModel> RSData = new List<pmsDashboardListDataRSModel>();
            pmsDashboardListDataRSModel Header = new pmsDashboardListDataRSModel();
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
                pmsDashboardListRQModel jObj = null;
                string
                    UserIDRS = "", CBC_PSM="";

                if (ERR != "Error")
                {
                    try
                    {
                        jObj = JsonConvert.DeserializeObject<pmsDashboardListRQModel>(json);

                        UserIDRS = jObj.UserID;
                        CBC_PSM = jObj.CBC_PSM;
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
                        string sql = "exec fact_pms_dashboard_list @UserID,@CBC_PSM";
                        Com1.CommandText = sql;
                        Com1.Parameters.Clear();
                        Com1.Parameters.AddWithValue("@UserID", UserIDRS);
                        Com1.Parameters.AddWithValue("@CBC_PSM", CBC_PSM);

                        DataTable dt = new DataTable();
                        dt.Load(Com1.ExecuteReader());

                        if (dt.Rows.Count > 0)
                        {
                            List<pmsDashboardListDataListRSModel> DataList = new List<pmsDashboardListDataListRSModel>();

                            for (int i = 0; i <= dt.Rows.Count - 1; i++)
                            {
                                pmsDashboardListDataListRSModel dtList = new pmsDashboardListDataListRSModel();
                                dtList.cID = dt.Rows[i]["cID"].ToString();
                                dtList.CustomerName = dt.Rows[i]["CustomerName"].ToString();
                                dtList.TriggerType = dt.Rows[i]["TriggerType"].ToString();

                                dtList.TEL_MOBILE = dt.Rows[i]["TEL_MOBILE"].ToString();
                                dtList.VillageBank = dt.Rows[i]["VillageBank"].ToString();
                                dtList.date_meet = Convert.ToDateTime(dt.Rows[i]["date_meet"]).ToString("yyyy-MM-dd hh:mm tt");

                                dtList.fDate = Convert.ToDateTime(dt.Rows[i]["fDate"]).ToString("yyyy-MM-dd");
                                dtList.tDate = Convert.ToDateTime(dt.Rows[i]["tDate"]).ToString("yyyy-MM-dd");

                                DataList.Add(dtList);
                            }

                            Header.DataList = DataList;

                        }
                        else
                        {

                            ERR = "Error";
                            SMS = "No data";

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

public class pmsDashboardListRQModel
{
    public string UserID { get; set; }
    public string CBC_PSM { get; set; }
}

public class pmsDashboardListDataRSModel
{
    public string ERR { get; set; }
    public string SMS { get; set; }
    public string ERRCode { get; set; }
    public List<pmsDashboardListDataListRSModel> DataList { get; set; }
}

public class pmsDashboardListDataListRSModel
{
    public string cID { get; set; }
    public string CustomerName { get; set; }
    public string TriggerType { get; set; }
    public string TEL_MOBILE { get; set; }
    public string VillageBank { get; set; }
    public string date_meet { get; set; }
    public string fDate { get; set; }
    public string tDate { get; set; }
}

#endregion