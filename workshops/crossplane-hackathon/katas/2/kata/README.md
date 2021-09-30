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

### 4. Add ProviderConfig.yaml ??

### 5. Deploy the ProviderConfig manifest

### 6. Create s3bucket.yaml that keeps your S3 bucket configuration on your filesystem

### 7. Deploy the S3 bucket manifest

### 8. Upload index.html to S3 bucket

### 9. Verify that index.html has been uploaded to S3 bucket

### 10. Update S3 bucket configurations

### 11. Observe sync status

### 12. Cleanup resources

## Want to help make our training material better?
 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/dojo/issues).