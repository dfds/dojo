# Base image
FROM ubuntu:18.04 as base

# install-stage image
FROM base as install-stage

## Install core packages
RUN apt update
RUN apt -y install sudo curl python3-pip apt-transport-https ca-certificates apt-utils unzip
RUN pip3 install --upgrade pip

## Add Kubernetes repository list
RUN curl -s https://packages.cloud.google.com/apt/doc/apt-key.gpg | sudo apt-key add -
RUN echo "deb https://apt.kubernetes.io/ kubernetes-xenial main" | sudo tee -a /etc/apt/sources.list.d/kubernetes.list

## Install kubectl
RUN apt update
RUN apt -y install kubectl=1.23.15-00

## Install AWS CLI
ENV AWS_CLI_VERSION=2.7.35

ADD src/aws-cli.asc /

RUN curl -s https://awscli.amazonaws.com/awscli-exe-linux-x86_64-${AWS_CLI_VERSION}.zip -o awscliv2.zip \
    && curl https://awscli.amazonaws.com/awscli-exe-linux-x86_64-${AWS_CLI_VERSION}.zip.sig  -o awscliv2.sig \
    && gpg --import aws-cli.asc \
    && gpg --verify awscliv2.sig awscliv2.zip \
    && unzip awscliv2.zip \
    && ./aws/install \
    && rm -rf ./aws \
    && rm -f awscliv2.zip aws-cli.asc awscliv2.sig

## Install saml2aws
ENV VERSION=2.36.2
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
