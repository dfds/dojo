# Build arg defaults
ARG UBUNTU_VERSION=18.04

# Initialize new build stage
FROM ubuntu:${UBUNTU_VERSION}

# Set environment parameters
ENV DEBIAN_FRONTEND=noninteractive

#Change source protocol from http(s) to ftp to avoid problems with certain internal firewall and cloud vendors
#RUN sed -i s/http/ftp/ /etc/apt/sources.list
#RUN sed -i s/https/ftp/ /etc/apt/sources.list

# Add support for en_US UTF8
RUN apt-get update && apt-get install -y locales && rm -rf /var/lib/apt/lists/* \
    && localedef -i en_US -c -f UTF-8 -A /usr/share/locale/locale.alias en_US.UTF-8

# Set LANG environment variable to en_US.utf8
ENV LANG en_US.utf8

# Install apt-utils
RUN apt-get update && apt-get install -y apt-utils

# Install apt-transport-https package
RUN apt-get update && apt-get install -y apt-transport-https

# Install sudo
RUN apt-get update && apt-get install -y sudo && rm -rf /var/lib/apt/lists/*

# Install iproute2 (used for ip cmd)
RUN apt-get update && apt-get install -y iproute2

# Install WGET
RUN apt-get update && apt-get install -y wget

# Install software-properties-common package
RUN apt-get install -y software-properties-common

# Download the Microsoft repository GPG keys
RUN wget -q https://packages.microsoft.com/config/ubuntu/18.04/packages-microsoft-prod.deb

# Register the Microsoft repository GPG keys
RUN sudo dpkg -i packages-microsoft-prod.deb

#Install linux dependencies required by dotnet core SDK
RUN sudo apt-get update && sudo apt-get install -y lsb-release
RUN sudo apt-get update && sudo apt-get install -y dirmngr

#Install dotnet core SDK
RUN sudo apt-get update && sudo apt-get install -y dotnet-sdk-3.1

# Install PowerShell
RUN sudo apt-get update && sudo apt-get install -y powershell

#Install Azure CLI
#RUN AZ_REPO=$(lsb_release -cs) 
#RUN echo "deb [arch=amd64] https://packages.microsoft.com/repos/azure-cli/ jessie main" | sudo tee /etc/apt/sources.list.d/azure-cli.list
#RUN sudo apt-get update && sudo apt-get install azure-cli