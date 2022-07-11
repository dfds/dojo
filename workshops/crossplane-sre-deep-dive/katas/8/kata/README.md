DFDS Crossplane - Code kata #8
======================================

This training exercise is an **intermediate-level** course on Crossplane that serves as a starting point for SRE's looking to install and maintain Crossplane at DFDS.

## Getting started
These instructions will help you prepare for the kata and ensure that your training machine has the tools installed you will need to complete the assignment(s). If you find yourself in a situation where one or more tools might not be available for your training environment please reach out to your instructor for assistance on how to proceed, post an [issue in our repository](https://github.com/dfds/dojo/issues) or fix it yourself and update the kata via a [pull request](https://github.com/dfds/dojo/pulls).

### Prerequisites
* Kubernetes Cluster
* Kubectl
* Helm
* Docker hub account

## Exercise
Your eigth assignment will see you deploy resources from your configuration package

### 1. Create a package-resource.yaml

Outside of the 'my-configuration' directory you created in the last kata, we will create a manifest to deploy a resource from our package. Create a package-resource.yaml with the below content:

```
apiVersion: crossplane.dfds.cloud/v1alpha1
kind: DatabaseInstance
metadata:
  namespace: my-namespace
  name: my-package-resource
spec:
  parameters:
    region: eu-west-1
    dbInstanceClass: db.t3.small
    masterUsername: masteruser
    allocatedStorage: 20
    engine: postgres
    engineVersion: "13"
    skipFinalSnapshotBeforeDeletion: true
  compositionSelector:
    matchLabels:
      purpose: database
      provider: aws
  
  writeConnectionSecretToRef:
    name: my-package-resource-secret
```

### 2. Deploy our resource

Next we will deploy the manifest:

```
kubectl apply -f package-resource.yaml
```

### 3. Verify our resource is created

Let's verify that our resources have been created:

```
kubectl get databaseinstance -n my-namespace
kubectl get securitygroup
kubectl get rdsinstance
```

Once the RDS instance has finished creating and the database instances recognises this and shows Ready, you should be able to see the secret in your namespace
```
kubectl get secret my-package-resource-secret -n my-namespace
```

### 4. Cleanup resources

```
kubectl delete -f package-resource.yaml
```


## Want to help make our training material better?
 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/dojo/issues).