using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using noteblog.Utils;

namespace noteblog.Controllers
{
  [RoutePrefix("api/status")]
  public class StatusController : ApiController
  {
    [HttpGet]
    [Route("check")]
    public HttpResponseMessage getStatus()
    {
      return Request.CreateResponse(HttpStatusCode.OK, "good :)");
    }
    [HttpGet]
    [Route("clearCache")]
    public HttpResponseMessage clearCache()
    {
      try
      {
        CacheHelper.ClearAllCache();
        return Request.CreateResponse(HttpStatusCode.OK, "cache is cleared");
      }
      catch (Exception ex)
      {
        return Request.CreateResponse(HttpStatusCode.InternalServerError, $"cache is not cleared: {ex.Message}");
      }
    }
  }
}
