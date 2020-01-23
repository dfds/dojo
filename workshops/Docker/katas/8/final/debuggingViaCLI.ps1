$ContainerId = ""

Start-Process docker inspect $ContainerId
Start-Process docker logs $ContainerId
Start-Process docker events
Start-Process docker exec $ContainerId pwsh