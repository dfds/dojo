DFDS Event API Deep Dive - Code kata #1
======================================

This training exercise is a **beginner-level** course on event APIs that serve as a starting point for Developers looking to get started with event-driven systems at DFDS. 

## Getting started
These instructions will help you prepare for the kata and ensure that your training machine has the tools installed you will need to complete the assignment(s). If you find yourself in a situation where one or more tools might not be available for your training environment please reach out to your instructor for assistance on how to proceed, post an [issue in our repository](https://github.com/dfds/dojo/issues) or fix it yourself and update the kata via a [pull request](https://github.com/dfds/dojo/pulls).

### Prerequisites
* [.NET Core](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)

## Exercise
This exercise will focus on using .NET Core to host a simple WebSocket endpoint that can echo messages back to the sender in order to verify that a full-duplex connection has succesfully been established between the client and the server.

### 1. Create a kata directory
First we setup a directory for our exercise files. It's pretty straight forward:

```
mkdir kata1
cd kata1
```

### 2. Create a empty ASP.NET Core project
Then we create a empty ASP.NET Core boilerplate project using the dotnet CLI:

```
dotnet new web
```

Just to explain: <br/>
`dotnet` - is the dotnet CLI <br/>
`new` - instructs the dotnet CLI to create a new project in the current directory.<br/>
`web` - tells the dotnet CLI which project template to use


### 3. Update Program.cs to enable WebSocket support
Once the boilerplate is generated we can begin modifying the Program.cs to enable the ASP.NET Core WebSocket feature:

```
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var app = builder.Build();

var webSocketOptions = new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromMinutes(2)
};

app.UseWebSockets(webSocketOptions);

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();

app.Run();

```

Just to explain: <br/>
`WebSocketOptions` - used to configure our WebSocket endpoint. It supports two options: KeepAliveInterval (ping-interval) & AllowedOrigins (CORS). <br/>
`app.UseWebSockets(webSocketOptions);` - enables the .NET Core WebSocket middleware. <br/>


### 4. Implement simple "Echo" middleware
Lastly we need to add middleware that can echo messages from our WS clients. Its important to keep in mind that when using a WebSocket you must keep the middleware pipeline running for the duration of the connection. While this might all sounds very confusing, thanks to recent improvements in ASP.NET Core its actually quiet simple to accomplish by appending the new "Echo" middleware via a few lambdas to the end of our `Program.cs` as follows:

```
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/ws")
    {
        if (context.WebSockets.IsWebSocketRequest)
        {
            using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
            await Echo(webSocket);
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }
    else
    {
        await next(context);
    }

    private static async Task Echo(WebSocket webSocket)
    {
        var buffer = new byte[1024 * 4];
        var receiveResult = await webSocket.ReceiveAsync(
            new ArraySegment<byte>(buffer), CancellationToken.None);

        while (!receiveResult.CloseStatus.HasValue)
        {
            await webSocket.SendAsync(
                new ArraySegment<byte>(buffer, 0, receiveResult.Count),
                receiveResult.MessageType,
                receiveResult.EndOfMessage,
                CancellationToken.None);

            receiveResult = await webSocket.ReceiveAsync(
                new ArraySegment<byte>(buffer), CancellationToken.None);
        }

        await webSocket.CloseAsync(
            receiveResult.CloseStatus.Value,
            receiveResult.CloseStatusDescription,
            CancellationToken.None);
    }
});
```

Just to explain: <br/>
`app.Use(async (context, next)` - add a new middleware lambda to the HTTP pipeline in .NET Core<br/>
`using var webSocket = await context.WebSockets.AcceptWebSocketAsync()` - enables async communication with WS clients. <br/>
`await Echo(webSocket)` - receives a message and immediately sends back the same message to the client. Messages are sent and received in a loop until the client closes the connection.<br/>

## Want to help make our training material better?
 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/dojo/issues).
 