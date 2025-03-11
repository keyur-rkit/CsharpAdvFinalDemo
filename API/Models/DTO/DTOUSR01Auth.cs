using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Models.DTO
{
    /// <summary>
    /// DTO for Authentication
    /// </summary>
    public class DTOUSR01Auth
    {
        /// <summary>
        /// Username
        /// </summary>
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(50, ErrorMessage = "Username cannot exceed 50 characters.")]
        [JsonProperty("R01102")]
        public string R01F02 { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        [Required(ErrorMessage = "Password is required.")]
        [StringLength(255, MinimumLength = 6, ErrorMessage = "Password should be between 6 and 100 characters.")]
        [JsonProperty("R01104")]
        public string R01F04 { get; set; }
    }
}