using API.BL.Operations;
using API.Filters;
using System.Web.Http;
using API.Models;
using API.Models.DTO;
using API.Models.Enum;

namespace API.Controllers
{
    /// <summary>
    /// Controller to handle CRUD of CAT01
    /// </summary>
    [RoutePrefix("api/Categories")]
    [ValidateModelState]
    public class CLCAT01Controller : ApiController
    {
        private Response _objResponse;
        private BLCAT01 _objBLCAT01;

        public CLCAT01Controller()
        {
            _objBLCAT01 = new BLCAT01();
            _objResponse = new Response();
        }

        /// <summary>
        /// Get all categories
        /// </summary>
        /// <returns>List of all categories</returns>
        [HttpGet]
        [Route("GetAllCategories")]
        public IHttpActionResult GetAllCategories()
        {
            _objResponse = _objBLCAT01.GetAll();

            return Ok(_objResponse);
        }

        /// <summary>
        /// Get category by ID
        /// </summary>
        /// <param name="id">Category ID</param>
        /// <returns>Category details</returns>
        [HttpGet]
        [Route("GetCategoryById")]
        public IHttpActionResult GetCategoryById(int id)
        {
            _objResponse = _objBLCAT01.GetById(id);
            return Ok(_objResponse);
        }

        /// <summary>
        /// Add a new category
        /// </summary>
        /// <param name="objDTOCAT01">Category data to add</param>
        /// <returns>Response of the add operation</returns>
        [HttpPost]
        [Route("AddCategory")]
        [JWTAuthorizationFilter("Admin")]
        public IHttpActionResult AddCategory(DTOCAT01 objDTOCAT01)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            _objBLCAT01.Type = EnmType.A;
            _objBLCAT01.PreSave(objDTOCAT01);
            _objResponse = _objBLCAT01.Validation();

            if (!_objResponse.IsError)
            {
                _objBLCAT01.Save();
            }

            return Ok(_objResponse);
        }

        /// <summary>
        /// Edit an existing category
        /// </summary>
        /// <param name="id">Category ID</param>
        /// <param name="objDTOCAT01">Updated category data</param>
        /// <returns>Response of the edit operation</returns>
        [HttpPut]
        [Route("EditCategory")]
        [JWTAuthorizationFilter("Admin")]
        public IHttpActionResult EditCategory(int id, DTOCAT01 objDTOCAT01)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            _objBLCAT01.Type = EnmType.E;
            _objBLCAT01.Id = id;
            _objBLCAT01.PreSave(objDTOCAT01);
            _objResponse = _objBLCAT01.Validation();

            if (!_objResponse.IsError)
            {
                _objBLCAT01.Save();
            }

            return Ok(_objResponse);
        }

        /// <summary>
        /// Delete a category
        /// </summary>
        /// <param name="id">Category ID</param>
        /// <returns>Response of the delete operation</returns>
        [HttpDelete]
        [Route("DeleteCategory")]
        [JWTAuthorizationFilter("Admin")]
        public IHttpActionResult DeleteCategory(int id)
        {
            _objBLCAT01.Type = EnmType.D;
            _objBLCAT01.Id = id;
            _objResponse = _objBLCAT01.Validation();
            if (!_objResponse.IsError)
            {
                _objResponse = _objBLCAT01.Delete();
            }
            return Ok(_objResponse);
        }
    }
}