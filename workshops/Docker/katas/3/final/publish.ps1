docker login -u toban

#Push to docker-training repo
docker tag docker-training-frontend:latest toban/docker-training-frontend:latest
docker push toban/docker-training-frontend:latest