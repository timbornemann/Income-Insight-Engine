using dataStructure;
using IncomeInsightEngine.Properties;
using IncomeInsightEngine.Resources.Language;
using IncomeInsightEngine.src.dataStructure.management;
using IncomeInsightEngine.src.parser;
using src.parser;
using System;
using System.Collections.Generic;
using System.Linq;



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

        /// <summary>
        /// Initiates the console application, displaying the initial mode selection to the user.
        /// </summary>
        /// <remarks>
        /// This method displays the available modes to the user and handles the chosen mode through the ChooseMode method, utilizing all transactions managed by the application.
        /// </remarks>
        private void Start()
        {
            Console.WriteLine(Strings.ConsoleMode);

         
            ChooseMode(manager.GetAllTransactions());
                       
        }

        /// <summary>
        /// Provides a looped menu for the user to select various modes for transaction management, including viewing, refining, sorting, grouping, analyzing, and editing transactions.
        /// </summary>
        /// <param name="transactions">The collection of transactions to operate on.</param>
        /// <returns>The potentially modified collection of transactions after performing operations like refining, sorting, or resetting.</returns>
        /// <remarks>
        /// Supports operations to refine, sort, group, and analyze transactions, along with the ability to reset to the original list or edit transactions directly.
        /// </remarks>
        private IEnumerable<Transaction> ChooseMode(IEnumerable<Transaction> transactions)
        {
           
            bool exit = false;
  
            while (!exit)
            {
                string message1 = $"| {Strings.ShowTransactions} = 0 | {Strings.RefineSelection} = 1 | {Strings.Sort} = 2 | {Strings.Group} = 3 | {Strings.AnalyzeTransactions} = 4 |";
                string message2 = $"| {Strings.Reset} = 5 | {Strings.EditTransactions} = 6 | {Strings.Settings} = 7 | {Strings.Exit} = -1";



                int m1adapter = message1.Length - message2.Length-1;
                string dots = new string('-', message1.Length);

                Console.WriteLine($"|>... {depthOfSelection}|");
                Console.WriteLine(Strings.ChooseMode + " " + dots.Substring(Strings.ChooseMode.Length + 1));
                Console.WriteLine(message1);
                Console.WriteLine(message2 + new string(' ', m1adapter) + "|");
                Console.WriteLine(dots);

                string input = Console.ReadLine().Trim();
                if (transactions == null)
                {
                    Console.WriteLine(Strings.notransactionsavailable);
                    input = "6";
                }
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
                        AnalyzeTransactions(transactions);
                        break;

                    case "5":
                        transactions = manager.GetAllTransactions();
                        depthOfSelection = "";
                        break;
                    case "6":
                        EditTransactions();
                        break;
                    case "7":
                        Settings();
                        break;

                    case "-1":
                        exit = true;                      
                        break;
                }
            }
            return transactions;

       }

        /// <summary>
        /// Displays the settings menu, allowing the user to change the application settings, such as language or manually opening a transaction file.
        /// </summary>
        /// <remarks>
        /// This method provides an interface for user settings adjustments, including language changes and manual file management for transactions.
        /// </remarks>
        private void Settings()
        {
           
           
            string message = $"| {Strings.ChangeLanguage} = 0 | {Strings.OpenTransactionFileManually} = 1 |";
            string dots = new string('-', message.Length);

            Console.WriteLine(Strings.Settings + " " + dots.Substring(Strings.Settings.Length + 1));
            Console.WriteLine(message);
            Console.WriteLine(dots);

            string input2 = Console.ReadLine().Trim();
            switch (input2)
            {
                case "0":
                    ChangeLanguage();
                    break;
                case "1":

                    manager.OpenTransactionFileManually();
                    break;
            }
        }

        /// <summary>
        /// Allows the user to change the application's language setting.
        /// </summary>
        /// <remarks>
        /// Provides a selection for the user to switch between supported languages, affecting the language of the application's user interface.
        /// </remarks>
        private void ChangeLanguage()
        {
            string message = $"| {Strings.German} = 0 | {Strings.English} = 1 |";
            string dots = new string('-', message.Length);

            Console.WriteLine(Strings.ChangeLanguage + " " + dots.Substring(Strings.ChangeLanguage.Length + 1));
            Console.WriteLine(message);
            Console.WriteLine(dots);

            string input2 = Console.ReadLine().Trim();
            switch (input2)
            {
                case "0":
                    LanguageManager.SetLanguage(LanguageManager.Language.German);
                    break;
                case "1":
                    LanguageManager.SetLanguage(LanguageManager.Language.English);
                    break;
            }
        }

        /// <summary>
        /// Provides a menu for editing transactions, including adding, importing, deleting, and batch processing transactions.
        /// </summary>
        /// <remarks>
        /// This method allows the user to perform various transaction editing operations such as adding new transactions, importing transactions from a file, deleting transactions, and batch processing details via partners.
        /// </remarks>
        private void EditTransactions()
        {
            string message1 = $"| {Strings.Addasingletransaction} = 0 | {Strings.importcsvfilefromDKB} = 1 | {Strings.DeleteTransaction} = 2 | {Strings.Batchprocessingoftransactiondetailsviapartners} = 3 |";
            string message2 = $"| {Strings.Showgeneraltransactioninformation} = 4 | {Strings.Generaterandomsampledata} = 5 | {Strings.Exit} = -1";

            int m1adapter = message1.Length - message2.Length - 1;
            string dots = new string('-', message1.Length);

            bool exit = false;

            while (!exit)
            {
                Console.WriteLine(Strings.EditTransactions + " " + dots.Substring(Strings.EditTransactions.Length + 1));
                Console.WriteLine(message1);
                Console.WriteLine(message2 + new string(' ', m1adapter) + "|");
                Console.WriteLine(dots);

                string input = Console.ReadLine().Trim();
                switch (input)
                {
                    case "0":
                        Addasingletransaction();
                        break;
                    case "1":
                        importcsvfilefromDKB();
                        break;
                        case "2":
                        Console.Write($"{Strings.DeleteTransaction} {Strings.Id} = ");
                        int.TryParse(Console.ReadLine().Trim(), out int id);
                        manager.RemoveTransaction(id);
                        break;
                        case "3":
                        BatchEditTransactionDetails();
                        break;
                        case "4":
                            manager.transaktionInformation.DisplayAllListsInComandline();
                        break;
                        case "5":
                        GenerateSampleData();
                        break;
                    case "-1":
                        exit = true;
                        break;

                }
            }
        }

        /// <summary>
        /// Generates sample data for a specified number of months and adds it to the application's transaction list.
        /// </summary>
        /// <remarks>
        /// Prompts the user for the number of months for which to generate sample data, then creates and adds this data to the current list of transactions.
        /// </remarks>
        private void GenerateSampleData()
        {
            Console.Write(Strings.Numberofsampledatamonths + " ");
            int.TryParse(Console.ReadLine().Trim(), out int number);
            DKBCsvDataCreator dKBCsvDataCreator = new DKBCsvDataCreator();
            manager.AddTransactionsFast(parser.ParseCsv(dKBCsvDataCreator.CreateData(number)));
        }

        /// <summary>
        /// Displays options for batch editing transaction details based on certain criteria such as partners, categories, or tags.
        /// </summary>
        /// <remarks>
        /// Offers a variety of batch processing options to modify transaction details collectively, improving data management efficiency.
        /// </remarks>
        private void BatchEditTransactionDetails()
        {
            Console.WriteLine($"{Strings.RenameAllTransactionPartners} = 0");
            Console.WriteLine($"{Strings.RenameAllNullDescriptions} = 1");
            Console.WriteLine($"{Strings.RenameAllNullCurrencies} = 2");
            Console.WriteLine($"{Strings.RenameAllNullPaymentMethods} = 3");
            Console.WriteLine($"{Strings.RenameAllNullCategories} = 4");
            Console.WriteLine($"{Strings.RenameAllNullBudgetCategories} = 5");
            Console.WriteLine($"{Strings.AddTags} = 6");
            Console.WriteLine($"{Strings.RenameAllNullClassifications} = 7");
            Console.WriteLine($"{Strings.RenameAllNullProjects} = 8");
            Console.WriteLine($"{Strings.RenameAllNullStatuses} = 9");
            Console.WriteLine($"{Strings.RenameAllNullPriorities} = 10");
            Console.WriteLine($"{Strings.RenameAllNullFrequencies} = 11");
            Console.WriteLine($"{Strings.RenameAllNullLocations} = 12");         

            string input = Console.ReadLine().Trim();

            switch (input)
            {
                case "0":
                    manager.RenameAllTransactionPartnersInComandline();
                    break;
                case "1":
                    manager.RenameAllNullDescriptionInComandline();
                    break;
                case "2":
                    manager.RenameAllNullCurrencyInComandline();
                    break;
                case "3":
                    manager.RenameAllNullPaymentMethodInComandline();
                    break;
                case "4":
                    manager.RenameAllNullCategoryInComandline();
                    break;
                case "5":
                    manager.RenameAllNullBudgetCategoryInComandline();
                    break;
                case "6":
                    manager.AddTagsInComandline();
                    break;
                case "7":
                    manager.RenameAllNullClassificationInComandline();
                    break;
                case "8":
                    manager.RenameAllNullProjectInComandline();
                    break;
                case "9":
                    manager.RenameAllNullStatusInComandline();
                    break;
                case "10":
                    manager.RenameAllNullPriorityInComandline();
                    break;
                case "11":
                    manager.RenameAllNullFrequencyInComandline();
                    break;
                case "12":
                    manager.RenameAllNullLocationInComandline();
                    break;
                default:
                    Console.WriteLine("Invalid selection.");
                    break;
            }

        }

        /// <summary>
        /// Allows importing of transaction data from a specified CSV file path.
        /// </summary>
        /// <remarks>
        /// This method prompts the user for a file path to import transaction data, supporting both fast and safe import methods.
        /// </remarks>
        private void importcsvfilefromDKB()
        {

            Console.Write(Strings.EnterPath + ": ");
            string input = Console.ReadLine().Trim();

            string message = $"| {Strings.AddTransactionsFast} = 0 | {Strings.AddTransactionsSave} = 1 |";
            string dots = new string('-', message.Length);

            Console.WriteLine(Strings.AddTransactions + " " + dots.Substring(Strings.AddTransactions.Length + 1));
            Console.WriteLine(message);
            Console.WriteLine(dots);

            string input2 = Console.ReadLine().Trim();
            switch (input2)
            {
                case "0":
                    manager.AddTransactionsFast(parser.ParseCsv(input));
                    break;
                case "1":
                   
                    manager.AddTransactionsSave(parser.ParseCsv(input));
                    break;
            }
        }

        /// <summary>
        /// Interactively adds a single transaction to the application's transaction list.
        /// </summary>
        /// <remarks>
        /// Guides the user through entering details for a new transaction, which is then added to the current list of transactions.
        /// </remarks>
        private void Addasingletransaction()
        {
            Transaction transaction = new Transaction();
            Console.WriteLine(Strings.Id + " = " + transaction.Id);

            Console.Write(Strings.Date + " = ");
            DateTime.TryParse(Console.ReadLine().Trim(), out DateTime someDate);
            transaction.Date = someDate;

            Console.Write(Strings.Description + " = ");
            transaction.Description = Console.ReadLine().Trim();

            Console.Write($"{Strings.Amount} = ");
            decimal.TryParse(Console.ReadLine().Trim(), out decimal someAmount);
            transaction.Amount = someAmount;

            Console.Write(Strings.Currency + " = ");
            transaction.Currency = Console.ReadLine().Trim();

            Console.Write(Strings.PaymentMethod + " = ");
            transaction.PaymentMethod = Console.ReadLine().Trim();

            Console.Write($"{Strings.TaxDeductible} = ");
            bool taxDeductible = bool.TryParse(Console.ReadLine().Trim(), out bool deductibleValue) ? deductibleValue : false;
            transaction.TaxDeductible = deductibleValue;

            Console.Write($"{Strings.Reimbursable} = ");
            bool reimbursable = bool.TryParse(Console.ReadLine().Trim(), out bool reimbursableValue) ? reimbursableValue : false;
            transaction.Reimbursable = reimbursableValue;

            Console.Write(Strings.Category + " = ");
            transaction.Category = Console.ReadLine().Trim();

            Console.Write(Strings.BudgetCategory + " = ");
            transaction.BudgetCategory = Console.ReadLine().Trim();

            Console.Write(Strings.Tags + Strings.SeparatedByCommas+ " = ");
            transaction.Tags = Console.ReadLine().Trim().Split(',').ToList();

            Console.Write(Strings.Classification + " = ");
            transaction.Classification = Console.ReadLine().Trim();

            Console.Write(Strings.PartnerIBAN + " = ");
            transaction.PartnerIban = Console.ReadLine().Trim();

            Console.Write(Strings.Partner + " = ");
            transaction.Partner = Console.ReadLine().Trim();

            Console.Write(Strings.Project + " = ");
            transaction.Project = Console.ReadLine().Trim();

            Console.Write(Strings.Status + " = ");
            transaction.Status = Console.ReadLine().Trim();

            Console.Write(Strings.Priority + " = ");
            transaction.Priority = Console.ReadLine().Trim();
            
            Console.Write(Strings.Frequency + " = ");
            transaction.Frequency = Console.ReadLine().Trim();

            Console.Write(Strings.Location + " = ");
            transaction.Location = Console.ReadLine().Trim();

            Console.Write(Strings.Receipt + " " + Strings.Location + " = ");
            transaction.Receipt = Console.ReadLine().Trim();

            Console.Write(Strings.Notes + " = ");
            transaction.Notes = Console.ReadLine().Trim();

           manager.AddTransaction(transaction);

        }

        /// <summary>
        /// Displays the list of transactions in the console, allowing the user to choose between a short view and a complete view.
        /// </summary>
        /// <param name="transactions">The enumerable collection of transactions to display.</param>
        /// <remarks>
        /// The user is prompted to select either a short or complete view of the transactions. Depending on the choice, different details of each transaction are displayed.
        /// </remarks>
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

        /// <summary>
        /// Provides options for refining the selection of transactions based on various criteria like income, expense, amount range, and more.
        /// </summary>
        /// <param name="transactions">The current collection of transactions to be refined.</param>
        /// <returns>The refined collection of transactions after applying the selected criteria.</returns>
        /// <remarks>
        /// This method allows the user to narrow down the transactions based on specific attributes, enhancing the focus on transactions of interest.
        /// </remarks>
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
                    depthOfSelection += Strings.GetIncomeTransactions;
                    break;
                case "1":
                    transactions = manager.GetExpenseTransactions(transactions);
                    depthOfSelection += Strings.GetExpenseTransactions;
                    break;
                case "2":
                    Console.Write($"{Strings.Amount}: ");
                    decimal.TryParse(Console.ReadLine().Trim(), out decimal someAmount);
                    transactions = manager.GetTransactionsByAmount(someAmount, transactions);
                    depthOfSelection += $"{Strings.GetTransactionsByExactAmount} ({someAmount})";
                    break;
                case "3":
                    Console.Write($"{Strings.MinAmount}: ");
                    decimal.TryParse(Console.ReadLine().Trim(), out decimal minAmount);
                    Console.Write($"{Strings.MaxAmount}: ");
                    decimal.TryParse(Console.ReadLine().Trim(), out decimal maxAmount);
                    transactions = manager.GetTransactionsByAmount(minAmount, maxAmount, transactions);
                    depthOfSelection += $"{Strings.GetTransactionsByAmountRange}({minAmount}, {maxAmount})";
                    break;
                case "4":
                    Console.Write($"{Strings.Date}: ");
                    DateTime.TryParse(Console.ReadLine().Trim(), out DateTime someDate);
                    transactions = manager.GetTransactionsByDate(someDate, transactions);
                    depthOfSelection += $"{Strings.GetTransactionsByDate}({someDate})";
                    break;
                case "5":
                    Console.Write($"{Strings.StartDate}: ");
                    DateTime.TryParse(Console.ReadLine().Trim(), out DateTime startDate);
                    Console.Write($"{Strings.EndDate}: ");
                    DateTime.TryParse(Console.ReadLine().Trim(), out DateTime endDate);
                    transactions = manager.GetTransactionsByDate(startDate, endDate, transactions);
                    depthOfSelection += $"{Strings.GetTransactionsByDate}({startDate}, {endDate})";
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
                    depthOfSelection += $"{Strings.GetTransactionsByCurrency}({currency})";
                    break;
                case "8":
                    Console.Write($"{Strings.PaymentMethod}: ");
                    string paymentMethod = Console.ReadLine().Trim();
                    transactions = manager.GetTransactionsByPaymentMethod(paymentMethod, transactions);
                    depthOfSelection += $"{Strings.GetTransactionsByPaymentMethod}({paymentMethod})";
                    break;
                case "9":
                    Console.Write($"{Strings.TaxDeductible}: ");
                    bool taxDeductible = bool.TryParse(Console.ReadLine().Trim(), out bool deductibleValue) ? deductibleValue : false;
                    transactions = manager.GetTaxDeductibleTransactions(taxDeductible, transactions);
                    depthOfSelection += $"{Strings.GetTaxDeductibleTransactions}({taxDeductible})";
                    break;
                case "10":
                    Console.Write($"{Strings.Reimbursable}: ");
                    bool reimbursable = bool.TryParse(Console.ReadLine().Trim(), out bool reimbursableValue) ? reimbursableValue : false;
                    transactions = manager.GetReimbursableTransactions(reimbursable, transactions);
                    depthOfSelection += $"{Strings.GetReimbursableTransactions}({reimbursable})";
                    break;
                case "11":
                    Console.Write($"{Strings.Category}: ");
                    string category = Console.ReadLine().Trim();
                    transactions = manager.GetTransactionsByCategory(category, transactions);
                    depthOfSelection += $"{Strings.GetTransactionsByCategory}({category})";
                    break;
                case "12":
                    Console.Write($"{Strings.BudgetCategory}: ");
                    string budgetCategory = Console.ReadLine().Trim();
                    transactions = manager.GetTransactionsByBudgetCategory(budgetCategory, transactions);
                    depthOfSelection += $"{Strings.GetTransactionsByBudgetCategory}({budgetCategory})";
                    break;
                case "13":
                    Console.Write($"{Strings.Classification}: ");
                    string classification = Console.ReadLine().Trim();
                    transactions = manager.GetTransactionsByClassification(classification, transactions);
                    depthOfSelection += $"{Strings.GetTransactionsByClassification}({classification})";
                    break;
                case "14":
                    Console.Write($"{Strings.Partner}: ");
                    string partner = Console.ReadLine().Trim();
                    transactions = manager.GetTransactionsByPartner(partner, transactions);
                    depthOfSelection += $"{Strings.GetTransactionsByPartner}({partner})";
                    break;
                case "15":
                    Console.Write($"{Strings.Project}: ");
                    string project = Console.ReadLine().Trim();
                    transactions = manager.GetTransactionsByProject(project, transactions);
                    depthOfSelection += $"{Strings.GetTransactionsByProject}({project})";
                    break;
                case "16":
                    Console.Write($"{Strings.Status}: ");
                    string status = Console.ReadLine().Trim();
                    transactions = manager.GetTransactionsByStatus(status, transactions);
                    depthOfSelection += $"{Strings.GetTransactionsByStatus}({status})";
                    break;
                case "17":
                    Console.Write($"{Strings.Priority}: ");
                    string priority = Console.ReadLine().Trim();
                    transactions = manager.GetTransactionsByPriority(priority, transactions);
                    depthOfSelection += $"{Strings.GetTransactionsByPriority}({priority})";
                    break;
                case "18":
                    Console.Write($"{Strings.Frequency}: ");
                    string frequency = Console.ReadLine().Trim();
                    transactions = manager.GetTransactionsByFrequency(frequency, transactions);
                    depthOfSelection += $"{Strings.GetTransactionsByFrequency}({frequency})";
                    break;
                case "19":
                    Console.Write($"{Strings.Location}: ");
                    string location = Console.ReadLine().Trim();
                    transactions = manager.GetTransactionsByLocation(location, transactions);
                    depthOfSelection += $"{Strings.GetTransactionsByLocation}({location})";
                    break;
                case "20":
                    Console.Write($"{Strings.Tag}: ");
                    string tag = Console.ReadLine().Trim();
                    transactions = manager.GetTransactionsByTag(tag, transactions);
                    depthOfSelection += $"{Strings.GetTransactionsByTag}({tag})";
                    break;
                default:
                    Console.WriteLine("Invalid input.");
                    break;
            }


            depthOfSelection += "> ";

            return transactions;

        }

        /// <summary>
        /// Offers sorting options for the list of transactions, including by ID, date, amount, and other attributes in both ascending and descending order.
        /// </summary>
        /// <param name="transactions">The collection of transactions to sort.</param>
        /// <returns>The sorted collection of transactions according to the user's choice.</returns>
        /// <remarks>
        /// Sorting can be done on various fields, allowing the user to organize the transaction data more effectively for analysis or display.
        /// </remarks>
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

        /// <summary>
        /// Allows the user to group transactions by different attributes such as date, amount range, currency, and more.
        /// </summary>
        /// <param name="transactions">The collection of transactions to be grouped.</param>
        /// <returns>A collection of groups, each containing transactions that share a common attribute.</returns>
        /// <remarks>
        /// This method provides a versatile way to categorize and examine transactions based on shared characteristics.
        /// </remarks>
        private IEnumerable<IGrouping<string, Transaction>> Group(IEnumerable<Transaction> transactions)
        {
            string message = $"| {Strings.Show} = 0 | {Strings.Group} = 1 | {Strings.AnalyzeGroups} = 2 | {Strings.Reset} = 3 | {Strings.Exit} = -1 |";
            string dots = new string('-', message.Length);
            bool exit = false;
            IEnumerable<IGrouping<string, Transaction>> groupedTransactions = manager.GroupByPartner(transactions);
            string tempDepthOfSelection = depthOfSelection;

            
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
                        depthOfSelection = tempDepthOfSelection;
                        groupedTransactions = GroupTransactions(transactions);                       
                        break;

                    case "2":                       
                        AnalyzeGroups(groupedTransactions);
                        break;

                    case "3":
                        groupedTransactions = manager.GroupByPartner();
                        depthOfSelection = tempDepthOfSelection;
                        break;
                    case "-1":
                        exit = true;
                        groupedTransactions = manager.GroupByPartner();
                        transactions = manager.GetAllTransactions();
                        depthOfSelection = tempDepthOfSelection;
                        break;
                }
            }
            return groupedTransactions;

        }

        /// <summary>
        /// Analyzes grouped transactions, calculating aggregates such as average amount, total income, total expenses, and more.
        /// </summary>
        /// <param name="groupedTransactions">The collection of grouped transactions to analyze.</param>
        /// <remarks>
        /// This method offers insights into the grouped transactions, enabling the user to perform quantitative analysis on different transaction categories.
        /// </remarks>
        private void AnalyzeGroups(IEnumerable<IGrouping<string, Transaction>> groupedTransactions)
        {
            Console.WriteLine($"{Strings.CalculateGroupedAverageAmount} = 0");
            Console.WriteLine($"{Strings.CalculateGroupedAverageIncome} = 1");
            Console.WriteLine($"{Strings.CalculateGroupedAverageExpense} = 2");
            Console.WriteLine($"{Strings.CalculateGroupedExpenses} = 3");
            Console.WriteLine($"{Strings.CalculateGroupedIncome} = 4");
            Console.WriteLine($"{Strings.CalculateGroupedAmount} = 5");
            Console.WriteLine($"{Strings.CalculateGroupedPercentageOfTotalIncome} = 6");
            Console.WriteLine($"{Strings.CalculateGroupedPercentageOfTotalExpanses} = 7");
            Console.WriteLine($"{Strings.GetGroupedTotalTransactionCount} = 8");

            Console.Write(Strings.Selection + " = ");
            string input = Console.ReadLine().Trim();
            IEnumerable<(string key, decimal Result)> results = null;
            switch (input)
            {
                case "0":
                    results = analyzer.CalculateGroupedAverageAmount(groupedTransactions);
                   
                    break;
                case "1":
                    results = analyzer.CalculateGroupedAverageIncome(groupedTransactions);
                    
                    break;
                case "2":
                    results = analyzer.CalculateGroupedAverageExpense(groupedTransactions);
              
                    break;
                case "3":
                    results = analyzer.CalculateGroupedExpenses(groupedTransactions);
                   
                    break;
                case "4":
                    results = analyzer.CalculateGroupedIncome(groupedTransactions);
                   
                    break;
                case "5":
                    results = analyzer.CalculateGroupedAmount(groupedTransactions);
                 
                    break;
                case "6":
                    results = analyzer.CalculateGroupedPercentageOfTotalIncome(groupedTransactions);
                   
                    break;
                case "7":
                    results = analyzer.CalculateGroupedPercentageOfTotalExpanses(groupedTransactions);
                   
                    break;
                case "8":
                    results = analyzer.GetGroupedTotalTransactionCount(groupedTransactions);
                  
                    break;
                default:
                    Console.WriteLine("Invalid selection.");
                    break;
            }

            depthOfSelection += ">";

            ShowGroupedAnalysis(results);


        }

        /// <summary>
        /// Displays the results of grouped transaction analyses, such as averages and totals, in the console.
        /// </summary>
        /// <param name="results">The collection of analysis results to display, each associated with a group key.</param>
        /// <remarks>
        /// The analysis results are presented in a structured format, allowing the user to easily review the calculated metrics for each group of transactions.
        /// </remarks>
        private void ShowGroupedAnalysis(IEnumerable<(string key, decimal Result)> results)
        {
            string message = $"| {Strings.View} = 0 | {Strings.Sort} = 1 | {Strings.Exit} = -1";
            string dots = new string('-', message.Length);
            string tempDepthOfSelection = depthOfSelection;


            bool exit = false;

            while(!exit){
                Console.WriteLine($"|>... {depthOfSelection}|");
                Console.WriteLine(Strings.AnalyzeGroups + " " + Strings.Result + " " + dots.Substring(Strings.AnalyzeGroups.Length + Strings.Result.Length + 2));
                Console.WriteLine(message);
                Console.WriteLine(dots);
                string input = Console.ReadLine().Trim();
                switch (input)
                {
                    case "0":
                        analyzer.DisplayGroupedAmountInComandline(results);
                        break;
                    case "1":
                        results = SortGroupedResult(results);
                        break;
                    case "-1":
                        exit = true;
                        depthOfSelection = tempDepthOfSelection;
                        break;
                }

            }
        }

        /// <summary>
        /// Provides options to sort the results of a grouped transaction analysis by key or by result in ascending or descending order.
        /// </summary>
        /// <param name="results">The analysis results to sort, each consisting of a group key and a numeric result.</param>
        /// <returns>The sorted collection of analysis results.</returns>
        /// <remarks>
        /// Sorting the results can help in identifying the highest or lowest values quickly, aiding in data interpretation and decision-making.
        /// </remarks>
        private IEnumerable<(string key, decimal Result)> SortGroupedResult(IEnumerable<(string key, decimal Result)> results)
        {
            Console.WriteLine($"{Strings.SortByAmountAscending} = 0");
            Console.WriteLine($"{Strings.SortByAmountDescending} = 1");
            Console.WriteLine($"{Strings.SortByKeyAscending} = 2");
            Console.WriteLine($"{Strings.SortByKeyDescending} = 3");
            Console.Write(Strings.Selection + " = ");
            string input = Console.ReadLine().Trim();
           
            switch (input)
            {
                case "0":
                    results = analyzer.SortByAmountAscending(results);
                    depthOfSelection += Strings.SortByAmountAscending;
                    break;
                case "1":
                    results = analyzer.SortByAmountDescending(results);
                    depthOfSelection += Strings.SortByAmountDescending;

                    break;
                case "2":
                    results = analyzer.SortByKeyAscending(results);
                    depthOfSelection += Strings.SortByKeyAscending;

                    break;
                case "3":
                    results = analyzer.SortByKeyDescending(results);
                    depthOfSelection += Strings.SortByKeyDescending;

                    break;
                default:
                    Console.WriteLine("Invalid selection.");
                    break;
            }

            depthOfSelection += "> ";

            return results;
        }

        /// <summary>
        /// Offers options for displaying grouped transactions, either by listing all transactions within each group or by showing only the group names.
        /// </summary>
        /// <param name="groupedTransactions">The collection of grouped transactions to display.</param>
        /// <remarks>
        /// This function enhances data visibility by allowing the user to review transaction groups in detail or to get an overview of the groups.
        /// </remarks>
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

        /// <summary>
        /// Prompts the user to select a grouping criterion from a set of transaction attributes, then groups the transactions accordingly.
        /// </summary>
        /// <param name="transactions">The collection of transactions to group.</param>
        /// <returns>A collection of groups formed by grouping transactions based on the selected attribute.</returns>
        /// <remarks>
        /// The user can choose to group transactions by various attributes like date, description, amount, etc., facilitating targeted analysis.
        /// </remarks>
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

            depthOfSelection += "> ";
            return groupedTransactions;
        }

        /// <summary>
        /// Provides statistical analyses options for transactions, including total, average, variance, and more.
        /// </summary>
        /// <param name="transactions">The collection of transactions to analyze.</param>
        /// <remarks>
        /// This function allows the user to compute various statistical metrics for the transactions, offering insights into the financial data.
        /// </remarks>
        private void AnalyzeTransactions(IEnumerable<Transaction> transactions)
        {
            Console.WriteLine($"{Strings.CalculateTotalAmount} = 0");
            Console.WriteLine($"{Strings.CalculateTotalIncome} = 1");
            Console.WriteLine($"{Strings.CalculateTotalExpenses} = 2");
            Console.WriteLine($"{Strings.CalculateAverageAmount} = 3");
            Console.WriteLine($"{Strings.CalculateAverageIncome} = 4");
            Console.WriteLine($"{Strings.CalculateAverageExpense} = 5");
            Console.WriteLine($"{Strings.CalculateVariance} = 6");
            Console.WriteLine($"{Strings.CalculateStandardDeviation} = 7");
            Console.WriteLine($"{Strings.CalculateMedian} = 8");
            Console.WriteLine($"{Strings.CalculateMode} = 9");
            Console.WriteLine($"{Strings.CalculateQuantile} = 10");
            Console.WriteLine($"{Strings.GetTotalTransactionCount} = 11");

            Console.Write(Strings.Selection + " = ");
            string input = Console.ReadLine().Trim();


            string message = string.Empty;

            switch (input)
            {
                case "0":
                    decimal totalAmount = analyzer.CalculateTotalAmount(transactions);
                    message = $"{Strings.CalculateTotalAmount} = {totalAmount}";
                    break;
                case "1":
                    decimal totalIncome = analyzer.CalculateTotalIncome(transactions);
                    message = $"{Strings.CalculateTotalIncome} = {totalIncome}";
                    break;
                case "2":
                    decimal totalExpenses = analyzer.CalculateTotalExpenses(transactions);
                    message = $"{Strings.CalculateTotalExpenses} = {totalExpenses}";
                    break;
                case "3":
                    decimal averageAmount = analyzer.CalculateAverageAmount(transactions);
                    message = $"{Strings.CalculateAverageAmount} = {averageAmount}";
                    break;
                case "4":
                    decimal averageIncome = analyzer.CalculateAverageIncome(transactions);
                    message = $"{Strings.CalculateAverageIncome} = {averageIncome}";
                    break;
                case "5":
                    decimal averageExpense = analyzer.CalculateAverageExpense(transactions);
                    message = $"{Strings.CalculateAverageExpense} = {averageExpense}";
                    break;
                case "6":
                    decimal variance = analyzer.CalculateVariance(transactions);
                    message = $"{Strings.CalculateVariance} = {variance}";
                    break;
                case "7":
                    decimal standardDeviation = analyzer.CalculateStandardDeviation(transactions);
                    message = $"{Strings.CalculateStandardDeviation} = {standardDeviation}";
                    break;
                case "8":
                    decimal median = analyzer.CalculateMedian(transactions);
                    message = $"{Strings.CalculateMedian} = {median}";
                    break;
                case "9":
                    decimal mode = analyzer.CalculateMode(transactions);
                    message = $"{Strings.CalculateMode} = {mode}";
                    break;
                case "10":                  
                    Console.Write(Strings.EnterQuantile + ": ");
                    double quantile;
                    if (!double.TryParse(Console.ReadLine(), out quantile) || quantile < 0 || quantile > 1)
                    {
                        Console.WriteLine(Strings.InvalidQuantile);
                        break;
                    }

                    decimal calculatedQuantile = analyzer.CalculateQuantile(transactions, quantile);
                    message = $"{Strings.CalculateQuantile} = {calculatedQuantile}";

                    break;
                case "11":
                    decimal count = analyzer.GetTotalTransactionCount(transactions);
                    message = $"{Strings.GetTotalTransactionCount} = {count}";
                    break;
                default:
                    message = "Invalid selection.";
                    break;
            }

            string dots = new string('-', message.Length+4);
            Console.WriteLine(dots);
            Console.WriteLine($"| {message} |");
            Console.WriteLine(dots);
        }

    }
}
