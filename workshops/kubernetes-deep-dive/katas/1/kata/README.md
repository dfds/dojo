DFDS Kubernetes Training - Code kata #1
======================================

This training exercise is a **beginner-level** course on Kubernetes that serves as a starting point for developers looking get started with container orchestration at DFDS.

## Getting started
These instructions will help you prepare for the kata exercise and ensure that your local machine has the tools installed you will need to complete the assignment(s). If you find yourself in a situation where one of more tools might not be available for your training environment please reach out to your instructor for assistance on how to proceed, post an [issue in our repository](https://github.com/dfds/dojo/issues) or fix it yourself and create a [pull request](https://github.com/dfds/dojo/pulls).

### Prerequisites
* [Kubectl](https://kubernetes.io/docs/tasks/tools/install-kubectl/)
* [Docker](https://www.docker.com/products/docker-desktop)
* [Powershell Core](https://docs.microsoft.com/en-us/powershell/scripting/install/installing-powershell?view=powershell-6)

## Exercise
Your team have just finalized their first container project and pushed it to the DockerHub registry. They are now looking for your help to deploy the containers to a k8s cluster so they can leverage its managed services and focus on building new features for DFDS.

### 1. Create your kata directory
First we setup a directory for our exercise files. It's pretty straight forward:

```
mkdir kata1
cd kata1
```

### 2. Create deployment manifest
Create a file named "solution_deployments.yml" and add two Deployment objects for our api + mvc containers:

```
apiVersion: apps/v1
kind: Deployment
---
apiVersion: apps/v1
kind: Deployment
```

Just to explain: <br/>
`.kind: Deployment` - Specifies a need for the k8s API controller to create an object of kind (type) Deployment. <br/>

### 3. Configure api-backend Deployment
Augment the first Deployment object configuration with the following markup to specify the setup for our api-backend deployment:

```
metadata:
  name: api-deployment
  labels:
    app: api
spec:
  replicas: 2
  selector:
    matchLabels:
      app: api
      tier: backend
      track: stable
  template:
    metadata:
      labels:
        app: api
        tier: backend
        track: stable
    spec:
      containers:
      - name: api-backend
        image: toban/k8s-training-api:latest
        imagePullPolicy: IfNotPresent
        ports:
          - name: http
            containerPort: 5000
      restartPolicy: Always
```

Just to explain: <br/>
`.metadata.name: api-backend` - Instructs k8s to name our deployment "api-backend" so we can reference it via kubectl, from other manifests, etc. <br/>
`.spec: ` - Specifies the blueprint for our deployment. <br/>
`.spec.replicas: ` - Indicates the number of replicas we want k8s to maintain. <br/>
`.spec.template.spec.containers.ports: ` - Specifies the port that k8s uses to proxy container networking.<br/>

### 4. Configure MVC Deployment
Augment the second Deployment object configuration with the following markup to specify the setup for our frontend ASP.NET MVC solution:

```
metadata:
  name: mvc-deployment
  labels:
    app: mvc
spec:
  replicas: 4
  selector:
    matchLabels:
      app: mvc
      tier: frontend
      track: stable
  template:
    metadata:
      labels:
        app: mvc
        tier: frontend
        track: stable
    spec:
      containers:
      - name: mvc-frontend
        image: toban/k8s-training-frontend:latest
        imagePullPolicy: IfNotPresent
        ports:
        - containerPort: 5000
      restartPolicy: Always
```

### 5. Use kubectl to create a two new Kubernetes Deployment from our manifest
`kubectl create -f .\solution_deployments.yml`


Just to explain: <br/>
`kubectl create` - Create a resource from a file or from stdin. <br/>
`-f .\solution_deployments.yml` -f point to the filename, directory or URL containing our "resource manifest" files.


### 6. Verify that Web API deployment is created
`kubectl describe deployment api-deployment`

Just to explain: <br/>
`kubectl describe` - Instructs the Kubernetes CLI to fetch metadata related to one or more resources via the API Server. <br/>
`deployment api-backend` - Indicates that we want to work with the deployment objects named "api-backend"

## 7. Verify that MVC deployment is created
`kubectl describe deployment mvc-deployment`

## Want to help make our training material better?
 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/dojo/issues).