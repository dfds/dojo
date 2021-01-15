using System;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using kafka_deep_dive.Models;
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
            var newWorkshop = new WorkshopCreated
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Kafka - The basics",
                Date = DateTime.Now.ToString("f")
            };
            
            using (var producer = _producerFactory.Create())
            {
                await producer.ProduceAsync(topic: topic, message: new Message<string, string>()
                {
                    Key = Guid.NewGuid().ToString(),
                    Value = MessagingHelper.MessageToEnvelope(newWorkshop, "workshop_created", Guid.NewGuid().ToString())
                });
            }
        }
    }
}