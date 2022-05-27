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
    public class ScheduleTaskGetByTabController : ApiController
    {
        //public IEnumerable<ProspectGetV2RSModel> Post([FromUri]string api_name, string api_key,string username, [FromBody]string json)
        public string Post([FromUri] string p, [FromUri] string msgid, [FromBody] string json)// {"LatestDateTime":"yyyy-MM-dd HH:mm:ss.fff"}
        {
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "", ExSMS = "", ERRCode = "";
            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string ControllerName = "ScheduleTaskGetByTab";
            string FileNameForLog = msgid + "_" + ServerDate.Replace("-", "_").Replace(" ", "_").Replace(":", "_").Replace(".", "_");
            List<SchduleTaskGetByTabRSModel> RSData = new List<SchduleTaskGetByTabRSModel>();
            SchduleTaskGetByTabRSModel ListHeader = new SchduleTaskGetByTabRSModel();
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
                    c.T24_AddLog(FileNameForLog, "1.RQ-p", p, ControllerName);
                    c.T24_AddLog(FileNameForLog, "1.RQ-json", json, ControllerName);
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
                SchduleTaskGetByTabRQ jObj = null;
                string LatestDateTime = "";
                if (ERR != "Error")
                {
                    try
                    {
                        jObj = JsonConvert.DeserializeObject<SchduleTaskGetByTabRQ>(json);
                        LatestDateTime = jObj.LatestDateTime;
                    }
                    catch (Exception e)
                    {
                        ERR = "Error";
                        SMS = "Invalid JSON";
                        Console.WriteLine("Task Error" + e.Message);
                    }
                }
                #endregion
                #region data
                if (ERR != "Error")
                {

                    List<SchduleTaskGetByTabRSList> DataList = new List<SchduleTaskGetByTabRSList>();
                    string sql = "exec sp_ScheduleTaskGetFromTab @UserID='" + UserID + "',@LatestDateTime='"+ LatestDateTime + "'";
                    DataTable dt = c.ReturnDT(sql);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        #region params
                        SchduleTaskGetByTabRSList data = new SchduleTaskGetByTabRSList();
                        data.TaskID = dt.Rows[i]["TaskID"].ToString();
                        data.Title = dt.Rows[i]["Title"].ToString();
                        data.Description = dt.Rows[i]["Description"].ToString();
                        data.PlanDateStart = dt.Rows[i]["PlanDateStart"].ToString();
                        data.PlanTimeStart = dt.Rows[i]["PlanTimeStart"].ToString();
                        data.PlanDateEnd = dt.Rows[i]["PlanDateEnd"].ToString();
                        data.PlanTimeEnd = dt.Rows[i]["PlanTimeEnd"].ToString();
                        data.ActualDateStart = dt.Rows[i]["ActualDateStart"].ToString();
                        data.ActualTimeStart = dt.Rows[i]["ActualTimeStart"].ToString();
                        data.ActualDateEnd = dt.Rows[i]["ActualDateEnd"].ToString();
                        data.ActualTimeEnd = dt.Rows[i]["ActualTimeEnd"].ToString();
                        data.OwnerUserID = dt.Rows[i]["OwnerUserID"].ToString();
                        data.OwnerUserText = dt.Rows[i]["OwnerUserText"].ToString();
                        data.TaskTypeID = dt.Rows[i]["TaskTypeID"].ToString();
                        data.TaskStatusID = dt.Rows[i]["TaskStatusID"].ToString();
                        data.Remark = dt.Rows[i]["Remark"].ToString();
                        data.LatestDateTime = dt.Rows[i]["LatestDateTime"].ToString();
                   

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
                Console.WriteLine("Task Error" + ExSMS);
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

    public class SchduleTaskGetByTabRQ
    {
        public string LatestDateTime { get; set; }
    }
    public class SchduleTaskGetByTabRSModel
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
        public string ERRCode { get; set; }
        public List<SchduleTaskGetByTabRSList> DataList { get; set; }
    }
    public class SchduleTaskGetByTabRSList
    {
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
        public string LatestDateTime { get; set; }
     
    }

}