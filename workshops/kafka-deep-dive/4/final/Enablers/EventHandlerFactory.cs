using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace kafka_deep_dive.Enablers
{
    public class EventHandlerFactory
    {
        public IEnumerable<IEventHandler<TEvent>> Create<TEvent>(IServiceScope serviceScope)
        {
            var eventHandlers = serviceScope.ServiceProvider.GetServices<IEventHandler<TEvent>>();
            return eventHandlers;
        }

        public IEnumerable<IEventHandler<TEvent>> GetEventHandlersFor<TEvent>(TEvent domainEvent, IServiceScope serviceScope)
        {
            var eventHandlers = Create<TEvent>(serviceScope);
            return eventHandlers;
        }
    }

    public interface IEventHandler<in T>
    {
        Task HandleAsync(T eventInstance);
    }
}