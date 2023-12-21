using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
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

        [HttpGet]
        [Route("getByKeyword/{text}")]
        public HttpResponseMessage getUserByKeyword(string text, [FromUri] string selectedUsers)
        {
            try
            {
                int[] selectedOtherUserIds = selectedUsers?.Split(',').Select(int.Parse).ToArray() ?? new int[0];
                int selfUserId = AuthenticationHelper.GetUserId();
                int[] selectedUserIds = selectedOtherUserIds.Concat(new int[] { selfUserId }).ToArray();
                List<User> userList = _repository.getByKeyword(text);
                List<User> filteredUsers = userList.Where(user => !selectedUserIds.Contains(user.id)).ToList();

                if (filteredUsers.Count > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, filteredUsers);
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
