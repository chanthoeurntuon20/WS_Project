using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using WebService.Models.Req.Reports;
using WebService.Models.Res.Reports;

namespace WebService.Repositories
{
    public class CommissionRepo
    {
        public CommissionReportByOfficeIDGetV21Res GetCommissionFinanceByOfficeID(CommissionReportByOfficeIDGetV21Req req)
        {
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "";
            string ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            var RSData = new CommissionReportByOfficeIDGetV21Res();
            var RSDataList = new List<WebService.Models.Res.Reports.CommissionReportByOfficeIDGetV21List>();
            string ControllerName = "CommissionReportFinanceByOfficeIDGetV21";
            string FileNameForLog = req.User + "_" + req.App_vName + "_" + ServerDate.Replace("-", "_").Replace(" ", "_").Replace(":", "_");
            #region data
            if (ERR != "Error")
            {
                DataTable dt = c.ReturnDT("exec T24_GetCommissionFinanceReportByOfficeID_V2 @OfficeID='" + req.OfficeID + "',@FDate='" + req.FromDate + "',@TDate='" + req.ToDate + "'");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string ProspectCode = dt.Rows[i]["ProspectCode"].ToString();
                    string OfficeName = dt.Rows[i]["OfficeName"].ToString();
                    string COName = dt.Rows[i]["COName"].ToString();
                    string ClientName = dt.Rows[i]["ClientName"].ToString();
                    string VBName = dt.Rows[i]["VBName"].ToString();
                    string AAID = dt.Rows[i]["AAID"].ToString();
                    string ProductID = dt.Rows[i]["ProductID"].ToString();
                    string DisbDate = dt.Rows[i]["DisbDate"].ToString();
                    string Currency = dt.Rows[i]["Currency"].ToString();
                    string ActualDisbAmt = dt.Rows[i]["ActualDisbAmt"].ToString();
                    string Mnemonic = dt.Rows[i]["Mnemonic"].ToString();
                    string OfficeID = dt.Rows[i]["OfficeID"].ToString();
                    string CommissionAmt = dt.Rows[i]["CommissionAmt"].ToString();
                    
                    var  DataList = new WebService.Models.Res.Reports.CommissionReportByOfficeIDGetV21List();
                    DataList.ProspectCode = ProspectCode;
                    DataList.OfficeName = OfficeName;
                    DataList.COName = COName;
                    DataList.ClientName = ClientName;
                    DataList.VBName = VBName;
                    DataList.AAID = AAID;
                    DataList.ProductID = ProductID;
                    DataList.DisbDate = DisbDate;
                    DataList.Currency = Currency;
                    DataList.ActualDisbAmt = ActualDisbAmt;
                    DataList.Mnemonic = Mnemonic;
                    DataList.OfficeID = OfficeID;
                    DataList.CommissionAmt = CommissionAmt;
                    RSDataList.Add(DataList);
                }
                RSData.ERR = ERR;
                RSData.SMS = SMS;
                RSData.DataList = RSDataList;
            }
            try
            {
                var jsonRS = new JavaScriptSerializer().Serialize(RSData);
                c.T24_AddLog(FileNameForLog, "RS", jsonRS.ToString(), ControllerName);
            }
            catch { }
            return RSData;
        }
        #endregion
    }
}