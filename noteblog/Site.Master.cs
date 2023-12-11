using System;
using System.Web.Configuration;
using System.Web.UI;

namespace noteblog
{
  public partial class SiteMaster : MasterPage
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      string apiKey = WebConfigurationManager.AppSettings["ChatGPTApiKey"];
      Response.Write("<script id='apiScript'>");
      Response.Write("var apiKey = '" + apiKey + "'");
      Response.Write("</script>");
    }
  }
}