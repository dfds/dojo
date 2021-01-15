using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace kafka_deep_dive.Enablers
{
    public class MessagingHelper
    {
        public static string MessageToEnvelope(object payload, string eventName, string correlationId, string version = "1")
        {
            var msg = new Message
            {
                Version = version,
                EventName = eventName,
                XCorrelationId = correlationId,
                XSender = Assembly.GetExecutingAssembly().FullName,
                Payload = payload
            };

            return JsonConvert.SerializeObject(msg, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }

        public static T MessageToObject<T>(string msg)
        {
            return (T) MessageEnvelopeToMessage(msg).Payload;
        }

        public static Message MessageEnvelopeToMessage(string msg)
        {
            return JsonConvert.DeserializeObject<Message>(msg);
        }
    }

    public class Message
    {
        public string Version { get; set; }
        public string EventName  { get; set; }
        [JsonProperty(PropertyName = "x-correlationId")]
        public string XCorrelationId  { get; set; }
        [JsonProperty(PropertyName = "x-sender")]
        public string XSender { get; set; }
        public object Payload { get; set; }

        public Message(string version, string eventName, string xCorrelationId, string xSender, object payload)
        {
            Version = version;
            EventName = eventName;
            XCorrelationId = xCorrelationId;
            XSender = xSender;
            Payload = payload;
        }
        
        public Message() {}

    }
}