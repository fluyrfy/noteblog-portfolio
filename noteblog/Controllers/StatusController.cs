using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json.Linq;

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
