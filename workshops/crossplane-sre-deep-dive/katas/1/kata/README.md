DFDS Crossplane - Code kata #1
======================================

This training exercise is a **beginner-level** course on Crossplane that serves as a starting point for SRE's looking to install and maintain Crossplane at DFDS.

## Getting started
These instructions will help you prepare for the kata and ensure that your training machine has the tools installed you will need to complete the assignment(s). If you find yourself in a situation where one or more tools might not be available for your training environment please reach out to your instructor for assistance on how to proceed, post an [issue in our repository](https://github.com/dfds/dojo/issues) or fix it yourself and update the kata via a [pull request](https://github.com/dfds/dojo/pulls).

### Prerequisites
* Kubernetes Cluster
* Kubectl
* Helm

> We strongly recommend the use of WSL due to limitations of the Crossplane CLI on native Windows

## Exercise
Your first assignment will see you install Crossplane into your Kubernetes Cluster and the Crossplane CLI onto your development machine

### 1. Create crossplane-system Namespace

Create a crossplane-system in your Kubernetes cluster to hold Crossplane system components

```
kubectl create namespace crossplane-system
```

Verify the namespace exists

```
kubectl get namespaces
```

### 2. Add Crossplane Stable Helm Repository

We need to add the stable crossplane repository in order to install Crossplane into our namespace

```
helm repo add crossplane-stable https://charts.crossplane.io/stable
helm repo update
```

Verify the repo has added successfully

```
helm repo list
```

### 3. Install Crossplane Helm Chart

We need to install crossplane to our namespace using the crossplane chart from the stable repository

```
helm install crossplane --namespace crossplane-system crossplane-stable/crossplane --version 1.3.0
```

Verify that the helm chart has successfully installed

```
helm list -n crossplane-system
```

### 4. Validate Installation

Verify that the installation has been successful and that the crossplane pods are running

```
kubectl get all -n crossplane-system
```

### 5. Install Crossplane CLI

Install the Crossplane CLI so that we are ready to easily perform some crossplane tasks

```
curl -sL https://raw.githubusercontent.com/crossplane/crossplane/master/install.sh | sh
sudo mv kubectl-crossplane /usr/local/bin
```

> **Windows Powershell Users**: You need to download [Crank.exe](https://releases.crossplane.io/stable/current/bin/windows_amd64/crank.exe), rename it to kubectl-crossplane.exe and save it to a directory in your user PATH

Verify that the CLI is installed
```
kubectl crossplane --version
```

## Want to help make our training material better?
 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/dojo/issues).