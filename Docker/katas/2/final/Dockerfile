FROM toban/docker-training-base:latest

# Set environment variables
ENV ASPNETCORE_URLS=http://+:5000

#Expose required ports
EXPOSE 5000

#Set container working dir
WORKDIR /app

#Create new dotnet webapi scaffold
RUN dotnet new webapi

#Remove launchSettings.json to avoid collision with localhost usage in scaffold code
RUN rm /app/Properties/launchSettings.json

#Copy HelloWorldController.cs to container file system
COPY ./HelloWorldController.cs /app/Controllers

#Restore required dependencies
RUN dotnet restore

#Build binaries
RUN dotnet build

#Start webapi
ENTRYPOINT ["dotnet", "run"]