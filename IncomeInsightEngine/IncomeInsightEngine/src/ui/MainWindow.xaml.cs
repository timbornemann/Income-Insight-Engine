using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IncomeInsightEngine.Properties;
using System.Windows;
using System.Windows.Input;
using IncomeInsightEngine.src.dataStructure.management;
using IncomeInsightEngine.src.ui.UserControls.SingleTransactions;
using dataStructure;
using IncomeInsightEngine.src.ui.UserControls;




namespace IncomeInsightEngine.src.ui
{
    public partial class MainWindow : Window
    {
        public TransactionManager Manager { get; }


        public MainWindow(TransactionManager manager)
        {
            InitializeComponent();

            Manager = manager;

            
            foreach (Transaction transaction in manager.GetAllTransactions())
            {
                var t = new SingleTransaction(transaction);                            
                this.overview.AddElementToOverview(t);
            }
            

        }

    }
}
