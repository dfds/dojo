using System;
using System.Threading.Tasks;
using kafka_deep_dive.Enablers;

namespace kafka_deep_dive.Models
{
    public class WorkshopCreatedHandler : IEventHandler<WorkshopCreated>
    {
        public async Task HandleAsync(WorkshopCreated eventInstance)
        {
            Console.WriteLine("WorkshopCreatedHandler has been triggered");
            Console.WriteLine($"A workshop was created at '{eventInstance.Date}' with name: {eventInstance.Title}");
        }
    }
}