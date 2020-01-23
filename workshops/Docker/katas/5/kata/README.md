DFDS Docker Training - Code kata #4
======================================

This training exercise is a **beginner-level** course on Docker that serves as a starting point for Developers looking to onboard the container efforts at DFDS. 


## Getting started

These instructions will help you prepare for the code kata and make sure that your local developer machine has the following tools installed and configured:


### Prerequisites

* [Docker](https://www.docker.com/get-started)
* [Powershell Core](https://docs.microsoft.com/en-us/powershell/scripting/install/installing-powershell?view=powershell-6)


## Exercise

The fourth exercise will help familiarize your with the networking commands in the Docker CLI.

### 1. Create your project directory
`mkdir /kata4`<br/>
`cd /kata4`

### 2. Create a simple powershell script
It's pretty simple: create a file named `useDefaultBridgeNetwork.ps1` containing the following code:

***Note*** <br/>
You can use `vi` to edit the file: <br/>
`vi useDefaultBridgeNetwork.ps1` will create the file and open the editor.

```
#Start two container instances
docker run -dit --name alpine1 alpine ash
docker run -dit --name alpine2 alpine ash

#Verify container network interface
docker exec -i alpine1 ip addr show
docker exec -i alpine2 ip addr show

#Ping alpine2 from within alpine1 based on name (this will fail)
docker exec -i alpine1 ping -c 2 alpine2

#Ping alpine2 from within alpine1 based on IP (this will succeed)
docker exec -i alpine1 ping -c 2 172.17.0.3

#Verify that alpine1 can reach the internet
docker exec -i alpine1 ping -c 2 google.com

#Delete containers
docker container stop alpine1 alpine2
docker container rm alpine1 alpine2
```

## 3. Run the powershell script and review the output

`pwsh .\useDefaultBridgeNetwork.ps1`


### 4. Create a simple powershell script
It's pretty simple: create a file named `useCustomBridgeNetwork.ps1` containing the following code:

***Note*** <br/>
You can use `vi` to edit the file: <br/>
`vi useDefaultBridgeNetwork.ps1` will create the file and open the editor.

```
#Create the alpine-net network.
docker network create --driver bridge alpine-net

#Create four instance of alpine (notice one is not connected to the alpine-net network)
docker run -dit --name alpine1 --network alpine-net alpine ash
docker run -dit --name alpine2 --network alpine-net alpine ash
docker run -dit --name alpine3 alpine ash
docker run -dit --name alpine4 --network alpine-net alpine ash

#Connect container #4 to default network
docker network connect bridge alpine4

#Inspect default network to ensure #3 + #4 is connected
docker network inspect bridge

#Inspect alpine-net to ensure #1 + #2 + #4 is connected
docker network inspect alpine-net

#Verify that alpine1 can ping containers in alpine-net
docker exec -i alpine1 ping -c 2 alpine2
docker exec -i alpine1 ping -c 2 alpine4

#Verify that alpine4 can ping containers in alpine-net
docker exec -i alpine4 ping -c 2 alpine1
docker exec -i alpine4 ping -c 2 alpine2

#Verify that alpine4 can ping containers in the default bridge network by IP only
docker exec -i alpine4 ping -c alpine3
docker exec -i alpine4 ping -c 2 172.17.0.2

#Verify that alpine4 can reach the internet
docker exec -i alpine4 ping -c 2 google.com

#Clean up
docker container stop alpine1 alpine2 alpine3 alpine4
docker container rm alpine1 alpine2 alpine3 alpine4
docker network rm alpine-net
```

## 5. Run the powershell script and review the output

`pwsh .\useCustomBridgeNetwork.ps1`

## Want to help make our training material better?

 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/roadmap/issues).
 