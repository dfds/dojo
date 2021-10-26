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
