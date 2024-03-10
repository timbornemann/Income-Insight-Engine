using dataStructure;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace src.parser
{
    /// <summary>
    /// Parses CSV files provided by DKB "Deutsche Kredit Bank" to process them in the program
    /// </summary>
    public class CsvDKBParser
    {

        /// <summary>
        /// Parses a CSV file containing transaction data, starting from the 6th line, and returns a list of <see cref="Transaction"/> objects.
        /// </summary>
        /// <param name="filePath">The path to the CSV file.</param>
        /// <returns>A list of <see cref="Transaction"/> objects constructed from the CSV file data.</returns>
        /// <remarks>
        /// The CSV file is expected to have the following format, starting from the 6th line: 
        /// Date in "dd.MM.yy" format, Status, Partner Name (for incoming transactions), Alternate Partner Name (for outgoing transactions), 
        /// Partner IBAN, Description, Transaction Type ("Eingang" for incoming), Amount, and Notes, separated by semicolons.
        /// The method skips the first 5 lines of the file, replaces double quotes with nothing, and treats the semicolon (;) as the column separator.
        /// </remarks>
        /// <example>
        /// var transactions = ParseCsv("path/to/your/file.csv");
        /// foreach(var transaction in transactions)
        /// {
        ///     Console.WriteLine(transaction.Date);
        /// }
        /// </example>
        public List<Transaction> ParseCsv(string filePath)
        {
            var transactions = new List<Transaction>();
            var lines = File.ReadAllLines(filePath).Skip(5); 

            foreach (var line in lines)
            {
                var cleanedLine = line.Replace("\"", "");

                var columns = cleanedLine.Split(';');
               
                bool eingang = false;
                if (columns[6].Equals("Eingang")) {
                    eingang = true;
                }

                var transaction = new Transaction
                {
                    Date = DateTime.ParseExact(columns[0], "dd.MM.yy", CultureInfo.InvariantCulture),
                    Status = columns[2],
                    Partner = (eingang) ? columns[3] : columns[4],
                    PartnerIban = columns[7],
                    Description = columns[5],
                    Amount = decimal.Parse(columns[8].Replace(",", "."), CultureInfo.InvariantCulture),
                    Notes = columns[11],
                };

                transactions.Add(transaction);

            }

            return transactions;
        }
    }
}