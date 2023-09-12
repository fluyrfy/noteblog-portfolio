using System;
using System.Text;
using System.Web.Security;
using System.Web.UI;
using MySql.Data.MySqlClient;
using noteblog.Utils;

namespace noteblog
{
    public partial class Verify : Page
    {
        private Logger log = new Logger(typeof(Verify).Name);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    string msg = "Token has been lost, please try again";
                    string encryptedCode = Request.QueryString["token"];
                    if (!string.IsNullOrEmpty(Request.QueryString["token"]))
                    {

                        byte[] encryptedBytes = hexStringToByteArray(encryptedCode);
                        byte[] decryptedBytes = MachineKey.Unprotect(encryptedBytes, null);
                        string decryptedString = Encoding.UTF8.GetString(decryptedBytes);
                        string[] parts = decryptedString.Split('_'); // 使用分隔符拆分
                        if (parts.Length == 2)
                        {
                            string email = parts[0];
                            string code = parts[1];
                            using (MySqlConnection con = DatabaseHelper.GetConnection())
                            {
                                MySqlCommand cmd = new MySqlCommand("SELECT verification_code, is_verified FROM users WHERE email = @email", con);
                                cmd.Parameters.AddWithValue("@email", email);
                                con.Open();
                                MySqlDataReader reader = cmd.ExecuteReader();
                                if (reader.Read())
                                {
                                    if ((bool)reader["is_verified"])
                                    {
                                        msg = "This user has already been verified";
                                    }
                                    else
                                    {
                                        if (reader["verification_code"].ToString() == code)
                                        {
                                            using (MySqlConnection con_verify = DatabaseHelper.GetConnection())
                                            {

                                                cmd.CommandText = "UPDATE users SET is_verified = @isVerified WHERE email = @email";

                                                cmd.Parameters.AddWithValue("@isVerified", true);
                                                cmd.Connection = con_verify;
                                                con_verify.Open();
                                                msg = cmd.ExecuteNonQuery() > 0 ? "Account verification successful" : "Account verification error";
                                            }
                                        }
                                        else
                                        {
                                            msg = "Account verification failed";
                                        }
                                    }
                                }
                            }
                        }
                    }
                    litMsg.Text = msg;
                    log.Info($"Verification msg: {msg}");
                }
                catch (Exception ex) { log.Info($"Verification exception: {ex}"); }
                finally
                {
                    //Response.Cache.SetNoStore();
                    //Response.Cache.AppendCacheExtension("no-cache");
                    Response.Expires = 0;
                    Response.Headers.Add("Refresh", "5;url=Sign");
                }

            }
        }

        public static byte[] hexStringToByteArray(string hexString)
        {
            hexString = hexString.Replace("-", "");
            int numberChars = hexString.Length;
            byte[] bytes = new byte[numberChars / 2];
            for (int i = 0; i < numberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
            }
            return bytes;
        }
    }
}