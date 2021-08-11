DFDS Crossplane - Code kata #6
======================================

This training exercise is an **intermediate-level** course on Crossplane that serves as a starting point for SRE's looking to install and maintain Crossplane at DFDS.

## Getting started
These instructions will help you prepare for the kata and ensure that your training machine has the tools installed you will need to complete the assignment(s). If you find yourself in a situation where one of more tools might not be available for your training environment please reach out to your instructor for assistance on how to proceed, post an [issue in our repository](https://github.com/dfds/dojo/issues) or fix it yourself and update the kata via a [pull request](https://github.com/dfds/dojo/pulls).

### Prerequisites
* Kubernetes Cluster
* Kubectl
* Helm

## Exercise
Your sixth assignment will see you deploy resources using custom compositions and claims

### 1. Create an instance.yaml

First we can deploy an instance of our new composite resource

Create an instance.yaml with the following contents:

```
apiVersion: crossplane.dfds.cloud/v1alpha1
kind: CompositeDatabaseInstance
metadata:
  # Composite resources are cluster scoped, so there's no need for a namespace.
  name: my-composite-database-instance
spec:
  parameters:
    region: eu-west-1
    dbInstanceClass: db.t3.small
    masterUsername: masteruser
    allocatedStorage: 20
    engine: postgres
    engineVersion: "13"
    skipFinalSnapshotBeforeDeletion: true
  compositionRef:
    name: compositedatabaseinstances.crossplane.dfds.cloud
  writeConnectionSecretToRef:
    namespace: default
    name: my-composite-database-credentials
```

### 2. Apply the instance manifest

Next we will apply the manifest

```
kubectl apply -f instance.yaml
```

### 3. Verify deployment and identify a problem

Verify the deployment by issuing a get and describe

```
kubectl get compositedatabaseinstance
kubectl describe compositedatabaseinstance
```

Notice that the resource is not yet ready. See if you can use your understanding to find a problem preventing the composed resources from 
being created and fix the issue. Ask for hints if you get stuck

It could take 10 minutes for the resources to provision once they have reconciliated after fixing the problem.

### 4. Create a static.yaml

Now let's make a static claim to the resource we just created. A static claim will take sole ownership of the resource

```
apiVersion: crossplane.dfds.cloud/v1alpha1
kind: DatabaseInstance
metadata:
  namespace: my-namespace
  name: my-static-claim
spec:
  parameters:
    region: eu-west-1
    dbInstanceClass: db.t3.small
    masterUsername: masteruser
    allocatedStorage: 20
    engine: postgres
    engineVersion: "13"
    skipFinalSnapshotBeforeDeletion: true
  resourceRef:
    apiVersion: crossplane.dfds.cloud/v1alpha1
    kind: CompositeDatabaseInstance
    name: my-composite-database-instance
  writeConnectionSecretToRef:
    name: my-static-database-creds
```

### 4. Apply the static manifest

Now apply the manifest to deploy the claim

```
kubectl apply -f static.yaml
```

### 5. Verify there is now a static databaseinstance and secret in your namespace

Observe the status of the database instance. Note how it is scoped to the namespace and that a secret also exists

```
kubectl get databaseinstance -n my-namespace
kubectl get secret -n my-namespace
```

### 5. Create a dynamic.yaml

Next we will create a dynamic claim. This should provision an instance dynamically on request from the claim

```
apiVersion: crossplane.dfds.cloud/v1alpha1
kind: DatabaseInstance
metadata:
  namespace: my-namespace
  name: dynamic-claim-example
spec:
  parameters:
    region: eu-west-1
    dbInstanceClass: db.t3.small
    masterUsername: masteruser
    allocatedStorage: 30
    engine: postgres
    engineVersion: "13"
    skipFinalSnapshotBeforeDeletion: true
  compositionSelector:
    matchLabels:
      purpose: database
      provider: aws
  
  writeConnectionSecretToRef:
    name: dynamic-claim-example-secret
```

### 6. Apply the dynamic manifest

Now apply the dynamic claim

```
kubectl apply -f dynamic.yaml
```

### 7. Verify dynamic databaseinstance, secret and resource creation
```
kubectl get databaseinstance -n my-namespace
kubectl get secret -n my-namespace
kubectl get securitygroup
kubectl get rdsinstance
```

### 8. Cleanup resources

## Want to help make our training material better?
 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/dojo/issues).