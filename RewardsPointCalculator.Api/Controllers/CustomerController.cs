using Microsoft.AspNetCore.Mvc;
using RewardsPointCalculator.Api.Entities;
using RewardsPointCalculator.Api.Repositories;

namespace RewardsPointCalculator.Api.Controllers
{
    [ApiController]
    [Route("customers")]
    public class CustomerController: ControllerBase
    {
        private readonly ICustomerRepository repository;
        public CustomerController(ICustomerRepository repository)
        {
            this.repository = repository;
        }

        // Define an Endpoint to get all customers and their transactions
        [HttpGet]
        public async Task<ActionResult<List<Customer>>> GetCustomersAsync()
        {
            try
            {
                var customers = (List<Customer>) await repository.GetCustomersAsync();
                return Ok(customers);     
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving customers: {ex.Message}");
            }
        }

        // Define an Endpoint to get a specific customer and their transactions by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomerAsync(Guid id)
        {
            try
            {
                Customer customer = await repository.GetCustomerAsync(id);
                if (customer is null)
                {
                    return NotFound();
                }
                return Ok(customer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving customer with ID {id}: {ex.Message}");
            }
            
        }        

        // Define an Endpoint to add a new customer with their transactions
        [HttpPost]
        public async Task<ActionResult<Customer>> AddCustomerAsync(Customer customer)
        {
            try
            {
                Customer newcustomer = new()
                {
                    CustomerId = Guid.NewGuid(),
                    CustomerName = customer.CustomerName,
                    AllTransactions = customer.AllTransactions
                };
                await repository.AddCustomerAsync(newcustomer);
                // Return a 201 status code and the new customer object
                return CreatedAtAction(nameof(GetCustomerAsync), new { id = newcustomer.CustomerId}, newcustomer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while adding customer: {ex.Message}");
            } 
        }
    }
}