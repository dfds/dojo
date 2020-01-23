# ado-k8s-aws-getstarted
This is sample code for getting up and running with sample code that lets you create an s3 bucket, create a docker image and read from the bucket from within a container running on kubernetes. All through a pipeline.

## AWS resources
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

Go to the terraform-1 folder:
```bash
cd terraform-1
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

Go back to the root workshop folder
```bash
cd ..
```

## Terraform for teams and pipelines
This is all very nice but due to the nature of terraform it saves something called a state. The state keeps track of the resources created with terraform and uses it to handle changes afterwards.
By default the state is saved in the folder with the terraform files which would be a huge problem if used in a team with more than 1 computer or if done within a pipeline.

To solve this we can introduce a concept of shared state.

Go to terraform-2

```bash
cd terraform-2
```
If you look inside the main.tf file you will notice it is very similar to the previous one. Except for the fact that we have included a terraform block with a backend called s3.

Notice that inside the block there is a bucket name and a key path.
Change these to something that makes sense for you, and remember that buckets needs to be unique across AWS.

This means that it will store the state of the things it create inside an s3 bucket instead of on your local machine. Unfortunately it won't create the bucket by itself.
There are tools that can do this for you but let's do it manually just to grasp the concept. Remember to change the bucket name to match what you used inside the block:

```bash
aws s3 mb s3://dfds-k8sworkshop-bucket-state --region eu-west-1
```

Now we got a bucket. But just in case something goes wrong like your computer locking up, internet get cut or what ever issues we IT people face, let's also put versioning on our bucket.

If the state is somehow corrupted we can easily role back so we got a working state of our terraform resources.

Remember to change the bucket name and then run this command:
```bash
aws s3api put-bucket-versioning --bucket dfds-k8sworkshop-bucket-state --versioning-configuration Status=Enabled
```

Go a head and follow the same steps as before:
```bash
terraform init
terraform plan
terraform apply
```

Don't forget to clean up
```bash
terraform destroy
cd ..
```