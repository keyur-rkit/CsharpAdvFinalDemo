

namespace API.Models
{
    public class Response
    {
        public dynamic Data { get; set; }

        public bool IsError { get; set; } = false;

        public string Message { get; set; }
    }
}