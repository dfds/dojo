#Create deployment for backend api
kubectl create -f .\api_mvc_deployment.yml

#View deployment meta data for API
kubectl describe deployment api-backend

#View deployment meta data for MVC
kubectl describe deployment mvc-frontend

#Get external IP of mvc-frontend service.
kubectl get service mvc-frontend --watch