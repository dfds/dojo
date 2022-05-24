DFDS ADO Training - Code Kata #2
======================================

This training exercise is a **beginner-level** course on DevOps at DFDS and serves as a starting point for developers looking to onboard [Azure DevOps](https://dev.azure.com/dfds).

## Getting started
These instructions will help you prepare for the kata and ensure that your training machine has the tools installed you will need to complete the assignment(s). If you find yourself in a situation where one of more tools might not be available for your training environment please reach out to your instructor for assistance on how to proceed, post an [issue in our repository](https://github.com/dfds/dojo/issues) or fix it yourself and update the kata via a [pull request](https://github.com/dfds/dojo/pulls).

### Prerequisites
* [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli)

## Exercise
In this exercise we will learn how we can use the Azure DevOps CLI to add a brand new pipeline and connect it to Hellman via a service account..

### 1. Install Azure DevOps Extension
In order to get our CLI working with Azure DevOps we need to install the [Azure DevOps Extension for Azure CLI](https://github.com/Azure/azure-devops-cli-extension). This is thankfully a very simply step which can be accomplished with a one-liner as follows:

```
az extension add --name azure-devops
```

***Note***
You must have at least v2.0.69 of the Azure CLI installed to use this extension. We can verify the Azure CLI version via the following command: `az --version`.

### 2. Authenticate via Azure CLI
To perform operations against the DFDS Azure cloud we need to authenticate with our DFDS login credentials.  This will generate the tokens required by the Azure CLI to communicate with the cloud. Thankfully the AZ CLI supports the OAuth2 device code flow, which can be initiated with the following command:

```
az login --allow-no-subscriptions
```

***Note***
DFDS does not have individual subscriptions for all teams and yours might not be an exception. This is why we use the `--allow-no-subscriptions` flag to instruct the Azure CLI to service our authentication attempt regardless of any subscription affiliations we might have associated with our Azure AD users.

### 3. Configure Azure DevOps CLI
Once we get going with the CLI there will be certain arguments that are common to most commands and thus it makes sense to leverage the `configure` command to set some sensible defaults for the `--organization` and `--project` flags:

```
az devops configure --defaults organization=https://dev.azure.com/{ado-org-name} project={ado-project-name} --use-git-aliases true
```

### 4. Create a service account
Having setup the CLI we can now commence with the process of creating the service account required by Azure DevOps to connect with our Hellman cluster:

```
az devops service-endpoint create --service-endpoint-configuration=service_endpoint_configuration.json
```

This above one-liner uses a pre-configured json file which contains the request payload expected by Azure DevOps REST API.  THe file can be found within the folder named 'final' in this Kata.

### 5. Create a Repository

In an upcoming step we will create a pipeline.  This will be created and then stored in a Repository.  If you already have an Azure DevOps Repo that you wish to use then you can skip this step, or, if you wish, you can execute the command below to have a new Azure DevOps Repo created:

```
az repos create --name {my-new-repo-name}
```

Be sure to replace {my-new-repo-name} with a unique name and note that the curly braces are not required.  If the process of creating the new Repo is succesful then a JSON payload with the details for the new Repo will be returned.

### 6. Create a pipeline
Once the service connection is in place we can move on to creating our pipeline. Luckily the guys at Redmond have made it easy for us with another one-liner and an interactive process.  Be sure to replace the variables in curly braces with suitable names for the pipeline and the ADO Repository where the resultant pipeline will be stored.

```
az pipelines create --name {ado-pipeline-name} --skip-first-run --repository {ado-project-repo-name} --branch main --repository-type tfsgit
```

Once the interactive process launches select the default option (1) to setup a starter pipeline and in the following step choose to edit the pipeline using option (2) which will launch your default text editor. Once the editor loads copy the following contents to the azure-pipelines.yaml file and substitute the required template values (namespace & arguments):

```
# Docker
# Build a Docker image
# https://docs.microsoft.com/azure/devops/pipelines/languages/docker

trigger:
- main

resources:
- repo: self

stages:
- stage: Build
  displayName: Build image
  jobs:
  - job: Build
    displayName: Build
    pool:
      vmImage: 'ubuntu-latest'
    steps:
      # Apply Kubernetes manifests
      - task: Kubernetes@1
        displayName: 'Apply manifests'
        inputs:
          connectionType: Kubernetes Service Connection
          kubernetesServiceEndpoint: 'Hellman-Sample'
          namespace: '{target-namespace-in-hellman}'
          command: apply
          arguments: '-f $(Build.SourcesDirectory)/{path-to-folder-container-k8s-assets}/'
```

The above command launches an interactive console that guides users through setting up a new pipeline for their repository.

Once you have made the required changes you can save and close your text editor.  You will then be prompted if you wish to proceed in creating the pipeline.  Simply select the default choice of 1 to continue.  You will then be asked how you want to commit the files to the repository.  Choose option 1 to have the change committed directly to the master branch.

And, that's it.  The new pipeline is now created and the generated YAML file can be located in the ADO repository.

### 7. Cleanup Steps

If you wish you may also complete this final step in order to clean up the content that has just been created.

```
# First you need to locate the ID of the repository to delete using this command
az repos show --repository {my-repository-name}

# Note down the GUID against id in the returned JSON and run this command
az repos delete --id {repository-id}

# Use this command to locate the service-endpoint we previously created
az devops service-endpoint list --query "[?name=='Kubernetes Sample Connection']"

# In the returned JSON note down the GUID against the id field.
# Be careful to choose the id related to the endpoint and not the one in the 'createdBy' block.
# Use the ID in the following command
az devops service-endpoint delete --id {service-endpoint-id}
```

This concludes the process of cleaning up the environment.

## Want to help make our training material better?
 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/dojo/issues).
