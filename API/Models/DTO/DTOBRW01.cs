using System;
using System.ComponentModel.DataAnnotations;

namespace API.Models.DTO
{
    public class DTOBRW01
    {
        /// <summary>
        /// UserId
        /// </summary>
        [Required(ErrorMessage = "UserId is required.")]
        public int W01F02 { get; set; }

        /// <summary>
        /// BookId
        /// </summary>
        [Required(ErrorMessage = "BookId is required.")]
        public int W01F03 { get; set; }

        /// <summary>
        /// BorrowDate
        /// </summary>
        [Required(ErrorMessage = "BorrowDate is required.")]
        public DateTime W01F04 { get; set; }

        /// <summary>
        /// ReturnDate (nullable)
        /// </summary>
        public DateTime? W01F05 { get; set; }
    }
}