DFDS Azure Monitoring Training - Code kata #2
======================================

This training exercise is a **beginner-level** course on Azure Monitoring that serves as a starting point for Developers looking to onboard the container efforts at DFDS. 

## Getting started
These instructions will help you prepare for the kata and ensure that your training machine has the tools installed you will need to complete the assignment(s). If you find yourself in a situation where one of more tools might not be available for your training environment please reach out to your instructor for assistance on how to proceed, post an [issue in our repository](https://github.com/dfds/dojo/issues) or fix it yourself and update the kata via a [pull request](https://github.com/dfds/dojo/pulls).

### Prerequisites
* [.NET Core](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)

## Exercise
This exercise will focus on extending the basic features provided out of the box by Application Insights SDK in our existing .NET Core application to help you better understand how customizer the logs and metrics that are shipped to Azure Resource Monitor.

### 1. Create a kata directory
First we setup a directory for our exercise files. It's pretty straight forward:

```
mkdir kata2
cd kata2
```

### 2. Working with the TelemetryClient
TODO: https://docs.microsoft.com/en-us/azure/azure-monitor/profiler/profiler-trackrequests


### 3. Working with TelemetryChannels
Telemetry channels are responsible for buffering telemetry items and sending them to the Application Insights service, where they're stored for querying and analysis. A telemetry channel is any class that implements the Microsoft.ApplicationInsights.ITelemetryChannel interface. 

To test it out ourselves we will extend our ASP.NET Core example from [Kata #1](../1/kata/README.md) by adding a built-in `ServerTelemetryChannel` that can route Telemetry data to a storage location we define. Simply add the following code to the existing `Program.cs`:

```
services.AddSingleton(typeof(ITelemetryChannel), new ServerTelemetryChannel() {StorageFolder = @"c:\temp\applicationinsights" });
```

Then build and re-run the application and now the Application Insights SDK will start shipping telemetry data to the folder you have specified.


### 4. Working with TelemetryProcessors
You can write and configure plug-ins for the Application Insights SDK to customize how telemetry can be enriched and processed before it's sent to the Application Insights service. In generel there are three different concepts we need to understand when working with processors which we will discuss in this section:

* **Sampling** => relates to reducing the volume of telemetry without affecting your statistics. It keeps together related data points so that you can navigate between them when you diagnose a problem.

* **Filtering** => telemetry processors lets you filter out telemetry in the SDK before it's sent to the server. For example, you could reduce the volume of telemetry by excluding requests from robots. Filtering is a more basic approach to reducing traffic than sampling. It allows you more control over what's transmitted, but it affects your statistics. For example, you might filter out all successful requests.

* **Initializers** add or modify properties to any telemetry sent from your app, which includes telemetry from the standard modules. For example, you could add calculated values or version numbers by which to filter the data in the portal.

#### Configure adaptive sampling
TODO: https://docs.microsoft.com/en-us/azure/azure-monitor/app/sampling#configuring-adaptive-sampling-for-aspnet-core-applications

#### Configure with Fixed-rate sampling
TODO: https://docs.microsoft.com/en-us/azure/azure-monitor/app/sampling#configuring-fixed-rate-sampling-for-aspnet-core-applications

#### Building a custom telemetry processor
TODO: https://docs.microsoft.com/en-us/azure/azure-monitor/app/api-filtering-sampling#create-a-telemetry-processor

#### Building a custom telemetry initializer
TODO: https://docs.microsoft.com/en-us/azure/azure-monitor/app/api-filtering-sampling#addmodify-properties-itelemetryinitializer

## Want to help make our training material better?
 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/dojo/issues).
 