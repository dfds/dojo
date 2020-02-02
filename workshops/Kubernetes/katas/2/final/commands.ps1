## 4. Create a pod from out manifest file
kubectl create -f my-pod.yml

## 6. Re-applying manifest to update existing pod:
kubectl apply -f my-pod.yml

## 7. Delete the pod once your done:
kubectl delete pod my-pod