using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.IO;

namespace noteblog
{
    public partial class Take : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            using (MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["Noteblog"].ConnectionString))
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = con;
                    cmd.CommandText = "INSERT INTO notes(development,title, content, keyword, published_at, pic) VALUES (@development, @title, @content, @keyword, @publishedAt, @pic)";
                    cmd.Parameters.AddWithValue("@development", rdlDevelopment.SelectedValue);
                    cmd.Parameters.AddWithValue("@title", txtTitle.Text);
                    cmd.Parameters.AddWithValue("@content", Server.HtmlEncode(txtContent.Text));
                    cmd.Parameters.AddWithValue("@keyword", txtKeyword.Text);
                    cmd.Parameters.AddWithValue("@publishedAt", DateTime.UtcNow);
                    if (fuCoverPhoto.HasFile)
                    {
                        using (Stream fs = fuCoverPhoto.PostedFile.InputStream)
                        {
                            using (BinaryReader br = new BinaryReader(fs))
                            {
                                byte[] imgData = br.ReadBytes((Int32)fs.Length);
                                cmd.Parameters.AddWithValue("@pic", imgData);
                            }
                        }
                    }
                    con.Open();
                    cmd.ExecuteNonQuery();
                    Response.Redirect("Default.aspx");
                }

                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }
    }
}