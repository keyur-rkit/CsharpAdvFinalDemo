using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;


namespace API.Models.DTO
{
    /// <summary>
    /// DTO for POCO CAT01
    /// </summary>
    public class DTOCAT01
    {
        /// <summary>
        /// Name
        /// </summary>
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters.")]
        [JsonProperty("T01102")]
        public string T01F02 { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        [JsonProperty("T01103")]
        public string T01F03 { get; set; }
    }
}