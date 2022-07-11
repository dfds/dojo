DFDS Docker Training - Code kata #1
======================================

This training exercise is a **beginner-level** course on Docker that serves as a starting point for Developers looking to onboard the container efforts at DFDS.

## Getting started
These instructions will help you prepare for the kata and ensure that your training machine has the tools installed you will need to complete the assignment(s). If you find yourself in a situation where one or more tools might not be available for your training environment please reach out to your instructor for assistance on how to proceed, post an [issue in our repository](https://github.com/dfds/dojo/issues) or fix it yourself and update the kata via a [pull request](https://github.com/dfds/dojo/pulls).

### Prerequisites
* [Docker](https://www.docker.com/get-started)

## Exercise
Your first assignment will see you build a container whose job is to **print "Hello!" every 10 seconds... forever**. Simple, no? Let's start then!

### 1. Create a kata directory
First we setup a directory for our exercise files. It's pretty straight forward:

```
mkdir kata1
cd kata1
```

### 2. Create a simple powershell script
It's pretty simple: create a file named `hello.ps1` containing the following code:

***Note*** <br/>
You can use `vi` to edit the file: <br/>
`vi hello.ps1` will create the file and open the editor.

```
$val = 1

while($val -ne 10)
{	
	Write-Host "Hello"
        
    $val++
}
```

## 3. Create a simple Dockerfile
Create a file called `Dockerfile` and add the following content:

```
FROM toban/docker-training-base:latest

COPY hello.ps1 .

ENTRYPOINT ["pwsh", "./hello.ps1"]
```

## 4. Build the image
`docker build -t docker-training-hello .`

## 5. Run your container
`docker run -d --name kata1 docker-training-hello`

Just to explain: <br/>
`docker run` - starts an image <br/>
`-d` - runs the container in the background (STDIN closed). Use `-i` instead if you want to keep STDIN open.<br/>
`--name kata1` - give the name `kata1` to the running container. If no name is provided you can still referee the container by ID<br/>
`docker-training-hello` - it's the image name

## 6. List running containers
`docker ps` <br />
Use `-a` to include "stopped" containers in the list.

## 7. See the logs of your container
`docker logs kata1`

If you want to attach to the log output (break out using ctrl + c) <br/>
`docker logs kata1 -f`

## 8. Send a command to the container
`docker exec -i kata1 ip addr show`

## 9. Stop the container (if you want)
`docker stop kata1`

To restart it: <br/>
`docker start kata1`

## 10. Remove the container
`docker rm kata1 -f`

## Want to help make our training material better?
 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/dojo/issues).
 