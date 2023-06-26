using System.Net;

namespace SensorProject.Models.Dto
{
    public class ResponseDetails
    {
        public string? Message { get; set; }
        public HttpStatusCode StatusCode { get; set; }  
        public dynamic? data { get; set; }
    }
}
