using System;
using System.Web;
using noteblog.Models;

namespace noteblog
{
    /// <summary>
    /// Handler1 的摘要描述
    /// </summary>
    public class Handler1 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            int userId = Convert.ToInt32(context.Request.QueryString["userId"]);
            User user = new UserRepository().get(userId);
            if (user != null && user.resume != null)
            {
                if (user.resume.Length > 0)
                {
                    // string filePath = context.Server.MapPath("~/Files/Fan_Resume.pdf");
                    string fileName = $"{user.name}_Resume.pdf";

                    context.Response.Clear();
                    context.Response.ContentType = "application/pdf";
                    context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
                    context.Response.StatusCode = 200;
                    context.Response.OutputStream.Write(user.resume, 0, user.resume.Length);
                    //context.Response.BinaryWrite(user.resume);
                    context.Response.Flush();
                    // using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    // {
                    //   fileStream.CopyTo(context.Response.OutputStream);
                    //   context.Response.Flush();
                    // }
                }

            }
            context.Response.Close();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}