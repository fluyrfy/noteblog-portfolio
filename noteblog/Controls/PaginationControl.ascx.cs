using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;

namespace noteblog.Controls
{


    public partial class PaginationControl : System.Web.UI.UserControl
    {
        public class PageIndexChangedEventArgs : EventArgs
        {
            public int _currentPage { get; }

            public PageIndexChangedEventArgs(int currentPage)
            {
                _currentPage = currentPage;
            }
        }
        public event EventHandler<PageIndexChangedEventArgs> PageIndexChanged;

        public int totalRecords { get; set; }
        public int nowSetRecords { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            litDataCount.Text = totalRecords.ToString();
            litPageSize.Text = nowSetRecords < 5 ? nowSetRecords.ToString() : "5";
            if (!IsPostBack)
            {
                try
                {
                    litDataCount.Text = totalRecords.ToString();
                    litPageSize.Text = nowSetRecords < 5 ? nowSetRecords.ToString() : "5";
                    int totalPages = (int)Math.Ceiling((double)totalRecords / 5);
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
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        public void bindPagination()
        {
            try
            {
                litDataCount.Text = totalRecords.ToString();
                litPageSize.Text = nowSetRecords < 5 ? nowSetRecords.ToString() : "5";
                int totalPages = (int)Math.Ceiling((double)totalRecords / 5);
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
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        protected void btnPage_Command(object sender, CommandEventArgs e)
        {
            hidNowPage.Value = e.CommandArgument.ToString();
            if (this.PageIndexChanged != null)
            {
                this.PageIndexChanged(this, new PageIndexChangedEventArgs(Convert.ToInt32(e.CommandArgument)));
            }
            litDataCount.Text = totalRecords.ToString();
            litPageSize.Text = nowSetRecords < 5 ? nowSetRecords.ToString() : "5";
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
        }

    }
}