using MiniSale.Generic.Repository.Models;
using MiniSale.Interfaces;
using System;

namespace MiniSale.Api.Models.Product.Entity
{
    public class ProductEModel : BaseEntity<Guid>, IProduct
    {
        public string Name { get; set; }

        public decimal Price { get; set; }
        public string BarCode { get; set; }
        public int PLU { get; set; }
    }
}
