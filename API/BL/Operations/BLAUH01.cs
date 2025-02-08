using API.BL.Interface;
using API.Models.DTO;
using API.Models;
using API.Models.Enum;
using API.Models.POCO;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using System;
using System.Linq;
using System.Web;
using API.Extensions;
using System.Data;
using System.Collections.Generic;

namespace API.BL.Operations
{
    /// <summary>
    /// 
    /// </summary>
    public class BLAUH01 : IDataHandler<DTOAUH01>
    {
        private AUH01 _objAUH01;
        private Response _objResponse;
        private readonly IDbConnectionFactory _dbFactory;

        public EnmType Type { get; set; }
        public int Id { get; set; }

        public BLAUH01()
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
                return db.Exists<AUH01>(c => c.H01F01 == id);
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
                    List<AUH01> result = db.Select<AUH01>().ToList();
                    if (result.Count == 0)
                    {
                        _objResponse.IsError = true;
                        _objResponse.Message = "Zero authors available";
                        _objResponse.Data = null;

                        return _objResponse;
                    }
                    _objResponse.IsError = false;
                    _objResponse.Data = result;
                    _objResponse.Message = "Authors get successfully";
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
                    _objResponse.Message = "Author dose not Exist";
                    _objResponse.Data = null;

                    return _objResponse;
                }
                using (IDbConnection db = _dbFactory.OpenDbConnection())
                {
                    _objResponse.Data = db.SingleById<AUH01>(id);
                    _objResponse.Message = "Author get successfully";
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
        /// <param name="objAUH01"></param>
        /// <returns></returns>
        public Response Add(AUH01 objAUH01)
        {
            try
            {
                using (IDbConnection db = _dbFactory.OpenDbConnection())
                {
                    objAUH01.H01F01 = (int)db.Insert(objAUH01, selectIdentity: true);
                    _objResponse.Message = $"Auhtor Added with Id {objAUH01.H01F01}";
                    //db.Insert(objAUH01);
                    //_objResponse.Message = "Auhtor Added";
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
        /// <param name="objAUH01"></param>
        /// <returns></returns>
        public Response Edit(AUH01 objAUH01)
        {
            try
            {
                using (IDbConnection db = _dbFactory.OpenDbConnection())
                {
                    db.Update(objAUH01);
                    _objResponse.Message = $"Auhtor with Id {Id} Edited";
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
        public void PreSave(DTOAUH01 objDTO)
        {
            _objAUH01 = objDTO.Convert<AUH01>();

            if (Type == EnmType.E)
            {
                _objAUH01.H01F01 = Id;
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
                    _objResponse.Message = "Invalid AuthorId";
                }
                else if (!IsExist(Id))
                {
                    _objResponse.IsError = true;
                    _objResponse.Message = "Author not found";
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
                return Add(_objAUH01);
            }
            else if (Type == EnmType.E)
            {
                return Edit(_objAUH01);
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
                        db.DeleteById<AUH01>(Id);
                        _objResponse.Message = $"Author with Id {Id} Deleted";
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