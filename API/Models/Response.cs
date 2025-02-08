

namespace API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Response
    {
        /// <summary>
        /// 
        /// </summary>
        public dynamic Data { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsError { get; set; } = false;

        /// <summary>
        /// 
        /// </summary>
        public string Message { get; set; }
    }
}