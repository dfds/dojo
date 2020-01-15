DFDS Docker Training - Code kata #4
======================================

This training exercise is a **beginner-level** course on Docker that serves as a starting point for Developers looking to onboard the container efforts at DFDS. 


## Getting started
These instructions will help you prepare for the code kata and make sure that your local developer machine has the following tools installed and configured:


### Prerequisites
* [Docker](https://www.docker.com/get-started)


## Exercise
The fourth exercise will help you get a better understanding of docker volumes and prepare you for some of the more advanced katas down the road.

### 1. Create your project directory
`mkdir /kata4`<br/>
`cd /kata4`

### 2. Create a simple powershell script
It's pretty simple: create a file named playingWithMounts.ps1 containing the following code:

Note <br/> You can use vi to edit the file: <br/> vi playingWithMounts.ps1 will create the file and open the editor.

docker run -d --name docker-training-webapi \
    -v /path/to/app/directory:/app \
    docker-training-webapi:latest

### 3. Run the powershell script
    pwsh .\playingWithMounts.ps1

### 4. Create a simple powershell script
It's pretty simple: create a file named playingWithVolumes.ps1 containing the following code:

Note <br/> You can use vi to edit the file: <br/> vi playingWithVolumes.ps1 will create the file and open the editor.

docker rm docker-training-webapi
docker volume create demo_volume
docker run -d --name docker-training-webapi \
    -v demo_volume:/app \
    docker-training-webapi:latest

### 5. Run the powershell script
pwsh .\playingWithVolumes.ps1

## Want to help make our training material better?

 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/roadmap/issues).
 