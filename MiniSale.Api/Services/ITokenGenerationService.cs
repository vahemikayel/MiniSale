using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using MiniSale.Api.Models.Account.Entity;
using MiniSale.Api.Models.Account.Response;

namespace MiniSale.Api.Services
{
    public interface ITokenGenerationService
    {
        Task<AccessTokenResponseModel> GenerateAccessTokenAsync(ApplicationUser user, List<string> scopes = null, CancellationToken cancellationToken = default);
    }
}
