docker rm docker-training-webapi 
docker volume create demo_volume 
docker run -d --name docker-training-webapi -v demo_volume:/app docker-training-webapi:latest