DFDS Crossplane Hackathon - Code kata #2
======================================

This training exercise is a **intermediate-level** course on Crossplane that serves as a starting point for Developers looking to onboard the provisioning efforts at DFDS.

## Getting started
These instructions will help you prepare for the kata and ensure that your training machine has the tools installed you will need to complete the assignment(s). If you find yourself in a situation where one of more tools might not be available for your training environment please reach out to your instructor for assistance on how to proceed, post an [issue in our repository](https://github.com/dfds/dojo/issues) or fix it yourself and update the kata via a [pull request](https://github.com/dfds/dojo/pulls).

### Prerequisites
* Kata 1
* [Terraform](https://learn.hashicorp.com/tutorials/terraform/install-cli)

## Exercise
This assignment will see you provision a S3 bucket via the Crossplane Terraform provider to act as storage for your very own Hello website. This will showcase how The Terraform provider for Crossplane can be used to consuming terraform modules and configurations from Crossplane setup in Kubernetes.

### 1. Prepare the Terraform environment
The Terraform provider require remote state, so will be creating 

### 2. Install the Terraform provider
We need to install the Terraform provider for crossplane to be able to deploy AWS resources

```
kubectl crossplane install provider crossplane/provider-aws:v0.18.1
```



### 3. Create index.html page for website
Create a file called `index.html` to act as the landing page for your personal website and add the following content:

```
<html>
    <h1>Hello from crossplane</h1>
</html>
```

### 4. Add providerconfig.yaml
```
apiVersion: tf.crossplane.io/v1alpha1
kind: ProviderConfig
metadata:
  name: tf-default
spec:
  credentials:
  - filename: aws-tf-creds
    source: Secret
    secretRef:
      namespace: default
      name: aws-creds
      key: key        
  configuration: |
      terraform {
        backend "s3" {
          bucket                      = "crossplane-dfds-state"
          key                         = "main/terraform.tfstate"
          region                      = "eu-central-1"
          force_path_style            = true
          dynamodb_table              = "terraform-locks"
          encrypt                     = true
          shared_credentials_file     = "aws-tf-creds"
        }
      }

      provider "aws" {
        shared_credentials_file     = "aws-tf-creds"
        region                      = "eu-central-1"   
        s3_force_path_style         = true    
      }
```      


### 5. Deploy the ProviderConfig manifest
Deploy this manifest to our cluster. ProviderConfigs exist at the cluster level and can not be namespaced

```
kubectl apply -f providerconfig.yaml
```

Verify that the providerconfig exists

```
kubectl get providerconfig.aws.crossplane.io
```
### 6. Create s3bucket.yaml that keeps your S3 bucket configuration on your filesystem
Once Crossplane has gotten a valid ProviderConfig it is able to begin provisioning resources in your AWS account. To create a S3 bucket we need to add the following:

```
apiVersion: tf.crossplane.io/v1alpha1
kind: Workspace
metadata:
  name: workspace-s3bucket
spec:
  providerConfigRef:
    name: tf-default
  forProvider:    
    # For simple cases you can use an inline source to specify the content of
    # main.tf as opaque, inline HCL.
    source: Inline
    module: |
      // Outputs are written to the connection secret.

      resource "aws_s3_bucket" "b" {
        bucket = "your-test-bucket"
        acl    = "public-read"  
      }

      output "url" {
        value       = aws_s3_bucket.b.id
      }
      
  writeConnectionSecretToRef:
    namespace: default
    name: tf-workspace-s3bucket
```

Just to explain: <br/>
`kind: Workspace` - is terraform workspace to keep the group of the resources you want to provision<br/>

### 7. Deploy the S3 bucket manifest
We will deploy manifest into our cluster

```
kubectl apply -f s3bucket.yaml
```

### 8. Upload index.html to S3 bucket
Upload content to S3 bucket using the AWS CLI:

```
aws s3 cp index.html s3://your-test-bucket --acl public-read
```

**Note**: Replace your-test-bucket with the name of the bucket you created in step 6
### 9. Verify that index.html has been uploaded to S3 bucket
The following AWS command will list the content of the bucket

```
aws s3 ls your-test-bucket
```

### 10. Update S3 bucket configurations
Update ACL configuration from public-read to private

```
resource "aws_s3_bucket" "b" {
    bucket = "your-test-bucket"
    acl    = "private"  
}
```
### 11. Observe sync status
```
kubectl describe workspace.tf.crossplane.io/workspace-s3bucket
```

### 12. Get Terraform output from secret
```
kubectl get secret tf-workspace-s3bucket -o yaml
```

### 12. Cleanup resources

We should clean up resources so that we do not incur any unnecessary costs

```
kubectl delete -f s3bucket.yaml
```
Check if S3 bucket has been deleted:
```
kubectl get workspace
```
If no results returned, then proceed with deleting the ProviderConfig resource:
```
kubectl delete -f providerconfig.yaml
```

## Want to help make our training material better?
 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/dojo/issues).