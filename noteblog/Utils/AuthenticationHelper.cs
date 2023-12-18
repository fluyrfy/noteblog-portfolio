using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using Newtonsoft.Json;
using noteblog.Models;
using Org.BouncyCastle.Asn1.Ocsp;

namespace noteblog.Utils
{
  public class AuthenticationHelper
  {
    public static Dictionary<string, object> GetUserData()
    {
      if (HttpContext.Current.User.Identity.IsAuthenticated)
      {
        FormsIdentity id = (FormsIdentity)HttpContext.Current.User.Identity;
        FormsAuthenticationTicket ticket = id.Ticket;
        Dictionary<string, object> userDataDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(ticket.UserData);

        return userDataDictionary;
      }
      return null;
    }

    public static int GetUserId()
    {
      if (GetUserData() != null)
      {
        return Convert.ToInt32(GetUserData()["id"]);
      }
      return 0;
    }

    public static bool IsUserAuthenticatedAndTicketValid()
    {
      if (HttpContext.Current.User.Identity.IsAuthenticated)
      {
        FormsIdentity id = (FormsIdentity)HttpContext.Current.User.Identity;
        FormsAuthenticationTicket ticket = id.Ticket;

        if (!ticket.Expired)
        {
          return true;
        }
      }

      return false;
    }
  }
}