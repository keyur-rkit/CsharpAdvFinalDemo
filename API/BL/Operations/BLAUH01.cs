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
    /// Business logic for operations on AUH01 (Authors)
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
        /// Check if author exists by ID
        /// </summary>
        /// <param name="id">Author ID</param>
        /// <returns>True if author exists, otherwise false</returns>
        public bool IsExist(int id)
        {
            using (IDbConnection db = _dbFactory.OpenDbConnection())
            {
                return db.Exists<AUH01>(c => c.H01F01 == id);
            }
        }

        /// <summary>
        /// Get all authors
        /// </summary>
        /// <returns>Response containing list of authors</returns>
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
                    _objResponse.Message = "Authors retrieved successfully";
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
        /// Get author by ID
        /// </summary>
        /// <param name="id">Author ID</param>
        /// <returns>Response containing author details</returns>
        public Response GetById(int id)
        {
            try
            {
                if (!IsExist(id))
                {
                    _objResponse.IsError = true;
                    _objResponse.Message = "Author does not exist";
                    _objResponse.Data = null;

                    return _objResponse;
                }
                using (IDbConnection db = _dbFactory.OpenDbConnection())
                {
                    _objResponse.Data = db.SingleById<AUH01>(id);
                    _objResponse.Message = "Author retrieved successfully";
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
        /// Add a new author
        /// </summary>
        /// <param name="objAUH01">Author object</param>
        /// <returns>Response of the add operation</returns>
        public Response Add(AUH01 objAUH01)
        {
            try
            {
                using (IDbConnection db = _dbFactory.OpenDbConnection())
                {
                    objAUH01.H01F01 = (int)db.Insert(objAUH01, selectIdentity: true);
                    _objResponse.Message = $"Author added with Id {objAUH01.H01F01}";
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
        /// Edit an existing author
        /// </summary>
        /// <param name="objAUH01">Author object</param>
        /// <returns>Response of the edit operation</returns>
        public Response Edit(AUH01 objAUH01)
        {
            try
            {
                using (IDbConnection db = _dbFactory.OpenDbConnection())
                {
                    db.Update(objAUH01);
                    _objResponse.Message = $"Author with Id {Id} edited";
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
        /// Prepare author object before save
        /// </summary>
        /// <param name="objDTO">Author DTO object</param>
        public void PreSave(DTOAUH01 objDTO)
        {
            _objAUH01 = objDTO.Convert<AUH01>();

            if (Type == EnmType.E)
            {
                _objAUH01.H01F01 = Id;
            }
        }

        /// <summary>
        /// Validate author data before save
        /// </summary>
        /// <returns>Response of the validation</returns>
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
        /// Save author data
        /// </summary>
        /// <returns>Response of the save operation</returns>
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
        /// Delete an author
        /// </summary>
        /// <returns>Response of the delete operation</returns>
        public Response Delete()
        {
            try
            {
                using (IDbConnection db = _dbFactory.OpenDbConnection())
                {
                    if (Type == EnmType.D)
                    {
                        db.DeleteById<AUH01>(Id);
                        _objResponse.Message = $"Author with Id {Id} deleted";
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