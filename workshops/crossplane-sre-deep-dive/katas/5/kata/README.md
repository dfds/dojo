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
Your fifth assignment will see you create a composite resource definition (XRD). 

### 1. Create a definition.yaml
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
                    enum: ["10", "11", "12"]
                    default: "12"
                  allocatedStorage:
                    type: integer
                  region:
                    description: Geographic location of this Database server.
                    type: string
                  dbInstanceClass:
                    type: string
                    enum: ["db.t2.small"]
                  masterUsername:
                    type: string
                  engine:
                    type: string
                    enum: ["postgres"]
                  skipFinalSnapshotBeforeDeletion:
                    type: boolean
                required:
                # - engineVersion
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
            description: >
              A status represents the observed state of a RDS instance.
            properties:
              dbInstanceConditions:
                description: >
                  Freeform field containing information about the RDS instance condition, e.g. reconcile errors due to bad parameters
                type: array
                items:
                  type: object
                  x-kubernetes-preserve-unknown-fields: true
    additionalPrinterColumns:
    - name: Synced
      type: string
      jsonPath: ".status.dbInstanceConditions[0].status"
    - name: Sync-Date
      type: string
      jsonPath: ".status.dbInstanceConditions[0].lastTransitionTime"
```

### 2. Apply the definition manifest
```
kubectl apply -f definition.yaml
```

### 3. Create a composition.yaml

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
  - name: configname
    patches:
    - fromFieldPath: spec.claimRef.namespace
      toFieldPath: spec.providerConfigRef.name
      policy:
        fromFieldPath: Required

  - name: DatabaseStatus
    patches:
      - type: ToCompositeFieldPath
        fromFieldPath: status.conditions
        toFieldPath: status.dbInstanceConditions
        policy:
          fromFieldPath: Optional

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
    patches:
    - type: PatchSet
      patchSetName: configname
    - fromFieldPath: "metadata.name"
      toFieldPath: "spec.forProvider.groupName"     
  - name: rdsinstance
    base:
      apiVersion: database.aws.crossplane.io/v1beta1
      kind: RDSInstance
      spec:
        forProvider:
          dbInstanceClass: db.t2.small
          engine: postgres
          masterUsername: masteruser
          skipFinalSnapshotBeforeDeletion: true
          allowMajorVersionUpgrade: true
          # Match only managed resources that are part of the same composite, i.e.
          # managed resources that have the same controller reference as the
          # selecting resource.
          vpcSecurityGroupIDSelector:
            matchControllerRef: true
            # Match only managed resources with the supplied labels (there might be multiple within the same composition).
            # matchLabels:
            # example: label

        writeConnectionSecretToRef:
          namespace: crossplane-system
    patches:
    - type: PatchSet
      patchSetName: DatabaseStatus    
    - type: PatchSet
      patchSetName: configname
    - type: CombineFromComposite
      combine:
        variables: 
          - fromFieldPath: spec.claimRef.namespace          
          - fromFieldPath: spec.claimRef.name    
        strategy: string        
        string:
          fmt: "%s-%s-databaseserver"
      toFieldPath: spec.writeConnectionSecretToRef.name

    - type: ToCompositeFieldPath
      fromFieldPath: "status.atProvider.endpoint.address"
      toFieldPath: "status.address"
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
      fieldPath: "status.atProvider.dbInstanceStatus"  # important to have CompositeDatabaseInstance be in ready state = True so claims can work properly
      matchString: "available"
  writeConnectionSecretsToNamespace: crossplane-system
```

### 4. Apply the composition manifest

```
kubectl apply -f composition.yaml
```

## Want to help make our training material better?
 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/dojo/issues).