using System.Diagnostics;
using hamal.Models;

namespace hamal.Services;

public class HamalService
{
    private readonly MongoService _mongoService;

    public HamalService(MongoService mongoService)
    {
        _mongoService = mongoService;
    }

    public async Task StartAsync()
    {
        Console.WriteLine("Hamal started");

        while (true)
        {
            Console.WriteLine("Searching for new alerts...");

            var alerts = await _mongoService.GetNewAsync();

            Console.WriteLine($"Found {alerts.Count} new alerts");

            foreach (var alert in alerts)
            {
                Console.WriteLine($"Alert: {alert.AlertId} | Priority: {alert.Priority}");

                if (alert.Priority == "LOW")
                {
                    await _mongoService.UpdateAsync(alert, "CANCEL");
                    Console.WriteLine($"Alert: {alert.AlertId} | CANCEL");
                    continue;
                }
                await _mongoService.UpdateAsync(alert, "INPROGRESS");
                Console.WriteLine($"Alert: {alert.AlertId} | INPROGRESS");
                await HandleAsync(alert);
            }

            await Task.Delay(1000);
        }
    }

    private async Task HandleAsync(Alert alert)
    {
        try
        {
            Console.WriteLine($"Alert: {alert.AlertId} | Treatment started");

            var stopwatch = Stopwatch.StartNew();

            await Task.Delay(5000);

            stopwatch.Stop();

            var seconds = stopwatch.Elapsed.TotalSeconds;

            await _mongoService.UpdateAsync(alert,"DONE",seconds);

            Console.WriteLine($"Alert: {alert.AlertId} | DONE | Time: {seconds:F2} seconds");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Alert: {alert.AlertId} | Error: {ex.Message}");
        }
    }
}