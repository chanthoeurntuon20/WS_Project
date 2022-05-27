using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace WebService
{
    public class VBGetByAMorCOFromDeviceController : ApiController
    {
        // GET api/<controller>
        //public IEnumerable<VillageBankModel> POST([FromUri]string api_name, string api_key, [FromBody]string json)
        public string Post([FromUri]string p, [FromUri]string msgid)
        {
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "", ExSMS = "", ERRCode = "";
            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string ControllerName = "VBGetByAMorCOFromDevice";
            string FileNameForLog = msgid + "_" + ServerDate.Replace("-", "_").Replace(" ", "_").Replace(":", "_").Replace(".", "_");
            List<VBModelRS> RSData = new List<VBModelRS>();
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
                //if (ERR != "Error")
                //{
                //    string[] str = c.CheckObjED(json, "2");
                //    ERR = str[0];
                //    SMS = str[1];
                //    ExSMS = str[2];
                //    json = str[3];
                //}
                #endregion

                #region data
                if (ERR != "Error")
                {
                    VBModelRS ListHeader = new VBModelRS();
                    ListHeader.ERR = ERR;
                    ListHeader.SMS = SMS;
                    ListHeader.ERRCode = ERRCode;
                    List<VBListRS> DataList = new List<VBListRS>();

                    DataTable dt = c.ReturnDT("exec T24_GetVBByAMOrCOFromDevice @UserID='" + UserID + "'");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        #region params
                        VBListRS data = new VBListRS();
                        data.VBLID = dt.Rows[i]["VBLID"].ToString();
                        data.CUAccountPortfolioID = dt.Rows[i]["CUAccountPortfolioID"].ToString();
                        data.VillageBankID = dt.Rows[i]["VillageBankID"].ToString();
                        data.VillageName = dt.Rows[i]["VillageName"].ToString();
                        data.MeetingDate = dt.Rows[i]["MeetingDate"].ToString();
                        data.Status = dt.Rows[i]["Status"].ToString();
                        data.ExpiredDate_Rotation = dt.Rows[i]["ExpiredDate_Rotation"].ToString();
                        data.NewCUAccountPortfolioID = dt.Rows[i]["NewCUAccountPortfolioID"].ToString();
                        DataList.Add(data);
                        #endregion params
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
                ERRCode = ex.Message.ToString();
            }
            #region if Error
            if (ERR == "Error")
            {
                VBModelRS ListHeader = new VBModelRS();
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
                c.T24_AddLog(FileNameForLog, "4.RS", jsonRS.ToString(), ControllerName);
            }
            catch { }
            return RSDataStr;
        }



    }

    public class VBModelRS
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
        public string ERRCode { get; set; }
        public List<VBListRS> DataList { get; set; }
    }
    public class VBListRS
    {
        public string VBLID { get; set; }
        public string CUAccountPortfolioID { get; set; }
        public string VillageBankID { get; set; }
        public string VillageName { get; set; }
        public string MeetingDate { get; set; }
        public string Status { get; set; }
        public string ExpiredDate_Rotation { get; set; }
        public string NewCUAccountPortfolioID { get; set; }
    }

}