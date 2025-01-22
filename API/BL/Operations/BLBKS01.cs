using API.BL.Interface;
using API.Extensions;
using API.Models;
using API.Models.DTO;
using API.Models.Enum;
using API.Models.POCO;
using Google.Protobuf.Compiler;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.BL.Operations
{
    public class BLBKS01 : IDataHandler<DTOBKS01>
    {
        private BKS01 _objBKS01;
        private Response _objResponse;
        private readonly IDbConnectionFactory _dbFactory;

        public EnmType Type { get; set; }
        public int Id { get; set; }

        public BLBKS01()
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
                return db.Exists<BKS01>(c => c.S01F01 == id);
            }
        }

        public Response GetAll()
        {
            try
            {
                using (var db = _dbFactory.OpenDbConnection())
                {
                    var result = db.Select<BKS01>().ToList();
                    if (result.Count == 0)
                    {
                        _objResponse.IsError = true;
                        _objResponse.Message = "Zero books available";
                        _objResponse.Data = null;

                        return _objResponse;
                    }
                    _objResponse.IsError = false;
                    _objResponse.Data = result;
                    _objResponse.Message = "Books get successfully";
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
                    _objResponse.Message = "Book dose not Exist";
                    _objResponse.Data = null;

                    return _objResponse;
                }
                using (var db = _dbFactory.OpenDbConnection())
                {
                    _objResponse.Data = db.SingleById<BKS01>(id);
                    _objResponse.Message = "Book get successfully";
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

        public Response Add(BKS01 objBKS01)
        {
            try
            {
                using (var db = _dbFactory.OpenDbConnection())
                {
                    db.Insert(objBKS01);
                    _objResponse.Message = "Book Added";
                }
            }
            catch (Exception ex)
            {
                _objResponse.IsError = true;
                _objResponse.Message = ex.Message;
            }
            return _objResponse;
        }

        public Response Edit(BKS01 objBKS01)
        {
            try
            {
                using (var db = _dbFactory.OpenDbConnection())
                {
                    db.Update(objBKS01);
                    _objResponse.Message = $"Book with Id {Id} Edited";
                }
            }
            catch (Exception ex)
            {
                _objResponse.IsError = true;
                _objResponse.Message = ex.Message;
            }
            return _objResponse;
        }

        public void PreSave(DTOBKS01 objDTO)
        {
            _objBKS01 = objDTO.Convert<BKS01>();

            if (Type == EnmType.E)
            {
                _objBKS01.S01F01 = Id;
            }
        }

        public Response Validation()
        {
            if (Type == EnmType.E || Type == EnmType.D)
            {
                if (Id <= 0)
                {
                    _objResponse.IsError = true;
                    _objResponse.Message = "Invalid BookId";
                }
                else if (!IsExist(Id))
                {
                    _objResponse.IsError = true;
                    _objResponse.Message = "Book not found";
                }
            }

            return _objResponse;
        }

        public Response Save()
        {

            if (Type == EnmType.A)
            {
                return Add(_objBKS01);
            }
            else if (Type == EnmType.E)
            {
                return Edit(_objBKS01);
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
                        db.DeleteById<BKS01>(Id);
                        _objResponse.Message = $"Book with Id {Id} Deleted";
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

        public Response DecreaseOne(int id)
        {
            using (var db = _dbFactory.OpenDbConnection())
            {
                _objBKS01 = db.SingleById<BKS01>(id);

                if (_objBKS01.S01F06 <= 0)
                {
                    _objResponse.IsError = true;
                    _objResponse.Message = "There is no book available";
                }
                else
                {
                    _objResponse.IsError = false;
                    _objResponse.Message = _objBKS01.S01F06 == 1 
                        ? "Last copy of book" 
                        : $"{_objBKS01.S01F06} books are available";
                    db.UpdateAdd(() => new BKS01 { S01F06 = -1 }, where: b => b.S01F01 == id);
                }
            }

            return _objResponse;
        }

        public void IncreaseOne(int id)
        {
            using (var db = _dbFactory.OpenDbConnection())
            {
                db.UpdateAdd(() => new BKS01 { S01F06 = 1 }, where: b => b.S01F01 == id);
            }
        }
    }
}