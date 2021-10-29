DFDS Crossplane Hackathon - Code kata #3
======================================

This training exercise is a **intermediate-level** course on Crossplane that serves as a starting point for Developers looking to onboard the provisioning efforts at DFDS.

## Getting started
These instructions will help you prepare for the kata and ensure that your training machine has the tools installed you will need to complete the assignment(s). If you find yourself in a situation where one of more tools might not be available for your training environment please reach out to your instructor for assistance on how to proceed, post an [issue in our repository](https://github.com/dfds/dojo/issues) or fix it yourself and update the kata via a [pull request](https://github.com/dfds/dojo/pulls).

### Prerequisites
* Kata 2

## Exercise
This assignment will see you package up the composition you made in the last exercise and publish it to a container registry. You will then consume this package and deploy it into 
your cluster. Finally, you will deploy a resource using the package.

### 1. Create a configuration directory
First we need to create a directory to contain the files necessary for our package. We will copy in the definition and composition we created in the last exercise.

```
mkdir my-configuration
cd my-configuration
cp /path/to/definition.yaml .
cp /path/to/composition.yaml .
```

### 2. Create a crossplane.yaml file
Next we will create a crossplane.yaml file which defines the package by giving it a name and specifying any dependencies.

```
apiVersion: meta.pkg.crossplane.io/v1
kind: Configuration
metadata:
  name: my-configuration
spec:
  crossplane:
    version: ">=v1.3.0"
  dependsOn:
    - provider: crossplane/provider-aws
      version: "v0.20.0"

```

### 3. Something something registry

```
```


### 4. Build and push the configuration

Build the configuration locally by running the following command from your configuration directory

```
kubectl crossplane build configuration
```

Verify that it creates a `.xpkg` file on your local filesystem

```
ls | grep xpkg
```

Push it to the container registy with the following command:

```
kubectl crossplane push configuration my-registry/my-configuration:v0.0.1-alpha.0
```

Verify that it has pushed an OCI image to the registry with the following command:

```
something something verify
```

### 5. Install the configuration package into our cluster

Next we will install our configuration package into our cluster so that we can consume the resource. Run the following command:

```
kubectl crossplane install configuration my-registry/my-configuration:v0.0.1-alpha.0
```

### 6. Verify installation

Run the following to confirm our configuration package is installed successfully:

```
kubectl get configuration.pkg
```

Run the following commands to verify that the XRD and CRD's have been installed by the package:

```
kubectl get xrd
kubectl get crd | grep dfds
```

## Want to help make our training material better?
 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/dojo/issues).