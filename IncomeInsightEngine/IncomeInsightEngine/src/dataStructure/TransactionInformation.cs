using dataStructure;
using IncomeInsightEngine.Properties;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IncomeInsightEngine.src.dataStructure
{
    public class TransactionInformation
    {
        public List<string> Currency { get; } = new List<string>();
        public List<string> PaymentMethod { get; } = new List<string>();
        public List<string> Category { get; } = new List<string>();
        public List<string> BudgetCategory { get; } = new List<string>();
        public List<string> Tags { get; } = new List<string>();
        public List<string> Classification { get; } = new List<string>();
        public List<string> Partner { get; } = new List<string>();
        public List<string> Project { get; } = new List<string>();
        public List<string> Status { get; } = new List<string>();
        public List<string> Priority { get; } = new List<string>();
        public List<string> Frequency { get; } = new List<string>();
        public List<string> Location { get; } = new List<string>();

        public TransactionInformation(IEnumerable<Transaction> transactions)
        {
            LoadData(transactions);
        }


        // Generic Add, Delete, Edit
        private void AddItem(List<string> list, string item) => list.Add(item);
        private bool DeleteItem(List<string> list, string item) => list.Remove(item);
        private bool EditItem(List<string> list, string currentItem, string newItem)
        {
            int index = list.IndexOf(currentItem);
            if (index != -1)
            {
                list[index] = newItem;
                return true;
            }
            return false;
        }    

        // Currency
        public void AddCurrency(string item) => AddItem(Currency, item);
        public bool DeleteCurrency(string item) => DeleteItem(Currency, item);
        public bool EditCurrency(string currentItem, string newItem) => EditItem(Currency, currentItem, newItem);

        // PaymentMethod
        public void AddPaymentMethod(string item) => AddItem(PaymentMethod, item);
        public bool DeletePaymentMethod(string item) => DeleteItem(PaymentMethod, item);
        public bool EditPaymentMethod(string currentItem, string newItem) => EditItem(PaymentMethod, currentItem, newItem);

        // Category
        public void AddCategory(string item) => AddItem(Category, item);
        public bool DeleteCategory(string item) => DeleteItem(Category, item);
        public bool EditCategory(string currentItem, string newItem) => EditItem(Category, currentItem, newItem);

        // BudgetCategory
        public void AddBudgetCategory(string item) => AddItem(BudgetCategory, item);
        public bool DeleteBudgetCategory(string item) => DeleteItem(BudgetCategory, item);
        public bool EditBudgetCategory(string currentItem, string newItem) => EditItem(BudgetCategory, currentItem, newItem);

        // Tags
        public void AddTag(string item) => AddItem(Tags, item);
        public bool DeleteTag(string item) => DeleteItem(Tags, item);
        public bool EditTag(string currentItem, string newItem) => EditItem(Tags, currentItem, newItem);

        // Classification
        public void AddClassification(string item) => AddItem(Classification, item);
        public bool DeleteClassification(string item) => DeleteItem(Classification, item);
        public bool EditClassification(string currentItem, string newItem) => EditItem(Classification, currentItem, newItem);

        // Partner
        public void AddPartner(string item) => AddItem(Partner, item);
        public bool DeletePartner(string item) => DeleteItem(Partner, item);
        public bool EditPartner(string currentItem, string newItem) => EditItem(Partner, currentItem, newItem);

        // Project
        public void AddProject(string item) => AddItem(Project, item);
        public bool DeleteProject(string item) => DeleteItem(Project, item);
        public bool EditProject(string currentItem, string newItem) => EditItem(Project, currentItem, newItem);

        // Status
        public void AddStatus(string item) => AddItem(Status, item);
        public bool DeleteStatus(string item) => DeleteItem(Status, item);
        public bool EditStatus(string currentItem, string newItem) => EditItem(Status, currentItem, newItem);

        // Priority
        public void AddPriority(string item) => AddItem(Priority, item);
        public bool DeletePriority(string item) => DeleteItem(Priority, item);
        public bool EditPriority(string currentItem, string newItem) => EditItem(Priority, currentItem, newItem);

        // Frequency
        public void AddFrequency(string item) => AddItem(Frequency, item);
        public bool DeleteFrequency(string item) => DeleteItem(Frequency, item);
        public bool EditFrequency(string currentItem, string newItem) => EditItem(Frequency, currentItem, newItem);

        // Location
        public void AddLocation(string item) => AddItem(Location, item);
        public bool DeleteLocation(string item) => DeleteItem(Location, item);
        public bool EditLocation(string currentItem, string newItem) => EditItem(Location, currentItem, newItem);

        /// <summary>
        /// Loads various attributes from a collection of transactions into distinct data sets for analysis and processing.
        /// </summary>
        /// <param name="listOfTransactions">The collection of transactions to be processed.</param>
        /// <remarks>
        /// This method delegates to specific loaders for each attribute type (e.g., Currency, PaymentMethod, Category), ensuring that all relevant data from the transactions are loaded into separate, distinct collections for further use.
        /// </remarks>
        public void LoadData(IEnumerable<Transaction> listOfTransactions)
        {
            Currency.Clear();
            PaymentMethod.Clear();
            Category.Clear();
            BudgetCategory.Clear();
            Tags.Clear();
            Classification.Clear();
            Partner.Clear();
            Project.Clear();
            Status.Clear();
            Priority.Clear();
            Frequency.Clear();
            Location.Clear();

            LoadCurrency(listOfTransactions);
            LoadPaymentMethod(listOfTransactions);
            LoadCategory(listOfTransactions);
            LoadBudgetCategory(listOfTransactions);
            LoadTags(listOfTransactions);
            LoadClassification(listOfTransactions);
            LoadPartner(listOfTransactions);
            LoadProject(listOfTransactions);
            LoadStatus(listOfTransactions);
            LoadPriority(listOfTransactions);
            LoadFrequency(listOfTransactions);
            LoadLocation(listOfTransactions);
        }

        /// <summary>
        /// Extracts and loads unique currency data from a collection of transactions.
        /// </summary>
        /// <param name="listOfTransactions">The collection of transactions from which to load currency data.</param>
        /// <remarks>
        /// Currencies are selected from the transactions, filtered for uniqueness, and added to the Currency collection, excluding duplicates.
        /// </remarks>
        private void LoadCurrency(IEnumerable<Transaction> listOfTransactions)
        {
            var currenciesToAdd = listOfTransactions.Select(t => t.Currency)
                                                    .Where(c => !Currency.Contains(c))
                                                    .Distinct();

            foreach (var currency in currenciesToAdd)
            {
                Currency.Add(currency);
            }
        }

        /// <summary>
        /// Loads unique payment methods from a collection of transactions into the PaymentMethod collection.
        /// </summary>
        /// <param name="listOfTransactions">The collection of transactions to extract payment methods from.</param>
        /// <remarks>
        /// Identifies distinct payment methods within the transactions, ensuring no duplicates are added to the PaymentMethod collection.
        /// </remarks>
        private void LoadPaymentMethod(IEnumerable<Transaction> listOfTransactions)
        {
            var paymentMethodsToAdd = listOfTransactions.Select(t => t.PaymentMethod)
                                                        .Where(pm => !PaymentMethod.Contains(pm))
                                                        .Distinct();

            foreach (var paymentMethod in paymentMethodsToAdd)
            {
                PaymentMethod.Add(paymentMethod);
            }
        }

        /// <summary>
        /// Extracts and loads unique categories from a given collection of transactions.
        /// </summary>
        /// <param name="listOfTransactions">The collection of transactions to analyze for category data.</param>
        /// <remarks>
        /// Categories are extracted from transactions, filtered for uniqueness, and added to the Category collection, avoiding repetition.
        /// </remarks>
        private void LoadCategory(IEnumerable<Transaction> listOfTransactions)
        {
            var categoriesToAdd = listOfTransactions.Select(t => t.Category)
                                                    .Where(c => !Category.Contains(c))
                                                    .Distinct();

            foreach (var category in categoriesToAdd)
            {
                Category.Add(category);
            }
        }

        /// <summary>
        /// Loads unique budget categories from a specified collection of transactions.
        /// </summary>
        /// <param name="listOfTransactions">The collection of transactions from which to extract budget category data.</param>
        /// <remarks>
        /// Distinct budget categories are identified and added to the BudgetCategory collection, with duplicate entries excluded.
        /// </remarks>
        private void LoadBudgetCategory(IEnumerable<Transaction> listOfTransactions)
        {
            var budgetCategoriesToAdd = listOfTransactions.Select(t => t.BudgetCategory)
                                                           .Where(bc => !BudgetCategory.Contains(bc))
                                                           .Distinct();

            foreach (var budgetCategory in budgetCategoriesToAdd)
            {
                BudgetCategory.Add(budgetCategory);
            }
        }

        /// <summary>
        /// Extracts and aggregates unique tags from a collection of transactions.
        /// </summary>
        /// <param name="listOfTransactions">The transactions to parse for tag data.</param>
        /// <remarks>
        /// All tags across the transactions are collected, deduplicated, and added to the Tags collection, ensuring a comprehensive list of unique tags.
        /// </remarks>
        private void LoadTags(IEnumerable<Transaction> listOfTransactions)
        {
            var tagsToAdd = listOfTransactions.SelectMany(t => t.Tags) 
                                              .Where(tag => !Tags.Contains(tag))
                                              .Distinct();

            foreach (var tag in tagsToAdd)
            {
                Tags.Add(tag);
            }
        }

        /// <summary>
        /// Loads distinct classifications from a collection of transactions.
        /// </summary>
        /// <param name="listOfTransactions">The transactions from which to extract classification data.</param>
        /// <remarks>
        /// Identifies unique classifications from the provided transactions, adding them to the Classification collection while avoiding duplicates.
        /// </remarks>
        private void LoadClassification(IEnumerable<Transaction> listOfTransactions)
        {
            var classificationsToAdd = listOfTransactions.Select(t => t.Classification)
                                                          .Where(cl => !Classification.Contains(cl))
                                                          .Distinct();

            foreach (var classification in classificationsToAdd)
            {
                Classification.Add(classification);
            }
        }

        /// <summary>
        /// Loads unique partner names from a specified collection of transactions.
        /// </summary>
        /// <param name="listOfTransactions">The collection of transactions to analyze for partner data.</param>
        /// <remarks>
        /// Extracts distinct partner names from transactions and adds them to the Partner collection, ensuring no duplicates are included.
        /// </remarks>
        private void LoadPartner(IEnumerable<Transaction> listOfTransactions)
        {
            var partnersToAdd = listOfTransactions.Select(t => t.Partner)
                                                  .Where(p => !Partner.Contains(p))
                                                  .Distinct();

            foreach (var partner in partnersToAdd)
            {
                Partner.Add(partner);
            }
        }

        /// <summary>
        /// Loads unique project names from a collection of transactions.
        /// </summary>
        /// <param name="listOfTransactions">The collection of transactions from which to extract project data.</param>
        /// <remarks>
        /// Identifies distinct project names within the transactions and adds them to the Project collection, excluding any duplicates.
        /// </remarks>
        private void LoadProject(IEnumerable<Transaction> listOfTransactions)
        {
            var projectsToAdd = listOfTransactions.Select(t => t.Project)
                                                  .Where(p => !Project.Contains(p))
                                                  .Distinct();

            foreach (var project in projectsToAdd)
            {
                Project.Add(project);
            }
        }

        /// <summary>
        /// Loads unique status labels from a specified collection of transactions.
        /// </summary>
        /// <param name="listOfTransactions">The collection of transactions to analyze for status data.</param>
        /// <remarks>
        /// Distinct status labels are extracted and added to the Status collection, ensuring no duplicates are added.
        /// </remarks>
        private void LoadStatus(IEnumerable<Transaction> listOfTransactions)
        {
            var statusToAdd = listOfTransactions.Select(t => t.Status)
                                                .Where(s => !Status.Contains(s))
                                                .Distinct();

            foreach (var status in statusToAdd)
            {
                Status.Add(status);
            }
        }

        /// <summary>
        /// Extracts and loads unique priority levels from a collection of transactions.
        /// </summary>
        /// <param name="listOfTransactions">The collection of transactions from which to load priority data.</param>
        /// <remarks>
        /// Priorities are selected from the transactions, filtered for uniqueness, and added to the Priority collection, excluding duplicates.
        /// </remarks>
        private void LoadPriority(IEnumerable<Transaction> listOfTransactions)
        {
            var prioritiesToAdd = listOfTransactions.Select(t => t.Priority)
                                                    .Where(p => !Priority.Contains(p))
                                                    .Distinct();

            foreach (var priority in prioritiesToAdd)
            {
                Priority.Add(priority);
            }
        }

        /// <summary>
        /// Loads distinct frequency types from a collection of transactions.
        /// </summary>
        /// <param name="listOfTransactions">The collection of transactions to analyze for frequency data.</param>
        /// <remarks>
        /// Frequencies are identified, deduplicated, and added to the Frequency collection, avoiding repetition of the same frequency types.
        /// </remarks>
        private void LoadFrequency(IEnumerable<Transaction> listOfTransactions)
        {
            var frequenciesToAdd = listOfTransactions.Select(t => t.Frequency)
                                                     .Where(f => !Frequency.Contains(f))
                                                     .Distinct();

            foreach (var frequency in frequenciesToAdd)
            {
                Frequency.Add(frequency);
            }
        }

        /// <summary>
        /// Extracts and loads unique location information from a given collection of transactions.
        /// </summary>
        /// <param name="listOfTransactions">The collection of transactions to analyze for location data.</param>
        /// <remarks>
        /// Locations are extracted from transactions, filtered for uniqueness, and added to the Location collection, avoiding duplicates.
        /// </remarks>
        private void LoadLocation(IEnumerable<Transaction> listOfTransactions)
        {
            var locationsToAdd = listOfTransactions.Select(t => t.Location)
                                                   .Where(l => !Location.Contains(l))
                                                   .Distinct();

            foreach (var location in locationsToAdd)
            {
                Location.Add(location);
            }
        }

        /// <summary>
        /// Displays all loaded lists (Currencies, Payment Methods, Categories, etc.) in the command line.
        /// </summary>
        /// <remarks>
        /// Iterates through each list associated with transactions, such as Currency, PaymentMethod, Category, and more, displaying their contents. Each list is preceded by its name for clarity.
        /// </remarks>
        public void DisplayAllListsInComandline()
        {
            Console.WriteLine(Strings.Currencies);
            foreach (var currency in Currency)
            {
                Console.WriteLine($"- {currency}");
            }

            Console.WriteLine("\n" + Strings.PaymentMethods);
            foreach (var method in PaymentMethod)
            {
                Console.WriteLine($"- {method}");
            }

            Console.WriteLine("\n" + Strings.Categories);
            foreach (var category in Category)
            {
                Console.WriteLine($"- {category}");
            }

            Console.WriteLine("\n" + Strings.BudgetCategories);
            foreach (var budgetCategory in BudgetCategory)
            {
                Console.WriteLine($"- {budgetCategory}");
            }

            Console.WriteLine("\n" + Strings.Tags);
            foreach (var tag in Tags)
            {
                Console.WriteLine($"- {tag}");
            }

            Console.WriteLine("\n" + Strings.Classifications);
            foreach (var classification in Classification)
            {
                Console.WriteLine($"- {classification}");
            }

            Console.WriteLine("\n" + Strings.Partners);
            foreach (var partner in Partner)
            {
                Console.WriteLine($"- {partner}");
            }

            Console.WriteLine("\n" + Strings.Projects);
            foreach (var project in Project)
            {
                Console.WriteLine($"- {project}");
            }

            Console.WriteLine("\n" + Strings.Statuses);
            foreach (var status in Status)
            {
                Console.WriteLine($"- {status}");
            }

            Console.WriteLine("\n" + Strings.Priorities);
            foreach (var priority in Priority)
            {
                Console.WriteLine($"- {priority}");
            }

            Console.WriteLine("\n" + Strings.Frequencies);
            foreach (var frequency in Frequency)
            {
                Console.WriteLine($"- {frequency}");
            }

            Console.WriteLine("\n" + Strings.Locations);
            foreach (var location in Location)
            {
                Console.WriteLine($"- {location}");
            }
        }
    }
}
