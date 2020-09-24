### 3. Use kubectl to create our new storage allocation
kubectl apply -f mysql-pv.yml

### 5. Use kubectl to apply our new storage claim
kubectl apply -f mysql-pvc.yml

### 7. Use kubectl to apply our mysql pod
kubectl apply -f mysql-pod.yml