## 5. Use kubectl to create a two new Kubernetes Deployment from our manifest
kubectl create -f .\solution_deployments.yml

## 6. Verify that Web API deployment is created
kubectl describe deployment api-deployment

## 7. Verify that MVC deployment is created
kubectl describe deployment mvc-deployment