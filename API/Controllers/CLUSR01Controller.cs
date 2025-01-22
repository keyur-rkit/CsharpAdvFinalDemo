using API.Models.DTO;
using System.Web.Http;
using API.Models;
using API.Models.Enum;
using API.BL.Operations;
using API.Filters;
using System;
using API.Helpers;

namespace API.Controllers
{
    [RoutePrefix("api/Users")]
    public class CLUSR01Controller : ApiController
    {
        private Response _objResponse;
        private BLUSR01 _objBLUSR01;

        public CLUSR01Controller()
        {
            _objBLUSR01 = new BLUSR01();
            _objResponse = new Response();
        }


        [HttpGet]
        [Route("GetAllUsers")]
        [JWTAuthorizationFilter("Admin")]
        public IHttpActionResult GetAllUsers()
        {
            _objResponse = _objBLUSR01.GetAll();

            return Ok(_objResponse);
        }

        [HttpGet]
        [Route("GetUserProfile")]
        [JWTAuthorizationFilter]
        public IHttpActionResult GetUserProfile() 
        {
            string token = GetTokenFromRequest();
            int userID = JWTHelper.GetUserIdFromToken(token);

            _objResponse = _objBLUSR01.GetProfile(userID);

            return Ok(_objResponse);
        }

        [HttpPost]
        [Route("AddUser")]
        [JWTAuthorizationFilter("Admin")]
        public IHttpActionResult AddUser(DTOUSR01 objDTOUSR01)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _objBLUSR01.Type = EnmType.A;
            _objBLUSR01.PreSave(objDTOUSR01);
            _objResponse = _objBLUSR01.Validation();

            if (!_objResponse.IsError)
            {
                _objBLUSR01.Save();
            }

            return Ok(_objResponse);
        }

        [HttpPut]
        [Route("EditUser")]
        [JWTAuthorizationFilter("Admin")]
        public IHttpActionResult EditUser(int id,DTOUSR01 objDTOUSR01)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _objBLUSR01.Type = EnmType.E;
            _objBLUSR01.Id = id;
            _objBLUSR01.PreSave(objDTOUSR01);
            _objResponse = _objBLUSR01.Validation();

            if(!_objResponse.IsError)
            {
                _objBLUSR01.Save();
            }

            return Ok(_objResponse);
        }

        [HttpDelete]
        [Route("DeleteUser")]
        [JWTAuthorizationFilter("Admin")]
        public IHttpActionResult DeleteUser(int id)
        {
            _objBLUSR01.Type = EnmType.D;
            _objBLUSR01.Id = id;
            _objResponse = _objBLUSR01.Validation();
            if(!_objResponse.IsError)
            {
                _objResponse = _objBLUSR01.Delete();
            }
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
