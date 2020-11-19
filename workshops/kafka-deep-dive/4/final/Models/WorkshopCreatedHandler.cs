using System;
using System.Threading.Tasks;
using kafka_the_basics.Enablers;

namespace kafka_the_basics.Models
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