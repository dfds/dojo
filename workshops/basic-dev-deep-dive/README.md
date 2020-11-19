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

Docker is a well known tool which is easily found on the internet. It is required to have a docker hub account to get it from the [official source](https://hub.docker.com/?overlay=onboarding) but the install process is described here: [Install Docker Desktop on Windows](https://docs.docker.com/docker-for-windows/install/)

Terraform can be downloaded for the platform you you need.
In general I recommend the latest version but once you touch the code only your version or above can modify it afterwards. As this is written the current supported version in pipelines is 0.12.19 so I recommend that or a version before it to avoid issues with this walk-through.
Also notice that the Terraform download is a single executable file which should be added to your [path](https://superuser.com/questions/284342/what-are-path-and-other-environment-variables-and-how-can-i-set-or-use-them) for the best experience.

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

This walk-through is designed to be ran from command line so depending on your platform, open up a powershell or terminal window and get ready.

## 1 - AWS resources
Welcome to this walk-through.
We are going to build a very simple application that reads a file for a cloud data storage and prints out the content.
This sample can be expanded to include other types of resources or data manipulation but is kept to the basics to focus on the core practices.

The application is going to run in a kubernetes cluster.
By design kubernetes is built to host stateless applications. If you do decide to have state inside the cluster you can experience issues during updates of the cluster and re-balancing of the containers/pods running in the cluster.
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
This can become a huge issue if it targets your database in production and accidentally delete your whole database.
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

When playing with cloud resources it is a very good idea to delete your work afterwards. Leaving cloud resources running will generate a cost and since we script it we might as well just remove it afterwards sinc we can easily spin it up again.

```
terraform destroy
```

Go back to the root workshop folder
```bash
cd ..
```

## 2- Terraform for teams and pipelines
You now know how to make a bucket using Infrastructure as code with Terraform.
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

From the previous modules we built a place to store data for out application and since we got that we should start building our actual application.

 We are going to build a docker image that can use our aws s3 bucket for its data.

Go to the 3-docker folder

```bash
cd 3-docker
```

Inspect the Dockerfile.
You will see it is build from different components.
It got a base image which is an image built by somebody else.
In this case python the base includes system commands and python which we will use to install AWS CLI.

Next we make sure the image is patched and upgraded. Just to follow best practices.

Then we install curl followed by AWS CLI which we will be using for our application.

And last we preset the folders for the image.

Build this image with docker by running:

```bash
docker build -t awscli .
```
It is important the Dockerfile is named exactly as it is since the build command will just look at the folder path it is stored, in this case ., and build it from there. In this command we also tag our image so it gets an easy name to remember and find it by.

Once the image is built we can try use it

```bash
docker run -it awscli
```

This will return the help text from the AWS CLI since we haven't specified a command.
Seeing this means that our image works and is ready to accept input.

In this module we made a docker image and tried running it as a container. This means we got a package of tools that we can spin up as needed without having to install them. This makes it easy to move this package around between machines like deployments on environments etc.

Return to the root folder

```bash
cd ..
```

## Combining AWS and Docker

In the previous modules we prepared storage in the cloud and made a container that includes the tools we need to utilize our storage.
But how do we mix it?

Jump in to the folder for module 4:

```bash
cd 4-docker
```

If you inspect the Terraform file, main.tf, you will notice another resource, named bucket-object, is added to the file.
This one is a bucket file and in this case is just a simple txt file with a test text inside of it.

Remember to change the bucket name of the bucket and the state in the Terraform file and let us provision it.

```bash
terraform init
terraform plan
terraform apply -auto-approve
```

Next let's see if we can access the file from our docker image.

Since we signed in earlier with saml2aws we got a session on our computer stored to let us interact with the s3 bucket. If you are experiencing issues with this try signing in again:

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

Spin up the docker image with your sessions credentials attached as environment variables and

```bash
docker run \
-e AWS_ACCESS_KEY_ID=$(aws configure get saml.aws_access_key_id) \
-e AWS_SECRET_ACCESS_KEY=$(aws configure get saml.aws_secret_access_key) \
-e AWS_SECURITY_TOKEN=$(aws configure get saml.aws_session_token) \
-e AWS_SESSION_TOKEN=$(aws configure get saml.aws_session_token) \
-e X_PRINCIPAL_ARN=$(aws configure get saml.x_principal_arn) \
awscli s3 cp s3://dfds-k8sworkshop-bucket/testfile.txt -
```

This should return the message hello from s3 which correspond to the content of the file created through Terraform.

What we do here is running the docker image as a container, mounting our credentials we got from saml2aws as environment variables inside the container and executing the command s3 copy from path to terminal output.

This is very handy for local development where we can change the file by editing and applying our terraform and handle the file in different ways sending commands to our container.

We are currently utilizing the fact that docker containers are only running as long as the main process is executing. Since we only execute a single command against it, it shuts down once it is done.

That means we don't really have anything to clean up unless we want to remove the images from our local machine.

These don't generate a cost so cleaning those will not be done in this walk-through.

But shutdown the s3 bucket:

```bash
terraform destroy
```

Return to the root:

```bash
cd ..
```
# 5 - Default behavior in docker

in the previous module we created a docker image that included the aws cli tools and accepted commands as input which would return the wanted output.

But wouldn't it be nice if the container just did what we wanted it to without having to write the command?

```bash
cd 5-docker
```

Inspect the Dockerfile.
You will notice that a script is added and the entrypoint is changed.
Inspect the script found in get-file.sh
You will notice that this file include our "application" which prints the content of an s3 bucket every 5 seconds.
It finds the content based on an environment variable which makes it easier for us to point our "application" to the correct file we want it to read through development to production.

Let us rebuild the image with the changes:
```bash
docker build -t awscli .
```
One of the neat features with docker build is that it is based on layers. It will find the changes performed and only rebuild everything after that point.
If you structure it correctly you can speed up your built process by a lot!

Lets try to run this container with the path to our file. Remember to change bucket and file name.

```bash
docker run \
-e AWS_ACCESS_KEY_ID=$(aws configure get saml.aws_access_key_id) \
-e AWS_SECRET_ACCESS_KEY=$(aws configure get saml.aws_secret_access_key) \
-e AWS_SECURITY_TOKEN=$(aws configure get saml.aws_session_token) \
-e AWS_SESSION_TOKEN=$(aws configure get saml.aws_session_token) \
-e X_PRINCIPAL_ARN=$(aws configure get saml.x_principal_arn) \
-e path_to_file="s3://dfds-k8sworkshop-bucket/testfile.txt" \
awscli
```
You can exit by clicking "ctrl+c" on your keyboard. 

Notice that we didn't have to specify the aws s3 cp command when running the container and that it kept running even after the first output.

End this part by cleaning up with

```
terraform destroy
```

Go back to the root workshop folder

```bash
cd ..
```

## 6 - Sharing images and running in a pipeline

It is very nice to be able to build images locally and test them out but just like with the Terraform state you might want to work together as a team.
To do so you would need to share the images with your team members and the process for doing so in DFDS is through a pipeline.

In the root workshop folder you will be able to see a file named azure-pipeline.
This is the final yaml description of the pipeline for this project.
But if you look at line 40 through 60 you can see the steps performed to build the image through a build agent and afterwards publish the image in the shared container repository for DFDS.

Since the images published here can be, and most often are, used for production we disapprove of pushes from a local machine. Instead we encourage people to push here from a pipeline to keep it versioned, consistent and automated.

If your team needs to publish images you can request an AWS service connection inside your Azure DevOps project which allows your team to push images for prod use. If you want more control with your images you can run [your own repository](https://playbooks.dfds.cloud/training/ecr-capability.html) and be responsible for creating your own sub repository and the access levels

Once an image is published in the repository it can be pulled from your colleagues machines.
Lets pretend that somebody published an image for us that we want to run locally to see how it works together with the terraform that we have been building.

If you do a saml2aws login and choose the [ECR-Pull role](https://playbooks.dfds.cloud/training/ecr-local.html) with

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
cd 6-docker
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
-e AWS_SESSION_TOKEN=$(aws configure get saml.aws_session_token) \
-e X_PRINCIPAL_ARN=$(aws configure get saml.x_principal_arn) \
-e path_to_file="s3://dfds-k8sworkshop-bucket/testfile.txt" \
579478677147.dkr.ecr.eu-central-1.amazonaws.com/ded/workshops:217340
```

You should see the text from the image printed again but this time from the image build by the pipeline and ran on your computer.

That means the image is now shareable with the team and deployable to a kubernetes cluster!

Don't forget to clean up

```bash
terraform destroy
cd ..
```

## 7 - Deploying from a pipeline

Lets try to build up a pipeline to bind all this together.

```bash
cd 7-pipeline
```

In this folder you will find the docker files we have seen before and an azure devops pipeline file. This new which describes the way to deploy.

Let's go through the components since it can feel like quite a lot.

### azure-pipeline.yaml

If you inspect the file you will see that the first part is about behavior of the pipeline. Triggers, build agent etc.

Next we get to the stages of the CI part:

Here we have a section for validating inputs.
It is a good idea to check that you actually got the input for your pipeline set. If they are missing it doesn't make sense to continue the pipeline since it would fail anyway. Catching them early on can save you time in the long run.

For this pipeline we will need a [service connection for the cluster](https://playbooks.dfds.cloud/deployment/k8s-service-connection.html#intro), but instead of the kubeconfig file mentioned here it will soon be recommended to use the common [kubeconfig](https://dfds-oxygen-k8s-public.s3-eu-west-1.amazonaws.com/kubeconfig/hellman-saml.config).
If you are using the workshop project it is already in place.
The main difference between them are that the common one uses Active Directory service accounts instead of a static token. By using the service accounts each login will acquire a new login making it more secure and actively removing login info from the pipeline making it less likely that you can leak passwords.

When setting up the pipeline you would need to add the variables needed for it to run as well. The variables needed is described here: [Authenticating against AWS and Kubernetes](https://playbooks.dfds.cloud/pipelines/authentication.html#pipeline-variables) and how to obtain the info here: [Obtaining info needed for pipeline](https://playbooks.dfds.cloud/pipelines/obtain-info.html#aws-account-id)

The next part in stages is the exciting part since this is where your application is actually built.
Just like we did locally, we can build the docker image through our pipeline and make an image for the container available.

Right after the build step there is a push task.
In DFDS the DED team provides a service connection for new projects in ADO for those that need access to the shared docker repository. This one is hosted in AWS and can be seen in the push step as awsCredentials.
The rest of the arguments is for our image like the name of it, which repository it should be pushed to and which tag it should have.
Since you can have multiple builds of your image and some of them failing it is very handy to have a history in case you need to use a previous one. For ease of naming the tag is set to the build id so ensure it is unique.

This is set to trigger on any changes to master so we would be able to push new images if the code in here is change. Always keeping it up to date.

The rest is not needed for now.

Try to upload these files to a new repo in Azure DevOps, create a pipeline based on the azure-pipeline.file and configure the variables correctly.

In the ded-workshops project there is an extended version of the pipeline will all this configured for inspiration.

Once complete go back to root

```bash
cd ..
```

## 8 - Automating terraform deployment

If you completed the previous step you should have a working pipeline that builds and publishes a docker image with our code for getting content from an s3 bucket.

Next let's try to make the pipeline provision our Terraform bucket.

Go to the module folder

```bash
cd 8-pipeline
```

We have the same files as before but added our Terraform from earlier.
Inspect the pipeline file.

Another step has been added to the CI where we publish our terraform files. When you set up your own pipeline you might need to adjust the path to the folder a bit.
This step converts our terraform to an artifact which can be used during our CD (deploy phase).

After the publish step the CD stage begins.
Remember all the tools we needed for this workshop at the beginning? Our build-server needs those as well so we got a bash step first that prepares the server to handle the way we deploy things. It would be possible to make this step cleaner by converting it to an image etc but to keep it visible and without baking too much in to our application we have decided to keep it here.

It is a recipe for installing and configured it all just like we did at the very beginning.

Next we get the artifacts for Terraform

Followed by a step that... You guessed it... initialize Terraform and deploys the code.

Since we have the shared state bucket for earlier it will just continue to use this and keep the state available here for future use.

Try configuring your own pipeline with these additions.

The pipeline should now create your docker image, publish it and create the s3 bucket with content for reading.

Go back to the root folder

```bash
cd ..
```

# 9 - Deploy to Kubernetes

By now we should have an available image and the data storage running in the cloud, all handled by a pipeline. So Lets deploy our application to the cluster and watch it run.

Go to the module folder

```bash
cd 9-pipeline
```

We have pretty much the same files as before.
But we have added the deployment.yaml file.

Inspect the file and see if you can understand it.

The first part of it is a secret. A normal way to deploy applications is by reading environment variables for various things like database connections etc. This enables the same application to span across multiple deployments without having to hardcode these values in to the application. This could also be said for usernames and passwords.

Our application took the path for the file in s3 as an environment variable. It is not a very secret thing but the principle works out just as well so here it is created as a secret.
For a real secret it should be substituted in to the deployment file when the pipeline ran to avoid having it hardcoded in the code which could be leaked in to git.

The next part is about deploying our application. Here it is made as what is called a "deployment". A deployment can control multiple pods.
Remember earlier when we mentioned Kubernetes preferring stateless applications to enable updates etc? If you want high availability on your applications it is a very good idea to have multiple replicas of it in case one of them gets shut down during an update etc. We usually recommend a replica of 3 to enable an update and an error to occur at the same time as your application keeps running on the last one. If one of your pods in a replica gets shut down the deployment will spin it up a new place to make sure there is always the required amount but your application can be down while this happens unless you got more of them.

Do replicas. But since this is for testing we just got 1.

We then got some naming and labeling of our application.
The names, also for the secret should be changed in your version to make it your own. Otherwise you would overwrite each others deployments.

We then reach our actual deployment. a name, the image we built before followed by our environment variable mounted in from the secret.

Last part of the file is resource limits. You should figure out what your application is requiring to run and set this to match it so your application doesn't impact the cluster in case you get a memory or compute leak.

Try to get this up on your own and make a deployment to the cluster.

Once done we can try to inspect it in kubernetes!

in your commandline try this:

```bash
kubectl get deployments -n ded-workshops-ljmra
```

You should get a list of the current deployments and most likely yours among them.

We can do the same for secrets to validate it is there:

```bash
kubectl get secrets -n ded-workshops-ljmra
```

Great! What about the pod?

```bash
kubectl get pods -n ded-workshops-ljmra
```

Do you see yours on the list?
It is most likely failing since we forgot a very important thing!

Remember when we ran it locally? We forwarded our credentials for AWS to the container so it could access the s3 bucket.

We didn't specify this for our kubernetes deployment so it got no access to our file in the s3 bucket.

Lets dig in to it

```bash
kubectl -n ded-workshops-ljmra logs <your pod name>
```

You will most likely some info about your error from here. With an access denied of some kind.

You can also do something like

```bash
kubectl -n ded-workshops-ljmra describe pod <your pod name>
```
This will show the configuration your pod is deployed with.

But we are missing the credentials so lets get back to the root folder and get started fixing this mistake.

```bash
cd ..
```

# 10 - Providing cloud access to kubernetes deployments

Since we are almost there and just need to provide access to our application to use the data we have saved for it lets dig right in to it.

```bash
cd 10-pipeline
```

We don't wish publish keys for our deployment since it could be leaked and which credential should we even give it? I wouldn't want to use my own since if I changed company or even if my password ran out my prod application could crash along with it.

So what do we do?

The way that AWSCLI and SDK works is that it looks for explicit keys like those we provided our docker container.
If these are not there it will look for the default configured profile like the one we configured to be saml for local access.
But there is a third level that works inside this cluster and a few other amazon resources.
If neither of the previous are there it will ask for access through something called the metadata server.

In AWS there is a service that lets amazon resources ask for their current access level and get temporary access keys for these permissions. If no other are specified it will pull these.
Our worker nodes are in AWS so our pods can use the access level of the nodes to do what they need.

You might ask how the nodes in the cluster can access the bucket that we just created? And in short it can't. But we can configure it to it. We do this using something called [KIAM](https://playbooks.dfds.cloud/processes/k8s-pods-aws-access-assume-role.html#namespace-configuration)(Kubernetes Identity and Access Management)

If you look in the folders Terraform file you will notice there has been added some stuff. We now also create a roll and assign it permissions. The special part about this role is that it is configured to be used by the pods running inside the cluster in our namespace.

In short by adding this code we can allow our pods to access the s3 bucket without ever having to think about credentials or even worse if they could get leaked.

For your own code you need to modify the terraform to create a role and policy with your own name to avoid conflict.

Once this is done we can look at the deployment file yet again.

At line 25 we annotate the pods to use a specific role, the one we create with terraform.
By doing so we assign the role to our pods with just the permissions needed and thus tying it all together.

Edit your pipeline to include this and deploy it!

Try debugging it again and see if it gives the right output this time:

```bash
kubectl -n ded-workshops-ljmra logs <your pod name>
```

And that concludes this module.

The complete files are also available in the root folder of the workshop and is deployed via Azure DevOps to provide guidance and inspiration.
