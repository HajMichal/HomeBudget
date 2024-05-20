using Core.Models;

namespace Core.DTOs
{
    public sealed class ProductDTO
    {
        public string Name { get; set; }
        public uint Count { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
    
        public static ProductDTO FromEntity(Product product) => new ProductDTO
        {
            Name = product.Name,
            Count = product.Count,
            Price = product.Price,
            TotalPrice = product.TotalPrice
        };
    }
}