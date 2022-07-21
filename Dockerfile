FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app
COPY ./*.sln ./
COPY src/*/*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p src/${file%.*}/ && mv $file src/${file%.*}/; done
COPY tests/*/*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p tests/${file%.*}/ && mv $file tests/${file%.*}/; done

RUN dotnet restore

COPY . .
ARG BUILDCONFIG=Release
RUN dotnet build ./src/Orders.Api/Orders.Api.csproj -c $BUILDCONFIG --warnaserror --no-restore -v Quiet --nologo
RUN dotnet publish ./src/Orders.Api/Orders.Api.csproj -c $BUILDCONFIG -o ./api --no-restore --no-build  -v Quiet --nologo
RUN dotnet build ./tests/Orders.Api.Stress.Test/Orders.Api.Stress.Test.csproj -c $BUILDCONFIG --no-restore -v Quiet --nologo
RUN dotnet publish ./tests/Orders.Api.Stress.Test/Orders.Api.Stress.Test.csproj  -c $BUILDCONFIG -o ./perftest --no-restore --no-build  -v Quiet --nologo


FROM base AS api
WORKDIR /app
COPY --from=build /app/api .
ENTRYPOINT ["dotnet", "Orders.Api.dll"]

FROM base AS perftest
WORKDIR /app
COPY --from=build /app/perftest .
ENTRYPOINT ["dotnet", "Orders.Api.Stress.Test.dll"]
