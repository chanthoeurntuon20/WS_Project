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
    public class AMRotateUpdateController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<AMRotateUpdateResModel> Get(string api_name, string api_key)
        {
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "";
            try
            {
                c.ReturnDT("exec T24_VBLinkCOIDRotateUpdate");
                //#region get list
                //DataTable dt = c.ReturnDT("select * from T24_VBLink where ExpireDate<=CAST(GETDATE() as date) and COID!=COIDRotate");
                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    string VBID = dt.Rows[i]["VBID"].ToString();
                //    //Desc19 = "Desc: Update " + i.ToString() + "/" + dt.Rows.Count.ToString() + " | " + VBID;
                //    //backgroundWorker19.ReportProgress(10 + i);
                //    c.ReturnDT("update T24_VBLink set COIDRotate=COID where VBID='" + VBID + "'");
                //}
                //#endregion get list
            }
            catch
            {
                ERR = "Error";
                SMS = "Something was wrong";
            }
            List<AMRotateUpdateResModel> RSData = new List<AMRotateUpdateResModel>();
            AMRotateUpdateResModel ResSMS = new AMRotateUpdateResModel();
            ResSMS.ERR = ERR;
            ResSMS.SMS = SMS;
            RSData.Add(ResSMS);
            return RSData;
        }
        
    }
    public class AMRotateUpdateResModel
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
    }
}