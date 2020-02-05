DFDS Kubernetes Training - Code kata #8
======================================

## Getting started

These instructions will help you prepare for the code kata and make sure that your local developer machine has the following tools installed and configured:

### Prerequisites

* [Kubernetes](https://kubernetes.io/docs/tasks/tools/install-kubectl/)
* [Powershell Core](https://docs.microsoft.com/en-us/powershell/scripting/install/installing-powershell?view=powershell-6)

## Exercise

[TODO]

### 1. Create a kata directory for our exercise files
`mkdir /kata8`<br/>
`cd /kata8`

### 2. Create a "sandbox" namespace to protect the remaining cluster from our experiment
`kubectl create namespace sandbox`

### 3. Create a broken pod
Create a file named "broken-pod.yml". We will be using the nginx image with a reference to an invalid image version (1.158):

```
apiVersion: v1
kind: Pod
metadata:
  name: nginx
  namespace: sandbox
spec:
  containers:
  - name: nginx
    image: nginx:1.158
```

### 4. Apply the broken pod to the cluster
`kubectl apply -f broken-pod.yml`

### 5. Locate the broken pod in our cluster
`kubectl get pods --all-namespaces`

Just to explain: <br/>
`--all-namespaces` - This flag instructs kubectl to ask the API controller for pods in all namespaces. Check the STATUS field to identify broken pods in our cluster.

### 6. Inspect CPU usage in the namespaces containing broken pods to evaluate the severity of the problem
`kubectl top pod -n sandbox`

Just to explain: <br/>
`kubectl top` - Instructs kubectl to execute the 'top' operation on all resources specified in the command (in our cases all pods in a given namespace).

### 7. Get the broken pod's summary data in the JSON format 
`kubectl get pod nginx -n sandbox -o json`

### 8. Get the broken pod's logs
`kubectl logs nginx -n sandbox -o json`

### 9. Fix the problem with the broken pod so that it enters the `Running` state
`kubectl patch pod nginx -p '{"spec":{"containers":[{"name":"nginx","image":"nginx:1.15.8"}]}}'`

### 10. Verify that our fix solved the problem and brought the pod back into the correct state
`kubectl get pod nginx -n sandbox`

## Want to help make our training material better?

 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/roadmap/issues).
 