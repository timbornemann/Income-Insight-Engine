using dataStructure;
using IncomeInsightEngine.Properties;
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
    /// <summary>
    /// Provides a window for manually entering transaction details into the TransactionManager.
    /// </summary>
    /// <remarks>
    /// <para>The ManualTransactionEntryWindow class is a user interface component that extends the Window class, 
    /// designed to capture and submit transaction data to a TransactionManager. 
    /// It features a dynamic layout with input fields for transaction properties such as amount, 
    /// date, partner, status, and description. The window supports advanced features like:</para>
    /// </remarks>
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

        /// <summary>
        /// Initializes a new instance of the ManualTransactionEntryWindow with specified owner, transaction manager, and optional configurations.
        /// </summary>
        /// <param name="owner">The parent window of this dialog, used for centering and modal behavior.</param>
        /// <param name="transactionManager">The transaction manager for adding transactions upon submission.</param>
        /// <remarks>
        /// This constructor sets up the window with a predefined size and startup location, 
        /// and initializes the UI components including a grid layout and various input controls for capturing transaction details.
        /// </remarks>
        public ManualTransactionEntryWindow(Window owner, TransactionManager transactionManager)
        {
            this.Owner = owner;
            this.transactionManager = transactionManager;

            this.Height = 300;
            this.Width = 500;

            this.WindowStartupLocation = (owner != null) ? WindowStartupLocation.CenterOwner : WindowStartupLocation.CenterScreen;

            InitGrid();
            InitComponents();

        }

        /// <summary>
        /// Initializes the grid layout for arranging UI components within the window.
        /// </summary>
        /// <remarks>
        /// This method sets up a grid with two columns and multiple rows to organize labels, textboxes, a combobox, and buttons for user input.
        /// </remarks>
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

        /// <summary>
        /// Initializes and configures UI components including labels, textboxes, a combobox, and buttons.
        /// </summary>
        /// <remarks>
        /// This method configures the visual appearance and behavior of input controls and adds them to the grid. It also sets up event handlers for buttons and other interactive elements.
        /// </remarks>
        private void InitComponents()
        {
            amountLabel.Content = Strings.Amount + ":";
            dateLabel.Content = Strings.Date + ":";
            partnerLabel.Content = Strings.Partner + ":";
            statusLabel.Content = Strings.Status + ":";
            descriptionLabel.Content = Strings.Description + ":";

            FormatDesignForLabel(amountLabel);
            FormatDesignForLabel(dateLabel);
            FormatDesignForLabel(partnerLabel);
            FormatDesignForLabel(statusLabel);
            FormatDesignForLabel(descriptionLabel);

            FormatDesignForTextBox(amountTextbox);
            FormatDesignForTextBox(dateTextbox);
            FormatDesignForTextBox(partnerTextbox);
            FormatDesignForTextBox(descriptionTextbox);

            AddLabelAndTextBoxToGrid(amountLabel, amountTextbox, 0, 0);
            AddLabelAndTextBoxToGrid(dateLabel, dateTextbox, 0, 1);
            AddLabelAndTextBoxToGrid(partnerLabel, partnerTextbox, 2, 0);
            AddLabelAndTextBoxToGrid(descriptionLabel, descriptionTextbox, 4, 0, true);


            FormatDesignForComboBox(statusComboBox);
            AddElementsToCombobox(statusComboBox, new List<string> { Strings.Completed, Strings.Pending });
            AddLabelAndComboBoxToGrid(statusLabel, statusComboBox, 2, 1);


            Button addMoreDetails = new Button { Content = Strings.AddMoreDetails };
            addMoreDetails.Click += AddMoreDetails_Click;
            FormatDesignForButton(addMoreDetails, new Thickness(20, 20, 20, 10));
            AddButtonToGrid(addMoreDetails, 6, 0, true);

            Button cancel = new Button { Content = Strings.Cancel };
            cancel.Click += Cancel_Click;
            FormatDesignForButton(cancel, new Thickness(20, 10, 20, 20));
            AddButtonToGrid(cancel, 7, 0);

            Button finish = new Button { Content = Strings.Finish };
            finish.Click += Finish_Click;
            FormatDesignForButton(finish, new Thickness(20, 10, 20, 20));
            AddButtonToGrid(finish, 7, 1);

        }

        /// <summary>
        /// Handles the "Finish" button click, creating and adding a new transaction to the transaction manager, then closes the window.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Event data.</param>
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

        /// <summary>
        /// Handles the "Cancel" button click, closing the window without saving any changes.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Event data.</param>
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddMoreDetails_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Applies a consistent design format to a TextBox control.
        /// </summary>
        /// <param name="textBox">The TextBox to format.</param>
        /// <remarks>
        /// This method sets the font, color, background, border, 
        /// and margins for the TextBox to ensure a uniform appearance across the application.
        /// </remarks>
        private void FormatDesignForTextBox(AdvancedInputBox textBox)
        {
            textBox.FontFamily = new FontFamily("Segoe UI");
            textBox.FontSize = 12;
            textBox.Foreground = new SolidColorBrush(Colors.Black);
            textBox.Background = new SolidColorBrush(Colors.WhiteSmoke);
            textBox.BorderBrush = new SolidColorBrush(Colors.Black);
            textBox.BorderThickness = new Thickness(1);
            textBox.Margin = new Thickness(20, 0, 20, 0);
            textBox.MinHeight = 20;

        }

        /// <summary>
        /// Applies a consistent design format to a ComboBox control.
        /// </summary>
        /// <param name="comboBox">The ComboBox to format.</param>
        /// <remarks>
        /// Similar to the TextBox formatting, this method sets the font, color, background, border, and margins for the ComboBox.
        /// </remarks>
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

        /// <summary>
        /// Applies a consistent design format to a Label control.
        /// </summary>
        /// <param name="label">The Label to format.</param>
        /// <remarks>
        /// Sets the font, foreground color, background, and margins for the Label to match the application's design theme.
        /// </remarks>
        private void FormatDesignForLabel(Label label)
        {

            label.FontFamily = new FontFamily("Segoe UI");
            label.FontSize = 12;
            label.Foreground = new SolidColorBrush(Colors.Black);
            label.Background = new SolidColorBrush(Colors.Transparent);
            label.Margin = new Thickness(15, 0, 20, 0);
            label.VerticalAlignment = VerticalAlignment.Bottom;

        }

        /// <summary>
        /// Applies a consistent design format to a Button control.
        /// </summary>
        /// <param name="button">The Button to format.</param>
        /// <param name="margin">The margin to apply to the Button.</param>
        /// <remarks>
        /// Configures the Button's font, color, background, border, padding, and margins, and optionally applies a drop shadow effect.
        /// </remarks>
        private void FormatDesignForButton(Button button, Thickness margin)
        {

            button.FontFamily = new FontFamily("Segoe UI");
            button.FontSize = 12;
            button.MinHeight = 20;
            button.Foreground = new SolidColorBrush(Colors.White);
            button.Background = new SolidColorBrush(Color.FromArgb(255, 0, 120, 215));
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

        /// <summary>
        /// Adds a Label and a TextBox to the grid layout, positioning them according to specified row and column indices.
        /// </summary>
        /// <param name="label">The Label to add to the grid.</param>
        /// <param name="textBox">The TextBox to add to the grid.</param>
        /// <param name="row">The row index where the Label and TextBox will be placed.</param>
        /// <param name="column">The column index for the Label. The TextBox is placed in the next row.</param>
        /// <param name="large">If true, spans the Label and TextBox across two columns.</param>
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

            Grid.SetRow(textBox, row + 1);
            Grid.SetColumn(textBox, column);
            grid.Children.Add(textBox);
        }

        /// <summary>
        /// Adds a Label and a ComboBox to the grid layout, positioning them according to specified row and column indices.
        /// </summary>
        /// <param name="label">The Label to add to the grid.</param>
        /// <param name="comboBox">The ComboBox to add to the grid.</param>
        /// <param name="row">The row index where the Label and ComboBox will be placed.</param>
        /// <param name="column">The column index for the Label. The ComboBox is placed in the next row.</param>
        /// <param name="large">If true, spans the Label and ComboBox across two columns.</param>
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

        /// <summary>
        /// Adds a Button to the grid layout, positioning it according to specified row and column indices.
        /// </summary>
        /// <param name="button">The Button to add to the grid.</param>
        /// <param name="row">The row index where the Button will be placed.</param>
        /// <param name="column">The column index for the Button.</param>
        /// <param name="large">If true, spans the Button across two columns.</param>
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

        /// <summary>
        /// Adds a list of items to a ComboBox control.
        /// </summary>
        /// <param name="comboBox">The ComboBox to which items will be added.</param>
        /// <param name="items">The list of items to add to the ComboBox.</param>
        /// <remarks>
        /// Populates the ComboBox with items, setting the first item as the selected index by default.
        /// </remarks>
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