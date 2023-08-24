using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web.UI;
using MySql.Data.MySqlClient;
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
                                    rdlDevelopment.SelectedValue = dr["development"].ToString();
                                    txtTitle.Text = dr["title"].ToString();
                                    txtKeyword.Text = dr["keyword"].ToString();
                                    imgCover.ImageUrl = $"data:image/png;base64,{Convert.ToBase64String((byte[])dr["pic"])}";
                                    txtContent.Text = dr["content"].ToString();
                                }
                                ViewState["SQL_QUERY"] = ct;
                                ViewState["NOTE"] = dt;
                                ViewState["ID"] = noteId;
                            }
                        }
                    }
                }
            }

        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {

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
                        dr["development"] = rdlDevelopment.SelectedValue;
                        dr["title"] = txtTitle.Text;
                        dr["content"] = txtContent.Text;
                        dr["keyword"] = txtKeyword.Text;
                        log.Debug($"New note development: {dr["development"] as string}, title: {dr["title"] as string}, content: {dr["content_text"] as string}, keyword: {dr["keyword"] as string}");
                        if (fuCoverPhoto.HasFile)
                        {
                            using (Stream fs = fuCoverPhoto.PostedFile.InputStream)
                            {
                                using (BinaryReader br = new BinaryReader(fs))
                                {
                                    byte[] imgData = br.ReadBytes((Int32)fs.Length);
                                    dr["pic"] = imgData;
                                }
                            }
                        }
                        int rowsUpdated = da.Update(dt);

                        if (rowsUpdated > 0)
                        {
                            log.Info($"Note modified successfully, note ID: {dr["id"].ToString()}");
                        }
                    }
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
                    log.Shutdown();
                    Response.Redirect("Dashboard.aspx");
                }
            }

        }
    }
}