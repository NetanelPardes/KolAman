using PikudMerkazRabbit.service;

namespace PikudMerkazRabbit;

class Program
{
    static async Task Main()
    {
        var mongoService = new MongoService("mongodb://localhost:27017");
        var rabbitService = new RabbitService(mongoService);
        await rabbitService.StartAsync();
    }
}