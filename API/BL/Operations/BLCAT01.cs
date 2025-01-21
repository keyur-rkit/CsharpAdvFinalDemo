using API.BL.Interface;
using API.Models;
using API.Models.DTO;
using API.Models.Enum;
using API.Models.POCO;
using ServiceStack.Data;
using System;
using System.Linq;
using System.Web;
using ServiceStack.OrmLite;
using API.Extensions;

namespace API.BL.Operations
{
    public class BLCAT01 : IDataHandler<DTOCAT01>
    {
        private CAT01 _objCAT01;
        private Response _objResponse;
        private readonly IDbConnectionFactory _dbFactory;

        public EnmType Type { get; set; }
        public int Id { get; set; }

        public BLCAT01()
        {
            _objResponse = new Response();

            _dbFactory = HttpContext.Current.Application["DbFactory"] as IDbConnectionFactory;

            if (_dbFactory == null)
            {
                throw new Exception("IDbConnectionFactory not found");
            }
        }

        public bool IsExist(int id)
        {
            using (var db = _dbFactory.OpenDbConnection())
            {
                return db.Exists<CAT01>(c => c.T01F01 == id);
            }
        }

        public Response GetAll()
        {
            try
            {
                using (var db = _dbFactory.OpenDbConnection())
                {
                    var result = db.Select<CAT01>().ToList();
                    if (result.Count == 0)
                    {
                        _objResponse.IsError = true;
                        _objResponse.Message = "Zero categories available";
                        _objResponse.Data = null;

                        return _objResponse;
                    }
                    _objResponse.IsError = false;
                    _objResponse.Data = result;
                }
            }
            catch (Exception ex)
            {
                _objResponse.IsError = true;
                _objResponse.Message = ex.Message;
            }
            return _objResponse;
        }

        public Response GetById(int id)
        {
            try
            {
                if (!IsExist(id))
                {
                    _objResponse.IsError = true;
                    _objResponse.Message = "Category dose not Exist";
                    _objResponse.Data = null;

                    return _objResponse;
                }
                using (var db = _dbFactory.OpenDbConnection())
                {
                    _objResponse.Data = db.SingleById<CAT01>(id);
                    return _objResponse;
                }
            }
            catch (Exception ex)
            {
                _objResponse.IsError = true;
                _objResponse.Message = ex.Message;
            }
            return _objResponse;
        }

        public Response Add(CAT01 objCAT01)
        {
            try
            {
                using (var db = _dbFactory.OpenDbConnection())
                {
                    db.Insert(objCAT01);
                    _objResponse.Message = "Category Added";
                }
            }
            catch (Exception ex)
            {
                _objResponse.IsError = true;
                _objResponse.Message = ex.Message;
            }
            return _objResponse;
        }

        public Response Edit(CAT01 objCAT01)
        {
            try
            {
                using (var db = _dbFactory.OpenDbConnection())
                {
                    db.Update(objCAT01);
                    _objResponse.Message = $"Category with Id {Id} Edited";
                }
            }
            catch (Exception ex)
            {
                _objResponse.IsError = true;
                _objResponse.Message = ex.Message;
            }
            return _objResponse;
        }

        public void PreSave(DTOCAT01 objDTO)
        {
            _objCAT01 = objDTO.Convert<CAT01>();

            if (Type == EnmType.E)
            {
                _objCAT01.T01F01 = Id;
            }
        }

        public Response Validation()
        {
            if (Type == EnmType.E || Type == EnmType.D)
            {
                if (Id <= 0)
                {
                    _objResponse.IsError = true;
                    _objResponse.Message = "Invalid CategoryId";
                }
                else if (!IsExist(Id))
                {
                    _objResponse.IsError = true;
                    _objResponse.Message = "Category not found";
                }
            }

            return _objResponse;
        }

        public Response Save()
        {

            if (Type == EnmType.A)
            {
                return Add(_objCAT01);
            }
            else if (Type == EnmType.E)
            {
                return Edit(_objCAT01);
            }

            return _objResponse;
        }

        public Response Delete()
        {
            try
            {
                using (var db = _dbFactory.OpenDbConnection())
                {
                    if (Type == EnmType.D)
                    {
                        db.DeleteById<CAT01>(Id);
                        _objResponse.Message = $"Category with Id {Id} Deleted";
                    }
                }
            }
            catch (Exception ex)
            {
                _objResponse.IsError = true;
                _objResponse.Message = ex.Message;
            }
            return _objResponse;
        }
    }
}