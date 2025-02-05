using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models.POCO
{
    /// <summary>
    /// POCO model of Books
    /// </summary>
    public class BKS01
    {
        /// <summary>
        /// BookId
        /// </summary>
        public int S01F01 { get; set; }

        /// <summary>
        /// Title
        /// </summary>
        public string S01F02 { get; set; }

        /// <summary>
        /// AuthorId
        /// </summary>
        public int S01F03 { get; set; }

        /// <summary>
        /// CategoryId
        /// </summary>
        public int S01F04 { get;  set; }

        /// <summary>
        /// ISBN
        /// </summary>
        public string S01F05 { get; set; }

        /// <summary>
        /// AvailableCopies
        /// </summary>
        public int S01F06 { get; set; }

        /// <summary>
        /// Rating
        /// </summary>
        public decimal S01F07 { get; set; }

        /// <summary>
        /// CreatedAt
        /// </summary>
        [IgnoreOnUpdate]
        public DateTime S01F08 { get; set; } = DateTime.Now;

        /// <summary>
        /// UpdatedAt
        /// </summary>
        public DateTime S01F09 { get; set; } = DateTime.Now;
    }
}