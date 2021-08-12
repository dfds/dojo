DFDS Crossplane - Code kata #7
======================================

This training exercise is an **intermediate-level** course on Crossplane that serves as a starting point for SRE's looking to install and maintain Crossplane at DFDS.

## Getting started
These instructions will help you prepare for the kata and ensure that your training machine has the tools installed you will need to complete the assignment(s). If you find yourself in a situation where one of more tools might not be available for your training environment please reach out to your instructor for assistance on how to proceed, post an [issue in our repository](https://github.com/dfds/dojo/issues) or fix it yourself and update the kata via a [pull request](https://github.com/dfds/dojo/pulls).

### Prerequisites
* Kubernetes Cluster
* Kubectl
* Helm
* Docker hub account

## Exercise
Your seventh assignment will see you create a configuration package

### 1. Create a configuration directory

First we need to create a configuration directory and copy in our definition and composition manifests from previous katas
```
mkdir my-configuration
cd my-configuration
cp /path/to/definition.yaml .
cp /path/to/composition.yaml .
```

### 2. Create a crossplane.yaml

Next we need to add a crossplane.yaml file to our directory. This file describes our configuration and any dependencies

```
apiVersion: meta.pkg.crossplane.io/v1
kind: Configuration
metadata:
  name: my-configuration
spec:
  crossplane:
    version: ">=v1.2.1"
  dependsOn:
    - provider: crossplane/provider-aws
      version: "v0.19.0"
```
### 3. Authenticate with Docker

We need to authenticate with Dockerhub in order to be able to push our configuration package

```
docker login -u my-username
```


### 4. Build and push the configuration

Ensure you change the following commands to refer to your dockerhub account name instead of my-dockerhub.
Build the configuration locally, notice how it creates a .xpkg file. Push it to your dockerhub repository with the push command below

```
kubectl crossplane build configuration
kubectl crossplane push configuration my-dockerhub/my-configuration:v0.0.1-alpha.0
```

### 5. Install the configuration package

Next we will install our configuration package into our cluster so that people can consume resources

```
kubectl crossplane install configuration my-dockerhub/my-configuration:v0.0.1-alpha.0
```

### 6. Verify installation

```
kubectl get configuration.pkg
kubectl get xrd
kubectl get crd | grep dfds
```


## Want to help make our training material better?
 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/dojo/issues).