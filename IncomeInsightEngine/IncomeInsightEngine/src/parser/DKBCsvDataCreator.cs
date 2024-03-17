using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IncomeInsightEngine.src.parser
{
    internal class DKBCsvDataCreator
    {
        private Random random = new Random();
        private DateTime startDate = DateTime.Now;
        public string CreateData(int months)
        {
            StringBuilder data = new StringBuilder();

            data.AppendLine("\"Girokonto\";\"DE1234567890123456789\"");
            data.AppendLine("\"\"");
            data.AppendLine($"\"Kontostand vom 00.00.0000:\";\"{random.Next(1, 20)}.{random.Next(100, 999)},{random.Next(10, 99)} €\"");
            data.AppendLine("\"\"");
            data.AppendLine("\"Buchungsdatum\";\"Wertstellung\";\"Status\";\"Zahlungspflichtige*r\";\"Zahlungsempfänger*in\";\"Verwendungszweck\";\"Umsatztyp\";\"IBAN\";\"Betrag (€)\";\"Gläubiger-ID\";\"Mandatsreferenz\";\"Kundenreferenz\"");


            for (int q = 0; q < months; q++)
            {
            int transactionAmount = random.Next(15,25);

                data.AppendLine(GenerateTransaction(("Gehalt", "Gehaltseingang", 2000, 5000, "DE1234567890123456789")));

                for (int i = 0; i < transactionAmount; i++)
                {

                    var transaction = ChooseTransactionType();
                    data.AppendLine(GenerateTransaction(transaction));
                }
                startDate = startDate.AddMonths(1);
            }

            Console.WriteLine(data.ToString());
            return SaveDataToCsv(data.ToString(), $"SampleData{months}Months.csv");             
        }

        private (string Type, string Description, int MinAmount, int MaxAmount, string IBAN) ChooseTransactionType()
        {
            var transactionTypes = new List<(string Type, string Description, int MinAmount, int MaxAmount, string IBAN, int Weight)>
        {
            ("Gehalt", "Gehaltseingang", 2000, 5000, "DE1234567890123456789", 0),
            ("Tankstelle", "Tanken", 50, 150, "DE9876543210987654321", 30),
            ("Supermarkt", "Lebensmittel", 30, 200, "DE2345678901234567890", 40),
            ("Restaurant", "Essen gehen", 10, 100, "DE3456789012345678901", 20),
            ("Kino", "Kinoabend", 15, 50, "DE4567890123456789012", 5),
            ("Elektronik", "Elektronikartikel", 200, 2000, "DE5678901234567890123", 10),
            ("Kleidung", "Kleidungskauf", 50, 300, "DE6789012345678901234", 15)
        };

            int totalWeight = 0;
            foreach (var type in transactionTypes)
            {
                totalWeight += type.Weight;
            }

            int choice = random.Next(totalWeight);
            foreach (var type in transactionTypes)
            {
                if ((choice -= type.Weight) < 0)
                {
                    return (type.Type, type.Description, type.MinAmount, type.MaxAmount, type.IBAN);
                }
            }

            return default;
        }

        private string GenerateTransaction((string Type, string Description, int MinAmount, int MaxAmount, string IBAN) transaction)
        {
            int amount = random.Next(transaction.MinAmount, transaction.MaxAmount + 1);
            string date = startDate.ToString("dd.MM.yy");
            string betrag = $"{(transaction.Type == "Gehalt" ? "+" : "-")}{amount},{random.Next(0, 99):00}";

            startDate = startDate.AddDays(random.Next(0, 2));
            return $"\"{date}\";\"{date}\";\"Abgeschlossen\";\"Max Mustermann\";\"{transaction.Description}\";\"{transaction.Type}\";\"\";\"{transaction.IBAN}\";\"{betrag}\";\"\";\"\";\"\"";
        }

        public string SaveDataToCsv(string csvData, string fileName)
        {
            string downloadsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string csvFilePath = Path.Combine(downloadsPath, fileName);
            File.WriteAllText(csvFilePath, csvData);            
            return csvFilePath;
        }
    }
}
