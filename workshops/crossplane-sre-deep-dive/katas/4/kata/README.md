DFDS Crossplane - Code kata #4
======================================

This training exercise is a **beginner-level** course on Crossplane that serves as a starting point for SRE's looking to install and maintain Crossplane at DFDS.

## Getting started
These instructions will help you prepare for the kata and ensure that your training machine has the tools installed you will need to complete the assignment(s). If you find yourself in a situation where one of more tools might not be available for your training environment please reach out to your instructor for assistance on how to proceed, post an [issue in our repository](https://github.com/dfds/dojo/issues) or fix it yourself and update the kata via a [pull request](https://github.com/dfds/dojo/pulls).

### Prerequisites
* Kubernetes Cluster
* Kubectl
* Helm
* Kata 3

## Exercise
Your third assignment will see you provision some database infrastructure into your cloud provider. 


### 1. Create a db.yaml file on your filesystem

```
---
apiVersion: database.aws.crossplane.io/v1beta1
kind: RDSInstance
metadata:
  name: my-database
  namespace: my-namespace
spec:
  forProvider:
    region: eu-west-1
    dbInstanceClass: db.t3.small
    masterUsername: masteruser
    allocatedStorage: 20
    engine: postgresql
    engineVersion: "13.3"
    skipFinalSnapshotBeforeDeletion: true
  providerConfigRef:
    name: my-provider-config
  writeConnectionSecretToRef:
    namespace: my-namespace
    name: my-database-creds
```

### 2. Deploy the DB manifest

```
kubectl apply -f db.yaml
```

### 3. Observe creation
```
kubectl get rdsinstance
```

### 4. Create a secg.yaml file on your system

```
---
apiVersion: ec2.aws.crossplane.io/v1beta1
kind: SecurityGroup
metadata:
  name: my-security-group
  namespace: my-namespace
spec:
  forProvider:
    region: eu-west-1
    tags:
    - key: Name
      value: my-security-group
    groupName: my-security-group
    description: Security Group for RDS
    ingress: 
    - fromPort: 5432
      toPort: 5432
      ipProtocol: tcp
      ipRanges:
      - cidrIp: 0.0.0.0/0
        description: postgresql
  providerConfigRef:
    name: my-provider-config
```

### 5. Deploy the security group manifest

```
kubectl apply -f secg.yaml
```

### 6. Observe security group creation

```
kubectl get securitygroup
```

### 7. Update db.yaml manifest to use security group

```
---
apiVersion: database.aws.crossplane.io/v1beta1
kind: RDSInstance
metadata:
  name: my-database
  namespace: my-namespace
spec:
  forProvider:
    region: eu-west-1
    dbInstanceClass: db.t3.small
    masterUsername: masteruser
    allocatedStorage: 20
    engine: postgresql
    engineVersion: "13.3"
    skipFinalSnapshotBeforeDeletion: true
    vpcSecurityGroupIDRefs:
    - name: my-security-group
  providerConfigRef:
    name: my-provider-config
  writeConnectionSecretToRef:
    namespace: my-namespace
    name: my-database-creds
```

### 8. Redeploy db.yaml
```
---
apiVersion: database.aws.crossplane.io/v1beta1
kind: RDSInstance
metadata:
  name: my-database
  namespace: my-namespace
spec:
  forProvider:
    region: eu-west-1
    dbInstanceClass: db.t3.small
    masterUsername: masteruser
    allocatedStorage: 20
    engine: postgresql
    engineVersion: "13.3"
    skipFinalSnapshotBeforeDeletion: true
    vpcSecurityGroupIDRefs:
    - name: my-security-group
  providerConfigRef:
    name: my-provider-config
  writeConnectionSecretToRef:
    namespace: my-namespace
    name: my-database-creds
```

### 9. Observe changes

## Want to help make our training material better?
 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/dojo/issues).