# ado-k8s-aws-getstarted
This is sample code for getting up and running with sample code that lets you create an s3 bucket, create a docker image and read from the bucket from within a container running on kubernetes. All through a pipeline.

Checkout step1 to get started.

## step-1-terraform
Sign in with saml2aws

### Windows
```Powershell
saml2aws login --force
$env:AWS_PROFILE = 'saml'
```

### Unix
```bash
saml2aws login --force
export AWS_PROFILE=saml
```

Inside the terraform-1 folder you will find a simple terraform file which will include a region to provision in and the recipe for an s3 bucket.

If running by yourself I would recommend changing the bucket name since it has to be unique across AWS.

From within this folder run a terraform init followed by a plan.

```bash
terraform init
terraform plan
```

The init sets up the API providers to enable terraform to talk with AWS and the plan will show you what will happen if you apply the terraform.

You should see something similar to this:

```bash
------------------------------------------------------------------------

An execution plan has been generated and is shown below.
Resource actions are indicated with the following symbols:
  + create

Terraform will perform the following actions:

  + aws_s3_bucket.test-bucket
      id:                          <computed>
      acceleration_status:         <computed>
      acl:                         "private"
      arn:                         <computed>
      bucket:                      "dfds-k8sworkshop-bucket"
      bucket_domain_name:          <computed>
      bucket_regional_domain_name: <computed>
      force_destroy:               "false"
      hosted_zone_id:              <computed>
      region:                      <computed>
      request_payer:               <computed>
      versioning.#:                <computed>
      website_domain:              <computed>
      website_endpoint:            <computed>


Plan: 1 to add, 0 to change, 0 to destroy.

------------------------------------------------------------------------
```


The important to notice part here is the line with the plan. It will inform you if your terraform will create new resources, change some or even delete.

If you do mistakes in your code you will very likely experience that your resources will be listed as destroy.
This can become a huge issue if it targets your database in production and accidentially delete your whole database.
It is a very good idea to do the plan and look it over before moving on.

For this it shouldn't be an issue and you can safely do the apply.

```bash
terraform apply
```

It will show you the plan again and ask if you want to apply it. Do so by typing in "yes".

For pipelines etc it is a good idea to use the auto approve since it can be troublesome to require user input. It is done like this:

```bash
terraform apply -auto-approve
```

End this part by cleaing up with
```
terraform destroy
```
