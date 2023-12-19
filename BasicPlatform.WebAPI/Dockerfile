#FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
#WORKDIR /app
#EXPOSE 80
#
#COPY /bin/Release/net6.0/publish/ .
#ENTRYPOINT ["dotnet", "BasicPlatform.WebAPI.dll"]

#FROM lrmtc-aspnet-runtime:6.0 AS base
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

VOLUME /app/Databases
VOLUME /app/wwwroot/static

COPY /BasicPlatform.WebAPI/bin/Release/net7.0/linux-x64/publish/ .
ENTRYPOINT ["dotnet", "BasicPlatform.WebAPI.dll"]
