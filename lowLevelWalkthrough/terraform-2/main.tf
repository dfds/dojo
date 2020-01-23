provider "aws" {
  region = "eu-west-1"
}

terraform {
  backend "s3" {
    bucket = "dfds-k8sworkshop-bucket-state"
    key    = "workshop/workshop.tfstate"
    region = "eu-west-1"
  }
}


resource "aws_s3_bucket" "test-bucket" {
  bucket = "dfds-k8sworkshop-bucket"
  acl    = "private"
}
