using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace noteblog
{
    /// <summary>
    /// Handler1 的摘要描述
    /// </summary>
    public class Handler1 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {

            string filePath = context.Server.MapPath("~/Files/Fan_Resume.pdf");
            string fileName = "FrankLiao_Resume.pdf";

            context.Response.Clear();
            context.Response.ContentType = "application/pdf";
            context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
            context.Response.StatusCode = 200;

            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                fileStream.CopyTo(context.Response.OutputStream);
                context.Response.Flush();
            }

            context.Response.End();
        }

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }
    }
}