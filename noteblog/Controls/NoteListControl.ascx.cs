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
using ZstdSharp;

namespace noteblog.Controls
{
  public partial class NoteListControl : System.Web.UI.UserControl
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        ViewState["Category"] = "ALL";
        hidCategoryName.Value = "ALL";
        ViewState["CurrentPage"] = 1;
        hidPageNumber.Value = "1";
        bindCategoriesData(new CategoryRepository().getAll(out int tr, out int nsr, 1, 0));
        queryNotesData();
      }
    }
    protected void queryNotesData()
    {
      try
      {
        string cacheKey = ViewState["Category"].ToString();
        int currentPage = Convert.ToInt32(ViewState["CurrentPage"]);
        string userId = Request.QueryString["uid"];
        if (Cache[$"{cacheKey}-{currentPage}"] == null)
        {
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
            if (!string.IsNullOrEmpty(userId))
            {
              sb.AppendLine("AND notes.user_id = @userId");
              cmd.Parameters.AddWithValue("@userId", userId);
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
        }
        getPagedDataTableFromCache(userId);
      }
      catch (Exception ex)
      {
        throw;
      }
    }
    protected void getPagedDataTableFromCache(string uid)
    {
      var CR = new CategoryRepository();
      var NR = new NoteRepository();
      try
      {
        string cacheKey = ViewState["Category"].ToString();
        hidCategoryName.Value = Regex.Replace(cacheKey, @"\s+", "");
        int currentPage = Convert.ToInt32(ViewState["CurrentPage"]);
        if (Cache[$"{cacheKey}-{currentPage}"] is DataTable dt)
        {
          int totalRecords = NR.getTotalCount(CR.getId(cacheKey), uid);
          // int totalRecords = dt.Rows.Count;
          int totalPages = (int)Math.Ceiling((double)totalRecords / 6);
          if (totalRecords == 0)
          {
            totalPages = 0;
          }
          ViewState["TotalPages"] = totalPages;
          hidTotalPages.Value = totalPages.ToString();
          DataTable dataTable = Cache[$"{cacheKey}-{currentPage}"] as DataTable;
          bindNotesData(dataTable);
        }
      }
      catch (Exception ex)
      {
        throw;
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
      hidPageNumber.Value = currentPage.ToString();
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
        List<Category> categories = new CategoryRepository().getAll(out int tr, out int nsr, 1, 0);
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

    private string StripHtmlTags(string input)
    {
      return Regex.Replace(input, "<.*?>", string.Empty);
    }
  }
}