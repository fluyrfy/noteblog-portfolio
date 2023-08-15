using System;
using System.Web.UI;
using noteblog.Utils;
using BCrypt;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Web.Security;
using System.Web;
using System.Net.Mail;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Org.BouncyCastle.Utilities.Encoders;
using System.Linq;

namespace noteblog
{
    public partial class Verify : Page
    {
        private Logger log = new Logger(typeof(Verify).Name);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string msg = "Token has been lost, please try again";
                string encryptedCode = Request.QueryString["token"].Replace(" ", "+");
                if (!string.IsNullOrEmpty(Request.QueryString["token"]))
                {
                    byte[] encryptedBytes = Convert.FromBase64String(encryptedCode);
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
                Response.Headers.Add("Refresh", "5;url=Sign");
            }
        }
    }
}