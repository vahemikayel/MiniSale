using System.Collections.Generic;

namespace MiniSale.Api.Infrastructure.Filters
{
    public class JsonErrorResponse
    {
        public string[] Messages { get; set; }

        public object DeveloperMessage { get; set; }
        public string ClientMessage { get; set; } = string.Empty;
        public List<string> Errors { get; set; }
        public List<string> Arguments { get; set; }
    }
}
