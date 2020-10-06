DFDS Kubernetes Training - Code kata #4
======================================

## Getting started

These instructions will help you prepare for the code kata and make sure that your local developer machine has the right tools installed and configured.

### Prerequisites

* [Kubectl](https://kubernetes.io/docs/tasks/tools/install-kubectl/)
* [Docker](https://www.docker.com/products/docker-desktop)
* [Powershell Core](https://docs.microsoft.com/en-us/powershell/scripting/install/installing-powershell?view=powershell-6)

## Exercise

Your team has succesfully deployed their first .NET core application to the Kubernetes cluster, exposed it to the world via a set of services and is now ready to persist some of the data collected from the end users to our MySQL DB. But beware, here be dragons! Given the empheral nature of containers (they are either there or not) a key challenge of transitioning application to the cloud remains, namely providing persistant storage for those parts of the application that needs to maintain state (data) beyond the span of a containers lifecycle. Luckily k8s has a myriad of solutions to this problem, depending on your needs and the architecture of your storage provider. For the purpose of our kata we will focus on the most simple kind of storage, "localdisk": 


### 1. Create a kata directory for our exercise files
`mkdir kata4`<br/>
`cd kata4`

### 2. Create a PersistantVolume descriptor file to allocate some storage in our cluster
Create a file named "mysql-pv.yml" and add a PersistantVolume definition:

```
kind: PersistentVolume
apiVersion: v1
metadata:
  name: mysql-pv
spec:
  storageClassName: local
  capacity:
    storage: 1Gi
  accessModes:
    - ReadWriteOnce
  hostPath:
    path: "/mnt/data"
```

Just to explain: <br/>
`.kind: PersistentVolume` - Specifies a need to create an object of kind (type) PersistantVolume which instructs a k8s cluster to provision storage from the targeted storage provider (storageClass). <br/>
`.kind: hostPath` - Specifies the host path on cluster nodes where storage will be mounted. <br/>

### 3. Use kubectl to create our new storage allocation
`kubectl apply -f mysql-pv.yml`

### 4. Create a PersistantVolumeClaim descriptor file to acquire some storage from our new "cluster partition"
Create a file named "mysql-pvc.yml" and add a PersistantVolumeClaim definition:

```
apiVersion: v1
kind: PersistentVolumeClaim
metadata:
  name: mysql-pv-claim
spec:
  storageClassName: local
  accessModes:
    - ReadWriteOnce
  resources:
    requests:
      storage: 500Mi
```

Just to explain: <br/>
`.kind: hostPath` - Specifies the host path on cluster nodes where storage will be mounted. <br/>

### 5. Use kubectl to apply our new storage claim
`kubectl apply -f mysql-pvc.yml`

### 6. Create a mysql pod definition to consume our new PersistentVolume resource via an appropriate PersistantVolumeClaim and mount it as volume on our mysql container.
Create a file named "my-sql-pod.yml" and add a Pod definition:

```
apiVersion: v1
kind: Pod
metadata:
  name: mysql-pod
spec:
  containers:
  - name: mysql
    image: mysql:5.6
    ports:
    - containerPort: 3306
    env:
    - name: MYSQL_ROOT_PASSWORD
      value: password
    volumeMounts:
    - name: mysql-storage
      mountPath: /var/lib/mysql
  volumes:
  - name: mysql-storage
    persistentVolumeClaim:
      claimName: mysql-pv-claim
```

### 7. Use kubectl to apply our mysql pod
`kubectl apply -f mysql-pod.yml`

## Want to help make our training material better?

 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/ded-dojo/issues).
 