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
using API.Extensions;
using System.Data;
using System.Collections.Generic;

namespace API.BL.Operations
{
    /// <summary>
    /// Business logic for operations on USR01 (User)
    /// </summary>
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
            _dbFactory = HttpContext.Current.Application["DbFactory"] as IDbConnectionFactory;

            if (_dbFactory == null)
            {
                throw new Exception("IDbConnectionFactory not found");
            }
        }

        /// <summary>
        /// Check if user exists by ID
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns>True if user exists, otherwise false</returns>
        public bool IsExist(int id)
        {
            using (IDbConnection db = _dbFactory.OpenDbConnection())
            {
                return db.Exists<USR01>(u => u.R01F01 == id);
            }
        }

        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns>Response containing list of users</returns>
        public Response GetAll()
        {
            try
            {
                using (IDbConnection db = _dbFactory.OpenDbConnection())
                {
                    List<USR01> result = db.Select<USR01>().ToList();
                    if (result.Count == 0)
                    {
                        _objResponse.IsError = true;
                        _objResponse.Message = "Zero users available";
                        _objResponse.Data = null;

                        return _objResponse;
                    }
                    _objResponse.IsError = false;
                    _objResponse.Data = result;
                    _objResponse.Message = "Users retrieved successfully";
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
        /// Get user profile by ID
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>Response containing user profile</returns>
        public Response GetProfile(int userId)
        {
            try
            {
                using (IDbConnection db = _dbFactory.OpenDbConnection())
                {
                    List<USR01> result = db.Select<USR01>(u => u.R01F01 == userId).ToList();
                    _objResponse.IsError = false;
                    _objResponse.Data = result;
                    _objResponse.Message = "User retrieved successfully";
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
        /// Add a new user
        /// </summary>
        /// <param name="objUSR01">User object</param>
        /// <returns>Response of the add operation</returns>
        public Response Add(USR01 objUSR01)
        {
            try
            {
                using (IDbConnection db = _dbFactory.OpenDbConnection())
                {
                    objUSR01.R01F01 = (int)db.Insert(objUSR01, selectIdentity: true);
                    _objResponse.Message = $"User Added with Id {objUSR01.R01F01}";
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
        /// Edit an existing user
        /// </summary>
        /// <param name="objUSR01">User object</param>
        /// <returns>Response of the edit operation</returns>
        public Response Edit(USR01 objUSR01)
        {
            try
            {
                using (IDbConnection db = _dbFactory.OpenDbConnection())
                {
                    db.Update(objUSR01);
                    _objResponse.Message = $"User with Id {Id} Edited";
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
        /// Prepare user object before save
        /// </summary>
        /// <param name="objDTO">User DTO object</param>
        public void PreSave(DTOUSR01 objDTO)
        {
            _objUSR01 = objDTO.Convert<USR01>();
            _objUSR01.R01F04 = BLEncryption.Encrypt(_objUSR01.R01F04);

            if (Type == EnmType.E)
            {
                _objUSR01.R01F01 = Id;
            }
        }

        /// <summary>
        /// Validate user data before save
        /// </summary>
        /// <returns>Response of the validation</returns>
        public Response Validation()
        {
            if (Type == EnmType.E || Type == EnmType.D)
            {
                if (Id <= 0)
                {
                    _objResponse.IsError = true;
                    _objResponse.Message = "Invalid UserId";
                }
                else if (!IsExist(Id))
                {
                    _objResponse.IsError = true;
                    _objResponse.Message = "User not found";
                }
            }

            return _objResponse;
        }

        /// <summary>
        /// Save user data
        /// </summary>
        /// <returns>Response of the save operation</returns>
        public Response Save()
        {
            if (Type == EnmType.A)
            {
                return Add(_objUSR01);
            }
            else if (Type == EnmType.E)
            {
                return Edit(_objUSR01);
            }

            return _objResponse;
        }

        /// <summary>
        /// Delete a user
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
                        int adminCount = (int)db.Count<USR01>(u => u.R01F05 == EnmRole.Admin);
                        _objUSR01.R01F05 = db.Single<USR01>(u => u.R01F01 == Id).R01F05;

                        if (adminCount <= 1 && _objUSR01.R01F05 == EnmRole.Admin)
                        {
                            _objResponse.IsError = true;
                            _objResponse.Message = "Minimum one admin needed";
                            return _objResponse;
                        }

                        db.DeleteById<USR01>(Id);
                        _objResponse.Message = $"User with Id {Id} Deleted";
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

        /// <summary>
        /// Get user by authentication details
        /// </summary>
        /// <param name="objAuth">Authentication details</param>
        /// <returns>User object</returns>
        public USR01 GetUser(DTOUSR01Auth objAuth)
        {
            string encryptedR01F04 = BLEncryption.Encrypt(objAuth.R01F04);
            using (IDbConnection db = _dbFactory.OpenDbConnection())
            {
                return db.Single<USR01>(
                    u => u.R01F02 == objAuth.R01F02
                    && u.R01F04 == encryptedR01F04);
            }
        }

        /// <summary>
        /// Decrease user's borrow limit by one
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns>Response of the operation</returns>
        public Response DecreaseOne(int id)
        {
            using (IDbConnection db = _dbFactory.OpenDbConnection())
            {
                _objUSR01 = db.SingleById<USR01>(id);

                if (_objUSR01.R01F06 <= 0)
                {
                    _objResponse.IsError = true;
                    _objResponse.Message = "Borrow Limit is over";
                }
                else
                {
                    _objResponse.IsError = false;
                    _objResponse.Message = _objUSR01.R01F06 == 1
                        ? "Borrow Limit is about to be over"
                        : $"{_objUSR01.R01F06} borrow limit available";
                    db.UpdateAdd(() => new USR01 { R01F06 = -1 }, where: u => u.R01F01 == id);
                }
            }

            return _objResponse;
        }

        /// <summary>
        /// Increase user's borrow limit by one
        /// </summary>
        /// <param name="id">User ID</param>
        public void IncreaseOne(int id)
        {
            using (IDbConnection db = _dbFactory.OpenDbConnection())
            {
                db.UpdateAdd(() => new USR01 { R01F06 = 1 }, where: u => u.R01F01 == id);
            }
        }
    }
}