﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using noteblog.Models;
using noteblog.Utils;
using System.Linq;
using System.Web;

namespace noteblog
{
    public partial class _Default : Page
    {
        private Logger logger;

        protected void Page_Load(object sender, EventArgs e)
        {
            logger = new Logger(typeof(_Default).Name);
            time.Text = DateTime.Now.ToString();
            if (!IsPostBack)
            {
                ViewState["CurrentPage"] = Session["CurrentPage"] == null ? 1 : Convert.ToInt32(Session["CurrentPage"]);
                ViewState["Category"] = Session["Category"] == null ? "ALL" : Session["Category"].ToString();
                bindCategoriesData(new CategoryRepository().getAll());
                queryNotesData("ALL");
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

        protected void repCategory_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                List<Category> categories = new CategoryRepository().getAll();
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


        protected void queryNotesData(string category)
        {
            try
            {
                //string cacheKey = ViewState["Category"].ToString();
                string cacheKey = category;
                int currentPage = Convert.ToInt32(ViewState["CurrentPage"]);
                if (Cache[$"{cacheKey}-{currentPage}"] == null)
                {
                    logger.Info("Starting to query notes");
                    //SqlConnection conn = new SqlConnection("data source=localhost; database=noteblog; integrated security=SSPI");
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
                    logger.Info("Notes queried successfully");
                }
                getPagedDataTableFromCache(category);
            }
            catch (Exception ex)
            {
                logger.Error("Failed to query notes", ex);
            }
            finally
            {
                logger.Info("End of notes query method");
            }
        }

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["Category"] = ddlCategory.SelectedValue;
            queryNotesData(ddlCategory.SelectedValue);
        }

        protected void getPagedDataTableFromCache(string category)
        {
            var CR = new CategoryRepository();
            var NR = new NoteRepository();
            logger.Info("Starting to load notes");
            try
            {
                //string cacheKey = ViewState["Category"].ToString();
                string cacheKey = category;
                hidCategoryName.Value = Regex.Replace(cacheKey, @"\s+", "");
                int currentPage = Convert.ToInt32(ViewState["CurrentPage"]);
                Session["Category"] = cacheKey;
                int pageNumber = Convert.ToInt32(ViewState["CurrentPage"]) > 0 ? Convert.ToInt32(ViewState["CurrentPage"]) : 1;
                logger.Debug($"Current Category: {cacheKey}");
                if (Cache[$"{cacheKey}-{currentPage}"] is DataTable dt)
                {

                    int totalRecords = NR.getTotalCount(CR.getId(cacheKey));
                    int totalPages;
                    totalPages = (int)Math.Ceiling((double)totalRecords / 6);
                    pageNumber = Math.Min(pageNumber, totalPages);
                    if (totalRecords == 0)
                    {
                        totalPages = 1;
                        pageNumber = 1;
                        pnlPagination.Visible = false;
                    }
                    logger.Debug($"Current page number: {pageNumber}");
                    ViewState["CurrentPage"] = pageNumber;
                    Session["CurrentPage"] = pageNumber;
                    if (totalPages > 1)
                    {
                        List<int> pageNumbers = new List<int>();
                        for (int i = 1; i <= totalPages; i++)
                        {
                            pageNumbers.Add(i);
                        }
                        repPagination.DataSource = pageNumbers;
                        repPagination.DataBind();
                        paginationActiveStyle();
                    }
                    else
                    {
                        pnlPagination.Visible = false;
                    }

                    ViewState["TotalPages"] = totalPages;
                    bindNotesData(Cache[$"{cacheKey}-{currentPage}"] as DataTable);
                    logger.Info($"Notes queried successfully, Datatable: {Cache[$"{cacheKey}-{currentPage}"] as DataTable}");
                }
            }
            catch (Exception ex)
            {
                logger.Error("Failed to load notes", ex);
            }
            finally
            {
                logger.Info("End of notes load method");
            }
        }

        protected void btnPage_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            ViewState["CurrentPage"] = button.CommandArgument;
            queryNotesData(button.CommandArgument);
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
            queryNotesData("ALL");
        }

        protected void btnFilter_Command(object sender, CommandEventArgs e)
        {
            string category = e.CommandArgument.ToString();
            hidCategoryName.Value = Regex.Replace(category, @"\s+", "");
            ViewState["Category"] = category;
            queryNotesData(category);
        }
        //protected void toggleFilterCss()
        //{
        //    Repeater repeater = this.Master.FindControl("MainContent").FindControl("repCategory") as Repeater;
        //    var activeCategory = ViewState["Category"]?.ToString();
        //    string active = " w3-black";
        //    if (repeater != null && !string.IsNullOrEmpty(activeCategory))
        //    {
        //        foreach (RepeaterItem item in repeater.Items)
        //        {
        //            if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
        //            {
        //                foreach (Control control in item.Controls)
        //                {
        //                    if (control is LinkButton)
        //                    {
        //                        LinkButton linkButton = (LinkButton)control;
        //                        linkButton.CssClass = linkButton.CssClass.Replace(active, "");
        //                        if (linkButton.ID == $"btn{activeCategory}")
        //                        {
        //                            linkButton.CssClass += active;
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        //public bool isRecacheRequired(string param)
        //{
        //    // 判斷是否要重新緩存輸出
        //    bool isRecacheRequired = false;
        //    // 檢查其他按鈕的狀態
        //    if (Session["Category"] as string != param || Cache["param"] == null)
        //    {
        //        isRecacheRequired = true;
        //    }

        //    return isRecacheRequired;
        //}

    }
}