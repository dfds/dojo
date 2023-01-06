DFDS Crossplane - Code kata #4
======================================

This training exercise is a **beginner-level** course on Crossplane that serves as a starting point for SRE's looking to install and maintain Crossplane at DFDS.

## Getting started
These instructions will help you prepare for the kata and ensure that your training machine has the tools installed you will need to complete the assignment(s). If you find yourself in a situation where one or more tools might not be available for your training environment please reach out to your instructor for assistance on how to proceed, post an [issue in our repository](https://github.com/dfds/dojo/issues) or fix it yourself and update the kata via a [pull request](https://github.com/dfds/dojo/pulls).

### Prerequisites
* Kubernetes Cluster
* Kubectl
* Helm
* Kata 3

## Exercise
Your third assignment will see you provision some database infrastructure into your cloud provider.

### 1. Create a secg.yaml file on your system

By default, unless we specify, our RDS instance will get created in the default VPC and attach the default security group. This means that traffic will unlikely be blocked on the port we require. Let's create a security group resource to use with our RDS Instance.

Create a secg.yaml file

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
    name: my-aws-provider-config
```

### 2. Deploy the security group manifest

Deploy the manifest to create the security group in AWS

```bash
kubectl apply -f secg.yaml
```

### 3. Observe security group creation

Check that the security group is showing in a READY and SYNCED state.

```bash
kubectl get securitygroup
```

You may also view in the AWS console to confirm that the group now exists

### 4. Create a db.yaml file on your filesystem

First we will create a manifest to describe an RDS database instance we want to provision. Create a db.yaml file

```yaml
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
    engine: postgres
    engineVersion: "13.3"
    skipFinalSnapshotBeforeDeletion: true
    applyModificationsImmediately: true
    vpcSecurityGroupIDRefs:
    - name: my-security-group
  providerConfigRef:
    name: my-aws-provider-config
  writeConnectionSecretToRef:
    namespace: my-namespace
    name: my-database-creds
```

### 5. View possible options for kind

If you are curious, you can use the following command to see the options you can specify

```bash
kubectl explain rdsinstance --recursive | less
```

Note that this does not tell you any special value requirements other than string, int etc. You can look at the Github source code to find any input requirements for the fields - https://github.com/crossplane/provider-aws/blob/master/package/crds/database.aws.crossplane.io_rdsinstances.yaml

### 6. Deploy the DB manifest

We will deploy manifest into our cluster

```bash
kubectl apply -f db.yaml
```

### 7. Observe creation

```bash
kubectl get rdsinstance
kubectl describe secret my-database-creds -n my-namespace
```

Note that if we describe the credentials again, we can see a password and username field

Our database should enter a state of creating. You can sign into the AWS console and go to the RDS section to see this being created there.

Wait until the RDS Instance has finished deploying and in a ready state (this should take 5 minutes, feel free to take a quick break)

> **Hint**: You can append -w to a get command to "watch" the status of the resource change. I.e `kubectl get rdsinstance -w`

Note that if we describe the credentials again once the database is fully deployed, this secret should now contain the endpoint and port


### 8. Change allocated storage

Next we will change the allocatedStorage value applied to our RDS Instance.

Update the db.yaml file

```yaml
allocatedStorage: 30
```

Apply this change

```bash
kubectl apply -f db.yaml
```

### 9. Observe sync status

Describe the RDS instance to see that there has been a successful request to update the external resource

```bash
kubectl describe rdsinstance my-database
```

Get the RDS Instance to see the state change to modifying and ready to false (this can take a minute or two to start reconciling)

```bash
kubectl get rdsinstance
```

You should also see this updating in the AWS Web Console.

Eventually the modification should complete


### 10. Cleanup resources

We should clean up resources so that we do not incur any unnecessary costs

```bash
kubectl delete -f db.yaml
kubectl delete -f secg.yaml
```

If you are not continuing to Kata 5, also clean up:

```bash
kubectl delete -f providerconfig.yaml
kubectl delete provider.pkg provider-aws-name
helm uninstall crossplane -n crossplane-system

```

## Want to help make our training material better?
 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/dojo/issues).
