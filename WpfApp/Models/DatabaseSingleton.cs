using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Core.Models
{
    public class DatabaseSingleton
    {
        private static readonly Lazy<DatabaseSingleton> lazy =
            new Lazy<DatabaseSingleton>(() => new DatabaseSingleton());

        public static DatabaseSingleton Instance => lazy.Value;

        private DatabaseSingleton()
        {
        }

        public ObservableCollection<Budget> Budgets { get; set; } = new ObservableCollection<Budget>();
    }
}