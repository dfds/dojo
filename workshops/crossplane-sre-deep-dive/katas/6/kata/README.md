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
Your fifth assignment will see you deploy resources using custom compositions and claims

### 1. Create an instance.yaml
```
apiVersion: crossplane.dfds.cloud/v1alpha1
kind: CompositeDatabaseInstance
metadata:
  # Composite resources are cluster scoped, so there's no need for a namespace.
  name: example-dfds-rds
spec:
  parameters:
    region: eu-west-1
    dbInstanceClass: db.t2.small
    masterUsername: masteruser
    allocatedStorage: 20
    engine: postgres
    engineVersion: "9.6"
    skipFinalSnapshotBeforeDeletion: true
  compositionRef:
    name: compositedatabaseinstances.crossplane.dfds.cloud
  writeConnectionSecretToRef:
    namespace: default
    name: example-databaseinstance
```

### 2. Apply the instance manifest
```
kubectl apply -f instance.yaml
```

### 3. Create a static.yaml

```
# The MySQLInstance always has the same API group and version as the
# resource it requires. Its kind is always suffixed with .
apiVersion: crossplane.dfds.cloud/v1alpha1
kind: DatabaseInstance
metadata:
  # Infrastructure claims are namespaced.
  namespace: default
  name: static-claim-example
spec:
  parameters:
    region: eu-west-1
    dbInstanceClass: db.t2.small
    masterUsername: masteruser
    allocatedStorage: 40
    engine: postgres
    engineVersion: "9.6"
    skipFinalSnapshotBeforeDeletion: true
  resourceRef:
    apiVersion: crossplane.dfds.cloud/v1alpha1
    kind: CompositeDatabaseInstance
    name: example-dfds-rds
  writeConnectionSecretToRef:
    name: example-dfds-rds-databaseinstance
```

### 4. Apply the static manifest

```
kubectl apply -f static.yaml
```

### 5. Create a dynamic.yaml
```
apiVersion: crossplane.dfds.cloud/v1alpha1
kind: DatabaseInstance
metadata:
  namespace: my-namespace
  name: dynamic-claim-example
spec:
  parameters:
    region: eu-west-1
    dbInstanceClass: db.t2.small
    masterUsername: masteruser
    allocatedStorage: 30
    engine: postgres
    engineVersion: "11"
    skipFinalSnapshotBeforeDeletion: true
  compositionSelector:
    matchLabels:
      purpose: database
      provider: aws
  
  writeConnectionSecretToRef:
    name: dynamic-claim-example-secret
```

### 6. Apply the dynamic manifest

```
kubectl apply -f dynamic.yaml
```

### 7. Cleanup resources

## Want to help make our training material better?
 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/dojo/issues).