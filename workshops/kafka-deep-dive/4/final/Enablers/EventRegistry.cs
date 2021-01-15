using System;
using System.Collections.Generic;
using System.Linq;

namespace kafka_deep_dive.Enablers
{
    public class EventRegistry
    {
        private readonly List<EventRegistration> _registrations = new List<EventRegistration>();

        public IEnumerable<EventRegistration> Registrations => _registrations;

        public EventRegistry Register<TEvent>(string eventTypeName, string topicName)
        {
            _registrations.Add(new EventRegistration
            (
                eventType: eventTypeName,
                eventInstanceType: typeof(TEvent),
                topic: topicName
            ));

            return this;
        }

        public bool IsRegistered(Type eventInstanceType)
        {
            return _registrations.Any(x => x.EventInstanceType == eventInstanceType);
        }

        public string GetTopicFor(string eventType)
        {
            var registration = _registrations.SingleOrDefault(x => x.EventType == eventType);

            if (registration == null)
            {
                throw new Exception($"Error! Could not determine \"topic name\" due to no registration was found for event type \"{eventType}\"!");
            }

            return registration.Topic;
        }

        public string GetTypeNameFor(object eventRegistration)
        {
            var registration = _registrations.SingleOrDefault(x => x.EventInstanceType == eventRegistration.GetType());

            if (registration == null)
            {
                throw new Exception($"Error! Could not determine \"event type name\" due to no registration was found for type {eventRegistration.GetType().FullName}!");
            }

            return registration.EventType;
        }
        
        public IEnumerable<string> GetAllTopics()
        {
            var topics = _registrations.Select(x => x.Topic).Distinct();           

            return topics;
        }
        
        public Type GetInstanceTypeFor(string eventName)
        {
            var registration = _registrations.SingleOrDefault(x => x.EventType == eventName);

            if (registration == null)
            {
                throw new Exception($"Error! Could not determine \"event instance type\" due to no registration was found for type {eventName}!");
            }

            return registration.EventInstanceType;
        }
    }

    public class EventRegistration
    {
        public string EventType { get; }
        public Type EventInstanceType { get; }
        public string Topic { get; }

        public EventRegistration(string eventType, Type eventInstanceType, string topic)
        {
            EventType = eventType;
            EventInstanceType = eventInstanceType;
            Topic = topic;
        }
    }
}