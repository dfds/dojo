#Clone sample repo
git clone https://github.com/Azure-Samples/application-insights-aspnet-sample-opentelemetry.git

#Step into repo
cd application-insights-aspnet-sample-opentelemetry/quickstart/sample/

#Assign env value for .env file
$envValues=@'
USE_APPLICATIONINSIGHTS=true
USE_OPENTELEMETRY=false
AI_INSTRUMENTATIONKEY=<ENTER-APPLICATION-INSIGHTS-INSTRUMENTATION-KEY>
'@

#Create .env file
New-Item -Path . -Name ".env" -ItemType "file" -Value $envValues

#Run docker-compose up
docker-compose up