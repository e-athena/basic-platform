FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["BasicPlatform.Gateway.Yarp/BasicPlatform.Gateway.Yarp.csproj", "BasicPlatform.Gateway.Yarp/"]
RUN dotnet restore "BasicPlatform.Gateway.Yarp/BasicPlatform.Gateway.Yarp.csproj"
COPY . .
WORKDIR "/src/BasicPlatform.Gateway.Yarp"
RUN dotnet build "BasicPlatform.Gateway.Yarp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BasicPlatform.Gateway.Yarp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BasicPlatform.Gateway.Yarp.dll"]
