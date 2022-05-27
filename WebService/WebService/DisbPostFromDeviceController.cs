using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebService
{
    public class DisbPostFromDeviceController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Post([FromUri]string api_name, string api_key, string json)
        {
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "";
            try {
                #region api
                string[] CheckApi = c.CheckApi(api_name, api_key);
                ERR = CheckApi[0];
                SMS = CheckApi[1];
                #endregion api
                #region json to object     
                string user = "", pwd = "", DeviceID="";
                DisbPostFromDeviceModel jObj = null;
                if (ERR != "Error")
                {
                    try
                    {                        
                        jObj = JsonConvert.DeserializeObject<DisbPostFromDeviceModel>(json);
                        user = jObj.user;
                        pwd = jObj.pwd;
                        DeviceID = jObj.device_id;
                    }
                    catch
                    {
                        ERR = "Error";
                        SMS = "Invalid JSON";
                    }
                }
                #endregion json to object
                #region get userid
                string UserID = "";
                if (ERR != "Error")
                {
                    DataTable dt = c.ReturnDT("exec T24_check_user @user='" + user + "',@pwd='" + c.Encrypt(pwd, c.SeekKeyGet()) + "'");
                    ERR = dt.Rows[0]["ERR"].ToString();
                    SMS = dt.Rows[0]["SMS"].ToString();
                    UserID = SMS;
                    if (ERR == "Error")
                    {
                        SMS = "Invalid User/Pwd";
                    }
                }
                #endregion get userid
                #region get UserDeviceID
                string UserDeviceID = "0";
                if (ERR != "Error") {
                    try {
                        DataTable dt = c.ReturnDT("select UserDeviceID from tblUserDevice where UserID='" + UserID + "' AND DeviceID='"+ DeviceID + "' AND DeviceStatus=1");
                        UserDeviceID = dt.Rows[0]["UserDeviceID"].ToString();
                    } catch {
                        ERR = "Error";
                        SMS = "Invalid UserDevice";
                    }
                }
                #endregion get UserDeviceID
                #region read list
                string dtNow = DateTime.Now.ToString(@"yyyy-MM-dd HH:mm:ss");
                string DisburseID = "", FeeAmount = "", ApprovedAmount = "", Ref = "", RefFee = "", DeviceDate;
                SqlConnection Con1 = new SqlConnection(c.ConStr());
                Con1.Open();
                SqlCommand cmd1 = new SqlCommand();
                cmd1.Connection = Con1;
                foreach (var l in jObj.DisbList)
                {
                    DisburseID = l.DisburseID;
                    FeeAmount = l.FeeAmount;
                    ApprovedAmount = l.ApprovedAmount;
                    Ref = l.Ref;
                    RefFee = l.RefFee;
                    DeviceDate = l.DeviceDate;
                    #region add to DB
                    try {                        
                        string sql = "exec T24_PostDisbFromDevice @DisbID=@DisbID,@ActualChgAmt=@ActualChgAmt,@ActualDisbAmt=@ActualDisbAmt,@Ref=@Ref,@RefFee=@RefFee,@DeviceDate=@DeviceDate,@COUserDeviceID=@COUserDeviceID,@CreateBy=@CreateBy,@CreateDate=@CreateDate";
                        cmd1.Parameters.Clear();
                        cmd1.CommandText = sql;
                        cmd1.Parameters.AddWithValue("@DisbID", DisburseID);
                        cmd1.Parameters.AddWithValue("@ActualChgAmt", FeeAmount);
                        cmd1.Parameters.AddWithValue("@ActualDisbAmt", ApprovedAmount);
                        cmd1.Parameters.AddWithValue("@Ref", Ref);
                        cmd1.Parameters.AddWithValue("@RefFee", RefFee);
                        cmd1.Parameters.AddWithValue("@DeviceDate", DeviceDate);
                        cmd1.Parameters.AddWithValue("@COUserDeviceID", UserDeviceID);
                        cmd1.Parameters.AddWithValue("@CreateBy", UserID);
                        cmd1.Parameters.AddWithValue("@CreateDate", dtNow);
                        cmd1.ExecuteNonQuery();
                    } catch { }
                    #endregion add to DB
                }
                Con1.Close();
                #endregion read list
            }
            catch { }
            return new string[] { ERR, SMS };
        }
        
    }
}