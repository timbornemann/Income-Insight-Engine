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

namespace IncomeInsightEngine.src.ui.internalFrames
{
    /// <summary>
    /// Interaktionslogik für TransactionOverview.xaml
    /// </summary>
    public partial class TransactionOverview : UserControl
    {
        public TransactionOverview()
        {
            InitializeComponent();
        }

        public void AddElementToOverview(UserControl userControl)
        {
           this.panel.Children.Add(userControl);
        }


    }
}
