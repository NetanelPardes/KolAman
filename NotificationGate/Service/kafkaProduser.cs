using System;
using System.Text.Json;
using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Serilog;
namespace NotificationGate.service
{
    public class kafkaProduser
    {
        private readonly string _bootstrapServvers;
        private readonly IProducer<Null, string> _producer;

        public kafkaProduser(string bootstrapServvers)
        {
            _bootstrapServvers = bootstrapServvers;

            var config = new ProducerConfig
            {
                BootstrapServers = _bootstrapServvers
            };

            _producer = new ProducerBuilder<Null, string>(config).Build();
        }
        public async Task EnsureTopicEistsAsync(string topic ,int numPartitions = 1 ,short replicationFactor = 1)
        {
            var config = new AdminClientConfig
            {
                BootstrapServers = _bootstrapServvers
            };
            using var adminClient = new AdminClientBuilder(config).Build();
            try
            {
                await adminClient.CreateTopicsAsync(new[]
                {
                    new TopicSpecification
                    {
                        Name = topic,
                        NumPartitions = numPartitions,
                        ReplicationFactor = replicationFactor
                    }
                });
            }
            catch (CreateTopicsException exception)
            {
                if (exception.Results[0].Error.Code == ErrorCode.TopicAlreadyExists)
                {
                    Console.WriteLine($"Topic {topic} already exists");
                }
                else
                {
                    throw;
                }
            }
        }
        public async Task<DeliveryResult<Null, string>> SendAsync<T>(string topic,T message)
        {
            string json = JsonSerializer.Serialize(message);
            var kafkaMessage = new Message<Null, string>
            {
                Value = json
            };
            var result = await _producer.ProduceAsync(topic, kafkaMessage);
            Console.WriteLine($"Sent to: {result.Topic}");
            return result;
        }
        
    }
}