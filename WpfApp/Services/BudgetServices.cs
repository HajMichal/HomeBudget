using Core.DTOs;
using Core.Exceptions;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Services
{
    public class BudgetServices
    {
        private DatabaseSingleton db => DatabaseSingleton.Instance;
        public IEnumerable<BudgetDTO> Get()
            => db.Budgets.Select(BudgetDTO.FromEntity);
    
        public BudgetDTO Create(CreateBudgetDTO dto)
        {
            var budget = Budget.New(dto.Name, dto.MonthSalary);

            db.Budgets.Add(budget);

            return BudgetDTO.FromEntity(budget);
        }

        public void Remove(Guid budgetId)
        {
            var budget = db.Budgets.Find(x => x.Id == budgetId);
        
            if (budget == null)
                throw new NotFoundException(nameof(Budget));
        
            db.Budgets.Remove(budget);
        }

        public void AddTransaction(CreateTransactionDTO transactionDto, Guid budgetId)
        {
            var budget = db.Budgets.Find(x => x.Id == budgetId);
        
            if (budget == null)
                throw new NotFoundException(nameof(Budget));
        
            var newTransaction = Transaction.New(
                transactionDto.Products.Select(p => p.ToEntity()).ToList(),
                transactionDto.Name,
                transactionDto.Category,
                transactionDto.FromDate);
        
            budget.AddTransaction(newTransaction);
        }
    
        public void RemoveTransaction(Guid transactionId, Guid budgetId)
        {
            var budget = db.Budgets.Find(x => x.Id == budgetId);

            if (budget == null)
                throw new NotFoundException(nameof(Budget));
        
            var transaction = budget.Transactions.Find(x => x.Id == transactionId);
        
            if (transaction == null)
                throw new NotFoundException(nameof(Transaction));
        
            budget.RemoveTransaction(transaction);
        }
    }
}