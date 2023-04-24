using MiniSale.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniSale.DummyData.Models
{
    public class ProductDummyModel : IProduct
    {
        public Guid Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public decimal Price { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string BarCode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int PLU { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
