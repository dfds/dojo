docker login -u toban

#Push to docker-training repo
docker tag docker-training-webapi:latest toban/docker-training-webapi:latest
docker push toban/docker-training-webapi:latest