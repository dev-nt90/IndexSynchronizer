if [ ! -f adventureworks.bak ]; then
    echo "Downloading AdventureWorks OLTP backup file from Microsoft..."
    wget https://github.com/Microsoft/sql-server-samples/releases/download/adventureworks/AdventureWorks2017.bak -O ./IndexSynchronizerServicesTests/adventureworks.bak
    echo "Download complete."
else
    echo "AdventureWorks OLTP backup file already downloaded. Skipping."
fi

echo "Building OLTP docker image."
docker build . -f ./IndexSynchronizerServicesTests/Dockerfile -t devnt90/sql_server_test_container:latest --build-arg BAK_FILE="./IndexSynchronizerServicesTests/adventureworks.bak"
docker tag devnt90/sql_server_test_container:latest devnt90/sql_server_test_container:latest