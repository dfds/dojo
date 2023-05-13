DFDS Crossplane Developer Deepdive - Code kata #2
======================================

This training exercise is a **beginner-level** course on Crossplane that serves as a starting point for Developers looking to onboard the provisioning efforts at DFDS.

## Getting started
These instructions will help you prepare for the kata and ensure that your training machine has the tools installed you will need to complete the assignment(s). If you find yourself in a situation where one of more tools might not be available for your training environment please reach out to your instructor for assistance on how to proceed, post an [issue in our repository](https://github.com/dfds/dojo/issues) or fix it yourself and update the kata via a [pull request](https://github.com/dfds/dojo/pulls).

### Prerequisites
* Kata 0
* Dockerhub account

## Exercise
In this exercise you will get some familarity with concepts of Configration packages and claims that you are going to use afterwards to provision PostgresSQL databases on AWS on-demand. 

### 1. Create folder my-configuration
mkdir my-configuration

### 1. Create a definition.yaml for RDSInstance

First we need to create a Composite Resource Definition (XRD) that describes the resource parameters we need to define in order to create the resource.

Create a definition.yaml

```
apiVersion: apiextensions.crossplane.io/v1
kind: CompositeResourceDefinition
metadata:
  name: compositedatabaseinstances.crossplane.dfds.cloud
spec:
  connectionSecretKeys:
  - username
  - password
  - hostname
  - port
  defaultCompositionRef:
    name: compositedatabaseinstances.crossplane.dfds.cloud
  group: crossplane.dfds.cloud
  names:
    kind: CompositeDatabaseInstance
    plural: compositedatabaseinstances
  claimNames:
    kind: DatabaseInstance
    plural: databaseinstances
  versions:
  - name: v1alpha1
    served: true 
    referenceable: true 
    schema:
      openAPIV3Schema:
        type: object
        properties:
          spec:
            type: object
            properties:
              parameters:
                type: object
                properties:
                  engineVersion:
                    description: Database engine version
                    type: string
                    enum: ["10", "11", "12", "13"]
                  allocatedStorage:
                    type: integer
                  region:
                    description: Geographic location of this Database server.
                    type: string
                  dbInstanceClass:
                    type: string
                    enum: ["db.t2.small", "db.t3.small"]
                  masterUsername:
                    type: string
                  engine:
                    type: string
                    enum: ["postgres"]
                  skipFinalSnapshotBeforeDeletion:
                    type: boolean
                required:
                - engineVersion
                - allocatedStorage
                - region
                - dbInstanceClass
                - masterUsername
                - engine
                - skipFinalSnapshotBeforeDeletion
            required:
            - parameters
          status:
            type: object
            properties:
              address:
                description: Address of this Database server.
                type: string
```

### 2. Apply the definition manifest

Apply the manifest file to deploy the XRD into your cluster

```
kubectl apply -f definition.yaml
```

### 3. Verify the XRD

Get the currently installed XRD's and verify that 2 new CRD's have also been deployed

```
kubectl get xrd
kubectl get crd | grep dfds
```

These CRD's should correlate with the names and claim names from our XRD

### 4. Create a composition.yaml

Now we will create a composition manifest which declares the resources that are to be created with base settings/defaults

Create a composition.yaml and populate with the following contents

```
apiVersion: apiextensions.crossplane.io/v1
kind: Composition
metadata:
  name: compositedatabaseinstances.crossplane.dfds.cloud
  labels:
    purpose: database
    provider: aws
spec:
  compositeTypeRef:
    apiVersion: crossplane.dfds.cloud/v1alpha1
    kind: CompositeDatabaseInstance
  resources:
  - name: securitygroup
    base:
      apiVersion: ec2.aws.crossplane.io/v1beta1
      kind: SecurityGroup
      spec:
        forProvider:
          region: eu-west-1
          description: Allow access to PostgreSQL
          ingress:
            - fromPort: 5432
              toPort: 5432
              ipProtocol: tcp
              ipRanges:
                - cidrIp: 0.0.0.0/0
                  description: postgresql
        providerConfigRef:
          name: my-aws-provider-config                  
    patches:
    - fromFieldPath: "metadata.name"
      toFieldPath: "spec.forProvider.groupName"
      transforms:
      - type: string
        string:
          fmt: "%s-rds-security-group"      
  - name: rdinstance
    base:
      apiVersion: database.aws.crossplane.io/v1beta1
      kind: RDSInstance
      spec:
        forProvider:
          dbInstanceClass: db.t3.small
          engine: postgres
          masterUsername: masteruser
          skipFinalSnapshotBeforeDeletion: true
          vpcSecurityGroupIDSelector:
            matchControllerRef: true
        providerConfigRef:
          name: my-aws-provider-config            
        writeConnectionSecretToRef:
          namespace: crossplane-system
    patches:
    - fromFieldPath: "metadata.uid"
      toFieldPath: "spec.writeConnectionSecretToRef.name"
      transforms:
      - type: string
        string:
          fmt: "%s-databaseserver"
    - fromFieldPath: "spec.parameters.engineVersion"
      toFieldPath: "spec.forProvider.engineVersion"
    - fromFieldPath: "spec.parameters.region"
      toFieldPath: "spec.forProvider.region"
      transforms:
      - type: map
        map:
          eu-west-1: eu-west-1
    - fromFieldPath: "spec.parameters.allocatedStorage"
      toFieldPath: "spec.forProvider.allocatedStorage"
    - fromFieldPath: "spec.parameters.dbInstanceClass"
      toFieldPath: "spec.forProvider.dbInstanceClass"
    connectionDetails:
    - fromConnectionSecretKey: username
    - fromConnectionSecretKey: password
    - name: hostname
      fromConnectionSecretKey: endpoint
    - type: FromValue
      name: port
      value: "5432"
    readinessChecks:
    - type: MatchString
      fieldPath: "status.atProvider.dbInstanceStatus"
      matchString: "available"
  writeConnectionSecretsToNamespace: crossplane-system
```

### 5. Create a crossplane.yaml

Next we need to add a crossplane.yaml file to our directory. This file describes our configuration and any dependencies

```
apiVersion: meta.pkg.crossplane.io/v1
kind: Configuration
metadata:
  name: my-configuration
spec:
  crossplane:
    version: ">=v1.2.1"
  dependsOn:
    - provider: crossplane/provider-aws
      version: "v0.19.0"
```
### 6. Authenticate with Docker

We need to authenticate with Dockerhub in order to be able to push our configuration package

```
docker login -u my-username
```


### 7. Build and push the configuration

Ensure you change the following commands to refer to your dockerhub account name instead of my-dockerhub.

Build the configuration locally, notice how it creates a .xpkg file. 

```
kubectl crossplane build configuration
```

Push it to your dockerhub repository with the push command below. The push command will automatically find the .xpkg file and push the image 

```
kubectl crossplane push configuration my-dockerhub/my-configuration:v0.0.1-alpha.0
```

You may observe on the dockerhub website that you have successfully pushed your package to your account

### 8. Install the configuration package

Next we will install our configuration package into our cluster so that people can consume resources

```
kubectl crossplane install configuration my-dockerhub/my-configuration:v0.0.1-alpha.0
```

### 9. Verify installation

```
kubectl get configuration.pkg
kubectl get xrd
kubectl get crd | grep dfds
```


## Want to help make our training material better?
 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/dojo/issues).