docker login -u toban

#Push to docker-training repo
docker tag docker-training-mvc:latest toban/docker-training-mvc:latest
docker push toban/docker-training-mvc:latest