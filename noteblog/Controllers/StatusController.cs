using System.Net;
using System.Net.Http;
using System.Web.Http;

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
    }
}
