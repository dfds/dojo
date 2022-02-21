FROM microsoft/dotnet:2.1.8-aspnetcore-runtime-stretch-slim

WORKDIR /app
COPY ./output/app ./

ENTRYPOINT [ "dotnet", "OrderService.dll" ]