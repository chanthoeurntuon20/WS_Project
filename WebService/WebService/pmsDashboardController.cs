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
    public class pmsDashboardController : ApiController
    {

        public string Post([FromUri]string p, [FromUri]string msgid, [FromBody]string json)
        {
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "", ExSMS = "", ERRCode = "";
            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string ControllerName = "pmsDashboard";
            string FileNameForLog = msgid + "_" + ServerDate.Replace("-", "_").Replace(" ", "_").Replace(":", "_").Replace(".", "_");
            List<pmsDashboardRSModel> RSData = new List<pmsDashboardRSModel>();
            pmsDashboardRSModel Header = new pmsDashboardRSModel();
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
                pmsDashboardRQModel jObj = null;
                string
                    UserIDRS = "";

                if (ERR != "Error")
                {
                    try
                    {
                        jObj = JsonConvert.DeserializeObject<pmsDashboardRQModel>(json);

                        UserIDRS = jObj.UserID;
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
                        string sql = "exec fact_pms_dashboard @UserID";
                        Com1.CommandText = sql;
                        Com1.Parameters.Clear();
                        Com1.Parameters.AddWithValue("@UserID", UserIDRS);

                        DataTable dt = new DataTable();
                        dt.Load(Com1.ExecuteReader());

                        if (dt.Rows.Count > 0)
                        {
                            List<pmsDashboardListRSModel> DataList = new List<pmsDashboardListRSModel>();

                            for (int i = 0; i <= dt.Rows.Count - 1; i++)
                            {
                                pmsDashboardListRSModel dtList = new pmsDashboardListRSModel();
                                dtList.ID = dt.Rows[i]["ID"].ToString();
                                dtList.CBC_PMS_Description = dt.Rows[i]["CBC_PMS_Description"].ToString();
                                dtList.CBC_PMS_Amount = dt.Rows[i]["CBC_PMS_Amount"].ToString();
                                DataList.Add(dtList);
                            }

                            Header.DataList = DataList;

                        }
                        else {

                            ERR = "Error";
                            SMS = "Something was wrong";

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

public class pmsDashboardRQModel
{
    public string UserID { get; set; }
}

public class pmsDashboardRSModel
{
    public string ERR { get; set; }
    public string SMS { get; set; }
    public string ERRCode { get; set; }
    public List<pmsDashboardListRSModel> DataList { get;set; }
}

public class pmsDashboardListRSModel
{
    public string ID { get; set; }
    public string CBC_PMS_Description { get; set; }
    public string CBC_PMS_Amount { get; set; }
}

#endregion