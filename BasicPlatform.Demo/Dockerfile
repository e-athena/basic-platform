FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["BasicPlatform.Demo/BasicPlatform.Demo.csproj", "BasicPlatform.Demo/"]
RUN dotnet restore "BasicPlatform.Demo/BasicPlatform.Demo.csproj"
COPY . .
WORKDIR "/src/BasicPlatform.Demo"
RUN dotnet build "BasicPlatform.Demo.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BasicPlatform.Demo.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BasicPlatform.Demo.dll"]
