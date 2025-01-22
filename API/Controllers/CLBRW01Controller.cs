using API.BL.Operations;
using API.Models;
using API.Models.DTO;
using API.Filters;
using System.Web.Http;
using API.Models.Enum;
using System;
using API.Helpers;

namespace API.Controllers
{
    [RoutePrefix("api/Records")]
    public class CLBRW01Controller : ApiController
    {
        private Response _objResponse;
        private BLBRW01 _objBLBRW01;

        public CLBRW01Controller()
        {
            _objResponse = new Response();
            _objBLBRW01 = new BLBRW01();
        }

        [HttpGet]
        [Route("GetAllRecords")]
        [JWTAuthorizationFilter("Admin")]
        public IHttpActionResult GetAllRecords()
        {
            _objResponse = _objBLBRW01.GetAll();

            return Ok(_objResponse);
        }

        [HttpGet]
        [Route("GetRecordById")]
        [JWTAuthorizationFilter("Admin")]
        public IHttpActionResult GetRecordById(int id)
        {
            _objResponse = _objBLBRW01.GetById(id);
            return Ok(_objResponse);
        }

        [HttpGet]
        [Route("GetRecordsByUser")]
        [JWTAuthorizationFilter]
        public IHttpActionResult GetRecordsByUser()
        {
            string token = GetTokenFromRequest();
            int userID = JWTHelper.GetUserIdFromToken(token);

            _objResponse = _objBLBRW01.GetByUser(userID);

            return Ok(_objResponse);
        }

        [HttpPost]
        [JWTAuthorizationFilter("Admin")]
        [Route("BorrowBook")]
        public IHttpActionResult BorrowBook(DTOBRW01 objDTOBRW01)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _objBLBRW01.Type = EnmType.A;
            _objBLBRW01.PreSave(objDTOBRW01);
            _objResponse = _objBLBRW01.Validation();
            if (!_objResponse.IsError)
            {
                _objBLBRW01.Save();
            }

            return Ok(_objResponse);
        }

        [HttpPost]
        [JWTAuthorizationFilter("Admin")]
        [Route("ReturnBook")]
        public IHttpActionResult ReturnBook(int recordId , DateTime returnDate)
        {
            _objBLBRW01.Type = EnmType.E;
            _objBLBRW01.Id = recordId;
            _objBLBRW01.ReturnDate = returnDate;

            _objResponse = _objBLBRW01.Validation();
            if (!_objResponse.IsError)
            {
                _objBLBRW01.Save();
            }

            return Ok(_objResponse);
        }

        [HttpGet]
        [Route("BackupRecords")]
        public IHttpActionResult BackupRecords()
        {
            _objResponse = _objBLBRW01.Backup();

            return Ok(_objResponse);
        }


        private string GetTokenFromRequest()
        {
            var token = string.Empty;

            // Check if the Authorization header exists
            if (Request.Headers.Authorization != null)
            {
                // The token should be in the format 'Bearer <token>'
                var authorizationHeader = Request.Headers.Authorization.Parameter;
                if (!string.IsNullOrEmpty(authorizationHeader))
                {
                    token = authorizationHeader;
                }
            }

            if (string.IsNullOrEmpty(token))
            {
                throw new UnauthorizedAccessException("Authorization token is missing.");
            }

            return token;
        }
    }
}
