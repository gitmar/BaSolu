using System.Net;

namespace GxPilo.Services
{
    public class ApiResult
    {
        public bool Success { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string? Message { get; set; }
    }
}
