using System;
using System.Collections.Generic;
using System.Linq;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;

namespace kafka_deep_dive.Enablers
{
    public class KafkaConfiguration
    {
        private const string KEY_PREFIX = "KAFKATHEBASICS_KAFKA_";
        private readonly IConfiguration _configuration;

        public KafkaConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private string Key(string keyName) => string.Join("", KEY_PREFIX, keyName.ToUpper().Replace('.', '_'));

        private Tuple<string, string> GetConfiguration(string key)
        {
            var value = _configuration[Key(key)];

            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            return Tuple.Create(key, value);
        }
        
        public ConsumerConfig GetConsumerConfiguration()
        {
            return new ConsumerConfig(AsDictionary());
        }

        public ProducerConfig GetProducerConfiguration()
        {
            return new ProducerConfig(AsDictionary());
        }

        public IDictionary<string, string> AsDictionary()
        {
            var configurationKeys = new[]
            {
                "group.id",
                "enable.auto.commit",
                "bootstrap.servers",
                "broker.version.fallback",
                "api.version.fallback.ms",
                "ssl.ca.location",
                "sasl.username",
                "sasl.password",
                "sasl.mechanisms",
                "security.protocol",
            };

            var config = configurationKeys
                .Select(key => GetConfiguration(key))
                .Where(pair => pair != null)
                .Select(pair => new KeyValuePair<string, string>(pair.Item1, pair.Item2))
                .ToDictionary(x => x.Key, v => v.Value);
            
            config.Add("request.timeout.ms", "3000");

            return config;
        }
        
    }
}