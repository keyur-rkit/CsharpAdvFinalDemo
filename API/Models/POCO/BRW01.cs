using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models.POCO
{
    /// <summary>
    /// POCO model of BorrowRecords
    /// </summary>
    public class BRW01
    {
        /// <summary>
        /// RecordId
        /// </summary>
        public int W01F01 { get; set; }

        /// <summary>
        /// UserId
        /// </summary>
        public int W01F02 { get;set; }

        /// <summary>
        /// BookId
        /// </summary>
        public int W01F03 { get; set; }

        /// <summary>
        /// BorrowDate
        /// </summary>
        public DateTime W01F04 { get; set; }

        /// <summary>
        /// ReturnDate (nullable)
        /// </summary>
        public DateTime? W01F05 { get; set; }

        /// <summary>
        /// CreatedAt
        /// </summary>
        [IgnoreOnUpdate]
        public DateTime W01F06 { get; set; } = DateTime.Now;
        
        /// <summary>
        /// UpdatedAt
        /// </summary>
        public DateTime W01F07 { get; set; } = DateTime.Now;
    }
}