using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using PikudMerkazRabbit.Models;

namespace PikudMerkazRabbit.service;

public class RabbitService
{
    private readonly MongoService _mongoService;

    public RabbitService(MongoService mongoService)
    {
        _mongoService = mongoService;
    }

    public async Task StartAsync()
    {
        var factory = new ConnectionFactory
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest"
        };

        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        await channel.ExchangeDeclareAsync(exchange: "alerts.exchange",type: ExchangeType.Direct,durable: true);
        await channel.QueueDeclareAsync(queue: "alerts.CENTER",durable: true,exclusive: false,autoDelete: false);
        await channel.QueueBindAsync(queue: "alerts.CENTER",exchange: "alerts.exchange",routingKey: "CENTER");
        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (_, e) =>
        {
            try
            {
                string message = Encoding.UTF8.GetString(e.Body.ToArray());

                Alert? alert = JsonSerializer.Deserialize<Alert>(message);

                if (alert == null || string.IsNullOrWhiteSpace(alert.AlertId) || alert.Command != "CENTER")
                {
                    Console.WriteLine("Invalid South alert.");

                    await channel.BasicRejectAsync(e.DeliveryTag, false);
                    return;
                }

                await _mongoService.SaveAsync(alert);

                await channel.BasicAckAsync(e.DeliveryTag, false);
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Invalid JSON: {ex.Message}");

                await channel.BasicRejectAsync(e.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Save failed: {ex.Message}");

                await Task.Delay(10);

                await channel.BasicNackAsync(e.DeliveryTag, false, true);
            }
        };

        await channel.BasicConsumeAsync(queue: "alerts.CENTER",autoAck: false,consumer: consumer);

        Console.WriteLine("Listening for center alerts...");
        Console.WriteLine("Press Enter to exit.");
        Console.ReadLine();
    }
}