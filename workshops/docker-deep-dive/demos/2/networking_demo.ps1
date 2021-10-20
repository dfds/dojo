#Create and start the container as a detached process.
docker run -d --rm --name my_nginx -d -p 8080:80 --network alpine-net nginx

#Test connectivity to Nginx
$PortNumber = 8080
$Uri = "http://localhost:$PortNumber/"

$StatusCode = (Invoke-WebRequest -Uri $Uri).StatusCode

if($StatusCode -eq 200)
{
	Write-Warning $StatusCode
}

#Get process that currently owns port 80
Get-Process -Id (Get-NetTCPConnection -LocalPort $PortNumber).OwningProcess

#Stop nginx container (will be auto-removed due to --rm flag)
docker container stop my_nginx