using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace WebService
{
    [BasicAuthentication]
    public class T24_GetDisbByAMOrCOFromDeviceController : ApiController
    {
        Class1 c = new Class1();
        //public IEnumerable<T24_GetDisbByAMOrCOFromDeviceModel> Post([FromUri]string api_name, string api_key, [FromBody] string json)
        public string Post([FromUri]string p, [FromUri]string msgid, [FromBody]string json)
        {
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "", ExSMS = "", ERRCode = "";
            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            List<T24_GetDisbRS> RSData = new List<T24_GetDisbRS>();
            string ControllerName = "T24_GetDisbByAMOrCOFromDevice";
            string FileNameForLog = msgid + "_" + ServerDate.Replace("-", "_").Replace(" ", "_").Replace(":", "_");
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
                T24_GetDisbRQ jObj = null;
                string VBIDList = "";
                if (ERR != "Error")
                {
                    try
                    {
                        jObj = JsonConvert.DeserializeObject<T24_GetDisbRQ>(json);
                        VBIDList = jObj.VBIDList;
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

                #region data
                if (ERR != "Error")
                {
                    T24_GetDisbRS ListHeader = new T24_GetDisbRS();
                    ListHeader.ERR = ERR;
                    ListHeader.SMS = SMS;
                    ListHeader.ERRCode = ERRCode;

                    List<T24_GetDisbRSDataList> DataList = new List<T24_GetDisbRSDataList>();

                    DataTable dt = c.ReturnDT("exec T24_GetDisbByVB_V2 @VBID='" + VBIDList + "'");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        #region params
                        T24_GetDisbRSDataList data = new T24_GetDisbRSDataList();
                        data.DisburseID = dt.Rows[i]["DisburseID"].ToString();
                        data.VillageBankID = dt.Rows[i]["VillageBankID"].ToString();
                        data.VBName = dt.Rows[i]["VBName"].ToString();
                        data.CurrencyID = dt.Rows[i]["CurrencyID"].ToString();
                        data.DisbCCY = dt.Rows[i]["DisbCCY"].ToString();
                        data.ClientID = dt.Rows[i]["ClientID"].ToString();
                        data.ClientName = dt.Rows[i]["ClientName"].ToString();
                        data.ClientNumber = dt.Rows[i]["ClientNumber"].ToString();
                        data.CUAccountID = dt.Rows[i]["CUAccountID"].ToString();
                        data.AccountNumber = dt.Rows[i]["AccountNumber"].ToString();
                        data.DGroup = dt.Rows[i]["DGroup"].ToString();
                        data.ProdCode = dt.Rows[i]["ProdCode"].ToString();
                        data.ProdName = dt.Rows[i]["ProdName"].ToString();
                        data.ValueDate = dt.Rows[i]["ValueDate"].ToString();
                        data.ApprovedDate = dt.Rows[i]["ApprovedDate"].ToString();
                        data.ApprovedAmount = dt.Rows[i]["ApprovedAmount"].ToString();
                        data.FeeAmount = dt.Rows[i]["FeeAmount"].ToString();
                        data.CompulsorySaving = dt.Rows[i]["CompulsorySaving"].ToString();
                        data.DisStatus = dt.Rows[i]["DisStatus"].ToString();
                        data.EditStatus = dt.Rows[i]["EditStatus"].ToString();
                        data.MITypeID = dt.Rows[i]["MITypeID"].ToString();
                        data.loanAppPersonType = dt.Rows[i]["loanAppPersonType"].ToString();
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
                SMS = "Something was wrong at line:" + c.GetLineNumber(ex) + " | Ex:" + ex.Message.ToString();
            }

            #region if Error
            if (ERR == "Error")
            {
                T24_GetDisbRS ListHeader = new T24_GetDisbRS();
                ListHeader.ERR = ERR;
                ListHeader.SMS = SMS;
                ListHeader.ERRCode = ERRCode;
                ListHeader.DataList = null;
                RSData.Add(ListHeader);
            }
            #endregion
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
}

public class T24_GetDisbRQ{
    public string VBIDList { get; set; }
}
public class T24_GetDisbRS
{
    public string ERR { get; set; }
    public string SMS { get; set; }
    public string ERRCode { get; set; }
    public List<T24_GetDisbRSDataList> DataList { get; set; }
}
public class T24_GetDisbRSDataList
{
    public string DisburseID { get; set; }
    public string VillageBankID { get; set; }
    public string VBName { get; set; }
    public string CurrencyID { get; set; }
    public string DisbCCY { get; set; }
    public string ClientID { get; set; }
    public string ClientName { get; set; }
    public string ClientNumber { get; set; }
    public string CUAccountID { get; set; }
    public string AccountNumber { get; set; }
    public string DGroup { get; set; }
    public string ProdCode { get; set; }
    public string ProdName { get; set; }
    public string ValueDate { get; set; }
    public string ApprovedDate { get; set; }
    public string ApprovedAmount { get; set; }
    public string FeeAmount { get; set; }
    public string CompulsorySaving { get; set; }
    public string DisStatus { get; set; }
    public string EditStatus { get; set; }
    public string MITypeID { get; set; }
    public string loanAppPersonType { get; set; }
}