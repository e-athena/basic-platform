FROM lrmtc-aspnet-runtime:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

COPY /BasicPlatform.WebAPI/bin/Release/net6.0/linux-x64/publish/ .
ENTRYPOINT ["dotnet", "BasicPlatform.WebAPI.dll"]
