DFDS Azure Monitoring Training - Code kata #3
======================================

This training exercise is a **beginner-level** course on Azure Monitoring that serves as a starting point for Developers looking to get started with distributed tracing at DFDS. 

## Getting started
These instructions will help you prepare for the kata and ensure that your training machine has the tools installed you will need to complete the assignment(s). If you find yourself in a situation where one or more tools might not be available for your training environment please reach out to your instructor for assistance on how to proceed, post an [issue in our repository](https://github.com/dfds/dojo/issues) or fix it yourself and update the kata via a [pull request](https://github.com/dfds/dojo/pulls).

### Prerequisites
* [.NET Core](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
* [Kata #2](../2/kata/README.md)

## Exercise
This exercise will focus on working with Metrics in the Application Insights SDK and how to make it easy using the emit custom metrics from your application workloads.

### 1. Create a kata directory
First we setup a directory for our exercise files. It's pretty straight forward:

```
mkdir kata3
cd kata3
```

### 2. Working with Metrics
Now its time to get working with metrics! 

However before we start there is a subtle, yet crucial, concept to understand when using Application Insights SDK to collect metrics which is that there are two different methods of collecting custom metrics, `TrackMetric()`, and `GetMetric()`. The key difference between these two methods is that `TrackMetric()` sends raw telemetry denoting a single metric. It's inefficient to send a single telemetry item for each value comes with a performance penelty since every TrackMetric(item) goes through the full pipeline of telemetry initializers and processors. Unlike `TrackMetric()`, `GetMetric()` handles local pre-aggregation for you and then only submits an aggregated summary metric at a fixed interval of one minute. So if you need to closely monitor some custom metric at the second or even millisecond level you can do so while only incurring the storage and network traffic cost of only monitoring every minute. This behavior also greatly reduces the risk of throttling occurring since the total number of telemetry items that need to be sent for an aggregated metric are greatly reduced.

#### Sending custom metrics
To built on the previous work we will implement a simple custom telemetry processer which will emit a single metric value everytime a telemetry pipeline is run. Thanks to the way `GetMetric()` was implemented this type of logic will not incure an overhead because of the local pre-aggregation. However using the `TrackMetric()` method in this type of implementation would most likely cause performance issues in your application.

```
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.DataContracts;

public class MyCustomMetricsProcessor : ITelemetryProcessor
{
    private ITelemetryProcessor Next { get; set; }
    private TelemetryClient _telemetryClient = new TelemetryClient();

    // next will point to the next TelemetryProcessor in the chain.
    public MyCustomMetricsProcessor(ITelemetryProcessor next, TelemetryClient tc)
    {
        this.Next = next;
        _telemetryClient = tc;
    }

    public void Process(ITelemetry item)
    {           
        _telemetryClient.GetMetric("MyCustomMetric").TrackValue(42);

        this.Next.Process(item);
    }
}
```

Once the processor is implemented all we have to do to wire it into our application is register it with the rest of our Application Insights DI resources in the `Program.cs` file using the following snippet:

```
services.AddApplicationInsightsTelemetryProcessor<MyCustomMetricsProcessor>();
```

#### Working with multi-dimensional metrics
Sometimes we want to ship more values in a metric these types of metrics are called multi-dimensional metrics and are great for adding in extra information to support the "core metric value" your measuring. A multi-dimensional implementation of the above would look something like this:

```
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.DataContracts;

public class MyCustomMultiDimensionalMetricsProcessor : ITelemetryProcessor
{
    private ITelemetryProcessor Next { get; set; }
    private TelemetryClient _telemetryClient = new TelemetryClient();

    // next will point to the next TelemetryProcessor in the chain.
    public MyCustomMultiDimensionalMetricsProcessor(ITelemetryProcessor next, TelemetryClient tc)
    {
        this.Next = next;
        _telemetryClient = tc;
    }

    public void Process(ITelemetry item)
    {           

        var metric = _telemetryClient.GetMetric("MyCustomMetric", "Dimension1");

        metric.TrackValue(42, "SomeDimensionalValue")
        metric.TrackValue(24, "SomeOtherDimensionalValue")
        metric.TrackValue(124, "YetAnotherDimensionalValue")

        this.Next.Process(item);
    }
}
```

Once the processor is implemented all we have to do to wire it into our application is register it with the rest of our Application Insights DI resources in the `Program.cs` file using the following snippet:

```
services.AddApplicationInsightsTelemetryProcessor<MyCustomMultiDimensionalMetricsProcessor>();
```


## Want to help make our training material better?
 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/dojo/issues).
 