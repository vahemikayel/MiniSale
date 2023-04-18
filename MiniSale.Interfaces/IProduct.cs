namespace MiniSale.Interfaces
{
    public interface IProduct
    {
        Guid Id { get; set; }

        /// <summary>
        /// 5-15 characters 
        /// </summary>
        string Name { get; set; }

        decimal Price { get; set; }

        /// <summary>
        /// Can be null or exactly contain 13 characters and be unique
        /// </summary>
        string BarCode { get; set; }

        /// <summary>
        /// Must be in range 1-99999 and unique
        /// </summary>
        int PLU { get; set; }
    }
}