using dataStructure;
using IncomeInsightEngine.src.dataStructure.management;
using IncomeInsightEngine.src.ui.CustomUiElements;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace IncomeInsightEngine.src.ui.externalFrames
{
    public class ManualTransactionEntryWindow : Window
    {
        private TransactionManager transactionManager;

        Grid grid = new Grid();

        AdvancedInputBox amountTextbox = new AdvancedInputBox();
        AdvancedInputBox dateTextbox = new AdvancedInputBox();
        AdvancedInputBox partnerTextbox = new AdvancedInputBox();
        AdvancedInputBox descriptionTextbox = new AdvancedInputBox();

        ComboBox statusComboBox = new ComboBox();

        Label amountLabel = new Label();
        Label dateLabel = new Label();
        Label partnerLabel = new Label();
        Label statusLabel = new Label();
        Label descriptionLabel = new Label();


        public ManualTransactionEntryWindow(Window owner, TransactionManager transactionManager)
        {
            this.Owner = owner;
            this.transactionManager = transactionManager;

            this.Height = 300;
            this.Width = 500;

            this.WindowStartupLocation = (owner != null ) ? WindowStartupLocation.CenterOwner : WindowStartupLocation.CenterScreen;

            InitGrid();
            InitComponents();
          
        }


        private void InitGrid()
        {
            for (int i = 0; i < 2; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            }

            for (int i = 0; i < 10; i++)
            {
                var rowDefinition = new RowDefinition() { Height = GridLength.Auto };
                grid.RowDefinitions.Add(rowDefinition);
            }

            this.Content = grid;
        }

        private void InitComponents()
        {         
            amountLabel.Content = "Amount:";
            dateLabel.Content = "Date:";
            partnerLabel.Content = "Partner:";
            statusLabel.Content = "Status:";
            descriptionLabel.Content = "Description:";

            FormatDesignForLabel(amountLabel);
            FormatDesignForLabel(dateLabel); 
            FormatDesignForLabel(partnerLabel); 
            FormatDesignForLabel(statusLabel); 
            FormatDesignForLabel(descriptionLabel);

            FormatDesignForTextBox(amountTextbox);
            FormatDesignForTextBox(dateTextbox); 
            FormatDesignForTextBox(partnerTextbox); 
            FormatDesignForTextBox(descriptionTextbox);
           
            AddLabelAndTextBoxToGrid(amountLabel, amountTextbox, 0,0);
            AddLabelAndTextBoxToGrid(dateLabel, dateTextbox, 0,1);
            AddLabelAndTextBoxToGrid(partnerLabel, partnerTextbox, 2,0);
            AddLabelAndTextBoxToGrid(descriptionLabel, descriptionTextbox,4,0, true);


            FormatDesignForComboBox(statusComboBox);
            AddElementsToCombobox(statusComboBox,new List<string> { "Completed", "Pending" });
            AddLabelAndComboBoxToGrid(statusLabel, statusComboBox, 2,1);


            Button addMoreDetails = new Button { Content = "Add more Details" };
            addMoreDetails.Click += AddMoreDetails_Click;
            FormatDesignForButton(addMoreDetails, new Thickness(20,20,20,10));
            AddButtonToGrid(addMoreDetails, 6,0, true);

            Button cancel = new Button { Content = "Cancel" };
            cancel.Click += Cancel_Click;
            FormatDesignForButton(cancel, new Thickness(20, 10, 20, 20));
            AddButtonToGrid(cancel, 7, 0);

            Button finish = new Button { Content = "Finish" };
            finish.Click += Finish_Click;
            FormatDesignForButton(finish, new Thickness(20, 10, 20, 20));
            AddButtonToGrid(finish, 7, 1);

        }

        private void Finish_Click(object sender, RoutedEventArgs e)
        {
            var transaction = new Transaction
            {
                Amount = amountTextbox.TryGetNumberAsDecimal(),
                Date = dateTextbox.TryGetDate(),
                Partner = partnerTextbox.Text,
                Status = statusComboBox.SelectedItem.ToString(),
                Description = descriptionTextbox.Text
            };

            transactionManager.AddTransaction(transaction);
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddMoreDetails_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void FormatDesignForTextBox(AdvancedInputBox textBox)
        {
            textBox.FontFamily = new FontFamily("Segoe UI");
            textBox.FontSize = 12;
            textBox.Foreground = new SolidColorBrush(Colors.Black);
            textBox.Background = new SolidColorBrush(Colors.WhiteSmoke);
            textBox.BorderBrush = new SolidColorBrush(Colors.Black);
            textBox.BorderThickness = new Thickness(1);
            textBox.Margin =  new Thickness(20, 0, 20, 0) ;
            textBox.MinHeight = 20;

        }

        private void FormatDesignForComboBox(ComboBox comboBox)
        {
            comboBox.FontFamily = new FontFamily("Segoe UI");
            comboBox.FontSize = 12;
            comboBox.Foreground = new SolidColorBrush(Colors.Black);
            comboBox.Background = new SolidColorBrush(Colors.WhiteSmoke);
            comboBox.BorderBrush = new SolidColorBrush(Colors.Black);
            comboBox.BorderThickness = new Thickness(1);
            comboBox.Margin = new Thickness(20, 0, 20, 0);
            comboBox.MinHeight = 20;

        }

        private void FormatDesignForLabel(Label label)
        {
          
            label.FontFamily = new FontFamily("Segoe UI");
            label.FontSize = 12;
            label.Foreground = new SolidColorBrush(Colors.Black);       
            label.Background = new SolidColorBrush(Colors.Transparent);
            label.Margin = new Thickness(15, 0, 20, 0);
            label.VerticalAlignment = VerticalAlignment.Bottom;
     
        }

        private void FormatDesignForButton(Button button, Thickness margin)
        {
           
            button.FontFamily = new FontFamily("Segoe UI");
            button.FontSize = 12;   
            button.MinHeight = 20;       
            button.Foreground = new SolidColorBrush(Colors.White);
            button.Background = new SolidColorBrush(Color.FromArgb(255, 0, 120, 215)); // Beispiel: Dunkelblau
            button.BorderBrush = new SolidColorBrush(Colors.Black);
            button.BorderThickness = new Thickness(2);
            button.Padding = new Thickness(10, 5, 10, 5); 
            button.Margin = margin;
            button.HorizontalContentAlignment = HorizontalAlignment.Center;
            button.VerticalContentAlignment = VerticalAlignment.Center;
            button.Effect = new DropShadowEffect
            {
                Color = Colors.Gray,
                Direction = 320,
                ShadowDepth = 5,
                Opacity = 0.5,
                BlurRadius = 5
            };
        }

        private void AddLabelAndTextBoxToGrid(Label label, AdvancedInputBox textBox, int row, int column, bool large = false)
        {
            if (large)
            {
                Grid.SetColumnSpan(label, 2);
                Grid.SetColumnSpan(textBox, 2);
            }

            Grid.SetRow(label, row);
            Grid.SetColumn(label, column);
            grid.Children.Add(label);

            Grid.SetRow(textBox,  row+1);
            Grid.SetColumn(textBox, column);
            grid.Children.Add(textBox);
        }

        private void AddLabelAndComboBoxToGrid(Label label, ComboBox comboBox, int row, int column, bool large = false)
        {
            if (large)
            {
                Grid.SetColumnSpan(label, 2);
                Grid.SetColumnSpan(comboBox, 2);
            }

            Grid.SetRow(label, row);
            Grid.SetColumn(label, column);
            grid.Children.Add(label);

            Grid.SetRow(comboBox, row + 1);
            Grid.SetColumn(comboBox, column);
            grid.Children.Add(comboBox);
        }

        private void AddButtonToGrid(Button button, int row, int column, bool large = false)
        {
            if (large)
            {
                Grid.SetColumnSpan(button, 2);

            }

            Grid.SetRow(button, row);
            Grid.SetColumn(button, column);
            grid.Children.Add(button);
        }

       private void AddElementsToCombobox(ComboBox comboBox, List<string> items)
        {
            foreach (string item in items)
            {
                comboBox.Items.Add(item);
            }
            comboBox.SelectedIndex = 0;
        }


    }
}
