#Get existing namespaces
kubectl get namespaces

#You can also create your own namespaces.
kubectl create ns my-ns

#To assign an object to a custom namespace, simply specify its metadata.namespace attribute.
apiVersion: v1
kind: Pod
metadata:
  name: my-ns-pod
  namespace: my-ns
  labels:
    app: myapp
spec:
  containers:
  - name: myapp-container
    image: busybox
    command: ['sh', '-c', 'echo Hello Kubernetes! && sleep 3600']

#Create the pod with the created yaml file.
kubectl create -f my-ns.yml

#Use the -n flag to specify a namespace when using commands like kubectl get.
kubectl get pods -n my-ns

#You can also use -n to specify a namespace when using kubectl describe.
kubectl describe pod my-ns-pod -n my-ns