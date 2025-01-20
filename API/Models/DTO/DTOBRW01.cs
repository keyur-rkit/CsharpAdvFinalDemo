using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models.DTO
{
    public class DTOBRW01
    {
        /// <summary>
        /// UserId
        /// </summary>
        public int W01F02 { get; set; }

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
    }
}