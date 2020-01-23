#Check version of kubectl.
kubectl version --short

#Minimize typing (im lazy).
Set-Alias -Name k -Value kubectl

#Check alias works.
k version --short

#Login to azure.
az login

#Create service principle.
$result = az ad sp create-for-rbac --skip-assignment | ConvertFrom-Json

#Assign reader role to service principal.
az role assignment create --assignee $result.appId --role Reader

#Create aks cluster.
az aks create --name kubernetes-training-cluster --resource-group kubernetes-training-rg --node-count 1 --generate-ssh-keys --service-principal $result.appId --client-secret $result.password

#Connect kubectl to AKS.
az aks get-credentials --name kubernetes-training-cluster --resource-group kubernetes-training-rg

#Verify that we are connected to AKS by listing nodes.
k get nodes

#Deploy to AKS.
k apply -f .\api_deployment.yml
k apply -f .\mvc_deployment.yml

#Watch AKS service creation.
k get service --watch

#Scale up number of nodes on cluster.
az aks scale --resource-group kubernetes-training-rg --name kubernetes-training-cluster --node-count 3

#Check that more nodes have been added.
k get nodes