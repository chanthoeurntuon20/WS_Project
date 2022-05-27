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
    public class AddressGetV2Controller : ApiController
    {
        // GET api/<controller>
        //public IEnumerable<AddressModel> Post([FromUri]string api_name, string api_key, [FromBody]string json)
        public IEnumerable<AddressModel> Post([FromUri]string p, [FromUri]string msgid)//p=dskjndsvkdjvnvsvnfd -> SessionID || json="{\"device_id\":\"355755085347904\",\"app_vName\":\"1.6\",\"mac_address\":\"123456789\",\"sdk\":\"29\",\"isRoot\":\"0\",\"deviceTime\":\"2020-09-21 14:53:20.123\"}"
        {
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "", ExSMS = "", ERRCode = "";
            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            List<AddressModel> RSData = new List<AddressModel>();
            string ControllerName = "AddressGetV2";
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
                    AddressModel ListHeader = new AddressModel();
                    ListHeader.ERR = ERR;
                    ListHeader.SMS = SMS;
                    ListHeader.ERRCode = ERRCode;

                    List<AddressList> DataList = new List<AddressList>();

                    DataTable dt = c.ReturnDT("exec T24_GetAddressByDevice @UserID='" + UserID + "'");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        #region params
                        AddressList data = new AddressList();
                        data.ID = dt.Rows[i]["ID"].ToString();
                        data.Name = dt.Rows[i]["Name"].ToString();
                        data.ParentID = dt.Rows[i]["ParentID"].ToString();
                        data.LevelID = dt.Rows[i]["LevelID"].ToString();
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
                ExSMS = ex.Message.ToString();
            }
            #region if Error
            if (ERR == "Error")
            {
                AddressModel ListHeader = new AddressModel();
                ListHeader.ERR = ERR;
                ListHeader.SMS = SMS;
                ListHeader.ERRCode = ERRCode;
                ListHeader.DataList = null;
                RSData.Add(ListHeader);
            }
            #endregion if Error

            return RSData;
        }


    }
    

}