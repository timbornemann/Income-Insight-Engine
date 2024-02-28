using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Principal;
using System.Threading;
using System.Windows;
using dataStructure;
using IncomeInsightEngine.Properties;
using IncomeInsightEngine.src.dataStructure.management;
using IncomeInsightEngine.src.ui.externalFrames;
using src.parser;


namespace src
{

   
    public class IncomeInsightEngineMain
    {
        [STAThread]
        public static void Main(string[] args)
        { 
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            CultureInfo culture = new CultureInfo("EN");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;


            Console.WriteLine(Strings.Transaction);

            
            /*
            TransactionManager manager = new TransactionManager();

            manager.DisplayTotalExpensesInComandline(manager.GetTransactionsByPartner("rewe"));
            */



            /*
            manager.DisplayShortTransactionInformationsInComandline(manager.SortTransactionsByAmountDescending(manager.GetTransactionsByDate( new DateTime(2023, 1, 1), new DateTime(2024, 1, 1))));
            manager.DisplayTotalIncomeInComandline(manager.GetTransactionsByDate(new DateTime(2023, 1, 1), new DateTime(2024, 1, 1)));
            manager.DisplayTotalExpensesInComandline(manager.GetTransactionsByDate(new DateTime(2023, 1, 1), new DateTime(2024, 1, 1)));
            */

            /*
            Application app = new Application(); 
            ManualTransactionEntryWindow window = new ManualTransactionEntryWindow(null, manager); 
            app.Run(window); 
            */
        }
    }
}
