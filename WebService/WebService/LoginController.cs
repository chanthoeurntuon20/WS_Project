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
    public class LoginController : ApiController
    {

        // POST api/<controller>
        //public IEnumerable<LoginResModel> Post([FromUri]string api_name, string api_key, string username, [FromBody]string json)
        public string Post([FromUri]string p, [FromUri]string msgid, [FromBody]string json)//p=ED(api_name||api_key) | json="{\"user\":\"02726\",\"pwd\":\"1\",\"device_id\":\"355755085347904\",\"app_vName\":\"1.6\",\"mac_address\":\"123456789\",\"sdk\":\"29\",\"isRoot\":\"0\",\"deviceTime\":\"2020-09-21 14:53:20.123\"}"
        {
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "",ExSMS="";
            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            List<LoginResModel> RSData = new List<LoginResModel>();
            List<LoginResData> RSLoginData = new List<LoginResData>();
            string ControllerName = "LoginV2";
            string FileNameForLog = msgid + "_" + ServerDate.Replace("-", "_").Replace(" ", "_").Replace(":", "_").Replace(".", "_");
            try {
                #region msgid
                if (ERR != "Error")
                {
                    string[] str = c.CheckMsgID(msgid);
                    ERR = str[0];
                    SMS = str[1];
                    ExSMS = str[2];
                }
                #endregion
                #region add log
                if (ERR != "Error") {
                    c.T24_AddLog(FileNameForLog, "1.RQ", "p: "+p+" | json: "+json, ControllerName);
                }
                #endregion
                #region p
                string api_name = "", api_key = "";
                if (ERR != "Error")
                {
                    string[] str = c.CheckUrlParam(p);
                    ERR = str[0];
                    SMS = str[1];
                    ExSMS = str[4];
                    if (ERR != "Error")
                    {
                        api_name = str[2];
                        api_key = str[3];
                    }
                }
                #endregion
                #region check api
                if (ERR != "Error")
                {
                    string[] CheckApi = c.CheckApi(api_name, api_key);
                    ERR = CheckApi[0];
                    SMS = CheckApi[1];
                }
                #endregion    
                
                
                            
                #region check json
                if (ERR != "Error")
                {
                    string[] str = c.CheckObjED(json,"2");
                    ERR = str[0];
                    SMS = str[1];
                    ExSMS = str[2];
                    json = str[3];
                }
                #endregion check json
                #region read json
                LoginDataModel jObj = null;
                string user = "", pwd = "", device_id = "", app_vName = "", mac_address = "", sdk="", isRoot="", deviceTime="";
                if (ERR != "Error")
                {
                    try
                    {
                        jObj = JsonConvert.DeserializeObject<LoginDataModel>(json);
                        user = jObj.user;
                        user = c.Encrypt(user, c.SeekKeyGet());
                        pwd = jObj.pwd;
                        pwd=c.Encrypt(pwd, c.SeekKeyGet());
                        device_id = jObj.device_id;
                        app_vName = jObj.app_vName;
                        mac_address = jObj.mac_address;
                        sdk = jObj.sdk;
                        isRoot = jObj.isRoot;
                        deviceTime = jObj.deviceTime;
                    }
                    catch(Exception ex)
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
                #region Login
                string UserID = "", ExpireSecond="";
                if (ERR != "Error") {
                    SqlConnection Con1 = new SqlConnection(c.ConStr());
                    Con1.Open();
                    SqlCommand Com1 = new SqlCommand();
                    Com1.Connection = Con1;
                    string sql = "exec sp_Login_V3 @user=@user,@pwd=@pwd,@device_id=@device_id,@app_vName=@app_vName,@mac_address=@mac_address,@sdk=@sdk,@isRoot=@isRoot,@deviceTime=@deviceTime,@FileNameForLog=@FileNameForLog";
                    Com1.CommandText = sql;
                    Com1.Parameters.Clear();
                    Com1.Parameters.AddWithValue("@user", user);
                    Com1.Parameters.AddWithValue("@pwd", pwd);
                    Com1.Parameters.AddWithValue("@device_id", device_id);
                    Com1.Parameters.AddWithValue("@app_vName", app_vName);
                    Com1.Parameters.AddWithValue("@mac_address", mac_address);
                    Com1.Parameters.AddWithValue("@sdk", sdk);
                    Com1.Parameters.AddWithValue("@isRoot", isRoot);
                    Com1.Parameters.AddWithValue("@deviceTime", deviceTime);
                    Com1.Parameters.AddWithValue("@FileNameForLog", FileNameForLog);
                    DataTable dt1 = new DataTable();
                    dt1.Load(Com1.ExecuteReader());
                    ERR = dt1.Rows[0]["ERR"].ToString();
                    SMS = dt1.Rows[0]["SMS"].ToString();
                    UserID = dt1.Rows[0]["UserID"].ToString();
                    ExpireSecond = dt1.Rows[0]["ExpireSecond"].ToString();
                    Con1.Close();
                }
                #endregion
                #region create session
                string SessionID = "";
                if (ERR != "Error") {
                    LoginSessionClass lsc = new LoginSessionClass();
                    string[] rsStr = lsc.CreateSession(FileNameForLog, UserID, ExpireSecond, api_name);
                    ERR = rsStr[0];
                    SMS = rsStr[1];
                    ExSMS = rsStr[2];
                    SessionID = rsStr[3];
                }
                #endregion 
                #region Get User Info.
                if (ERR != "Error")
                {
                    SqlConnection Con1 = new SqlConnection(c.ConStr());
                    Con1.Open();
                    SqlCommand Com1 = new SqlCommand();
                    Com1.Connection = Con1;
                    string sql = "exec T24_GetUserInfo_V2 @user=@user,@pwd=@pwd,@device_id=@device_id";
                    Com1.CommandText = sql;
                    Com1.Parameters.Clear();
                    Com1.Parameters.AddWithValue("@user", user);
                    Com1.Parameters.AddWithValue("@pwd", pwd);
                    Com1.Parameters.AddWithValue("@device_id", device_id);
                    DataTable dt1 = new DataTable();
                    dt1.Load(Com1.ExecuteReader());
                    for (int i = 0; i < dt1.Rows.Count; i++) {
                        LoginResData lrd = new LoginResData();
                        //string UserID = dt1.Rows[i]["UserID"].ToString();
                        string GroupID = dt1.Rows[i]["GroupID"].ToString();
                        string OfficeID = dt1.Rows[i]["OfficeID"].ToString();
                        string OfficeCode = dt1.Rows[i]["OfficeCode"].ToString();
                        string OfficeName = dt1.Rows[i]["OfficeName"].ToString();
                        string ConnectionID = dt1.Rows[i]["ConnectionID"].ToString();
                        string OfficeHierachyID = dt1.Rows[i]["OfficeHierachyID"].ToString();
                        string UserDeviceID = dt1.Rows[i]["UserDeviceID"].ToString();
                        string StaffName = dt1.Rows[i]["StaffName"].ToString();

                        #region Permission
                        List<Permission> DataPermission = new List<Permission>();
                        try {                            
                            //DataTable dt2 = new DataTable();
                            //dt2 = c.ReturnDT("SELECT * from tblUserPermission where UserID='" + UserID + "'");
                            //for (int ip = 0; ip < dt2.Rows.Count; ip++)
                            //{
                            //    Permission DataPer = new Permission();
                            //    string UserPermissionID = dt2.Rows[ip]["UserPermissionID"].ToString();
                            //    string FormID = dt2.Rows[ip]["FormID"].ToString();
                            //    string Active = dt2.Rows[ip]["Active"].ToString();
                            //    string JSON = dt2.Rows[ip]["JSON"].ToString();
                            //    DataPer.UserPermissionID = UserPermissionID;
                            //    DataPer.FormID = FormID;
                            //    DataPer.Active = Active;
                            //    DataPer.JSON = JSON;
                            //    DataPermission.Add(DataPer);
                            //}
                        } catch { }
                        #endregion Permission
                        #region ContraAccount
                        List<ContraAccount> DataContraAccount = new List<ContraAccount>();
                        try
                        {
                            DataTable dt2 = new DataTable();
                            dt2 = c.ReturnDT("EXEC sp_GetUserContraAccount2 @UserID='" + UserID + "'");
                            for (int ip = 0; ip < dt2.Rows.Count; ip++)
                            {
                                ContraAccount DataP = new ContraAccount();
                                string ACCTID = dt2.Rows[ip]["ACCTID"].ToString();
                                string CCYCODE = dt2.Rows[ip]["CCYCODE"].ToString();
                                string DAOID = dt2.Rows[ip]["DAOID"].ToString();
                                string COMPCODE = dt2.Rows[ip]["COMPCODE"].ToString();
                                DataP.ACCTID = ACCTID;
                                DataP.CCYCODE = CCYCODE;
                                DataP.DAOID = DAOID;
                                DataP.COMPCODE = COMPCODE;
                                DataContraAccount.Add(DataP);
                            }
                        }
                        catch { }
                        #endregion ContraAccount
                        #region UrlData
                        List<DataUrl> DataUrl = new List<DataUrl>();
                        try
                        {
                            DataTable dt2 = new DataTable();
                            dt2 = c.ReturnDT("select ID,pms_name,pms_url,pms_name+pms_id as pms_sp from tbl_pms_url where pms_id in (select pms_id from tbl_pms_office where office_id='" + OfficeID + "')");
                            for (int ip = 0; ip < dt2.Rows.Count; ip++)
                            {
                                DataUrl d = new DataUrl();
                                string UrlID = dt2.Rows[ip]["ID"].ToString();
                                string UrlName = dt2.Rows[ip]["pms_name"].ToString();
                                string UrlLink = dt2.Rows[ip]["pms_url"].ToString();
                                string UrlSP = dt2.Rows[ip]["pms_sp"].ToString();
                                d.UrlID = UrlID;
                                d.UrlName = UrlName;
                                d.UrlLink = UrlLink;
                                d.UrlSP = UrlSP;
                                DataUrl.Add(d);
                            }
                        }
                        catch { }
                        #endregion

                        lrd.UserID = UserID;
                        lrd.GroupID = GroupID;
                        lrd.OfficeID = OfficeID;
                        lrd.OfficeCode = OfficeCode;
                        lrd.OfficeName = OfficeName;
                        lrd.ConnectionID = ConnectionID;
                        lrd.OfficeHierachyID = OfficeHierachyID;
                        lrd.UserDeviceID = UserDeviceID;
                        lrd.StaffName = StaffName;
                        lrd.SessionID = SessionID;
                        lrd.DataPermission = DataPermission;
                        lrd.DataContraAccount = DataContraAccount;
                        lrd.DataUrl = DataUrl;
                        RSLoginData.Add(lrd);
                    }
                    Con1.Close();
                }
                #endregion Get User Info.
            }
            catch (Exception ex) {
                ERR = "Error";
                SMS = "Something was wrong at line:" + c.GetLineNumber(ex) + " | Ex:" + ex.Message.ToString();                
            }

            LoginResModel data = new LoginResModel();
            data.ERR = ERR;
            data.SMS = SMS;
            data.DataList = RSLoginData;
            RSData.Add(data);

            string RSDataStr = "";
            try
            {
                var jsonRS = new JavaScriptSerializer().Serialize(RSData);
                RSDataStr= c.Encrypt(jsonRS, c.SeekKeyGet());
                c.T24_AddLog(FileNameForLog, "RS", jsonRS.ToString(), ControllerName);
            }
            catch { }
            //[{"ERR":"Succeed","SMS":"","DataList":[{"UserID":"2112","GroupID":"5","OfficeID":"KH0010202","OfficeCode":"KH0010202","OfficeName":"Aoral","ConnectionID":"1","OfficeHierachyID":"0482","UserDeviceID":"3824","StaffName":"Hin Synon","SessionID":"gEPiza4WVkN+rOAnxB87WaH0WaeLJsj9cB6VichlEdlWSEUllgZAWnxS44bwI7DicIvBKi8Uo9DQwbtlPL79K2/jW68U7AJpeQT37FdJ8HBz4sy5Op0yKLz0hHKD3afa","DataPermission":[],"DataContraAccount":[{"ACCTID":"KHR1226204820206","CCYCODE":"KHR","DAOID":"0482","COMPCODE":"KH0010206"},{"ACCTID":"USD1226204820206","CCYCODE":"USD","DAOID":"0482","COMPCODE":"KH0010206"},{"ACCTID":"THB1226204820206","CCYCODE":"THB","DAOID":"0482","COMPCODE":"KH0010206"}]}]}]
            return RSDataStr;
        }

    }
    public class LoginResModel
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
        public List<LoginResData> DataList { get; set; }
    }
    public class LoginResData {
        public string UserID { get; set; }
        public string GroupID { get; set; }
        public string OfficeID { get; set; }
        public string OfficeCode { get; set; }
        public string OfficeName { get; set; }
        public string ConnectionID { get; set; }
        public string OfficeHierachyID { get; set; }
        public string UserDeviceID { get; set; }
        public string StaffName { get; set; }
        public string SessionID { get; set; }
        public List<Permission> DataPermission { get; set; }
        public List<ContraAccount> DataContraAccount { get; set; }
        public List<DataUrl> DataUrl { get; set; }
    }
    public class Permission {
        public string UserPermissionID { get; set; }
        public string FormID { get; set; }
        public string Active { get; set; }
        public string JSON { get; set; }
    }
    public class ContraAccount {
        public string ACCTID { get; set; }
        public string CCYCODE { get; set; }
        public string DAOID { get; set; }
        public string COMPCODE { get; set; }
    }
    public class DataUrl
    {
        public string UrlID { get; set; }
        public string UrlName { get; set; }
        public string UrlLink { get; set; }
        public string UrlSP { get; set; }
    }
    public class LoginDataModel {
        public string user { get; set; }
        public string pwd { get; set; }
        public string device_id { get; set; }
        public string app_vName { get; set; }
        public string mac_address { get; set; } = "";
        public string sdk { get; set; } = "";
        public string isRoot { get; set; } = "";// "" | 0=Root | 1=not root
        public string deviceTime { get; set; } = "";//yyyy-MM-dd HH:mm:ss
    }

}