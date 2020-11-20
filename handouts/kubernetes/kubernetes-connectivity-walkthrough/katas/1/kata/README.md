DFDS Kubectl Training - Code kata #1
======================================

This training exercise is a **beginner-level** course on Kubectl and serves as a starting point for developers looking to connect to Hellman.

## Getting started
These instructions will help you prepare for the kata exercise and ensure that your local machine has the tools installed you will need to complete the assignment(s). If you find yourself in a situation where one of more tools might not be available for your training environment please reach out to your instructor for assistance on how to proceed, post an [issue in our repository](https://github.com/dfds/dojo/issues) or fix it yourself and create a [pull request](https://github.com/dfds/dojo/pulls).

### Prerequisites
* [Docker](https://www.docker.com/products/docker-desktop)

## Exercise
In this exercise we will learn how to connect to Hellman via Docker so we have a portable process that can be re-used in different contexts (e.g. local development, Azure DevOps pipelines, etc)

### 1. Create your kata directory
First we setup a directory for our exercise files. It's pretty straight forward:

```
mkdir kata1
cd kata1
```

### 2. Fetch Capability ARN
TODO

### 3. Create a Dockerfile
TODO

### 4. Specify base image
TODO

```
FROM ubuntu:18.04 as base
```

Just to explain: <br/>
`FROM ubuntu:18.04 as base` - TODO.

### 5. Setup install-stage and install various dependencies
TODO

```
FROM base as install-stage

RUN apt update
RUN apt -y install sudo curl python3-pip apt-transport-https ca-certificates apt-utils
RUN pip3 install --upgrade pip
```

Just to explain: <br/>
`RUN apt -y install sudo curl python3-pip apt-transport-https ca-certificates apt-utils` - TODO.

### 6. Install kubectl
TODO

```
RUN curl -s https://packages.cloud.google.com/apt/doc/apt-key.gpg | sudo apt-key add -
RUN echo "deb https://apt.kubernetes.io/ kubernetes-xenial main" | sudo tee -a /etc/apt/sources.list.d/kubernetes.list

RUN apt update
RUN apt -y install kubectl
```

Just to explain: <br/>
`curl -s https://packages.cloud.google.com/apt/doc/apt-key.gpg | sudo apt-key add -` - TODO.

### 7. Install AWS CLI
TODO

```
RUN pip3 install awscli --upgrade
```

Just to explain: <br/>
`RUN pip3 install awscli --upgrade` - TODO.

### 8. Install aws-iam-authenticator
TODO

```
ENV VERSION=1.14.6
ENV BUILD_DATE=2019-08-22
ENV DOWNLOAD_URL=https://amazon-eks.s3-us-west-2.amazonaws.com/${VERSION}/${BUILD_DATE}/bin/linux/amd64/aws-iam-authenticator
ENV LOCAL_FILE=./aws-iam-authenticator

RUN curl -Lo $LOCAL_FILE $DOWNLOAD_URL
RUN chmod +x $LOCAL_FILE
RUN sudo mv $LOCAL_FILE /usr/local/bin
```

### 9. Install saml2aws
TODO

```
ENV VERSION=2.20.0
ENV DOWNLOAD_URL=https://github.com/Versent/saml2aws/releases/download/v${VERSION}/saml2aws_${VERSION}_linux_amd64.tar.gz
ENV LOCAL_FILE=./saml2aws.tar.gz

RUN curl -Lo $LOCAL_FILE $DOWNLOAD_URL
RUN tar xvzf $LOCAL_FILE
RUN rm $LOCAL_FILE
RUN sudo mv saml2aws /usr/local/bin
```

### 10. Setup configure-stage
TODO

```
FROM install-stage as configure-stage
```

### 11. Configure Kubectl
TODO

```
ENV DOWNLOAD_URL=https://dfds-oxygen-k8s-public.s3-eu-west-1.amazonaws.com/kubeconfig/hellman-saml.config
ENV LOCAL_FILE=~/.kube/hellman-saml.config
ENV KUBECONFIG=~/.kube/hellman-saml.config

RUN mkdir -p $(dirname $LOCAL_FILE)
RUN curl -Lo $LOCAL_FILE $DOWNLOAD_URL
```

Just to explain: <br/>
`ENV DOWNLOAD_URL=https://dfds-oxygen-k8s-public.s3-eu-west-1.amazonaws.com/kubeconfig/hellman-saml.config` - TODO.

### 12. Configure saml2aws
TODO

```
ENV AWS_PROFILE=saml
RUN saml2aws configure --url=https://adfs.dfds.com/adfs/ls/IdpInitiatedSignOn.aspx --idp-provider=ADFS --mfa=Auto --session-duration=28800 --skip-prompt
```

Just to explain: <br/>
`RUN saml2aws configure --url=https://adfs.dfds.com/adfs/ls/IdpInitiatedSignOn.aspx --idp-provider=ADFS --mfa=Auto --session-duration=28800 --skip-prompt` - TODO.

### 13. Setup authentication-stage image
TODO

```
FROM configure-stage as authentication-stage
```

### 13. Launch saml2aws login command and leave user with a bash shell
TODO

```
CMD saml2aws login && bash
```

## Want to help make our training material better?
 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/dojo/issues).