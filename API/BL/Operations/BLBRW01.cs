﻿using API.BL.Interface;
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
    /// 
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
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsExist(int id)
        {
            using (IDbConnection db = _dbFactory.OpenDbConnection())
            {
                return db.Exists<BRW01>(r => r.W01F01 == id);
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
                    _objResponse.Message = "Records get successfully";
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
                    _objResponse.Message = "Record dose not Exist";
                    _objResponse.Data = null;

                    return _objResponse;
                }
                using (IDbConnection db = _dbFactory.OpenDbConnection())
                {
                    _objResponse.Data = db.SingleById<BRW01>(id);
                    _objResponse.Message = "Record get successfully";
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
        /// <param name="id"></param>
        /// <returns></returns>
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
                    _objResponse.Message = "Records get successfully";
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
        /// <param name="objBRW01"></param>
        /// <returns></returns>
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
                    _objResponse.Message = $"{bookResponse.Message}, {userResponse.Message}, Record Added";
                }
            }
            catch(Exception ex)
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
        /// <param name="returnDate"></param>
        /// <returns></returns>
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
                        _objResponse.Message = "Book returned already";
                        return _objResponse;
                    }

                    if(returnDate < _objBRW01.W01F04)
                    {
                        _objResponse.IsError = true;
                        _objResponse.Message = "ReturnDate must be after BorrowDate";
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
            catch(Exception ex)
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
        public void PreSave(DTOBRW01 objDTO)
        {
            _objBRW01 = objDTO.Convert<BRW01>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Response Validation()
        {
            if(Type == EnmType.E)
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
        /// 
        /// </summary>
        /// <returns></returns>
        public Response Save()
        {
            if(Type == EnmType.A)
            {
                return Borrow(_objBRW01);
            }
            else if(Type == EnmType.E)
            {
                return Return(Id,ReturnDate);
            }

            return _objResponse;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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

                using(StreamWriter sw  = new StreamWriter(pathOfBackup))
                {
                    sw.WriteLine($"W01F01,W01F02,W01F03,W01F04,W01F05,W01F06,W01F07");

                    foreach(BRW01 item in result)
                    {   dynamic temp;
                        if(item.W01F05 == null)
                        {
                            temp = "NULL";
                        }
                        else
                        {
                            temp = item.W01F05;
                        }

                        sw.WriteLine($"{item.W01F01},{item.W01F02},{item.W01F03},{item.W01F04},{temp},{item.W01F06},{item.W01F07}");
                    }
                }

                _objResponse.IsError = false;
                //_objResponse.Data = result;
                _objResponse.Message = "Records backup successfull";
            }

            return _objResponse;
        }

    }
}