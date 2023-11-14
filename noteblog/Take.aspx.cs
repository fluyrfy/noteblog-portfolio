using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using noteblog.Models;
using noteblog.Utils;

namespace noteblog
{
    public partial class Take : Page
    {
        private Logger log;
        protected void Page_Load(object sender, EventArgs e)
        {
            log = new Logger(typeof(Take).Name);
            if (!IsPostBack)
            {
                CategoryRepository categoryRepository = new CategoryRepository();
                List<Category> categories = categoryRepository.getAll(out int tr, out int nsr);

                foreach (Category category in categories)
                {
                    ListItem item = new ListItem(category.name, category.id.ToString());
                    rdlCategory.Items.Add(item);
                }
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid && AuthenticationHelper.IsUserAuthenticatedAndTicketValid())
            {
                int userId = AuthenticationHelper.GetUserId();
                int maxFileSizeInBytes = 1024 * 1024;
                if (fuCoverPhoto.HasFile)
                {
                    if (fuCoverPhoto.PostedFile.ContentLength > maxFileSizeInBytes)
                    {
                        lblPhotoMsg.Text = "Image size exceeds limit（1MB）";
                        lblPhotoMsg.ForeColor = System.Drawing.Color.Red;
                        return;
                    }
                }
                using (MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["Noteblog"].ConnectionString))
                {
                    try
                    {
                        log.Info("Starting to create new note");
                        MySqlCommand cmd = new MySqlCommand();
                        cmd.Connection = con;
                        cmd.CommandText = "INSERT INTO notes(user_id, category_id, title, content, content_text, keyword, published_at, pic) VALUES (@userId, @categoryId, @title, @content, @contentText, @keyword, @publishedAt, @pic)";
                        var content = HttpUtility.HtmlEncode(txtContent.Text);
                        cmd.Parameters.AddWithValue("@userId", userId);
                        cmd.Parameters.AddWithValue("@categoryId", rdlCategory.SelectedValue);
                        cmd.Parameters.AddWithValue("@title", txtTitle.Text);
                        cmd.Parameters.AddWithValue("@content", HttpUtility.HtmlEncode(txtContent.Text));
                        cmd.Parameters.AddWithValue("@contentText", ConverterHelper.ExtractTextFromHtml(txtContent.Text));
                        cmd.Parameters.AddWithValue("@keyword", txtKeyword.Text);
                        cmd.Parameters.AddWithValue("@publishedAt", DateTime.UtcNow);
                        byte[] imgData = new byte[0];
                        using (Stream fs = fuCoverPhoto.HasFile ? fuCoverPhoto.PostedFile.InputStream : new FileStream(Server.MapPath("~/Images/cover/default.jpg"), FileMode.Open, FileAccess.Read))
                        {
                            using (BinaryReader br = new BinaryReader(fs))
                            {
                                imgData = br.ReadBytes((Int32)fs.Length);
                            }
                        }
                        if (!string.IsNullOrEmpty(hdnImgData.Value))
                        {
                            imgData = Convert.FromBase64String(hdnImgData.Value);
                        }
                        cmd.Parameters.AddWithValue("@pic", imgData);
                        log.Debug($"New note info: {txtTitle.Text} - {txtContent.Text}");
                        con.Open();
                        cmd.ExecuteNonQuery();
                        MySqlCommand cmd2 = new MySqlCommand();
                        cmd2.Connection = con;
                        cmd2.CommandText = "SELECT LAST_INSERT_ID()";
                        string newId = cmd2.ExecuteScalar().ToString();
                        log.Info("Note created successfully, note ID: " + newId);
                        DraftRepository draftRepository = new DraftRepository(userId);
                        draftRepository.delete(userId, 0);
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
                        Response.Redirect("Dashboard.aspx");
                    }
                }

            }
        }
    }
}