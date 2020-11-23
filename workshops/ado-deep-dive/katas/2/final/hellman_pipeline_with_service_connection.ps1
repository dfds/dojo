#Install Azure DevOps Extension
az extension add --name azure-devops

#Authenticate via Azure CLI
az login --allow-no-subscriptions

#Configure Azure DevOps CLI
az devops configure --defaults organization=https://dev.azure.com/{my-org-name} project={my-project-name} --use-git-aliases true

TODO