#!/bin/bash
docker-compose down
docker-compose up -d
taskkill //F //IM "Orders.Api.exe"
dotnet run --project ./src/Orders.Api/ > ./logs/api.log &

taskkill //F //IM "Orders.Api.Stress.Test.exe"
dotnet run --project ./tests/Orders.Api.Stress.Test/ > ./logs/api.tests.log &
