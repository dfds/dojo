DFDS Azure Monitoring Training - Code kata #2
======================================

This training exercise is a **beginner-level** course on Azure Monitoring that serves as a starting point for Developers looking to get started with distributed tracing at DFDS. 

## Getting started
These instructions will help you prepare for the kata and ensure that your training machine has the tools installed you will need to complete the assignment(s). If you find yourself in a situation where one of more tools might not be available for your training environment please reach out to your instructor for assistance on how to proceed, post an [issue in our repository](https://github.com/dfds/dojo/issues) or fix it yourself and update the kata via a [pull request](https://github.com/dfds/dojo/pulls).

### Prerequisites
* [.NET Core](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
* [Kata #1](../1/kata/README.md)

## Exercise
This exercise will focus on extending the basic features provided out of the box by Application Insights SDK in our existing .NET Core application to help you better understand how customizer the logs and metrics that are shipped to Azure Resource Monitor.

### 1. Create a kata directory
First we setup a directory for our exercise files. It's pretty straight forward:

```
mkdir kata2
cd kata2
```

### 2. Working with TelemetryChannels
Telemetry channels are responsible for buffering telemetry items and sending them to the Application Insights service, where they're stored for querying and analysis. A telemetry channel is any class that implements the Microsoft.ApplicationInsights.ITelemetryChannel interface. 

To test it out ourselves we will extend our ASP.NET Core example from [Kata #1](../1/kata/README.md) by adding a built-in `ServerTelemetryChannel` that can route Telemetry data to a storage location we define. Simply add the following snippet after the `builder.Services.AddApplicationInsightsTelemetry();` in the existing `Program.cs` file:

```
services.AddSingleton(typeof(ITelemetryChannel), new ServerTelemetryChannel() {StorageFolder = @"c:\temp\applicationinsights" });
```

Then build and run the application to verify that Application Insights SDK will start shipping telemetry data to the folder you have specified.


### 3. Working with TelemetryProcessors
You can write and configure plug-ins for the Application Insights SDK to customize how telemetry can be enriched and processed before it's sent to the Application Insights service. In generel there are three different concepts we need to understand when working with processors which we will discuss in this section:

* **Sampling** => relates to reducing the volume of telemetry without affecting your statistics. It keeps together related data points so that you can navigate between them when you diagnose a problem.

* **Filtering** => telemetry processors lets you filter out telemetry in the SDK before it's sent to the server. For example, you could reduce the volume of telemetry by excluding requests from robots. Filtering is a more basic approach to reducing traffic than sampling. It allows you more control over what's transmitted, but it affects your statistics. For example, you might filter out all successful requests.

* **Initializers** add or modify properties to any telemetry sent from your app, which includes telemetry from the standard modules. For example, you could add calculated values or version numbers by which to filter the data in the portal.

So lets get started and learn about all of these interesting topics!

#### How to configure sampling
TODO: https://docs.microsoft.com/en-us/azure/azure-monitor/app/sampling#configuring-adaptive-sampling-for-aspnet-core-applications
TODO: https://docs.microsoft.com/en-us/azure/azure-monitor/app/sampling#configuring-fixed-rate-sampling-for-aspnet-core-applications

#### Filtering via custom telemetry processors
Processors are quiet simple to reason about as they follow a simple [Chain-Of-Responsibility pattern](https://www.dofactory.com/net/chain-of-responsibility-design-pattern) which just chains processors together from start to finish and then ships off the data to the targeted telemetry channels. So lets get cracking! 

First we need to create a simple telemetry processer called `MyFilter`. It can be done using the following code:

```
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.DataContracts;

public class MyFilter : ITelemetryProcessor
{
    private ITelemetryProcessor Next { get; set; }

    // next will point to the next TelemetryProcessor in the chain.
    public MyFilter(ITelemetryProcessor next)
    {
        this.Next = next;
    }

    public void Process(ITelemetry item)
    {
        // Check if the item should be filtered out or sent to the next processor.
        if (!OKtoSend(item)) { return; }

        this.Next.Process(item);
    }

    // Example: replace with your own criteria.
    private bool OKtoSend (ITelemetry item)
    {
        var dependency = item as DependencyTelemetry;
        if (dependency == null) return true;

        return dependency.Success != true;
    }
}
```

Once the processor is implemented all we have to do to wire it into our application is register it with the rest of our Application Insights DI resources in the `Program.cs` file using the following snippet:

```
services.AddApplicationInsightsTelemetryProcessor<MyFilter>();
```


#### Initializing via custom telemetry initializers
Initializers are also quiet simple to reason about as they follow a simple [Pipes & Filters pattern](https://docs.microsoft.com/en-us/azure/architecture/patterns/pipes-and-filters) which most of us are familiar with from ASP.NET Core APIs. In this example we will implement a custom TelemetryInitializer that overrides the default SDK behavior of treating response codes >= 400 as failed requests. 

First step is to create a simple telemetry initializer called `MyInitializer` via the following code:

```
using System;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

public class MyInitializer : ITelemetryInitializer
{
    public void Initialize(ITelemetry telemetry)
    {
        var requestTelemetry = telemetry as RequestTelemetry;
        if (requestTelemetry == null) return;

        int code;        
        if (!Int32.TryParse(requestTelemetry.ResponseCode, out code)) return;
        
        if (code >= 400 && code < 500)
        {
            // If we set the Success property, the SDK won't change it:
            requestTelemetry.Success = true;

            // Allow us to filter these requests in the portal:
            requestTelemetry.Properties["Overridden400s"] = "true";
        }
        
        // else leave the SDK to set the Success property
    }
}
```

Once the initializer is implemented all we have to do in order to wire it into our application is register it with the rest of our Application Insights DI resources in the `Program.cs` file using the following snippet:

```
services.AddSingleton<ITelemetryInitializer, MyFilter>();
```


### 4. Working with the TelemetryClient
In some scenarios you cant leverage the auto-instrumentation features provided by the Application Insights SDK (e.g. Azure Cloud Service workers). Thankfully MS provides us with a way to manually track requests in these scenarios via the TelemetryClient which coincidently can also be used for other things like emitting events, exceptions and metrics. The code you need to start a request tracking operation looks something like this:

```
var productId = 1234;
var client = new TelemetryClient();

using (var getDetailsOperation = client.StartOperation<RequestTelemetry>("GetProductDetails"))
{
    try
    {
        var productDetails = new { Id = productId, Price = 0 };
        getDetailsOperation.Telemetry.Properties["ProductId"] = productId.ToString();

        // By using DependencyTelemetry class the nested 'GetProductPrice' request is correctly linked as part of the parent 'GetProductDetails' request.
        using (var getPriceOperation = client.StartOperation<DependencyTelemetry>("GetProductPrice"))
        {
            //Do whatever you need in here to fetch the price and assign it to the product details object. In this kata we will just assign a random value in memory.
            productDetails.Price = 1234;
        }

        getDetailsOperation.Telemetry.Success = true;

        return productDetails;
    }
    catch(Exception ex)
    {
        getDetailsOperation.Telemetry.Success = false;

        // This exception gets linked to the 'GetProductDetails' request telemetry.
        client.TrackException(ex);

        throw;
    }
}
```

Just to explain: <br/>
`TelemetryClient` - is the default client provided by the SDK. It will use the config from the static `TelemetryConfiguration.Active` unless another config is passed to it during initialization <br/>
`RequestTelemetry` - instructs the SDK that the operation in question is a "root" operation.<br/>
`DependencyTelemetry` - instructs the SDK that the operation in question is a "nested" operation.<br/>
`client.TrackException` - emit exception data to the Application Insights instance being targeted by the configuration (`TelemetryConfiguration.Active.InstrumentationKey`).<br/>


## Want to help make our training material better?
 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/dojo/issues).
 