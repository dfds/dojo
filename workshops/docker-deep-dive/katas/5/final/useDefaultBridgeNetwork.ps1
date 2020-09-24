#Start two container instances
docker run -dit --name alpine1 alpine ash
docker run -dit --name alpine2 alpine ash

#Verify container network interface
docker exec -i alpine1 ip addr show
docker exec -i alpine2 ip addr show

#Ping alpine2 from within alpine1 based on name (this will fail)
docker exec -i alpine1 ping -c 2 alpine2

#Ping alpine2 from within alpine1 based on IP (this will succeed)
docker exec -i alpine1 ping -c 2 172.17.0.3

#Verify that alpine1 can reach the internet
docker exec -i alpine1 ping -c 2 google.com

#Delete containers
docker container stop alpine1 alpine2
docker container rm alpine1 alpine2