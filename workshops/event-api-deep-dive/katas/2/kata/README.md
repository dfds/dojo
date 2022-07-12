DFDS Event API Deep Dive - Code kata #2
======================================

This training exercise is a **beginner-level** course on [AsyncAPI](https://www.asyncapi.com/docs/reference/specification/v2.1.0) and how it can help developers provide discovery meta-data services for their event APIs in DFDS. 

## Getting started
These instructions will help you prepare for the kata and ensure that your training machine has the tools installed you will need to complete the assignment(s). If you find yourself in a situation where one or more tools might not be available for your training environment please reach out to your instructor for assistance on how to proceed, post an [issue in our repository](https://github.com/dfds/dojo/issues) or fix it yourself and update the kata via a [pull request](https://github.com/dfds/dojo/pulls).

### Prerequisites
* [.NET Core](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)

## Exercise
In this exercise we will use the Neuroglia.AsyncApi & MQTTnet packages to create a simple .NET Core Background service that runs a MQTT client which emits a ProductSoldEvent message that others clients can subscribe too. 

### 1. Create a kata directory
First we setup a directory for our exercise files. It's pretty straight forward:

```
mkdir kata2
cd kata2
```

### 2. Create a ASP.NET Core Web App project
Secondly we need to create a ASP.NET Core Web App boilerplate project using the dotnet CLI:

```
dotnet new webapp,razor
```

Just to explain: <br/>
`dotnet` - is the dotnet CLI <br/>
`new` - instructs the dotnet CLI to create a new project in the current directory.<br/>
`webapp,razor` - tells the dotnet CLI which project templates to apply


### 3. Add required NuGet packages for our sample service
Once our new boilerplate is up and running we need to add the third-party dependencies (NuGet packages) needed to implement a working AsyncAPI sample:

```
dotnet add package Neuroglia.AsyncApi.AspNetCore.UI
dotnet add package MQTTnet
```

Just to explain: <br/>
`dotnet` - is the dotnet CLI <br/>
`add` - instructs the dotnet CLI to add a new item to the project.<br/>
`package` - specifies the type of item being added to the project


### 4. Create a simple ProductSoldEvent implementation
Now we can begin working on the actual implementation and our first task we be to create a ProductSoldEvent that can be emitted from our Event API to its subscribers:

```
using Neuroglia.AsyncApi;
using System;
using System.Collections.Generic;
using System.ComponentModel;

[Tag("product", "A tag for product messages")]
[Message(Name = "ProductSold")]
public class ProductSoldEvent
{
    [Description("The id of the event")]
    public int Id { get; set; }

    [Description("The date and time at which the event has been created")]
    public DateTime SentAt { get; set; }

    [Description("The event's metadata")]
    public Dictionary<string, string> Metadata { get; set; }

}
```


### 5. Implement a BackgroundService that exposes a Event API
With our ProductSoldEvent implementation in place we can now finalize the Event API by implementing the ProductService as follows:

```
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using StreetLightsApi.Server.Messages;
using Neuroglia.Serialization;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Neuroglia.AsyncApi;

[AsyncApi("Product API", "1.0.0", Description = "The Product API.", LicenseName = "Apache 2.0", LicenseUrl = "https://www.apache.org/licenses/LICENSE-2.0")]
public class ProductService : BackgroundService
{
    public ProductService(ILogger<ProductService> logger, IJsonSerializer serializer)
    {
        this.Logger = logger;
        this.Serializer = serializer;
    }

    protected ILogger Logger { get; }

    protected IJsonSerializer Serializer { get; }

    protected IMqttClient MqttClient { get; private set; }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        this.MqttClient = new MqttFactory().CreateMqttClient();

        MqttClientOptions options = new()
        {
            ChannelOptions = new MqttClientTcpOptions()
            {
                Server = "test.mosquitto.org"
            }
        };

        await this.MqttClient.ConnectAsync(options, stoppingToken);

        stoppingToken.Register(async () => await this.MqttClient.DisconnectAsync());

        this.MqttClient.UseApplicationMessageReceivedHandler(async message =>
        {
            var e = await this.Serializer.DeserializeAsync<ProductSoldEvent>(Encoding.UTF8.GetString(message.ApplicationMessage.Payload));
            await this.OnProductSold(e);
            await message.AcknowledgeAsync(stoppingToken);
        });

        await this.MqttClient.SubscribeAsync("onProductSold");
        await this.PublishLightMeasured(new() { Id = 415, SentAt = DateTime.UtcNow });
    }

    [Tag("sold", "A tag for product operations")]
    [Channel("sold"), PublishOperation(OperationId = "NotifyProductSold", Summary = "Notifies remote consumers about product sales")]
    public async Task PublishProductSold(ProductSoldEvent e)
    {
        MqttApplicationMessage message = new()
        {
            Topic = "onProductSold",
            ContentType = "application/json",
            Payload = Encoding.UTF8.GetBytes(await this.Serializer.SerializeAsync(e))
        };
        await this.MqttClient.PublishAsync(message);
    }

    [Tag("sold", "A tag for product operations")]
    [Channel("sold"), SubscribeOperation(OperationId = "OnProductSold", Summary = "Inform about sales params for a particular product")]
    protected async Task OnProductSold(LightMeasuredEvent e)
    {
        this.Logger.LogInformation($"Event received:{Environment.NewLine}{await this.Serializer.SerializeAsync(e)}");
    }
}
```


### 6. Add AsyncAPI generation & ProductService to DI in Startup.cs
Once our event & service is complete all that remains to be done is to register them with the DI container in the ConfigureServices method in `Startup.cs` and setup the AsyncAPiGeneration for our meta-data endpoint:

```
services.AddAsyncApiGeneration(builder => builder.WithMarkupType<StreetLightsService>()
.UseDefaultConfiguration(asyncApi =>
{
    asyncApi
        .WithTermsOfService(new Uri("https://www.websitepolicies.com/blog/sample-terms-service-template"))
        .UseServer("mosquitto", server => server
            .WithUrl(new Uri("mqtt://test.mosquitto.org"))
            .WithProtocol(AsyncApiProtocols.Mqtt)
            .WithDescription("The Mosquitto test MQTT server")
            .UseBinding(new MqttServerBindingDefinition()
            {
                ClientId = "ProductsAPI:1.0.0",
                CleanSession = true
            }));
}));

services.AddSingleton<ProductService>();
services.AddSingleton<IHostedService>(provider => provider.GetRequiredService<ProductService>());
```


### 7. Build & run solution
Lastly we need to build & run the code, which is as simple as running the following dotnet CLI command:

```
dotnet run
```

## Want to help make our training material better?
 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/dojo/issues).
 