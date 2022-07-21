#!/bin/bash
docker-compose down
docker-compose up -d
taskkill //F //IM "Orders.Api.exe"
dotnet run --project ./src/Orders.Api/ > ./logs/api.log &
