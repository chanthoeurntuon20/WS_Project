using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;

namespace WebService
{
    [BasicAuthentication]
    public class ScheduleTaskGetByWebController : ApiController
    {
        public string Post([FromUri]string api_name, string api_key, string username, [FromBody]string json)//{"UserOwnerID":"5935","StartDate":"2020-01-10","EndDate":"2021-05-20"}
        {
            string msgid = username;
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "", ExSMS = "", ERRCode = "";
            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string ControllerName = "ScheduleTaskGetByWeb";
            string FileNameForLog = msgid + "_" + ServerDate.Replace("-", "_").Replace(" ", "_").Replace(":", "_").Replace(".", "_");
            List<SchduleTaskGetByWebRSModel> RSData = new List<SchduleTaskGetByWebRSModel>();
            SchduleTaskGetByWebRSModel ListHeader = new SchduleTaskGetByWebRSModel();
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
                    c.T24_AddLog(FileNameForLog, "1.RQ", "api_name:"+ api_name+ " | api_key:"+ api_key+ " | json:"+ json, ControllerName);
                }
                #endregion
                #region p -> SessionID
                //string UserID = "";
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
                SchduleTaskGetByWebRQ jObj = null;
                string StartDate = "", EndDate = "", UserOwnerID = "";
                if (ERR != "Error")
                {
                    try
                    {
                        jObj = JsonConvert.DeserializeObject<SchduleTaskGetByWebRQ>(json);
                        StartDate = jObj.StartDate;
                        EndDate = jObj.EndDate;
                        UserOwnerID = jObj.UserOwnerID;

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


                    List<SchduleTaskGetByWebRSList> DataList = new List<SchduleTaskGetByWebRSList>();
                    string sql = "exec [dbo].[sp_ScheduleTaskGetByWeb]  @UserOwnerID='" + UserOwnerID + "',@StartDate='" + StartDate + "',@EndDate='" + EndDate + "'";
                    DataTable dt = c.ReturnDT(sql);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        #region params
                        SchduleTaskGetByWebRSList data = new SchduleTaskGetByWebRSList();
                        data.TaskID = dt.Rows[i]["TaskID"].ToString();
                        data.Title = dt.Rows[i]["Title"].ToString();
                        data.Description = dt.Rows[i]["Description"].ToString();
                        data.PlanDateStart = dt.Rows[i]["PlanDateStart"].ToString();
                        data.PlanDateEnd = dt.Rows[i]["PlanDateEnd"].ToString();
                        data.ActualDateStart = dt.Rows[i]["ActualDateStart"].ToString();
                        data.ActualDateEnd = dt.Rows[i]["ActualDateEnd"].ToString();
                        data.OwnerUserID = dt.Rows[i]["OwnerUserId"].ToString();
                        data.OwnerUserText = dt.Rows[i]["OwnerUserText"].ToString();
                        data.TaskTypeID = dt.Rows[i]["TaskTypeID"].ToString();
                        data.TaskStatusID = dt.Rows[i]["TaskStatusID"].ToString();
                        data.Remark = dt.Rows[i]["Remark"].ToString();
                        data.CreateDate = dt.Rows[i]["CreateDate"].ToString();

                        DataList.Add(data);
                        #endregion params
                    }
                    ListHeader.DataList = DataList;

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
                RSData.Add(ListHeader);

                var jsonRS = new JavaScriptSerializer().Serialize(RSData);
                RSDataStr = c.Encrypt(jsonRS, c.SeekKeyGet());
                c.T24_AddLog(FileNameForLog, "RS", jsonRS.ToString(), ControllerName);
            }
            catch { }
            return RSDataStr;
        }

    }
    public class SchduleTaskGetByWebRQ
    {
        public string UserOwnerID { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
    public class SchduleTaskGetByWebRSModel
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
        public string ERRCode { get; set; }
        public List<SchduleTaskGetByWebRSList> DataList { get; set; }
    }
    public class SchduleTaskGetByWebRSList
    {
        public string TaskID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PlanDateStart { get; set; }
        public string PlanDateEnd { get; set; }
        public string ActualDateStart { get; set; }
        public string ActualDateEnd { get; set; }
        public string OwnerUserID { get; set; }
        public string OwnerUserText { get; set; }
        public string TaskTypeID { get; set; }
        public string TaskStatusID { get; set; }
        public string Remark { get; set; }
        public string CreateDate { get; set; }
        
       
    }

}