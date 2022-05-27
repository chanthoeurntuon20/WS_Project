using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;

namespace WebService
{
    [BasicAuthentication]
    public class ScheduleTaskAddEditByTabController : ApiController
    {
        //[EnableCors(origins: "*", headers: "*", methods: "*")]
        // GET api/<controller>
        //public IEnumerable<ProspectGetV2RSModel> Post([FromUri]string api_name, string api_key,string username, [FromBody]string json)
        public string Post([FromUri]string p, [FromUri]string msgid, [FromBody]string json)
        {
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "", ExSMS = "", ERRCode = "";
            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string ControllerName = "ScheduleTaskAddEditByTab";
            string FileNameForLog = msgid + "_" + ServerDate.Replace("-", "_").Replace(" ", "_").Replace(":", "_").Replace(".", "_");
            List<ScheduleTaskAddEditByTabRSModel> RSData = new List<ScheduleTaskAddEditByTabRSModel>();
            ScheduleTaskAddEditByTabRSModel ListHeader = new ScheduleTaskAddEditByTabRSModel();
            List<ScheduleTaskAddEditByTabRSList> DataList = new List<ScheduleTaskAddEditByTabRSList>();
            string TaskIDRS = "", ClientID=""; 
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
                List<ScheduleTaskAddEditByTabRQ> jObj = null;
                if (ERR != "Error")
                {
                    try
                    {
                        jObj = JsonConvert.DeserializeObject<List<ScheduleTaskAddEditByTabRQ>>(json);
                    }
                    catch
                    {
                        ERR = "Error";
                        SMS = "Invalid JSON";
                    }
                }
                #endregion
                #region data
                if (ERR != "Error")
                {
                    #region cn
                    SqlConnection Con1 = new SqlConnection(c.ConStr());
                    Con1.Open();
                    SqlCommand Com1 = new SqlCommand();
                    Com1.Connection = Con1;
                    #endregion
                    try
                    {             
                        for(int i = 0; i < jObj.Count; i++) {
                            #region para
                            string sql = "exec sp_ScheduleTaskAddEdit @Action=@Action,@TaskID=@TaskID,@NewTitle=@NewTitle,@NewDescription=@NewDescription"
                            + ",@NewPlanDateStart=@NewPlanDateStart,@NewPlanTimeStart=@NewPlanTimeStart,@NewPlanDateEnd=@NewPlanDateEnd"
                            + ",@NewPlanTimeEnd=@NewPlanTimeEnd,@NewActualDateStart=@NewActualDateStart,@NewActualTimeStart=@NewActualTimeStart"
                            + ",@NewActualDateEnd=@NewActualDateEnd,@NewActualTimeEnd=@NewActualTimeEnd,@NewOwnerUserID=@NewOwnerUserID"
                            + ",@NewOwnerUserText=@NewOwnerUserText,@NewTaskTypeID=@NewTaskTypeID,@NewTaskStatusID=@NewTaskStatusID"
                            + ",@NewRemark=@NewRemark,@NewCreateDate=@NewCreateDate,@NewCreateByUserID=@NewCreateByUserID";

                        Com1.CommandText = sql;
                        Com1.Parameters.Clear();
                        Com1.Parameters.AddWithValue("@Action", jObj[i].Action);
                        Com1.Parameters.AddWithValue("@TaskID", jObj[i].TaskID);
                        Com1.Parameters.AddWithValue("@NewTitle", jObj[i].Title);
                        Com1.Parameters.AddWithValue("@NewDescription", jObj[i].Description);
                        Com1.Parameters.AddWithValue("@NewPlanDateStart", jObj[i].PlanDateStart);
                        Com1.Parameters.AddWithValue("@NewPlanTimeStart", jObj[i].PlanTimeStart);
                        Com1.Parameters.AddWithValue("@NewPlanDateEnd", jObj[i].PlanDateEnd);
                        Com1.Parameters.AddWithValue("@NewPlanTimeEnd", jObj[i].PlanTimeEnd);
                        Com1.Parameters.AddWithValue("@NewActualDateStart", jObj[i].ActualDateStart);
                        Com1.Parameters.AddWithValue("@NewActualTimeStart", jObj[i].ActualTimeStart);
                        Com1.Parameters.AddWithValue("@NewActualDateEnd", jObj[i].ActualDateEnd);
                        Com1.Parameters.AddWithValue("@NewActualTimeEnd", jObj[i].ActualTimeEnd);
                        Com1.Parameters.AddWithValue("@NewOwnerUserID", jObj[i].OwnerUserID);
                        Com1.Parameters.AddWithValue("@NewOwnerUserText", jObj[i].OwnerUserText);
                        Com1.Parameters.AddWithValue("@NewTaskTypeID", jObj[i].TaskTypeID);
                        Com1.Parameters.AddWithValue("@NewTaskStatusID", jObj[i].TaskStatusID);
                        Com1.Parameters.AddWithValue("@NewRemark", jObj[i].Remark);
                        Com1.Parameters.AddWithValue("@NewCreateDate", jObj[i].CreateDate);
                        Com1.Parameters.AddWithValue("@NewCreateByUserID", UserID);
                            #endregion
                            #region exec to db
                            DataTable dt1 = new DataTable();
                        dt1.Load(Com1.ExecuteReader());
                        String ERR_d = dt1.Rows[0]["ERR"].ToString();
                        String SMS_d = dt1.Rows[0]["SMS"].ToString();
                        TaskIDRS = dt1.Rows[0]["TaskIDRS"].ToString();
                            #endregion

                            ScheduleTaskAddEditByTabRSList data = new ScheduleTaskAddEditByTabRSList();
                            data.ClientID = jObj[i].ClientID;
                            data.TaskID = TaskIDRS;
                            data.ERR = ERR_d;
                            data.SMS = SMS_d;
                            DataList.Add(data);
                        }
                        ERR = "Succeed";

                    } catch (Exception ex) {
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
            
            string RSDataStr = "";
            try
            {
                ListHeader.ERR = ERR;
                ListHeader.SMS = SMS;
                ListHeader.ERRCode = ERRCode;
                ListHeader.DataList = DataList;
                RSData.Add(ListHeader);

                var jsonRS = new JavaScriptSerializer().Serialize(RSData);
                RSDataStr = c.Encrypt(jsonRS, c.SeekKeyGet());
                c.T24_AddLog(FileNameForLog, "RS", jsonRS.ToString(), ControllerName);
            }
            catch { }
            return RSDataStr;
        }

    }
    public class ScheduleTaskAddEditByTabRQ
    {
        public string Action { get; set; }
        public string TaskID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PlanDateStart { get; set; }
        public string PlanTimeStart { get; set; }
        public string PlanDateEnd { get; set; }
        public string PlanTimeEnd { get; set; }
        public string ActualDateStart { get; set; }
        public string ActualTimeStart { get; set; }
        public string ActualDateEnd { get; set; }
        public string ActualTimeEnd { get; set; }
        public string OwnerUserID { get; set; }
        public string OwnerUserText { get; set; }
        public string TaskTypeID { get; set; }
        public string TaskStatusID { get; set; }
        public string Remark { get; set; }
        public string CreateDate { get; set; }
        public string ClientID { get; set; }
    }
    public class ScheduleTaskAddEditByTabRSModel
    {
        public string ERR { get; set; }//Error | succeed
        public string SMS { get; set; }
        public string ERRCode { get; set; }
        public List<ScheduleTaskAddEditByTabRSList> DataList { get; set; }
    }
    public class ScheduleTaskAddEditByTabRSList
    {
        public string TaskID { get; set; }
        public string ClientID { get; set; }
        public string ERR { get; set; }
        public string SMS { get; set; } 
    }

}