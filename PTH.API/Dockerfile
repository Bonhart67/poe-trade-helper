FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ./PTH.API/*.csproj src/PTH.API/
COPY ./PTH.Domain/*.csproj src/PTH.Domain/
COPY ./PTH.Logic/*.csproj src/PTH.Logic/

RUN dotnet restore src/PTH.API/*.csproj

COPY . .
WORKDIR /src/PTH.API/
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS runtime
WORKDIR /app

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PTH.API.dll"]
