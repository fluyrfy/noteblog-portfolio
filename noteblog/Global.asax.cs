using noteblog.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace noteblog
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // 應用程式啟動時執行的程式碼
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<NoteDBContext>());

            // map path
            string logPath = Server.MapPath("~/Logs");

            // check exist
            if (!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath);
            }

            // folder permissions
            var writeAllow = new FileSystemAccessRule("Users",
                                                       FileSystemRights.Write,
                                                       AccessControlType.Allow);
            var writeDeny = new FileSystemAccessRule("Users",
                                                      FileSystemRights.Write,
                                                      AccessControlType.Deny);

            var directorySecurity = Directory.GetAccessControl(logPath);

            directorySecurity.AddAccessRule(writeAllow);
            //directorySecurity.AddAccessRule(writeDeny);

            Directory.SetAccessControl(logPath, directorySecurity);
            log4net.Config.XmlConfigurator.Configure();
        }
    }
}