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
    public class ProsCustGetV2Controller : ApiController
    {
        //[EnableCors(origins: "*", headers: "*", methods: "*")]
        // GET api/<controller>
        //public IEnumerable<ProsCustGetV2RSModel> Post([FromUri]string api_name, string api_key, [FromBody]string json)
        public string Post([FromUri]string p, [FromUri]string msgid, [FromBody]string json)
        {
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "", ExSMS = "", ERRCode = "";
            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string ControllerName = "ProsCustGetV2";
            string FileNameForLog = msgid + "_" + ServerDate.Replace("-", "_").Replace(" ", "_").Replace(":", "_").Replace(".", "_");
            List<ProsCustGetV2RSModel> RSData = new List<ProsCustGetV2RSModel>();
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
                ProsCustGetV2RQ jObj = null;
                string userDec = "", criteriaValue = "";
                if (ERR != "Error")
                {
                    try
                    {
                        jObj = JsonConvert.DeserializeObject<ProsCustGetV2RQ>(json);                        
                        criteriaValue = jObj.criteriaValue;
                    }
                    catch (Exception ex)
                    {
                        ERR = "Error";
                        SMS = "Invalid JSON";
                        ExSMS = ex.Message.ToString();
                    }
                }
                #endregion
                #region data
                if (ERR != "Error")
                {
                    ProsCustGetV2RSModel ListHeader = new ProsCustGetV2RSModel();
                    ListHeader.ERR = ERR;
                    ListHeader.SMS = SMS;
                    ListHeader.ERRCode = ERRCode;

                    List<ProsCustGetV2RSList> DataList = new List<ProsCustGetV2RSList>();
                    string sql = "exec T24_GetProsCust_V2 @UserName='" + userDec + "',@CustServerID='" + criteriaValue + "'";
                    DataTable dt = c.ReturnDT2(sql);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        #region params
                        ProsCustGetV2RSList data = new ProsCustGetV2RSList();
                        data.CustClientID = dt.Rows[i]["CustClientID"].ToString();
                        data.CustServerID = dt.Rows[i]["CustServerID"].ToString();
                        data.ProsCode = dt.Rows[i]["ProspectCode"].ToString();
                        data.CreateDateClient = dt.Rows[i]["CreateDateClient"].ToString();
                        data.VBID = dt.Rows[i]["VBID"].ToString();
                        data.ReferByID = dt.Rows[i]["ReferByID"].ToString();
                        data.ReferName = dt.Rows[i]["ReferName"].ToString();
                        data.NameKhLast = dt.Rows[i]["NameKhLast"].ToString();
                        data.NameKhFirst = dt.Rows[i]["NameKhFirst"].ToString();
                        data.NameEnLast = dt.Rows[i]["NameEnLast"].ToString();
                        data.NameEnFirst = dt.Rows[i]["NameEnFirst"].ToString();
                        data.TitleID = dt.Rows[i]["TitleID"].ToString();
                        data.GenderID = dt.Rows[i]["GenderID"].ToString();
                        data.DateOfBirth = dt.Rows[i]["DateOfBirth"].ToString();
                        data.IDCardType = dt.Rows[i]["IDCardType"].ToString();
                        data.IDCardNumber = dt.Rows[i]["IDCardNumber"].ToString();
                        data.IDCardIssueDate = dt.Rows[i]["IDCardIssueDate"].ToString();
                        data.IDCardExpiryDate = dt.Rows[i]["IDCardExpiryDate"].ToString();
                        data.MaritalStatusID = dt.Rows[i]["MaritalStatusID"].ToString();
                        data.EducationLevelID = dt.Rows[i]["EducationLevelID"].ToString();
                        data.Phone = dt.Rows[i]["Phone"].ToString();
                        data.PlaceOfBirth = dt.Rows[i]["PlaceOfBirth"].ToString();
                        data.VillageIDPer = dt.Rows[i]["VillageIDPer"].ToString();
                        data.VillageIDCur = dt.Rows[i]["VillageIDCur"].ToString();
                        data.ShortAddress = dt.Rows[i]["ShortAddress"].ToString();
                        data.FamilyMenber = dt.Rows[i]["FamilyMenber"].ToString();
                        data.FamilyMenberActive = dt.Rows[i]["FamilyMenberActive"].ToString();
                        data.PoorLevelID = dt.Rows[i]["PoorLevelID"].ToString();
                        data.LatLon = dt.Rows[i]["LatLon"].ToString();
                        #endregion params
                        #region img
                        List<ProsCustGetV2RSListImg> DataList2 = new List<ProsCustGetV2RSListImg>();
                        try {                            
                            sql = "exec T24_GetProsCustImg_V2 @UserName='" + userDec + "',@CustServerID='" + criteriaValue + "'";
                            DataTable dt2 = c.ReturnDT2(sql);
                            for (int i2 = 0; i2 < dt2.Rows.Count; i2++) {
                                ProsCustGetV2RSListImg data2 = new ProsCustGetV2RSListImg();
                                data2.CustImageServerID= dt2.Rows[i2]["CustImageServerID"].ToString();
                                data2.CustImageClientID = dt2.Rows[i2]["CustImageClientID"].ToString();
                                data2.OneCardTwoDoc = dt2.Rows[i2]["OneCardTwoDoc"].ToString();
                                data2.Ext = dt2.Rows[i2]["Ext"].ToString();
                                data2.ImgPath = dt2.Rows[i2]["ImgPath"].ToString();
                                DataList2.Add(data2);
                            }
                        }
                        catch (Exception ex) { }
                        #endregion img
                        data.DataListImg = DataList2;
                        DataList.Add(data);                        
                    }

                    ListHeader.DataList = DataList;
                    RSData.Add(ListHeader);
                }
                #endregion data
            }
            catch (Exception ex)
            {
                ERR = "Error";
                SMS = "Something was wrong";
                ExSMS = ex.Message.ToString();
            }
            #region if Error
            if (ERR == "Error")
            {
                ProsCustGetV2RSModel ListHeader = new ProsCustGetV2RSModel();
                ListHeader.ERR = ERR;
                ListHeader.SMS = SMS;
                ListHeader.ERRCode = ERRCode;
                ListHeader.DataList = null;
                RSData.Add(ListHeader);
            }
            #endregion if Error

            string RSDataStr = "";
            try
            {
                var jsonRS = new JavaScriptSerializer().Serialize(RSData);
                RSDataStr = c.Encrypt(jsonRS, c.SeekKeyGet());
                c.T24_AddLog(FileNameForLog, "RS", jsonRS.ToString(), ControllerName);
            }
            catch { }
            return RSDataStr;
        }

    }
    public class ProsCustGetV2RQ
    {
        public string user { get; set; } = "";
        public string pwd { get; set; } = "";
        public string device_id { get; set; } = "";
        public string app_vName { get; set; } = "";
        public string mac_address { get; set; } = "";
        public string criteriaValue { get; set; }
    }
    public class ProsCustGetV2RSModel
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
        public string ERRCode { get; set; }
        public List<ProsCustGetV2RSList> DataList { get; set; }
    }
    public class ProsCustGetV2RSList
    {
        public string CustClientID { get; set; }
        public string CustServerID { get; set; }
        public string ProsCode { get; set; }
        public string CreateDateClient { get; set; }
        public string VBID { get; set; }
        public string ReferByID { get; set; }
        public string ReferName { get; set; }
        public string NameKhLast { get; set; }
        public string NameKhFirst { get; set; }
        public string NameEnLast { get; set; }
        public string NameEnFirst { get; set; }
        public string TitleID { get; set; }
        public string GenderID { get; set; }
        public string DateOfBirth { get; set; }
        public string IDCardType { get; set; }
        public string IDCardNumber { get; set; }
        public string IDCardIssueDate { get; set; }
        public string IDCardExpiryDate { get; set; }
        public string MaritalStatusID { get; set; }
        public string EducationLevelID { get; set; }
        public string Phone { get; set; }
        public string PlaceOfBirth { get; set; }
        public string VillageIDPer { get; set; }
        public string VillageIDCur { get; set; }
        public string ShortAddress { get; set; }
        public string FamilyMenber { get; set; }
        public string FamilyMenberActive { get; set; }
        public string PoorLevelID { get; set; }
        public string LatLon { get; set; }
        public List<ProsCustGetV2RSListImg> DataListImg { get; set; }
    }
    public class ProsCustGetV2RSListImg
    {
        public string CustImageServerID { get; set; }
        public string CustImageClientID { get; set; }
        public string OneCardTwoDoc { get; set; }
        public string Ext { get; set; }
        public string ImgPath { get; set; }
    }

}