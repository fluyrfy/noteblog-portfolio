using System.Net;
using System.Net.Http;
using System.Web.Http;

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

        [HttpGet]
        [Route("get")]
        public object getStats(string startDate, string endDate)
        {
            var notes = _repository.getNotes(startDate, endDate);
            var visits = _repository.getVisits(startDate, endDate);
            var regions = _repository.getRegions(startDate, endDate);
            var ctr = _repository.getNoteCTR(startDate, endDate);
            return new
            {
                notes = notes,
                visits = visits,
                regions = regions,
                ctr = ctr
            };
        }

        [HttpPost]
        [Route("save")]
        public HttpResponseMessage saveStats([FromBody] dynamic value)
        {
            var pageName = value.pageName.Value;
            string response = _repository.insert(pageName);
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [HttpPost]
        [Route("saveNoteCTR")]
        public HttpResponseMessage saveNoteCTR([FromBody] dynamic value)
        {
            var noteId = value.noteId.Value;
            string response = _repository.insertNoteCTR(noteId);
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
    }
}
