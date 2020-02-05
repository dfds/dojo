DFDS Kubernetes Training - Code kata #7
======================================

## Getting started

These instructions will help you prepare for the code kata and make sure that your local developer machine has the following tools installed and configured:

### Prerequisites

* [Kubernetes](https://kubernetes.io/docs/tasks/tools/install-kubectl/)
* [Powershell Core](https://docs.microsoft.com/en-us/powershell/scripting/install/installing-powershell?view=powershell-6)

## Exercise

In this exercise we will focus on how k8s probes can be leveraged to ensure that your teams application abide by the high-observability principles.

### 1. Create a kata directory for our exercise files
`mkdir /kata7`<br/>
`cd /kata7`

### 2. Create a k8s manifest for liveness probe
Create a file named "liveness-probe.yml". We will be using the node500 image from magalix which contains a simple NodeJS web application that responds with a 500 to any HTTP request which will trigger our liveness-probe to restart the pod:

```
apiVersion: v1
kind: Pod
metadata:
 name: node500
spec:
 containers:
   - image: magalix/node500
     name: node500
     ports:
       - containerPort: 3000
         protocol: TCP
     livenessProbe:
       httpGet:
         path: /
         port: 3000
       initialDelaySeconds: 5
```

### 3. Apply the liveness probe configuration to the cluster
`kubectl apply -f liveness-probe.yml`

### 4. Wait for a few seconds then investigate what’s happening inside the pod
`kubectl describe pods node500`

### 5. Create a k8s manifest for our readiness probe
Readiness Probes do the same kind of checks as the Liveness Probes (GET requests, TCP connections, and command executions). However, corrective action differs. Instead of restarting the failing container, it temporarily isolates it from incoming traffic. Let's give it a try by creating a manifest file called "readiness-probe.yml" containing the following markup:

```
apiVersion: v1
kind: Pod
metadata:
 name: nodedelayed
spec:
 containers:
   - image: afakharany/node_delayed
     name: nodedelayed
     ports:
       - containerPort: 3000
         protocol: TCP
     readinessProbe:
       httpGet:
         path: /
         port: 3000
       timeoutSeconds: 2
```     

### 6. Apply the readiness probe configuration to the cluster
kubectl apply -f readiness-probe.yml

### 7. Give it a few seconds then let’s see how the readiness probe worked
kubectl describe pods nodedelayed

## Want to help make our training material better?

 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/roadmap/issues).
 