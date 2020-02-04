### 3. Create a pod from out manifest file
kubectl create -f my-pod.yml

### 4. Inspect the pod configuration. Notice how k8s has added ALOT of meta data to ensure it can maintain our desired state
`kubectl inspect pod my-pod`

### 6. Re-applying manifest to update existing pod:
kubectl apply -f my-pod.yml

### 7. Inspect the pod configuration to verify that the new annotation has been applied
kubectl inspect pod my-pod

### 8. Delete the pod once your done
kubectl delete pod my-pod