using System;
using System.Data.SQLite;
using System.IO;

namespace Core.Models
{
    public static class DbHelper
    {
        private static string connectionString = @"Data Source=C:\Programowanie\Domain\Core\DB\db.db;Version=3;";
        public static void InitializeDatabase()
        {
            if (!File.Exists(@"C:\Programowanie\Domain\Core\DB\db.db"))
            {

                SQLiteConnection.CreateFile(@"C:\Programowanie\Domain\Core\DB\db.db");




                    string createBudgetTableQuery = @"
                        CREATE TABLE Budget (
                            id          INTEGER PRIMARY KEY,
                            name        TEXT NOT NULL,
                            salary      INTEGER,
                            expenses    INTEGER,
                            balance     INTEGER
                        );";

                    string createTransactionTableQuery = @"
                        CREATE TABLE `Transaction` (
                            id          INTEGER PRIMARY KEY,
                            name        TEXT NOT NULL,
                            fromDate    TEXT,
                            category    TEXT,
                            totalCost   INTEGER,
                            budgetId    INTEGER NOT NULL,
                            FOREIGN KEY (budgetId) REFERENCES `Budget`(id)
                        );";

                    string createProductTableQuery = @"
                        CREATE TABLE `Product` (
                            id              INTEGER PRIMARY KEY,
                            name            TEXT NOT NULL,
                            count           INTEGER NOT NULL,
                            price           INTEGER NOT NULL,
                            totalprice      INTEGER,
                            transactionId   INTEGER NOT NULL,
                            FOREIGN KEY (transactionId) REFERENCES `Transaction`(id)
                        );";

                SQLiteConnection connection = new SQLiteConnection(connectionString);
                    connection.Open();

                    SQLiteCommand command1 = new SQLiteCommand(createBudgetTableQuery, connection);
                    command1.ExecuteNonQuery();
                    Console.WriteLine("Tabela budget zostala utworzona");

                    SQLiteCommand command2 = new SQLiteCommand(createTransactionTableQuery, connection);
                    command2.ExecuteNonQuery();
                    Console.WriteLine("Tabela transaction zostala utworzona");

                    SQLiteCommand command3 = new SQLiteCommand(createProductTableQuery, connection);
                    command3.ExecuteNonQuery();
                    Console.WriteLine("Tabela product zostala utworzona");

                    connection.Close();
                
            }
        }
    }
}
