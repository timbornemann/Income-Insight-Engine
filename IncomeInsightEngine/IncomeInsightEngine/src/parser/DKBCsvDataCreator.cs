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

                data.AppendLine(GenerateTransaction(("Gehalt", "Gehaltseingang", 2000, 6000, "DE1234567890123456789")));
                data.AppendLine(GenerateTransaction(("Miete", "Monatliche Miete", 600, 2000, "DE7890123456789012345")));
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
                ("Gehalt", "Gehaltseingang", 1000, 5000, "DE1234567890123456789", 0),
                ("Tankstelle", "Tanken", 50, 150, "DE9876543210987654321", 30),
                ("Supermarkt", "Lebensmittel", 4, 100, "DE2345678901234567890", 40),
                ("Restaurant", "Essen gehen", 20, 100, "DE3456789012345678901", 20),
                ("Kino", "Kinoabend", 10, 30, "DE4567890123456789012", 5),
                ("Elektronik", "Elektronikartikel", 200, 2000, "DE5678901234567890123", 1),
                ("Kleidung", "Kleidungskauf", 50, 300, "DE6789012345678901234", 15),
                ("Miete", "Monatliche Miete", 600, 2000, "DE7890123456789012345", 0),
                ("Versicherung", "Versicherungsbeitrag", 100, 500, "DE8901234567890123456", 5),
                ("Fitnessstudio", "Mitgliedsbeitrag", 15, 80, "DE9012345678901234567", 5),
                ("Handy", "Handyrechnung", 20, 100, "DE0123456789012345678", 5),
                ("Internet", "Internetrechnung", 20, 100, "DE0987654321098765432", 5),
                ("Haustiere", "Ausgaben für Haustiere", 20, 300, "DE1122334455667788990", 5),
                ("Geschenke", "Geschenke", 10, 500, "DE2233445566778899001", 15),
                ("Reisen", "Reisekosten", 100, 5000, "DE3344556677889900112", 1),
                ("Bildung", "Bildungsausgaben", 50, 2000, "DE4455667788990011223", 10),
                ("Spenden", "Spenden", 5, 1000, "DE5566778899001122334", 1),
                ("Abonnements", "Abonnements und Mitgliedschaften", 5, 150, "DE6677889900112233445", 10),
                ("Strom", "Stromrechnung", 50, 250, "DE0000111122223333", 5),
                ("Wasser", "Wasserrechnung", 25, 150, "DE4444555566667777", 5),
                ("Heizung", "Heizkosten", 50, 200, "DE8888999900001111", 10),
                ("Abfall", "Abfallentsorgungsgebühren", 20, 100, "DE2222333344445555", 5),
                ("Gartenarbeit", "Ausgaben für Gartenarbeit", 30, 300, "DE6666777788889999", 5),
                ("Hausreparaturen", "Reparaturen am Haus", 100, 250, "DE1111222233334444", 10),
                ("Fahrradreparatur", "Reparaturkosten für Fahrräder", 20, 200, "DE5555666677778888", 5),
                ("Öffentlicher Nahverkehr", "Tickets für den öffentlichen Nahverkehr", 5, 15, "DE9999000011112222", 20),
                ("Parkgebühren", "Parkgebühren", 1, 5, "DE3333444455556666", 5),
                ("Haushaltshilfe", "Ausgaben für Haushaltshilfe", 50, 400, "DE7777888899990000", 10),
                ("Software", "Kauf von Software", 5, 99, "DE1000200030004000", 10),
                ("Bücher", "Kauf von Büchern", 10, 40, "DE5000600070008000", 10),
                ("Haustierbedarf", "Kauf von Haustierbedarf", 20, 150, "DE6000700080009000", 10),
                ("Spielzeug", "Kauf von Spielzeug", 10, 50, "DE7000800090001000", 5),
                ("Sportausrüstung", "Kauf von Sportausrüstung", 50, 500, "DE8000900010002000", 5),
                ("Apotheke", "Ausgaben in der Apotheke", 10, 100, "DE9000100010003000", 20),
                ("Freiberufliche Dienstleistungen", "Bezahlung für freiberufliche Arbeit", 100, 1000, "DE0000000000000002", 10),
                ("Online-Kurse", "Gebühren für Online-Bildungsplattformen", 50, 500, "DE0000000000000004", 5),
                ("Software-Abonnement", "Monatliche Abonnementgebühren für Software", 5, 60, "DE0000000000000005", 10),
                ("Streaming-Dienste", "Abonnement für Streaming-Dienste", 10, 30, "DE0000000000000006", 20),
                ("Wohltätigkeitsorganisationen", "Spenden an Wohltätigkeitsorganisationen", 5, 2000, "DE0000000000000007", 1),
                ("Luxusgüter", "Kauf von Luxusgütern", 500, 2000, "DE0000000000000009", 1),
                ("Reparatur und Wartung", "Ausgaben für Reparatur und Wartung von Haushaltsgeräten", 50, 1000, "DE0000000000000010", 5),
                ("Gartengeräte", "Kauf von Gartengeräten", 100, 2000, "DE0000000000000011", 1),
                ("Haustierversicherung", "Monatsbeitrag für die Haustierversicherung", 10, 60, "DE0000000000000012", 5),
                ("Rechtsberatung", "Gebühren für Rechtsberatung", 200, 5000, "DE0000000000000013", 1),
                ("Sprachkurse", "Gebühren für Sprachkurse", 10, 70, "DE0000000000000015", 5),
                ("Tierarztkosten", "Kosten für Tierarztbesuche", 50, 1000, "DE0000000000000017", 4),
                ("Wein und Spirituosen", "Kauf von Wein und Spirituosen", 20, 100, "DE0000000000000020", 15)
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
            string betrag = $"{((transaction.Type == "Gehalt" || transaction.Type == "Freiberufliche Dienstleistungen") ? "+" : "-")}{amount},{random.Next(0, 99):00}";

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
