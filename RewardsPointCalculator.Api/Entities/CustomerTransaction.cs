using System.ComponentModel.DataAnnotations;

namespace RewardsPointCalculator.Api.Entities
{
    // This class represents a transaction made by a customer.
    public class CustomerTransaction
    {
        // This property represents the amount of money spent in this transaction.
        public decimal Amount { get; set; }
        // This property represents the date and time of the transaction.
        public DateTime Date { get; set; }
    }
}