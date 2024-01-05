using System;
using System.Configuration;
using System.Web.UI;
using noteblog.Models;

namespace noteblog
{
  public partial class SiteMaster : MasterPage
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      string apiKey = ConfigurationManager.AppSettings["ChatGPTApiKey"];
      Response.Write("<script id='apiScript'>");
      Response.Write("var apiKey = '" + apiKey + "'");
      Response.Write("</script>");
      if (!IsPostBack)
      {
        // var id = AuthenticationHelper.GetUserId();
        // id = id == 0 ? 1 : id;
        // User user = new UserRepository().get(id);
        // litUserName.Text = user.name;
        // hlkUserGitHub.NavigateUrl = user.githubLink;
        string userId = Request.QueryString["uid"];
        User user = new UserRepository().get(userId);
        litUserName.Text = user.name;
        hlkUserGitHub.NavigateUrl = user.githubLink;
      }
    }
  }
}