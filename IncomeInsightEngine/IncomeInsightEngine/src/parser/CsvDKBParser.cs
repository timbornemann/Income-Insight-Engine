using dataStructure;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace src.parser
{
    public class CsvDKBParser
    {
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
                    PartnerIBAN = columns[7],
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