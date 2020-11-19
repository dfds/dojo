using System;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace kafka_the_basics.Enablers
{
    public class KafkaConsumer : IHostedService
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly KafkaConsumerFactory _consumerFactory;
        private readonly IServiceProvider _serviceProvider;
        private readonly EventRegistry _eventRegistry;
        private readonly EventHandlerFactory _eventHandlerFactory;
        private Task _executingTask;

        public KafkaConsumer(KafkaConsumerFactory consumerFactory, IServiceProvider serviceProvider, EventRegistry eventRegistry, EventHandlerFactory eventHandlerFactory)
        {
            _consumerFactory = consumerFactory;
            _serviceProvider = serviceProvider;
            _eventRegistry = eventRegistry;
            _eventHandlerFactory = eventHandlerFactory;
        }
        
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _executingTask = Task.Factory.StartNew(async () =>
                    {
                        using (var consumer = _consumerFactory.Create())
                        {
                            const string topic = "build.workshop.something";
                            consumer.Subscribe(topic);
                            Console.WriteLine($"KafkaConsumer started. Listening to topic: {topic}");

                            // Consume loop
                            while (!_cancellationTokenSource.IsCancellationRequested)
                            {
                                ConsumeResult<string, string> msg;
                                try
                                {
                                    msg = consumer.Consume(cancellationToken);
                                    using (var scope = _serviceProvider.CreateScope())
                                    {
                                        var message = MessagingHelper.MessageEnvelopeToMessage(msg.Value);
                                        var eventType = _eventRegistry.GetInstanceTypeFor(message.EventName);
                                        dynamic eventInstance = Activator.CreateInstance(eventType, message);
                                        dynamic handlersList = _eventHandlerFactory.GetEventHandlersFor(eventInstance, scope);

                                        foreach (var handler in handlersList)
                                        {
                                            await handler.HandleAsync(eventInstance);
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Consumption of message failed, reason: {ex}");
                                    continue;
                                }

                                try
                                {
                                    await Task.Run(() => consumer.Commit(msg));
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Error consuming message: Exception message: {ex.Message}. Raw message: '{msg.Value}'");
                                }
                            }
                        }
                    }, _cancellationTokenSource.Token,
                    TaskCreationOptions.LongRunning, TaskScheduler.Default)
                .ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        Console.WriteLine("Event loop crashed");
                    }
                }, cancellationToken);
            
            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                _cancellationTokenSource.Cancel();
            }
            finally
            {
                await Task.WhenAny(_executingTask, Task.Delay(-1, cancellationToken));
            }
            
            _cancellationTokenSource.Dispose();
        }
    }
}