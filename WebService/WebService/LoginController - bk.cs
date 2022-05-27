//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Web.Http;

//namespace WebService
//{
//    [BasicAuthentication]
//    public class LoginController : ApiController
//    {

//        // POST api/<controller>
//        public IEnumerable<LoginResModel> Post([FromUri]string api_name, string api_key, [FromBody]string json)
//        {
//            Class1 c = new Class1();
//            string ERR = "Succeed", SMS = "";
//            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
//            List<LoginResModel> RSData = new List<LoginResModel>();
//            List<LoginResData> RSLoginData = new List<LoginResData>();
//            try {
//                //Add log
//                c.T24_AddLog("Login_" + api_name + "_" + ServerDate.Replace("-", "_").Replace(" ", "_").Replace(":", "_"), "RQ", json, "Login");
//                #region check json
//                if (json == null || json == "")
//                {
//                    ERR = "Error";
//                    SMS = "Invalid JSON";
//                }
//                #endregion check json
//                LoginDataModel jObj = null;
//                string user="",pwd="",device_id="",app_vName = "", mac_address="";
//                #region json
//                if (ERR != "Error")
//                {
//                    try
//                    {
//                        jObj = JsonConvert.DeserializeObject<LoginDataModel>(json);
//                        user = jObj.user;
//                        user = c.Encrypt(user, c.SeekKeyGet());
//                        pwd = jObj.pwd;
//                        pwd=c.Encrypt(pwd, c.SeekKeyGet());
//                        device_id = jObj.device_id;
//                        app_vName = jObj.app_vName;
//                        mac_address = jObj.mac_address;
//                    }
//                    catch
//                    {
//                        ERR = "Error";
//                        SMS = "Invalid JSON";
//                    }
//                }
//                #endregion json
//                #region Login
//                if (ERR != "Error") {
//                    SqlConnection Con1 = new SqlConnection(c.ConStr());
//                    Con1.Open();
//                    SqlCommand Com1 = new SqlCommand();
//                    Com1.Connection = Con1;
//                    string sql = "exec T24_Login_V2 @user=@user,@pwd=@pwd,@device_id=@device_id,@app_vName=@app_vName,@mac_address=@mac_address";
//                    Com1.CommandText = sql;
//                    Com1.Parameters.Clear();
//                    Com1.Parameters.AddWithValue("@user", user);
//                    Com1.Parameters.AddWithValue("@pwd", pwd);
//                    Com1.Parameters.AddWithValue("@device_id", device_id);
//                    Com1.Parameters.AddWithValue("@app_vName", app_vName);
//                    Com1.Parameters.AddWithValue("@mac_address", mac_address);
//                    DataTable dt1 = new DataTable();
//                    dt1.Load(Com1.ExecuteReader());
//                    ERR = dt1.Rows[0]["ERR"].ToString();
//                    SMS = dt1.Rows[0]["SMS"].ToString();
//                    Con1.Close();
//                }
//                #endregion Login
//                #region Get User Info.
//                if (ERR != "Error")
//                {
//                    SqlConnection Con1 = new SqlConnection(c.ConStr());
//                    Con1.Open();
//                    SqlCommand Com1 = new SqlCommand();
//                    Com1.Connection = Con1;
//                    string sql = "exec T24_GetUserInfo_V2 @user=@user,@pwd=@pwd";
//                    Com1.CommandText = sql;
//                    Com1.Parameters.Clear();
//                    Com1.Parameters.AddWithValue("@user", user);
//                    Com1.Parameters.AddWithValue("@pwd", pwd);
//                    DataTable dt1 = new DataTable();
//                    dt1.Load(Com1.ExecuteReader());
//                    for (int i = 0; i < dt1.Rows.Count; i++) {
//                        LoginResData lrd = new LoginResData();
//                        string UserID = dt1.Rows[i]["UserID"].ToString();
//                        string GroupID = dt1.Rows[i]["GroupID"].ToString();
//                        string OfficeID = dt1.Rows[i]["OfficeID"].ToString();
//                        string OfficeHierachyID = dt1.Rows[i]["OfficeHierachyID"].ToString();
//                        lrd.UserID = UserID;
//                        lrd.GroupID = GroupID;
//                        lrd.OfficeID = OfficeID;
//                        lrd.OfficeHierachyID = OfficeHierachyID;
//                        RSLoginData.Add(lrd);
//                    }
//                    Con1.Close();
//                }
//                #endregion Get User Info.
//            }
//            catch (Exception ex) {
//                ERR = "Error";
//                SMS = "Something was wrong at line:" + c.GetLineNumber(ex) + " | Ex:" + ex.Message.ToString();                
//            }

//            LoginResModel data = new LoginResModel();
//            data.ERR = ERR;
//            data.SMS = SMS;
//            data.DataList = RSLoginData;
//            RSData.Add(data);
//            return RSData;
//        }

//    }
//    public class LoginResModel
//    {
//        public string ERR { get; set; }
//        public string SMS { get; set; }
//        public List<LoginResData> DataList { get; set; }
//    }
//    public class LoginResData {
//        public string UserID { get; set; }
//        public string GroupID { get; set; }
//        public string OfficeID { get; set; }
//        public string OfficeHierachyID { get; set; }
//    }

//    public class LoginDataModel {
//        public string user { get; set; }
//        public string pwd { get; set; }
//        public string device_id { get; set; }
//        public string app_vName { get; set; }
//        public string mac_address { get; set; }
//    }

//}