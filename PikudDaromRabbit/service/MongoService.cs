using MongoDB.Driver;
using PikudDaromRabbit.Models;

namespace PikudDaromRabbit.service;

public class MongoService
{
    private readonly IMongoCollection<Alert> _collection;

    public MongoService(string connection)
    {
        var client = new MongoClient(connection);
        var database = client.GetDatabase("kolaman");
        _collection = database.GetCollection<Alert>("south");
    }
    public async Task SaveAsync(Alert alert)
    {
        await _collection.ReplaceOneAsync(item => item.AlertId == alert.AlertId,alert,new ReplaceOptions { IsUpsert = true });
        Console.WriteLine($"Saved to Mongo: {alert.AlertId}");
    }
}