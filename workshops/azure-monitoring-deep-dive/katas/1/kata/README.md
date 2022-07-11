DFDS Azure Monitoring Training - Code kata #1
======================================

This training exercise is a **beginner-level** course on Azure Monitoring that serves as a starting point for Developers looking to get started with distributed tracing at DFDS. 

## Getting started
These instructions will help you prepare for the kata and ensure that your training machine has the tools installed you will need to complete the assignment(s). If you find yourself in a situation where one or more tools might not be available for your training environment please reach out to your instructor for assistance on how to proceed, post an [issue in our repository](https://github.com/dfds/dojo/issues) or fix it yourself and update the kata via a [pull request](https://github.com/dfds/dojo/pulls).

### Prerequisites
* [.NET Core](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)

## Exercise
This exercise will focus on setting up the Application Insights SDK in a .NET Core application to help you ship logs & metrics to a Azure Monitoring workspace in order to trace user activities across a distributed system.

### 1. Create a kata directory
First we setup a directory for our exercise files. It's pretty straight forward:

```
mkdir kata1
cd kata1
```

### 2. Create a new Web API .NET Core project
Then we create some boilerplate code to test ApplicationInsights:

```
dotnet new webapi
```

Just to explain: <br/>
`dotnet` - is the dotnet CLI <br/>
`new` - instructs the dotnet CLI to create a new project in the current directory.<br/>
`webapi` - tells the dotnet CLI which project template to use


### 3. Add Microsoft.ApplicationInsights NuGet package
Once our boilerplate is generated we have to install the required libraries that will allow ApplicationInsights to collect data from the host process:

```
dotnet add package Microsoft.ApplicationInsights.AspNetCore
```

Just to explain: <br/>
`dotnet` - is the dotnet CLI <br/>
`add` - instructs the dotnet CLI its performing an add operation (akin to insert or create).<br/>
`package` - tells the dotnet CLI what type of resource we are adding (in this case a nuget package)


### 4. Enable Application Insights server-side telemetry
After the project is created and the necessary NuGet packages have been added we can proceed to registering the dependencies needed to run the server-side telemetry component of ApplicationInsights by adding the following to the newly generated `Program.cs`:

```
builder.Services.AddApplicationInsightsTelemetry();
```


### 5. Add Application Insights configuration section
Having registered the required dependencies we now simply have to add a ApplicationInsights configuration section to `appsettings.json` containing the connection string for our trainingdojo-appinsights resource:

```
"ApplicationInsights": {
    "ConnectionString" : "InstrumentationKey=03d03408-cd84-429d-a4ee-72b496f645fb;IngestionEndpoint=https://westeurope-5.in.applicationinsights.azure.com/;LiveEndpoint=https://westeurope.livediagnostics.monitor.azure.com/"
},
```


### 6. Build our application
With everything in place we can now build our code using the dotnet CLI:

```
dotnet build
```

Just to explain: <br/>
`dotnet` - is the dotnet CLI <br/>
`build` - instructs the dotnet CLI to build our code.

### 7. Run our application 
And finally we can run the application and start collection telemetry data in our ApplicationInsights resource. Good job!

```
dotnet run
```

Just to explain: <br/>
`dotnet` - is the dotnet CLI <br/>
`build` - instructs the dotnet CLI to run our code.

## Want to help make our training material better?
 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/dojo/issues).
 