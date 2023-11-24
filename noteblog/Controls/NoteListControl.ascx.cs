using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using noteblog.Models;

namespace noteblog.Controls
{
    public partial class NoteListControl : System.Web.UI.UserControl
    {
        //private Logger logger;
        protected void Page_Load(object sender, EventArgs e)
        {
            //RequestHelper.HandleHeadRequest();
            //logger = new Logger(typeof(_Default).Name);
            if (!IsPostBack)
            {
                ViewState["CurrentPage"] = 1;
                ViewState["Category"] = "ALL";
                hidCategoryName.Value = ViewState["Category"].ToString();
                hidPageNumber.Value = ViewState["CurrentPage"].ToString();
                bindCategoriesData(new CategoryRepository().getAll(out int tr, out int nsr));
                queryNotesData();
            }
        }
        protected void queryNotesData()
        {
            try
            {
                string cacheKey = ViewState["Category"].ToString();
                int currentPage = Convert.ToInt32(ViewState["CurrentPage"]);
                if (Cache[$"{cacheKey}-{currentPage}"] == null)
                {
                    //logger.Info("Starting to query notes");
                    using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["Noteblog"].ConnectionString))
                    {
                        MySqlCommand cmd = new MySqlCommand();
                        cmd.Connection = conn;
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("SELECT notes.*, categories.name as category_name FROM notes INNER JOIN categories ON notes.category_id = categories.id WHERE 1 = 1");
                        if (cacheKey != "ALL")
                        {
                            sb.AppendLine("AND categories.name = @categoryName");
                            cmd.Parameters.AddWithValue("@categoryName", cacheKey);
                        }
                        sb.AppendLine("ORDER BY updated_at DESC");
                        sb.AppendLine("LIMIT 6 OFFSET @offset");
                        cmd.CommandText = sb.ToString();
                        cmd.Parameters.AddWithValue("@offset", (currentPage - 1) * 6);
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
                        Cache[$"{cacheKey}-{currentPage}"] = dt;
                    }
                    //logger.Info("Notes queried successfully");
                }
                getPagedDataTableFromCache();
            }
            catch (Exception ex)
            {
                //logger.Error("Failed to query notes", ex);
            }
            finally
            {
                //logger.Info("End of notes query method");
            }
        }
        protected void getPagedDataTableFromCache()
        {
            var CR = new CategoryRepository();
            var NR = new NoteRepository();
            //logger.Info("Starting to load notes");
            try
            {
                string cacheKey = ViewState["Category"].ToString();
                hidCategoryName.Value = Regex.Replace(cacheKey, @"\s+", "");
                int currentPage = Convert.ToInt32(ViewState["CurrentPage"]);
                //logger.Debug($"Current Category: {cacheKey}");
                if (Cache[$"{cacheKey}-{currentPage}"] is DataTable dt)
                {
                    int totalRecords = NR.getTotalCount(CR.getId(cacheKey));
                    int totalPages = (int)Math.Ceiling((double)totalRecords / 6);
                    if (totalRecords == 0)
                    {
                        totalPages = 0;
                    }
                    //logger.Debug($"Current page number: {currentPage}");
                    bindPagination(totalPages);
                    ViewState["TotalPages"] = totalPages;
                    DataTable dataTable = Cache[$"{cacheKey}-{currentPage}"] as DataTable;
                    bindNotesData(dataTable);
                    //logger.Info($"Notes queried successfully, Datatable: {dataTable.Rows.Count} rows");
                }
            }
            catch (Exception ex)
            {
                //logger.Error("Failed to load notes", ex);
            }
            finally
            {
                //logger.Info("End of notes load method");
            }
        }

        protected void btnPage_Command(object sender, CommandEventArgs e)
        {
            ViewState["CurrentPage"] = e.CommandArgument;
            queryNotesData();
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
            string category = e.CommandArgument.ToString();
            ViewState["Category"] = category;
            ViewState["CurrentPage"] = 1;
            queryNotesData();
        }

        protected void repCategory_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                List<Category> categories = new CategoryRepository().getAll(out int tr, out int nsr);
                foreach (Control c in e.Item.Controls)
                {
                    if (c is LinkButton)
                    {
                        LinkButton lb = (LinkButton)c;
                        lb.ID = $"btn{Regex.Replace(categories[e.Item.ItemIndex].name, @"\s+", "")}";
                        lb.ClientIDMode = ClientIDMode.Static;
                    }
                }
            }
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

        protected void bindNotesData(DataTable notes)
        {
            repNote.DataSource = notes;
            repNote.DataBind();
        }
        protected void bindCategoriesData(List<Category> categories)
        {
            repCategory.DataSource = categories;
            repCategory.DataBind();
        }
        protected void bindPagination(int totalPages)
        {
            List<int> pageNumbers = new List<int>();
            for (int i = 1; i <= totalPages; i++)
            {
                pageNumbers.Add(i);
            }
            repPagination.DataSource = pageNumbers;
            repPagination.DataBind();
        }

        private string StripHtmlTags(string input)
        {
            return Regex.Replace(input, "<.*?>", string.Empty);
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
    }
}