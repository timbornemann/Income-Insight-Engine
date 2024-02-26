using dataStructure;
using IncomeInsightEngine.src.ui.CustomUiElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace IncomeInsightEngine.src.ui.externalFrames
{
    internal class ManualTransactionEntryWindow : Window
    {
        StackPanel panel = new StackPanel();
        public ManualTransactionEntryWindow()
        {

            this.Content = panel;

            AdvancedInputBox textBox = new AdvancedInputBox(new List<string> { "aa", "aaa", "aaaa", "bb" });
            AdvancedInputBox textBox1 = new AdvancedInputBox();

            textBox.ChangeBackgroundColor(Brushes.Green);

            
          
            panel.Children.Add(textBox );
            panel.Children.Add(textBox1);
        }


    }
}
