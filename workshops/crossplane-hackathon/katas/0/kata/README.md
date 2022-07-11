DFDS Crossplane Hackathon - Code kata #0
======================================

This training exercise is a **beginner-level** course on Crossplane that serves as a starting point for Developers looking to onboard the provisioning efforts at DFDS.

## Getting started
These instructions will help you prepare for the kata and ensure that your training machine has the tools installed you will need to complete the assignment(s). If you find yourself in a situation where one or more tools might not be available for your training environment please reach out to your instructor for assistance on how to proceed, post an [issue in our repository](https://github.com/dfds/dojo/issues) or fix it yourself and update the kata via a [pull request](https://github.com/dfds/dojo/pulls).

### Prerequisites
* Linux/Unix development enviornment. For Windows users: Windows Subsystem for Linux (WSL) v2, installed and configured in your machine (https://docs.microsoft.com/en-us/windows/wsl/install) and install Ubuntu distribution
* [This repo](https://github.com/dfds/crossplane-offline) is cloned locally in a directory on your machine
* [Docker](https://www.docker.com/get-started)
* [Golang](https://golang.org/doc/install) version 1.6+ is recommended.
* [Kind](https://kind.sigs.k8s.io/)

## Exercise
In the first part of this assignment will see you configuring your localstack enabled cluster on your local machine. You will be making use of Kind cluster using images and scripts provided by Upbound for the purpose of running a cluster and provisioning amazon resources on localstack without an internet connection. Once you have completed this kata you will have an environment ready for the local development environment to run the following katas.

### 1. Prepare Kind cluster images and binaries
This step must be done while you are connected to the internet.

Execute this shell script file from within the cloned crossplane-offline repo folder
```bash
cd crossplane-offline
./online.sh
```
This will pull `Kind` image with required images pre-loaded. Requied CLI tools will also be installed in your local machine
### 2. Test creating a new Kind cluster in offline mode

While being disconnected from Internet, run this command from within the cloned crossplane-offline repo folder
```bash
cd crossplane-offline
./offline.sh
```

This will create a new Kind cluster using the pre-loaded images and configure it with Crossplane with AWS provider and it will also run Localstack inside the cluster.


### 3. Verify setup

Verify kind cluster is running by running kind command
```
kind get clusters 
```
Make sure you are using the correct kubectl context:
```
kubectl config use-context kind-kind
```

Validate that you can access the Kind cluster's API
```
kubectl get nodes
```

Verify crossplane helm is installed
```
helm list -n crossplane-system
```
Verify that the installation has been successful and that the crossplane pods are running
```
kubectl get all -n crossplane-system
```

Verify Localstack helm is installed:
```
helm list
```
Verify that the installation has been successful and that the localstack and k8scr pods are running
```
kubectl get all
```
# Cleaning up
You can simply delete the Kind cluster after workshop is completed by running this command:
```
kind delete clusters kind
```
Then manually remove all yaml manifests created during the workshop.
## Want to help make our training material better?
 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/dojo/issues).