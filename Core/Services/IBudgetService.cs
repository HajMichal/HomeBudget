using System;
using System.Collections.Generic;
using Core.DTOs;

namespace Core.Services
{
    public interface IBudgetService
    {
        IEnumerable<BudgetDTO> Get();
        BudgetDTO Create(CreateBudgetDTO dto);
        void Remove(Guid budgetId);
        void AddTransaction(CreateTransactionDTO transactionDto, Guid budgetId);
        void RemoveTransaction(Guid transactionId, Guid budgetId);
    }
}