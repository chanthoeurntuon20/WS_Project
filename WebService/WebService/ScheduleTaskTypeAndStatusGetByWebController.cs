using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;
using WebService.Models.Req.Schedules;
using WebService.Models.Res.Schedules;

namespace WebService
{
    [BasicAuthentication]
    public class ScheduleTaskTypeAndStatusGetByWebController : ApiController
    {
        
        public string Post([FromUri]string api_name, string api_key, string username, [FromBody] string req)//{"UserOwnerID":"5935","StartDate":"2020-01-10","EndDate":"2021-05-20"}
        {
            string msgid = username;
            Class1 c = new Class1();
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
                    string[] str = c.CheckObjED(req, "2");
                    ERR = str[0];
                    SMS = str[1];
                    ExSMS = str[2];
                    schedule = JsonConvert.DeserializeObject<SchduleTaskStatusAndTypeReq>(str[3]);
                }
                #endregion check json
                #region add log
                if (ERR != "Error")
                {
                    c.T24_AddLog(FileNameForLog, "1.RQ", "api_name:"+ api_name+ " | api_key:"+ api_key+ " | json:"+ req, ControllerName);
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
            var RSDataStr = c.Encrypt(jsonRS, c.SeekKeyGet());
            c.T24_AddLog(FileNameForLog, "RS", jsonRS.ToString(), ControllerName);

            return RSDataStr;
        }
    }
}