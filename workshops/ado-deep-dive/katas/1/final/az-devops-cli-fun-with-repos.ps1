#Install Azure DevOps Extension
az extension add --name azure-devops

#Authenticate via Azure CLI
az login --allow-no-subscriptions

#Configure Azure DevOps CLI
az devops configure --defaults organization=https://dev.azure.com/{my-unique-org-name} project=ADODD-Demo --use-git-aliases true

#Create a new repository
az repos create --name my-repo

#Import a remote Git repo into "my-repo"
az repos import create --repository my-repo --git-url=https://github.com/MicrosoftDocs/pipelines-dotnet-core

#Add a few policies to "my-repo"
az repos policy work-item-linking create --blocking=true --branch=master --enabled=true --repository-id={use-az-repos-show-to-fetch-my-repo-id}
az repos policy approver-count create --allow-downvotes=true --blocking=true --branch=master --creator-vote-counts=false --enabled=true  --minimum-approver-count=2 --reset-on-source-push=true --repository-id={use-az-repos-show-to-fetch-my-repo-id}

#Verify that policies have been applied correctly
az repos policy list