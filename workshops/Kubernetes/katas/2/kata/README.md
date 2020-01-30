DFDS Kubernetes Training - Code kata #2
======================================

## Getting started

These instructions will help you prepare for the code kata and make sure that your local developer machine has the following tools installed and configured:

### Prerequisites

* [Kubernetes](https://kubernetes.io/docs/tasks/tools/install-kubectl/)
* [Powershell Core](https://docs.microsoft.com/en-us/powershell/scripting/install/installing-powershell?view=powershell-6)

## Exercise

In this exercise we will be creating a simple pod containing a busybox image to explore some of the inner workings. 

### 1. Create your project directory
`mkdir /kata2`<br/>
`cd /kata2`

### 2. Create a Kubernetes "app descriptor"
Create a file named "my-pod.yml" and add one pod object to hold our busybox container image:

```
apiVersion: v1
kind: Pod
```

## 3. Configure pod metadata and spec
Augment the pod object configuration with the following markup:

```
metadata:
  name: my-pod
  labels:
    app: myapp
spec:
  containers:
  - name: myapp-container
    image: busybox
    command: ['sh', '-c', 'echo Hello Kubernetes! && sleep 3600']
```

## 4. Create a pod from the yaml definition file

```
kubectl create -f my-pod.yml
```

## 5. Edit the pod by updating the yaml definiton with a custom annotation:

```
metadata:
  annotation: foo
```

## 6. Re-applying yaml definiton:

```
kubectl apply -f my-pod.yml
```

## 6. Delete the pod like this:

```
kubectl delete pod my-pod
```

## Want to help make our training material better?

 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/roadmap/issues).
 