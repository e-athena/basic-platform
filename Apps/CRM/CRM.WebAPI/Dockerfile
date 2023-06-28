FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Apps/CRM/CRM.WebAPI/CRM.WebAPI.csproj", "Apps/CRM/CRM.WebAPI/"]
RUN dotnet restore "Apps/CRM/CRM.WebAPI/CRM.WebAPI.csproj"
COPY . .
WORKDIR "/src/Apps/CRM/CRM.WebAPI"
RUN dotnet build "CRM.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CRM.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CRM.WebAPI.dll"]
