#You can deploy a stack on Kubernetes with docker stack deploy
docker stack deploy --namespace my-app --compose-file docker-compose.yml mystack
docker stack services mystack

#You can see the service deployed with the kubectl get services command.
kubectl get services -n my-app

#You can list available nodes to verify.
kubectl get nodes