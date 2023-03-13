using RewardsPointCalculator.Api.Entities;

namespace RewardsPointCalculator.Api.Repositories
{
    // This interface represents a repository that can be used to manage customer data.
    public interface ICustomerRepository
    {

        // This method retrieves a customer with a specific ID.
        // Parameters:
        // - id: The unique identifier of the customer to retrieve.
        // Returns:
        // - A Task that represents the asynchronous operation. The result of the Task is the retrieved customer.        
        public Task<Customer> GetCustomerAsync(Guid id);

        // This method retrieves all customers.
        // Returns:
        // - A Task that represents the asynchronous operation. The result of the Task is a collection of all customers.
        public Task<IEnumerable<Customer>> GetCustomersAsync();

        // This method adds a new customer to the repository.
        // Parameters:
        // - customer: The customer to add to the repository.
        // Returns:
        // - A Task that represents the asynchronous operation.
        public Task AddCustomerAsync(Customer customer);
    }
}