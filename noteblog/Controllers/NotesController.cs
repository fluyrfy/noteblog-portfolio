using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using noteblog.Utils;

namespace noteblog.Controllers
{
    [RoutePrefix("api/notes")]
    public class NotesController : ApiController
    {
        private readonly int _userId;
        private readonly NoteRepository _repository;

        public NotesController()
        {
            _userId = AuthenticationHelper.GetUserId();
            _repository = new NoteRepository();
        }

        [HttpGet]
        [Route("getLatestNoteTime")]
        public HttpResponseMessage getLatestNoteTime()
        {
            try
            {
                Models.Note note = _repository.getLatestNote();

                if (note != null)
                {
                    return Request.CreateResponse(System.Net.HttpStatusCode.OK, note.updatedAt);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NoContent);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(InternalServerError(ex));
            }
        }
    }
}
