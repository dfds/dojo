DFDS Crossplane Developer Deepdive - Code kata #1
======================================

This training exercise is a **beginner-level** course on Crossplane that serves as a starting point for Developers looking to onboard the provisioning efforts at DFDS.

## Getting started
These instructions will help you prepare for the kata and ensure that your training machine has the tools installed you will need to complete the assignment(s). If you find yourself in a situation where one of more tools might not be available for your training environment please reach out to your instructor for assistance on how to proceed, post an [issue in our repository](https://github.com/dfds/dojo/issues) or fix it yourself and update the kata via a [pull request](https://github.com/dfds/dojo/pulls).

### Prerequisites
* Linux/Unix development enviornment. For Windows users: Windows Subsystem for Linux (WSL) v2, installed and configured in your machine (https://docs.microsoft.com/en-us/windows/wsl/install) and install Ubuntu distribution
* Docker
* Kubernetes Cluster. This could be [Kubernetes feature enabled from within Docker for Desktop](https://docs.docker.com/desktop/kubernetes/) or one of these: Kind, Minikube, DFDS sandbox etc.)
* Kubectl
* Helm 3

## Exercise
Your first assignment will see you preapre your environment to work with Crossplane. After this excersice you will have Crossplane installed into your Kubernetes Cluster and the Crossplane CLI onto your development machine

### 1. Ensure prerequsites:

Check access to your Kubenetes cluster
Run the following script:
```
kubectl cluster-info
```
This should return endpoint information for like Kubernetes control plane

Ensure helm installed:
```
helm version
```

### 2. Run install_crossplane.sh
```
install_crossplane.sh <AWS_KEY_ACCESS_ID> <AWS_SECRET_ACCESS_KEY>
```

This will install the crosspane in Kubernetes and creates 
 these namespaces: 
 * crossplane-system for crossplane setup
 * and my-app, for your deployments

Other crossplane resources that are created:
- Secret that keeps the AWS key access and secret that you have provided, and
- a ProviderConfig that uses the secret and enables K8s to provision resources in the AWS account

### 3. Control resources 
List namespaces:
```
kubectl get ns 
```
* crossplane-system 
* my-app

Control secret in the Namespace my-app:
```
kubectl get secret -n my-app
```

AWS provider should also be installed and the Healthy should be True:
```
kubectl get provider.pkg
```

Provider config named my-app-aws should also be deployed:
```
kubectl get providerconfig
```

Now you should have a running Crossplane environemnt that can be used for the following katas.


## Want to help make our training material better?
 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/dojo/issues).
