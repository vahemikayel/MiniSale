using System;

namespace MiniSale.Api.Models.Account.Response
{
    public class ApplicationUserResponseModel
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }
    }
}
