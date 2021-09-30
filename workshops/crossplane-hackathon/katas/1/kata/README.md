DFDS Crossplane Hackathon - Code kata #1
======================================

This training exercise is a **beginner-level** course on Crossplane that serves as a starting point for Developers looking to onboard the provisioning efforts at DFDS.

## Getting started
These instructions will help you prepare for the kata and ensure that your training machine has the tools installed you will need to complete the assignment(s). If you find yourself in a situation where one of more tools might not be available for your training environment please reach out to your instructor for assistance on how to proceed, post an [issue in our repository](https://github.com/dfds/dojo/issues) or fix it yourself and update the kata via a [pull request](https://github.com/dfds/dojo/pulls).

### Prerequisites
* [Docker](https://www.docker.com/get-started)
* [AWS CLI](https://aws.amazon.com/cli/)
* [Terraform](https://learn.hashicorp.com/tutorials/terraform/install-cli)
* [Crossplane](https://github.com/dfds/dojo/blob/master/workshops/crossplane-sre-deep-dive/katas/1/kata/README.md)
* [Crossplane/provider-aws](https://github.com/dfds/dojo/blob/master/workshops/crossplane-sre-deep-dive/katas/2/kata/README.md)

## Exercise
Your first assignment will see you provision a S3 bucket via the Crossplane AWS provider to act as storage for your very own Hello website!. Sounds simple, no? Let's start then!

### 1. Create index.html page for website
Create a file called `index.html` to act as the landing page for your personal website and add the following content:

```
<html>
    <h1>Hello from crossplane</h1>
</html>
```

### 2. Add ProviderConfig.yaml with your AWS credentials
In order to enable Crossplane to provision a S3 bucket in your AWS account we need to add a ProviderConfig containing a set of valid account credentials. In order to accomplish this we will create a file called `provider-config.yaml` and add the following content:

```
---
apiVersion: aws.crossplane.io/v1beta1
kind: ProviderConfig
metadata:
  name: aws-default
spec:
  credentials:
    source: Secret
    secretRef:
      namespace: default
      name: aws-creds
      key: key
```

Just to explain: <br/>
`metadata: name: aws-default` - is the name of the resource your creating<br/>
`spec: credentials: source: Secret` - instructs Crossplane that the credentials will be sourced from a secret<br/>
`spec: secretRef: namespace: default` - tells Crossplane that the secret in question is in the default namespace<br/>
`spec: secretRef: name: aws-creds` - providers the name of the secret that should be referenced<br/>

### 3. Deploy the ProviderConfig manifest

Deploy this manifest to our cluster. ProviderConfigs exist at the cluster level and can not be namespaced

```
kubectl apply -f providerconfig.yaml
```

Verify that the providerconfig exists

```
kubectl get providerconfig.aws.crossplane.io
```

### 4. Create s3bucket.yaml that keeps your S3 bucket configuration on your filesystem
Once Crossplane has gotten a valid ProviderConfig it is able to begin provisioning resources in your AWS account. To create a S3 bucket we need to add the following:

```
apiVersion: s3.aws.crossplane.io/v1beta1
kind: Bucket
metadata:
  name: your-test-bucket
spec:
  forProvider:
    acl: public-read
    locationConstraint: us-east-1
    publicAccessBlockConfiguration:
      blockPublicPolicy: false    
    corsConfiguration:
      corsRules:
        - allowedMethods:
            - "GET"
          allowedOrigins:
            - "*"
          allowedHeaders:
            - "*"
          exposeHeaders:
            - "x-amz-server-side-encryption"
  providerConfigRef:
    name: aws-default
```

Just to explain: <br/>
`kind: Bucket` - is the type of resource we want to provision<br/>
`metadata: name: your-test-bucket` - is the name of the resource your creating. Replace the value with a name that is globally unique. You might need to do a couple of attempts before you find a good name <br/>
`spec: forProvider:` - contains the configuration to be passed by the provider in charge of provisioning the S3 Bucket<br/>
`spec: providerConfigRef: name: aws-default` - points to the ProviderConfig used for provisioning the S3 Bucket<br/>


### 5. Deploy the S3 bucket manifest

We will deploy manifest into our cluster

```
kubectl apply -f s3bucket.yaml
```

### 6. Upload index.html to S3 bucket
Upload content to S3 bucket using the AWS CLI:

```
aws s3 cp index.html s3://your-test-bucket --acl public-read
```

**Note**: Replace your-test-bucket with the name of the bucket you created in step 4

### 7. Verify that index.html has been uploaded to S3 bucket
The following AWS command will list the content of the bucket

```
aws ls your-test-bucket
```

**Note**: Replace your-test-bucket with the name of the bucket you created in step 4
### 8. Update S3 bucket configurations
Update ACL configuration from public-read to private

```
apiVersion: s3.aws.crossplane.io/v1beta1
kind: Bucket
metadata:
  name: your-test-bucket
spec:
  forProvider:
    acl: private
...
```

### 9. Observe sync status

Describe the S3 bucket to see that there has been a successful request to update the external resource

```
kubectl describe bucket your-test-bucket
```
### 10. Cleanup resources

We should clean up resources so that we do not incur any unnecessary costs

```
kubectl delete -f s3bucket.yaml
```
Check if S3 bucket has been deleted:
```
kubectl get bucket
```
If no results returned, then proceed with deleting the ProviderConfig resource:
```
kubectl delete -f provider-config.yaml
```

## Want to help make our training material better?
 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/dojo/issues).
 