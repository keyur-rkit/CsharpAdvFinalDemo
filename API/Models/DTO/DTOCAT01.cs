using System.ComponentModel.DataAnnotations;


namespace API.Models.DTO
{
    public class DTOCAT01
    {
        /// <summary>
        /// Name
        /// </summary>
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters.")]
        public string T01F02 { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string T01F03 { get; set; }
    }
}