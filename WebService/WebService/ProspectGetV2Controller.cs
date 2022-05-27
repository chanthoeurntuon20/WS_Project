using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;

namespace WebService
{
    [BasicAuthentication]
    public class ProspectGetV2Controller : ApiController
    {
        //[EnableCors(origins: "*", headers: "*", methods: "*")]
        // GET api/<controller>
        //public IEnumerable<ProspectGetV2RSModel> Post([FromUri]string api_name, string api_key,string username, [FromBody]string json)
        public string Post([FromUri]string p, [FromUri]string msgid, [FromBody]string json)
        {
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "", ExSMS = "", ERRCode = "";
            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string ControllerName = "ProspectGetV2";
            string FileNameForLog = msgid + "_" + ServerDate.Replace("-", "_").Replace(" ", "_").Replace(":", "_").Replace(".", "_");
            List<ProspectGetV2RSModel> RSData = new List<ProspectGetV2RSModel>();
            ProspectGetV2RSModel ListHeader = new ProspectGetV2RSModel();
            try
            {
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
                ProspectGetV2RQ jObj = null;
                string userDec = "", criteriaValue = "";
                if (ERR != "Error")
                {
                    try
                    {
                        jObj = JsonConvert.DeserializeObject<ProspectGetV2RQ>(json);
                        criteriaValue = jObj.criteriaValue;
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
                   

                    List<ProspectGetV2RSList> DataList = new List<ProspectGetV2RSList>();
                    string sql = "exec T24_GetProspect_V2 @UserID='"+UserID+"',@UserName='" + userDec + "',@Code='" + criteriaValue + "'";
                    DataTable dt = c.ReturnDT(sql);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        #region params
                        ProspectGetV2RSList data = new ProspectGetV2RSList();
                        data.Code = dt.Rows[i]["Code"].ToString(); 
                        data.CreateDateClient = dt.Rows[i]["CreateDateClient"].ToString();
                        data.RegisterDate = dt.Rows[i]["RegisterDate"].ToString();
                        data.ReferByID = dt.Rows[i]["ReferByID"].ToString();
                        data.ReferName = dt.Rows[i]["ReferName"].ToString();
                        data.NameKh = dt.Rows[i]["NameKh"].ToString();
                        data.NameEn = dt.Rows[i]["NameEn"].ToString();
                        data.GenderID = dt.Rows[i]["GenderID"].ToString();
                        data.Age = dt.Rows[i]["Age"].ToString();
                        data.Phone = dt.Rows[i]["Phone"].ToString();
                        data.VillageID = dt.Rows[i]["VillageID"].ToString();
                        data.BizStatusID = dt.Rows[i]["BizStatusID"].ToString();
                        data.CollateralStatusID = dt.Rows[i]["CollateralStatusID"].ToString();
                        data.LoanEligibleID = dt.Rows[i]["LoanEligibleID"].ToString();
                        data.PromotionDate1 = dt.Rows[i]["PromotionDate1"].ToString();
                        data.PromotionDate2 = dt.Rows[i]["PromotionDate2"].ToString();
                        data.PromotionDate3 = dt.Rows[i]["PromotionDate3"].ToString();
                        data.CustComment = dt.Rows[i]["CustComment"].ToString();
                        data.ExpectOnBoardDate = dt.Rows[i]["ExpectOnBoardDate"].ToString();
                        data.ProspectStatusID = dt.Rows[i]["ProspectStatusID"].ToString();
                        data.CustServerID = dt.Rows[i]["CustServerID"].ToString();
                        data.IsOldCust = dt.Rows[i]["IsOldCust"].ToString();
                        data.FromSystemID = dt.Rows[i]["APIID"].ToString();//3=Refferal
                        data.OccupationType = dt.Rows[i]["OccupationType"].ToString();
                        data.COCommends = dt.Rows[i]["COCommends"].ToString();
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
                RSDataStr = c.Encrypt(jsonRS, c.SeekKeyGet());
                c.T24_AddLog(FileNameForLog, "RS", jsonRS.ToString(), ControllerName);
            }
            catch { }
            return RSDataStr;
        }

    }
    public class ProspectGetV2RQ
    {
        public string user { get; set; }
        public string pwd { get; set; }
        public string device_id { get; set; }
        public string app_vName { get; set; }
        public string mac_address { get; set; }
        public string criteriaValue { get; set; }
    }
    public class ProspectGetV2RSModel
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
        public string ERRCode { get; set; }
        public List<ProspectGetV2RSList> DataList { get; set; }
    }
    public class ProspectGetV2RSList
    {
        public string Code { get; set; }
        public string CreateDateClient { get; set; }        
        public string RegisterDate { get; set; }
        public string ReferByID { get; set; }
        public string ReferName { get; set; }
        public string NameKh { get; set; }
        public string NameEn { get; set; }
        public string GenderID { get; set; }
        public string Age { get; set; }
        public string Phone { get; set; }
        public string VillageID { get; set; }
        public string BizStatusID { get; set; }
        public string CollateralStatusID { get; set; }
        public string LoanEligibleID { get; set; }
        public string PromotionDate1 { get; set; }
        public string PromotionDate2 { get; set; }
        public string PromotionDate3 { get; set; }
        public string CustComment { get; set; }
        public string ExpectOnBoardDate { get; set; }
        public string ProspectStatusID { get; set; }
        public string CustServerID { get; set; }
        public string IsOldCust { get; set; }
        public string FromSystemID { get; set; }
        public string OccupationType { get; set; } = "";
        public string COCommends { get; set; } = "";
    }

}