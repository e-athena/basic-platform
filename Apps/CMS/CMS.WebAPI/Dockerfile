FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Apps/CMS/CMS.WebAPI/CMS.WebAPI.csproj", "Apps/CMS/CMS.WebAPI/"]
RUN dotnet restore "Apps/CMS/CMS.WebAPI/CMS.WebAPI.csproj"
COPY . .
WORKDIR "/src/Apps/CMS/CMS.WebAPI"
RUN dotnet build "CMS.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CMS.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CMS.WebAPI.dll"]
