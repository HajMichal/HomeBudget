using Core.Exceptions;
using Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Models
{
    public class Budget : AuditedEntity
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Name { get; private set; }
        public decimal MonthSalary { get; private set; }
        public decimal MonthExpenses { get; private set; }
        public decimal Balance => MonthSalary - MonthExpenses;

        public List<Transaction> Transactions { get; private set; } = new List<Transaction>();

        public static Budget New(string name, decimal monthSalary)
        {
            if (!name.HasValue())
                throw new NameIsRequiredException();

            // todo poprawic na polskie exception i rowniez tutaj zrobic parsowanie i wywalac wyjatki
        
            return new Budget(name, monthSalary);
        }
    
        public void AddTransaction(Transaction transaction)
        {
            Transactions.Add(transaction);
            Update();
        }
    
        public void RemoveTransaction(Transaction transaction)
        {
            Transactions.Remove(transaction);
            Update();
        }

        public void Update(string name)
        {
            if (!name.HasValue())
                throw new NameIsRequiredException();

            if (Name != name) 
                Name = name;
        }

        protected override void Update()
        {
            MonthExpenses = Transactions.Sum(p => p.TotalCost);
            base.Update();
        }

        private Budget(string name, decimal monthSalary)
        {
            Name = name;
            MonthSalary = monthSalary;
        }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}