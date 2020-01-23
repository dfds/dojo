FROM toban/docker-training-base:latest

# Set environment variables
ENV ASPNETCORE_URLS=http://+:5000

#Expose required ports
EXPOSE 5000

#Set container working dir
WORKDIR /app

#Create new dotnet webapi scaffold
RUN dotnet new mvc

#Remove launchSettings.json to avoid collision with localhost usage in scaffold code
RUN rm /app/Properties/launchSettings.json

#Customize project items.
COPY ./HomeController.cs /app/Controllers
COPY ./Index.cshtml /app/Views/Home

#Restore required dependencies
RUN dotnet restore

#Build binaries
RUN dotnet build

#Start webapi
ENTRYPOINT ["dotnet", "run"]