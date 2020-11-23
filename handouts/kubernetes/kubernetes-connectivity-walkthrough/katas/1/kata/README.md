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
FROM ubuntu:18.04 as base
```

Just to explain: <br/>
`FROM ubuntu:18.04 as base` - Instructs Docker to create an `image stage` labeled `base` based on the `ubuntu:18.04` image from DockerHub.

### 3. Setup install-stage and install various dependencies
Once we have selected our base image-stage we can start "compiling" our `install-stage` which take care of installing basic dependencies required for `kubectl` to work:

```
FROM base as install-stage

RUN apt update
RUN apt -y install sudo curl python3-pip apt-transport-https ca-certificates apt-utils
RUN pip3 install --upgrade pip
```

Just to explain: <br/>
`RUN apt -y install sudo curl python3-pip apt-transport-https ca-certificates apt-utils` - Instructs the APT package manager to fetch various dependencies needed by `kubectl`.

### 5. Install kubectl
Having installed the required dependencies we can now begin the process of installing `kubectl` which requires us to add the Google GPG key to our container so we can access their package feed via HTTPs:

```
RUN curl -s https://packages.cloud.google.com/apt/doc/apt-key.gpg | sudo apt-key add -
RUN echo "deb https://apt.kubernetes.io/ kubernetes-xenial main" | sudo tee -a /etc/apt/sources.list.d/kubernetes.list

RUN apt update
RUN apt -y install kubectl
```

Just to explain: <br/>
`curl -s https://packages.cloud.google.com/apt/doc/apt-key.gpg | sudo apt-key add -` - Fetches & adds the GPG key to our local container image.

### 6. Install AWS CLI
After installing Kubectl we need to install the `awscli` so we can use it to authenticate our DFDS user credentials and fetch an access token to interface with the AWS API:

```
RUN pip3 install awscli --upgrade
```

Just to explain: <br/>
`RUN pip3 install awscli --upgrade` - Using Pip3 we install the `awscli` and instruct it to upgrade to the latest available version.

### 7. Install aws-iam-authenticator
Once the `awscli` is in place we need to install the `aws-iam-authenticator` which Amazon EKS uses IAM to provide authentication to Kubernetes clusters:

```
ENV VERSION=1.14.6
ENV BUILD_DATE=2019-08-22
ENV DOWNLOAD_URL=https://amazon-eks.s3-us-west-2.amazonaws.com/${VERSION}/${BUILD_DATE}/bin/linux/amd64/aws-iam-authenticator
ENV LOCAL_FILE=./aws-iam-authenticator

RUN curl -Lo $LOCAL_FILE $DOWNLOAD_URL
RUN chmod +x $LOCAL_FILE
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
FROM install-stage as configure-stage

ENV DOWNLOAD_URL=https://dfds-oxygen-k8s-public.s3-eu-west-1.amazonaws.com/kubeconfig/hellman-saml.config
ENV LOCAL_FILE=~/.kube/hellman-saml.config
ENV KUBECONFIG=~/.kube/hellman-saml.config

RUN mkdir -p $(dirname $LOCAL_FILE)
RUN curl -Lo $LOCAL_FILE $DOWNLOAD_URL

ENV AWS_PROFILE=saml
RUN saml2aws configure --url=https://adfs.dfds.com/adfs/ls/IdpInitiatedSignOn.aspx --idp-provider=ADFS --mfa=Auto --session-duration=28800 --skip-prompt
```

Just to explain: <br/>
`ENV DOWNLOAD_URL=https://dfds-oxygen-k8s-public.s3-eu-west-1.amazonaws.com/kubeconfig/hellman-saml.config` - Uri to the latests known "self-service" configuration for Hellman.<br/>
`RUN saml2aws configure --url=https://adfs.dfds.com/adfs/ls/IdpInitiatedSignOn.aspx --idp-provider=ADFS --mfa=Auto --session-duration=28800 --skip-prompt` - Configures `saml2aws` to target the DFDS ADFS instance in Azure.

### 10. Setup authentication-stage image
The last stage in our "build-chain" is the `authentication-stage` where we will use `saml2aws` to fetch login credentials and launch a `bash` shell with our pre-configured `kubectl` process:

```
FROM configure-stage as authentication-stage

CMD saml2aws login && bash
```

### 12. Build & run container image
Now that we have finalized our `Dockerfile` we can build it out via the `docker build` command and launch it using `docker run` as follows:

```
docker build .
docker run -it -e "SAML2AWS_USERNAME={DFDS_USERNAME}" -e "SAML2AWS_PASSWORD={DFDS_PASSWORD}" {IMAGE_ID}
```

## Want to help make our training material better?
 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/dojo/issues).
