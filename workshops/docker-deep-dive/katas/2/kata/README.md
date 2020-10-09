DFDS Docker Training - Code kata #2
======================================

This training exercise is a **beginner-level** course on Docker that serves as a starting point for Developers looking to onboard the container efforts at DFDS.

## Getting started
These instructions will help you prepare for the kata and ensure that your training machine has the tools installed you will need to complete the assignment(s). If you find yourself in a situation where one of more tools might not be available for your training environment please reach out to your instructor for assistance on how to proceed, post an [issue in our repository](https://github.com/dfds/dojo/issues) or fix it yourself and update the kata via a [pull request](https://github.com/dfds/dojo/pulls).

### Prerequisites
* [Docker](https://www.docker.com/get-started)

## Exercise
Your second assignment will see you build a container containing a .Net Core WebApi with a simple HelloWorldController that returns the value "Hello, World!".

### 1. Create a kata directory
First we setup a directory for our exercise files. It's pretty straight forward:

```
mkdir kata2
cd kata2
```

### 2. Create a simple Api controller
It's pretty simple: create a file named `HelloWorldController.cs` containing the following code:

***Note*** <br/>
You can use `vi` to edit the file: <br/>
`vi HelloWorldController.cs` will create the file and open the editor.

```
using Microsoft.AspNetCore.Mvc;

namespace HelloUniverse
{
    [Route("api/[controller]")]
    public class HelloWorldController : Controller
    {
        // GET api/HelloWorld
        [HttpGet]
        public string Get()
        {
            return "Hello, world!";
        }
    }
}
```

## 3. Create a simple Dockerfile
Create a file called `Dockerfile` and add the following content:

```
FROM toban/docker-training-base:latest

# Set environment variables
ENV ASPNETCORE_URLS=http://+:5000

#Expose required ports
EXPOSE 5000

#Set container working dir
WORKDIR /app

#Create new dotnet webapi scaffold
RUN dotnet new webapi

#Remove launchSettings.json to avoid collision with localhost usage in scaffold code
RUN rm /app/Properties/launchSettings.json

#Copy HelloWorldController.cs to container file system
COPY ./HelloWorldController.cs /app/Controllers

#Restore required dependencies
RUN dotnet restore

#Build binaries
RUN dotnet build

#Start webapi
ENTRYPOINT ["dotnet", "run"]
```

## 4. Build the image
`docker build -t docker-training-webapi .`

## 5. Run your container
`docker run -d --name kata2 docker-training-webapi`

Just to explain: <br/>
`docker run` - starts an image <br/>
`-d` - runs the container in the background (STDIN closed). Use `-i` instead if you want to keep STDIN open.<br/>
`--name kata2` - give the name `kata2` to the running container. If no name is provided you can still referee the container by ID<br/>
`docker-training-webapi` - it's the image name

## 6. List running containers
`docker ps` <br />
Use `-a` to include "stopped" containers in the list.

## 7. See the logs of your container
`docker logs kata2`

If you want to follow the logs: <br/>
`docker logs kata2 -f`

## 8. Send a command to the container
`docker exec -i kata2 ip addr show`

## 9. Stop the container (if you want)
`docker stop kata2`

To restart it: <br/>
`docker start kata2`

## 10. Remove the container
`docker rm kata2 -f`

## Want to help make our training material better?
 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/dojo/issues).
 