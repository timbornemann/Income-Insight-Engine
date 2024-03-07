using System;
using System.Collections.Generic;
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
using dataStructure;
using IncomeInsightEngine.Properties;
using IncomeInsightEngine.src.dataStructure.management;
using IncomeInsightEngine.src.ui.UserControls.SingleTransactions;
using IncomeInsightEngine.src.ui.CustomUiElements;

namespace IncomeInsightEngine.src.ui.internalFrames
{
    /// <summary>
    /// Interaktionslogik für TransactionOverview.xaml
    /// </summary>
    public partial class TransactionOverview : UserControl
    {
        public TransactionManager transactionManager { get; }


        public TransactionOverview(TransactionManager transactionManager)
        {
            this.transactionManager = transactionManager;

            InitializeComponent();
            InitializeTexts();
            InitializeAdvancedInputBoxes();

        }

        private void InitializeTexts()
        {
            sortingLabel.Text = Strings.SortingOptions;

            orderComboBox.Items.Clear();
            orderComboBox.Items.Add(Strings.Ascending);
            orderComboBox.Items.Add(Strings.Descending);
            orderComboBox.SelectedIndex = 0;


            List<string> sortOptions = new List<string>()
            {
                Properties.Strings.SortById,
                Properties.Strings.SortByDate,
                Properties.Strings.SortByAmount,
                Properties.Strings.SortByPartner,
                Properties.Strings.SortByLocation
            };

            sortingTypes.ItemsSource = sortOptions;
            sortingTypes.SelectedIndex = 0;


            filterLabel.Text = Strings.FilterOptions;

            incomeExpaseComboBox.Items.Clear();
            incomeExpaseComboBox.Items.Add(Strings.All);
            incomeExpaseComboBox.Items.Add(Strings.Income);
            incomeExpaseComboBox.Items.Add(Strings.Expenses);
            incomeExpaseComboBox.SelectedIndex = 0;

            filterButton.Content = Strings.Apply;
            resetFilterButton.Content = Strings.Reset;

            currencyLabel.Content = Strings.Currency;

            currencyComboBox.Items.Insert(0, Strings.Blank);

            paymentMethodLabel.Content = Strings.PaymentMethod;

            List<string> paymentMethods = new List<string>
        {
            Strings.Blank,
            Strings.CreditCard,
            Strings.DebitCard,
            Strings.PayPal,
            Strings.BankTransfer,
            Strings.Invoice,
            Strings.DirectDebit,
            Strings.Cash,
            Strings.Voucher,
            Strings.Cryptocurrency
        };

            paymentMethodComboBox.ItemsSource = paymentMethods;


            taxDeductible.Content = Strings.TaxDeductible;
            reimbursable.Content = Strings.Reimbursable;

            categoryLabel.Content = Strings.Category;

            List<string> categorys = new List<string>
        {
            Strings.Blank,

        };
            categoryComboBox.ItemsSource = categorys;


            budgetCategoryLabel.Content = Strings.BudgetCategory;

            List<string> budgetCategorys = new List<string>
        {
            Strings.Blank,

        };
            budgetCategoryComboBox.ItemsSource = budgetCategorys;


            statusLabel.Content = Strings.Status;

            List<string> states = new List<string>
        {
            Strings.Blank,

        };
            budgetCategoryComboBox.ItemsSource = states;


            priorityLabel.Content = Strings.Priority;

            List<string> priorities = new List<string>
        {
            Strings.Blank,

        };
            priorityComboBox.ItemsSource = priorities;

            frequencyLabel.Content = Strings.Priority;

            List<string> frequencies = new List<string>
        {
            Strings.Blank,

        };
            frequencyComboBox.ItemsSource = frequencies;


        }



        private void InitializeAdvancedInputBoxes()
        {
            minAmount.InitAdvancedInputBox(null, null, Strings.MinAmount);
            maxAmount.InitAdvancedInputBox(null, null, Strings.MaxAmount);
            description.InitAdvancedInputBox(null, null, Strings.Description);
            partner.InitAdvancedInputBox(null, null, Strings.Partner);
            project.InitAdvancedInputBox(null, null, Strings.Project);
            location.InitAdvancedInputBox(null, null, Strings.Location);
            tag.InitAdvancedInputBox(null, null, Strings.Tags);
            notes.InitAdvancedInputBox(null, null, Strings.Notes);
        }

        public void AddElementToOverview(UserControl userControl)
        {
            overviewPanel.Children.Add(userControl);
        }

        public void AddTransactionsToView(IEnumerable<Transaction> transactions)
        {
            overviewPanel.Children.Clear();

            foreach (Transaction transaction in transactions)
            {
                var transactionUiElement = new SingleTransaction(transaction);
                overviewPanel.Children.Add(transactionUiElement);
            }
        }

        private async Task ChangeOrderAsync()
        {
            int sortingTypeSelectedIndex = sortingTypes.SelectedIndex;
            int orderComboBoxSelectedIndex = orderComboBox.SelectedIndex;

            overviewPanel.Children.Clear();

            IEnumerable<Transaction> transactions = Enumerable.Empty<Transaction>();

            await Task.Run(() =>
            {
                switch (sortingTypeSelectedIndex)
                {
                    case 0:
                        transactions = orderComboBoxSelectedIndex == 0
                            ? transactionManager.SortTransactionsByIdAscending()
                            : transactionManager.SortTransactionsByIdDescending();
                        break;
                    case 1:
                        transactions = orderComboBoxSelectedIndex == 0
                            ? transactionManager.SortTransactionsByDateAscending()
                            : transactionManager.SortTransactionsByDateDescending();
                        break;
                    case 2:
                        transactions = orderComboBoxSelectedIndex == 0
                            ? transactionManager.SortTransactionsByAmountAscending()
                            : transactionManager.SortTransactionsByAmountDescending();
                        break;
                    case 3:
                        transactions = orderComboBoxSelectedIndex == 0
                            ? transactionManager.SortTransactionsByPartnerAscending()
                            : transactionManager.SortTransactionsByPartnerDescending();
                        break;
                    case 4:
                        transactions = orderComboBoxSelectedIndex == 0
                            ? transactionManager.SortTransactionsByLocationAscending()
                            : transactionManager.SortTransactionsByLocationDescending();
                        break;
                    default:
                        break;
                }
            });

            Dispatcher.Invoke(() =>
            {
                foreach (var transaction in transactions)
                {
                    var transactionUiElement = new SingleTransaction(transaction);
                    overviewPanel.Children.Add(transactionUiElement);
                }
            });
        }


        private async void sortingTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await ChangeOrderAsync();
        }

        private async void orderComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await ChangeOrderAsync();
        }

        private async void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double width = overviewPanel.ActualWidth;

            await Task.Run(() =>{
                Dispatcher.Invoke(() =>
                {
                    foreach (SingleTransaction transaction in overviewPanel.Children)
                    {
                        transaction.Width = width;
                    }
                });
            });
        }

    }
}
