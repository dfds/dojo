provider "aws" {
  region = "eu-west-1"
}

resource "aws_s3_bucket" "test-bucket" {
  bucket = "dfds-k8sworkshop-bucket"
  acl    = "private"
}
