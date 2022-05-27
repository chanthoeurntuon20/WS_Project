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
    [BasicAuthentication]
    public class ProductGetV2Controller : ApiController
    {
        // POST api/<controller>
        //public IEnumerable<ProductModel> Post([FromUri]string api_name, string api_key, [FromBody]string json)
        public string Post([FromUri]string p, [FromUri]string msgid)
        {
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "", ExSMS = "", ERRCode = "";
            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            List<ProductModel> RSData = new List<ProductModel>();
            ProductModel ListHeader = new ProductModel();
            string ControllerName = "ProductGetV2";
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

                #region data
                if (ERR != "Error")
                {
                    List<ProList> DataList = new List<ProList>();

                    DataTable dt = c.ReturnDT("exec T24_GetProductByDevice @UserID='" + UserID + "'");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        #region params
                        ProList data = new ProList();
                        data.AbacusProductCode = dt.Rows[i]["AbacusProductCode"].ToString();
                        data.PRODUCTID = dt.Rows[i]["PRODUCTID"].ToString();
                        data.PRODUCTDESC = dt.Rows[i]["PRODUCTDESC"].ToString();
                        data.CATEGORY = dt.Rows[i]["CATEGORY"].ToString();
                        data.CURRENCY = dt.Rows[i]["CURRENCY"].ToString();
                        data.BOFO = dt.Rows[i]["BOFO"].ToString();
                        data.RATETYPE = dt.Rows[i]["RATETYPE"].ToString();
                        data.EFFDATE = dt.Rows[i]["EFFDATE"].ToString();
                        data.MINTERM = dt.Rows[i]["MINTERM"].ToString();
                        data.MAXTERM = dt.Rows[i]["MAXTERM"].ToString();
                        data.MINAMOUNT = dt.Rows[i]["MINAMOUNT"].ToString();
                        data.MAXAMOUNT = dt.Rows[i]["MAXAMOUNT"].ToString();
                        data.MINRATE = dt.Rows[i]["MINRATE"].ToString();
                        data.MAXRATE = dt.Rows[i]["MAXRATE"].ToString();
                        data.DEFRATE = dt.Rows[i]["DEFRATE"].ToString();
                        data.MINAGE = dt.Rows[i]["MINAGE"].ToString();
                        data.MAXAGE = dt.Rows[i]["MAXAGE"].ToString();
                        data.REPAYTYPE = dt.Rows[i]["REPAYTYPE"].ToString();
                        data.MINUPFRONTFEE = dt.Rows[i]["MINUPFRONTFEE"].ToString();
                        data.MAXUPFRONTFEE = dt.Rows[i]["MAXUPFRONTFEE"].ToString();
                        data.MINMNTHTXNFEE = dt.Rows[i]["MINMNTHTXNFEE"].ToString();
                        data.MAXMNTHTXNFEE = dt.Rows[i]["MAXMNTHTXNFEE"].ToString();
                        DataList.Add(data);
                        #endregion params
                    }

                    ListHeader.DataList = DataList;
                    
                }
                #endregion data

            }
            catch { }
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
    public class ProductGetV2RQ
    {
        public string user { get; set; }
        public string pwd { get; set; }
        public string device_id { get; set; }
        public string app_vName { get; set; }
        public string mac_address { get; set; }
    }

}