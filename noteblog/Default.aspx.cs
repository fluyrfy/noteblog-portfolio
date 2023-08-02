using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;
using MarkdownSharp;
using System.Text.RegularExpressions;
using System.Text;

namespace noteblog
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Cache["NotesAll"] = queryNotesData();
                bindNotesData(Cache["NotesAll"]);
            }
        }
        protected void bindNotesData(object cacheNotes)
        {
            try
            {
                repNote.DataSource = cacheNotes;
                repNote.DataBind();
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        protected DataTable queryNotesData(string dev = "")
        {
            //SqlConnection conn = new SqlConnection("data source=localhost; database=noteblog; integrated security=SSPI");
            using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["Noteblog"].ConnectionString))
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("SELECT * FROM notes WHERE 1 = 1");
                if (dev != "")
                {
                    sb.AppendLine("AND development = @development");
                    cmd.Parameters.AddWithValue("@development", dev);
                }
                sb.AppendLine("ORDER BY updated_at DESC");
                cmd.CommandText = sb.ToString();
                conn.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                foreach (DataRow row in dt.Rows)
                {
                    string content = Server.HtmlDecode(row["content"].ToString());
                    string strippedContent = StripHtmlTags(content);
                    row["content"] = strippedContent;
                }
                return dt;
            }

        }

        protected void btnAll_Click(object sender, EventArgs e)
        {
            if (Cache["NotesAll"] == null)
            {
                Cache["NotesAll"] = queryNotesData();
            }
            bindNotesData(Cache["NotesAll"]);
        }

        protected void btnFront_Click(object sender, EventArgs e)
        {
            if (Cache["NotesFront"] == null)
            {
                Cache["NotesFront"] = queryNotesData("F");
            }
            bindNotesData(Cache["NotesFront"]);
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            if (Cache["NotesBack"] == null)
            {
                Cache["NotesBack"] = queryNotesData("B");
            }
            bindNotesData(Cache["NotesBack"]);
        }
        private string StripHtmlTags(string input)
        {
            return Regex.Replace(input, "<.*?>", string.Empty); // 使用正则表达式去除 HTML 标签
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            // 設定下載檔案的路徑和檔名
            string filePath = Server.MapPath("~/Files/Fan_Resume.pdf"); ;
            string fileName = "FrankLiao_Resume.pdf";

            // 執行下載檔案的動作
            Response.Clear();
            Response.ContentType = "application/pdf"; // 設定檔案的內容類型，這裡以 PDF 為例
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
            Response.WriteFile(filePath);
            Response.End();
        }
        //protected void lnkNote_Command(object sender, CommandEventArgs e)
        //{
        //    if (e.CommandName == "ReadNote")
        //    {
        //        string noteId = e.CommandArgument.ToString();
        //        Response.Write(noteId);
        //        //Response.Redirect("Note.aspx?id=" + Server.UrlEncode(noteId));
        //    }
        //}
    }
}