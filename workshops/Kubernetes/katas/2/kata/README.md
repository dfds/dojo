DFDS Kubernetes Training - Code kata #2
======================================

## Getting started

These instructions will help you prepare for the code kata and make sure that your local developer machine has the following tools installed and configured:

### Prerequisites

* [Kubernetes](https://kubernetes.io/docs/tasks/tools/install-kubectl/)
* [Powershell Core](https://docs.microsoft.com/en-us/powershell/scripting/install/installing-powershell?view=powershell-6)

## Exercise

Before we move on to expose our deployments to the world we will first take a quick stop to examine some of the basic mechanics of Kubernetes pods as we will need this knowledge as we move forward with the workshop:

### 1. Create your project directory
`mkdir /kata2`<br/>
`cd /kata2`

### 2. Create a k8s manifest for our pod
Create a file named "my-pod.yml" and add a pod primative definition to hold our busybox container image:

```
apiVersion: v1
kind: Pod
```

Just to explain: <br/>
`.kind: Pod` - Specifies a need to create an object of kind (type) Pod. <br/>

### 3. Configure pod metadata and spec
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

Just to explain: <br/>
`.metadata.name: my-pod` - Instructs k8s to name our pod "my-pod" so we can reference it via kubectl, from other manifests, etc. <br/>
`.metadata.labels: ` - Labels are intended to be used to specify identifying attributes of objects that are meaningful and relevant to users, but do not directly imply semantics to the core system. Labels can be used to organize and to select subsets of objects.<br/>

### 4. Create a pod from out manifest file
`kubectl create -f my-pod.yml`

### 5. Edit the pod by updating the yaml file with a custom annotation:

```
metadata:
  annotation: my-annotation
```

### 6. Re-applying manifest to update existing pod:
`kubectl apply -f my-pod.yml`

### 7. Delete the pod once your done:
`kubectl delete pod my-pod`

## Want to help make our training material better?

 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/roadmap/issues).
 