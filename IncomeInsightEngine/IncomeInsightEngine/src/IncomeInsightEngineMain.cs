using System;
using System.Collections.Generic;
using dataStructure;
using IncomeInsightEngine.src.dataStructure.management;

namespace src
{
    public class IncomeInsightEngineMain
    {
        public static void Main(string[] args)
        {
            Transaction transaction = new Transaction
            {
                Id = 1,
                Date = DateTime.Now,
                Amount = -50.75m,
                Category = "Lebensmittel",
                Description = "Wochenendeinkauf",
                PaymentMethod = "Debitkarte",
                Frequency = "einmalig",
                Location = "Berlin, Deutschland",
                Account = "Girokonto",
                Tags = new List<string> { "Wochenende", "Lebensmittel" },
                Partner = "Supermarkt XY",
                Priority = "notwendig",
                Status = "abgeschlossen",
                Project = "Familienbudget",
                TaxDeductible = false,
                Currency = "EUR",
                Reimbursable = false,
                Receipt = "path/to/receipt1.jpg",
                BudgetCategory = "Alltag",
                Notes = "Sonderangebote genutzt",
                Classification = "manuell"
            };

            // Anzeige der Transaktionsdetails
            transaction.DisplayTransactionDetails();

            // Erstelle eine Instanz der JsonTransaction Klasse
            JsonTransaction jsonTransaction = new JsonTransaction();

            // Erstelle die JSON-Datei, wenn sie nicht existiert
            jsonTransaction.CreateFile();

            // Füge die neue Transaktion zur JSON-Datei hinzu
            bool addResult = jsonTransaction.AddTransaction(transaction);
            Console.WriteLine($"Transaction added: {addResult}");

            // Lese alle Transaktionen aus der JSON-Datei
            List<Transaction> transactions = jsonTransaction.ReadData();
            foreach (var trans in transactions)
            {
                trans.DisplayTransactionDetails();
            }
        }
    }
}
