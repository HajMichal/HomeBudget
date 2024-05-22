using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Core.DTOs;
using Core.Models;
using Core.Services;
using Core.Utils;

namespace YourNamespace
{
    public partial class MainWindow : Window
    {
        // [TODO]: Dodac zaleznosci wyswietlanych transakcji wzgledem budzetu oraz wyswietlanych produktow wzgledem transakcji
        private ObservableCollection<CreateProductDTO> products;
        private DatabaseSingleton db;

        public MainWindow()
        {
            InitializeComponent();
            LoadCategories();

            db = DatabaseSingleton.Instance;
            products = new ObservableCollection<CreateProductDTO>();
            BudgetListBox.ItemsSource = db.Budgets;
            TransactionDetailsListBox.ItemsSource = null;
            ProductListBox.ItemsSource = products;
        }


        // BUDGET
        private void btnShowCreateBudgetForm_Click(object sender, RoutedEventArgs e)
        {
            TransactionsListBox.SelectedIndex = -1;
            if (BudgetForm.Visibility == Visibility.Collapsed)
            {
                // display budget form
                BudgetForm.Visibility = Visibility.Visible;

                // hide other formulas / lists
                TransactionForm.Visibility = Visibility.Collapsed;
                StatsField.Visibility = Visibility.Collapsed;
            }
            else
            {
                BudgetForm.Visibility = Visibility.Collapsed;
            }
        }

        private void btnAddBudget_Click(object sender, RoutedEventArgs e)
        {
            // Get values from inputs
            string budgetName = BudgetName.Text;
            string monthSalary = MonthSalary.Text;

            TransactionsListBox.SelectedIndex = -1;
            BudgetListBox.SelectedIndex = -1;
            if (!string.IsNullOrWhiteSpace(budgetName) && !string.IsNullOrWhiteSpace(monthSalary))
            {
                if (decimal.TryParse(monthSalary, out decimal income))
                {
                    BudgetForm.Visibility = Visibility.Collapsed;
                    var service = new BudgetServices();
                    var newBudget = new CreateBudgetDTO { Name = budgetName, MonthSalary = income };
                    service.Create(newBudget);

                    MessageBox.Show("Stworzono budżet");
                }
                else
                {
                    MessageBox.Show("Zarobek należy wpisać jako liczbę");
                }

                // Clear input fields
                BudgetName.Clear();
                MonthSalary.Clear();
            }
            else
            {
                MessageBox.Show("Wypełnij dane w każdym polu.");
            }
        }

        private void btnRemoveBudget_Click(object sender, RoutedEventArgs e)
        {
            TransactionsListBox.SelectedIndex = -1;
            BudgetListBox.SelectedIndex = -1;

            if (BudgetListBox.SelectedItem is Budget selectdBudget)
            {
                var service = new BudgetServices();
                service.Remove(selectdBudget.Id);
            }
            else
            {
                MessageBox.Show("Wybierz budżet do usunięcia.");
            }
        }

        private void btnShowStats_Click(object sender, RoutedEventArgs e)
        {
            TransactionsListBox.SelectedIndex = -1;

            if (BudgetListBox.SelectedItem is Budget selectedBudget)
            {
                if (StatsField.Visibility == Visibility.Collapsed)
                {
                    // display stat field
                    StatsField.Visibility = Visibility.Visible;
                    TransactionsListBox.ItemsSource = selectedBudget.Transactions;

                    // hide other formulas / fields
                    BudgetForm.Visibility = Visibility.Collapsed;
                    TransactionForm.Visibility = Visibility.Collapsed;
                }
                else
                {
                    StatsField.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                MessageBox.Show("Musisz zaznczayć budżet, dla którego chcesz zobaczyć statystyki");
            }
        }

        // TRANSACTIONS
        private void btnShowCreateTransactionForm_Click(object sender, RoutedEventArgs e)
        {
            TransactionsListBox.SelectedIndex = -1;
            if (TransactionForm.Visibility == Visibility.Collapsed)
            {
                // display transaction form
                TransactionForm.Visibility = Visibility.Visible;

                // hide other formulas / lists
                BudgetForm.Visibility = Visibility.Collapsed;
                StatsField.Visibility = Visibility.Collapsed;
            }
            else
            {
                TransactionForm.Visibility = Visibility.Collapsed;
            }
        }
        private void btnShowCreateTransactionPanel_Click(object sender, SelectionChangedEventArgs e)
        {
            if (BudgetListBox.SelectedItem != null)
            {
                CreateTransactionPanel.Visibility = Visibility.Visible;
            }
            else
            {
                CreateTransactionPanel.Visibility = Visibility.Collapsed;
            }
        }


        private void btnCreateTransaction_Click(object sender, RoutedEventArgs e)
        {
            TransactionsListBox.SelectedIndex = -1;

            if (BudgetListBox.SelectedItem is Budget selectedBudget)
            {
                string transactionName = TransactionName.Text;
                Category transactionCategory = (Category)TransactionCategory.SelectedItem;
                string transactionDate = TransactionDate.Text;

                if (!string.IsNullOrWhiteSpace(transactionName))
                {
                    var service = new BudgetServices();

                    DateTime? parsedDate = string.IsNullOrEmpty(transactionDate) ?
                        (DateTime?)null :
                        DateTime.Parse(transactionDate);

                    var newTransaction = new CreateTransactionDTO 
                    { 
                        Name = transactionName, 
                        FromDate = parsedDate, 
                        Category = transactionCategory,
                        Products = products.ToList(),
                    };
                    service.AddTransaction(newTransaction, selectedBudget.Id);

                    products.Clear();
                    TransactionForm.Visibility = Visibility.Collapsed;
                    MessageBox.Show("Transakcja została zapisana");
                }
                else
                {
                    MessageBox.Show("Wypełnij dane w każdym polu.");
                }

                TransactionName.Clear();
                TransactionDate.Clear();
            }
            else
            {
                MessageBox.Show("Musisz zaznczayć budżet, dla którego chcesz utworzyć transakcję");
            }
        }

        private void btnRemoveTransaction_Click(object sender, RoutedEventArgs e)
        {
            if (TransactionsListBox.SelectedItem is Transaction selectedTransaction &&
                BudgetListBox.SelectedItem is Budget selectedBudget)
            {
                StatsField.Visibility = Visibility.Collapsed;
                ModifyTransactionPanel.Visibility = Visibility.Collapsed;
                TransactionDetailsListBox.ItemsSource = null;
                TransactionsListBox.SelectedIndex = -1;

                var service = new BudgetServices();
                service.RemoveTransaction(selectedTransaction.Id, selectedBudget.Id);
                MessageBox.Show("Transakcja została pomyślnie usunięta");
            }
            else
            {
                MessageBox.Show("Wybierz transakcję do usunięcia.");
            }
        }

        private void btnShowTransactionDetails_Click(object sender, RoutedEventArgs e)
        {
            if (TransactionsListBox.SelectedItem is Transaction selectedTransaction)
            {   
                // Ta linijka psuje wyswietlanie transakcji (gdy ona dziala pokazuje tylko pierwsza transakcje)
                // TransactionDetailsListBox.ItemsSource = selectedTransaction.Products;
                ModifyTransactionPanel.Visibility = Visibility.Visible;
                TransactionDetailsListBox.Visibility = Visibility.Visible;
            }
            else
            {
                ModifyTransactionPanel.Visibility = Visibility.Collapsed;
                TransactionDetailsListBox.Visibility = Visibility.Collapsed;
            }
        }

        // PRODUCT
        private void btnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            // Get values from inputs
            string productName = ProductName.Text;
            string productPrice = ProductPrice.Text;
            string productQuantity = ProductQuantity.Text;

            if (!string.IsNullOrWhiteSpace(productName) && !string.IsNullOrWhiteSpace(productPrice) && !string.IsNullOrWhiteSpace(productQuantity))
            {
                if (decimal.TryParse(productPrice, out decimal parsedPrice) && uint.TryParse(productQuantity, out uint parsedQuantity))
                {
                    var newProduct = new CreateProductDTO 
                    { 
                        Name = productName, 
                        Count = parsedQuantity, 
                        Price = parsedPrice 
                    };
                    products.Add(newProduct);
                }
                else
                {
                    MessageBox.Show("Ilość oraz cenę należy wpisać jako liczbę");
                }

                // Clear input fields
                ProductName.Clear();
                ProductPrice.Clear();
                ProductQuantity.Clear();
            }
            else
            {
                MessageBox.Show("Wypełnij dane w każdym polu.");
            }
        }

        private void btnRemoveProduct_Click(object sender, RoutedEventArgs e)
        {
            if (ProductListBox.SelectedItem is CreateProductDTO selectedProduct)
            {
                products.Remove(selectedProduct);
            }
            else
            {
                MessageBox.Show("Wybierz produkt do usunięcia.");
            }
        }

        private void LoadCategories()
        {
            TransactionCategory.ItemsSource = Enum.GetValues(typeof(Category)).Cast<Category>();
        }

    }
}


