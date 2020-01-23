#Login to repo
docker login -u toban

#Push to docker-training repo
docker tag docker-training-base:latest toban/docker-training-base:latest
docker push toban/docker-training-base:latest