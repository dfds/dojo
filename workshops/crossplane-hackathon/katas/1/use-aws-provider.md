
# Deploy AWS Provider
# Deploy s3 bucket using crossplane on AWS
# Upload file to S3 bucket
cd final/html
aws s3 cp . s3://dfds-crossplane-bucket --recursive --acl public-read

# View files in s3 bucket
aws ls dfds-crossplane-bucket

