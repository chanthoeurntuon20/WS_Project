using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebService
{
    [BasicAuthentication]
    public class RepayMIController : ApiController
    {
        public IEnumerable<RepayMIResModel> Post([FromUri]string api_name, string api_key, string username, [FromBody]string json) {
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "";
            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            List<RepayMIResModel> RSData = new List<RepayMIResModel>();
            List<RepayMIResData> RSDisbData = new List<RepayMIResData>();
            string ControllerName = "DisbReject";
            string FileNameForLog = username + "_" + api_name + "_" + ServerDate.Replace("-", "_").Replace(" ", "_").Replace(":", "_");
            try
            {
                //Add log
                c.T24_AddLog(FileNameForLog, "RQ", json, ControllerName);

                #region check json
                if (json == null || json == "")
                {
                    ERR = "Error";
                    SMS = "Invalid JSON";
                }
                #endregion check json
                RepayMIDataModel jObj = null;
                string user = "", pwd = "", device_id = "", app_vName = "", mac_address = "";
                #region json
                if (ERR != "Error")
                {
                    try
                    {
                        jObj = JsonConvert.DeserializeObject<RepayMIDataModel>(json);
                        user = jObj.user;
                        pwd = jObj.pwd;
                        device_id = jObj.device_id;
                        app_vName = jObj.app_vName;
                        mac_address = jObj.mac_address;
                    }
                    catch
                    {
                        ERR = "Error";
                        SMS = "Invalid JSON";
                    }
                }
                #endregion json

                #region get userid
                if (ERR != "Error")
                {
                    DataTable dts = c.ReturnDT("exec T24_check_user @user='" + user + "',@pwd='" + c.Encrypt(pwd, c.SeekKeyGet()) + "'");
                    ERR = dts.Rows[0]["ERR"].ToString();
                    SMS = dts.Rows[0]["SMS"].ToString();

                }
                #endregion get userid

                #region data
                string MIID = "", DisbRejID = "", DisbID="", LookUp="", LookUpDescription="", RepayID = "", RepayPostID = "", RepayMIID = "", AAID="", Remark = "", CreateDate = "", CreateBy = "", GPS = "";
                if (ERR != "Error")
                {
                    foreach (RepayMIResData lis in jObj.DataList)
                    {
                        MIID = lis.MIID;
                        DisbRejID = lis.DisbRejID;
                        RepayID = lis.RepayID;
                        RepayPostID = lis.RepayPostID;
                        RepayMIID = lis.RepayMIID;
                        AAID = lis.AAID;
                        Remark = lis.Remark;
                        CreateDate = lis.CreateDate;
                        CreateBy = lis.CreateBy;
                        GPS = lis.GPS;
                    }
                }
                #endregion

                #region do with db
                string code = "";
                DataTable dt = new DataTable();
                dt = c.ReturnDT("exec disburment_reject_ins '" + DisbRejID + "','" + DisbID + "','" + AAID + "','" + LookUp + "',N'" + LookUpDescription + "',N'" + Remark + "','" + CreateDate + "','" + CreateBy + "','" + GPS + "'");
                try
                {
                    code = dt.Rows[0]["code"].ToString();
                    SMS = dt.Rows[0]["sms"].ToString();
                    if (code != "0")
                    {
                        ERR = "Error";
                    }
                }
                catch (Exception ex)
                {
                    ERR = "Error";
                    SMS = "Something was wrong at line:" + c.GetLineNumber(ex) + " | Ex:" + ex.Message.ToString();
                }
                #endregion

            }
            catch (Exception ex)
            {
                ERR = "Error";
                SMS = "Something was wrong at line:" + c.GetLineNumber(ex) + " | Ex:" + ex.Message.ToString();
            }

            c.T24_AddLog(FileNameForLog, "RS", ERR + " " + SMS, ControllerName);

            RepayMIResModel data = new RepayMIResModel();
            data.ERR = ERR;
            data.SMS = SMS;
            RSData.Add(data);

            return RSData;
        }
    }
}

public class RepayMIResModel
{
    public string ERR { get; set; }
    public string SMS { get; set; }
}

public class RepayMIDataModel
{
    public string user { get; set; }
    public string pwd { get; set; }
    public string device_id { get; set; }
    public string app_vName { get; set; }
    public string mac_address { get; set; }
    public List<RepayMIResData> DataList { get; set; }
}
public class RepayMIResData
{
    public string MIID { get; set; }
    public string DisbRejID { get; set; }
    public string RepayID { get; set; }
    public string RepayPostID { get; set; }
    public string RepayMIID { get; set; }
    public string AAID { get; set; }
    public string CID { get; set; }
    public string CustomerName { get; set; }
    public string MITypeID { get; set; }
    public string Remark { get; set; }
    public string CreateDate { get; set; }
    public string CreateBy { get; set; }
    public string GPS { get; set; }
}