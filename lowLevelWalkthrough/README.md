# ado-k8s-aws-getstarted
This is sample code for getting up and running with sample code that lets you create an s3 bucket, create a docker image and read from the bucket from within a container running on kubernetes. All through a pipeline while utilizing built in features to secure your workload.

## Pre-reqs
To follow this workshop you will have to the following tools installed:
* [Docker](https://hub.docker.com/?overlay=onboarding)
* [Terraform < 0.12.19](https://releases.hashicorp.com/terraform/0.12.19/)
* [AWS CLI](https://aws.amazon.com/cli/)
* [KubeCTL](https://kubernetes.io/docs/tasks/tools/install-kubectl/)
* [Saml2aws](https://github.com/Versent/saml2aws)
* [aws-iam-authenticator](https://docs.aws.amazon.com/eks/latest/userguide/install-aws-iam-authenticator.html)

Docker is a well known tool which is easily found on the internet. It is required to have a docker hub account to get it from the [offical source](https://hub.docker.com/?overlay=onboarding) but the install process is described here: [Install Docker Desktop on Windows](https://docs.docker.com/docker-for-windows/install/)

Terraform can be downloaded for the platform you you need.
In general I recommend the latest version but once you touch the code only your version or above can modify it afterwards. As this is written the current supported version in pipelines is 0.12.19 so I recommend that or a version before it to avoid issues with this walkthrough.
Also notice that the Terraform download is a single executeable file which should be added to your [path](https://superuser.com/questions/284342/what-are-path-and-other-environment-variables-and-how-can-i-set-or-use-them) for the best experience.

The last 4 can be installed following this:
* [Technical prerequisites (Windows)](https://playbooks.dfds.cloud/getting-started/prereqs-win.html#setting-up-tools-as-administrator)
* [Technical prerequisites (Linux)](https://playbooks.dfds.cloud/getting-started/prereqs-linux.html)
* [Technical prerequisites (Mac)](https://playbooks.dfds.cloud/getting-started/prereqs-mac.html)

What are the tools for?
* We will be using docker to build and run containers locally
* We will use AWS CLI to manage cloud resources and make a sample application
* We will use KubeCTL to verify our deployments and debug them
* We will use saml2aws to grant access to our cloud environment via our company account
* We will use aws-iam-authenticator to convert our cloud environment login to a login for the kubernetes platform

Before you begin you should have each of these tools installed, joined the ded-workshops capability at [build.dfds.cloud](https://build.dfds.cloud/capabilitydashboard?capabilityId=f45ec4da-e5d8-4c3e-b688-59e9b1de3fd1) and downloaded the [default kubeconfig file](https://build.dfds.cloud/downloads/kubeconfig)

This walkthrough is designed to be ran from command line so depending on your platform, open up a powershell or terminal window and get ready.

## 1 - AWS resources
Welcome to this walkthrough.
We are going to build a very simple application that reads a file for a cloud data storage and prints out the content.
This sample can be expanded to include other types of resources or data manipultation but is kept to the basics to focus on the core practices.

The application is going to run in a kubernetes cluster.
By design kubernetes is built to host stateless applications. If you do decide to have state inside the cluster you can experience issues during updates of the cluster and rebalancing of the containers/pods running in the cluster.
The advise is to keep the state of your application outside of the cluster and which is why we start by adding some storage by setting up an s3 bucket with Terraform.

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
cd 1-terraform
```

Inside the 1-terraform folder you will find a simple terraform file, named main.tf, which will include a region to provision in and the recipe for an s3 bucket.

Try opening it and see if you can understand the content. Maybe even talk about it with the person next to you.

When running this sample code by yourself I would recommend changing the bucket name since it has to be unique across AWS. If you run the sample as it is you might experience errors like the bucket already exists.

From within this folder run a terraform init followed by a plan.

```bash
terraform init
terraform plan
```

The init sets up the API providers to enable terraform to talk with AWS and the plan will show you what will happen if you apply the terraform.
The init is only needed the first time terraform is ran from a folder.

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

You can sign in the AWS console in your browser and verify that the bucket is in fact there.

End this part by cleaing up with
```
terraform destroy
```

Go back to the root workshop folder
```bash
cd ..
```

## 2- Terraform for teams and pipelines
You now know how to make a bucket using Infrastrucre as code with Terraform.
This is all very nice but due to the nature of Terraform it saves something called a state.
The state keeps track of the resources created with Terraform and uses it to handle changes afterwards.
By default the state is saved in the folder with the terraform files which would be a huge problem if used in a team with more than 1 computer or if done within a pipeline which is cleaned out after each run. If done so you will get errors like the resources already exists on next run.

To solve this we can introduce a concept of shared state.
Instead of having the state on our local machine we push it to cloud storage in an s3 bucket. That way everybody who should work on this got the same state.

Go to the 2-terraform folder

```bash
cd 2-terraform
```
If you look inside the main.tf file you will notice it is very similar to the previous one. Except for the fact that we have included a terraform block with a backend called s3.

Notice that inside the block there is a bucket name and a key path.
Change these to something that makes sense for you, and remember that buckets needs to be unique across AWS.

This means that it will store the state of the things it create inside an s3 bucket instead of on your local machine. Unfortunately it won't create the bucket by itself.
There are tools that can do this for you. For additional reading you can have a look at [Terragrunt](https://terragrunt.gruntwork.io/docs/getting-started/quick-start/) which can auto create the state bucket and let you run multiple terraforms in parallel. For simplicity let's do it manually just to grasp the concept. Remember to change the bucket name to match what you used inside the block:

```bash
aws s3 mb s3://dfds-k8sworkshop-bucket-state --region eu-west-1
```

Now we got a bucket. But just in case something goes wrong like your computer locking up, internet get cut or what ever issues we IT people face, let's also put versioning on our bucket.

If the state is somehow corrupted we can easily revert back so we got a working state of our terraform resources.
Another concept that can be used to protect the state is [state locking](https://www.terraform.io/docs/state/locking.html). State locking means that Terraform locks the state when it begins running to avoid multiple pipelines or people are editing the same resources at once. I recommend doing this for production environments.

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

If you go to the AWS console again you should be able to see your 2 buckets. One for content and one for the state. Inside the state bucket you will be able to read the current state of the deployed resources by Terraform.
The state bucket should be protected since it can contain sensitive information like passwords for databases etc so make sure to limit who and what got access to this.

Don't forget to clean up and go back to the root folder
```bash
terraform destroy
cd ..
```

## 3 - Docker image



Next we are going to build a docker image that can use our aws s3 bucket.

Go to the docker-3 folder

```bash
cd docker-3
```

Inspect the Dockerfile.
You will see it is build from different components.
It got a base image which is an image built by somebody else.
In this case python which we will use to install AWS CLI.

Next we make sure the image is patched and upgraded. Just to follow best practices.

Then we install curl followed by AWS CLI which we will be using.

And last we preset the folders for the image.

Build this image by running:

```bash
docker build -t awscli .
```

Once the image is built we can try use it

```bash
docker run -it awscli
```

This will return the help text from the AWS CLI since we haven't specified a command.
Seeing this means that our image works and is ready to accept input.

Return to the root folder

```bash
cd ..
```

## Combining AWS and Docker

So far we should have learned how to create AWS resources and how to create and run docker images.
But how do we mix it?

```bash
cd docker-4
```

If you inspect the terraform file you will notice another resource is added to the file.
This one is a bucket file and in this case a simple txt file.

Remember to change the bucket name of the bucket and the state and let's provision it.

```bash
terraform init
terraform plan
terraform apply -auto-approve
```

Next let's see if we can access the file from our docker image

```bash
docker run \
-e AWS_ACCESS_KEY_ID=$(aws configure get saml.aws_access_key_id) \
-e AWS_SECRET_ACCESS_KEY=$(aws configure get saml.aws_secret_access_key) \
-e AWS_SECURITY_TOKEN=$(aws configure get saml.aws_session_token) \
-e X_PRINCIPAL_ARN=$(aws configure get saml.x_principal_arn) \
awscli s3 cp s3://dfds-k8sworkshop-bucket/testfile.txt -
```

This should return the message hello from s3 which corrospond to the content of the file created through terraform.

What we do here is running the docker image as a container, mounting our credentials we got from saml2aws as environment variables inside the container and executing the command s3 copy from path to terminal output.

This is very handy for local development where we can change the file by editing and applying our terraform and handle the file in different ways sending commands to our container.

But wouldn't it be nice if the container just did what we wanted it to without having to write the command?

Ïnspect the Dockerfile.
You will notice that a script is added and the entrypoint is changed.
Inspect the script found in get-file.sh.
You will notice that this file include our "application" which prints the content of an s3 bucket every 5 seconds.
It finds the content based on an environment variable.

Let's rebuild the image with the changes:
```bash
docker build -t awscli .
```
One of the neat features with docker build is that it is based on layers. It will find the changes performed and only rebuild everything after that point.
If you structure it  correctly you can speed up your built process by a lot!

Lets try to run this container with the path to our file. Remember to change bucket and file name.

```bash
docker run \
-e AWS_ACCESS_KEY_ID=$(aws configure get saml.aws_access_key_id) \
-e AWS_SECRET_ACCESS_KEY=$(aws configure get saml.aws_secret_access_key) \
-e AWS_SECURITY_TOKEN=$(aws configure get saml.aws_session_token) \
-e X_PRINCIPAL_ARN=$(aws configure get saml.x_principal_arn) \
-e path_to_file="s3://dfds-k8sworkshop-bucket/testfile.txt" \
awscli
```

You can exit by clicking "ctrl+c" on your keyboard. 

End this part by cleaing up with
```
terraform destroy
```

Go back to the root workshop folder
```bash
cd ..
```

## Sharing images and running in a pipeline

It is very nice to be able to build images locally and test them out but just like with the terraform state you might want to work together as a team.
To do so you would need to share the images with your team members and the process for doing so in DFDS is through a pipeline.

In the root workshop folder you will be able to see a file named azure-pipeline.
This is the final yaml description of the pipeline for this project.

If you inspect it you will see that the first part os about behavior of the pipeline. Triggers, build agent etc.

The next part about Stages is the exciting part since this is where your application is actually built.

The first stage of the pipeline is used to validate if the neccesary input is available.
If it is not there is no reason to wait for the pipeline to fail since we already know it based on this.

Next one is building the docker image.
Just like we did locally, we can build the docker image through our pipeline and make an image for the container available.

Right after the build step there is a push task.
In DFDS the DED team provides a service connection for new projects in ADO for those that need access to the shared docker repository. This one is hosted in AWS and can be seen in the push step as awsCredentials.
The rest of the arguments is for our image like the name of it, which repository it should be pushed to and which tag it should have.
Since you can have multiple builds of your image and some of them failing it is very handy to have a history in case you need to use a previous one. For ease of naming the tag is set to the build id so ensure it is unique.

This is set to trigger on any changes to master so we would be able to push new images if the code in here is change. Always keeping it up to date.

If you do a saml2aws login and choose the ECR-Pull role with

```bash
saml2aws login --force
```

Then you can do

Windows:
```Powershell
aws ecr get-login --no-include-email --region eu-central-1 --profile saml | Invoke-Expression
```

Unix:
```bash
eval $(aws ecr get-login --no-include-email --region eu-central-1 --profile saml)
```

Then do:
```bash
docker pull 579478677147.dkr.ecr.eu-central-1.amazonaws.com/ded/workshops:217340
docker images
```

You should be able to see the image at the top of the list with the tag 217340

Do another round of login to get back to your aws account:

```bash
saml2aws login --force
```

Set up the s3 bucket with the test file again:
```bash
cd pipeline-5
terraform init
terraform plan
terraform apply
```

You can then test it out with the command from earlier and the new image:

```bash
docker run \
-e AWS_ACCESS_KEY_ID=$(aws configure get saml.aws_access_key_id) \
-e AWS_SECRET_ACCESS_KEY=$(aws configure get saml.aws_secret_access_key) \
-e AWS_SECURITY_TOKEN=$(aws configure get saml.aws_session_token) \
-e X_PRINCIPAL_ARN=$(aws configure get saml.x_principal_arn) \
-e path_to_file="s3://dfds-k8sworkshop-bucket/testfile.txt" \
579478677147.dkr.ecr.eu-central-1.amazonaws.com/ded/workshops:217340
```

You should see the text from the image printed again but this time from the image build by the pipeline and ran on your computer.

That means your image is now shareable with the team and deployable to a kubernetes cluster!

Don't forget to clean up
```bash
terraform destroy
cd ..
```

## Deploying to kubernetes

