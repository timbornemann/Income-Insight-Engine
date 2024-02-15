using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dataStructure
{
    public class Transaction
    {
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
        public string Account { get; set; }
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
        }

        // Method to display transaction details
        public void DisplayTransactionDetails()
        {
            Console.WriteLine($"ID: {Id}, Date: {Date.ToShortDateString()}, Amount: {Amount}, Category: {Category}, Description: {Description}, Payment Method: {PaymentMethod}, Frequency: {Frequency}, Location: {Location}, Account: {Account}, Tags: {string.Join(", ", Tags)}, Partner: {Partner}, Priority: {Priority}, Status: {Status}, Project: {Project}, Tax Deductible: {TaxDeductible}, Currency: {Currency}, Reimbursable: {Reimbursable}, Receipt: {Receipt}, Budget Category: {BudgetCategory}, Notes: {Notes}, Classification: {Classification}");
        }
    }
}
