using API.BL.Operations;
using API.Filters;
using API.Models;
using API.Models.DTO;
using API.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API.Controllers
{
    /// <summary>
    /// Controller to handle CRUD of AUH01
    /// </summary>
    [RoutePrefix("api/Authors")]
    public class CLAUH01Controller : ApiController
    {
        private Response _objResponse;
        private BLAUH01 _objBLAUH01;

        public CLAUH01Controller()
        {
            _objBLAUH01 = new BLAUH01();
            _objResponse = new Response();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllAuthors")]
        public IHttpActionResult GetAllAuthors()
        {
            _objResponse = _objBLAUH01.GetAll();

            return Ok(_objResponse);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAuthorById")]
        public IHttpActionResult GetAuthorById(int id)
        {
            _objResponse = _objBLAUH01.GetById(id);
            return Ok(_objResponse);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objDTOAUH01"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddAuthor")]
        [JWTAuthorizationFilter("Admin")]
        public IHttpActionResult AddAuthor(DTOAUH01 objDTOAUH01)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _objBLAUH01.Type = EnmType.A;
            _objBLAUH01.PreSave(objDTOAUH01);
            _objResponse = _objBLAUH01.Validation();

            if (!_objResponse.IsError)
            {
                _objBLAUH01.Save();
            }

            return Ok(_objResponse);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="objDTOAUH01"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("EditAuhtor")]
        [JWTAuthorizationFilter("Admin")]
        public IHttpActionResult EditAuhtor(int id, DTOAUH01 objDTOAUH01)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _objBLAUH01.Type = EnmType.E;
            _objBLAUH01.Id = id;
            _objBLAUH01.PreSave(objDTOAUH01);
            _objResponse = _objBLAUH01.Validation();

            if (!_objResponse.IsError)
            {
                _objBLAUH01.Save();
            }

            return Ok(_objResponse);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("DeleteAuthor")]
        [JWTAuthorizationFilter("Admin")]
        public IHttpActionResult DeleteAuthor(int id)
        {
            _objBLAUH01.Type = EnmType.D;
            _objBLAUH01.Id = id;
            _objResponse = _objBLAUH01.Validation();
            if (!_objResponse.IsError)
            {
                _objResponse = _objBLAUH01.Delete();
            }
            return Ok(_objResponse);
        }
    }
}
