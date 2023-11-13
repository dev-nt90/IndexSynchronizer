# Check if adventureworks.bak does not exist
if (-not (Test-Path -Path ".\IndexSynchronizerServicesTests\adventureworks.bak")) {
    Write-Host "Downloading AdventureWorks OLTP backup file from Microsoft..."
    Invoke-WebRequest -Uri "https://github.com/Microsoft/sql-server-samples/releases/download/adventureworks/AdventureWorks2017.bak" -OutFile ".\IndexSynchronizerServicesTests\adventureworks.bak"
    Write-Host "Download complete."
}
else {
    Write-Host "AdventureWorks OLTP backup file already downloaded. Skipping."
}

Write-Host "Building OLTP docker image."
docker build . -f .\IndexSynchronizerServicesTests\Dockerfile -t devnt90/sql_server_test_container:latest --build-arg BAK_FILE="./IndexSynchronizerServicesTests/adventureworks.bak"
docker tag devnt90/sql_server_test_container:latest devnt90/sql_server_test_container:latest