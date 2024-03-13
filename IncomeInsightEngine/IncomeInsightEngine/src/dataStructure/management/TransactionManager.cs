using dataStructure;
using IncomeInsightEngine.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace IncomeInsightEngine.src.dataStructure.management
{
    /// <summary>
    /// Manages transactions including their creation, modification, and deletion, as well as sorting, retrieval based on specific criteria and displaying transaction information.
    /// </summary>
    /// <remarks>
    /// <para>The TransactionManager class serves as a comprehensive controller for handling transaction data within the application. It encapsulates functionality for:</para>
    /// <list type="bullet">
    ///<item><description>Creating, updating, and deleting transactions to ensure accurate and up-to-date transaction records.</description></item>
    /// <item><description>Retrieving transactions based on a variety of filters such as ID, date, amount, description, currency, payment method, category, partner, and more, allowing for detailed analysis and reporting.</description></item>
    /// <item><description>Sorting transactions by key attributes like ID, date, amount, partner, and location in both ascending and descending order, facilitating organized views and insights.</description></item>
    /// <item><description>Displaying transaction details directly to the command line, offering quick access to transaction summaries, comprehensive details, and aggregate financial metrics such as total income, expenses, and net amounts.</description></item>
    /// <item><description>Manually controlling the opening and closing of transaction files, thus managing when transaction data is accessed and secured, leveraging the FileEncryptor for encryption.</description></item>
    /// </list>
    /// <para>This class interacts with the JsonTransaction class for persistence, ensuring that transactions are stored and retrieved from a JSON-formatted file, and utilizes the FileEncryptor class for securing transaction data at rest.</para>
    /// <para>By centralizing transaction management within this class, it simplifies the interface for other components of the application to interact with transaction data, ensuring consistent application logic and data integrity.</para>
    /// </remarks>
    public class TransactionManager
    {   
        private List<Transaction> transactions = new List<Transaction>();
        private JsonTransaction jsonTransaction;

        /// <summary>
        /// Initializes a new instance of the TransactionManager class, setting up the JsonTransaction instance and loading existing transactions.
        /// </summary>
        /// <remarks>
        /// This constructor creates a new JsonTransaction object responsible for managing the transaction data stored in a JSON file. 
        /// After initializing the JsonTransaction object, it calls the LoadTransactions method to load existing transactions into memory, 
        /// making them readily available for processing. This setup ensures that the TransactionManager is immediately ready to handle 
        /// transaction data upon instantiation.
        /// </remarks>
        public TransactionManager()
        {
            jsonTransaction = new JsonTransaction();
          
            LoadTransactions();
        }

        /// <summary>
        /// Loads transaction data from the JSON storage file into the local transactions collection.
        /// </summary>
        /// <remarks>
        /// This method calls the ReadData method of the JsonTransaction instance to fetch the list of transactions stored in the JSON file.
        /// It then assigns this list to the local transactions collection, making it available for further processing within the class.
        /// This approach encapsulates the data loading logic, ensuring that the transaction data is up-to-date and accurately reflects the contents of the storage file.
        /// </remarks>
        private void LoadTransactions()
        {                     
                transactions = jsonTransaction.ReadData();        
        }
        
        /// <summary>
        /// Adds a new transaction to the collection if it does not already exist and is not null.
        /// </summary>
        /// <param name="transaction">The Transaction object to add.</param>
        /// <returns>True if the transaction was successfully added; otherwise, false.</returns>
        /// <remarks>
        /// This method first checks if the provided transaction object is null, already exists in the current collection, 
        /// or has an ID that matches any existing transaction's ID. If any of these conditions are true, the method returns false,
        /// indicating that the transaction will not be added. If none of these conditions are met, the transaction is added
        /// to the local collection and also saved to the JSON file using JsonTransaction.AddTransaction method. The method
        /// returns true to indicate successful addition of the transaction.
        /// </remarks>
        public bool AddTransaction(Transaction transaction)
        {
            if (transaction == null || DoesTransactionAlreadyExists(transaction) || GetTransactionById(transaction.Id) != null)
            {
              return false;
            }

            transactions.Add(transaction);
            jsonTransaction.AddTransaction(transaction);
            return true;
        }

        /// <summary>
        /// Adds a list of new transactions to the collection, ensuring each transaction is unique and not null.
        /// </summary>
        /// <param name="data">The list of Transaction objects to be added.</param>
        /// <returns>True if at least one transaction was successfully added; otherwise, false.</returns>
        /// <remarks>
        /// This method processes each transaction in the provided list individually. It checks if the transaction is null, 
        /// already exists in the collection, or has a duplicate ID. If any of these conditions are met, the method skips the 
        /// current transaction and continues with the next one. Only transactions that pass these checks are added to both the 
        /// local collection and saved to the JSON file using JsonTransaction.AddTransaction method.
        /// </remarks>
        public bool AddTransactions(IEnumerable<Transaction> data)
        {
            if(data == null)
            {
                return false;
            }

            foreach (var transaction in data)
            {

                if (transaction == null || DoesTransactionAlreadyExists(transaction) || GetTransactionById(transaction.Id) != null)
                {
                    continue;
                }

                transactions.Add(transaction);
                jsonTransaction.AddTransaction(transaction);        
            }
            return true;
        }

        /// <summary>
        /// Removes a transaction from the collection based on its ID and updates the JSON file.
        /// </summary>
        /// <param name="id">The ID of the transaction to be removed.</param>
        /// <returns>True if the transaction was successfully removed and the JSON file updated; otherwise, false.</returns>
        /// <remarks>
        /// This method searches for a transaction with the specified ID within the current collection of transactions. If a transaction with the
        /// given ID is found, it is removed from the collection. Then, the updated collection of transactions is saved to the JSON file using
        /// the SaveData method of the JsonTransaction class to reflect the removal. The method returns true to indicate successful removal and
        /// update. If no transaction with the specified ID exists in the collection, the method returns false, indicating that no action was taken.
        /// </remarks>
        public bool RemoveTransaction(int id)
        {
            var transaction = GetTransactionById(id);
            if (transaction != null)
            {
                transactions.Remove(transaction);
                jsonTransaction.SaveData(transactions);
                return true;
            }
            return false;
        }
       
        /// <summary>
        /// Updates an existing transaction in the collection with new details provided in the updatedTransaction object.
        /// </summary>
        /// <param name="updatedTransaction">The transaction object containing the updated details.</param>
        /// <returns>True if the transaction was successfully updated; otherwise, false.</returns>
        /// <remarks>
        /// This method looks for an existing transaction with the same ID as the updatedTransaction. If no such transaction is found,
        /// it returns false, indicating the transaction does not exist and cannot be updated. If the transaction is found, it updates all
        /// properties of the existing transaction with values from the updatedTransaction object. After updating the transaction, it saves
        /// the entire collection of transactions to the JSON file to ensure the update is persisted. This method ensures that the transaction
        /// details are fully synchronized with the provided updatedTransaction object.
        /// </remarks>
        public bool UpdateTransaction(Transaction updatedTransaction)
        {
            var transaction = GetTransactionById(updatedTransaction.Id);
            if (transaction == null)
            {               
                return false;
            }
        
            transaction.Date = updatedTransaction.Date;
            transaction.Amount = updatedTransaction.Amount;
            transaction.Category = updatedTransaction.Category;
            transaction.Description = updatedTransaction.Description;
            transaction.PaymentMethod = updatedTransaction.PaymentMethod;
            transaction.Frequency = updatedTransaction.Frequency;
            transaction.Location = updatedTransaction.Location;
            transaction.PartnerIban = updatedTransaction.PartnerIban;
            transaction.Tags = new List<string>(updatedTransaction.Tags); 
            transaction.Partner = updatedTransaction.Partner;
            transaction.Priority = updatedTransaction.Priority;
            transaction.Status = updatedTransaction.Status;
            transaction.Project = updatedTransaction.Project;
            transaction.TaxDeductible = updatedTransaction.TaxDeductible;
            transaction.Currency = updatedTransaction.Currency;
            transaction.Reimbursable = updatedTransaction.Reimbursable;
            transaction.Receipt = updatedTransaction.Receipt;
            transaction.BudgetCategory = updatedTransaction.BudgetCategory;
            transaction.Notes = updatedTransaction.Notes;
            transaction.Classification = updatedTransaction.Classification;

            jsonTransaction.SaveData(transactions);

            return true;
        }

        public void DescriptionBatchProcessing(string original, string replacement)
        {
            foreach(Transaction t in transactions.Where(t=> t.Description.Equals(original)))
            {
                t.Description = replacement;
            }
            jsonTransaction.SaveData(transactions);
        }

        public void DescriptionPartnerBatchProcessing(string comparator, string replacement)
        {
            foreach (Transaction t in transactions.Where(t => t.Partner.Equals(comparator)))
            {
                t.Description = replacement;
            }
            jsonTransaction.SaveData(transactions);
        }

        public void CurrencyBatchProcessing(string original, string replacement)
        {
            foreach (Transaction t in transactions.Where(t => t.Currency.Equals(original)))
            {
                t.Currency = replacement;
            }
            jsonTransaction.SaveData(transactions);
        }

        public void CurrencyDescriptionBatchProcessing(string comparator, string replacement)
        {
            foreach (Transaction t in transactions.Where(t => t.Description.Equals(comparator)))
            {
                t.Currency = replacement;
            }
            jsonTransaction.SaveData(transactions);
        }

        public void CurrencyCategoryBatchProcessing(string comparator, string replacement)
        {
            foreach (Transaction t in transactions.Where(t => t.Category.Equals(comparator)))
            {
                t.Currency = replacement;
            }
            jsonTransaction.SaveData(transactions);
        }

        public void CurrencyBudgetCategoryBatchProcessing(string comparator, string replacement)
        {
            foreach (Transaction t in transactions.Where(t => t.BudgetCategory.Equals(comparator)))
            {
                t.Currency = replacement;
            }
            jsonTransaction.SaveData(transactions);
        }

        public void CurrencyPartnerBatchProcessing(string comparator, string replacement)
        {
            foreach (Transaction t in transactions.Where(t => t.Partner.Equals(comparator)))
            {
                t.Currency = replacement;
            }
            jsonTransaction.SaveData(transactions);
        }

        public void CurrencyLocationBatchProcessing(string comparator, string replacement)
        {
            foreach (Transaction t in transactions.Where(t => t.Location.Equals(comparator)))
            {
                t.Currency = replacement;
            }
            jsonTransaction.SaveData(transactions);
        }

        public void PaymentMethodBatchProcessing(string original, string replacement)
        {
            foreach (Transaction t in transactions.Where(t => t.PaymentMethod.Equals(original)))
            {
                t.PaymentMethod = replacement;
            }
            jsonTransaction.SaveData(transactions);
        }

        public void CategoryBatchProcessing(string original, string replacement)
        {
            foreach (Transaction t in transactions.Where(t => t.Category.Equals(original)))
            {
                t.Category = replacement;
            }
            jsonTransaction.SaveData(transactions);
        }

        public void CategoryDescriptionBatchProcessing(string comparator, string replacement)
        {
            foreach (Transaction t in transactions.Where(t => t.Description.Equals(comparator)))
            {
                t.Category = replacement;
            }
            jsonTransaction.SaveData(transactions);
        }

        public void CategoryCurrencyBatchProcessing(string comparator, string replacement)
        {
            foreach (Transaction t in transactions.Where(t => t.Currency.Equals(comparator)))
            {
                t.Category = replacement;
            }
            jsonTransaction.SaveData(transactions);
        }

        public void CategoryBudgetCategoryBatchProcessing(string comparator, string replacement)
        {
            foreach (Transaction t in transactions.Where(t => t.BudgetCategory.Equals(comparator)))
            {
                t.Category = replacement;
            }
            jsonTransaction.SaveData(transactions);
        }

        public void CategoryPartnerBatchProcessing(string comparator, string replacement)
        {
            foreach (Transaction t in transactions.Where(t => t.Partner.Equals(comparator)))
            {
                t.Category = replacement;
            }
            jsonTransaction.SaveData(transactions);
        }

        public void BudgetCategoryBatchProcessing(string original, string replacement)
        {
            foreach (Transaction t in transactions.Where(t => t.BudgetCategory.Equals(original)))
            {
                t.BudgetCategory = replacement;
            }
            jsonTransaction.SaveData(transactions);
        }

        public void BudgetCategoryPartnerBatchProcessing(string comparator, string replacement)
        {
            foreach (Transaction t in transactions.Where(t => t.Partner.Equals(comparator)))
            {
                t.BudgetCategory = replacement;
            }
            jsonTransaction.SaveData(transactions);
        }

        public void TagPartnerAddBatchProcessing(string comparator, string tag)
        {
            foreach (Transaction t in transactions.Where(t => t.Partner.Equals(comparator)))
            {
                t.Tags.Add(tag);
            }
            jsonTransaction.SaveData(transactions);
        }

        public void TagPartnerRemoveBatchProcessing(string comparator, string tag)
        {
            foreach (Transaction t in transactions.Where(t => t.Partner.Equals(comparator)))
            {
                t.Tags.Remove(tag);
            }
            jsonTransaction.SaveData(transactions);
        }

        public void TagPartnerChangeBatchProcessing(string comparator, string originalTag, string replacementTag)
        {
            foreach (Transaction t in transactions.Where(t => t.Partner.Equals(comparator)))
            {
                t.Tags[t.Tags.IndexOf(originalTag)] = replacementTag;
            }
            jsonTransaction.SaveData(transactions);
        }

        public void ClassificationBatchProcessing(string original, string replacement)
        {
            foreach (Transaction t in transactions.Where(t => t.Classification.Equals(original)))
            {
                t.Classification = replacement;
            }
            jsonTransaction.SaveData(transactions);
        }

        public void PartnerIbanBatchProcessing(string original, string replacement)
        {
            foreach (Transaction t in transactions.Where(t => t.PartnerIban.Equals(original)))
            {
                t.PartnerIban = replacement;
            }
            jsonTransaction.SaveData(transactions);
        }

        public void PartnerIbanPartnerBatchProcessing(string comparator, string replacement)
        {
            foreach (Transaction t in transactions.Where(t => t.Partner.Equals(comparator)))
            {
                t.PartnerIban = replacement;
            }
            jsonTransaction.SaveData(transactions);
        }

        public void PartnerBatchProcessing(string original, string replacement)
        {
            foreach (Transaction t in transactions.Where(t => t.Partner.Equals(original) || (t.Partner != null && t.Partner.IndexOf(original, StringComparison.OrdinalIgnoreCase) >= 0)))
            {
                t.Partner = replacement;
            }
            jsonTransaction.SaveData(transactions);
        }

        public void PartnerDescriptionProcessing(string comparator, string replacement)
        {
            foreach (Transaction t in transactions.Where(t => t.Description.Equals(comparator)))
            {
                t.Partner = replacement;
            }
            jsonTransaction.SaveData(transactions);
        }

        public void ProjectBatchProcessing(string original, string replacement)
        {
            foreach (Transaction t in transactions.Where(t => t.Project.Equals(original)))
            {
                t.Project = replacement;
            }
            jsonTransaction.SaveData(transactions);
        }

        public void ProjectPartnerBatchProcessing(string comparator, string replacement)
        {
            foreach (Transaction t in transactions.Where(t => t.Partner.Equals(comparator)))
            {
                t.Project = replacement;
            }
            jsonTransaction.SaveData(transactions);
        }

        public void StatusBatchProcessing(string original, string replacement)
        {
            foreach (Transaction t in transactions.Where(t => t.Status.Equals(original)))
            {
                t.Status = replacement;
            }
            jsonTransaction.SaveData(transactions);
        }

        public void StatusPartnerBatchProcessing(string comparator, string replacement)
        {
            foreach (Transaction t in transactions.Where(t => t.Partner.Equals(comparator)))
            {
                t.Status = replacement;
            }
            jsonTransaction.SaveData(transactions);
        }

        public void PriorityBatchProcessing(string original, string replacement)
        {
            foreach (Transaction t in transactions.Where(t => t.Priority.Equals(original)))
            {
                t.Priority = replacement;
            }
            jsonTransaction.SaveData(transactions);
        }

        public void PriorityPartnerBatchProcessing(string comparator, string replacement)
        {
            foreach (Transaction t in transactions.Where(t => t.Partner.Equals(comparator)))
            {
                t.Priority = replacement;
            }
            jsonTransaction.SaveData(transactions);
        }

        public void FrequencyBatchProcessing(string original, string replacement)
        {
            foreach (Transaction t in transactions.Where(t => t.Frequency.Equals(original)))
            {
                t.Frequency = replacement;
            }
            jsonTransaction.SaveData(transactions);
        }

        public void FrequencyPartnerBatchProcessing(string comparator, string replacement)
        {
            foreach (Transaction t in transactions.Where(t => t.Partner.Equals(comparator)))
            {
                t.Frequency = replacement;
            }
            jsonTransaction.SaveData(transactions);
        }

        public void LocationBatchProcessing(string original, string replacement)
        {
            foreach (Transaction t in transactions.Where(t => t.Location.Equals(original)))
            {
                t.Location = replacement;
            }
            jsonTransaction.SaveData(transactions);
        }

        public void LocationCurrencyBatchProcessing(string comparator, string replacement)
        {
            foreach (Transaction t in transactions.Where(t => t.Currency.Equals(comparator)))
            {
                t.Location = replacement;
            }
            jsonTransaction.SaveData(transactions);
        }

        public void LocationPartnerBatchProcessing(string comparator, string replacement)
        {
            foreach (Transaction t in transactions.Where(t => t.Partner.Equals(comparator)))
            {
                t.Location = replacement;
            }
            jsonTransaction.SaveData(transactions);
        }

        public void NotesBatchProcessing(string original, string replacement)
        {
            foreach (Transaction t in transactions.Where(t => t.Notes.Equals(original)))
            {
                t.Notes = replacement;
            }
            jsonTransaction.SaveData(transactions);
        }

        public void NotesPartnerBatchProcessing(string comparator, string replacement)
        {
            foreach (Transaction t in transactions.Where(t => t.Partner.Equals(comparator)))
            {
                t.Notes = replacement;
            }
            jsonTransaction.SaveData(transactions);
        }

        /// <summary>
        /// Retrieves a transaction by its ID from the current collection of transactions.
        /// </summary>
        /// <param name="id">The ID of the transaction to find.</param>
        /// <returns>The Transaction object with the specified ID, or null if no such transaction exists.</returns>
        /// <remarks>
        /// This method searches the current collection of transactions for a transaction with the specified ID. It returns the first transaction that matches the ID.
        /// If no transaction with the specified ID is found, the method returns null. This provides a straightforward way to access specific transactions for review,
        /// update, or deletion based on their unique identifier.
        /// </remarks>
        public Transaction GetTransactionById(int id)
        {
            return transactions.FirstOrDefault(t => t.Id == id);
        }

        /// <summary>
        /// Retrieves all transactions in the current collection as a read-only list.
        /// </summary>
        /// <returns>An IEnumerable of Transaction objects representing all transactions in the collection, provided in a read-only format.</returns>
        /// <remarks>
        /// This method provides access to the entire collection of transactions without allowing modifications to the original list. By returning a read-only view,
        /// it ensures that the integrity of the transaction data is maintained while still allowing consumers of the method to iterate over and inspect the transactions.
        /// It is useful for scenarios where the transaction data needs to be displayed or analyzed without the risk of altering the underlying collection.
        /// </remarks>
        public IEnumerable<Transaction> GetAllTransactions()
        {
            return transactions.AsReadOnly();
        }

        /// <summary>
        /// Retrieves all transactions considered as income from the provided or current collection of transactions.
        /// </summary>
        /// <param name="listOfTransactions">An optional enumerable collection of Transaction objects to filter for income transactions. If null, the current collection is used.</param>
        /// <returns>An IEnumerable of Transaction objects representing income transactions, where the amount is positive.</returns>
        /// <remarks>
        /// This method filters a given collection of transactions (or the current collection if none is provided) to include only those transactions
        /// where the amount is greater than zero, indicating income. It allows for flexible analysis of income transactions, either from a specific subset
        /// of transactions or the entire collection, by dynamically selecting the source based on the presence of the optional parameter.
        /// </remarks>
        public IEnumerable<Transaction> GetIncomeTransactions(IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).Where(t => t.Amount > 0);
        }

        /// <summary>
        /// Retrieves all transactions considered as expenses from the provided or current collection of transactions.
        /// </summary>
        /// <param name="listOfTransactions">An optional enumerable collection of Transaction objects to filter for expense transactions. If null, the current collection is used.</param>
        /// <returns>An IEnumerable of Transaction objects representing expense transactions, where the amount is negative.</returns>
        /// <remarks>
        /// This method filters a given collection of transactions (or the current collection if none is provided) to include only those transactions
        /// where the amount is less than zero, indicating expenses. It allows for flexible analysis of expense transactions, either from a specific subset
        /// of transactions or the entire collection, by dynamically selecting the source based on the presence of the optional parameter.
        /// </remarks>
        public IEnumerable<Transaction> GetExpenseTransactions(IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).Where(t => t.Amount < 0);
        }

        /// <summary>
        /// Retrieves all transactions with a specific amount from the provided or current collection of transactions.
        /// </summary>
        /// <param name="amount">The specific amount to filter transactions by.</param>
        /// <param name="listOfTransactions">An optional enumerable collection of Transaction objects to filter. If null, the current collection is used.</param>
        /// <returns>An IEnumerable of Transaction objects where the transaction amount matches the specified amount.</returns>
        /// <remarks>
        /// This method filters a given collection of transactions (or the current collection if none is provided) to include only those transactions
        /// with an amount exactly matching the specified amount. It facilitates the identification and analysis of transactions of a particular value,
        /// either within a specific subset of transactions or across the entire collection, enhancing the ability to perform detailed financial analysis
        /// based on transaction amounts.
        /// </remarks>
        public IEnumerable<Transaction> GetTransactionsByAmount(decimal amount , IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).Where(t => t.Amount == amount);
        }

        /// <summary>
        /// Retrieves all transactions within a specified amount range from the provided or current collection of transactions.
        /// </summary>
        /// <param name="minAmount">The minimum amount in the range to filter transactions by.</param>
        /// <param name="maxAmount">The maximum amount in the range to filter transactions by.</param>
        /// <param name="listOfTransactions">An optional enumerable collection of Transaction objects to filter. If null, the current collection is used.</param>
        /// <returns>An IEnumerable of Transaction objects where the transaction amount is within the specified range, inclusive of the boundaries.</returns>
        /// <remarks>
        /// This method filters a given collection of transactions (or the current collection if none is provided) to include only those transactions
        /// whose amounts fall within the specified minimum and maximum range, inclusive of both boundaries. It provides a way to analyze transactions
        /// based on a range of values, facilitating the examination of spending or income patterns within specific monetary thresholds, whether applied
        /// to a particular set of transactions or across the entire collection.
        /// </remarks>
        public IEnumerable<Transaction> GetTransactionsByAmount(decimal minAmount, decimal maxAmount, IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).Where(t => t.Amount >= minAmount && t.Amount <= maxAmount);
        }

        /// <summary>
        /// Retrieves all transactions that occurred on a specific date from the provided or current collection of transactions.
        /// </summary>
        /// <param name="date">The date to filter transactions by.</param>
        /// <param name="listOfTransactions">An optional enumerable collection of Transaction objects to filter. If null, the current collection is used.</param>
        /// <returns>An IEnumerable of Transaction objects where the transaction date matches the specified date.</returns>
        /// <remarks>
        /// This method filters a given collection of transactions (or the current collection if none is provided) to include only those transactions
        /// whose date matches exactly with the specified date, ignoring the time component. It enables precise filtering of transactions based on the
        /// transaction date, facilitating detailed analysis or reporting of transactions on a day-to-day basis.
        /// </remarks>
        public IEnumerable<Transaction> GetTransactionsByDate(DateTime date, IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).Where(t => t.Date.Date == date.Date);
        }

        /// <summary>
        /// Retrieves all transactions within a specific date range from the provided or current collection of transactions.
        /// </summary>
        /// <param name="startDate">The start date of the range to filter transactions by.</param>
        /// <param name="endDate">The end date of the range to filter transactions by.</param>
        /// <param name="listOfTransactions">An optional enumerable collection of Transaction objects to filter. If null, the current collection is used.</param>
        /// <returns>An IEnumerable of Transaction objects where the transaction date falls within the specified start and end dates, inclusive.</returns>
        /// <remarks>
        /// This method filters a given collection of transactions (or the current collection if none is provided) to include only those transactions
        /// whose dates fall within the specified range, including the start and end dates. It enables detailed analysis of transactions over specific
        /// periods, facilitating financial tracking, reporting, and analysis based on transaction dates.
        /// </remarks>
        public IEnumerable<Transaction> GetTransactionsByDate(DateTime startDate, DateTime endDate, IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).Where(t => t.Date.Date >= startDate.Date && t.Date.Date <= endDate.Date);
        }

        /// <summary>
        /// Retrieves transactions that contain the specified description in a case-insensitive manner.
        /// </summary>
        /// <param name="description">The description to search for within transaction descriptions.</param>
        /// <param name="listOfTransactions">Optional. The collection of transactions to search. If null, searches the entire collection.</param>
        /// <returns>An IEnumerable of Transaction objects that match the specified description criteria.</returns>
        public IEnumerable<Transaction> GetTransactionsByDescription(string description, IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).Where(t => t.Description != null && t.Description.IndexOf(description, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        /// <summary>
        /// Retrieves transactions with the specified currency in a case-insensitive manner.
        /// </summary>
        /// <param name="currency">The currency code to filter transactions by.</param>
        /// <param name="listOfTransactions">Optional. The collection of transactions to search. If null, searches the entire collection.</param>
        /// <returns>An IEnumerable of Transaction objects that match the specified currency criteria.</returns>
        public IEnumerable<Transaction> GetTransactionsByCurrency(string currency, IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).Where(t => t.Currency.Equals(currency, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Retrieves transactions that were made using the specified payment method in a case-insensitive manner.
        /// </summary>
        /// <param name="paymentMethod">The payment method to filter transactions by.</param>
        /// <param name="listOfTransactions">Optional. The collection of transactions to search. If null, searches the entire collection.</param>
        /// <returns>An IEnumerable of Transaction objects that match the specified payment method criteria.</returns>
        public IEnumerable<Transaction> GetTransactionsByPaymentMethod(string paymentMethod, IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).Where(t => t.PaymentMethod.Equals(paymentMethod, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Retrieves transactions that are marked as tax deductible.
        /// </summary>
        /// <param name="listOfTransactions">Optional. The collection of transactions to search. If null, searches the entire collection.</param>
        /// <returns>An IEnumerable of Transaction objects that are tax deductible.</returns>
        public IEnumerable<Transaction> GetTaxDeductibleTransactions(IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).Where(t => t.TaxDeductible);
        }

        /// <summary>
        /// Retrieves transactions that are marked as reimbursable.
        /// </summary>
        /// <param name="listOfTransactions">Optional. The collection of transactions to search. If null, searches the entire collection.</param>
        /// <returns>An IEnumerable of Transaction objects that are reimbursable.</returns>
        public IEnumerable<Transaction> GetReimbursableTransactions(IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).Where(t => t.Reimbursable);
        }

        /// <summary>
        /// Retrieves transactions belonging to the specified category in a case-insensitive manner.
        /// </summary>
        /// <param name="category">The category to filter transactions by.</param>
        /// <param name="listOfTransactions">Optional. The collection of transactions to search. If null, searches the entire collection.</param>
        /// <returns>An IEnumerable of Transaction objects that match the specified category criteria.</returns>
        public IEnumerable<Transaction> GetTransactionsByCategory(string category, IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).Where(t => t.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Retrieves transactions belonging to the specified budget category in a case-insensitive manner.
        /// </summary>
        /// <param name="budgetCategory">The budget category to filter transactions by.</param>
        /// <param name="listOfTransactions">Optional. The collection of transactions to search. If null, searches the entire collection.</param>
        /// <returns>An IEnumerable of Transaction objects that match the specified budget category criteria.</returns>
        public IEnumerable<Transaction> GetTransactionsByBudgetCategory(string budgetCategory, IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).Where(t => t.BudgetCategory.Equals(budgetCategory, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Retrieves transactions associated with the specified partner in a case-insensitive manner.
        /// </summary>
        /// <param name="partner">The partner name to filter transactions by.</param>
        /// <param name="listOfTransactions">Optional. The collection of transactions to search. If null, searches the entire collection.</param>
        /// <returns>An IEnumerable of Transaction objects that match the specified partner criteria.</returns>
        public IEnumerable<Transaction> GetTransactionsByPartner(string partner, IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).Where(t => t.Partner != null && t.Partner.IndexOf(partner, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        /// <summary>
        /// Retrieves transactions associated with the specified project in a case-insensitive manner.
        /// </summary>
        /// <param name="project">The project name to filter transactions by.</param>
        /// <param name="listOfTransactions">Optional. The collection of transactions to search. If null, searches the entire collection.</param>
        /// <returns>An IEnumerable of Transaction objects that match the specified project criteria.</returns>
        public IEnumerable<Transaction> GetTransactionsByProject(string project, IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).Where(t => t.Project.Equals(project, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Retrieves transactions with the specified status in a case-insensitive manner.
        /// </summary>
        /// <param name="status">The status to filter transactions by.</param>
        /// <param name="listOfTransactions">Optional. The collection of transactions to search. If null, searches the entire collection.</param>
        /// <returns>An IEnumerable of Transaction objects that match the specified status criteria.</returns>
        public IEnumerable<Transaction> GetTransactionsByStatus(string status, IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).Where(t => t.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Retrieves transactions with the specified priority in a case-insensitive manner.
        /// </summary>
        /// <param name="priority">The priority level to filter transactions by.</param>
        /// <param name="listOfTransactions">Optional. The collection of transactions to search. If null, searches the entire collection.</param>
        /// <returns>An IEnumerable of Transaction objects that match the specified priority criteria.</returns>
        public IEnumerable<Transaction> GetTransactionsByPriority(string priority, IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).Where(t => t.Priority.Equals(priority, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Retrieves transactions with the specified frequency in a case-insensitive manner.
        /// </summary>
        /// <param name="frequency">The frequency to filter transactions by (e.g., monthly, weekly).</param>
        /// <param name="listOfTransactions">Optional. The collection of transactions to search. If null, searches the entire collection.</param>
        /// <returns>An IEnumerable of Transaction objects that match the specified frequency criteria.</returns>
        public IEnumerable<Transaction> GetTransactionsByFrequency(string frequency, IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).Where(t => t.Frequency.Equals(frequency, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Retrieves transactions that occurred at the specified location in a case-insensitive manner.
        /// </summary>
        /// <param name="location">The location to filter transactions by.</param>
        /// <param name="listOfTransactions">Optional. The collection of transactions to search. If null, searches the entire collection.</param>
        /// <returns>An IEnumerable of Transaction objects that match the specified location criteria.</returns>
        public IEnumerable<Transaction> GetTransactionsByLocation(string location, IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).Where(t => t.Location != null && t.Location.IndexOf(location, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        /// <summary>
        /// Retrieves transactions that contain the specified tag.
        /// </summary>
        /// <param name="tag">The tag to filter transactions by.</param>
        /// <param name="listOfTransactions">Optional. The collection of transactions to search. If null, searches the entire collection.</param>
        /// <returns>An IEnumerable of Transaction objects that contain the specified tag.</returns>
        public IEnumerable<Transaction> GetTransactionsByTag(string tag, IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).Where(t => t.Tags.Contains(tag));
        }

        /// <summary>
        /// Retrieves transactions that contain the specified notes in a case-insensitive manner.
        /// </summary>
        /// <param name="notes">The notes to search for within transaction notes.</param>
        /// <param name="listOfTransactions">Optional. The collection of transactions to search. If null, searches the entire collection.</param>
        /// <returns>An IEnumerable of Transaction objects that match the specified notes criteria.</returns>
        public IEnumerable<Transaction> GetTransactionsByNotes(string notes, IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).Where(t => t.Notes != null && t.Notes.IndexOf(notes, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        /// <summary>
        /// Sorts the transactions by their ID in ascending order.
        /// </summary>
        /// <returns>An IEnumerable of Transaction objects sorted by ID in ascending order.</returns>
        public IEnumerable<Transaction> SortTransactionsByIdAscending()
        {
            transactions = SortTransactionsByIdAscending(transactions).ToList();
            return transactions.AsReadOnly();
        }

        /// <summary>
        /// Sorts a given collection of transactions by their ID in ascending order.
        /// </summary>
        /// <param name="listOfTransactions">The collection of transactions to be sorted.</param>
        /// <returns>An IEnumerable of Transaction objects sorted by ID in ascending order from the provided collection.</returns>
        public IEnumerable<Transaction> SortTransactionsByIdAscending(IEnumerable<Transaction> listOfTransactions)
        {
            return listOfTransactions.OrderBy(t => t.Id);
        }

        /// <summary>
        /// Sorts the transactions by their ID in descending order.
        /// </summary>
        /// <returns>An IEnumerable of Transaction objects sorted by ID in descending order.</returns>
        public IEnumerable<Transaction> SortTransactionsByIdDescending()
        {
            transactions = SortTransactionsByIdDescending(transactions).ToList();
            return transactions.AsReadOnly();
        }

        /// <summary>
        /// Sorts a given collection of transactions by their ID in descending order.
        /// </summary>
        /// <param name="listOfTransactions">The collection of transactions to be sorted.</param>
        /// <returns>An IEnumerable of Transaction objects sorted by ID in descending order from the provided collection.</returns>
        public IEnumerable<Transaction> SortTransactionsByIdDescending(IEnumerable<Transaction> listOfTransactions)
        {
            return listOfTransactions.OrderByDescending(t => t.Id);
        }

        /// <summary>
        /// Sorts the transactions by their date in ascending order.
        /// </summary>
        /// <returns>An IEnumerable of Transaction objects sorted by Date in ascending order.</returns>
        public IEnumerable<Transaction> SortTransactionsByDateAscending()
        {
            transactions = SortTransactionsByDateAscending(transactions).ToList();
            return transactions.AsReadOnly();
        }

        /// <summary>
        /// Sorts a given collection of transactions by their date in ascending order.
        /// </summary>
        /// <param name="listOfTransactions">The collection of transactions to be sorted.</param>
        /// <returns>An IEnumerable of Transaction objects sorted by Date in ascending order from the provided collection.</returns>
        public IEnumerable<Transaction> SortTransactionsByDateAscending(IEnumerable<Transaction> listOfTransactions)
        {
            return listOfTransactions.OrderBy(t => t.Date);
        }

        /// <summary>
        /// Sorts the transactions by their date in descending order.
        /// </summary>
        /// <returns>An IEnumerable of Transaction objects sorted by Date in descending order.</returns>
        public IEnumerable<Transaction> SortTransactionsByDateDescending()
        {
            transactions = SortTransactionsByDateDescending(transactions).ToList();
            return transactions.AsReadOnly();
        }

        /// <summary>
        /// Sorts a given collection of transactions by their date in descending order.
        /// </summary>
        /// <param name="listOfTransactions">The collection of transactions to be sorted.</param>
        /// <returns>An IEnumerable of Transaction objects sorted by Date in descending order from the provided collection.</returns>
        public IEnumerable<Transaction> SortTransactionsByDateDescending(IEnumerable<Transaction> listOfTransactions)
        {
            return listOfTransactions.OrderByDescending(t => t.Date);
        }

        /// <summary>
        /// Sorts the transactions by their amount in ascending order.
        /// </summary>
        /// <returns>An IEnumerable of Transaction objects sorted by Amount in ascending order.</returns>
        public IEnumerable<Transaction> SortTransactionsByAmountAscending()
        {
            transactions = SortTransactionsByAmountAscending(transactions).ToList();
            return transactions.AsReadOnly();
        }

        /// <summary>
        /// Sorts a given collection of transactions by their amount in ascending order.
        /// </summary>
        /// <param name="listOfTransactions">The collection of transactions to be sorted.</param>
        /// <returns>An IEnumerable of Transaction objects sorted by Amount in ascending order from the provided collection.</returns>
        public IEnumerable<Transaction> SortTransactionsByAmountAscending(IEnumerable<Transaction> listOfTransactions)
        {
            return listOfTransactions.OrderBy(t => t.Amount);
        }

        /// <summary>
        /// Sorts the transactions by their amount in descending order.
        /// </summary>
        /// <returns>An IEnumerable of Transaction objects sorted by Amount in descending order.</returns>
        public IEnumerable<Transaction> SortTransactionsByAmountDescending()
        {
            transactions = SortTransactionsByAmountDescending(transactions).ToList();
            return transactions.AsReadOnly();
        }

        /// <summary>
        /// Sorts a given collection of transactions by their amount in descending order.
        /// </summary>
        /// <param name="listOfTransactions">The collection of transactions to be sorted.</param>
        /// <returns>An IEnumerable of Transaction objects sorted by Amount in descending order from the provided collection.</returns>
        public IEnumerable<Transaction> SortTransactionsByAmountDescending(IEnumerable<Transaction> listOfTransactions)
        {
            return listOfTransactions.OrderByDescending(t => t.Amount);
        }

        /// <summary>
        /// Sorts the transactions by their partner in ascending order.
        /// </summary>
        /// <returns>An IEnumerable of Transaction objects sorted by Partner in ascending order.</returns>
        public IEnumerable<Transaction> SortTransactionsByPartnerAscending()
        {
            transactions = SortTransactionsByPartnerAscending(transactions).ToList();
            return transactions.AsReadOnly();
        }

        /// <summary>
        /// Sorts a given collection of transactions by their partner in ascending order.
        /// </summary>
        /// <param name="listOfTransactions">The collection of transactions to be sorted.</param>
        /// <returns>An IEnumerable of Transaction objects sorted by Partner in ascending order from the provided collection.</returns>
        public IEnumerable<Transaction> SortTransactionsByPartnerAscending(IEnumerable<Transaction> listOfTransactions)
        {
            return listOfTransactions.OrderBy(t => t.Partner);
        }

        /// <summary>
        /// Sorts the transactions by their partner in descending order.
        /// </summary>
        /// <returns>An IEnumerable of Transaction objects sorted by Partner in descending order.</returns>
        public IEnumerable<Transaction> SortTransactionsByPartnerDescending()
        {
            transactions = SortTransactionsByPartnerDescending(transactions).ToList();
            return transactions.AsReadOnly();
        }

        /// <summary>
        /// Sorts a given collection of transactions by their partner in descending order.
        /// </summary>
        /// <param name="listOfTransactions">The collection of transactions to be sorted.</param>
        /// <returns>An IEnumerable of Transaction objects sorted by Partner in descending order from the provided collection.</returns>
        public IEnumerable<Transaction> SortTransactionsByPartnerDescending(IEnumerable<Transaction> listOfTransactions)
        {
            return listOfTransactions.OrderByDescending(t => t.Partner);
        }

        /// <summary>
        /// Sorts the transactions by their location in ascending order.
        /// </summary>
        /// <returns>An IEnumerable of Transaction objects sorted by Location in ascending order.</returns>
        public IEnumerable<Transaction> SortTransactionsByLocationAscending()
        {
            transactions = SortTransactionsByLocationAscending(transactions).ToList();
            return transactions.AsReadOnly();
        }

        /// <summary>
        /// Sorts a given collection of transactions by their location in ascending order.
        /// </summary>
        /// <param name="listOfTransactions">The collection of transactions to be sorted.</param>
        /// <returns>An IEnumerable of Transaction objects sorted by Location in ascending order from the provided collection.</returns>
        public IEnumerable<Transaction> SortTransactionsByLocationAscending(IEnumerable<Transaction> listOfTransactions)
        {
            return listOfTransactions.OrderBy(t => t.Location);
        }

        /// <summary>
        /// Sorts the transactions by their location in descending order.
        /// </summary>
        /// <returns>An IEnumerable of Transaction objects sorted by Location in descending order.</returns>
        public IEnumerable<Transaction> SortTransactionsByLocationDescending()
        {
            transactions = SortTransactionsByLocationDescending(transactions).ToList();
            return transactions.AsReadOnly();
        }

        /// <summary>
        /// Sorts a given collection of transactions by their location in descending order.
        /// </summary>
        /// <param name="listOfTransactions">The collection of transactions to be sorted.</param>
        /// <returns>An IEnumerable of Transaction objects sorted by Location in descending order from the provided collection.</returns>
        public IEnumerable<Transaction> SortTransactionsByLocationDescending(IEnumerable<Transaction> listOfTransactions)
        {
            return listOfTransactions.OrderByDescending(t => t.Location);
        }

        public IEnumerable<IGrouping<string, Transaction>> GroupByDate(IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).GroupBy(t => t.Date.ToShortDateString()).ToList();
        }

        public IEnumerable<IGrouping<string, Transaction>> GroupByDateDay(IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).GroupBy(t => t.Date.Day.ToString()).ToList();
        }

        public IEnumerable<IGrouping<string, Transaction>> GroupByDateWeekDay(IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).GroupBy(t => t.Date.ToString("dddd", CultureInfo.CurrentCulture)).ToList();
        }

        public IEnumerable<IGrouping<string, Transaction>> GroupByDateMonth(IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).GroupBy(t => CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(t.Date.Month)).ToList();
        }

        public IEnumerable<IGrouping<string, Transaction>> GroupByDateYear(IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).GroupBy(t => t.Date.Year.ToString()).ToList();
        }

        public IEnumerable<IGrouping<string, Transaction>> GroupByDescription(IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).GroupBy(t => t.Description).ToList();
        }

        public IEnumerable<IGrouping<string, Transaction>> GroupByAmount(IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).GroupBy(t => t.Amount.ToString()).ToList();
        }

        public IEnumerable<IGrouping<string, Transaction>> GroupByAmountrange(IEnumerable<Transaction> listOfTransactions = null)
        {
            var groupedByAmount = (listOfTransactions ?? transactions)
            .GroupBy(t =>
            {
                int lowerBound = (int)Math.Floor(t.Amount / 100) * 100;
                int upperBound = lowerBound + 99;
                return $"{lowerBound}-{upperBound}";
            })
            .ToList();

            return groupedByAmount;
        }

        public IEnumerable<IGrouping<string, Transaction>> GroupByCurrency(IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).GroupBy(t => t.Currency).ToList();
        }

        public IEnumerable<IGrouping<string, Transaction>> GroupByPaymentMethod(IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).GroupBy(t => t.PaymentMethod).ToList();
        }

        public IEnumerable<IGrouping<string, Transaction>> GroupByCategory(IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).GroupBy(t => t.Category).ToList();
        }

        public IEnumerable<IGrouping<string, Transaction>> GroupByBudgetCategory(IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).GroupBy(t => t.BudgetCategory).ToList();
        }

        public IEnumerable<IGrouping<string, Transaction>> GroupByTag(IEnumerable<Transaction> listOfTransactions = null)
        {
            var groupedByTag = (listOfTransactions ?? transactions)
                .SelectMany(transaction => transaction.Tags.Select(tag => new { transaction, tag }))
                .GroupBy(x => x.tag, x => x.transaction)
                .ToList();
            return groupedByTag;
        }

        public IEnumerable<IGrouping<string, Transaction>> GroupByClassification(IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).GroupBy(t => t.Classification).ToList();
        }

        public IEnumerable<IGrouping<string, Transaction>> GroupByPartnerIBAN(IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).GroupBy(t => t.PartnerIban).ToList();
        }

        public IEnumerable<IGrouping<string, Transaction>> GroupByPartner(IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).GroupBy(t => t.Partner).ToList();
        }

        public IEnumerable<IGrouping<string, Transaction>> GroupByProject(IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).GroupBy(t => t.Project).ToList();
        }

        public IEnumerable<IGrouping<string, Transaction>> GroupByStatus(IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).GroupBy(t => t.Status).ToList();
        }

        public IEnumerable<IGrouping<string, Transaction>> GroupByPriority(IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).GroupBy(t => t.Priority).ToList();
        }

        public IEnumerable<IGrouping<string, Transaction>> GroupByFrequency(IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).GroupBy(t => t.Frequency).ToList();
        }

        public IEnumerable<IGrouping<string, Transaction>> GroupByLocation(IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).GroupBy(t => t.Location).ToList();
        }

        public IEnumerable<IGrouping<string, Transaction>> GroupByNotes(IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).GroupBy(t => t.Notes).ToList();
        }

        /// <summary>
        /// Checks if a specified transaction already exists in the current collection of transactions.
        /// </summary>
        /// <param name="transaction">The transaction to check for existence.</param>
        /// <returns>True if the transaction already exists in the collection; otherwise, false.</returns>
        /// <remarks>
        /// This method iterates over the collection of transactions and compares each existing transaction with the specified transaction
        /// using the Equals method. It returns true if any transaction in the collection is considered equal to the specified transaction, 
        /// indicating that the transaction already exists in the collection. Otherwise, it returns false.
        /// </remarks>
        private bool DoesTransactionAlreadyExists(Transaction transaction)
        {
            return transactions.Any(t => transaction.Equals(t));
        }

        public void RenameAllTransactionPartnersInComandline()
        {
            IEnumerable<IGrouping<string, Transaction>> groupedTransactions = GroupByPartner();

            Console.WriteLine(Strings.BatchProcessingOfPartnerNames);
            Console.WriteLine();
            foreach (var group in groupedTransactions)
            {
                Console.WriteLine($"{Strings.CurrentPartnerName} {group.Key}");
                Console.WriteLine($"{Strings.Change}? {Strings.Yes} = 1 / {Strings.No} = 0");
                string input = Console.ReadLine().Trim();
                if (input == "1")
                {
                    Console.WriteLine(Strings.EnterNewName + ": ");
                    string newName = Console.ReadLine()?.Trim();
                    if (!string.IsNullOrEmpty(newName))
                    {
                        PartnerBatchProcessing(group.Key, newName);
                    }                   
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Opens the transaction file for reading or writing by decrypting its contents manually.
        /// </summary>
        /// <remarks>
        /// This method provides manual control over the decryption and opening of the transaction file, allowing for operations on the file outside of automatic opening mechanisms.
        /// </remarks>
        public void OpenTransactionFileManually()
        {
            jsonTransaction.OpenTransactionFile();
        }

        /// <summary>
        /// Closes the transaction file and encrypts its contents manually.
        /// </summary>
        /// <remarks>
        /// This method allows for manual control over encrypting and closing the transaction file, ensuring data is securely stored after operations are completed.
        /// </remarks>
        public void CloseTransactionFileManually()
        {
            jsonTransaction.CloseTransactionFile();
        }

        /// <summary>
        /// Displays a short summary of transaction details in the command line for each transaction in the provided or current collection.
        /// </summary>
        /// <param name="listOfTransactions">Optional. The collection of transactions to display. If null, displays the current collection.</param>
        /// <remarks>
        /// This method iterates over the specified collection of transactions, invoking a method on each transaction to print a short summary of its details to the command line.
        /// </remarks>
        public void DisplayShortTransactionInformationsInComandline(IEnumerable<Transaction> listOfTransactions = null)
        {
            foreach (var transaction in (listOfTransactions ?? transactions))
            {
                transaction.DisplayShortTransactionDetails();
            }
        }

        /// <summary>
        /// Displays complete transaction details in the command line for each transaction in the provided or current collection.
        /// </summary>
        /// <param name="listOfTransactions">Optional. The collection of transactions to display. If null, displays the current collection.</param>
        /// <remarks>
        /// This method iterates over the specified collection of transactions, invoking a method on each transaction to print its complete details to the command line.
        /// </remarks>
        public void DisplayCompleteTransactionInformationsInComandline(IEnumerable<Transaction> listOfTransactions = null)
        {
            foreach (var transaction in (listOfTransactions ?? transactions))
            {
                transaction.DisplayTransactionDetails();
            }
        }

        public void DisplayGroupedTransactions(IEnumerable<IGrouping<string, Transaction>> groupedTransactions)
        {
            foreach (var group in groupedTransactions)
            {
                Console.WriteLine(group.Key);
                foreach (Transaction transaction in group)
                {
                    transaction.DisplayShortTransactionDetails();
                }
                Console.WriteLine();
            }
        }
    }

}

