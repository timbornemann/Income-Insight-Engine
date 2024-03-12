using IncomeInsightEngine.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dataStructure
{
    /// <summary>
    /// Represents a financial transaction with comprehensive details including basic information,
    /// financial details, categorization, relationships, and additional administrative information.
    /// </summary>
    /// <remarks>
    /// This class includes properties for tracking transaction ID, date, description, amount, currency,
    /// payment method, tax status, reimbursability, categorization (including category, budget category,
    /// and tags), classification, partner details (including IBAN), project, administrative details
    /// (status, priority, frequency, location), receipt information, and notes. It provides functionality
    /// to display detailed or short summaries of transactions, compare transaction objects, and generate
    /// a hash code for a transaction instance.
    /// </remarks>
    public class Transaction
    {
        public static int IDCOUNTER = 0;


        // Basic Information
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }

        // Financial Details
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string PaymentMethod { get; set; }
        public bool TaxDeductible { get; set; }
        public bool Reimbursable { get; set; }

        // Categorization
        public string Category { get; set; }
        public string BudgetCategory { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
        public string Classification { get; set; }

        // Relationship
        public string PartnerIban { get; set; }
        public string Partner { get; set; }
        public string Project { get; set; }

        // Administrative
        public string Status { get; set; }
        public string Priority { get; set; }
        public string Frequency { get; set; }
        public string Location { get; set; }

        // Additional Information
        public string Receipt { get; set; }
        public string Notes { get; set; }

        /// <summary>
        /// Initializes a new instance of the Transaction class, automatically assigning a unique ID.
        /// </summary>
        /// <remarks>
        /// The constructor increments the static ID counter (IDCOUNTER) to ensure each transaction instance receives a unique identifier.
        /// </remarks>
        public Transaction()
        {
            this.Id = IDCOUNTER++;
        }

        /// <summary>
        /// Displays detailed information about the transaction on the console.
        /// </summary>
        /// <remarks>
        /// This method prints an extensive list of transaction properties, including but not limited to ID, date, amount, currency, 
        /// payment method, tax deductibility, reimbursability, categories, tags, partner information, project, status, priority, 
        /// frequency, location, receipt status, notes, and classification.
        /// Each property is printed in a formatted manner, providing a comprehensive overview of the transaction.
        /// </remarks>
        public void DisplayTransactionDetails()
        {
            Console.WriteLine($"{Strings.Id,-10} {Id,-10}" +
                              $"{Strings.Date,-12} {Date.ToShortDateString(),-12}" +
                              $"{Strings.Amount,-10} {Amount.ToString("C"),-10}" +
                              $"{Strings.Currency,-8} {Currency,-8}" +
                              $"{Strings.Description,-8} {Description,-15}" +
                              $"{Strings.PaymentMethod,-15} {PaymentMethod,-15}" +
                              $"{Strings.TaxDeductible,-15} {TaxDeductible,-15}" +
                              $"{Strings.Reimbursable,-15} {Reimbursable,-15}" +
                              $"{Strings.Category,-15} {Category,-15}" +
                              $"{Strings.BudgetCategory,-15} {BudgetCategory,-15}" +
                              $"{Strings.Tags,-10} {string.Join(", ", Tags),-10}" +
                              $"{Strings.Partner,-15} {Partner,-15}" +
                              $"{Strings.PartnerIBAN,-15} {PartnerIban,-15}" +
                              $"{Strings.Project,-15} {Project,-15}" +
                              $"{Strings.Status,-10} {Status,-10}" +
                              $"{Strings.Priority,-10} {Priority,-10}" +
                              $"{Strings.Frequency,-10} {Frequency,-10}" +
                              $"{Strings.Location,-15} {Location,-15}" +
                              $"{Strings.Receipt,-10} {Receipt,-10}" +
                              $"{Strings.Notes,-15} {Notes,-15}" +
                              $"{Strings.Classification,-15} {Classification,-15}");
        }

        /// <summary>
        /// Displays a short summary of the transaction details on the console.
        /// </summary>
        /// <remarks>
        /// This method prints the transaction's ID, date, amount, and partner name in a formatted manner to the console.
        /// The date is shown in short date format, and the amount is formatted as currency.
        /// </remarks>
        public void DisplayShortTransactionDetails()
        {
            Console.WriteLine($"{Strings.Id,-1} {Id,-6}" +
                              $"{Strings.Date,-1} {Date.ToShortDateString(),-12}" +
                              $"{Strings.Amount,-2} {Amount.ToString("C"),-12}" +
                              $"{Strings.Partner,-2} {Partner,-50}" 
                             // +$"{Strings.Description,-5} {Description,-50}"
                              );

        }

        public override bool Equals(object obj)
        {          
            if (obj == null || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }

            var other = (Transaction)obj;
         
            return Date == other.Date &&
                   string.Equals(Description, other.Description) &&
                   Amount == other.Amount &&
                   string.Equals(Currency, other.Currency) &&
                   string.Equals(PaymentMethod, other.PaymentMethod) &&
                   TaxDeductible == other.TaxDeductible &&
                   Reimbursable == other.Reimbursable &&
                   string.Equals(Category, other.Category) &&
                   string.Equals(BudgetCategory, other.BudgetCategory) &&
                   (Tags != null && other.Tags != null && Tags.SequenceEqual(other.Tags)) &&
                   string.Equals(Classification, other.Classification) &&
                   string.Equals(PartnerIban, other.PartnerIban) &&
                   string.Equals(Partner, other.Partner) &&
                   string.Equals(Project, other.Project) &&
                   string.Equals(Status, other.Status) &&
                   string.Equals(Priority, other.Priority) &&
                   string.Equals(Frequency, other.Frequency) &&
                   string.Equals(Location, other.Location) &&
                   string.Equals(Receipt, other.Receipt) &&
                   string.Equals(Notes, other.Notes);
        }

        public override int GetHashCode()
        {
            unchecked 
            {
                int hash = 17;              
                hash = hash * 23 + Id.GetHashCode();
                hash = hash * 23 + Date.GetHashCode();
                hash = hash * 23 + (Description != null ? Description.GetHashCode() : 0);
                hash = hash * 23 + Amount.GetHashCode();
                hash = hash * 23 + (Currency != null ? Currency.GetHashCode() : 0);
                hash = hash * 23 + (PaymentMethod != null ? PaymentMethod.GetHashCode() : 0);
                hash = hash * 23 + TaxDeductible.GetHashCode();
                hash = hash * 23 + Reimbursable.GetHashCode();
                hash = hash * 23 + (Category != null ? Category.GetHashCode() : 0);
                hash = hash * 23 + (BudgetCategory != null ? BudgetCategory.GetHashCode() : 0);
                hash = hash * 23 + (Tags != null ? Tags.Aggregate(0, (current, item) => current + (item != null ? item.GetHashCode() : 0)) : 0);
                hash = hash * 23 + (Classification != null ? Classification.GetHashCode() : 0);
                hash = hash * 23 + (PartnerIban != null ? PartnerIban.GetHashCode() : 0);
                hash = hash * 23 + (Partner != null ? Partner.GetHashCode() : 0);
                hash = hash * 23 + (Project != null ? Project.GetHashCode() : 0);
                hash = hash * 23 + (Status != null ? Status.GetHashCode() : 0);
                hash = hash * 23 + (Priority != null ? Priority.GetHashCode() : 0);
                hash = hash * 23 + (Frequency != null ? Frequency.GetHashCode() : 0);
                hash = hash * 23 + (Location != null ? Location.GetHashCode() : 0);
                hash = hash * 23 + (Receipt != null ? Receipt.GetHashCode() : 0);
                hash = hash * 23 + (Notes != null ? Notes.GetHashCode() : 0);

                return hash;
            }
        }


    }
}
