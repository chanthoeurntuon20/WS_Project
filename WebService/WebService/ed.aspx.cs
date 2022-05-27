using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebService
{
    public partial class ed : System.Web.UI.Page
    {
        string key = "TabletAMK*&*2014";
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region ed
        public byte[] Decrypt(byte[] encryptedData, RijndaelManaged rijndaelManaged)
        {
            byte[] numArray = rijndaelManaged.CreateDecryptor().TransformFinalBlock(encryptedData, 0, (int)encryptedData.Length);
            return numArray;
        }

        public string Decrypt(string encryptedText, string key)
        {
            byte[] numArray = Convert.FromBase64String(encryptedText);
            string str = Encoding.UTF8.GetString(this.Decrypt(numArray, this.GetRijndaelManaged(key)));
            return str;
        }
        
        public byte[] Encrypt(byte[] plainBytes, RijndaelManaged rijndaelManaged)
        {
            byte[] numArray = rijndaelManaged.CreateEncryptor().TransformFinalBlock(plainBytes, 0, (int)plainBytes.Length);
            return numArray;
        }

        public string Encrypt(string plainText, string key)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(plainText);
            string base64String = Convert.ToBase64String(this.Encrypt(bytes, this.GetRijndaelManaged(key)));
            return base64String;
        }

        public RijndaelManaged GetRijndaelManaged(string secretKey)
        {
            byte[] numArray = new byte[16];
            byte[] bytes = Encoding.UTF8.GetBytes(secretKey);
            Array.Copy(bytes, numArray, Math.Min((int)numArray.Length, (int)bytes.Length));
            RijndaelManaged rijndaelManaged = new RijndaelManaged()
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7,
                KeySize = 128,
                BlockSize = 128,
                Key = numArray,
                IV = numArray
            };
            return rijndaelManaged;
        }
        #endregion

        protected void bten_Click(object sender, EventArgs e)
        {
            try
            {
                txtde.Text = this.Encrypt(txten.Text.Trim(), key);
            }
            catch
            {
            }
        }
        protected void btde_Click(object sender, EventArgs e)
        {
            try
            {
                txten.Text = this.Decrypt(txtde.Text.Trim(), key);
            }
            catch
            {
            }
        }
    }
}