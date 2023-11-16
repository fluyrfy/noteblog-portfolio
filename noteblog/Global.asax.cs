using System;
using System.IO.Compression;
using System.Web;
using System.Web.Http;
using System.Web.Optimization;
using System.Web.Routing;
using noteblog.App_Start;
using noteblog.Utils;

namespace noteblog
{
    public class Global : HttpApplication
    {
        private Logger log;

        void Application_Start(object sender, EventArgs e)
        {

            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            //log4net.Config.XmlConfigurator.Configure();

            // 應用程式啟動時執行的程式碼
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            GlobalConfiguration.Configure(WebApiConfig.Register);

            // map path
            //string logPath = Server.MapPath("~/Logs");

            // check exist
            //if (!Directory.Exists(logPath))
            //{
            //    Directory.CreateDirectory(logPath);
            //}

            // folder permissions
            //var writeAllow = new FileSystemAccessRule("Users",
            //                                           FileSystemRights.Write,
            //                                           AccessControlType.Allow);
            //var writeDeny = new FileSystemAccessRule("Users",
            //                                          FileSystemRights.Write,
            //                                          AccessControlType.Deny);

            //var directorySecurity = Directory.GetAccessControl(logPath);

            //directorySecurity.AddAccessRule(writeAllow);
            //directorySecurity.AddAccessRule(writeDeny);
            //Directory.SetAccessControl(logPath, directorySecurity);

        }

        void Session_Start(object sender, EventArgs e)
        {
            log = new Logger(typeof(Global).Name);

            // Delete over 1 days log file
            DateTime currentDate = DateTime.Now;
            TimeSpan retentionPeriod = TimeSpan.FromDays(1);
            DateTime expiredTime = DateTime.Now.AddDays(-1);

            new LogRepository().delete(expiredTime);



            // 列出日誌目錄中的所有檔案
            //string logPath = Server.MapPath("~/Logs");
            //string[] logFiles = Directory.GetFiles(logPath, "*.log");

            //foreach (string logFile in logFiles)
            //{
            //    DateTime fileCreationDate = File.GetCreationTime(logFile);
            //    string fileName = Path.GetFileNameWithoutExtension(logFile);
            //    if ((currentDate - fileCreationDate > retentionPeriod) || !DateTime.TryParseExact(fileName, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            //    {
            //        File.Delete(logFile);
            //        log.Debug($"Delete log files older than seven days or other unexpected: {HttpUtility.UrlEncode(logFile)}");
            //    }
            //}
        }

        void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpContext context = HttpContext.Current;

            // 檢查是否已經進行了壓縮
            if (context.Request.Headers["Accept-encoding"] != null &&
                context.Request.Headers["Accept-encoding"].Contains("gzip"))
            {
                return; // 如果已經進行過壓縮，不再進行
            }

            // 進行GZip壓縮
            context.Response.Filter = new GZipStream(context.Response.Filter, CompressionMode.Compress);
            context.Response.AppendHeader("Content-encoding", "gzip");
            context.Response.Cache.VaryByHeaders["Accept-encoding"] = true;
        }

    }
}