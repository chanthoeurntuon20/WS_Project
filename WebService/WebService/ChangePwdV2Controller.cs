using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace WebService
{
    [BasicAuthentication]
    public class ChangePwdV2Controller : ApiController
    {

        // POST api/<controller>
        //public IEnumerable<ChangePwdResModel> Post([FromUri]string api_name, string api_key, string username, [FromBody]string json)
        //public IEnumerable<ChangePwdResModel> Post([FromUri]string p, [FromUri]string msgid, [FromBody]string json)
        public string Post([FromUri]string p, [FromUri]string msgid, [FromBody]string json)
        {
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "", ExSMS = "", ERRCode = "";
            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            List<ChangePwdResModel> RSData = new List<ChangePwdResModel>();
            string ControllerName = "ChangePwdV2";
            string FileNameForLog = msgid + "_" + ServerDate.Replace("-", "_").Replace(" ", "_").Replace(":", "_");
            try {
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
                ChangePwdDataModel jObj = null;
                string OldPwd = "", NewPwd = "";
                if (ERR != "Error")
                {
                    try
                    {
                        jObj = JsonConvert.DeserializeObject<ChangePwdDataModel>(json);
                        OldPwd = jObj.OldPwd;
                        NewPwd = jObj.NewPwd;
                    }
                    catch (Exception ex)
                    {
                        ERR = "Error";
                        ExSMS = ex.Message.ToString();
                        //get sms
                        string[] str = c.GetSMSByMsgID("3");
                        ERR = str[0];
                        if (ERR == "Error")
                        {
                            SMS = str[1];
                            ExSMS = ExSMS + "|" + str[2];
                        }
                        else
                        {
                            SMS = str[3];
                        }
                        ERR = "Error";
                    }
                }
                #endregion
                
                #region ChangePwd
                if (ERR != "Error")
                {
                    SqlConnection Con1 = new SqlConnection(c.ConStr());
                    Con1.Open();
                    SqlCommand Com1 = new SqlCommand();
                    Com1.Connection = Con1;
                    string sql = "exec v2_change_pwd_2 @UserID=@UserID,@OldPwd=@OldPwd,@NewPwd=@NewPwd";
                    Com1.CommandText = sql;
                    Com1.Parameters.Clear();
                    Com1.Parameters.AddWithValue("@UserID", UserID);
                    Com1.Parameters.AddWithValue("@OldPwd", c.Encrypt(OldPwd, c.SeekKeyGet()));
                    Com1.Parameters.AddWithValue("@NewPwd", c.Encrypt(NewPwd, c.SeekKeyGet()));
                    DataTable dt1 = new DataTable();
                    dt1.Load(Com1.ExecuteReader());
                    ERR = dt1.Rows[0]["ERR"].ToString();
                    SMS = dt1.Rows[0]["SMS"].ToString();
                    Con1.Close();
                }
                #endregion ChangePwd
            }
            catch (Exception ex) {
                ERR = "Error";
                SMS = "Something was wrong at line:" + c.GetLineNumber(ex) + " | Ex:" + ex.Message.ToString();                
            }

            ChangePwdResModel data = new ChangePwdResModel();
            data.ERR = ERR;
            data.SMS = SMS;
            data.ERRCode = ERRCode;
            RSData.Add(data);

            string RSDataStr = "";
            try
            {
                var jsonRS = new JavaScriptSerializer().Serialize(RSData);
                RSDataStr = c.Encrypt(jsonRS, c.SeekKeyGet());
                c.T24_AddLog(FileNameForLog, "4.RS", jsonRS.ToString(), ControllerName);
            }
            catch { }
            return RSDataStr;
        }

    }
    public class ChangePwdDataModel
    {
        public string OldPwd { get; set; }
        public string NewPwd { get; set; }
    }

    public class ChangePwdResModel
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
        public string ERRCode { get; set; }
    }
}