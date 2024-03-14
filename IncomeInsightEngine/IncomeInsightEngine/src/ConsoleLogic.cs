using dataStructure;
using IncomeInsightEngine.Properties;
using IncomeInsightEngine.src.dataStructure.management;
using src.parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.AvalonDock.Layout;

namespace IncomeInsightEngine
{
    internal class ConsoleLogic
    {
        TransactionManager manager;
        TransactionAnalyzer analyzer;
        CsvDKBParser parser;

        string depthOfSelection = "";

        public ConsoleLogic()
        {
            manager = new TransactionManager();
            analyzer = new TransactionAnalyzer();
            parser = new CsvDKBParser();

            Start();
        }

        private void Start()
        {
            Console.WriteLine(Strings.ConsoleMode);

         
            ChooseMode(manager.GetAllTransactions(), manager.GroupByPartner());
                       
        }


        private void ChooseMode(IEnumerable<Transaction> transactions, IEnumerable<IGrouping<string, Transaction>> groupedTransactions)
        {
            
            

            bool exit = false;
            string message = $"| {Strings.ViewList} = 0 | {Strings.Group} = 1 | {Strings.Sort} = 2 | {Strings.Reset} = 3 | {Strings.Exit} = -1 |";
            string dots = new string('-', message.Length);
            while (!exit)
            {
                Console.WriteLine($"|>... {depthOfSelection}|");
                Console.WriteLine(Strings.ChooseMode + " "+ dots.Substring(Strings.ChooseMode.Length+1));
                Console.WriteLine(message);
                Console.WriteLine(dots);

                string input = Console.ReadLine().Trim();
                switch (input)
                {
                    case "0":
                       transactions = ViewList(transactions);
                        break;
                    case "1":
                        groupedTransactions = Group(transactions);
                        break;
                    case "2":
                      transactions =  Sort(transactions);
                        break;
                    case "3":
                        transactions = manager.GetAllTransactions();
                        depthOfSelection = "";
                        break;
                    case "-1":
                        exit = true;
                        break;
                }
            }


        }

        private IEnumerable<Transaction> ViewList(IEnumerable<Transaction> transactions)
        {
            string message = $"| {Strings.Show} = 0 | {Strings.RefineSelection} = 1 | {Strings.Sort} = 2 | {Strings.Group} = 3 | {Strings.AnalyzeTransactions} = 4 | {Strings.Reset} = 5 | {Strings.Exit} = -1 |";
            string dots = new string('-', message.Length);
            bool exit = false;
           
            while (!exit)
            {

                Console.WriteLine($"|>... {depthOfSelection}|");
                Console.WriteLine(Strings.ViewList + " " + dots.Substring(Strings.ViewList.Length + 1));
                Console.WriteLine(message);
                Console.WriteLine(dots);

                string input = Console.ReadLine().Trim();
                switch (input)
                {
                    case "0":
                        ShowListInConsole(transactions);
                        break;
                    case "1":
                        transactions = RefineSelection(transactions);
                        break;
                    case "2":
                        transactions = Sort(transactions);

                        break;
                    case "3":
                        Group(transactions);
                        break;

                    case "4":

                        break;

                    case "5":
                        transactions = manager.GetAllTransactions();
                        depthOfSelection = "";
                        break;
                    case "-1":
                        exit = true;                      
                        break;
                }
            }
            return transactions;

       }

        private void ShowListInConsole(IEnumerable<Transaction> transactions)
        {
            string message = $"| {Strings.ShortView} = 0 | {Strings.CompleteView} = 1 |";
            string dots = new string('-', message.Length);

            Console.WriteLine(Strings.Show + " " + dots.Substring(Strings.Show.Length + 1));
            Console.WriteLine(message);
            Console.WriteLine(dots);

            string input = Console.ReadLine().Trim();
            switch (input)
            {
                case "0":
                    manager.DisplayShortTransactionInformationsInComandline(transactions);
                    break;
                case "1":
                    manager.DisplayCompleteTransactionInformationsInComandline(transactions);
                    break;
            }

        }

        private IEnumerable<Transaction> RefineSelection(IEnumerable<Transaction> transactions)
        {

            Console.WriteLine(Strings.RefineSelection + ": ");

            Console.WriteLine($"{Strings.GetIncomeTransactions} = 0");
            Console.WriteLine($"{Strings.GetExpenseTransactions} = 1");
            Console.WriteLine($"{Strings.GetTransactionsByExactAmount} = 2");
            Console.WriteLine($"{Strings.GetTransactionsByAmountRange} = 3");
            Console.WriteLine($"{Strings.GetTransactionsByDate} = 4");
            Console.WriteLine($"{Strings.GetTransactionsByDate} = 5");
            Console.WriteLine($"{Strings.GetTransactionsByDescription} = 6");
            Console.WriteLine($"{Strings.GetTransactionsByCurrency} = 7");
            Console.WriteLine($"{Strings.GetTransactionsByPaymentMethod} = 8");
            Console.WriteLine($"{Strings.GetTaxDeductibleTransactions} = 9");
            Console.WriteLine($"{Strings.GetReimbursableTransactions} = 10");
            Console.WriteLine($"{Strings.GetTransactionsByCategory} = 11");
            Console.WriteLine($"{Strings.GetTransactionsByBudgetCategory} = 12");
            Console.WriteLine($"{Strings.GetTransactionsByClassification} = 13");
            Console.WriteLine($"{Strings.GetTransactionsByPartner} = 14");
            Console.WriteLine($"{Strings.GetTransactionsByProject} = 15");
            Console.WriteLine($"{Strings.GetTransactionsByStatus} = 16");
            Console.WriteLine($"{Strings.GetTransactionsByPriority} = 17");
            Console.WriteLine($"{Strings.GetTransactionsByFrequency} = 18");
            Console.WriteLine($"{Strings.GetTransactionsByLocation} = 19");
            Console.WriteLine($"{Strings.GetTransactionsByTag} = 20");
            Console.WriteLine();
            Console.Write(Strings.Selection + " = ");
            string input = Console.ReadLine().Trim();

            switch (input)
            {
                case "0":
                    transactions = manager.GetIncomeTransactions(transactions);
                    depthOfSelection += "GetIncomeTransactions";
                    break;
                case "1":
                    transactions = manager.GetExpenseTransactions(transactions);
                    depthOfSelection += "GetExpenseTransactions";
                    break;
                case "2":
                    Console.Write($"{Strings.Amount}: ");
                    decimal.TryParse(Console.ReadLine().Trim(), out decimal someAmount);
                    transactions = manager.GetTransactionsByAmount(someAmount, transactions);
                    depthOfSelection += $"GetTransactionsByAmount({someAmount})";
                    break;
                case "3":
                    Console.Write($"{Strings.MinAmount}: ");
                    decimal.TryParse(Console.ReadLine().Trim(), out decimal minAmount);
                    Console.Write($"{Strings.MaxAmount}: ");
                    decimal.TryParse(Console.ReadLine().Trim(), out decimal maxAmount);
                    transactions = manager.GetTransactionsByAmount(minAmount, maxAmount, transactions);
                    depthOfSelection += $"GetTransactionsByAmountRange({minAmount}, {maxAmount})";
                    break;
                case "4":
                    Console.Write($"{Strings.Date}: ");
                    DateTime.TryParse(Console.ReadLine().Trim(), out DateTime someDate);
                    transactions = manager.GetTransactionsByDate(someDate, transactions);
                    depthOfSelection += $"GetTransactionsByDate({someDate})";
                    break;
                case "5":
                    Console.Write($"{Strings.StartDate}: ");
                    DateTime.TryParse(Console.ReadLine().Trim(), out DateTime startDate);
                    Console.Write($"{Strings.EndDate}: ");
                    DateTime.TryParse(Console.ReadLine().Trim(), out DateTime endDate);
                    transactions = manager.GetTransactionsByDate(startDate, endDate, transactions);
                    depthOfSelection += $"GetTransactionsByDateRange({startDate}, {endDate})";
                    break;
                case "6":
                    Console.Write($"{Strings.Description}: ");
                    string description = Console.ReadLine().Trim();
                    transactions = manager.GetTransactionsByDescription(description, transactions);
                    depthOfSelection += $"GetTransactionsByDescription({description})";
                    break;
                case "7":
                    Console.Write($"{Strings.Currency}: ");
                    string currency = Console.ReadLine().Trim();
                    transactions = manager.GetTransactionsByCurrency(currency, transactions);
                    depthOfSelection += $"GetTransactionsByCurrency({currency})";
                    break;
                case "8":
                    Console.Write($"{Strings.PaymentMethod}: ");
                    string paymentMethod = Console.ReadLine().Trim();
                    transactions = manager.GetTransactionsByPaymentMethod(paymentMethod, transactions);
                    depthOfSelection += $"GetTransactionsByPaymentMethod({paymentMethod})";
                    break;
                case "9":
                    Console.Write($"{Strings.TaxDeductible}: ");
                    bool taxDeductible = bool.TryParse(Console.ReadLine().Trim(), out bool deductibleValue) ? deductibleValue : false;
                    transactions = manager.GetTaxDeductibleTransactions(taxDeductible, transactions);
                    depthOfSelection += $"GetTaxDeductibleTransactions({taxDeductible})";
                    break;
                case "10":
                    Console.Write($"{Strings.Reimbursable}: ");
                    bool reimbursable = bool.TryParse(Console.ReadLine().Trim(), out bool reimbursableValue) ? reimbursableValue : false;
                    transactions = manager.GetReimbursableTransactions(reimbursable, transactions);
                    depthOfSelection += $"GetReimbursableTransactions({reimbursable})";
                    break;
                case "11":
                    Console.Write($"{Strings.Category}: ");
                    string category = Console.ReadLine().Trim();
                    transactions = manager.GetTransactionsByCategory(category, transactions);
                    depthOfSelection += $"GetTransactionsByCategory({category})";
                    break;
                case "12":
                    Console.Write($"{Strings.BudgetCategory}: ");
                    string budgetCategory = Console.ReadLine().Trim();
                    transactions = manager.GetTransactionsByBudgetCategory(budgetCategory, transactions);
                    depthOfSelection += $"GetTransactionsByBudgetCategory({budgetCategory})";
                    break;
                case "13":
                    Console.Write($"{Strings.Classification}: ");
                    string classification = Console.ReadLine().Trim();
                    transactions = manager.GetTransactionsByClassification(classification, transactions);
                    depthOfSelection += $"GetTransactionsByClassification({classification})";
                    break;
                case "14":
                    Console.Write($"{Strings.Partner}: ");
                    string partner = Console.ReadLine().Trim();
                    transactions = manager.GetTransactionsByPartner(partner, transactions);
                    depthOfSelection += $"GetTransactionsByPartner({partner})";
                    break;
                case "15":
                    Console.Write($"{Strings.Project}: ");
                    string project = Console.ReadLine().Trim();
                    transactions = manager.GetTransactionsByProject(project, transactions);
                    depthOfSelection += $"GetTransactionsByProject({project})";
                    break;
                case "16":
                    Console.Write($"{Strings.Status}: ");
                    string status = Console.ReadLine().Trim();
                    transactions = manager.GetTransactionsByStatus(status, transactions);
                    depthOfSelection += $"GetTransactionsByStatus({status})";
                    break;
                case "17":
                    Console.Write($"{Strings.Priority}: ");
                    string priority = Console.ReadLine().Trim();
                    transactions = manager.GetTransactionsByPriority(priority, transactions);
                    depthOfSelection += $"GetTransactionsByPriority({priority})";
                    break;
                case "18":
                    Console.Write($"{Strings.Frequency}: ");
                    string frequency = Console.ReadLine().Trim();
                    transactions = manager.GetTransactionsByFrequency(frequency, transactions);
                    depthOfSelection += $"GetTransactionsByFrequency({frequency})";
                    break;
                case "19":
                    Console.Write($"{Strings.Location}: ");
                    string location = Console.ReadLine().Trim();
                    transactions = manager.GetTransactionsByLocation(location, transactions);
                    depthOfSelection += $"GetTransactionsByLocation({location})";
                    break;
                case "20":
                    Console.Write($"{Strings.Tag}: ");
                    string tag = Console.ReadLine().Trim();
                    transactions = manager.GetTransactionsByTag(tag, transactions);
                    depthOfSelection += $"GetTransactionsByTag({tag})";
                    break;
                default:
                    Console.WriteLine("Invalid input.");
                    break;
            }


            depthOfSelection += "> ";

            return transactions;

        }

        private IEnumerable<Transaction> Sort(IEnumerable<Transaction> transactions)
        {
            Console.WriteLine(Strings.Sort + ": ");

                Console.WriteLine($"{Strings.SortTransactionsByIdAscending} = 0");
                Console.WriteLine($"{Strings.SortTransactionsByIdDescending} = 1");
                Console.WriteLine($"{Strings.SortTransactionsByDateAscending} = 2");
                Console.WriteLine($"{Strings.SortTransactionsByDateDescending} = 3");
                Console.WriteLine($"{Strings.SortTransactionsByAmountAscending} = 4");
                Console.WriteLine($"{Strings.SortTransactionsByAmountDescending} = 5");
                Console.WriteLine($"{Strings.SortTransactionsByPartnerAscending} = 6");
                Console.WriteLine($"{Strings.SortTransactionsByPartnerDescending} = 7");
                Console.WriteLine($"{Strings.SortTransactionsByLocationAscending} = 8");
                Console.WriteLine($"{Strings.SortTransactionsByLocationDescending} = 9");
            Console.WriteLine();
            Console.Write(Strings.Selection + " = ");
            string input = Console.ReadLine().Trim();

            switch (input)
            {
                case "0":
                    depthOfSelection += $"{Strings.SortTransactionsByIdAscending}";
                    transactions = manager.SortTransactionsByIdAscending(transactions);
                    break;
                case "1":
                    depthOfSelection += $"{Strings.SortTransactionsByIdDescending}";
                    transactions = manager.SortTransactionsByIdDescending(transactions);
                    break;
                case "2":
                    depthOfSelection += $"{Strings.SortTransactionsByDateAscending}";
                    transactions = manager.SortTransactionsByDateAscending(transactions);
                    break;
                case "3":
                    depthOfSelection += $"{Strings.SortTransactionsByDateDescending}";
                    transactions = manager.SortTransactionsByDateDescending(transactions);
                    break;
                case "4":
                    depthOfSelection += $"{Strings.SortTransactionsByAmountAscending}";
                    transactions = manager.SortTransactionsByAmountAscending(transactions);
                    break;
                case "5":
                    depthOfSelection += $"{Strings.SortTransactionsByAmountDescending}";
                    transactions = manager.SortTransactionsByAmountDescending(transactions);
                    break;
                case "6":
                    depthOfSelection += $"{Strings.SortTransactionsByPartnerAscending}";
                    transactions = manager.SortTransactionsByPartnerAscending(transactions);
                    break;
                case "7":
                    depthOfSelection += $"{Strings.SortTransactionsByPartnerDescending}";
                    transactions = manager.SortTransactionsByPartnerDescending(transactions);
                    break;
                case "8":
                    depthOfSelection += $"{Strings.SortTransactionsByLocationAscending}";
                    transactions = manager.SortTransactionsByLocationAscending(transactions);
                    break;
                case "9":
                    depthOfSelection += $"{Strings.SortTransactionsByLocationDescending}";
                    transactions = manager.SortTransactionsByLocationDescending(transactions);
                    break;
                default:
                    Console.WriteLine("Invalid selection.");
                    break;
            }

            depthOfSelection += "> ";



            return transactions;
        }


        private IEnumerable<IGrouping<string, Transaction>> Group(IEnumerable<Transaction> transactions)
        {
            string message = $"| {Strings.Show} = 0 | {Strings.Group} = 1 | {Strings.Sort} = 2 | {Strings.AnalyzeTransactions} = 3 | {Strings.Reset} = 4 | {Strings.Exit} = -1 |";
            string dots = new string('-', message.Length);
            bool exit = false;
            IEnumerable<IGrouping<string, Transaction>> groupedTransactions = null;

            while (!exit)
            {

                Console.WriteLine($"|>... {depthOfSelection}|");
                Console.WriteLine(Strings.Group + " " + dots.Substring(Strings.Group.Length + 1));
                Console.WriteLine(message);
                Console.WriteLine(dots);

                string input = Console.ReadLine().Trim();
                switch (input)
                {
                    case "0":
                        ShowGroupInConsole(groupedTransactions);
                        break;
                    case "1":
                        groupedTransactions = GroupTransactions(transactions);                       
                        break;
                    case "2":

                        break;
                    case "3":

                        break;

                    case "4":
                        groupedTransactions = manager.GroupByPartner();
                        transactions = manager.GetAllTransactions();
                        depthOfSelection = "";
                        break;
                    case "-1":
                        exit = true;
                        depthOfSelection = "";
                        break;
                }
            }
            return groupedTransactions;

        }

        private void ShowGroupInConsole(IEnumerable<IGrouping<string, Transaction>> groupedTransactions)
        {
            string message = $"| {Strings.ViewTransactions} = 0 | {Strings.ViewOnlyGroupnames} = 1 |";
            string dots = new string('-', message.Length);

            Console.WriteLine(Strings.Show + " " + Strings.Group + " " + dots.Substring(Strings.Show.Length + Strings.Group.Length + 2));
            Console.WriteLine(message);
            Console.WriteLine(dots);

            string input = Console.ReadLine().Trim();
            switch (input)
            {
                case "0":
                    manager.DisplayGroupedTransactionsComandline(groupedTransactions);
                    break;
                case "1":
                    manager.DisplayOnlyGroupnamesComandline(groupedTransactions);
                    break;
            }
        }

        private IEnumerable<IGrouping<string, Transaction>> GroupTransactions(IEnumerable<Transaction> transactions)
        {
            Console.WriteLine(Strings.GroupBy + ": ");

            Console.WriteLine($"{Strings.GroupByDate} = 0");
            Console.WriteLine($"{Strings.GroupByDateDay} = 1");
            Console.WriteLine($"{Strings.GroupByDateWeekDay} = 2");
            Console.WriteLine($"{Strings.GroupByDateMonth} = 3");
            Console.WriteLine($"{Strings.GroupByDateYear} = 4");
            Console.WriteLine($"{Strings.GroupByDescription} = 5");
            Console.WriteLine($"{Strings.GroupByAmount} = 6");
            Console.WriteLine($"{Strings.GroupByAmountrange} = 7");
            Console.WriteLine($"{Strings.GroupByCurrency} = 8");
            Console.WriteLine($"{Strings.GroupByPaymentMethod} = 9");
            Console.WriteLine($"{Strings.GroupByCategory} = 10");
            Console.WriteLine($"{Strings.GroupByBudgetCategory} = 11");
            Console.WriteLine($"{Strings.GroupByTag} = 12");
            Console.WriteLine($"{Strings.GroupByClassification} = 13");
            Console.WriteLine($"{Strings.GroupByPartnerIBAN} = 14");
            Console.WriteLine($"{Strings.GroupByPartner} = 15");
            Console.WriteLine($"{Strings.GroupByProject} = 16");
            Console.WriteLine($"{Strings.GroupByStatus} = 17");
            Console.WriteLine($"{Strings.GroupByPriority} = 18");
            Console.WriteLine($"{Strings.GroupByFrequency} = 19");
            Console.WriteLine($"{Strings.GroupByLocation} = 20");
            Console.WriteLine($"{Strings.GroupByNotes} = 21");

            Console.Write(Strings.Selection + " = ");
            string input = Console.ReadLine().Trim();

            IEnumerable<IGrouping<string, Transaction>> groupedTransactions = null;

            switch (input)
            {
                case "0":
                    depthOfSelection += $"{Strings.GroupByDate}";
                    groupedTransactions = manager.GroupByDate(transactions);
                    break;
                case "1":
                    depthOfSelection += $"{Strings.GroupByDateDay}";
                    groupedTransactions = manager.GroupByDateDay(transactions);
                    break;
                case "2":
                    depthOfSelection += $"{Strings.GroupByDateWeekDay}";
                    groupedTransactions = manager.GroupByDateWeekDay(transactions);
                    break;
                case "3":
                    depthOfSelection += $"{Strings.GroupByDateMonth}";
                    groupedTransactions = manager.GroupByDateMonth(transactions);
                    break;
                case "4":
                    depthOfSelection += $"{Strings.GroupByDateYear}";
                    groupedTransactions = manager.GroupByDateYear(transactions);
                    break;
                case "5":
                    depthOfSelection += $"{Strings.GroupByDescription}";
                    groupedTransactions = manager.GroupByDescription(transactions);
                    break;
                case "6":
                    depthOfSelection += $"{Strings.GroupByAmount}";
                    groupedTransactions = manager.GroupByAmount(transactions);
                    break;
                case "7":
                    depthOfSelection += $"{Strings.GroupByAmountrange}";
                    groupedTransactions = manager.GroupByAmountrange(transactions);
                    break;
                case "8":
                    depthOfSelection += $"{Strings.GroupByCurrency}";
                    groupedTransactions = manager.GroupByCurrency(transactions);
                    break;
                case "9":
                    depthOfSelection += $"{Strings.GroupByPaymentMethod}";
                    groupedTransactions = manager.GroupByPaymentMethod(transactions);
                    break;
                case "10":
                    depthOfSelection += $"{Strings.GroupByCategory}";
                    groupedTransactions = manager.GroupByCategory(transactions);
                    break;
                case "11":
                    depthOfSelection += $"{Strings.GroupByBudgetCategory}";
                    groupedTransactions = manager.GroupByBudgetCategory(transactions);
                    break;
                case "12":
                    depthOfSelection += $"{Strings.GroupByTag}";
                    groupedTransactions = manager.GroupByTag(transactions);
                    break;
                case "13":
                    depthOfSelection += $"{Strings.GroupByClassification}";
                    groupedTransactions = manager.GroupByClassification(transactions);
                    break;
                case "14":
                    depthOfSelection += $"{Strings.GroupByPartnerIBAN}";
                    groupedTransactions = manager.GroupByPartnerIBAN(transactions);
                    break;
                case "15":
                    depthOfSelection += $"{Strings.GroupByPartner}";
                    groupedTransactions = manager.GroupByPartner(transactions);
                    break;
                case "16":
                    depthOfSelection += $"{Strings.GroupByProject}";
                    groupedTransactions = manager.GroupByProject(transactions);
                    break;
                case "17":
                    depthOfSelection += $"{Strings.GroupByStatus}";
                    groupedTransactions = manager.GroupByStatus(transactions);
                    break;
                case "18":
                    depthOfSelection += $"{Strings.GroupByPriority}";
                    groupedTransactions = manager.GroupByPriority(transactions);
                    break;
                case "19":
                    depthOfSelection += $"{Strings.GroupByFrequency}";
                    groupedTransactions = manager.GroupByFrequency(transactions);
                    break;
                case "20":
                    depthOfSelection += $"{Strings.GroupByLocation}";
                    groupedTransactions = manager.GroupByLocation(transactions);
                    break;
                case "21":
                    depthOfSelection += $"{Strings.GroupByNotes}";
                    groupedTransactions = manager.GroupByNotes(transactions);
                    break;
                default:
                    Console.WriteLine("Invalid selection.");
                    break;
            }
            return groupedTransactions;
        }

    


        private void AnalyzeTransactions()
        {

        }

        private void AddTransactions()
        {

        }
        private void Exit()
        {
            System.Environment.Exit(1);
        }

    }
}
