using System;
using Confluent.Kafka;

namespace kafka_the_basics.Enablers
{
    public class KafkaConsumerFactory
    {
        private readonly KafkaConfiguration _configuration;

        public KafkaConsumerFactory(KafkaConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IConsumer<string, string> Create()
        {
            var config = new ConsumerConfig(_configuration.GetConsumerConfiguration());
            var builder = new ConsumerBuilder<string, string>(config);
            builder.SetErrorHandler(OnKafkaError);
            return builder.Build();
        }
        
        private void OnKafkaError(IConsumer<string, string> consumer, Error error)
        {
            if (error.IsFatal)
                Environment.FailFast($"Fatal error in Kafka consumer: {error.Reason}. Shutting down...");
            else
                throw new Exception(error.Reason);
        }
    }
}