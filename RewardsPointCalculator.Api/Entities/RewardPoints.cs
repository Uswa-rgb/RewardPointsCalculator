
namespace RewardsPointCalculator.Api.Entities
{
    // This class represents the reward points earned by a customer.
    public class RewardPoints
    {
        // This property represents the unique identifier of the customer for whom the points are earned.
        public Guid CustomerId { get; set; }

        // This property represents the name of the customer for whom the points are earned.
        public string? CustomerName { get; set; }

        // This property represents the total number of points earned by the customer.
        public int TotalPoints {get; set;}

        // This property represents a dictionary of monthly points earned by the customer, with keys representing
        // the month and year (in the format "MM/yyyy") and values representing the number of points earned in that month.
        public Dictionary<string,int> MonthlyPoints {get; set;}

    }
}