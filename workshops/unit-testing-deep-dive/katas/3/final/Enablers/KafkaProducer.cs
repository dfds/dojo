using System;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;

namespace kafka_deep_dive.Enablers
{
    public class KafkaProducer : BackgroundService
    {
        private readonly KafkaProducerFactory _producerFactory;

        public KafkaProducer(KafkaProducerFactory producerFactory)
        {
            _producerFactory = producerFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("KafkaProducer started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await DoWork(stoppingToken);
                }
                catch (Exception err)
                {
                    Console.WriteLine($"Error processing and/or publishing messages to Kafka.");
                }

                await Task.Delay(TimeSpan.FromSeconds(6), stoppingToken);
            }
        }

        private async Task DoWork(CancellationToken stoppingToken)
        {
            const string topic = "build.workshop.something";
            using (var producer = _producerFactory.Create())
            {
                await producer.ProduceAsync(topic: topic, message: new Message<string, string>()
                {
                    Key = Guid.NewGuid().ToString(),
                    Value = $"kafka-the-basics is running. This message ID is {Guid.NewGuid().ToString()}"
                });
            }
        }
    }
}