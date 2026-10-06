using MongoDB.Driver;
using PikudMerkazRabbit.Models;

namespace PikudMerkazRabbit.service;

public class MongoService
{
    private readonly IMongoCollection<Alert> _collection;

    public MongoService(string connection)
    {
        var client = new MongoClient(connection);
        var database = client.GetDatabase("kolaman");
        _collection = database.GetCollection<Alert>("center");
    }
    public async Task SaveAsync(Alert alert)
    {
        await _collection.ReplaceOneAsync(item => item.AlertId == alert.AlertId,alert,new ReplaceOptions { IsUpsert = true });
        Console.WriteLine($"Saved to Mongo: {alert.AlertId}");
    }
}