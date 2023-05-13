
TODO: input AWS credentials

kubectl create namespace crossplane-system

helm repo add crossplane-stable https://charts.crossplane.io/stable
helm repo update

helm install crossplane --namespace crossplane-system crossplane-stable/crossplane --version 1.4.0

helm list -n crossplane-system

kubectl get all -n crossplane-system


curl -sL https://raw.githubusercontent.com/crossplane/crossplane/master/install.sh | sh
sudo mv kubectl-crossplane /usr/local/bin

kubectl crossplane --version

wait

kubectl crossplane install provider crossplane/provider-aws:v0.20.0

wait

TODO: create namespace my-app-namespace
TODO: create secret in my-app-namespace
TODO: install providerconfig that references secret in my-app-namespace




