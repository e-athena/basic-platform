FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["BasicPlatform.WebAPI/BasicPlatform.WebAPI.csproj", "BasicPlatform.WebAPI/"]
RUN dotnet restore "BasicPlatform.WebAPI/BasicPlatform.WebAPI.csproj"
COPY . .
WORKDIR "/src/BasicPlatform.WebAPI"
RUN dotnet build "BasicPlatform.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BasicPlatform.WebAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BasicPlatform.WebAPI.dll"]
