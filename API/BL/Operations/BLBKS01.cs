using API.BL.Interface;
using API.Extensions;
using API.Models;
using API.Models.DTO;
using API.Models.Enum;
using API.Models.POCO;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace API.BL.Operations
{
    /// <summary>
    /// Business logic for operations on BKS01 (Books)
    /// </summary>
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

        /// <summary>
        /// Check if book exists by ID
        /// </summary>
        /// <param name="id">Book ID</param>
        /// <returns>True if book exists, otherwise false</returns>
        public bool IsExist(int id)
        {
            using (IDbConnection db = _dbFactory.OpenDbConnection())
            {
                return db.Exists<BKS01>(c => c.S01F01 == id);
            }
        }

        /// <summary>
        /// Get all books
        /// </summary>
        /// <returns>Response containing list of books</returns>
        public Response GetAll()
        {
            try
            {
                using (IDbConnection db = _dbFactory.OpenDbConnection())
                {
                    List<BKS01> result = db.Select<BKS01>().ToList();
                    if (result.Count == 0)
                    {
                        _objResponse.IsError = true;
                        _objResponse.Message = "Zero books available";
                        _objResponse.Data = null;

                        return _objResponse;
                    }
                    _objResponse.IsError = false;
                    _objResponse.Data = result;
                    _objResponse.Message = "Books retrieved successfully";
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
        /// Get book by ID
        /// </summary>
        /// <param name="id">Book ID</param>
        /// <returns>Response containing book details</returns>
        public Response GetById(int id)
        {
            try
            {
                if (!IsExist(id))
                {
                    _objResponse.IsError = true;
                    _objResponse.Message = "Book does not exist";
                    _objResponse.Data = null;

                    return _objResponse;
                }
                using (IDbConnection db = _dbFactory.OpenDbConnection())
                {
                    _objResponse.Data = db.SingleById<BKS01>(id);
                    _objResponse.Message = "Book retrieved successfully";
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
        /// Add a new book
        /// </summary>
        /// <param name="objBKS01">Book object</param>
        /// <returns>Response of the add operation</returns>
        public Response Add(BKS01 objBKS01)
        {
            try
            {
                using (IDbConnection db = _dbFactory.OpenDbConnection())
                {
                    objBKS01.S01F01 = (int)db.Insert(objBKS01, selectIdentity: true);
                    _objResponse.Message = $"Book added with Id {objBKS01.S01F01}";
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
        /// Edit an existing book
        /// </summary>
        /// <param name="objBKS01">Book object</param>
        /// <returns>Response of the edit operation</returns>
        public Response Edit(BKS01 objBKS01)
        {
            try
            {
                using (IDbConnection db = _dbFactory.OpenDbConnection())
                {
                    db.Update(objBKS01);
                    _objResponse.Message = $"Book with Id {Id} edited";
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
        /// Prepare book object before save
        /// </summary>
        /// <param name="objDTO">Book DTO object</param>
        public void PreSave(DTOBKS01 objDTO)
        {
            _objBKS01 = objDTO.Convert<BKS01>();

            if (Type == EnmType.E)
            {
                _objBKS01.S01F01 = Id;
            }
        }

        /// <summary>
        /// Validate book data before save
        /// </summary>
        /// <returns>Response of the validation</returns>
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

        /// <summary>
        /// Save book data
        /// </summary>
        /// <returns>Response of the save operation</returns>
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

        /// <summary>
        /// Delete a book
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
                        db.DeleteById<BKS01>(Id);
                        _objResponse.Message = $"Book with Id {Id} deleted";
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
        /// Decrease the number of available copies of a book by one
        /// </summary>
        /// <param name="id">Book ID</param>
        /// <returns>Response of the operation</returns>
        public Response DecreaseOne(int id)
        {
            using (IDbConnection db = _dbFactory.OpenDbConnection())
            {
                _objBKS01 = db.SingleById<BKS01>(id);

                if (_objBKS01.S01F06 <= 0)
                {
                    _objResponse.IsError = true;
                    _objResponse.Message = "There are no books available";
                }
                else
                {
                    _objResponse.IsError = false;
                    _objResponse.Message = _objBKS01.S01F06 == 1
                        ? "Last copy of the book"
                        : $"{_objBKS01.S01F06} books are available";
                    db.UpdateAdd(() => new BKS01 { S01F06 = -1 }, where: b => b.S01F01 == id);
                }
            }

            return _objResponse;
        }

        /// <summary>
        /// Increase the number of available copies of a book by one
        /// </summary>
        /// <param name="id">Book ID</param>
        public void IncreaseOne(int id)
        {
            using (IDbConnection db = _dbFactory.OpenDbConnection())
            {
                db.UpdateAdd(() => new BKS01 { S01F06 = 1 }, where: b => b.S01F01 == id);
            }
        }
    }
}