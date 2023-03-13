using MongoDB.Bson.Serialization.Attributes;

namespace RewardsPointCalculator.Api.Entities
{
    // Attribute tells MongoDB to ignore any extra elements that may exist in the document.
    [BsonIgnoreExtraElements]
    public class Customer
    {   
        // This property represents the unique identifier for this customer.
        public Guid CustomerId { get; set; }
        // This property represents the name of the customer.
        public string? CustomerName { get; set; }
        // This property represents a list of all the transactions associated with this customer.
        public List<CustomerTransaction>? AllTransactions { get; set; }
    }
}