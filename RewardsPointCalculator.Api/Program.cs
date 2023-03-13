using System.Net.Mime;
using System.Text.Json;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using RewardsPointCalculator.Api.Repositories;
using RewardsPointCalculator.Api.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConfigurationManager configuration = builder.Configuration;

// Registering a GUID serializer
BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));

// Getting the MongoDB configuration settings
var MongoDbsettings = configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();

// Adding a singleton instance of IMongoClient to the container
builder.Services.AddSingleton<IMongoClient>(serviceProvider => {
    return new MongoClient(MongoDbsettings.ConnectionString);
});

// Adding health checks to the container
builder.Services.AddHealthChecks().AddMongoDb(MongoDbsettings.ConnectionString,
                                             name:"mongodb",
                                             timeout: TimeSpan.FromSeconds(3),
                                             tags: new[]{"ready"});

// Adding a singleton instance of ICustomerRepository to the container
builder.Services.AddSingleton<ICustomerRepository, MongoDBCustomerCollection>();

builder.Services.AddControllers(options => {
    options.SuppressAsyncSuffixInActionNames = false;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();

//To make sure database is ready to serve requests
app.MapHealthChecks("/healthz/ready", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions{
    Predicate = check => check.Tags.Contains("ready"),
    ResponseWriter = async(context,report) =>
    {
        var result = JsonSerializer.Serialize(
            new 
            {
                status = report.Status.ToString(),
                checks = report.Entries.Select(entry => new
                {
                    name = entry.Key,
                    status = entry.Value.Status.ToString(),
                    exception = entry.Value.Exception != null ? entry.Value.Exception.Message : "none",
                    duration = entry.Value.Duration.ToString()
                })     
            }
        );
        context.Response.ContentType = MediaTypeNames.Application.Json;
        await context.Response.WriteAsync(result);
    }
});

//To make sure service is up and running
app.MapHealthChecks("/healthz/live", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions{
    Predicate = (_) => false
});

app.MapControllers();

app.Run();
