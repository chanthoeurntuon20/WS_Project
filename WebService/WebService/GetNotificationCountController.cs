using System;
using System.Web.Http;
using System.Data;

namespace WebService
{
    [BasicAuthentication]
    public class GetNotificationCountController : ApiController
    {
        // GET api/<controller>
        public JsonResponse<NotificationCountModel> Get(string api_name, string api_key, string json)//json=[{"UserId":"none"}]
        {
            var response = new JsonResponse<NotificationCountModel>();
            Class1 c = new Class1();
            Common cmn = new Common();
            string ERR = "Succeed", SMS = "";
            string UserId = "";

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
                    }
                }
                #endregion json

                #region data
                if (ERR != "Error")
                {
                    NotificationCountModel ListHeader = new NotificationCountModel();
                    ListHeader.ERR = ERR;
                    ListHeader.SMS = SMS;
                    DataTable dt = c.ReturnDT("Exec [T24_GetNotificationsCount] @UserId=" + Convert.ToInt32(UserId) + "");
                    if (dt.Rows.Count > 0)
                    {
                        ListHeader.TotalCount = dt.Rows[0]["totalCount"].ToString();
                    }
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