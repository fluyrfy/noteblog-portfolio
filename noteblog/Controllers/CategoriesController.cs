using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using noteblog.Models;

namespace noteblog.Controllers
{
    [RoutePrefix("api/categories")]
    public class CategoriesController : ApiController
    {
        private readonly CategoryRepository _repository;

        public CategoriesController()
        {
            _repository = new CategoryRepository();
        }

        //[HttpPost]
        //[Route("save")]
        //public IHttpActionResult saveCategory([FromBody] Category category)
        //{
        //    try
        //    {
        //        category.userId = _userId;
        //        if (_repository.isCategoryExist(_userId, category.categoryId))
        //        {
        //            bool updatedSuccessfully = _repository.update(category);
        //            if (updatedSuccessfully)
        //            {
        //                return Ok("category updated successfully.");
        //            }
        //            return BadRequest("category could not be updated.");
        //        }
        //        else
        //        {
        //            bool insertedSuccessfully = _repository.insert(category);
        //            if (insertedSuccessfully)
        //            {
        //                return Ok("category saved successfully.");
        //            }
        //            return BadRequest("category could not be saved.");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return InternalServerError(ex);
        //    }
        //}

        [HttpGet]
        [Route("get/{categoryId}")]
        public HttpResponseMessage getCategory(int categoryId)
        {
            try
            {
                Category category = _repository.get(categoryId);

                if (category != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, category);
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

        //[HttpDelete]
        //[Route("delete/{categoryId}")]
        //public IHttpActionResult deleteCategory(int categoryId)
        //{
        //    try
        //    {
        //        bool deletedSuccessfully = _repository.delete(_userId, categoryId);
        //        if (deletedSuccessfully)
        //        {
        //            return Ok("Category deleted successfully.");
        //        }
        //        return NotFound();
        //    }
        //    catch (Exception ex)
        //    {
        //        return InternalServerError(ex);
        //    }
        //}

    }
}
