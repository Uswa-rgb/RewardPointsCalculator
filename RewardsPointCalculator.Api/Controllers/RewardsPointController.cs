using Microsoft.AspNetCore.Mvc;
using RewardsPointCalculator.Api.Entities;
using RewardsPointCalculator.Api.Repositories;

namespace RewardsPointCalculator.Api.Controllers
{
    [ApiController]
    [Route("api/rewards")]
    public class RewardsPointController: ControllerBase
    {
        private readonly ICustomerRepository repository;
        private readonly ILogger<RewardsPointController> logger;

        public RewardsPointController(ICustomerRepository repository, ILogger<RewardsPointController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        // Define an endpoint for getting reward points for all customers
        [HttpGet]
        public async Task<ActionResult<List<RewardPoints>>> GetRewardPointsForAllCustomersAsync()
        {
            try
            {
                var customers = await repository.GetCustomersAsync();
                if (customers == null)
                {
                    return NotFound();
                }
                List<RewardPoints> AllCustomersRewardPoints = new List<RewardPoints>();

                // Loop through each customer and calculate their reward points and monthly points
                foreach (var customer in customers)
                {
                    var rewardPoints = CalculateRewardPoints(customer.AllTransactions);
                    var rewardPointsPerMonth = CalculateRewardPointsPerMonth(customer.AllTransactions);

                    // Add the customer's reward points and monthly points to the list
                    AllCustomersRewardPoints.Add(new RewardPoints
                    {
                        CustomerId = customer.CustomerId,
                        CustomerName = customer.CustomerName,
                        TotalPoints = rewardPoints,
                        MonthlyPoints = rewardPointsPerMonth
                    });
                }
 
                return AllCustomersRewardPoints;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrieving reward points for all customers");
                return StatusCode(500, "An error occurred while retrieving reward points for all customers");
            }
        }

        // Define an endpoint for getting reward points for a specific customer
        [HttpGet("{id}")]
        public async Task<ActionResult<RewardPoints>> GetRewardPointsForCustomerAsync(Guid id)
        {
            try
            {
                var customer = await repository.GetCustomerAsync(id);
                if (customer == null)
                {
                    return NotFound();
                }
                // Calculate the customer's reward points and monthly points
                var rewardPoints = CalculateRewardPoints(customer.AllTransactions);
                var rewardPointsPerMonth = CalculateRewardPointsPerMonth(customer.AllTransactions);

                // Return the customer's reward points and monthly points
                return new RewardPoints
                {
                    CustomerId = customer.CustomerId,
                    CustomerName = customer.CustomerName,
                    TotalPoints = rewardPoints,
                    MonthlyPoints = rewardPointsPerMonth
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"An error occurred while retrieving reward points for customer with ID {id}");
                return StatusCode(500, $"An error occurred while retrieving reward points for customer with ID {id}");
            }
        }

        //Calculate the reward points earned by a customer based on their transactions.
        private int CalculateRewardPoints(List<CustomerTransaction> transactions)
        {
            try
            {
                int rewardPoints = 0;
                //Loop through each transaction and calculate total points of a customer
                foreach (var transaction in transactions)
                {
                    // Calculate the amount above $50 and above $100, respectively.
                    var amountOver50 = Math.Max(0, transaction.Amount - 50);
                    var amountOver100 = Math.Max(0, transaction.Amount - 100);
                    // Calculate the reward points for this transaction based on the two calculated values
                    rewardPoints += (int)(amountOver50 * 1 + amountOver100 * 1);
                }
                return rewardPoints;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while calculating reward points for customer transactions");
                throw;
            }
            
        }

        // Calculate the reward points earned by a customer per month based on their transactions.
        private Dictionary<string, int> CalculateRewardPointsPerMonth(List<CustomerTransaction> transactions)
        {
            // Initialize a dictionary to store the reward points earned per month.
            var rewardPointsPerMonth = new Dictionary<string, int>();
            // Group the transactions by month.
            var transactionsByMonth = transactions.GroupBy(t => t.Date.Month);

            try
            {
                // Loop through each group of transactions for each month and calculate points for each month.
                foreach (var monthTransactions in transactionsByMonth)
                {
                    var totalPoints = 0;
                    var monthYearKey = "";

                    // Loop through each transaction in this month.
                    foreach (var perTransaction in monthTransactions)
                    {
                        var amountOver50 = Math.Max(0, perTransaction.Amount - 50);
                        var amountOver100 = Math.Max(0, perTransaction.Amount - 100);
                        totalPoints += (int)(amountOver50 * 1 + amountOver100 * 1);
                        // Set the month-year key to the current transaction's month and year.
                        monthYearKey = perTransaction.Date.ToString("MM/yyyy");
                    }
                    // add a new entry to the dictionary with the month-year key and total reward points.
                    rewardPointsPerMonth.Add(monthYearKey, totalPoints);
                    
                }
                return rewardPointsPerMonth;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while calculating monthly reward points for customer transactions");
                throw;
            }            
        }
    }
}