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
using System.Security.Cryptography;

namespace noteblog
{
    public partial class Note : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string noteIdString = Request.QueryString["id"];
                if (string.IsNullOrEmpty(noteIdString))
                {
                    // 如果文章ID为空，则执行重定向到首页
                    Response.Redirect("~/Default.aspx"); // 请替换为你的首页URL
                }
                else
                {

                    if (int.TryParse(noteIdString, out int noteId))
                    {
                        try
                        {
                            // 转换成功，result包含转换后的整数值
                            using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["Noteblog"].ConnectionString))
                            {
                                MySqlCommand cmd = new MySqlCommand();
                                cmd.Connection = conn;
                                StringBuilder sb = new StringBuilder();
                                sb.AppendLine("SELECT * FROM notes WHERE 1 = 1");
                                sb.AppendLine("AND id = @id");
                                cmd.Parameters.AddWithValue("@id", noteId);
                                cmd.CommandText = sb.ToString();
                                conn.Open();
                                using (MySqlDataReader reader = cmd.ExecuteReader())
                                {
                                    if (reader.HasRows)
                                    {
                                        reader.Read();
                                        litTitle.Text = reader["title"].ToString();
                                        litContent.Text = Server.HtmlDecode(reader["content"].ToString());
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

                    }
                    else
                    {
                        // 转换失败，处理错误情况
                    }
                }
            }
        }
    }
}