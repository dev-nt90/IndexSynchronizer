#!/bin/bash

echo "Running installation script..."

#wait for the SQL Server to come up
sleep 15s

#run the setup script to create the DB and the schema in the DB
echo "Restoring database."
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "$SA_PASSWORD" -d master -i install.sql
echo
echo "Done restoring database."
echo "Server is ready."

echo "Creating test runner login"
sed -i 's/#Password#/ChangeThisHardc0dedThing!/' CreateTestRunnerLogin.sql
sed -i 's/#TargetDatabase#/AdventureWorks/' CreateTestRunnerLogin.sql

/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "$SA_PASSWORD" -d master -i CreateTestRunnerLogin.sql
echo "Done creating test runner login"

sleep infinity