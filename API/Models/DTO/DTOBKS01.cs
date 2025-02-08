using System;
using System.ComponentModel.DataAnnotations;

namespace API.Models.DTO
{
    /// <summary>
    /// DTO for POCO BKS01
    /// </summary>
    public class DTOBKS01
    {
        /// <summary>
        /// Title
        /// </summary>
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(255, ErrorMessage = "Title cannot exceed 255 characters.")]
        public string S01F02 { get; set; }

        /// <summary>
        /// AuthorId
        /// </summary>
        [Required(ErrorMessage = "AuthorId is required.")]
        public int S01F03 { get; set; }

        /// <summary>
        /// CategoryId
        /// </summary>
        [Required(ErrorMessage = "CategoryId is required.")]
        public int S01F04 { get; set; }

        /// <summary>
        /// ISBN
        /// </summary>
        [Required(ErrorMessage = "ISBN is required.")]
        [StringLength(13, ErrorMessage = "ISBN must be 13 digits.", MinimumLength =13)]
        public string S01F05 { get; set; }

        /// <summary>
        /// AvailableCopies
        /// </summary>
        [Required(ErrorMessage = "AvailableCopies is required.")]
        public int S01F06 { get; set; }

        /// <summary>
        /// Rating
        /// </summary>
        [Required(ErrorMessage = "Rating is required.")]
        [Range(0.0,5.0, ErrorMessage = "Rating must be in 0.0 to 5.0")]
        public decimal S01F07 { get; set; }
    }
}