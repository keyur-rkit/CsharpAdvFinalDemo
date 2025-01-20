using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models.POCO
{
    /// <summary>
    /// POCO model of Categories
    /// </summary>
    public class CAT01
    {
        /// <summary>
        /// CategoryId
        /// </summary>
        public int T01F01 { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string T01F02 { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string T01F03 { get; set; }

        /// <summary>
        /// CreatedAt
        /// </summary>
        [IgnoreOnUpdate]
        public DateTime T01F04 { get; set; } = DateTime.Now;

        /// <summary>
        /// UpdatedAt
        /// </summary>
        public DateTime T01F05 { get; set; } = DateTime.Now;
    }
}