FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 12122
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["LinkShortenerSite/LinkShortenerService/LinkShortenerService.csproj", "LinkShortenerService/"]
RUN dotnet restore "LinkShortenerSite/LinkShortenerService/LinkShortenerService.csproj"
COPY . . 
WORKDIR "/src/LinkShortenerSite/LinkShortenerService"
RUN dotnet build "LinkShortenerService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LinkShortenerService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LinkShortenerService.dll", "--host=0.0.0.0", "--port=12122"]
