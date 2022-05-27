using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebService
{
    public class ProductGetController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<ProductModel> Get(string api_name, string api_key, string json)////json=[{"user":"01804","pwd":"040882","device_id":"352405061542333","app_vName":"1.6"}]
        {
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "";
            List<ProductModel> RSData = new List<ProductModel>();
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
                    }
                }
                #endregion json

                #region data
                if (ERR != "Error")
                {
                    ProductModel ListHeader = new ProductModel();
                    ListHeader.ERR = ERR;
                    ListHeader.SMS = SMS;

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
                ProductModel ListHeader = new ProductModel();
                ListHeader.ERR = ERR;
                ListHeader.SMS = SMS;
                ListHeader.DataList = null;
                RSData.Add(ListHeader);
            }
            #endregion if Error

            return RSData;
        }
    
    }
}