using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Data;

namespace WebService
{
    [BasicAuthentication]
    public class GetNotificationController : ApiController
    {
        // GET api/<controller>
        public JsonResponse<NotificationModel> Get(string api_name, string api_key, string json)//json=[{"UserId":"none"}]
        {
            var response = new JsonResponse<NotificationModel>();
            var list = new NotificationModel();
            Class1 c = new Class1();
            Common cmn = new Common();
            string ERR = "Succeed", SMS = "";
            string UserId = "";
            int page = 0, limit = 0;

            try
            {
                #region api
                string[] CheckApi = c.CheckApi(api_name, api_key);
                ERR = CheckApi[0];
                SMS = CheckApi[1];
                #endregion api

                #region json
                if (ERR != "Error")
                {
                    string[] CheckJson = cmn.CheckJsonString(json);

                    ERR = CheckJson[0];
                    if (ERR == "Error")
                    {
                        SMS = CheckJson[1];
                    }
                    else
                    {
                        UserId = CheckJson[8];
                        page = Convert.ToInt32(CheckJson[4]);
                        limit = Convert.ToInt32(CheckJson[5]);
                    }
                }
                #endregion json

                #region data
                if (ERR != "Error")
                {
                    NotificationModel ListHeader = new NotificationModel();
                    ListHeader.ERR = ERR;
                    ListHeader.SMS = SMS;
                    page = page == 0 ? page = 1 : page;
                    limit = limit == 0 ? limit = 1 : limit;
                    List<NotificationList> DataList = new List<NotificationList>();
                    DataTable dt = c.ReturnDT("Exec [T24_GetNotifications] @UserId=" + Convert.ToInt32(UserId) + ",@PageNumber=" + page + ",@PageSize=" + limit + "");

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        #region params
                        NotificationList data = new NotificationList();
                        data.Id = dt.Rows[i]["Id"].ToString();
                        data.Title = dt.Rows[i]["Title"].ToString();
                        data.Message = dt.Rows[i]["Message"].ToString();
                        data.IsRead = Convert.ToBoolean(dt.Rows[i]["IsRead"]);
                        data.Type = dt.Rows[i]["Type"].ToString();
                        data.Date = Convert.ToDateTime(dt.Rows[i]["DateAdded"]).ToUniversalTime().ToUnixTimestamp();
                        data.LoanAcc = dt.Rows[i]["LoanAcc"].ToString();
                        DataList.Add(data);
                        #endregion params
                    }

                    ListHeader.DataList = DataList;

                    ListHeader.DataList = DataList;
                    response.ERR = ERR;
                    response.SMS = SMS;
                    response.Data = ListHeader;
                }
                #endregion data
            }
            catch (Exception ex)
            {
                ERR = "Error";
                SMS = "Something was wrong";
            }
            #region if Error
            if (ERR == "Error")
            {
                response.ERR = ERR;
                response.SMS = SMS;
                response.Data = null;
            }
            #endregion if Error

            return response;
        }
    }
}