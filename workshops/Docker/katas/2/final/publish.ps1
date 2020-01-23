docker login -u toban

#Push to docker-training repo
docker tag docker-training-webapi:latest toban/docker-training-api:latest
docker push toban/docker-training-api:latest