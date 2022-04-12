FROM mcr.microsoft.com/dotnet/aspnet:6.0

COPY PTH.Console/bin/Release/net6.0/publish/ App/
WORKDIR /App
ENTRYPOINT ["dotnet", "PTH.Console.dll"]
