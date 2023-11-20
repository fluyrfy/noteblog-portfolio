using System;
using System.Web.UI;

namespace noteblog
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //RequestHelper.HandleHeadRequest();
            if (!IsPostBack)
            {
                new AccessStatsRepository().insert("Default");
            }
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
    }
}