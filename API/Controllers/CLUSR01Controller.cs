using API.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using API.Models;
using API.BL.Operations;

namespace API.Controllers
{
    [RoutePrefix("api/Users")]
    public class CLUSR01Controller : ApiController
    {
        private Response _objResponse;
        private BLUSR01 _objUSR01 = new BLUSR01();



        [HttpGet]
        [Route("GetAllUsers")]
        public IHttpActionResult GetAllUsers()
        {
            _objResponse = _objUSR01.GetAll();

            return Ok(_objResponse);
        }

        [HttpPost]
        [Route("AddUser")]
        public IHttpActionResult AddUser(DTOUSR01 objDTOUSR01)
        {


            return Ok();
        }
    }
}
