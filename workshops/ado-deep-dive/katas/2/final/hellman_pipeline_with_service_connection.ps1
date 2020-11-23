#Install Azure DevOps Extension
az extension add --name azure-devops

#Authenticate via Azure CLI
az login --allow-no-subscriptions

#Configure Azure DevOps CLI
az devops configure --defaults organization=https://dev.azure.com/{my-org-name} project={my-project-name} --use-git-aliases true

#Create service connection
az devops service-endpoint create --service-endpoint-configuration=service_endpoint_configuration.json

#Create pipeline
az pipelines create --name Hellman-Sample --skip-first-run --repository {ado-project-repo-name} --branch main --repository-type tfsgit