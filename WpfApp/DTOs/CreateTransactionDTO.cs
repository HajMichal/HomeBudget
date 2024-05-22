using Core.Models;
using System;
using System.Collections.Generic;

namespace Core.DTOs
{
    public class CreateTransactionDTO
    {
        public string Name { get; set; }
        public DateTime? FromDate { get; set; }
        public Category Category { get; set; }
        public List<CreateProductDTO> Products { get; set; }
    }

    public class CreateProductDTO
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public uint Count { get; set; }

        public Product ToEntity()
            => Product.New(Name, Count, Price);

        public override string ToString()
        {
            return $"{Name} - {Price} z³ - {Count} szt";
        }
    }


}