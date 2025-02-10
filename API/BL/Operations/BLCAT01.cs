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
    /// Business logic for operations on CAT01 (Category)
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
        /// Check if category exists by ID
        /// </summary>
        /// <param name="id">Category ID</param>
        /// <returns>True if category exists, otherwise false</returns>
        public bool IsExist(int id)
        {
            using (IDbConnection db = _dbFactory.OpenDbConnection())
            {
                return db.Exists<CAT01>(c => c.T01F01 == id);
            }
        }

        /// <summary>
        /// Get all categories
        /// </summary>
        /// <returns>Response containing list of categories</returns>
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
                    _objResponse.Message = "Categories retrieved successfully";
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
        /// Get category by ID
        /// </summary>
        /// <param name="id">Category ID</param>
        /// <returns>Response containing category details</returns>
        public Response GetById(int id)
        {
            try
            {
                if (!IsExist(id))
                {
                    _objResponse.IsError = true;
                    _objResponse.Message = "Category does not exist";
                    _objResponse.Data = null;

                    return _objResponse;
                }
                using (IDbConnection db = _dbFactory.OpenDbConnection())
                {
                    _objResponse.Data = db.SingleById<CAT01>(id);
                    _objResponse.Message = "Category retrieved successfully";
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
        /// Add a new category
        /// </summary>
        /// <param name="objCAT01">Category object</param>
        /// <returns>Response of the add operation</returns>
        public Response Add(CAT01 objCAT01)
        {
            try
            {
                using (IDbConnection db = _dbFactory.OpenDbConnection())
                {
                    objCAT01.T01F01 = (int)db.Insert(objCAT01, selectIdentity: true);
                    _objResponse.Message = $"Category added with Id {objCAT01.T01F01}";
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
        /// Edit an existing category
        /// </summary>
        /// <param name="objCAT01">Category object</param>
        /// <returns>Response of the edit operation</returns>
        public Response Edit(CAT01 objCAT01)
        {
            try
            {
                using (IDbConnection db = _dbFactory.OpenDbConnection())
                {
                    db.Update(objCAT01);
                    _objResponse.Message = $"Category with Id {Id} edited";
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
        /// Prepare category object before save
        /// </summary>
        /// <param name="objDTO">Category DTO object</param>
        public void PreSave(DTOCAT01 objDTO)
        {
            _objCAT01 = objDTO.Convert<CAT01>();

            if (Type == EnmType.E)
            {
                _objCAT01.T01F01 = Id;
            }
        }

        /// <summary>
        /// Validate category data before save
        /// </summary>
        /// <returns>Response of the validation</returns>
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
        /// Save category data
        /// </summary>
        /// <returns>Response of the save operation</returns>
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
        /// Delete a category
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
                        db.DeleteById<CAT01>(Id);
                        _objResponse.Message = $"Category with Id {Id} deleted";
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