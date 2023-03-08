FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/TvMazeScraper.API/TvMazeScraper.API.csproj", "src/TvMazeScraper.API/"]
RUN dotnet restore "src/TvMazeScraper.API/TvMazeScraper.API.csproj"
COPY . .
WORKDIR "/src/src/TvMazeScraper.API"
RUN dotnet build "TvMazeScraper.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TvMazeScraper.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TvMazeScraper.API.dll"]