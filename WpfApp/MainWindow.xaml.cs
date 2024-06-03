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
        private ObservableCollection<CreateProductDTO> products;
        private DatabaseSingleton db;

        public MainWindow()
        {
            InitializeComponent();
            LoadCategories();
            db = DatabaseSingleton.Instance;

            products = new ObservableCollection<CreateProductDTO>();
            ProductListBox.ItemsSource = products;
   
            TransactionDetailsListBox.ItemsSource = null;
            BudgetListBox.ItemsSource = db.Budgets;
        }

        private void btnShowCreateBudgetForm_Click(object sender, RoutedEventArgs e)
        {
                BudgetForm.Visibility = Visibility.Visible;
                StatsField.Visibility = Visibility.Collapsed;
                TransactionForm.Visibility = Visibility.Collapsed;
                ModifyTransactionPanel.Visibility = Visibility.Collapsed;
        }

        private void btnAddBudget_Click(object sender, RoutedEventArgs e)
        {
            if (!decimal.TryParse(MonthSalary.Text, out decimal salary))
            {
                MessageBox.Show("Zarobek należy wpisać jako liczbę");
            }
            var service = new BudgetServices();
            var newBudget = new CreateBudgetDTO 
            { 
                Name = BudgetName.Text,
                MonthSalary = salary 
            };

            try
            {
                service.Create(newBudget);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }


            BudgetListBox.Items.Refresh();
            MessageBox.Show("Stworzono budżet");

            BudgetName.Clear();
            MonthSalary.Clear();
            BudgetForm.Visibility = Visibility.Collapsed;
        }

        private void btnRemoveBudget_Click(object sender, RoutedEventArgs e)
        {
            if (BudgetListBox.SelectedItem is Budget selectedBudget)
            {
                TransactionsListBox.SelectedItem = null;
                var service = new BudgetServices();

                try
                {
                    service.Remove(selectedBudget.Id);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }

                BudgetListBox.Items.Refresh();
                StatsField.Visibility = Visibility.Collapsed;
                TransactionForm.Visibility = Visibility.Collapsed;
                ModifyTransactionPanel.Visibility = Visibility.Collapsed;
                MessageBox.Show("Pomyślnie usunięto budżet.");
            }
            else
            {
                MessageBox.Show("Wybierz budżet do usunięcia.");
            }
        }

        private void btnShowStats_Click(object sender, RoutedEventArgs e)
        {
            if (BudgetListBox.SelectedItem is Budget selectedBudget)
            {
                if (StatsField.Visibility == Visibility.Collapsed)
                {
                    BudgetForm.Visibility = Visibility.Collapsed;
                    TransactionForm.Visibility = Visibility.Collapsed;
                    StatsField.Visibility = Visibility.Visible;
                    TransactionsListBox.ItemsSource = selectedBudget.Transactions;
                }
            }
            else
            {
                MessageBox.Show("Musisz zaznczayć budżet, dla którego chcesz zobaczyć statystyki");
            }
        }

        private void btnShowCreateTransactionForm_Click(object sender, RoutedEventArgs e)
        {
            if (TransactionForm.Visibility == Visibility.Collapsed)
            {
                TransactionForm.Visibility = Visibility.Visible;
                BudgetForm.Visibility = Visibility.Collapsed;
                StatsField.Visibility = Visibility.Collapsed;
                ModifyTransactionPanel.Visibility = Visibility.Collapsed;
            }
        }
        private void btnShowCreateTransactionPanel_Click(object sender, SelectionChangedEventArgs e)
        {
            if (BudgetListBox.SelectedItem != null)
            {
                CreateTransactionPanel.Visibility = Visibility.Visible;
                BudgetForm.Visibility = Visibility.Collapsed;
            }
        }


        private void btnCreateTransaction_Click(object sender, RoutedEventArgs e)
        {
            if (BudgetListBox.SelectedItem is Budget selectedBudget)
            {
                var service = new BudgetServices();

                DateTime? parsedDate = string.IsNullOrEmpty(TransactionDate.Text) ?
                     (DateTime?)null :
                     DateTime.Parse(TransactionDate.Text);

                var newTransaction = new CreateTransactionDTO 
                { 
                     Name = TransactionName.Text, 
                     FromDate = parsedDate, 
                     Category = (Category)TransactionCategory.SelectedItem,
                     Products = products.ToList(),
                };
                // todo zrobic tutaj string i dopiero w service sprawdzac date
                try
                {
                    service.AddTransaction(newTransaction, selectedBudget.Id);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }

                MessageBox.Show("Transakcja została zapisana");

                TransactionForm.Visibility = Visibility.Collapsed;
                BudgetForm.Visibility = Visibility.Collapsed;
                TransactionsListBox.Items.Refresh();
                products.Clear();
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

                var service = new BudgetServices();
                try
                {
                    service.RemoveTransaction(selectedTransaction.Id, selectedBudget.Id);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }

                TransactionsListBox.Items.Refresh();
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
                TransactionDetailsListBox.ItemsSource = selectedTransaction.Products;
                ModifyTransactionPanel.Visibility = Visibility.Visible;
                TransactionDetailsListBox.Visibility = Visibility.Visible;
            }
        }

        private void btnAddProduct_Click(object sender, RoutedEventArgs e)
        {
           if (decimal.TryParse(ProductPrice.Text, out decimal parsedPrice) && uint.TryParse(ProductQuantity.Text, out uint parsedQuantity))
           {
              var newProduct = new CreateProductDTO 
              { 
                 Name = ProductName.Text,
                 Count = parsedQuantity, 
                 Price = parsedPrice 
              };

                try
                {
                    products.Add(newProduct);
                }   
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }

                ProductName.Clear();
                ProductPrice.Clear();
                ProductQuantity.Clear();
                ProductListBox.Items.Refresh();
           }
           else
           {
                MessageBox.Show("Ilość oraz cenę należy wpisać jako liczbę");
           }
        }

        private void btnRemoveProduct_Click(object sender, RoutedEventArgs e)
        {
            if (ProductListBox.SelectedItem is CreateProductDTO selectedProduct)
            {
                products.Remove(selectedProduct);
                ProductListBox.Items.Refresh();
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