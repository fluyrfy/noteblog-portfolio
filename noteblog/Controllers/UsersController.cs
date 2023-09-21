using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using noteblog.Models;
using noteblog.Utils;

namespace Userblog.Controllers
{
    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        private readonly UserRepository _repository;

        public UsersController()
        {
            _repository = new UserRepository();
        }

        [HttpGet]
        [Route("get/{userId}")]
        public HttpResponseMessage getUser(int userId)
        {
            try
            {
                User User = _repository.get(userId);

                if (User != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, User);
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
