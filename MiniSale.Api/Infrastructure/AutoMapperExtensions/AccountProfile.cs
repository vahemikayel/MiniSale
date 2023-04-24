using AutoMapper;
using MiniSale.Api.Application.Commands.Account;
using MiniSale.Api.Models.Account.Entity;
using MiniSale.Api.Models.Account.Response;
using System;

namespace MiniSale.Api.Infrastructure.AutoMapperExtensions
{
    public class AccountProfile : Profile
    {
        /// <summary>
        /// 
        /// </summary>
        public AccountProfile()
        {
            CreateMap<RegisterUserCommand, ApplicationUser>();

            CreateMap<ApplicationUser, ApplicationUserResponseModel>()
                .ForMember(d => d.Id, src => src.MapFrom(s => Guid.Parse(s.Id)));
        }
    }
}
