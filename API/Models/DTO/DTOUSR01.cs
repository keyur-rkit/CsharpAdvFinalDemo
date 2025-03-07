﻿using API.Models.Enum;
using System.ComponentModel.DataAnnotations;


namespace API.Models.DTO
{
    /// <summary>
    /// DTO for POCO USR01 
    /// </summary>
    public class DTOUSR01
    {
        /// <summary>
        /// Username
        /// </summary>
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(50, ErrorMessage = "Username cannot exceed 50 characters.")]
        public string R01F02 { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [StringLength(255, ErrorMessage = "Email cannot exceed 255 characters.")]
        public string R01F03 { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        [Required(ErrorMessage = "Password is required.")]
        [StringLength(255, MinimumLength = 6, ErrorMessage = "Password should be between 6 and 100 characters.")]
        public string R01F04 { get; set; }

        /// <summary>
        /// Role
        /// </summary>
        [Required(ErrorMessage = "Role is required.")]
        [EnumDataType(typeof(EnmRole), ErrorMessage = "Invalid role.")]
        public EnmRole R01F05 { get; set; }

        /// <summary>
        /// BorrowLimit
        /// </summary>
        [Required(ErrorMessage = "BorrowLimit is required.")]
        [Range(0,20, ErrorMessage = "Borrow Limit must be in 0 to 20")]
        public int R01F06 { get; set; }
    }
}