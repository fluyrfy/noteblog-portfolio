using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using noteblog.Models;
using noteblog.Utils;

namespace noteblog.Controllers
{
    [RoutePrefix("api/drafts")]
    public class DraftsController : ApiController
    {
        private readonly int _userId;
        private readonly DraftRepository _repository;

        public DraftsController()
        {
            _userId = AuthenticationHelper.GetUserId();
            _repository = new DraftRepository(_userId);
        }

        [HttpPost]
        [Route("save")]
        public IHttpActionResult saveDraft([FromBody] Draft draft)
        {
            try
            {
                draft.userId = _userId;
                if (_repository.isDraftExist(_userId, draft.noteId))
                {
                    bool updatedSuccessfully = _repository.update(draft);
                    if (updatedSuccessfully)
                    {
                        return Ok("draft updated successfully.");
                    }
                    return BadRequest("draft could not be updated.");
                }
                else
                {
                    bool insertedSuccessfully = _repository.insert(draft);
                    if (insertedSuccessfully)
                    {
                        return Ok("draft saved successfully.");
                    }
                    return BadRequest("draft could not be saved.");
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("get/{noteId}")]
        public HttpResponseMessage getDraft(int noteId)
        {
            try
            {
                Draft draft = _repository.get(_userId, noteId);

                if (_repository.isDraftExist(_userId, noteId) && draft != null)
                {
                    return Request.CreateResponse(System.Net.HttpStatusCode.OK, draft);
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

        [HttpDelete]
        [Route("delete/{noteId}")]
        public IHttpActionResult deleteDraft(int noteId)
        {
            try
            {
                bool deletedSuccessfully = _repository.delete(_userId, noteId);
                if (deletedSuccessfully)
                {
                    return Ok("Draft deleted successfully.");
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

    }
}
