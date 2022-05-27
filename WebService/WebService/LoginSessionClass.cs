using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebService
{
    public class LoginSessionClass
    {
        Class1 c = new Class1();
        string secureSeperator = "|";
        public string[] CreateSession(string FileNameForLog, string UserID,string Expire,string api_name)//FileNameForLog|SS|UserID|SS|Expire|SS|api_name
        {
            string[] rs = new string[4];
            string ERR = "", SMS = "", ExSMS = "",rsStr="";
            try
            {//yyyy_MM_dd_HH_mm_ss_fffxopfcb187gdfadsd495faxc1xopfcb187gdfadsd495faxc30 -> UN3S0aqL4/CrcO6gl2z4HCfMnpljTfb1skFBLAlmnb6UL5oE3npODK2u+zTaQi6sTC6yPOS/sbExeflZasKihQu1cJzOQkU1BdIclZnJ8rM=
                string str = FileNameForLog + secureSeperator + UserID + secureSeperator + Expire + secureSeperator + api_name;
                rsStr = c.Encrypt(str, c.SeekKeyGet());
                //insert into DB
                SqlConnection Con1 = new SqlConnection(c.ConStr());
                Con1.Open();
                SqlCommand Com1 = new SqlCommand();
                Com1.Connection = Con1;
                string sql = "exec sp_LoginSession @UserID=@UserID,@Session=@Session,@Expire=@Expire,@FileNameForLog=@FileNameForLog";
                Com1.CommandText = sql;
                Com1.Parameters.Clear();
                Com1.Parameters.AddWithValue("@UserID", UserID);
                Com1.Parameters.AddWithValue("@Session", rsStr);
                Com1.Parameters.AddWithValue("@Expire", Expire);
                Com1.Parameters.AddWithValue("@FileNameForLog", FileNameForLog);
                DataTable dt1 = new DataTable();
                dt1.Load(Com1.ExecuteReader());
                ERR = dt1.Rows[0]["ERR"].ToString();
                SMS = dt1.Rows[0]["SMS"].ToString();
                Con1.Close();
            } catch(Exception ex) {
                ERR = "Error";
                ExSMS = ex.Message.ToString();
                //get sms
                string[] str = c.GetSMSByMsgID("6");
                ERR = str[0];
                if (ERR == "Error")
                {
                    SMS = str[1];
                    ExSMS = ExSMS + "|" + str[2];
                }
                else
                {
                    SMS = str[3];
                }
                ERR = "Error";
            }
            rs[0] = ERR;
            rs[1] = SMS;
            rs[2] = ExSMS;
            rs[3] = rsStr;
            return rs;
        }


    }
}