using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using noteblog.Models;
using noteblog.Utils;

namespace noteblog
{
    public partial class Modify : Page
    {
        private Logger log;
        protected void Page_Load(object sender, EventArgs e)
        {
            log = new Logger(typeof(Modify).Name);
            if (!IsPostBack)
            {
                string noteIdString = Request.QueryString["id"];
                if (string.IsNullOrEmpty(noteIdString))
                {
                    Response.Redirect("Default.aspx");
                }
                else
                {
                    if (int.TryParse(noteIdString, out int noteId))
                    {
                        CategoryRepository categoryRepository = new CategoryRepository();
                        List<Category> categories = categoryRepository.getAll(out int tr, out int nsr, 1, 0);

                        foreach (Category category in categories)
                        {
                            ListItem item = new ListItem(category.name, category.id.ToString());
                            rdlCategory.Items.Add(item);
                        }
                        using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["Noteblog"].ConnectionString))
                        {
                            MySqlCommand cmd = new MySqlCommand();
                            cmd.Connection = conn;
                            string ct = "SELECT * FROM notes WHERE id = @id";
                            cmd.CommandText = ct;
                            cmd.Parameters.AddWithValue("@id", noteId);

                            conn.Open();
                            using (MySqlDataReader reader = cmd.ExecuteReader())
                            {
                                DataTable dt = new DataTable();
                                dt.Load(reader);
                                if (dt.Rows.Count > 0)
                                {
                                    DataRow dr = dt.Rows[0];
                                    rdlCategory.SelectedValue = dr["category_id"].ToString();
                                    txtTitle.Text = dr["title"].ToString();
                                    txtKeyword.Text = dr["keyword"].ToString();
                                    if (dr["pic"] != DBNull.Value)
                                    {
                                        string picString = Convert.ToBase64String((byte[])dr["pic"]);
                                        imgCover.ImageUrl = $"data:image/png;base64,{picString}";
                                        hdnImgData.Value = picString;
                                    }
                                    txtContent.Text = HttpUtility.HtmlDecode(dr["content"].ToString());
                                    if (Convert.ToInt32(dr["user_id"]) != AuthenticationHelper.GetUserId())
                                    {
                                        pnlCoAuthor.Visible = false;
                                        pnlCoAuthorAlert.Visible = true;
                                    }
                                }
                                ViewState["SQL_QUERY"] = ct;
                                ViewState["NOTE"] = dt;
                                ViewState["ID"] = noteId;
                            }
                        }
                        List<User> coAuthorList = new NoteRepository().getCoAuthor(noteId);
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "coAuthors", $"let selectedCoAuthorUser =  {JsonConvert.SerializeObject(coAuthorList)};", true);
                    }
                }
            }

        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
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
                    log.Info("Starting to modify note");
                    MySqlDataAdapter da = new MySqlDataAdapter();
                    da.SelectCommand = new MySqlCommand();
                    da.SelectCommand.CommandText = ViewState["SQL_QUERY"].ToString();
                    da.SelectCommand.Parameters.AddWithValue("@id", ViewState["ID"]);
                    da.SelectCommand.Connection = con;
                    MySqlCommandBuilder cmd = new MySqlCommandBuilder();
                    cmd.DataAdapter = da;
                    DataTable dt = (DataTable)ViewState["NOTE"];
                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];
                        dr["category_id"] = rdlCategory.SelectedValue;
                        dr["title"] = txtTitle.Text;
                        dr["content"] = HttpUtility.HtmlEncode(hdnContent.Value);
                        dr["content_text"] = ConverterHelper.ExtractTextFromHtml(txtContent.Text);
                        dr["keyword"] = txtKeyword.Text;
                        log.Debug($"New note category: title: {dr["title"] as string}, keyword: {dr["keyword"] as string}");
                        if (fuCoverPhoto.HasFile)
                        {
                            if (fuCoverPhoto.PostedFile.ContentLength > maxFileSizeInBytes)
                            {
                                lblPhotoMsg.Text = "Image size exceeds limit（1MB）";
                                lblPhotoMsg.ForeColor = System.Drawing.Color.Red;
                                return;
                            }
                            using (Stream fs = fuCoverPhoto.PostedFile.InputStream)
                            {
                                using (BinaryReader br = new BinaryReader(fs))
                                {
                                    byte[] imgData = br.ReadBytes((Int32)fs.Length);
                                    dr["pic"] = imgData;
                                }
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(hdnImgData.Value))
                            {
                                dr["pic"] = Convert.FromBase64String(hdnImgData.Value);
                            }
                            else
                            {
                                dr["pic"] = null;
                            }
                        }
                        int rowsUpdated = da.Update(dt);

                        if (rowsUpdated > 0)
                        {
                            log.Info($"Note modified successfully, note ID: {dr["id"].ToString()}");
                        }
                        if (!string.IsNullOrEmpty(hdnSelectedCoAuthorUserIds.Value))
                        {
                            string[] hdnArray = hdnSelectedCoAuthorUserIds.Value.Split(',');
                            int noteId = Convert.ToInt32(dr["id"]);
                            new NoteRepository().deleteCoAuthor(noteId);
                            foreach (string coAuthorId in hdnArray)
                            {
                                new NoteRepository().insertCoAuthor(noteId, Convert.ToInt32(coAuthorId));
                            }
                        }
                    }
                    var userId = AuthenticationHelper.GetUserId();
                    DraftRepository draftRepository = new DraftRepository(userId);
                    draftRepository.delete(userId, Convert.ToInt32(Request.QueryString["id"]));
                    CacheHelper.ClearAllCache();
                }

                catch (Exception ex)
                {
                    log.Error("Failed to modify new note", ex);
                    throw;
                }
                finally
                {
                    log.Info("End of note modification method");
                    Response.Redirect("Dashboard.aspx");
                }
            }

        }
    }
}