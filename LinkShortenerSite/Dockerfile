FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["LinkShortenerSite.csproj", "LinkShortenerSite/"]
RUN dotnet restore "LinkShortenerSite/LinkShortenerSite.csproj"
COPY .. /src/LinkShortenerSite/
WORKDIR "/src/LinkShortenerSite"
RUN dotnet build "LinkShortenerSite.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LinkShortenerSite.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 80
EXPOSE 8080
EXPOSE 443

ENTRYPOINT ["dotnet", "LinkShortenerSite.dll", "--host=0.0.0.0", "--port=8080"]
