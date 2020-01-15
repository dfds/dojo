DFDS Kubernetes Training - Code kata #1
======================================

## Getting started

These instructions will help you prepare for the code kata and make sure that your local developer machine has the required tools installed and configured.


### Prerequisites

* [Kubernetes](https://kubernetes.io/docs/tasks/tools/install-kubectl/)
* [Powershell Core](https://docs.microsoft.com/en-us/powershell/scripting/install/installing-powershell?view=powershell-6)


## Exercise

In this exercise we will be creating a simple deployment for a containerized Web Api that is pulled from DockerHub @ toban/k8s-training-api:latest

### 1. Create your project directory
`mkdir /kata1`<br/>
`cd /kata1`

### 2. Create a Kubernetes "app descriptor"
Create a file named "api_deployment.yml" and add a Deployment object to host our Web Api container and a Service object to expose the Api:

```
apiVersion: apps/v1
kind: Deployment
---
apiVersion: v1
kind: Service
```

Comment: The "---" is a logical seperator that help kubectl figure out where a object configuration begins and when it ends.

## 3. Configure API Deployment
Augment the Deployment object configuration with the following markup to finish the definition for our very first pod!

```
metadata:
  name: api-backend
spec:
  replicas: 1
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

## 4. Configure API Service
Augment the Service configuration with the following markup to expose our pod to the world!

```
metadata:
  name: api-backend
spec:
  selector:
    app: api
    tier: backend
  ports:
    - port: 80
      protocol: TCP
      targetPort: http
```

## 5. Use "app descriptor" to create a new Kubernetes Deployment
`kubectl create -f .\api_deployment.yml`

Just to explain: <br/>
`kubectl create` - Create a resource from a file or from stdin. <br/>
`-f .\api_deployment.yml` -f point to the filename, directory or URL containing our "app descriptor" files.

## 6. Verify that service is up and running
`kubectl get svc`

Just to explain: <br/>
`kubectl get` - Instructs the Kubernetes CLI to fetch one or more resources via the API Server. <br/>
`svc` - Indicates that we want to list service objects.