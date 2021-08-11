The solution for the problem in step 3 is to create a ProviderConfig named 'default' which uses some credentials you specify

```
kubectl create secret generic aws-creds -n my-namespace --from-file=creds=./creds.conf
kubectl apply -f providerconfig.yaml
```