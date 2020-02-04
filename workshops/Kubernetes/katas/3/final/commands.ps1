### 3. Use kubectl to create our new service
kubectl apply -f mvc-svc.yml

### 5. Use kubectl to create our new service
kubectl apply -f api-svc.yml

### 6. Verify that services are created
kubectl get svc

### 7. Verify that mvc-svc has two endpoints
kubectl get ep mvc-svc

### 8. Verify that api-svc has two endpoints
kubectl get ep mvc-svc

### 9. Verify that api-svc and mvc-svc has a total of four pods
kubectl get pods