DFDS Docker Training - Code kata #3
======================================

This training exercise is a **beginner-level** course on Docker that serves as a starting point for Developers looking to onboard the container efforts at DFDS. 

## Getting started
These instructions will help you prepare for the kata and ensure that your training machine has the tools installed you will need to complete the assignment(s). If you find yourself in a situation where one of more tools might not be available for your training environment please reach out to your instructor for assistance on how to proceed, post an [issue in our repository](https://github.com/dfds/dojo/issues) or fix it yourself and update the kata via a [pull request](https://github.com/dfds/dojo/pulls).

### Prerequisites
* [Docker](https://www.docker.com/get-started)

## Exercise
The third exercise will see you build a container containing a AspNetCore MVC application that prints a welcome message obtained from a WebApi listening on the hostname "server" and using the HelloWorldController we created in the previous exercise.

### 1. Create a kata directory
First we setup a directory for our exercise files. It's pretty straight forward:

```
mkdir kata3
cd kata3
```

### 2. Create a simple MVC controller
It's pretty simple: create a file named `HomeController.cs` containing the following code:

***Note*** <br/>
You can use `vi` to edit the file: <br/>
`vi HomeController.cs` will create the file and open the editor.

```
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using app.Models;

namespace app.Controllers
{
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://server:5000/api/HelloWorld"))
                {
                    ViewData["HelloMessage"] = await response.Content.ReadAsStringAsync();
                }
            }
            
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

```

### 3. Create a simple razor view
It's pretty simple: create a file named `Index.cshtml` containing the following code:

***Note*** <br/>
You can use `vi` to edit the file: <br/>
`vi Index.cshtml` will create the file and open the editor.

```
@{
    ViewData["Title"] = "Home Page";
}

<div class="text-center">
    <h1 class="display-4">
        @ViewData["HelloMessage"]
    </h1>
</div>
```

## 4. Create a simple Dockerfile
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
RUN dotnet new mvc

#Remove launchSettings.json to avoid collision with localhost usage in scaffold code
RUN rm /app/Properties/launchSettings.json

#Customize project items.
COPY ./HomeController.cs /app/Controllers
COPY ./Index.cshtml /app/Views/Home

#Restore required dependencies
RUN dotnet restore

#Build binaries
RUN dotnet build

#Start webapi
ENTRYPOINT ["dotnet", "run"]
```

## 4. Build the image
`docker build -t docker-training-mvc .`

## 5. Run your container
`docker run -d --name kata3 docker-training-mvc`

Just to explain: <br/>
`docker run` - starts an image <br/>
`-d` - runs the container in the background (STDIN closed). Use `-i` instead if you want to keep STDIN open.<br/>
`--name kata3` - give the name `kata3` to the running container. If no name is provided you can still referee the container by ID<br/>
`docker-training-mvc` - it's the image name

## 6. List running containers
`docker ps` <br />
Use `-a` to include "stopped" containers in the list.

## 7. See the logs of your container
`docker logs kata3`

If you want to follow the logs: <br/>
`docker logs kata3 -f`

## 8. Send a command to the container
`docker exec -i kata3 ip addr show`

## 9. Stop the container (if you want)
`docker stop kata3`

To restart it: <br/>
`docker start kata3`

## 10. Remove the container
`docker rm kata3`

## Want to help make our training material better?
 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/dojo/issues).
 