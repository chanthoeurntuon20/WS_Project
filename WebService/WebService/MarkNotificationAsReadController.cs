using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Http;
using Dapper;


namespace WebService
{
    public class MarkNotificationAsReadController : ApiController
    {
        public JsonResponse<string> Post([FromUri] string api_name, string api_key, string json)
        {
            //json=[{"UserId": "none"}]
            var response = new JsonResponse<string>();
            Class1 c = new Class1();
            Common cmn = new Common();
            string ERR = "Succeed", SMS = "", UserId = "";

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

                #region UpdateNotificationStatus 
                if (ERR != "Error")
                {
                    SqlConnection Con1 = new SqlConnection(c.ConStr());
                    try
                    {
                        Con1.Open();
                        SqlCommand Com1 = new SqlCommand();
                        Com1.Connection = Con1;
                        var res = Con1.Query<string>("[sp_UpdateNotificationStatus]", new { UserId = UserId }, commandType: CommandType.StoredProcedure);
                        ERR = res.First();
                    }
                    catch (Exception ex)
                    {
                        ERR = "Error";
                        SMS = "Invalid JSON";
                    }
                    finally
                    {
                        Con1.Close();
                    }
                }
                #endregion UpdateNotificationStatus
            }
            catch (Exception ex)
            {
                ERR = "Error";
            }

            response.ERR = ERR;
            response.SMS = SMS;
            return response;
        }
    }
}