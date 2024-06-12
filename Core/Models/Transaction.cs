using Core.Exceptions;
using Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Models
{
    public class Transaction : AuditedEntity
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Name { get; private set; }
        public DateTime FromDate { get; private set; }
        public Category Category { get; private set; }
        public decimal TotalCost { get; private set; }

        public IEnumerable<Product> Products { get; private set; }

        public static Transaction New(IList<Product> products, string name, Category category = Category.Brak, DateTime? fromDate = null)
        {
            if (!name.HasValue())
            {
                throw new NameIsRequiredException();
            }
        
            if(!fromDate.HasValue)
            {
                fromDate = DateTime.Now;
            }

            return new Transaction(products, name, category, fromDate.Value);
        }
    
        private Transaction(IList<Product> products, string name, Category category, DateTime fromDate)
        {
            Name = name;
            Products = products;
            Category = category;
            FromDate = fromDate;
            TotalCost = products.Sum(x => x.TotalPrice);
        }

        public override string ToString()
        {
            return $"{Name} - {Category} -> {TotalCost} z�";
        }
    }
}