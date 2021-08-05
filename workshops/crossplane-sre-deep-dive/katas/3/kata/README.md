DFDS Crossplane - Code kata #3
======================================

This training exercise is a **beginner-level** course on Crossplane that serves as a starting point for SRE's looking to install and maintain Crossplane at DFDS.

## Getting started
These instructions will help you prepare for the kata and ensure that your training machine has the tools installed you will need to complete the assignment(s). If you find yourself in a situation where one of more tools might not be available for your training environment please reach out to your instructor for assistance on how to proceed, post an [issue in our repository](https://github.com/dfds/dojo/issues) or fix it yourself and update the kata via a [pull request](https://github.com/dfds/dojo/pulls).

### Prerequisites
* Kubernetes Cluster
* Kubectl
* Helm
* Kata 2
* AWS Admin Credentials

## Exercise
Your third assignment will see you configure your provider by provisioning a ProviderConfig. 

### 1. Create a creds.conf file on your filesystem

Crossplane works by using credentials from your Cloud Provider. We will temporarily create a creds.conf file on the local file system. Do not commit this to any repository

```
[default]
aws_access_key_id = KEY_HERE
aws_secret_access_key = SECRET_HERE

```

### 2. Create a namespace for provisioning

Next, create a namespace which will hold our secret

```
kubectl create namespace my-namespace
```

### 3. Create a provider secret using the creds.conf

Then we must create a secret in our namespace from the creds.conf file

```
kubectl create secret generic aws-creds -n my-namespace --from-file=creds=./creds.conf
```

Verify that the secret exists

```
kubectl describe secret aws-creds -n my-namespace
```

Then delete our creds.conf file from our filesystem

```
rm creds.conf
```

### 4. Create a ProviderConfig manifest file providerconfig.yaml

We must create a ProviderConfig which uses our secret so that we have permissions to deploy resources in our cloud account. Create a file on the local filesystem called providerconfig.yaml

```
apiVersion: aws.crossplane.io/v1beta1
kind: ProviderConfig
metadata:
  name: my-aws-providerconfig
spec:
  credentials:
    source: Secret
    secretRef:
      namespace: my-namespace
      name: aws-creds
      key: creds
```

### 5. Deploy the ProviderConfig manifest

Deploy this manifest to our cluster. ProviderConfigs exist at the cluster level and can not be namespaced

```
kubectl apply -f providerconfig.yaml
```

Verify that the providerconfig exists

```
kubectl get ProviderConfig
```

## Want to help make our training material better?
 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/dojo/issues).