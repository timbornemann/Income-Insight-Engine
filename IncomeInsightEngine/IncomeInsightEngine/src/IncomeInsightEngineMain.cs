using System;
using System.Collections.Generic;
using System.Security.Principal;
using dataStructure;
using IncomeInsightEngine.src.dataStructure.management;
using src.parser;


namespace src
{
    public class IncomeInsightEngineMain
    {
        public static void Main(string[] args)
        {


            
            TransactionManager manager = new TransactionManager();                                           
           
            
            DKBParser parser = new DKBParser();

            List<Transaction> t =   parser.ParseCsv("Link to file");

           
            manager.AddTransaction(t);

            manager.DisplayAllTransactionsShortInComandline();

       
            
        }
    }
}
