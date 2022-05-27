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
    public class VBByOfficeIDGetController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<VBByOfficeIDModel> Get(string api_name, string api_key, string json)////json=[{"user":"none","pwd":"none","device_id":"none","app_vName":"none","criteriaValue":"KH0010101"}]
        {
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "", OfficeID="";
            List<VBByOfficeIDModel> RSData = new List<VBByOfficeIDModel>();
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
                        OfficeID = CheckJson[2];
                    }
                }
                #endregion json

                #region data
                if (ERR != "Error")
                {
                    VBByOfficeIDModel ListHeader = new VBByOfficeIDModel();
                    ListHeader.ERR = ERR;
                    ListHeader.SMS = SMS;

                    List<VBByOfficeIDList> DataList = new List<VBByOfficeIDList>();

                    DataTable dt = c.ReturnDT("exec T24_GetVBByOfficeID @UserID='" + UserID + "',@OfficeID='"+ OfficeID + "'");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        #region params
                        VBByOfficeIDList data = new VBByOfficeIDList();
                        data.VBID = dt.Rows[i]["VBID"].ToString();
                        data.VBName = dt.Rows[i]["VBName"].ToString();
                        data.Status = dt.Rows[i]["Status"].ToString();
                        data.COID = dt.Rows[i]["COID"].ToString();
                        data.MeetingDate = dt.Rows[i]["MeetingDate"].ToString();
                        data.ExpireDate = dt.Rows[i]["ExpireDate"].ToString();
                        data.COIDRotate = dt.Rows[i]["COIDRotate"].ToString();
                        data.GSTREET = dt.Rows[i]["GSTREET"].ToString();
                        data.GADDRESS = dt.Rows[i]["GADDRESS"].ToString();
                        data.GTOWN = dt.Rows[i]["GTOWN"].ToString();
                        data.AMKPROVINCE = dt.Rows[i]["AMKPROVINCE"].ToString();
                        data.AMKDISTRICT = dt.Rows[i]["AMKDISTRICT"].ToString();
                        data.AMKCOMMUNE = dt.Rows[i]["AMKCOMMUNE"].ToString();
                        data.AMKVILLAGE = dt.Rows[i]["AMKVILLAGE"].ToString();
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
                VBByOfficeIDModel CustHeader = new VBByOfficeIDModel();
                CustHeader.ERR = ERR;
                CustHeader.SMS = SMS;
                CustHeader.DataList = null;
                RSData.Add(CustHeader);
            }
            #endregion if Error

            return RSData;
        }

    }
    public class VBByOfficeIDModel
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
        public List<VBByOfficeIDList> DataList { get; set; }
    }
    public class VBByOfficeIDList
    {
        public string VBID { get; set; }
        public string VBName { get; set; }
        public string Status { get; set; }
        public string COID { get; set; }
        public string MeetingDate { get; set; }
        public string ExpireDate { get; set; }
        public string COIDRotate { get; set; }
        public string GSTREET { get; set; }
        public string GADDRESS { get; set; }
        public string GTOWN { get; set; }
        public string GPOSTCODE { get; set; }
        public string AMKPROVINCE { get; set; }
        public string AMKDISTRICT { get; set; }
        public string AMKCOMMUNE { get; set; }
        public string AMKVILLAGE { get; set; }
    }


}