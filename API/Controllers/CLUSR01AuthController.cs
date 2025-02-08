using API.BL.Operations;
using API.Helpers;
using API.Models;
using API.Models.DTO;
using API.Models.POCO;
using System.Web.Http;

namespace API.Controllers
{
    /// <summary>
    /// Controller to handle Authentication 
    /// </summary>
    [RoutePrefix("api/Auth")]
    [AllowAnonymous]
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

        [HttpPost]
        [Route("GetToken")]
        public IHttpActionResult GetToken(DTOUSR01Auth objAuth)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _objUSR01 = _objBLUSR01.GetUser(objAuth);

            if(_objUSR01 != null)
            {
                _objResponse.Data = JWTHelper.GenerateJwtToken(
                    _objUSR01.R01F02,_objUSR01.R01F01,_objUSR01.R01F05.ToString());
                _objResponse.Message = "Authentication Successfull";
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
