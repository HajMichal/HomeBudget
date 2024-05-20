using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.DTOs
{
    public sealed class TransactionDTO
    {
        public Guid Id { get; set; }
        public string Category { get; set; }
        public decimal TotalCost { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    
        public IEnumerable<ProductDTO> Products { get; set; }
    
        public static TransactionDTO FromEntity(Transaction transaction) => new TransactionDTO
        {
            Id = transaction.Id,
            Category = transaction.Category.ToString(),
            TotalCost = transaction.TotalCost,
            FromDate = transaction.FromDate,
            CreatedAt = transaction.CreatedAt,
            ModifiedAt = transaction.ModifiedAt,
            Products = transaction.Products.Select(ProductDTO.FromEntity)
        };
    }
}