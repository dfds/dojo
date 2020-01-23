#Create the alpine-net network.
docker network create --driver bridge alpine-net

#Create four instance of alpine (notice one is not connected to the alpine-net network)
docker run -dit --name alpine1 --network alpine-net alpine ash
docker run -dit --name alpine2 --network alpine-net alpine ash
docker run -dit --name alpine3 alpine ash
docker run -dit --name alpine4 --network alpine-net alpine ash

#Connect container #4 to default network
docker network connect bridge alpine4

#Inspect default network to ensure #3 + #4 is connected
docker network inspect bridge

#Inspect alpine-net to ensure #1 + #2 + #4 is connected
docker network inspect alpine-net

#Verify that alpine1 can ping containers in alpine-net
docker exec -i alpine1 ping -c 2 alpine2
docker exec -i alpine1 ping -c 2 alpine4

#Verify that alpine4 can ping containers in alpine-net
docker exec -i alpine4 ping -c 2 alpine1
docker exec -i alpine4 ping -c 2 alpine2

#Verify that alpine4 can ping containers in the default bridge network by IP only
docker exec -i alpine4 ping -c alpine3 #will fail because its on default network
docker exec -i alpine4 ping -c 2 172.17.0.2

#Verify that alpine4 can reach the internet
docker exec -i alpine4 ping -c 2 google.com

#Clean up
docker container stop alpine1 alpine2 alpine3 alpine4
docker container rm alpine1 alpine2 alpine3 alpine4
docker network rm alpine-net