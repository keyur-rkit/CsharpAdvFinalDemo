using API.BL.Operations;
using API.Filters;
using API.Helpers;
using API.Models;
using API.Models.DTO;
using API.Models.Enum;
using System;
using System.Net.Http;
using System.Web.Http;

namespace API.Controllers
{
    /// <summary>
    /// Controller to handle CRUD of BKS01
    /// </summary>
    [RoutePrefix("api/Books")]
    [ValidateModelState]
    public class CLBKS01Controller : ApiController
    {
        private Response _objResponse;
        private BLBKS01 _objBLBKS01;

        public string cacheKey = "GetAllCacheKey";

        public CLBKS01Controller()
        {
            _objBLBKS01 = new BLBKS01();
            _objResponse = new Response();
        }

        /// <summary>
        /// Get all books
        /// </summary>
        /// <returns>List of all books</returns>
        [HttpGet]
        [Route("GetAllBooks")]
        public IHttpActionResult GetAllBooks()
        {
            var cachedResponse = CacheHelper.Get(cacheKey);

            if (cachedResponse != null)
            {
                return Ok(cachedResponse);
            }

            _objResponse = _objBLBKS01.GetAll();
            CacheHelper.Set(cacheKey, _objResponse, TimeSpan.FromSeconds(30));
            return Ok(_objResponse);
        }

        /// <summary>
        /// Get book by ID
        /// </summary>
        /// <param name="id">Book ID</param>
        /// <returns>Book details</returns>
        [HttpGet]
        [Route("GetBookById")]
        public IHttpActionResult GetBookById(int id)
        {
            _objResponse = _objBLBKS01.GetById(id);
            return Ok(_objResponse);
        }

        /// <summary>
        /// Add a new book
        /// </summary>
        /// <param name="objDTOBKS01">Book data to add</param>
        /// <returns>Response of the add operation</returns>
        [HttpPost]
        [Route("AddBook")]
        [JWTAuthorizationFilter("Admin")]
        public IHttpActionResult AddBook(DTOBKS01 objDTOBKS01)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            _objBLBKS01.Type = EnmType.A;
            _objBLBKS01.PreSave(objDTOBKS01);
            _objResponse = _objBLBKS01.Validation();

            if (!_objResponse.IsError)
            {
                _objBLBKS01.Save();
            }

            CacheHelper.Remove(cacheKey);
            return Ok(_objResponse);
        }

        /// <summary>
        /// Edit an existing book
        /// </summary>
        /// <param name="id">Book ID</param>
        /// <param name="objDTOBKS01">Updated book data</param>
        /// <returns>Response of the edit operation</returns>
        [HttpPut]
        [Route("EditBook")]
        [JWTAuthorizationFilter("Admin")]
        public IHttpActionResult EditBook(int id, DTOBKS01 objDTOBKS01)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            _objBLBKS01.Type = EnmType.E;
            _objBLBKS01.Id = id;
            _objBLBKS01.PreSave(objDTOBKS01);
            _objResponse = _objBLBKS01.Validation();

            if (!_objResponse.IsError)
            {
                _objBLBKS01.Save();
            }

            CacheHelper.Remove(cacheKey);
            return Ok(_objResponse);
        }

        /// <summary>
        /// Delete a book
        /// </summary>
        /// <param name="id">Book ID</param>
        /// <returns>Response of the delete operation</returns>
        [HttpDelete]
        [Route("DeleteBook")]
        [JWTAuthorizationFilter("Admin")]
        public IHttpActionResult DeleteBook(int id)
        {
            _objBLBKS01.Type = EnmType.D;
            _objBLBKS01.Id = id;
            _objResponse = _objBLBKS01.Validation();
            if (!_objResponse.IsError)
            {
                _objResponse = _objBLBKS01.Delete();
            }

            CacheHelper.Remove(cacheKey);
            return Ok(_objResponse);
        }
    }
}