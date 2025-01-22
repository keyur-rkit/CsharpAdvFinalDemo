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

        public bool IsExist(int id)
        {
            using (var db = _dbFactory.OpenDbConnection())
            {
                return db.Exists<USR01>(u => u.R01F01 == id);
            }
        }

        public Response GetAll()
        {
            try
            {
                using (var db = _dbFactory.OpenDbConnection())
                {
                    var result = db.Select<USR01>().ToList();
                    if (result.Count == 0)
                    {
                        _objResponse.IsError = true;
                        _objResponse.Message = "Zero users available";
                        _objResponse.Data = null;

                        return _objResponse;
                    }
                    _objResponse.IsError = false;
                    _objResponse.Data = result;
                    _objResponse.Message = "Users get successfully";
                }
            }
            catch(Exception ex)
            {
                _objResponse.IsError = true;
                _objResponse.Message = ex.Message;
            }
            return _objResponse;
        }

        public Response GetProfile(int userId)
        {
            try
            {
                using (var db = _dbFactory.OpenDbConnection())
                {
                    var result = db.Select<USR01>(u => u.R01F01 == userId).ToList(); 
                    _objResponse.IsError = false;
                    _objResponse.Data = result;
                    _objResponse.Message = "User get successfully";
                }
            }
            catch (Exception ex)
            {
                _objResponse.IsError = true;
                _objResponse.Message = ex.Message;
            }
            return _objResponse;
        }

        public Response Add(USR01 objUSR01)
        {
            try
            {
                using (var db = _dbFactory.OpenDbConnection())
                {
                    db.Insert(objUSR01);
                    _objResponse.Message = "User Added";
                }
            }
            catch (Exception ex)
            {
                _objResponse.IsError = true;
                _objResponse.Message = ex.Message;
            }
            return _objResponse;
        }

        public Response Edit(USR01 objUSR01)
        {
            try
            {
                using (var db = _dbFactory.OpenDbConnection())
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

        public void PreSave(DTOUSR01 objDTO)
        {
            _objUSR01 = objDTO.Convert<USR01>();
            _objUSR01.R01F04 = BLEncryption.Encrypt(_objUSR01.R01F04);

            if(Type == EnmType.E)
            {
                _objUSR01.R01F01 = Id;
            }
        }

        public Response Validation()
        {
            if(Type == EnmType.E || Type == EnmType.D)
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

        public Response Save()
        {

            if(Type == EnmType.A)
            {
                return Add(_objUSR01);
            }
            else if(Type == EnmType.E)
            {
                return Edit(_objUSR01);
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
                        int adminCount = (int)db.Count<USR01>(u => u.R01F05 == EnmRole.Admin);
                        _objUSR01.R01F05 = db.Single<USR01>(u=> u.R01F01 == Id).R01F05;

                        if (adminCount <= 1 &&  _objUSR01.R01F05 == EnmRole.Admin) 
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

        public USR01 GetUser(DTOUSR01Auth objAuth)
        {
            string encryptedR01F04 = BLEncryption.Encrypt(objAuth.R01F04);
            using (var db = _dbFactory.OpenDbConnection())
            {
                return db.Single<USR01>(
                    u => u.R01F02 == objAuth.R01F02 
                    && u.R01F04 == encryptedR01F04);
            }
        }

        public Response DecreaseOne(int id)
        {
            using (var db = _dbFactory.OpenDbConnection())
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
                        ? "Borrow Limit is about to over"
                        : $"{_objUSR01.R01F06} borrow limit available";
                    db.UpdateAdd(() => new USR01 { R01F06 = -1 }, where: u => u.R01F01 == id);
                }
            }

            return _objResponse;
        }

        public void IncreaseOne(int id)
        {
            using (var db = _dbFactory.OpenDbConnection())
            {
                db.UpdateAdd(() => new USR01 { R01F06 = 1 }, where: u => u.R01F01 == id);
            }
        }

    }
}