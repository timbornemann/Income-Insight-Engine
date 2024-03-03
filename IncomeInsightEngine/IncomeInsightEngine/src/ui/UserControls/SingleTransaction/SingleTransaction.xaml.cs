using dataStructure;
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

namespace IncomeInsightEngine.src.ui.UserControls
{
    /// <summary>
    /// Interaktionslogik für SingleTransaction.xaml
    /// </summary>
    public partial class SingleTransaction : UserControl
    {
        public Transaction transaction { get; }

      

        public SingleTransaction(Transaction transaction)
        {
            InitializeComponent();

            this.transaction = transaction;
            this.dateLabel.Content = transaction.Date.ToShortDateString();
            this.partnerLabel.Content = transaction.Partner;
            SetDescriptionText(transaction.Description);
            SetAmount(transaction.Amount);

        }

        private void SetDescriptionText(string description)
        {
            this.descriptionTextBox.Document.Blocks.Clear();
            Paragraph paragraph = new Paragraph();
            paragraph.Inlines.Add(new Run(description));
            this.descriptionTextBox.Document.Blocks.Add(paragraph);
        }

        private void SetAmount(decimal amount)
        {
            this.amountLabel.Content = amount.ToString();

            if (amount > 0)
            {
                this.amoutBackground.Background = (Brush) new BrushConverter().ConvertFrom("#FF7B947A");
            }
            else if(amount < 0)
            {
                this.amoutBackground.Background = (Brush)new BrushConverter().ConvertFrom("#FF8C5A5A");

            }
            else
            {
                this.amoutBackground.Background = Brushes.White;
            }

        }


        private void settingsButton_Click(object sender, RoutedEventArgs e)

        {
           
            Console.WriteLine("Button Click");
        }

        private void settingsButton_MouseEnter(object sender, MouseEventArgs e)
        {
            settingsBackground.Background = Brushes.DarkGray;
        }

        private void settingsButton_MouseLeave(object sender, MouseEventArgs e)
        {
            settingsBackground.Background = Brushes.White;
        }

        private void settingsButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            settingsBackground.Background = Brushes.Gray;
        }

        private void settingsButton_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            settingsBackground.Background = Brushes.DarkGray;
        }
    }
}
