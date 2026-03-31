#!/bin/bash

# Start SQL server in the background
/opt/mssql/bin/sqlservr &
pid=$!

# wait for SQL server
echo "Waiting for SQL Server to start..."
sleep 15

# Run the SQL Script
echo "Initialization Northwind data..."
/opt/mssql-tools18/bin/sqlcmd \
    -S localhost \
    -U SA \
    -P "$MSSQL_SA_PASSWORD" \
    -i /NorthwindDB/instnwnd.sql \
    -C
    
echo "Initialization complete."

wait $pid