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

### 1. Create index.html page for website
Create a file called `index.html` to act as the landing page for your personal website and add the following content:

```
<html>
    <h1>Hello from crossplane</h1>
</html>
```

### 2. Add ProviderConfig.yaml ??

### 3. Deploy the ProviderConfig manifest

### 4. Create s3bucket.yaml that keeps your S3 bucket configuration on your filesystem

### 5. Deploy the S3 bucket manifest

### 6. Upload index.html to S3 bucket

### 7. Verify that index.html has been uploaded to S3 bucket

### 8. Update S3 bucket configurations

### 9. Observe sync status

### 10. Cleanup resources

## Want to help make our training material better?
 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/dojo/issues).