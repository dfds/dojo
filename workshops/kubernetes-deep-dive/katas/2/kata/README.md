DFDS Kubernetes Training - Code kata #2
======================================

This training exercise is a **beginner-level** course on Kubernetes that serves as a starting point for developers looking get started with container orchestration at DFDS.

## Getting started
These instructions will help you prepare for the kata exercise and ensure that your local machine has the tools installed you will need to complete the assignment(s). If you find yourself in a situation where one or more tools might not be available for your training environment please reach out to your instructor for assistance on how to proceed, post an [issue in our repository](https://github.com/dfds/dojo/issues) or fix it yourself and create a [pull request](https://github.com/dfds/dojo/pulls).


### Prerequisites
* [Kubectl](https://kubernetes.io/docs/tasks/tools/install-kubectl/)
* [Docker](https://www.docker.com/products/docker-desktop)
* [Powershell Core](https://docs.microsoft.com/en-us/powershell/scripting/install/installing-powershell?view=powershell-6)

## Exercise
Before we move on to expose our application to the world we need to take a quick stop to examine some of the basic mechanics of Kubernetes pods to better prepare us for the road ahead.

### 1. Create your kata directory
First we setup a directory for our exercise files. It's pretty straight forward:

```
mkdir kata2
cd kata2
```

### 2. Configure pod metadata and spec
Augment the pod object configuration with the following markup:

```
apiVersion: v1
kind: Pod
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

Just to explain: <br/>
`.kind: Pod` - Specifies a need to create an object of kind (type) Pod. <br/>
`.metadata.name: my-pod` - Instructs k8s to name our pod "my-pod" so we can reference it via kubectl, from other manifests, etc. <br/>
`.metadata.labels: ` - Labels are intended to be used to specify identifying attributes of objects that are meaningful and relevant to users, but do not directly imply semantics to the core system. Labels can be used to organize and to select subsets of objects.<br/>

### 3. Create a pod from out manifest file
`kubectl create -f my-pod.yml`

### 4. Inspect the pod configuration. Notice how k8s has added ALOT of meta data to ensure it can maintain our desired state
`kubectl describe pod my-pod`

### 5. Edit the pod by updating the yaml file with a custom annotation (be mindful of the indendation when you add this)
```
metadata:
  annotations: 
    mykey: "my-annotation"
```

### 6. Re-applying manifest to update existing pod
`kubectl apply -f my-pod.yml`

### 7. Inspect the pod configuration to verify that the new annotation has been applied
`kubectl describe pod my-pod`


### 8. Delete the pod once your done
`kubectl delete pod my-pod`

## Want to help make our training material better?
 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/dojo/issues).