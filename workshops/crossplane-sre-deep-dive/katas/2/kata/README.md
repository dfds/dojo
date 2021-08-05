DFDS Crossplane - Code kata #2
======================================

This training exercise is a **beginner-level** course on Crossplane that serves as a starting point for SRE's looking to install and maintain Crossplane at DFDS.

## Getting started
These instructions will help you prepare for the kata and ensure that your training machine has the tools installed you will need to complete the assignment(s). If you find yourself in a situation where one of more tools might not be available for your training environment please reach out to your instructor for assistance on how to proceed, post an [issue in our repository](https://github.com/dfds/dojo/issues) or fix it yourself and update the kata via a [pull request](https://github.com/dfds/dojo/pulls).

### Prerequisites
* Kubernetes Cluster
* Kubectl
* Helm
* Kata 1

## Exercise
Your second assignment will see you install a Crossplane provider into your Kubernetes cluster using the Crossplane CLI and verify its installation. 
You will also upgrade this package to the latest version and switch between provider revisions

### 1. Install provider package

We need to install the AWS provider for crossplane to be able to deploy AWS resources

```
kubectl crossplane install provider crossplane/provider-aws:v0.18.1
```

### 2. Verify provider installation

We should verify that the AWS provider and providerrevision have installed successfully and are in a healthy state

```
kubectl get provider.pkg
kubectl get providerrevision
```

After a small period of time these should report as Healthy

### 3. Update provider to latest version

We intentionally installed an older version of the AWS provider. We should update to the latest version. First retrieve the NAME of the deployed provider.pkg and run the update command using the Crossplane CLI

```
kubectl crossplane update provider [provider-name] v0.19.0
```

### 4. Verify provider update and revision history

```
kubectl get provider.pkg
kubectl get providerrevision
```

Note that we now have 2 revisions of the same crossplane provider. Our new version should show in Active state

## Want to help make our training material better?
 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/dojo/issues).