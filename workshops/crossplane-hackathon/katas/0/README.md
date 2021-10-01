DFDS Crossplane Hackathon - Code kata #0
======================================

This training exercise is a **beginner-level** course on Crossplane that serves as a starting point for Developers looking to onboard the provisioning efforts at DFDS.

## Getting started
These instructions will help you prepare for the kata and ensure that your training machine has the tools installed you will need to complete the assignment(s). If you find yourself in a situation where one of more tools might not be available for your training environment please reach out to your instructor for assistance on how to proceed, post an [issue in our repository](https://github.com/dfds/dojo/issues) or fix it yourself and update the kata via a [pull request](https://github.com/dfds/dojo/pulls).

### Prerequisites
* [Docker](https://www.docker.com/get-started) and kubernetes feature is activated
* [AWS CLI](https://aws.amazon.com/cli/)
* [Terraform](https://learn.hashicorp.com/tutorials/terraform/install-cli)

## Exercise
In the first part of this assignment will see you setup the [LocalStack](https://github.com/localstack/localstack) enviornment on your machine. Localstack enables you to provision AWS resources on your machine without Internet connection.
In the second part you will provision the AWS resources needed in Kata 2 which basically allow Terraform provider for Crossplane to keep state configurations on your local machine

### 1. Create docker-compose.yml for running localstack in docker container
```
version: "3.2"
services:
  localstack:
    image: localstack/localstack:latest
    container_name: localstack
    ports:
      - "4563-4599:4563-4599"
      - "8080:8080"
    environment:
      - DATA_DIR=/tmp/localstack/data
      - DEBUG=1
      - SERVICES=${SERVICES- }
      - DISABLE_CORS_CHECKS=1
    volumes:
      - "./.localstack:/tmp/localstack"
      - "/var/run/docker.sock:/var/run/docker.sock"
```

### 2. Start docker container and LocalStack
Make sure you are in the same directory as docker-compose.yml
Run the following command:
```
docker-compose up
```

### 3. Create Terraform state configurations for your LocalStack environment ## Change to use plain AWS CLI commands:
You will configure an S3 bucket to keep the Terraform state and a DynamoDB to keep the Terraform locks

Create tf.yaml in your local filesystem:
```
provider "aws" {
  region                      = "us-east-1"
  access_key                  = "foo"
  secret_key                  = "bar"
  skip_credentials_validation = true
  skip_requesting_account_id  = true
  skip_metadata_api_check     = true
  s3_force_path_style         = true
  endpoints {
    apigateway     = "http://host.docker.internal:4566"
    cloudformation = "http://host.docker.internal:4566"
    cloudwatch     = "http://host.docker.internal:4566"
    dynamodb       = "http://host.docker.internal:4566"
    es             = "http://host.docker.internal:4566"
    firehose       = "http://host.docker.internal:4566"
    iam            = "http://host.docker.internal:4566"
    kinesis        = "http://host.docker.internal:4566"
    lambda         = "http://host.docker.internal:4566"
    route53        = "http://host.docker.internal:4566"
    redshift       = "http://host.docker.internal:4566"
    s3             = "http://host.docker.internal:4566"
    secretsmanager = "http://host.docker.internal:4566"
    ses            = "http://host.docker.internal:4566"
    sns            = "http://host.docker.internal:4566"
    sqs            = "http://host.docker.internal:4566"
    ssm            = "http://host.docker.internal:4566"
    stepfunctions  = "http://host.docker.internal:4566"
    sts            = "http://host.docker.internal:4566"
  }
}

resource "aws_s3_bucket" "terraform_state" {
  bucket = "terraform-state"
  acl    = "private"

  versioning {
    enabled = true
  }

  server_side_encryption_configuration {
    rule {
      apply_server_side_encryption_by_default {
        sse_algorithm = "AES256"
      }
    }
  }

  lifecycle {
    prevent_destroy = true
  }
}

resource "aws_s3_bucket_public_access_block" "terraform_state_access" {
  bucket = aws_s3_bucket.terraform_state.id

  block_public_acls       = true
  ignore_public_acls      = true
  block_public_policy     = true
  restrict_public_buckets = true
}

resource "aws_dynamodb_table" "terraform_state_lock" {
  name           = "terraformlock"
  read_capacity  = 5
  write_capacity = 5
  billing_mode   = "PAY_PER_REQUEST"
  hash_key       = "LockID"

  attribute {
    name = "LockID"
    type = "S"
  }
}
```
### 4. Create Terraform state resources for your Terraform setup in your LocalStack environment
Apply Terraform resources in your LocalStack environment:

```
terraform init
```

```
terraform apply 
```

hit yes when prompted



### 5. Check resources running in LocalStack

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
aws --endpoint-url=http://host.docker.internal:4566 s3 ls
```
Then run AWS command to check terraformlock bucket
```
aws --endpoint-url=http://host.docker.internal:4566 dynamodb list-tables
```
