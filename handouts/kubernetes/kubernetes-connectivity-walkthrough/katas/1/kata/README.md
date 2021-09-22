DFDS Kubectl Training - Code kata #1
======================================

This training exercise is a **beginner-level** course on Kubectl which serves as a starting point for developers looking to connect with Hellman.

## Getting started
These instructions will help you prepare for the kata exercise and ensure that your local machine has the tools installed you will need to complete the assignment(s). If you find yourself in a situation where one of more tools might not be available for your training environment please reach out to your instructor for assistance on how to proceed, post an [issue in our repository](https://github.com/dfds/dojo/issues) or fix it yourself and create a [pull request](https://github.com/dfds/dojo/pulls).

### Prerequisites
* [Docker](https://www.docker.com/products/docker-desktop)

## Exercise
In this exercise we will learn how to connect to Hellman using a multi-stage Docker container that contains a pre-configured `kubectl` process that can be re-used in different contexts (e.g. local development, Azure DevOps pipelines, etc).

### 1. Create your kata directory
First we setup a directory for our exercise files. It's pretty straight forward:

```
mkdir kata1
cd kata1
```

### 2. Create a Dockerfile & specify base image
Next we create a `Dockerfile` and add the following content:

```
# Instructs Docker to create an 'image stage' labelled 'base' based on the 'ubuntu:18.04' image from DockerHub
FROM ubuntu:18.04 as base
```

### 3. Setup install-stage and install various dependencies
Once we have selected our base image-stage we can start "compiling" our `install-stage` which take care of installing basic dependencies required for `kubectl` to work:

```
# Use the previously defined stage named 'base' as the starting point
FROM base as install-stage

# Update packages using the APT package manager
RUN apt update

# Install required packages using the APT package manager
RUN apt -y install sudo curl python3-pip apt-transport-https ca-certificates apt-utils

# Upgrade the version of PIP
RUN pip3 install --upgrade pip
```
### 5. Install kubectl
Having installed the required dependencies we can now begin the process of installing `kubectl` which requires us to add the Google GPG key to our container so we can access their package feed via HTTPs:

```
# Use CURL to retrieve Googles GPG key and pipe the obtained key to an apt-key command that will add it into the keystore
RUN curl -s https://packages.cloud.google.com/apt/doc/apt-key.gpg | sudo apt-key add -

# Use an echo command to display a new package source, and use tee to write the source to the list of local APT sources
RUN echo "deb https://apt.kubernetes.io/ kubernetes-xenial main" | sudo tee -a /etc/apt/sources.list.d/kubernetes.list

# Update the package manager
RUN apt update

# Install Kubectl.  The prior steps configured the package manager so it could locate and obtain this package.
RUN apt -y install kubectl
```
### 6. Install AWS CLI
After installing Kubectl we need to install the `awscli` so we can use it to authenticate our DFDS user credentials and fetch an access token to interface with the AWS API:

```
# Install the awscli package and ensure that it's upgraded to the latest available version
RUN pip3 install awscli --upgrade
```
### 7. Install aws-iam-authenticator
Once the `awscli` is in place we need to install the `aws-iam-authenticator`.  This allows AWS IAM credentials to be used to authenticate to a Kubernetes cluster.
```
# Set some environment varaibles
ENV VERSION=1.14.6
ENV BUILD_DATE=2019-08-22
ENV DOWNLOAD_URL=https://amazon-eks.s3-us-west-2.amazonaws.com/${VERSION}/${BUILD_DATE}/bin/linux/amd64/aws-iam-authenticator
ENV LOCAL_FILE=./aws-iam-authenticator

# Use CURL to obtain the required file from the S3 bucket
RUN curl -Lo $LOCAL_FILE $DOWNLOAD_URL

# Set the permission on the downloaded file to allow it to be executed
RUN chmod +x $LOCAL_FILE

# And move the file to /usr/local/bin so it's stored with other binaries and included on the search path
RUN sudo mv $LOCAL_FILE /usr/local/bin
```

### 8. Install saml2aws
The last piece of the puzzle in our `install-stage` is the `saml2aws` which wraps the `awscli` and makes it possible for us to authenticate via SAML2:

```
ENV VERSION=2.20.0
ENV DOWNLOAD_URL=https://github.com/Versent/saml2aws/releases/download/v${VERSION}/saml2aws_${VERSION}_linux_amd64.tar.gz
ENV LOCAL_FILE=./saml2aws.tar.gz

RUN curl -Lo $LOCAL_FILE $DOWNLOAD_URL
RUN tar xvzf $LOCAL_FILE
RUN rm $LOCAL_FILE
RUN sudo mv saml2aws /usr/local/bin
```

### 9. Setup configure-stage
Once our base `install-stage` is finalized we can commence to the `configure-stage` where we will configure `kubectl` to access Hellman:

```
# Define the starting point for this stage
FROM install-stage as configure-stage

# Define an environment variable that defines the URL to the latests known "self-service" configuration for Hellman.
ENV DOWNLOAD_URL=https://dfds-oxygen-k8s-public.s3-eu-west-1.amazonaws.com/kubeconfig/hellman-saml.config

# Additional variables for the local copy of the downloaded configuration
ENV LOCAL_FILE=/root/.kube/hellman-saml.config
ENV KUBECONFIG=/root/.kube/hellman-saml.config

# Create the local directory where the file needs to be placed
RUN mkdir -p $(dirname $LOCAL_FILE)

# Use CURL to download the file from S3 into the local file system
RUN curl -Lo $LOCAL_FILE $DOWNLOAD_URL

# Set a further environment variable
ENV AWS_PROFILE=saml

# Configure the SAML2AWS command line tool for future usage.  This tells it to use DFDS' ADFS instance in Azure when handling SAML requests.
RUN saml2aws configure --url=https://adfs.dfds.com/adfs/ls/IdpInitiatedSignOn.aspx --idp-provider=ADFS --mfa=Auto --session-duration=28800 --skip-prompt
```
### 10. Setup authentication-stage image
The last stage in our "build-chain" is the `authentication-stage` where we will use `saml2aws` to fetch login credentials and launch a `bash` shell with our pre-configured `kubectl` process:

```
# Define the starting point for this stage
FROM configure-stage as authentication-stage

# Execute a logon process using SAML2AWS and then move into a BASH shell
CMD saml2aws login && bash
```
### 12. Build & run container image
Now that we have finalized our `Dockerfile` we can build it out via the `docker build` command and launch it using `docker run` as shown below.  Please remember to replace DFDS_USERNAME and DFDS_PASSWORD with your credentials.  The user name should be provided in UPN format, for example pwes@dfds.com.

```
docker build . -t kubectl-kata:latest
docker run --name kubectl-kata -it -e "SAML2AWS_USERNAME={DFDS_USERNAME}" -e "SAML2AWS_PASSWORD={DFDS_PASSWORD}" kubectl-kata
```

Because credentials were passed into the 'docker run' command in the form of environment variables it should not be necessary for you to manually enter the credentials.  You will be prompted for a username but can simply hit RETURN twice to skip both the username and password prompts.  You will then be prompted to choose a role from a list.

At this point you can type and the list will filter to try to match what you have typed.  Try to search for the word Cloud and then select either the CloudAdmin or CloudEngineer role by using the up and down arrow keys followed by RETURN.  If everything worked as expected then you should be at a hash prompt where you can try out some kubectl commands.  Some example commands to try are shown below.

```
# Display all nodes in the K8s cluster
kubectl get nodes

# Show me the pods in the selfservice namespace
kubectl get pods -n selfservice

# Show me the services that are defined in the selfservice namespace
kubectl get service -n selfservice
```

When you have finished trying out the commands press CTRL+D to disconnect from the container.
### 13. Cleaning up
The container will have automatically terminated when you disconnected but the inactive container and Docker image will persist.  If you wish to clean these up then follow these steps.

```
# Delete the Docker container instance
kubectl rm kubectl-kata

# And delete the Docker image that was used by the container
kubectl image rm kubectl-kata
```
## Want to help make our training material better?
 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/dojo/issues).
