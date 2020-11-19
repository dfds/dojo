using kafka_the_basics.Enablers;
using Newtonsoft.Json.Linq;

namespace kafka_the_basics.Models
{
    public class WorkshopCreated
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Date { get; set; }

        public WorkshopCreated(Message msg)
        {
            var data = (msg.Payload as JObject)?.ToObject<WorkshopCreated>();
            Id = data.Id;
            Title = data.Title;
            Date = data.Date;
        }
        public WorkshopCreated() {}
    }
}