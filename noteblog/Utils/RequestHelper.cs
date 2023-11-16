using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace noteblog.Utils
{
    public class RequestHelper
    {
        public static void HandleHeadRequest()
        {
            if (HttpContext.Current.Request.HttpMethod == "HEAD")
            {
                // 如果是HEAD請求，處理相應的邏輯
                HttpContext.Current.Response.StatusCode = 200;
                HttpContext.Current.Response.StatusDescription = "OK";
                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.End();
            }
        }
    }
}