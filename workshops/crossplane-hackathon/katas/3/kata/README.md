DFDS Crossplane Hackathon - Code kata #3
======================================

This training exercise is a **intermediate-level** course on Crossplane that serves as a starting point for Developers looking to onboard the provisioning efforts at DFDS.

## Getting started
These instructions will help you prepare for the kata and ensure that your training machine has the tools installed you will need to complete the assignment(s). If you find yourself in a situation where one of more tools might not be available for your training environment please reach out to your instructor for assistance on how to proceed, post an [issue in our repository](https://github.com/dfds/dojo/issues) or fix it yourself and update the kata via a [pull request](https://github.com/dfds/dojo/pulls).

### Prerequisites
* Kata 2

## Exercise
This assignment will see you package up the composition you made in the last exercise and publish it to a container registry. You will then consume this package and deploy it into  your cluster. Finally, you will deploy a resource using the package.

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
    version: ">=v1.0.0-0"
  dependsOn:
    - provider: crossplane/provider-aws
      version: "v0.20.0"

```

### 3. Build and push the configuration

Build the configuration locally by running the following command from your configuration directory

```
kubectl crossplane build configuration --name my-configuration
```

Verify that it creates a `.xpkg` file on your local filesystem

```
ls | grep xpkg
```

Push it to the container registy with the following set of commands:

```
docker load -i my-configuration.xpkg
```

This should give you an image ID to use in the next command:

docker tag <image-id-from-previous-command> myorg/my-configuration:v0.0.1
kubectl k8scr push myorg/my-configuration:v0.0.1
```

### 5. Install the configuration package into our cluster

Next we will install our configuration package into our cluster so that we can consume the resource. 

First, create a configuration.yaml

```
apiVersion: pkg.crossplane.io/v1
kind: Configuration
metadata:
  name: myorg-my-configuration
spec:
  package: myorg/my-configuration:v0.0.1
  packagePullPolicy: IfNotPresent
  revisionActivationPolicy: Automatic
  revisionHistoryLimit: 1
```

Then run the following command:

```
kubectl apply -f configuration.yaml
```

### 6. Verify installation

Run the following to confirm our configuration package is installed successfully:

```
kubectl get configuration.pkg
```

Run the following commands to verify that the XRD and CRD's have been installed by the package:

```
kubectl get xrd
kubectl get crd | grep example.org
```

### 6. Make a claim.yaml manifest

Here we create a manifest file which defines the creation of an instance of our composite resource

```
apiVersion: storage.example.org/v1alpha1
kind: MyBucket
metadata:
  name: my-bucket-from-package
  namespace: default
spec:
  compositionSelector:
    matchLabels:
      provider: aws
  parameters:
    bucketName: test-bucket-from-package
```

Next we will deploy our manifest into our cluster:

```
kubectl apply -f claim.yaml
```

We can then verify the creation of our composite resource:

```
kubectl get MyBucket
```

And we should also verify that the 2 s3 buckets have been created by the composite:

```
kubectl get Bucket
```

And verify that the s3 buckets are visible using the AWS CLI:

```
kubectl exec --stdin --tty aws-cli-runtime -- aws s3 --endpoint-url=http://localstack.default.svc.cluster.local:4566 ls
```

### 7. Cleanup Resources

We need to clean up our resources so that they are not left behind for the next Kata

```
kubectl delete -f claim.yaml
kubectl delete configuration.pkg myorg-my-configuration
```

## Want to help make our training material better?
 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/dojo/issues).