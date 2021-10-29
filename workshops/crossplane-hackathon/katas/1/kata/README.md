DFDS Crossplane Hackathon - Code kata #1
======================================

This training exercise is a **beginner-level** course on Crossplane that serves as a starting point for Developers looking to onboard the provisioning efforts at DFDS.

## Getting started
These instructions will help you prepare for the kata and ensure that your training machine has the tools installed you will need to complete the assignment(s). If you find yourself in a situation where one of more tools might not be available for your training environment please reach out to your instructor for assistance on how to proceed, post an [issue in our repository](https://github.com/dfds/dojo/issues) or fix it yourself and update the kata via a [pull request](https://github.com/dfds/dojo/pulls).

### Prerequisites
* Kata 0
* [Docker](https://www.docker.com/get-started) and kubernetes feature is activated
* [AWS CLI](https://aws.amazon.com/cli/)
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

### 2. Install the AWS provider
We need to install the Terraform provider for crossplane to be able to deploy AWS resources
**Note:** We are using a custom build for compatibility with Localstack

```
kubectl crossplane install provider muvaf/provider-aws:v0.19.1-dfds.1
```

### 3. Create a secret containing AWS credentials

Create a creds.conf file with the following content:

```
[default]
aws_access_key_id = test
aws_secret_access_key = test

```

Run the following command to populate a Kubernetes secret using the above file:

```
kubectl create secret generic aws-creds --from-file=creds=./creds.conf

```

### 4. Add providerconfig.yaml with your AWS credentials
In order to enable Crossplane to provision a S3 bucket in your AWS account we need to add a ProviderConfig containing a set of valid account credentials. In order to accomplish this we will create a file called `providerconfig.yaml` and add the following content:

Run the following to obtain the correct URL to place in the `static` attribute in the yaml below:
```
export NODE_PORT=$(kubectl get --namespace default -o jsonpath="{.spec.ports[0].nodePort}" services localstack)
export NODE_IP=$(kubectl get nodes --namespace default -o jsonpath="{.items[0].status.addresses[0].address}")
echo http://$NODE_IP:$NODE_PORT
```

```
---
apiVersion: aws.crossplane.io/v1beta1
kind: ProviderConfig
metadata:
  name: default-aws
spec:
  endpoint:
    hostnameImmutable: true
    url:
      type: Static
      static: http://1.2.3.4:56789
  credentials:
    source: Secret
    secretRef:
      namespace: default
      name: aws-creds
      key: creds
```

Just to explain: <br/>
`metadata: name: default-aws` - is the name of the resource your creating<br/>
`spec: credentials: source: Secret` - instructs Crossplane that the credentials will be sourced from a secret<br/>
`spec: secretRef: namespace: default` - tells Crossplane that the secret in question is in the default namespace<br/>
`spec: secretRef: name: aws-creds` - providers the name of the secret that should be referenced<br/>

### 5. Deploy the ProviderConfig manifest

Deploy this manifest to our cluster. ProviderConfigs exist at the cluster level and can not be namespaced

```
kubectl apply -f providerconfig.yaml
```

Verify that the providerconfig exists

```
kubectl get providerconfig.aws.crossplane.io
```

### 6. Create s3bucket.yaml that keeps your S3 bucket configuration on your filesystem
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
      blockPublicAcls: true
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
    name: default-aws
```

Just to explain: <br/>
`kind: Bucket` - is the type of resource we want to provision<br/>
`metadata: name: your-test-bucket` - is the name of the resource your creating. Replace the value with a name that is globally unique. You might need to do a couple of attempts before you find a good name <br/>
`spec: forProvider:` - contains the configuration to be passed by the provider in charge of provisioning the S3 Bucket<br/>
`spec: providerConfigRef: name: aws-default` - points to the ProviderConfig used for provisioning the S3 Bucket<br/>


### 7. Deploy the S3 bucket manifest

We will deploy manifest into our cluster

```
kubectl apply -f s3bucket.yaml
```

### 8. Upload index.html to S3 bucket
Upload content to S3 bucket using the AWS CLI:

```
aws s3 --endpoint-url=$LOCALSTACK_URL cp index.html s3://your-test-bucket --acl public-read
```

**Note**: Replace your-test-bucket with the name of the bucket you created in step 4

### 9. Verify that index.html has been uploaded to S3 bucket
The following AWS command will list the content of the bucket

```
aws s3 --endpoint-url=$LOCALSTACK_URL ls your-test-bucket
```

**Note**: Replace your-test-bucket with the name of the bucket you created in step 4
### 10. Update S3 bucket configurations
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

### 11. Observe sync status

Describe the S3 bucket to see that there has been a successful request to update the external resource

```
kubectl describe bucket your-test-bucket
```
### 12. Cleanup resources

We should clean up resources so that we do not incur any unnecessary costs

Delete all objects from inside the bucket
```
aws s3 rm --endpoint-url=$LOCALSTACK_URL s3://your-test-bucket --recursive
```

Delete the bucket using the manifest
```
kubectl delete -f s3bucket.yaml
```

Check if S3 bucket has been deleted from the cluster:
```
kubectl get bucket
```

Check if it has also been deleted from localstack:
```
aws s3 --endpoint-url=$LOCALSTACK_URL ls
```

If no results returned, then proceed with deleting the ProviderConfig resource:
```
kubectl delete -f providerconfig.yaml
```

## Want to help make our training material better?
 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/dojo/issues).
 