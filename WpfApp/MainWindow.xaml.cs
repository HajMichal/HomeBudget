using System;
using System.Collections.ObjectModel;
using System.Linq;
using Core.DTOs;
using Core.Models;
using Core.Services;

namespace WpfApp
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<CreateProductDTO> products;
        private DatabaseSingleton db;
        private IBudgetService budgetService;

        public MainWindow()
        {
            InitializeComponent();
            LoadCategories();
            db = DatabaseSingleton.Instance;
            budgetService = new BudgetServices();

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
                TransactionDetailsListBox.Visibility = Visibility.Collapsed;
                TransactionsListBox.SelectedIndex = -1;
        }

        private void btnAddBudget_Click(object sender, RoutedEventArgs e)
        {
            if (!decimal.TryParse(MonthSalary.Text, out decimal salary))
            {
                MessageBox.Show("Zarobek należy wpisać jako liczbę");
            }
            
            var newBudget = new CreateBudgetDTO 
            { 
                Name = BudgetName.Text,
                MonthSalary = salary 
            };

            try
            {
                budgetService.Create(newBudget);
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

                try
                {
                    budgetService.Remove(selectedBudget.Id);
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
                TransactionDetailsListBox.Visibility = Visibility.Collapsed;
                TransactionsListBox.SelectedIndex = -1;

                Salary.Text = null;
                Expenses.Text = null;
                Balance.Text = null;

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
                TransactionDetailsListBox.Visibility = Visibility.Collapsed;
                TransactionsListBox.SelectedIndex = -1;
            }
        }
        private void btnShowCreateTransactionPanel_Click(object sender, SelectionChangedEventArgs e)
        {
            if (BudgetListBox.SelectedItem is Budget selectedBudget)
            {
                CreateTransactionPanel.Visibility = Visibility.Visible;
                BudgetForm.Visibility = Visibility.Collapsed;
                
                Salary.Text = selectedBudget.MonthSalary.ToString();
                Expenses.Text = selectedBudget.MonthExpenses.ToString();
                Balance.Text = selectedBudget.Balance.ToString();
            }
        }


        private void btnCreateTransaction_Click(object sender, RoutedEventArgs e)
        {
            if (BudgetListBox.SelectedItem is Budget selectedBudget)
            {
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
                
                try
                {
                    budgetService.AddTransaction(newTransaction, selectedBudget.Id);
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

                Expenses.Text = selectedBudget.MonthExpenses.ToString();
                Balance.Text = selectedBudget.Balance.ToString();
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
                TransactionDetailsListBox.Visibility = Visibility.Collapsed;
                TransactionDetailsListBox.ItemsSource = null;
                
                try
                {
                    budgetService.RemoveTransaction(selectedTransaction.Id, selectedBudget.Id);
                    Expenses.Text = selectedBudget.MonthExpenses.ToString();
                    Balance.Text = selectedBudget.Balance.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }

                TransactionsListBox.Items.Refresh();
                TransactionsListBox.SelectedIndex = -1;
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