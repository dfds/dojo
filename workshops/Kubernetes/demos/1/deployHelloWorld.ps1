#Imperatively working with your cluster. Run will "generate" a Deployment by default.
#This is pulling a specified image from Google's container registry.
#kubectl run, will convert a pod creation into a "Deployment generation"
#http://kubernetes.io/docs/user-guide/kubectl-conventions/#generators 
kubectl run hello-world --image=gcr.io/google-samples/hello-app:1.0

#But let's deploy a single pod too...
kubectl run hello-world-pod --image=gcr.io/google-samples/hello-app:1.0 --generator=run-pod/v1

#Let's follow our pod and deployment status
kubectl get pods
kubectl get deployment
kubectl get pods -o wide

#Back on the Master, we can pull the logs from the container. Which is going to be anything written to stdout. 
#Maybe something went wrong inside our app, and our pod won't start. This is useful for troubleshooting.
kubectl logs hello-world-pod

#Starting a process inside a container inside a pod.
#We can use this to launch any process as long as the executable/binary is in the container.
#Launch a shell into the container. Callout that this is on the *pod* network.
kubectl exec -it hello-world-pod  -- /bin/sh
hostname
ip addr
exit

#Remember that first kubectl run we executed, it created a Deployment for us.
#Let's look more closely at the deployment
#Deployments are made of ReplicaSets!
kubectl get deployment hello-world
kubectl get replicaset
kubectl get pods

#Let's take a closer look at our pod.
#Walk through the pods Events...
#Name, Containers, Ports, Conditions, and Events. 
#Deployments are made of ReplicaSets!
kubectl describe deployment hello-world | more

#Let's see what describe can tell us about a deployed Pod.
#Check out the Name, Node, Status, Containers, and events.
kubectl get pods
kubectl describe pods $PASTEPODNAMEHERE | more

#Expose the Deployment as a Service.
#This will create a Service for the ReplicaSet behind the Deployment
#We are exposing our serivce on port 80, connecting to an application running on 8080 in our pod.
#Port: Interal Cluster Port, the Service's port. You will point cluster resources here.
#TargetPort: The Pod's Serivce Port, your application. That one we defined when we started the pods.
kubectl expose deployment hello-world --port=80 --target-port=8080

#Check out the IP: and Port:, that's where we'll access this service.
kubectl get service hello-world

#We can also get that information from using get
kubectl describe service hello-world

#Access the service inside the cluster
curl http://$SERVCIEIP:$PORT

#Access the pod's application directly, useful for troubleshooting.
kubectl get endpoints hello-world
curl http://$ENDPOINT:$TARGETORT

#Using kubectl to generate yaml or json files of our imperitive configuration.
kubectl get service hello-world -o yaml
kubectl get service hello-world -o json

#Exported resources are stripped of cluster-specific information.
kubectl get service hello-world -o yaml --export > service-hello-world.yaml
kubectl get deployment hello-world -o yaml --export > deployment-hello-world.yaml
ls *.yaml

more service-hello-world.yaml

#We can ask the API for more information about an object
kubectl explain service | more

#And drill down further if needed, includes very good explanation of what's available for that resource
kubectl explain service.spec | more
kubectl explain service.spec.ports
kubectl explain service.spec.ports.targetPort

#Let's remove everything we created imperitively and start over using a declarative model
kubectl delete service hello-world
kubectl delete deployment hello-world
kubectl delete pod hello-world-pod
kubectl get all

#Deploying applications declaratively
#we can use apply to create our resources from yaml.
kubectl apply -f deployment-hello-world.yaml
kubectl apply -f service-hello-world.yaml 

#This re-creates everything we created, but in yaml
kubectl get all

#scale up our deployment
vi deployment-hello-world.yaml
Change replicas from 1 to 2
     replicas: 2

#update our configuration with apply
kubectl apply -f deployment-hello-world.yaml

#And check the current configuration of our deployment
kubectl get deployment hello-world

#Repeat the curl access to see the load balancing of the HTTP request
kubectl get service hello-world
curl http://$SERVICEIP:PORT

#We can edit the resources "on the fly" with kubectl edit. But this isn't reflected in our yaml. But is
#persisted in the etcd database...cluster store. Change 2 to 3.
kubectl edit deployment hello-world

#Let's clean up our deployment and remove everything
kubectl delete deployment hello-world
kubectl delete service hello-world

kubectl get all
