# ========================================
# GENERAL PREREQUISITES
# ========================================

FROM mcr.microsoft.com/powershell:debian-stretch-slim

# Explicitly set USER env variable to accomodate issues with golang code being cross-compiled
ENV USER root

# Versions of software to install
ENV AWSCLI_VERSION="1.16.313"
ENV TERRAFORM_VERSION="0.12.19"
ENV TERRAGRUNT_VERSION="0.21.10"
ENV KUBECTL_VERSION="v1.17.0"
ENV SAML2AWS_VERSION="2.20.0"
# AWS IAM Docs for date https://docs.aws.amazon.com/eks/latest/userguide/install-aws-iam-authenticator.html#w243aac27c11b5b3
ENV AWS_IAM_VERSION="1.14.6"
ENV AWS_IAM_BUILD_DATE="2019-08-22"


# Install general tools
RUN apt-get update \
    && apt-get install -y apt-utils eval curl unzip git bash-completion jq ssh \
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/*

# ========================================
# PYTHON
# ========================================

RUN apt-get update \
    && apt-get install -y python python-pip python3 python3-pip \
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/*

# ========================================
# AWS CLI
# ========================================

RUN python3 -m pip install --upgrade pip \
    && pip3 install pipenv awscli==${AWSCLI_VERSION} \
    && echo "complete -C '$(which aws_completer)' aws" >> ~/.bashrc

# ========================================
# TERRAFORM
# ========================================

RUN curl -L https://releases.hashicorp.com/terraform/${TERRAFORM_VERSION}/terraform_${TERRAFORM_VERSION}_linux_amd64.zip -o terraform.zip \
    && unzip terraform.zip \
    && rm terraform.zip \
    && mv terraform /usr/local/bin/ \
    && terraform -install-autocomplete

# ========================================
# TERRAGRUNT
# ========================================

RUN curl -L https://github.com/gruntwork-io/terragrunt/releases/download/v${TERRAGRUNT_VERSION}/terragrunt_linux_amd64 -o terragrunt \
    && chmod +x terragrunt \
    && mv terragrunt /usr/local/bin/

# ========================================
# KUBECTL
# ========================================

RUN curl -L https://storage.googleapis.com/kubernetes-release/release/${KUBECTL_VERSION}/bin/linux/amd64/kubectl -o /usr/local/bin/kubectl \
    && chmod +x /usr/local/bin/kubectl

# ========================================
# AWS-IAM-AUTHENTICATOR
# ========================================
 
RUN DOWNLOAD_URL=https://amazon-eks.s3-us-west-2.amazonaws.com/${AWS_IAM_VERSION=}/${AWS_IAM_BUILD_DATE}/bin/linux/amd64/aws-iam-authenticator \
    && LOCAL_FILE=./aws-iam-authenticator \
    && curl -Lo $LOCAL_FILE $DOWNLOAD_URL \
    && chmod +x $LOCAL_FILE \
    && mv $LOCAL_FILE /usr/local/bin

# ========================================
# SAML2AWS
# ========================================

RUN DOWNLOAD_URL=https://github.com/Versent/saml2aws/releases/download/v${SAML2AWS_VERSION}/saml2aws_${SAML2AWS_VERSION}_linux_amd64.tar.gz \
    && LOCAL_FILE=./saml2aws.tar.gz \
    && curl -Lo $LOCAL_FILE $DOWNLOAD_URL \
    && tar xvzf $LOCAL_FILE \
    && rm $LOCAL_FILE \
    && mv saml2aws /usr/local/bin
    
CMD [ "bash" ]