DFDS Docker Training - Code kata #7
======================================

This training exercise is a **beginner-level** course on Docker that serves as a starting point for Developers looking to onboard the container efforts at DFDS. 


## Getting started
These instructions will help you prepare for the code kata and make sure that your local developer machine has the following tools installed and configured:


### Prerequisites
* [Docker](https://www.docker.com/get-started)


## Exercise
The sixth exercise will see you build your very first compose file that will bring together some of the work from earlier katas and deploy it as a single interconnected unit. 

### 1. Create your project directory
`mkdir kata7`<br/>
`cd kata7`

### 2. Create a docker-compose.yml file to setup your "composition"
It's pretty simple: create a file named `docker-compose.yml` containing the following mark-up:

***Note*** <br/>
version: '3.7'

services:
```

Just to explain: <br/>
`version: '3.7'` - specifies the docker-compose file-format (schema) we want the interpreter to use for validating our "composition" <br/>

### 3. Create a simple configuration for our docker-training-webapi container
It's pretty simple: Add a new container section to our `docker-compose.yml` "composition":

```
version: '3.7'

services:
  docker-training-webapi:
    hostname: webapi
    image: docker-training-webapi
    labels:
      com.example.description: "Training API"
      com.example.department: "IT"
      com.example.label-with-empty-value: ""
    ports:
      - "5001:5000"
```

Just to explain: <br/>
`docker-training-webapi:` - specifies the beginning of a new container configuration we wish to name "docker-training-webapi". <br/>
`hostname:` - instructs the docker-compose parser that we want our container mounted with a hostname called "webapi". This means other containers scoped to the same network will be able to communicate with the container using the assigned hostname. <br/>
'image:` - assigns a image named "docker-training-webapi" from the local docker daemon. If the image is not available from the local daemon, dockerd will try to fetch the image from its upstream parent (Docker Hub by default). 
'ports:` - exposes the containers internal application port (5000) to accept traffic from network-scoped containers on port 5001.

### 4. Create a simple configuration for our docker-training-mvc container
It's pretty simple: Add a new container section to our `docker-compose.yml` services section:

```
version: '3.7'

services:
  docker-training-webapi:
    hostname: webapi
    image: docker-training-webapi
    labels:
      com.example.description: "Training API"
      com.example.department: "IT"
      com.example.label-with-empty-value: ""
    ports:
      - "5001:5000"
  docker-training-mvc:
    hostname: mvc
    image: docker-training-mvc
    labels:
      com.example.description: "Training MVC"
      com.example.department: "IT"
      com.example.label-with-empty-value: ""
    ports:
      - "5002:5000"
```

Just to explain: <br/>
`docker-training-mvc:` - specifies the beginning of a new container configuration we wish to name "docker-training-mvc". <br/>
`hostname:` - instructs the docker-compose parser that we want our container mounted with a hostname called "mvc". This means other containers scoped to the same network will be able to communicate with the container using the assigned hostname. <br/>
'image:` - assigns a image named "docker-training-mvc" from the local docker daemon. If the image is not available from the local daemon, dockerd will try to fetch the image from its upstream parent (Docker Hub by default). 
'ports:` - exposes the containers internal application port (5000) to accept traffic from network-scoped containers on port 5002.

### 5. Setup a docker event listener so you can keep track of what is happening under the hood when we build our "composition"
It's pretty simple: Open up a new console and run the following command:

`docker events`<br/>

Just to explain: <br/>
`docker events` - instructs the docker CLI to emit all events processed by the local docker daemon to the console. This will enable us to keep track of what is happening when we use docker-compose.<br/>

### 6. Build your new docker-compose "composition" using the CLI
It's pretty simple: Open up a new console, navigate to the folder containing your docker-compose "composition" and run the following command:

`docker-compose up --build`<br/>

Just to explain: <br/>
`docker-compose up --build:` - instructs the docker-compose CLI to parse & run the contents of docker-compose.yaml in the current directory. The --build flag instructs the CLI to rebuild the entire "composition" before mounting the containers defined within the services section which can be very useful when authoring new "compositions".<br/>

### 7. Teardown your docker-compose "composition" using the CLI
It's pretty simple: Open up a new console, navigate to the folder containing your docker-compose "composition" and run the following command:

`docker-compose down`<br/>

Just to explain: <br/>
`docker-compose down:` - instructs the docker-compose CLI to parse & destroy all resources created based on thecontents of the docker-compose.yaml in the current directory.

## Want to help make our training material better?

 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/ded-dojo/issues).
 