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
        public TransactionManager manager { get; }

        private IEnumerable<Transaction> usedTransactions = null;

        public TransactionOverview(TransactionManager transactionManager)
        {
            this.manager = transactionManager;

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

            Dispatcher.Invoke(() =>
            {
            double width = overviewPanel.ActualWidth;
            overviewPanel.Children.Clear();
                foreach (var transaction in transactions)
                {
                    var transactionUiElement = new SingleTransaction(transaction);
                    transactionUiElement.Width = width;
                    overviewPanel.Children.Add(transactionUiElement);
                }
            });
        }

        private async Task ChangeOrderAsync()
        {
            int sortingTypeSelectedIndex = sortingTypes.SelectedIndex;
            int orderComboBoxSelectedIndex = orderComboBox.SelectedIndex;

            overviewPanel.Children.Clear();

            IEnumerable<Transaction> transactions = Enumerable.Empty<Transaction>();

            if(usedTransactions == null)
            {
                usedTransactions = manager.GetAllTransactions();
            }

            await Task.Run(() =>
            {
                switch (sortingTypeSelectedIndex)
                {
                    case 0:
                        transactions = orderComboBoxSelectedIndex == 0
                            ? manager.SortTransactionsByIdAscending(usedTransactions)
                            : manager.SortTransactionsByIdDescending(usedTransactions);
                        break;
                    case 1:
                        transactions = orderComboBoxSelectedIndex == 0
                            ? manager.SortTransactionsByDateAscending(usedTransactions)
                            : manager.SortTransactionsByDateDescending(usedTransactions);
                        break;
                    case 2:
                        transactions = orderComboBoxSelectedIndex == 0
                            ? manager.SortTransactionsByAmountAscending(usedTransactions)
                            : manager.SortTransactionsByAmountDescending(usedTransactions);
                        break;
                    case 3:
                        transactions = orderComboBoxSelectedIndex == 0
                            ? manager.SortTransactionsByPartnerAscending(usedTransactions)
                            : manager.SortTransactionsByPartnerDescending(usedTransactions);
                        break;
                    case 4:
                        transactions = orderComboBoxSelectedIndex == 0
                            ? manager.SortTransactionsByLocationAscending(usedTransactions)
                            : manager.SortTransactionsByLocationDescending(usedTransactions);
                        break;
                    default:
                        break;
                }
            });

            AddTransactionsToView(transactions);
        }


        private async void sortingTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await ChangeOrderAsync();
        }

        private async void orderComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await ChangeOrderAsync();
        }


        private async Task filterTransactions()
        {

            overviewPanel.Children.Clear();
            IEnumerable<Transaction> tempTransactions = null;

            int incomeExpaseComboBoxSelectedIndex = incomeExpaseComboBox.SelectedIndex;

            await Task.Run(() =>
            {

                // all or income or expense

                if(incomeExpaseComboBoxSelectedIndex == 0)
                {
                    tempTransactions = manager.GetAllTransactions();
                }
                else if (incomeExpaseComboBoxSelectedIndex == 1)
                {
                    tempTransactions = manager.GetIncomeTransactions();
                }
                else if(incomeExpaseComboBoxSelectedIndex == 2)
                {
                    tempTransactions = manager.GetExpenseTransactions();
                }

                // amount
                decimal tempmin = 0;
                decimal tempmax = 0;
                Dispatcher.Invoke(() =>
                {
                     tempmin = minAmount.TryGetNumberAsDecimal();
                     tempmax = maxAmount.TryGetNumberAsDecimal();
                });


                if(tempmin > tempmax)
                {
                    tempTransactions = manager.GetTransactionsByAmount(tempmax, tempmin, tempTransactions);
                }
                else if(tempmax> tempmin)
                {
                    tempTransactions = manager.GetTransactionsByAmount(tempmin, tempmax, tempTransactions);
                }
                else
                {
                    tempTransactions = manager.GetTransactionsByAmount(tempmin, tempTransactions);

                }

                // date
                DateTime? tempStartDate = null;
                DateTime? tempEndDate = null;

                Dispatcher.Invoke(() =>
                {
                    tempStartDate = startDate.SelectedDate;
                    tempEndDate = endDate.SelectedDate;
                });

                DateTime start = tempStartDate ?? DateTime.MinValue;
                DateTime end = tempEndDate ?? DateTime.MaxValue;

                if (start > end)
                {
                    tempTransactions = manager.GetTransactionsByDate(end, start, tempTransactions);
                }
                else
                { 
                    tempTransactions = manager.GetTransactionsByDate(start, end, tempTransactions);
                }

                //description
                string tempDescription = null;
                Dispatcher.Invoke(() =>
                {
                    tempDescription = description.TryGetText();
                });

                if(tempDescription != null)
                {
                tempTransactions = manager.GetTransactionsByDescription(tempDescription, tempTransactions);
                }

                //Currency

                //Payment Method

                //taxDeductible

                //reimbursable

                //Category

                //Budget Category

                //partner
                string temppartner = null;
                Dispatcher.Invoke(() =>
                {
                    temppartner = partner.TryGetText();
                });

                if (temppartner != null)
                {
                    tempTransactions = manager.GetTransactionsByPartner(temppartner, tempTransactions);
                }

                //project

                //Status

                //Priority

                //Frequency

                //location

                //tag

                //notes

            });

            usedTransactions = tempTransactions;
            AddTransactionsToView(tempTransactions);
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

        private void resetFilterButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void filterButton_Click(object sender, RoutedEventArgs e)
        {
            await filterTransactions();
        }
    }
}
