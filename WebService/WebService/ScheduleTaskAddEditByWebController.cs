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
    public class ScheduleTaskAddEditByWebController : ApiController
    {
        public string Post([FromUri] string api_name, string api_key, string username, [FromBody] string json)
        {//json={"Action": "1","TaskID": "10","Title": "Coll-VB Ou ton 1","Description": "today is holiday","PlanDateStart": "2021-06-10","PlanTimeStart": "01: 38: 54","PlanDateEnd": "2021-06-30",
         // "PlanTimeEnd": "02: 38: 54","ActualDateStart": "2021-07-01","ActualTimeStart": "03: 38: 54","ActualDateEnd": "2021-07-20","ActualTimeEnd": "04:38:54","OwnerUserID": "5935",
         //"OwnerUserText": "AAA","TaskTypeID": "1","TaskStatusID": "1","Remark": "AAA","CreateDate": "","CreateByUserID":"2465"} | Action=1(Add),Action=2(Edit)
            string msgid = username;
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "", ExSMS = "", ERRCode = "";
            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string ControllerName = "ScheduleTaskAddEditByWeb";
            string FileNameForLog = msgid + "_" + ServerDate.Replace("-", "_").Replace(" ", "_").Replace(":", "_").Replace(".", "_");
            List<ScheduleTaskAddEditByWebRSModel> RSData = new List<ScheduleTaskAddEditByWebRSModel>();
            ScheduleTaskAddEditByWebRSModel ListHeader = new ScheduleTaskAddEditByWebRSModel();
            List<ScheduleTaskAddEditByWebRSList> DataList = new List<ScheduleTaskAddEditByWebRSList>();
            string TaskIDRS = "";
            try
            {
                #region msgid
                if (ERR != "Error")
                {
                    //string[] str = c.CheckMsgID(msgid);
                    //ERR = str[0];
                    //SMS = str[1];
                }
                #endregion
                #region add log
                if (ERR != "Error")
                {
                    c.T24_AddLog(FileNameForLog, "1.RQ", "api_name:" + api_name + " | api_key:" + api_key + " | json:" + json, ControllerName);
                }
                #endregion
                #region p -> SessionID
                string UserID = "";
                if (ERR != "Error")
                {
                    ////p = System.Web.HttpUtility.UrlDecode(p);
                    //string[] rs = c.SessionIDCheck(ServerDate, p);
                    //ERR = rs[0];
                    //SMS = rs[1];
                    //ExSMS = rs[2];
                    //UserID = rs[3];
                    //ERRCode = rs[4];
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
                ScheduleTaskAddEditByWebRQ jObj = null;
                if (ERR != "Error")
                {
                    try
                    {
                        jObj = JsonConvert.DeserializeObject<ScheduleTaskAddEditByWebRQ>(json);
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
                    SqlConnection Con1 = new SqlConnection(c.ConStr());
                    Con1.Open();
                    SqlCommand Com1 = new SqlCommand();
                    Com1.Connection = Con1;

                    try
                    {
                        string sql = "exec sp_ScheduleTaskAddEdit @Action=@Action,@TaskID=@TaskID,@NewTitle=@NewTitle,@NewDescription=@NewDescription"
                            + ",@NewPlanDateStart=@NewPlanDateStart,@NewPlanTimeStart=@NewPlanTimeStart,@NewPlanDateEnd=@NewPlanDateEnd"
                            + ",@NewPlanTimeEnd=@NewPlanTimeEnd,@NewActualDateStart=@NewActualDateStart,@NewActualTimeStart=@NewActualTimeStart"
                            + ",@NewActualDateEnd=@NewActualDateEnd,@NewActualTimeEnd=@NewActualTimeEnd,@NewOwnerUserID=@NewOwnerUserID"
                            + ",@NewOwnerUserText=@NewOwnerUserText,@NewTaskTypeID=@NewTaskTypeID,@NewTaskStatusID=@NewTaskStatusID"
                            + ",@NewRemark=@NewRemark,@NewCreateDate=@NewCreateDate,@NewCreateByUserID=@NewCreateByUserID";

                        Com1.CommandText = sql;
                        Com1.Parameters.Clear();
                        Com1.Parameters.AddWithValue("@Action", jObj.Action);
                        Com1.Parameters.AddWithValue("@TaskID", jObj.TaskID);
                        Com1.Parameters.AddWithValue("@NewTitle", jObj.Title);
                        Com1.Parameters.AddWithValue("@NewDescription", jObj.Description);
                        Com1.Parameters.AddWithValue("@NewPlanDateStart", jObj.PlanDateStart);
                        Com1.Parameters.AddWithValue("@NewPlanTimeStart", jObj.PlanTimeStart);
                        Com1.Parameters.AddWithValue("@NewPlanDateEnd", jObj.PlanDateEnd);
                        Com1.Parameters.AddWithValue("@NewPlanTimeEnd", jObj.PlanTimeEnd);
                        Com1.Parameters.AddWithValue("@NewActualDateStart", jObj.ActualDateStart);
                        Com1.Parameters.AddWithValue("@NewActualTimeStart", jObj.ActualTimeStart);
                        Com1.Parameters.AddWithValue("@NewActualDateEnd", jObj.ActualDateEnd);
                        Com1.Parameters.AddWithValue("@NewActualTimeEnd", jObj.ActualTimeEnd);
                        Com1.Parameters.AddWithValue("@NewOwnerUserID", jObj.OwnerUserID);
                        Com1.Parameters.AddWithValue("@NewOwnerUserText", jObj.OwnerUserText);
                        Com1.Parameters.AddWithValue("@NewTaskTypeID", jObj.TaskTypeID);
                        Com1.Parameters.AddWithValue("@NewTaskStatusID", jObj.TaskStatusID);
                        Com1.Parameters.AddWithValue("@NewRemark", jObj.Remark);
                        Com1.Parameters.AddWithValue("@NewCreateDate", jObj.CreateDate);
                        Com1.Parameters.AddWithValue("@NewCreateByUserID", jObj.CreateByUserID);
                        

                        DataTable dt1 = new DataTable();
                        dt1.Load(Com1.ExecuteReader());
                        ERR = dt1.Rows[0]["ERR"].ToString();
                        SMS = dt1.Rows[0]["SMS"].ToString();
                        TaskIDRS = dt1.Rows[0]["TaskIDRS"].ToString();
                        

                        ScheduleTaskAddEditByWebRSList data = new ScheduleTaskAddEditByWebRSList();
                        data.TaskID = TaskIDRS;
                        DataList.Add(data);

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
    public class ScheduleTaskAddEditByWebRQ
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
        public string CreateByUserID { get; set; }
        
    }
    public class ScheduleTaskAddEditByWebRSModel
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
        public string ERRCode { get; set; }
        public List<ScheduleTaskAddEditByWebRSList> DataList { get; set; }
    }
    public class ScheduleTaskAddEditByWebRSList
    {
        public string TaskID { get; set; }
       
    }

}