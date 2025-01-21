using API.BL.Operations;
using API.Filters;
using API.Models;
using API.Models.DTO;
using API.Models.Enum;
using System.Net.Http;
using System.Web.Http;

namespace API.Controllers
{
    [RoutePrefix("api/Books")]
    public class CLBKS01Controller : ApiController
    {
        private Response _objResponse;
        private BLBKS01 _objBLBKS01;

        public CLBKS01Controller()
        {
            _objBLBKS01 = new BLBKS01();
            _objResponse = new Response();
        }

        [HttpGet]
        [Route("GetAllBooks")]
        public IHttpActionResult GetAllBooks()
        {
            _objResponse = _objBLBKS01.GetAll();

            return Ok(_objResponse);
        }

        [HttpGet]
        [Route("GetBookById")]
        public IHttpActionResult GetBookById(int id)
        {
            _objResponse = _objBLBKS01.GetById(id);
            return Ok(_objResponse);
        }

        [HttpPost]
        [Route("AddBook")]
        [JWTAuthorizationFilter("Admin")]
        public IHttpActionResult AddBook(DTOBKS01 objDTOBKS01)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _objBLBKS01.Type = EnmType.A;
            _objBLBKS01.PreSave(objDTOBKS01);
            _objResponse = _objBLBKS01.Validation();

            if (!_objResponse.IsError)
            {
                _objBLBKS01.Save();
            }

            return Ok(_objResponse);
        }

        [HttpPut]
        [Route("EditBook")]
        [JWTAuthorizationFilter("Admin")]
        public IHttpActionResult EditBook(int id, DTOBKS01 objDTOBKS01)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _objBLBKS01.Type = EnmType.E;
            _objBLBKS01.Id = id;
            _objBLBKS01.PreSave(objDTOBKS01);
            _objResponse = _objBLBKS01.Validation();

            if (!_objResponse.IsError)
            {
                _objBLBKS01.Save();
            }

            return Ok(_objResponse);
        }

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
            return Ok(_objResponse);
        }
    }
}
