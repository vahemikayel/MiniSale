using System.Collections.Generic;
using System.Net;

namespace MiniSale.Api.Models.Response
{
    public class ApiValidationErrorResponse : ResultModel
    {
        public ApiValidationErrorResponse()
        {
        }
        public IEnumerable<string> Errors { get; set; }
        public int StatusCode { get; set; } = (int)HttpStatusCode.BadRequest;
        public string Message { get; set; }
    }
}
