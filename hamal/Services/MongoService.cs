using MongoDB.Driver;
using hamal.Models;

namespace hamal.Services;

public class MongoService
{
    private readonly IMongoCollection<Alert> _collection;

    public MongoService(string connection)
    {
        var client = new MongoClient(connection);
        var database = client.GetDatabase("kolaman");
        _collection = database.GetCollection<Alert>("alertsDb");
    }

    public async Task<List<Alert>> GetNewAsync()
    {
        var filter = Builders<Alert>.Filter.Or(
            Builders<Alert>.Filter.Eq(x => x.Status, "WAITING"),
            Builders<Alert>.Filter.Exists(x => x.Status, false)
        );

        return await _collection.Find(filter).ToListAsync();
    }

    public async Task UpdateAsync(Alert alert,string status,double seconds = 0)
    {
        var update = Builders<Alert>.Update
            .Set(x => x.Status, status)
            .Set(x => x.DurationSeconds, seconds);

        await _collection.UpdateOneAsync(x => x.AlertId == alert.AlertId,update);
    }
}