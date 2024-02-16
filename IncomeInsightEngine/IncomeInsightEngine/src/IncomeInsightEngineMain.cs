using System;
using System.Collections.Generic;
using System.Security.Principal;
using dataStructure;
using IncomeInsightEngine.src.dataStructure.management;


namespace src
{
    public class IncomeInsightEngineMain
    {
        public static void Main(string[] args)
        {

            TransactionManager manager = new TransactionManager();         
            manager.DisplayAllTransactionsShortInComandline();

            manager.Close();
            


        }
    }
}
