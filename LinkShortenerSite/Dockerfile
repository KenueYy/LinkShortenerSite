FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Убедитесь, что путь корректный относительно контекста сборки
COPY ["LinkShortenerSite.csproj", "LinkShortenerSite/"]

RUN dotnet restore "LinkShortenerSite.csproj"

COPY . .

WORKDIR "/src/LinkShortenerSite"
RUN dotnet build "LinkShortenerSite.csproj" -c Release -o /app/build

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM build AS publish
RUN dotnet publish "LinkShortenerSite.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LinkShortenerSite.dll"]

