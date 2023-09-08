using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using JiebaNet.Segmenter;
using MySql.Data.MySqlClient;
using noteblog.Utils;
using Spectre.Console;

namespace noteblog
{

    public partial class Dashboard : Page
    {
        private JiebaSegmenter _segmenter;
        public Dashboard()
        {
            _segmenter = new JiebaSegmenter();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/Sign.aspx");
                return;
            }
            else
            {
                // 檢查票證是否過期
                if (FormsAuthenticationTicketExpired())
                {
                    // 票證過期，執行登出操作
                    FormsAuthentication.SignOut();
                    Response.Redirect("~/Sign.aspx");
                    return;
                }
                else
                {
                    if (User.Identity is FormsIdentity formsIdentity)
                    {
                        string name = AuthenticationHelper.GetUserData()["name"].ToString();
                        lblUser.Text = name;
                        byte[] avatar = new UserRepository().get(AuthenticationHelper.GetUserId()).avatar;
                        if (avatar.Length == 0)
                        {
                            imgAvatar.ImageUrl = "~/Images/logo/logo-icononly.png";
                        }
                        else
                        {
                            imgAvatar.ImageUrl = $"data:image/png;base64,{Convert.ToBase64String(avatar)}";
                        }
                    }
                }
            }

            if (!IsPostBack)
            {
                ViewState["CurrentPage"] = 1;
                queryNotesData();
            }
        }
        private Logger log = new Logger(typeof(Dashboard).Name);


        private bool FormsAuthenticationTicketExpired()
        {
            // 從 FormsIdentity 取得目前使用者的票證
            FormsIdentity identity = (FormsIdentity)User.Identity;
            FormsAuthenticationTicket ticket = identity.Ticket;

            // 檢查票證的到期時間是否已過期
            if (ticket.Expired)
            {
                return true;
            }
            return false;
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
                    sb.AppendLine("SELECT notes.*, categories.name as development FROM notes INNER JOIN categories ON notes.category_id = categories.id WHERE 1 = 1");
                    string keyQuery = "AND MATCH (title, content_text, keyword) AGAINST (@word IN BOOLEAN MODE) OR keyword LIKE @likeWord";
                    if (!string.IsNullOrEmpty(ViewState["Word"]?.ToString()))
                    {
                        string word = ViewState["Word"] as string;
                        sb.AppendLine(keyQuery);
                        da.SelectCommand.Parameters.AddWithValue("@word", word);
                        da.SelectCommand.Parameters.AddWithValue("@likeWord", $"%{word}%");
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
                        var word = ViewState["Word"] as string;
                        mCmd.Parameters.AddWithValue("@word", word);
                        mCmd.Parameters.AddWithValue("@likeWord", $"%{word}%");
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
            catch (Exception ex)
            {
                throw;
            }
            finally
            {

            }
        }

        protected void btnOut_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Response.Redirect("~/Sign.aspx");
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
                        con.Open();
                        MySqlTransaction transaction = con.BeginTransaction();
                        try
                        {
                            MySqlCommand cmd = new MySqlCommand(deleteQuery, con, transaction);
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

        protected void lbtnView_Command(object sender, CommandEventArgs e)
        {
            mvMainContent.ActiveViewIndex = Convert.ToInt32(e.CommandArgument);
            hidActiveView.Value = e.CommandArgument.ToString();
        }

        protected void btnSearch_Command(object sender, CommandEventArgs e)
        {
            if (e.CommandArgument.ToString() == "user")
            {

            }
            else
            {
                string encodedInput = hdnSearch.Value; // 獲取編碼後的值
                string userInput = HttpUtility.UrlDecode(Encoding.UTF8.GetString(Convert.FromBase64String(encodedInput))); // 解碼處理
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
        }

        protected void vManageUsers_Activate(object sender, EventArgs e)
        {
            bindUserRepeater();
        }

        protected void vManageCategories_Activate(object sender, EventArgs e)
        {
            bindCategoryRepeater();
        }

        protected void bindUserRepeater()
        {
            repUsers.DataSource = new UserRepository().getAll();
            repUsers.DataBind();
        }

        protected void bindCategoryRepeater()
        {
            repCategories.DataSource = new CategoryRepository().getAll();
            repCategories.DataBind();
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

        protected void btnManageUser_Command(object sender, CommandEventArgs e)
        {
            string method = e.CommandArgument.ToString();
            int id = Convert.ToInt32(hidUserId.Value);
            switch (method)
            {
                case "delete":
                    try
                    {
                        log.Info("Starting to delete user");
                        log.Debug($"Delete user id: {id}");
                        new UserRepository().delete(id);
                        log.Info("User deleted successfully");
                    }
                    catch (Exception ex)
                    {
                        log.Error("Failed to delete user", ex);
                    }
                    finally
                    {
                        log.Info("End of user deletion method");
                    }
                    break;
                case "update":
                    try
                    {
                        log.Info("Starting to update user");
                        string name = Request.Form["editUserName"];
                        string role = Request.Form["editUserRole"];
                        byte[] avatar = ConverterHelper.ConvertFileToBytes(Request.Files["editUserAvatar"].InputStream);
                        log.Debug($"Update user id: {id}, name: {name}, role: {role}, avatar: {avatar.Length}");
                        new UserRepository().update(name, avatar, role, id);
                        log.Info("User updated successfully");
                    }
                    catch (Exception ex)
                    {
                        log.Error("Failed to update user data", ex);
                    }
                    finally
                    {
                        log.Info("End of user data update method");
                    }
                    break;
            }
            bindUserRepeater();
        }

        protected void btnManageCategory_Command(object sender, CommandEventArgs e)
        {
            string method = e.CommandArgument.ToString();
            int id = Convert.ToInt32(hidCategoryId.Value);
            switch (method)
            {
                case "delete":
                    try
                    {
                        log.Info("Starting to delete category");
                        log.Debug($"Delete category id: {id}");
                        new CategoryRepository().delete(id);
                        log.Info("Category deleted successfully");
                    }
                    catch (Exception ex)
                    {
                        log.Error("Failed to delete category", ex);
                    }
                    finally
                    {
                        log.Info("End of category delete method");
                    }
                    break;
                case "update":
                    try
                    {
                        log.Info("Starting to update category");
                        string name = Request.Form["editCategoryName"];
                        string description = Request.Form["editCategoryDescription"];
                        log.Debug($"Update category name: {name}, description: {description}");
                        new CategoryRepository().update(id, name, description);
                        log.Info("Category updated successfully");
                    }
                    catch (Exception ex)
                    {
                        log.Error("Failed to update category", ex);
                    }
                    finally
                    {
                        log.Info("End of category update method");
                    }
                    break;
                case "insert":
                    try
                    {
                        log.Info("Starting to insert category");
                        string name = Request.Form["insertCategoryName"];
                        string description = Request.Form["insertCategoryDescription"];
                        log.Debug($"Insert category name: {name}, description: {description}");
                        new CategoryRepository().insert(name, description);
                        log.Info("Category inserted successfully");
                    }
                    catch (Exception ex)
                    {
                        log.Error("Failed to insert category", ex);
                    }
                    finally
                    {
                        log.Info("End of category insert method");
                    }
                    break;
            }
            bindCategoryRepeater();
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