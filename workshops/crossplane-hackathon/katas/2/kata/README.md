DFDS Crossplane Hackathon - Code kata #2
======================================

This training exercise is a **intermediate-level** course on Crossplane that serves as a starting point for Developers looking to onboard the provisioning efforts at DFDS.

## Getting started
These instructions will help you prepare for the kata and ensure that your training machine has the tools installed you will need to complete the assignment(s). If you find yourself in a situation where one of more tools might not be available for your training environment please reach out to your instructor for assistance on how to proceed, post an [issue in our repository](https://github.com/dfds/dojo/issues) or fix it yourself and update the kata via a [pull request](https://github.com/dfds/dojo/pulls).

### Prerequisites
* Kata 2

## Exercise
This assignment will see you create a composite resource by creating a definition and a composition. You will deploy these into your cluster and verify them.

### 2. Create a definition.yaml
First we need to create a Composite Resource Definition (XRD) that describes the resource parameters we need to define in order to create the resource.

Create a definition.yaml containing the below content:

```
apiVersion: apiextensions.crossplane.io/v1
kind: CompositeResourceDefinition
metadata:
  name: xmybuckets.storage.example.org
spec:
  group: storage.example.org
  names:
    kind: XMyBucket
    plural: xmybuckets
  claimNames:
    kind: MyBucket
    plural: mybuckets
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
                  bucketName:
                    type: string
                required:
                  - bucketName

```

### 3. Apply the definition manifest and verify its installation

Apply the manifest file to deploy the XRD into your cluster by running the following command:

```
kubectl apply -f definition.yaml
```

Verify its existence by running the following command:

```
kubectl get xrd
```

Verify that it has created the custom resource definitions (claims) by running the following command:

```
kubectl get crd | grep example.org
```


### 4. Create a composition.yaml

Now we will create a composition manifest which declares the resources that are to be created with base settings/defaults.

Create a composition.yaml and populate with the following contents:

```
apiVersion: apiextensions.crossplane.io/v1
kind: Composition
metadata:
  name: xmybuckets.aws.storage.example.org
  labels:
    provider: aws
spec:
  compositeTypeRef:
    apiVersion: storage.example.org/v1alpha1
    kind: XMyBucket
  resources:
    - name: s3bucket
      base:
        apiVersion: s3.aws.crossplane.io/v1beta1
        kind: Bucket
        spec:
          forProvider:
            acl: public-read-write
            locationConstraint: us-east-1
          providerConfigRef:
            name: localstack-aws
      patches:
        - fromFieldPath: "spec.parameters.bucketName"
          toFieldPath: "metadata.name"
          transforms:
            - type: string
              string:
                fmt: "org-example-%s"
    - name: s3bucket2
      base:
        apiVersion: s3.aws.crossplane.io/v1beta1
        kind: Bucket
        spec:
          forProvider:
            acl: public-read-write
            locationConstraint: us-east-1
          providerConfigRef:
            name: localstack-aws
      patches:
        - fromFieldPath: "spec.parameters.bucketName"
          toFieldPath: "metadata.name"
          transforms:
            - type: string
              string:
                fmt: "org-example-2-%s"

```

### 5. Apply the composition manifest and verify its installation

Apply the manifest to deploy the composition to your cluster by running the following command:

```
kubectl apply -f composition.yaml
```

Verify that the composition exists by running the following command:

```
kubectl get composition
```

### 6. Make a claim.yaml manifest

Here we create a manifest file which defines the creation of an instance of our composite resource

```
apiVersion: storage.example.org/v1alpha1
kind: MyBucket
metadata:
  name: my-bucket
  namespace: default
spec:
  compositionSelector:
    matchLabels:
      provider: aws
  parameters:
    bucketName: test-bucket
```

Next we will deploy our manifest into our cluster:

```
kubectl apply -f claim.yaml
```

We can then verify the creation of our composite resource:

```
kubectl get MyBucket
```

And we should also verify that the 2 s3 buckets have been created by the composite:

```
kubectl get Bucket
```

And verify that the s3 buckets are visible using the AWS CLI:

```
kubectl exec --stdin --tty aws-cli-runtime -- aws s3 --endpoint-url=http://localstack.default.svc.cluster.local:4566 ls
```

### 7. Cleanup Resources

We need to clean up our resources so that they are not left behind for the next Kata

```
kubectl delete -f claim.yaml
kubectl delete -f composition.yaml
kubectl delete -f definition.yaml
```


## Want to help make our training material better?
 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/dojo/issues).