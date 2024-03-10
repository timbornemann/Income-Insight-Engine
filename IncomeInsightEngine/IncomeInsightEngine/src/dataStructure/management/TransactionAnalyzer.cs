using dataStructure;
using IncomeInsightEngine.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncomeInsightEngine.src.dataStructure.management
{
    internal class TransactionAnalyzer
    {


        /// <summary>
        /// Calculates the total amount from a provided collection of transactions.
        /// </summary>
        /// <param name="listOfTransactions">The enumerable collection of Transaction objects.</param>
        /// <returns>The total amount as a decimal, representing the sum of the Amount property of each transaction in the collection.</returns>
        /// <remarks>
        /// This method iterates over the provided collection of transactions, summing up the 'Amount' property of each transaction to calculate the total amount. 
        /// It is a flexible and reusable method that can operate on any enumerable collection of Transaction objects, making it useful for calculating totals 
        /// from different sets of transactions.
        /// </remarks>
        public decimal CalculateTotalAmount(IEnumerable<Transaction> listOfTransactions)
        {
            return listOfTransactions.Sum(t => t.Amount);
        }

        /// <summary>
        /// Calculates the total income from a provided collection of transactions, considering only transactions with positive amounts.
        /// </summary>
        /// <param name="listOfTransactions">The enumerable collection of Transaction objects to calculate income from.</param>
        /// <returns>The total income as a decimal, summed from the Amount property of transactions with positive amounts.</returns>
        /// <remarks>
        /// This method filters the provided collection of transactions to include only those with a positive amount, indicating income transactions.
        /// It then sums the amounts of these filtered transactions to calculate the total income. This approach allows for flexibility in calculating
        /// income from different sets of transactions, whether they are the entire collection or a subset.
        /// </remarks>
        public decimal CalculateTotalIncome(IEnumerable<Transaction> listOfTransactions)
        {
            return listOfTransactions.Where(t => t.Amount > 0).Sum(t => t.Amount);
        }

        /// <summary>
        /// Calculates the total expenses from a provided collection of transactions, considering only transactions with negative amounts.
        /// </summary>
        /// <param name="listOfTransactions">The enumerable collection of Transaction objects to calculate expenses from.</param>
        /// <returns>The total expenses as a decimal, summed from the Amount property of transactions with negative amounts.</returns>
        /// <remarks>
        /// This method filters the provided collection of transactions to include only those with a negative amount, indicating expense transactions.
        /// It then sums the amounts of these filtered transactions to calculate the total expenses. This approach allows for flexibility in calculating
        /// expenses from different sets of transactions, whether they are the entire collection or a subset, ensuring accurate financial analysis and reporting.
        /// </remarks>
        public decimal CalculateTotalExpenses(IEnumerable<Transaction> listOfTransactions)
        {
            return listOfTransactions.Where(t => t.Amount < 0).Sum(t => t.Amount);
        }

        public IEnumerable<(string key, decimal TotalExpenses)> CalculateGroupedExpenses(IEnumerable<IGrouping<string, Transaction>> groupedTransactions)
        {
            return groupedTransactions
                .Select(group => (
                    key: group.Key,
                    TotalExpenses: CalculateTotalExpenses(group)
                ))
                .ToList();
        }

        public IEnumerable<(string key, decimal TotalIncome)> CalculateGroupedIncome(IEnumerable<IGrouping<string, Transaction>> groupedTransactions)
        {
            return groupedTransactions
                .Select(group => (
                    key: group.Key,
                    TotalIncome: CalculateTotalIncome(group)
                ))
                .ToList();
        }

        public IEnumerable<(string key, decimal TotalAmount)> CalculateGroupedAmount(IEnumerable <IGrouping<string, Transaction>> groupedTransactions)
        {
            return groupedTransactions
                .Select(group => (
                    key: group.Key,
                    TotalAmount: CalculateTotalAmount(group)
                ))
                .ToList();
        }



        public IEnumerable<(string key, decimal Amount)> SortAmountAscending(IEnumerable<(string key, decimal Amount)> groups)
        {
            return groups.OrderBy(g => g.Amount);
        }

        public IEnumerable<(string key, decimal Amount)> SortAmountDescending(IEnumerable<(string key, decimal Amount)> groups)
        {
            return groups.OrderByDescending(g => g.Amount);
        }

        public IEnumerable<(string key, decimal Amount)> SortKeyAscending(IEnumerable<(string key, decimal Amount)> groups)
        {
            return groups.OrderBy(g => g.key);
        }

        public IEnumerable<(string key, decimal Amount)> SortKeyDescending(IEnumerable<(string key, decimal Amount)> groups)
        {
            return groups.OrderByDescending(g => g.key);
        }



        /// <summary>
        /// Displays the total expenses calculated from the provided or current collection of transactions in the command line.
        /// </summary>
        /// <param name="listOfTransactions">Optional. The collection of transactions to calculate expenses from. If null, calculates from the current collection.</param>
        /// <remarks>
        /// This method calculates the total expenses from the specified collection of transactions and prints the result to the command line.
        /// </remarks>
        public void DisplayTotalExpensesInComandline(IEnumerable<Transaction> listOfTransactions)
        {
            Console.WriteLine(Strings.TotalExpenses + " " + CalculateTotalExpenses(listOfTransactions));
        }

        /// <summary>
        /// Displays the total income calculated from the provided or current collection of transactions in the command line.
        /// </summary>
        /// <param name="listOfTransactions">Optional. The collection of transactions to calculate income from. If null, calculates from the current collection.</param>
        /// <remarks>
        /// This method calculates the total income from the specified collection of transactions and prints the result to the command line.
        /// </remarks>
        public void DisplayTotalIncomeInComandline(IEnumerable<Transaction> listOfTransactions)
        {
            Console.WriteLine(Strings.TotalIncome + " " + CalculateTotalIncome(listOfTransactions));
        }

        /// <summary>
        /// Displays the total amount calculated from the provided or current collection of transactions in the command line.
        /// </summary>
        /// <param name="listOfTransactions">Optional. The collection of transactions to calculate the total amount from. If null, calculates from the current collection.</param>
        /// <remarks>
        /// This method calculates the total amount from the specified collection of transactions and prints the result to the command line.
        /// </remarks>
        public void DisplayTotalAmountInComandline(IEnumerable<Transaction> listOfTransactions)
        {
            Console.WriteLine(Strings.TotalAmount + " " + CalculateTotalAmount(listOfTransactions));
        }

        public void DisplayGroupedAmountInComandline(IEnumerable<(string key, decimal Amount)> GroupedExpenses)
        {          
            foreach (var (Key, Amount) in GroupedExpenses)
            {
                Console.WriteLine($"{Key,-50} {Strings.TotalAmount,-1} {Amount:C}");
            }
        }

    }
}
