DFDS Kubernetes Training - Code kata #1
======================================

## Getting started

These instructions will help you prepare for the code kata and make sure that your local developer machine has the following tools installed and configured:

### Prerequisites

* [Kubernetes](https://kubernetes.io/docs/tasks/tools/install-kubectl/)
* [Powershell Core](https://docs.microsoft.com/en-us/powershell/scripting/install/installing-powershell?view=powershell-6)

## Exercise

Your team have just finalized their first container project which they have deployed to DockerHub. They are now looking for your help to deploy the containers to the Hellman cluster so they can leverage that managed k8s services and focus on building new features for our company:

### 1. Create your project directory
`mkdir /kata1`<br/>
`cd /kata1`

### 2. Create deployment manifest
Create a file named "api_mvc_deployment.yml" and add two Deployment objects for our Web Api + MVC containers:

```
apiVersion: apps/v1
kind: Deployment
---
apiVersion: apps/v1
kind: Deployment
```

Comment: The "---" is a logical seperator that help kubectl figure out where a object configuration begins and when it ends.

### 3. Configure API Deployment
Augment the first Deployment object configuration with the following markup to specify the setup for our Web API:

```
metadata:
  name: api-backend
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
`kubectl create -f .\api_mvc_deployment.yml`

Just to explain: <br/>
`kubectl create` - Create a resource from a file or from stdin. <br/>
`-f .\api_mvc_deployment.yml` -f point to the filename, directory or URL containing our "resource manifest" files.

### 6. Verify that Web API deployment is created
`kubectl describe deployment api-backend`

Just to explain: <br/>
`kubectl describe` - Instructs the Kubernetes CLI to fetch metadata related to one or more resources via the API Server. <br/>
`deployment api-backend` - Indicates that we want to work with the deployment objects named "api-backend"

## 7. Verify that MVC deployment is created
`kubectl describe deployment mvc-frontend`

Just to explain: <br/>
`kubectl describe` - Instructs the Kubernetes CLI to fetch metadata about one or more resources via the API Server. <br/>
`deployment mvc-frontend` - Indicates that we want to work with the deployment objects named "api-backend"