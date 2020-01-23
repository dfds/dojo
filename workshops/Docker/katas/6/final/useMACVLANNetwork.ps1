#Create macvlan (parent=eth0.10 (trunked subinterface) parent=eth0 (direct interface)
docker network create -d macvlan --subnet=172.16.86.0/24 --gateway=172.16.86.1 -o parent=eth0 alpine-net

#Create container
docker run --rm -itd --network alpine-net --name my-macvlan-alpine alpine:latest ash

#Check container network interface
docker exec my-macvlan-alpine ip addr show eth0
docker exec my-macvlan-alpine ip route

#Clean up
docker container stop my-macvlan-alpine
docker network rm alpine-net