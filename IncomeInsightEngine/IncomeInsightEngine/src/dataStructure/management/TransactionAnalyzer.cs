using dataStructure;
using IncomeInsightEngine.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

        public decimal CalculateAverageAmount(IEnumerable<Transaction> listOfTransactions)
        {
            if (listOfTransactions == null || !listOfTransactions.Any())
            {
                return 0m; 
            }

            return listOfTransactions.Average(t => t.Amount);
        }

        public decimal CalculateAverageIncome(IEnumerable<Transaction> listOfTransactions)
        {
            if (listOfTransactions == null || !listOfTransactions.Any(t => t.Amount > 0))
            {
                return 0m; 
            }

            return listOfTransactions.Where(t => t.Amount > 0).Average(t => t.Amount);
        }

        public decimal CalculateAverageExpense(IEnumerable<Transaction> listOfTransactions)
        {
            if (listOfTransactions == null || !listOfTransactions.Any(t => t.Amount < 0))
            {
                return 0m; 
            }

            return listOfTransactions.Where(t => t.Amount < 0).Average(t => t.Amount);
        }

        public decimal CalculateVariance(IEnumerable<Transaction> listOfTransactions)
        {
            var average = CalculateAverageAmount(listOfTransactions);
            var variance = listOfTransactions.Sum(t => Math.Pow((double)(t.Amount - average), 2)) / listOfTransactions.Count();
            return (decimal)variance;
        }

        public decimal CalculateStandardDeviation(IEnumerable<Transaction> listOfTransactions)
        {
            var variance = CalculateVariance(listOfTransactions);
            return (decimal)Math.Sqrt((double)variance);
        }

        public decimal CalculateMedian(IEnumerable<Transaction> listOfTransactions)
        {
            var sortedTransactions = listOfTransactions.Select(t => t.Amount).OrderBy(amount => amount).ToList();
            int count = sortedTransactions.Count;
            if (count % 2 == 1)
            {
                return sortedTransactions[count / 2];
            }
            else
            {
                return (sortedTransactions[(count - 1) / 2] + sortedTransactions[count / 2]) / 2.0M;
            }
        }

        public decimal CalculateMode(IEnumerable<Transaction> listOfTransactions)
        {
            return listOfTransactions
                .GroupBy(t => t.Amount)
                .OrderByDescending(g => g.Count())
                .First()
                .Key;
        }

        public decimal CalculateQuantile(IEnumerable<Transaction> listOfTransactions, double quantile)
        {
            var sortedTransactions = listOfTransactions.Select(t => t.Amount).OrderBy(amount => amount).ToList();
            int N = sortedTransactions.Count;
            double position = (N + 1) * quantile;
            int index = (int)Math.Floor(position) - 1;
            decimal fraction = (decimal)(position - Math.Floor(position));

            if (index + 1 < N)
            {
                return sortedTransactions[index] + fraction * (sortedTransactions[index + 1] - sortedTransactions[index]);
            }
            return sortedTransactions[index];
        }

        public IEnumerable<(string key, decimal Average)> CalculateGroupedAverageAmount(IEnumerable<IGrouping<string, Transaction>> groupedTransactions)
        {
            return groupedTransactions
                .Select(group => (
                    key: group.Key,
                    Average: CalculateAverageAmount(group)
                ))
                .ToList();
        }

        public IEnumerable<(string key, decimal Average)> CalculateGroupedAverageIncome(IEnumerable<IGrouping<string, Transaction>> groupedTransactions)
        {
            return groupedTransactions
                 .Select(group => (
                     key: group.Key,
                     Average: CalculateAverageIncome(group)
                 ))
                 .ToList();
        }

        public IEnumerable<(string key, decimal Average)> CalculateGroupedAverageExpense(IEnumerable<IGrouping<string, Transaction>> groupedTransactions)
        {
            return groupedTransactions
                .Select(group => (
                    key: group.Key,
                    Average: CalculateAverageExpense(group)
                ))
                .ToList();
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

        public IEnumerable<(string key, decimal Percentage)> CalculateGroupedPercentageOfTotalIncome(IEnumerable<IGrouping<string, Transaction>> groupedTransactions, bool showZeroValues = false)
        {
            var totalAmount = groupedTransactions.Sum(group => CalculateTotalIncome(group));

            var results = groupedTransactions
                .Select(group => (
                    key: group.Key,
                    percentage: CalculateTotalIncome(group) / totalAmount * 100
                ))
                .Where(result => showZeroValues || result.percentage != 0);

            return results;
        }

        public IEnumerable<(string key, decimal percentage)> CalculateGroupedPercentageOfTotalExpanses(IEnumerable<IGrouping<string, Transaction>> groupedTransactions, bool showZeroValues = false)
        {
            
            var totalAmount = groupedTransactions.Sum(group => CalculateTotalExpenses(group));

            var results = groupedTransactions
                .Select(group => (
                    key: group.Key,
                    percentage: CalculateTotalExpenses(group) / totalAmount * 100
                ))
                .Where(result => showZeroValues || result.percentage != 0); 

            return results;
        }

        public IEnumerable<(string key, decimal Amount)> SortByAmountAscending(IEnumerable<(string key, decimal Amount)> groups)
        {
            return groups.OrderBy(g => g.Amount);
        }

        public IEnumerable<(string key, decimal Amount)> SortByAmountDescending(IEnumerable<(string key, decimal Amount)> groups)
        {
            return groups.OrderByDescending(g => g.Amount);
        }

        public IEnumerable<(string key, decimal Amount)> SortByKeyAscending(IEnumerable<(string key, decimal Amount)> groups)
        {
            return groups.OrderBy(g => g.key);
        }

        public IEnumerable<(string key, decimal Amount)> SortByKeyDescending(IEnumerable<(string key, decimal Amount)> groups)
        {
            return groups.OrderByDescending(g => g.key);
        }

        public int GetTotalTransactionCount(IEnumerable<Transaction> listOfTransactions)
        {
            return listOfTransactions.Count();
        }

        public IEnumerable<(string key, decimal Number)> GetGroupedTotalTransactionCount(IEnumerable<IGrouping<string, Transaction>> groupedTransactions)
        {
            return groupedTransactions
                .Select(group => (
                    key: group.Key,
                    Number: GetTotalTransactionCount(group)+0m
                ))
                .ToList();
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

        public void DisplayGroupedAmountInComandline(IEnumerable<(string key, decimal Amount)> Groups)
        {          
            foreach (var (Key, Amount) in Groups)
            {
                Console.WriteLine($"{Key,-50} {Strings.TotalAmount,-1} {Amount:C}");
            }
        }

    }
}
