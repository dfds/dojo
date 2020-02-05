### 2. Create a "sandbox" namespace to protect the remaining cluster from our experiment
kubectl create namespace sandbox

### 4. Apply the broken pod to the cluster
kubectl apply -f broken-pod.yml

### 5. Locate the broken pod in our cluster
kubectl get pods --all-namespaces

### 6. Inspect CPU usage in the namespaces containing broken pods to evaluate the severity of the problem
kubectl top pod -n sandbox

### 7. Get the broken pod's summary data in the JSON format 
kubectl get pod nginx -n sandbox -o json

### 8. Get the broken pod's logs
kubectl logs nginx -n sandbox -o json

### 9. Fix the problem with the broken pod so that it enters the `Running` state
kubectl patch pod nginx -p '{"spec":{"containers":[{"name":"nginx","image":"nginx:1.15.8"}]}}'

### 10. Verify that our fix solved the problem and brought the pod back into the correct state
kubectl get pod nginx -n sandbox