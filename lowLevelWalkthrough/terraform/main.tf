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

resource "aws_iam_role" "test-role" {
  name = "test-role"

  assume_role_policy = <<EOF
{
  "Version": "2012-10-17",
  "Statement": [
    {
      "Sid": "",
      "Effect": "Allow",
      "Principal": {
        "AWS": "arn:aws:iam::738063116313:role/eks-hellman-kiam-server"
      },
      "Action": "sts:AssumeRole"
    }
  ]
}
EOF

}

resource "aws_iam_policy" "test-policy" {
  name        = "test-policy"
  description = "A test policy"

  policy = <<EOF
{
    "Version": "2012-10-17",
    "Statement": [
        {
            "Effect": "Allow",
            "Action": [
                "s3:Get*",
                "s3:List*",
                "s3:Describe*"
            ],
            "Resource": "arn:aws:s3:::dfds-k8sworkshop-bucket"
        },
        {
            "Effect": "Allow",
            "Action": [
                "s3:GetAccessPoint",
                "s3:GetAccountPublicAccessBlock",
                "s3:ListAccessPoints",
                "s3:ListJobs"
            ],
            "Resource": "*"
        }
    ]
}
EOF
}

resource "aws_iam_policy_attachment" "test-attach" {
  name       = "test-attachment"
  roles      = ["${aws_iam_role.test-role.name}"]
  policy_arn = "${aws_iam_policy.test-policy.arn}"
}