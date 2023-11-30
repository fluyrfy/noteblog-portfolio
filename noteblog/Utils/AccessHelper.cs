using System.Text.RegularExpressions;
using System.Web;

namespace noteblog.Utils
{
    public class AccessHelper
    {
        public static string GetIPAddress()
        {
            HttpContext context = HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            string cleanIp = "";

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    
                    cleanIp = Regex.Match(addresses[0], @"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}").Value;
                }
            } else
            {                
                cleanIp = Regex.Match(context.Request.ServerVariables["REMOTE_ADDR"], @"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}").Value;
            }
            return cleanIp;
        }
    }
}