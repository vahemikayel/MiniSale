using MiniSale.Interfaces;
using System;

namespace MiniSale.Api.Models.Product.Response
{
    public class ProductReadModel : BaseReadModel, IProduct
    {
        public string Name { get; set; }

        public decimal Price { get; set; }
        public string BarCode { get; set; }
        public int PLU { get; set; }
    }
}
