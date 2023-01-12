# ========================================
# CREATE UPDATED BASE IMAGE
# ========================================

FROM python:slim-bullseye AS base

RUN apt-get update \
    && apt-get dist-upgrade -y \
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/*


# ========================================
# GENERAL PREREQUISITES
# ========================================

FROM base

RUN apt-get update \
    && apt-get install -y curl jq \
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/*


# ========================================
# AWS CLI
# ========================================

ENV AWS_CLI_VERSION=2.7.35

RUN curl -s https://awscli.amazonaws.com/awscli-exe-linux-x86_64-${AWS_CLI_VERSION}.zip -o awscliv2.zip \
    && unzip awscliv2.zip \
    && ./aws/install \
    && rm -rf ./aws \
    && rm -f awscliv2.zip


# ========================================
# END
# ========================================

VOLUME /root/.aws
VOLUME /project
WORKDIR /project

ADD /get-file.sh /project/get-file.sh

ENTRYPOINT ["/project/get-file.sh"]
