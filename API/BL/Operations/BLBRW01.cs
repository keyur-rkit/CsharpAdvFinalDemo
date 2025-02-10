using API.BL.Interface;
using API.Extensions;
using API.Models;
using API.Models.DTO;
using API.Models.Enum;
using API.Models.POCO;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace API.BL.Operations
{
    /// <summary>
    /// Business logic for operations on BRW01 (Borrow Records)
    /// </summary>
    public class BLBRW01 : IDataHandler<DTOBRW01>
    {
        private BRW01 _objBRW01;
        private BLBKS01 _objBLBKS01;
        private BLUSR01 _objBLUSR01;
        private Response _objResponse;
        private readonly IDbConnectionFactory _dbFactory;

        public EnmType Type { get; set; }
        public int Id { get; set; }
        public DateTime ReturnDate { get; set; }

        public BLBRW01()
        {
            _objResponse = new Response();
            _objBLBKS01 = new BLBKS01();
            _objBLUSR01 = new BLUSR01();

            _dbFactory = HttpContext.Current.Application["DbFactory"] as IDbConnectionFactory;

            if (_dbFactory == null)
            {
                throw new Exception("IDbConnectionFactory not found");
            }
        }

        /// <summary>
        /// Check if record exists by ID
        /// </summary>
        /// <param name="id">Record ID</param>
        /// <returns>True if record exists, otherwise false</returns>
        public bool IsExist(int id)
        {
            using (IDbConnection db = _dbFactory.OpenDbConnection())
            {
                return db.Exists<BRW01>(r => r.W01F01 == id);
            }
        }

        /// <summary>
        /// Get all records
        /// </summary>
        /// <returns>Response containing list of records</returns>
        public Response GetAll()
        {
            try
            {
                using (IDbConnection db = _dbFactory.OpenDbConnection())
                {
                    List<BRW01> result = db.Select<BRW01>().ToList();
                    if (result.Count == 0)
                    {
                        _objResponse.IsError = true;
                        _objResponse.Message = "Zero records available";
                        _objResponse.Data = null;

                        return _objResponse;
                    }
                    _objResponse.IsError = false;
                    _objResponse.Data = result;
                    _objResponse.Message = "Records retrieved successfully";
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
        /// Get record by ID
        /// </summary>
        /// <param name="id">Record ID</param>
        /// <returns>Response containing record details</returns>
        public Response GetById(int id)
        {
            try
            {
                if (!IsExist(id))
                {
                    _objResponse.IsError = true;
                    _objResponse.Message = "Record does not exist";
                    _objResponse.Data = null;

                    return _objResponse;
                }
                using (IDbConnection db = _dbFactory.OpenDbConnection())
                {
                    _objResponse.Data = db.SingleById<BRW01>(id);
                    _objResponse.Message = "Record retrieved successfully";
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
        /// Get records by user ID
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns>Response containing list of user records</returns>
        public Response GetByUser(int id)
        {
            try
            {
                using (IDbConnection db = _dbFactory.OpenDbConnection())
                {
                    List<BRW01> result = db.Select<BRW01>(r => r.W01F02 == id).ToList();
                    if (result.Count == 0)
                    {
                        _objResponse.IsError = true;
                        _objResponse.Message = "Zero records available";
                        _objResponse.Data = null;

                        return _objResponse;
                    }
                    _objResponse.IsError = false;
                    _objResponse.Data = result;
                    _objResponse.Message = "Records retrieved successfully";
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
        /// Borrow a book
        /// </summary>
        /// <param name="objBRW01">Borrow record object</param>
        /// <returns>Response of the borrow operation</returns>
        public Response Borrow(BRW01 objBRW01)
        {
            try
            {
                Response bookResponse = _objBLBKS01.DecreaseOne(objBRW01.W01F03);
                Response userResponse = _objBLUSR01.DecreaseOne(objBRW01.W01F02);

                if (bookResponse.IsError)
                {
                    _objResponse.IsError = true;
                    _objResponse.Message = bookResponse.Message;
                }
                else if (userResponse.IsError)
                {
                    _objResponse.IsError = true;
                    _objResponse.Message = userResponse.Message;
                }
                else
                {
                    using (IDbConnection db = _dbFactory.OpenDbConnection())
                    {
                        db.Insert(_objBRW01);
                    }

                    _objResponse.IsError = false;
                    _objResponse.Message = $"{bookResponse.Message}, {userResponse.Message}, Record added";
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
        /// Return a borrowed book
        /// </summary>
        /// <param name="id">Record ID</param>
        /// <param name="returnDate">Return date</param>
        /// <returns>Response of the return operation</returns>
        public Response Return(int id, DateTime returnDate)
        {
            try
            {
                using (IDbConnection db = _dbFactory.OpenDbConnection())
                {
                    _objBRW01 = db.Single<BRW01>(b => b.W01F01 == id);

                    if (_objBRW01.W01F05 != null)
                    {
                        _objResponse.IsError = true;
                        _objResponse.Message = "Book already returned";
                        return _objResponse;
                    }

                    if (returnDate < _objBRW01.W01F04)
                    {
                        _objResponse.IsError = true;
                        _objResponse.Message = "Return date must be after borrow date";
                        return _objResponse;
                    }

                    _objBRW01.W01F05 = returnDate;
                    db.Update(_objBRW01);
                    _objBLBKS01.IncreaseOne(_objBRW01.W01F03);
                    _objBLUSR01.IncreaseOne(_objBRW01.W01F02);
                    _objResponse.IsError = false;
                    _objResponse.Message = "Book returned successfully";
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
        /// Prepare borrow record object before save
        /// </summary>
        /// <param name="objDTO">Borrow record DTO object</param>
        public void PreSave(DTOBRW01 objDTO)
        {
            _objBRW01 = objDTO.Convert<BRW01>();
        }

        /// <summary>
        /// Validate borrow record data before save
        /// </summary>
        /// <returns>Response of the validation</returns>
        public Response Validation()
        {
            if (Type == EnmType.E)
            {
                if (Id <= 0)
                {
                    _objResponse.IsError = true;
                    _objResponse.Message = "Invalid RecordId";
                }
                else if (!IsExist(Id))
                {
                    _objResponse.IsError = true;
                    _objResponse.Message = "Record not found";
                }
            }

            return _objResponse;
        }

        /// <summary>
        /// Save borrow record data
        /// </summary>
        /// <returns>Response of the save operation</returns>
        public Response Save()
        {
            if (Type == EnmType.A)
            {
                return Borrow(_objBRW01);
            }
            else if (Type == EnmType.E)
            {
                return Return(Id, ReturnDate);
            }

            return _objResponse;
        }

        /// <summary>
        /// Backup all borrow records
        /// </summary>
        /// <returns>Response of the backup operation</returns>
        public Response Backup()
        {
            using (IDbConnection db = _dbFactory.OpenDbConnection())
            {
                List<BRW01> result = db.Select<BRW01>().ToList();
                if (result.Count == 0)
                {
                    _objResponse.IsError = true;
                    _objResponse.Message = "No records available for backup";
                    _objResponse.Data = null;

                    return _objResponse;
                }

                string backup = @"F:\Keyur-417\Code\CsharpAdvFinalDemo\API\Backup";
                if (!backup.DirectoryExists())
                {
                    Directory.CreateDirectory(backup);
                }
                string pathOfBackup = backup + @"\backupData.csv";

                using (StreamWriter sw = new StreamWriter(pathOfBackup))
                {
                    sw.WriteLine($"W01F01,W01F02,W01F03,W01F04,W01F05,W01F06,W01F07");

                    foreach (BRW01 item in result)
                    {
                        dynamic temp = item.W01F05 ?? "NULL";
                        sw.WriteLine($"{item.W01F01},{item.W01F02},{item.W01F03},{item.W01F04},{temp},{item.W01F06},{item.W01F07}");
                    }
                }

                _objResponse.IsError = false;
                _objResponse.Message = "Records backup successful";
            }

            return _objResponse;
        }
    }
}