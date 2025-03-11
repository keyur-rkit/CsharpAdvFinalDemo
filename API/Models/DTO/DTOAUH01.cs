using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace API.Models.DTO
{
    /// <summary>
    /// DTO for POCO AUH01
    /// </summary>
    public class DTOAUH01
    {
        /// <summary>
        /// Name
        /// </summary>
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters.")]
        [JsonProperty("H01102")]
        public string H01F02 { get; set; }

        /// <summary>
        /// Bio
        /// </summary>
        [JsonProperty("H01103")]
        public string H01F03 { get; set; }
    }
}