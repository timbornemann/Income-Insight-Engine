using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;
using System.Windows;
using dataStructure;
using IncomeInsightEngine.Properties;
using IncomeInsightEngine.src.dataStructure.management;
using IncomeInsightEngine.src.ui;
using src.parser;


namespace src
{

   
    public class IncomeInsightEngineMain
    {
       // [STAThread]
        public static void Main(string[] args)
        {
          //  AllocConsole();
            Console.OutputEncoding = System.Text.Encoding.UTF8;


            TransactionManager manager = new TransactionManager();
            TransactionAnalyzer analyzer = new TransactionAnalyzer();

           

         

            analyzer.DisplayGroupedAmountInComandline(analyzer.SortByAmountDescending(analyzer.CalculateGroupedPercentageOfTotalExpanses(manager.GroupByDate())));

            //  analyzer.DisplayTotalAmountInComandline(manager.GetAllTransactions());

           // manager.DisplayShortTransactionInformationsInComandline(manager.SortTransactionsByDateAscending());

            // manager.RenameAllTransactionPartnersInComandline();

            // manager.DisplayGroupedTransactions(manager.GroupByDateWeekDay());

            /*
            Application app = new Application(); 
           MainWindow window = new MainWindow(manager); 
            app.Run(window); 
           */
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool AllocConsole();
    }
}
