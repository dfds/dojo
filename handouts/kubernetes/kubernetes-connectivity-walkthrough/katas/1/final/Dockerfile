# Base image
FROM ubuntu:18.04 as base

# install-stage image
FROM base as install-stage

## Install core packages
RUN apt update
RUN apt -y install sudo curl python3-pip apt-transport-https ca-certificates apt-utils
RUN pip3 install --upgrade pip

## Add Kubernetes repository list
RUN curl -s https://packages.cloud.google.com/apt/doc/apt-key.gpg | sudo apt-key add -
RUN echo "deb https://apt.kubernetes.io/ kubernetes-xenial main" | sudo tee -a /etc/apt/sources.list.d/kubernetes.list

## Install kubectl
RUN apt update
RUN apt -y install kubectl

## Install AWS CLI
RUN pip3 install awscli --upgrade

## Install aws-iam-authenticator
ENV VERSION=1.14.6
ENV BUILD_DATE=2019-08-22
ENV DOWNLOAD_URL=https://amazon-eks.s3-us-west-2.amazonaws.com/${VERSION}/${BUILD_DATE}/bin/linux/amd64/aws-iam-authenticator
ENV LOCAL_FILE=./aws-iam-authenticator

RUN curl -Lo $LOCAL_FILE $DOWNLOAD_URL
RUN chmod +x $LOCAL_FILE
RUN sudo mv $LOCAL_FILE /usr/local/bin

## Install saml2aws
ENV VERSION=2.20.0
ENV DOWNLOAD_URL=https://github.com/Versent/saml2aws/releases/download/v${VERSION}/saml2aws_${VERSION}_linux_amd64.tar.gz
ENV LOCAL_FILE=./saml2aws.tar.gz

RUN curl -Lo $LOCAL_FILE $DOWNLOAD_URL
RUN tar xvzf $LOCAL_FILE
RUN rm $LOCAL_FILE
RUN sudo mv saml2aws /usr/local/bin

# Configure-stage image
FROM install-stage as configure-stage

## Configure Kubectl
ENV DOWNLOAD_URL=https://dfds-oxygen-k8s-public.s3-eu-west-1.amazonaws.com/kubeconfig/hellman-saml.config
ENV LOCAL_FILE=~/.kube/hellman-saml.config
ENV KUBECONFIG=~/.kube/hellman-saml.config

RUN mkdir -p $(dirname $LOCAL_FILE)
RUN curl -Lo $LOCAL_FILE $DOWNLOAD_URL

## Configure saml2aws
ENV AWS_PROFILE=saml
RUN saml2aws configure --url=https://adfs.dfds.com/adfs/ls/IdpInitiatedSignOn.aspx --idp-provider=ADFS --mfa=Auto --session-duration=28800 --skip-prompt

# authentication-stage image
FROM configure-stage as authentication-stage

CMD saml2aws login && bash