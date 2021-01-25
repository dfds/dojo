#Setup build image
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env

# Build args
ARG SRC_DIR="./project/"

#Set container filesystem to /build (and create folder if it doesnt exist)
WORKDIR /build

#Copy files to container file system.
ENV SRC_DIR=${SRC_DIR}
COPY $SRC_DIR ./src

#Set workdir to current project folder
WORKDIR /build/src/

#Restore csproj packages.
RUN dotnet restore

#Compile source code using standard Release profile
RUN dotnet publish -c Release -o /build/out
COPY ./waitForKafka.sh /build/out/


#Setup final container images.
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime
WORKDIR /app

RUN apt update && apt install -y curl

# SSL
ENV KAFKATHEBASICS_KAFKA_SSL_CA_LOCATION=/app/cert.pem

RUN curl -sS -o /app/cert.pem https://curl.haxx.se/ca/cacert.pem

#Copy binaries from publish container to final container
COPY --from=build-env /build/out .

#Run dotnet executable
#ENTRYPOINT ["dotnet", "kafka-the-basics.dll"]
ENTRYPOINT ["sh", "waitForKafka.sh"]