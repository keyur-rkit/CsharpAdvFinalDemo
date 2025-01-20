using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models.DTO
{
    public class DTOBKS01
    {
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
        public int S01F04 { get; set; }

        /// <summary>
        /// ISBN
        /// </summary>
        public string S01F05 { get; set; }

        /// <summary>
        /// AvailableCopies
        /// </summary>
        public int S01F06 { get; set; }
    }
}