DFDS Kubernetes Training - Code kata #1
======================================

This training exercise is a **beginner-level** course on Kubernetes that serves as a starting point for developers looking to get started with container orchestration at DFDS.

## Getting started
These instructions will help you prepare for the kata exercise and ensure that your local machine has the tools installed you will need to complete the assignment(s). If you find yourself in a situation where one or more tools might not be available for your training environment please reach out to your instructor for assistance on how to proceed, post an [issue in our repository](https://github.com/dfds/dojo/issues) or fix it yourself and create a [pull request](https://github.com/dfds/dojo/pulls).

### Prerequisites
* [Kubectl](https://kubernetes.io/docs/tasks/tools/install-kubectl/)
* [Docker](https://www.docker.com/products/docker-desktop)
* [Powershell Core](https://docs.microsoft.com/en-us/powershell/scripting/install/installing-powershell?view=powershell-6)

## Exercise
Your team have just finalized their first container project and pushed it to the DockerHub registry. They are now looking for your help to deploy the containers to a k8s cluster so they can leverage its managed services and focus on building new features for DFDS.

### 1. Create your kata directory
First we setup a directory for our exercise files. It's pretty straight forward:

```
mkdir kata1
cd kata1
```

### 2. Create a Kubernetes namespace

This Kata may be completed by a number of people and in some cases they could be doing the Kata at the same time.  For this reason we recommend defining your own namespace into which your resources will then be provisioned.  This first section covers the steps required to define the namespace.

Start by creating a file named "k8s-namespace.yml" and add the code fragment below, making sure to change the value 'my-namespace' to something which is unique.

```
apiVersion: v1
kind: Namespace
metadata:
  name: my-namespace
```

### 3. Create deployment manifest
Create a file named "solution_deployments.yml" and add two Deployment objects for our API + MVC containers:

```
apiVersion: apps/v1
kind: Deployment
---
apiVersion: apps/v1
kind: Deployment
```

**A tabular explanation of the above Manifest**
| Parameter | Value | Explanation |
| --- | --- | --- |
| .kind | Deployment | Instructs the K8s API controller to create an object of kind (type) Deployment |

### 4. Configure api-backend Deployment
Augment the first Deployment object configuration with the following markup to specify the setup for our api-backend deployment.  Again, make sure that you modify the namespace to match up with what you defined in step 2 of the Kata:

```
metadata:
  name: api-deployment
  namespace: my-namespace
  labels:
    app: api
spec:
  replicas: 2
  selector:
    matchLabels:
      app: api
      tier: backend
      track: stable
  template:
    metadata:
      labels:
        app: api
        tier: backend
        track: stable
    spec:
      containers:
      - name: api-backend
        image: toban/k8s-training-api:latest
        imagePullPolicy: IfNotPresent
        ports:
          - name: http
            containerPort: 5000
      restartPolicy: Always
```

**A tabular explanation of the above Manifest**
| Parameter | Value | Explanation |
| --- | --- | --- |
| .metadata.name | api-backend | Instructs K8s to name our deployment "api-backend" so we can reference it via kubectl, from other manifest etc. |
| .metadata.namespace | my-namespace | Targets the deployment to a specific namespace to maintain segregation with other deployments of K8s resources |
| .spec | | Specifies the blueprint for the deployment. |
| .spec.replicas | 2 | Incidcates the number of replicas we want K8s to maintain for scalability and redundancy purposes |
| spec.template.spec.container.ports | 5000 | Specifies the port that K8s uses to proxy container networking |

### 5. Configure MVC Deployment
Augment the second Deployment object configuration with the following markup to specify the setup for our frontend ASP.NET MVC solution.  Again, the namespace parameter should be updated so that the deployment is performed in the K8s namespace defined in step 2 of the Kata:

```
metadata:
  name: mvc-deployment
  namespace: my-namespace
  labels:
    app: mvc
spec:
  replicas: 4
  selector:
    matchLabels:
      app: mvc
      tier: frontend
      track: stable
  template:
    metadata:
      labels:
        app: mvc
        tier: frontend
        track: stable
    spec:
      containers:
      - name: mvc-frontend
        image: toban/k8s-training-frontend:latest
        imagePullPolicy: IfNotPresent
        ports:
          - name: http
            containerPort: 5000
      restartPolicy: Always
```

### 6. Use kubectl to create the namespace
Execute the following command to apply the previously defined manifest file that contains the namespace resource.
```
kubectl create -f .\k8s-namespace.yml
```
Once the command has completed you can use the following command to verify that the namespace exists.
```
kubectl get namespace
```

### 7. Use kubectl to create a two new Kubernetes Deployment from our manifest
Execute the following command to apply the previously defined manifest file that contains the definition of the two deployments.
```
kubectl create -f .\solution_deployments.yml
```
As the name suggests the create parameter of kubectl will cause it to create one or more resources, the details of which can either be provided from file or via standard input.  In this case we wish to apply our previously defined manifest, so we use the -f parameter along with the path to the file.

### 8. Verify that Web API deployment is created
Execute the following command to verify that the manifest has been applied and the expected resources created.  The -n parameter targets a specific namespace so the value following the option should be modified to match the namespace name that was selected in step 2 of the Kata.
```
kubectl describe deployment api-deployment -n my-namespace
```
Using the describe keyword with kubectl causes it to fetch metadata related to one or more resources via the API Server.  Following the describe keyword we then also provide the type of resource and the name of the resource.

## 9. Verify that MVC deployment is created
Similarly you can use the following command to view the MVC deployment.  Again, the -n parameter is used to target the namespace defined in step 2 of the Kata.
```
kubectl describe deployment mvc-deployment -n my-namespace
```

## 10. Cleaning up
You can use the same manifests that were written to create the K8s resources to delete them as well.  If you wish to clean up after doing this Kata then you can do so by simply running the following commands.  Simply modify *namespace-file.yml* and *deployment-file.yml* to reflect the names of the files that you created.
```
kubectl delete -f ./solution_deployments.yml
kubectl delete -f ./k8s-namespace.yml
```
## Want to help make our training material better?
 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/dojo/issues).
