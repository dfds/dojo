DFDS Kubernetes Training - Code kata #2
======================================

## Getting started

These instructions will help you prepare for the code kata and make sure that your local developer machine has the following tools installed and configured:

### Prerequisites

* [Kubernetes](https://kubernetes.io/docs/tasks/tools/install-kubectl/)
* [Powershell Core](https://docs.microsoft.com/en-us/powershell/scripting/install/installing-powershell?view=powershell-6)

## Exercise

Before we move on to expose our deployments to the world we will first take a quick stop to examine some of the basic mechanics of Kubernetes pods to better prepare us for the road ahead.

### 1. Create your project directory
`mkdir /kata2`<br/>
`cd /kata2`

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

### 4. Edit the pod by updating the yaml file with a custom annotation:

```
metadata:
  annotation: my-annotation
```

### 5. Re-applying manifest to update existing pod:
`kubectl apply -f my-pod.yml`

### 6. Delete the pod once your done:
`kubectl delete pod my-pod`

## Want to help make our training material better?

 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/roadmap/issues).