DFDS Kubernetes Training - Code kata #2
======================================

## Getting started

These instructions will help you prepare for the code kata and make sure that your local developer machine has the following tools installed and configured:

### Prerequisites

* [Kubernetes](https://kubernetes.io/docs/tasks/tools/install-kubectl/)
* [Powershell Core](https://docs.microsoft.com/en-us/powershell/scripting/install/installing-powershell?view=powershell-6)

## Exercise

In this exercise we will be creating a simple deployment for a containerized Web Api that is pulled from DockerHub @ toban/k8s-training-api:latest

### 1. Create your project directory
`mkdir /kata2`<br/>
`cd /kata2`

### 2. Create a Kubernetes "app descriptor"
Create a file named "api_mvc_deployment.yml" and add two Deployment objects for our Web Api + MVC containers and two Service objects to expose the containers to the world:

```
apiVersion: apps/v1
kind: Deployment
---
apiVersion: apps/v1
kind: Deployment
---
apiVersion: v1
kind: Service
---
apiVersion: v1
kind: Service
```

Comment: The "---" is a logical seperator that help kubectl figure out where a object configuration begins and when it ends.

## 3. Configure API Deployment
Augment the first Deployment object configuration with the following markup:

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

## 4. Configure MVC Deployment
Augment the second Deployment object configuration with the following markup:

```
metadata:
  name: mvc-deployment
  labels:
    app: mvc
spec:
  replicas: 1
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

## 5. Configure API Service
Augment the first Service object configuration with the following markup to expose our API to the world.

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

## 6. Configure MVC Service
Augment the second Service object configuration with the following markup to expose our MVC app to the world.

```
metadata:
  name: mvc-frontend
spec:
  selector:
    app: mvc
    tier: frontend
  ports:
    - protocol: "TCP"
      port: 80
      targetPort: 80
  type: LoadBalancer
```

## 7. Use "app descriptor" to create a new Kubernetes Deployment
`kubectl create -f .\api_mvc_deployment.yml`

Just to explain: <br/>
`kubectl create` - Create a resource from a file or from stdin. <br/>
`-f .\api_deployment.yml` -f point to the filename, directory or URL containing our "resource manifest" files.

## 8. Verify that API deployment is created
`kubectl describe deployment api-backend`

Just to explain: <br/>
`kubectl describe` - Instructs the Kubernetes CLI to fetch metadata related to one or more resources via the API Server. <br/>
`deployment api-backend` - Indicates that we want to work with the deployment objects named "api-backend"

## 9. Verify that MVC deployment is created
`kubectl describe deployment mvc-frontend`

Just to explain: <br/>
`kubectl describe` - Instructs the Kubernetes CLI to fetch metadata about one or more resources via the API Server. <br/>
`deployment mvc-frontend` - Indicates that we want to work with the deployment objects named "api-backend"

## 10. Watch the frontend service until External IP is assigned
`kubectl get service mvc-frontend --watch`

Just to explain: <br/>
`kubectl get` - Instructs the Kubernetes CLI to fetch one or more resources via the API Server. <br/>
`service mvc-frontend` - Indicates that we want to work with the service object named "mvc-frontend"