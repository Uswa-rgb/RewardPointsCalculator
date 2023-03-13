namespace RewardsPointCalculator.Api.Settings
{
    //Store the configuration settings required to connect to a MongoDB database.
    public class MongoDbSettings
    {
        public string? Host { get; set;} // Property for MongoDB host address
        public int Port {get; set;} // Property for MongoDB port number
        public string? User { get; set; } // Property for MongoDB username
        public string? Password { get; set; } // Property for MongoDB password
        
        // Connection string for MongoDB database
        public string ConnectionString 
        {   get
            {
                return $"mongodb://{User}:{Password}@{Host}:{Port}"; 
            } 
        }
    }
}