using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Windows;
using dataStructure;
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
            string partner = "ABC";
            TransactionManager manager = new TransactionManager();       
            manager.DisplayShortTransactionInformationsInComandline(manager.SortTransactionsByAmountDescending(manager.GetTransactionsByPartner(partner)));
            manager.DisplayTotalAmountInComandline(manager.GetTransactionsByPartner(partner));
            
            /*
            Application app = new Application(); 
            ManualTransactionEntryWindow window = new ManualTransactionEntryWindow(); 
            app.Run(window); 
            */
        }
    }
}
