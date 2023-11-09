if [ ! -f adventureworks.bak ]; then
    echo "Downloading AdventureWorks OLTP backup file from Microsoft..."
    wget https://github.com/Microsoft/sql-server-samples/releases/download/adventureworks/AdventureWorks2017.bak -O adventureworks.bak
    echo "Download complete."
else
    echo "AdventureWorks OLTP backup file already downloaded. Skipping."
fi

echo "Building OLTP docker image."
docker build . -t chriseaton/adventureworks:latest --build-arg BAK_FILE="./adventureworks.bak"
docker tag chriseaton/adventureworks:latest chriseaton/adventureworks:oltp
docker run -p 1433:1433 -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=ChangeThisHardc0dedThing!' -d chriseaton/adventureworks:latest