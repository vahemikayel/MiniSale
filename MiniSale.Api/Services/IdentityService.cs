using Microsoft.AspNetCore.Http;
using MiniSale.Api.Extensions;
using System;
using System.Linq;

namespace MiniSale.Api.Services
{
    public class IdentityService : IIdentityService
    {
        private IHttpContextAccessor _context;

        public IdentityService(IHttpContextAccessor context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Guid GetIdentity()
        {
            return _context.HttpContext.GetIdentity();
        }
        public string GetScope()
        {

            return _context.HttpContext.User.Claims.Where(x => x.Type == "scope")
                                                   .Select(x => x.Value)
                                                   .FirstOrDefault();
        }

        public string GetUserIdentity()
        {
            return _context.HttpContext.GetUserIdentity();
        }

        public int GetUserType()
        {
            return _context.HttpContext.GetUserType();
        }

        public Uri GetAbsoluteUri()
        {
            return _context.HttpContext.GetAbsoluteUri();
        }

        //public Guid GetSalePortal()
        //{
        //    return _context.HttpContext.GetSalePortal();
        //}
        public bool IsRegistered()
        {
            return _context.HttpContext.IsRegistered();
        }

        public string[] GetUserRoles()
        {
            return _context.HttpContext.GetUserRoles();

        }

        public Guid GetSubId()
        {
            return _context.HttpContext.GetSubId();
        }

        public Guid GetUserId()
        {
            return _context.HttpContext.GetUserId();
        }
    }
}
