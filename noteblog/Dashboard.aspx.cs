using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using noteblog.Models;
using noteblog.Utils;

namespace noteblog
{
    public partial class Dashboard : Page
    {
        private Logger log = new Logger(typeof(Dashboard).Name);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["CurrentPage"] = 1;
                queryNotesData();
            }
        }
        protected void queryNotesData()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["Noteblog"].ConnectionString))
                {
                    MySqlDataAdapter da = new MySqlDataAdapter();
                    da.SelectCommand = new MySqlCommand();
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("SELECT * FROM notes WHERE 1 = 1");
                    string keyQuery = @"
AND (
title LIKE @word
OR content LIKE @word
OR keyword LIKE @word
)
";
                    if (!string.IsNullOrEmpty(ViewState["Word"]?.ToString()))
                    {
                        sb.AppendLine(keyQuery);
                        da.SelectCommand.Parameters.AddWithValue("@word", $"%{ViewState["Word"]?.ToString()}%");
                    }
                    sb.AppendLine("ORDER BY updated_at DESC");
                    sb.AppendLine("LIMIT 5 OFFSET @offset");
                    da.SelectCommand.CommandText = sb.ToString();
                    da.SelectCommand.Connection = conn;
                    da.SelectCommand.Parameters.AddWithValue("@offset", (Convert.ToInt32(ViewState["CurrentPage"]) - 1) * 5);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    repNotes.DataSource = dt;
                    repNotes.DataBind();
                    MySqlCommand mCmd = new MySqlCommand();
                    StringBuilder mSb = new StringBuilder();
                    mSb.AppendLine("SELECT COUNT(*) FROM notes WHERE 1 = 1");
                    if (!string.IsNullOrEmpty(ViewState["Word"]?.ToString()))
                    {
                        mSb.AppendLine(keyQuery);
                        mCmd.Parameters.AddWithValue("@word", $"%{ViewState["Word"]?.ToString()}%");
                    }
                    mCmd.CommandText = mSb.ToString();
                    mCmd.Connection = conn;
                    conn.Open();
                    int dataCount = Convert.ToInt32(mCmd.ExecuteScalar());
                    litDataCount.Text = dataCount.ToString();
                    litPageSize.Text = dataCount < 5 ? dataCount.ToString() : "5";
                    int totalPages = (int)Math.Ceiling((double)dataCount / 5);
                    ViewState["TotalPages"] = totalPages;
                    List<int> pageNumbers = new List<int>();
                    for (int i = 1; i <= totalPages; i++)
                    {
                        pageNumbers.Add(i);
                    }
                    repPage.DataSource = pageNumbers;
                    repPage.DataBind();
                    if (totalPages == 1)
                    {
                        btnNext.CssClass += " disabled";
                        btnPrevious.CssClass += " disabled";
                    }
                    paginationActiveStyle();
                }
            }
            catch { }
            finally
            {
            }
        }
        protected void btnNoteDelete_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Request.Form["noteId"]);
            if (id != 0)
            {
                log.Info("Starting to delete note");
                log.Debug($"Delete note id: {id}");
                if (id > 0)
                {
                    try
                    {
                        using (MySqlConnection con = DatabaseHelper.GetConnection())
                        {
                            MySqlCommand cmd = new MySqlCommand("DELETE FROM notes WHERE id = @id", con);
                            cmd.Parameters.AddWithValue("@id", id);
                            con.Open();
                            if (cmd.ExecuteNonQuery() > 0)
                            {
                                CacheHelper.ClearAllCache();
                                log.Info("Note deleted successfully");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error("Failed to delete note", ex);
                        throw;
                    }
                    finally
                    {
                        log.Info("End of note deletion method");
                        Response.Redirect("Dashboard.aspx");
                    }
                }
            }
            else
            {
                log.Info("Starting to delete notes");
                List<int> selectedIds = new List<int>();
                foreach (RepeaterItem repeaterItem in repNotes.Items)
                {
                    CheckBox cbNote = (CheckBox)repeaterItem.FindControl("cbNote");
                    if (cbNote.Checked)
                    {
                        int noteId = Convert.ToInt32(((Literal)repeaterItem.FindControl("litNoteId")).Text);
                        selectedIds.Add(noteId);
                    }
                }
                if (selectedIds.Count > 0)
                {
                    string ids = string.Join(",", selectedIds);
                    log.Debug($"Delete notes id: {ids}");
                    string deleteQuery = $"DELETE FROM notes WHERE id IN ({ids})";
                    using (MySqlConnection con = DatabaseHelper.GetConnection())
                    {
                        MySqlTransaction transaction = con.BeginTransaction();
                        try
                        {
                            MySqlCommand cmd = new MySqlCommand(deleteQuery, con, transaction);
                            con.Open();
                            int affectedRows = cmd.ExecuteNonQuery();
                            transaction.Commit();
                            if (affectedRows > 0)
                            {
                                CacheHelper.ClearAllCache();
                                log.Info("Notes deleted successfully");
                            }
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            log.Error("Failed to delete notes", ex);
                            throw;
                        }
                        finally
                        {
                            log.Info("End of notes deletion method");
                            Response.Redirect("Dashboard.aspx");
                        }
                    }
                }
            }
        }

        protected void btnPage_Click(object sender, EventArgs e)
        {
            LinkButton button = (LinkButton)sender;
            ViewState["CurrentPage"] = button.CommandArgument;
            queryNotesData();
        }

        protected void cbSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cbSelectAll = (CheckBox)sender;
            foreach (RepeaterItem repeaterItem in repNotes.Items)
            {
                CheckBox cbNote = (CheckBox)repeaterItem.FindControl("cbNote");
                cbNote.Checked = cbSelectAll.Checked;
            }
            updateMultiDeleteButtonClass();
        }

        protected void cbNote_CheckedChanged(object sender, EventArgs e)
        {
            updateMultiDeleteButtonClass();
            CheckBox cbSelectAll = (CheckBox)FindControlRecursive(Page, "cbSelectAll");
            foreach (RepeaterItem repeaterItem in repNotes.Items)
            {
                CheckBox cbNote = (CheckBox)repeaterItem.FindControl("cbNote");
                if (!cbNote.Checked)
                {
                    cbSelectAll.Checked = false;
                    return;
                }
            }
            cbSelectAll.Checked = true;
        }

        protected void btnPreNext_Command(object sender, CommandEventArgs e)
        {
            int cPage = Convert.ToInt16(ViewState["CurrentPage"]);
            int tPage = Convert.ToInt16(ViewState["TotalPages"]);
            string disabled = " disabled";
            switch (e.CommandName)
            {
                case "Previous":
                    if (cPage > 1)
                    {
                        cPage--;
                        if (cPage == 1)
                        {
                            btnPrevious.CssClass += disabled;
                        }
                        btnNext.CssClass.Replace(disabled, "");
                    }
                    else
                    {
                        cPage = 1;
                        btnPrevious.CssClass += disabled;
                    }
                    break;
                case "Next":
                    btnPrevious.CssClass.Replace(disabled, "");
                    if (cPage < tPage)
                    {
                        cPage++;
                        if (cPage == tPage)
                        {
                            btnNext.CssClass += disabled;
                        }
                        else
                        {

                            btnNext.CssClass.Replace(disabled, "");
                        }
                    }
                    else
                    {
                        cPage = tPage;
                        btnNext.CssClass += disabled;
                    }
                    break;
            }
            ViewState["CurrentPage"] = cPage;
            queryNotesData();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string encodedInput = hdnSearch.Value; // 獲取編碼後的值
            string userInput = Encoding.UTF8.GetString(Convert.FromBase64String(encodedInput)); // 解碼處理
            if (!string.IsNullOrEmpty(userInput))
            {
                ViewState["Word"] = HttpUtility.HtmlEncode(userInput);
            }
            else
            {
                ViewState["Word"] = null;
            }
            queryNotesData();
            hdnSearch.Value = "";
        }

        protected void paginationActiveStyle()
        {
            foreach (RepeaterItem item in repPage.Items)
            {
                int index = item.ItemIndex;
                LinkButton button = (LinkButton)item.FindControl("btnPage");

                int currentPage = Convert.ToInt32(ViewState["CurrentPage"]);

                if (index == currentPage - 1)
                {
                    button.CssClass += " active";
                }
                else
                {
                    button.CssClass = button.CssClass.Replace(" active", string.Empty);
                }
            }
        }

        private void updateMultiDeleteButtonClass()
        {
            int checkedItem = 0;
            foreach (RepeaterItem repeaterItem in repNotes.Items)
            {
                CheckBox cbNote = (CheckBox)repeaterItem.FindControl("cbNote");
                if (cbNote.Checked)
                {
                    checkedItem++;
                }
            }
            string currentClass = MultiDelete.Attributes["class"];
            if (checkedItem == 0)
            {
                if (!currentClass.Contains("disabled"))
                {
                    MultiDelete.Attributes["class"] = currentClass + " disabled";
                }
            }
            else
            {
                if (currentClass.Contains("disabled"))
                {
                    MultiDelete.Attributes["class"] = currentClass.Replace("disabled", "").Trim();
                }
            }
        }

        protected Control FindControlRecursive(Control root, string id)
        {
            if (root.ID == id)
            {
                return root;
            }
            foreach (Control control in root.Controls)
            {
                Control foundControl = FindControlRecursive(control, id);
                if (foundControl != null)
                {
                    return foundControl;
                }
            }
            return null;
        }
    }
}