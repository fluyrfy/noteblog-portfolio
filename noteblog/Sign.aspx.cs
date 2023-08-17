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
using Org.BouncyCastle.Asn1.Ocsp;
using System.Text.Encodings.Web;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Security.Cryptography;

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
                        modalBgPanel.Visible = true;
                        modalContentPanel.Visible = true;
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
            Response.Redirect("~/Sign.aspx");
        }

        protected void btnLogIn_Click(object sender, EventArgs e)
        {
            string errMsg = "";
            string userData = "";
            List<TextBox> inTextBoxs = new List<TextBox> { txtInEmail, txtInPwd };
            if (isTextValid(inTextBoxs) && isUserValid(txtInEmail.Text, txtInPwd.Text, out errMsg, out userData))
            {
                var expiration = cbRememberMe.Checked ? DateTime.Now.AddDays(1) : DateTime.Now.AddHours(1);
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

        private string generateVerificationCode()
        {
            var guid = Guid.NewGuid().ToString();
            // 取GUID的前6位作為驗證碼
            return guid.Substring(0, 6).ToUpper();
        }

        private string generateVerifyLink(string email, string code, out string domain)
        {
            string token = BitConverter.ToString(MachineKey.Protect(Encoding.UTF8.GetBytes($"{email}_{code}"), null));
            domain = Request.Url.GetLeftPart(UriPartial.Authority);
            //domain = $"{currentUrl.Scheme}://{currentUrl.Host}:{currentUrl.Port}";
            return $"{domain}/Verify?token={token}";
        }

        private void sendVerifyEmail(string userName, string userEmail, string verificationCode)
        {
            string sender = "yufanliaocestlavie@gmail.com";
            MailAddress from = new MailAddress(sender, "F.L.", Encoding.UTF8);
            MailAddress to = new MailAddress(userEmail);
            MailMessage mailMessage = new MailMessage(from, to);
            mailMessage.Subject = "Account Verification for F.L.";
            string htmlBody = File.ReadAllText(Server.MapPath("~/Templates/email.html"));
            string link = generateVerifyLink(userEmail, verificationCode, out string domain);
            htmlBody = htmlBody.Replace("{logoUrl}", "https://i.imgur.com/HeSD3Um.png");
            htmlBody = htmlBody.Replace("{homeUrl}", domain);
            htmlBody = htmlBody.Replace("{verifyLink}", link);
            //          AlternateView plainView = AlternateView.CreateAlternateViewFromString(
            //"Please click the link in this email to verify your account", Encoding.UTF8, "text/plain");
            //          AlternateView htmlView = AlternateView.CreateAlternateViewFromString(
            //htmlBody, null, "text/html");
            //          mailMessage.AlternateViews.Add(plainView);
            //          mailMessage.AlternateViews.Add(htmlView);
            mailMessage.Body = htmlBody;
            mailMessage.IsBodyHtml = true;
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.Credentials = new System.Net.NetworkCredential()
            {
                UserName = "yufanliaocestlavie@gmail.com",
                Password = "kouzymscksjckbye"
            };
            smtpClient.EnableSsl = true;
            smtpClient.Send(mailMessage);
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
                                string hashedPassword = dr["password_hash"].ToString();
                                if (hashedPassword != null && BCrypt.Net.BCrypt.Verify(password, hashedPassword))
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
                                    userData = $"{dr["name"].ToString()}|{dr["email"].ToString()}";
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