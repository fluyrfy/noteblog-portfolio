using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using JiebaNet.Segmenter;
using MySql.Data.MySqlClient;
using noteblog.Controls;
using noteblog.Models;
using noteblog.Utils;
using Spectre.Console;
using static noteblog.Controls.PaginationControl;

namespace noteblog
{
    public partial class Dashboard : Page
    {
        private int _userId;
        private string _role;
        public Dashboard()
        {
            if (AuthenticationHelper.GetUserId() != 0)
            {
                _userId = AuthenticationHelper.GetUserId();
                _role = new UserRepository().get(AuthenticationHelper.GetUserId()).role;
            }
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
                        bindUserData();
                    }
                }
            }

            //PaginationControlNotes.PageIndexChanged += PaginationControl_PageButtonClick;
            subscribeToChildControlEvents(this);

            if (!IsPostBack)
            {
                ViewState["CurrentPage"] = 1;
                queryNotesData();
            }
        }

        private Logger log = new Logger(typeof(Dashboard).Name);

        // bind child control event
        private void subscribeToChildControlEvents(Control parentControl)
        {
            foreach (Control childControl in parentControl.Controls)
            {
                if (childControl is PaginationControl)
                {
                    var userControl = (PaginationControl)childControl;
                    userControl.PageIndexChanged += PaginationControl_PageButtonClick;
                }

                if (childControl.HasControls())
                {
                    subscribeToChildControlEvents(childControl);
                }
            }
        }

        // child control pagination click event
        public void PaginationControl_PageButtonClick(object sender, PageIndexChangedEventArgs e)
        {
            ViewState["CurrentPage"] = e._currentPage;
            switch (hidActiveSidebarItem.Value)
            {
                case "notes":
                    queryNotesData();
                    break;
                case "users":
                    bindUserData();
                    break;
                case "categories":
                    bindCategoryRepeater();
                    break;
                case "logs":
                    bindLogData();
                    break;
            }
        }

        // get & bind notes list
        protected void queryNotesData()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["Noteblog"].ConnectionString))
                {
                    MySqlCommand cmd = new MySqlCommand();
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("SELECT notes.*, categories.name as development FROM notes INNER JOIN categories ON notes.category_id = categories.id WHERE 1 = 1");
                    if (_role != "admin")
                    {
                        sb.AppendLine("AND user_id = @userId");
                        cmd.Parameters.AddWithValue("@userId", _userId);
                    }
                    string keyQuery = "AND MATCH (title, content_text, keyword) AGAINST (@word IN BOOLEAN MODE) OR keyword LIKE @likeWord";
                    if (!string.IsNullOrEmpty(ViewState["Word"]?.ToString()))
                    {
                        string word = ViewState["Word"] as string;
                        sb.AppendLine(keyQuery);
                        cmd.Parameters.AddWithValue("@word", word);
                        cmd.Parameters.AddWithValue("@likeWord", $"%{word}%");
                    }
                    sb.AppendLine("ORDER BY updated_at DESC");
                    string dataCountSql = sb.ToString();
                    sb.AppendLine("LIMIT 5 OFFSET @offset");
                    string nowSetSql = sb.ToString();
                    cmd.Parameters.AddWithValue("@offset", (Convert.ToInt32(ViewState["CurrentPage"]) - 1) * 5);
                    int a = Convert.ToInt32(ViewState["CurrentPage"]);
                    cmd.Connection = conn;
                    cmd.CommandText = sb.ToString();
                    DataTable dt = new DataTable();
                    conn.Open();
                    dt.Load(cmd.ExecuteReader());
                    repNotes.DataSource = dt;
                    repNotes.DataBind();
                    cmd.CommandText = DatabaseHelper.GetTotalRecords(dataCountSql);
                    var totalRecords = Convert.ToInt32(cmd.ExecuteScalar());
                    cmd.CommandText = DatabaseHelper.GetTotalRecords(nowSetSql);
                    var nowSetRecords = Convert.ToInt32(cmd.ExecuteScalar());
                    bindPagination("PaginationControlNotes", totalRecords, nowSetRecords);
                }
            }
            catch (Exception ex)
            {
                throw;
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

        protected void lbtnView_Command(object sender, CommandEventArgs e)
        {
            ViewState["CurrentPage"] = 1;
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


        protected void vManageNotes_Activate(object sender, EventArgs e)
        {
            ViewState["CurrentPage"] = 1;
            queryNotesData();
        }

        protected void vManageUsers_Activate(object sender, EventArgs e)
        {
            ViewState["CurrentPage"] = 1;
            bindUserRepeater();
        }

        protected void vManageCategories_Activate(object sender, EventArgs e)
        {
            bindCategoryRepeater();
        }

        protected void vManageProfile_Activate(object sender, EventArgs e)
        {
            bindProfileData();
        }

        protected void vManageLogs_Activate(object sender, EventArgs e)
        {
            bindLogData();
        }

        protected void bindPagination(string paginationControlId, int totalRecords, int nowSetRecords)
        {
            PaginationControl paginationControl = this.Master.FindControl("MainContent").FindControl(paginationControlId) as PaginationControl;
            if (paginationControl != null)
            {
                paginationControl.totalRecords = totalRecords;
                paginationControl.nowSetRecords = nowSetRecords;
                paginationControl.bindPagination();
            }
        }

        protected void bindUserRepeater()
        {
            repUsers.DataSource = new UserRepository().getAll(out int tr, out int nsr, Convert.ToInt32(ViewState["CurrentPage"]));
            repUsers.DataBind();
            bindPagination("PaginationControlUsers", tr, nsr);
        }

        protected void bindCategoryRepeater()
        {
            repCategories.DataSource = new CategoryRepository().getAll(out int tr, out int nsr, (int)ViewState["CurrentPage"]);
            repCategories.DataBind();
            bindPagination("PaginationControlCategories", tr, nsr);
        }

        protected void bindProfileData()
        {
            User user = new UserRepository().get(AuthenticationHelper.GetUserId());
            byte[] avatar = new byte[0];
            if (user.avatar != null)
            {
                avatar = user.avatar;
            }
            string name = user.name;
            txtEditProfileName.Text = name;
            if (avatar.Length == 0)
            {
                imgProfileAvatar.ImageUrl = "~/Images/logo/logo-icononly.png";
            }
            else
            {
                imgProfileAvatar.ImageUrl = $"data:image/png;base64,{Convert.ToBase64String(avatar)}";
            }
        }

        protected void bindLogData()
        {
            repLogs.DataSource = new LogRepository().getAll(out int tr, out int nsr, (int)ViewState["CurrentPage"]);
            repLogs.DataBind();
            bindPagination("PaginationControlLogs", tr, nsr);
        }

        protected void bindUserData()
        {
            int id = AuthenticationHelper.GetUserId();
            User user = new UserRepository().get(id);
            string name = new UserRepository().get(id).name;
            lblUser.Text = name;
            string role = user.role;
            if (role == "admin")
            {
                pnlAdmin.Visible = true;
            }
            byte[] avatar = new UserRepository().get(id).avatar;
            if (avatar == null || avatar.Length == 0)
            {
                imgAvatar.ImageUrl = "~/Images/logo/logo-icononly.png";
            }
            else
            {
                imgAvatar.ImageUrl = $"data:image/png;base64,{Convert.ToBase64String(avatar)}";
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
                        new UserRepository().update(name, avatar, id, role);
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

        protected void btnManageProfile_Command(object sender, CommandEventArgs e)
        {
            string method = e.CommandArgument.ToString();
            int id = AuthenticationHelper.GetUserId();
            switch (method)
            {
                case "update":
                    try
                    {
                        log.Info("Starting to update profile");
                        string name = txtEditProfileName.Text;
                        byte[] avatar = new byte[0];
                        if (fuEditProfileAvatar.HasFile)
                        {
                            avatar = ConverterHelper.ConvertFileToBytes(fuEditProfileAvatar.PostedFile.InputStream);
                        }
                        log.Debug($"Update profile name: {name}, avatar: {avatar.Length}");
                        new UserRepository().update(name, avatar, id);
                        log.Info("Profile updated successfully");
                    }
                    catch (Exception ex)
                    {
                        log.Error("Failed to update profile", ex);
                    }
                    finally
                    {
                        log.Info("End of profile update method");
                    }
                    break;
            }
            bindProfileData();
            bindUserData();
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