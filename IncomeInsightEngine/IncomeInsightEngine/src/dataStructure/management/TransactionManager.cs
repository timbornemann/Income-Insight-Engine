using dataStructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IncomeInsightEngine.src.dataStructure.management
{
    public class TransactionManager
    {
      

        private List<Transaction> transactions;
        private JsonTransaction jsonTransaction;

        public TransactionManager()
        {
            jsonTransaction = new JsonTransaction();
            transactions = new List<Transaction>();


            
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

        public bool AddTransaction(List<Transaction> data)
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




        public IEnumerable<Transaction> GetAllTransactions()
        {
            return transactions.AsReadOnly();
        }

        public Transaction GetTransactionById(int id)
        {
            return transactions.FirstOrDefault(t => t.Id == id);
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

        public void DisplayAllTransactionsShortInComandline()
        {
            foreach (var transaction in transactions)
            {
                transaction.DisplayShortTransactionDetails();
            }
        }

        public void DisplayAllTransactionsCompleteInComandline()
        {
            foreach (var transaction in transactions)
            {
                transaction.DisplayTransactionDetails();
            }
        }


        public IEnumerable<Transaction> FilterTransactionsByCategory(string category)
        {
            return transactions.Where(t => t.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
        }

        public decimal CalculateTotalIncome()
        {
            return transactions.Where(t => t.Amount > 0).Sum(t => t.Amount);
        }

        public decimal CalculateTotalExpenses()
        {
            return transactions.Where(t => t.Amount < 0).Sum(t => t.Amount);
        }

        
        public void SortTransactionsByDateAscending()
        {
            transactions = transactions.OrderBy(t => t.Date).ToList();
        }

       
        public void SortTransactionsByDateDescending()
        {
            transactions = transactions.OrderByDescending(t => t.Date).ToList();
        }

      
        public void SortTransactionsByAmountAscending()
        {
            transactions = transactions.OrderBy(t => t.Amount).ToList();
        }

       
        public void SortTransactionsByAmountDescending()
        {
            transactions = transactions.OrderByDescending(t => t.Amount).ToList();
        }

        
        public void SortTransactionsByPartnerAscending()
        {
            transactions = transactions.OrderBy(t => t.Partner).ToList();
        }

        public void SortTransactionsByPartnerDescending()
        {
            transactions = transactions.OrderByDescending(t => t.Partner).ToList();
        }

        public void SortTransactionsByLocationAscending()
        {
            transactions = transactions.OrderBy(t => t.Location).ToList();
        }

        public void SortTransactionsByLocationDescending()
        {
            transactions = transactions.OrderByDescending(t => t.Location).ToList();
        }

        public void OpenFileManually()
        {
            jsonTransaction.OpenTransactionFile();
        }

        public void CloseFileManually()
        {
            jsonTransaction.CloseTransactionFile();
        }

    }
}
