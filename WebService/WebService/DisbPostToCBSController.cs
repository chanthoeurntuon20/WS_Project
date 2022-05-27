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
    public class DisbPostToCBSController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Post([FromUri]string api_name, string api_key, string json)
        {
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "";
            try {
                #region api
                string[] CheckApi = c.CheckApi(api_name, api_key);
                ERR = CheckApi[0];
                SMS = CheckApi[1];
                #endregion api
                #region json to object     
                string user = "", pwd = "", DeviceID = "";
                DisbPostToCBSModel jObj = null;
                if (ERR != "Error")
                {
                    try
                    {
                        jObj = JsonConvert.DeserializeObject<DisbPostToCBSModel>(json);
                        user = jObj.user;
                        pwd = jObj.pwd;
                        DeviceID = jObj.device_id;
                    }
                    catch
                    {
                        ERR = "Error";
                        SMS = "Invalid JSON";
                    }
                }
                #endregion json to object
                #region - get T24 path
                string T24PathStr = "", T24PathUsername = "", T24PathPwd = "", T24Server = "";
                if (ERR != "Error") {
                    try
                    {
                        DataTable dt = new DataTable();
                        dt = c.ReturnDT("select * from T24_Path where PathID=1");
                        T24PathStr = dt.Rows[0]["PathStr"].ToString();
                        T24PathUsername = dt.Rows[0]["Username"].ToString();
                        T24PathPwd = dt.Rows[0]["Pwd"].ToString();
                        T24Server = dt.Rows[0]["Server"].ToString();
                    }
                    catch
                    {
                        ERR = "Error";
                        SMS = "Get T24 Path Error";
                    }
                }
                #endregion - T24 path
                #region dis
                try
                {
                    #region DisbParam
                    foreach (var l in jObj.DisbParam) {
                        #region param
                        string TraDate = l.TraDate;
                        string COUserID = l.COUserID;
                        string DataType = l.DataType;
                        string UserID = l.UserID;
                        #endregion param
                        #region CSV
                        string sql = "EXEC T24_GetPrePostDataDetailDisbToPost @TraDate='" + TraDate + "',@COUserID='" + COUserID
                            + "',@DataType='" + DataType + "',@UserID='" + UserID + "'";
                            DataTable dtd = new DataTable();
                            dtd = c.ReturnDT(sql);
                            //if (dtd.Rows.Count > 0)
                            //{
                                string DisbPostID = "";
                                for (int i = 0; i < dtd.Rows.Count; i++)
                                {
                                    DisbPostID += dtd.Rows[i]["DisbPostID"].ToString() + ",";
                                }
                                if (dtd.Rows.Count > 0) {
                                    DisbPostID = DisbPostID.Substring(0, DisbPostID.Length - 1);
                                }
                            
                                //////////////////////////////////write file for T24
                                string fName = "DISB_POST_" + UserID + "_" + DateTime.Now.ToString("yyyy.MM.dd.HH.mm.ss.fff") + "_" + "_" + DateTime.Now.ToString("yyyy.MM.dd") + ".csv";
                                DataTable dt_clone = new DataTable();
                                dt_clone.TableName = "CloneTable";
                                dt_clone = dtd.Copy();
                                dt_clone.Columns.Remove("DisbPostID");
                                string[] rs = c.WriteCSVFile(dt_clone, fName, T24PathStr, T24PathUsername, T24PathPwd, T24Server);
                                //PostDisb = rs[1];
                                string PostStatusID = "2";
                                if (rs[0] == "1")
                                {
                                    PostStatusID = "3";
                                }
                                c.ReturnDT("update T24_DisbPost set PostStatusID='" + PostStatusID + "',fName='" + fName + "',CBSStatusID='1' where DisbPostID in (select * from Split2('" + DisbPostID + "',','))");
                            //}
                        #endregion CSV
                    }
                    #endregion DisbParam                        
                }
                catch (Exception ex) { }
                #endregion


            }
            catch { }

            return new string[] { ERR, SMS };
        }

        
    }
}