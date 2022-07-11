#Create a new docker network
docker network create app-tier --driver bridge

#Launch a Zookeeper server instance
docker run -d --name zookeeper-server --network app-tier -e ALLOW_ANONYMOUS_LOGIN=yes bitnami/zookeeper:latest

#Launch a Apache Kafka server instance
docker run -d --name kafka-server --network app-tier -e ALLOW_PLAINTEXT_LISTENER=yes -e KAFKA_CFG_ZOOKEEPER_CONNECT=zookeeper-server:2181 bitnami/kafka:latest

#Use kcat to fetch meta data from broker
docker run -it --network app-tier --rm edenhill/kcat:1.7.1 -b kafka-server:9092 -L
