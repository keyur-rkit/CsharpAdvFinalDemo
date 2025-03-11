using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace API.Models.DTO
{
    /// <summary>
    /// DTO for POCO BRW01
    /// </summary>
    public class DTOBRW01
    {
        /// <summary>
        /// UserId
        /// </summary>
        [Required(ErrorMessage = "UserId is required.")]
        [JsonProperty("W01102")]
        public int W01F02 { get; set; }

        /// <summary>
        /// BookId
        /// </summary>
        [Required(ErrorMessage = "BookId is required.")]
        [JsonProperty("W01103")]
        public int W01F03 { get; set; }

        /// <summary>
        /// BorrowDate
        /// </summary>
        [Required(ErrorMessage = "BorrowDate is required.")]
        [JsonProperty("W01104")]
        public DateTime W01F04 { get; set; }

        /// <summary>
        /// ReturnDate (nullable)
        /// </summary>
        [JsonProperty("W01105")]
        public DateTime? W01F05 { get; set; }
    }
}