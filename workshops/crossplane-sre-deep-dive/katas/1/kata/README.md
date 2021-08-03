DFDS Crossplane - Code kata #1
======================================

This training exercise is a **beginner-level** course on Crossplane that serves as a starting point for SRE's looking to install and maintain Crossplane at DFDS.

## Getting started
These instructions will help you prepare for the kata and ensure that your training machine has the tools installed you will need to complete the assignment(s). If you find yourself in a situation where one of more tools might not be available for your training environment please reach out to your instructor for assistance on how to proceed, post an [issue in our repository](https://github.com/dfds/dojo/issues) or fix it yourself and update the kata via a [pull request](https://github.com/dfds/dojo/pulls).

### Prerequisites
* Kubernetes Cluster
* Kubectl
* Helm

## Exercise
Your first assignment will see you install Crossplane into your Kubernetes Cluster and the Crossplane CLI onto your development machine

### 1. Create crossplane-system Namespace

```
kubectl create namespace crossplane-system
```

### 2. Add Crossplane Stable Helm Repository

```
helm repo add crossplane-stable https://charts.crossplane.io/stable
helm repo update
```

### 3. Install Crossplane Helm Chart

```
helm install crossplane --namespace crossplane-system crossplane-stable/crossplane --version 1.3.0
```

### 4. Validate Installation

```
helm list -n crossplane-system
kubectl get all -n crossplane-system
```

### 5. Install Crossplane CLI

```
curl -sL https://raw.githubusercontent.com/crossplane/crossplane/master/install.sh | sh
```

### 6. Verify Crossplane CLI
```
```

## Want to help make our training material better?
 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/dojo/issues).