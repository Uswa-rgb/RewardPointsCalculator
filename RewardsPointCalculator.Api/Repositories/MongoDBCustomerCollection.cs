using MongoDB.Bson;
using MongoDB.Driver;
using RewardsPointCalculator.Api.Entities;

namespace RewardsPointCalculator.Api.Repositories
{
    public class MongoDBCustomerCollection : ICustomerRepository
    {
        private const string databaseName = "RewardsPointCalculator";
        private const string collectionName = "customer";
        private readonly IMongoCollection<Customer> customerCollection;
        private readonly ILogger<MongoDBCustomerCollection> logger;

        public MongoDBCustomerCollection(IMongoClient mongoClient,ILogger<MongoDBCustomerCollection> logger)
        {
            // Initialize MongoDB client
            IMongoDatabase database = mongoClient.GetDatabase(databaseName);
            customerCollection = database.GetCollection<Customer>(collectionName);
            this.logger = logger;
        }

        // Add a new customer to the database
        async Task ICustomerRepository.AddCustomerAsync(Customer customer)
        {
            try
            {
                await customerCollection.InsertOneAsync(customer);    
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while adding a new customer");
                throw;
            }
        }

        // Get a customer by their id from the database
        async Task<Customer> ICustomerRepository.GetCustomerAsync(Guid id)
        {
            try
            {
                var filter = Builders<Customer>.Filter.Eq(c => c.CustomerId, id);
                var customer =  await customerCollection.Find(filter).FirstOrDefaultAsync();
                return customer;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"An error occurred while getting the customer with id {id}");
                throw;
            }
        }

        // Get all customers from the database
        async Task<IEnumerable<Customer>> ICustomerRepository.GetCustomersAsync()
        {
            try
            {
                var customers = await customerCollection.Find(new BsonDocument()).ToListAsync();
                return customers;
            }
            catch (System.Exception ex)
            {
                logger.LogError(ex, $"An error occurred while getting the customers from the database.");
                throw;
            }
        }        
    }
}