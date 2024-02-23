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

            //  TransactionManager manager = new TransactionManager();                                                          

            Application app = new Application(); 
            ManualTransactionEntryWindow window = new ManualTransactionEntryWindow(); 
            app.Run(window); 

        }
    }
}
