using Microsoft.AspNetCore.Identity;

namespace MiniSale.Api.Models.Account.Entity
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
