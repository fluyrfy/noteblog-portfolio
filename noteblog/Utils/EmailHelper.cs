using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Hosting;
using MySql.Data.MySqlClient;
using noteblog.Utils;

public static class EmailHelper
{
    private static string domain;
    //private static string htmlBody;
    private static string sender;
    private static Logger log;

    static EmailHelper()
    {
        domain = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
        //htmlBody = File.ReadAllText(HostingEnvironment.MapPath("~/Templates/email.html"));
        sender = "yufanliaocestlavie@gmail.com";
        log = new Logger(typeof(EmailHelper).Name);
    }

    public static void SendVerificationEmail(string userEmail, string subject, string title, string content, string hint, string link)
    {
        try
        {
            log.Info("Starting to send email");
            MailAddress from = new MailAddress(sender, "F.L.", Encoding.UTF8);
            MailAddress to = new MailAddress(userEmail);
            MailMessage mailMessage = new MailMessage(from, to);
            mailMessage.Bcc.Add(sender);
            mailMessage.Subject = subject;

            string htmlBody = File.ReadAllText(HostingEnvironment.MapPath("~/Templates/email.html"));
            htmlBody = htmlBody.Replace("{logoUrl}", "https://i.imgur.com/HeSD3Um.png");
            htmlBody = htmlBody.Replace("{homeUrl}", domain);
            htmlBody = htmlBody.Replace("{title}", title);
            htmlBody = htmlBody.Replace("{content}", content);
            htmlBody = htmlBody.Replace("{hint}", hint);
            htmlBody = htmlBody.Replace("{verifyLink}", $"{domain}/{link}");
            log.Debug($"Email title: {title}, content: {content}, hint: {hint}, link: {link}");
            mailMessage.Body = htmlBody;
            mailMessage.IsBodyHtml = true;
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            string smtpAcct = "";
            string smtpPwd = "";
            using (MySqlConnection con = DatabaseHelper.GetConnection())
            {
                MySqlCommand cmd = new MySqlCommand("SELECT name, value from configurations WHERE name = @acct OR name = @pwd", con);
                cmd.Parameters.AddWithValue("@acct", "google-app-acct");
                cmd.Parameters.AddWithValue("@pwd", "google-app-pwd");
                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (reader["name"].ToString() == "google-app-acct")
                    {
                        smtpAcct = reader["value"].ToString();
                    }
                    else if (reader["name"].ToString() == "google-app-pwd")
                    {
                        smtpPwd = reader["value"].ToString();
                    }
                }
            }
            smtpClient.Credentials = new NetworkCredential()
            {
                UserName = smtpAcct,
                Password = smtpPwd
            };
            smtpClient.EnableSsl = true;
            smtpClient.Send(mailMessage);
            log.Info("Email sent successfully");
        }
        catch (Exception ex)
        {
            log.Error("Failed to send email", ex);
            throw;
        }
        finally
        {
            log.Info("End of email send method");
        }
    }
}