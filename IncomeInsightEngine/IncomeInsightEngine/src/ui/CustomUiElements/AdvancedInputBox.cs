using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Text.RegularExpressions;
using System.Data;
using System.Windows.Input;


namespace IncomeInsightEngine.src.ui.CustomUiElements
{
    /// <summary>
    /// Represents an enhanced input box that supports features like placeholder text, suggestions, password mode, and custom color settings.
    /// </summary>
    /// <remarks>
    /// The AdvancedInputBox class extends the standard TextBox control to include:
    /// <list type="bullet">
    /// <item><description>Automatic suggestions based on user input and a customizable list of suggestions.</description></item>
    /// <item><description>Password mode that masks input characters with asterisks.</description></item>
    /// <item><description>Dynamic evaluation of expressions, converting input like mathematical expressions into their evaluated results.</description></item>
    /// <item><description>Customizable background and foreground colors for both the input box and the suggestions list.</description></item>
    /// <item><description>Placeholder text functionality, displaying a placeholder when the input box is empty and not focused.</description></item>
    /// <item><description>Utility methods for copying to and pasting from the clipboard, and preventing or transforming certain input characters.</description></item>
    /// </list>
    /// This class is designed to enhance the user interface of applications by providing a versatile and user-friendly input control.
    /// </remarks>
    /// <example>
    /// Usage:
    /// <code>
    /// AdvancedInputBox advancedInputBox = new AdvancedInputBox();
    /// advancedInputBox.SetSuggestionOn(true);
    /// advancedInputBox.ChangeBackgroundColor(Brushes.LightGray);
    /// advancedInputBox.ChangeForegroundColor(Brushes.Black);
    /// </code>
    /// </example>
    public class AdvancedInputBox : TextBox
    {
        private string _placeholderText;
        private Brush _placeholderColor = Brushes.Gray;
        private Brush _textColor = Brushes.Black;

        private bool suggestionsOn = false;
        private Popup suggestionsPopup;
        private ListBox suggestionsList;
        private List<string> suggestionsSource = new List<string>();

        private bool _isPasswordMode = false;
        private string _realText = "";

        /// <summary>
        /// Initializes a new instance of the AdvancedInputBox class with optional suggestions, text, and placeholder text.
        /// </summary>
        /// <param name="suggestions">Optional. A list of suggestions to be used by the autocomplete feature. If null, no suggestions are preloaded.</param>
        /// <param name="text">Optional. The initial text to display in the input box. If null and placeholderText is provided, placeholderText is displayed instead.</param>
        /// <param name="placeholderText">Optional. The text to display as a placeholder when the input box is empty and not focused.</param>
        /// <remarks>
        /// This constructor sets up the AdvancedInputBox with customized behaviors and styles based on the provided parameters. 
        /// It configures event handlers for focus, text change, and key input events to implement features like placeholder text management, 
        /// expression evaluation, and input character transformation. A Popup control containing a ListBox is also initialized for displaying suggestions, 
        /// which can be populated and controlled through the provided methods and properties of this class.
        /// </remarks>
        public AdvancedInputBox(List<string> suggestions = null, string text = null, string placeholderText = null)
        {

            this.suggestionsSource = (suggestions == null) ? new List<string>() : suggestions;

            this.Text = (text != null) ? text : placeholderText;
            this._placeholderText = placeholderText;

            this.Foreground = (placeholderText == null) ? _textColor : _placeholderColor;

            this.GotFocus += PlaceholderTextBox_GotFocus;
            this.LostFocus += PlaceholderTextBox_LostFocus;
            this.PreviewKeyDown += AdvancedTextBox_PreviewKeyDown;
            this.TextChanged += EvaluateExpression_TextChanged;
            this.PreviewTextInput += PreventCommaInput_PreviewTextInput;
            this.TextChanged += AutoCompleteTextBox_TextChanged;
           


            DataObject.AddPastingHandler(this, OnPastePreventCommaInput);

            suggestionsPopup = new Popup();
            suggestionsPopup.PlacementTarget = this;
            suggestionsPopup.Placement = PlacementMode.Bottom;
            suggestionsPopup.StaysOpen = false;

            suggestionsList = new ListBox();
            suggestionsPopup.Child = suggestionsList;
            suggestionsList.PreviewMouseDown += SuggestionsList_PreviewMouseDown;
            suggestionsList.PreviewKeyDown += SuggestionsList_PreviewKeyDown;

        }

        /// <summary>
        /// Dynamically adjusts the font size and vertical padding of the input box based on its current height.
        /// </summary>
        /// <remarks>
        /// This method calculates a new font size as half of the input box's actual height, 
        /// ensuring that the font size never goes below 0.1. 
        /// It then adjusts the vertical padding to center the text vertically within the input box. 
        /// The font size is scaled to maintain legibility and aesthetic appeal as the input box's size changes. 
        /// The vertical padding is recalculated to ensure that the text remains vertically centered, 
        /// taking into account the new font size and the inherent line height of the text.
        /// </remarks>
        public void RefreshFontSize()
        {          
            this.FontSize = Math.Max(0.1, this.ActualHeight * 0.5);

            double textHeight = this.FontSize * 1.3; 
            double verticalPadding = (this.ActualHeight - textHeight) / 2;
            verticalPadding = Math.Max(0, verticalPadding);

            this.Padding = new Thickness(this.Padding.Left, verticalPadding, this.Padding.Right, verticalPadding);
        }

        /// <summary>
        /// Enables or disables the suggestion feature.
        /// </summary>
        /// <param name="state">If true, suggestions are enabled; if false, suggestions are disabled. Default is true.</param>
        public void SetSuggestionOn(bool state = true)
        {
            suggestionsOn = state;
        }

        /// <summary>
        /// Sets the tooltip text for the UI component.
        /// </summary>
        /// <param name="tooltipText">The text to be displayed as a tooltip.</param>
        public void SetTooltip(string tooltipText)
        {
            this.ToolTip = tooltipText;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the component is in password mode.
        /// </summary>
        /// <value>
        /// True if the component is in password mode and should mask input; otherwise, false.
        /// </value>
        public bool IsPasswordMode
        {
            get { return _isPasswordMode; }
            set
            {
                _isPasswordMode = value;
                if (_isPasswordMode)
                {
                    this.PasswordModeOn();
                }
                else
                {
                    this.PasswordModeOff();
                }
            }
        }

        /// <summary>
        /// Activates password mode, masking the displayed text.
        /// </summary>
        /// <remarks>
        /// When password mode is activated, the text is masked with asterisks (*) and any text change event is handled to maintain the masking.
        /// </remarks>
        private void PasswordModeOn()
        {
            _realText = this.Text;
            this.Text = new string('*', _realText.Length);
            this.TextChanged += HandleMaskedText_TextChanged;
        }

        /// <summary>
        /// Deactivates password mode, revealing the original text.
        /// </summary>
        /// <remarks>
        /// When password mode is deactivated, the original text is displayed and the text change event handler for masking is removed.
        /// </remarks>
        private void PasswordModeOff()
        {
            this.Text = _realText;
            this.TextChanged -= HandleMaskedText_TextChanged;
        }

        /// <summary>
        /// Adds a single suggestion item to the suggestion source.
        /// </summary>
        /// <param name="suggestion">The suggestion item to add.</param>
        public void AddSuggestionItem(string suggestion)
        {
            suggestionsSource.Add(suggestion);
        }

        /// <summary>
        /// Adds multiple suggestion items to the suggestion source.
        /// </summary>
        /// <param name="suggestions">The list of suggestion items to add.</param>
        public void AddSuggestionItem(List<string> suggestions)
        {
            suggestionsSource.AddRange(suggestions);
        }

        /// <summary>
        /// Removes a specific suggestion item from the suggestion source.
        /// </summary>
        /// <param name="suggestion">The suggestion item to remove.</param>
        public void RemoveSuggestionItem(string suggestion)
        {
            suggestionsSource.Remove(suggestion);
        }

        /// <summary>
        /// Clears all suggestion items from the suggestion source.
        /// </summary>
        public void ClearSuggestionItems()
        {
            suggestionsSource.Clear();
        }

        /// <summary>
        /// Attempts to convert the component's text to a decimal number.
        /// </summary>
        /// <returns>The converted decimal number if successful; otherwise, 0.</returns>
        public decimal TryGetNumberAsDecimal()
        {
            string input = this.Text;
            Decimal.TryParse(input, out decimal result);
            return result;
        }

        /// <summary>
        /// Attempts to convert the component's text to an integer.
        /// </summary>
        /// <returns>The converted integer if successful; otherwise, 0.</returns>
        public int TrygetNumberAsInt()
        {
            string input = this.Text;
            int.TryParse(input, out int result);
            return result;
        }

        /// <summary>
        /// Attempts to convert the component's text to a DateTime object.
        /// </summary>
        /// <returns>The converted DateTime if successful; otherwise, the default value of DateTime.</returns>
        public DateTime TryGetDate()
        {
            string input = this.Text;
            DateTime.TryParse(input, out DateTime result);
            return result;
        }

        /// <summary>
        /// Attempts to parse the component's text into a list of strings, separated by commas.
        /// </summary>
        /// <returns>A list of strings derived from the component's text, if any; otherwise, an empty list.</returns>
        public List<string> TryGetItemsAsListSeparatedByCommas()
        {
            string text = this.Text;

            if (string.IsNullOrWhiteSpace(text))
            {
                return new List<string>();
            }

            return text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                       .Select(item => item.Trim()) 
                       .ToList();
        }

        /// <summary>
        /// Gets the number of characters in the component's text.
        /// </summary>
        /// <returns>The number of characters.</returns>
        public int GetCharacterCount()
        {
            return this.Text.Length;
        }

        /// <summary>
        /// Gets the number of words in the component's text, using space, tab, and newline characters as separators.
        /// </summary>
        /// <returns>The number of words.</returns>
        public int GetWordCount()
        {
            return !string.IsNullOrEmpty(this.Text) ? this.Text.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length : 0;
        }

        /// <summary>
        /// Validates the component's text against a specified regular expression pattern.
        /// </summary>
        /// <param name="pattern">The regular expression pattern to match against the component's text.</param>
        /// <returns>True if the text matches the pattern; otherwise, false.</returns>
        public bool ValidateText(string pattern)
        {
            Regex regex = new Regex(pattern);
            return regex.IsMatch(this.Text);
        }

        /// <summary>
        /// Copies the component's text to the system clipboard if it is not empty.
        /// </summary>
        public void CopyToClipboard()
        {
            if (!string.IsNullOrEmpty(this.Text))
            {
                Clipboard.SetText(this.Text);
            }
        }

        /// <summary>
        /// Pastes text from the system clipboard into the component, replacing its current text.
        /// </summary>
        public void PasteFromClipboard()
        {
            this.Text = Clipboard.GetText();
        }

        /// <summary>
        /// Changes the background color of the textbox and its suggestions list.
        /// </summary>
        /// <param name="brush">The new background color as a Brush.</param>
        public void ChangeBackgroundColor(Brush brush)
        {
            this.Background = brush;
            suggestionsList.Background = brush;
        }

        /// <summary>
        /// Changes the foreground color of the textbox and its suggestions list.
        /// </summary>
        /// <param name="brush">The new foreground color as a Brush.</param>
        public void ChangeForegroundColor(Brush brush)
        {
            this.Foreground = brush;
            suggestionsList.Foreground = brush;
        }

        /// <summary>
        /// Evaluates a mathematical expression and returns the result.
        /// </summary>
        /// <param name="expression">The mathematical expression to evaluate.</param>
        /// <returns>The result of the evaluated expression.</returns>
        private object Evaluate(string expression)
        {
            var table = new DataTable();
            table.Columns.Add("expression", string.Empty.GetType(), expression);
            DataRow row = table.NewRow();
            table.Rows.Add(row);
            return row["expression"];
        }

        /// <summary>
        /// Handles paste operations, replacing commas with periods to maintain numeric format consistency.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Event data containing the pasted text.</param>
        private void OnPastePreventCommaInput(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                string pasteText = (string)e.DataObject.GetData(typeof(String));
                string modifiedText = pasteText.Replace(',', '.');

                e.CancelCommand();
                int caretIndex = this.CaretIndex;
                this.Text = this.Text.Insert(caretIndex, modifiedText);
                this.CaretIndex = caretIndex + modifiedText.Length;
            }
            else
            {
                e.CancelCommand();
            }
        }

        /// <summary>
        /// Filters suggestions based on the current text input and displays a suggestions list if matches are found.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Event data for the text change.</param>
        private void AutoCompleteTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (suggestionsOn)
            {

                string text = this.Text;
                List<string> filteredSuggestions = suggestionsSource
                    .Where(s => s.StartsWith(text, StringComparison.InvariantCultureIgnoreCase))
                    .ToList();

                if (filteredSuggestions.Any() && !string.IsNullOrEmpty(text))
                {
                    suggestionsList.ItemsSource = filteredSuggestions;
                    suggestionsPopup.IsOpen = true;
                }
                else
                {
                    suggestionsPopup.IsOpen = false;
                }
            }
        }

        /// <summary>
        /// Handles text changes in password mode by masking the displayed text with asterisks.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Event data for the text change.</param>
        private void HandleMaskedText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_isPasswordMode)
            {
                int diff = this.Text.Length - _realText.Length;
                if (diff > 0)
                {
                    _realText += this.Text.Substring(this.Text.Length - diff);
                }
                else
                {
                    _realText = _realText.Substring(0, _realText.Length + diff);
                }
                this.Text = new string('*', _realText.Length);
                this.CaretIndex = this.Text.Length;
            }
        }

        /// <summary>
        /// Evaluates expressions entered into the textbox, displaying the result when the user types an equals sign.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Event data for the text change.</param>
        private void EvaluateExpression_TextChanged(object sender, TextChangedEventArgs e)
        {
            var input = this.Text;
            if (input.EndsWith("="))
            {
                try
                {
                    var result = Evaluate(input.TrimEnd('='));
                    this.Text = result.ToString().Replace(",", ".");
                    this.CaretIndex = this.Text.Length;
                }
                catch (Exception)
                {

                    this.Text = input;
                    this.CaretIndex = this.Text.Length;
                }
            }
        }

        /// <summary>
        /// Prevents comma input during text entry, replacing it with a period to ensure numeric format consistency.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Event data for the text input.</param>
        private void PreventCommaInput_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text == ",")
            {
                e.Handled = true;
                int caretIndex = this.CaretIndex;
                this.Text = this.Text.Insert(caretIndex, ".");
                this.CaretIndex = caretIndex + 1;
            }
        }

        /// <summary>
        /// Handles key down events to navigate the suggestions list or perform other custom actions.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Event data for the key press.</param>
        private void AdvancedTextBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Down)
            {
                suggestionsList.Focus();
                return;
            }



        }

        /// <summary>
        /// Handles key down events on the suggestions list, allowing users to select a suggestion with the keyboard.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Event data for the key press.</param>
        private void SuggestionsList_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter || e.Key == System.Windows.Input.Key.Space)
            {
                if (sender is ListBox listBox && listBox.SelectedItem is string selectedItem)
                {
                    this.Text = selectedItem;
                    this.CaretIndex = this.Text.Length;
                    suggestionsPopup.IsOpen = false;
                    this.Focus();
                }
            }
        }

        /// <summary>
        /// Handles mouse down events on the suggestions list, allowing users to select a suggestion with the mouse.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Event data for the mouse click.</param>
        private void SuggestionsList_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is ListBox listBox && listBox.SelectedItem is string selectedItem)
            {
                this.Text = selectedItem;
                this.CaretIndex = this.Text.Length;
                suggestionsPopup.IsOpen = false;
            }
        }

        /// <summary>
        /// Handles the textbox's focus events to manage placeholder text visibility and color.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Event data for the focus change.</param>
        private void PlaceholderTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            // Wenn der Benutzer auf die TextBox klickt und der Platzhaltertext angezeigt wird
            if (this.Text == _placeholderText)
            {
                this.Text = "";
                this.Foreground = _textColor; // Setzt die normale Textfarbe
            }
        }

        /// <summary>
        /// Handles the textbox's focus events to manage placeholder text visibility and color.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Event data for the focus change.</param>
        private void PlaceholderTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            // Wenn der Benutzer die TextBox verlässt und keinen Text eingegeben hat
            if (string.IsNullOrWhiteSpace(this.Text))
            {
                this.Text = _placeholderText;
                this.Foreground = _placeholderColor; // Setzt die Farbe des Platzhaltertexts
            }
        }


    }
}
