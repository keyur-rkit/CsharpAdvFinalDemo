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
using System.Data;
using System.Collections.Generic;

namespace API.BL.Operations
{
    /// <summary>
    /// 
    /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsExist(int id)
        {
            using (IDbConnection db = _dbFactory.OpenDbConnection())
            {
                return db.Exists<CAT01>(c => c.T01F01 == id);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Response GetAll()
        {
            try
            {
                using (IDbConnection db = _dbFactory.OpenDbConnection())
                {
                    List<CAT01> result = db.Select<CAT01>().ToList();
                    if (result.Count == 0)
                    {
                        _objResponse.IsError = true;
                        _objResponse.Message = "Zero categories available";
                        _objResponse.Data = null;

                        return _objResponse;
                    }
                    _objResponse.IsError = false;
                    _objResponse.Data = result;
                    _objResponse.Message = "Categories get successfully";
                }
            }
            catch (Exception ex)
            {
                _objResponse.IsError = true;
                _objResponse.Message = ex.Message;
            }
            return _objResponse;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
                using (IDbConnection db = _dbFactory.OpenDbConnection())
                {
                    _objResponse.Data = db.SingleById<CAT01>(id);
                    _objResponse.Message = "Category get successfully";
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objCAT01"></param>
        /// <returns></returns>
        public Response Add(CAT01 objCAT01)
        {
            try
            {
                using (IDbConnection db = _dbFactory.OpenDbConnection())
                {
                    objCAT01.T01F01 = (int)db.Insert(objCAT01, selectIdentity: true);
                    _objResponse.Message = $"Category Added with Id {objCAT01.T01F01}";
                    //db.Insert(objCAT01);
                    //_objResponse.Message = "Category Added";
                }
            }
            catch (Exception ex)
            {
                _objResponse.IsError = true;
                _objResponse.Message = ex.Message;
            }
            return _objResponse;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objCAT01"></param>
        /// <returns></returns>
        public Response Edit(CAT01 objCAT01)
        {
            try
            {
                using (IDbConnection db = _dbFactory.OpenDbConnection())
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objDTO"></param>
        public void PreSave(DTOCAT01 objDTO)
        {
            _objCAT01 = objDTO.Convert<CAT01>();

            if (Type == EnmType.E)
            {
                _objCAT01.T01F01 = Id;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Response Delete()
        {
            try
            {
                using (IDbConnection db = _dbFactory.OpenDbConnection())
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