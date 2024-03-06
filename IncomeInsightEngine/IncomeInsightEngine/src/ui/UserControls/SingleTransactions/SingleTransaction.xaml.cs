using dataStructure;
using IncomeInsightEngine.Resources.Colors;
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

            SetColors();

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
                this.amountBackground.Background = UiColorsManager.GetBrush(UiColorsManager.ColorName.LaurelGreen);
                
            }
            else if (amount < 0)
            {
                this.amountBackground.Background = UiColorsManager.GetBrush(UiColorsManager.ColorName.MediumCarmine);

            }
            else
            {
                this.amountBackground.Background = UiColorsManager.GetBrush(UiColorsManager.ColorName.White);
            }

        }

        private void SetColors()
        {

            amountBackground.Background = UiColorsManager.GetBrush(UiColorsManager.ColorName.White);
            dateBackground.Background = UiColorsManager.GetBrush(UiColorsManager.ColorName.White);
            partnerBackground.Background = UiColorsManager.GetBrush(UiColorsManager.ColorName.White);
            descriptionBackground.Background = UiColorsManager.GetBrush(UiColorsManager.ColorName.White);
            settingsBackground.Background = UiColorsManager.GetBrush(UiColorsManager.ColorName.White);

            amountLabel.Foreground = UiColorsManager.GetBrush(UiColorsManager.ColorName.Black);
            dateLabel.Foreground = UiColorsManager.GetBrush(UiColorsManager.ColorName.Black);
            partnerLabel.Foreground = UiColorsManager.GetBrush(UiColorsManager.ColorName.Black);
            descriptionTextBox.Foreground = UiColorsManager.GetBrush(UiColorsManager.ColorName.LightGray);


            string imageSource = (UiColorsManager.IsLightMode) ? "pack://application:,,,/Resources/Images/Icons/settings-2-Black.png" : "pack://application:,,,/Resources/Images/Icons/settings-2-White.png";
            settingsButton.Background = new ImageBrush(new BitmapImage(new Uri(imageSource)));

        }

        private void settingsButton_Click(object sender, RoutedEventArgs e)

        {
            transaction.DisplayShortTransactionDetails();
        }

        private void settingsButton_MouseEnter(object sender, MouseEventArgs e)
        {
            settingsBackground.Background = UiColorsManager.GetBrush(UiColorsManager.ColorName.DarkGrey);
        }

        private void settingsButton_MouseLeave(object sender, MouseEventArgs e)
        {
            settingsBackground.Background = UiColorsManager.GetBrush(UiColorsManager.ColorName.White);
        }

        private void settingsButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            settingsBackground.Background = UiColorsManager.GetBrush(UiColorsManager.ColorName.LightGray);
        }

        private void settingsButton_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            settingsBackground.Background = UiColorsManager.GetBrush(UiColorsManager.ColorName.DarkGrey);
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
