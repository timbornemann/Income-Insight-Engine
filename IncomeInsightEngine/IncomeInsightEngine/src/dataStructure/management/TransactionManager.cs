using dataStructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace IncomeInsightEngine.src.dataStructure.management
{
    public class TransactionManager
    {
      

        private List<Transaction> transactions = new List<Transaction>();
        private JsonTransaction jsonTransaction;

        public TransactionManager()
        {
            jsonTransaction = new JsonTransaction();
          
            LoadTransactions();
        }

        private void LoadTransactions()
        {
                        
                transactions = jsonTransaction.ReadData();
            
        }

        public bool AddTransaction(Transaction transaction)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException(nameof(transaction));
            }

            if (TransactionAlreadyExists(transaction))
            {
                return false;
            }

            var existingTransaction = GetTransactionById(transaction.Id);
            if (existingTransaction != null)
            {             
                return false; 
            }

            transactions.Add(transaction);
            jsonTransaction.AddTransaction(transaction);
            return true;
        }

        public bool AddTransactions(List<Transaction> data)
        {
            if(data == null)
            {
                return false;
            }


            foreach (var transaction in data)
            {

                if (transaction == null)
                {
                    throw new ArgumentNullException(nameof(transaction));
                }

                if (TransactionAlreadyExists(transaction))
                {                
                    continue;
                }

                var existingTransaction = GetTransactionById(transaction.Id);
                if (existingTransaction == null)
                {
                    transactions.Add(transaction);
                    jsonTransaction.AddTransaction(transaction);
                }

            
            }
            return true;
        }
        
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
            transaction.PartnerIBAN = updatedTransaction.PartnerIBAN;
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

            return true;
        }

        public bool RemoveTransaction(int id)
        {
            var transaction = GetTransactionById(id);
            if (transaction != null)
            {
                transactions.Remove(transaction);
                return true;
            }
            return false;
        }

        public Transaction GetTransactionById(int id)
        {
            return transactions.FirstOrDefault(t => t.Id == id);
        }

        public IEnumerable<Transaction> GetAllTransactions()
        {
            return transactions.AsReadOnly();
        }

        public IEnumerable<Transaction> GetIncomeTransactions(IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).Where(t => t.Amount > 0);
        }

        public IEnumerable<Transaction> GetExpenseTransactions(IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).Where(t => t.Amount < 0);
        }

        public IEnumerable<Transaction> GetTransactionsByAmount(decimal amount , IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).Where(t => t.Amount == amount);
        }

        public IEnumerable<Transaction> GetTransactionsByAmount(decimal minAmount, decimal maxAmount, IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).Where(t => t.Amount >= minAmount && t.Amount <= maxAmount);
        }

        public IEnumerable<Transaction> GetTransactionsByDate(DateTime date, IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).Where(t => t.Date.Date == date.Date);
        }

        public IEnumerable<Transaction> GetTransactionsByDate(DateTime startDate, DateTime endDate, IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).Where(t => t.Date.Date >= startDate.Date && t.Date.Date <= endDate.Date);
        }

        public IEnumerable<Transaction> GetTransactionsByDescription(string description, IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).Where(t => t.Description != null && t.Description.IndexOf(description, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        public IEnumerable<Transaction> GetTransactionsByCurrency(string currency, IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).Where(t => t.Currency.Equals(currency, StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable<Transaction> GetTransactionsByPaymentMethod(string paymentMethod, IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).Where(t => t.PaymentMethod.Equals(paymentMethod, StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable<Transaction> GetTaxDeductibleTransactions(IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).Where(t => t.TaxDeductible);
        }

        public IEnumerable<Transaction> GetReimbursableTransactions(IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).Where(t => t.Reimbursable);
        }

        public IEnumerable<Transaction> GetTransactionsByCategory(string category, IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).Where(t => t.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable<Transaction> GetTransactionsByBudgetCategory(string budgetCategory, IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).Where(t => t.BudgetCategory.Equals(budgetCategory, StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable<Transaction> GetTransactionsByPartner(string partner, IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).Where(t => t.Partner != null && t.Partner.IndexOf(partner, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        public IEnumerable<Transaction> GetTransactionsByProject(string project, IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).Where(t => t.Project.Equals(project, StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable<Transaction> GetTransactionsByStatus(string status, IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).Where(t => t.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable<Transaction> GetTransactionsByPriority(string priority, IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).Where(t => t.Priority.Equals(priority, StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable<Transaction> GetTransactionsByFrequency(string frequency, IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).Where(t => t.Frequency.Equals(frequency, StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable<Transaction> GetTransactionsByLocation(string location, IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).Where(t => t.Location != null && t.Location.IndexOf(location, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        public IEnumerable<Transaction> GetTransactionsByTag(string tag, IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).Where(t => t.Tags.Contains(tag));
        }

        public IEnumerable<Transaction> GetTransactionsByNotes(string notes, IEnumerable<Transaction> listOfTransactions = null)
        {
            return (listOfTransactions ?? transactions).Where(t => t.Notes != null && t.Notes.IndexOf(notes, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        public decimal CalculateTotalAmount()
        {
            return CalculateTotalAmount(transactions);
        }

        public decimal CalculateTotalAmount(IEnumerable<Transaction> listOfTransactions)
        {
            return listOfTransactions.Sum(t => t.Amount);
        }

        public decimal CalculateTotalIncome()
        {
            return CalculateTotalIncome(transactions);
        }

        public decimal CalculateTotalIncome(IEnumerable<Transaction> listOfTransactions)
        {
            return listOfTransactions.Where(t => t.Amount > 0).Sum(t => t.Amount);
        }

        public decimal CalculateTotalExpenses()
        {
            return CalculateTotalExpenses(transactions);
        }

        public decimal CalculateTotalExpenses(IEnumerable<Transaction> listOfTransactions)
        {
            return listOfTransactions.Where(t => t.Amount < 0).Sum(t => t.Amount);
        }

        public IEnumerable<Transaction> SortTransactionsByDateAscending()
        {
            transactions = SortTransactionsByDateAscending(transactions).ToList();
            return transactions.AsReadOnly();
        }

        public IEnumerable<Transaction> SortTransactionsByDateAscending(IEnumerable<Transaction> listOfTransactions)
        {
            return listOfTransactions.OrderBy(t => t.Date);
        }

        public IEnumerable<Transaction> SortTransactionsByDateDescending()
        {
            transactions = SortTransactionsByDateDescending(transactions).ToList();
            return transactions.AsReadOnly();
        }

        public IEnumerable<Transaction> SortTransactionsByDateDescending(IEnumerable<Transaction> listOfTransactions)
        {
            return listOfTransactions.OrderByDescending(t => t.Date);
        }

        public IEnumerable<Transaction> SortTransactionsByAmountAscending()
        {
            transactions = SortTransactionsByAmountAscending(transactions).ToList();
            return transactions.AsReadOnly();
        }

        public IEnumerable<Transaction> SortTransactionsByAmountAscending(IEnumerable<Transaction> listOfTransactions)
        {
            return listOfTransactions.OrderBy(t => t.Amount);
        }

        public IEnumerable<Transaction> SortTransactionsByAmountDescending()
        {
            transactions = SortTransactionsByAmountDescending(transactions).ToList();
            return transactions.AsReadOnly();
        }

        public IEnumerable<Transaction> SortTransactionsByAmountDescending(IEnumerable<Transaction> listOfTransactions)
        {
            return listOfTransactions.OrderByDescending(t => t.Amount);
        }

        public IEnumerable<Transaction> SortTransactionsByPartnerAscending()
        {
            transactions = SortTransactionsByPartnerAscending(transactions).ToList();
            return transactions.AsReadOnly();
        }

        public IEnumerable<Transaction> SortTransactionsByPartnerAscending(IEnumerable<Transaction> listOfTransactions)
        {
            return listOfTransactions.OrderBy(t => t.Partner);
        }

        public IEnumerable<Transaction> SortTransactionsByPartnerDescending()
        {
            transactions = SortTransactionsByPartnerDescending(transactions).ToList();
            return transactions.AsReadOnly();
        }

        public IEnumerable<Transaction> SortTransactionsByPartnerDescending(IEnumerable<Transaction> listOfTransactions)
        {
            return listOfTransactions.OrderByDescending(t => t.Partner);
        }

        public IEnumerable<Transaction> SortTransactionsByLocationAscending()
        {
            transactions = SortTransactionsByLocationAscending(transactions).ToList();
            return transactions.AsReadOnly();
        }

        public IEnumerable<Transaction> SortTransactionsByLocationAscending(IEnumerable<Transaction> listOfTransactions)
        {
            return listOfTransactions.OrderBy(t => t.Location);
        }

        public IEnumerable<Transaction> SortTransactionsByLocationDescending()
        {
            transactions = SortTransactionsByLocationDescending(transactions).ToList();
            return transactions.AsReadOnly();
        }

        public IEnumerable<Transaction> SortTransactionsByLocationDescending(IEnumerable<Transaction> listOfTransactions)
        {
            return listOfTransactions.OrderByDescending(t => t.Location);
        }

        private bool TransactionAlreadyExists(Transaction transaction)
        {

            foreach(Transaction transaction1 in transactions)
            {
                if (transaction.Equals(transaction1))
                {
                    return true;
                }
            }

            return false;
        }
        
        public void OpenTransactionFileManually()
        {
            jsonTransaction.OpenTransactionFile();
        }

        public void CloseTransactionFileManually()
        {
            jsonTransaction.CloseTransactionFile();
        }
        
        public void DisplayShortTransactionInformationsInComandline(IEnumerable<Transaction> listOfTransactions = null)
        {
            foreach (var transaction in (listOfTransactions ?? transactions))
            {
                transaction.DisplayShortTransactionDetails();
            }
        }

        public void DisplayCompleteTransactionInformationsInComandline(IEnumerable<Transaction> listOfTransactions = null)
        {
            foreach (var transaction in (listOfTransactions ?? transactions))
            {
                transaction.DisplayTransactionDetails();
            }
        }

        public void DisplayTotalExpensesInComandline(IEnumerable<Transaction> listOfTransactions = null)
        {
           Console.WriteLine("Total expenses: " + CalculateTotalExpenses(listOfTransactions));
        }

        public void DisplayTotalIncomeInComandline(IEnumerable<Transaction> listOfTransactions = null)
        {
            Console.WriteLine("Total income: " + CalculateTotalIncome(listOfTransactions));
        }

        public void DisplayTotalAmountInComandline(IEnumerable<Transaction> listOfTransactions = null)
        {
            Console.WriteLine("Total amount: " + CalculateTotalAmount(listOfTransactions));
        }

    }
}
