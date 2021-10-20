DFDS Docker Training - Code kata #4
======================================

This training exercise is a **beginner-level** course on Docker that serves as a starting point for Developers looking to onboard the container efforts at DFDS. 

## Getting started
These instructions will help you prepare for the kata and ensure that your training machine has the tools installed you will need to complete the assignment(s). If you find yourself in a situation where one of more tools might not be available for your training environment please reach out to your instructor for assistance on how to proceed, post an [issue in our repository](https://github.com/dfds/dojo/issues) or fix it yourself and update the kata via a [pull request](https://github.com/dfds/dojo/pulls).

### Prerequisites
* [Docker](https://www.docker.com/get-started)

## Exercise
The fourth exercise will help you get a better understanding of docker volumes and prepare you for some of the more advanced katas down the road.

### 1. Create a kata directory
First we setup a directory for our exercise files. It's pretty straight forward:

```
mkdir kata4
cd kata4
```

### 2. Create a simple powershell script
It's pretty simple: create a file named playingWithMounts.ps1 containing the following code:

Note <br/> You can use vi to edit the file: <br/> vi playingWithMounts.ps1 will create the file and open the editor.

```
docker run -d --name docker-training-bind-mount \
    -v /c/temp:/my-mounted-temp-dir \
    docker-training-webapi:latest
```

### 3. Run the powershell script
    pwsh .\playingWithMounts.ps1

### 4. Create a simple powershell script
It's pretty simple: create a file named playingWithVolumes.ps1 containing the following code:

Note <br/> You can use vi to edit the file: <br/> vi playingWithVolumes.ps1 will create the file and open the editor.

```
docker volume create demo_volume
docker run -d --name docker-training-volume \
    -v demo_volume:/my-volume \
    docker-training-webapi:latest
```

### 5. Run the powershell script
pwsh .\playingWithVolumes.ps1

## Want to help make our training material better?
 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/dojo/issues).
 