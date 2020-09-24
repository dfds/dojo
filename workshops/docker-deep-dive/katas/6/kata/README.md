DFDS Docker Training - Code kata #6
======================================

This training exercise is a **beginner-level** course on Docker that serves as a starting point for Developers looking to onboard the container efforts at DFDS. 


## Getting started

These instructions will help you prepare for the code kata and make sure that your local developer machine has the following tools installed and configured:


### Prerequisites

* [Docker](https://www.docker.com/get-started)
* [Powershell Core](https://docs.microsoft.com/en-us/powershell/scripting/install/installing-powershell?view=powershell-6)


## Exercise

The fifth exercise will help familiarize your with the networking commands in the Docker CLI.

### 1. Create your project directory
`mkdir /kata6`<br/>
`cd /kata6`

### 2. Create a simple powershell script
It's pretty simple: create a file named `useMACVLANNetwork.ps1` containing the following code:

***Note*** <br/>
You can use `vi` to edit the file: <br/>
`vi useMACVLANNetwork.ps1` will create the file and open the editor.

```
#Create macvlan (parent=eth0.10 (trunked subinterface) parent=eth0 (direct interface)
docker network create -d macvlan --subnet=172.16.86.0/24 --gateway=172.16.86.1 -o parent=eth0 alpine-net

#Create container
docker run --rm -itd --network alpine-net --name my-macvlan-alpine alpine:latest ash

#Check container network interface
docker exec my-macvlan-alpine ip addr show eth0
docker exec my-macvlan-alpine ip route

#Clean up
docker container stop my-macvlan-alpine
docker network rm alpine-net
```

## 3. Run the powershell script and review the output

`pwsh .\useMACVLANNetwork.ps1`

### 4. Create a simple powershell script
It's pretty simple: create a file named `useIPVLANNetwork.ps1` containing the following code:

***Note*** <br/>
You can use `vi` to edit the file: <br/>
`vi useIPVLANNetwork.ps1` will create the file and open the editor.

```
#Create ipvlan (this requires us to enable "experimental features" in daemon.json or via a call to dockerd --experimental)
docker network create -d ipvlan --subnet=192.168.210.0/24 --subnet=192.168.212.0/24 --gateway=192.168.210.254 --gateway=192.168.212.254 -o ipvlan_mode=l2 alpine-net

#Create container
docker run --rm -itd --network alpine-net --name my-ipvlan-alpine alpine:latest ash

#Check container network interface
docker exec my-ipvlan-alpine ip addr show eth0
docker exec my-ipvlan-alpine ip route

#Clean up
docker container stop my-ipvlan-alpine
docker network rm alpine-net
```

## 5. Run the powershell script and review the output

`pwsh .\useIPVLANNetwork.ps1`

## Want to help make our training material better?

 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/roadmap/issues).
 