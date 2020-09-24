### 3. Apply the liveness probe configuration to the cluster
kubectl apply -f liveness-probe.yml

### 4. Wait for a few seconds then investigate what’s happening inside the pod
kubectl describe pods node500

### 6. Apply the readiness probe configuration to the cluster
kubectl apply -f readiness-probe.yml

### 7. Give it a few seconds then let’s see how the readiness probe worked
kubectl describe pods nodedelayed