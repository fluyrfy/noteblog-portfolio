using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using noteblog.Utils;

namespace noteblog.Controllers
{
    [RoutePrefix("api/stats")]
    public class StatsController : ApiController
    {
        private readonly AccessStatsRepository _repository;

        public StatsController()
        {
            _repository = new AccessStatsRepository();
        }

        [HttpPost]
        [Route("save")]
        public HttpResponseMessage saveStats([FromBody] dynamic value)
        {
            var pageName = value.pageName.Value;
            string a = _repository.insert(pageName);
            return Request.CreateResponse(HttpStatusCode.OK, a);
        }
    }
}
