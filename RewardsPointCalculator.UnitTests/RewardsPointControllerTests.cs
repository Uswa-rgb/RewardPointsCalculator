using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using RewardsPointCalculator.Api.Controllers;
using RewardsPointCalculator.Api.Entities;
using RewardsPointCalculator.Api.Repositories;

namespace RewardsPointCalculator.UnitTests;
public class RewardsPointCalculatorTests
{
    // Create a mock customer repository and logger
    private readonly Mock<ICustomerRepository> repositoryStub = new ();
    private readonly Mock<ILogger<RewardsPointController>> loggerStub = new ();

    [Fact]
    public async Task GetRewardPointsForAllCustomersAsync_ReturnsAllCustomersRewardPoints()
    {
        //Arrange
        // Create an array of customers with random transactions
        var customers = new[]{CreateRandomCustomer(new[]{50,100,150}), CreateRandomCustomer(new[]{20,20,50})};
        // Set up the mock repository's GetCustomersAsync method to return the array of customers
        repositoryStub.Setup(repo => repo.GetCustomersAsync()).ReturnsAsync(customers);
        var controller = new RewardsPointController(repositoryStub.Object,loggerStub.Object);

        //Act
        var result = await controller.GetRewardPointsForAllCustomersAsync();

        //Assert
        var actionResult = Assert.IsType<ActionResult<List<RewardPoints>>>(result);
        // Check that the value of the action result is a sequence of RewardPoints objects
        var rewardPointsList = Assert.IsAssignableFrom<IEnumerable<RewardPoints>>(actionResult.Value);
        var rewardPointsArray = rewardPointsList.ToArray();

        // Check that the first element has the correct customer ID, name, and reward points for the month
        Assert.Equal(2, rewardPointsArray.Length);
        Assert.Equal(customers[0].CustomerId, rewardPointsArray[0].CustomerId);
        Assert.Equal(customers[0].CustomerName, rewardPointsArray[0].CustomerName);
        Assert.Equal(200, rewardPointsArray[0].TotalPoints);
        Assert.Equal(3, rewardPointsArray[0].MonthlyPoints.Count);
        Assert.Equal(0, rewardPointsArray[0].MonthlyPoints["01/2023"]);
        Assert.Equal(50, rewardPointsArray[0].MonthlyPoints["02/2023"]);
        Assert.Equal(150, rewardPointsArray[0].MonthlyPoints["03/2023"]);

        // Check that the second element has the correct customer ID, name, and zero reward points for the month
        Assert.Equal(customers[1].CustomerId, rewardPointsArray[1].CustomerId);
        Assert.Equal(customers[1].CustomerName, rewardPointsArray[1].CustomerName);
        Assert.Equal(0, rewardPointsArray[1].TotalPoints);
        Assert.Equal(3, rewardPointsArray[1].MonthlyPoints.Count);
        Assert.Equal(0, rewardPointsArray[1].MonthlyPoints["01/2023"]);
        Assert.Equal(0, rewardPointsArray[1].MonthlyPoints["02/2023"]);
        Assert.Equal(0, rewardPointsArray[1].MonthlyPoints["03/2023"]);
    }

    [Fact]
    public async Task GetRewardPointsForAllCustomersAsync_CustomersWithNoTransaction_ReturnsZeroRewardPointsforAllCustomers()
    {
        //Arrange
        var customers = new[]{CreateRandomCustomer(new int[]{}), CreateRandomCustomer(new int[]{})};
        repositoryStub.Setup(repo => repo.GetCustomersAsync()).ReturnsAsync(customers);
        var controller = new RewardsPointController(repositoryStub.Object,loggerStub.Object);

        //Act
        var result = await controller.GetRewardPointsForAllCustomersAsync();

        //Assert
        var actionResult = Assert.IsType<ActionResult<List<RewardPoints>>>(result);
        var rewardPointsList = Assert.IsAssignableFrom<IEnumerable<RewardPoints>>(actionResult.Value);
        var rewardPointsArray = rewardPointsList.ToArray();

        // Check that the first element has the correct customer ID, name, and 0 reward points for the month
        Assert.Equal(2, rewardPointsArray.Length);
        Assert.Equal(customers[0].CustomerId, rewardPointsArray[0].CustomerId);
        Assert.Equal(customers[0].CustomerName, rewardPointsArray[0].CustomerName);
        Assert.Equal(0, rewardPointsArray[0].TotalPoints);
        Assert.Equal(0, rewardPointsArray[0].MonthlyPoints.Count);

        // Check that the second element has the correct customer ID, name, and 0 total reward points and monthly
        Assert.Equal(customers[1].CustomerId, rewardPointsArray[1].CustomerId);
        Assert.Equal(customers[1].CustomerName, rewardPointsArray[1].CustomerName);
        Assert.Equal(0, rewardPointsArray[1].TotalPoints);
        Assert.Equal(0, rewardPointsArray[1].MonthlyPoints.Count);
       
    }

    [Fact]
    public async Task GetRewardPointsForAllCustomersAsync_ReturnsNotFound_WhenNoCustomersExist()
    {
        // Arrange
        repositoryStub.Setup(repo => repo.GetCustomersAsync())
            .ReturnsAsync((List<Customer>)null);
        var controller = new RewardsPointController(repositoryStub.Object,loggerStub.Object);

        // Act
        var result = await controller.GetRewardPointsForAllCustomersAsync();

        // Assert
        var actionResult = Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetRewardPointsForCustomerAsync_ReturnsCustomerRewardPoints()
    {
        //Arrange
        var customer = CreateRandomCustomer(new[]{50,100,150});
        repositoryStub.Setup(repo => repo.GetCustomerAsync(customer.CustomerId)).ReturnsAsync(customer);
        var controller = new RewardsPointController(repositoryStub.Object,loggerStub.Object);

        //Act
        var result = await controller.GetRewardPointsForCustomerAsync(customer.CustomerId);

        //Assert
        var actionResult = Assert.IsType<ActionResult<RewardPoints>>(result);
        var rewardPoints = actionResult.Value;

        // Check that result has the correct customer ID, name, and reward points for the month
        Assert.Equal(customer.CustomerId, rewardPoints.CustomerId);
        Assert.Equal(customer.CustomerName, rewardPoints.CustomerName);
        Assert.Equal(200, rewardPoints.TotalPoints);
        Assert.Equal(3, rewardPoints.MonthlyPoints.Count);
        Assert.Equal(0, rewardPoints.MonthlyPoints["01/2023"]);
        Assert.Equal(50, rewardPoints.MonthlyPoints["02/2023"]);
        Assert.Equal(150, rewardPoints.MonthlyPoints["03/2023"]);
        
    }
    [Fact]
    public async Task GetRewardPointsForCustomerAsync_ReturnsNotFound_WhenCustomerNotExist()
    {
        // Arrange
        var customer = CreateRandomCustomer(new[]{50,100,150});
        repositoryStub.Setup(repo => repo.GetCustomerAsync(customer.CustomerId))
            .ReturnsAsync((Customer)null);
        var controller = new RewardsPointController(repositoryStub.Object,loggerStub.Object);

        // Act
        var result = await controller.GetRewardPointsForCustomerAsync(customer.CustomerId);

        // Assert
        var actionResult = Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetRewardPointsForCustomerAsync_CustomerWithNoTransaction_ReturnsZeroRewardPointsforCustomer()
    {
        //Arrange
        var customer = CreateRandomCustomer(new int[]{});
        repositoryStub.Setup(repo => repo.GetCustomerAsync(customer.CustomerId)).ReturnsAsync(customer);
        var controller = new RewardsPointController(repositoryStub.Object,loggerStub.Object);

        //Act
        var result = await controller.GetRewardPointsForCustomerAsync(customer.CustomerId);

        //Assert
        var actionResult = Assert.IsType<ActionResult<RewardPoints>>(result);
        var rewardPoints = actionResult.Value;

        // Check that the result has the correct customer ID, name, and 0 reward points for the month
        Assert.Equal(customer.CustomerId, rewardPoints.CustomerId);
        Assert.Equal(customer.CustomerName, rewardPoints.CustomerName);
        Assert.Equal(0, rewardPoints.TotalPoints);
        Assert.Equal(0, rewardPoints.MonthlyPoints.Count);

    }

    private Customer CreateRandomCustomer(int[] amounts)
    {
        var allTansList = new List<CustomerTransaction>();
        // Iterate over the array of transaction amounts and create a new customerTransaction object for each amount
        // and different month
        foreach (var amount in amounts.Select((value, i) => new { i, value }))
        {
            allTansList.Add(new CustomerTransaction {
                Amount=amount.value,
                Date= new DateTime(2023, amount.i+1, 1)
            });
        }

        return new(){
            CustomerId = Guid.NewGuid(),
            CustomerName = Guid.NewGuid().ToString(),
            AllTransactions = allTansList
            
        };
    }
}