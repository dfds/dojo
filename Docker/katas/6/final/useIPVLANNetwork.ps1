#Create ipvlan (this requires us to enable "experimental features" in daemon.json or via a call to dockerd --experimental)
docker network create -d ipvlan --subnet=192.168.210.0/24 --subnet=192.168.212.0/24 --gateway=192.168.210.254 --gateway=192.168.212.254 -o ipvlan_mode=l2 alpine-net

#Create container
docker run --rm -itd --network alpine-net --name my-ipvlan-alpine alpine:latest ash

#Check container network interface
docker exec my-ipvlan-alpine ip addr show eth0
docker exec my-ipvlan-alpine ip route

#Clean up
docker container stop my-ipvlan-alpine
docker network rm alpine-net