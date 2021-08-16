DFDS Crossplane - Code kata #5
======================================

This training exercise is an **intermediate-level** course on Crossplane that serves as a starting point for SRE's looking to install and maintain Crossplane at DFDS.

## Getting started
These instructions will help you prepare for the kata and ensure that your training machine has the tools installed you will need to complete the assignment(s). If you find yourself in a situation where one of more tools might not be available for your training environment please reach out to your instructor for assistance on how to proceed, post an [issue in our repository](https://github.com/dfds/dojo/issues) or fix it yourself and update the kata via a [pull request](https://github.com/dfds/dojo/pulls).

### Prerequisites
* Kubernetes Cluster
* Kubectl
* Helm

## Exercise
Your fifth assignment will see you create a composite resource by creating a definition and a composition.


### 1. Create a definition.yaml

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

### 3. Create a composition.yaml

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
  patchSets:
  - name: metadata
    patches:
    - fromFieldPath: metadata.labels
    - fromFieldPath: metadata.annotations[crossplane.dfds.cloud/app-name]
  - name: external-name
    patches:
    - type: FromCompositeFieldPath
      fromFieldPath: metadata.annotations[crossplane.io/external-name]
      policy:
        fromFieldPath: Required
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

### 4. Apply the composition manifest

Apply the manifest to deploy the composition to your cluster

```
kubectl apply -f composition.yaml
```

### 5. Verify that the composition exists

Verify that the composition now exists in your cluster

```
kubectl get composition
```

## Want to help make our training material better?
 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/dojo/issues).