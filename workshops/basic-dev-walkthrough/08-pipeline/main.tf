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

resource "aws_s3_bucket_object" "test-file" {
  key = "testfile.txt"
  bucket = "${aws_s3_bucket.test-bucket.id}"
  content = "Hello from s3"
}

