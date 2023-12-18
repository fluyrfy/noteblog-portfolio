using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using noteblog.Models;

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
        string uid = Request.QueryString["uid"];
        if (string.IsNullOrEmpty(uid))
        {
          uid = "1";
        }
        int userId = Convert.ToInt32(uid);
        UserRepository userRepository = new UserRepository();
        User user = userRepository.get(Convert.ToInt32(userId));
        lblBiography.Text = user.about;
        litContactEmail.Text = user.email;
        litContactRegionName.Text = user.region;
        litContactPhone.Text = user.phone;
        hlkContactRegionLink.NavigateUrl = user.regionLink;
        repUserSkills.DataSource = userRepository.getSkills(userId);
        repUserSkills.DataBind();
      }
    }

        protected void repUserSkills_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                UserSkill userSkill = (UserSkill)e.Item.DataItem;
                string skillName = userSkill.name;

                if (string.IsNullOrEmpty(skillName))
                {
                    e.Item.Visible = false;
                }
                else
                {
                    e.Item.Visible = true;
                }
            }
        }

        protected void btnDownload_Click(object sender, EventArgs e)
    {
      // 設定下載檔案的路徑和檔名
      string filePath = Server.MapPath("~/Files/Fan_Resume.pdf");
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