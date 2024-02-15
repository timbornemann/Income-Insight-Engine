using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src
{
    public class IncomeInsightEngineMain
    {
        
       

       public static void Main(string[] args){
    
       
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
        }

    }
}