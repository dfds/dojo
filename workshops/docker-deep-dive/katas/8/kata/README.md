DFDS Docker Training - Code kata #8
======================================

This training exercise is a **beginner-level** course on Docker that serves as a starting point for Developers looking to onboard the container efforts at DFDS. 

## Getting started
These instructions will help you prepare for the kata and ensure that your training machine has the tools installed you will need to complete the assignment(s). If you find yourself in a situation where one of more tools might not be available for your training environment please reach out to your instructor for assistance on how to proceed, post an [issue in our repository](https://github.com/dfds/dojo/issues) or fix it yourself and update the kata via a [pull request](https://github.com/dfds/dojo/pulls).

### Prerequisites
* [Docker](https://www.docker.com/get-started)

## Exercise
The eigth exercise will help you get a better understanding of how to debug your docker containers via the CLI or your IDE.

### 1. Create your kata directory
First we setup a directory for our exercise files. It's pretty straight forward:

```
mkdir kata8
cd kata8
```

### 2. Create a simple powershell script
It's pretty simple: create a file named debuggingViaCLI.ps1 containing the following code:

Note <br/> You can use vi to edit the file: <br/> vi debuggingViaCLI.ps1 will create the file and open the editor.

#Specify container id of the debug target
$ContainerId = ""

Start-Process docker inspect $ContainerId
Start-Process docker logs $ContainerId
Start-Process docker events
Start-Process docker exec $ContainerId pwsh

### 3. Run the powershell script
pwsh .\debuggingViaCLI.ps1

## Want to help make our training material better?
 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/dojo/issues).
 