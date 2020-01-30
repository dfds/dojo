param(
    [switch]$push = $false,
    $buildNumber = $null
)

$rootDir = resolve-path .
$outputDir = "$rootDir/output/app"
$projectFile = "$rootDir/src/UI/UI.csproj"
$dockerImageName = "dockerton/shop-ui"

cd src

write-host "Restore frontend dependencies" -ForegroundColor Cyan
npm install

write-host "Build frontend and backend" -ForegroundColor Cyan
npm run build:frontend
dotnet publish --output $outputDir --force --configuration Release "$projectFile"

cd ..

write-host "Create container image" -ForegroundColor Cyan
docker build -t $dockerImageName .

if ($push)
{
    write-host "Push container image to ECR" -ForegroundColor Cyan
 
    $dockerlogincmd = aws ecr get-login --no-include-email
    invoke-expression -command $dockerlogincmd

    $awsAccountId = aws sts get-caller-identity --output text --query 'Account'
    $awsRegion = $env:AWS_DEFAULT_REGION

    $ecrImageName = "${awsAccountId}.dkr.ecr.${awsRegion}.amazonaws.com/${dockerImageName}:${buildNumber}"

    docker tag "${dockerImageName}:latest" "${ecrImageName}"
    docker push "${ecrImageName}"
}