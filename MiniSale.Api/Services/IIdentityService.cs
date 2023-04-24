using System;

namespace MiniSale.Api.Services
{
    public interface IIdentityService
    {
        internal static string ClientKey = "clientId";
        internal static string UserIdKey = "userId";

        string GetUserIdentity();
        Guid GetIdentity();
        string GetScope();
        int GetUserType();
        Guid GetSubId();
        Uri GetAbsoluteUri();
        bool IsRegistered();
        string[] GetUserRoles();

        Guid GetUserId();
    }
}
