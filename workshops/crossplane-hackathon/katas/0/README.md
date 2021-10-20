DFDS Crossplane Hackathon - Code kata #0
======================================

This training exercise is a **beginner-level** course on Crossplane that serves as a starting point for Developers looking to onboard the provisioning efforts at DFDS.

## Getting started
These instructions will help you prepare for the kata and ensure that your training machine has the tools installed you will need to complete the assignment(s). If you find yourself in a situation where one of more tools might not be available for your training environment please reach out to your instructor for assistance on how to proceed, post an [issue in our repository](https://github.com/dfds/dojo/issues) or fix it yourself and update the kata via a [pull request](https://github.com/dfds/dojo/pulls).

### Prerequisites
* [Docker](https://www.docker.com/get-started) and kubernetes feature is activated
* [AWS CLI](https://aws.amazon.com/cli/)

## Exercise
In the first part of this assignment will see you setup the [LocalStack](https://github.com/localstack/localstack) environment in your machine's Kubernetes cluster. Localstack enables you to provision AWS resources on your machine without Internet connection.
In the second part you will provision the AWS resources needed in Kata 2 which basically allow Terraform provider for Crossplane to keep state configurations on your local machine


### 1. Create a values.yaml to enable persistence in Localstack helm chart
Create and save the following file as values.yaml
```
persistence:
  enabled: true
```

### 2. Install Localstack Helm Chart
```
helm repo add localstack-repo https://helm.localstack.cloud
helm upgrade --install localstack localstack-repo/localstack -f values.yaml
```
`TODO: Add a local copy of the helm chart to machines so there is no reliance on Internet connection`

### 3. Obtain the Localstack Endpoint Port from your Installation

```
export NODE_PORT=$(kubectl get --namespace default -o jsonpath="{.spec.ports[0].nodePort}" services localstack)
export LOCALSTACK_URL=http://localhost:$NODE_PORT
```

### 4. Create Terraform state configurations for your LocalStack environment
You will configure an S3 bucket to keep the Terraform state and a DynamoDB to keep the Terraform locks

Run the following commands in the AWS CLI enabled terminal:
```
aws --endpoint-url=$LOCALSTACK_URL s3api create-bucket --bucket terraform-state --region us-east-1 --acl private

aws --endpoint-url=$LOCALSTACK_URL s3api put-bucket-encryption \
    --bucket terraform-state \
    --server-side-encryption-configuration '{"Rules": [{"ApplyServerSideEncryptionByDefault": {"SSEAlgorithm": "AES256"}}]}'


aws --endpoint-url=$LOCALSTACK_URL s3api put-bucket-versioning --bucket terraform-state --versioning-configuration Status=Enabled


# Dynamodb
aws --endpoint-url=$LOCALSTACK_URL dynamodb create-table \
    --table-name terraformlock \
    --attribute-definitions AttributeName=LockID,AttributeType=S \
    --key-schema AttributeName=LockID,KeyType=HASH \
    --provisioned-throughput ReadCapacityUnits=1,WriteCapacityUnits=1
```
### 6. Check resources running in LocalStack

Configure you CLI environment as follows:
```
aws configure

AWS Access Key ID: dummy 
AWS Secret Access Key: dummy
Default region name: us-east-1
Default output format: <Enter>

```
Then run AWS command to check terraform-state bucket
```
aws --endpoint-url=$LOCALSTACK_URL s3 ls
```
Then run AWS command to check terraformlock bucket
```
aws --endpoint-url=$LOCALSTACK_URL dynamodb list-tables
```
