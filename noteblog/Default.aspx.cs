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
using noteblog.Utils;
using System.Drawing.Printing;


namespace noteblog
{
    public partial class _Default : Page
    {
        private Logger logger;

        protected void Page_Load(object sender, EventArgs e)
        {
            logger = new Logger(typeof(_Default).Name);
            if (!IsPostBack)
            {
                ViewState["CurrentPage"] = Session["CurrentPage"] == null ? 1 : Convert.ToInt32(Session["CurrentPage"]);
                ViewState["Development"] = Session["Development"] == null ? "NotesAll" : Session["Development"].ToString();
                queryNotesData();
            }
        }

        protected void bindNotesData(DataTable notes)
        {
            repNote.DataSource = notes;
            repNote.DataBind();
        }
        protected void queryNotesData()
        {
            try
            {
                string cacheKey = ViewState["Development"].ToString();
                if (Cache[cacheKey] == null)
                {
                    logger.Info("Starting to query notes");
                    //SqlConnection conn = new SqlConnection("data source=localhost; database=noteblog; integrated security=SSPI");
                    using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["Noteblog"].ConnectionString))
                    {
                        MySqlCommand cmd = new MySqlCommand();
                        cmd.Connection = conn;
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("SELECT * FROM notes WHERE 1 = 1");
                        sb.AppendLine("ORDER BY updated_at DESC");
                        //sb.AppendLine("LIMIT @startIndex, @pageSize");
                        //cmd.Parameters.AddWithValue("@startIndex", (pageIndex - 1) * 6);
                        //cmd.Parameters.AddWithValue("@pageSize", 6);
                        cmd.CommandText = sb.ToString();
                        conn.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        DataTable filteredTable = new DataTable();
                        dt.Load(reader);
                        foreach (DataRow row in dt.Rows)
                        {
                            string content = Server.HtmlDecode(row["content"].ToString());
                            string strippedContent = StripHtmlTags(content);
                            row["content"] = strippedContent;
                        }
                        string dev = ViewState["Development"].ToString();
                        if (dev == "NotesAll")
                        {
                            filteredTable = dt;
                            Cache["NotesAll"] = dt;
                        }
                        else
                        {
                            DataRow[] resultRows;
                            if (dev == "NotesFront")
                            {
                                resultRows = dt.Select("development = 'F'");
                                filteredTable = dt.Clone();
                                foreach (DataRow row in resultRows)
                                {
                                    filteredTable.ImportRow(row);
                                }
                                Cache["NotesFront"] = filteredTable;
                            }
                            else if (dev == "NotesBack")
                            {
                                resultRows = dt.Select("development = 'B'");
                                filteredTable = dt.Clone();
                                foreach (DataRow row in resultRows)
                                {
                                    filteredTable.ImportRow(row);
                                }
                                Cache["NotesBack"] = filteredTable;
                            }

                        }
                    }
                    logger.Info("Notes queried successfully");
                    getPagedDataTableFromCache();
                }
                else
                {
                    getPagedDataTableFromCache();
                }
            }
            catch (Exception ex)
            {
                logger.Error("Failed to query notes", ex);
            }
            finally
            {
                logger.Info("End of notes query method");
                logger.Shutdown();
            }
        }

        protected void getPagedDataTableFromCache()
        {
            logger.Info("Starting to load notes");
            try
            {
                string cacheKey = ViewState["Development"].ToString();
                Session["Development"] = cacheKey;
                int pageNumber = Convert.ToInt32(ViewState["CurrentPage"]) > 0 ? Convert.ToInt32(ViewState["CurrentPage"]) : 1;
                logger.Debug($"Current development: {cacheKey}");
                int pageSize = 6;
                if (Cache[cacheKey] is DataTable dt)
                {
                    int totalRecords = dt.Rows.Count;
                    int totalPages;
                    totalPages = (int)Math.Ceiling((double)totalRecords / 6);
                    pageNumber = Math.Min(pageNumber, totalPages);
                    pnlPagination.Visible = totalRecords == 0 ? false : true;
                    if (totalRecords == 0)
                    {
                        totalPages = 0;
                        pageNumber = 0;
                        pageSize = 0;
                    }
                    logger.Debug($"Current page number: {pageNumber}");
                    ViewState["CurrentPage"] = pageNumber;
                    Session["CurrentPage"] = pageNumber;
                    List<int> pageNumbers = new List<int>();
                    for (int i = 1; i <= totalPages; i++)
                    {
                        pageNumbers.Add(i);
                    }
                    repPagination.DataSource = pageNumbers;
                    repPagination.DataBind();

                    // 樣式切換
                    paginationActiveStyle();
                    toggleFilterCss();

                    ViewState["TotalPages"] = totalPages;
                    int startIndex = (pageNumber - 1) * pageSize;
                    int endIndex = Math.Min(startIndex + pageSize - 1, dt.Rows.Count - 1);
                    startIndex = startIndex < 0 ? 0 : startIndex;
                    endIndex = endIndex < 0 ? 0 : endIndex;

                    DataTable currentPageData = dt.Clone();

                    if (dt.Rows.Count > 0)
                    {
                        for (int i = startIndex; i <= endIndex; i++)
                        {
                            currentPageData.ImportRow(dt.Rows[i]);
                        }
                    }
                    bindNotesData(currentPageData);
                    logger.Info($"Notes queried successfully, Datatable: {currentPageData}");

                }
            }
            catch (Exception ex)
            {
                logger.Error("Failed to load notes", ex);
            }
            finally
            {
                logger.Info("End of notes load method");
                logger.Shutdown();
            }
        }

        //protected void btnAll_Click(object sender, EventArgs e)
        //{
        //    ViewState["Development"] = "NotesAll";
        //    queryNotesData();
        //}

        //protected void btnFront_Click(object sender, EventArgs e)
        //{
        //    ViewState["Development"] = "NotesFront";
        //    queryNotesData();
        //}

        //protected void btnBack_Click(object sender, EventArgs e)
        //{
        //    ViewState["Development"] = "NotesBack";
        //    queryNotesData();
        //}
        protected void btnPage_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            ViewState["CurrentPage"] = button.CommandArgument;
            queryNotesData();
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

        protected void repPagination_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int index = e.Item.ItemIndex;

                if (index == 0)
                {
                    Button button = (Button)e.Item.FindControl("btnPage");
                    button.CssClass += " w3-black";
                }
            }
        }
        protected void paginationActiveStyle()
        {
            foreach (RepeaterItem item in repPagination.Items)
            {
                int index = item.ItemIndex;
                Button button = (Button)item.FindControl("btnPage");

                int currentPage = Convert.ToInt32(ViewState["CurrentPage"]);

                if (index == currentPage - 1)
                {
                    button.CssClass += " w3-black";
                }
                else
                {
                    button.CssClass = button.CssClass.Replace(" w3-black", string.Empty);
                }
            }
        }

        protected void btnNavigation_Command(object sender, CommandEventArgs e)
        {
            string commandArgument = e.CommandArgument.ToString();
            int currentPage = Convert.ToInt32(ViewState["CurrentPage"]);
            int totalPages = Convert.ToInt32(ViewState["TotalPages"]);
            if (commandArgument == "Previous")
            {
                if (currentPage > 1)
                {
                    currentPage--;
                }
            }
            else if (commandArgument == "Next")
            {
                if (currentPage < totalPages)
                {
                    currentPage++;
                }
            }
            ViewState["CurrentPage"] = currentPage;
            queryNotesData();
        }

        protected void btnFilter_Command(object sender, CommandEventArgs e)
        {
            ViewState["Development"] = e.CommandArgument.ToString();
            queryNotesData();
        }
        protected void toggleFilterCss()
        {
            string active = "w3-button w3-black";
            string inactive = "w3-button w3-white";
            btnAll.CssClass = inactive;
            btnFrontEnd.CssClass = inactive;
            btnBackEnd.CssClass = inactive;
            if (Session["Development"] != null)
            {
                switch (Session["Development"].ToString())
                {
                    case "NotesAll":
                        btnAll.CssClass = active;
                        break;
                    case "NotesFront":
                        btnFrontEnd.CssClass = active; break;
                    case "NotesBack":
                        btnBackEnd.CssClass = active; break;
                    default:
                        break;
                }
            }
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