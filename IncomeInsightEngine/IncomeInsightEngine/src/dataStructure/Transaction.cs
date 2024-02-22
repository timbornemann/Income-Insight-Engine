using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dataStructure
{
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
        public string PartnerIBAN { get; set; }
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

        // Default constructor
        public Transaction()
        {
            this.Id = IDCOUNTER++;
        }

        // Method to display transaction details
        public void DisplayTransactionDetails()
        {
            Console.WriteLine($"ID: {Id}, Date: {Date.ToShortDateString()}, Amount: {Amount}, Category: {Category}, Description: {Description}, Payment Method: {PaymentMethod}, Frequency: {Frequency}, Location: {Location}, Account: {PartnerIBAN}, Tags: {string.Join(", ", Tags)}, Partner: {Partner}, Priority: {Priority}, Status: {Status}, Project: {Project}, Tax Deductible: {TaxDeductible}, Currency: {Currency}, Reimbursable: {Reimbursable}, Receipt: {Receipt}, Budget Category: {BudgetCategory}, Notes: {Notes}, Classification: {Classification}");
        }

        public void DisplayShortTransactionDetails()
        {
            Console.WriteLine($"ID: {Id}, Date: {Date.ToShortDateString()}, Amount: {Amount}, Partner: {Partner}, Priority: {Priority}, Status: {Status}, Currency: {Currency}");
        }


        public override bool Equals(object obj)
        {
            // Check for null and compare run-time types.
            if (obj == null || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }

            var other = (Transaction)obj;

            // Compare each property. Use SequenceEqual to compare the Tags list.
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
                   string.Equals(PartnerIBAN, other.PartnerIBAN) &&
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
                // For each field, compute a hash code and combine it with the current hash code.
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
                hash = hash * 23 + (PartnerIBAN != null ? PartnerIBAN.GetHashCode() : 0);
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
