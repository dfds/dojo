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

***Note*** <br/>
You must have at least v2.0.69 of the Azure CLI installed to use this extension. We can verify the Azure CLI version via the following command: `az --version`.

### 2. Authenticate via Azure CLI
To perform operations against the DFDS Azure cloud we need to authenticate with our Azure AD account to generate the necessary tokens required by the Azure CLI to communicate with the cloud. Thankfully the AZ CLI supports the OAuth2 device code flow, which can be initiated with the following command:

```
az login --allow-no-subscriptions
```

***Note*** <br/>
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

Just to explain: <br/>
`az devops service-endpoint create --service-endpoint-configuration=service_endpoint_configuration.json` - This one-liner uses a pre-configured json file which contains the request payload expected by Azure DevOps REST API.

### 5. Create a pipeline
Once the service connection is in place we can move on to creating our pipeline. Luckily the guys at Redmond has made it easy for us with another one-liner and an interactive process:

```
az pipelines create --name {ado-pipeline-name} --skip-first-run --repository {ado-project-repo-name} --branch master --repository-type tfsgit
```

Just to explain: <br/>
`az pipelines create --name {ado-pipeline-name} --skip-first-run --repository {ado-project-repo-name} --branch master --repository-type tfsgit` - Launches an interactive console that guides users through setting up a new pipeline for their repository.

### 5. Update pipeline (use service conn + apply step)
TODO

## Want to help make our training material better?
 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/dojo/issues).
