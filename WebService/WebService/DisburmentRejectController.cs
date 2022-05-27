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
    public class DisburmentRejectController : ApiController
    {
        public IEnumerable<DisbResModel> Post([FromUri]string api_name, string api_key, string username, [FromBody]string json)
        {

            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "", MIIDList = "", UserID = "";
            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            List<DisbResModel> RSData = new List<DisbResModel>();
            List<DisbResData> RSDisbData = new List<DisbResData>();
            List<MIImgListRP> MIImgList = new List<MIImgListRP>();
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
                DisbDataModel jObj = null;
                string user = "", pwd = "", device_id = "", app_vName = "", mac_address = "";
                #region json
                if (ERR != "Error")
                {
                    try
                    {
                        jObj = JsonConvert.DeserializeObject<DisbDataModel>(json);
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
                    UserID = dts.Rows[0]["UserID"].ToString();
                }
                #endregion get userid

                #region DisbReject

                string isAllow = "0";

                try
                {

                    DataTable dt = c.ReturnDT("select AllowUploadRepayDisbTime from tblUser u left join tblUserDec d on d.UserID=u.UserID where d.UserName='" + username + "'");
                    isAllow = dt.Rows[0][0].ToString();

                }
                catch (Exception ex) { }

                DataTable dtDisbReject = new DataTable();
                string DisbRejID = "", DisbID = "", AAID = "", LookID = "", LookUpDescription = "", Remark = "", CreateDate = "", CreateBy = "", GPS = "";
                if (ERR != "Error")
                {
                    foreach (DisbResData lis in jObj.DisbReject)
                    {
                        //DisbRejID = lis.DisbRejID;
                        DisbID = lis.DisbID;
                        AAID = lis.AAID;
                        LookID = lis.LookID;
                        LookUpDescription = lis.LookUpDescription;
                        Remark = lis.Remark;
                        CreateDate = lis.CreateDate;
                        CreateBy = lis.CreateBy;
                        GPS = lis.GPS;

                        if (isAllow != "0")
                        {
                            c.ReturnDT("exec disburment_reject_ins '',N'" + DisbID + "',N'"
                            + AAID + "',N'" + LookID + "',N'" + LookUpDescription + "',N'"
                            + Remark + "',N'" + CreateDate + "',N'" + CreateBy + "',N'" + GPS + "'");
                        }
                        else
                        {
                            ERR = "Error";
                            SMS = "Uploading is not allow";
                        }
                    }
                }
                #endregion

                #region RepayMI
                DataTable dtRepayMI = new DataTable();
                DataTable dtMICardList = new DataTable();

                if (ERR != "Error")
                {
                    string IDMI = "";

                    string MIID = "", DisbRejIDRepay = "", RepayID = "", RepayPostID = "", RepayMIID = "", AAIDRepay = "", CID = "", CustomerName = "", MITypeID = "", RemarkRepay = "", CreateDateRepay = "", CreateByRepay = "", GPSRepay = "", RVRef = "";
                    foreach (RepayMI_ResData lis in jObj.RepayMIS)
                    {

                        try
                        {
                            DataTable dts = c.ReturnDT("select ISNULL(MAX(MIID),0) + 1 from V2_tbl_repay_mi ");
                            IDMI = dts.Rows[0][0].ToString();

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
                        CreateByRepay = UserID;
                        GPSRepay = lis.GPS;
                        RVRef = lis.RVRef;

                        try
                        {
                            c.ReturnDT("exec repay_mi_ins '" + IDMI + "','" + DisbRejIDRepay + "','"
                                + DisbID + "','" + RepayID + "','" + RepayPostID + "','" + RepayMIID + "','"
                                + AAIDRepay + "','" + CID + "','" + CustomerName + "','" + MITypeID + "','"
                                + RemarkRepay + "','" + CreateDateRepay + "','" + CreateByRepay + "','" + GPSRepay + "',N'" + RVRef + "'");
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
                                detailDT.Columns.AddRange(new DataColumn[5] {
                                new DataColumn("MIID",typeof(string)),
                                new DataColumn("MICard",typeof(string)),
                                new DataColumn("EffectiveDate",typeof(string)),
                                new DataColumn("ExpireDate",typeof(string)),
                                new DataColumn("RelativeTypeID",typeof(string))
                            });

                                foreach (MICardList ls in lis.MICardList)
                                {
                                    detailDT.Rows.Add(IDMI, ls.MICard, ls.EffectiveDate, ls.ExpireDate, ls.RelativeTypeID);
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

                                foreach (MIImgList ImgLs in lis.MIImgList)
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
                    MIImgListRP dataImg = new MIImgListRP();
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

            DisbResModel data = new DisbResModel();
            data.ERR = ERR;
            data.SMS = SMS;
            data.MIImgListRP = MIImgList;
            RSData.Add(data);

            return RSData;
        }
    }


}


public class DisbResModel
{
    public string ERR { get; set; }
    public string SMS { get; set; }
    public List<MIImgListRP> MIImgListRP { get; set; }
}

public class DisbDataModel
{
    public string user { get; set; }
    public string pwd { get; set; }
    public string device_id { get; set; }
    public string app_vName { get; set; }
    public string mac_address { get; set; }
    public List<DisbResData> DisbReject { get; set; }
    public List<RepayMI_ResData> RepayMIS { get; set; }
}
public class DisbResData
{
    public string DisbRejID { get; set; }
    public string DisbID { get; set; }
    public string AAID { get; set; }
    public string LookID { get; set; }
    public string LookUpDescription { get; set; }
    public string Remark { get; set; }
    public string CreateDate { get; set; }
    public string CreateBy { get; set; }
    public string GPS { get; set; }
}


public class RepayMI_ResData
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
    public List<MICardList> MICardList { get; set; }
    public List<MIImgList> MIImgList { get; set; }
}

public class MICardList
{
    public string MIDetailID { get; set; }
    public string MIID { get; set; }
    public string MICard { get; set; }
    public string EffectiveDate { get; set; }
    public string ExpireDate { get; set; }
    public string RelativeTypeID { get; set; }
}

public class MIImgList
{
    public string clientID { get; set; }
    public string MIID { get; set; }
    public string ImgNameOri { get; set; }
    public string ImgName { get; set; }
    public string imgExt { get; set; }
}

public class MIImgListRP
{
    public string MIImageID { get; set; }
    public string clientID { get; set; }
    public string MIID { get; set; }
    public string ImgNameOri { get; set; }
    public string ImgName { get; set; }
    public string imgExt { get; set; }
}