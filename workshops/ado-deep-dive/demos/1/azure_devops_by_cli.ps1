#Run the CLI in a docker container
docker run --name=ado-demo -d -i mcr.microsoft.com/azure-cli

#Login to azure.
docker exec ado-demo az login --allow-no-subscriptions

#Add azure-devops extension.
docker exec ado-demo az extension add --name azure-devops

#Configure default org & git aliases (all `az repos` commands will now be aliased to `git repo` and all `az repos pr` commands to `git pr .`).
docker exec ado-demo az devops configure --defaults organization=https://dev.azure.com/dfds --use-git-aliases true

#Create a new project
$projectId = (docker exec ado-demo az devops project create --name ADO-DeepDive-Demo | ConvertFrom-Json).id

#Configure default project
docker exec ado-demo az devops configure --defaults project=ADO-DeepDive-Demo

#List areas in the project
docker exec ado-demo az boards area project list

#Delete the new project
docker exec ado-demo az devops project delete --id $projectId --yes