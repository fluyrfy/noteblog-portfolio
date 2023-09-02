using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using noteblog.Utils;

namespace noteblog
{
    public partial class Sign : Page
    {
        private Logger log;

        protected void Page_Load(object sender, EventArgs e)
        {
            log = new Logger(typeof(Sign).Name);
            if (!IsPostBack)
            {
                string queryReset = Request.QueryString["reset"] as string;
                string queryEmail = Request.QueryString["email"] as string;
                string[] decryptEmail = EncryptionHelper.Decrypt(queryEmail).Split('|');
                if (!string.IsNullOrEmpty(queryReset))
                {
                    pnlResetPwd.Visible = true;
                    pnlConfirmEmail.Visible = true;
                    pnlResetExistPwd.Visible = false;
                    pnlSign.Visible = false;
                    if (!string.IsNullOrEmpty(queryEmail) && queryReset == "2" && decryptEmail.Length == 4)
                    {
                        pnlConfirmEmail.Visible = false;
                        pnlResetExistPwd.Visible = true;
                        string time = decryptEmail[2];
                        ViewState["ResetTime"] = time;
                        if (DateTime.TryParseExact(time, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime storedTime))
                        {
                            DateTime currentTime = DateTime.Now;
                            TimeSpan timeDifference = currentTime - storedTime;
                            if (timeDifference.TotalHours > 1)
                            {
                                setModalText("Link Expired", "Please request a new password reset.");
                            }
                        }
                    }
                }
                else
                {
                    pnlResetPwd.Visible = false;
                    pnlSign.Visible = true;
                }
            }
        }

        protected void btnConfirmEmail_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    log.Info("Starting to confirm account");
                    using (MySqlConnection con = DatabaseHelper.GetConnection())
                    {
                        MySqlCommand cmd = new MySqlCommand("SELECT email, verification_code FROM users WHERE email = @email", con);
                        cmd.Parameters.AddWithValue("@email", txtConfirmEmail.Text);
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                string combinedData = $"{reader["email"].ToString()}|{reader["verification_code"]}|{DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")}";
                                log.Debug($"Confirm email: {reader["email"] as string}, verification_code: {reader["verification_code"] as string}, date_time: {DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")}");
                                string hexString = EncryptionHelper.Encrypt(combinedData);
                                string link = $"Sign?reset=2&email={hexString}";
                                EmailHelper.SendVerificationEmail(txtConfirmEmail.Text, "Password Reset for Your Account", "Password Reset Instructions", "We received a request to reset the password for your account. To reset your password, please click the button below", "Reset Password", link);
                                setModalText("Confirm email", "Please check your email and reset your password");
                            }
                            log.Info("Account confirmed successfully");
                        }
                        else
                        {
                            log.Info("Failed to confirm account: email does not exist");
                            lblConfirmHint.Text = "Email does not exist";
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Account confirmation error", ex);
                    throw;
                }
                finally
                {
                    log.Info("End of confirm account method");
                }

            }
        }

        protected void btnResetPwd_Click(object sender, EventArgs e)
        {
            log.Info("Starting to reset password");
            try
            {

                if (DateTime.TryParseExact(ViewState["ResetTime"] as string, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime storedTime))
                {
                    DateTime currentTime = DateTime.Now;
                    TimeSpan timeDifference = currentTime - storedTime;
                    if (timeDifference.TotalHours > 1)
                    {
                        log.Info("Failed to reset password: link expired");
                        setModalText("Link Expired", "Please request a new password reset.");
                        return;
                    }
                    else
                    {
                        if (Page.IsValid)
                        {
                            string queryEmail = Request.QueryString["email"].ToString();
                            string hashEmail = EncryptionHelper.Decrypt(queryEmail);
                            string[] parts = hashEmail.Split('|');
                            if (parts.Length == 4)
                            {
                                string email = parts[0];
                                string code = parts[1];
                                using (MySqlConnection con = DatabaseHelper.GetConnection())
                                {
                                    MySqlCommand cmd = new MySqlCommand("SELECT verification_code from users WHERE email = @email", con);
                                    cmd.Parameters.AddWithValue("@email", email);
                                    con.Open();
                                    string dataCode = cmd.ExecuteScalar() as string;
                                    if (code == dataCode)
                                    {
                                        if (txtResetPwdConfirm.Text == txtResetPwd.Text)
                                        {
                                            string newPassword = BCrypt.Net.BCrypt.HashPassword(txtResetPwd.Text);
                                            MySqlCommand resetPwdCmd = new MySqlCommand("UPDATE users SET password_hash = @newPassword WHERE email = @email", con);
                                            resetPwdCmd.Parameters.AddWithValue("@email", email);
                                            resetPwdCmd.Parameters.AddWithValue("@newPassword", newPassword);
                                            int rowsAffected = resetPwdCmd.ExecuteNonQuery();

                                            if (rowsAffected > 0)
                                            {
                                                // 密碼欄位已成功修改
                                                log.Info("Password reset successfully");
                                                setModalText("Password Reset", "Your password has been reset.");
                                                return;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                setModalText("Password Reset Failed", "Password reset unsuccessful. Please check your information and try again.");
            }
            catch (Exception ex)
            {
                log.Error("Password reset error", ex);
                throw;
            }
            finally
            {
                log.Info("End of password reset method");
            }
        }

        protected void btnSignUp_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    List<TextBox> upTextBoxs = new List<TextBox> { txtUpEmail, txtUpName, txtUpPwd };
                    if (isTextValid(upTextBoxs))
                    {
                        string email = txtUpEmail.Text;
                        string name = txtUpName.Text;
                        string password = txtUpPwd.Text;

                        // check email unique
                        if (isEmailExists(email))
                        {
                            lblUpHint.Text = "Email already exists";
                            lblUpHint.ForeColor = System.Drawing.Color.Red;
                            return;
                        }

                        //string hashedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(txtUpPwd.Text, "SHA1");
                        // BCrypt.Net encrypt pwd
                        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
                        string verificationCode = generateVerificationCode();

                        // create new user
                        insertUserToDatabase(email, name, hashedPassword, verificationCode);

                        lblUpHint.Text = "Sign up success";
                        lblUpHint.ForeColor = System.Drawing.Color.Green;
                        sendVerifyEmail(name, email, verificationCode);
                        log.Debug($"New user info: {name} - {email}");
                        log.Info("User registration is successful");
                        setModalText("Sign up success", "Please check your email and verify your account");
                    }
                }
                catch (Exception ex)
                {
                    log.Error("User registration error", ex);
                }
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            modalBgPanel.Visible = false;
            modalContentPanel.Visible = false;
            Response.AddHeader("Cache-Control", "no-store, no-cache, must-revalidate");
            Response.Redirect("~/Sign.aspx");
        }

        protected void btnLogIn_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string errMsg = "";
                string userData = "";
                List<TextBox> inTextBoxs = new List<TextBox> { txtInEmail, txtInPwd };
                if (isTextValid(inTextBoxs) && isUserValid(txtInEmail.Text, txtInPwd.Text, out errMsg, out userData))
                {
                    var expiration = cbRememberMe.Checked ? DateTime.Now.AddDays(3) : DateTime.Now.AddHours(1);
                    var ticket = new FormsAuthenticationTicket(1, txtInEmail.Text, DateTime.Now, expiration, cbRememberMe.Checked, userData);
                    string encryptedTicket = FormsAuthentication.Encrypt(ticket);
                    HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    authCookie.Expires = expiration;
                    Response.Cookies.Add(authCookie);
                    Response.Redirect("~/Dashboard.aspx");
                }
                else
                {
                    lblInHint.Text = errMsg;
                    lblInHint.ForeColor = System.Drawing.Color.Red;
                }
            }
        }

        private string generateVerificationCode()
        {
            var guid = Guid.NewGuid().ToString();
            // 取GUID的前6位作為驗證碼
            return guid.Substring(0, 6).ToUpper();
        }


        private string generateVerifyLink(string email, string code)
        {
            string token = BitConverter.ToString(MachineKey.Protect(Encoding.UTF8.GetBytes($"{email}_{code}"), null));
            string domain = Request.Url.GetLeftPart(UriPartial.Authority);
            //domain = $"{currentUrl.Scheme}://{currentUrl.Host}:{currentUrl.Port}";
            return $"Verify?token={token}";
        }

        private void sendVerifyEmail(string userName, string userEmail, string verificationCode)
        {
            string link = generateVerifyLink(userEmail, verificationCode);
            EmailHelper.SendVerificationEmail(userEmail, "Account Verification for F.L.", "Please verify your email", "Verifying your email allows you to access all features on F.L.", "Verify My Account", link);
        }

        private bool isEmailExists(string email)
        {
            using (MySqlConnection con = DatabaseHelper.GetConnection())
            {
                string query = "SELECT COUNT(*) FROM users WHERE email = @email";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@email", email);
                con.Open();
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
        }

        private void insertUserToDatabase(string email, string name, string hashedPassword, string verificationCode)
        {
            using (MySqlConnection con = DatabaseHelper.GetConnection())
            {
                string query = "INSERT INTO users (email, name, password_hash, verification_code) VALUES (@email, @name, @password, @verification_code)";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@password", hashedPassword);
                cmd.Parameters.AddWithValue("@verification_code", verificationCode);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private void setModalText(string title, string content, bool visible = true)
        {
            modalBgPanel.Visible = visible;
            modalContentPanel.Visible = visible;
            litModalTitle.Text = title;
            litModalContent.Text = content;
        }

        protected bool isTextValid(List<TextBox> textBoxs)
        {
            foreach (var txt in textBoxs)
            {
                if (string.IsNullOrEmpty(txt.Text))
                {
                    return false;
                }
            }
            return true;
        }

        protected bool isUserValid(string email, string password, out string errMsg, out string userData)
        {
            using (MySqlConnection con = DatabaseHelper.GetConnection())
            {
                userData = "";
                string query = "SELECT COUNT(*) FROM users WHERE email = @email";
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@email", email);
                    con.Open();
                    int count = Convert.ToInt16(cmd.ExecuteScalar());
                    if (count > 0)
                    {
                        string query_auth = "SELECT * FROM users WHERE email = @email";
                        using (MySqlCommand cmd_auth = new MySqlCommand(query_auth, con))
                        {
                            cmd_auth.Parameters.AddWithValue("@email", email);
                            MySqlDataReader dr = cmd_auth.ExecuteReader();
                            if (dr.HasRows && dr.Read())
                            {
                                string hashedPassword = dr["password_hash"] as string;
                                if (!string.IsNullOrEmpty(hashedPassword) && BCrypt.Net.BCrypt.Verify(password, hashedPassword))
                                {
                                    if (!(bool)dr["is_verified"])
                                    {
                                        errMsg = "User has not been verified, please check the email";
                                        return false;
                                    }
                                    else if (dr["role"].ToString() != "editor" && dr["role"].ToString() != "admin")
                                    {
                                        errMsg = "The user does not have permission, please contact the administrator";
                                        return false;
                                    }
                                    errMsg = null;
                                    Dictionary<string, object> userDataObject = new Dictionary<string, object>
                                    {
                                        { "id", dr["id"] },
                                        { "name", dr["name"] },
                                        { "email", dr["email"] }
                                    };
                                    userData = JsonConvert.SerializeObject(userDataObject); ;
                                    return true;
                                }
                                else
                                {
                                    errMsg = "Incorrect password";
                                    return false;
                                }
                            }
                        }
                    }
                    errMsg = "User does not exist";
                    return false;
                }
            }
        }
    }
}