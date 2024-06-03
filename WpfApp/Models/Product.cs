using Core.Exceptions;
using Core.Utils;

namespace Core.Models
{
    public class Product
    {
        public string Name { get; private set; }
        public uint Count { get; private set; } 
        public decimal Price { get; private set; }
        public decimal TotalPrice { get; private set; }
    
        public static Product New(
            string name, 
            uint count, 
            decimal price)
        {
            if (!name.HasValue())
                throw new NameIsRequiredException();

            if (price < 0)
                throw new PriceException();
        
            return new Product(name, count, price);
        }

        private Product(
            string name,
            uint count,
            decimal price)
        {
            Name = name;
            Price = price;
            Count = count;
            TotalPrice = price * count;
        }

        public override string ToString()
        {
            return $"{Name} - {Count} x {Price} z³ -> {TotalPrice} z³";
        }
    }
}