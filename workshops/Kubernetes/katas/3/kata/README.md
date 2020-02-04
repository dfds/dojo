DFDS Kubernetes Training - Code kata #3
======================================

## Getting started

These instructions will help you prepare for the code kata and make sure that your local developer machine has the following tools installed and configured:

### Prerequisites

* [Kubernetes](https://kubernetes.io/docs/tasks/tools/install-kubectl/)
* [Powershell Core](https://docs.microsoft.com/en-us/powershell/scripting/install/installing-powershell?view=powershell-6)

## Exercise

Your team has just deployed two components of a web application to Hellman, using deployments with multiple replicas. Now you need a way to provide dynamic network access to these replicas so that there will be uninterrupted access to the components whenever replicas are created, removed, or replaced. One deployment is called mvc-frontend, an .NET core MVC application that needs to be accessible from outside the cluster. The other is called api-backend, and it is a component designed to be accessed only by other pods within the cluster.

Your team wants you to create two services to expose these deployments:

### 1. Create your project directory
`mkdir /kata3`<br/>
`cd /kata3`

### 2. Create a service descriptor file for our mvc-frontend deployment
Create a file named "mvc-svc.yml" and add a service definition:

```
apiVersion: v1
kind: Service
metadata:
  name: mvc-svc
spec:
  type: NodePort
  selector:
    app: mvc
  ports:
  - protocol: TCP
    port: 8080
    targetPort: 80
```

Just to explain: <br/>
`.kind: Service` - Specifies a need to create an object of kind (type) Service. <br/>
`.metadata.name: my-pod` - Instructs k8s to name our pod "my-pod" so we can reference it via kubectl, from other manifests, etc. <br/>
`.metadata.labels: ` - Labels are intended to be used to specify identifying attributes of objects that are meaningful and relevant to users, but do not directly imply semantics to the core system. Labels can be used to organize and to select subsets of objects.<br/>
`.spec.type: NodePort` - Instructs k8s to expose our service using an IP that is external to the cluster. <br/>

### 3. Use kubectl to create our new service
`kubectl apply -f mvc-svc.yml`

Just to explain: <br/>
`kubectl apply` - Apply a resource from a file or from stdin. <br/>
`-f .\mvc-svc.yml` -f point to the filename, directory or URL containing our "resource manifest" files.

### 4. Create a service descriptor file for our api-backend deployment
Create a file named "api-svc.yml" and add a service definition:

```
apiVersion: v1
kind: Service
metadata:
  name: api-svc
spec:
  type: ClusterIP
  selector:
    app: api
  ports:
  - protocol: TCP
    port: 8080
    targetPort: 80
```

Just to explain: <br/>
`.spec.type: ClusterIP` - Instructs k8s to expose our service using an IP that is internal to the cluster. <br/>

### 5. Use kubectl to create our new service
`kubectl apply -f api-svc.yml`

### 6. Verify that services are created
`kubectl get svc`

Just to explain: <br/>
`kubectl get svc` - Fetches all services in the current namespace. <br/>

### 7. Verify that mvc-svc has two endpoints
`kubectl get ep mvc-svc`

### 8. Verify that api-svc has two endpoints
`kubectl get ep mvc-svc`

### 9. Verify that api-svc and mvc-svc has a total of four pods
`kubectl get pods`

## Want to help make our training material better?

 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/roadmap/issues).
 