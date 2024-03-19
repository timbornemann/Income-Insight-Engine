using dataStructure;
using IncomeInsightEngine.Properties;
using System;
using System.Collections.Generic;
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
        public TransactionInformation transaktionInformation { get; }

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

            transaktionInformation = new TransactionInformation(transactions);
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
            transaktionInformation.LoadData(transactions);
            return true;
        }

        /// <summary>
        /// Adds a collection of transactions to the existing data store with validation checks.
        /// </summary>
        /// <param name="data">The collection of transactions to be added.</param>
        /// <returns>True if the data is not null and is processed, false if the data is null.</returns>
        /// <remarks>
        /// This method performs checks to avoid adding null transactions or duplicates based on transaction ID. 
        /// After adding, it saves the updated collection to a JSON data store and reloads the transaction information.
        /// </remarks>
        public bool AddTransactionsSave(IEnumerable<Transaction> data)
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
            }
            jsonTransaction.SaveData(transactions);
            transaktionInformation.LoadData(transactions);

            return true;
        }

        /// <summary>
        /// Adds a collection of transactions to the existing data store quickly without individual validation.
        /// </summary>
        /// <param name="data">The collection of transactions to be added.</param>
        /// <returns>True if the data is not null and added, false if the data is null.</returns>
        /// <remarks>
        /// This method bypasses individual transaction checks for a faster data addition. 
        /// It updates the data store with the new collection and reloads the transaction information.
        /// </remarks>
        public bool AddTransactionsFast(IEnumerable<Transaction> data)
        {
            if (data == null)
            {
                return false;
            }

            transactions.AddRange(data);            
            jsonTransaction.SaveData(transactions);
            transaktionInformation.LoadData(transactions);
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

        /// <summary>
        /// Replaces the description for transactions with a specified original description.
        /// </summary>
        /// <param name="original">The original description to match, or null to match transactions with no description.</param>
        /// <param name="replacement">The new description to apply.</param>
        /// <remarks>
        /// This method iterates over all transactions, updating the description where it matches the specified original.
        /// After processing, it saves the updated transaction list.
        /// </remarks>
        public void DescriptionBatchProcessing(string original, string replacement)
        {
            foreach(Transaction t in transactions.Where(t=> original == null ? t.Description == null : t.Description.Equals(original)))
            {
                t.Description = replacement;
            }
            jsonTransaction.SaveData(transactions);
        }

        /// <summary>
        /// Replaces the description for transactions associated with a specific partner.
        /// </summary>
        /// <param name="comparator">The partner identifier used to select transactions for description replacement.</param>
        /// <param name="replacement">The new description to apply.</param>
        /// <remarks>
        /// Targets transactions based on partner matching and updates their descriptions accordingly. 
        /// The transaction list is saved after processing.
        /// </remarks>
        public void DescriptionPartnerBatchProcessing(string comparator, string replacement)
        {
            foreach (Transaction t in transactions.Where(t => t.Partner.Equals(comparator)))
            {
                t.Description = replacement;
            }
            jsonTransaction.SaveData(transactions);
        }

        /// <summary>
        /// Replaces the currency for transactions with a specified original currency.
        /// </summary>
        /// <param name="original">The original currency to match, or null to match transactions with no currency set.</param>
        /// <param name="replacement">The new currency to apply.</param>
        /// <remarks>
        /// Updates the currency of transactions where the original currency matches the specified condition. 
        /// Processes all transactions and saves the updated list afterward.
        /// </remarks>
        public void CurrencyBatchProcessing(string original, string replacement)
        {
            foreach (Transaction t in transactions.Where(t => original == null ? t.Currency == null : t.Currency.Equals(original)))
            {
                t.Currency = replacement;
            }
            jsonTransaction.SaveData(transactions);
        }

        /// <summary>
        /// Updates the currency for transactions based on a matching description.
        /// </summary>
        /// <param name="comparator">The description used to identify transactions for currency update.</param>
        /// <param name="replacement">The new currency to apply to transactions matching the specified description.</param>
        /// <remarks>
        /// For transactions with a description matching the specified comparator, their currency is updated to the new value.
        /// The dataset is saved after updates.
        /// </remarks>
        public void CurrencyDescriptionBatchProcessing(string comparator, string replacement)
        {
            foreach (Transaction t in transactions.Where(t => t.Description.Equals(comparator)))
            {
                t.Currency = replacement;
            }
            jsonTransaction.SaveData(transactions);
        }

        /// <summary>
        /// Updates the currency for transactions within a specified category.
        /// </summary>
        /// <param name="comparator">The category used to select transactions for currency update.</param>
        /// <param name="replacement">The new currency to apply to transactions within the specified category.</param>
        /// <remarks>
        /// Targets transactions within a specific category for currency updates, saving the dataset with these changes.
        /// </remarks>
        public void CurrencyCategoryBatchProcessing(string comparator, string replacement)
        {
            foreach (Transaction t in transactions.Where(t => t.Category.Equals(comparator)))
            {
                t.Currency = replacement;
            }
            jsonTransaction.SaveData(transactions);
        }

        /// <summary>
        /// Modifies the currency for transactions belonging to a specified budget category.
        /// </summary>
        /// <param name="comparator">The budget category identifier for selecting transactions.</param>
        /// <param name="replacement">The new currency to assign to the targeted transactions.</param>
        /// <remarks>
        /// Applies a new currency value to transactions under a particular budget category, with changes saved to the dataset.
        /// </remarks>
        public void CurrencyBudgetCategoryBatchProcessing(string comparator, string replacement)
        {
            foreach (Transaction t in transactions.Where(t => t.BudgetCategory.Equals(comparator)))
            {
                t.Currency = replacement;
            }
            jsonTransaction.SaveData(transactions);
        }

        /// <summary>
        /// Changes the currency for transactions associated with a particular partner.
        /// </summary>
        /// <param name="comparator">The partner identifier for transaction selection.</param>
        /// <param name="replacement">The new currency to be applied to the selected transactions.</param>
        /// <remarks>
        /// This method is used for updating the currency of transactions related to a specific partner, followed by saving these updates.
        /// </remarks>
        public void CurrencyPartnerBatchProcessing(string comparator, string replacement)
        {
            foreach (Transaction t in transactions.Where(t => t.Partner.Equals(comparator)))
            {
                t.Currency = replacement;
            }
            jsonTransaction.SaveData(transactions);
        }

        /// <summary>
        /// Adjusts the currency for transactions based on their location.
        /// </summary>
        /// <param name="comparator">The location identifier for choosing transactions.</param>
        /// <param name="replacement">The new currency to apply to transactions from the specified location.</param>
        /// <remarks>
        /// This process involves updating the currency for transactions linked to a certain location, ensuring the dataset is saved with these modifications.
        /// </remarks>
        public void CurrencyLocationBatchProcessing(string comparator, string replacement)
        {
            foreach (Transaction t in transactions.Where(t => t.Location.Equals(comparator)))
            {
                t.Currency = replacement;
            }
            jsonTransaction.SaveData(transactions);
        }

        /// <summary>
        /// Updates the payment method for transactions matching a specific original payment method to a new payment method.
        /// </summary>
        /// <param name="original">The original payment method to match. Null matches transactions with null payment methods.</param>
        /// <param name="replacement">The new payment method to apply.</param>
        /// <remarks>
        /// Iterates through transactions, updating the payment method for those that match the original criteria or have a null payment method, then saves the updated dataset.
        /// </remarks>
        public void PaymentMethodBatchProcessing(string original, string replacement)
        {
            foreach (Transaction t in transactions.Where(t => original == null ? t.PaymentMethod == null : t.PaymentMethod.Equals(original)))
            {
                t.PaymentMethod = replacement;
            }
            jsonTransaction.SaveData(transactions);
        }

        /// <summary>
        /// Updates the payment method for transactions associated with a specified partner, including partial matches.
        /// </summary>
        /// <param name="comparator">The partner name to match against transaction partners.</param>
        /// <param name="replacement">The new payment method to apply to the matched transactions.</param>
        /// <remarks>
        /// Transactions where the partner name matches or partially matches the specified comparator have their payment method updated, followed by saving the dataset.
        /// </remarks>
        public void PaymentMethodPartnerBatchProcessing(string comparator, string replacement)
        {
            foreach (Transaction t in transactions.Where(t => t.Partner.Equals(comparator) || (t.Partner != null && t.Partner.IndexOf(comparator, StringComparison.OrdinalIgnoreCase) >= 0)))
            {
                t.PaymentMethod = replacement;
            }
            jsonTransaction.SaveData(transactions);
        }

        /// <summary>
        /// Updates the payment method for transactions based on matching or partially matching descriptions.
        /// </summary>
        /// <param name="comparator">The description text to search for in transaction descriptions.</param>
        /// <param name="replacement">The new payment method to be applied to transactions with matching descriptions.</param>
        /// <remarks>
        /// Searches for transactions where the description matches or contains the specified comparator, updating their payment method and saving the changes.
        /// </remarks>
        public void PaymentMethodDescriptionBatchProcessing(string comparator, string replacement)
        {
            foreach (Transaction t in transactions.Where(t => t.Description.Equals(comparator) || (t.Description != null && t.Description.IndexOf(comparator, StringComparison.OrdinalIgnoreCase) >= 0)))
            {
                t.PaymentMethod = replacement;
            }
            jsonTransaction.SaveData(transactions);
        }

        /// <summary>
        /// Changes the category of transactions matching a specific original category to a new category.
        /// </summary>
        /// <param name="original">The original category to match. Null matches transactions with null categories.</param>
        /// <param name="replacement">The replacement category.</param>
        /// <remarks>
        /// Applies a new category to transactions that either match the original category or have a null category, then saves the updated transactions.
        /// </remarks>
        public void CategoryBatchProcessing(string original, string replacement)
        {
            foreach (Transaction t in transactions.Where(t => original == null ? t.Category == null : t.Category.Equals(original)))
            {
                t.Category = replacement;
            }
            jsonTransaction.SaveData(transactions);
        }

        /// <summary>
        /// Updates the category of transactions based on a specific description.
        /// </summary>
        /// <param name="comparator">The description to match against transaction descriptions.</param>
        /// <param name="replacement">The new category to apply.</param>
        /// <remarks>
        /// For transactions with a matching description, the category is updated to the new value, with changes saved thereafter.
        /// </remarks>
        public void CategoryDescriptionBatchProcessing(string comparator, string replacement)
        {
            foreach (Transaction t in transactions.Where(t => t.Description.Equals(comparator)))
            {
                t.Category = replacement;
            }
            jsonTransaction.SaveData(transactions);
        }

        /// <summary>
        /// Assigns a new category to transactions based on their currency.
        /// </summary>
        /// <param name="comparator">The currency to match for transaction selection.</param>
        /// <param name="replacement">The new category to apply to transactions with the specified currency.</param>
        /// <remarks>
        /// Transactions with a currency matching the comparator have their category updated, followed by dataset saving.
        /// </remarks>
        public void CategoryCurrencyBatchProcessing(string comparator, string replacement)
        {
            foreach (Transaction t in transactions.Where(t => t.Currency.Equals(comparator)))
            {
                t.Category = replacement;
            }
            jsonTransaction.SaveData(transactions);
        }

        /// <summary>
        /// Updates the transaction category for transactions within a specified budget category.
        /// </summary>
        /// <param name="comparator">The budget category used to select transactions.</param>
        /// <param name="replacement">The new transaction category to apply.</param>
        /// <remarks>
        /// Selected transactions within the specified budget category are updated to the new transaction category, with changes saved.
        /// </remarks>
        public void CategoryBudgetCategoryBatchProcessing(string comparator, string replacement)
        {
            foreach (Transaction t in transactions.Where(t => t.BudgetCategory.Equals(comparator)))
            {
                t.Category = replacement;
            }
            jsonTransaction.SaveData(transactions);
        }

        /// <summary>
        /// Updates the category of transactions associated with a particular partner, including those partially matching the partner name.
        /// </summary>
        /// <param name="comparator">The partner name to match.</param>
        /// <param name="replacement">The new category for these transactions.</param>
        /// <remarks>
        /// Affects transactions where the partner name matches or partially matches the specified comparator, updating their category and saving the dataset.
        /// </remarks>
        public void CategoryPartnerBatchProcessing(string comparator, string replacement)
        {
            foreach (Transaction t in transactions.Where(t => t.Partner.Equals(comparator) || (t.Partner != null && t.Partner.IndexOf(comparator, StringComparison.OrdinalIgnoreCase) >= 0)))
            {
                t.Category = replacement;
            }
            jsonTransaction.SaveData(transactions);
        }

        /// <summary>
        /// Updates the budget category for transactions matching a specific original budget category to a new one.
        /// </summary>
        /// <param name="original">The original budget category to match. Null matches transactions with null budget categories.</param>
        /// <param name="replacement">The new budget category to apply.</param>
        /// <remarks>
        /// Modifies the budget category for transactions that match the original budget category or have null as their budget category, saving the dataset afterwards.
        /// </remarks>
        public void BudgetCategoryBatchProcessing(string original, string replacement)
        {
            foreach (Transaction t in transactions.Where(t => original == null ? t.BudgetCategory == null : t.BudgetCategory.Equals(original)))
            {
                t.BudgetCategory = replacement;
            }
            jsonTransaction.SaveData(transactions);
        }

        /// <summary>
        /// Changes the budget category of transactions associated with a specified partner.
        /// </summary>
        /// <param name="comparator">The partner name to match transactions against.</param>
        /// <param name="replacement">The new budget category to apply to these transactions.</param
        public void BudgetCategoryPartnerBatchProcessing(string comparator, string replacement)
        {
            foreach (Transaction t in transactions.Where(t => t.Partner.Equals(comparator)))
            {
                t.BudgetCategory = replacement;
            }
            jsonTransaction.SaveData(transactions);
        }

        /// <summary>
        /// Adds a specified tag to all transactions associated with a particular partner.
        /// </summary>
        /// <param name="comparator">The partner name to match transactions against.</param>
        /// <param name="tag">The tag to add to each matching transaction.</param>
        /// <remarks>
        /// Iterates over transactions matching the specified partner, adding the given tag to their tag collection. The updated dataset is then saved.
        /// </remarks>
        public void TagPartnerAddBatchProcessing(string comparator, string tag)
        {
            foreach (Transaction t in transactions.Where(t => t.Partner.Equals(comparator)))
            {
                t.Tags.Add(tag);
            }
            jsonTransaction.SaveData(transactions);
        }

        /// <summary>
        /// Removes a specified tag from all transactions associated with a particular partner.
        /// </summary>
        /// <param name="comparator">The partner name to identify the transactions.</param>
        /// <param name="tag">The tag to be removed from each matching transaction.</param>
        /// <remarks>
        /// Searches for transactions with the specified partner and removes the given tag from their tag list. Changes are saved to the dataset.
        /// </remarks>
        public void TagPartnerRemoveBatchProcessing(string comparator, string tag)
        {
            foreach (Transaction t in transactions.Where(t => t.Partner.Equals(comparator)))
            {
                t.Tags.Remove(tag);
            }
            jsonTransaction.SaveData(transactions);
        }

        /// <summary>
        /// Changes a specific tag to a new tag for all transactions associated with a particular partner.
        /// </summary>
        /// <param name="comparator">The partner name to match transactions.</param>
        /// <param name="originalTag">The tag to replace.</param>
        /// <param name="replacementTag">The new tag to apply.</param>
        /// <remarks>
        /// For transactions matched by partner, replaces the specified original tag with the new tag, then saves the updated transactions.
        /// </remarks>
        public void TagPartnerChangeBatchProcessing(string comparator, string originalTag, string replacementTag)
        {
            foreach (Transaction t in transactions.Where(t => t.Partner.Equals(comparator)))
            {
                t.Tags[t.Tags.IndexOf(originalTag)] = replacementTag;
            }
            jsonTransaction.SaveData(transactions);
        }

        /// <summary>
        /// Updates the classification for transactions matching a specific original classification to a new classification.
        /// </summary>
        /// <param name="original">The original classification to match. Null matches transactions with null classifications.</param>
        /// <param name="replacement">The new classification to apply.</param>
        /// <remarks>
        /// Transactions with a matching or null classification are updated to the new classification, with the dataset saved subsequently.
        /// </remarks>
        public void ClassificationBatchProcessing(string original, string replacement)
        {
            foreach (Transaction t in transactions.Where(t => original == null ? t.Classification == null : t.Classification.Equals(original)))
            {
                t.Classification = replacement;
            }
            jsonTransaction.SaveData(transactions);
        }

        /// <summary>
        /// Updates the classification of transactions associated with a specified partner.
        /// </summary>
        /// <param name="comparator">The partner name to match transactions.</param>
        /// <param name="replacement">The new classification to apply to these transactions.</param>
        /// <remarks>
        /// Matches transactions by partner, updating their classification to the new value, and saves the updated dataset.
        /// </remarks>
        public void ClassificationPartnerBatchProcessing(string comparator, string replacement)
        {
            foreach (Transaction t in transactions.Where(t =>  t.Partner.Equals(comparator)))
            {
                t.Classification = replacement;
            }
            jsonTransaction.SaveData(transactions);
        }

        /// <summary>
        /// Updates the IBAN for transactions matching a specific original partner IBAN to a new IBAN.
        /// </summary>
        /// <param name="original">The original partner IBAN to match. Null matches transactions with null IBANs.</param>
        /// <param name="replacement">The new IBAN to apply.</param>
        /// <remarks>
        /// Identifies transactions with a matching or null IBAN, updates them to the new IBAN, and saves the dataset.
        /// </remarks>
        public void PartnerIbanBatchProcessing(string original, string replacement)
        {
            foreach (Transaction t in transactions.Where(t => original == null ? t.PartnerIban == null : t.PartnerIban.Equals(original)))
            {
                t.PartnerIban = replacement;
            }
            jsonTransaction.SaveData(transactions);
        }

        /// <summary>
        /// Updates the IBAN of transactions associated with a specific partner.
        /// </summary>
        /// <param name="comparator">The partner name to match transactions.</param>
        /// <param name="replacement">The new IBAN to apply to these transactions.</param>
        /// <remarks>
        /// Finds transactions by partner name, updates their IBAN to the new value, and saves the changes.
        /// </remarks>
        public void PartnerIbanPartnerBatchProcessing(string comparator, string replacement)
        {
            foreach (Transaction t in transactions.Where(t => t.Partner.Equals(comparator)))
            {
                t.PartnerIban = replacement;
            }
            jsonTransaction.SaveData(transactions);
        }

        /// <summary>
        /// Updates the partner name for transactions matching a specific original partner name to a new partner name, including partial matches.
        /// </summary>
        /// <param name="original">The original partner name to match. Null matches transactions with null partner names.</param>
        /// <param name="replacement">The new partner name to apply.</param>
        /// <remarks>
        /// This method updates transactions with a matching or partially matching original partner name to the new partner name and saves the dataset.
        /// </remarks>
        public void PartnerBatchProcessing(string original, string replacement)
        {
            foreach (Transaction t in transactions.Where(t => original == null ? t.Partner == null : t.Partner.Equals(original) || (t.Partner != null && t.Partner.IndexOf(original, StringComparison.OrdinalIgnoreCase) >= 0)))
            {
                t.Partner = replacement;
            }
            jsonTransaction.SaveData(transactions);
        }

        /// <summary>
        /// Changes the partner name for transactions based on a specific description.
        /// </summary>
        /// <param name="comparator">The description to match against transaction descriptions.</param>
        /// <param name="replacement">The new partner name to apply to transactions with the matching description.</param>
        /// <remarks>
        /// For transactions with a matching description, updates the partner name to the new value and saves the dataset.
        /// </remarks>
        public void PartnerDescriptionProcessing(string comparator, string replacement)
        {
            foreach (Transaction t in transactions.Where(t => t.Description.Equals(comparator)))
            {
                t.Partner = replacement;
            }
            jsonTransaction.SaveData(transactions);
        }

        /// <summary>
        /// Updates the project designation for transactions matching a specific original project to a new project.
        /// </summary>
        /// <param name="original">The original project to match. Null matches transactions with null projects.</param>
        /// <param name="replacement">The new project designation.</param>
        /// <remarks>
        /// Applies the new project designation to transactions associated with the specified partner and saves the updated transactions.
        /// </remarks>
        public void ProjectBatchProcessing(string original, string replacement)
        {
            foreach (Transaction t in transactions.Where(t => original == null ? t.Project == null : t.Project.Equals(original)))
            {
                t.Project = replacement;
            }
            jsonTransaction.SaveData(transactions);
        }

        /// <summary>
        /// Updates the project of transactions associated with a specific partner.
        /// </summary>
        /// <param name="comparator">The partner name to match transactions.</param>
        /// <param name="replacement">The new project to apply to these transactions.</param>
        /// <remarks>
        /// Finds transactions by partner name, updates their project to the new value, and saves the changes.
        /// </remarks>
        public void ProjectPartnerBatchProcessing(string comparator, string replacement)
        {
            foreach (Transaction t in transactions.Where(t => t.Partner.Equals(comparator)))
            {
                t.Project = replacement;
            }
            jsonTransaction.SaveData(transactions);
        }

        /// <summary>
        /// Updates the status for transactions matching a specific original status to a new status.
        /// </summary>
        /// <param name="original">The original status to match. Null matches transactions with null status.</param>
        /// <param name="replacement">The new status to apply.</param>
        /// <remarks>
        /// Transactions with a matching or null status are updated to the new status, and the dataset is saved with these changes.
        /// </remarks>
        public void StatusBatchProcessing(string original, string replacement)
        {
            foreach (Transaction t in transactions.Where(t => original == null ? t.Status == null : t.Status.Equals(original)))
            {
                t.Status = replacement;
            }
            jsonTransaction.SaveData(transactions);
        }

        /// <summary>
        /// Updates the status of transactions associated with a specified partner to a new status.
        /// </summary>
        /// <param name="comparator">The partner name to match transactions.</param>
        /// <param name="replacement">The new status to apply to these transactions.</param>
        /// <remarks>
        /// Finds transactions by partner name and updates their status to the new value, saving the updated dataset afterwards.
        /// </remarks>
        public void StatusPartnerBatchProcessing(string comparator, string replacement)
        {
            foreach (Transaction t in transactions.Where(t => t.Partner.Equals(comparator)))
            {
                t.Status = replacement;
            }
            jsonTransaction.SaveData(transactions);
        }

        /// <summary>
        /// Changes the priority of transactions matching a specific original priority to a new priority.
        /// </summary>
        /// <param name="original">The original priority to match. Null matches transactions with null priority.</param>
        /// <param name="replacement">The new priority to apply.</param>
        /// <remarks>
        /// Identifies transactions with a matching or null priority, updates their priority to the new value, and saves the dataset.
        /// </remarks>
        public void PriorityBatchProcessing(string original, string replacement)
        {
            foreach (Transaction t in transactions.Where(t => original == null ? t.Priority == null : t.Priority.Equals(original)))
            {
                t.Priority = replacement;
            }
            jsonTransaction.SaveData(transactions);
        }

        /// <summary>
        /// Updates the priority of transactions associated with a specific partner to a new priority.
        /// </summary>
        /// <param name="comparator">The partner name to match transactions.</param>
        /// <param name="replacement">The new priority to apply to these transactions.</param>
        /// <remarks>
        /// Matches transactions by partner name, updating their priority to the new value, and saves the updated dataset.
        /// </remarks>
        public void PriorityPartnerBatchProcessing(string comparator, string replacement)
        {
            foreach (Transaction t in transactions.Where(t => t.Partner.Equals(comparator)))
            {
                t.Priority = replacement;
            }
            jsonTransaction.SaveData(transactions);
        }

        /// <summary>
        /// Updates the frequency of transactions matching a specific original frequency to a new frequency.
        /// </summary>
        /// <param name="original">The original frequency to match. Null matches transactions with null frequency.</param>
        /// <param name="replacement">The new frequency to apply.</param>
        /// <remarks>
        /// Applies the new frequency to transactions with a matching or null frequency and saves the dataset with these updates.
        /// </remarks>
        public void FrequencyBatchProcessing(string original, string replacement)
        {
            foreach (Transaction t in transactions.Where(t => original == null ? t.Frequency == null : t.Frequency.Equals(original)))
            {
                t.Frequency = replacement;
            }
            jsonTransaction.SaveData(transactions);
        }

        /// <summary>
        /// Changes the frequency of transactions associated with a specified partner to a new frequency.
        /// </summary>
        /// <param name="comparator">The partner name to match transactions.</param>
        /// <param name="replacement">The new frequency to apply to these transactions.</param>
        /// <remarks>
        /// Identifies transactions by partner name and updates their frequency to the new value, with the dataset saved thereafter.
        /// </remarks>
        public void FrequencyPartnerBatchProcessing(string comparator, string replacement)
        {
            foreach (Transaction t in transactions.Where(t => t.Partner.Equals(comparator)))
            {
                t.Frequency = replacement;
            }
            jsonTransaction.SaveData(transactions);
        }

        /// <summary>
        /// Updates the location for transactions matching a specific original location to a new location.
        /// </summary>
        /// <param name="original">The original location to match. Null matches transactions with null location.</param>
        /// <param name="replacement">The new location to apply.</param>
        /// <remarks>
        /// Targets transactions with a matching or null location for update to the new location and saves the updated dataset.
        /// </remarks>
        public void LocationBatchProcessing(string original, string replacement)
        {
            foreach (Transaction t in transactions.Where(t => original == null ? t.Location == null : t.Location.Equals(original)))
            {
                t.Location = replacement;
            }
            jsonTransaction.SaveData(transactions);
        }

        /// <summary>
        /// Updates the location of transactions based on their currency to a new location.
        /// </summary>
        /// <param name="comparator">The currency to match for selecting transactions.</param>
        /// <param name="replacement">The new location to apply to transactions with the specified currency.</param>
        /// <remarks>
        /// Applies a new location to transactions with a matching currency and saves the dataset with these changes.
        /// </remarks>
        public void LocationCurrencyBatchProcessing(string comparator, string replacement)
        {
            foreach (Transaction t in transactions.Where(t => t.Currency.Equals(comparator)))
            {
                t.Location = replacement;
            }
            jsonTransaction.SaveData(transactions);
        }

        /// <summary>
        /// Changes the location of transactions associated with a specific partner to a new location.
        /// </summary>
        /// <param name="comparator">The partner name to match transactions.</param>
        /// <param name="replacement">The new location to apply to these transactions.</param>
        /// <remarks>
        /// Matches transactions by partner name, updating their location to the new value, and saves the updated dataset.
        /// </remarks>
        public void LocationPartnerBatchProcessing(string comparator, string replacement)
        {
            foreach (Transaction t in transactions.Where(t => t.Partner.Equals(comparator)))
            {
                t.Location = replacement;
            }
            jsonTransaction.SaveData(transactions);
        }

        /// <summary>
        /// Updates the notes field for transactions matching a specific original note to a new note.
        /// </summary>
        /// <param name="original">The original note to match. Null matches transactions with null notes.</param>
        /// <param name="replacement">The new note to apply.</param>
        /// <remarks>
        /// Iterates over transactions, updating the notes field where the original note matches or is null, then saves the updated dataset.
        /// </remarks>
        public void NotesBatchProcessing(string original, string replacement)
        {
            foreach (Transaction t in transactions.Where(t => original == null ? t.Notes == null : t.Notes.Equals(original)))
            {
                t.Notes = replacement;
            }
            jsonTransaction.SaveData(transactions);
        }

        /// <summary>
        /// Updates the notes field of transactions associated with a specific partner to a new note.
        /// </summary>
        /// <param name="comparator">The partner name to match transactions.</param>
        /// <param name="replacement">The new note to apply to these transactions.</param>
        /// <remarks>
        /// Finds transactions by partner name and updates their notes to the new value, with the updated dataset saved thereafter.
        /// </remarks>
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
            return (listOfTransactions ?? transactions).Where(t =>  t.Amount > 0);
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
            return (listOfTransactions ?? transactions).Where(t => date == null ? t.Date == null : t.Date.Date == date.Date);
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
            return (listOfTransactions ?? transactions).Where(t => description == null ? t.Description == null : t.Description.IndexOf(description, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        /// <summary>
        /// Retrieves transactions with the specified currency in a case-insensitive manner.
        /// </summary>
        /// <param name="currency">The currency code to filter transactions by.</param>
        /// <param name="listOfTransactions">Optional. The collection of transactions to search. If null, searches the entire collection.</param>
        /// <returns>An IEnumerable of Transaction objects that match the specified currency criteria.</returns>
        public IEnumerable<Transaction> GetTransactionsByCurrency(string currency, IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).Where(t => currency == null ? t.Currency == null : t.Currency?.Equals(currency, StringComparison.OrdinalIgnoreCase) == true);
        }

        /// <summary>
        /// Retrieves transactions that were made using the specified payment method in a case-insensitive manner.
        /// </summary>
        /// <param name="paymentMethod">The payment method to filter transactions by.</param>
        /// <param name="listOfTransactions">Optional. The collection of transactions to search. If null, searches the entire collection.</param>
        /// <returns>An IEnumerable of Transaction objects that match the specified payment method criteria.</returns>
        public IEnumerable<Transaction> GetTransactionsByPaymentMethod(string paymentMethod, IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).Where(t => paymentMethod == null ? t.PaymentMethod == null : t.PaymentMethod?.Equals(paymentMethod, StringComparison.OrdinalIgnoreCase) == true);
        }

        /// <summary>
        /// Retrieves transactions that are marked as tax deductible.
        /// </summary>
        /// <param name="listOfTransactions">Optional. The collection of transactions to search. If null, searches the entire collection.</param>
        /// <returns>An IEnumerable of Transaction objects that are tax deductible.</returns>
        public IEnumerable<Transaction> GetTaxDeductibleTransactions( bool taxDeductible, IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).Where(t => taxDeductible ?  t.TaxDeductible: !t.TaxDeductible);
        }

        /// <summary>
        /// Retrieves transactions that are marked as reimbursable.
        /// </summary>
        /// <param name="listOfTransactions">Optional. The collection of transactions to search. If null, searches the entire collection.</param>
        /// <returns>An IEnumerable of Transaction objects that are reimbursable.</returns>
        public IEnumerable<Transaction> GetReimbursableTransactions(bool reimburable, IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).Where(t => reimburable ? t.Reimbursable : !t.Reimbursable);
        }

        /// <summary>
        /// Retrieves transactions belonging to the specified category in a case-insensitive manner.
        /// </summary>
        /// <param name="category">The category to filter transactions by.</param>
        /// <param name="listOfTransactions">Optional. The collection of transactions to search. If null, searches the entire collection.</param>
        /// <returns>An IEnumerable of Transaction objects that match the specified category criteria.</returns>
        public IEnumerable<Transaction> GetTransactionsByCategory(string category, IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).Where(t => category == null ? t.Category == null : t.Category?.Equals(category, StringComparison.OrdinalIgnoreCase) == true);
        }

        /// <summary>
        /// Retrieves transactions belonging to the specified budget category in a case-insensitive manner.
        /// </summary>
        /// <param name="budgetCategory">The budget category to filter transactions by.</param>
        /// <param name="listOfTransactions">Optional. The collection of transactions to search. If null, searches the entire collection.</param>
        /// <returns>An IEnumerable of Transaction objects that match the specified budget category criteria.</returns>
        public IEnumerable<Transaction> GetTransactionsByBudgetCategory(string budgetCategory, IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).Where(t => budgetCategory == null ? t.BudgetCategory == null : t.BudgetCategory?.Equals(budgetCategory, StringComparison.OrdinalIgnoreCase) == true);
        }

        /// <summary>
        /// Retrieves transactions with a specific classification from a provided or default collection of transactions.
        /// </summary>
        /// <param name="classification">The classification to filter transactions by. Null will match transactions with null classification.</param>
        /// <param name="listOfTransactions">Optional. The collection of transactions to search. If not provided, searches the default transactions collection.</param>
        /// <returns>A filtered enumerable of transactions with the specified classification.</returns>
        /// <remarks>
        /// This method supports filtering transactions by classification, including handling of null values effectively.
        /// </remarks>
        public IEnumerable<Transaction> GetTransactionsByClassification(string classification, IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).Where(t => classification == null ? t.Classification == null : t.Classification?.Equals(classification, StringComparison.OrdinalIgnoreCase) == true);
        }

        /// <summary>
        /// Retrieves transactions associated with the specified partner in a case-insensitive manner.
        /// </summary>
        /// <param name="partner">The partner name to filter transactions by.</param>
        /// <param name="listOfTransactions">Optional. The collection of transactions to search. If null, searches the entire collection.</param>
        /// <returns>An IEnumerable of Transaction objects that match the specified partner criteria.</returns>
        public IEnumerable<Transaction> GetTransactionsByPartner(string partner, IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).Where(t => partner == null ? t.Partner == null : t.Partner.IndexOf(partner, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        /// <summary>
        /// Retrieves transactions associated with the specified project in a case-insensitive manner.
        /// </summary>
        /// <param name="project">The project name to filter transactions by.</param>
        /// <param name="listOfTransactions">Optional. The collection of transactions to search. If null, searches the entire collection.</param>
        /// <returns>An IEnumerable of Transaction objects that match the specified project criteria.</returns>
        public IEnumerable<Transaction> GetTransactionsByProject(string project, IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).Where(t => project == null ? t.Project == null : t.Project?.Equals(project, StringComparison.OrdinalIgnoreCase) == true);
        }

        /// <summary>
        /// Retrieves transactions with the specified status in a case-insensitive manner.
        /// </summary>
        /// <param name="status">The status to filter transactions by.</param>
        /// <param name="listOfTransactions">Optional. The collection of transactions to search. If null, searches the entire collection.</param>
        /// <returns>An IEnumerable of Transaction objects that match the specified status criteria.</returns>
        public IEnumerable<Transaction> GetTransactionsByStatus(string status, IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).Where(t => status == null ? t.Status == null : t.Status?.Equals(status, StringComparison.OrdinalIgnoreCase) == true);
        }

        /// <summary>
        /// Retrieves transactions with the specified priority in a case-insensitive manner.
        /// </summary>
        /// <param name="priority">The priority level to filter transactions by.</param>
        /// <param name="listOfTransactions">Optional. The collection of transactions to search. If null, searches the entire collection.</param>
        /// <returns>An IEnumerable of Transaction objects that match the specified priority criteria.</returns>
        public IEnumerable<Transaction> GetTransactionsByPriority(string priority, IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).Where(t => priority == null ? t.Priority == null : t.Priority?.Equals(priority, StringComparison.OrdinalIgnoreCase) == true);
        }

        /// <summary>
        /// Retrieves transactions with the specified frequency in a case-insensitive manner.
        /// </summary>
        /// <param name="frequency">The frequency to filter transactions by (e.g., monthly, weekly).</param>
        /// <param name="listOfTransactions">Optional. The collection of transactions to search. If null, searches the entire collection.</param>
        /// <returns>An IEnumerable of Transaction objects that match the specified frequency criteria.</returns>
        public IEnumerable<Transaction> GetTransactionsByFrequency(string frequency, IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).Where(t => frequency == null ? t.Frequency == null : t.Frequency?.Equals(frequency, StringComparison.OrdinalIgnoreCase) == true);
        }

        /// <summary>
        /// Retrieves transactions that occurred at the specified location in a case-insensitive manner.
        /// </summary>
        /// <param name="location">The location to filter transactions by.</param>
        /// <param name="listOfTransactions">Optional. The collection of transactions to search. If null, searches the entire collection.</param>
        /// <returns>An IEnumerable of Transaction objects that match the specified location criteria.</returns>
        public IEnumerable<Transaction> GetTransactionsByLocation(string location, IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).Where(t => location == null ? t.Location == null : t.Location.IndexOf(location, StringComparison.OrdinalIgnoreCase) >= 0);
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

        /// <summary>
        /// Groups transactions by date, formatting the key as a short date string.
        /// </summary>
        /// <param name="listOfTransactions">Optional. The collection of transactions to group. If not provided, uses the default transaction collection.</param>
        /// <returns>A collection of transaction groups, each grouped by their transaction date.</returns>
        public IEnumerable<IGrouping<string, Transaction>> GroupByDate(IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).GroupBy(t => t.Date.ToShortDateString()).ToList();
        }

        /// <summary>
        /// Groups transactions by the day of the month.
        /// </summary>
        /// <param name="listOfTransactions">Optional. Specifies the transactions to group. Defaults to the entire transaction collection if not provided.</param>
        /// <returns>A collection of groups, each containing transactions occurring on a specific day of any month.</returns>
        public IEnumerable<IGrouping<string, Transaction>> GroupByDateDay(IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).GroupBy(t => t.Date.Day.ToString()).ToList();
        }

        /// <summary>
        /// Groups transactions by the day of the week.
        /// </summary>
        /// <param name="listOfTransactions">Optional. The collection of transactions to be grouped. Uses the default transaction set if not specified.</param>
        /// <returns>Groups of transactions, each representing transactions occurring on a particular weekday.</returns>
        public IEnumerable<IGrouping<string, Transaction>> GroupByDateWeekDay(IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).GroupBy(t => t.Date.ToString("dddd", CultureInfo.CurrentCulture)).ToList();
        }

        /// <summary>
        /// Groups transactions by the month of occurrence.
        /// </summary>
        /// <param name="listOfTransactions">Optional. The pool of transactions to group. Defaults to the main transaction collection if omitted.</param>
        /// <returns>A collection of transaction groups, each associated with a specific month.</returns>
        public IEnumerable<IGrouping<string, Transaction>> GroupByDateMonth(IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).GroupBy(t => CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(t.Date.Month)).ToList();
        }

        /// <summary>
        /// Groups transactions by the year of occurrence.
        /// </summary>
        /// <param name="listOfTransactions">Optional. Specifies the transactions to be grouped. Uses the entire transaction dataset if not provided.</param>
        /// <returns>A collection of transaction groups, each corresponding to a specific year.</returns>
        public IEnumerable<IGrouping<string, Transaction>> GroupByDateYear(IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).GroupBy(t => t.Date.Year.ToString()).ToList();
        }

        /// <summary>
        /// Groups transactions by their description.
        /// </summary>
        /// <param name="listOfTransactions">Optional. The collection of transactions to be grouped. Defaults to the default transaction dataset if not specified.</param>
        /// <returns>Transaction groups organized by identical descriptions.</returns>
        public IEnumerable<IGrouping<string, Transaction>> GroupByDescription(IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).GroupBy(t => t.Description).ToList();
        }

        /// <summary>
        /// Groups transactions by their amount.
        /// </summary>
        /// <param name="listOfTransactions">Optional. The set of transactions to group. If not provided, the default set of transactions is used.</param>
        /// <returns>A collection of transaction groups, each keyed by a unique transaction amount.</returns>
        public IEnumerable<IGrouping<string, Transaction>> GroupByAmount(IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).GroupBy(t => t.Amount.ToString()).ToList();
        }

        /// <summary>
        /// Groups transactions by a range of amounts, with each group representing a specific range.
        /// </summary>
        /// <param name="listOfTransactions">Optional. The collection of transactions to be grouped. If not provided, the default transaction collection is used.</param>
        /// <returns>A collection of transaction groups, each key representing a range of amounts (e.g., "100-199").</returns>
        /// <remarks>
        /// The range is determined by the floor of the transaction amount divided by 100, multiplied by 100, creating ranges in increments of 100.
        /// </remarks>
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

        /// <summary>
        /// Groups transactions by their currency.
        /// </summary>
        /// <param name="listOfTransactions">Optional. The collection of transactions to be grouped. Defaults to the main transaction collection if not provided.</param>
        /// <returns>A collection of transaction groups, each keyed by a unique currency.</returns>
        public IEnumerable<IGrouping<string, Transaction>> GroupByCurrency(IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).GroupBy(t => t.Currency).ToList();
        }

        /// <summary>
        /// Groups transactions by their payment method.
        /// </summary>
        /// <param name="listOfTransactions">Optional. Specifies the transactions to be grouped. Uses the entire transaction dataset if not provided.</param>
        /// <returns>Groups of transactions, each representing a specific payment method.</returns>
        public IEnumerable<IGrouping<string, Transaction>> GroupByPaymentMethod(IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).GroupBy(t => t.PaymentMethod).ToList();
        }

        /// <summary>
        /// Groups transactions by their category.
        /// </summary>
        /// <param name="listOfTransactions">Optional. The collection of transactions to be grouped. Defaults to the default transaction dataset if not specified.</param>
        /// <returns>Transaction groups organized by category.</returns>
        public IEnumerable<IGrouping<string, Transaction>> GroupByCategory(IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).GroupBy(t => t.Category).ToList();
        }

        /// <summary>
        /// Groups transactions by their budget category.
        /// </summary>
        /// <param name="listOfTransactions">Optional. The set of transactions to group. Defaults to the default set of transactions if not provided.</param>
        /// <returns>A collection of transaction groups, each associated with a specific budget category.</returns>
        public IEnumerable<IGrouping<string, Transaction>> GroupByBudgetCategory(IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).GroupBy(t => t.BudgetCategory).ToList();
        }

        /// <summary>
        /// Groups transactions by tags, with each transaction potentially appearing in multiple groups if it has multiple tags.
        /// </summary>
        /// <param name="listOfTransactions">Optional. The pool of transactions to group. Defaults to the main transaction collection if omitted.</param>
        /// <returns>A collection of transaction groups, each keyed by a unique tag.</returns>
        /// <remarks>
        /// Transactions are expanded into a flat list where each tag is treated as a separate entry, allowing for multi-tag grouping.
        /// </remarks>
        public IEnumerable<IGrouping<string, Transaction>> GroupByTag(IEnumerable<Transaction> listOfTransactions = null)
        {
            var groupedByTag = (listOfTransactions ?? transactions)
                .SelectMany(transaction => transaction.Tags.Select(tag => new { transaction, tag }))
                .GroupBy(x => x.tag, x => x.transaction)
                .ToList();
            return groupedByTag;
        }

        /// <summary>
        /// Groups transactions by their classification.
        /// </summary>
        /// <param name="listOfTransactions">Optional. Specifies the transactions to be grouped. Uses the entire transaction dataset if not provided.</param>
        /// <returns>A collection of transaction groups, each corresponding to a specific classification.</returns>
        public IEnumerable<IGrouping<string, Transaction>> GroupByClassification(IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).GroupBy(t => t.Classification).ToList();
        }

        /// <summary>
        /// Groups transactions by their partner IBAN.
        /// </summary>
        /// <param name="listOfTransactions">Optional. The collection of transactions to group. Defaults to using the primary transaction collection if not provided.</param>
        /// <returns>A collection of transaction groups, each keyed by a unique partner IBAN.</returns>
        public IEnumerable<IGrouping<string, Transaction>> GroupByPartnerIBAN(IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).GroupBy(t => t.PartnerIban).ToList();
        }

        /// <summary>
        /// Groups transactions by their partner name.
        /// </summary>
        /// <param name="listOfTransactions">Optional. Specifies the transactions to be grouped. If not provided, the default transaction collection is used.</param>
        /// <returns>Groups of transactions, each representing a unique partner.</returns>
        public IEnumerable<IGrouping<string, Transaction>> GroupByPartner(IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).GroupBy(t => t.Partner).ToList();
        }

        /// <summary>
        /// Groups transactions by their associated project.
        /// </summary>
        /// <param name="listOfTransactions">Optional. The pool of transactions to be grouped. If omitted, the default set of transactions is used.</param>
        /// <returns>A collection of transaction groups, each associated with a specific project.</returns>
        public IEnumerable<IGrouping<string, Transaction>> GroupByProject(IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).GroupBy(t => t.Project).ToList();
        }

        /// <summary>
        /// Groups transactions by their status.
        /// </summary>
        /// <param name="listOfTransactions">Optional. The set of transactions to group. Defaults to the default set of transactions if not specified.</param>
        /// <returns>Transaction groups organized by status.</returns>
        public IEnumerable<IGrouping<string, Transaction>> GroupByStatus(IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).GroupBy(t => t.Status).ToList();
        }

        /// <summary>
        /// Groups transactions by their priority level.
        /// </summary>
        /// <param name="listOfTransactions">Optional. The collection of transactions to be grouped. If not provided, uses the main transaction collection.</param>
        /// <returns>A collection of transaction groups, each keyed by a unique priority level.</returns>
        public IEnumerable<IGrouping<string, Transaction>> GroupByPriority(IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).GroupBy(t => t.Priority).ToList();
        }

        /// <summary>
        /// Groups transactions by their frequency.
        /// </summary>
        /// <param name="listOfTransactions">Optional. Specifies the transactions to be grouped. Defaults to the entire transaction dataset if not provided.</param>
        /// <returns>A collection of transaction groups, each representing a specific frequency.</returns>
        public IEnumerable<IGrouping<string, Transaction>> GroupByFrequency(IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).GroupBy(t => t.Frequency).ToList();
        }

        /// <summary>
        /// Groups transactions by their location.
        /// </summary>
        /// <param name="listOfTransactions">Optional. The collection of transactions to group. Defaults to the default transaction dataset if not specified.</param>
        /// <returns>Transaction groups organized by location.</returns>
        public IEnumerable<IGrouping<string, Transaction>> GroupByLocation(IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).GroupBy(t => t.Location).ToList();
        }

        /// <summary>
        /// Groups transactions by the notes associated with them.
        /// </summary>
        /// <param name="listOfTransactions">Optional. The set of transactions to be grouped. If not provided, the default set of transactions is used.</param>
        /// <returns>A collection of transaction groups, each associated with specific notes.</returns>
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

        /// <summary>
        /// Initiates a command line interface process for renaming all transaction partners. 
        /// Users are prompted to approve and provide new names for each unique transaction partner.
        /// </summary>
        /// <remarks>
        /// This method groups transactions by partner and iteratively asks the user to confirm renaming. 
        /// If confirmed, it prompts for a new name and applies the change through a batch processing function.
        /// </remarks>
        public void RenameAllTransactionPartnersInComandline()
        {
            IEnumerable<IGrouping<string, Transaction>> groupedTransactions = GroupByPartner(SortTransactionsByPartnerDescending());

            Console.WriteLine(Strings.BatchProcessingOfPartnerNames);
            Console.WriteLine();
            foreach (var group in groupedTransactions)
            {
                Console.WriteLine($"{Strings.Currently} {group.Key}");
                Console.WriteLine($"{Strings.Change}? {Strings.Yes} = 1 / {Strings.No} = 0");
                string input = Console.ReadLine().Trim();
                if (input == "1")
                {
                    Console.WriteLine(Strings.EnterNew + ": ");
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
        /// Offers a command line interface to rename all transaction descriptions that are null. 
        /// Users decide for each partner if the transactions' descriptions should be renamed, providing new descriptions where applicable.
        /// </summary>
        /// <remarks>
        /// Transactions are first filtered by null descriptions and then grouped by partner for batch processing, 
        /// where the user is prompted for each group.
        /// </remarks>
        public void RenameAllNullDescriptionInComandline()
        {
            IEnumerable<IGrouping<string, Transaction>> groupedTransactions = GroupByPartner(SortTransactionsByPartnerDescending(GetTransactionsByDescription(null)));

            Console.WriteLine(Strings.BatchProcessing);
            Console.WriteLine();
            foreach (var group in groupedTransactions)
            {
                Console.WriteLine($"{Strings.Partner} {group.Key}");
                Console.WriteLine($"{Strings.Change}? {Strings.Yes} = 1 / {Strings.No} = 0");
                string input = Console.ReadLine().Trim();
                if (input == "1")
                {
                    Console.WriteLine(Strings.EnterNew + ": ");
                    string newName = Console.ReadLine()?.Trim();
                    if (!string.IsNullOrEmpty(newName))
                    {
                        DescriptionPartnerBatchProcessing(group.Key, newName);
                    }
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Provides a command line interface for renaming transactions with null currency values. 
        /// For each group of transactions by partner, the user can specify new currency values.
        /// </summary>
        /// <remarks>
        /// Targets transactions with null currency values, grouping them by partner. 
        /// It prompts the user to input new currency values where they're missing.
        /// </remarks>
        public void RenameAllNullCurrencyInComandline()
        {
            IEnumerable<IGrouping<string, Transaction>> groupedTransactions = GroupByPartner(SortTransactionsByPartnerDescending(GetTransactionsByCurrency(null)));

            Console.WriteLine(Strings.BatchProcessing);
            Console.WriteLine();
            foreach (var group in groupedTransactions)
            {
                Console.WriteLine($"{Strings.Partner} {group.Key}");
                Console.WriteLine($"{Strings.Change}? {Strings.Yes} = 1 / {Strings.No} = 0");
                string input = Console.ReadLine().Trim();
                if (input == "1")
                {
                    Console.WriteLine(Strings.EnterNew + ": ");
                    string newName = Console.ReadLine()?.Trim();
                    if (!string.IsNullOrEmpty(newName))
                    {
                        CurrencyPartnerBatchProcessing(group.Key, newName);
                    }
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Enables command line batch renaming of transactions lacking a payment method. 
        /// Users are guided to review each partner group and input new payment methods as needed.
        /// </summary>
        /// <remarks>
        /// Focuses on transactions with null payment methods, grouped by partner. 
        /// Each group is presented to the user for potential renaming with new payment method values.
        /// </remarks>
        public void RenameAllNullPaymentMethodInComandline()
        {
            IEnumerable<IGrouping<string, Transaction>> groupedTransactions = GroupByPartner(SortTransactionsByPartnerDescending(GetTransactionsByPaymentMethod(null)));

            Console.WriteLine(Strings.BatchProcessing);
            Console.WriteLine();
            foreach (var group in groupedTransactions)
            {
                Console.WriteLine($"{Strings.Partner} {group.Key}");
                Console.WriteLine($"{Strings.Change}? {Strings.Yes} = 1 / {Strings.No} = 0");
                string input = Console.ReadLine().Trim();
                if (input == "1")
                {
                    Console.WriteLine(Strings.EnterNew + ": ");
                    string newName = Console.ReadLine()?.Trim();
                    if (!string.IsNullOrEmpty(newName))
                    {
                        PaymentMethodPartnerBatchProcessing(group.Key, newName);
                    }
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Initiates a command line process for renaming transaction categories that are null. 
        /// Users can update categories for transactions grouped by partner.
        /// </summary>
        /// <remarks>
        /// Specifically targets transactions with null categories, grouping them for user review. 
        /// Offers the option to update the category for each group.
        /// </remarks>
        public void RenameAllNullCategoryInComandline()
        {
            IEnumerable<IGrouping<string, Transaction>> groupedTransactions = GroupByPartner(SortTransactionsByPartnerDescending(GetTransactionsByCategory(null)));

            Console.WriteLine(Strings.BatchProcessing);
            Console.WriteLine();
            foreach (var group in groupedTransactions)
            {
                Console.WriteLine($"{Strings.Partner} {group.Key}");
                Console.WriteLine($"{Strings.Change}? {Strings.Yes} = 1 / {Strings.No} = 0");
                string input = Console.ReadLine().Trim();
                if (input == "1")
                {
                    Console.WriteLine(Strings.EnterNew + ": ");
                    string newName = Console.ReadLine()?.Trim();
                    if (!string.IsNullOrEmpty(newName))
                    {
                        CategoryPartnerBatchProcessing(group.Key, newName);
                    }
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Launches a command line interface for users to rename transactions with null budget categories. 
        /// Each partner's transaction group is presented for optional renaming.
        /// </summary>
        /// <remarks>
        /// Addresses transactions missing budget categories, organizing them by partner for user action. 
        /// Users can provide new budget categories for each group.
        /// </remarks>
        public void RenameAllNullBudgetCategoryInComandline()
        {
            IEnumerable<IGrouping<string, Transaction>> groupedTransactions = GroupByPartner(SortTransactionsByPartnerDescending(GetTransactionsByBudgetCategory(null)));

            Console.WriteLine(Strings.BatchProcessing);
            Console.WriteLine();
            foreach (var group in groupedTransactions)
            {
                Console.WriteLine($"{Strings.Partner} {group.Key}");
                Console.WriteLine($"{Strings.Change}? {Strings.Yes} = 1 / {Strings.No} = 0");
                string input = Console.ReadLine().Trim();
                if (input == "1")
                {
                    Console.WriteLine(Strings.EnterNew + ": ");
                    string newName = Console.ReadLine()?.Trim();
                    if (!string.IsNullOrEmpty(newName))
                    {
                        BudgetCategoryPartnerBatchProcessing(group.Key, newName);
                    }
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Provides a command line interface for adding tags to transactions. 
        /// Users can repeatedly add tags to transactions grouped by partner until they choose to stop.
        /// </summary>
        /// <remarks>
        /// Allows users to iteratively add tags to transaction groups, sorted by partner. 
        /// After adding a tag, users are prompted if they want to add another, continuing until they decline.
        /// </remarks>
        public void AddTagsInComandline()
        {
            IEnumerable<IGrouping<string, Transaction>> groupedTransactions = GroupByPartner(SortTransactionsByPartnerDescending(GetTransactionsByBudgetCategory(null)));

            Console.WriteLine(Strings.BatchProcessing);
            Console.WriteLine();
            foreach (var group in groupedTransactions)
            {
                Console.WriteLine($"{Strings.Partner} {group.Key}");
                string input;
                do
                {
                    Console.WriteLine($"{Strings.Add}? {Strings.Yes} = 1 / {Strings.No} = 0");
                    input = Console.ReadLine().Trim();
                    if (input == "1")
                    {
                        Console.WriteLine(Strings.EnterNew + ": ");
                        string newName = Console.ReadLine()?.Trim();
                        if (!string.IsNullOrEmpty(newName))
                        {
                            TagPartnerAddBatchProcessing(group.Key, newName);
                        }
                    }
                } while(input == "1");

                Console.WriteLine();
            }
        }

        /// <summary>
        /// Initiates a command line interface process for batch updating transactions with null classifications. 
        /// Transactions are grouped and sorted by partner in descending order for user review and update.
        /// </summary>
        /// <remarks>
        /// Users are prompted to enter new classification names for groups of transactions, 
        /// allowing for data correction and enrichment directly via the command line.
        /// </remarks>
        public void RenameAllNullClassificationInComandline()
        {
            IEnumerable<IGrouping<string, Transaction>> groupedTransactions = GroupByPartner(SortTransactionsByPartnerDescending(GetTransactionsByClassification(null)));

            Console.WriteLine(Strings.BatchProcessing);
            Console.WriteLine();
            foreach (var group in groupedTransactions)
            {
                Console.WriteLine($"{Strings.Partner} {group.Key}");
                Console.WriteLine($"{Strings.Change}? {Strings.Yes} = 1 / {Strings.No} = 0");
                string input = Console.ReadLine().Trim();
                if (input == "1")
                {
                    Console.WriteLine(Strings.EnterNew + ": ");
                    string newName = Console.ReadLine()?.Trim();
                    if (!string.IsNullOrEmpty(newName))
                    {
                        ClassificationPartnerBatchProcessing(group.Key, newName);
                    }
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Provides a command line interface for batch updating transactions with null project names. 
        /// Transactions are grouped and sorted by partner in descending order for user action.
        /// </summary>
        /// <remarks>
        /// This process helps in updating transaction records with missing project information, 
        /// facilitating data management and consistency.
        /// </remarks>
        public void RenameAllNullProjectInComandline()
        {
            IEnumerable<IGrouping<string, Transaction>> groupedTransactions = GroupByPartner(SortTransactionsByPartnerDescending(GetTransactionsByProject(null)));

            Console.WriteLine(Strings.BatchProcessing);
            Console.WriteLine();
            foreach (var group in groupedTransactions)
            {
                Console.WriteLine($"{Strings.Partner} {group.Key}");
                Console.WriteLine($"{Strings.Change}? {Strings.Yes} = 1 / {Strings.No} = 0");
                string input = Console.ReadLine().Trim();
                if (input == "1")
                {
                    Console.WriteLine(Strings.EnterNew + ": ");
                    string newName = Console.ReadLine()?.Trim();
                    if (!string.IsNullOrEmpty(newName))
                    {
                        ProjectPartnerBatchProcessing(group.Key, newName);
                    }
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Offers a command line process for batch renaming transactions that lack a status. 
        /// Transactions are grouped by partner and sorted in descending order.
        /// </summary>
        /// <remarks>
        /// The interface prompts users for new status values, targeting transactions with null statuses 
        /// to improve data completeness and accuracy.
        /// </remarks>
        public void RenameAllNullStatusInComandline()
        {
            IEnumerable<IGrouping<string, Transaction>> groupedTransactions = GroupByPartner(SortTransactionsByPartnerDescending(GetTransactionsByStatus(null)));

            Console.WriteLine(Strings.BatchProcessing);
            Console.WriteLine();
            foreach (var group in groupedTransactions)
            {
                Console.WriteLine($"{Strings.Partner} {group.Key}");
                Console.WriteLine($"{Strings.Change}? {Strings.Yes} = 1 / {Strings.No} = 0");
                string input = Console.ReadLine().Trim();
                if (input == "1")
                {
                    Console.WriteLine(Strings.EnterNew + ": ");
                    string newName = Console.ReadLine()?.Trim();
                    if (!string.IsNullOrEmpty(newName))
                    {
                        StatusPartnerBatchProcessing(group.Key, newName);
                    }
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Enables a command line interface for users to update transactions missing priority information, 
        /// grouping and sorting transactions by partner in descending order.
        /// </summary>
        /// <remarks>
        /// By providing new priority values, users can directly enhance the transaction dataset, 
        /// ensuring that priority data is comprehensive and up-to-date.
        /// </remarks>
        public void RenameAllNullPriorityInComandline()
        {
            IEnumerable<IGrouping<string, Transaction>> groupedTransactions = GroupByPartner(SortTransactionsByPartnerDescending(GetTransactionsByPriority(null)));

            Console.WriteLine(Strings.BatchProcessing);
            Console.WriteLine();
            foreach (var group in groupedTransactions)
            {
                Console.WriteLine($"{Strings.Partner} {group.Key}");
                Console.WriteLine($"{Strings.Change}? {Strings.Yes} = 1 / {Strings.No} = 0");
                string input = Console.ReadLine().Trim();
                if (input == "1")
                {
                    Console.WriteLine(Strings.EnterNew + ": ");
                    string newName = Console.ReadLine()?.Trim();
                    if (!string.IsNullOrEmpty(newName))
                    {
                        PriorityPartnerBatchProcessing(group.Key, newName);
                    }
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Launches a command line interface for updating transactions with null frequency values, 
        /// sorting and grouping them by partner in descending order for ease of update.
        /// </summary>
        /// <remarks>
        /// This method assists in the correction and completion of transaction records, 
        /// particularly in updating frequency information where it is absent.
        /// </remarks>
        public void RenameAllNullFrequencyInComandline()
        {
            IEnumerable<IGrouping<string, Transaction>> groupedTransactions = GroupByPartner(SortTransactionsByPartnerDescending(GetTransactionsByFrequency(null)));

            Console.WriteLine(Strings.BatchProcessing);
            Console.WriteLine();
            foreach (var group in groupedTransactions)
            {
                Console.WriteLine($"{Strings.Partner} {group.Key}");
                Console.WriteLine($"{Strings.Change}? {Strings.Yes} = 1 / {Strings.No} = 0");
                string input = Console.ReadLine().Trim();
                if (input == "1")
                {
                    Console.WriteLine(Strings.EnterNew + ": ");
                    string newName = Console.ReadLine()?.Trim();
                    if (!string.IsNullOrEmpty(newName))
                    {
                        FrequencyPartnerBatchProcessing(group.Key, newName);
                    }
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Initiates a command line process for users to batch update transactions with null location data, 
        /// with transactions grouped and sorted by partner in descending order.
        /// </summary>
        /// <remarks>
        /// Allows users to fill in missing location details in transaction records, 
        /// contributing to the overall data quality and utility.
        /// </remarks>
        public void RenameAllNullLocationInComandline()
        {
            IEnumerable<IGrouping<string, Transaction>> groupedTransactions = GroupByPartner(SortTransactionsByPartnerDescending(GetTransactionsByLocation(null)));

            Console.WriteLine(Strings.BatchProcessing);
            Console.WriteLine();
            foreach (var group in groupedTransactions)
            {
                Console.WriteLine($"{Strings.Partner} {group.Key}");
                Console.WriteLine($"{Strings.Change}? {Strings.Yes} = 1 / {Strings.No} = 0");
                string input = Console.ReadLine().Trim();
                if (input == "1")
                {
                    Console.WriteLine(Strings.EnterNew + ": ");
                    string newName = Console.ReadLine()?.Trim();
                    if (!string.IsNullOrEmpty(newName))
                    {
                        LocationPartnerBatchProcessing(group.Key, newName);
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
            jsonTransaction.OpenFileWithDefaultProgram();
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

        /// <summary>
        /// Displays transactions grouped by a key in the console, including each transaction's details.
        /// </summary>
        /// <param name="groupedTransactions">The collection of transaction groups, each identified by a string key.</param>
        /// <remarks>
        /// For each group, this method first prints the group key to the console. 
        /// It then iterates through each transaction within the group, calling a method to display its short details.
        /// A new line is printed after all transactions in a group have been displayed.
        /// </remarks>
        public void DisplayGroupedTransactionsComandline(IEnumerable<IGrouping<string, Transaction>> groupedTransactions)
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

        /// <summary>
        /// Displays only the names (keys) of grouped transactions in the console.
        /// </summary>
        /// <param name="groupedTransactions">The collection of transaction groups, each identified by a string key.</param>
        /// <remarks>
        /// This method iterates through each group of transactions and prints just the group key to the console, 
        /// allowing for a quick overview of all group names without transaction details.
        /// </remarks>
        public void DisplayOnlyGroupnamesComandline(IEnumerable<IGrouping<string, Transaction>> groupedTransactions)
        {
            foreach (var group in groupedTransactions)
            {
                Console.WriteLine(group.Key);
            }
        }
    }
}

