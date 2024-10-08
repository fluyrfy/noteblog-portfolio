﻿using System;
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
    public HttpResponseMessage getUser(string userId)
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
    public HttpResponseMessage getUserByKeyword(string text, [FromUri] string selectedUsers = "")
    {
      try
      {
        string[] selectedOtherUserIds = selectedUsers?.Split(',').ToArray() ?? new string[0];
        string selfUserId = AuthenticationHelper.GetUserId();
        string[] selectedUserIds = selectedOtherUserIds.Concat(new string[] { selfUserId }).ToArray();
        HashSet<string> selectedIdSet = new HashSet<string>(selectedUserIds);
        List<User> userList = _repository.getByKeyword(text);
        List<User> filteredUsers = userList.Where(user => !selectedIdSet.Contains(user.id.ToString())).ToList();

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

    [HttpGet]
    [Route("auth-state")]
    public IHttpActionResult getLoginStatus()
    {
      bool isLoggedIn = AuthenticationHelper.IsUserAuthenticatedAndTicketValid();
      string status = isLoggedIn ? "active" : "inactive";
      var userInfo = new { status, userId = AuthenticationHelper.GetUserId() };
      return Ok(userInfo);
    }
  }
}
