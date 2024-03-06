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
            InitializeLists();


        }

        private void InitializeLists()
        {
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
        }

        public void AddElementToOverview(UserControl userControl)
        {
            overviewPanel.Children.Add(userControl);
        }

        private void AddTransactionsToView(IEnumerable<Transaction> transactions)
        {
            foreach (Transaction transaction in transactions)
            {
                var transactionUiElement = new SingleTransaction(transaction);
                overviewPanel.Children.Add(transactionUiElement);
            }
        }

        public void ChangeOrder()
        {




        }

    }
}
