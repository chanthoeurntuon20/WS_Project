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
using WebService.Models.Req.Schedules;
using WebService.Models.Res.Schedules;
using WebService.Repositories;

namespace WebService.ApiControllers.ScheduleTask
{
    [BasicAuthentication]
    public class ScheduleTaskController : ApiController
    {
       private readonly ScheduleTaskRepository schedule = new ScheduleTaskRepository();
        // POST api/<controller>
        [Route("api/v1/schedule/task-with-status")]
        [HttpPost()]
        public string ScheduleTaskTypeAndStatus([FromUri] string api_name, string api_key, string username, [FromBody] string json)//{"Type":"TaskType"}
        {
            try
            {
                ScheduleTaskByWebReq req = new ScheduleTaskByWebReq();
                req.ApiName = api_name;
                req.ApiKey = api_key;
                req.Username = username;
                req.JsonFormat = json;
                return schedule.GetScheduleTaskByTaskTypeAndTaskStatus(req);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        
        [Route("api/v1/schedule/tablet/addOredit")]
        [HttpPost()]
        public string ScheduleTaskAddEditByTab([FromUri] string p, [FromUri] string msgid, [FromBody] string json)
        {
            try
            {
                ScheduleTaskByTabReq req = new ScheduleTaskByTabReq();
                req.Token = p;
                req.MsgId = msgid;
                req.JsonFormat = json;

                return schedule.AddAndEditScheduleTaskByTab(req);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        [Route("api/v1/schedule/web/addOredit")]
        [HttpPost()]
        public string ScheduleTaskAddEditByWeb([FromUri] string api_name, string api_key, string username, [FromBody] string json)
        {
            try
            {
                ScheduleTaskByWebReq req = new ScheduleTaskByWebReq();
                req.ApiName = api_name;
                req.ApiKey = api_key;
                req.Username = username;
                req.JsonFormat = json;
                return schedule.AddAndEditScheduleTaskByWeb(req);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
           
        }

        [Route("api/v1/schedule/tasks/tablet")]
        [HttpPost()]
        public string ScheduleTaskGetByTab([FromUri] string p, [FromUri] string msgid, [FromBody] string json)// {"LatestDateTime":"yyyy-MM-dd HH:mm:ss.fff"}
        {
            try
            {
                ScheduleTaskByTabReq req = new ScheduleTaskByTabReq();
                req.Token = p;
                req.MsgId = msgid;
                req.JsonFormat = json;

                return schedule.GetScheduleTaskByTab(req);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        [Route("api/v1/schedule/tasks/web")]
        [HttpPost()]
        public string ScheduleTaskGetByWeb([FromUri] string api_name, string api_key, string username, [FromBody] string json)//{"UserOwnerID":"5935","StartDate":"2020-01-10","EndDate":"2021-05-20"}
        {
            try
            {
                ScheduleTaskByWebReq req = new ScheduleTaskByWebReq();
                req.ApiName = api_name;
                req.ApiKey = api_key;
                req.Username = username;
                req.JsonFormat = json;
                return schedule.GetScheduleTaskByWeb(req);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        [Route("api/v1/schedule/web/report")]
        [HttpPost()]
        public string COReportByWeb([FromUri] string api_name, string api_key, string username, [FromBody] string json)//{"UserOwnerID":"5935","StartDate":"2020-01-10","EndDate":"2021-05-20"}
        {
            try
            {
                ScheduleTaskByWebReq req = new ScheduleTaskByWebReq();
                req.ApiName = api_name;
                req.ApiKey = api_key;
                req.Username = username;
                req.JsonFormat = json;
                return schedule.GetReportByWeb(req);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

    }
}
