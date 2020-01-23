DFDS Docker Training - Code kata #8
======================================

This training exercise is a **beginner-level** course on Docker that serves as a starting point for Developers looking to onboard the container efforts at DFDS. 


## Getting started
These instructions will help you prepare for the code kata and make sure that your local developer machine has the following tools installed and configured:


### Prerequisites
* [Docker](https://www.docker.com/get-started)


## Exercise
The eigth exercise will help you get a better understanding of how to debug your docker containers via the CLI or your IDE.

### 1. Create your project directory
`mkdir /kata8`<br/>
`cd /kata8`

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

 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/roadmap/issues).
 