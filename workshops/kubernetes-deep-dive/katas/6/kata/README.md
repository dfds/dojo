DFDS Kubernetes Training - Code kata #6
======================================

This training exercise is a **beginner-level** course on Kubernetes that serves as a starting point for developers looking get started with container orchestration at DFDS.

## Getting started
These instructions will help you prepare for the kata exercise and ensure that your local machine has the tools installed you will need to complete the assignment(s). If you find yourself in a situation where one or more tools might not be available for your training environment please reach out to your instructor for assistance on how to proceed, post an [issue in our repository](https://github.com/dfds/dojo/issues) or fix it yourself and create a [pull request](https://github.com/dfds/dojo/pulls).

### Prerequisites
* [Kubectl](https://kubernetes.io/docs/tasks/tools/install-kubectl/)
* [Docker](https://www.docker.com/products/docker-desktop)
* [Powershell Core](https://docs.microsoft.com/en-us/powershell/scripting/install/installing-powershell?view=powershell-6)

## Exercise
Secrets.

### 1. Create your kata directory
First we setup a directory for our exercise files. It's pretty straight forward:

```
mkdir kata6
cd kata6
```

### 2. Create a NetworkPolicy descriptor file to allocate some storage in our cluster
Create a file named "my-secret.yml" and add a Secret definition:

```
apiVersion: v1
kind: Secret
metadata:
  name: my-secret
stringData:
  myKey: myPassword
```

### 3. Use kubectl to apply our new secret
`kubectl apply -f my-secret.yml`

### 4. Verify that secret is created.
`kubectl get secrets`

### 5. Create a sample pod to consume the secret
Create a file named "my-secret-pod.yml" and add a Secret definition:

```
apiVersion: v1
kind: Pod
metadata:
  name: my-secret-pod
spec:
  containers:
  - name: myapp-container
    image: busybox
    command: ['sh', '-c', "echo Hello, Kubernetes here is your secret: $MY_PASSWORD! && sleep 3600"]
    env:
    - name: MY_PASSWORD
      valueFrom:
        secretKeyRef:
          name: my-secret
          key: myKey
```

### 6. Use kubectl to apply our new secret
`kubectl apply -f my-secret-pod.yml`

### 7. Print logs from my-secrets-pod busybox container
`kubectl logs my-secret-pod`

## Want to help make our training material better?
 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/dojo/issues). 