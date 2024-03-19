using dataStructure;
using IncomeInsightEngine.Properties;
using System;
using System.Collections.Generic;
using System.Linq;


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

        /// <summary>
        /// Calculates the average amount of a collection of transactions.
        /// </summary>
        /// <param name="listOfTransactions">The collection of transactions to calculate the average amount for.</param>
        /// <returns>The average amount as a decimal. Returns 0m if the collection is null or empty.</returns>
        public decimal CalculateAverageAmount(IEnumerable<Transaction> listOfTransactions)
        {
            if (listOfTransactions == null || !listOfTransactions.Any())
            {
                return 0m; 
            }

            return listOfTransactions.Average(t => t.Amount);
        }

        /// <summary>
        /// Calculates the average income from a collection of transactions.
        /// </summary>
        /// <param name="listOfTransactions">The collection of transactions to calculate the average income for. Only transactions with a positive amount are considered.</param>
        /// <returns>The average income as a decimal. Returns 0m if the collection is null, empty, or contains no transactions with a positive amount.</returns>
        public decimal CalculateAverageIncome(IEnumerable<Transaction> listOfTransactions)
        {
            if (listOfTransactions == null || !listOfTransactions.Any(t => t.Amount > 0))
            {
                return 0m; 
            }

            return listOfTransactions.Where(t => t.Amount > 0).Average(t => t.Amount);
        }

        /// <summary>
        /// Calculates the average expense from a collection of transactions.
        /// </summary>
        /// <param name="listOfTransactions">The collection of transactions to calculate the average expense for. Only transactions with a negative amount are considered.</param>
        /// <returns>The average expense as a decimal. Returns 0m if the collection is null, empty, or contains no transactions with a negative amount.</returns>
        public decimal CalculateAverageExpense(IEnumerable<Transaction> listOfTransactions)
        {
            if (listOfTransactions == null || !listOfTransactions.Any(t => t.Amount < 0))
            {
                return 0m; 
            }

            return listOfTransactions.Where(t => t.Amount < 0).Average(t => t.Amount);
        }

        /// <summary>
        /// Calculates the variance of the amounts in a collection of transactions.
        /// </summary>
        /// <param name="listOfTransactions">The collection of transactions to calculate the variance for.</param>
        /// <returns>The variance as a decimal. The collection must not be null or empty.</returns>
        public decimal CalculateVariance(IEnumerable<Transaction> listOfTransactions)
        {
            var average = CalculateAverageAmount(listOfTransactions);
            var variance = listOfTransactions.Sum(t => Math.Pow((double)(t.Amount - average), 2)) / listOfTransactions.Count();
            return (decimal)variance;
        }

        /// <summary>
        /// Calculates the standard deviation of the amounts in a collection of transactions.
        /// </summary>
        /// <param name="listOfTransactions">The collection of transactions to calculate the standard deviation for.</param>
        /// <returns>The standard deviation as a decimal. The collection must not be null or empty.</returns>
        public decimal CalculateStandardDeviation(IEnumerable<Transaction> listOfTransactions)
        {
            var variance = CalculateVariance(listOfTransactions);
            return (decimal)Math.Sqrt((double)variance);
        }

        /// <summary>
        /// Calculates the median amount of a collection of transactions.
        /// </summary>
        /// <param name="listOfTransactions">The collection of transactions to calculate the median amount for.</param>
        /// <returns>The median amount as a decimal. If the collection's count is even, it returns the average of the two middle numbers.</returns>
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

        /// <summary>
        /// Calculates the mode of the amounts in a collection of transactions.
        /// </summary>
        /// <param name="listOfTransactions">The collection of transactions to calculate the mode for.</param>
        /// <returns>The mode as a decimal. In case of multiple modes, returns the one that appears first.</returns>
        public decimal CalculateMode(IEnumerable<Transaction> listOfTransactions)
        {
            return listOfTransactions
                .GroupBy(t => t.Amount)
                .OrderByDescending(g => g.Count())
                .First()
                .Key;
        }

        /// <summary>
        /// Calculates a specific quantile of the amounts in a collection of transactions.
        /// </summary>
        /// <param name="listOfTransactions">The collection of transactions to calculate the quantile for.</param>
        /// <param name="quantile">The quantile to calculate, where 0 < quantile < 1.</param>
        /// <returns>The calculated quantile as a decimal. Handles both discrete and continuous cases for quantile calculation.</returns>
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

        /// <summary>
        /// Calculates the average amount for each group of transactions.
        /// </summary>
        /// <param name="groupedTransactions">The collection of transaction groups, each identified by a string key.</param>
        /// <returns>A collection of tuples, each containing a group key and the average amount of transactions in that group.</returns>
        public IEnumerable<(string key, decimal Average)> CalculateGroupedAverageAmount(IEnumerable<IGrouping<string, Transaction>> groupedTransactions)
        {
            return groupedTransactions
                .Select(group => (
                    key: group.Key,
                    Average: CalculateAverageAmount(group)
                ))
                .ToList();
        }

        /// <summary>
        /// Calculates the average income for each group of transactions.
        /// </summary>
        /// <param name="groupedTransactions">The collection of transaction groups, each identified by a string key, to calculate the average income for.</param>
        /// <returns>A collection of tuples, each containing a group key and the average income of transactions in that group.</returns>
        public IEnumerable<(string key, decimal Average)> CalculateGroupedAverageIncome(IEnumerable<IGrouping<string, Transaction>> groupedTransactions)
        {
            return groupedTransactions
                 .Select(group => (
                     key: group.Key,
                     Average: CalculateAverageIncome(group)
                 ))
                 .ToList();
        }

        /// <summary>
        /// Calculates the average expense for each group of transactions.
        /// </summary>
        /// <param name="groupedTransactions">The collection of transaction groups, each identified by a string key, to calculate the average expense for.</param>
        /// <returns>A collection of tuples, each containing a group key and the average expense of transactions in that group.</returns>
        public IEnumerable<(string key, decimal Average)> CalculateGroupedAverageExpense(IEnumerable<IGrouping<string, Transaction>> groupedTransactions)
        {
            return groupedTransactions
                .Select(group => (
                    key: group.Key,
                    Average: CalculateAverageExpense(group)
                ))
                .ToList();
        }

        /// <summary>
        /// Calculates the total expenses for each group of transactions.
        /// </summary>
        /// <param name="groupedTransactions">The collection of transaction groups, each identified by a string key, to calculate the total expenses for.</param>
        /// <returns>A collection of tuples, each containing a group key and the total expenses of transactions in that group.</returns>
        public IEnumerable<(string key, decimal TotalExpenses)> CalculateGroupedExpenses(IEnumerable<IGrouping<string, Transaction>> groupedTransactions)
        {
            return groupedTransactions
                .Select(group => (
                    key: group.Key,
                    TotalExpenses: CalculateTotalExpenses(group)
                ))
                .ToList();
        }

        /// <summary>
        /// Calculates the total income for each group of transactions.
        /// </summary>
        /// <param name="groupedTransactions">The collection of transaction groups, each identified by a string key, to calculate the total income for.</param>
        /// <returns>A collection of tuples, each containing a group key and the total income of transactions in that group.</returns>
        public IEnumerable<(string key, decimal TotalIncome)> CalculateGroupedIncome(IEnumerable<IGrouping<string, Transaction>> groupedTransactions)
        {
            return groupedTransactions
                .Select(group => (
                    key: group.Key,
                    TotalIncome: CalculateTotalIncome(group)
                ))
                .ToList();
        }

        /// <summary>
        /// Calculates the total amount for each group of transactions.
        /// </summary>
        /// <param name="groupedTransactions">The collection of transaction groups, each identified by a string key, to calculate the total amount for.</param>
        /// <returns>A collection of tuples, each containing a group key and the total amount of transactions in that group.</returns>
        public IEnumerable<(string key, decimal TotalAmount)> CalculateGroupedAmount(IEnumerable <IGrouping<string, Transaction>> groupedTransactions)
        {
            return groupedTransactions
                .Select(group => (
                    key: group.Key,
                    TotalAmount: CalculateTotalAmount(group)
                ))
                .ToList();
        }

        /// <summary>
        /// Calculates the percentage of total income for each group of transactions relative to the total income of all groups.
        /// </summary>
        /// <param name="groupedTransactions">The collection of transaction groups to calculate the percentage for.</param>
        /// <param name="showZeroValues">Whether to include groups with a percentage of zero in the result.</param>
        /// <returns>A collection of tuples, each containing a group key and the percentage of total income that group represents.</returns>
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

        /// <summary>
        /// Calculates the percentage of total expenses for each group of transactions relative to the total expenses of all groups.
        /// </summary>
        /// <param name="groupedTransactions">The collection of transaction groups to calculate the percentage for.</param>
        /// <param name="showZeroValues">Whether to include groups with a percentage of zero in the result.</param>
        /// <returns>A collection of tuples, each containing a group key and the percentage of total expenses that group represents.</returns>
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

        /// <summary>
        /// Gets the total count of transactions in a collection.
        /// </summary>
        /// <param name="listOfTransactions">The collection of transactions to count.</param>
        /// <returns>The total number of transactions in the collection.</returns>
        public int GetTotalTransactionCount(IEnumerable<Transaction> listOfTransactions)
        {
            return listOfTransactions.Count();
        }

        /// <summary>
        /// Calculates the total transaction count for each group of transactions.
        /// </summary>
        /// <param name="groupedTransactions">The collection of transaction groups, each identified by a string key.</param>
        /// <returns>A collection of tuples, each containing a group key and the total number of transactions in that group.</returns>
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
        /// Sorts a collection of groups by their amount in ascending order.
        /// </summary>
        /// <param name="groups">The collection of groups, each represented by a tuple containing a key and an amount.</param>
        /// <returns>A sorted collection of groups by amount in ascending order.</returns>
        public IEnumerable<(string key, decimal Amount)> SortByAmountAscending(IEnumerable<(string key, decimal Amount)> groups)
        {
            return groups.OrderBy(g => g.Amount);
        }

        /// <summary>
        /// Sorts a collection of groups by their amount in descending order.
        /// </summary>
        /// <param name="groups">The collection of groups, each represented by a tuple containing a key and an amount.</param>
        /// <returns>A sorted collection of groups by amount in descending order.</returns>
        public IEnumerable<(string key, decimal Amount)> SortByAmountDescending(IEnumerable<(string key, decimal Amount)> groups)
        {
            return groups.OrderByDescending(g => g.Amount);
        }

        /// <summary>
        /// Sorts a collection of groups by their key in ascending order.
        /// </summary>
        /// <param name="groups">The collection of groups, each represented by a tuple containing a key and an amount.</param>
        /// <returns>A sorted collection of groups by key in ascending order.</returns>
        public IEnumerable<(string key, decimal Amount)> SortByKeyAscending(IEnumerable<(string key, decimal Amount)> groups)
        {
            return groups.OrderBy(g => g.key);
        }

        /// <summary>
        /// Sorts a collection of groups by their key in descending order.
        /// </summary>
        /// <param name="groups">The collection of groups, each represented by a tuple containing a key and an amount.</param>
        /// <returns>A sorted collection of groups by key in descending order.</returns>
        public IEnumerable<(string key, decimal Amount)> SortByKeyDescending(IEnumerable<(string key, decimal Amount)> groups)
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

        /// <summary>
        /// Displays the amount associated with each group in the console.
        /// </summary>
        /// <param name="Groups">The collection of groups, each represented by a tuple containing a key and an amount.</param>
        /// <remarks>
        /// For each group, this method prints the key and its corresponding rounded amount to the console, 
        /// formatting the output with the group key aligned to the left and the amount to the right.
        /// </remarks>
        public void DisplayGroupedAmountInComandline(IEnumerable<(string key, decimal Amount)> Groups)
        {          
            foreach (var (Key, Amount) in Groups)
            {
                Console.WriteLine($"{Key,-50} {Strings.Amount,-1} {System.Math.Round(Amount,2)}");
            }
        }

    }
}
