using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

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
            int nowPage = Convert.ToInt32(hidNowPage.Value);
            switch (e.CommandName)
            {
                case "Previous":
                    if (nowPage > 1)
                    {
                        nowPage = nowPage - 1;
                    }
                    break;
                case "Next":
                    nowPage = nowPage + 1;
                    break;
            }
            PageIndexChanged(this, new PageIndexChangedEventArgs(nowPage));
            hidNowPage.Value = nowPage.ToString();
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
    }
}