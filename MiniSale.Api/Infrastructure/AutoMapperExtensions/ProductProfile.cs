using AutoMapper;
using MiniSale.Api.Application.Commands.Informers;
using MiniSale.Api.Models.Product.Entity;
using MiniSale.Api.Models.Product.Response;
using MiniSale.DummyData.Models;

namespace MiniSale.Api.Infrastructure.AutoMapperExtensions
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductEModel, ProductReadModel>();
            CreateMap<ProductDummyModel, ProductEModel>();
            CreateMap<ProductAddCommand, ProductEModel>();
            CreateMap<ProductEditCommand, ProductEModel>();
        }
    }
}
