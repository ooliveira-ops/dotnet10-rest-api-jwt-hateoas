#!/bin/bash

echo "Starting SQL Server..."
/opt/mssql/bin/sqlservr &
MSSQL_PID=$!

echo "Waiting for SQL Server to be ready..."
for i in 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20 21 22 23 24 25 26 27 28 29 30; do
    if /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "SENHA_REMOVIDA" -Q "SELECT 1" > /dev/null 2>&1; then
        echo "SQL Server is ready!"
        break
    fi
    echo "Attempt $i/30 - waiting..."
    sleep 1
done

echo "Executing init.sql..."
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "SENHA_REMOVIDA" -i /var/opt/mssql/backup/init.sql

echo "Initialization complete!"
wait $MSSQL_PID