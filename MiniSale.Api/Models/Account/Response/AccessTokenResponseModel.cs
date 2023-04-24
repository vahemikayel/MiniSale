using System.Collections.Generic;

namespace MiniSale.Api.Models.Account.Response
{
    public class AccessTokenResponseModel
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }
}
