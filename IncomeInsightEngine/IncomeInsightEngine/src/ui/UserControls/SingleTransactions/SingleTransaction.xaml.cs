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

namespace IncomeInsightEngine.src.ui.UserControls.SingleTransactions
{
    /// <summary>
    /// Interaktionslogik für SingleTransaktion7.xaml
    /// </summary>
    public partial class SingleTransaction : UserControl
    {
        public Transaction transaction { get; }

        public SingleTransaction()
        {
            InitializeComponent();
        }


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
            this.descriptionTextBox.Text = description;
        }

        private void SetAmount(decimal amount)
        {
            this.amountLabel.Content = amount.ToString();

            if (amount > 0)
            {
                this.amountBackground.Background = (Brush)new BrushConverter().ConvertFrom("#FF7B947A");
                
            }
            else if (amount < 0)
            {
                this.amountBackground.Background = (Brush)new BrushConverter().ConvertFrom("#FF8C5A5A");

            }
            else
            {
                this.amountBackground.Background = Brushes.White;
            }

        }

        private void settingsButton_Click(object sender, RoutedEventArgs e)

        {
            transaction.DisplayShortTransactionDetails();
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

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            
            this.Height += 4;
            this.Margin = new Thickness(0,3,0,3);
            this.amountShadow.ShadowDepth += 2;
            this.dateShadow.ShadowDepth += 2;
            this.descriptionShadow.ShadowDepth += 2;
            this.partnerShadow.ShadowDepth += 2;
            this.settingsShadow.ShadowDepth += 2;

            this.amountLabel.FontSize += 2;
            this.dateLabel.FontSize += 2;
            this.descriptionTextBox.FontSize += 2;
            this.partnerLabel.FontSize += 2;
            
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Height -= 4;
            this.Margin = new Thickness(0, 5, 0, 5);
            this.amountShadow.ShadowDepth -= 2;
            this.dateShadow.ShadowDepth -= 2;
            this.descriptionShadow.ShadowDepth -= 2;
            this.partnerShadow.ShadowDepth -= 2;
            this.settingsShadow.ShadowDepth -= 2;

            this.amountLabel.FontSize -= 2;
            this.dateLabel.FontSize -= 2;
            this.descriptionTextBox.FontSize -= 2;
            this.partnerLabel.FontSize -= 2;

        }

    }
}
