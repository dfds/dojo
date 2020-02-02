## 5. Use kubectl to create a two new Kubernetes Deployment from our manifest
kubectl create -f .\api_mvc_deployment.yml

## 6. Verify that Web API deployment is created
kubectl describe deployment api-backend

## 7. Verify that MVC deployment is created
kubectl describe deployment mvc-frontend