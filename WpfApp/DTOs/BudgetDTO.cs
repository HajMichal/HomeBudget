using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.DTOs
{
    public sealed class BudgetDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal MonthSalary { get; set; }
        public decimal MonthExpenses { get; set; }
        public decimal Summary { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    
        public IEnumerable<TransactionDTO> Transactions { get; set; }
    
        public static BudgetDTO FromEntity(Budget budget) => new BudgetDTO
        {   
            Id = budget.Id,
            Name = budget.Name,
            MonthSalary = budget.MonthSalary,
            MonthExpenses = budget.MonthExpenses,
            Summary = budget.Summary,
            CreatedAt = budget.CreatedAt,
            ModifiedAt = budget.ModifiedAt,
            Transactions = budget.Transactions.Select(TransactionDTO.FromEntity)
        };
    }
}