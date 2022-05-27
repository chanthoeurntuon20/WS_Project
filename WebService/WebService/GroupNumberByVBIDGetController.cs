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
    public class GroupNumberByVBIDGetController : ApiController
    {
        public IEnumerable<GroupNumberByVBIDModel> Get(string api_name, string api_key, string json)////json=[{"user":"none","pwd":"none","device_id":"none","app_vName":"none","criteriaValue":"005515"}]
        {
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "", criteriaValue = "";
            List<GroupNumberByVBIDModel> RSData = new List<GroupNumberByVBIDModel>();
            try
            {
                #region api
                string[] CheckApi = c.CheckApi(api_name, api_key);
                ERR = CheckApi[0];
                SMS = CheckApi[1];
                #endregion api

                #region json
                string UserID = "";
                if (ERR != "Error")
                {
                    string[] CheckJson = c.CheckJson(json);
                    ERR = CheckJson[0];
                    if (ERR == "Error")
                    {
                        SMS = CheckJson[1];
                    }
                    else
                    {
                        UserID = CheckJson[1];
                        criteriaValue = CheckJson[2];
                    }
                }
                #endregion json

                #region data
                if (ERR != "Error")
                {
                    GroupNumberByVBIDModel ListHeader = new GroupNumberByVBIDModel();
                    ListHeader.ERR = ERR;
                    ListHeader.SMS = SMS;

                    List<GroupNumberByVBIDList> DataList = new List<GroupNumberByVBIDList>();

                    DataTable dt = c.ReturnDT("exec T24_GetGroupNumberByVBID @UserID='" + UserID + "',@VBID='" + criteriaValue + "'");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        #region params
                        GroupNumberByVBIDList data = new GroupNumberByVBIDList();
                        data.GroupNumber = dt.Rows[i]["GroupNumber"].ToString();
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
            }
            #region if Error
            if (ERR == "Error")
            {
                GroupNumberByVBIDModel CustHeader = new GroupNumberByVBIDModel();
                CustHeader.ERR = ERR;
                CustHeader.SMS = SMS;
                CustHeader.DataList = null;
                RSData.Add(CustHeader);
            }
            #endregion if Error

            return RSData;
        }

    }
    public class GroupNumberByVBIDModel
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
        public List<GroupNumberByVBIDList> DataList { get; set; }
    }
    public class GroupNumberByVBIDList
    {
        public string GroupNumber { get; set; }
    }


}