using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Web.Http;
using noteblog.Models;

namespace noteblog.Controllers
{
    [RoutePrefix("api/drafts")]
    public class DraftsController : ApiController
    {
        private readonly DraftRepository _repository = new DraftRepository();

        [HttpPost]
        [Route("save")]
        public IHttpActionResult SaveDraft([FromBody] Draft draft)
        {
            try
            {
                if (_repository.isDraftExist())
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
        [Route("get/{id}")]
        public IHttpActionResult GetDraft(int id)
        {
            try
            {
                if (id >= 0 && id < drafts.Count)
                {
                    // 根据 ID 获取草稿内容
                    string draftContent = drafts[id];
                    return Ok(draftContent);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
