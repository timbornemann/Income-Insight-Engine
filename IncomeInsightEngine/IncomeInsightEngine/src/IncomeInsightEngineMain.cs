using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;
using System.Windows;
using dataStructure;
using IncomeInsightEngine;
using IncomeInsightEngine.Properties;
using IncomeInsightEngine.src.dataStructure.management;
using IncomeInsightEngine.src.parser;
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

            DKBCsvDataCreator dKBCsvDataCreator = new DKBCsvDataCreator();
           dKBCsvDataCreator.CreateData(12*1000);

       //   ConsoleLogic console = new ConsoleLogic();
/*
            TransactionManager manager = new TransactionManager();
            TransactionAnalyzer analyzer = new TransactionAnalyzer();
*/


            // manager.transaktionInformation.DisplayAllListsInComandline();



            //manager.PaymentMethodDescriptionBatchProcessing("Giro", "Girocard");
     


           //  manager.transaktionInformation.DisplayAllListsInComandline();
            // manager.DisplayOnlyGroupnamesComandline(manager.GroupByPartner(manager.SortTransactionsByPartnerAscending()));




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
