#! /bin/sh

while true
do
    aws s3 cp $path_to_file - --region eu-west-1
    echo "\n"
    sleep 600
done