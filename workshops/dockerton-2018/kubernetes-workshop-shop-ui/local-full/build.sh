#!/bin/bash
echo 'Call from root directory of workshop repos'
for input in kubernetes-workshop-crm kubernetes-workshop-orders kubernetes-workshop-recommendations kubernetes-workshop-shop-ui
do
    pushd $input
    pwsh "./pipeline.ps1"
    popd
done

echo '\nPossibly built files. Start with: docker-compose -f docker-compose -f kubernetes-workshop-shop-ui/local-full/docker-compose.yml up'