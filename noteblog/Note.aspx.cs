using System;
using System.Configuration;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.UI;
using MySql.Data.MySqlClient;

namespace noteblog
{
  public partial class Note : Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      // if (!IsPostBack)
      // {
      string noteIdString = Request.QueryString["id"];
      if (string.IsNullOrEmpty(noteIdString))
      {
        Response.Redirect("Default.aspx");
      }
      else
      {
        if (int.TryParse(noteIdString, out int noteId))
        {
          try
          {
            using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["Noteblog"].ConnectionString))
            {
              MySqlCommand cmd = new MySqlCommand();
              cmd.Connection = conn;
              StringBuilder sb = new StringBuilder();
              sb.AppendLine("SELECT notes.*, users.name as user_name FROM notes INNER JOIN users ON notes.user_id = users.id WHERE 1 = 1");
              sb.AppendLine("AND notes.id = @id");
              cmd.Parameters.AddWithValue("@id", noteId);
              cmd.CommandText = sb.ToString();
              conn.Open();
              using (MySqlDataReader reader = cmd.ExecuteReader())
              {
                if (reader.HasRows)
                {
                  reader.Read();
                  string title = reader["title"].ToString();
                  string content = HttpUtility.HtmlDecode(reader["content"].ToString());
                  string contentText = reader["content_text"].ToString();
                  string keyword = reader["keyword"].ToString();
                  litTitle.Text = title;
                  litContent.Text = content;
                  litAuthor.Text = reader["user_name"].ToString();
                  Page.Title = title;
                  Page.MetaDescription = contentText;
                  Page.MetaKeywords = keyword;
                  DateTime createdAt = (DateTime)reader["created_at"];
                  litCreatedAt.Text = createdAt.ToString("MMMM dd, yyyy", new CultureInfo("en-US"));
                }
                else
                {
                  Response.Redirect("~/Default.aspx");
                }
              }

            }
          }
          catch (Exception ex)
          {
            throw;
          }
          finally
          {
            new AccessStatsRepository().insert("Note");
          }

        }
      }
      // }
    }
  }
}