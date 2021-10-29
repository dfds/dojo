DFDS Crossplane Hackathon - Code kata #2
======================================

This training exercise is a **intermediate-level** course on Crossplane that serves as a starting point for Developers looking to onboard the provisioning efforts at DFDS.

## Getting started
These instructions will help you prepare for the kata and ensure that your training machine has the tools installed you will need to complete the assignment(s). If you find yourself in a situation where one of more tools might not be available for your training environment please reach out to your instructor for assistance on how to proceed, post an [issue in our repository](https://github.com/dfds/dojo/issues) or fix it yourself and update the kata via a [pull request](https://github.com/dfds/dojo/pulls).

### Prerequisites
* Kata 2

## Exercise
This assignment will see you create a composite resource by creating a definition and a composition. You will deploy these into your cluster and verify them.

### 1. Create a definition.yaml
First we need to create a Composite Resource Definition (XRD) that describes the resource parameters we need to define in order to create the resource.

Create a definition.yaml containing the below content:

```
yaml goes here
```

### 2. Apply the definition manifest and verify its installation

Apply the manifest file to deploy the XRD into your cluster by running the following command:

```
kubectl apply -f definition.yaml
```

Verify its existence by running the following command:

```
kubectl get xrd
```

Verify that it has created the custom resource definitions (claims) by running the following command:

```
kubectl get crd | grep dfds
```


### 3. Create a composition.yaml

Now we will create a composition manifest which declares the resources that are to be created with base settings/defaults.

Create a composition.yaml and populate with the following contents:

```
yaml content here
```

### 4. Apply the composition manifest and verify its installation

Apply the manifest to deploy the composition to your cluster by running the following command:

```
kubectl apply -f composition.yaml
```

Verify that the composition exists by running the following command:

```
kubectl get composition

```



## Want to help make our training material better?
 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/dojo/issues).