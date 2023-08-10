using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.IO;
using System.Collections;
using System.Web.Caching;
using noteblog.Utils;

namespace noteblog
{
    public partial class Take : Page
    {
        private Logger log;
        protected void Page_Load(object sender, EventArgs e)
        {
            log = new Logger(typeof(Take).Name);
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {

                using (MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["Noteblog"].ConnectionString))
                {
                    try
                    {
                        log.Info("Starting to create new note");
                        MySqlCommand cmd = new MySqlCommand();
                        cmd.Connection = con;
                        cmd.CommandText = "INSERT INTO notes(development,title, content, keyword, published_at, pic) VALUES (@development, @title, @content, @keyword, @publishedAt, @pic)";
                        cmd.Parameters.AddWithValue("@development", rdlDevelopment.SelectedValue);
                        cmd.Parameters.AddWithValue("@title", txtTitle.Text);
                        cmd.Parameters.AddWithValue("@content", HttpUtility.HtmlEncode(txtContent.Text));
                        cmd.Parameters.AddWithValue("@keyword", txtKeyword.Text);
                        cmd.Parameters.AddWithValue("@publishedAt", DateTime.UtcNow);
                        using (Stream fs = fuCoverPhoto.HasFile ? fuCoverPhoto.PostedFile.InputStream : new FileStream(Server.MapPath("~/Images/cover/default.jpg"), FileMode.Open, FileAccess.Read))
                        {
                            using (BinaryReader br = new BinaryReader(fs))
                            {
                                byte[] imgData = br.ReadBytes((Int32)fs.Length);
                                cmd.Parameters.AddWithValue("@pic", imgData);
                            }
                        }
                        log.Debug($"New note info: {txtTitle.Text} - {txtContent.Text}");
                        con.Open();
                        cmd.ExecuteNonQuery();
                        MySqlCommand cmd2 = new MySqlCommand();
                        cmd2.Connection = con;
                        cmd2.CommandText = "SELECT LAST_INSERT_ID()";
                        string newId = cmd2.ExecuteScalar().ToString();
                        log.Info("Note created successfully, note ID: " + newId);
                        CacheHelper.ClearAllCache();
                    }

                    catch (Exception ex)
                    {
                        log.Error("Failed to create new note", ex);
                        throw;
                    }
                    finally
                    {
                        log.Info("End of note creation method");
                        log.Shutdown();
                        Response.Redirect("Dashboard.aspx");
                    }
                }

            }
        }
    }
}