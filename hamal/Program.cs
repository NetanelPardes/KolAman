using System;
using MongoDB.Driver;
using hamal.Services;

namespace hamal
{
    public class Program
    {
        static async Task Main()
        {
            var mongoService = new MongoService("mongodb://localhost:27017");
            var hamalService = new HamalService(mongoService);
            await hamalService.StartAsync();
        }
    }
}
