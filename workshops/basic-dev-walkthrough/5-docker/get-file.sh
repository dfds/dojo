#! /bin/sh

while true
do
    aws s3 cp $path_to_file -
    echo "\n"
    sleep 5
done