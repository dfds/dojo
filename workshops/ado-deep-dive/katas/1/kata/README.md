DFDS ADO Training - Code Kata #1
======================================

This training exercise is a **beginner-level** course on DevOps at DFDS and serves as a starting point for developers looking to onboard [Azure DevOps](https://dev.azure.com/dfds).

## Getting started
These instructions will help you prepare for the kata and ensure that your training machine has the tools installed you will need to complete the assignment(s). If you find yourself in a situation where one of more tools might not be available for your training environment please reach out to your instructor for assistance on how to proceed, post an [issue in our repository](https://github.com/dfds/dojo/issues) or fix it yourself and update the kata via a [pull request](https://github.com/dfds/dojo/pulls).

### Prerequisites
* [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli)

## Exercise
In this exercise we will learn how we can use the Azure DevOps CLI to add a brand new code repository to our ADO project(s).

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
az devops configure --defaults organization=https://dev.azure.com/{my-unique-org-name} project=ADODD-Demo --use-git-aliases true
```

### 4. Create a new repository
Having configured the CLi for use lets try out the `configure` command to create a new repository named `my-repo`:

```
az repos create --name my-repo
```

***Note*** <br/>
Remember to record the ID of the newly created repo. You will need it later. If you forgot to record it run the `az repos show -r my-repo` command.

### 5. Import a remote Git repo into "my-repo"
Obviously having an empty repo isnt much good. So lets try using the `import` command to clone a remote target into "my-repo":

```
az repos import create --repository my-repo --git-url=https://github.com/MicrosoftDocs/pipelines-dotnet-core
```

### 6. Add a few policies to "my-repo"
Lets use the the `policy` command to add a few policies to our new repository that ensures contributors link work-items to commits and ensure that a minimum number of approvers are included on each pull requests:

```
az repos policy work-item-linking create --blocking=true --branch=master --enabled=true --repository-id={use-az-repos-show-to-fetch-my-repo-id}
az repos policy approver-count create --allow-downvotes=true --blocking=true --branch=master --creator-vote-counts=false --enabled=true  --minimum-approver-count=2 --reset-on-source-push=true --repository-id={use-az-repos-show-to-fetch-my-repo-id}
```

***Note*** <br/>
For more info on available policies visit [Microsoft Docs](https://docs.microsoft.com/en-us/cli/azure/ext/azure-devops/repos/policy?view=azure-cli-latest)

### 7. Verify that policies have been applied correctly
Use the `policy list` subcommand to verify that our two policies have been succesfully applied:

```
az repos policy list
```

## Want to help make our training material better?
 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/dojo/issues).
