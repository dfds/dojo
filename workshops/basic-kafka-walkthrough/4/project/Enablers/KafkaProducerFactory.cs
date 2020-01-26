using System;
using Confluent.Kafka;

namespace kafka_the_basics.Enablers
{
    public class KafkaProducerFactory
    {
        private readonly KafkaConfiguration _configuration;

        public KafkaProducerFactory(KafkaConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IProducer<string, string> Create()
        {
            var config = new ProducerConfig(_configuration.GetProducerConfiguration());
            var builder = new ProducerBuilder<string, string>(config);
            builder.SetErrorHandler(OnKafkaError);
            return builder.Build();
        }
        
        private void OnKafkaError(IProducer<string, string> producer, Error error)
        {
            if (error.IsFatal)
                Environment.FailFast($"Fatal error in Kafka producer: {error.Reason}. Shutting down...");
            else
                throw new Exception(error.Reason);
        }
    }
}