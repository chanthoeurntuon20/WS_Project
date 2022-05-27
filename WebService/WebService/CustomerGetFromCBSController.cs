using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace WebService
{
    [BasicAuthentication]
    public class CustomerGetFromCBSController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<CustomerGetFromCBSRes> Get(string api_name, string api_key)
        {
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "";
            try {
                #region api
                string[] CheckApi = c.CheckApi(api_name, api_key);
                ERR = CheckApi[0];
                SMS = CheckApi[1];
                #endregion api
                if (ERR != "Error") {
                    #region - get T24 path
                    string T24PathStr = "", T24PathUsername = "", T24PathPwd = "", T24Server = "";
                    DataTable dtT24Path = new DataTable();
                    dtT24Path = c.ReturnDT("select * from T24_Path where PathID=2");
                    T24PathStr = dtT24Path.Rows[0]["PathStr"].ToString();
                    T24PathUsername = dtT24Path.Rows[0]["Username"].ToString();
                    T24PathPwd = dtT24Path.Rows[0]["Pwd"].ToString();
                    T24Server = dtT24Path.Rows[0]["Server"].ToString();
                    #endregion - T24 path
                    #region connect to ftp
                    string host = T24Server;
                    string username = T24PathUsername;
                    string password = T24PathPwd;
                    string downsavepath = HttpContext.Current.Server.MapPath("~/Download/");
                    string remoteDirectory = T24PathStr;
                    using (var sftp = new SftpClient(host, username, password)) {
                        sftp.Connect();
                        var files = sftp.ListDirectory(remoteDirectory);
                        foreach (var file in files) {
                            if (!file.Name.StartsWith(".")) {
                                string remoteFileName = file.Name;
                                string CustFile = remoteFileName.Substring(0, 8);
                                if (CustFile == "CUSTOMER") {
                                    #region is file existed?
                                    if (!File.Exists(downsavepath + remoteFileName))
                                    {
                                        #region write file
                                        using (Stream fileStream = File.Create(downsavepath + remoteFileName))
                                        {
                                            sftp.DownloadFile(remoteDirectory + remoteFileName, fileStream);
                                        }
                                        #region read CVS
                                        string[,] values = c.LoadCsv(downsavepath + remoteFileName);
                                        int num_rows = values.GetUpperBound(0) + 1;
                                        int num_cols = values.GetUpperBound(1) + 1;
                                        #endregion read CVS
                                        for (int r = 0; r < num_rows; r++)
                                        {
                                            #region Params
                                            string CUSTID = values[r, 0];
                                            string CUSTTYPE = values[r, 1];
                                            string SHORTNAME = values[r, 2];
                                            string NAME1 = values[r, 3];
                                            string BIRTHINCORPDATE = values[r, 4];
                                            string AMKBIRTHPLACE = values[r, 5];
                                            string GENDER = values[r, 6];
                                            string MARITALSTATUS = values[r, 7];
                                            string RESIDEYN = values[r, 8];
                                            string NATIONALITY = values[r, 9];
                                            string RESIDENCE = values[r, 10];
                                            string AMKIDTYPE = values[r, 11];
                                            string AMKIDNO = values[r, 12];
                                            string AMKIDISSDATE = values[r, 13];
                                            string AMKIDEXDATE = values[r, 39];//AN
                                            string STREET = values[r, 15];
                                            string AMKPROVINCE = values[r, 16];
                                            string AMKDISTRICT = values[r, 17];
                                            string AMKCOMMUNE = values[r, 18];
                                            string AMKVILLAGE = values[r, 19];
                                            string POSTALCODE = values[r, 20];
                                            string TELMOBILE = values[r, 21];
                                            string EMAILADDRESS = values[r, 22];
                                            string AMKOCCUPTYPE = values[r, 23];
                                            string AMKOCCUPDET = values[r, 24];
                                            string SPMEMNO = values[r, 25];
                                            string SPNAME = values[r, 26];
                                            string AMKSPDOB = values[r, 27];
                                            string AMKSPIDTYPE = values[r, 28];
                                            string AMKSPIDNO = values[r, 29];
                                            string AMKSPIDISDT = values[r, 30];
                                            string AMKSPIDEXDT = values[r, 31];
                                            string PROFESSION = values[r, 32];
                                            string AMKPOVERTYST = values[r, 33];
                                            string NOOFDEPEND = values[r, 34];
                                            string MAININCOME = values[r, 35];
                                            string TITLE = values[r, 36];
                                            string AMKVILLAGEBK = values[r, 37];
                                            string AMKNOACTMEM = values[r, 38];
                                            #endregion Params
                                            #region add to db
                                            SqlConnection Con1 = new SqlConnection(c.ConStr());
                                            Con1.Open();
                                            SqlCommand Com1 = new SqlCommand();
                                            Com1.Connection = Con1;
                                            Com1.Parameters.Clear();
                                            Com1.CommandText = "exec T24_AddUpdateCustomer @CUSTID=@CUSTID,@CUSTTYPE=@CUSTTYPE,@SHORTNAME=@SHORTNAME,@NAME1=@NAME1,@BIRTHINCORPDATE=@BIRTHINCORPDATE,@AMKBIRTHPLACE=@AMKBIRTHPLACE,@GENDER=@GENDER,@MARITALSTATUS=@MARITALSTATUS,@RESIDEYN=@RESIDEYN,@NATIONALITY=@NATIONALITY,@RESIDENCE=@RESIDENCE,@AMKIDTYPE=@AMKIDTYPE,@AMKIDNO=@AMKIDNO,@AMKIDISSDATE=@AMKIDISSDATE,@AMKIDEXDATE=@AMKIDEXDATE,@STREET=@STREET,@AMKPROVINCE=@AMKPROVINCE,@AMKDISTRICT=@AMKDISTRICT,@AMKCOMMUNE=@AMKCOMMUNE,@AMKVILLAGE=@AMKVILLAGE,@POSTALCODE=@POSTALCODE,@TELMOBILE=@TELMOBILE,@EMAILADDRESS=@EMAILADDRESS,@AMKOCCUPTYPE=@AMKOCCUPTYPE,@AMKOCCUPDET=@AMKOCCUPDET,@SPMEMNO=@SPMEMNO,@SPNAME=@SPNAME,@AMKSPDOB=@AMKSPDOB,@AMKSPIDTYPE=@AMKSPIDTYPE,@AMKSPIDNO=@AMKSPIDNO,@AMKSPIDISDT=@AMKSPIDISDT,@AMKSPIDEXDT=@AMKSPIDEXDT,@PROFESSION=@PROFESSION,@AMKPOVERTYST=@AMKPOVERTYST,@NOOFDEPEND=@NOOFDEPEND,@MAININCOME=@MAININCOME,@TITLE=@TITLE,@AMKVILLAGEBK=@AMKVILLAGEBK,@AMKNOACTMEM=@AMKNOACTMEM";
                                            #region params
                                            Com1.Parameters.AddWithValue("@CUSTID", CUSTID);
                                            Com1.Parameters.AddWithValue("@CUSTTYPE", CUSTTYPE);
                                            Com1.Parameters.AddWithValue("@SHORTNAME", SHORTNAME);
                                            Com1.Parameters.AddWithValue("@NAME1", NAME1);
                                            Com1.Parameters.AddWithValue("@BIRTHINCORPDATE", BIRTHINCORPDATE);
                                            Com1.Parameters.AddWithValue("@AMKBIRTHPLACE", AMKBIRTHPLACE);
                                            Com1.Parameters.AddWithValue("@GENDER", GENDER);
                                            Com1.Parameters.AddWithValue("@MARITALSTATUS", MARITALSTATUS);
                                            Com1.Parameters.AddWithValue("@RESIDEYN", RESIDEYN);
                                            Com1.Parameters.AddWithValue("@NATIONALITY", NATIONALITY);
                                            Com1.Parameters.AddWithValue("@RESIDENCE", RESIDENCE);
                                            Com1.Parameters.AddWithValue("@AMKIDTYPE", AMKIDTYPE);
                                            Com1.Parameters.AddWithValue("@AMKIDNO", AMKIDNO);
                                            Com1.Parameters.AddWithValue("@AMKIDISSDATE", AMKIDISSDATE);
                                            Com1.Parameters.AddWithValue("@AMKIDEXDATE", AMKIDEXDATE);
                                            Com1.Parameters.AddWithValue("@STREET", STREET);
                                            Com1.Parameters.AddWithValue("@AMKPROVINCE", AMKPROVINCE);
                                            Com1.Parameters.AddWithValue("@AMKDISTRICT", AMKDISTRICT);
                                            Com1.Parameters.AddWithValue("@AMKCOMMUNE", AMKCOMMUNE);
                                            Com1.Parameters.AddWithValue("@AMKVILLAGE", AMKVILLAGE);
                                            Com1.Parameters.AddWithValue("@POSTALCODE", POSTALCODE);
                                            Com1.Parameters.AddWithValue("@TELMOBILE", TELMOBILE);
                                            Com1.Parameters.AddWithValue("@EMAILADDRESS", EMAILADDRESS);
                                            Com1.Parameters.AddWithValue("@AMKOCCUPTYPE", AMKOCCUPTYPE);
                                            Com1.Parameters.AddWithValue("@AMKOCCUPDET", AMKOCCUPDET);
                                            Com1.Parameters.AddWithValue("@SPMEMNO", SPMEMNO);
                                            Com1.Parameters.AddWithValue("@SPNAME", SPNAME);
                                            Com1.Parameters.AddWithValue("@AMKSPDOB", AMKSPDOB);
                                            Com1.Parameters.AddWithValue("@AMKSPIDTYPE", AMKSPIDTYPE);
                                            Com1.Parameters.AddWithValue("@AMKSPIDNO", AMKSPIDNO);
                                            Com1.Parameters.AddWithValue("@AMKSPIDISDT", AMKSPIDISDT);
                                            Com1.Parameters.AddWithValue("@AMKSPIDEXDT", AMKSPIDEXDT);
                                            Com1.Parameters.AddWithValue("@PROFESSION", PROFESSION);
                                            Com1.Parameters.AddWithValue("@AMKPOVERTYST", AMKPOVERTYST);
                                            Com1.Parameters.AddWithValue("@NOOFDEPEND", NOOFDEPEND);
                                            Com1.Parameters.AddWithValue("@MAININCOME", MAININCOME);
                                            Com1.Parameters.AddWithValue("@TITLE", TITLE);
                                            Com1.Parameters.AddWithValue("@AMKVILLAGEBK", AMKVILLAGEBK);
                                            Com1.Parameters.AddWithValue("@AMKNOACTMEM", AMKNOACTMEM);
                                            #endregion params
                                            Com1.ExecuteNonQuery();
                                            Con1.Close();
                                            #endregion add to db
                                        }
                                        #endregion write file
                                    }
                                    else {
                                        sftp.DeleteFile(remoteDirectory + remoteFileName);
                                    }
                                    #endregion is file existed?
                                }
                            }
                        }

                    }
                    #endregion connect to ftp
                }


            }
            catch (Exception ex){  }
            List<CustomerGetFromCBSRes> RSData = new List<CustomerGetFromCBSRes>();
            CustomerGetFromCBSRes ResSMS = new CustomerGetFromCBSRes();
            ResSMS.ERR = ERR;
            ResSMS.SMS = SMS;
            RSData.Add(ResSMS);
            return RSData;
        }
        

    }
    public class CustomerGetFromCBSRes
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
    }
}