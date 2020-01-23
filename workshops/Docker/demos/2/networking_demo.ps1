#Create and start the container as a detached process.
docker run --rm -d --network host --name my_nginx nginx

#Test connectivity to Nginx
$PortNumber = 80
$Uri = "http://localhost:${$PortNumber}/"
$StatusCode = (Invoke-WebRequest -Uri $Uri).StatusCode

if($StatusCode != 200)
{
	Write-Warning $StatusCode
}

#Get process that currently owns port 80
Get-Process -Id (Get-NetTCPConnection -LocalPort $PortNumber).OwningProcess

#Stop nginx container
docker container stop my_nginx

#Remove nginx container
docker rm my_nginx