using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using WebService.Helpers;
using WebService.Models.Req.Schedules;
using WebService.Models.Res.Schedules;

namespace WebService.Repositories
{
    public class ScheduleTaskRepository
    {
        private readonly AppDbContext c = new AppDbContext();
        public string GetScheduleTaskByTaskTypeAndTaskStatus(ScheduleTaskByWebReq req)
        {
            string msgid = req.Username;
            string ERR = "Succeed", SMS = "", ExSMS = "", ERRCode = "";
            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string ControllerName = "ScheduleTaskTypeAndStatusGetByWeb";
            string FileNameForLog = msgid + "_" + ServerDate.Replace("-", "_").Replace(" ", "_").Replace(":", "_").Replace(".", "_");
            List<SchduleTaskStatusAndTypeRes> RSData = new List<SchduleTaskStatusAndTypeRes>();
            SchduleTaskStatusAndTypeRes ListHeader = new SchduleTaskStatusAndTypeRes();

            try
            {
                var schedule = new SchduleTaskStatusAndTypeReq();
                #region check json
                if (ERR != "Error")
                {
                    string[] str = c.CheckObjED(req.JsonFormat, "2");
                    ERR = str[0];
                    SMS = str[1];
                    ExSMS = str[2];
                    schedule = JsonConvert.DeserializeObject<SchduleTaskStatusAndTypeReq>(str[3]);
                }
                #endregion check json
                #region add log
                if (ERR != "Error")
                {
                    c.T24_AddLog(FileNameForLog, "1.RQ", "api_name:" + req.ApiName + " | api_key:" + req.ApiKey + " | json:" + req, ControllerName);
                }
                #endregion
                #region data

                if (ERR != "Error")
                {
                    var DataList = new List<SchduleTaskStatusAndTypeDetail>();

                    string sql = "exec sp_ScheduleTask @Type='" + schedule.TYPE + "'";
                    DataTable dt = c.ReturnDT(sql);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        #region params
                        var data = new SchduleTaskStatusAndTypeDetail();
                        data.ID = dt.Rows[i]["ID"].ToString();
                        data.DESCRIPTION = dt.Rows[i]["DESCRIPTION"].ToString();
                        data.TYPE = schedule.TYPE;
                        DataList.Add(data);
                        #endregion params
                    }
                    ListHeader.DataDetail = DataList;

                }
                #endregion
            }
            catch (Exception ex)
            {
                ERR = "Error";
                SMS = "Something was wrong :" + ex.Message.ToString();
            }

            ListHeader.ERR = ERR;
            ListHeader.SMS = SMS;
            ListHeader.ERRCode = ERRCode;
            RSData.Add(ListHeader);

            var jsonRS = new JavaScriptSerializer().Serialize(RSData);
            var RSDataStr = Cryptography.Encrypt(jsonRS, Cryptography.SeekKeyGet());
            c.T24_AddLog(FileNameForLog, "RS", jsonRS.ToString(), ControllerName);

            return RSDataStr;
        }
        public string  AddAndEditScheduleTaskByTab(ScheduleTaskByTabReq req)
        {
            string ERR = "Succeed", SMS = "", ExSMS = "", ERRCode = "";
            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string ControllerName = "ScheduleTaskAddEditByTab";
            string FileNameForLog = req.MsgId + "_" + ServerDate.Replace("-", "_").Replace(" ", "_").Replace(":", "_").Replace(".", "_");
            List<ScheduleTaskAddEditByTabRSModel> RSData = new List<ScheduleTaskAddEditByTabRSModel>();
            ScheduleTaskAddEditByTabRSModel ListHeader = new ScheduleTaskAddEditByTabRSModel();
            List<ScheduleTaskAddEditByTabRSList> DataList = new List<ScheduleTaskAddEditByTabRSList>();
            string TaskIDRS = "", ClientID = "";
            try
            {
                #region msgid
                if (ERR != "Error")
                {
                    string[] str = c.CheckMsgID(req.MsgId);
                    ERR = str[0];
                    SMS = str[1];
                }
                #endregion
                #region add log
                if (ERR != "Error")
                {
                    c.T24_AddLog(FileNameForLog, "1.RQ", req.Token, ControllerName);
                }
                #endregion
                #region p -> SessionID
                string UserID = "";
                if (ERR != "Error")
                {
                    //p = System.Web.HttpUtility.UrlDecode(p);
                    string[] rs = c.SessionIDCheck(ServerDate, req.Token);
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
                    string[] str = c.CheckObjED(req.JsonFormat, "2");
                    ERR = str[0];
                    SMS = str[1];
                    ExSMS = str[2];
                    req.JsonFormat = str[3];
                }
                #endregion check json
                #region read json
                List<ScheduleTaskTabReq> jObj = null;
                if (ERR != "Error")
                {
                    try
                    {
                        jObj = JsonConvert.DeserializeObject<List<ScheduleTaskTabReq>>(req.JsonFormat);
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
                    SqlConnection Con1 = new SqlConnection(AppConfig.ConStr());
                    Con1.Open();
                    SqlCommand Com1 = new SqlCommand();
                    Com1.Connection = Con1;
                    try
                    {
                        for (int i = 0; i < jObj.Count; i++)
                        {
                            string sql = "exec sp_ScheduleTaskAddEdit @Action=@Action,@TaskID=@TaskID,@NewTitle=@NewTitle,@NewDescription=@NewDescription"
                                + ",@NewPlanDateStart=@NewPlanDateStart,@NewPlanTimeStart=@NewPlanTimeStart,@NewPlanDateEnd=@NewPlanDateEnd"
                                + ",@NewPlanTimeEnd=@NewPlanTimeEnd,@NewActualDateStart=@NewActualDateStart,@NewActualTimeStart=@NewActualTimeStart"
                                + ",@NewActualDateEnd=@NewActualDateEnd,@NewActualTimeEnd=@NewActualTimeEnd,@NewOwnerUserID=@NewOwnerUserID"
                                + ",@NewOwnerUserText=@NewOwnerUserText,@NewTaskTypeID=@NewTaskTypeID,@NewTaskStatusID=@NewTaskStatusID"
                                + ",@NewRemark=@NewRemark,@NewCreateDate=@NewCreateDate,@NewCreateByUserID=@NewCreateByUserID,@NewClientID=@NewClientID";

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
                            Com1.Parameters.AddWithValue("@NewClientID", jObj[i].ClientID);

                            DataTable dt1 = new DataTable();
                            dt1.Load(Com1.ExecuteReader());
                            String ERR_d = dt1.Rows[0]["ERR"].ToString();
                            String SMS_d = dt1.Rows[0]["SMS"].ToString();
                            TaskIDRS = dt1.Rows[0]["TaskIDRS"].ToString();
                            ClientID = dt1.Rows[0]["ClientID"].ToString();


                            ScheduleTaskAddEditByTabRSList data = new ScheduleTaskAddEditByTabRSList();
                            data.TaskID = TaskIDRS;
                            data.ClientID = ClientID;
                            data.ERR = ERR_d;
                            data.SMS = SMS_d;
                            DataList.Add(data);
                        }
                        ERR = "Succeed";

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
                RSDataStr = Cryptography.Encrypt(jsonRS, Cryptography.SeekKeyGet());
                c.T24_AddLog(FileNameForLog, "RS", jsonRS.ToString(), ControllerName);
            }
            catch { }
            return RSDataStr;
        }
        public string AddAndEditScheduleTaskByWeb(ScheduleTaskByWebReq req)
        {
            string msgid = req.Username;
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
                    c.T24_AddLog(FileNameForLog, "1.RQ", "api_name:" + req.ApiName + " | api_key:" + req.ApiKey + " | json:" + req.JsonFormat, ControllerName);
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
                    string[] str = c.CheckObjED(req.JsonFormat, "2");
                    ERR = str[0];
                    SMS = str[1];
                    ExSMS = str[2];
                    req.JsonFormat = str[3];
                }
                #endregion check json
                #region read json
                ScheduleTaskAddEditByWebRQ jObj = null;
                if (ERR != "Error")
                {
                    try
                    {
                        jObj = JsonConvert.DeserializeObject<ScheduleTaskAddEditByWebRQ>(req.JsonFormat);
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
                    SqlConnection Con1 = new SqlConnection(AppConfig.ConStr());
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
                        Com1.Parameters.AddWithValue("@NewTaskTypeID", jObj.TaskStatusID);
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
                RSDataStr = Cryptography.Encrypt(jsonRS, Cryptography.SeekKeyGet());
                c.T24_AddLog(FileNameForLog, "RS", jsonRS.ToString(), ControllerName);
            }
            catch { }
            return RSDataStr;
        }
        public string GetScheduleTaskByTab(ScheduleTaskByTabReq req)
        {
            string ERR = "Succeed", SMS = "", ExSMS = "", ERRCode = "";
            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string ControllerName = "ScheduleTaskGetByTab";
            string FileNameForLog = req.MsgId + "_" + ServerDate.Replace("-", "_").Replace(" ", "_").Replace(":", "_").Replace(".", "_");
            List<SchduleTaskGetByTabRSModel> RSData = new List<SchduleTaskGetByTabRSModel>();
            SchduleTaskGetByTabRSModel ListHeader = new SchduleTaskGetByTabRSModel();
            try
            {
                #region msgid
                if (ERR != "Error")
                {
                    string[] str = c.CheckMsgID(req.MsgId);
                    ERR = str[0];
                    SMS = str[1];
                }
                #endregion
                #region add log
                if (ERR != "Error")
                {
                    c.T24_AddLog(FileNameForLog, "1.RQ", req.Token, ControllerName);
                }
                #endregion
                #region p -> SessionID
                string UserID = "";
                if (ERR != "Error")
                {
                    //p = System.Web.HttpUtility.UrlDecode(p);
                    string[] rs = c.SessionIDCheck(ServerDate, req.Token);
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
                    string[] str = c.CheckObjED(req.JsonFormat, "2");
                    ERR = str[0];
                    SMS = str[1];
                    ExSMS = str[2];
                    req.JsonFormat = str[3];
                }
                #endregion check json
                #region read json
                SchduleTaskGetByTabRQ jObj = null;
                string LatestDateTime = "";
                if (ERR != "Error")
                {
                    try
                    {
                        jObj = JsonConvert.DeserializeObject<SchduleTaskGetByTabRQ>(req.JsonFormat);
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
                    string sql = "exec sp_ScheduleTaskGetFromTab @UserID='" + UserID + "',@LatestDateTime='" + LatestDateTime + "'";
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
                RSDataStr = Cryptography.Encrypt(jsonRS, Cryptography.SeekKeyGet());
                c.T24_AddLog(FileNameForLog, "RS", jsonRS.ToString(), ControllerName);
            }
            catch { }
            return RSDataStr;

        }
        public string GetScheduleTaskByWeb(ScheduleTaskByWebReq req)
        {
            string msgid = req.Username;
            string ERR = "Succeed", SMS = "", ExSMS = "", ERRCode = "";
            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string ControllerName = "GetScheduleTaskByWeb";
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
                    c.T24_AddLog(FileNameForLog, "1.RQ", "api_name:" + req.ApiName + " | api_key:" + req.ApiKey + " | json:" + req.JsonFormat, ControllerName);
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
                    string[] str = c.CheckObjED(req.JsonFormat, "2");
                    ERR = str[0];
                    SMS = str[1];
                    ExSMS = str[2];
                    req.JsonFormat = str[3];
                }
                #endregion check json
                #region read json
                SchduleTaskGetByWebRQ jObj = null;
                string StartDate = "", EndDate = "", UserOwnerID = "";
                if (ERR != "Error")
                {
                    try
                    {
                        jObj = JsonConvert.DeserializeObject<SchduleTaskGetByWebRQ>(req.JsonFormat);
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
                    string sql = "exec [dbo].[sp_ScheduleTaskGetByWeb]  @UserOwnerID='" + UserOwnerID  + "',@StartDate='" + StartDate + "',@EndDate='" + EndDate + "'";
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
                RSDataStr = Cryptography.Encrypt(jsonRS, Cryptography.SeekKeyGet());
                c.T24_AddLog(FileNameForLog, "RS", jsonRS.ToString(), ControllerName);
            }
            catch { }
            return RSDataStr;
        }
        public string GetReportByWeb(ScheduleTaskByWebReq req)
        {
            string msgid = req.Username;
            string ERR = "Succeed", SMS = "", ExSMS = "", ERRCode = "";
            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string ControllerName = "GetReportByWeb";
            string FileNameForLog = msgid + "_" + ServerDate.Replace("-", "_").Replace(" ", "_").Replace(":", "_").Replace(".", "_");
            List<ReportCOByWebRSModel> RSData = new List<ReportCOByWebRSModel>();
            ReportCOByWebRSModel ListHeader = new ReportCOByWebRSModel();
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
                    c.T24_AddLog(FileNameForLog, "1.RQ", "api_name:" + req.ApiName + " | api_key:" + req.ApiKey + " | json:" + req.JsonFormat, ControllerName);
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
                    string[] str = c.CheckObjED(req.JsonFormat, "2");
                    ERR = str[0];
                    SMS = str[1];
                    ExSMS = str[2];
                    req.JsonFormat = str[3];
                }
                #endregion check json
                #region read json
                ReportCOByWebRQ jObj = null;
                string StartDate = "", EndDate = "", UserOwnerID = "", OwnerType = "", PMuserID="", BMUserID="", AMUserID="";
                if (ERR != "Error")
                {
                    try
                    {
                        jObj = JsonConvert.DeserializeObject<ReportCOByWebRQ>(req.JsonFormat);
                        StartDate = jObj.StartDate;
                        EndDate = jObj.EndDate;
                        UserOwnerID = jObj.UserOwnerID;
                        OwnerType = jObj.OwnerType;
                        PMuserID = jObj.PMuserID;
                        BMUserID = jObj.BMUserID;
                        AMUserID = jObj.AMUserID;
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
                    List<ReportCOByWebRSList> DataList = new List<ReportCOByWebRSList>();
                    string sql = "exec sp_ScheduleTaskByWeb   @UserOwnerID='" + UserOwnerID + "',@OwnerType='" + OwnerType + "',@StartDate='" + StartDate + "',@EndDate='" + 
                        EndDate+ "',@PMuserID='"+ PMuserID+ "',@BMUserID='" + BMUserID + "',@AMUserID='" + AMUserID + "'";
                    DataTable dt = c.ReturnDT(sql);
                    string prospact = "Prospect On Board";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        
                        #region params
                        ReportCOByWebRSList data = new ReportCOByWebRSList();
                        var titles = dt.Rows[i]["Title"].ToString().Split('-');
                        if (titles[0].Trim() == prospact.Trim())
                        {
                            data.AssignedBy = dt.Rows[i]["AssignedBy"].ToString();
                            data.AssignedByName = "System";
                        }
                        else
                        {
                            data.AssignedBy = dt.Rows[i]["AssignedBy"].ToString();
                            data.AssignedByName = dt.Rows[i]["AssignedByName"].ToString();
                        }

                        data.TaskID = dt.Rows[i]["TaskID"].ToString();
                        data.COID = dt.Rows[i]["COCode"].ToString();
                        data.COName = dt.Rows[i]["COName"].ToString();
                        data.Title = dt.Rows[i]["Title"].ToString();
                        data.Description = dt.Rows[i]["Description"].ToString();
                        data.TaskTypeID = dt.Rows[i]["TaskTypeID"].ToString();
                        data.TaskType = dt.Rows[i]["TaskType"].ToString();;
                        data.PlanDateStart = dt.Rows[i]["PlanDateStart"].ToString();
                        data.PlanDateEnd = dt.Rows[i]["PlanDateEnd"].ToString();
                        data.ActualDateStart = dt.Rows[i]["ActualDateStart"].ToString();
                        data.ActualDateEnd = dt.Rows[i]["ActualDateEnd"].ToString();
                        data.TaskStatusID = dt.Rows[i]["TaskStatusID"].ToString();
                        data.Remark = dt.Rows[i]["Remark"].ToString();
                        data.TaskStatus = dt.Rows[i]["TaskStatus"].ToString();
                       
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
                RSDataStr = Cryptography.Encrypt(jsonRS, Cryptography.SeekKeyGet());
                c.T24_AddLog(FileNameForLog, "RS", jsonRS.ToString(), ControllerName);
            }
            catch { }
            return RSDataStr;
        }
    }
}