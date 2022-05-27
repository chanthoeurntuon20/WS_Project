using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;

namespace WebService
{
    [BasicAuthentication]
    public class MiRegisterController : ApiController
    {
        public IEnumerable<MIResModel> Post([FromUri]string api_name, string api_key, string username, [FromBody]string json)
        {

            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "", MIIDList = "", IDMI = ""; ;
            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            List<MIResModel> RSData = new List<MIResModel>();
            List<MIDataModel> RsMIData = new List<MIDataModel>();
            List<MIMIImgListRP> MIImgList = new List<MIMIImgListRP>();
            string ControllerName = "MISystem_";
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
                MIDataModel jObj = null;
                string user = "", pwd = "", device_id = "", app_vName = "", mac_address = "";
                #region json
                if (ERR != "Error")
                {
                    try
                    {
                        jObj = JsonConvert.DeserializeObject<MIDataModel>(json);
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

                #region RepayMI
                DataTable dtRepayMI = new DataTable();
                DataTable dtMICardList = new DataTable();

                if (ERR != "Error")
                {


                    string DisbID = "", DisbRejIDRepay = "", RepayID = "", RepayPostID = "", RepayMIID = "", AAIDRepay = "", CID = "", CustomerName = "", MITypeID = "", RemarkRepay = "", CreateDateRepay = "", CreateByRepay = "", GPSRepay = "", RVRef = "", EventType = "";
                    foreach (MIRepayMI_ResData lis in jObj.RepayMIS)
                    {

                        try
                        {
                            if (lis.EventType == "INS")
                            {
                                DataTable dts = c.ReturnDT("select ISNULL(MAX(MIID),0) + 1 from V2_tbl_repay_mi ");
                                IDMI = dts.Rows[0][0].ToString();
                            }
                            else
                            {
                                IDMI = lis.MIID;
                            }
                        }
                        catch { }

                        #region RepayMI
                        DisbRejIDRepay = lis.DisbRejID;
                        if (DisbRejIDRepay == "") { DisbRejIDRepay = null; }
                        DisbID = lis.DisbID;
                        if (DisbID == "") { DisbID = null; }
                        RepayID = lis.RepayID;
                        if (RepayID == "") { RepayID = null; }
                        RepayPostID = lis.RepayPostID;
                        if (RepayPostID == "") { RepayPostID = null; }
                        RepayMIID = lis.RepayMIID;
                        if (RepayMIID == "") { RepayMIID = null; }
                        AAIDRepay = lis.AAID;
                        CID = lis.CID;
                        CustomerName = lis.CustomerName;
                        MITypeID = lis.MITypeID;
                        RemarkRepay = lis.Remark;
                        CreateDateRepay = lis.CreateDate;
                        CreateByRepay = lis.CreateBy;
                        GPSRepay = lis.GPS;
                        RVRef = lis.RVRef;
                        EventType = lis.EventType;

                        try
                        {
                            c.ReturnDT("exec repay_from_mi_ins '" + IDMI + "','" + DisbRejIDRepay + "','"
                                + DisbID + "','" + RepayID + "','" + RepayPostID + "','" + RepayMIID + "','"
                                + AAIDRepay + "','" + CID + "','" + CustomerName + "','" + MITypeID + "','"
                                + RemarkRepay + "','" + CreateDateRepay + "','" + CreateByRepay + "','" + GPSRepay + "',N'" + RVRef + "','" + EventType + "'");
                            MIIDList = MIIDList + "," + IDMI;
                        }
                        catch (Exception ex)
                        {
                            ERR = "Error";
                            SMS = "MI Main " + ex.Message.ToString();
                        }


                        #endregion

                        #region MICardList
                        if (ERR != "Error")
                        {
                            try
                            {
                                DataTable detailDT = new DataTable();
                                detailDT.Columns.AddRange(new DataColumn[13] {
                                new DataColumn("MIID",typeof(string)),
                                new DataColumn("MICard",typeof(string)),
                                new DataColumn("EffectiveDate",typeof(string)),
                                new DataColumn("ExpireDate",typeof(string)),
                                new DataColumn("RelativeTypeID",typeof(string)),

                                new DataColumn("mi_amount",typeof(string)),
                                new DataColumn("isPosting",typeof(string)),
                                new DataColumn("PostingRemark",typeof(string)),
                                new DataColumn("currency",typeof(string)),

                                new DataColumn("MIStatus",typeof(string)),
                                new DataColumn("CBSStatus",typeof(string)),
                                new DataColumn("MIPostRemark",typeof(string)),
                                new DataColumn("CBSPostRemark",typeof(string))

                            });

                                foreach (MIMICardList ls in lis.MICardList)
                                {
                                    detailDT.Rows.Add(
                                        IDMI, ls.MICard, ls.EffectiveDate, ls.ExpireDate, ls.RelativeTypeID, "30000", "3", "Create from MI System", "KHR", "3", "3", "Create from MI System", "Create from MI System");
                                }

                                if (detailDT.Rows.Count > 0)
                                {
                                    string consString = ConfigurationManager.AppSettings["ConStr"].ToString();
                                    using (SqlConnection con = new SqlConnection(consString))
                                    {
                                        using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                                        {
                                            //Set the database table name
                                            sqlBulkCopy.DestinationTableName = "dbo.V2_tbl_mi_detail";

                                            //[OPTIONAL]: Map the DataTable columns with that of the database table
                                            sqlBulkCopy.ColumnMappings.Add("MIID", "MIID");
                                            sqlBulkCopy.ColumnMappings.Add("MICard", "MICard");
                                            sqlBulkCopy.ColumnMappings.Add("EffectiveDate", "EffectiveDate");
                                            sqlBulkCopy.ColumnMappings.Add("ExpireDate", "ExpireDate");
                                            sqlBulkCopy.ColumnMappings.Add("RelativeTypeID", "RelativeTypeID");

                                            sqlBulkCopy.ColumnMappings.Add("mi_amount", "mi_amount");
                                            sqlBulkCopy.ColumnMappings.Add("isPosting", "isPosting");
                                            sqlBulkCopy.ColumnMappings.Add("PostingRemark", "PostingRemark");
                                            sqlBulkCopy.ColumnMappings.Add("currency", "currency");
                                            sqlBulkCopy.ColumnMappings.Add("MIStatus", "MIStatus");
                                            sqlBulkCopy.ColumnMappings.Add("CBSStatus", "CBSStatus");
                                            sqlBulkCopy.ColumnMappings.Add("MIPostRemark", "MIPostRemark");
                                            sqlBulkCopy.ColumnMappings.Add("CBSPostRemark", "CBSPostRemark");
                                            con.Open();
                                            sqlBulkCopy.WriteToServer(detailDT);
                                            con.Close();
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                ERR = "Error";
                                SMS = "MI Detail " + ex.Message.ToString();
                            }
                        }


                        #endregion


                        #region MIImgList

                        if (ERR != "Error")
                        {
                            try
                            {

                                DataTable imgDT = new DataTable();
                                imgDT.Columns.AddRange(new DataColumn[6] {
                                    new DataColumn("clientID",typeof(string)),
                                    new DataColumn("MIID",typeof(string)),
                                    new DataColumn("ImgNameOri",typeof(string)),
                                    new DataColumn("ImgName",typeof(string)),
                                    new DataColumn("imgExt",typeof(string)),
                                    new DataColumn("createDate",typeof(string))
                                });

                                string createDate = DateTime.Today.ToString("yyyy-MM-dd hh:mm:ss.fff");
                                string fileName = DateTime.Today.ToString("yyyyMMddhhmmssffff");

                                foreach (MIMIImgList ImgLs in lis.MIImgList)
                                {

                                    imgDT.Rows.Add(
                                        ImgLs.clientID,
                                        IDMI,
                                        ImgLs.ImgNameOri,
                                        ImgLs.clientID + '_' + IDMI + '_' + fileName,
                                        ImgLs.imgExt, createDate);

                                }

                                if (imgDT.Rows.Count > 0)
                                {
                                    string consString = ConfigurationManager.AppSettings["ConStr"].ToString();
                                    using (SqlConnection con = new SqlConnection(consString))
                                    {
                                        using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                                        {
                                            //Set the database table name
                                            sqlBulkCopy.DestinationTableName = "dbo.V2_tbl_mi_images";

                                            //[OPTIONAL]: Map the DataTable columns with that of the database table
                                            sqlBulkCopy.ColumnMappings.Add("clientID", "clientID");
                                            sqlBulkCopy.ColumnMappings.Add("MIID", "MIID");
                                            sqlBulkCopy.ColumnMappings.Add("ImgNameOri", "ImgNameOri");
                                            sqlBulkCopy.ColumnMappings.Add("ImgName", "ImgName");
                                            sqlBulkCopy.ColumnMappings.Add("imgExt", "imgExt");
                                            sqlBulkCopy.ColumnMappings.Add("createDate", "createDate");
                                            con.Open();
                                            sqlBulkCopy.WriteToServer(imgDT);
                                            con.Close();
                                        }
                                    }
                                }

                            }
                            catch (Exception ex)
                            {
                                ERR = "Error";
                                SMS = "MI Image " + ex.Message.ToString();
                            }
                        }

                        #endregion

                    }
                }
                #endregion

            }
            catch (Exception ex)
            {
                ERR = "Error";
                SMS = "Something was wrong at line:" + c.GetLineNumber(ex) + " | Ex:" + ex.Message.ToString();
            }

            if (ERR != "Error")
            {
                DataTable dts = new DataTable();
                dts = c.ReturnDT("exec V2_mi_images_by_MIList '" + MIIDList + "'");
                for (int i = 0; i <= dts.Rows.Count - 1; i++)
                {
                    MIMIImgListRP dataImg = new MIMIImgListRP();
                    #region list
                    dataImg.MIImageID = "" + dts.Rows[i]["MIImageID"];
                    dataImg.clientID = "" + dts.Rows[i]["clientID"];
                    dataImg.MIID = "" + dts.Rows[i]["MIID"];
                    dataImg.ImgNameOri = "" + dts.Rows[i]["ImgNameOri"];
                    dataImg.ImgName = "" + dts.Rows[i]["ImgName"];
                    dataImg.imgExt = "" + dts.Rows[i]["imgExt"];
                    MIImgList.Add(dataImg);
                    #endregion
                }
            }

            c.T24_AddLog(FileNameForLog, "RS", ERR + " " + SMS, ControllerName);

            MIResModel data = new MIResModel();
            data.ERR = ERR;
            data.SMS = SMS;
            data.MIID = IDMI;
            data.MIImgListRP = MIImgList;
            RSData.Add(data);

            return RSData;
        }
    }
}

public class MIResModel
{
    public string ERR { get; set; }
    public string SMS { get; set; }
    public string MIID { get; set; }
    public List<MIMIImgListRP> MIImgListRP { get; set; }
}

public class MIDataModel
{
    public string user { get; set; }
    public string pwd { get; set; }
    public string device_id { get; set; }
    public string app_vName { get; set; }
    public string mac_address { get; set; }
    public List<MIRepayMI_ResData> RepayMIS { get; set; }
}

public class MIRepayMI_ResData
{
    public string MIID { get; set; }
    public string DisbRejID { get; set; }
    public string DisbID { get; set; }
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
    public string RVRef { get; set; }
    public string EventType { get; set; }
    public List<MIMICardList> MICardList { get; set; }
    public List<MIMIImgList> MIImgList { get; set; }
}

public class MIMICardList
{
    public string MIDetailID { get; set; }
    public string MIID { get; set; }
    public string MICard { get; set; }
    public string EffectiveDate { get; set; }
    public string ExpireDate { get; set; }
    public string RelativeTypeID { get; set; }
}

public class MIMIImgList
{
    public string clientID { get; set; }
    public string MIID { get; set; }
    public string ImgNameOri { get; set; }
    public string ImgName { get; set; }
    public string imgExt { get; set; }
}

public class MIMIImgListRP
{
    public string MIImageID { get; set; }
    public string clientID { get; set; }
    public string MIID { get; set; }
    public string ImgNameOri { get; set; }
    public string ImgName { get; set; }
    public string imgExt { get; set; }
}