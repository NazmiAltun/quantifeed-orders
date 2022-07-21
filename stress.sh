#!/bin/bash
taskkill //F //IM "Orders.Api.Stress.Test.exe"
dotnet run --project ./tests/Orders.Api.Stress.Test/ > ./logs/api.tests.log &
