FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["BasicPlatform.SSO/BasicPlatform.SSO.csproj", "BasicPlatform.SSO/"]
RUN dotnet restore "BasicPlatform.SSO/BasicPlatform.SSO.csproj"
COPY . .
WORKDIR "/src/BasicPlatform.SSO"
RUN dotnet build "BasicPlatform.SSO.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BasicPlatform.SSO.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BasicPlatform.SSO.dll"]
