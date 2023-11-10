if [ ! -f adventureworks.bak ]; then
    echo "Downloading AdventureWorks OLTP backup file from Microsoft..."
    wget https://github.com/Microsoft/sql-server-samples/releases/download/adventureworks/AdventureWorks2017.bak -O adventureworks.bak
    echo "Download complete."
else
    echo "AdventureWorks OLTP backup file already downloaded. Skipping."
fi

echo "Building OLTP docker image."
docker build . -f ./IndexSynchronizerServicesTests/Dockerfile -t devnt90/sql_server_test_container:latest --build-arg BAK_FILE="./adventureworks.bak"
docker run --name sql_server_test_container -p 1433:1433 -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=ChangeThisHardc0dedThing!' -d devnt90/sql_server_test_container:latest
ls
pwd

chmod +x ./IndexSynchronizerServicesTests/startup.sh
chmod +x ./IndexSynchronizerServicesTests/install.sh
