DFDS Crossplane Hackathon - Code kata #1
======================================

This training exercise is a **beginner-level** course on Crossplane that serves as a starting point for Developers looking to onboard the provisioning efforts at DFDS.

## Getting started
These instructions will help you prepare for the kata and ensure that your training machine has the tools installed you will need to complete the assignment(s). If you find yourself in a situation where one of more tools might not be available for your training environment please reach out to your instructor for assistance on how to proceed, post an [issue in our repository](https://github.com/dfds/dojo/issues) or fix it yourself and update the kata via a [pull request](https://github.com/dfds/dojo/pulls).

### Prerequisites
* Kata 0

## Exercise
Your first assignment will see you provision an S3 bucket using a local Kubernetes cluster and the Crossplane AWS provider to act as storage for your files. Sounds simple, no? Let's start then!

The S3 bucket that you are going to provision will be running in a 
[LocalStack](https://github.com/localstack/localstack) environment running in your local Kubernetes cluster. Localstack enables you to provision AWS resources locally on your machine in an offline setup without the need to spin up actual resource on Amazon Cloud and paying additional costs.


### 1. Create a secret containing AWS credentials

Create a creds.conf file with the following content:

```
[default]
aws_access_key_id = test
aws_secret_access_key = test

```

Run the following command to populate a Kubernetes secret using the above file:

```
kubectl create secret generic localstack-creds --from-file=creds=./creds.conf

```

### 2. Add providerconfig.yaml with your AWS credentials
In order to enable Crossplane to provision a S3 bucket in a AWS account or in LocalStack setup we need to add a ProviderConfig containing a set of valid account credentials. In order to accomplish this we will create a file called `providerconfig.yaml` and add the following content:

```
# AWS ProviderConfig that references the secret credentials
apiVersion: aws.crossplane.io/v1beta1
kind: ProviderConfig
metadata:
  name: localstack-aws
spec:
  endpoint:
    hostnameImmutable: true
    url:
      type: Static
      static: http://localstack.default.svc.cluster.local:4566
  credentials:
    source: Secret
    secretRef:
      namespace: default
      name: localstack-creds
      key: credentials
```

Description of the content of the providerconfig: <br/>
`metadata: name: localstack-aws` - is the name of the resource you arr creating<br/>
`spec: secretRef: endpoint` - enables the AWS provider with connection info so you can provision AWS resources in your localstack setup. This is usually not needed when connecting to the actual AWS cloud<br/>
`spec: credentials: source: Secret` - instructs Crossplane that the credentials will be sourced from a secret<br/>
`spec: secretRef: namespace: default` - tells Crossplane that the secret in question is in the default namespace<br/>
`spec: secretRef: name: localstack-creds` - providers the name of the secret that should be referenced<br/>

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
Once Crossplane has gotten a valid ProviderConfig it is able to begin provisioning resources in your LocalStack environment. To create a S3 bucket we need to add the following:


```
apiVersion: s3.aws.crossplane.io/v1beta1
kind: Bucket
metadata:
    name: your-test-bucket
spec:
    forProvider:
        acl: public-read-write
        locationConstraint: us-east-1
    providerConfigRef:
        name: localstack-aws
```

The content of the manifest is explained as follows: <br/>
`kind: Bucket` - is the type of resource we want to provision<br/>
`metadata: name: your-test-bucket` - is the name of the resource your creating<br/>
`spec: forProvider:` - contains the configuration to be passed by the provider in charge of provisioning the S3 Bucket<br/>
`spec: providerConfigRef: name: localstack-creds` - points to the ProviderConfig used for provisioning the S3 Bucket<br/>


### 7. Deploy the S3 bucket manifest

We will deploy manifest into our cluster

```
kubectl apply -f s3bucket.yaml
```

### 8. Check whether the bucket was created

Check the kubernetes deployment
```
kubectl get bucket
NAME               READY   SYNCED   AGE
your-test-bucket   True    True     8s
```

### 9. Verify that the bucket was created in the localstack backend: 
Start a new Terminal window and run the following command.
```
kubectl run aws-cli-runtime --image=luebken/aws-cli-runtime:latest --image-pull-policy='Never'
kubectl exec --stdin --tty aws-cli-runtime -- /bin/bash
```
This will start a AWS CLI terminal that runs inside a pod in Kubernetes 


Configure the AWS CLI using this command
```
# configure the aws cli for localstack setup
# use test/test for key and secret and default for the rest
aws configure
...
```

Ensure the following the values are entered

```
AWS Access Key ID [None]: test
AWS Secret Access Key [None]: test
Default region name [None]: us-east-1
Default output format [None]: json
```
Then execute the following command to list provisiod:

```
aws --endpoint-url=http://localstack.default.svc.cluster.local:4566 s3 ls
2021-10-25 20:44:28 test-bucket
```


### 8. Upload and test a website

Create html and upload it to the bucket:
```
echo "<html>hello from crossplane</html>" > index.html
aws --endpoint-url=http://localstack.default.svc.cluster.local:4566 s3 cp index.html s3://your-test-bucket --acl public-read
upload: ./index.html to s3://test-bucket/index.html
```

Verify the bucket has the html file:
```
aws --endpoint-url=http://localstack.default.svc.cluster.local:4566 s3api head-object --bucket your-test-bucket --key index.html
{
"LastModified": "2021-10-21T11:52:01+00:00",
"ContentLength": 35,
"ETag": "\"b785e6dedf26b0acefc463b9f12a74df\"",
"ContentType": "text/html",
"Metadata": {}
}

curl localstack.default.svc.cluster.local:4566/test-bucket/index.html
<html>hello from crossplane</html>
```

### 12. Cleanup resources

We should clean up resources so that we do not incur any unnecessary costs

Use the AWS CLI window that you started in step 8 to delete all objects from inside the bucket
```
aws s3 rm --endpoint-url=http://localstack.default.svc.cluster.local:4566 s3://your-test-bucket --recursive
```

Delete the bucket using the manifest
```
kubectl delete bucket your-test-bucket
```

Check if S3 bucket has been deleted from the cluster:
```
kubectl get bucket
```

Check if it has also been deleted from localstack
```
aws s3 --endpoint-url=http://localstack.default.svc.cluster.local:4566 ls
```

If no results returned, then proceed with deleting the ProviderConfig resource:
```
kubectl delete -f providerconfig.yaml
```

## Want to help make our training material better?
 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/dojo/issues).
 