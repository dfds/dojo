# ========================================
# CREATE UPDATED BASE IMAGE
# ========================================

FROM python:3.9.10-slim-bullseye AS base

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

ENV AWSCLI_VERSION=1.17.0

RUN python3 -m pip install --upgrade pip \
    && pip3 install pipenv awscli==${AWSCLI_VERSION}


# ========================================
# END
# ========================================

VOLUME /root/.aws
VOLUME /project
WORKDIR /project

ADD /get-file.sh /project/get-file.sh

ENTRYPOINT ["/project/get-file.sh"]