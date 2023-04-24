using MiniSale.Api.Infrastructure.BaseReuqestTypes;
using MiniSale.Api.Models.Product.Response;

namespace MiniSale.Api.Application.Commands.Informers
{
    public class ProductBaseCommand : BaseRequest<ProductReadModel>
    {
        public string Name { get; set; }

        public decimal Price { get; set; }
        public string BarCode { get; set; }
        public int PLU { get; set; }
    }
}
