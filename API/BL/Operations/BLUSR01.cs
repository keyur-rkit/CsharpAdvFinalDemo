using API.BL.Interface;
using API.Models;
using API.Models.DTO;
using API.Models.Enum;
using API.Models.POCO;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using System;
using System.Web;
using System.Linq;

namespace API.BL.Operations
{
    public class BLUSR01 : IDataHandler<DTOUSR01>
    {
        private USR01 _objUSR01;
        private Response _objResponse;
        private readonly IDbConnectionFactory _dbFactory;


        public EnmType Type { get; set; }
        public int Id { get; set; }


        public BLUSR01()
        {
            _objResponse = new Response();

            _dbFactory= HttpContext.Current.Application["DbFactory"] as IDbConnectionFactory;
            
            if (_dbFactory == null)
            {
                throw new Exception("IDbConnectionFactory not found");
            }
        }

        public bool IsExist(string username,string email)
        {
            using (var db = _dbFactory.OpenDbConnection())
            {
                return db.Exists<USR01>(u => u.R01F02 == username || u.R01F03 == email);
            }
        }

        public Response GetAll()
        {
            using (var db = _dbFactory.OpenDbConnection())
            {
                var result = db.Select<USR01>().ToList();
                if(result.Count == 0)
                {
                    _objResponse.IsError = true;
                    _objResponse.Message = "Zero users available";
                    _objResponse.Data = null;

                    return _objResponse;
                }
                _objResponse.IsError = false;
                _objResponse.Data = result;
                return _objResponse;
            }
        }

        public void PreSave(DTOUSR01 objDTO)
        {
            throw new NotImplementedException();
        }

        public Response Save()
        {
            throw new NotImplementedException();
        }

        public Response Validation()
        {
            throw new NotImplementedException();
        }
    }
}