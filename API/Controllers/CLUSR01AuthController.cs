using API.BL.Operations;
using API.Helpers;
using API.Models;
using API.Models.DTO;
using API.Models.POCO;
using System.Web.Http;
using API.Filters;

namespace API.Controllers
{
    /// <summary>
    /// Controller to handle Authentication 
    /// </summary>
    [RoutePrefix("api/Auth")]
    [AllowAnonymous]
    [ValidateModelState]
    public class CLUSR01AuthController : ApiController
    {
        private Response _objResponse;
        private BLUSR01 _objBLUSR01;
        private USR01 _objUSR01;

        public CLUSR01AuthController()
        {
            _objBLUSR01 = new BLUSR01();
            _objResponse = new Response();
        }

        /// <summary>
        /// Generate JWT token for user authentication
        /// </summary>
        /// <param name="objAuth">Authentication data</param>
        /// <returns>JWT token if authentication is successful</returns>
        [HttpPost]
        [Route("GetToken")]
        public IHttpActionResult GetToken(DTOUSR01Auth objAuth)
        {
            //if(!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            _objUSR01 = _objBLUSR01.GetUser(objAuth);

            if(_objUSR01 != null)
            {
                _objResponse.Data = JWTHelper.GenerateJwtToken(
                    _objUSR01.R01F02, _objUSR01.R01F01, _objUSR01.R01F05.ToString());
                _objResponse.Message = "Authentication Successful";
            }
            else
            {
                _objResponse.IsError = true;
                _objResponse.Message = "Credentials are invalid";
            }

            return Ok(_objResponse);
        }
    }
}