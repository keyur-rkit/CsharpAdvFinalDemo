using API.Models.Enum;
using System;
using ServiceStack.DataAnnotations;


namespace API.Models.POCO
{
    /// <summary>
    /// POCO model of Authors
    /// </summary>
    public class AUH01
    {
        /// <summary>
        /// AuthorId
        /// </summary>
        public int H01F01 { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string H01F02 { get; set; }

        /// <summary>
        /// Bio
        /// </summary>
        public string H01F03 { get; set; }

        /// <summary>
        /// CreatedAt
        /// </summary>
        [IgnoreOnUpdate]
        public DateTime H01F04 { get; set; } = DateTime.Now;

        /// <summary>
        /// UpdatedAt
        /// </summary>
        public DateTime H01F05 { get; set; } = DateTime.Now;
    }
}